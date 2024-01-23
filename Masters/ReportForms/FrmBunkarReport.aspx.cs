using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_ReportForms_FrmBunkarReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //TRlotNo.Visible = false;

            ViewState["OrderId"] = 0;

            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Companyname 
                           SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE order by Month_Id
                           SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA order by Year";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            // CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 2, true, "--Select--");
            //TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");          
            int varcompanyNo = Convert.ToInt16(Session["varMasterCompanyIDForERP"].ToString());

            if (DDMonth.Items.Count > 0)
            {
                DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            }
            if (DDYear.Items.Count > 0)
            {
                DDYear.SelectedValue = DateTime.Now.Year.ToString();
            }
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            BindCategory();
            BindContractor();
        }
    }
    protected void BindContractor()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand("PRO_DDBINDCONTRACTOR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            DDContractorName.DataSource = dr;
                            DDContractorName.DataTextField = "EmpName";
                            DDContractorName.DataValueField = "EmpId";
                            DDContractorName.DataBind();
                            DDContractorName.Items.Insert(0, new ListItem("--Select--", "0"));
                        }
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error:" + ex.Message.ToString());
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindContractor();
        ddItemName.Items.Clear();
        DDBunkarName.Items.Clear();
    }
    private void BindBunkarName()
    {
        string str = "";

        str = @" select BMID,BunkarName from bunkarmaster Where Status=0 and ContractorId=" + DDContractorName.SelectedValue + " Order by BunkarName";

        UtilityModule.ConditionalComboFill(ref DDBunkarName, str, true, "-----------Select------");
    }
    protected void DDContractorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBunkarName();
    }
    protected void DDBunkarName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();
    }
    protected void BindCategory()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, "select ICM.CATEGORY_ID,ICM.CATEGORY_NAME From Item_category_Master ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=CS.Categoryid and CS.id=0", true, "--Plz Select--");

        if (DDCategory.Items.Count > 0)
        {
            DDCategory.SelectedIndex = 1;
            BindItem();
        }

    }

    protected void BindItem()
    {
        string Str1 = "";

        Str1 = @"select distinct BM.Item_Id,IM.Item_Name From BunkarPaymentMaster BM INNER JOIN ITEM_MASTER IM ON BM.Item_Id=IM.Item_ID Where 1=1";

        if (DDCompany.SelectedIndex > 0)
        {
            Str1 = Str1 + " And BM.companyid=" + DDCompany.SelectedValue;
        }
        if (DDContractorName.SelectedIndex > 0)
        {
            Str1 = Str1 + " And BM.ContractorId=" + DDContractorName.SelectedValue;
        }
        if (DDBunkarName.SelectedIndex > 0)
        {
            Str1 = Str1 + " And BM.BunkarId=" + DDBunkarName.SelectedValue;
        }
        Str1 = Str1 + " order by Item_name";
        UtilityModule.ConditionalComboFill(ref ddItemName, Str1, true, "--Select--");

    }

    protected void FillQuality()
    {
        string Str1 = "";

        Str1 = @"Select Distinct Q.QualityId,Q.QualityName from BunkarPaymentMaster BM INNER JOIN BunkarPaymentDetail BD ON BM.ID=BD.ID
                    INNER JOIN V_FinishedItemDetailNew VF ON BD.Item_Finished_Id=VF.ITEM_FINISHED_ID
                    INNER JOIN Quality Q ON VF.Qualityid=Q.QualityID Where 1=1";

        if (ddItemName.SelectedIndex > 0)
        {
            Str1 = Str1 + " And Q.Item_Id=" + ddItemName.SelectedValue;
        }
        Str1 = Str1 + " Order by Q.QualityName";
        UtilityModule.ConditionalComboFill(ref DDQuality, Str1, true, "--Select--");

    }

    //protected void FillGrid()
    //{
    //    DataSet ds = new DataSet();
    //    string strCondition = "";
    //    ////Check Conditions
    //    //if (ChkForDate.Checked == true)
    //    //{
    //    //    strCondition = strCondition + " And PM.Assigndate>='" + TxtFromDate.Text + "' And PM.Assigndate<='" + TxtToDate.Text + "'";
    //    //}
    //    if (TRcustcode.Visible == true && DDcustcode.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And OM.Customerid=" + DDcustcode.SelectedValue;
    //    }

    //    if (DDCategory.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue;
    //    }
    //    if (ddItemName.SelectedIndex > 0)
    //    {
    //        strCondition = strCondition + " And VF.ITEM_ID=" + ddItemName.SelectedValue;
    //    }
    //    if (DDQuality.SelectedIndex > 0 && TRDDQuality.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.QualityId=" + DDQuality.SelectedValue;
    //    }
    //    if (DDDesign.SelectedIndex > 0 && TRDDDesign.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.designId=" + DDDesign.SelectedValue;
    //    }
    //    if (DDColor.SelectedIndex > 0 && TRDDColor.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ColorId=" + DDColor.SelectedValue;
    //    }
    //    if (DDShape.SelectedIndex > 0 && TRDDShape.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ShapeId=" + DDShape.SelectedValue;
    //    }
    //    if (DDSize.SelectedIndex > 0 && TRDDSize.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.SizeId=" + DDSize.SelectedValue;
    //    }
    //    if (DDShadeColor.SelectedIndex > 0 && TRDDShadeColor.Visible == true)
    //    {
    //        strCondition = strCondition + " And VF.ShadecolorId=" + DDShadeColor.SelectedValue;
    //    }
    //    //End Conditions
    //    SqlParameter[] param = new SqlParameter[5];
    //    param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
    //    param[1] = new SqlParameter("@Dateflag", ChkForDate.Checked == true ? "1" : "0");
    //    param[2] = new SqlParameter("@FromDate", TxtFromDate.Text);
    //    param[3] = new SqlParameter("@Todate", TxtToDate.Text);
    //    param[4] = new SqlParameter("@Where", strCondition);

    //    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Fill_CustomerOrderWith_ProductionReportMaltiRugs", param);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        GDOrderDetail.DataSource = ds;
    //        GDOrderDetail.DataBind();
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
    //    }

    //}
    //protected void BtnShowData_Click(object sender, EventArgs e)
    //{
    //    FillGrid();        
    //}
    //private void CHECKVALIDCONTROL()
    //{
    //    lblMessage.Text = "";
    //    //if (UtilityModule.VALIDDROPDOWNLIST(DDProcessName) == false)
    //    //{
    //    //    goto a;
    //    //}
    //    if (UtilityModule.VALIDTEXTBOX(TxtFromDate) == false)
    //    {
    //        goto a;
    //    }
    //    if (UtilityModule.VALIDTEXTBOX(TxtToDate) == false)
    //    {
    //        goto a;
    //    }
    //    else
    //    {
    //        goto B;
    //    }
    //a:
    //    UtilityModule.SHOWMSG(lblMessage);
    //B: ;
    //}
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void ShowParameters()
    {
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
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
                    case "4":
                        TRDDShape.Visible = false;
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        break;
                    case "6":
                        TRDDShadeColor.Visible = false;
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowParameters();
        FillQuality();
        DDDesign.Items.Clear();
        DDColor.Items.Clear();
        DDSize.Items.Clear();
        //if (DDQuality.Items.Count > 0)
        //{
        //    DDQuality.SelectedIndex = 0;
        //    DDQuality_SelectedIndexChanged(sender, new EventArgs());
        //}
        ////************
        //UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,ShapeName From Shape", true, "--Plz Select--");
    }
    protected void FillDesign()
    {
        string Str = "";

        Str = @"Select Distinct D.DesignId,D.DesignName from BunkarPaymentMaster BM INNER JOIN BunkarPaymentDetail BD ON BM.ID=BD.ID
                INNER JOIN V_FinishedItemDetailNew VF ON BD.Item_Finished_Id=VF.ITEM_FINISHED_ID
                INNER JOIN Design D ON VF.DesignId=D.designId Where 1=1";

        if (DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityID=" + DDQuality.SelectedValue;
        }
        Str = Str + " Order by D.DesignName";
        UtilityModule.ConditionalComboFill(ref DDDesign, Str, true, "--Select--");

    }
    protected void FillColor()
    {
        string Str = "";

        Str = @"Select Distinct C.ColorId,C.ColorName from BunkarPaymentMaster BM INNER JOIN BunkarPaymentDetail BD ON BM.ID=BD.ID
                INNER JOIN V_FinishedItemDetailNew VF ON BD.Item_Finished_Id=VF.ITEM_FINISHED_ID
                INNER JOIN Color C ON VF.ColorId=C.ColorId Where 1=1";

        if (DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.DesignId=" + DDDesign.SelectedValue;
        }
        Str = Str + " Order by C.ColorName";
        UtilityModule.ConditionalComboFill(ref DDColor, Str, true, "--Select--");

    }
    protected void FillSize()
    {

        string Str = "";

        Str = @"Select Distinct QS.SizeId,QS.Production_Ft_Format+'['+CAST(QS.SizeId as varchar)+']' as Production_Ft_Format from BunkarPaymentMaster BM 
                INNER JOIN BunkarPaymentDetail BD ON BM.ID=BD.ID
                INNER JOIN V_FinishedItemDetailNew VF ON BD.Item_Finished_Id=VF.ITEM_FINISHED_ID
                INNER JOIN QualitySizeNew QS ON VF.SizeId=QS.SizeId Where 1=1";

        if (DDQuality.SelectedIndex > 0)
        {
            Str = Str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        Str = Str + " Order by Production_Ft_Format";
        UtilityModule.ConditionalComboFill(ref DDSize, Str, true, "--Select--");

    }
    //protected void chkmtr_CheckedChanged(object sender, EventArgs e)
    //{
    //    FillSize();
    //}
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
        FillColor();
        FillSize();

    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblshadename.Text = ParameterList[7];
    }
    protected void RDBunkarDetail_CheckedChanged(object sender, EventArgs e)
    {
        //if (RDBunkarDetail.Checked == true)
        //{            
        //    TRcustcode.Visible = true;          
        //    ChkForDate.Checked = false;
        //    //ChkForDate_CheckedChanged(sender, e);
        //    //UtilityModule.ConditionalComboFill(ref DDEmpName, "Select Distinct EI.EmpId,EI.EmpName from Empinfo EI,PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + " PIM WHERE PIM.EmpId=EI.EmpId And CompanyId=" + DDCompany.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName", true, "--Select--");

        //    BindCategory(); 

        //}
    }
    protected void RDBunkarWise_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void RDQualityWise_CheckedChanged(object sender, EventArgs e)
    {

    }
    private DataSet GetReportData()
    {
        string str = "";
        //str = " Where BM.CompanyId=" + DDCompany.SelectedValue;
        str = " Where BM.MonthId=" + DDMonth.SelectedValue + " and BM.Year=" + DDYear.SelectedValue + "";
        if (DDCompany.SelectedIndex > 0)
        {
            str = str + " and BM.CompanyId=" + DDCompany.SelectedValue;
        }
        if (DDContractorName.SelectedIndex > 0)
        {
            str = str + " and BM.ContractorId=" + DDContractorName.SelectedValue;
        }
        if (DDBunkarName.SelectedIndex > 0)
        {
            str = str + " and BM.BunkarId=" + DDBunkarName.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and VF.Category_id=" + DDCategory.SelectedValue;
        }
        if (ddItemName.SelectedIndex > 0)
        {
            str = str + " and VF.Item_Id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and vf.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + DDSize.SelectedValue;
        }
        if (DDReportType.SelectedIndex > 0)
        {
            if (DDReportType.SelectedValue == "1")
            {
                str = str + " and PRM.SampleFlag=0";
            }
            else if (DDReportType.SelectedValue == "2")
            {
                str = str + " and PRM.SampleFlag=1";
            }
        }

        DataSet ds = null;

        //*****************
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
        param[1] = new SqlParameter("@UserID", Session["VarUserId"]);
        param[2] = new SqlParameter("@Where", str);
        param[3] = new SqlParameter("@ContractorId", DDContractorName.SelectedValue);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetBunkarReportData", param);

        return ds;
    }
    protected void GetBunkarDetailReport()
    {
        DataSet ds = GetReportData();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBunkarDetailReport.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBunkarDetailReport.xsd";
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
    protected void GetBunkarWiseReport()
    {
        DataSet ds = GetReportData();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBunkarWiseReport.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBunkarWiseReport.xsd";
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
    protected void GetBunkarQualityWiseReport()
    {
        DataSet ds = GetReportData();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBunkarQualityWiseReport.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBunkarQualityWiseReport.xsd";
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
    protected void GetBunkarVoucherSummaryReport()
    {
        DataSet ds = GetReportData();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBunkarVoucherSummaryReport.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBunkarVoucherSummaryReport.xsd";
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
    protected void GetBunkarVoucherReport()
    {
        DataSet ds = GetReportData();

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptBunkarVoucherReport.rpt";

            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptBunkarVoucherReport.xsd";
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
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDBunkarDetail.Checked == true)
        {
            GetBunkarDetailReport();
        }
        else if (RDBunkarWise.Checked == true)
        {
            GetBunkarWiseReport();
        }
        else if (RDQualityWise.Checked == true)
        {
            GetBunkarQualityWiseReport();
        }
        else if (RDVoucherSummary.Checked == true)
        {
            if (ChkForVoucher.Checked == true)
            {
                GetBunkarVoucherReport();
            }
            else
            {
                GetBunkarVoucherSummaryReport();
            }
        }
    }
}