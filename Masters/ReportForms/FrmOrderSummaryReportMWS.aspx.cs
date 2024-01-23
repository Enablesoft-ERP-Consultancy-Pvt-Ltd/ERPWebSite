using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmOrderSummaryReportMWS : System.Web.UI.Page
{
    static int masterunitid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           select CustomerId,CustomerCode+'/'+CompanyName from Customerinfo Where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by Customercode select isnull(masterunitid,0) as masterunitid from mastersetting";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDcustomer, ds, 1, true, "--Plz Select Customer--");
            
            if (ds != null)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    masterunitid = Convert.ToInt32(ds.Tables[2].Rows[0]["masterunitid"]);

                }

            }
            ds.Dispose();
        }
    }
    protected DataSet BindGrid()
    {
        DataSet ds = new DataSet();
        lblMessage.Text = "";
        string str = string.Empty;
        try
        {
            //SqlParameter[] param = new SqlParameter[2];
            //param[0] = new SqlParameter("@orderid", DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0");
            //param[1] = new SqlParameter("@customerid", DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0");
            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ordersummary", param);
            //if (ChkselectDate.Checked == true)
            //{
            //    //str = str + " and PIM.Assigndate>='" + txtfromDate.Text + "' and PIM.AssignDate<='" + txttodate.Text + "'";                
            //    str = str + " and PRM.Receivedate>='" + txtfromDate.Text + "' and PRM.Receivedate<='" + txttodate.Text + "'";
            //   // FilterBy = FilterBy + ", From -" + txtfromDate.Text + " To - " + txttodate.Text;
            //}
            string SP = string.Empty;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            //if (Session["varMasterCompanyIDForERP"].ToString() == "45")
            //{
                SP = "Pro_ordersummary_new";
            //}
            //else
            //{
            //    SP = "Pro_ordersummary";
            //}
            SqlCommand cmd = new SqlCommand(SP , con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@orderid", DDOrder.SelectedIndex > 0 ? DDOrder.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@customerid", DDcustomer.SelectedIndex > 0 ? DDcustomer.SelectedValue : "0");
            if (ChkselectDate.Checked)
            {
                cmd.Parameters.AddWithValue("@FROMDATE", Convert.ToDateTime(txtfromDate.Text));
                cmd.Parameters.AddWithValue("@TODATE", Convert.ToDateTime(txttodate.Text));
            }
            else
            {
                cmd.Parameters.AddWithValue("@FROMDATE", txtfromDate.Text);
                cmd.Parameters.AddWithValue("@TODATE", txttodate.Text);
            
            }
            cmd.Parameters.AddWithValue("@ISDATECHECKED", ChkselectDate.Checked?1:0);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            //*************
            ds.Tables.Add(dt);
            ViewState["ds"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                TDExport.Visible = true;
            }
            else
            {
                TDExport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }

        return ds;
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        GVDetails.DataSource = BindGrid();
        GVDetails.DataBind();
    }
    protected void DDcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrder, "select OM.Orderid,OM.LocalOrder+'#'+OM.customerorderno as OrderNo from orderMaster OM Where OM.CustomerId=" + DDcustomer.SelectedValue + " And OM.CompanyId=" + DDCompany.SelectedValue + " and OM.Status=0 order by Om.orderid", true, "--Plz Select Order No--");
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
     
        if (Session["varMasterCompanyIDForERP"].ToString() == "45")
        {
            DataSet ds = new DataSet();
            if (ViewState["ds"] != null)
            {

                ds = (DataSet)ViewState["ds"];
            
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/TempExcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/TempExcel/"));
                }
                //*********************
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("OrderSummaryReport");

                //****************
                //*************
                sht.Range("B2:S2").Merge();
                sht.Range("B2:S2").Style.Font.FontSize = 11;
                sht.Range("B2:S2").Style.Font.Bold = true;
                sht.Range("B2:S2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("B2:S2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("B2").SetValue(ds.Tables[0].Rows[0]["companyname"] +"Order Summary Report" + txtfromDate.Text + "--" + txttodate.Text + "");
                sht.Row(1).Height = 21.75;
                //sht.Range("A2:L2").Merge();
                //sht.Range("A2:L2").Style.Font.FontSize = 11;
                //sht.Range("A2:L2").Style.Font.Bold = true;
                //sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //sht.Range("A2").SetValue(DDcompany.SelectedItem.Text);
                ////*********
                //sht.Range("A3:L3").Style.Font.FontSize = 11;
                //sht.Range("A3:L3").Style.Font.Bold = true;
                //sht.Range("A3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                //sht.Range("B3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Row(3).Height = 18.00;

                //if (DDGateTypeInOut.SelectedValue == "1")
                //{
                //    sht.Range("A3").SetValue("GateInDate");
                //    sht.Range("G3").SetValue("Challan No");
                //    sht.Range("K3").SetValue("InTime");
                //}
                //else if (DDGateTypeInOut.SelectedValue == "2")
                //{
                //    sht.Range("A3").SetValue("GateOutDate");
                //    sht.Range("G3").SetValue("GPNo");
                //    sht.Range("K3").SetValue("OutTime");
                //}
                if (masterunitid == 1)
                {
                   sht.Range("B3").SetValue("FOLIO NO.");
                }
                else
                {
                    sht.Range("B3").SetValue("BUYER ORDER NO.");
                }
              
                sht.Range("C3").SetValue("LOCALORDER");
                sht.Range("D3").SetValue("ORDERDATE");
                sht.Range("E3").SetValue("DISPATCHDATE");
                sht.Range("F3").SetValue("PRODUCTIONUNIT");

                sht.Range("G3").SetValue("QUALITYNAME");
                sht.Range("H3").SetValue("DESIGNNAME");
                sht.Range("I3").SetValue("COLORNAME");
                sht.Range("J3").SetValue("SIZE");
                sht.Range("K3").SetValue("TOTALPCS");
                sht.Range("L3").SetValue("TOTALAREA");
                sht.Range("M3").SetValue("ISSUE");
               
                sht.Range("N3").SetValue("ONLOOM");
                sht.Range("O3").SetValue("OFFLOOM");
                if (masterunitid == 1)
                {
                    sht.Range("P3").SetValue("OUT");
                    //sht.Range("Q3").SetValue("FINISHED");
                    //sht.Range("R3").SetValue("PACKED");
                    //sht.Range("S3").SetValue("STOCKOUT");
                   
                }
                else
                {

                    sht.Range("P3").SetValue("UNDERFINISHING");
                    sht.Range("Q3").SetValue("FINISHED");
                    sht.Range("R3").SetValue("PACKED");
                    sht.Range("S3").SetValue("STOCKOUT");
                  
                
                }
                
                using (var a = sht.Range("A3:S3"))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //**************************************************

                int Row = 4;
                int rowcount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    sht.Range("B" + Row + ":S" + Row).Style.Font.FontSize = 11;

                    sht.Range("B" + Row).SetValue(ds.Tables[0].Rows[i]["CUSTOMERORDERNO"]);
                    sht.Range("C" + Row).SetValue(ds.Tables[0].Rows[i]["LOCALORDER"]);
                    sht.Range("D" + Row).SetValue(ds.Tables[0].Rows[i]["ORDERDATE"]);
                    sht.Range("E" + Row).SetValue(ds.Tables[0].Rows[i]["DISPATCHDATE"]);
                    sht.Range("F" + Row).SetValue(ds.Tables[0].Rows[i]["PRODUCTIONUNIT"]);
                    sht.Range("G" + Row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);

                    sht.Range("H" + Row).SetValue(ds.Tables[0].Rows[i]["DESIGNNAME"]);
                    sht.Range("I" + Row).SetValue(ds.Tables[0].Rows[i]["COLORNAME"]);
                    sht.Range("J" + Row).SetValue(ds.Tables[0].Rows[i]["SIZE"]);
                    sht.Range("K" + Row).SetValue(ds.Tables[0].Rows[i]["TOTALPCS"]);
                    sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["TOTALAREA"]);
                    sht.Range("M" + Row).SetValue(ds.Tables[0].Rows[i]["TOBEISSUED"]);
                    sht.Range("N" + Row).SetValue(ds.Tables[0].Rows[i]["ONLOOM"]);
                    sht.Range("O" + Row).SetValue(ds.Tables[0].Rows[i]["OFFLOOM"]);
                    if (masterunitid == 1)
                    {
                        sht.Range("P" + Row).SetValue(ds.Tables[0].Rows[i]["OUTCOUNT"]);
                        //sht.Range("Q" + Row).SetValue(ds.Tables[0].Rows[i]["FINISHED"]);
                        //sht.Range("R" + Row).SetValue(ds.Tables[0].Rows[i]["PACKED"]);
                        //sht.Range("S" + Row).SetValue("0");
                    }
                    else {
                        sht.Range("P" + Row).SetValue(ds.Tables[0].Rows[i]["UNDERFINISHING"]);
                        sht.Range("Q" + Row).SetValue(ds.Tables[0].Rows[i]["FINISHED"]);
                        sht.Range("R" + Row).SetValue(ds.Tables[0].Rows[i]["PACKED"]);
                        sht.Range("S" + Row).SetValue("0");
                    
                    
                    }

                   // sht.Range("L" + Row).SetValue(ds.Tables[0].Rows[i]["PACKED"]);

                    using (var a = sht.Range("A" + Row + ":S" + Row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }

                    Row = Row + 1;
                }
                //**********Total
                //var TotalQty = sht.Evaluate("SUM(D4:D" + (Row - 1) + ")");
                ////*************
                //sht.Columns(1, 13).AdjustToContents();
                //sht.Range("C" + Row).SetValue("Total");
                //sht.Range("C" + Row + ":D" + Row).Style.Font.Bold = true;
                //sht.Range("D" + Row).SetValue(TotalQty);

                using (var a = sht.Range("A" + Row + ":S" + Row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //**************Save
                //******SAVE FILE
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("StockSummaryReport-" + DateTime.Now + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('No records found')", true);
            }


        }
        else
        {


            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "ORDERSUMMARYREPORT_" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GridView gv = new GridView();
            DataSet ds = BindGrid();
            //*************** 
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["orderid"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["Item_finished_id"]);
            ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["unit"]);
            ds.Tables[0].Columns["customerorderNo"].ColumnName = "Buyer Order No.";

            if (Session["VarCompanyNo"].ToString() != "39")
            {
                ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["BalanceQty"]);
                ds.Tables[0].Columns.Remove(ds.Tables[0].Columns["DISPATCHED"]);
            }

            if (Session["VarCompanyNo"].ToString() == "39")
            {
                ds.Tables[0].Columns["OFFLOOM"].ColumnName = "RECEIVED";
            }

            //***************
            gv.DataSource = ds;
            gv.DataBind();


            gv.GridLines = GridLines.Both;

            gv.HeaderStyle.Font.Bold = true;
            gv.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void lnkunderfinishing_Click(object sender, EventArgs e)
    {
        string fruitName = ((sender as LinkButton).NamingContainer as GridViewRow).Cells[0].Text;

        lblMessage.Text = "";
        LinkButton lnk = sender as LinkButton;

        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblorderid = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblorderid");
            Label lblitemfinishedid = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblitemfinishedid");
            Label lblorderNo = (Label)GVDetails.Rows[grv.RowIndex].FindControl("lblorderNo");
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@orderid", lblorderid.Text);
                param[1] = new SqlParameter("@item_finished_id", lblitemfinishedid.Text);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderUnderFinishing", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    string FileName = "ORDER_UNDERFINISHING_" + DateTime.Now + ".xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    GridView gv1 = new GridView();
                    gv1.DataSource = ds;
                    gv1.DataBind();


                    gv1.GridLines = GridLines.Both;
                    gv1.HeaderStyle.Font.Bold = true;
                    gv1.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = ex.Message;
            }
        }
    }
    protected void GVDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkFull = (LinkButton)e.Row.FindControl("lblUF");
            ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            sm.RegisterPostBackControl(lnkFull);

            for (int i = 0; i < GVDetails.Columns.Count; i++)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "39")
                {
                    if (GVDetails.Columns[i].HeaderText == "BALANCE" || GVDetails.Columns[i].HeaderText == "DISPATCHED")
                    {
                        GVDetails.Columns[i].Visible = true;
                    }
                }
                if (Session["varMasterCompanyIDForERP"].ToString() == "45" )
                {
                    if (GVDetails.Columns[i].HeaderText == "STOCKOUT" || GVDetails.Columns[i].HeaderText == "PRODUCTION UNIT")
                    {
                        GVDetails.Columns[i].Visible = true;
                    }
                    if (GVDetails.Columns[i].HeaderText == "UNDER FINISHING" || GVDetails.Columns[i].HeaderText == "FINISHED" || GVDetails.Columns[i].HeaderText == "PACKED" || GVDetails.Columns[i].HeaderText == "STOCKOUT")
                    {
                        if (masterunitid == 1)
                        {
                            GVDetails.Columns[i].Visible = false;
                        }
                    }
                    else if (GVDetails.Columns[i].HeaderText == "OUT")
                    {
                        if (masterunitid == 1)
                        {
                            GVDetails.Columns[i].Visible = true;
                        }
                    }
                }
                else
                {
                    if (GVDetails.Columns[i].HeaderText == "BALANCE" || GVDetails.Columns[i].HeaderText == "DISPATCHED")
                    {
                        GVDetails.Columns[i].Visible = false;
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Session["VarCompanyNo"].ToString() == "39")
            {
                e.Row.Cells[12].Text = "RECEIVED";
                //GVDetails.Columns[15].HeaderText = "DISPATCHED";
            }
            if (Session["VarCompanyNo"].ToString() == "45")
            {
                if (masterunitid == 1)
                {
                    e.Row.Cells[0].Text = "FOLIO NO.";
                }
                e.Row.Cells[11].Text = "ISSUE";
                //GVDetails.Columns[15].HeaderText = "DISPATCHED";
            }
        }
    }
}