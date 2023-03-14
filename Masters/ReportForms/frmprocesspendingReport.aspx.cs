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

public partial class Masters_ReportForms_frmprocesspendingReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = null;
            DataSet ds = null;
            str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                    select  UnitsId,UnitName from  units with(nolock) Where Mastercompanyid=" + Session["varcompanyid"] + @"
                    select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                    cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid=" + Session["varcompanyid"] + @"
                    select  ShapeId,ShapeName from Shape with(nolock)
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) where MasterCompanyid=" + Session["varcompanyid"] + @"
                    select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticleName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 5, true, "--Plz Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            if (DDShape.Items.Count > 0)
            {
                DDShape.SelectedIndex = 0;
            }
            ds.Dispose();
            txtFdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtTdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            switch (Session["varcompanyId"].ToString())
            {
                case "8":
                    ChkForExcel.Visible = false; 
                    break;
                case "22":
                    ChkForExcel.Visible = true;
                    break;
                default:
                    ChkForExcel.Visible = false;
                    divQuality.Visible = true;
                    divDesign.Visible = true;
                    divCategory.Visible = true;
                    if (DDCategory.Items.Count > 0)
                    {
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                    }
                    break;
            }
        }
    }
    protected void DDArticleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct QualityId,QualityName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And Qualityid<>0 order by QualityName", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct ColorId,ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And ColorId<>0", true, "--Plz Select--");
        }

    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSize, @"select Distinct SizeId,SizeMtr from V_FinishedItemDetail vf with(nolock)
                                                      inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + @"
                                                      And ColorId=" + DDColor.SelectedValue + " And ShapeId= " + DDShape.SelectedValue + "", true, "--Plz Select--");
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (ChkForExcel.Checked == true)
        {
            ExcelReport();
        }
        else
        {

            string str;
            DataSet ds = new DataSet();
            try
            {
                str = @"select distinct  CI.CompanyId,ci.CompanyName,'" + ddJob.SelectedItem.Text + @"' as Job,U.UnitsId,U.UnitName,vf.item_id,vf.ITEM_NAME+' '+vf.qualityname+' '+vf.designname as ITEM_NAME,vf.ColorId,vf.ColorName,
                    vf.SizeId,vf.ProdSizeMtr,prm.ReceiveDate,cn.TStockNo,cn.StockNo,'" + txtFdate.Text + "' as FromDate,'" + txtTdate.Text + "' as Todate," + (chkDate.Checked == true ? 1 : 0) + @" as Dateflag 
                    from  PROCESS_RECEIVE_MASTER_1 PRM 
                    inner join PROCESS_RECEIVE_DETAIL_1 PRD on PRM.Process_Rec_Id=prd.Process_Rec_Id 
                    inner join CarpetNumber cn on cn.Process_Rec_id=prm.Process_Rec_Id and cn.Process_Rec_Detail_Id=prd.Process_Rec_Detail_Id
                    inner join PROCESS_ISSUE_MASTER_1 PIM on PIM.IssueOrderId=prd.IssueOrderId
                    inner join Units U on U.UnitsId=PIM.Units
                    inner join V_FinishedItemDetail vf on vf.ITEM_FINISHED_ID=cn.Item_Finished_Id
                    inner join Process_Stock_Detail PSD on PSD.StockNo=CN.StockNo and psd.IssueDetailId=prd.Issue_Detail_Id and psd.ToProcessId=1
                    inner join companyinfo CI on Ci.CompanyId=PRM.Companyid 
                    and not exists(select StockNo from Process_Stock_Detail Where ToProcessId=" + ddJob.SelectedValue + @" and StockNo=cn.StockNo) 
                    Where PRM.CompanyId=" + DDCompanyName.SelectedValue;
                if (DDUnitName.SelectedIndex != -1)
                {
                    str = str + " and PIM.units=" + DDUnitName.SelectedValue;
                }
                if (DDArticleName.SelectedIndex > 0)
                {
                    str = str + " and VF.Item_Id=" + DDArticleName.SelectedValue;
                }
                if (ddquality.SelectedIndex > 0)
                {
                    str = str + " and VF.qualityid=" + ddquality.SelectedValue;
                }
                if (DDDesign.SelectedIndex > 0)
                {
                    str = str + " and VF.DesignId=" + DDDesign.SelectedValue;
                }
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and vf.shapeid=" + DDShape.SelectedValue;
                }
                if (ddSize.SelectedIndex > 0)
                {
                    str = str + " and vf.SizeId=" + ddSize.SelectedValue;
                }
                if (chkDate.Checked == true)
                {
                    str = str + " and Prm.ReceiveDate>='" + txtFdate.Text + "' and prm.receivedate<='" + txtTdate.Text + "'";
                }
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {


                    Session["dsFileName"] = "~\\ReportSchema\\RptProcessPendingReport.xsd";
                    Session["rptFileName"] = "Reports/RptProcessPendingReport.rpt";
                    Session["GetDataset"] = ds;
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
                lblMessage.Text = ex.Message;
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divDesign.Visible == true)
        {
            string str = @"select Distinct Designid,designName from V_FinishedItemDetail vf with(nolock)
                         inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and DesignId<>0";
            if (ddquality.SelectedIndex > 0)
            {
                str = str + " And Qualityid=" + ddquality.SelectedValue + "";
            }
            str = str + "  order by designName";
            UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Plz Select--");
            DDDesign_SelectedIndexChanged(sender, e);
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divColor.Visible == true)
        {
            string str = @"select Distinct ColorId,ColorName from V_FinishedItemDetail vf with(nolock)
                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And ColorId<>0";
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + " and Designid=" + DDDesign.SelectedValue;
            }
            str = str + " order by colorname";
            UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Plz Select--");
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDArticleName, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varcompanyId"] + " order by IM.ITEM_NAME", true, "--Plz Select--");
    }

    protected void ExcelReport()
    {
        string Where = "";
        if (DDUnitName.SelectedIndex != -1)
        {
            Where = Where + " and PIM.units=" + DDUnitName.SelectedValue;
        }
        if (DDArticleName.SelectedIndex > 0)
        {
            Where = Where + " and VF.Item_Id=" + DDArticleName.SelectedValue;
        }
        if (ddquality.SelectedIndex > 0)
        {
            Where = Where + " and VF.qualityid=" + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " and VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (ddSize.SelectedIndex > 0)
        {
            Where = Where + " and vf.SizeId=" + ddSize.SelectedValue;
        }
        if (chkDate.Checked == true)
        {
            Where = Where + " and Prm.ReceiveDate>='" + txtFdate.Text + "' and prm.receivedate<='" + txtTdate.Text + "'";
        }
     

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_GETPROCESSPENDINGEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid",  ddJob.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessName",  ddJob.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@ChkForDate", chkDate.Checked == true ? 1 : 0);            
            cmd.Parameters.AddWithValue("@Fromdate", txtFdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtTdate.Text);
            cmd.Parameters.AddWithValue("@Where",Where);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sda.Fill(dt);
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel"));
                }

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");

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
                sht.Range("A1").SetValue(ds1.Tables[0].Rows[0]["CompanyName"].ToString());

                sht.Row(2).Height = 24;
                sht.Range("A2:F2").Merge();
                sht.Range("A2:F2").Style.Font.FontSize = 10;
                sht.Range("A2:F2").Style.Font.Bold = true;
                sht.Range("A2:F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:F2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2:F2").Style.Alignment.WrapText = true;
                sht.Range("A2").SetValue("PROCESS PENDING REPORT  (From -" + txtFdate.Text + " To-" + txtTdate.Text + ") ");

                sht.Row(3).Height = 24;
                sht.Range("A3:F3").Merge();
                sht.Range("A3:F3").Style.Font.FontSize = 10;
                sht.Range("A3:F3").Style.Font.Bold = true;
                sht.Range("A3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:F3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A3:F3").Style.Alignment.WrapText = true;
                sht.Range("A3").SetValue("JOB NAME  (" + ds1.Tables[0].Rows[0]["Job"].ToString() + ") ");

                sht.Range("A4:F4").Style.Font.FontSize = 10;
                sht.Range("A4:F4").Style.Font.Bold = true;

                sht.Range("A4").Value = "SR.NO";
                sht.Range("B4").Value = "STOCK NO";
                sht.Range("C4").Value = "RECEIVE DATE";
                sht.Range("D4").Value = "ITEM DESCRIPTION";
                sht.Range("E4").Value = "COLOR";
                sht.Range("F4").Value = "SIZE";               

                int row = 5;
                int SRNo = 0;

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    SRNo = SRNo + 1;

                    sht.Range("A" + row).SetValue(SRNo);
                    sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["TStockNo"]);
                    sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ReceiveDate"]);
                    sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Item_Name"]);
                    sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["ProdSizeMtr"]);
                   
                    //sht.Range("I" + row).FormulaA1 = "=F" + row + '-' + "($H$" + row + '+' + "$G$" + row + ")";

                    row = row + 1;
                }

                ds1.Dispose();

                //************** Save
                String Path;
                sht.Columns(1, 12).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("ProcessPendingReport_" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt3", "alert('No Record Found');", true);
            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }



    }
}