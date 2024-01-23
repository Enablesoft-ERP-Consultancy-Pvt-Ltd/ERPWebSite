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


public partial class Masters_ReportForms_FrmMapStencilReport : System.Web.UI.Page
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
    
                        Select Distinct EI.EmpID, EI.EmpName + '(' + EI.EmpCode + ')' EmpName
                        From MAP_ISSUEMASTER a(nolock) 
                        JOIN Empinfo EI(nolock) ON EI.EmpId = a.EmpId 
                        Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" Order By EI.EmpName + '(' + EI.EmpCode + ')'

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
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignerName, ds, 2, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "---Plz Select---");

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDAll.Checked = true;

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
    protected void DDDesignerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesignerName_SelectedIndexChanged();
    }
    protected void FillDesignerName_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select a.ID, a.ChallanNo 
            From MAP_ISSUEMASTER a(nolock) 
            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And a.EmpId = " + DDDesignerName.SelectedValue + " Order By a.ID", true, "--Select--");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChallanNo_SelectedIndexChanged();
    }
    protected void FillChallanNo_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
            From MAP_ISSUEMASTER a(nolock) 
            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And a.ID = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME ", true, "--Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory_SelectedIndexChanged();
    }
    protected void FillCategory_SelectedIndexChanged()
    {
        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From MAP_ISSUEMASTER a(nolock) 
            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
            Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And a.ID = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME";
        }
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
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

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
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
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

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

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
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
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

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ColorName";

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void FillShape()
    {
        string str = @"Select Distinct VF.ShapeID, VF.ShapeName 
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
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

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ShapeName";

        UtilityModule.ConditionalComboFill(ref DDShape, str, true, "---Plz Select---");
    }
    protected void FillShadeColor()
    {
        string str = @"Select Distinct VF.ShadeID, VF.ShadeColorName 
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
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

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

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
                    From MAP_ISSUEMASTER a(nolock) 
                    JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
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

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

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

        if (RDAll.Checked == true)
        {
            ForAllClick();
        }
        else if (RDOrder.Checked == true)
        {
            ForIssueClick();
        }
        else if (RDReceive.Checked == true)
        {
            ForReceiveClick();
        }
    }
    protected void ForAllClick()
    {

        string str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                    a.AssignDate, a.RequiredDate, OM.CustomerOrderNo, MT.MapType, 
                    VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                    Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, b.MapIssueQty IssQty, 
                    IsNull((Select Sum(MRD.ReceiveQty) 
	                    From MAP_RECEIVEMASTER MRM(nolock)
	                    JOIN MAP_RECEIVEDETAIL MRD(Nolock) ON MRD.RID = MRM.RID And MRD.ID = a.ID And MRD.DetailId = b.Detailid), 0) RecQty
                    From MAP_ISSUEMASTER a(nolock)
                    JOIN MAP_ISSUEDETAIL b(Nolock) ON b.Masterid = a.ID 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
                    LEFT JOIN CustomerInfo CU(Nolock) ON CU.CustomerID = a.CustomerId 
                    JOIN Empinfo EI(Nolock) ON EI.EmpId = a.EmpId 
                    JOIN Unit U(Nolock) ON U.UnitId = a.UnitID 
                    JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
                    JOIN MapType MT(Nolock) ON MT.ID = b.MapIssueType 
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.AssignDate >= '" + txtfromDate.Text + @"'
                    And a.AssignDate <= '" + txttodate.Text + "'";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And a.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDDesignerName.SelectedIndex > 0)
        {
            str = str + " And a.EmpID = " + DDDesignerName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

        str = str + " And a.MapStencilType = " + DDMapStencilType.SelectedValue;

        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.Category_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.Item_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.SizeID = " + DDSize.SelectedValue;
        }
        if (DDshade.SelectedIndex > 0)
        {
            str = str + " And VF.ShadeColorID=" + DDshade.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=MapTraceAllIssRecDetail" + DateTime.Now + ".xls");
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
    }
    protected void ForIssueClick()
    {
        string str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                        a.AssignDate, a.RequiredDate, OM.CustomerOrderNo, MT.MapType, 
                        VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                        Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, b.MapIssueQty Qty 
                        From MAP_ISSUEMASTER a(nolock)
                        JOIN MAP_ISSUEDETAIL b(Nolock) ON b.Masterid = a.ID 
                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
                        LEFT JOIN CustomerInfo CU(Nolock) ON CU.CustomerID = a.CustomerId 
                        JOIN Empinfo EI(Nolock) ON EI.EmpId = a.EmpId 
                        JOIN Unit U(Nolock) ON U.UnitId = a.UnitID 
                        JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
                        JOIN MapType MT(Nolock) ON MT.ID = b.MapIssueType 
                        Where a.CompanyID = " + DDCompany.SelectedValue + " And a.AssignDate >= '" + txtfromDate.Text + @"'
                        And a.AssignDate <= '" + txttodate.Text + "'";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And a.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDDesignerName.SelectedIndex > 0)
        {
            str = str + " And a.EmpID = " + DDDesignerName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.ID = " + DDChallanNo.SelectedValue;
        }

        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.Category_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.Item_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.SizeID = " + DDSize.SelectedValue;
        }
        if (DDshade.SelectedIndex > 0)
        {
            str = str + " And VF.ShadeColorID=" + DDshade.SelectedValue;
        }
        str = str + " And a.MapStencilType = " + DDMapStencilType.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=MapTraceIssueDetail" + DateTime.Now + ".xls");
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
    }

    protected void ForReceiveClick()
    {
        string str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                    a.ReceiveDate, OM.CustomerOrderNo, MT.MapType, 
                    VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                    Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, b.ReceiveQty Qty, 
                    (Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) Where MSS.CompanyId = a.CompanyId And MSS.RID = a.RID AND MSS.RDetailId = b.RDetailid For XML Path('')) MAPSTENCILNo,
                    isnull(b.ReceiveRate,0) as ReceiveRate,isnull(b.ReceiveAmount,0) as ReceiveAmount
                    From MAP_RECEIVEMASTER a(nolock)
                    JOIN MAP_RECEIVEDETAIL b(Nolock) ON b.RID = a.RID 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
                    LEFT JOIN CustomerInfo CU(Nolock) ON CU.CustomerID = a.CustomerId 
                    JOIN Empinfo EI(Nolock) ON EI.EmpId = a.EmpId 
                    JOIN Unit U(Nolock) ON U.UnitId = b.UnitID 
                    JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
                    JOIN MapType MT(Nolock) ON MT.ID = b.MapIssueType 
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.ReceiveDate >= '" + txtfromDate.Text + @"'
                    And a.ReceiveDate <= '" + txttodate.Text + "'";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And a.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDDesignerName.SelectedIndex > 0)
        {
            str = str + " And a.EmpID = " + DDDesignerName.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.RID = " + DDChallanNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.Category_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.Item_ID = " + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.SizeID = " + DDSize.SelectedValue;
        }
        if (DDshade.SelectedIndex > 0)
        {
            str = str + " And VF.ShadeColorID=" + DDshade.SelectedValue;
        }

        str = str + " And a.MapStencilType = " + DDMapStencilType.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=MapTraceReceiveDetail" + DateTime.Now + ".xls");
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
    }
}