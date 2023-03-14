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

public partial class Masters_ReportForms_Frm12EmpReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select UnitsId,UnitName from Units order by UnitName
                           select Month_Id,Month_Name from MonthTable 
                           select Year,Year from YearData";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessUnit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 3, true, "--Plz Select--");

            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }
            if (DDProcessUnit.Items.Count > 0)
            {
                DDProcessUnit.SelectedIndex = 1;
            }

            if (DDMonth.Items.Count > 0)
            {
                DDMonth.SelectedValue = currentMonth;
            }
            if (DDYear.Items.Count > 0)
            {
                DDYear.SelectedValue = currentYear;
            }

            //TxtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            // TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (DDProcessUnit.SelectedIndex > 0)
        {
            Report();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no not showing!');", true);
        }
    }
    private void Report()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[7];
        // array[0] = new SqlParameter("@FromDate", TxtFromDate.Text);
        // array[1] = new SqlParameter("@Todate", TxtToDate.Text);
        array[0] = new SqlParameter("@FromDate", DDMonth.SelectedValue);
        array[1] = new SqlParameter("@Todate", DDYear.SelectedValue);
        array[2] = new SqlParameter("@MasterCompanyId", Session["varcompanyId"]);
        array[3] = new SqlParameter("@UserId", Session["varuserId"]);
        array[4] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
        array[4].Direction = ParameterDirection.Output;
        //array[5] = new SqlParameter("@Where", null); 
        array[5] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
        array[6] = new SqlParameter("@UnitId", DDProcessUnit.SelectedIndex > 0 ? DDProcessUnit.SelectedValue : "0");

        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1; 

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetForm12EmpReportData", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptForm12EmpSalaryReport.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptForm12EmpSalaryReport.xsd";
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
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDMonth) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDYear) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        //lblcategoryname.Text = ParameterList[5];
        //lblitemname.Text = ParameterList[6];
        //lblqualityname.Text = ParameterList[0];
        //lbldesignname.Text = ParameterList[1];
        //lblcolorname.Text = ParameterList[2];
        //lblshapename.Text = ParameterList[3];
        //lblsizename.Text = ParameterList[4];
        //lblshadename.Text = ParameterList[7];
    }


}