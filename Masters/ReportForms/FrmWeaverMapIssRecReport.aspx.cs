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


public partial class Masters_ReportForms_FrmWeaverMapIssRecReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) ON a.CompanyId = CI.CompanyId 
                        Where CI.MasterCompanyid = " + Session["varCompanyId"] + @" Order By CI.CompanyName 

                        Select Distinct CI.CustomerId, CI.CustomerCode 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID
                        JOIN CustomerInfo CI(nolock) ON OM.CustomerId = CI.CustomerId 
                        Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" Order By CI.CustomerCode 

                        Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                        JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId  
                        Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                        Order By VF.CATEGORY_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustCode, ds, 1, true, "---Plz Select---");

            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");

            FillWeaverName();

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDAll.Checked = true;

            switch (Session["VarCompanyId"].ToString())
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
    protected void FillWeaverName()
    {
        if (RDReceive.Checked == true)
        {
            if (Session["varCompanyId"].ToString() == "30")
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, EI.EmpName + '(' + EI.EmpCode + ')' EmpName
                From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
                JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID  
                INNER JOIN Process_Issue_Master_1 PIM ON b.ISSUEORDERID=PIM.ISSUEORDERID
                 JOIN Empinfo EI(nolock) ON EI.EmpId=PIM.EmpID                   
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By EI.EmpName + '(' + EI.EmpCode + ')'", true, "--Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, VE.EmpName + '(' + VE.EmpIdNo + ')' EmpName
                From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
                JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID  
                INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON b.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
                JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo)                
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By VE.EmpName + '(' + VE.EmpIdNo + ')'", true, "--Select--");
            }

        }
        else
        {
            if (Session["varCompanyId"].ToString() == "30")
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, EI.EmpName + '(' + EI.EmpCode + ')' EmpName
                From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                INNER JOIN Process_Issue_Master_1 PIM ON a.ISSUEORDERID=PIM.ISSUEORDERID 
                JOIN Empinfo EI(nolock) ON EI.EmpId=PIM.EmpID 
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By EI.EmpName + '(' + EI.EmpCode + ')'", true, "--Select--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, VE.EmpName + '(' + VE.EmpIdNo + ')' EmpName
                From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
                JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo)  
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By VE.EmpName + '(' + VE.EmpIdNo + ')'", true, "--Select--");
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
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID
                        JOIN CustomerInfo CI(nolock) ON OM.CustomerId = CI.CustomerId 
                        Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + " Order By CI.CustomerCode", true, "--Select--");

    }
    protected void DDCustCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCustCode_SelectedIndexChanged();
    }
    protected void FillCustCode_SelectedIndexChanged()
    {
        string Str = @"Select Distinct b.OrderId, OM.CustomerOrderNo 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID 
                        Where a.MasterCompanyID = " + Session["varCompanyId"] + @"  And a.CompanyId = " + DDCompany.SelectedValue;
        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            Str = @"Select Distinct b.OrderId, OM.CustomerOrderNo 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID And OM.Status = 0 
                        Where a.MasterCompanyID = " + Session["varCompanyId"] + @"  And a.CompanyId = " + DDCompany.SelectedValue;                        
        }

        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And OM.CustomerId = " + DDCustCode.SelectedValue;
        }

        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
    protected void DDWeaverName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWeaverName_SelectedIndexChanged();
    }
    protected void FillWeaverName_SelectedIndexChanged()
    {
        //        if (RDReceive.Checked == true)
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.RecId, cast( b.IssueOrderid as varchar)+'/'+ cast( a.ChallanNo  as varchar) as ChallanNo
        //            From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
        //            JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID  
        //            INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON b.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
        //            JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo) 
        //            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
        //            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.RecId", true, "--Select--");
        //        }
        //        else
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.IssueId, cast( a.IssueOrderid as varchar)+'/'+ cast( a.ChallanNo  as varchar) as ChallanNo
        //            From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
        //            INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
        //            JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo) 
        //            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
        //            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.IssueId", true, "--Select--");
        //        }

        if (Session["varCompanyId"].ToString() == "30")
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.IssueOrderid, cast( PIM.ChallanNo as varchar) as ChallanNo
            From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            --INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
            INNER JOIN Process_Issue_Master_1 PIM ON a.ISSUEORDERID=PIM.ISSUEORDERID
            JOIN Empinfo EI(nolock) ON EI.EmpId=PIM.EmpID   
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.IssueOrderid", true, "--Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.IssueOrderid, cast( a.IssueOrderid as varchar) as ChallanNo
            From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
            JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo) 
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.IssueOrderid", true, "--Select--");
        }

    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChallanNo_SelectedIndexChanged();
    }
    protected void FillChallanNo_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
            From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And a.IssueOrderid = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME ", true, "--Select--");

        //        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
        //            From MAP_ISSUEMASTER a(nolock) 
        //            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
        //            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
        //            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
        //            And a.ID = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME ", true, "--Select--");

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory_SelectedIndexChanged();
    }
    protected void FillCategory_SelectedIndexChanged()
    {
        //        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
        //            From MAP_ISSUEMASTER a(nolock) 
        //            JOIN MAP_ISSUEDETAIL b(nolock) ON b.Masterid = a.ID
        //            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
        //            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;


        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME";
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
                  " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
                    JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId";
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " And VF.CATEGORY_ID = " + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.ITEM_ID = " + DDItemName.SelectedValue;
        }
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
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
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
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
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ColorName";

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void FillShape()
    {
        string str = @"Select Distinct VF.ShapeID, VF.ShapeName 
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
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
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ShapeName";

        UtilityModule.ConditionalComboFill(ref DDShape, str, true, "---Plz Select---");
    }
    protected void FillShadeColor()
    {
        string str = @"Select Distinct VF.ShadeColorId, VF.ShadeColorName 
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
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
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.Issueid = a.ISSUEID
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
        str = str + " Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
        string str = "";
        if (Session["varCompanyId"].ToString() == "30")
        {
            str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName,U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                        a.IssueDate, OM.CustomerOrderNo,  
                        VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                        Case When MSS.UnitID = 1 Then VF.SizeMtr Else Case When MSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE,( b.Qty) IssQty, 
                        IsNull((Select Sum(MRD.Qty) 
	                        From MAP_ReceiveONPRODUCTIONORDERMASTER MRM(nolock)
	                        JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL MRD(Nolock) ON MRD.RecId = MRM.RecId And MRD.IssueId = a.IssueId And MRD.IssueDetailId = b.IssueDetailid), 0) RecQty
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock)
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(Nolock) ON b.Issueid = a.ISSUEID 
                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId                        
					    JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
					    LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
					    JOIN Process_issue_Master_1 PIM(NoLock) ON a.IssueOrderId=PIM.ISSUEORDERID
					    JOIN Empinfo EI(nolock) ON PIM.EmpID=EI.EmpID
                       JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.ISSUEID=MSSN.Issueid and b.IssueDetailId=MSSN.IssueDetailId
				       JOIN MAP_STENCILSTOCKNO MSS(nolock) ON MSSN.MapStencilNo=MSS.MapStencilNo
				       JOIN Unit U(Nolock) ON U.UnitId = MSS.UnitID
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId  
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.IssueDate >= '" + txtfromDate.Text + @"'
                    And a.IssueDate <= '" + txttodate.Text + "'";
        }
        else
        {

            str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName,U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                        a.IssueDate, OM.CustomerOrderNo,  
                        VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                        Case When MSS.UnitID = 1 Then VF.SizeMtr Else Case When MSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE,( b.Qty) IssQty, 
                        IsNull((Select Sum(MRD.Qty) 
	                        From MAP_ReceiveONPRODUCTIONORDERMASTER MRM(nolock)
	                        JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL MRD(Nolock) ON MRD.RecId = MRM.RecId And MRD.IssueId = a.IssueId And MRD.IssueDetailId = b.IssueDetailid), 0) RecQty
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock)
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(Nolock) ON b.Issueid = a.ISSUEID 
                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId                        
					    JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
					    LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
					    INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
					    JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo) 
                       JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.ISSUEID=MSSN.Issueid and b.IssueDetailId=MSSN.IssueDetailId
				       JOIN MAP_STENCILSTOCKNO MSS(nolock) ON MSSN.MapStencilNo=MSS.MapStencilNo
				       JOIN Unit U(Nolock) ON U.UnitId = MSS.UnitID
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId  
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.IssueDate >= '" + txtfromDate.Text + @"'
                    And a.IssueDate <= '" + txttodate.Text + "'";
        }

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And CU.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDWeaverName.SelectedIndex > 0)
        {
            str = str + " And EI.EmpID in( " + DDWeaverName.SelectedValue + ")";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            //str = str + " And a.ISSUEID = " + DDChallanNo.SelectedValue;

            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
        str = str + @"  Group By CI.CompanyName, CU.CustomerCode, EI.EmpName, a.ChallanNo,a.MapStencilType,a.IssueDate, OM.CustomerOrderNo,  
                    VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName,MSS.UnitID,VF.SizeMtr,VF.SizeInch,VF.SizeFt, a.IssueId,b.IssueDetailid,b.Qty,U.UnitName";

        str = str + " Order By a.IssueDate";

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
             "attachment;filename=WeaverMapTraceAllIssRecDetail" + DateTime.Now + ".xls");
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
        string str = "";
        if (Session["varCompanyId"].ToString() == "30")
        {
            str = @"SELECT CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, PRM.ISSUEORDERID FOLIONO, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                        a.IssueDate, OM.CustomerOrderNo, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                        Case When MSSS.UnitID = 1 Then VF.SizeMtr Else Case When MSSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, b.Qty,
                        (Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) JOIN MAP_STENCILSTOCKNO_DETAIL MSSD(Nolock) ON MSS.MapStencilNo=MSSD.MapStencilNo
                        Where MSS.CompanyId = a.CompanyId And MSSD.IssueId = a.IssueID and MSSD.IssueDetailId=b.IssueDetailId For XML Path('')) MAPSTENCILNo, 
                        Case When PRM.Purchaseflag = 1 Then 'PURCHASE' ELSE 'JOB' End FolioType, 
                        CASE WHEN MSSS.MapIssueType = 1 Then 'FULL' Else CASE WHEN MSSS.MapIssueType = 1 Then 'HALF' ELSE 'QUARTER' END END MAPTYPE, 
                        Round((CASE WHEN MSSS.UnitID = 1 Then VF.AreaMtr Else Case When MSSS.UnitID = 6 Then VF.AreaInch Else VF.Actualfullareasqyd END END) 
		                         / (CASE WHEN MSSS.MapIssueType = 3 Then 4 Else MSSS.MapIssueType END), 4) AREA, MSSS.ReceiveRATE, 
	                    CASE WHEN MSSS.ReceiveRateCalcType = 1 Then 1 
		                    ELSE (CASE WHEN MSSS.UnitID = 1 Then VF.AreaMtr Else Case When MSSS.UnitID = 6 Then VF.AreaInch Else VF.Actualfullareasqyd END END) 
		                     END / (CASE WHEN MSSS.MapIssueType = 3 Then 4 Else MSSS.MapIssueType END) * (b.Qty * MSSS.ReceiveRATE) AMOUNT 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock)
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(Nolock) ON b.Issueid = a.ISSUEID 
                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
                        JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
                        LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
                        JOIN PROCESS_ISSUE_MASTER_1 PRM(Nolock) ON PRM.IssueOrderId = a.IssueOrderId 
                        JOIN Empinfo EI(nolock) ON PRM.EmpID=EI.EmpID
                        JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.ISSUEID=MSSN.Issueid and b.IssueDetailId=MSSN.IssueDetailId
                        JOIN VIEW_MAP_STENCILSTOCKNO_MAP_RECEIVEDETAIL MSSS(nolock) ON MSSS.MapStencilNo = MSSN.MapStencilNo 
                        JOIN Unit U(Nolock) ON U.UnitId = MSSS.UnitID                         
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId
                        Where a.CompanyID = " + DDCompany.SelectedValue + " And a.IssueDate >= '" + txtfromDate.Text + @"'
                        And a.IssueDate <= '" + txttodate.Text + "'";
        }
        else
        {
            str = @"SELECT CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, PRM.ISSUEORDERID FOLIONO, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                        a.IssueDate, OM.CustomerOrderNo, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                        Case When MSSS.UnitID = 1 Then VF.SizeMtr Else Case When MSSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, b.Qty,
                        (Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) JOIN MAP_STENCILSTOCKNO_DETAIL MSSD(Nolock) ON MSS.MapStencilNo=MSSD.MapStencilNo
                        Where MSS.CompanyId = a.CompanyId And MSSD.IssueId = a.IssueID and MSSD.IssueDetailId=b.IssueDetailId For XML Path('')) MAPSTENCILNo, 
                        Case When PRM.Purchaseflag = 1 Then 'PURCHASE' ELSE 'JOB' End FolioType, 
                        CASE WHEN MSSS.MapIssueType = 1 Then 'FULL' Else CASE WHEN MSSS.MapIssueType = 1 Then 'HALF' ELSE 'QUARTER' END END MAPTYPE, 
                        Round((CASE WHEN MSSS.UnitID = 1 Then VF.AreaMtr Else Case When MSSS.UnitID = 6 Then VF.AreaInch Else VF.Actualfullareasqyd END END) 
		                         / (CASE WHEN MSSS.MapIssueType = 3 Then 4 Else MSSS.MapIssueType END), 4) AREA, MSSS.ReceiveRATE, 
	                    CASE WHEN MSSS.ReceiveRateCalcType = 1 Then 1 
		                    ELSE (CASE WHEN MSSS.UnitID = 1 Then VF.AreaMtr Else Case When MSSS.UnitID = 6 Then VF.AreaInch Else VF.Actualfullareasqyd END END) 
		                     END / (CASE WHEN MSSS.MapIssueType = 3 Then 4 Else MSSS.MapIssueType END) * (b.Qty * MSSS.ReceiveRATE) AMOUNT 
                        From MAP_ISSUEONPRODUCTIONORDERMASTER a(nolock)
                        JOIN MAP_ISSUEONPRODUCTIONORDERDETAIL b(Nolock) ON b.Issueid = a.ISSUEID 
                        JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
                        JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
                        LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
                        JOIN PROCESS_ISSUE_MASTER_1 PRM(Nolock) ON PRM.IssueOrderId = a.IssueOrderId 
                        JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
                        JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo) 
                        JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.ISSUEID=MSSN.Issueid and b.IssueDetailId=MSSN.IssueDetailId
                        JOIN VIEW_MAP_STENCILSTOCKNO_MAP_RECEIVEDETAIL MSSS(nolock) ON MSSS.MapStencilNo = MSSN.MapStencilNo 
                        JOIN Unit U(Nolock) ON U.UnitId = MSSS.UnitID                         
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId
                        Where a.CompanyID = " + DDCompany.SelectedValue + " And a.IssueDate >= '" + txtfromDate.Text + @"'
                        And a.IssueDate <= '" + txttodate.Text + "'";
        }

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And CU.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDWeaverName.SelectedIndex > 0)
        {
            //str = str + " And a.EmpID = " + DDWeaverName.SelectedValue;
            str = str + " And EI.EmpID in( " + DDWeaverName.SelectedValue + ")";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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

        str = str + @" Group by a.CompanyId,CI.CompanyName, CU.CustomerCode, EI.EmpName, a.ChallanNo,a.MapStencilType,a.IssueDate, OM.CustomerOrderNo,  
			VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName,MSSS.UnitID,VF.SizeMtr,VF.SizeInch,VF.SizeFt, a.IssueId, 
            b.IssueDetailid,U.UnitName,b.Qty, PRM.Purchaseflag, MSSS.ReceiveRATE, MSSS.MapIssueType, 
            VF.AreaMtr, VF.AreaInch, VF.Actualfullareasqyd, MSSS.ReceiveRateCalcType, PRM.ISSUEORDERID";

        str = str + " Order By a.IssueDate";

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
             "attachment;filename=WeaverMapTraceIssueDetail" + DateTime.Now + ".xls");
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
        string str = "";
        if (Session["varCompanyId"].ToString() == "30")
        {
            str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                    a.ReceiveDate, OM.CustomerOrderNo,OM.LocalOrder, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                    Case When MSS.UnitID = 1 Then VF.SizeMtr Else Case When MSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, sum(b.Qty) as Qty, 
					(Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) JOIN MAP_STENCILSTOCKNO_DETAIL MSSD(Nolock) ON MSS.MapStencilNo=MSSD.MapStencilNo
					 Where MSS.CompanyId = a.CompanyId And MSSD.ReceiveId = a.RecID For XML Path('')) MAPSTENCILNo                   
                    From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
                    JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
					JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
					LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
					JOIN PROCESS_ISSUE_MASTER_1 PIM ON b.IssueOrderID=PIM.ISSUEORDERID
					JOIN EmpInfo EI(NoLock) ON PIM.EmpID=EI.EmpID
					JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.RecId=MSSN.ReceiveId and b.RecDetailId=MSSN.ReceiveDetailId
					JOIN MAP_STENCILSTOCKNO MSS(nolock) ON MSSN.MapStencilNo=MSS.MapStencilNo
					JOIN Unit U(Nolock) ON U.UnitId = MSS.UnitID                    
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId                     
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.ReceiveDate >= '" + txtfromDate.Text + @"'
                    And a.ReceiveDate <= '" + txttodate.Text + "'";

//            str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
//                    a.ReceiveDate, OM.CustomerOrderNo, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
//                    Case When MSS.UnitID = 1 Then VF.SizeMtr Else Case When MSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, sum(b.Qty) as Qty, 
//					(Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) JOIN MAP_STENCILSTOCKNO_DETAIL MSSD(Nolock) ON MSS.MapStencilNo=MSSD.MapStencilNo
//					 Where MSS.CompanyId = a.CompanyId And MSSD.ReceiveId = a.RecID  For XML Path('')) MAPSTENCILNo                   
//                    From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
//                    JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID 
//                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
//					JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
//					LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
//					INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON b.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
//					JOIN Empinfo EI(nolock) ON EI.EmpCode in( VE.EmpIdNo)
//					JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.RecId=MSSN.ReceiveId and b.RecDetailId=MSSN.ReceiveDetailId
//					JOIN MAP_STENCILSTOCKNO MSS(nolock) ON MSSN.MapStencilNo=MSS.MapStencilNo
//					JOIN Unit U(Nolock) ON U.UnitId = MSS.UnitID                    
//                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId                     
//                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.ReceiveDate >= '" + txtfromDate.Text + @"'
//                    And a.ReceiveDate <= '" + txttodate.Text + "'";
        }
        else
        {
            str = @"Select CI.CompanyName, isnull(CU.CustomerCode,'') as CustomerCode, EI.EmpName, U.UnitName, a.ChallanNo, CASE WHEN a.MapStencilType = 1 Then 'MAP' ELSE 'TRACE' END MAPType, 
                    a.ReceiveDate, OM.CustomerOrderNo,OM.LocalOrder, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, 
                    Case When MSS.UnitID = 1 Then VF.SizeMtr Else Case When MSS.UnitID = 6 Then VF.SizeInch Else VF.SizeFt END END SIZE, sum(b.Qty) as Qty, 
					(Select MSSTOCKNO + ', ' From MAP_STENCILSTOCKNO MSS(Nolock) JOIN MAP_STENCILSTOCKNO_DETAIL MSSD(Nolock) ON MSS.MapStencilNo=MSSD.MapStencilNo
					 Where MSS.CompanyId = a.CompanyId And MSSD.ReceiveId = a.RecID For XML Path('')) MAPSTENCILNo                   
                    From MAP_ReceiveONPRODUCTIONORDERMASTER a(nolock)
                    JOIN MAP_ReceiveONPRODUCTIONORDERDETAIL b(Nolock) ON b.RecID = a.RecID 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyId 
					JOIN OrderMaster OM(Nolock) ON OM.OrderiD = b.Orderid 
					LEFT JOIN CustomerInfo CU(nolock) ON OM.CustomerId = CU.CustomerId 
					JOIN PROCESS_ISSUE_MASTER_1 PIM ON b.IssueOrderID=PIM.ISSUEORDERID
					JOIN EmpInfo EI(NoLock) ON PIM.EmpID=EI.EmpID
					JOIN MAP_STENCILSTOCKNO_DETAIL MSSN(nolock) ON b.RecId=MSSN.ReceiveId and b.RecDetailId=MSSN.ReceiveDetailId
					JOIN MAP_STENCILSTOCKNO MSS(nolock) ON MSSN.MapStencilNo=MSS.MapStencilNo
					JOIN Unit U(Nolock) ON U.UnitId = MSS.UnitID                    
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId                     
                    Where a.CompanyID = " + DDCompany.SelectedValue + " And a.ReceiveDate >= '" + txtfromDate.Text + @"'
                    And a.ReceiveDate <= '" + txttodate.Text + "'";
        }

        if (DDCustCode.SelectedIndex > 0)
        {
            //str = str + " And a.CustomerID = " + DDCustCode.SelectedValue;

            str = str + " And CU.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And b.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDWeaverName.SelectedIndex > 0)
        {
            //str = str + " And a.EmpID = " + DDWeaverName.SelectedValue;

            str = str + " And EI.EmpID in( " + DDWeaverName.SelectedValue + ")";
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            ////str = str + " And a.ID = " + DDChallanNo.SelectedValue;

            //str = str + " And a.RecId = " + DDChallanNo.SelectedValue;

            str = str + " And b.ISSUEORDERID = " + DDChallanNo.SelectedValue;
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

        str = str + @" Group by a.CompanyId,CI.CompanyName, CU.CustomerCode, EI.EmpName, a.ChallanNo,a.MapStencilType,a.ReceiveDate, OM.CustomerOrderNo, OM.LocalOrder, 
                    VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName,MSS.UnitID,VF.SizeMtr,VF.SizeInch,VF.SizeFt, U.UnitName,a.RecID";

        str = str + " Order By a.ReceiveDate";

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
             "attachment;filename=WeaverMapTraceReceiveDetail" + DateTime.Now + ".xls");
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
    protected void RDReceive_CheckedChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
    protected void RDOrder_CheckedChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
    protected void RDAll_CheckedChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
}