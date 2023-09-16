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


public partial class Masters_ReportForms_frmRptForProduction_JobSummary : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER  order by Process_Name
                         select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                         select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varcompanyid"] + @"
                         select Val,Type from Sizetype
                         Select CustomerID, CustomerCode From CustomerInfo Where MasterCompanyid = " + Session["varcompanyid"] + " Order By CustomerCode ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDjob, ds, 0, true, "--Plz Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerOrderNo, ds, 4, true, "--Plz Select--");

            txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            //*************
            switch (Session["varcompanyId"].ToString())
            {
                case "8":
                    TRcategoty.Visible = false;
                    TRddItemName.Visible = false;
                    TDempwise.Visible = true;
                    break;
                case "16":
                    TRForProcessWiseSummary.Visible = true;
                    break;  
                case "21":
                    TRForWithoutWeavingProcess.Visible = true;
                    break;
                default:
                    TRForWithoutWeavingProcess.Visible = false; 
                    break;
            }
            //**************
            switch (variable.ReportWithpdf)
            {
                case "1":
                    chkexport.Visible = true;
                    Export = "N";
                    break;
                default:
                    Export = "Y";
                    chkexport.Visible = false;
                    break;
            }
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        switch (Session["varcompanyId"].ToString())
        {
            case "8":
                Showreportforanisa();
                break;
            case "22":
                if ((chkexport.Checked == true) && (chkwithstockdetail.Checked == true) && (chkforday.Checked == true))
                {
                    ShowreportforOther_day();
                }
                else if (ChkDetailWithAllProcess.Checked == true)
                {
                    ShowReportWithAllProcessSize();
                }
                else
                {
                    ShowreportforOther();
                }                
                break;
            default:
                if ((chkexport.Checked == true) && (chkwithstockdetail.Checked == true) && (chkforday.Checked == true))
                {
                    ShowreportforOther_day();
                }
                else
                {
                    ShowreportforOther();
                }
                break;
        }
    }
    protected void Showreportforanisa()
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] array = new SqlParameter[10];
            array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[1] = new SqlParameter("@Fromdate", SqlDbType.SmallDateTime);
            array[2] = new SqlParameter("@Todate", SqlDbType.SmallDateTime);
            array[3] = new SqlParameter("@Companyid", SqlDbType.Int);
            array[4] = new SqlParameter("@UserId", SqlDbType.Int);
            array[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            array[6] = new SqlParameter("@UnitsId", SqlDbType.Int);

            array[0].Value = DDjob.SelectedIndex <= 0 ? "0" : DDjob.SelectedValue;
            array[1].Value = txtFromdate.Text;
            array[2].Value = txtToDate.Text;
            array[3].Value = Session["CurrentWorkingCompanyID"];
            array[4].Value = Session["varuserId"];
            array[5].Value = Session["varcompanyId"];
            array[6].Value = DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue;

            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "[Pro_ForProduction_JobSummary]", array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["dsFileName"] = "~\\ReportSchema\\RptProduction_JobSummary.xsd";
                if (chkempwise.Checked == true)
                {
                    Session["rptFileName"] = "Reports/RptProduction_JobSummaryEmpWise.rpt";
                }
                else
                {
                    Session["rptFileName"] = "Reports/RptProduction_JobSummary.rpt";
                }
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
        catch (Exception)
        {
        }
        finally
        {
            con.Dispose();
            con.Close();
            ds.Dispose();
        }

    }
    protected void ShowreportforOther()
    {
        string str = "";
        string FilterBy = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.item_id=" + ddItemName.SelectedValue;
            FilterBy = FilterBy + " ITEMNAME-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
            FilterBy = FilterBy + " QUALITY-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            FilterBy = FilterBy + " DESIGN-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            FilterBy = FilterBy + " COLOR-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDShape.SelectedValue;
            FilterBy = FilterBy + " SHAPE-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            FilterBy = FilterBy + " SIZE-" + DDSize.SelectedItem.Text;
        }
        //**********************************
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_ForProduction_JobSummaryOther", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@ProcessId", DDjob.SelectedIndex <= 0 ? "0" : DDjob.SelectedValue);
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
            cmd.Parameters.AddWithValue("@Companyid", Session["CurrentWorkingCompanyID"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@UnitsId", DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@excelexport", chkexport.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@WITHSTOCKDETAIL", chkwithstockdetail.Checked == true ? "1" : "0");
            cmd.Parameters.AddWithValue("@CustomerID", TrCustomerCode.Visible == true ? DDCustomerOrderNo.SelectedIndex <= 0 ? "0" : DDCustomerOrderNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@OrderID", TrOrderNo.Visible == true ? DDOrderNo.SelectedIndex <= 0 ? "0" : DDOrderNo.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@WithoutWeavingProcess", TRForWithoutWeavingProcess.Visible == true ? ChkForWithoutWeavingProcess.Checked == true ? "1" : "0" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForProcessWiseSummary.Checked == true)
                {
                    ForProcessWiseSummary(ds, FilterBy);
                    return;
                }
                if (chkexport.Checked == true && chkwithstockdetail.Checked == false)
                {
                    JobwiseissueExcelExport(ds, FilterBy);
                    return;
                }
                else if (chkexport.Checked == true && chkwithstockdetail.Checked == true)
                {
                    switch (Session["VarCompanyId"].ToString())
                    {
                        case "22":
                            if (ChkForSummaryDetail.Checked == true)
                            {
                                JobwiseissueExcelExportDiamond_WithstockDetailNewFormat(ds, FilterBy);
                            }
                            else
                            {
                                JobwiseissueExcelExportDiamond_WithstockDetail(ds, FilterBy);
                            }
                            break;
                        default:
                            JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);
                            break;

                    }

                    //JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);
                    //return;
                }
                else
                {
                    Session["dsFileName"] = "~\\ReportSchema\\RptProduction_JobSummary.xsd";
                    if (Session["VarCompanyId"].ToString() == "21")
                    {
                        if (Session["UserType"].ToString() != "1")
                        {
                            Session["rptFileName"] = "Reports/RptProduction_JobSummaryNewWithoutAmt.rpt";
                        }
                        else
                        {
                            Session["rptFileName"] = "~\\Reports\\RptProduction_JobSummaryNewKaysons.rpt";
                        }
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/RptProduction_JobSummaryNew.rpt";
                    }
                    Session["GetDataset"] = ds;
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
        catch (Exception)
        {
        }
        finally
        {

        }


        //DataSet ds = new DataSet();
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //try
        //{
        //    SqlParameter[] array = new SqlParameter[10];

        //    array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
        //    array[1] = new SqlParameter("@Fromdate", SqlDbType.SmallDateTime);
        //    array[2] = new SqlParameter("@Todate", SqlDbType.SmallDateTime);
        //    array[3] = new SqlParameter("@Companyid", SqlDbType.Int);
        //    array[4] = new SqlParameter("@UserId", SqlDbType.Int);
        //    array[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        //    array[6] = new SqlParameter("@UnitsId", SqlDbType.Int);
        //    array[7] = new SqlParameter("@Where", str);
        //    array[8] = new SqlParameter("@excelexport", chkexport.Checked == true ? "1" : "0");
        //    array[9] = new SqlParameter("@WITHSTOCKDETAIL", chkwithstockdetail.Checked == true ? "1" : "0");


        //    array[0].Value = DDjob.SelectedIndex <= 0 ? "0" : DDjob.SelectedValue;
        //    array[1].Value = txtFromdate.Text;
        //    array[2].Value = txtToDate.Text;
        //    array[3].Value = 1;
        //    array[4].Value = Session["varuserId"];
        //    array[5].Value = Session["varcompanyId"];
        //    array[6].Value = DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue;

        //    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_ForProduction_JobSummaryOther", array);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (chkexport.Checked == true && chkwithstockdetail.Checked == false)
        //        {
        //            JobwiseissueExcelExport(ds, FilterBy);
        //            return;
        //        }
        //        else if (chkexport.Checked == true && chkwithstockdetail.Checked == true)
        //        {
        //            switch (Session["VarCompanyId"].ToString())
        //            {
        //                case "22":
        //                    JobwiseissueExcelExportDiamond_WithstockDetail(ds, FilterBy);                           
        //                    break;                            
        //                default :
        //                    JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);                            
        //                    break;

        //            }

        //            //JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);
        //            //return;
        //        }
        //        else
        //        {
        //            Session["dsFileName"] = "~\\ReportSchema\\RptProduction_JobSummary.xsd";
        //            Session["rptFileName"] = "Reports/RptProduction_JobSummaryNew.rpt";
        //            Session["GetDataset"] = ds;
        //            StringBuilder stb = new StringBuilder();
        //            stb.Append("<script>");
        //            stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //    }

        //}
        //catch (Exception)
        //{
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //    ds.Dispose();
        //}

    }
    protected void ShowreportforOther_day()
    {
        //string str = "";
        //string FilterBy = "";
        //if (ddItemName.SelectedIndex > 0)
        //{
        //    str = str + " and vf.item_id=" + ddItemName.SelectedValue;
        //    FilterBy = FilterBy + " ITEMNAME-" + ddItemName.SelectedItem.Text;
        //}
        //if (DDQuality.SelectedIndex > 0)
        //{
        //    str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
        //    FilterBy = FilterBy + " QUALITY-" + DDQuality.SelectedItem.Text;
        //}
        //if (DDDesign.SelectedIndex > 0)
        //{
        //    str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        //    FilterBy = FilterBy + " DESIGN-" + DDDesign.SelectedItem.Text;
        //}
        //if (DDColor.SelectedIndex > 0)
        //{
        //    str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        //    FilterBy = FilterBy + " COLOR-" + DDColor.SelectedItem.Text;
        //}
        //if (DDShape.SelectedIndex > 0)
        //{
        //    str = str + " and vf.shapeid=" + DDShape.SelectedValue;
        //    FilterBy = FilterBy + " SHAPE-" + DDShape.SelectedItem.Text;
        //}
        //if (DDSize.SelectedIndex > 0)
        //{
        //    str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        //    FilterBy = FilterBy + " SIZE-" + DDSize.SelectedItem.Text;
        //}
        //**********************************
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_ForProduction_JobSummaryOther_Day", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@ProcessId", DDjob.SelectedIndex <= 0 ? "0" : DDjob.SelectedValue);
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
            cmd.Parameters.AddWithValue("@Companyid", Session["CurrentWorkingCompanyID"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@UnitsId", DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@category", DDCategory.SelectedIndex <= 0 ? "0" : DDCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@item", ddItemName.SelectedIndex <= 0 ? "0" : ddItemName.SelectedValue);
            cmd.Parameters.AddWithValue("@quality", DDQuality.SelectedIndex <= 0 ? "0" : DDQuality.SelectedValue);
            cmd.Parameters.AddWithValue("@design", DDDesign.SelectedIndex <= 0 ? "0" : DDDesign.SelectedValue);
            cmd.Parameters.AddWithValue("@color", DDColor.SelectedIndex <= 0 ? "0" : DDColor.SelectedValue);
            cmd.Parameters.AddWithValue("@shape", DDShape.SelectedIndex <= 0 ? "0" : DDShape.SelectedValue);


            DataSet ds = new DataSet();
            DataSet dsfinal = new DataSet();
            DataTable dtstock = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                     int min = 0, max = ds.Tables[0].Rows.Count;
                     dtstock = ds.Tables[1].AsEnumerable().Where(a => a.Field<Int32>("ToProcessId") == 1).GroupBy(a => a.Field<Int32>("stockno")).Select(x => x.First()).CopyToDataTable();
                     dsfinal.Tables.Add(new DataTable());
                    // dsfinal.Tables[0].Columns.Add("stockno", typeof(string));
                     dsfinal.Tables[0].Columns.Add("TSTOCKNO", typeof(string));
                     dsfinal.Tables[0].Columns.Add("QUALITY", typeof(string));
                     dsfinal.Tables[0].Columns.Add("DESIGN", typeof(string));
                     dsfinal.Tables[0].Columns.Add("COLORNAME", typeof(string));
                     dsfinal.Tables[0].Columns.Add("SIZE", typeof(string));
                     foreach(DataRow item in ds.Tables[0].Rows)
                     {
                         dsfinal.Tables[0].Columns.Add(Convert.ToString(item["PROCESS_NAME"]), typeof(string));
                     }
                     foreach (DataRow stocks in dtstock.Rows)
                     {
                         DateTime? issuedate = new DateTime();
                         DateTime? receivedate = new DateTime();
                         DateTime? assigndate = new DateTime();
                         string processname = string.Empty, issue_date = string.Empty, receive_date = string.Empty, assign_date = string.Empty;
                         DataRow finalrow = dsfinal.Tables[0].NewRow();
                         List<stockdata> lst = ds.Tables[1].AsEnumerable().Where(a=>a.Field<Int32>("stockno") == Convert.ToInt32(stocks["stockno"])).Select(b => new stockdata { stockno = b.Field<Int32?>("stockno"),
                             FromProcessId = b.Field<Int32?>("FromProcessId"),Tstockno=b.Field<string>("Tstockno"),
                             ToProcessId = b.Field<Int32?>("ToProcessId"),AssignDate = b.Field<DateTime?>("AssignDate"),OrderDate = b.Field<DateTime?>("OrderDate"),ReceiveDate = b.Field<DateTime?>("ReceiveDate"),
                             Rec_Date = b.Field<DateTime?>("Rec_Date"),item_name = b.Field<string>("item_name"),QualityName = b.Field<string>("QualityName"),
                             DesignName = b.Field<string>("DesignName"),ColorName = b.Field<string>("ColorName"),SizeFt = b.Field<string>("SizeMtr") }).ToList();

                        // finalrow["stockno"] = stocks["stockno"];
                         finalrow["TSTOCKNO"] = stocks["Tstockno"];
                         finalrow["QUALITY"] = stocks["QualityName"];
                         finalrow["DESIGN"] = stocks["DesignName"];
                         finalrow["COLORNAME"] = stocks["colorname"];
                         finalrow["SIZE"] = stocks["SizeMtr"];
                         assigndate = Convert.ToDateTime(stocks["AssignDate"]);
                         if (assigndate != null)
                         {
                             assign_date = assigndate.Value.ToShortDateString();
                         }
                         Int32? toproid = lst.Where(b => b.FromProcessId == 1).Count() > 0 ? lst.Where(b => b.FromProcessId == 1).FirstOrDefault().ToProcessId : 0;
                         if (toproid > 0)
                         {
                             foreach (var item in lst.OrderBy(b => b.FromProcessId))
                             {
                                 issue_date = string.Empty;
                                 receive_date = string.Empty;
                              //   assign_date = string.Empty;
                                     toproid = item.ToProcessId;
                                    

                                     issuedate =lst.Where(b => b.ToProcessId == toproid).FirstOrDefault().OrderDate;
                                     if (issuedate != null)
                                     {
                                         issue_date = issuedate.Value.ToShortDateString();
                                     }
                                 
                                     receivedate = lst.Where(b => b.ToProcessId == toproid).FirstOrDefault().ReceiveDate;
                                     if (receivedate != null)
                                     {
                                         receive_date = receivedate.Value.ToShortDateString();
                                     }
                                     processname = ds.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("PROCESS_NAME_ID") == toproid).Select(b => b.Field<string>("PROCESS_NAME")).FirstOrDefault();
                                     if (toproid == 1)
                                     {
                                         finalrow[processname] = Convert.ToString(assign_date) + "|" + Convert.ToString(receive_date);
                                     }
                                     else
                                     {
                                         finalrow[processname] = Convert.ToString(issue_date) + "|" + Convert.ToString(receive_date);
                                     }
                             }
                         }
                         else
                         {
                             issue_date = string.Empty;
                             receive_date = string.Empty;
                             issuedate = lst.Where(b => b.ToProcessId == 1).FirstOrDefault().ReceiveDate;
                             receivedate = lst.Where(b => b.ToProcessId == 1).FirstOrDefault().ReceiveDate;
                             if (issuedate != null)
                             {
                                 issue_date = issuedate.Value.ToShortDateString();
                             }
                           //  receivedate = lst.Where(b => b.ToProcessId == toproid).FirstOrDefault().ReceiveDate;
                             if (receivedate != null)
                             {
                                 receive_date = receivedate.Value.ToShortDateString();
                             }
                             
                             processname = ds.Tables[0].AsEnumerable().Where(a => a.Field<Int32>("PROCESS_NAME_ID") == 1).Select(b => b.Field<string>("PROCESS_NAME")).FirstOrDefault();
                             finalrow[processname] = Convert.ToString(assign_date) + "|" + Convert.ToString(receive_date);
                         }
                        
                         dsfinal.Tables[0].Rows.Add(finalrow);

                     }
                       
                    }
                }

                if (dsfinal != null)
                {

                    JobwiseissueExcelExport_day(dsfinal);
                }
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {

        }


        //DataSet ds = new DataSet();
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        //try
        //{
        //    SqlParameter[] array = new SqlParameter[10];

        //    array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
        //    array[1] = new SqlParameter("@Fromdate", SqlDbType.SmallDateTime);
        //    array[2] = new SqlParameter("@Todate", SqlDbType.SmallDateTime);
        //    array[3] = new SqlParameter("@Companyid", SqlDbType.Int);
        //    array[4] = new SqlParameter("@UserId", SqlDbType.Int);
        //    array[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        //    array[6] = new SqlParameter("@UnitsId", SqlDbType.Int);
        //    array[7] = new SqlParameter("@Where", str);
        //    array[8] = new SqlParameter("@excelexport", chkexport.Checked == true ? "1" : "0");
        //    array[9] = new SqlParameter("@WITHSTOCKDETAIL", chkwithstockdetail.Checked == true ? "1" : "0");


        //    array[0].Value = DDjob.SelectedIndex <= 0 ? "0" : DDjob.SelectedValue;
        //    array[1].Value = txtFromdate.Text;
        //    array[2].Value = txtToDate.Text;
        //    array[3].Value = 1;
        //    array[4].Value = Session["varuserId"];
        //    array[5].Value = Session["varcompanyId"];
        //    array[6].Value = DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue;

        //    ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_ForProduction_JobSummaryOther", array);

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (chkexport.Checked == true && chkwithstockdetail.Checked == false)
        //        {
        //            JobwiseissueExcelExport(ds, FilterBy);
        //            return;
        //        }
        //        else if (chkexport.Checked == true && chkwithstockdetail.Checked == true)
        //        {
        //            switch (Session["VarCompanyId"].ToString())
        //            {
        //                case "22":
        //                    JobwiseissueExcelExportDiamond_WithstockDetail(ds, FilterBy);                           
        //                    break;                            
        //                default :
        //                    JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);                            
        //                    break;

        //            }

        //            //JobwiseissueExcelExport_WithstockDetail(ds, FilterBy);
        //            //return;
        //        }
        //        else
        //        {
        //            Session["dsFileName"] = "~\\ReportSchema\\RptProduction_JobSummary.xsd";
        //            Session["rptFileName"] = "Reports/RptProduction_JobSummaryNew.rpt";
        //            Session["GetDataset"] = ds;
        //            StringBuilder stb = new StringBuilder();
        //            stb.Append("<script>");
        //            stb.Append("window.open('../../ViewReport.aspx?Export=Y', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //    }

        //}
        //catch (Exception)
        //{
        //}
        //finally
        //{
        //    con.Dispose();
        //    con.Close();
        //    ds.Dispose();
        //}

    }
    protected void DDjob_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        switch (Session["VarCompanyId"].ToString())
        {
            case "22":
                if (DDjob.SelectedItem.Text.ToUpper() == "WEAVING")
                {
                    TRDetailWithAllProcess.Visible = true;
                    ChkDetailWithAllProcess.Visible = true;
                    chkforday.Visible = true;
                    if (Session["UserType"].ToString() == "1")
                    {
                        TRForSummaryDetail.Visible = true;
                        TRFORDAY.Visible = true;
                    }
                    else
                    {
                        TRForSummaryDetail.Visible = false;
                        TRFORDAY.Visible = false;
                    }
                    
                }
                else
                {
                    TRDetailWithAllProcess.Visible = false;
                    ChkDetailWithAllProcess.Visible = false;
                    chkforday.Visible = false;
                    if (Session["UserType"].ToString() == "1")
                    {
                        TRForSummaryDetail.Visible = true;
                        TRForFinishingDetail.Visible = true;
                    }
                    else
                    {
                        TRForSummaryDetail.Visible = false;
                        TRForFinishingDetail.Visible = false;
                    }
                }
                break;
            default:
                 TRDetailWithAllProcess.Visible = false;
                    ChkDetailWithAllProcess.Visible = false;
                    TRForSummaryDetail.Visible = false;
                    chkforday.Visible = false;
                    TRFORDAY.Visible = false;
                    if (Session["VarCompanyNo"].ToString() == "21")
                    {
                        if (DDjob.SelectedIndex > 0)
                        {
                            TRForWithoutWeavingProcess.Visible = false;
                        }
                        else
                        {
                            TRForWithoutWeavingProcess.Visible = true;
                        }
                    }
                break;

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
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDShape, Convert.ToInt16(ddItemName.SelectedValue));
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
    protected void ForProcessWiseSummary(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Production_JOB SUMMARY");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:G1").Merge();
            sht.Range("A1:G1").Style.Font.FontSize = 10;
            sht.Range("A1:G1").Style.Font.Bold = true;
            sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:G1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:G1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:G2").Style.Font.FontSize = 10;
            sht.Range("A2:G2").Style.Font.Bold = true;
            sht.Range("F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "UNIT NAME";
            sht.Range("B2").Value = "JOB NAME";
            sht.Range("C2").Value = "ITEM NAME";
            sht.Range("D2").Value = "QUALITY";
            sht.Range("E2").Value = "SIZE";
            sht.Range("F2").Value = "QTY";
            sht.Range("G2").Value = "AREA";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate,challanNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["Job"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["ItemNameNew"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Area"]);

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Totalp
            var sum = sht.Evaluate("SUM(F3:F" + (row - 1) + ")");
            sht.Range("F" + row).SetValue(sum);
            sht.Range("F" + row).Style.Font.Bold = true;

            var Area = sht.Evaluate("SUM(G3:G" + (row - 1) + ")");
            sht.Range("G" + row).SetValue(Area);
            sht.Range("G" + row).Style.Font.Bold = true;

            using (var a = sht.Range("A1:G" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            String Path;
            sht.Columns(1, 18).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwiseissueExcelExport(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Production_JOB SUMMARY");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:Q1").Merge();
            sht.Range("A1:Q1").Style.Font.FontSize = 10;
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:Q1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:Q2").Style.Font.FontSize = 10;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            sht.Range("H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "REC. DATE";
            sht.Range("B2").Value = "CH NO";
            sht.Range("C2").Value = "UNIT NAME";
            sht.Range("D2").Value = "JOB NAME";
            sht.Range("E2").Value = "QUALITY";
            sht.Range("F2").Value = "COLOR";
            sht.Range("G2").Value = "SIZE";
            sht.Range("H2").Value = "QTY";
            sht.Range("I2").Value = "WEIGHT";
            sht.Range("J2").Value = "EMPLOYEE";
            sht.Range("K2").Value = "CHECKED BY";
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(12).Hide();
                sht.Column(13).Hide();
            }
            else
            {
                sht.Range("L2").Value = "RATE";
                sht.Range("M2").Value = "AMOUNT";
            }
            sht.Range("N2").Value = "EWAY BILLNO";
            sht.Range("O2").Value = "HSN CODE";
            sht.Range("P2").Value = "EMP GSTNO";

            if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
            {
                sht.Range("Q2").Value = "PARTY CHALLANNO"; 
            }
            else
            {
                sht.Column(17).Hide();
            }

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate,challanNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Job"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Size"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["WEIGHT"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["checkedby"]);
                if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
                {
                    sht.Column(12).Hide();
                    sht.Column(13).Hide();
                }
                else
                {
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["RATE"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["AMOUNT"]);
                }
                sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["EWayBillNo"]);
                sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["HsnCode"]);
                sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["EmpGstNo"]);

                if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
                {
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);                    
                }
                else
                {
                    sht.Column(17).Hide();
                }
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(H3:H" + (row - 1) + ")");
            sht.Range("H" + row).SetValue(sum);
            sht.Range("H" + row).Style.Font.Bold = true;

            var weight = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            sht.Range("I" + row).SetValue(weight);
            sht.Range("I" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 18).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwiseissueExcelExport_day(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("JOB SUMMARY Day Report");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:Q1").Merge();
            sht.Range("A1:Q1").Style.Font.FontSize = 10;
            sht.Range("A1:Q1").Style.Font.Bold = true;
            sht.Range("A1:Q1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:Q1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") ");

            sht.Range("A2:Q2").Style.Font.FontSize = 12;
            sht.Range("A2:Q2").Style.Font.Bold = true;
            sht.Range("H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Columns.Count > 0)
                    {
                        int c = 1;
                        for (int i = 1; i < ds.Tables[0].Columns.Count; i=i+1)
                        {
                            if (i >= 5)
                            {
                                // sht.Cell(2, (i+1)).Value = ds.Tables[0].Columns[i - 1].ColumnName;
                                sht.Range(2, (c+1), 2, (c + 2)).Value = ds.Tables[0].Columns[i].ColumnName;
                                sht.Range(2, (c+1), 2, (c + 2)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                sht.Range(2, (c+1), 2, (c + 2)).Style.Font.FontSize = 12;
                                sht.Range(2, (c+1), 2, (c + 2)).Style.Font.Bold = true;
                                sht.Range(2, (c+1), 2, (c+2)).Merge();
                                c = c + 2;
                            }
                            else
                            {
                                sht.Cell(2, c).Value = string.Empty;

                                sht.Cell(2, c).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                sht.Cell(2, c).Style.Font.FontSize = 12;
                                sht.Cell(2, c).Style.Font.Bold = true;
                                c++;
                            }
                            sht.Column(i).Width = 25;
                           // c++;
                        }
                        int m=1;
                        for (int i = 1; i < ds.Tables[0].Columns.Count+1;i=i+1)
                        {
                            if (i > 5)
                            {
                                sht.Range(3, (m), 3, (m)).Value = "ISSUE";
                                sht.Range(3, (m+1), 3, (m+1)).Value = "RECEIVE";
                            }
                            
                            else
                            { sht.Cell(3, i).Value = ds.Tables[0].Columns[i - 1].ColumnName; }

                            sht.Cell(3, m).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(3, m).Style.Font.FontSize = 12;
                            sht.Cell(3, m).Style.Font.Bold = true;
                            sht.Cell(3, (m+1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Cell(3, (m+1)).Style.Font.FontSize = 12;
                            sht.Cell(3, (m+1)).Style.Font.Bold = true;
                            //sht.Column(m).Width = 15;
                            sht.Column(i).Width = 15;
                            if (i > 5)
                            {
                                m = m + 2;
                            }
                            else
                            {
                                m = m + 1;
                            }
                        }
                       string issue_Date=string.Empty,receivedate=string.Empty,assigndate=string.Empty;
                       
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            int n = 1;
                            for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                            {
                                string[] alldate = new string[] { };
                                sht.Column(k+1).Width = 15;
                                if (k >=5)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[j].ItemArray[k])))
                                    {
                                        alldate = Convert.ToString(ds.Tables[0].Rows[j].ItemArray[k]).Split('|');

                                    }
                                    if (alldate.Length >0)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(alldate[0])))
                                        {
                                            sht.Range(j + 4, (n), j + 4, (n)).Value = alldate[0].ToString();
                                        }
                                        else
                                        { sht.Range(j + 4, (n), j + 4, (n)).Value = string.Empty; }

                                        if (!string.IsNullOrEmpty(Convert.ToString(alldate[1])))
                                        {
                                            sht.Range(j + 4, (n+1), j + 4, (n+1)).Value = alldate[1].ToString();
                                        }
                                        else
                                        { sht.Range(j + 4, (n+1), j + 4, (n+1)).Value = string.Empty; }
                                      
                                       
                                        //sht.Cell(j + 4, k).Value = alldate[0].ToString();
                                        //sht.Cell(j + 4, (k + 1)).Value = alldate[1].ToString();

                                    }
                                    else
                                    {
                                        sht.Range(j + 4, (n), j + 4, (n)).Value = "";
                                        sht.Range(j + 4, (n+1), j + 4, (n+1)).Value = "";
                                    
                                    }
                                   
                                    n = n + 2;
                                }
                                else
                                {
                                    sht.Cell(j + 4, k + 1).Value = ds.Tables[0].Rows[j].ItemArray[k].ToString();
                                    n = n + 1;
                                }
                                sht.Cell(j + 4, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            }
                        }


                    }
                }
            }

            //if (ds != null)
            //{
            //    if (ds.Tables.Count > 0)
            //    {

            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            int min = 0, max = ds.Tables[0].Rows.Count;
            //           for(int i=min;i<=max;i++)
            //            {
                           
            //                sht.Range("D2").Value = "SIZE";


            //            }
            //        }
            //    }
            //}
           
            ds.Dispose();
           
            //************** Save
            String Path;
           
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void JobwiseissueExcelExport_WithstockDetail(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Production_JOB SUMMARY");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:X1").Merge();
            sht.Range("A1:X1").Style.Font.FontSize = 10;
            sht.Range("A1:X1").Style.Font.Bold = true;
            sht.Range("A1:X1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:X1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:X1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:X2").Style.Font.FontSize = 10;
            sht.Range("A2:X2").Style.Font.Bold = true;
            sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "REC. DATE";
            sht.Range("B2").Value = "CH NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "UNIT NAME";
            sht.Range("E2").Value = "JOB NAME";
            sht.Range("F2").Value = "QUALITY";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "WIDTH";
            sht.Range("I2").Value = "LENGTH";
            sht.Range("J2").Value = "QTY";
            sht.Range("K2").Value = "AREA";
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(11).Hide();
                sht.Column(12).Hide();
                sht.Column(13).Hide();
            }
            else
            {
                sht.Range("L2").Value = "WEIGHT";
                sht.Range("M2").Value = "EMPLOYEE";
                sht.Range("N2").Value = "CHECKED BY";
            }
            sht.Range("O2").Value = "USER NAME";
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(16).Hide();
                sht.Column(17).Hide();
            }
            else
            {
                sht.Range("P2").Value = "RATE";
                sht.Range("Q2").Value = "AMOUNT";
            }

            sht.Range("R2").Value = "DEFECTS";
            if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
            {
                sht.Column(19).Hide();
            }
            else
            {
                sht.Range("S2").Value = "STOCK STATUS";
            }
            sht.Range("T2").Value = "REMOVE DEFECTS";
            sht.Range("U2").Value = "INSPECTED BY";
            sht.Range("V2").Value = "INSPECTION DATE";

            if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
            {
                sht.Range("W2").Value = "PARTY CHALLANNO";
            }
            else
            {
                sht.Column(23).Hide();                
            }
            if (Session["varCompanyId"].ToString() == "21")
            {
                sht.Range("X2").Value = "QC COMMENT";
            }
            else
            {
                sht.Column(24).Hide();
            }

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate,challanNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["TSTOCKNO"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Job"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["WIDTH"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["LENGTH"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["AREA"]);
                if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
                {
                    sht.Column(11).Hide();
                    sht.Column(12).Hide();
                    sht.Column(13).Hide();

                    sht.Range("L" + row).SetValue("");
                    sht.Range("M" + row).SetValue("");
                    sht.Range("N" + row).SetValue("");
                }
                else
                {
                    sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["WEIGHT"]);
                    sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                    sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["checkedby"]);
                }
                sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
                if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
                {
                    sht.Range("P" + row).SetValue("");
                    sht.Range("Q" + row).SetValue("");
                }
                else
                {
                    sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["RATE"]);
                    sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
                }
                sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
                if (Session["varCompanyId"].ToString() == "21" && Session["usertype"].ToString() != "1")
                {
                    sht.Range("S" + row).SetValue("");
                }
                else
                {
                    sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["StockStatus"]);
                }
                sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
                sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);
                sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);

                if (Session["varCompanyId"].ToString() == "16" || Session["varCompanyId"].ToString() == "28")
                {
                    sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["PartyChallanNo"]);
                                    
                }
                else
                {
                    sht.Range("W" + row).SetValue("");                      
                }

                sht.Range("X" + row).SetValue("");

                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
            sht.Range("J" + row).SetValue(sum);
            sht.Range("J" + row).Style.Font.Bold = true;

            var weight = sht.Evaluate("SUM(L3:L" + (row - 1) + ")");
            sht.Range("L" + row).SetValue(weight);
            sht.Range("L" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 26).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }

    protected void JobwiseissueExcelExportDiamond_WithstockDetail(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Production_JOB SUMMARY");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:AA1").Merge();
            sht.Range("A1:AA1").Style.Font.FontSize = 10;
            sht.Range("A1:AA1").Style.Font.Bold = true;
            sht.Range("A1:AA1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:AA1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:AA1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:AG2").Style.Font.FontSize = 10;
            sht.Range("A2:AG2").Style.Font.Bold = true;
            sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "REC. DATE";
            sht.Range("B2").Value = "CH NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "UNIT NAME";
            sht.Range("E2").Value = "JOB NAME";
            sht.Range("F2").Value = "QUALITY";
            sht.Range("G2").Value = "COLOR";
            sht.Range("H2").Value = "WIDTH";
            sht.Range("I2").Value = "LENGTH";
            sht.Range("J2").Value = "QTY";
            sht.Range("K2").Value = "AREA";
            sht.Range("L2").Value = "WEIGHT";
            sht.Range("M2").Value = "EMPLOYEE";
            sht.Range("N2").Value = "CHECKED BY";
            sht.Range("O2").Value = "USER NAME";
            sht.Range("P2").Value = "RATE";
            sht.Column(16).Hide();
            sht.Range("Q2").Value = "AMOUNT";
            sht.Column(17).Hide();
            sht.Range("R2").Value = "DEFECTS";
            sht.Range("S2").Value = "STOCK STATUS";
            sht.Range("T2").Value = "REMOVE DEFECTS";
            sht.Range("U2").Value = "REMOVE BY";
            sht.Range("V2").Value = "REMOVE DATE";
            sht.Range("W2").Value = "COTTON LOTNO";
            sht.Range("X2").Value = "WEIGHT";
            sht.Range("Y2").Value = "DATE STAMP";
            sht.Range("Z2").Value = "ULL NO";
            sht.Range("AA2").Value = "PENALITY REMARKS";
            sht.Range("AB2").Value = "LOOM NO";
            sht.Range("AC2").Value = "COTTON MOISTURE(%)";
            sht.Range("AD2").Value = "WOOL MOISTURE(%)";
            sht.Range("AE2").Value = "ECISNO";
            sht.Range("AF2").Value = "FOLIO NO";
            sht.Range("AG2").Value = "STOCKNO REMARK";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate,challanNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["TSTOCKNO"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["Job"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"]);
                sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["WIDTH"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["LENGTH"]);
                sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);
                sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["AREA"]);
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["WEIGHT"]);
                sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["checkedby"]);
                sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
                //sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["RATE"]);
                //sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
                sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
                sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["StockStatus"]);
                sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
                sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);
                sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);
                sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["TanaLotNo"]);
                sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueWeight"]);
                sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueDateStamp"]);
                sht.Range("Z" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueULLNo"]);
                sht.Range("AA" + row).SetValue(ds1.Tables[0].Rows[i]["PRemarks"]);
                sht.Range("AB" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("AC" + row).SetValue(ds1.Tables[0].Rows[i]["CottonMoisture"]);
                sht.Range("AD" + row).SetValue(ds1.Tables[0].Rows[i]["WoolMoisture"]);
                sht.Range("AE" + row).SetValue(ds1.Tables[0].Rows[i]["ECISNO"]);
                sht.Range("AF" + row).SetValue(ds1.Tables[0].Rows[i]["BazaarFolioNo"]);
                sht.Range("AG" + row).SetValue(ds1.Tables[0].Rows[i]["StockNoRemark"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(J3:J" + (row - 1) + ")");
            sht.Range("J" + row).SetValue(sum);
            sht.Range("J" + row).Style.Font.Bold = true;

            var weight = sht.Evaluate("SUM(L3:L" + (row - 1) + ")");
            sht.Range("L" + row).SetValue(weight);
            sht.Range("L" + row).Style.Font.Bold = true;
            //************** Save
            String Path;
            sht.Columns(1, 34).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    protected void ShowReportWithAllProcessSize()
    {
        string str = "";
        string FilterBy = "";
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and vf.item_id=" + ddItemName.SelectedValue;
            FilterBy = FilterBy + " ITEMNAME-" + ddItemName.SelectedItem.Text;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
            FilterBy = FilterBy + " QUALITY-" + DDQuality.SelectedItem.Text;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
            FilterBy = FilterBy + " DESIGN-" + DDDesign.SelectedItem.Text;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
            FilterBy = FilterBy + " COLOR-" + DDColor.SelectedItem.Text;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDShape.SelectedValue;
            FilterBy = FilterBy + " SHAPE-" + DDShape.SelectedItem.Text;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
            FilterBy = FilterBy + " SIZE-" + DDSize.SelectedItem.Text;
        }
        //**********************************
        try
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_FORPRODUCTION_JOBDETAIL_WITHPROCESSSIZE_DIAMOND", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@ProcessId", "1");
            cmd.Parameters.AddWithValue("@Fromdate", txtFromdate.Text);
            cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
            cmd.Parameters.AddWithValue("@Companyid", Session["CurrentWorkingCompanyID"]);
            cmd.Parameters.AddWithValue("@UserId", Session["varuserId"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);
            cmd.Parameters.AddWithValue("@UnitsId", DDUnitName.SelectedIndex <= 0 ? "0" : DDUnitName.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", str);
            cmd.Parameters.AddWithValue("@DetailWithAllProcess", ChkDetailWithAllProcess.Checked == true ? "1" : "0");           


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Remove("StockNo");
                ds.Tables[0].Columns.Remove("IssueOrderId");
                ds.Tables[0].Columns.Remove("Area");
                ds.Tables[0].Columns.Remove("UserId");
                ds.Tables[0].Columns.Remove("UnitName");
                ds.Tables[0].Columns.Remove("ReceiveDate");
               

                //Export to excel
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                 "attachment;filename=ProcessSizeTracebility" + DateTime.Now + ".xls");
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception)
        {
        }
        finally
        {

        }
    }

    protected void JobwiseissueExcelExportDiamond_WithstockDetailNewFormat(DataSet ds, string FilterBy)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("Production_JOB SUMMARY");

            //*************
            //***********
            sht.Row(1).Height = 24;
            sht.Range("A1:AA1").Merge();
            sht.Range("A1:AA1").Style.Font.FontSize = 10;
            sht.Range("A1:AA1").Style.Font.Bold = true;
            sht.Range("A1:AA1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:AA1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:AA1").Style.Alignment.WrapText = true;
            //************
            sht.Range("A1").SetValue("PRODUCTION/JOB SUMMARY (From -" + txtFromdate.Text + " To-" + txtToDate.Text + ") " + FilterBy);

            sht.Range("A2:AG2").Style.Font.FontSize = 10;
            sht.Range("A2:AG2").Style.Font.Bold = true;
            sht.Range("J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            sht.Range("A2").Value = "REC. DATE";
            sht.Range("B2").Value = "CH NO";
            sht.Range("C2").Value = "STOCK NO";
            sht.Range("D2").Value = "JOB NAME";
            sht.Range("E2").Value = "QUALITY";
            sht.Range("F2").Value = "COLOR";
            sht.Range("G2").Value = "";
            sht.Range("H2").Value = "";
            sht.Range("I2").Value = "QTY";
           
            sht.Range("L2").Value = "EMPLOYEE";
            sht.Range("M2").Value = "COTTON LOTNO";
            sht.Range("N2").Value = "DATE STAMP";
            sht.Range("O2").Value = "LOOM NO";
            sht.Range("P2").Value = "FOLIO NO";

            sht.Column(7).Hide();
            sht.Column(8).Hide();

            //sht.Range("G2").Value = "WIDTH";
            //sht.Range("H2").Value = "LENGTH";

            if (ChkForFinishingDetail.Checked == true)
            {
                sht.Column(10).Hide();
                sht.Column(11).Hide();
            }
            else
            {
                sht.Range("J2").Value = "AREA";
                sht.Range("K2").Value = "WEIGHT";
            }


            //sht.Range("D2").Value = "UNIT NAME";
           
            //sht.Range("N2").Value = "CHECKED BY";
            //sht.Range("O2").Value = "USER NAME";
           // sht.Range("P2").Value = "RATE";
            //sht.Column(16).Hide();
           // sht.Range("Q2").Value = "AMOUNT";
            //sht.Column(17).Hide();
           // sht.Range("R2").Value = "DEFECTS";
            //sht.Range("S2").Value = "STOCK STATUS";
            //sht.Range("T2").Value = "REMOVE DEFECTS";
            //sht.Range("U2").Value = "REMOVE BY";
            //sht.Range("V2").Value = "REMOVE DATE";
            //sht.Range("W2").Value = "COTTON LOTNO";
            //sht.Range("X2").Value = "WEIGHT";
            //sht.Range("Y2").Value = "DATE STAMP";
            //sht.Range("Z2").Value = "ULL NO";
            //sht.Range("AA2").Value = "PENALITY REMARKS";
            //sht.Range("AB2").Value = "LOOM NO";
            //sht.Range("AC2").Value = "COTTON MOISTURE(%)";
            //sht.Range("AD2").Value = "WOOL MOISTURE(%)";
            //sht.Range("AE2").Value = "ECISNO";
           // sht.Range("AF2").Value = "FOLIO NO";
            //sht.Range("AG2").Value = "STOCKNO REMARK";

            int row = 3;
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "ReceiveDate,challanNo";
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dv.ToTable());
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds1.Tables[0].Rows[i]["Receivedate"]);
                sht.Range("B" + row).SetValue(ds1.Tables[0].Rows[i]["ChallanNo"]);
                sht.Range("C" + row).SetValue(ds1.Tables[0].Rows[i]["TSTOCKNO"]);
                sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Job"]);
                sht.Range("E" + row).SetValue(ds1.Tables[0].Rows[i]["ItemName"]);
                sht.Range("F" + row).SetValue(ds1.Tables[0].Rows[i]["Colorname"]);
                sht.Range("G" + row).SetValue("");
                sht.Range("H" + row).SetValue("");

                //sht.Range("G" + row).SetValue(ds1.Tables[0].Rows[i]["WIDTH"]);
                //sht.Range("H" + row).SetValue(ds1.Tables[0].Rows[i]["LENGTH"]);
                sht.Range("I" + row).SetValue(ds1.Tables[0].Rows[i]["Recqty"]);

                if (ChkForFinishingDetail.Checked == true)
                {
                    sht.Range("J" + row).SetValue("");
                    sht.Range("K" + row).SetValue("");
                }
                else
                {
                    sht.Range("J" + row).SetValue(ds1.Tables[0].Rows[i]["AREA"]);
                    sht.Range("K" + row).SetValue(ds1.Tables[0].Rows[i]["WEIGHT"]);
                }
               
                sht.Range("L" + row).SetValue(ds1.Tables[0].Rows[i]["Employee"]);
                sht.Range("M" + row).SetValue(ds1.Tables[0].Rows[i]["TanaLotNo"]);
                sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueDateStamp"]);
                sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["BazaarFolioNo"]);


                //sht.Range("D" + row).SetValue(ds1.Tables[0].Rows[i]["Unitname"]);               
                //sht.Range("N" + row).SetValue(ds1.Tables[0].Rows[i]["checkedby"]);
                //sht.Range("O" + row).SetValue(ds1.Tables[0].Rows[i]["username"]);
                //sht.Range("P" + row).SetValue(ds1.Tables[0].Rows[i]["RATE"]);
                //sht.Range("Q" + row).SetValue(ds1.Tables[0].Rows[i]["Amount"]);
                //sht.Range("R" + row).SetValue(ds1.Tables[0].Rows[i]["Defect"]);
                //sht.Range("S" + row).SetValue(ds1.Tables[0].Rows[i]["StockStatus"]);
                //sht.Range("T" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemoveVALUE"]);
                //sht.Range("U" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_UserID"]);
                //sht.Range("V" + row).SetValue(ds1.Tables[0].Rows[i]["QCRemove_Date"]);
                //sht.Range("W" + row).SetValue(ds1.Tables[0].Rows[i]["TanaLotNo"]);
                //sht.Range("X" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueWeight"]);
                //sht.Range("Y" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueDateStamp"]);
                //sht.Range("Z" + row).SetValue(ds1.Tables[0].Rows[i]["JobIssueULLNo"]);
                //sht.Range("AA" + row).SetValue(ds1.Tables[0].Rows[i]["PRemarks"]);
                //sht.Range("AB" + row).SetValue(ds1.Tables[0].Rows[i]["LoomNo"]);
                //sht.Range("AC" + row).SetValue(ds1.Tables[0].Rows[i]["CottonMoisture"]);
                //sht.Range("AD" + row).SetValue(ds1.Tables[0].Rows[i]["WoolMoisture"]);
                //sht.Range("AE" + row).SetValue(ds1.Tables[0].Rows[i]["ECISNO"]);
                //sht.Range("AF" + row).SetValue(ds1.Tables[0].Rows[i]["BazaarFolioNo"]);
                //sht.Range("AG" + row).SetValue(ds1.Tables[0].Rows[i]["StockNoRemark"]);
                row = row + 1;
            }
            ds.Dispose();
            ds1.Dispose();
            //**************grand Total
            var sum = sht.Evaluate("SUM(I3:I" + (row - 1) + ")");
            sht.Range("I" + row).SetValue(sum);
            sht.Range("I" + row).Style.Font.Bold = true;

            if (ChkForFinishingDetail.Checked == true)
            {                
                sht.Range("K" + row).SetValue("");
                sht.Range("K" + row).Style.Font.Bold = true;
            }
            else
            {
                var weight = sht.Evaluate("SUM(K3:K" + (row - 1) + ")");
                sht.Range("K" + row).SetValue(weight);
                sht.Range("K" + row).Style.Font.Bold = true;
            }

         
            //************** Save
            String Path;
            sht.Columns(1, 34).AdjustToContents();
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("PRODUCTION_JOB SUMMARY_NEW_" + DateTime.Now + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JobPendalt", "alert('No records found..')", true);
        }
    }
    class stockdata
    {
        public Int32? stockno { get; set; }
        public string Tstockno { get; set; }
        public Int32? FromProcessId { get; set; }
        public Int32? ToProcessId { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? Rec_Date { get; set; }
        public string item_name { get; set; }
        public string QualityName { get; set; }
        public string DesignName { get; set; }
        public string ColorName { get; set; }
        public string SizeFt { get; set; }
    
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "Select OrderID, CustomerOrderNo From OrderMaster(Nolock) Where CustomerID = " + DDCustomerOrderNo.SelectedValue + " Order By CustomerOrderNo ", true, "Select");
    }
}

