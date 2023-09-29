using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using ClosedXML.Excel;
using System.IO;


public partial class Masters_ReportForms_frmIndentIssRecDetail : System.Web.UI.Page
{
    string TempCustomerCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname ";

            if (Convert.ToInt32(Session["varcompanyId"]) == 16 || Convert.ToInt32(Session["varcompanyId"]) == 28)
            {
                str = str + @" Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                    From IndentMaster a(Nolock)
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                    WHere a.MasterCompanyId = " + Session["varCompanyId"] + @" Order  By PNM.PROCESS_NAME ";
            }
            else if (Convert.ToInt32(Session["varcompanyId"]) == 44)
            {
                str = str + @" Select PROCESS_NAME_ID,PROCESS_NAME 
                    From Process_Name_Master 
                    Where MasterCompanyId=" + Session["varCompanyId"] + " and Process_name_id in (10,11,5,18)"; 
            }
            else
            {
                str = str + @" Select PROCESS_NAME_ID,PROCESS_NAME 
                    From Process_Name_Master 
                    Where MasterCompanyId=" + Session["varCompanyId"] + @" and " + (variable.Carpetcompany == "1" ? " Process_Name='DYEING'" : "1=1") + @" Order By PROCESS_NAME ";
            }
            if (Convert.ToInt32(Session["varcompanyId"]) == 44)
            {
                str = str + @" select EmpId,ltrim(Empname)+'/'+Address As Empname  from Empinfo  Where MasterCompanyId=" + Session["varCompanyId"] + @"  Order by EmpName                  
                select distinct CI.CustomerId,CI.CustomerCode as customercode from OrderMaster OM inner join V_Indent_OredrId VO on Om.OrderId=VO.Orderid inner join customerinfo CI on CI.CustomerId=OM.CustomerId order by CustomerCode 
                select IM.Item_Id,Im.Item_Name From Item_Master Im inner join CategorySeparate cs on IM.CATEGORY_ID=Cs.Categoryid and cs.id=1 order by IM.ITEM_NAME 
                select OrderCategoryId, OrderCategory from OrderCategory order by OrderCategory";
            }
            else
            {

                str = str + @" select EmpId,ltrim(Empname)+'/'+Address As Empname  from Empinfo  Where MasterCompanyId=" + Session["varCompanyId"] + @"  Order by EmpName                  
                select distinct CI.CustomerId,CI.CustomerCode+'/'+CompanyName as customercode from OrderMaster OM inner join V_Indent_OredrId VO on Om.OrderId=VO.Orderid inner join customerinfo CI on CI.CustomerId=OM.CustomerId order by CustomerCode 
                select IM.Item_Id,Im.Item_Name From Item_Master Im inner join CategorySeparate cs on IM.CATEGORY_ID=Cs.Categoryid and cs.id=1 order by IM.ITEM_NAME 
                select OrderCategoryId, OrderCategory from OrderCategory order by OrderCategory";
            
            }
            if (variable.JoborderNewModule == "1")
            {
                str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
            }
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
                if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
                {
                    DDCustCode_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDEmpName, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddItemName, ds, 4, true, "--ALL--");
            UtilityModule.ConditionalComboFillWithDS(ref DDOrderCategory, ds, 5, true, "--ALL--");
            if (ddItemName.Items.Count > 0)
            {
                ddItemName.SelectedIndex = 0;
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCompany.Items.Count > 0)
            {
                //    DDCompany.SelectedIndex = 1;
            }

            //
            if (variable.Carpetcompany == "1")
            {
                RDShadewiseDetail.Visible = true;
                RDSubitemwiseBalance.Visible = true;
                RDorderwiseshadedetail.Visible = true;
                RDProcessProgramWiseShadeDetail.Visible = true;
            }

            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                TRCustomerCode.Visible = false;
            }
            if (Session["varcompanyId"].ToString() == "16")
            {
                RDPONot.Visible = false;
                RDdyerledger.Visible = false;
                RDIndentRecPending.Visible = true;
                RDIndentIssueWithPPConsumption.Visible = true;

            }

            if (Session["VarCompanyId"].ToString() == "44")
            {
                RDProcessIssDetail.Visible = false;
                RDProcessRecDetail.Visible = false;
                RDProcessIss_REcDetail.Visible = true;
                RDPONot.Visible = false;
                RDShadewiseDetail.Visible = true;
                RDSubitemwiseBalance.Visible = false;
                RDProcessProgramWiseShadeDetail.Visible = false;
                RDorderwiseshadedetail.Visible = false;
                RDorderwiseindentdetail.Visible = true;
                RDdyerledger.Visible = true;
                RDIndentPendingDetail.Visible = false;
                RDIndentRecPending.Visible = false;
                RDIndentIssueWithPPConsumption.Visible = false;
            }

            if (Session["VarCompanyNo"].ToString() == "43")
            {
                RDGenerateIndentDetail.Visible = true;
                RDIndentMaterialIssueDetail.Visible = true;
                RDIndentRecPending.Visible = true;
            }
        }
    }

    protected void DDIndentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDIndentNo.SelectedIndex > 0)
        {
            TRPartyChallanNo.Visible = false;
            txtPartyChallanNo.Text = "";
        }
        else
        {
            TRPartyChallanNo.Visible = true;
        }
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = string.Empty;
        if (chksample.Checked == true)
        {
            TRPartyChallanNo.Visible = false;
            txtPartyChallanNo.Text = "";

            str = @" Select ID as Indentid,indentNo From SampleDyeingmaster SM Where 1=1";
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " and companyid=" + DDCompany.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " and Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and empid=" + DDEmpName.SelectedValue;
            }
            str = str + " order by Indentid";
        }
        else
        {
            if (Session["varCompanyNo"].ToString() == "9")
            {
                str = @"select Distinct IM.IndentId,isnull(OM.LocalOrder,'')+'/'+ IM.IndentNo  from IndentMaster IM inner join V_Indent_OredrId VO on Im.IndentID=VO.IndentId
                        JOIN OrderMaster OM ON VO.OrderId=OM.OrderId
                        Where IM.MasterCompanyid= " + Session["varcompanyid"];
            }
            else
            {
                str = @"select Distinct IM.IndentId,IM.IndentNo  from IndentMaster IM inner join V_Indent_OredrId VO on Im.IndentID=VO.IndentId
                  Where IM.MasterCompanyid= " + Session["varcompanyid"];
            }


            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " and IM.companyid=" + DDCompany.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and VO.Orderid=" + DDOrderNo.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " and IM.Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " and IM.partyid=" + DDEmpName.SelectedValue;
            }

            if (variable.JoborderNewModule == "1")
            {
                str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
            }
            str = str + "  order by IndentId";
        }

        UtilityModule.ConditionalComboFill(ref DDIndentNo, str, true, "--Select--");
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str, strsample = string.Empty;
        str = @"select Distinct EI.EmpId,ltrim(Empname)+'/'+Address as EmpName from IndentMaster IM inner join V_Indent_OredrId VO on Im.IndentID=VO.IndentId
                inner join empinfo EI on EI.EmpId=IM.PartyId  Where EI.MasterCompanyid= " + Session["varcompanyid"];

        strsample = "select Distinct ei.EmpId,ei.EmpName+'/'+Address as Empname From SampleDyeingmaster SM inner join EmpInfo ei on SM.empid=ei.EmpId Where 1=1";

        if (DDCompany.SelectedIndex > 0)
        {
            str = str + " and IM.companyid=" + DDCompany.SelectedValue;
            strsample = strsample + " and SM.companyid=" + DDCompany.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and VO.Orderid=" + DDOrderNo.SelectedValue;
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + " and IM.Processid=" + DDProcessName.SelectedValue;
            strsample = strsample + " and SM.Processid=" + DDProcessName.SelectedValue;
        }
        strsample = strsample + "  order by Empname";
        if (variable.JoborderNewModule == "1")
        {
            str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
        }
        str = str + " UNION " + strsample;
        UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Select--");

    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
        }
        else
        {
            trDates.Visible = false;
        }
    }
    protected void ShadewiseDetail()
    {
        string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ShadeWiseIndentDetail Where Companyid=" + DDCompany.SelectedValue;


        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + "  and Processid=" + DDProcessName.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + "  and Empid=" + DDEmpName.SelectedValue;
        }
        if (DDindentStatus.SelectedIndex > 0)
        {
            str = str + " and Status='" + DDindentStatus.SelectedItem.Text + "'";
        }
        if (DDIndentNo.SelectedIndex > 0)
        {
            if (chksample.Checked == true)
            {
                str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
            }
            else
            {
                str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
            }
        }

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        if (ChkForDate.Checked == true)
        {
            if (RDSubitemwiseBalance.Checked == true)
            {
                //str = str + "  and IndentDate<='" + TxtFromDate.Text + "'";
                str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
            }
            else
            {
                str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
            }
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and orderid=" + DDOrderNo.SelectedValue;
        }
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

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

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFilename"] = "~\\ReportSchema\\rptindentshadewiseDetail.xsd";
            if (RDSubitemwiseBalance.Checked == true)
            {
                switch (Session["varCompanyId"].ToString())
                {
                    case "27":
                        Session["rptFilename"] = "Reports/rptindentSubitemwiseDetailForAntique.rpt";
                        break;
                    case "14":
                        Session["rptFilename"] = "Reports/rptindentSubitemwiseDetailEastern.rpt";
                        break;
                    default:
                        Session["rptFilename"] = "Reports/rptindentSubitemwiseDetail.rpt";
                        break;
                }
            }
            else
            {
                Session["rptFilename"] = "Reports/rptindentshadewiseDetail.rpt";
            }
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    protected void OrderShadewiseDetail()
    {
        string str = "";
        if (Session["varCompanyId"].ToString() == "30")
        {
            str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + @" as Dateflag 
            From V_ORDERSHADEWISEINDENTDETAIL_SAMARA Where Companyid=" + DDCompany.SelectedValue;
        }
        else if (Session["varCompanyId"].ToString() == "42")
        {
            str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + @" as Dateflag 
            From V_ORDERSHADEWISEINDENTDETAIL_VIKRAMMIRZAPUR Where Companyid=" + DDCompany.SelectedValue;
        }
        else if (Session["varCompanyId"].ToString() == "43")
        {
            str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + @" as Dateflag 
            From V_ORDERSHADEWISEINDENTDETAIL_CarpetInternational Where Companyid=" + DDCompany.SelectedValue;
        }
        else
        {
            str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + @" as Dateflag 
            From V_ORDERSHADEWISEINDENTDETAIL Where Companyid=" + DDCompany.SelectedValue;
        }

        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + "  and Processid=" + DDProcessName.SelectedValue;
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            str = str + "  and Empid=" + DDEmpName.SelectedValue;
        }
        if (DDindentStatus.SelectedIndex > 0)
        {
            str = str + " and Status='" + DDindentStatus.SelectedItem.Text + "'";
        }
        if (DDIndentNo.SelectedIndex > 0)
        {
            if (chksample.Checked == true)
            {
                str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
            }
            else
            {
                str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
            }

        }

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + "  and Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + "  and Qualityid=" + DDQuality.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
        }
        if (ChkForDate.Checked == true)
        {
            if (RDSubitemwiseBalance.Checked == true)
            {
                str = str + "  and IndentDate<='" + TxtFromDate.Text + "'";
            }
            else
            {
                str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
            }
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " and Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " and orderid=" + DDOrderNo.SelectedValue;
        }
        if (RDProcessProgramWiseShadeDetail.Checked == true)
        {
            str = str + " and orderid in (Select Order_ID From ProcessProgram(Nolock) Where PPID = " + DDProcessProgram.SelectedValue + ")";
        }
        if (DDorderstatus.SelectedValue != "-1")
        {
            str = str + " and Orderstatus=" + DDorderstatus.SelectedValue;
        }
        if (DDOrderCategory.SelectedIndex > 0)
        {
            str = str + " And OrderCategoryId = " + DDOrderCategory.SelectedValue;
        }
        if (ChkForExportExcel.Checked == true)
        {
            str = str + " Order By OrderSampleFlag, CUSTOMERCODE, ORDERNO";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ChkForExportExcel.Checked == true)
            {
                OrderWiseShadeDetailExcell(ds);
            }
            else
            {
                Session["dsFilename"] = "~\\ReportSchema\\rptindentordershadewiseDetail.xsd";
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    Session["rptFilename"] = "Reports/rptindentordershadewiseDetailCI.rpt";
                }
                else
                {
                    Session["rptFilename"] = "Reports/rptindentordershadewiseDetail.rpt";
                }                  

                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
    }
    private void OrderWiseShadeDetailExcell(DataSet ds)
    {
        if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
        }
        string Path = "";
        var xapp = new XLWorkbook();
        var sht = xapp.Worksheets.Add("sheet1");
        int row = 0;

        sht.Column("A").Width = 35.22;
        sht.Column("B").Width = 20.22;
        sht.Column("C").Width = 25.22;
        sht.Column("D").Width = 25.33;
        sht.Column("E").Width = 15.00;
        sht.Column("F").Width = 18.00;
        sht.Column("G").Width = 18.00;
        sht.Column("H").Width = 18.00;
        sht.Column("I").Width = 18.00;
        sht.Column("J").Width = 9.00;
        sht.Column("K").Width = 9.00;
        sht.Column("L").Width = 9.00;
        sht.Column("M").Width = 9.00;
        sht.Column("N").Width = 9.00;
        sht.Column("O").Width = 9.00;
        sht.Column("P").Width = 9.00;
        sht.Column("Q").Width = 9.00;
        sht.Column("R").Width = 9.00;
        sht.Column("S").Width = 9.00;
        sht.Column("T").Width = 35.00;

        sht.Range("A1:T1").Merge();
        sht.Range("A1").Value = DDCompany.SelectedItem.Text;
        sht.Range("A2:T2").Merge();
        sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
        sht.Range("A3:T3").Merge();
        sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"] + "  " + "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
        sht.Range("A4:T4").Merge();
        sht.Range("A4").Value = "Dying Detail From Date " + TxtFromDate.Text + " To Date " + TxtToDate.Text;

        sht.Range("A1:T1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A2:T2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A4:T4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A4:T4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        sht.Range("A1:T1").Style.Alignment.SetWrapText();
        sht.Range("A2:T2").Style.Alignment.SetWrapText();
        sht.Range("A3:T3").Style.Alignment.SetWrapText();
        sht.Range("A4:T4").Style.Alignment.SetWrapText();
        sht.Range("A1:T4").Style.Font.FontName = "Arial Unicode MS";
        sht.Range("A1:T4").Style.Font.FontSize = 10;
        sht.Range("A1:T4").Style.Font.Bold = true;

        //*******Header
        sht.Range("A5").Value = "BUYER CODE";
        sht.Range("B5").Value = "PO NO";
        sht.Range("C5").Value = "DYER NAME";
        sht.Range("D5").Value = "PP NO";
        sht.Range("E5").Value = "INDENT No";
        sht.Range("F5").Value = "INDENT STATUS";
        sht.Range("G5").Value = "ISSUE DATE";
        sht.Range("H5").Value = "ITEM NAME";
        sht.Range("I5").Value = "SUB ITEM NAME";
        sht.Range("J5").Value = "COLOR NAME";
        sht.Range("K5").Value = "LOT NO";
        sht.Range("L5").Value = "O_QTY";
        sht.Range("M5").Value = "DYED QTY";
        sht.Range("N5").Value = "UNDYED QTY";
        sht.Range("O5").Value = "LOSS Qty";
        sht.Range("P5").Value = "RETN. Qty";
        sht.Range("Q5").Value = "PENDING Qty";
        sht.Range("R5").Value = "RATE";
        sht.Range("S5").Value = "AMOUNT";
        sht.Range("T5").Value = "REMARK";

        sht.Range("A5:T5").Style.Font.FontName = "Arial Unicode MS";
        sht.Range("A5:T5").Style.Font.FontSize = 10;
        sht.Range("A5:T5").Style.Font.Bold = true;
        //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        sht.Range("A5:T5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sht.Range("A5:T5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        sht.Range("A5:T5").Style.Alignment.SetWrapText();

        row = 6;
        int rowfrom = 6;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sht.Range("A" + row + ":T" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":T" + row).Style.Font.FontSize = 10;

            sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
            sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ORDERNO"]);
            //sht.Range("B" + row).Style.Alignment.SetWrapText();
            sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
            sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["PPNO"]);
            sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["INDENTNO"]);
            //sht.Range("D" + row).Style.Alignment.SetWrapText();
            sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["STATUS"]);

            sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["INDENTDATE"]);
            sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
            sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
            sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
            sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["LOTNO"]);
            sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["OQTY"]);

            decimal ActualDyedQty = 0;
            ActualDyedQty = Convert.ToDecimal(ds.Tables[0].Rows[i]["DYEDQTY"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["RETQTY"]);

            sht.Range("M" + row).SetValue(ActualDyedQty);
            sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["UNDYEDQTY"]);
            sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["LOSSQTY"]);
            sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["RETQTY"]);

            decimal PendingQty = 0;
            PendingQty = (Convert.ToDecimal(ds.Tables[0].Rows[i]["OQTY"]) - (Convert.ToDecimal(ds.Tables[0].Rows[i]["DYEDQTY"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["LOSSQTY"]))) + Convert.ToDecimal(ds.Tables[0].Rows[i]["RETQTY"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["UNDYEDQTY"]);
            sht.Range("Q" + row).SetValue(PendingQty);

            sht.Range("R" + row).SetValue(ds.Tables[0].Rows[i]["RATE"]);
            sht.Range("S" + row).SetValue((Convert.ToDecimal(ds.Tables[0].Rows[i]["DYEDQTY"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["LOSSQTY"]) - Convert.ToDecimal(ds.Tables[0].Rows[i]["RETQTY"])) * Convert.ToDecimal(ds.Tables[0].Rows[i]["RATE"]));
            sht.Range("T" + row).SetValue(ds.Tables[0].Rows[i]["Remark"]);

            row = row + 1;
        }
        sht.Range("L" + row).FormulaA1 = "=SUM(L" + (rowfrom) + ":L" + (row - 1) + ")";
        sht.Range("M" + row).FormulaA1 = "=SUM(M" + (rowfrom) + ":M" + (row - 1) + ")";
        sht.Range("N" + row).FormulaA1 = "=SUM(N" + (rowfrom) + ":N" + (row - 1) + ")";
        sht.Range("O" + row).FormulaA1 = "=SUM(O" + (rowfrom) + ":O" + (row - 1) + ")";
        sht.Range("P" + row).FormulaA1 = "=SUM(P" + (rowfrom) + ":P" + (row - 1) + ")";
        sht.Range("Q" + row).FormulaA1 = "=SUM(Q" + (rowfrom) + ":Q" + (row - 1) + ")";
        sht.Range("S" + row).FormulaA1 = "=SUM(S" + (rowfrom) + ":S" + (row - 1) + ")";

        sht.Range("J" + row + ":T" + row).Style.Font.FontName = "Arial Unicode MS";
        sht.Range("J" + row + ":T" + row).Style.Font.FontSize = 9;
        sht.Range("J" + row + ":T" + row).Style.Font.Bold = true;
        row = row + 1;


        //*************
        using (var a = sht.Range("A1:T" + row))
        {
            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        }

        sht.Columns(1, 30).AdjustToContents();

        string Fileextension = "xlsx";
        string filename = UtilityModule.validateFilename("DYING DETAIL_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

        //////IndentWiseDetailExportExcelNew();

        ////********************
        if (RDProcessIssDetail.Checked == true && ChkForExportExcel.Checked == true)
        {
            IndentIssueDetailExportExcel();
        }
        if (RDShadewiseDetail.Checked == true || RDSubitemwiseBalance.Checked == true)
        {
            if (ChkForExportExcel.Checked == true)
            {
                if (Session["varcompanyId"].ToString() == "21")
                {
                    ShadewiseDetailExportExcelNew();
                }
                else if (Session["varcompanyId"].ToString() == "38" || Session["varcompanyId"].ToString() == "42")
                {
                    ShadewiseDetailExportExcelWithoutGateInNo();
                }

                else
                {
                    ShadewiseDetailExportExcel();
                }
            }
            else
            {
                ShadewiseDetail();
                return;
            }

        }
        if (RDorderwiseshadedetail.Checked == true || RDProcessProgramWiseShadeDetail.Checked == true)
        {
            OrderShadewiseDetail();
            return;
        }
        if (RDIndentMaterialIssueDetail.Checked == true)
        {
            PurchaseandIndentIssueDetailReportData();
            return;
        }
        if (RDorderwiseindentdetail.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "44")
            {
                Orderwiseindentdetail_AGNI();
            }
            else
            {
                Orderwiseindentdetail();
            
            }
            return;
        }
        if (RDdyerledger.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "27")
            {
                DyerledgerAntique();
                return;
            }
            else if (Session["VarCompanyNo"].ToString() == "44")
            {
                Dyerledger_agni();
                return;
            }
            else
            {
                Dyerledger();
                return;
            }
        }
        if (RDIndentRecPending.Checked == true)
        {
            IndentRecPendingTillDateExcelReport();
            return;
        }
        if (RDIndentIssueWithPPConsumption.Checked == true)
        {
            IndentIssueWithPPConsumptionExcelReport();
            return;
        }
        if (RDGenerateIndentDetail.Checked == true)
        {
            GenerateIndentDetailReportData();
            return;
        }

        if (RDIndentPendingDetail.Checked == true)
        {
            if (Session["VarCompanyNo"].ToString() == "27")
            {
                IndentWisePendingDetailExcelReportAntique();
                return;
            }
            else
            {
                IndentWisePendingDetailExcelReport();
                return;
            }
        }

        //*********************
        string str, strsample = "";
        DataSet ds;
        string VarDateflag;
        if (ChkForDate.Checked == true)
        {
            VarDateflag = "1";
        }
        else
        {
            VarDateflag = "0";
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {


            if (RDProcessIssDetail.Checked == true)
            {
                if (variable.JoborderNewModule == "1")
                {
                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End  As CompanyName,Empname,
                           IndentNo,ChallanNo,PM.Date,Finishedid,PT.LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName
                           +' '+ShapeName+' '+case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft  End As Description,
                            Sum(issueQuantity-isnull(canQty,0)) As IssueQty,'" + TxtFromDate.Text + " ' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" As dateflag,
                            OM.LocalOrder,OM.CustomerOrderNo ,case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process 
                            ,isnull((Select distinct ReqDate from IndentMaster Where IndentId=PT.IndentId),'') as IndentReqDate
                            From PP_Processrawmaster PM inner join PP_Processrawtran PT  on PM.prmid=PT.prmid
                            inner join companyinfo CI on PM.companyid=CI.companyid
                            inner join Empinfo EI on PM.Empid=Ei.EmpId
                            inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID
                            inner join v_finisheditemdetail vf on PT.finishedid=vf.item_finished_id
                            INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId                            
                            left join orderdetail OD on PT.orderdetailid=OD.orderdetailid
                            left join ordermaster Om on OD.orderid=OM.orderid WHere PM.mastercompanyid=" + Session["varcompanyid"];
                }
                else
                {

                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,ChallanNo,PM.Date,Finishedid,PT.LotNo,
                    CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch Else Sizemtr End End End As Description,
                    Sum(issueQuantity-isnull(canQty,0)) As IssueQty,'" + TxtFromDate.Text + " ' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,
                    OM.CustomerOrderNo,case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process 
                    ,isnull((Select distinct ReqDate from IndentMaster Where IndentId=PT.IndentId),'') as IndentReqDate
                    From PP_ProcessRawMaster PM INNER JOIN PP_ProcessRawTran PT ON PM.PrmId=PT.PrmId
                    INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId
                    INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.Item_Finished_Id
                    INNER JOIN Empinfo E ON Pm.EmpId=E.EmpId
                    INNER JOIN OrderMaster OM ON OM.Orderid=IM.OrderId
                    INNER JOIN Companyinfo CI ON PM.CompanyId=CI.CompanyId
                    INNER JOIN Process_Name_Master PNM ON PM.ProcessId=PNM.PROCESS_NAME_ID
                    Where PM.MasterCompanyId=" + Session["varCompanyId"];

                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = @"SELECT PNM.PROCESS_NAME,CASE WHEN " + DDCompany.SelectedIndex + @">0 THEN CI.COMPANYNAME ELSE 'ALL' END AS COMPANYNAME,EMPNAME,CASE WHEN CHARINDEX('S',SM.INDENTNO)=0 THEN 'S-'+SM.INDENTNO ELSE SM.INDENTNO END AS INDENTNO
                                ,CASE WHEN CHARINDEX('S',SM.INDENTNO)=0 THEN 'S-'+SM.INDENTNO ELSE SM.INDENTNO END AS CHALLANNO,SM.ISSUEDATE AS DATE,SD.IFINISHEDID AS FINISHEDID,SD.LOTNO,
                                CATEGORY_NAME+' '+ITEM_NAME+' '+QUALITYNAME+' '+DESIGNNAME+' '+COLORNAME+' '+SHADECOLORNAME+' '+SHAPENAME+' '+CASE WHEN SD.UNITID=1 THEN SIZEMTR ELSE CASE WHEN SD.UNITID=2 THEN SIZEFT ELSE CASE WHEN SD.UNITID=6 THEN SIZEINCH ELSE SIZEMTR END END END AS DESCRIPTION,
                                SUM(SD.ISSUEQTY) AS ISSUEQTY,'" + TxtFromDate.Text + " ' AS FROMDATE,'" + TxtToDate.Text + "' AS TODATE,'" + VarDateflag + @"' AS DATEFLAG,'' LOCALORDER,
                                ''  AS CUSTOMERORDERNO,PNM.PROCESS_NAME AS RE_PROCESS,isnull(SM.ReqDate,'') as IndentReqDate
                                 FROM SAMPLEDYEINGMASTER SM INNER JOIN  SAMPLEDYEINGDETAIL SD ON SM.ID=SD.MASTERID
                                INNER JOIN COMPANYINFO CI ON SM.COMPANYID=CI.COMPANYID
                                INNER JOIN V_FINISHEDITEMDETAIL VF ON SD.IFINISHEDID=VF.ITEM_FINISHED_ID
                                INNER JOIN PROCESS_NAME_MASTER PNM ON SM.PROCESSID=PNM.PROCESS_NAME_ID
                                INNER JOIN EMPINFO EI ON SM.EMPID=EI.EMPID Where 1=1";
                    }

                }
                if (DDCompany.SelectedIndex > 0)
                {
                    str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.Companyid=" + DDCompany.SelectedValue;
                    }
                }
                if (DDCustCode.SelectedIndex > 0)
                {
                    str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                }
                if (DDOrderNo.SelectedIndex > 0)
                {
                    str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                }
                if (DDProcessName.SelectedIndex > 0)
                {
                    str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.Processid=" + DDProcessName.SelectedValue;
                    }
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.empid=" + DDEmpName.SelectedValue;
                    }
                }
                if (DDIndentNo.SelectedIndex > 0)
                {
                    if (chksample.Checked == true)
                    {
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " and SM.Id=" + DDIndentNo.SelectedValue;
                        }
                        str = str + "  And IM.IndentId=0";
                    }
                    else
                    {
                        str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " and SM.Id=0";
                        }
                    }

                }
                else
                {
                    if (chksample.Checked == true)
                    {
                        str = str + "  And IM.IndentId=0";
                    }
                }
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    }
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    }
                }
                if (DDShadeColor.SelectedIndex > 0)
                {
                    str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    }
                }
                if (ChkForDate.Checked == true)
                {
                    str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
                    }
                }
                if (variable.JoborderNewModule == "1")
                {
                    str = str + " group by Process_Name,CI.CompanyName,Empname,IndentNo,ChallanNo,PM.Date,Finishedid,PT.Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,IM.Re_Process,PT.IndentId";
                }
                else
                {
                    str = str + " group by Process_Name,CI.CompanyName,Empname,IndentNo,ChallanNo,PM.Date,Finishedid,PT.Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,IM.Re_Process,PT.IndentId";
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " GROUP BY PROCESS_NAME,CI.COMPANYNAME,EMPNAME,INDENTNO,SM.ISSUEDATE,SD.IFINISHEDID,SD.LOTNO,CATEGORY_NAME,ITEM_NAME,QUALITYNAME,DESIGNNAME,COLORNAME,SHADECOLORNAME,SHAPENAME,SD.UNITID,SIZEMTR,SIZEFT,SIZEINCH,SM.ReqDate";
                        str = str + " UNION ALL " + strsample;
                    }
                }
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
                }

                Session["dsFilename"] = "~\\ReportSchema\\RptIndentRawIssDetailIndentWise.xsd";

                if (Session["VarCompanyNo"].ToString() == "38")
                {
                    Session["rptFilename"] = "Reports/RptIndentRawIssDetailIndentWiseVikramKM.rpt";
                }
                else
                {
                    Session["rptFilename"] = "Reports/RptIndentRawIssDetailIndentWise.rpt";
                }
                
                ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                Session["GetDataset"] = ds;
            }
            if (RDProcessRecDetail.Checked == true)
            {

                if (Session["VarCompanyNo"].ToString() == "22")
                {
                    if (variable.JoborderNewModule == "1")
                    {
                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End as CompanyName,Empname,IndentNo,
                            '' as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                            Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                            CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                            case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft  End As Description,Sum(RecQuantity) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,
                            '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid
                            ,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process ,PT.TagNo,'' as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks
                            ,'' as IssLotNo,'' as IssTagNo,IM.Date as IndentDate,isnull(PT.Rate,0) as IndentRate,PM.BillNo
                            From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                            inner join IndentMaster Im on PT.IndentId=IM.IndentID
                            inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                            inner join EmpInfo E on PM.Empid=E.EmpId
                            inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                            inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                            inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID                           
                            left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                            left join OrderMaster OM on OD.OrderId=OM.OrderId
                            left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                            INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                            Where IM.MasterCompanyId=" + Session["varCompanyId"];

                    }
                    else
                    {   

                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                      Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,
                        SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo,isnull(CustInfo.CustomerCode,'') as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,
                        isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks
                        ,(Select distinct ID2.lotno+',' From IndentDetail ID2 Where ID2.IndentId=PT.IndentId and ID2.OFinishedId=PT.Finishedid FOR XML PATH('')) as IssLotNo 
                        ,(Select distinct ID2.TagNo+',' From IndentDetail ID2 Where ID2.IndentId=PT.IndentId and ID2.OFinishedId=PT.Finishedid FOR XML PATH('')) as IssTagNo 
                        ,IM.Date as IndentDate,isnull(PT.Rate,0) as IndentRate,PM.BillNo
                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                        inner join EmpInfo E on PM.Empid=E.EmpId
                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                        LEFT JOIN OrderMaster OM ON OM.OrderId in(Select ID.OrderID From IndentDetail ID JOIN OrderMaster OM3 ON ID.OrderID=OM3.OrderID  Where ID.IndentId=IM.IndentId)
                        LEFT JOIN CustomerInfo CustInfo ON OM.CustomerID=CustInfo.CustomerID
                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "16":
                                str = str + " and PT.Rec_Iss_ItemFlag=0";
                                break;
                            default:
                                break;
                        }
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = @"select pnm.process_name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,ei.EmpName,case When CHARINDEX('s',sm.indentNo)=0 then 'S-'+sm.indentNo else Sm.indentNo End as Indentno
                                ,Sm.indentno as Issuechallanno,srm.ChallanNo as RecChallanNo,
                                Case When CI.MasterCompanyid=14 Then Srm.ReceiveDate Else Replace(convert(varchar(11),Srm.ReceiveDate,106),' ','-') End as Date,Srd.Rfinishedid as Finishedid,srd.LotNo,
                                Vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName+' '+Vf.ShapeName+' '+
                                case When vf.SizeId>0 then vf.SizeFt else '' End As Description,Sum(Srd.recqty) as Recqty,sum(srd.lossqty) as Lossqty,0 as retqty,
                                '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,'' as Localorder,'' as Customerorderno,0 as Lshort,0 as shrinkage,sm.id as prmid,pnm.process_name as Re_process,
                                SUM(SRD.UNDYEDQTY) AS UNDYEDQTY,Srd.TagNo,'' as CustomerCode,0 as Moisture,'' as CheckedBy,'' as IndentRecRemarks
                                 ,'' as IssLotNo,'' as IssTagNo,'' as IndentDate,0 as IndentRate,'' BillNo
                                From SampleDyeingReceivemaster Srm inner join SampleDyeingReceiveDetail srd on srm.ID=srd.Masterid
                                inner join SampleDyeingmaster sm on srd.issueid=sm.ID
                                inner join companyinfo ci on srm.companyid=ci.companyid
                                inner join EmpInfo ei on srm.empid=ei.EmpId
                                inner join PROCESS_NAME_MASTER pnm on sm.processid=pnm.PROCESS_NAME_ID
                                inner join V_FinishedItemDetail vf on srd.Rfinishedid=vf.ITEM_FINISHED_ID Where 1=1 and (Recqty+lossqty+undyedqty)>0";
                        }

                    }
                    if (DDCompany.SelectedIndex > 0)
                    {
                        if (variable.JoborderNewModule == "1")
                        {
                            str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                        }
                        else
                        {
                            str = str + " And PRM.CompanyId=" + DDCompany.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + " And SRM.CompanyId=" + DDCompany.SelectedValue;
                            }
                        }
                    }
                    if (DDCustCode.SelectedIndex > 0)
                    {
                        str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                    }
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                    }
                    if (DDProcessName.SelectedIndex > 0)
                    {
                        str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Sm.Processid=" + DDProcessName.SelectedValue;
                        }
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Srm.EmpId=" + DDEmpName.SelectedValue;
                        }

                    }
                    if (DDIndentNo.SelectedIndex > 0)
                    {
                        if (chksample.Checked == true)
                        {
                            strsample = strsample + "  And sm.id=" + DDIndentNo.SelectedValue;
                            str = str + "  And IM.IndentId=0";
                        }
                        else
                        {
                            str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + "  And sm.id=0";
                            }
                        }
                    }
                    else
                    {
                        if (chksample.Checked == true)
                        {
                            str = str + "  And IM.IndentId=0";
                        }
                    }
                    if (TRPartyChallanNo.Visible == true)
                    {
                        if (txtPartyChallanNo.Text != "")
                        {
                            str = str + "  And PM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                            strsample = strsample + "  And SRM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                        }
                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        }
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        }
                    }
                    if (DDShadeColor.SelectedIndex > 0)
                    {
                        str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        }
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                        strsample = strsample + "  And Srm.Receivedate>='" + TxtFromDate.Text + "' And srm.Receivedate<='" + TxtToDate.Text + "'";
                    }
                    if (variable.JoborderNewModule == "1")
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid,IM.Date,PT.Rate,PM.BillNo ";
                    }
                    else
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PRM.ChallanNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,CustInfo.CustomerCode,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid,PT.IndentId ,IM.Date,PT.Rate,PM.BillNo";
                        strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Srd.TagNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID,CI.MasterCompanyid  ";

                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            str = str + " UNION ALL " + strsample;
                        }
                    }
                    str = str + " Order by  Indentno";
                }
                else if(Session["VarCompanyNo"].ToString()=="43")
                {
                    if (variable.JoborderNewModule == "1")
                    {
                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End as CompanyName,Empname,IndentNo,
                            '' as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                            Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                            ITEM_NAME,QualityName,ShadeColorName,
                            CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                            case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft  End As Description,Sum(RecQuantity) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,
                            '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid
                            ,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process ,PT.TagNo,'' as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks
                            ,sum(isnull(PT.IssueQtyOnMachine,0)) as IssueQtyOnMachine,
                            
                            ----,Case When CI.MasterCompanyid=43 Then isnull((select Distinct VF2.QualityName+',' From ProcessProgram PP(NoLock) JOIN OrderMaster OM2(NoLock) on PP.Order_ID=OM2.OrderId  
	                            ----JOIN OrderDetail OD2 ON OM2.OrderId=OD2.OrderId JOIN V_FinishedItemDetail VF2(NoLock) ON OD2.Item_Finished_Id=VF2.ITEM_FINISHED_ID
	                            ----Where OM2.OrderId=OM.OrderId for xml path('')),'') Else '' End as OrderQuality,
                            
                            Case When CI.MasterCompanyid=43 Then isnull((select Distinct VF2.QualityName+',' From PP_Consumption PP(NoLock) 
                            JOIN OrderMaster OM2(NoLock) on PP.OrderId=OM2.OrderId  
                            JOIN OrderDetail OD2 ON OM2.OrderId=OD2.OrderId and PP.OrderDetailId=OD2.OrderDetailId
                            JOIN V_FinishedItemDetail VF2(NoLock) ON OD2.Item_Finished_Id=VF2.ITEM_FINISHED_ID
                            Where PP.FinishedId=PT.FinishedId and OM2.OrderId=OM.OrderId for xml path('')),'') Else '' End as OrderQuality,                            
                            GM.GodownName
                            From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                            inner join IndentMaster Im on PT.IndentId=IM.IndentID
                            inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                            inner join EmpInfo E on PM.Empid=E.EmpId
                            inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                            inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                            inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID                           
                            left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                            left join OrderMaster OM on OD.OrderId=OM.OrderId
                            left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                            INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                            JOIN GodownMaster GM ON PT.Godownid=GM.GoDownID
                            Where IM.MasterCompanyId=" + Session["varCompanyId"];

                    }
                    else
                    {
                        ////                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
                        ////                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,PM.Date,Finishedid,PT.LotNo,
                        ////                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                        ////                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
                        ////                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
                        ////                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                        ////                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
                        ////                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo
                        ////                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        ////                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
                        ////                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                        ////                        inner join EmpInfo E on PM.Empid=E.EmpId
                        ////                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        ////                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        ////                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                        ////                        left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                        ////                        left join OrderMaster OM on OD.OrderId=OM.OrderId
                        ////                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                        ////                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        ////                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];


                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                      Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                      ITEM_NAME,QualityName,ShadeColorName,
                       CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,
                        SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo,isnull(CustInfo.CustomerCode,'') as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,
                        isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks ,sum(isnull(PT.IssueQtyOnMachine,0)) as IssueQtyOnMachine,
                        
                        ----,Case When CI.MasterCompanyid=43 Then isnull((select Distinct VF2.QualityName+',' From ProcessProgram PP(NoLock) JOIN OrderMaster OM2(NoLock) on PP.Order_ID=OM2.OrderId  
	                            ----JOIN OrderDetail OD2 ON OM2.OrderId=OD2.OrderId JOIN V_FinishedItemDetail VF2(NoLock) ON OD2.Item_Finished_Id=VF2.ITEM_FINISHED_ID
	                            ----Where OM2.OrderId=OM.OrderId for xml path('')),'') Else '' End as OrderQuality,

                        Case When CI.MasterCompanyid=43 Then isnull((select Distinct VF2.QualityName+',' From PP_Consumption PP(NoLock) 
                            JOIN OrderMaster OM2(NoLock) on PP.OrderId=OM2.OrderId  
                            JOIN OrderDetail OD2 ON OM2.OrderId=OD2.OrderId and PP.OrderDetailId=OD2.OrderDetailId
                            JOIN V_FinishedItemDetail VF2(NoLock) ON OD2.Item_Finished_Id=VF2.ITEM_FINISHED_ID
                            Where PP.FinishedId=PT.FinishedId and OM2.OrderId=OM.OrderId for xml path('')),'') Else '' End as OrderQuality,
                        GM.GodownName
                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                        inner join EmpInfo E on PM.Empid=E.EmpId
                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                        LEFT JOIN OrderMaster OM ON OM.OrderId in(Select ID.OrderID From IndentDetail ID JOIN OrderMaster OM3 ON ID.OrderID=OM3.OrderID  Where ID.IndentId=IM.IndentId)
                        LEFT JOIN CustomerInfo CustInfo ON OM.CustomerID=CustInfo.CustomerID
                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        JOIN GodownMaster GM ON PT.Godownid=GM.GoDownID
                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "16":
                                str = str + " and PT.Rec_Iss_ItemFlag=0";
                                break;
                            default:
                                break;
                        }
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = @"select pnm.process_name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,ei.EmpName,case When CHARINDEX('s',sm.indentNo)=0 then 'S-'+sm.indentNo else Sm.indentNo End as Indentno
                                ,Sm.indentno as Issuechallanno,srm.ChallanNo as RecChallanNo,
                                Case When CI.MasterCompanyid=14 Then Srm.ReceiveDate Else Replace(convert(varchar(11),Srm.ReceiveDate,106),' ','-') End as Date,Srd.Rfinishedid as Finishedid,srd.LotNo,
                                Vf.ITEM_NAME,Vf.QualityName,Vf.ShadeColorName,
                                Vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName+' '+Vf.ShapeName+' '+
                                case When vf.SizeId>0 then vf.SizeFt else '' End As Description,Sum(Srd.recqty) as Recqty,sum(srd.lossqty) as Lossqty,0 as retqty,
                                '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,'' as Localorder,'' as Customerorderno,0 as Lshort,0 as shrinkage,sm.id as prmid,pnm.process_name as Re_process,
                                SUM(SRD.UNDYEDQTY) AS UNDYEDQTY,Srd.TagNo,'' as CustomerCode,0 as Moisture,'' as CheckedBy,'' as IndentRecRemarks 
                                ,sum(isnull(srd.IssQtyOnMachine,0)) as IssueQtyOnMachine,'' as OrderQuality,GM.GodownName
                                From SampleDyeingReceivemaster Srm inner join SampleDyeingReceiveDetail srd on srm.ID=srd.Masterid
                                inner join SampleDyeingmaster sm on srd.issueid=sm.ID
                                inner join companyinfo ci on srm.companyid=ci.companyid
                                inner join EmpInfo ei on srm.empid=ei.EmpId
                                inner join PROCESS_NAME_MASTER pnm on sm.processid=pnm.PROCESS_NAME_ID
                                inner join V_FinishedItemDetail vf on srd.Rfinishedid=vf.ITEM_FINISHED_ID 
                                JOIN GodownMaster GM ON srd.Godownid=GM.GoDownID
                                Where 1=1 and (Recqty+lossqty+undyedqty)>0";
                        }

                    }
                    if (DDCompany.SelectedIndex > 0)
                    {
                        if (variable.JoborderNewModule == "1")
                        {
                            str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                        }
                        else
                        {
                            str = str + " And PRM.CompanyId=" + DDCompany.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + " And SRM.CompanyId=" + DDCompany.SelectedValue;
                            }
                        }
                    }
                    if (DDCustCode.SelectedIndex > 0)
                    {
                        str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                    }
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                    }
                    if (DDProcessName.SelectedIndex > 0)
                    {
                        str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Sm.Processid=" + DDProcessName.SelectedValue;
                        }
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Srm.EmpId=" + DDEmpName.SelectedValue;
                        }

                    }
                    if (DDIndentNo.SelectedIndex > 0)
                    {
                        if (chksample.Checked == true)
                        {
                            strsample = strsample + "  And sm.id=" + DDIndentNo.SelectedValue;
                            str = str + "  And IM.IndentId=0";
                        }
                        else
                        {
                            str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + "  And sm.id=0";
                            }
                        }
                    }
                    else
                    {
                        if (chksample.Checked == true)
                        {
                            str = str + "  And IM.IndentId=0";
                        }
                    }
                    if (TRPartyChallanNo.Visible == true)
                    {
                        if (txtPartyChallanNo.Text != "")
                        {
                            str = str + "  And PM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                            strsample = strsample + "  And SRM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                        }
                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        }
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        }
                    }
                    if (DDShadeColor.SelectedIndex > 0)
                    {
                        str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        }
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                        strsample = strsample + "  And Srm.Receivedate>='" + TxtFromDate.Text + "' And srm.Receivedate<='" + TxtToDate.Text + "'";
                    }
                    if (variable.JoborderNewModule == "1")
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid,OM.OrderID ,GM.GodownName";
                    }
                    else
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PRM.ChallanNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,CustInfo.CustomerCode,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid,OM.OrderID,GM.GodownName ";
                        strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Srd.TagNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID,CI.MasterCompanyid,GM.GodownName  ";

                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            str = str + " UNION ALL " + strsample;
                        }
                    }
                    str = str + " Order by  RecChallanNo";
                }
                else
                {
                    if (variable.JoborderNewModule == "1")
                    {
                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End as CompanyName,Empname,IndentNo,
                            '' as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                            Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                            CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                            case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft  End As Description,Sum(RecQuantity) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,
                            '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid
                            ,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process ,PT.TagNo,'' as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks
                            From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                            inner join IndentMaster Im on PT.IndentId=IM.IndentID
                            inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                            inner join EmpInfo E on PM.Empid=E.EmpId
                            inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                            inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                            inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID                           
                            left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                            left join OrderMaster OM on OD.OrderId=OM.OrderId
                            left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                            INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                            Where IM.MasterCompanyId=" + Session["varCompanyId"];

                    }
                    else
                    {
                        ////                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
                        ////                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,PM.Date,Finishedid,PT.LotNo,
                        ////                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                        ////                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
                        ////                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
                        ////                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                        ////                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
                        ////                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo
                        ////                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        ////                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
                        ////                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                        ////                        inner join EmpInfo E on PM.Empid=E.EmpId
                        ////                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        ////                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        ////                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                        ////                        left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                        ////                        left join OrderMaster OM on OD.OrderId=OM.OrderId
                        ////                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                        ////                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        ////                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];


                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IndentNo,
                      PRM.ChallanNo as IssueChallanNo,PM.ChallanNo As RecChallanNo,
                      Case When CI.MasterCompanyid=14 Then PM.Date Else Replace(convert(varchar(11),PM.Date,106),' ','-') End as Date,Finishedid,PT.LotNo,
                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName+' '+
                      case When PT.unitId=1 Then Sizemtr Else Case When PT.UnitId=2 Then Sizeft Else case When PT.UnitId=6 Then Sizeinch 
                      Else Sizemtr End End End As Description,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) AS RECQTY,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty
                      ,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                       OM.LocalOrder,OM.CustomerOrderNo,isnull(sum(Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,PM.PRMid,
                       case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,
                        SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,pT.TagNo,isnull(CustInfo.CustomerCode,'') as CustomerCode,isnull(sum(PT.Moisture),0) as Moisture,
                        isnull(PM.CheckedBy,'') as CheckedBy,isnull(PM.RRRemark,'') as IndentRecRemarks 
                        From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        inner join IndentMaster Im on PT.IndentId=IM.IndentID
                        inner join CompanyInfo CI on Im.CompanyId=CI.CompanyId
                        inner join EmpInfo E on PM.Empid=E.EmpId
                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID
                        LEFT JOIN OrderMaster OM ON OM.OrderId in(Select ID.OrderID From IndentDetail ID JOIN OrderMaster OM3 ON ID.OrderID=OM3.OrderID  Where ID.IndentId=IM.IndentId)
                        LEFT JOIN CustomerInfo CustInfo ON OM.CustomerID=CustInfo.CustomerID
                        left join V_IndentRawReturnQty V on Pt.PRMid=v.prmid and Pt.PRTid=v.PrtId 
                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        Where PRM.MasterCompanyId=" + Session["varCompanyId"];
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "16":
                                str = str + " and PT.Rec_Iss_ItemFlag=0";
                                break;
                            default:
                                break;
                        }
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = @"select pnm.process_name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,ei.EmpName,case When CHARINDEX('s',sm.indentNo)=0 then 'S-'+sm.indentNo else Sm.indentNo End as Indentno
                                ,Sm.indentno as Issuechallanno,srm.ChallanNo as RecChallanNo,
                                Case When CI.MasterCompanyid=14 Then Srm.ReceiveDate Else Replace(convert(varchar(11),Srm.ReceiveDate,106),' ','-') End as Date,Srd.Rfinishedid as Finishedid,srd.LotNo,
                                Vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName+' '+Vf.ShapeName+' '+
                                case When vf.SizeId>0 then vf.SizeFt else '' End As Description,Sum(Srd.recqty) as Recqty,sum(srd.lossqty) as Lossqty,0 as retqty,
                                '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,'' as Localorder,'' as Customerorderno,0 as Lshort,0 as shrinkage,sm.id as prmid,pnm.process_name as Re_process,
                                SUM(SRD.UNDYEDQTY) AS UNDYEDQTY,Srd.TagNo,'' as CustomerCode,0 as Moisture,'' as CheckedBy,'' as IndentRecRemarks
                                From SampleDyeingReceivemaster Srm inner join SampleDyeingReceiveDetail srd on srm.ID=srd.Masterid
                                inner join SampleDyeingmaster sm on srd.issueid=sm.ID
                                inner join companyinfo ci on srm.companyid=ci.companyid
                                inner join EmpInfo ei on srm.empid=ei.EmpId
                                inner join PROCESS_NAME_MASTER pnm on sm.processid=pnm.PROCESS_NAME_ID
                                inner join V_FinishedItemDetail vf on srd.Rfinishedid=vf.ITEM_FINISHED_ID Where 1=1 and (Recqty+lossqty+undyedqty)>0";
                        }

                    }
                    if (DDCompany.SelectedIndex > 0)
                    {
                        if (variable.JoborderNewModule == "1")
                        {
                            str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                        }
                        else
                        {
                            str = str + " And PRM.CompanyId=" + DDCompany.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + " And SRM.CompanyId=" + DDCompany.SelectedValue;
                            }
                        }
                    }
                    if (DDCustCode.SelectedIndex > 0)
                    {
                        str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                    }
                    if (DDOrderNo.SelectedIndex > 0)
                    {
                        str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                    }
                    if (DDProcessName.SelectedIndex > 0)
                    {
                        str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Sm.Processid=" + DDProcessName.SelectedValue;
                        }
                    }
                    if (DDEmpName.SelectedIndex > 0)
                    {
                        str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " And Srm.EmpId=" + DDEmpName.SelectedValue;
                        }

                    }
                    if (DDIndentNo.SelectedIndex > 0)
                    {
                        if (chksample.Checked == true)
                        {
                            strsample = strsample + "  And sm.id=" + DDIndentNo.SelectedValue;
                            str = str + "  And IM.IndentId=0";
                        }
                        else
                        {
                            str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                            if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                            {
                                strsample = strsample + "  And sm.id=0";
                            }
                        }
                    }
                    else
                    {
                        if (chksample.Checked == true)
                        {
                            str = str + "  And IM.IndentId=0";
                        }
                    }
                    if (TRPartyChallanNo.Visible == true)
                    {
                        if (txtPartyChallanNo.Text != "")
                        {
                            str = str + "  And PM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                            strsample = strsample + "  And SRM.ChallanNo='" + txtPartyChallanNo.Text + "'";
                        }
                    }
                    if (ddItemName.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                        }
                    }
                    if (DDQuality.SelectedIndex > 0)
                    {
                        str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                        }
                    }
                    if (DDShadeColor.SelectedIndex > 0)
                    {
                        str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                        }
                    }
                    if (ChkForDate.Checked == true)
                    {
                        str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                        strsample = strsample + "  And Srm.Receivedate>='" + TxtFromDate.Text + "' And srm.Receivedate<='" + TxtToDate.Text + "'";
                    }
                    if (variable.JoborderNewModule == "1")
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,Pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid ";
                    }
                    else
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IndentNo,PRM.ChallanNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,PT.TagNo,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,PM.PRMid,ID.Re_Process,CustInfo.CustomerCode,PM.CheckedBy,PM.RRRemark,CI.MasterCompanyid ";
                        strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Srd.TagNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID,CI.MasterCompanyid  ";

                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            str = str + " UNION ALL " + strsample;
                        }
                    }
                    str = str + " Order by  Indentno";
                }

                
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
                }

                if (ChkForExportExcel.Checked == true)
                {
                    if (Session["VarCompanyNo"].ToString() == "22")
                    {
                        ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                        IndentRecDetailExcelReportDiamond(ds);
                    }
                    else
                    {

                        ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                        IndentRecDetailExcelReport(ds);
                    }
                }
                else
                {
                    switch (Session["varcompanyid"].ToString())
                    {
                        case "6":
                        case "12":
                            Session["rptFilename"] = "Reports/RptIndentRawRecDetailIndentWiseArtindia.rpt";
                            break;
                        case "43":
                            if (ChkForIndentRecMachineIssQtyWise.Checked == true)
                            {
                                Session["rptFilename"] = "Reports/RptIndentRawRecDetailMachineIssQtyWiseCI.rpt";
                            }
                            else
                            {
                                Session["rptFilename"] = "Reports/RptIndentRawRecDetailIndentWise.rpt";
                            }
                            break;
                        default:
                            Session["rptFilename"] = "Reports/RptIndentRawRecDetailIndentWise.rpt";
                            break;

                    }
                    Session["dsFilename"] = "~\\ReportSchema\\RptIndentRawRecDetailIndentWise.xsd";


                    ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                    Session["GetDataset"] = ds;
                }
            }
            if (RDProcessIss_REcDetail.Checked == true)
            {
                if (variable.JoborderNewModule == "1")
                {
                    str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,IndentNo,PM.GatePassNo As GateNo,ChallanNo,PM.Date,Finishedid,LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName As Description,
                        case When PT.flagsize=1 Then vf.Sizemtr When PT.flagsize=0 Then Sizeft When Pt.flagsize=2 Then vf.Sizeinch Else vf.Sizeft End  As Size,
                        Sum(issueQuantity-isnull(canQty,0)) As IssueQty,0 As RecQty,0 As LossQty,0 As RetQty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + @"' As ToDate,
                        " + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                        case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process 
                        From PP_ProcessRawMaster PM inner join PP_ProcessRawTran PT on PM.PRMid=PT.PRMid
                        INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId
                        inner join CompanyInfo CI on IM.CompanyId=CI.CompanyId
                        inner join EmpInfo E on PM.empid=E.EmpId
                        inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID
                        left join OrderDetail OD on PT.Orderdetailid=OD.OrderDetailId
                        left join OrderMaster OM on OD.OrderId=OM.OrderId
                        inner join V_FinishedItemDetail vf on pt.Finishedid=vf.ITEM_FINISHED_ID Where PM.MasterCompanyId=" + Session["varCompanyId"];
                }
                else
                {
                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,Empname,IM.IndentId,
                    IndentNo,PM.GatePassNo As GateNo,ChallanNo,PM.Date,Finishedid,LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName 
                    As Description,case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch Else Sizemtr End End End As Size,
                    Sum(issueQuantity-isnull(canQty,0)) As IssueQty,0 As RecQty,0 As LossQty,0 As RetQty,'" + TxtFromDate.Text + " ' As FromDate,'" + TxtToDate.Text + @"' As ToDate,
                    " + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                    case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,0 AS UNDYEDQTY,ND.UserName
                    From PP_ProcessRawMaster PM INNER JOIN PP_ProcessRawTran PT ON PM.PrmId=PT.PrmId
                    INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId
                    INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.Item_Finished_Id
                    INNER JOIN Empinfo E ON Pm.EmpId=E.EmpId
                    INNER JOIN OrderMaster OM ON OM.Orderid=IM.OrderId
                    INNER JOIN Companyinfo CI ON PM.CompanyId=CI.CompanyId
                    INNER JOIN Process_Name_Master PNM ON PM.ProcessId=PNM.PROCESS_NAME_ID
                    LEFT JOIN NEWUSERDETAIL ND(Nolock) ON PM.USERID = ND.USERID 
                    Where PM.MasterCompanyId=" + Session["varCompanyId"];



                    }
                    else
                    {
                        str = @"select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,
                    IndentNo,PM.GatePassNo As GateNo,ChallanNo,PM.Date,Finishedid,LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName 
                    As Description,case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch Else Sizemtr End End End As Size,
                    Sum(issueQuantity-isnull(canQty,0)) As IssueQty,0 As RecQty,0 As LossQty,0 As RetQty,'" + TxtFromDate.Text + " ' As FromDate,'" + TxtToDate.Text + @"' As ToDate,
                    " + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo,0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                    case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,0 AS UNDYEDQTY
                    From PP_ProcessRawMaster PM INNER JOIN PP_ProcessRawTran PT ON PM.PrmId=PT.PrmId
                    INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId
                    INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.Item_Finished_Id
                    INNER JOIN Empinfo E ON Pm.EmpId=E.EmpId
                    INNER JOIN OrderMaster OM ON OM.Orderid=IM.OrderId
                    INNER JOIN Companyinfo CI ON PM.CompanyId=CI.CompanyId
                    INNER JOIN Process_Name_Master PNM ON PM.ProcessId=PNM.PROCESS_NAME_ID
                    Where PM.MasterCompanyId=" + Session["varCompanyId"];


                    
                    }
                   

                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = @"SELECT PNM.PROCESS_NAME,CASE WHEN " + DDCompany.SelectedIndex + @">0 THEN CI.COMPANYNAME ELSE 'ALL' END AS COMPANYNAME,EMPNAME,SM.ID AS INDENTID,CASE WHEN CHARINDEX('S',SM.INDENTNO)=0 THEN 'S-'+SM.INDENTNO ELSE SM.INDENTNO END AS INDENTNO
                               ,SM.Gatepassno as GATENO ,CASE WHEN CHARINDEX('S',SM.INDENTNO)=0 THEN 'S-'+SM.INDENTNO ELSE SM.INDENTNO END AS CHALLANNO,SM.ISSUEDATE AS DATE,SD.IFINISHEDID AS FINISHEDID,SD.LOTNO,
                                CATEGORY_NAME+' '+ITEM_NAME+' '+QUALITYNAME+' '+DESIGNNAME+' '+COLORNAME+' '+SHADECOLORNAME+' '+SHAPENAME AS DESCRIPTION,
                                CASE WHEN SD.UNITID=1 THEN SIZEMTR ELSE CASE WHEN SD.UNITID=2 THEN SIZEFT ELSE CASE WHEN SD.UNITID=6 THEN SIZEINCH ELSE SIZEMTR END END END as Size,
                                SUM(SD.ISSUEQTY) AS ISSUEQTY,0 As RecQty,0 As LossQty,0 As RetQty,'" + TxtFromDate.Text + " ' AS FROMDATE,'" + TxtToDate.Text + "' AS TODATE,'" + VarDateflag + @"' AS DATEFLAG,'' LOCALORDER,
                                ''  AS CUSTOMERORDERNO,0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,PNM.PROCESS_NAME AS RE_PROCESS,0 as Undyedqty
                                 FROM SAMPLEDYEINGMASTER SM INNER JOIN  SAMPLEDYEINGDETAIL SD ON SM.ID=SD.MASTERID
                                INNER JOIN COMPANYINFO CI ON SM.COMPANYID=CI.COMPANYID
                                INNER JOIN V_FINISHEDITEMDETAIL VF ON SD.IFINISHEDID=VF.ITEM_FINISHED_ID
                                INNER JOIN PROCESS_NAME_MASTER PNM ON SM.PROCESSID=PNM.PROCESS_NAME_ID
                                INNER JOIN EMPINFO EI ON SM.EMPID=EI.EMPID Where 1=1";
                    }

                }
                if (DDCompany.SelectedIndex > 0)
                {
                    str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.companyid=" + DDCompany.SelectedValue;
                    }
                }
                if (DDCustCode.SelectedIndex > 0)
                {
                    str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                }
                if (DDOrderNo.SelectedIndex > 0)
                {
                    str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                }
                if (DDProcessName.SelectedIndex > 0)
                {
                    str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.processid=" + DDProcessName.SelectedValue;
                    }
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.empid=" + DDEmpName.SelectedValue;
                    }
                }
                if (DDIndentNo.SelectedIndex > 0)
                {
                    if (chksample.Checked == true)
                    {
                        strsample = strsample + " and SM.ID=" + DDIndentNo.SelectedValue;
                        str = str + "  And IM.IndentId=0";
                    }
                    else
                    {
                        str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + " and SM.ID=0";
                        }

                    }
                }
                else
                {
                    if (chksample.Checked == true)
                    {
                        str = str + "  And IM.IndentId=0";
                    }
                }
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    }
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    }
                }
                if (DDShadeColor.SelectedIndex > 0)
                {
                    str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    }
                }
                if (ChkForDate.Checked == true)
                {
                    str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  And SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
                    }
                }
                if (variable.JoborderNewModule == "1")
                {
                    str = str + " group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GatePassNo,ChallanNo,PM.Date,Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,PT.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,IM.Re_Process  union All";
                }
                else
                {
                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        str = str + " group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GatePassNo,ChallanNo,PM.Date,Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,IM.Re_Process,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,ND.UserName ";
                    }
                    else
                    {
                        str = str + " group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GatePassNo,ChallanNo,PM.Date,Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,IM.Re_Process";
                    
                    }
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " GROUP BY PROCESS_NAME,CI.COMPANYNAME,EMPNAME,SM.ID,SM.Gatepassno,INDENTNO,SM.ISSUEDATE,SD.IFINISHEDID,SD.LOTNO,CATEGORY_NAME,ITEM_NAME,QUALITYNAME,DESIGNNAME,COLORNAME,SHADECOLORNAME,SHAPENAME,SD.UNITID,SIZEMTR,SIZEFT,SIZEINCH,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId";
                        str = str + "   union All " + strsample + " UNION ALL ";
                    }
                    else
                    {
                        str = str + "   union All ";
                    }
                }
                if (variable.JoborderNewModule == "1")
                {
                    str = str + @"  select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,IndentNo
                            ,PM.GateINNo As GateNo,PM.ChallanNo,PM.Date,Finishedid,PT.LotNo,
                            CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName As Description,
                            case When PT.flagsize=1 Then vf.Sizemtr  When PT.flagsize=0 Then vf.Sizeft  When Pt.flagsize=2 Then Sizeinch 
                            Else vf.Sizeft End As Size,0 As IssueQty,Sum(RecQuantity) As RecQty,Sum(LossQty) As LossQty,
                            isnull(Sum(RetQty),0) As RetQty,'" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,
                            OM.LocalOrder,OM.CustomerOrderNo,isnull(Sum(PT.Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId
                            ,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process
                            From PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                            inner join IndentMaster Im on PT.IndentId=IM.IndentID
                            inner join companyinfo CI on PM.Companyid=Ci.CompanyId
                            inner join EmpInfo E on PM.Empid=e.EmpId
                            inner join PROCESS_NAME_MASTER PNM on pM.Processid=PNM.process_name_id
                            inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                            inner join V_FinishedItemDetail vf on PT.Finishedid=vf.ITEM_FINISHED_ID                           
                            left join OrderDetail oD on PT.Orderdetailid=OD.OrderDetailId
                            left join OrderMaster OM on OD.OrderId=OM.OrderId
                            left join V_IndentRawReturnQty  V on Pm.PRMid=v.prmid and PT.PRTid=v.PrtId 
                            INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                            Where IM.MasterCompanyId=" + Session["varCompanyId"];

                }
                else
                {

                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        str = str + "  select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,Empname,IM.IndentId,IndentNo
                      ,PM.GateINNo As GateNo,PM.ChallanNo,PM.Date,Finishedid,LotNo,
                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName As Description,case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch 
                      Else Sizemtr End End End As Size,0 As IssueQty,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,'" + TxtFromDate.Text + @"' As FromDate,
                      '" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(Sum(PT.Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage
                      ,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                        case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY,ND.UserName  
                        FROM PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentID
                        INNER JOIN OrderMaster OM ON OM.OrderId=IM.OrderId
                        INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.ITEM_FINISHED_ID
                        INNER JOIN EmpInfo E on PM.Empid=E.EmpId
                        INNER JOIN CompanyInfo CI on IM.CompanyId=CI.CompanyId
                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        left outer Join V_IndentRawReturnQty V  on V.prmId=Pt.PrmId and V.prtId=PT.prtId
                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                        LEFT JOIN NEWUSERDETAIL ND(Nolock) ON PT.USERID = ND.USERID 
                      Where PRM.MasterCompanyId=" + Session["varCompanyId"];

                    }
                    else
                    {
                        str = str + "  select Process_Name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,IndentNo
                      ,PM.GateINNo As GateNo,PM.ChallanNo,PM.Date,Finishedid,LotNo,
                      CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName As Description,case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch 
                      Else Sizemtr End End End As Size,0 As IssueQty,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,'" + TxtFromDate.Text + @"' As FromDate,
                      '" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(Sum(PT.Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage
                      ,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                        case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY 
                        FROM PP_ProcessRecMaster PM inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
                        INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentID
                        INNER JOIN OrderMaster OM ON OM.OrderId=IM.OrderId
                        INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.ITEM_FINISHED_ID
                        INNER JOIN EmpInfo E on PM.Empid=E.EmpId
                        INNER JOIN CompanyInfo CI on IM.CompanyId=CI.CompanyId
                        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
                        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
                        left outer Join V_IndentRawReturnQty V  on V.prmId=Pt.PrmId and V.prtId=PT.prtId
                        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
                       
                      Where PRM.MasterCompanyId=" + Session["varCompanyId"];
                    
                    }
                    

                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = @"select pnm.process_name,case When " + DDCompany.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,ei.EmpName,Sm.iD as indentid,case When CHARINDEX('s',sm.indentNo)=0 then 'S-'+sm.indentNo else Sm.indentNo End as Indentno
                                ,Srm.Gateinno as gateno,srm.ChallanNo as RecChallanNo,Srm.ReceiveDate as Date,Srd.Rfinishedid as Finishedid,srd.LotNo,
                                Vf.CATEGORY_NAME+' '+Vf.ITEM_NAME+' '+Vf.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+Vf.ShadeColorName+' '+Vf.ShapeName As Description, case When vf.SizeId>0 then vf.SizeFt else '' End as Size,
                                0 as Issueqty,Sum(Srd.recqty) as Recqty,sum(srd.lossqty) as Lossqty,0 as retqty,
                                '" + TxtFromDate.Text + "' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @"  As dateflag,'' as Localorder,'' as Customerorderno,
                                0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,pnm.process_name as Re_process,SUM(Srd.Undyedqty) as Undyedqty
                                 From SampleDyeingReceivemaster Srm inner join SampleDyeingReceiveDetail srd on srm.ID=srd.Masterid
                                inner join SampleDyeingmaster sm on srd.issueid=sm.ID
                                inner join companyinfo ci on srm.companyid=ci.companyid
                                inner join EmpInfo ei on srm.empid=ei.EmpId
                                inner join PROCESS_NAME_MASTER pnm on sm.processid=pnm.PROCESS_NAME_ID
                                inner join V_FinishedItemDetail vf on srd.Rfinishedid=vf.ITEM_FINISHED_ID Where 1=1 and (Recqty+lossqty+undyedqty)>0";
                    }
                }
                if (DDCompany.SelectedIndex > 0)
                {
                    str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " And sm.CompanyId=" + DDCompany.SelectedValue;
                    }
                }
                if (DDCustCode.SelectedIndex > 0)
                {
                    str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                }
                if (DDOrderNo.SelectedIndex > 0)
                {
                    str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                }
                if (DDProcessName.SelectedIndex > 0)
                {
                    str = str + " And PM.Processid=" + DDProcessName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " And sm.Processid=" + DDProcessName.SelectedValue;
                    }
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    str = str + " And PM.EmpId=" + DDEmpName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + " And sm.EmpId=" + DDEmpName.SelectedValue;
                    }
                }
                if (DDIndentNo.SelectedIndex > 0)
                {
                    if (chksample.Checked == true)
                    {
                        strsample = strsample + "  And sm.id=" + DDIndentNo.SelectedValue;
                        str = str + "  And IM.IndentId=0";
                    }
                    else
                    {
                        str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  And sm.id=0";
                        }
                    }

                }
                else
                {
                    if (chksample.Checked == true)
                    {
                        str = str + "  And IM.IndentId=0";
                    }
                }
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                    }
                }
                if (DDQuality.SelectedIndex > 0)
                {
                    str = str + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                    }
                }
                if (DDShadeColor.SelectedIndex > 0)
                {
                    str = str + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                    }
                }
                if (ChkForDate.Checked == true)
                {
                    if (variable.JoborderNewModule == "1")
                    {
                        str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                    }
                    else
                    {
                        str = str + "  And PRM.Date>='" + TxtFromDate.Text + "' And PRM.Date<='" + TxtToDate.Text + "'";
                        if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                        {
                            strsample = strsample + "  And srm.receiveDate>='" + TxtFromDate.Text + "' And srm.receiveDate<='" + TxtToDate.Text + "'";
                        }
                    }

                }
                if (variable.JoborderNewModule == "1")
                {
                    str = str + "  group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GateINNo,PM.ChallanNo,PM.Date,Finishedid,PT.Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,pt.flagsize,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,ID.Re_Process";
                }
                else
                {
                    if (Session["varcompanyNo"].ToString() == "44")
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GateINNo,PM.ChallanNo,PM.Date,Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,ID.Re_Process,CI.CompAddr1,CI.CompAddr2,CI.CompAddr3,ND.UserName";
                    }
                    else
                    {
                        str = str + "  group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GateINNo,PM.ChallanNo,PM.Date,Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,ID.Re_Process";
                    
                    }
                    
                    if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                    {
                        strsample = strsample + "  group by pnm.process_name,CI.CompanyName,ei.EmpName,SrM.GateinNo,sm.indentNo,srm.ChallanNo,Srm.ReceiveDate,Srd.Rfinishedid,srd.LotNo,Vf.CATEGORY_NAME,Vf.ITEM_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Vf.ShadeColorName,Vf.ShapeName,vf.SizeId,vf.SizeFt,sm.ID,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId";
                        str = str + " UNION ALL " + strsample;
                    }

                }
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
                }

                switch (variable.JoborderNewModule)
                {
                    case "1":
                        Session["rptFilename"] = "Reports/RptIndentRawIss_RecDetailjobordernew.rpt";
                        break;
                    default:
                        switch (Session["varcompanyid"].ToString())
                        {
                            case "6":
                                Session["rptFilename"] = "Reports/RptIndentRawIss_RecDetailIndentWiseArtindia.rpt";
                                break;
                            case "44":
                                Session["rptFilename"] = "Reports/RptIndentRawIss_RecDetailIndentWise_agni.rpt";
                                break;
                            default:
                                Session["rptFilename"] = "Reports/RptIndentRawIss_RecDetailIndentWise.rpt";
                                break;

                        }
                        break;
                }

                Session["dsFilename"] = "~\\ReportSchema\\RptIndentRawIss_RecDetailIndentWise.xsd";

                ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                Session["GetDataset"] = ds;
            }
            if (RDPONot.Checked == true)
            {
                str = @"select Distinct Process_Name,case When " + DDCompany.SelectedIndex + ">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,INM.IndentNo,INM.Date,'" + TxtFromDate.Text + " ' As FromDate,'" + TxtToDate.Text + "' As ToDate," + VarDateflag + @" As dateflag,OM.LocalOrder,OM.CustomerOrderNo
              From IndentMaster INM,V_Indent_OredrId IM,OrderMaster OM,Empinfo E,Companyinfo CI,Process_Name_Master PNM
              Where INM.IndentId=IM.IndentId And INM.PartyId=E.EmpId And OM.Orderid=IM.OrderId And INM.Indentid Not in(select Distinct IndentId from PP_ProcessRawTran PT Where PT.indentid=INM.IndentId)
              And INM.Status<>'cancelled' And IM.CompanyId=CI.CompanyId And INM.ProcessId=PNM.PROCESS_NAME_ID And INM.MasterCompanyId=" + Session["varCompanyId"];

                if (DDCompany.SelectedIndex > 0)
                {
                    str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                }
                if (DDCustCode.SelectedIndex > 0)
                {
                    str = str + " And OM.CustomerId=" + DDCustCode.SelectedValue;
                }
                if (DDOrderNo.SelectedIndex > 0)
                {
                    str = str + " And OM.OrderId=" + DDOrderNo.SelectedValue;
                }
                if (DDProcessName.SelectedIndex > 0)
                {
                    str = str + " And INM.Processid=" + DDProcessName.SelectedValue;
                }
                if (DDEmpName.SelectedIndex > 0)
                {
                    str = str + " And INM.PartyId=" + DDEmpName.SelectedValue;
                }
                if (DDIndentNo.SelectedIndex > 0)
                {
                    str = str + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
                }
                if (ChkForDate.Checked == true)
                {
                    str = str + "  And INM.Date>='" + TxtFromDate.Text + "' And INM.Date<='" + TxtToDate.Text + "'";
                }
                if (Session["WithoutBOM"].ToString() == "1")
                {
                    str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
                }
                Session["dsFilename"] = "~\\ReportSchema\\RptPONotProceed.xsd";
                Session["rptFilename"] = "Reports/RptPONotProceed.rpt";
                ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                Session["GetDataset"] = ds;
            }
            DataSet DS = (DataSet)(Session["GetDataset"]);
            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["GetDataset"] = DS;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void ShadewiseDetailExportExcelNew()
    {
        try
        {
            // string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ShadeWiseIndentDetail Where Companyid=" + DDCompany.SelectedValue;
            string str = "";
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + "  and Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + "  and Empid=" + DDEmpName.SelectedValue;
            }
            if (DDindentStatus.SelectedIndex > 0)
            {
                str = str + " and Status='" + DDindentStatus.SelectedItem.Text + "'";
            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
                }
            }

            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                if (RDSubitemwiseBalance.Checked == true)
                {
                    str = str + "  and IndentDate<='" + TxtFromDate.Text + "'";
                }
                else
                {
                    str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
                }
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and orderid=" + DDOrderNo.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETSHADEWISEDETAILEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 35.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 25.22;
                sht.Column("D").Width = 25.33;
                sht.Column("E").Width = 9.00;
                sht.Column("F").Width = 18.00;
                sht.Column("G").Width = 9.00;
                sht.Column("H").Width = 9.00;
                sht.Column("I").Width = 9.00;
                sht.Column("J").Width = 9.00;
                sht.Column("K").Width = 9.00;
                sht.Column("L").Width = 9.00;
                sht.Column("M").Width = 9.00;
                sht.Column("N").Width = 9.00;
                sht.Column("O").Width = 9.00;
                sht.Column("P").Width = 9.00;
                sht.Column("Q").Width = 9.00;
                sht.Column("R").Width = 9.00;

                sht.Range("A1:R1").Merge();
                sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                sht.Range("A2:R2").Merge();
                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
                sht.Range("A3:R3").Merge();
                sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"];
                sht.Range("A4:R4").Merge();
                sht.Range("A4").Value = "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
                sht.Row(4).Height = 25;
                sht.Range("A1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:R3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:R4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:R4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:R1").Style.Alignment.SetWrapText();
                sht.Range("A2:R2").Style.Alignment.SetWrapText();
                sht.Range("A3:R3").Style.Alignment.SetWrapText();
                sht.Range("A4:R4").Style.Alignment.SetWrapText();
                sht.Range("A1:R4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:R4").Style.Font.FontSize = 10;
                sht.Range("A1:R4").Style.Font.Bold = true;

                //*******Header
                sht.Range("A5").Value = "ItemName";
                sht.Range("B5").Value = "Sub-ItemName";
                sht.Range("C5").Value = "ColorName";
                sht.Range("D5").Value = "OrderNo";
                sht.Range("E5").Value = "LotNo";
                sht.Range("F5").Value = "TagNo";
                sht.Range("G5").Value = "BillNo";
                sht.Range("H5").Value = "OQty";
                sht.Range("I5").Value = "Dyed Qty";
                sht.Range("J5").Value = "Undyed Qty";
                sht.Range("K5").Value = "Loss Qty";
                sht.Range("L5").Value = "Retn Qty";
                sht.Range("M5").Value = "Pending Qty";
                sht.Range("N5").Value = "Employee Name";
                sht.Range("O5").Value = "Indent No";
                sht.Range("P5").Value = "Order Date";
                sht.Range("Q5").Value = "Gate In No";
                sht.Range("R5").Value = "Receive Date";

                sht.Range("A5:R5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:R5").Style.Font.FontSize = 10;
                sht.Range("A5:R5").Style.Font.Bold = true;

                sht.Range("A5:R5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A5:R5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A5:R5").Style.Alignment.SetWrapText();

                row = row + 6;

                Decimal amt = 0;
                string tagno = "";
                string LotNo = "";
                ////string IndentNo = "";

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":R" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":R" + row).Style.Font.FontSize = 10;



                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["Item_Name"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["ShadeColorName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["OrderNo"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["LotNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["TagNo"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["BillNo"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["OQty"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["dyedqty"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[j]["undyedqty"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[j]["Lossqty"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[j]["Retqty"]);

                    amt = Convert.ToDecimal(ds.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["Retqty"]);

                    //if (LotNo == ds.Tables[0].Rows[j]["LotNo"].ToString() && tagno == ds.Tables[0].Rows[j]["TagNo"].ToString())
                    //{
                    //    amt = amt + Convert.ToDecimal(ds.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["Retqty"]);
                    //}
                    //if (LotNo == ds.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds.Tables[0].Rows[j]["TagNo"].ToString())
                    //{
                    //    amt = 0;
                    //    amt = Convert.ToDecimal(ds.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["Retqty"]);
                    //}
                    //if (LotNo != ds.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds.Tables[0].Rows[j]["TagNo"].ToString())
                    //{
                    //    amt = 0;
                    //    amt = Convert.ToDecimal(ds.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["Retqty"]);
                    //    tagno = ds.Tables[0].Rows[j]["TagNo"].ToString();
                    //    LotNo = ds.Tables[0].Rows[j]["LotNo"].ToString();
                    //    IndentNo = ds.Tables[0].Rows[j]["IndentNo"].ToString();
                    //}
                    ////if (IndentNo != ds.Tables[0].Rows[j]["IndentNo"].ToString() )
                    ////{
                    ////    amt = 0;
                    ////    amt = Convert.ToDecimal(ds.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["Retqty"]);
                    ////    tagno = ds.Tables[0].Rows[j]["TagNo"].ToString();
                    ////    LotNo = ds.Tables[0].Rows[j]["LotNo"].ToString();
                    ////    IndentNo = ds.Tables[0].Rows[j]["IndentNo"].ToString();
                    ////}


                    sht.Range("M" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["OQty"]) - (amt));

                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[j]["IndentDate"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[j]["GateInNo"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[j]["ReceiveDate"]);

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:R" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ShadeWiseDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void BindPPNo()
    {
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_BindPPNoPendingIndentRawIssue", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                UtilityModule.ConditionalComboFillWithDS(ref DDProcessProgram, ds, 0, true, "--Plz Select--");
            }
            else
            {
                DDProcessProgram.Items.Clear();
            }
        }
        catch (Exception)
        {
        }
        finally
        {

        }

    }

    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
        {
            DDCustCode_SelectedIndexChanged(sender, new EventArgs());
        }
        UtilityModule.ConditionalComboFill(ref DDCustCode, "Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "", str1 = "", str2 = "";
        if (Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "28" || Session["varcompanyid"].ToString() == "44")
        {
            str = @"Select distinct OM.OrderId, CustomerOrderNo as CustomerOrderNo 
                From OrderMaster OM 
                Where OM.Status=0";
        }
        else if (Session["varcompanyid"].ToString() == "43")
        {
            str = @"Select distinct OM.OrderId, CustomerOrderNo+ ' / ' +LocalOrder as CustomerOrderNo 
                From OrderMaster OM 
                join V_Indent_OredrId VO ON Om.OrderId=VO.Orderid Where OM.Status=0";
        }
        else
        {
            str = @"Select distinct OM.OrderId, LocalOrder+ ' / ' +CustomerOrderNo as CustomerOrderNo 
                From OrderMaster OM 
                join V_Indent_OredrId VO ON Om.OrderId=VO.Orderid Where OM.Status=0";
        }
        str1 = @" Select Distinct PP.PPID, PP.CHALLANNO  
                From OrderMaster OM 
                JOIN ProcessProgram PP ON PP.Order_ID = OM.OrderID 
                Where OM.Status = 0";

        if (DDCompany.SelectedIndex > 0)
        {
            str = str + " And OM.companyid=" + DDCompany.SelectedValue;
            str1 = str1 + " And OM.CompanyId = " + DDCompany.SelectedValue;
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And Om.customerid=" + DDCustCode.SelectedValue;
            str = str + " And OM.CustomerId = " + DDCustCode.SelectedValue;
        }
        str = str + " Order By CustomerOrderNo";
        str1 = str1 + " Order BY PP.PPID Desc";

        if (variable.JoborderNewModule == "1")
        {
            str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        UtilityModule.ConditionalComboFill(ref DDProcessProgram, str1, true, "--Select--");

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            str2 = @"Select CustomerCode From CustomerInfo Where Customerid=" + DDCustCode.SelectedValue + "";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnCustomerCode.Value = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
            }
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = string.Empty;
        str = @"select Distinct PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME from IndentMaster IM inner join V_Indent_OredrId VO on Im.IndentID=VO.IndentId
                inner join PROCESS_NAME_MASTER PNM on PNM.PROCESS_NAME_ID=IM.ProcessID Where PNM.mastercompanyId=" + Session["varcompanyid"];
        if (DDCompany.SelectedIndex > 0)
        {
            str = str + "  and IM.Companyid=" + DDCompany.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + "  and VO.orderid=" + DDOrderNo.SelectedValue;
        }
        str = str + "  order by PNM.PROCESS_NAME";

        if (variable.JoborderNewModule == "1")
        {
            str = str.Replace("V_Indent_OredrId", "V_Indent_OredrId_withoutBom");
        }
        UtilityModule.ConditionalComboFill(ref DDProcessName, str, true, "--Select--");
    }

    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Q.QualityId,Q.QualityName From Quality Q inner Join ITEM_MASTER IM on Q.Item_Id=IM.ITEM_ID
                    inner join CategorySeparate cs on IM.CATEGORY_ID=cs.Categoryid	 and cs.id=1 ";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and Im.Item_id=" + ddItemName.SelectedValue;
        }
        str = str + " order by QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---ALL---");
        if (DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedIndex = 0;
            DDQuality_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct ShadecolorId,ShadeColorName From V_finisheditemdetail VF Where Vf.ShadeColorName<>''";

        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and VF.QualityId=" + DDQuality.SelectedValue;
        }
        str = str + " order by ShadeColorName";
        UtilityModule.ConditionalComboFill(ref DDShadeColor, str, true, "---Select---");
    }
    protected void chksample_CheckedChanged(object sender, EventArgs e)
    {
        FillIndentNo();
    }
    protected void Orderwiseindentdetail_AGNI()
    {
        string process_id = "0", Customerid = "0", orderid = "0", Orderstatus = "-1", LocalOrder = "";

        if (DDProcessName.SelectedIndex > 0)
        {
            process_id = DDProcessName.SelectedValue;
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            Customerid = DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            orderid = DDOrderNo.SelectedValue;
        }
        if (DDorderstatus.SelectedValue != "-1")
        {
            Orderstatus = DDorderstatus.SelectedValue;
        }

        if (TRLocalOrderNo.Visible == true)
        {
            if (txtLocalOrderNo.Text != "")
            {
                LocalOrder = txtLocalOrderNo.Text;
            }
        }
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetOrderWiseDyingOrderStatusReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@process_id", process_id);
            cmd.Parameters.AddWithValue("@Customerid", Customerid);
            cmd.Parameters.AddWithValue("@orderid", orderid);
            cmd.Parameters.AddWithValue("@Orderstatus", Orderstatus);
            cmd.Parameters.AddWithValue("@LocalOrder", LocalOrder);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

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

                    //*******Header
                    sht.Range("A1:M1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A1:M1").Style.Font.FontSize = 13;
                    sht.Range("A1:M1").Style.Font.Bold = true;
                    sht.Range("A1:M1").Merge();
                    sht.Range("A2:M2").Value = ds.Tables[0].Rows[0]["COMPADDR1"].ToString()+"    "+"Print Date-"+DateTime.Now.ToShortDateString();
                    sht.Range("A2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:M2").Style.Font.FontSize = 13;
                    sht.Range("A2:M2").Style.Font.Bold = true;
                    sht.Range("A2:M2").Merge();
                    //sht.Range("I2:M2").Value = "Print Date-"+DateTime.Now.ToShortDateString();
                    //sht.Range("I2:M2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //sht.Range("I2:M2").Style.Font.FontSize = 13;
                    //sht.Range("I2:M2").Style.Font.Bold = true;
                    //sht.Range("I2:M2").Merge();
                    string PROCESSNAME = string.Empty;
              //  int seletecindex=0;
                if (DDProcessName.SelectedIndex>0)
                {
                    PROCESSNAME = DDProcessName.SelectedItem.Text;
                
                }
               
                    if (ChkForDate.Checked)
                    {
                        sht.Range("A3:M3").Value = "Date From:-" + TxtFromDate.Text + "Date To-" + TxtToDate.Text + "             " + "Order Wise " + PROCESSNAME + " Status ";
                        sht.Range("A3:M3").Merge();
                        sht.Range("A3:M3").Style.Font.FontSize = 11;
                        sht.Range("A3:M3").Style.Font.Bold = true;

                    }
                    else
                    {
                        sht.Range("A3:M3").Value = "Order Wise " + PROCESSNAME+ " Status ";
                        sht.Range("A3:M3").Merge();
                        sht.Range("A3:M3").Style.Font.FontSize = 11;
                        sht.Range("A3:M3").Style.Font.Bold = true;
                    
                    }
                    sht.Range("A3:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4").Value = "B CODE";
                    sht.Range("B4").Value = "BPO";
                    //sht.Range("C4").Value = "DELV DT.";
                    sht.Range("C4").Value = "QUALITY";
                    //sht.Range("D4").Value = "ORDER AREA(SQYD)";
                    sht.Range("D4").Value = "SHADE NO";
                    sht.Range("E4").Value = "CONSUMPTION(1 PC)";
                    sht.Range("F4").Value = "UNIT";
                    sht.Range("G4").Value = "REQ QTY";
                    sht.Range("H4").Value = "ISSUED QTY";
                    sht.Range("I4").Value = "RCVD";
                    sht.Range("J4").Value = "LOSS QTY";
                    sht.Range("K4").Value = "BAL";
                    sht.Range("L4").Value = "REQ QTY BAL";
                    sht.Range("M4").Value = "JOBWORKER NAME";

                    sht.Range("A4:M4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A4:M4").Style.Font.FontSize = 9;
                    sht.Range("A4:M4").Style.Font.Bold = true;
                    sht.Range("G4:M4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("E4:E4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    DataView dv = new DataView(ds.Tables[0]);
                   // dv.Sort = "DispatchDate,customerorderno";
                    dv.Sort = "customerorderno";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());
                    row = 5;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 9;


                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                        //sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Dispatchdate"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                       // sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["orderarea"]);
                        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["shadecolorname"]);
                        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["consmpqty"]);
                        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["unitname"]);
                        sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["reqqty"]);
                        sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["indentqty"]);
                        sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);
                        sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["LossQty"]);

                        sht.Range("K" + row).FormulaA1 = "=(H" + row + '-' + "($I$" + row + '+' + "$J$" + row + "))";
                        //sht.Range("L" + row).FormulaA1 = "=I" + row + '-' + "$J$" + row + "";
                        sht.Range("L" + row).FormulaA1 = "=G" + row + '-' + "$H$" + row + "";
                        sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Dyername"]);

                        row = row + 1;
                    }
                    //*************
                    using (var a = sht.Range("A1:M" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                  //  sht.Columns(1, 30).AdjustToContents();

                    string Fileextension = "xlsx";
                    Orderstatus = "Allorders";
                    if (DDorderstatus.SelectedValue != "-1")
                    {
                        Orderstatus = (DDorderstatus.SelectedValue == "0" ? "Runningorders" : "Completeorders");
                    }

                    string filename = UtilityModule.validateFilename("IndentreqissbalQty" + Orderstatus + "" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void Orderwiseindentdetail()
    {
        string process_id = "0", Customerid = "0", orderid = "0", Orderstatus = "-1", LocalOrder = "";

        if (DDProcessName.SelectedIndex > 0)
        {
            process_id = DDProcessName.SelectedValue;
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            Customerid = DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            orderid = DDOrderNo.SelectedValue;
        }
        if (DDorderstatus.SelectedValue != "-1")
        {
            Orderstatus = DDorderstatus.SelectedValue;
        }

        if (TRLocalOrderNo.Visible == true)
        {
            if (txtLocalOrderNo.Text != "")
            {
                LocalOrder = txtLocalOrderNo.Text;
            }
        }
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetOrderWiseDyingOrderStatusReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@process_id", process_id);
            cmd.Parameters.AddWithValue("@Customerid", Customerid);
            cmd.Parameters.AddWithValue("@orderid", orderid);
            cmd.Parameters.AddWithValue("@Orderstatus", Orderstatus);
            cmd.Parameters.AddWithValue("@LocalOrder", LocalOrder);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["varCompanyNo"].ToString() == "42")
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
                    sht.Range("A1").Value = "B CODE";
                    sht.Range("B1").Value = "BPO";
                    sht.Range("C1").Value = "DELV DT.";
                    sht.Range("D1").Value = "COUNT";
                    sht.Range("E1").Value = "SHADE NO";
                    sht.Range("F1").Value = "ORDER AREA(SQYD)";
                    sht.Range("G1").Value = "LAGAT(SQYD)";
                    sht.Range("H1").Value = "REQ QTY";
                    sht.Range("I1").Value = "INDENT QTY";
                    sht.Range("J1").Value = "REQ BAL QTY";
                    sht.Range("K1").Value = "LOSS QTY";
                    sht.Range("L1").Value = "REC QTY";
                    sht.Range("M1").Value = "BAL TO REC";
                    sht.Range("N1").Value = "DYER NAME";

                    sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:N1").Style.Font.FontSize = 9;
                    sht.Range("A1:N1").Style.Font.Bold = true;
                    sht.Range("G1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("E1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    DataView dv = new DataView(ds.Tables[0]);
                    dv.Sort = "DispatchDate,customerorderno";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());
                    row = 2;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 9;


                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Dispatchdate"]);
                        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["shadecolorname"]);
                        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["orderarea"]);
                        sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["consmpqty"]);
                        sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["reqqty"]);
                        sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["indentqty"]);

                        sht.Range("J" + row).FormulaA1 = "=H" + row + '-' + "$I$" + row + "";

                        //sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);
                        sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["LossQty"]);
                        sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);

                        sht.Range("M" + row).FormulaA1 = "=(I" + row + '-' + "($L$" + row + '+' + "$K$" + row + "))";
                        //sht.Range("L" + row).FormulaA1 = "=I" + row + '-' + "$J$" + row + "";

                        sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Dyername"]);

                        row = row + 1;
                    }
                    //*************
                    using (var a = sht.Range("A1:N" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Columns(1, 30).AdjustToContents();

                    string Fileextension = "xlsx";
                    Orderstatus = "Allorders";
                    if (DDorderstatus.SelectedValue != "-1")
                    {
                        Orderstatus = (DDorderstatus.SelectedValue == "0" ? "Runningorders" : "Completeorders");
                    }

                    string filename = UtilityModule.validateFilename("IndentreqissbalQty" + Orderstatus + "" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                    if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                    }
                    string Path = "";
                    var xapp = new XLWorkbook();
                    var sht = xapp.Worksheets.Add("sheet1");
                    int row = 0;

                    //*******Header
                    sht.Range("A1").Value = "B CODE";
                    sht.Range("B1").Value = "BPO";
                    sht.Range("C1").Value = "DELV DT.";
                    sht.Range("D1").Value = "COUNT";
                    if (Session["VarCompanyNo"].ToString() == "14")
                    {
                        sht.Range("E1").Value = "ORDER AREA(SQM)";
                    }
                    else
                    {
                        sht.Range("E1").Value = "ORDER AREA(SQYD)";
                    }                   
                    sht.Range("F1").Value = "SHADE NO";

                    if (Session["VarCompanyNo"].ToString() == "14")
                    {
                        sht.Range("G1").Value = "LAGAT(SQM)";
                    }
                    else
                    {
                        sht.Range("G1").Value = "LAGAT(SQYD)";
                    } 
                    
                    sht.Range("H1").Value = "REQ QTY";
                    sht.Range("I1").Value = "INDENT QTY";
                    sht.Range("J1").Value = "REC QTY";
                    sht.Range("K1").Value = "LOSS QTY";
                    sht.Range("L1").Value = "INDENT BAL";
                    sht.Range("M1").Value = "REQ QTY BAL";
                    sht.Range("N1").Value = "DYER NAME";

                    sht.Range("A1:N1").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:N1").Style.Font.FontSize = 9;
                    sht.Range("A1:N1").Style.Font.Bold = true;
                    sht.Range("G1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("E1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    DataView dv = new DataView(ds.Tables[0]);
                    dv.Sort = "DispatchDate,customerorderno";
                    DataSet ds1 = new DataSet();
                    ds1.Tables.Add(dv.ToTable());
                    row = 2;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 9;


                        sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["customercode"]);
                        sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["customerorderno"]);
                        sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Dispatchdate"]);
                        sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Qualityname"]);
                        sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["orderarea"]);
                        sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["shadecolorname"]);
                        sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["consmpqty"]);
                        sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["reqqty"]);
                        sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["indentqty"]);
                        sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["recqty"]);
                        sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["LossQty"]);

                        sht.Range("L" + row).FormulaA1 = "=(I" + row + '-' + "($J$" + row + '+' + "$K$" + row + "))";
                        //sht.Range("L" + row).FormulaA1 = "=I" + row + '-' + "$J$" + row + "";
                        sht.Range("M" + row).FormulaA1 = "=H" + row + '-' + "$I$" + row + "";
                        sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["Dyername"]);

                        row = row + 1;
                    }
                    //*************
                    using (var a = sht.Range("A1:N" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    sht.Columns(1, 30).AdjustToContents();

                    string Fileextension = "xlsx";
                    Orderstatus = "Allorders";
                    if (DDorderstatus.SelectedValue != "-1")
                    {
                        Orderstatus = (DDorderstatus.SelectedValue == "0" ? "Runningorders" : "Completeorders");
                    }

                    string filename = UtilityModule.validateFilename("IndentreqissbalQty" + Orderstatus + "" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //protected void ChkForExportExcel_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (ChkForDate.Checked == true)
    //    {
    //        trDates.Visible = true;
    //    }
    //    else
    //    {
    //        trDates.Visible = false;
    //    }
    //}
    protected void ShadewiseDetailExportExcel()
    {
        try
        {
            // string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ShadeWiseIndentDetail Where Companyid=" + DDCompany.SelectedValue;
            string str = "";
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + "  and Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + "  and Empid=" + DDEmpName.SelectedValue;
            }
            if (DDindentStatus.SelectedIndex > 0)
            {
                str = str + " and Status='" + DDindentStatus.SelectedItem.Text + "'";
            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
                }
            }

            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                if (RDSubitemwiseBalance.Checked == true)
                {
                    str = str + "  and IndentDate<='" + TxtFromDate.Text + "'";
                }
                else
                {
                    str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
                }
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and orderid=" + DDOrderNo.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETSHADEWISEDETAILEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 35.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 25.22;
                sht.Column("D").Width = 25.33;
                sht.Column("E").Width = 9.00;
                sht.Column("F").Width = 18.00;
                sht.Column("G").Width = 9.00;
                sht.Column("H").Width = 9.00;
                sht.Column("I").Width = 9.00;
                sht.Column("J").Width = 9.00;
                sht.Column("K").Width = 9.00;
                sht.Column("L").Width = 9.00;
                sht.Column("M").Width = 9.00;
                sht.Column("N").Width = 9.00;

                sht.Range("A1:N1").Merge();
                sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                sht.Range("A2:N2").Merge();
                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
                sht.Range("A3:N3").Merge();
                sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"];
                sht.Range("A4:N4").Merge();
                sht.Range("A4").Value = "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
                sht.Row(4).Height = 30;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N1").Style.Alignment.SetWrapText();
                sht.Range("A2:N2").Style.Alignment.SetWrapText();
                sht.Range("A3:N3").Style.Alignment.SetWrapText();
                sht.Range("A4:N4").Style.Alignment.SetWrapText();
                sht.Range("A1:N4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:N4").Style.Font.FontSize = 10;
                sht.Range("A1:N4").Style.Font.Bold = true;

                //*******Header
                sht.Range("A5").Value = "ItemName";
                sht.Range("B5").Value = "Sub-ItemName";
                sht.Range("C5").Value = "ColorName";
                sht.Range("D5").Value = "OrderNo";
                sht.Range("E5").Value = "LotNo";
                sht.Range("F5").Value = "TagNo";
                sht.Range("G5").Value = "BillNo";
                sht.Range("H5").Value = "OQty";
                sht.Range("I5").Value = "Dyed Qty";
                sht.Range("J5").Value = "Undyed Qty";
                sht.Range("K5").Value = "Loss Qty";
                sht.Range("L5").Value = "Retn Qty";
                sht.Range("M5").Value = "Pending Qty";
                sht.Range("N5").Value = "Gate In No";

                // sht.Column("D").Width = 9.33;


                sht.Range("A5:N5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:N5").Style.Font.FontSize = 10;
                sht.Range("A5:N5").Style.Font.Bold = true;
                //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A5:N5").Style.Alignment.SetWrapText();

                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "EmpId,EmpName,IndentId,IndentNo";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                DataTable dtdistinct = ds1.Tables[0].DefaultView.ToTable(true, "EmpId", "IndentId", "EmpName", "IndentNo", "IndentDate");
                DataView dv1 = new DataView(dtdistinct);
                DataSet ds2 = new DataSet();
                ds2.Tables.Add(dv1.ToTable());
                row = 6;
                int rowfrom = 6;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;


                    sht.Range("A" + row).SetValue("EmpName:" + ds2.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue("IndentNo:" + ds2.Tables[0].Rows[i]["IndentNo"]);
                    sht.Range("C" + row).SetValue("OrderDate:" + Convert.ToDateTime(ds2.Tables[0].Rows[i]["IndentDate"]).ToString("dd-MMM-yyyy"));

                    row = row + 1;

                    DataView dv2 = new DataView(ds1.Tables[0]);
                    dv2.RowFilter = "EmpId=" + ds2.Tables[0].Rows[i]["EmpId"] + " and IndentId=" + ds2.Tables[0].Rows[i]["IndentId"];
                    DataSet ds3 = new DataSet();
                    ds3.Tables.Add(dv2.ToTable());
                    int k = ds3.Tables[0].Rows.Count;
                    Decimal amt = 0;
                    string tagno = "";
                    string LotNo = "";
                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + row).SetValue(ds3.Tables[0].Rows[j]["Item_Name"]);
                        sht.Range("B" + row).SetValue(ds3.Tables[0].Rows[j]["QualityName"]);
                        sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(ds3.Tables[0].Rows[j]["ShadeColorName"]);
                        sht.Range("D" + row).SetValue(ds3.Tables[0].Rows[j]["OrderNo"]);
                        sht.Range("D" + row).Style.Alignment.SetWrapText();
                        sht.Range("E" + row).SetValue(ds3.Tables[0].Rows[j]["LotNo"]);
                        sht.Range("F" + row).SetValue(ds3.Tables[0].Rows[j]["TagNo"]);
                        sht.Range("G" + row).SetValue(ds3.Tables[0].Rows[j]["BillNo"]);
                        sht.Range("H" + row).SetValue(ds3.Tables[0].Rows[j]["OQty"]);
                        sht.Range("I" + row).SetValue(ds3.Tables[0].Rows[j]["dyedqty"]);
                        sht.Range("J" + row).SetValue(ds3.Tables[0].Rows[j]["undyedqty"]);
                        sht.Range("K" + row).SetValue(ds3.Tables[0].Rows[j]["Lossqty"]);
                        sht.Range("L" + row).SetValue(ds3.Tables[0].Rows[j]["Retqty"]);

                        if (LotNo == ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno == ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = amt + Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        }
                        if (LotNo == ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = 0;
                            amt = Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        }
                        if (LotNo != ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = 0;
                            amt = Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                            tagno = ds3.Tables[0].Rows[j]["TagNo"].ToString();
                            LotNo = ds3.Tables[0].Rows[j]["LotNo"].ToString();
                        }


                        // amt =amt+ Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        //sht.Range("M" + row).SetValue(Convert.ToDecimal(ds3.Tables[0].Rows[j]["OQty"]) - (Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"])));
                        sht.Range("M" + row).SetValue(Convert.ToDecimal(ds3.Tables[0].Rows[j]["OQty"]) - (amt));
                        sht.Range("N" + row).SetValue(ds3.Tables[0].Rows[j]["GateInNo"]);

                        // {Table.OQTY}-({Table.Dyedqty}+{Table.Undyedqty}+{Table.Lossqty}-{Table.Retqty})
                        row = row + 1;

                    }
                    //sht.Range("G" + row).FormulaA1 = "=SUM(G" + (row-k) + ":G" + (row - 1) + ")";
                    sht.Range("H" + row).FormulaA1 = "=SUM(H" + (row - k) + ":H" + (row - 1) + ")";
                    sht.Range("I" + row).FormulaA1 = "=SUM(I" + (row - k) + ":I" + (row - 1) + ")";
                    sht.Range("J" + row).FormulaA1 = "=SUM(J" + (row - k) + ":J" + (row - 1) + ")";
                    sht.Range("K" + row).FormulaA1 = "=SUM(K" + (row - k) + ":K" + (row - 1) + ")";
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + (row - k) + ":L" + (row - 1) + ")";
                    sht.Range("M" + row).FormulaA1 = "=SUM(M" + (row - k) + ":M" + (row - 1) + ")";
                    sht.Range("H" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("H" + row + ":N" + row).Style.Font.FontSize = 9;
                    sht.Range("H" + row + ":N" + row).Style.Font.Bold = true;
                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:N" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ShadeWiseDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void Dyerledger()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            param[3] = new SqlParameter("@indentid", DDIndentNo.SelectedValue);
            param[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? 1 : 0);
            param[5] = new SqlParameter("@fromdate", TxtFromDate.Text);
            param[6] = new SqlParameter("@Todate", TxtToDate.Text);
            param[7] = new SqlParameter("@Forsample", chksample.Checked == true ? 1 : 0);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYERLEDGER", param);
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

                sht.Range("A1:O1").Merge();
                sht.Range("A1").SetValue(DDCompany.SelectedItem.Text);
                sht.Range("A2:O2").Merge();
                sht.Range("A2").SetValue("DYER LEDGER" + (ChkForDate.Checked == true ? "(" + TxtFromDate.Text + " TO : " + TxtToDate.Text + "" : ""));
                sht.Range("A1:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O2").Style.Font.SetBold();

                //Headers
                sht.Range("A3").Value = "Indent No.";
                sht.Range("B3").Value = "Indent Date";
                sht.Range("C3").Value = "Dyer Name";
                sht.Range("D3").Value = "Item Name";
                sht.Range("E3").Value = "Shade Name";
                sht.Range("F3").Value = "Iss Qty";
                sht.Range("G3").Value = "Rec Qty";
                sht.Range("H3").Value = "Undyed Qty";
                sht.Range("I3").Value = "Loss Qty";
                sht.Range("J3").Value = "Pend. Qty";
                sht.Range("K3").Value = "Rate";
                sht.Range("L3").Value = "Amount";
                sht.Range("M3").Value = "Debit Amt";
                sht.Range("N3").Value = "Total Amt";
                sht.Range("O3").Value = "Bill No";

                sht.Range("A3:O3").Style.Font.Bold = true;

                row = 4;

                DataTable dtdistinctindent = ds.Tables[0].DefaultView.ToTable(true, "Indentid", "Date", "IndentNo", "Empname", "DebitNote");
                DataView dvindetnNo = new DataView(dtdistinctindent);
                dvindetnNo.Sort = "EMpname,Date asc,IndentId,indentNo";
                DataTable dtdistinct = dvindetnNo.ToTable();


                int rowfrom = 0, rowto = 0;
                string Tamtrow = "", TDebitrow = "", TNetamtrow = "";
                string QualityName = "", Shadename = "";
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dvdetail = new DataView(ds.Tables[0]);
                    dvdetail.RowFilter = "Indentid=" + dr["indentid"] + " and Date='" + dr["date"] + "' and IndentNo='" + dr["indentNo"] + "' and empname='" + dr["empname"] + "'";
                    dvdetail.Sort = "QualityName,ShadecolorName";
                    DataTable dt = dvdetail.ToTable();



                    rowfrom = row;
                    QualityName = ""; Shadename = "";
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        sht.Range("A" + row).SetValue(dr1["IndentNo"]);
                        sht.Range("B" + row).SetValue(dr1["Date"]);
                        sht.Range("C" + row).SetValue(dr1["empname"]);
                        sht.Range("D" + row).SetValue(dr1["qualityName"]);
                        sht.Range("E" + row).SetValue(dr1["shadecolorname"]);
                        sht.Range("F" + row).SetValue(dr1["OQTY"]);
                        sht.Range("G" + row).SetValue(dr1["DYEDQTY"]);
                        sht.Range("H" + row).SetValue(dr1["UNDYEDQTY"]);
                        sht.Range("I" + row).SetValue(dr1["Lossqty"]);

                        if (QualityName == "" && Shadename == "")
                        {
                            sht.Range("J" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        else if (QualityName == dr1["QualityName"].ToString() && Shadename == dr1["shadecolorname"].ToString())
                        {
                            sht.Range("J" + row).FormulaA1 = "=J" + (row - 1) + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        else
                        {
                            sht.Range("J" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        sht.Range("K" + row).SetValue(dr1["RATE"]);
                        sht.Range("L" + row).SetValue(dr1["AMount"]);
                        sht.Range("M" + row).SetValue(dr1["PenaltyDebitnote"]);
                        sht.Range("N" + row).FormulaA1 = "=L" + row + '-' + "M" + row;
                        sht.Range("O" + row).SetValue(dr1["BillNo"]);

                        QualityName = dr1["QualityName"].ToString();
                        Shadename = dr1["Shadecolorname"].ToString();

                        row = row + 1;
                    }

                    rowto = row - 1;
                    sht.Range("K" + row).SetValue("Total");
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + rowfrom + ":L" + rowto + ")";
                    sht.Range("M" + row).FormulaA1 = "SUM(M" + rowfrom + ":M" + rowto + ")" + '+' + dr["debitnote"];
                    sht.Range("N" + row).FormulaA1 = "=L" + row + '-' + "M" + row;
                    sht.Range("K" + row + ":N" + row).Style.Font.Bold = true;

                    Tamtrow = Tamtrow + "+" + "L" + row;
                    TDebitrow = TDebitrow + "+" + "M" + row;
                    TNetamtrow = TNetamtrow + "+" + "N" + row;

                    row = row + 1;
                }

                Tamtrow = Tamtrow.TrimStart('+');
                TDebitrow = TDebitrow.TrimStart('+');
                TNetamtrow = TNetamtrow.TrimStart('+');


                sht.Range("K" + row).SetValue("G. Total");
                sht.Range("L" + row).FormulaA1 = "=SUM(" + Tamtrow + ")";
                sht.Range("M" + row).FormulaA1 = "=SUM(" + TDebitrow + ")";
                sht.Range("N" + row).FormulaA1 = "=SUM(" + TNetamtrow + ")";

                sht.Range("K" + row + ":N" + row).Style.Font.Bold = true;

                using (var a = sht.Range("A3:O" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 15).AdjustToContents();
                string Fileextension = "xlsx";
                string name = "DyerLedger";
                //if (DDEmpName.SelectedIndex > 0)
                //{
                //    name = name + "-" + DDEmpName.SelectedItem.Text;
                //}
                string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdyerled", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    protected void Dyerledger_agni()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            param[3] = new SqlParameter("@indentid", DDIndentNo.SelectedValue);
            param[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? 1 : 0);
            param[5] = new SqlParameter("@fromdate", TxtFromDate.Text);
            param[6] = new SqlParameter("@Todate", TxtToDate.Text);
            param[7] = new SqlParameter("@Forsample", chksample.Checked == true ? 1 : 0);
            param[8] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"].ToString());

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYERLEDGER", param);
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

                sht.Range("A1:Q1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:Q1").Style.Font.FontSize = 13;
                sht.Range("A1:Q1").Style.Font.Bold = true;
                sht.Range("A1:Q1").Merge();
                sht.Range("A2:Q2").Value = ds.Tables[0].Rows[0]["COMPADDR1"].ToString() + "    " + "Print Date-" + DateTime.Now.ToShortDateString();
                sht.Range("A2:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:Q2").Style.Font.FontSize = 13;
                sht.Range("A2:Q2").Style.Font.Bold = true;
                sht.Range("A2:Q2").Merge();
                sht.Range("A3:Q3").Value = "GSTNo.-" + ds.Tables[0].Rows[0]["GSTNO"].ToString();
                sht.Range("A3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:Q3").Style.Font.FontSize = 12;
                sht.Range("A3:Q3").Style.Font.Bold = true;
                sht.Range("A3:Q3").Merge();

                sht.Range("A4:Q4").Merge();
                sht.Range("A4").SetValue("DYER LEDGER" + (ChkForDate.Checked == true ? "(" + TxtFromDate.Text + " TO : " + TxtToDate.Text + "" : ""));
                sht.Range("A4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:Q4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A4:Q4").Style.Font.SetBold();
                sht.Range("A4:Q4").Style.Font.FontSize = 12;

                //sht.Range("A1:O1").Merge();
                //sht.Range("A1").SetValue(DDCompany.SelectedItem.Text);
                //sht.Range("A2:O2").Merge();
                //sht.Range("A2").SetValue("DYER LEDGER" + (ChkForDate.Checked == true ? "(" + TxtFromDate.Text + " TO : " + TxtToDate.Text + "" : ""));
                //sht.Range("A1:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A1:O2").Style.Font.SetBold();

                //Headers
                sht.Range("A5").Value = "Indent No.";
                sht.Range("B5").Value = "Indent Date";
                sht.Range("C5").Value = "Dyer Name";
                sht.Range("D5").Value = "Item Name";
                sht.Range("E5").Value = "Shade Name";
                sht.Range("F5").Value = "Issued Qty";
                sht.Range("G5").Value = "Rcvd. Qty";
                sht.Range("H5").Value = "Undyed Return Qty";
                sht.Range("I5").Value = "Loss Qty";
                sht.Range("J5").Value = "Pend. Qty";
                sht.Range("K5").Value = "Rate";
                sht.Range("L5").Value = "Amount";
                sht.Range("M5").Value = "Deduction Amt";
                sht.Range("N5").Value = "Debit Amt";
                sht.Range("O5").Value = "TDS";
                sht.Range("P5").Value = "Total Amt";
                sht.Range("Q5").Value = "Bill No";

                sht.Range("A5:Q5").Style.Font.Bold = true;

                row = 6;

                DataTable dtdistinctindent = ds.Tables[0].DefaultView.ToTable(true, "Indentid", "Date", "IndentNo", "Empname", "DebitNote");
                DataView dvindetnNo = new DataView(dtdistinctindent);
                dvindetnNo.Sort = "EMpname,Date asc,IndentId,indentNo";
                DataTable dtdistinct = dvindetnNo.ToTable();


                int rowfrom = 0, rowto = 0;
                string Tamtrow = "", TDebitrow = "", TNetamtrow = "";
                string QualityName = "", Shadename = "";
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dvdetail = new DataView(ds.Tables[0]);
                    dvdetail.RowFilter = "Indentid=" + dr["indentid"] + " and Date='" + dr["date"] + "' and IndentNo='" + dr["indentNo"] + "' and empname='" + dr["empname"] + "'";
                    dvdetail.Sort = "QualityName,ShadecolorName";
                    DataTable dt = dvdetail.ToTable();



                    rowfrom = row;
                    QualityName = ""; Shadename = "";
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        sht.Range("A" + row).SetValue(dr1["IndentNo"]);
                        sht.Range("B" + row).SetValue(dr1["Date"]);
                        sht.Range("C" + row).SetValue(dr1["empname"]);
                        sht.Range("D" + row).SetValue(dr1["qualityName"]);
                        sht.Range("E" + row).SetValue(dr1["shadecolorname"]);
                        sht.Range("F" + row).SetValue(dr1["OQTY"]);
                        sht.Range("G" + row).SetValue(dr1["DYEDQTY"]);
                        sht.Range("H" + row).SetValue(dr1["UNDYEDQTY"]);
                        sht.Range("I" + row).SetValue(dr1["Lossqty"]);

                        if (QualityName == "" && Shadename == "")
                        {
                            sht.Range("J" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        else if (QualityName == dr1["QualityName"].ToString() && Shadename == dr1["shadecolorname"].ToString())
                        {
                            sht.Range("J" + row).FormulaA1 = "=J" + (row - 1) + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        else
                        {
                            sht.Range("J" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "H" + row + '+' + "I" + row + ")";
                        }
                        sht.Range("K" + row).SetValue(dr1["RATE"]);
                        sht.Range("L" + row).SetValue(dr1["AMount"]);
                        sht.Range("M" + row).SetValue(dr1["DeductionAmt"]);
                        sht.Range("N" + row).SetValue(dr1["DebitAmt"]);

                        float TDSAMOUNT = 0;
                        TDSAMOUNT = (Convert.ToInt32(dr1["AMount"]) * Convert.ToInt16(dr1["tds"])) / 100;

                        sht.Range("O" + row).SetValue(TDSAMOUNT);
                        sht.Range("P" + row).FormulaA1 = "=L" + row + '-' + "M" + row + '-' + "N" + row + '-' + "O"+row;
                        sht.Range("O" + row).SetValue(dr1["BillNo"]);

                        QualityName = dr1["QualityName"].ToString();
                        Shadename = dr1["Shadecolorname"].ToString();

                        row = row + 1;
                    }

                    rowto = row - 1;
                    sht.Range("K" + row).SetValue("Total");
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + rowfrom + ":L" + rowto + ")";
                   // sht.Range("O" + row).FormulaA1 = "SUM(O" + rowfrom + ":O" + rowto + ")" + '+' + dr["debitnote"];
                    sht.Range("P" + row).FormulaA1 = "=L" + row + '-' + "M" + row + '-' + "N" + row + '-' + "O" + row;
                    sht.Range("K" + row + ":P" + row).Style.Font.Bold = true;

                    Tamtrow = Tamtrow + "+" + "L" + row;
                   // TDebitrow = TDebitrow + "+" + "M" + row;
                    TNetamtrow = TNetamtrow + "+" + "P" + row;

                    row = row + 1;
                }

                Tamtrow = Tamtrow.TrimStart('+');
                TDebitrow = TDebitrow.TrimStart('+');
                TNetamtrow = TNetamtrow.TrimStart('+');


                sht.Range("K" + row).SetValue("G. Total");
                sht.Range("L" + row).FormulaA1 = "=SUM(" + Tamtrow + ")";
               // sht.Range("M" + row).FormulaA1 = "=SUM(" + TDebitrow + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(" + TNetamtrow + ")";

                sht.Range("K" + row + ":P" + row).Style.Font.Bold = true;

                using (var a = sht.Range("A3:Q" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 15).AdjustToContents();
                string Fileextension = "xlsx";
                string name = "DyerLedger";
                //if (DDEmpName.SelectedIndex > 0)
                //{
                //    name = name + "-" + DDEmpName.SelectedItem.Text;
                //}
                string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdyerled", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    #region
    protected void IndentIssueDetailExportExcel()
    {
        try
        {
            // string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ORDERWISEINDENTDETAIL Where Companyid=" + DDCompany.SelectedValue;
            string str = "", strsample = ""; ;
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " And CompanyId=" + DDCompany.SelectedValue;
                //if (chksample.Checked == true)
                //{
                //    strsample = strsample + " and Companyid=" + DDCompany.SelectedValue;
                //}
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " And CustomerId=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " And OrderId=" + DDOrderNo.SelectedValue;
            }
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " And Processid=" + DDProcessName.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.Processid=" + DDProcessName.SelectedValue;
                //}
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And EmpId=" + DDEmpName.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.empid=" + DDEmpName.SelectedValue;
                //}
            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
                }
            }

            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and Item_id=" + ddItemName.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
                //}
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and Qualityid=" + DDQuality.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
                //}
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
                //}
            }
            if (ChkForDate.Checked == true)
            {
                str = str + "  And IndentDate>='" + TxtFromDate.Text + "' And IndentDate<='" + TxtToDate.Text + "'";
                //str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
                //}
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINDENTISSUEDETAILEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@ChkForSample", chksample.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    sht.Column("A").Width = 35.22;
                    sht.Column("B").Width = 20.22;
                    sht.Column("C").Width = 25.22;
                    sht.Column("D").Width = 25.33;
                    sht.Column("E").Width = 15.00;
                    sht.Column("F").Width = 18.00;
                    sht.Column("G").Width = 18.00;
                    sht.Column("H").Width = 18.00;
                    sht.Column("I").Width = 18.00;
                    sht.Column("J").Width = 18.00;
                    sht.Column("K").Width = 18.00;
                    sht.Column("L").Width = 9.00;
                    sht.Column("M").Width = 9.00;
                    sht.Column("N").Width = 9.00;
                    sht.Column("O").Width = 9.00;
                    sht.Column("P").Width = 9.00;

                    sht.Range("A1:P1").Merge();
                    sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                    sht.Range("A2:P2").Merge();
                    sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
                    sht.Range("A3:P3").Merge();
                    sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"];
                    sht.Range("A4:P4").Merge();
                    sht.Range("A4").Value = "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
                    sht.Row(4).Height = 30;
                    sht.Range("A1:P1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:P2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A3:P3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:P4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:P4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:P1").Style.Alignment.SetWrapText();
                    sht.Range("A2:P2").Style.Alignment.SetWrapText();
                    sht.Range("A3:P3").Style.Alignment.SetWrapText();
                    sht.Range("A4:P4").Style.Alignment.SetWrapText();
                    sht.Range("A1:P4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:P4").Style.Font.FontSize = 10;
                    sht.Range("A1:P4").Style.Font.Bold = true;

                    //*******Header
                    sht.Range("A5").Value = "DyerName";
                    sht.Range("B5").Value = "Indent Date";
                    sht.Range("C5").Value = "Delay Days";
                    sht.Range("D5").Value = "Indent No";
                    sht.Range("E5").Value = "Buyer Code";
                    sht.Range("F5").Value = "Customer OrderNO";
                    sht.Range("G5").Value = "Category";
                    sht.Range("H5").Value = "Design";
                    sht.Range("I5").Value = "Item Name";
                    sht.Range("J5").Value = "Sub ItemName";
                    sht.Range("K5").Value = "Color Name";
                    sht.Range("L5").Value = "Order Qty";
                    sht.Range("M5").Value = "Rec Qty";
                    sht.Range("N5").Value = "Loss Qty";
                    sht.Range("O5").Value = "Return Qty";
                    sht.Range("P5").Value = "Pending Qty";


                    sht.Range("A5:P5").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A5:P5").Style.Font.FontSize = 10;
                    sht.Range("A5:P5").Style.Font.Bold = true;
                    //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A5:P5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A5:P5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A5:P5").Style.Alignment.SetWrapText();

                    row = 6;
                    int rowfrom = 6;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                        //sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IndentDate"]);
                        //sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["NoOfDelayDays"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IndentNo"]);
                        //sht.Range("D" + row).Style.Alignment.SetWrapText();
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["CATEGORY_NAME"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                        sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["OQTY"]);
                        sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["DYEDQTY"]);
                        sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["LOSSQTY"]);
                        sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["RETQTY"]);
                        decimal PendingQty = 0;
                        PendingQty = (Convert.ToDecimal(ds.Tables[0].Rows[i]["OQTY"]) - (Convert.ToDecimal(ds.Tables[0].Rows[i]["DYEDQTY"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["LOSSQTY"]))) + Convert.ToDecimal(ds.Tables[0].Rows[i]["RETQTY"]);
                        sht.Range("P" + row).SetValue(PendingQty);

                        row = row + 1;
                    }
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + (rowfrom) + ":L" + (row - 1) + ")";
                    sht.Range("M" + row).FormulaA1 = "=SUM(M" + (rowfrom) + ":M" + (row - 1) + ")";
                    sht.Range("N" + row).FormulaA1 = "=SUM(N" + (rowfrom) + ":N" + (row - 1) + ")";
                    sht.Range("O" + row).FormulaA1 = "=SUM(O" + (rowfrom) + ":O" + (row - 1) + ")";
                    sht.Range("P" + row).FormulaA1 = "=SUM(P" + (rowfrom) + ":P" + (row - 1) + ")";
                    sht.Range("L" + row + ":P" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("L" + row + ":P" + row).Style.Font.FontSize = 9;
                    sht.Range("L" + row + ":P" + row).Style.Font.Bold = true;
                    row = row + 1;


                    //*************
                    using (var a = sht.Range("A1:P" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                }
                else
                {
                    sht.Column("A").Width = 35.22;
                    sht.Column("B").Width = 20.22;
                    sht.Column("C").Width = 25.22;
                    sht.Column("D").Width = 25.33;
                    sht.Column("E").Width = 15.00;
                    sht.Column("F").Width = 18.00;
                    sht.Column("G").Width = 18.00;
                    sht.Column("H").Width = 18.00;
                    sht.Column("I").Width = 18.00;
                    sht.Column("J").Width = 9.00;
                    sht.Column("K").Width = 9.00;
                    sht.Column("L").Width = 9.00;
                    sht.Column("M").Width = 9.00;

                    sht.Range("A1:N1").Merge();
                    sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                    sht.Range("A2:N2").Merge();
                    sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
                    sht.Range("A3:N3").Merge();
                    sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"];
                    sht.Range("A4:N4").Merge();
                    sht.Range("A4").Value = "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
                    sht.Row(4).Height = 30;
                    sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A1:N1").Style.Alignment.SetWrapText();
                    sht.Range("A2:N2").Style.Alignment.SetWrapText();
                    sht.Range("A3:N3").Style.Alignment.SetWrapText();
                    sht.Range("A4:N4").Style.Alignment.SetWrapText();
                    sht.Range("A1:N4").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A1:N4").Style.Font.FontSize = 10;
                    sht.Range("A1:N4").Style.Font.Bold = true;

                    //*******Header
                    sht.Range("A5").Value = "DyerName";
                    sht.Range("B5").Value = "Indent Date";
                    sht.Range("C5").Value = "Delay Days";
                    sht.Range("D5").Value = "Indent No";
                    sht.Range("E5").Value = "Buyer Code";
                    sht.Range("F5").Value = "Customer OrderNO";
                    sht.Range("G5").Value = "Item Name";
                    sht.Range("H5").Value = "Sub ItemName";
                    sht.Range("I5").Value = "Color Name";
                    sht.Range("J5").Value = "Order Qty";
                    sht.Range("K5").Value = "Rec Qty";
                    sht.Range("L5").Value = "Loss Qty";
                    sht.Range("M5").Value = "Return Qty";
                    sht.Range("N5").Value = "Pending Qty";


                    sht.Range("A5:N5").Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A5:N5").Style.Font.FontSize = 10;
                    sht.Range("A5:N5").Style.Font.Bold = true;
                    //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                    sht.Range("A5:N5").Style.Alignment.SetWrapText();

                    row = 6;
                    int rowfrom = 6;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                        sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                        //sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;

                        sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"]);
                        sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IndentDate"]);
                        //sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["NoOfDelayDays"]);
                        sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IndentNo"]);
                        //sht.Range("D" + row).Style.Alignment.SetWrapText();
                        sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERCODE"]);
                        sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                        sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                        sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                        sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["SHADECOLORNAME"]);
                        sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["OQTY"]);
                        sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["DYEDQTY"]);
                        sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["LOSSQTY"]);
                        sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["RETQTY"]);
                        decimal PendingQty = 0;
                        PendingQty = (Convert.ToDecimal(ds.Tables[0].Rows[i]["OQTY"]) - (Convert.ToDecimal(ds.Tables[0].Rows[i]["DYEDQTY"]) + Convert.ToDecimal(ds.Tables[0].Rows[i]["LOSSQTY"]))) + Convert.ToDecimal(ds.Tables[0].Rows[i]["RETQTY"]);
                        sht.Range("N" + row).SetValue(PendingQty);

                        row = row + 1;
                    }
                    sht.Range("J" + row).FormulaA1 = "=SUM(J" + (rowfrom) + ":J" + (row - 1) + ")";
                    sht.Range("K" + row).FormulaA1 = "=SUM(K" + (rowfrom) + ":K" + (row - 1) + ")";
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + (rowfrom) + ":L" + (row - 1) + ")";
                    sht.Range("M" + row).FormulaA1 = "=SUM(M" + (rowfrom) + ":M" + (row - 1) + ")";
                    sht.Range("N" + row).FormulaA1 = "=SUM(N" + (rowfrom) + ":N" + (row - 1) + ")";
                    sht.Range("J" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("J" + row + ":N" + row).Style.Font.FontSize = 9;
                    sht.Range("J" + row + ":N" + row).Style.Font.Bold = true;
                    row = row + 1;


                    //*************
                    using (var a = sht.Range("A1:N" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                }
                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("OrderWiseDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    protected void IndentRecPendingTillDateExcelReport()
    {
        #region Where Condition
        string Where = "";

        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            Where = Where + " And CI.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Where = Where + " And OM.orderid=" + DDOrderNo.SelectedValue;
        }

        #endregion
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINDENTWISEPENDINGRECQTYEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@MasterCompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 20.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 20.22;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 20.22;
                sht.Column("F").Width = 20.22;
                sht.Column("G").Width = 20.22;
                sht.Column("H").Width = 20.22;
                sht.Column("I").Width = 20.22;


                //*******Header
                sht.Range("A2").Value = "Customer Code";
                sht.Range("B2").Value = "Customer OrderNo";

                sht.Range("C2").Value = "Indent No";
                sht.Range("D2").Value = "Issue Date";
                sht.Range("E2").Value = "Required Date";
                sht.Range("F2").Value = "Dyer Name";
                sht.Range("G2").Value = "Item Name";
                sht.Range("H2").Value = "Quality Name";

                sht.Range("I2").Value = "Iss Qty";
                sht.Range("J2").Value = "Rec Qty";
                sht.Range("K2").Value = "Pending Qty";


                sht.Range("A2:K2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:K2").Style.Font.FontSize = 10;
                sht.Range("A2:K2").Style.Font.Bold = true;

                sht.Range("A2:K2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:K2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:K2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["CustomerCode"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    //sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["ReqDate"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    sht.Range("F" + row).Style.Alignment.SetWrapText();

                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["ITEM_NAME"]);
                    sht.Range("G" + row).Style.Alignment.SetWrapText();
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    sht.Range("H" + row).Style.Alignment.SetWrapText();

                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["IssueQty"]);
                    decimal RecQty = 0;
                    RecQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["UndyedQty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["LossQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RetQty"]);
                    sht.Range("J" + row).SetValue(RecQty);
                    sht.Range("K" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(RecQty));

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:K" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentRecPendingTillDate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void IndentIssueWithPPConsumptionExcelReport()
    {
        #region Where Condition
        string Where = "";
        string WhereOrderDate = "";

        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";

        if (DDProcessProgram.SelectedIndex > 0)
        {
            Where = Where + " and TPI.PPID=" + DDProcessProgram.SelectedValue;
        }
        if (ChkForDate.Checked == true)
        {
            WhereOrderDate = WhereOrderDate + "  And OM.OrderDate>='" + TxtFromDate.Text + "' And OM.OrderDate<='" + TxtToDate.Text + "' ";
        }

        #endregion
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINDENTISSUEQTY_WITH_PPCONSUMPTION_EXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@MasterCompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@WhereOrderDate", WhereOrderDate);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 40.22;
                sht.Column("B").Width = 40.22;
                sht.Column("C").Width = 20.22;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 20.22;
                sht.Column("F").Width = 20.22;
                sht.Column("G").Width = 20.22;
                sht.Column("H").Width = 20.22;
                sht.Column("I").Width = 20.22;
                sht.Column("J").Width = 20.22;

                //*******Header
                sht.Range("A2").Value = "CustomerCode ";
                sht.Range("B2").Value = "Order No";

                sht.Range("C2").Value = "PPNO";
                sht.Range("D2").Value = "Item Name";

                sht.Range("E2").Value = "Quality";
                sht.Range("F2").Value = "PPQty";
                sht.Range("G2").Value = "Indent Qty";
                sht.Range("H2").Value = "PP Bal Qty";
                sht.Range("I2").Value = "Indent Iss Qty";
                sht.Range("J2").Value = "Indent Bal Qty";

                sht.Range("A2:J2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:J2").Style.Font.FontSize = 10;
                sht.Range("A2:J2").Style.Font.Bold = true;

                sht.Range("A2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:J2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:J2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":J" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":J" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["CustomerCode"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["CHALLANNO"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["ITEM_NAME"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["ConsumptionQty"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["IndentQty"]);
                    sht.Range("H" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["ConsumptionQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["IndentQty"]));
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["IndentIssueQty"]);
                    sht.Range("I" + row).Style.Alignment.SetWrapText();
                    sht.Range("J" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IndentQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["IndentIssueQty"]));

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:J" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentIssueWithPPConsumption_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void IndentRecDetailExcelReport(DataSet ds)
    {

        try
        {

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 14.56;
                sht.Column("B").Width = 16.11;
                sht.Column("C").Width = 13.00;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 10.89;
                sht.Column("F").Width = 14.45;
                sht.Column("G").Width = 14.45;
                sht.Column("H").Width = 13.56;
                sht.Column("I").Width = 36.44;
                sht.Column("J").Width = 14.44;
                sht.Column("K").Width = 11.22;
                sht.Column("L").Width = 10.22;
                sht.Column("M").Width = 10.22;
                sht.Column("N").Width = 10.22;
                sht.Column("O").Width = 10.22;
                sht.Column("P").Width = 12.22;
                sht.Column("Q").Width = 12.22;
                sht.Column("R").Width = 16.22;

                //*******Header
                sht.Range("A2").Value = "CustomerCode ";
                sht.Range("B2").Value = "Order No";

                sht.Range("C2").Value = "Indent No";
                sht.Range("D2").Value = "Party Name";

                sht.Range("E2").Value = "Rec No";
                sht.Range("F2").Value = "Iss ChallanNo";
                sht.Range("G2").Value = "Rec ChallanNo";
                sht.Range("H2").Value = "Rec Date";
                sht.Range("I2").Value = "Item Description";
                sht.Range("J2").Value = "Lot No";
                sht.Range("K2").Value = "Tag No";
                sht.Range("L2").Value = "Rec Qty";
                sht.Range("M2").Value = "Loss Qty";
                sht.Range("N2").Value = "Ret Qty";
                sht.Range("O2").Value = "Undyed Qty";
                sht.Range("P2").Value = "Moisture(%)";
                sht.Range("Q2").Value = "Checked By";
                sht.Range("R2").Value = "Remarks";

                sht.Range("A2:R2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:R2").Style.Font.FontSize = 10;
                sht.Range("A2:R2").Style.Font.Bold = true;

                sht.Range("A2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:R2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:R2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":R" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":R" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["CustomerCode"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Empname"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["PRMid"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["IssueChallanNo"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["RecChallanNo"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["Description"]);
                    sht.Range("I" + row).Style.Alignment.SetWrapText();
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[j]["LotNo"]);
                    sht.Range("J" + row).Style.Alignment.SetWrapText();
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[j]["TagNo"]);
                    sht.Range("K" + row).Style.Alignment.SetWrapText();
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[j]["RECQTY"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[j]["LossQty"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[j]["RetQty"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[j]["UNDYEDQTY"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[j]["Moisture"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[j]["CheckedBy"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[j]["IndentRecRemarks"]);
                    sht.Range("R" + row).Style.Alignment.SetWrapText();

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:R" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentReceiveDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void IndentWisePendingDetailExcelReport()
    {
        #region Where Condition
        string Where = "";
        string WhereSample = "";

        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            Where = Where + " And CI.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Where = Where + " And OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            Where = Where + "  and IM.Processid=" + DDProcessName.SelectedValue;
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and IM.Processid=" + DDProcessName.SelectedValue;
            }
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            Where = Where + "  and IM.Partyid=" + DDEmpName.SelectedValue;
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and SM.Empid=" + DDEmpName.SelectedValue;
            }
        }
        if (chksample.Checked == true)
        {
            if (DDIndentNo.SelectedIndex > 0)
            {
                WhereSample = WhereSample + " and SM.Id=" + DDIndentNo.SelectedValue;
            }
        }
        else
        {
            if (DDIndentNo.SelectedIndex > 0)
            {
                Where = Where + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
            }
        }

        if (ChkForDate.Checked == true)
        {
            Where = Where + "  And IM.Date>='" + TxtFromDate.Text + "' And IM.Date<='" + TxtToDate.Text + "'";
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
            }
        }

        #endregion
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINDENTWISEPENDINGDETAILEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@MasterCompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@WhereSample", WhereSample);
            cmd.Parameters.AddWithValue("@ChkForSample", chksample.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 20.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 20.22;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 20.22;
                sht.Column("F").Width = 20.22;
                sht.Column("G").Width = 20.22;
                sht.Column("H").Width = 20.22;
                sht.Column("I").Width = 20.22;


                //*******Header
                sht.Range("A2").Value = "Dyer Name";
                sht.Range("B2").Value = "Indent No";

                sht.Range("C2").Value = "Pending Qty";
                //sht.Range("D2").Value = "Issue Date";
                //sht.Range("E2").Value = "Required Date";
                //sht.Range("F2").Value = "Dyer Name";
                //sht.Range("G2").Value = "Item Name";
                //sht.Range("H2").Value = "Quality Name";

                //sht.Range("I2").Value = "Iss Qty";
                //sht.Range("J2").Value = "Rec Qty";
                //sht.Range("K2").Value = "Pending Qty";


                sht.Range("A2:C2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:C2").Style.Font.FontSize = 10;
                sht.Range("A2:C2").Style.Font.Bold = true;

                sht.Range("A2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:C2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:C2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    decimal RecQty = 0;
                    RecQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["UndyedQty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["LossQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RetQty"]);
                    sht.Range("C" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(RecQty));



                    //sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    //sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    ////sht.Range("B" + row).Style.Alignment.SetWrapText();
                    //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["ReqDate"]);
                    //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    //sht.Range("F" + row).Style.Alignment.SetWrapText();

                    //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["ITEM_NAME"]);
                    //sht.Range("G" + row).Style.Alignment.SetWrapText();
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    //sht.Range("H" + row).Style.Alignment.SetWrapText();

                    //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["IssueQty"]);
                    //decimal RecQty = 0;
                    //RecQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["UndyedQty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["LossQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RetQty"]);
                    //sht.Range("J" + row).SetValue(RecQty);
                    //sht.Range("K" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(RecQty));

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:C" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentPendingDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void IndentWisePendingDetailExcelReportAntique()
    {
        #region Where Condition
        string Where = "";
        string WhereSample = "";

        //string filterby = "From : " + TxtFRDate.Text + "  To : " + TxtTODate.Text;
        //Where = Where + " and Prm.Receivedate>='" + TxtFRDate.Text + "' and PRM.Receivedate<='" + TxtTODate.Text + "'";
        if (DDCustCode.SelectedIndex > 0)
        {
            Where = Where + " And CI.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            Where = Where + " And OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (DDProcessName.SelectedIndex > 0)
        {
            Where = Where + "  and IM.Processid=" + DDProcessName.SelectedValue;
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and IM.Processid=" + DDProcessName.SelectedValue;
            }
        }
        if (DDEmpName.SelectedIndex > 0)
        {
            Where = Where + "  and IM.Partyid=" + DDEmpName.SelectedValue;
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and SM.Empid=" + DDEmpName.SelectedValue;
            }
        }
        if (chksample.Checked == true)
        {
            if (DDIndentNo.SelectedIndex > 0)
            {
                WhereSample = WhereSample + " and SM.Id=" + DDIndentNo.SelectedValue;
            }
        }
        else
        {
            if (DDIndentNo.SelectedIndex > 0)
            {
                Where = Where + "  And IM.IndentId=" + DDIndentNo.SelectedValue;
            }
        }

        if (ChkForDate.Checked == true)
        {
            Where = Where + "  And IM.Date>='" + TxtFromDate.Text + "' And IM.Date<='" + TxtToDate.Text + "'";
            if (chksample.Checked == true)
            {
                WhereSample = WhereSample + " and SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
            }
        }

        #endregion
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETINDENTWISEPENDINGDETAILEXCELREPORT_ANTIQUE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@MasterCompanyid", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);
            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@WhereSample", WhereSample);
            cmd.Parameters.AddWithValue("@ChkForSample", chksample.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 20.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 20.22;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 20.22;
                sht.Column("F").Width = 20.22;
                sht.Column("G").Width = 20.22;
                sht.Column("H").Width = 20.22;
                sht.Column("I").Width = 20.22;


                //*******Header
                sht.Range("A2").Value = "Dyer Name";
                sht.Range("B2").Value = "Buyer Code";
                sht.Range("C2").Value = "Order No";
                sht.Range("D2").Value = "Indent No";
                sht.Range("E2").Value = "Indent Date";
                sht.Range("F2").Value = "Material Name";
                sht.Range("G2").Value = "ShadeColor Name";
                sht.Range("H2").Value = "Pending Qty";

                //sht.Range("D2").Value = "Issue Date";
                //sht.Range("E2").Value = "Required Date";
                //sht.Range("F2").Value = "Dyer Name";
                //sht.Range("G2").Value = "Item Name";
                //sht.Range("H2").Value = "Quality Name";

                //sht.Range("I2").Value = "Iss Qty";
                //sht.Range("J2").Value = "Rec Qty";
                //sht.Range("K2").Value = "Pending Qty";


                sht.Range("A2:H2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:H2").Style.Font.FontSize = 10;
                sht.Range("A2:H2").Style.Font.Bold = true;

                sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:H2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":I" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":I" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["CustomerCode"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    sht.Range("E" + row).Style.Alignment.SetWrapText();
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["QUALITYNAME"]);
                    sht.Range("F" + row).Style.Alignment.SetWrapText();
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["SHADECOLORNAME"]);
                    sht.Range("G" + row).Style.Alignment.SetWrapText();

                    decimal RecQty = 0;
                    RecQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["UndyedQty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["LossQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RetQty"]);
                    sht.Range("H" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(RecQty));



                    //sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    //sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    ////sht.Range("B" + row).Style.Alignment.SetWrapText();
                    //sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["ReqDate"]);
                    //sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
                    //sht.Range("F" + row).Style.Alignment.SetWrapText();

                    //sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["ITEM_NAME"]);
                    //sht.Range("G" + row).Style.Alignment.SetWrapText();
                    //sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["QualityName"]);
                    //sht.Range("H" + row).Style.Alignment.SetWrapText();

                    //sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["IssueQty"]);
                    //decimal RecQty = 0;
                    //RecQty = Convert.ToDecimal(ds.Tables[0].Rows[j]["Recqty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["UndyedQty"]) + Convert.ToDecimal(ds.Tables[0].Rows[j]["LossQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RetQty"]);
                    //sht.Range("J" + row).SetValue(RecQty);
                    //sht.Range("K" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(RecQty));

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:H" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentPendingDetailAntique_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void DyerledgerAntique()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
            param[2] = new SqlParameter("@Empid", DDEmpName.SelectedValue);
            param[3] = new SqlParameter("@indentid", DDIndentNo.SelectedValue);
            param[4] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? 1 : 0);
            param[5] = new SqlParameter("@fromdate", TxtFromDate.Text);
            param[6] = new SqlParameter("@Todate", TxtToDate.Text);
            param[7] = new SqlParameter("@Forsample", chksample.Checked == true ? 1 : 0);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDYERLEDGER_ANTIQUE", param);
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

                sht.Range("A1:O1").Merge();
                sht.Range("A1").SetValue(DDCompany.SelectedItem.Text);
                sht.Range("A2:O2").Merge();
                sht.Range("A2").SetValue("DYER LEDGER" + (ChkForDate.Checked == true ? "(" + TxtFromDate.Text + " TO : " + TxtToDate.Text + "" : ""));
                sht.Range("A1:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:O2").Style.Font.SetBold();

                ////Headers
                //sht.Range("A3").Value = "Indent No.";
                //sht.Range("B3").Value = "Indent Date";
                //sht.Range("C3").Value = "Dyer Name";
                //sht.Range("D3").Value = "Item Name";
                //sht.Range("E3").Value = "Shade Name";
                //sht.Range("F3").Value = "Iss Qty";
                //sht.Range("G3").Value = "Rec Qty";
                //sht.Range("H3").Value = "Undyed Qty";
                //sht.Range("I3").Value = "Loss Qty";
                //sht.Range("J3").Value = "Pend. Qty";
                //sht.Range("K3").Value = "Rate";
                //sht.Range("L3").Value = "Amount";
                //sht.Range("M3").Value = "Debit Amt";
                //sht.Range("N3").Value = "Total Amt";
                //sht.Range("O3").Value = "Bill No";

                //Headers
                sht.Range("A3").Value = "Indent No.";
                sht.Range("B3").Value = "Indent Date";
                sht.Range("C3").Value = "OrderNo";
                sht.Range("D3").Value = "Item Name";
                sht.Range("E3").Value = "Shade Name";
                sht.Range("F3").Value = "Iss Qty";
                sht.Range("G3").Value = "Rec Qty";
                sht.Range("H3").Value = "Rec Date";
                sht.Range("I3").Value = "TotalNoOfDays";

                sht.Range("J3").Value = "Undyed Qty";
                sht.Range("K3").Value = "Loss Qty";
                sht.Range("L3").Value = "Pend. Qty";
                sht.Range("M3").Value = "Rate";
                sht.Range("N3").Value = "Amount";
                sht.Range("O3").Value = "Debit Amt";
                sht.Range("P3").Value = "Total Amt";
                sht.Range("Q3").Value = "Bill No";


                sht.Range("A3:Q3").Style.Font.Bold = true;

                row = 4;

                DataTable dtdistinctindent = ds.Tables[0].DefaultView.ToTable(true, "Indentid", "Date", "IndentNo", "Empname", "DebitNote");
                DataView dvindetnNo = new DataView(dtdistinctindent);
                dvindetnNo.Sort = "EMpname,Date asc,IndentId,indentNo";
                DataTable dtdistinct = dvindetnNo.ToTable();


                int rowfrom = 0, rowto = 0;
                string Tamtrow = "", TDebitrow = "", TNetamtrow = "", TIssueQtyRow = "0", TRecQtyRow = "0", TUndyedQtyRow = "0", TLossQtyRow = "0";
                string QualityName = "", Shadename = "";
                foreach (DataRow dr in dtdistinct.Rows)
                {
                    DataView dvdetail = new DataView(ds.Tables[0]);
                    dvdetail.RowFilter = "Indentid=" + dr["indentid"] + " and Date='" + dr["date"] + "' and IndentNo='" + dr["indentNo"] + "' and empname='" + dr["empname"] + "'";
                    dvdetail.Sort = "QualityName,ShadecolorName";
                    DataTable dt = dvdetail.ToTable();



                    rowfrom = row;
                    QualityName = ""; Shadename = "";
                    foreach (DataRow dr1 in dt.Rows)
                    {

                        DateTime d1 = Convert.ToDateTime(dr1["Date"]);
                        DateTime d2;

                        if (dr1["MaxRecDate"].ToString() == null || dr1["MaxRecDate"].ToString() == "")
                        {
                            d2 = Convert.ToDateTime(dr1["Date"]);
                        }
                        else
                        {
                            d2 = Convert.ToDateTime(dr1["MaxRecDate"]);
                        }
                        var totalDays = (d2 - d1).Days;

                        sht.Range("A" + row).SetValue(dr1["IndentNo"]);
                        sht.Range("B" + row).SetValue(dr1["Date"]);
                        sht.Range("C" + row).SetValue(dr1["CustomerOrderNo"]);
                        sht.Range("D" + row).SetValue(dr1["qualityName"]);
                        sht.Range("E" + row).SetValue(dr1["shadecolorname"]);
                        sht.Range("F" + row).SetValue(dr1["OQTY"]);
                        sht.Range("G" + row).SetValue(dr1["DYEDQTY"]);
                        sht.Range("H" + row).SetValue(dr1["MaxRecDate"]);
                        sht.Range("I" + row).SetValue(totalDays);


                        sht.Range("J" + row).SetValue(dr1["UNDYEDQTY"]);
                        sht.Range("K" + row).SetValue(dr1["Lossqty"]);

                        if (QualityName == "" && Shadename == "")
                        {
                            sht.Range("L" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "J" + row + '+' + "K" + row + ")";
                        }
                        else if (QualityName == dr1["QualityName"].ToString() && Shadename == dr1["shadecolorname"].ToString())
                        {
                            sht.Range("L" + row).FormulaA1 = "=L" + (row - 1) + '-' + "(G" + row + '+' + "J" + row + '+' + "K" + row + ")";
                        }
                        else
                        {
                            sht.Range("L" + row).FormulaA1 = "=F" + row + '-' + "(G" + row + '+' + "J" + row + '+' + "K" + row + ")";
                        }
                        sht.Range("M" + row).SetValue(dr1["RATE"]);
                        sht.Range("N" + row).SetValue(dr1["AMount"]);
                        sht.Range("O" + row).SetValue(dr1["PenaltyDebitnote"]);
                        sht.Range("P" + row).FormulaA1 = "=N" + row + '-' + "O" + row;
                        sht.Range("Q" + row).SetValue(dr1["BillNo"]);

                        QualityName = dr1["QualityName"].ToString();
                        Shadename = dr1["Shadecolorname"].ToString();

                        row = row + 1;
                    }

                    rowto = row - 1;
                    sht.Range("E" + row).SetValue("Total");
                    sht.Range("F" + row).FormulaA1 = "=SUM(F" + rowfrom + ":F" + rowto + ")";
                    sht.Range("G" + row).FormulaA1 = "=SUM(G" + rowfrom + ":G" + rowto + ")";
                    sht.Range("J" + row).FormulaA1 = "=SUM(J" + rowfrom + ":J" + rowto + ")";
                    sht.Range("K" + row).FormulaA1 = "=SUM(K" + rowfrom + ":K" + rowto + ")";
                    sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + rowto + ")";
                    sht.Range("O" + row).FormulaA1 = "SUM(O" + rowfrom + ":O" + rowto + ")" + '+' + dr["debitnote"];
                    sht.Range("P" + row).FormulaA1 = "=N" + row + '-' + "O" + row;
                    sht.Range("E" + row + ":P" + row).Style.Font.Bold = true;

                    TIssueQtyRow = TIssueQtyRow + "+" + "F" + row;
                    TRecQtyRow = TRecQtyRow + "+" + "G" + row;
                    TUndyedQtyRow = TUndyedQtyRow + "+" + "J" + row;
                    TLossQtyRow = TLossQtyRow + "+" + "K" + row;
                    Tamtrow = Tamtrow + "+" + "N" + row;
                    TDebitrow = TDebitrow + "+" + "O" + row;
                    TNetamtrow = TNetamtrow + "+" + "P" + row;

                    //sht.Range("M" + row).SetValue("Total");
                    //sht.Range("N" + row).FormulaA1 = "=SUM(N" + rowfrom + ":N" + rowto + ")";
                    //sht.Range("O" + row).FormulaA1 = "SUM(O" + rowfrom + ":O" + rowto + ")" + '+' + dr["debitnote"];
                    //sht.Range("P" + row).FormulaA1 = "=N" + row + '-' + "O" + row;
                    //sht.Range("M" + row + ":P" + row).Style.Font.Bold = true;

                    //Tamtrow = Tamtrow + "+" + "N" + row;
                    //TDebitrow = TDebitrow + "+" + "O" + row;
                    //TNetamtrow = TNetamtrow + "+" + "P" + row;

                    row = row + 1;
                }

                TIssueQtyRow = TIssueQtyRow.TrimStart('+');
                TRecQtyRow = TRecQtyRow.TrimStart('+');
                TUndyedQtyRow = TUndyedQtyRow.TrimStart('+');
                TLossQtyRow = TLossQtyRow.TrimStart('+');
                Tamtrow = Tamtrow.TrimStart('+');
                TDebitrow = TDebitrow.TrimStart('+');
                TNetamtrow = TNetamtrow.TrimStart('+');

                sht.Range("E" + row).SetValue("G. Total");
                sht.Range("F" + row).FormulaA1 = "=SUM(" + TIssueQtyRow + ")";
                sht.Range("G" + row).FormulaA1 = "=SUM(" + TRecQtyRow + ")";
                sht.Range("J" + row).FormulaA1 = "=SUM(" + TUndyedQtyRow + ")";
                sht.Range("K" + row).FormulaA1 = "=SUM(" + TLossQtyRow + ")";
                sht.Range("N" + row).FormulaA1 = "=SUM(" + Tamtrow + ")";
                sht.Range("O" + row).FormulaA1 = "=SUM(" + TDebitrow + ")";
                sht.Range("P" + row).FormulaA1 = "=SUM(" + TNetamtrow + ")";

                sht.Range("E" + row + ":P" + row).Style.Font.Bold = true;

                //Tamtrow = Tamtrow.TrimStart('+');
                //TDebitrow = TDebitrow.TrimStart('+');
                //TNetamtrow = TNetamtrow.TrimStart('+');


                //sht.Range("M" + row).SetValue("G. Total");
                //sht.Range("N" + row).FormulaA1 = "=SUM(" + Tamtrow + ")";
                //sht.Range("O" + row).FormulaA1 = "=SUM(" + TDebitrow + ")";
                //sht.Range("P" + row).FormulaA1 = "=SUM(" + TNetamtrow + ")";

                //sht.Range("M" + row + ":P" + row).Style.Font.Bold = true;

                using (var a = sht.Range("A3:Q" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //*************
                sht.Columns(1, 20).AdjustToContents();
                string Fileextension = "xlsx";
                string name = "DyerLedger";
                //if (DDEmpName.SelectedIndex > 0)
                //{
                //    name = name + "-" + DDEmpName.SelectedItem.Text;
                //}
                string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdyerled", "alert('No records found for this combination.')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void ShadewiseDetailExportExcelWithoutGateInNo()
    {
        try
        {
            // string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ShadeWiseIndentDetail Where Companyid=" + DDCompany.SelectedValue;
            string str = "";
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + "  and Processid=" + DDProcessName.SelectedValue;
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + "  and Empid=" + DDEmpName.SelectedValue;
            }
            if (DDindentStatus.SelectedIndex > 0)
            {
                str = str + " and Status='" + DDindentStatus.SelectedItem.Text + "'";
            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
                }
            }

            if (ddItemName.SelectedIndex > 0)
            {
                str = str + "  and Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                str = str + "  and Qualityid=" + DDQuality.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0)
            {
                str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
            }
            if (ChkForDate.Checked == true)
            {
                if (RDSubitemwiseBalance.Checked == true)
                {
                    str = str + "  and IndentDate<='" + TxtFromDate.Text + "'";
                }
                else
                {
                    str = str + "  and IndentDate>='" + TxtFromDate.Text + "' and IndentDate<='" + TxtToDate.Text + "'";
                }
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                str = str + " and Customerid=" + DDCustCode.SelectedValue;
            }
            if (DDOrderNo.SelectedIndex > 0)
            {
                str = str + " and orderid=" + DDOrderNo.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETSHADEWISEDETAILEXCELREPORT_WITHOUTGATEINNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 35.22;
                sht.Column("B").Width = 20.22;
                sht.Column("C").Width = 25.22;
                sht.Column("D").Width = 25.33;
                sht.Column("E").Width = 15.00;
                sht.Column("F").Width = 18.00;
                sht.Column("G").Width = 9.00;
                sht.Column("H").Width = 9.00;
                sht.Column("I").Width = 9.00;
                sht.Column("J").Width = 9.00;
                sht.Column("K").Width = 9.00;
                sht.Column("L").Width = 9.00;
                sht.Column("M").Width = 9.00;
                sht.Column("N").Width = 9.00;

                sht.Range("A1:N1").Merge();
                sht.Range("A1").Value = DDCompany.SelectedItem.Text;
                sht.Range("A2:N2").Merge();
                sht.Range("A2").Value = ds.Tables[0].Rows[0]["CompAddr1"] + " " + ds.Tables[0].Rows[0]["CompAddr2"] + " " + ds.Tables[0].Rows[0]["CompAddr3"];
                sht.Range("A3:N3").Merge();
                sht.Range("A3").Value = "Tel No:" + ds.Tables[0].Rows[0]["comptel"] + " " + "Email:" + ds.Tables[0].Rows[0]["companyemail"];
                sht.Range("A4:N4").Merge();
                sht.Range("A4").Value = "GSTIN No:" + ds.Tables[0].Rows[0]["GSTNo"];
                sht.Row(4).Height = 30;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:N3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:N4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:N4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N1").Style.Alignment.SetWrapText();
                sht.Range("A2:N2").Style.Alignment.SetWrapText();
                sht.Range("A3:N3").Style.Alignment.SetWrapText();
                sht.Range("A4:N4").Style.Alignment.SetWrapText();
                sht.Range("A1:N4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:N4").Style.Font.FontSize = 10;
                sht.Range("A1:N4").Style.Font.Bold = true;

                //*******Header
                sht.Range("A5").Value = "ItemName";
                sht.Range("B5").Value = "Sub-ItemName";
                sht.Range("C5").Value = "ColorName";
                sht.Range("D5").Value = "OrderNo";
                sht.Range("E5").Value = "LotNo";
                sht.Range("F5").Value = "TagNo";
                sht.Range("G5").Value = "BillNo";
                sht.Range("H5").Value = "OQty";
                sht.Range("I5").Value = "Dyed Qty";
                sht.Range("J5").Value = "Undyed Qty";
                sht.Range("K5").Value = "Loss Qty";
                sht.Range("L5").Value = "Retn Qty";
                sht.Range("M5").Value = "Pending Qty";
                //sht.Range("N5").Value = "Gate In No";

                // sht.Column("D").Width = 9.33;


                sht.Range("A5:N5").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A5:N5").Style.Font.FontSize = 10;
                sht.Range("A5:N5").Style.Font.Bold = true;
                //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A5:N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A5:N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A5:N5").Style.Alignment.SetWrapText();

                DataView dv = new DataView(ds.Tables[0]);
                dv.Sort = "EmpId,EmpName,IndentId,IndentNo";
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dv.ToTable());

                DataTable dtdistinct = ds1.Tables[0].DefaultView.ToTable(true, "EmpId", "IndentId", "EmpName", "IndentNo", "IndentDate");
                DataView dv1 = new DataView(dtdistinct);
                DataSet ds2 = new DataSet();
                ds2.Tables.Add(dv1.ToTable());
                row = 6;
                int rowfrom = 6;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":N" + row).Style.Font.FontSize = 10;
                    sht.Range("A" + row + ":C" + row).Style.Font.Bold = true;


                    sht.Range("A" + row).SetValue("EmpName:" + ds2.Tables[0].Rows[i]["EmpName"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue("IndentNo:" + ds2.Tables[0].Rows[i]["IndentNo"]);
                    sht.Range("C" + row).SetValue("OrderDate:" + Convert.ToDateTime(ds2.Tables[0].Rows[i]["IndentDate"]).ToString("dd-MMM-yyyy"));
                    sht.Range("A" + row + ":C" + row).Style.Alignment.SetWrapText();

                    row = row + 1;

                    DataView dv2 = new DataView(ds1.Tables[0]);
                    dv2.RowFilter = "EmpId=" + ds2.Tables[0].Rows[i]["EmpId"] + " and IndentId=" + ds2.Tables[0].Rows[i]["IndentId"];
                    DataSet ds3 = new DataSet();
                    ds3.Tables.Add(dv2.ToTable());
                    int k = ds3.Tables[0].Rows.Count;
                    Decimal amt = 0;
                    string tagno = "";
                    string LotNo = "";
                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + row).SetValue(ds3.Tables[0].Rows[j]["Item_Name"]);
                        sht.Range("B" + row).SetValue(ds3.Tables[0].Rows[j]["QualityName"]);
                        sht.Range("B" + row).Style.Alignment.SetWrapText();
                        sht.Range("C" + row).SetValue(ds3.Tables[0].Rows[j]["ShadeColorName"]);
                        sht.Range("D" + row).SetValue(ds3.Tables[0].Rows[j]["OrderNo"]);
                        sht.Range("D" + row).Style.Alignment.SetWrapText();
                        sht.Range("E" + row).SetValue(ds3.Tables[0].Rows[j]["LotNo"]);
                        sht.Range("F" + row).SetValue(ds3.Tables[0].Rows[j]["TagNo"]);
                        sht.Range("G" + row).SetValue(ds3.Tables[0].Rows[j]["BillNo"]);
                        sht.Range("H" + row).SetValue(ds3.Tables[0].Rows[j]["OQty"]);
                        sht.Range("I" + row).SetValue(ds3.Tables[0].Rows[j]["dyedqty"]);
                        sht.Range("J" + row).SetValue(ds3.Tables[0].Rows[j]["undyedqty"]);
                        sht.Range("K" + row).SetValue(ds3.Tables[0].Rows[j]["Lossqty"]);
                        sht.Range("L" + row).SetValue(ds3.Tables[0].Rows[j]["Retqty"]);

                        if (LotNo == ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno == ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = amt + Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        }
                        if (LotNo == ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = 0;
                            amt = Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        }
                        if (LotNo != ds3.Tables[0].Rows[j]["LotNo"].ToString() && tagno != ds3.Tables[0].Rows[j]["TagNo"].ToString())
                        {
                            amt = 0;
                            amt = Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                            tagno = ds3.Tables[0].Rows[j]["TagNo"].ToString();
                            LotNo = ds3.Tables[0].Rows[j]["LotNo"].ToString();
                        }


                        // amt =amt+ Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"]);
                        //sht.Range("M" + row).SetValue(Convert.ToDecimal(ds3.Tables[0].Rows[j]["OQty"]) - (Convert.ToDecimal(ds3.Tables[0].Rows[j]["dyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["undyedqty"]) + Convert.ToDecimal(ds3.Tables[0].Rows[j]["Lossqty"]) - Convert.ToDecimal(ds3.Tables[0].Rows[j]["Retqty"])));
                        sht.Range("M" + row).SetValue(Convert.ToDecimal(ds3.Tables[0].Rows[j]["OQty"]) - (amt));
                        //sht.Range("N" + row).SetValue(ds3.Tables[0].Rows[j]["GateInNo"]);

                        // {Table.OQTY}-({Table.Dyedqty}+{Table.Undyedqty}+{Table.Lossqty}-{Table.Retqty})
                        row = row + 1;

                    }
                    //sht.Range("G" + row).FormulaA1 = "=SUM(G" + (row-k) + ":G" + (row - 1) + ")";
                    sht.Range("H" + row).FormulaA1 = "=SUM(H" + (row - k) + ":H" + (row - 1) + ")";
                    sht.Range("I" + row).FormulaA1 = "=SUM(I" + (row - k) + ":I" + (row - 1) + ")";
                    sht.Range("J" + row).FormulaA1 = "=SUM(J" + (row - k) + ":J" + (row - 1) + ")";
                    sht.Range("K" + row).FormulaA1 = "=SUM(K" + (row - k) + ":K" + (row - 1) + ")";
                    sht.Range("L" + row).FormulaA1 = "=SUM(L" + (row - k) + ":L" + (row - 1) + ")";
                    sht.Range("M" + row).FormulaA1 = "=SUM(M" + (row - k) + ":M" + (row - 1) + ")";
                    sht.Range("H" + row + ":N" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("H" + row + ":N" + row).Style.Font.FontSize = 9;
                    sht.Range("H" + row + ":N" + row).Style.Font.Bold = true;
                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:N" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ShadeWiseDetailNew_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void IndentRecDetailExcelReportDiamond(DataSet ds)
    {

        try
        {

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 14.56;
                sht.Column("B").Width = 23.33;
                sht.Column("C").Width = 13.00;
                sht.Column("D").Width = 20.22;
                sht.Column("E").Width = 10.89;
                sht.Column("F").Width = 14.45;
                sht.Column("G").Width = 14.45;
                sht.Column("H").Width = 13.56;
                sht.Column("I").Width = 11.67;
                sht.Column("J").Width = 33.89;
                sht.Column("K").Width = 11.22;
                sht.Column("L").Width = 10.22;
                sht.Column("M").Width = 10.22;
                sht.Column("N").Width = 10.22;
                sht.Column("O").Width = 20.22;
                sht.Column("P").Width = 15.11;
                sht.Column("Q").Width = 12.22;
                sht.Column("R").Width = 16.22;

                //*******Header
                sht.Range("A2").Value = "CustomerCode ";
                sht.Range("B2").Value = "Order No";

                sht.Range("C2").Value = "Indent No";
                sht.Range("D2").Value = "Party Name";

                sht.Range("E2").Value = "Rec No";
                sht.Range("F2").Value = "Iss ChallanNo";
                sht.Range("G2").Value = "Bill No";

                sht.Range("H2").Value = "Inwards No";

                sht.Range("I2").Value = "Rec Date";
                sht.Range("J2").Value = "Item Description";
                sht.Range("K2").Value = "Rec LotNo";
                sht.Range("L2").Value = "Rec TagNo";
                sht.Range("M2").Value = "Rec Qty";

                sht.Range("N2").Value = "Remarks";
                sht.Range("O2").Value = "Iss LotNo";
                sht.Range("P2").Value = "Iss TagNo";
                sht.Range("Q2").Value = "Indent Date";
                sht.Range("R2").Value = "Indent Rate";
                //sht.Range("R2").Value = "";

                sht.Range("A2:R2").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A2:R2").Style.Font.FontSize = 10;
                sht.Range("A2:R2").Style.Font.Bold = true;

                sht.Range("A2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:R2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A2:R2").Style.Alignment.SetWrapText();

                row = row + 3;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {

                    sht.Range("A" + row + ":R" + row).Style.Font.FontName = "Arial Unicode MS";
                    sht.Range("A" + row + ":R" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["CustomerCode"]);
                    sht.Range("A" + row).Style.Alignment.SetWrapText();
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("B" + row).Style.Alignment.SetWrapText();

                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["Empname"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["PRMid"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["IssueChallanNo"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["RecChallanNo"]);

                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["BillNo"]);


                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[j]["Description"]);
                    sht.Range("J" + row).Style.Alignment.SetWrapText();
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[j]["LotNo"]);
                    sht.Range("K" + row).Style.Alignment.SetWrapText();
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[j]["TagNo"]);
                    sht.Range("L" + row).Style.Alignment.SetWrapText();
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[j]["RECQTY"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[j]["IndentRecRemarks"]);
                    sht.Range("N" + row).Style.Alignment.SetWrapText();
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[j]["IssLotNo"]);
                    sht.Range("P" + row).SetValue(ds.Tables[0].Rows[j]["IssTagNo"]);
                    sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[j]["IndentDate"]);
                    sht.Range("R" + row).SetValue(ds.Tables[0].Rows[j]["IndentRate"]); 

                    row = row + 1;
                }

                //*************
                using (var a = sht.Range("A1:R" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("IndentReceiveDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #region
    protected void GenerateIndentDetailReportData()
    {
        try
        {
            // string str = "select *,'" + TxtFromDate.Text + "' as FromDate,'" + TxtToDate.Text + "' as ToDate," + (ChkForDate.Checked == true ? "1" : "0") + " as Dateflag From V_ORDERWISEINDENTDETAIL Where Companyid=" + DDCompany.SelectedValue;
            string str = "", strsample = ""; ;
            if (DDCompany.SelectedIndex > 0)
            {
                str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;
                //if (chksample.Checked == true)
                //{
                //    strsample = strsample + " and Companyid=" + DDCompany.SelectedValue;
                //}
            }
            if (DDCustCode.SelectedIndex > 0)
            {
                //str = str + " And OD.CustomerCode=" + DDCustCode.SelectedItem.Text;
                str = str + " and OD.CustomerCode='" + hnCustomerCode.Value + "'";
            }
            //if (DDOrderNo.SelectedIndex > 0)
            //{
            //    str = str + " And OrderId=" + DDOrderNo.SelectedValue;
            //}
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " And IM.Processid=" + DDProcessName.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.Processid=" + DDProcessName.SelectedValue;
                //}
            }
            if (DDEmpName.SelectedIndex > 0)
            {
                str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.empid=" + DDEmpName.SelectedValue;
                //}
            }
            if (DDIndentNo.SelectedIndex > 0)
            {
                if (chksample.Checked == true)
                {
                    str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
                }
                else
                {
                    str = str + "  and IM.Indentid=" + DDIndentNo.SelectedValue + " and IM.IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
                }
            }

            //if (ddItemName.SelectedIndex > 0)
            //{
            //    str = str + "  and Item_id=" + ddItemName.SelectedValue;
            //    //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
            //    //{
            //    //    strsample = strsample + "  and VF.Item_id=" + ddItemName.SelectedValue;
            //    //}
            //}
            //if (DDQuality.SelectedIndex > 0)
            //{
            //    str = str + "  and Qualityid=" + DDQuality.SelectedValue;
            //    //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
            //    //{
            //    //    strsample = strsample + "  and VF.Qualityid=" + DDQuality.SelectedValue;
            //    //}
            //}
            //if (DDShadeColor.SelectedIndex > 0)
            //{
            //    str = str + "  and ShadecolorId=" + DDShadeColor.SelectedValue;
            //    //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
            //    //{
            //    //    strsample = strsample + "  and VF.ShadecolorId=" + DDShadeColor.SelectedValue;
            //    //}
            //}
            if (ChkForDate.Checked == true)
            {
                str = str + "  And IM.Date>='" + TxtFromDate.Text + "' And IM.Date<='" + TxtToDate.Text + "'";
                //str = str + "  And PM.Date>='" + TxtFromDate.Text + "' And PM.Date<='" + TxtToDate.Text + "'";
                //if (variable.VarIndentIssRecReportDataWithSample == "1" || chksample.Checked == true)
                //{
                //    strsample = strsample + " and SM.issueDate>='" + TxtFromDate.Text + "' And sm.issueDate<='" + TxtToDate.Text + "'";
                //}
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETGENERATEINDENTDETAILREPORTDATA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@ChkForDate", ChkForDate.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@ChkForSample", chksample.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptGenerateIndentDetail.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptGenerateIndentDetail.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region
    protected void PurchaseandIndentIssueDetailReportData()
    {
        try
        {
            
            //string str = "", strsample = ""; ;
            //if (DDCompany.SelectedIndex > 0)
            //{
            //    str = str + " And IM.CompanyId=" + DDCompany.SelectedValue;               
            //}
            //if (DDCustCode.SelectedIndex > 0)
            //{                
            //    str = str + " and OD.CustomerCode='" + hnCustomerCode.Value + "'";
            //}           
            //if (DDProcessName.SelectedIndex > 0)
            //{
            //    str = str + " And IM.Processid=" + DDProcessName.SelectedValue;
                
            //}
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    str = str + " And EI.EmpId=" + DDEmpName.SelectedValue;               
            //}
            //if (DDIndentNo.SelectedIndex > 0)
            //{
            //    if (chksample.Checked == true)
            //    {
            //        str = str + "  and Indentid=" + DDIndentNo.SelectedValue + " and IndentNo='Sample-" + DDIndentNo.SelectedItem.Text + "'";
            //    }
            //    else
            //    {
            //        str = str + "  and IM.Indentid=" + DDIndentNo.SelectedValue + " and IM.IndentNo='" + DDIndentNo.SelectedItem.Text + "'";
            //    }
            //}
           
            //if (ChkForDate.Checked == true)
            //{
            //    str = str + "  And IM.Date>='" + TxtFromDate.Text + "' And IM.Date<='" + TxtToDate.Text + "'";
                
            //}

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETPURCHASEINDENTISSUEDETAILREPORTDATA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@TagNo", txtTagNo.Text);            
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptPurchaseIndentIssueMaterialDetailReportCI.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptPurchaseIndentIssueMaterialDetailReportCI.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion
    //protected void IndentWiseDetailExportExcelNew()
    //{
    //    try
    //    {

    //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //        if (con.State == ConnectionState.Closed)
    //        {
    //            con.Open();
    //        }
    //        SqlCommand cmd = new SqlCommand("PRO_GETINDENTWISEDETAILEXCELREPORTFOREMAIL", con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandTimeout = 300;

    //        cmd.Parameters.AddWithValue("@Companyid", DDCompany.SelectedValue);
    //        cmd.Parameters.AddWithValue("@Where", "");

    //        DataSet ds = new DataSet();
    //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
    //        cmd.ExecuteNonQuery();
    //        ad.Fill(ds);
    //        //*************

    //        con.Close();
    //        con.Dispose();

    //        //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
    //            {
    //                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
    //            }
    //            string Path = "";
    //            var xapp = new XLWorkbook();
    //            var sht = xapp.Worksheets.Add("sheet1");
    //            int row = 0;

    //            sht.Column("A").Width = 20.22;
    //            sht.Column("B").Width = 20.22;
    //            sht.Column("C").Width = 20.22;
    //            sht.Column("D").Width = 20.22;
    //            sht.Column("E").Width = 20.22;
    //            sht.Column("F").Width = 20.22;
    //            sht.Column("G").Width = 20.22;


    //            //*******Header
    //            sht.Range("A2").Value = "Indent No";
    //            sht.Range("B2").Value = "Issue Date";
    //            sht.Range("C2").Value = "Required Date";
    //            sht.Range("D2").Value = "Dyer Name";
    //            sht.Range("E2").Value = "Iss Qty";
    //            sht.Range("F2").Value = "Rec Qty";
    //            sht.Range("G2").Value = "Pending Qty";


    //            sht.Range("A2:G2").Style.Font.FontName = "Arial Unicode MS";
    //            sht.Range("A2:G2").Style.Font.FontSize = 10;
    //            sht.Range("A2:G2").Style.Font.Bold = true;

    //            sht.Range("A5:R5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    //            sht.Range("A5:R5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
    //            sht.Range("A5:R5").Style.Alignment.SetWrapText();

    //            row = row + 3;

    //            Decimal amt = 0;
    //            string tagno = "";
    //            string LotNo = "";
    //            ////string IndentNo = "";

    //            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
    //            {

    //                sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial Unicode MS";
    //                sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 10;

    //                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["IndentNo"]);
    //                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[j]["Date"]);
    //                //sht.Range("B" + row).Style.Alignment.SetWrapText();
    //                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["ReqDate"]);
    //                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["EmpName"]);
    //                sht.Range("D" + row).Style.Alignment.SetWrapText();
    //                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["IssueQty"]);
    //                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["RecQty"]);
    //                sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Rows[j]["IssueQty"]) - Convert.ToDecimal(ds.Tables[0].Rows[j]["RecQty"]));

    //                row = row + 1;
    //            }

    //            //*************
    //            using (var a = sht.Range("A1:G" + row))
    //            {
    //                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //            }

    //            sht.Columns(1, 30).AdjustToContents();

    //            string Fileextension = "xlsx";
    //            string filename = UtilityModule.validateFilename("IndentWiseDetailForEmail_" + DateTime.Now + "." + Fileextension);
    //            Path = HttpRuntime.AppDomainAppPath + "\\Tempexcel\\" + filename;
    //            xapp.SaveAs(Path);
    //            xapp.Dispose();
    //            string attchmnt;
    //            attchmnt = Path;

    //            Sendmail.SendMail(pTo: "", pSubject: "Indent Pending Detail.", pBody: "", pFromSMTP: "smtp_1", pFromDispName: "", pAttachments: attchmnt, pCC: "", pAttchmntName: "", pBCC: "");


    //            //string Fileextension = "xlsx";
    //            //string filename = UtilityModule.validateFilename("IndentWiseDetailForEmail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
    //            //Path = Server.MapPath("~/Tempexcel/" + filename);
    //            //xapp.SaveAs(Path);
    //            //xapp.Dispose();
    //            ////Download File
    //            //Response.ClearContent();
    //            //Response.ClearHeaders();
    //            //// Response.Clear();
    //            //Response.ContentType = "application/vnd.ms-excel";
    //            //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
    //            //Response.WriteFile(Path);
    //            //// File.Delete(Path);
    //            //Response.End();




    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}

}