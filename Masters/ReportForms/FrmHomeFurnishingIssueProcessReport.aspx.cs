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
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmHomeFurnishingIssueProcessReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TRlotNo.Visible = false;
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname ";
                if(Session["varCompanyId"].ToString()=="44")
                {
                    str+="Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME  From PROCESS_NAME_MASTER PNM(Nolock) Where PNM.AddProcessName = 1 And PNM.MasterCompanyID = " + Session["varCompanyId"] + " Order By PNM.PROCESS_NAME "; 
                }
                else
                {
                       str+="SELECT PROCESS_NAME_ID, PROCESS_NAME From Process_Name_Master(Nolock) Where Process_Name in ('WEAVING','STITCHING','CUTTING', 'PANEL MAKING', 'FILLER MAKING', 'FILLAR MOUTH CLOSING', 'FILLER BHARAI', 'FILLER PALTI', 'FILLER CUTTING', 'PANEL PRESS', 'LABEL TAGGING', 'FILLER JOB WORK', 'FILLER FILLING+MOUTH CLOSING', 'SLIDER PLATING ON ZIPPER') And MasterCompanyId=" + Session["varCompanyId"] + @" Order By PROCESS_NAME";
                }

                str += " select CI.CustomerId,CI.CustomerCode from customerinfo  CI order by CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Select--");
            TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");           
            
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            int varcompanyNo = Convert.ToInt16(Session["varcompanyid"].ToString());
            switch (varcompanyNo)
            {
                case 8:                   
                    break;
                case 16:                   
                    break;               

            }

            TRcustcode.Visible = true;
            TRorderno.Visible = true;
           TRlotNo.Visible = false;            
           TRRecChallan.Visible = true;
            TRProcessName.Visible = true;
            ChkForDate.Checked = false; 
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarFinishingNewModuleWise == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "select EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname From EmpInfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId Where EMP.ProcessId=" + DDProcessName.SelectedValue + " order by EmpName", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName+case When isnull(ei.empcode,'')<>'' then ' ['+ei.empcode+']' else '' end as Empname from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EmpName", true, "--Select--");
        }
        ////  EmpSelectedChanged();
        //if (RDHomeFurnishingRecDetail.Checked == true )
        //{
            UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");
        //}
    }
    protected void DDEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanged();
    }
    private void EmpSelectedChanged()
    {
        string Str = "";
        if (DDProcessName.SelectedIndex > 0)
        {
            if (DDProcessName.SelectedItem.Text == "WEAVING" || DDProcessName.SelectedItem.Text == "CUTTING")
            {
                Str = @"Select distinct HFOM.IssueOrderID,isnull(ChallanNo,HFOM.IssueOrderID) as IssueOrderId1 from HomeFurnishingOrderMaster HFOM(NoLock) 
                        LEFT JOIN Employee_HomeFurnishingOrderMaster EM(NoLock)  ON HFOM.IssueOrderID=EM.IssueOrderID and EM.ProcessID=" + DDProcessName.SelectedValue + @" Where HFOM.CompanyId=" + DDCompany.SelectedValue;
                if (DDEmpName.SelectedIndex > 0)
                {
                    Str = Str + " And EM.EmpId=" + DDEmpName.SelectedValue;
                }
                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
                Label4.Text = "Issue Challan No.";
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str = @"Select distinct PIM.IssueOrderID,isnull(ChallanNo,PIM.IssueOrderID) as IssueOrderId1 from PROCESS_Issue_MASTER_13 PIM(NoLock)  
                        LEFT JOIN Employee_ProcessOrderNo EM(NoLock)  ON PIM.IssueOrderId=EM.IssueOrderID and EM.PROCESSID=117 Where PIM.IssueOrderId>0 and PIM.CompanyId=" + DDCompany.SelectedValue;
                if (DDEmpName.SelectedIndex > 0)
                {
                    Str = Str + " And EM.EmpId=" + DDEmpName.SelectedValue;
                }
                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
                Label4.Text = "Issue Challan No.";
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str = @"Select distinct HFMOM.IssueOrderId,isnull(ChallanNo,HFMOM.IssueOrderId) as IssueOrderId1 from HomeFurnishingMakingOrderMaster HFMOM(NoLock)  
                        LEFT JOIN Employee_HomeFurnishingMakingOrderMaster EM(NoLock)  ON HFMOM.IssueOrderID=EM.IssueOrderID and EM.PROCESSID=" + DDProcessName.SelectedValue + @" 
                        Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + @"   and HFMOM.CompanyId=" + DDCompany.SelectedValue;
                if (DDEmpName.SelectedIndex > 0)
                {
                    Str = Str + " And EM.EmpId=" + DDEmpName.SelectedValue;
                }
                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--Select--");
                Label4.Text = "Issue Challan No.";
            }

            ChallanNoSelectedIndexChange();
        }
       
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedIndexChange();
    }
    private void ChallanNoSelectedIndexChange()
    {
        string Str = "";
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0)
        {
            if (DDProcessName.SelectedItem.Text == "WEAVING" || DDProcessName.SelectedItem.Text == "CUTTING" || DDProcessName.SelectedItem.Text == "DIGITAL EMBROIDERY(PCS)" || DDProcessName.SelectedItem.Text == "MANUAL EMBROIDERY(PCS)")
            {

                Str1 = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME from HomeFurnishingOrderMaster HFOM(NoLocK)  JOIN HomeFurnishingOrderDetail HFOD(NoLock)  ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock)  ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {

                Str1 = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID 
                         JOIN CategorySeparate cs(NoLock) on VF.CATEGORY_ID=CS.Categoryid and CS.id=0
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {

                Str1 = @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID 
                         JOIN CategorySeparate cs(NoLock) on VF.CATEGORY_ID=CS.Categoryid and CS.id=0
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
           
            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDCategory, Str1, true, "--Select--");
           
        }
    }   
    
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
       
        if (lblMessage.Text == "")
        {
            CHECKVALIDCONTROL();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (DDProcessName.SelectedItem.Text == "WEAVING" || DDProcessName.SelectedItem.Text == "CUTTING" || DDProcessName.SelectedItem.Text == "DIGITAL EMBROIDERY(PCS)" || DDProcessName.SelectedItem.Text == "MANUAL EMBROIDERY(PCS)" || DDProcessName.SelectedItem.Text == "COMPUTER EMBROIDERY(PCS)" || DDProcessName.SelectedItem.Text == "DIGITAL PRINTING(PCS)" || DDProcessName.SelectedItem.Text == "TABLE TUFTING" || DDProcessName.SelectedItem.Text == "KANTHA HANDWORK" || DDProcessName.SelectedItem.Text == "TUFTING" || DDProcessName.SelectedItem.Text == "APLIQUE CUTTING" || DDProcessName.SelectedItem.Text == "UPHOLSTERY" || DDProcessName.SelectedItem.Text == "BLOCK PRINTING")
                {
                    if (ChkForIssRecSummary.Checked == true)
                    {
                        if (Session["varCompanyId"].ToString() == "44")
                        {
                            HomeFurnishingIssueRecReport_agni();
                        }
                        else
                        {
                            HomeFurnishingIssueRecReport();
                        }
                    }
                    else
                    {
                        if (Session["varCompanyId"].ToString() == "44")
                        {
                            HomeFurnishingIssueDetailReport_agni();
                        }
                        else
                        {

                            HomeFurnishingIssueDetailReport();
                        
                        }
                    }
                                                       
                }
                else if (DDProcessName.SelectedItem.Text == "STITCHING")
                {
                    if (ChkForIssRecSummary.Checked == true)
                    {
                        HomeFurnishingStitchingIssueRecReport();
                    }
                    else
                    {
                        HomeFurnishingStitchingIssueDetailReport();
                    }
                }
                else if (DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER" )
                {
                    if (ChkForIssRecSummary.Checked == true)
                    {
                        HomeFurnishingPanelFillerIssueRecReport();
                    }
                    else
                    {
                        HomeFurnishingPanelFillerIssueDetailReport();
                    }
                }
               
                tran.Commit();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/ReportForms/FrmHomeFurnishingReceiveProcessReport.aspx");
                tran.Rollback();
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
    }
    protected void HomeFurnishingIssueDetailReport()
    {        
            string str = "";
            DataSet ds = new DataSet();
           
            //Check Conditions
            if (ChkForDate.Checked == true)
            {
                str = str + " And HFOM.AssignDate>='" + TxtFromDate.Text + "' And HFOM.AssignDate<='" + TxtToDate.Text + "'";
            }
            if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
            {
                str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;                
            }
            if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
            {
                str = str + " And OM.orderid=" + DDorderno.SelectedValue;               
            }   
            if (DDChallanNo.SelectedIndex > 0)
            {
                str = str + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
            {
                str = str + " And VF.designId=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
            {
                str = str + " And VF.ColorId=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
            {
                str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
            {
                str = str + " And VF.SizeId=" + DDSize.SelectedValue;
            }
            if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
            {
                str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
            }
           

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHINGISSUEDETAILREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
            cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
            cmd.Parameters.AddWithValue("@where", str);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
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

                sht.Range("A1").Value = "HOME FURNISHING ISSUE DETAIL";
                sht.Range("A1:K1").Style.Font.FontName = "Calibri";
                sht.Range("A1:K1").Style.Font.Bold = true;
                sht.Range("A1:K1").Style.Font.FontSize = 12;
                sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:K1").Merge();

                //*******Header
                sht.Range("A2").Value = "Issue Date";
                sht.Range("B2").Value = "Customer Code";
                sht.Range("C2").Value = "Customer OrderNo";
                sht.Range("D2").Value = "Issue ChallanNo";
                sht.Range("E2").Value = "Job Name";
                sht.Range("F2").Value = "Quality";
                sht.Range("G2").Value = "Design";
                sht.Range("H2").Value = "Color";
                sht.Range("I2").Value = "Size";
                sht.Range("J2").Value = "Issue Qty";
                sht.Range("K2").Value = "Emp Name";
                sht.Range("L2").Value = "Checked By";
                sht.Range("M2").Value = "Rate";
                sht.Range("N2").Value = "Amount";
                sht.Range("O2").Value = "User Name";


                sht.Range("A2:O2").Style.Font.FontName = "Calibri";
                sht.Range("A2:O2").Style.Font.FontSize = 11;
                sht.Range("A2:O2").Style.Font.Bold = true;
                //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                row = 3;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Calibri";
                    sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 10;

                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                    sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                    sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                    sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                    row = row + 1;
                }

                //*************
                sht.Columns(1, 30).AdjustToContents();

                using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
                {
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("HomeFurnishingIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
            }
            //*************
        
    }
    protected void HomeFurnishingIssueDetailReport_agni()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And HFOM.AssignDate>='" + TxtFromDate.Text + "' And HFOM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHINGISSUEDETAILREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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
            sht.Range("A1:O1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            sht.Range("A1:O1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:O1").Style.Font.FontSize = 13;
            sht.Range("A1:O1").Style.Font.Bold = true;
            sht.Range("A1:O1").Merge();
            sht.Range("A2:O2").Value = ds.Tables[0].Rows[0]["COMPADDR1"].ToString() + "    " + "Print Date-" + DateTime.Now.ToShortDateString();
            sht.Range("A2:O2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:O2").Style.Font.FontSize = 13;
            sht.Range("A2:O2").Style.Font.Bold = true;
            sht.Range("A2:O2").Merge();
            sht.Range("A3:O3").Value = "GSTNo.-" + ds.Tables[0].Rows[0]["GSTNO"].ToString();
            sht.Range("A3:O3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:O3").Style.Font.FontSize = 12;
            sht.Range("A3:O3").Style.Font.Bold = true;
            sht.Range("A3:O3").Merge();
            sht.Range("A4").Value = "CUTTING ISSUE DETAIL";
            sht.Range("A4:O4").Style.Font.FontName = "Calibri";
            sht.Range("A4:O4").Style.Font.Bold = true;
            sht.Range("A4:O4").Style.Font.FontSize = 12;
            sht.Range("A4:O4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:O4").Merge();

            //*******Header
            sht.Range("A5").Value = "Issue Date";
            sht.Range("B5").Value = "Customer Code";
            sht.Range("C5").Value = "Customer OrderNo";
            sht.Range("D5").Value = "Issue ChallanNo";
            sht.Range("E5").Value = "Job Name";
            sht.Range("F5").Value = "Quality";
            sht.Range("G5").Value = "Design";
            sht.Range("H5").Value = "Color";
            sht.Range("I5").Value = "Size";
            sht.Range("J5").Value = "Issue Qty";
            sht.Range("K5").Value = "Emp Name";
            sht.Range("L5").Value = "Checked By";
            sht.Range("M5").Value = "Rate";
            sht.Range("N5").Value = "Amount";
            sht.Range("O5").Value = "User Name";


            sht.Range("A5:O5").Style.Font.FontName = "Calibri";
            sht.Range("A5:O5").Style.Font.FontSize = 11;
            sht.Range("A5:O5").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 6;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    protected void HomeFurnishingStitchingIssueDetailReport()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And PIM.AssignDate>='" + TxtFromDate.Text + "' And PIM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHING_STITCHING_ISSUEDETAILREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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

            sht.Range("A1").Value = "HOME FURNISHING STITCHING ISSUE DETAIL";
            sht.Range("A1:K1").Style.Font.FontName = "Calibri";
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Font.FontSize = 12;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Merge();

            //*******Header
            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Customer Code";
            sht.Range("C2").Value = "Customer OrderNo";
            sht.Range("D2").Value = "Issue ChallanNo";
            sht.Range("E2").Value = "Job Name";
            sht.Range("F2").Value = "Quality";
            sht.Range("G2").Value = "Design";
            sht.Range("H2").Value = "Color";
            sht.Range("I2").Value = "Size";
            sht.Range("J2").Value = "Issue Qty";
            sht.Range("K2").Value = "Emp Name";
            sht.Range("L2").Value = "Checked By";
            sht.Range("M2").Value = "Rate";
            sht.Range("N2").Value = "Amount";
            sht.Range("O2").Value = "User Name";


            sht.Range("A2:O2").Style.Font.FontName = "Calibri";
            sht.Range("A2:O2").Style.Font.FontSize = 11;
            sht.Range("A2:O2").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingStitchingIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    protected void HomeFurnishingPanelFillerIssueDetailReport()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And HFMOM.AssignDate>='" + TxtFromDate.Text + "' And HFMOM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHING_PANELFILLER_ISSUEDETAILREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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

            sht.Range("A1").Value = "HOME FURNISHING  "+DDProcessName.SelectedItem.Text+" "+"ISSUE DETAIL";
            sht.Range("A1:K1").Style.Font.FontName = "Calibri";
            sht.Range("A1:K1").Style.Font.Bold = true;
            sht.Range("A1:K1").Style.Font.FontSize = 12;
            sht.Range("A1:K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:K1").Merge();

            //*******Header
            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Customer Code";
            sht.Range("C2").Value = "Customer OrderNo";
            sht.Range("D2").Value = "Issue ChallanNo";
            sht.Range("E2").Value = "Job Name";
            sht.Range("F2").Value = "Quality";
            sht.Range("G2").Value = "Design";
            sht.Range("H2").Value = "Color";
            sht.Range("I2").Value = "Size";
            sht.Range("J2").Value = "Issue Qty";
            sht.Range("K2").Value = "Emp Name";
            sht.Range("L2").Value = "Checked By";
            sht.Range("M2").Value = "Rate";
            sht.Range("N2").Value = "Amount";
            sht.Range("O2").Value = "User Name";


            sht.Range("A2:O2").Style.Font.FontName = "Calibri";
            sht.Range("A2:O2").Style.Font.FontSize = 11;
            sht.Range("A2:O2").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":O" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":O" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Qty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingPanelFillerIssueDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    protected void HomeFurnishingIssueRecReport()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And HFOM.AssignDate>='" + TxtFromDate.Text + "' And HFOM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHINGISSUE_REC_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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

            sht.Range("A1").Value = "HOME FURNISHING ISSUE REC DETAIL";
            sht.Range("A1:Q1").Style.Font.FontName = "Calibri";
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:Q1").Style.Font.FontSize = 12;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Merge();

            //*******Header
            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Customer Code";
            sht.Range("C2").Value = "Customer OrderNo";
            sht.Range("D2").Value = "Issue ChallanNo";
            sht.Range("E2").Value = "Job Name";
            sht.Range("F2").Value = "Quality";
            sht.Range("G2").Value = "Design";
            sht.Range("H2").Value = "Color";
            sht.Range("I2").Value = "Size";
            sht.Range("J2").Value = "Issue Qty";
            sht.Range("K2").Value = "Rec Qty";
            sht.Range("L2").Value = "Pending Qty";

            sht.Range("M2").Value = "Emp Name";
            sht.Range("N2").Value = "Checked By";
            sht.Range("O2").Value = "Rate";
            sht.Range("P2").Value = "Amount";
            sht.Range("Q2").Value = "User Name";


            sht.Range("A2:Q2").Style.Font.FontName = "Calibri";
            sht.Range("A2:Q2").Style.Font.FontSize = 11;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["RecQty"]));


                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "Q")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingIssueRecDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    protected void HomeFurnishingIssueRecReport_agni()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And HFOM.AssignDate>='" + TxtFromDate.Text + "' And HFOM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHINGISSUE_REC_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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
            sht.Range("A4").Value = "CUTTING ISSUE REC DETAIL";
            sht.Range("A4:Q4").Style.Font.FontName = "Calibri";
            sht.Range("A4:Q4").Style.Font.Bold = true;
            sht.Range("A4:Q4").Style.Font.FontSize = 12;
            sht.Range("A4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:Q4").Merge();

            //*******Header
            sht.Range("A5").Value = "Issue Date";
            sht.Range("B5").Value = "Customer Code";
            sht.Range("C5").Value = "Customer OrderNo";
            sht.Range("D5").Value = "Issue ChallanNo";
            sht.Range("E5").Value = "Job Name";
            sht.Range("F5").Value = "Quality";
            sht.Range("G5").Value = "Design";
            sht.Range("H5").Value = "Color";
            sht.Range("I5").Value = "Size";
            sht.Range("J5").Value = "Issue Qty";
            sht.Range("K5").Value = "Rec Qty";
            sht.Range("L5").Value = "Pending Qty";

            sht.Range("M5").Value = "Emp Name";
            sht.Range("N5").Value = "Checked By";
            sht.Range("O5").Value = "Rate";
            sht.Range("P5").Value = "Amount";
            sht.Range("Q5").Value = "User Name";


            sht.Range("A5:Q5").Style.Font.FontName = "Calibri";
            sht.Range("A5:Q5").Style.Font.FontSize = 11;
            sht.Range("A5:Q5").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 6;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["RecQty"]));


                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "Q")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingIssueRecDetailreport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }

    protected void HomeFurnishingStitchingIssueRecReport()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And PIM.AssignDate>='" + TxtFromDate.Text + "' And PIM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHING_STITCHING_ISSUE_REC_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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

            sht.Range("A1").Value = "HOME FURNISHING STITCHING ISSUE REC DETAIL";
            sht.Range("A1:Q1").Style.Font.FontName = "Calibri";
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:q1").Style.Font.FontSize = 12;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Merge();

            //*******Header
            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Customer Code";
            sht.Range("C2").Value = "Customer OrderNo";
            sht.Range("D2").Value = "Issue ChallanNo";
            sht.Range("E2").Value = "Job Name";
            sht.Range("F2").Value = "Quality";
            sht.Range("G2").Value = "Design";
            sht.Range("H2").Value = "Color";
            sht.Range("I2").Value = "Size";
            sht.Range("J2").Value = "Issue Qty";
            sht.Range("K2").Value = "Rec Qty";
            sht.Range("L2").Value = "Pending Qty";

            sht.Range("M2").Value = "Emp Name";
            sht.Range("N2").Value = "Checked By";
            sht.Range("O2").Value = "Rate";
            sht.Range("P2").Value = "Amount";
            sht.Range("Q2").Value = "User Name";


            sht.Range("A2:Q2").Style.Font.FontName = "Calibri";
            sht.Range("A2:Q2").Style.Font.FontSize = 11;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderId"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["RecQty"]));


                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "Q")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingStitchingIssueRecReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    protected void HomeFurnishingPanelFillerIssueRecReport()
    {
        string str = "";
        DataSet ds = new DataSet();

        //Check Conditions
        if (ChkForDate.Checked == true)
        {
            str = str + " And HFMOM.AssignDate>='" + TxtFromDate.Text + "' And HFMOM.AssignDate<='" + TxtToDate.Text + "'";
        }
        if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
        {
            str = str + " And OM.Customerid=" + DDcustcode.SelectedValue;
        }
        if (TRorderno.Visible == true && DDorderno.SelectedIndex > 0)
        {
            str = str + " And OM.orderid=" + DDorderno.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
        {
            str = str + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
        {
            str = str + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
        {
            str = str + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            str = str + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {
            str = str + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_HOMEFURNISHING_PANELFILLER_ISSUE_REC_REPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 1000;

        cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@processid", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@FromDate", TxtFromDate.Text);
        cmd.Parameters.AddWithValue("@Todate", TxtToDate.Text);
        cmd.Parameters.AddWithValue("@Empid", DDEmpName.SelectedIndex <= 0 ? "0" : DDEmpName.SelectedValue);
        cmd.Parameters.AddWithValue("@where", str);
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varCompanyId"]);
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
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

            sht.Range("A1").Value = "HOME FURNISHING  " + DDProcessName.SelectedItem.Text + " " + "ISSUE REC DETAIL";
            sht.Range("A1:Q1").Style.Font.FontName = "Calibri";
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:Q1").Style.Font.FontSize = 12;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Merge();

            //*******Header
            sht.Range("A2").Value = "Issue Date";
            sht.Range("B2").Value = "Customer Code";
            sht.Range("C2").Value = "Customer OrderNo";
            sht.Range("D2").Value = "Issue ChallanNo";
            sht.Range("E2").Value = "Job Name";
            sht.Range("F2").Value = "Quality";
            sht.Range("G2").Value = "Design";
            sht.Range("H2").Value = "Color";
            sht.Range("I2").Value = "Size";
            sht.Range("J2").Value = "Issue Qty";
            sht.Range("K2").Value = "Rec Qty";
            sht.Range("L2").Value = "Pending Qty";

            sht.Range("M2").Value = "Emp Name";
            sht.Range("N2").Value = "Checked By";
            sht.Range("O2").Value = "Rate";
            sht.Range("P2").Value = "Amount";
            sht.Range("Q2").Value = "User Name";


            sht.Range("A2:Q2").Style.Font.FontName = "Calibri";
            sht.Range("A2:Q2").Style.Font.FontSize = 11;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            //sht.Range("M1:S1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            row = 3;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 10;

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["AssignDate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["IssueOrderID"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["PROCESS_NAME"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Width"].ToString() + 'x' + ds.Tables[0].Rows[i]["Length"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["IssueQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecQty"]);
                sht.Range("L" + row).SetValue(Convert.ToDouble(ds.Tables[0].Rows[i]["IssueQty"]) - Convert.ToDouble(ds.Tables[0].Rows[i]["RecQty"]));

                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["EMPNAME"]);
                sht.Range("N" + row).SetValue(ds.Tables[0].Rows[i]["Checkedby"]);
                sht.Range("O" + row).SetValue(ds.Tables[0].Rows[i]["Rate"]);
                sht.Range("P" + row).SetValue(ds.Tables[0].Rows[i]["Amount"]);
                sht.Range("Q" + row).SetValue(ds.Tables[0].Rows[i]["UserName"]);

                row = row + 1;
            }

            //*************
            sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range(sht.Cell(1, 1), sht.Cell(row, "O")))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("HomeFurnishingPanelFillerIssueRecReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "altNo", "alert('No records found..')", true);
        }
        //*************

    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS(sender);
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDQuality.Visible == true)
        {          

            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.QualityId,VF.QualityName from HomeFurnishingOrderMaster HFOM JOIN HomeFurnishingOrderDetail HFOD ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.QualityId,VF.QualityName from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.QualityId,VF.QualityName from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            Str1 = Str1 + " Order By VF.QualityName ";
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        else
        {
            Str1 = @"Select Distinct VF.Qualityid,VF.QualityNAME from V_FinishedItemDetail VF Where  VF.MasterCompanyId=" + Session["varCompanyId"] +" and vf.qualityname<>''";
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.Category_id=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            Str1 = Str1 + " Order BY VF.QualityNAME ";
            UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");
        }
        if (DDQuality.Items.Count > 0)
        {
            DDQuality.SelectedIndex = 0;
            DDQuality_SelectedIndexChanged(sender, new EventArgs());
        }
        //************
        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName From Shape", true, "--Plz Select--");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        QDCSDDFill(DDDesign, DDColor, DDShape, DDShadeColor);
        
    }
    private void CATEGORY_DEPENDS_CONTROLS(object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0)
        {  

            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME from HomeFurnishingOrderMaster HFOM JOIN HomeFurnishingOrderDetail HFOD ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.ITEM_ID,VF.ITEM_NAME from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (DDCategory.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");

        }
       
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
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
                    case "4":
                        TRDDShape.Visible = true;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;

                        //                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from View_StockTranGetPassDetail PM,
                        //                        V_FinishedItemDetail VF Where PM.finishedid=VF.Item_finished_id And VF.MasterCompanyId=" + Session["varCompanyId"];
                        //                        if (DDCategory.SelectedIndex > 0)
                        //                        {
                        //                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        //                        }
                        //                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        Str1 = @"Select Distinct VF.ShadecolorId,VF.shadecolorname from V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " and vf.shadecolorname<>''";
                        if (DDCategory.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
                        }
                        if (ddItemName.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Item_id=" + ddItemName.SelectedValue;
                        }
                        if (DDQuality.SelectedIndex > 0)
                        {
                            Str1 = Str1 + " And VF.Qualityid=" + DDQuality.SelectedValue;

                        }
                        Str1 = Str1 + " Order BY VF.shadecolorname ";
                        UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
                        break;
                }
            }
        }
        if (ddItemName.Items.Count > 0)
        {
            ddItemName.SelectedIndex = 0;
            if (sender != null)
            {
                ddItemName_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    private void QDCSDDFill(DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, object sender = null)
    {
        string Str1 = "";
        if (DDProcessName.SelectedIndex > 0 && TRDDDesign.Visible == true)
        { 
            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.designId,VF.designName from HomeFurnishingOrderMaster HFOM(NoLock) JOIN HomeFurnishingOrderDetail HFOD(NoLock) ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock) ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.designId,VF.designName from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.designId,VF.designName from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Designname";
            UtilityModule.ConditionalComboFill(ref DDDesign, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDColor.Visible == true)
        {  
            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.ColorId,VF.ColorName from HomeFurnishingOrderMaster HFOM(NoLock) JOIN HomeFurnishingOrderDetail HFOD(NoLock) ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock) ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.ColorId,VF.ColorName from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.ColorId,VF.ColorName from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by Colorname";
            UtilityModule.ConditionalComboFill(ref DDColor, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShape.Visible == true)
        {            

            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName from HomeFurnishingOrderMaster HFOM(NoLock) JOIN HomeFurnishingOrderDetail HFOD(NoLock) ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock) ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.ShapeId,VF.ShapeName from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }

            UtilityModule.ConditionalComboFill(ref DDShape, Str1, true, "--Select--");
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
        {  
            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName from HomeFurnishingOrderMaster HFOM(NoLock) JOIN HomeFurnishingOrderDetail HFOD(NoLock) ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock) ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.ShadecolorId,VF.ShadeColorName from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0 && TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDShadeColor, Str1, true, "--Select--");
        }

    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblshadename.Text = ParameterList[7];
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str1 = "";
        string strSize = "Sizeft";
        if (chkmtr.Checked == true)
        {
            strSize = "Sizemtr";
        }
        if (DDProcessName.SelectedIndex > 0 && TRDDSize.Visible == true)
        {
            if (DDProcessName.SelectedItem.Text == "WEAVING")
            {
                Str1 = @"Select Distinct VF.SizeId,VF." + strSize + @" as size from HomeFurnishingOrderMaster HFOM(NoLock) JOIN HomeFurnishingOrderDetail HFOD(NoLock) ON HFOM.IssueOrderID=HFOD.IssueOrderID
                        JOIN V_FinishedItemDetail VF(NoLock) ON HFOD.OrderDetailDetail_FinishedID=VF.Item_Finished_ID Where HFOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if (DDProcessName.SelectedItem.Text == "STITCHING")
            {
                Str1 = @"Select Distinct VF.SizeId,VF." + strSize + @" as size from PROCESS_ISSUE_MASTER_13 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_13 PID(NoLock) ON PIM.IssueOrderID=PID.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON PID.ITEM_FINISHED_ID=VF.Item_Finished_ID                         
                         Where PIM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And PIM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }
            else if ((DDProcessName.SelectedItem.Text == "PANEL MAKING" || DDProcessName.SelectedItem.Text == "FILLER MAKING" || DDProcessName.SelectedItem.Text == "FILLAR MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "FILLER BHARAI" || DDProcessName.SelectedItem.Text == "FILLER PALTI" || DDProcessName.SelectedItem.Text == "FILLER CUTTING" || DDProcessName.SelectedItem.Text == "PANEL PRESS" || DDProcessName.SelectedItem.Text == "LABEL TAGGING" || DDProcessName.SelectedItem.Text == "FILLER JOB WORK" || DDProcessName.SelectedItem.Text == "FILLER FILLING+MOUTH CLOSING" || DDProcessName.SelectedItem.Text == "SLIDER PLATING ON ZIPPER"))
            {
                Str1 = @"Select Distinct VF.SizeId,VF." + strSize + @" as size from HomeFurnishingMakingOrderMaster HFMOM(NoLock) JOIN HomeFurnishingMakingOrderDetail HFMOD(NoLock) ON HFMOM.IssueOrderID=HFMOD.IssueOrderID
                         JOIN V_FinishedItemDetail VF(NoLock) ON HFMOD.ITEM_FINISHED_ID=VF.Item_Finished_ID                          
                         Where HFMOM.ProcessId=" + DDProcessName.SelectedValue + " HFMOM.CompanyId=" + DDCompany.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (DDChallanNo.SelectedIndex > 0)
                {
                    Str1 = Str1 + " And HFMOM.IssueOrderID=" + DDChallanNo.SelectedValue;
                }
            }

            if (DDEmpName.SelectedIndex > 0 && variable.VarFinishingNewModuleWise != "1")
            {
                Str1 = Str1 + " And EmpId=" + DDEmpName.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                Str1 = Str1 + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
            {
                Str1 = Str1 + " And VF.QualityId=" + DDQuality.SelectedValue;
            }
            Str1 = Str1 + " order by size";
            UtilityModule.ConditionalComboFill(ref DDSize, Str1, true, "--Select--");
        }
    }


    //protected void RDHomeFurnishingRecDetail_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (RDHomeFurnishingRecDetail.Checked == true)
    //    {
           
    //        TRcustcode.Visible = true;
    //        TRorderno.Visible = true;
           
    //        TRlotNo.Visible = false;            
    //        TRRecChallan.Visible = true;
    //        TRProcessName.Visible = true;
    //        ChkForDate.Checked = false;           

    //        //UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " Order By EI.EmpName", true, "--Select--");
    //    }
    //}
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

    protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }    
    
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (variable.VarReportPwd == txtpwd.Text)
        {
            Session["ReportPath"] = "Reports/RptProcessDetailNEWIssueNoWise.rpt";
            //Finishinghissab();
        }
        else
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Please Enter Correct Password..";

        }

    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }   
    
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Om.OrderId,Om.CustomerOrderNo From OrderMaster OM where CompanyId=" + DDCompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + "  order by CustomerOrderNo";
        UtilityModule.ConditionalComboFill(ref DDorderno, str, true, "--Select--");

        //string str = "select Om.OrderId,Om.CustomerOrderNo From OrderMaster OM where CompanyId=" + DDCompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + "  and  Om.Status=0 order by CustomerOrderNo";
        //UtilityModule.ConditionalComboFill(ref DDorderno, str, true, "--Select--");
    }   

   
}