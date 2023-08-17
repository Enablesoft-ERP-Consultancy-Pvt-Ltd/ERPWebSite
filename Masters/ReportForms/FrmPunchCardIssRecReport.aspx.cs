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


public partial class Masters_ReportForms_FrmPunchCardIssRecReport : System.Web.UI.Page
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
                        JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) ON a.CompanyId = CI.CompanyId 
                        Where CI.MasterCompanyid = " + Session["varCompanyId"] + @" Order By CI.CompanyName 

                        Select Distinct CI.CustomerId, CI.CustomerCode 
                        From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIIssueid
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID
                        JOIN CustomerInfo CI(nolock) ON OM.CustomerId = CI.CustomerId 
                        Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" Order By CI.CustomerCode 

                        Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                        From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIIssueid
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
            BindProcess();

            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //RDAll.Checked = true;
            RDPunchCardIssueDetail.Checked = true;

            //switch (Session["VarCompanyId"].ToString())
            //{
            //    case "30":
            //        TRCustomerCode.Visible = false;
            //        FillCustCode_SelectedIndexChanged();
            //        break;
            //    default:
            //        TRCustomerCode.Visible = true;
            //        break;
            //}
        }
    }
    protected void BindProcess()
    {
        string str;
        if (Session["varcompanyId"].ToString() == "9")
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER  where process_name_id in(1,16,35)";                    
        }
        else
        {
            str = @"select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where Process_Name_id=1";                       
        }
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 0, true, "--Select--");
    }
    protected void FillWeaverName()
    {
        if (RDPunchCardReceiveDetail.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, EI.EmpName + '(' + EI.EmpCode + ')' EmpName
                From PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERMASTER a(nolock)
                JOIN PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERDETAIL b(Nolock) ON b.PCIReceiveId = a.PCIReceiveId  
                INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @" PIM(nolock) ON a.FolioISSUEORDERID=PIM.ISSUEORDERID
                JOIN Empinfo EI(nolock) ON EI.EmpId=PIM.EmpID                   
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By EI.EmpName + '(' + EI.EmpCode + ')'", true, "--Select--");            

        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDWeaverName, @"Select Distinct EI.EmpID, EI.EmpName + '(' + EI.EmpCode + ')' EmpName
                From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @" PIM(nolock) ON a.FolioISSUEORDERID=PIM.ISSUEORDERID 
                JOIN Empinfo EI(nolock) ON EI.EmpId=PIM.EmpID 
                Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By EI.EmpName + '(' + EI.EmpCode + ')'", true, "--Select--");
           
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCompany_SelectedIndexChanged();
    }
    protected void FillCompany_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDCustCode, @"Select Distinct CI.CustomerId, CI.CustomerCode 
                        From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIIssueid
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
                        From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                        JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIIssueid
                        JOIN OrderMaster OM(Nolock) ON OM.Orderid = b.OrderID 
                        Where a.MasterCompanyID = " + Session["varCompanyId"] + @"  And a.CompanyId = " + DDCompany.SelectedValue;       

        if (DDCustCode.SelectedIndex > 0)
        {
            Str = Str + " And OM.CustomerId = " + DDCustCode.SelectedValue;
        }

        UtilityModule.ConditionalComboFill(ref DDOrderNo, Str, true, "--Select--");
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
    protected void DDWeaverName_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmployeeSelectedChange();
    }
   
    private void EmployeeSelectedChange()
    {
        string str = "";
        if (variable.VarLoomNoGenerated == "1")
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM(nolock) 
                    inner join PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD(nolock) on Pm.issueorderid=Pd.issueorderid
                    Left join LoomstockNo ls(nolock) on pm.issueorderid=Ls.issueorderid And ls.ProcessID = " + DDProcessName.SelectedValue + @"
                    where ls.issueorderid is null and  PM.Empid=" + DDWeaverName.SelectedValue + " and PM.CompanyId=" + DDCompany.SelectedValue + @" 
                    and PD.OrderId=" + DDOrderNo.SelectedValue + " and PM.Status<>'canceled' order by PM.Issueorderid";
        }
        else
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PM(nolock),PROCESS_ISSUE_Detail_" + DDProcessName.SelectedValue + @" PD(nolock) 
                where PM.Empid=" + DDWeaverName.SelectedValue + " and PM.CompanyId=" +DDCompany.SelectedValue + @" and PM.IssueOrderId=PD.IssueOrderId 
                and PD.OrderId=" + DDOrderNo.SelectedValue + " and PM.Status<>'canceled' order by PM.Issueorderid";
        }
        UtilityModule.ConditionalComboFill(ref DDPOrderNo, str, true, "--Select--");
    }
    protected void fillChallanNo()
    {
        string str = "";
        if (RDPunchCardReceiveDetail.Checked == true)
        {
            str = @"Select distinct a.PCIReceiveID, cast( a.FolioIssueOrderId as varchar)+'/'+ cast( a.RecChallanNo  as varchar) as ChallanNo
            From PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERMASTER a(nolock)             
            INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM(nolock) ON a.FolioIssueOrderId=PIM.ISSUEORDERID
            JOIN Empinfo EI(nolock) ON EI.EmpId=a.EmpID   
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And EI.EmpId in(" + DDWeaverName.SelectedValue + ")";

            if (DDPOrderNo.SelectedIndex > 0)
            {
                str = str + " and a.FolioIssueOrderId="+DDPOrderNo.SelectedValue;
            }
            str = str + " Order by a.PCIReceiveID";

            UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");

//            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.PCIReceiveID, cast( a.FolioIssueOrderId as varchar)+'/'+ cast( a.RecChallanNo  as varchar) as ChallanNo
//            From PUNCHCARDINDENT_RECEIVEFROMPRODUCTIONORDERMASTER a(nolock)             
//            INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM ON a.FolioIssueOrderId=PIM.ISSUEORDERID
//            JOIN Empinfo EI(nolock) ON EI.EmpId=a.EmpID   
//            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
//            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.FolioIssueOrderId", true, "--Select--");
        }
        else
        {

            str = @"Select distinct a.PCIIssueId, cast( a.FolioIssueOrderid as varchar)+'/'+ cast( a.ChallanNo  as varchar) as ChallanNo
            From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            --INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
            INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM(nolock) ON a.FolioIssueOrderId=PIM.ISSUEORDERID
            JOIN Empinfo EI(nolock) ON EI.EmpId=a.EmpID   
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") ";

            if (DDPOrderNo.SelectedIndex > 0)
            {
                str = str + " and a.FolioIssueOrderId=" + DDPOrderNo.SelectedValue;
            }
            str = str + " Order by a.PCIIssueId";

            UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Select--");


//            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select distinct a.PCIIssueId, cast( a.FolioIssueOrderid as varchar)+'/'+ cast( a.ChallanNo  as varchar) as ChallanNo
//            From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
//            --INNER JOIN V_GETCOMMASEPARATEEMPLOYEE VE ON a.ISSUEORDERID=VE.ISSUEORDERID AND VE.PROCESSID=1
//            INNER JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM ON a.FolioIssueOrderId=PIM.ISSUEORDERID
//            JOIN Empinfo EI(nolock) ON EI.EmpId=a.EmpID   
//            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
//            And EI.EmpId in(" + DDWeaverName.SelectedValue + ") Order By a.FolioIssueOrderId", true, "--Select--");
        }

    }
    protected void DDPOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillChallanNo();
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChallanNo_SelectedIndexChanged();
    }
    protected void FillChallanNo_SelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
            From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId 
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue + @" 
            And a.IssueOrderid = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME ", true, "--Select--");      

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory_SelectedIndexChanged();
    }
    protected void FillCategory_SelectedIndexChanged()
    {       

        string Str = @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
            JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = b.ItemFinishedId And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
            Where a.MasterCompanyid = " + Session["varCompanyId"] + @" AND a.CompanyId = " + DDCompany.SelectedValue;

        if (DDChallanNo.SelectedIndex > 0)
        {
            Str = Str + " And a.PCIIssueId = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME";
            
            //Str = Str + " And a.FolioIssueOrderid = " + DDChallanNo.SelectedValue + " Order By VF.CATEGORY_NAME";

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
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
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
            str = str + " And a.PCIIssueId= " + DDChallanNo.SelectedValue;
            
            //str = str + " And a.FolioIssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIIssueid
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
            str = str + " And a.PCIIssueId = " + DDChallanNo.SelectedValue;

           // str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
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
            str = str + " And a.PCIIssueId = " + DDChallanNo.SelectedValue;

            //str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ColorName";

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "---Plz Select---");
    }
    protected void FillShape()
    {
        string str = @"Select Distinct VF.ShapeID, VF.ShapeName 
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
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
            str = str + " And a.PCIIssueId = " + DDChallanNo.SelectedValue;

            //str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
        }

        str = str + " Order By VF.ShapeName";

        UtilityModule.ConditionalComboFill(ref DDShape, str, true, "---Plz Select---");
    }
    protected void FillShadeColor()
    {
        string str = @"Select Distinct VF.ShadeColorId, VF.ShadeColorName 
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
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
            str = str + " And a.PCIIssueID = " + DDChallanNo.SelectedValue;

            //str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
                    From PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERMASTER a(nolock) 
                    JOIN PUNCHCARDINDENT_ISSUEONPRODUCTIONORDERDETAIL b(nolock) ON b.PCIIssueid = a.PCIISSUEID
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
            str = str + " And a.PCIIssueID = " + DDChallanNo.SelectedValue;

            //str = str + " And a.IssueOrderid = " + DDChallanNo.SelectedValue;
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
       
        if (RDPunchCardIssueDetail.Checked == true)
        {
            ForIssueClick();
        }
        else if (RDPunchCardReceiveDetail.Checked == true)
        {
            ForReceiveClick();
        }
    }
    protected void ForIssueClick()
    {
        string str = "";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And OM.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And MID.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDWeaverName.SelectedIndex > 0)
        {
            //str = str + " And a.EmpID = " + DDWeaverName.SelectedValue;
            str = str + " And EI.EmpID in( " + DDWeaverName.SelectedValue + ")";
        }
        if (DDPOrderNo.SelectedIndex > 0)
        {
            str = str + " And MIM.FolioIssueOrderID = " + DDPOrderNo.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And MIM.PCIIssueID = " + DDChallanNo.SelectedValue;
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

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[3] = new SqlParameter("@ToDate", txttodate.Text);       
        param[4] = new SqlParameter("@Where", str);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardIssueDetailExcelReport", param);
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

            sht.Range("A1:L1").Merge();
            sht.Range("A1").SetValue("PUNCH CARD ISSUE DETAIL");
            sht.Range("A2:L2").Merge();
            sht.Range("A2").SetValue("FROM :" + txtfromDate.Text + " TO :" + txttodate.Text);
            sht.Range("A1:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L2").Style.Font.SetBold();

            //Headers
            sht.Range("A3").Value = "CHALLAN NO.";
            sht.Range("B3").Value = "ISSUE DATE";
            sht.Range("C3").Value = "ORDERNO";
            sht.Range("D3").Value = "EMP NAME";
            sht.Range("E3").Value = "FOLIO NO";
            sht.Range("F3").Value = "QUALITY";
            sht.Range("G3").Value = "DESIGN";
            sht.Range("H3").Value = "COLOR";
            sht.Range("I3").Value = "SIZE";
            sht.Range("J3").Value = "PER SETQTY";
            sht.Range("K3").Value = "NO OF SET";
            sht.Range("L3").Value = "REMARKS";
            //sht.Range("L3").Value = "TOTAL QTY";

            sht.Range("A3:L3").Style.Font.Bold = true;

            row = 4;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ChallanNo"].ToString());
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["IssueDate"].ToString());
        
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"].ToString());
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"].ToString());
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["FolioIssueOrderId"].ToString());
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"].ToString());
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"].ToString());
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"].ToString());
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Size"].ToString());
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["PerSetQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["NoOfSet"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Remarks"]);
                //sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["TotalIssueQty"]);

                row = row + 1;
            }           

            //sht.Range("I" + row).SetValue("G. Total");
            //sht.Range("L" + row).SetValue(ds.Tables[0].Compute("sum(TotalIssueQty)", ""));
           //// sht.Range("L" + row).SetValue(string.Format("{0:0.00}", ds.Tables[0].Compute("sum(TotalIssueQty)", "")));
            ////sht.Range("F" + row).FormulaA1 = "=SUM(" + TPcsRow + ")";
            ////sht.Range("G" + row).FormulaA1 = "=SUM(" + TAreaRow + ")";

            sht.Range("I" + row + ":L" + row).Style.Font.Bold = true;

            using (var a = sht.Range("A3:L" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 18).AdjustToContents();
            string Fileextension = "xlsx";
            string name = "PunchCardIssueDetailReport";
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    name = name + "-" + DDEmpName.SelectedItem.Text;
            //}
            string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
        
    }
    protected void ForReceiveClick()
    {
        string str = "";

        if (DDCustCode.SelectedIndex > 0)
        {
            str = str + " And OM.CustomerID = " + DDCustCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex > 0)
        {
            str = str + " And MID.OrderID = " + DDOrderNo.SelectedValue;
        }
        if (DDWeaverName.SelectedIndex > 0)
        {
            //str = str + " And a.EmpID = " + DDWeaverName.SelectedValue;
            str = str + " And EI.EmpID in( " + DDWeaverName.SelectedValue + ")";
        }
        if (DDPOrderNo.SelectedIndex > 0)
        {
            str = str + " And MIM.FolioIssueOrderID = " + DDPOrderNo.SelectedValue;
        }
        if (DDChallanNo.SelectedIndex > 0)
        {
            str = str + " And MIM.PCIReceiveID = " + DDChallanNo.SelectedValue;
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

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
        param[2] = new SqlParameter("@FromDate", txtfromDate.Text);
        param[3] = new SqlParameter("@ToDate", txttodate.Text);
        param[4] = new SqlParameter("@Where", str);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PunchCardReceiveDetailExcelReport", param);
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

            sht.Range("A1:L1").Merge();
            sht.Range("A1").SetValue("PUNCH CARD RECEIVE DETAIL");
            sht.Range("A2:L2").Merge();
            sht.Range("A2").SetValue("FROM :" + txtfromDate.Text + " TO :" + txttodate.Text);
            sht.Range("A1:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:L2").Style.Font.SetBold();

            //Headers
            sht.Range("A3").Value = "CHALLAN NO.";
            sht.Range("B3").Value = "RECEIVE DATE";
            sht.Range("C3").Value = "ORDERNO";
            sht.Range("D3").Value = "EMP NAME";
            sht.Range("E3").Value = "FOLIO NO";
            sht.Range("F3").Value = "QUALITY";
            sht.Range("G3").Value = "DESIGN";
            sht.Range("H3").Value = "COLOR";
            sht.Range("I3").Value = "SIZE";
            sht.Range("J3").Value = "PER SETQTY";
            sht.Range("K3").Value = "NO OF SET";
            //sht.Range("L3").Value = "TOTAL QTY";

            sht.Range("A3:L3").Style.Font.Bold = true;

            row = 4;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["RecChallanNo"].ToString());
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["ReceiveDate"].ToString());
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"].ToString());
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["EmpName"].ToString());
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["FolioChallanNo"].ToString());
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"].ToString());
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"].ToString());
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"].ToString());
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["Size"].ToString());
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["RecPerSetQty"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RecNoOfSet"]);
                //sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["TotalReceiveQty"]);

                row = row + 1;
            }

            //sht.Range("I" + row).SetValue("G. Total");
            //sht.Range("L" + row).SetValue(ds.Tables[0].Compute("sum(TotalReceiveQty)", ""));
            ////sht.Range("E" + row).SetValue("G. Total");
            ////sht.Range("F" + row).FormulaA1 = "=SUM(" + TPcsRow + ")";
            ////sht.Range("G" + row).FormulaA1 = "=SUM(" + TAreaRow + ")";

            sht.Range("I" + row + ":L" + row).Style.Font.Bold = true;

            using (var a = sht.Range("A3:K" + row))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            //*************
            sht.Columns(1, 18).AdjustToContents();
            string Fileextension = "xlsx";
            string name = "PunchCardReceiveDetailReport";
            //if (DDEmpName.SelectedIndex > 0)
            //{
            //    name = name + "-" + DDEmpName.SelectedItem.Text;
            //}
            string filename = UtilityModule.validateFilename("" + name + "_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
       
    }
    protected void RDPunchCardReceiveDetail_CheckedChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
    protected void RDPunchCardIssueDetail_CheckedChanged(object sender, EventArgs e)
    {
        FillWeaverName();
    }
    
}