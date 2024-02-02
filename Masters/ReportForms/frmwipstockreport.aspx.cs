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
using System.Configuration;
using System.Text;


public partial class Masters_ReportForms_frmwipstockreport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                                                      
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=cs.Categoryid and cs.id=0 and ICM.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                           select Val,Type from Sizetype
                           Select CustomerId,CustomerCode From CustomerInfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CustomerCode";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 3, true, "--Select--");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Session["varMasterCompanyIDForERP"])))
            {
                if (Convert.ToString(Session["varMasterCompanyIDForERP"]) == "22")
                {
                    trdiareport.Visible = true;
                
                }
                
            }
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
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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

        string stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by IM.item_name";
        UtilityModule.ConditionalComboFill(ref ddItemName, stritem, true, "---Select Item----");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, int Itemid)
    {
        string Str = @"SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By QUALITYNAME
                     SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By DESIGNNAME
                     SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By COLORNAME
                     SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By SHAPENAME
                     SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";

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
                 Where shapeid=" + DDShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--SELECT--");
        //

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["varMasterCompanyIDForERP"]) == "22")
        {
            if (chkstocksumm.Checked==true)
            {
                WIPStockDetail();
            }
            else if (chkprocsumm.Checked==true)
            {
                WIPProcesWiseDetail();
            }
            else if (chksumm.Checked==true)
            {
                WIPSummaryDetail();
            }
            else if (chkWIPdetail.Checked == true || chkwpidetailwithlotno.Checked == true)
            {
                WIPDetailDiamond();
            }
            else
            {
                WIPsummary();
            }

        }
        else
        {
            if (chkWIPdetail.Checked == true || chkwpidetailwithlotno.Checked == true)
            {
                WIPDetail();
            }
            else
            {
                WIPsummary();
            }
        }



    }
    protected void WIPsummary()
    {
        string where = "";
        lblmsg.Text = "";
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_getWipreport", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@ItemId", ddItemName.SelectedIndex>0?ddItemName.SelectedValue:"0");
        cmd.Parameters.AddWithValue("@Qualityid", DDQuality.SelectedIndex > 0 ? DDQuality.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@DesignId", DDDesign.SelectedIndex > 0 ? DDDesign.SelectedValue : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        con.Close();
        con.Dispose();

        ////SqlParameter[] param = new SqlParameter[2];
        ////param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        ////param[1] = new SqlParameter("@where", where);
        //////*************
        ////DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getWipreport", param);


        ds.Tables[0].Columns.Remove("Finishedid");
        //Export to excel
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;

        GridView1.DataSource = ds;
        GridView1.DataBind();
        lblmsg.Text = "Wait....";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=WIPStock" + DateTime.Now + ".xls");
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
        lblmsg.Text = "Done.....";
        //*************
    }
    protected void WIPDetail()
    {
        string where = "";
        lblmsg.Text = "";
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@where", where);
        //param[2] = new SqlParameter("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");
        ////*************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wipdetails", param);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_Wipdetails", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");     

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Columns.Remove("Stockno");
            ds.Tables[0].Columns.Remove("SeqNo");
            //Export to excel
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=WIPStockDetail" + DateTime.Now + ".xls");
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
            lblmsg.Text = "Done.....";
            //*************    
        }
        else
        {
            lblmsg.Text = "No records found for this combination.";
        }

    }
    protected void WIPStockDetail()
    {
        string where = "";
        lblmsg.Text = "";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            var xapp = new XLWorkbook();

            var sht = xapp.Worksheets.Add("WIP Stock Summary");

            sht.Column("B").Width = 30.89;
            sht.Column("C").Width = 25.89;
            sht.Column("E").Width = 30.89;

            if (DDCategory.SelectedIndex > 0)
            {
                where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                where = where + " and vf.Designid=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                where = where + " and vf.colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                where = where + " and vf.shapeid=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                where = where + " and vf.sizeid=" + DDSize.SelectedValue;
            }

            //SqlParameter[] param = new SqlParameter[3];
            //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            //param[1] = new SqlParameter("@where", where);
            //param[2] = new SqlParameter("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");
            ////*************
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wipdetails", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_Wipdetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Remove("Stockno");
                ds.Tables[0].Columns.Remove("SeqNo");
                //Export to excel
               
                var queryDIM = from t in ds.Tables[0].AsEnumerable()
                               where t.Field<Int32>("qty") > 0
                               group t by new { design = t.Field<string>("design"), color = t.Field<string>("color"), size = t.Field<String>("size"), qty = t.Field<Int32>("qty") }
                                   into res
                                   select new { design = res.Key.design, color = res.Key.color, size = res.Key.size, qty = res.Sum(a => a.Field<Int32>("qty")) };
                sht.Cell("B3").Value = "DESIGN";
                sht.Range("B3:B3").Style.Font.FontName = "Arial";
                sht.Range("B3:B3").Style.Font.FontSize = 12;
                sht.Range("B3:B3").Style.Font.Bold = true;
                sht.Range("B3:B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("C3").Value = "COLOR";
                sht.Range("C3:C3").Style.Font.FontName = "Arial";
                sht.Range("C3:C3").Style.Font.FontSize = 12;
                sht.Range("C3:C3").Style.Font.Bold = true;
                sht.Range("C3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("D3").Value = "SIZE";
                sht.Range("D3:D3").Style.Font.FontName = "Arial";
                sht.Range("D3:D3").Style.Font.FontSize = 12;
                sht.Range("D3:D3").Style.Font.Bold = true;
                sht.Range("D3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("E3").Value = "STOCK PCS";
                sht.Range("E3:E3").Style.Font.FontName = "Arial";
                sht.Range("E3:E3").Style.Font.FontSize = 12;
                sht.Range("E3:E3").Style.Font.Bold = true;
                sht.Range("E3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
              //  sht.Range("B3:B3").Merge();
                //***************

                using (var a = sht.Range("B3:E3"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                int row = 4,total=0;
                foreach (var item in queryDIM)
                {
                    sht.Cell("B"+row).Value = item.design;
                    sht.Range("B"+row+":B"+row).Style.Font.FontName = "Arial";
                    sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                   // sht.Range("B" + row + ":B" + row).Style.Font.Bold = true;
                    sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell("C" + row).Value = item.color;
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 11;
                   // sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell("D" + row).Value = item.size;
                    sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                    sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                    //sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Cell("E" + row).Value = item.qty;
                    sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                    sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                  //  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                    sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    using (var a = sht.Range("B"+row+":E"+row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    total = total + item.qty;
                    row = row + 1;

                }

                row = row + 1;
                sht.Cell("D" + row).Value = "Total";
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                //sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("E" + row).Value = total;
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                //  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var a = sht.Range("B" + row + ":E" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                string Path = "";
                string Pathpdf = "";
                string Fileextension = "xlsx";
                string filename = "WIP_STOCK_SUMMARY." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();

                //*************    
            }
            else
            {
                lblmsg.Text = "No records found for this combination.";
            }
        }
        catch (Exception)
        {

            throw;
        }


    }
    protected void WIPProcesWiseDetail()
    {
        string where = "";
        lblmsg.Text = "";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            var xapp = new XLWorkbook();

            var sht = xapp.Worksheets.Add("WIP Summary");

            sht.Column("B").Width = 30.89;
            sht.Column("C").Width = 25.89;
            sht.Column("E").Width = 30.89;

            if (DDCategory.SelectedIndex > 0)
            {
                where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                where = where + " and vf.Designid=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                where = where + " and vf.colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                where = where + " and vf.shapeid=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                where = where + " and vf.sizeid=" + DDSize.SelectedValue;
            }

            //SqlParameter[] param = new SqlParameter[3];
            //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            //param[1] = new SqlParameter("@where", where);
            //param[2] = new SqlParameter("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");
            ////*************
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wipdetails", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_Wipdetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Remove("Stockno");
                ds.Tables[0].Columns.Remove("SeqNo");
                //Export to excel

                var finalquery  = from t in ds.Tables[0].AsEnumerable()
                               where t.Field<Int32>("qty") > 0
                               group t by new { process = t.Field<string>("jobname"), status = t.Field<string>("status"), qty = t.Field<Int32>("qty") }

                                   into res

                                   select new { process = res.Key.process, status = res.Key.status, qty = res.Sum(a => a.Field<Int32>("qty")) };
                var queryDIM = finalquery.OrderBy(a=>a.process);
                sht.Cell("B3").Value = "PROCESS";
                sht.Range("B3:B3").Style.Font.FontName = "Arial";
                sht.Range("B3:B3").Style.Font.FontSize = 12;
                sht.Range("B3:B3").Style.Font.Bold = true;
                sht.Range("B3:B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("C3").Value = "STATUS";
                sht.Range("C3:C3").Style.Font.FontName = "Arial";
                sht.Range("C3:C3").Style.Font.FontSize = 12;
                sht.Range("C3:C3").Style.Font.Bold = true;
                sht.Range("C3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("D3").Value = "PCS";
                sht.Range("D3:D3").Style.Font.FontName = "Arial";
                sht.Range("D3:D3").Style.Font.FontSize = 12;
                sht.Range("D3:D3").Style.Font.Bold = true;
                sht.Range("D3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                sht.Cell("E3").Value = "TOTAL PCS";
                sht.Range("E3:E3").Style.Font.FontName = "Arial";
                sht.Range("E3:E3").Style.Font.FontSize = 12;
                sht.Range("E3:E3").Style.Font.Bold = true;
                sht.Range("E3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //  sht.Range("B3:B3").Merge();
                //***************

                using (var a = sht.Range("B3:E3"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                int row = 4, total = 0,lastqty=0;
                string lastprocess = string.Empty; ;
                foreach (var item in queryDIM)
                {
                    //if (string.IsNullOrEmpty(lastprocess))
                    //{
                    //    lastprocess = item.process;
                    //    lastqty = item.qty;
                    //}
                    if (item.process == lastprocess)
                    {
                        sht.Cell("B" + row).Value = item.process;
                        sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                        sht.Range("B" + (row-1) + ":B" + (row)).Merge();
                        sht.Range("B" + (row - 1) + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + (row - 1) + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }
                    else {
                        sht.Cell("B" + row).Value = item.process;
                        sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                        sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                       // sht.Range("B" + row + ":B" + (row + 1)).Merge();
                        sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    
                    
                    }
                    sht.Cell("C" + row).Value = item.status;
                    sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                    sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 11;
                    // sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                    sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell("D" + row).Value = item.qty;
                    sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                    sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                    //sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                    sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    if (item.process == lastprocess)
                    {
                      
                        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                        sht.Range("E" + (row-1) + ":E" + row).Merge();
                        sht.Cell("E" + (row-1)).Value = lastqty + item.qty;
                        sht.Range("E" + (row - 1) + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + (row - 1) + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }
                    else {
                        sht.Cell("E" + row).Value = item.qty;
                        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                        //  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("E" + row + ":E" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    
                    }

                    using (var a = sht.Range("B" + (row-1) + ":E" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    total = total + item.qty;
                    if (item.process == lastprocess)
                    {
                        row = row + 1;
                    }
                    else { row = row + 1; }
                    lastprocess = item.process;
                    lastqty = item.qty;
                }

                row = row + 1;
                sht.Cell("D" + row).Value = "Total";
                sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                //sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("E" + row).Value = total;
                sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                //  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var a = sht.Range("B" + row + ":E" + row))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                string Path = "";
                string Pathpdf = "";
                string Fileextension = "xlsx";
                string filename = "WIP_SUMMARY." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();

                //*************    
            }
            else
            {
                lblmsg.Text = "No records found for this combination.";
            }
        }
        catch (Exception)
        {

            throw;
        }


    }

    protected void WIPSummaryDetail()
    {
        string where = "";
        lblmsg.Text = "";
        if (!Directory.Exists(Server.MapPath("~/InvoiceExcel/")))
        {
            Directory.CreateDirectory(Server.MapPath("~/InvoiceExcel/"));
        }

        try
        {
            var xapp = new XLWorkbook();

            var sht = xapp.Worksheets.Add("WIP Process Wise Stock Summary");

            sht.Column("B").Width = 30.89;
            sht.Column("C").Width = 25.89;
            sht.Column("E").Width = 30.89;

            if (DDCategory.SelectedIndex > 0)
            {
                where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
            }
            if (ddItemName.SelectedIndex > 0)
            {
                where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
            }
            if (DDQuality.SelectedIndex > 0)
            {
                where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                where = where + " and vf.Designid=" + DDDesign.SelectedValue;
            }
            if (DDColor.SelectedIndex > 0)
            {
                where = where + " and vf.colorid=" + DDColor.SelectedValue;
            }
            if (DDShape.SelectedIndex > 0)
            {
                where = where + " and vf.shapeid=" + DDShape.SelectedValue;
            }
            if (DDSize.SelectedIndex > 0)
            {
                where = where + " and vf.sizeid=" + DDSize.SelectedValue;
            }

            //SqlParameter[] param = new SqlParameter[3];
            //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
            //param[1] = new SqlParameter("@where", where);
            //param[2] = new SqlParameter("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");
            ////*************
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wipdetails", param);

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_WIPDETAILS_SUMMMARY", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Remove("Stockno");
                ds.Tables[0].Columns.Remove("SeqNo");
                //Export to excel
                var finalheader = from t in ds.Tables[0].AsEnumerable()
                                  where t.Field<Int32>("qty") > 0
                                  group t by new { hcolor = t.Field<string>("color"), hsize = t.Field<string>("size"), design = t.Field<string>("design") , colorid = t.Field<Int32>("colorid") ,sizeid = t.Field<Int32>("sizeid"),designid = t.Field<Int32>("designid")}
                                     into res
                                      select new { hcolor = res.Key.hcolor, hsize = res.Key.hsize, design = res.Key.design ,colorid=res.Key.colorid,sizeid=res.Key.sizeid,designid=res.Key.designid};

                sht.Cell("C3").Value = "COLOR";
                sht.Range("C3:C3").Style.Font.FontName = "Arial";
                sht.Range("C3:C3").Style.Font.FontSize = 12;
                sht.Range("C3:C3").Style.Font.Bold = true;
                sht.Range("C3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("C4").Value = "SIZE";
                sht.Range("C4:C4").Style.Font.FontName = "Arial";
                sht.Range("C4:C4").Style.Font.FontSize = 12;
                sht.Range("C4:C4").Style.Font.Bold = true;
                sht.Range("C4:C4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("C5").Value = "STATUS";
                sht.Range("C5:C5").Style.Font.FontName = "Arial";
                sht.Range("C5:C5").Style.Font.FontSize = 12;
                sht.Range("C5:C5").Style.Font.Bold = true;
                sht.Range("C5:C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Cell("B5").Value = "PROCESS";
                sht.Range("B5:B5").Style.Font.FontName = "Arial";
                sht.Range("B5:B5").Style.Font.FontSize = 12;
                sht.Range("B5:B5").Style.Font.Bold = true;
                sht.Range("B5:B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                using (var a = sht.Range("B3:C5"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                char first = 'C';
               // char nextChar = new char();
                int nextcharindex = 3;
                string nextChar = string.Empty;
                string[] arr=new string[]{};
                arr=GetExcelStrings().ToArray();
                foreach (var head in finalheader)
                {
                    nextChar = arr[nextcharindex];
                    sht.Column(nextChar).Width = 25.89;
                   
                    sht.Cell(nextChar + "3" ).Value = head.hcolor;
                    sht.Range(nextChar + "3:" + nextChar + "3").Style.Font.FontName = "Arial";
                    sht.Range(nextChar + "3:" + nextChar + "3").Style.Font.FontSize = 12;
                    sht.Range(nextChar + "3:" + nextChar + "3").Style.Font.Bold = true;
                    sht.Range(nextChar + "3:" + nextChar + "3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(nextChar + "4").Value = head.hsize;
                    sht.Range(nextChar + "4:" + nextChar + "4").Style.Font.FontName = "Arial";
                    sht.Range(nextChar + "4:" + nextChar + "4").Style.Font.FontSize = 12;
                    sht.Range(nextChar + "4:" + nextChar + "4").Style.Font.Bold = true;
                    sht.Range(nextChar + "4:" + nextChar + "4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Cell(nextChar + "5").Value = head.design;
                    sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.FontName = "Arial";
                    sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.FontSize = 12;
                    sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.Bold = true;
                    sht.Range(nextChar + "5:" + nextChar + "5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    using (var a = sht.Range(nextChar + "3:" + nextChar + "5"))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    nextcharindex++;
                }
                nextChar = arr[nextcharindex];
                sht.Column(nextChar).Width = 25.89;
                sht.Cell(nextChar + "5").Value = "TOTAL";
                sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.FontName = "Arial";
                sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.FontSize = 12;
                sht.Range(nextChar + "5:" + nextChar + "5").Style.Font.Bold = true;
                sht.Range(nextChar + "5:" + nextChar + "5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var a = sht.Range(nextChar + "3:" + nextChar + "5"))
                {
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //  sht.Range("B3:B3").Merge();
                var finalquery = from t in ds.Tables[0].AsEnumerable()
                                 where t.Field<Int32>("qty") > 0
                                 group t by new { process = t.Field<string>("jobname"), status = t.Field<string>("status"), qty = t.Field<Int32>("qty"), colorid = t.Field<Int32>("colorid"), sizeid = t.Field<Int32>("sizeid"), designid = t.Field<Int32>("designid") }
                                     into res
                                     select new { process = res.Key.process, status = res.Key.status, qty = res.Sum(a => a.Field<Int32>("qty")), colorid = res.Key.colorid, sizeid = res.Key.sizeid, designid = res.Key.designid };

                var finalprocess =from  q in finalquery
                                 group q by new { process = q.process, status = q.status }
                                     into res
                                     select new { process = res.Key.process, status = res.Key.status };
                var queryDIM = finalprocess.OrderBy(a => a.process);
              //  var queryDIM = finalquery.OrderBy(a => a.process);
                //sht.Cell("B3").Value = "PROCESS";
                //sht.Range("B3:B3").Style.Font.FontName = "Arial";
                //sht.Range("B3:B3").Style.Font.FontSize = 12;
                //sht.Range("B3:B3").Style.Font.Bold = true;
                //sht.Range("B3:B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Cell("C3").Value = "STATUS";
                //sht.Range("C3:C3").Style.Font.FontName = "Arial";
                //sht.Range("C3:C3").Style.Font.FontSize = 12;
                //sht.Range("C3:C3").Style.Font.Bold = true;
                //sht.Range("C3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Cell("D3").Value = "PCS";
                //sht.Range("D3:D3").Style.Font.FontName = "Arial";
                //sht.Range("D3:D3").Style.Font.FontSize = 12;
                //sht.Range("D3:D3").Style.Font.Bold = true;
                //sht.Range("D3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //sht.Cell("E3").Value = "TOTAL PCS";
                //sht.Range("E3:E3").Style.Font.FontName = "Arial";
                //sht.Range("E3:E3").Style.Font.FontSize = 12;
                //sht.Range("E3:E3").Style.Font.Bold = true;
                //sht.Range("E3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ////  sht.Range("B3:B3").Merge();
                ////***************

                //using (var a = sht.Range("B3:E3"))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
               
               
                int innernextcharindex = 3;
                string innernextChar = string.Empty;
                string[] innerarr = new string[] { };
                innerarr = GetExcelStrings().ToArray();
                foreach (var head in finalheader)
                {
                    int row = 7, total = 0, lastqty = 0, headcount = 0; ;
                    innernextChar = arr[innernextcharindex];
                    string lastprocess = string.Empty,laststatus=string.Empty;
                    foreach (var item in queryDIM)
                    {
                    var innerresult = finalquery.Where(a => a.designid == head.designid && a.colorid == head.colorid && a.sizeid == head.sizeid && a.process==item.process && a.status==item.status ).GroupBy(b => new { b.status }).ToList();
                    //foreach (var item in innerresult)
                    //{

                    if (headcount == 0)
                    {
                        //if (string.IsNullOrEmpty(lastprocess))
                        //{
                        //    lastprocess = item.process;
                        //    // lastqty = innerresult;
                        //}
                        if (item.process == lastprocess)
                        {
                            sht.Cell("B" + row).Value = item.process;
                            sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                            sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                            sht.Range("B" + (row - 1) + ":B" + (row)).Merge();
                            sht.Range("B" + (row - 1) + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("B" + (row - 1) + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        }
                        else
                        {
                            sht.Cell("B" + row).Value = item.process;
                            sht.Range("B" + row + ":B" + row).Style.Font.FontName = "Arial";
                            sht.Range("B" + row + ":B" + row).Style.Font.FontSize = 11;
                            // sht.Range("B" + row + ":B" + (row + 1)).Merge();
                            sht.Range("B" + row + ":B" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            sht.Range("B" + row + ":B" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                        }

                        sht.Cell("C" + row).Value = item.status;
                        sht.Range("C" + row + ":C" + row).Style.Font.FontName = "Arial";
                        sht.Range("C" + row + ":C" + row).Style.Font.FontSize = 11;
                        // sht.Range("C" + row + ":C" + row).Style.Font.Bold = true;
                        sht.Range("C" + row + ":C" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        using (var a = sht.Range("B" + (row-1) + ":C" + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                    }

                        sht.Cell(innernextChar + row).Value = innerresult.Select(a => a.FirstOrDefault().qty).FirstOrDefault();
                        sht.Range(innernextChar + row + ":" + innernextChar + row).Style.Font.FontName = "Arial";
                        sht.Range(innernextChar + row + ":" + innernextChar + row).Style.Font.FontSize = 12;
                        sht.Range(innernextChar + row + ":" + innernextChar + row).Style.Font.Bold = true;
                        sht.Range(innernextChar + row + ":" + innernextChar + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    

                    //sht.Cell("D" + row).Value = innerresult.Select(a=>a.FirstOrDefault().qty).FirstOrDefault();
                    //sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                    //sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                    //sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                  //  sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //    if (item.process == lastprocess)
                    //    {

                    //        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                    //        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                    //        sht.Range("E" + (row - 1) + ":E" + row).Merge();
                    //        sht.Cell("E" + (row - 1)).Value = lastqty + item.qty;
                    //        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    //    }
                    //    else
                    //    {
                    //        sht.Cell("E" + row).Value = item.qty;
                    //        sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                    //        sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                    //        //  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                    //        sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    //    }

                        using (var a = sht.Range(innernextChar + row + ":" + innernextChar + row))
                        {
                            a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                  //  total = total + item.qty;
                    if (item.process == lastprocess)
                    {
                        row = row + 1;
                    }
                    else { 
                        row = row + 1; 
                    
                    }
                    lastprocess = item.process;
                    laststatus = item.status;
                    //lastqty = item.qty;
                    }

                  
                    
                    headcount += 1;
                    innernextcharindex += 1;
                }
                int rowtotal = 7;
                string rowlastprocess = string.Empty;
                foreach (var head in queryDIM)
                {
                   // sht.Column(nextChar+rowtotal).Width = 25.89;
                    int sum = finalquery.Where(a => a.process == head.process && a.status == head.status).Sum(a => a.qty);
                    sht.Cell(nextChar + rowtotal).Value = sum;
                    sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal).Style.Font.FontName = "Arial";
                    sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal).Style.Font.FontSize = 12;
                    sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal).Style.Font.Bold = true;
                    sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    using (var a = sht.Range(nextChar + rowtotal + ":" + nextChar + rowtotal))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    if (head.process == rowlastprocess)
                    {
                        rowtotal = rowtotal + 1;
                    }
                    else
                    {
                        rowtotal = rowtotal + 1;

                    }
                    rowlastprocess = head.process;
                }

               // nextChar = arr[nextcharindex];
               
                //row = row + 1;
                //sht.Cell("D" + row).Value = "Total";
                //sht.Range("D" + row + ":D" + row).Style.Font.FontName = "Arial";
                //sht.Range("D" + row + ":D" + row).Style.Font.FontSize = 11;
                ////sht.Range("D" + row + ":D" + row).Style.Font.Bold = true;
                //sht.Range("D" + row + ":D" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                //sht.Cell("E" + row).Value = total;
                //sht.Range("E" + row + ":E" + row).Style.Font.FontName = "Arial";
                //sht.Range("E" + row + ":E" + row).Style.Font.FontSize = 11;
                ////  sht.Range("E" + row + ":E" + row).Style.Font.Bold = true;
                //sht.Range("E" + row + ":E" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                //using (var a = sht.Range("B" + row + ":E" + row))
                //{
                //    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //}
                string Path = "";
                string Pathpdf = "";
                string Fileextension = "xlsx";
                string filename = "WIP_PROCESS_WISE_STOCK_SUMMARY." + Fileextension + "";
                Path = Server.MapPath("~/InvoiceExcel/" + filename);
                xapp.SaveAs(Path);

                xapp.Dispose();


                //Download File
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //   Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(Path);
                Response.End();

                //*************    
            }
            else
            {
                lblmsg.Text = "No records found for this combination.";
            }
        }
        catch (Exception)
        {

            throw;
        }


    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
    }
    static IEnumerable<string> GetExcelStrings()
    {
        string[] alphabet = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        string[] alphabet1 = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        string[] alphabet2 = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        return from c1 in alphabet
               from c2 in alphabet
               from c3 in alphabet.Skip(1)                    // c3 is never empty
               where c1 == string.Empty || c2 != string.Empty // only allow c2 to be empty if c1 is also empty
               select c1 + c2 + c3;
    }

    protected void WIPDetailDiamond()
    {
        string where = "";
        lblmsg.Text = "";
        if (DDCategory.SelectedIndex > 0)
        {
            where = where + " and vf.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            where = where + " and vf.Item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.qualityid=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            where = where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }
        if (DDCustCode.SelectedIndex > 0)
        {
            where = where + " and OM.CUSTOMERID=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            where = where + " and OM.ORDERID=" + DDOrderNo.SelectedValue;
        }

        //SqlParameter[] param = new SqlParameter[3];
        //param[0] = new SqlParameter("@companyId", DDcompany.SelectedValue);
        //param[1] = new SqlParameter("@where", where);
        //param[2] = new SqlParameter("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");
        ////*************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Wipdetails", param);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_WIPDETAILS_DIAMOND", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@companyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@where", where);
        cmd.Parameters.AddWithValue("@WIPDETAILWITHLOTNO", chkwpidetailwithlotno.Checked == true ? "1" : "0");

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        con.Close();
        con.Dispose();

        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Columns.Remove("Stockno");
            ds.Tables[0].Columns.Remove("SeqNo");
            //Export to excel
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            lblmsg.Text = "Wait....";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=WIPStockDetail" + DateTime.Now + ".xls");
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
            lblmsg.Text = "Done.....";
            //*************    
        }
        else
        {
            lblmsg.Text = "No records found for this combination.";
        }

    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string orderNo = "CustomerOrderNo";
        if (Session["varMasterCompanyIDForERP"].ToString() == "9")
        {
            orderNo = "Localorder + '#' + CustomerorderNo ";
        }
        string Str = @"Select OrderId," + orderNo + " as CustomerOrderNo From OrderMaster where CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDcompany.SelectedValue + " Order By CustomerOrderNo";
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            Str = @"Select OrderId," + orderNo + " as CustomerOrderNo From OrderMaster where Status = 0 And CustomerId=" + DDCustCode.SelectedValue + " And CompanyId=" + DDcompany.SelectedValue + " Order By CustomerOrderNo";
        }
        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
  
}