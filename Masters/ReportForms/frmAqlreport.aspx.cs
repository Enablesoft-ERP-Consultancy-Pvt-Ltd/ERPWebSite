using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
public partial class Masters_ReportForms_frmAqlreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.COmpanyid And CA.Userid=" + Session["varuserid"] + @"
                           SELECT PROCESS_NAME_ID,PROCESS_NAME FROM  PROCESS_NAME_MASTER PNM WHERE PROCESS_NAME LIKE 'AQL%' ORDER BY PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocessname, ds, 1, true, "--Plz Select Process--");
            ds.Dispose();
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        DataSet ds = Bindgrid();
        DGDetail.DataSource = ds;
        DGDetail.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            TDexportexcel.Visible = true;
        }
        else
        {
            TDexportexcel.Visible = false;
        }
    }
    protected DataSet Bindgrid()
    {
        lblmsg.Text = "";
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {

            SqlCommand cmd = new SqlCommand("PRO_AQLREPORTDATA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyId", DDCompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Processid", DDprocessname.SelectedValue);
            cmd.Parameters.AddWithValue("@fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@TOdate", txttodate.Text);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;

    }
    protected void btnexporttoexcel_Click(object sender, EventArgs e)
    {
        DataSet ds = Bindgrid();
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
            sht.Range("A1").SetValue(DDprocessname.SelectedItem.Text + " REPORT");
            sht.Range("A1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:H2").Merge();
            sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
            sht.Range("A2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:H2").Style.Font.SetBold();
            //Headers
            sht.Range("A3").Value = "DATE";
            sht.Range("B3").Value = "BATCH NO.";
            sht.Range("C3").Value = "TOTAL PCS";
            sht.Range("D3").Value = "SAMPLE PCS";
            sht.Range("E3").Value = "FAIL PCS";
            sht.Range("F3").Value = "AQL DONE BY";
            sht.Range("G3").Value = "RESULT";
            sht.Range("H3").Value = "DESCRIPTION";

            sht.Range("C3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:H3").Style.Font.Bold = true;

            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Aqldate"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Aqllotno"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["TOtalpcs"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["samplepcs"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["failpcs"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Aqlstatus"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Description"]);

                row = row + 1;

            }
            sht.Range("C" + row).Value = sht.Evaluate("SUM(C2:C" + (row - 1) + ")");
            sht.Range("D" + row).Value = sht.Evaluate("SUM(D2:D" + (row - 1) + ")");
            sht.Range("E" + row).Value = sht.Evaluate("SUM(E2:E" + (row - 1) + ")");
            using (var a = sht.Range("A3:H" + row))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Columns(1, 20).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("" + DDprocessname.SelectedItem.Text + "  (FROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            lblmsg.Text = "No records found...";
        }
        //************************
    }
    protected void lbltotalpcs_click(object sender, EventArgs e)
    {
        int aqlid = 0;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblaqlid = (Label)DGDetail.Rows[grv.RowIndex].FindControl("lblaqlid");
            aqlid = Convert.ToInt32(lblaqlid.Text);
            Getpcsdetail_TOTALPCS(0, aqlid);
        }


    }
    //lblsamplepcs
    protected void lblsamplepcs_click(object sender, EventArgs e)
    {
        int aqlid = 0;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblaqlid = (Label)DGDetail.Rows[grv.RowIndex].FindControl("lblaqlid");
            aqlid = Convert.ToInt32(lblaqlid.Text);
            Getpcsdetail(1, aqlid);
        }
    }
    protected void lblfailpcs_click(object sender, EventArgs e)
    {
        int aqlid = 0;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label lblaqlid = (Label)DGDetail.Rows[grv.RowIndex].FindControl("lblaqlid");
            aqlid = Convert.ToInt32(lblaqlid.Text);
            Getpcsdetail(2, aqlid);
        }
    }
    protected void Getpcsdetail(int Pcstype, int Aqlid)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Aqlid", Aqlid);
            param[1] = new SqlParameter("@pcstype", Pcstype);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETAQLPCSDETAIL", param);
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
                string pcsstring = "";
                switch (Pcstype)
                {
                    case 0:
                        pcsstring = "TOTAL PCS";
                        break;
                    case 1:
                        pcsstring = "SAMPLE PCS";
                        break;
                    case 2:
                        pcsstring = "FAIL PCS";
                        break;
                    default:
                        break;
                }
                sht.Range("A1:G1").Merge();
                sht.Range("A1").SetValue(DDprocessname.SelectedItem.Text + " REPORT " + pcsstring);
                sht.Range("A1:G1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:G2").Merge();
                sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:G2").Style.Font.SetBold();
                //Headers
                sht.Range("A3").Value = "STOCK NO.";
                sht.Range("B3").Value = "ITEM NAME";
                sht.Range("C3").Value = "QUALITY NAME";
                sht.Range("D3").Value = "DESIGN NAME";
                sht.Range("E3").Value = "COLOR NAME";
                sht.Range("F3").Value = "SHAPE";
                sht.Range("G3").Value = "SIZE";
                sht.Range("H3").Value = "DEFECTS";
                sht.Range("I3").Value = "BATCH NO.";
                sht.Range("A3:I3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_name"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["qualityname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["shapename"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["sizeft"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Defects"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["AqlLotno"]);

                    row = row + 1;

                }
                using (var a = sht.Range("A3:I" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("" + DDprocessname.SelectedItem.Text + "  " + pcsstring + "  (FROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void Getpcsdetail_TOTALPCS(int Pcstype, int Aqlid)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Aqlid", Aqlid);
            param[1] = new SqlParameter("@pcstype", Pcstype);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETAQLPCSDETAIL", param);
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
                string pcsstring = "";
                switch (Pcstype)
                {
                    case 0:
                        pcsstring = "TOTAL PCS";
                        break;
                    case 1:
                        pcsstring = "SAMPLE PCS";
                        break;
                    case 2:
                        pcsstring = "FAIL PCS";
                        break;
                    default:
                        break;
                }
                sht.Range("A1:L1").Merge();
                sht.Range("A1").SetValue(DDprocessname.SelectedItem.Text + " REPORT " + pcsstring);
                sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:L2").Merge();
                sht.Range("A2").SetValue("From :" + txtfromdate.Text + "  To : " + txttodate.Text);
                sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:L2").Style.Font.SetBold();
                //Headers
                sht.Range("A3").Value = "STOCK NO.";
                sht.Range("B3").Value = "ITEM NAME";
                sht.Range("C3").Value = "QUALITY NAME";
                sht.Range("D3").Value = "DESIGN NAME";
                sht.Range("E3").Value = "COLOR NAME";
                sht.Range("F3").Value = "SHAPE";
                sht.Range("G3").Value = "SIZE";
                sht.Range("H3").Value = "SAMPLE PCS";
                sht.Range("I3").Value = "DEFECTS";
                sht.Range("J3").Value = "RESULT";
                sht.Range("K3").Value = "AQL DONE BY";
                sht.Range("L3").Value = "BATCH NO.";

                sht.Range("A3:L3").Style.Font.Bold = true;

                row = 4;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Tstockno"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_name"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["qualityname"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["designname"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["colorname"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["shapename"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["sizeft"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Samplepcs"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Defects"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["Aqlstatus"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["empname"]);
                    sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["AqlLotno"]);

                    row = row + 1;

                }
                using (var a = sht.Range("A3:L" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                sht.Columns(1, 20).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("" + DDprocessname.SelectedItem.Text + "  " + pcsstring + "  (FROM : " + txtfromdate.Text + " TO :" + txttodate.Text + ")" + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No records found.')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbltotalpcs = (LinkButton)e.Row.FindControl("lbltotalpcs");
            LinkButton lblsamplepcs = (LinkButton)e.Row.FindControl("lblsamplepcs");
            LinkButton lblfailpcs = (LinkButton)e.Row.FindControl("lblfailpcs");
            ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            sm.RegisterPostBackControl(lbltotalpcs);
            sm.RegisterPostBackControl(lblsamplepcs);
            sm.RegisterPostBackControl(lblfailpcs);
        }
    }
}
