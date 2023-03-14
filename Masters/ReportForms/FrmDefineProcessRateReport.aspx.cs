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
using System.Text.RegularExpressions;
using System.Text;

public partial class Masters_ReportForms_FrmDefineProcessRateReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" and Process_Name='Weaving' Order By PROCESS_NAME
                            select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory
                            select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDOrderType, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "--Select--");            

            //txtFromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
            }
        }
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, @"select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                                    cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid = " + Session["varcompanyid"] + @" and Category_Id= " + DDCategory.SelectedValue + "", true, "--Plz Select--");

    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct vf.Qualityid,vf.Qualityname from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDItemName.SelectedValue + " And QualityId<>0 order by vf.Qualityname", true, "--Plz Select--");
                      
    }
    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct designId,designName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDItemName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " And Designid<>0 order by designName", true, "--Plz Select--");
        
    }    

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        ProcessWiseHissabPaymentSummary();
       
    }
    private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
    public static string validateFilename(string filename)
    {
        return InvalidFileRegex.Replace(filename, string.Empty);
    }
    protected void ProcessWiseHissabPaymentSummary()
    {
        try
        {
            string str = "";

            str = str + "  And Tj.OrderTypeId= " + DDOrderType.SelectedValue;

            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + "  And Tj.Jobid= " + DDProcessName.SelectedValue;
            }
            if (DDItemName.SelectedIndex > 0)
            {
                str = str + "  And vf.Item_Id= " + DDItemName.SelectedValue;
            }
            if (ddquality.SelectedIndex > 0)
            {
                str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
            }

            if (DDRatetype.SelectedIndex != -1)
            {
                str = str + "  And TJ.Ratetype= " + DDRatetype.SelectedValue;
            }

            str = str + "  And TJ.RateLocation = " + DDRateLocation.SelectedValue;


            //*****************
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@where", str);
            param[2] = new SqlParameter("@CurrentRate", chkcurrentrate.Checked == true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_DefineProcessRateReport", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptDefineJobRateVikramMirzapur.rpt";

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptDefineJobRateVikramMirzapur.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "RawBal", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    
}
