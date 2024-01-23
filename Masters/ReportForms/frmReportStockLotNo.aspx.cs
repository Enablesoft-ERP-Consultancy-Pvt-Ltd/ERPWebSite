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


public partial class Masters_ReportForms_frmReportStockLotNo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Session["varCompanyNo"].ToString() == "22" && Session["UserType"].ToString() == "1")
        {
            TRTagNo.Visible = true;
            ChkForExcelExport.Visible = true;
        }
        else
        {
            TRTagNo.Visible = false;
            ChkForExcelExport.Visible = false;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        if (ChkForExcelExport.Checked == true)
        {
            try
            {
                string str = "";
                if (txtLotno.Text != "")
                {

                    str = @"Select distinct CI.CompanyName,LSN.TStockNo,Lotno,vf.ITEM_NAME as Item_Name,vf.ColorName as ColorName,vf.SizeMtr as SizeMtr,vf.DesignName
                            From ProcessRawMaster PM(Nolock) 
                            JOIN ProcessRawTran PT(Nolock) ON PT.PRMid = PM.PRMid And PT.Lotno = '" + txtLotno.Text + @"'
                            JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PID.IssueOrderId = PM.Prorderid 
                            JOIN LoomStockNo LSN(NoLock) ON LSN.IssueDetailid=PID.Issue_Detail_Id and LSN.Issueorderid=PM.Prorderid and LSN.ProcessId=1	
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = LSN.Item_Finished_Id 
                            JOIN CompanyInfo CI(Nolock) ON CI.Companyid = LSN.Companyid 
                            Where PM.TypeFlag = 0 and LSN.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                            Order By LSN.TStockNo 

                            Select CompanyId,'~/Images/Logo/1_company.gif' Photo From Companyinfo Where CompanyId=" + Session["CurrentWorkingCompanyID"];

//                    str = @"Select distinct CI.CompanyName,CN.TStockNo,CN.StockNo,Lotno,vf.ITEM_NAME as Item_Name,vf.ColorName as ColorName,vf.SizeMtr as SizeMtr
//                            From ProcessRawMaster PM(Nolock) 
//                            JOIN ProcessRawTran PT(Nolock) ON PT.PRMid = PM.PRMid And PT.Lotno = '" + txtLotno.Text + @"'
//                            JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PID.IssueOrderId = PM.Prorderid 
//                            JOIN Process_Stock_Detail PSD(NoLock) ON PSD.IssueDetailId=PID.Issue_Detail_Id and PSD.ToProcessId=1
//                            JOIN CarpetNumber CN(NoLock) ON CN.StockNo=PSD.StockNo and CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
//                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
//                            JOIN CompanyInfo CI(Nolock) ON CI.Companyid = CN.Companyid 
//                            Where PM.TypeFlag = 0 
//                            Order By CN.StockNo 
                            
                }
                else if (txtTagNo.Text != "")
                {

                    str = @"Select distinct CI.CompanyName,LSN.TStockNo,Lotno,PT.TagNo,vf.ITEM_NAME as Item_Name,vf.ColorName as ColorName,vf.SizeMtr as SizeMtr,vf.DesignName
                            From ProcessRawMaster PM(Nolock) 
                            JOIN ProcessRawTran PT(Nolock) ON PT.PRMid = PM.PRMid And PT.Tagno = '" + txtTagNo.Text + @"'
                            JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PID.IssueOrderId = PM.Prorderid 
                            JOIN LoomStockNo LSN(NoLock) ON LSN.IssueDetailid=PID.Issue_Detail_Id and LSN.Issueorderid=PM.Prorderid and LSN.ProcessId=1	
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = LSN.Item_Finished_Id 
                            JOIN CompanyInfo CI(Nolock) ON CI.Companyid = LSN.Companyid 
                            Where PM.TypeFlag = 0 and LSN.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                            Order By LSN.TStockNo


                            Select CompanyId,'~/Images/Logo/1_company.gif' Photo From Companyinfo Where CompanyId=" + Session["CurrentWorkingCompanyID"];


//                    str = @"Select distinct CI.CompanyName,CN.TStockNo,CN.StockNo,Lotno,PT.TagNo,vf.ITEM_NAME as Item_Name,vf.ColorName as ColorName,vf.SizeMtr as SizeMtr
//                            From ProcessRawMaster PM(Nolock) 
//                            JOIN ProcessRawTran PT(Nolock) ON PT.PRMid = PM.PRMid And PT.Tagno = '" + txtTagNo.Text + @"'
//                            JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PID.IssueOrderId = PM.Prorderid 
//                            JOIN Process_Stock_Detail PSD(NoLock) ON PSD.IssueDetailId=PID.Issue_Detail_Id and PSD.ToProcessId=1
//						    JOIN CarpetNumber CN(NoLock) ON CN.StockNo=PSD.StockNo and CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
//                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
//                            JOIN CompanyInfo CI(Nolock) ON CI.Companyid = CN.Companyid 
//                            Where PM.TypeFlag = 0 
//                            Order By CN.StockNo               

                           
                }

                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StockNoWithLotAndTagNoReportExcel(ds);                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Opn1", "alert('No records found...');", true);
                }
                Tran.Commit();
            }
            catch (Exception)
            {
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

        }
        else
        {
            if (txtLotno.Text != "")
            {

                try
                {

                    string str = @"Select CI.CompanyName,CN.TStockNo,CN.StockNo,Lotno,vf.ITEM_NAME+' '+vf.ColorName +'  '+vf.SizeMtr As Description 
                        From ProcessRawMaster PM(Nolock) 
                        JOIN ProcessRawTran PT(Nolock) ON PT.PRMid = PM.PRMid And PT.Lotno = '" + txtLotno.Text + @"'
                        JOIN PROCESS_ISSUE_DETAIL_1 PID(Nolock) ON PID.IssueOrderId = PM.Prorderid 
                        JOIN PROCESS_RECEIVE_DETAIL_1 PRD(Nolock) ON PRD.IssueOrderId = PID.IssueOrderId And PRD.Issue_Detail_Id = PID.Issue_Detail_Id 
                        JOIN CarpetNumber CN(Nolock) ON CN.Process_Rec_id = PRD.Process_Rec_Id And CN.Process_Rec_Detail_Id = PRD.Process_Rec_Detail_Id 
                            And CN.CompanyID = " + Session["CurrentWorkingCompanyID"] + @"
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
                        JOIN CompanyInfo CI(Nolock) ON CI.Companyid = CN.Companyid 
                        Where PM.TypeFlag = 0 
                        Order By CN.StockNo 

                        Select CompanyId,'~/Images/Logo/1_company.gif' Photo From Companyinfo Where CompanyId=" + Session["CurrentWorkingCompanyID"];

                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {

                            if (Convert.ToString(dr["Photo"]) != "")
                            {
                                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                                if (TheFile.Exists)
                                {
                                    string img = dr["Photo"].ToString();
                                    img = Server.MapPath(img);
                                    Byte[] img_Byte = File.ReadAllBytes(img);
                                    dr["Image"] = img_Byte;
                                }
                            }
                        }

                        Session["rptFileName"] = "~\\Reports\\RptStockNoWithLotno.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\RptStockNoWithLotno.xsd";
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Opn1", "alert('No records found...');", true);
                    }
                    Tran.Commit();
                }
                catch (Exception)
                {
                    Tran.Rollback();
                }
                finally
                {
                    con.Dispose();
                    con.Close();
                }
            }
        }



    }
    protected void txtLotno_TextChanged(object sender, EventArgs e)
    {
        btnPrint_Click(sender, e);
    }
    protected void txtTagNo_TextChanged(object sender, EventArgs e)
    {
        btnPrint_Click(sender, e);
    }
    protected void StockNoWithLotAndTagNoReportExcel(DataSet ds)
    {
        
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("Sheet1");

                //*************
                //***********
                sht.Row(1).Height = 24;
                sht.Range("A1:E1").Merge();
                sht.Range("A1:E1").Style.Font.FontSize = 10;
                sht.Range("A1:E1").Style.Font.Bold = true;
                sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1:E1").Style.Alignment.WrapText = true;
                //************

                if (txtLotno.Text != "")
                {
                    sht.Range("A1").SetValue("LOTNO :" + txtLotno.Text);
                }
                else if (txtTagNo.Text != "")
                {
                    sht.Range("A1").SetValue("TAGNO :" + txtTagNo.Text);
                }
                else
                {
                    sht.Range("A1").SetValue("");
                }
               

                sht.Range("A2:E2").Style.Font.FontSize = 10;
                sht.Range("A2:E2").Style.Font.Bold = true;
                //sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                //sht.Range("L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                sht.Range("A2").Value = "SR NO";
                sht.Range("B2").Value = "STOCK NO";
                sht.Range("C2").Value = "ARTICLE";
                sht.Range("D2").Value = "COLOR";
                sht.Range("E2").Value = "SIZE";               

                int row = 3;
                int SrNo = 0;
                //DataView dv = ds.Tables[0].DefaultView;
                //dv.Sort = "ReceiveDate,challanNo";
                //DataSet ds1 = new DataSet();
                //ds1.Tables.Add(dv.ToTable());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SrNo = SrNo + 1;

                    sht.Range("A" + row).SetValue(SrNo);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["TStockNo"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["SizeMtr"]);
                  
                    row = row + 1;
                }
                ds.Dispose(); 
               
                //************** Save
                String Path;
                sht.Columns(1, 20).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("StockNoLotTagNoWise_" + DateTime.Now + "." + Fileextension);
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
}