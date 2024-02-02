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


public partial class Masters_ReportForms_FrmMapTraceStockReport : System.Web.UI.Page
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
                        JOIN MAP_ISSUEMASTER a(nolock) ON a.CompanyId = CI.CompanyId 
                        Where CI.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" Order By CI.CompanyName 

                        Select Distinct CI.CustomerId, CI.CustomerCode 
                        From MAP_ISSUEMASTER a(nolock) 
                        JOIN CustomerInfo CI(nolock) ON a.CustomerId = CI.CustomerId 
                        Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @"  Order By CI.CustomerCode    
                      

                        Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                        From MAP_ISSUEMASTER a(nolock) 
                        JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                        JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
                        Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @"
                        Order By VF.CATEGORY_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 1, true, "---Plz Select---");
            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    DDCustCode.SelectedValue = "1";
            //    FillCustCode_SelectedIndexChanged();
            //}
           
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");

            //txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //RDAll.Checked = true;

            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "30":
                    TRCustomerCode.Visible = false;
                    FillCustCode_SelectedIndexChanged();
                    break;
                default:
                    TRCustomerCode.Visible = true;
                    break;
            }
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCompany_SelectedIndexChanged();
    }
    protected void FillCompany_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, @"Select Distinct CI.CustomerId, CI.CustomerCode 
                        From MAP_ISSUEMASTER a(nolock) 
                        JOIN CustomerInfo CI(nolock) ON a.CustomerId = CI.CustomerId 
                        Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + " Order By CI.CustomerCode", true, "--Select--");
    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCustCode_SelectedIndexChanged();
    }
    protected void FillCustCode_SelectedIndexChanged()
    {
        string Str = @"Select Distinct b.OrderId, OM.CustomerOrderNo 
            From MAP_ISSUEMASTER a(nolock) 
            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
            JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID ";
        if (Session["varMasterCompanyIDForERP"].ToString() == "16" || Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            Str = Str + " And OM.Status = 0 ";
        }
        Str = Str + " Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" And a.CompanyId = " + DDCompany.SelectedValue;

        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And a.CustomerId = " + DDCustCode.SelectedValue;
        }

        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
//    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        FillDesignerName_SelectedIndexChanged();
//    }
//    protected void FillDesignerName_SelectedIndexChanged()
//    {
//        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select a.ID, a.ChallanNo 
//            From MAP_ISSUEMASTER a(nolock) 
//            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
//            And a.EmpId = " + DDDesignerName.SelectedValue + " Order By a.ID", true, "--Select--");
//    }
//    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        FillChallanNo_SelectedIndexChanged();
//    }
//    protected void FillChallanNo_SelectedIndexChanged()
//    {
//        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
//            From MAP_ISSUEMASTER a(nolock) 
//            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
//            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
//            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
//            And a.ID = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME ", true, "--Select--");
//    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory_SelectedIndexChanged();
    }
    protected void FillCategory_SelectedIndexChanged()
    {
        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From MAP_STENCILSTOCKNO a(nolock)            
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

//        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
//            From MAP_ISSUEMASTER a(nolock) 
//            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
//            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
//            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    Str = Str + " And a.ID = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME";
        //}
        Str = Str + " Order By VF.ITEM_NAME ";

        UtilityModule.ConditionalComboFill(ref DDItemName, Str, true, "--Select--");
        Fillcombo();
    }
    protected void Fillcombo()
    {
        Trquality.Visible = false;
        Trdesign.Visible = false;
        Trcolor.Visible = false;
        TrShape.Visible = false;
        Trsize.Visible = false;
        Trshadecolor.Visible = false;
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
                        FillShadeColor();
                        break;
                    case "2":
                        Trdesign.Visible = true;
                        FillColor();
                        break;
                    case "3":
                        Trcolor.Visible = true;
                        FillShape();
                        break;
                    case "4":
                        TrShape.Visible = true;
                        FillSize();
                        break;
                    case "5":
                        Trsize.Visible = true;
                        FillShadeColor();
                        break;
                    case "6":
                        Trshadecolor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQuality();
    }
    protected void FillQuality()
    {
        string str = @"Select Distinct VF.QualityId, VF.QualityName 
                    From MAP_STENCILSTOCKNO a(nolock)                     
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By VF.QualityName";

        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "---Plz Select---");
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
    }
    protected void FillDesign()
    {
        string str = @"Select Distinct VF.DesignID, VF.DesignName 
                    From MAP_STENCILSTOCKNO a(nolock)                     
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By VF.DesignName";

        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "---Plz Select---");
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void FillColor()
    {
        string str = @"Select Distinct VF.ColorID, VF.ColorName 
                    From MAP_STENCILSTOCKNO a(nolock)                    
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By VF.ColorName";

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void FillShape()
    {
        string str = @"Select Distinct VF.ShapeID, VF.ShapeName 
                    From MAP_STENCILSTOCKNO a(nolock)                    
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By VF.ShapeName";

        UtilityModule.ConditionalComboFill(ref DDShape, str, true, "---Plz Select---");
    }
    protected void FillShadeColor()
    {
        string str = @"Select Distinct VF.ShadeID, VF.ShadeColorName 
                    From MAP_STENCILSTOCKNO a(nolock)                   
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By VF.ShadeColorName";

        UtilityModule.ConditionalComboFill(ref DDshade, str, true, "---Plz Select---");

    }
    protected void FillSize()
    {
        string Size = "SizeFt";
        if (Chkmtrsize.Checked == true)
        {
            Size = "Sizemtr";
        }

        string str = @"Select Distinct VF.SizeId, " + Size + @"
                    From MAP_STENCILSTOCKNO a(nolock)                     
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (DDshade.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        //if (DDChallanNo.SelectedIndex > 0)
        //{
        //    str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        //}

        str = str + " Order By " + Size;

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "---Plz Select---");
    }
    protected void Chkmtrsize_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShape();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        MapTraceStockReport();
        
    }
    protected void MapTraceStockReport()
    {
        // DataSet ds = new DataSet();        

        string strCondition = "";
        //Check Conditions
        //if (ChkForDate.Checked == true)
        //{
        //    strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
        //}
        if (DDCustCode.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.Customerid=" + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            strCondition = strCondition + " And OM.orderid=" + DDOrderNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ITEM_ID=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (DDshade.SelectedIndex > 0)
        {
            strCondition = strCondition + " And VF.ShadecolorId=" + DDshade.SelectedValue;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_MAPTRACESTOCKREPORT", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", DDCompany.SelectedValue);
        cmd.Parameters.AddWithValue("@MapStencilType", DDMapStencilType.SelectedValue);
        cmd.Parameters.AddWithValue("@Where", strCondition);

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
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=MapTraceTotalStock" + DateTime.Now + ".xls");
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

            //Session["rptFileName"] = "~\\Reports\\RptFinishingBalanceOrderWiseReport.rpt";
            //Session["Getdataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\RptFinishingBalanceOrderWiseReport.xsd";
            //StringBuilder stb = new StringBuilder();
            //stb.Append("<script>");
            //stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }


}