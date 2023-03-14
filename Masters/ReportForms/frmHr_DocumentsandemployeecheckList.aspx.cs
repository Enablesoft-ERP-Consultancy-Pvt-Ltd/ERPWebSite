using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
public partial class Masters_ReportForms_frmHr_DocumentsandemployeecheckList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                From Companyinfo CI(nolock) 
                JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order by CI.CompanyName 
                SELECT Distinct a.SRNO, [DESCRIPTION] 
                FROM HR_employeechecklist a(Nolock) 
                JOIN HR_EMPLOYEECHECKLISTBRANCHWISE b ON b.SRNO = a.SRNO 
                JOIN BranchUser BU(nolock) ON BU.BranchID = b.BRANCHID And BU.UserID = " + Session["varuserId"] + @" 
                order by a.Srno 
                Select ID, BranchName 
                From BRANCHMASTER BM(nolock) 
                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDocuments, ds, 1, true, "--Plz select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 2, false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (txtempid.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Please Enter Employee code');", true);
            return;
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Top 1 1 
            From Empinfo EI(Nolock)
            JOIN HR_EmployeeInformation HEI(Nolock) ON HEI.EMPID = EI.EmpID And HEI.CompanyID = " + DDcompany.SelectedValue + @" 
            Where EI.EmpID = " + txtempid.Text + " And EI.BranchID = " + DDBranchName.SelectedValue);
        if (Ds.Tables[0].Rows.Count < 1)
        {
            txtempcode_name.Text = "";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('Employee code is not exists in this company or branch');", true);
            return;
        }
        try
        {
            SqlParameter[] arr = new SqlParameter[3];
            DataSet DS;
            switch (DDDocuments.SelectedValue)
            {
                case "1": //BIO DATA
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_BIODATA", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DS.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (Convert.ToString(dr["empphoto"]) != "")
                            {
                                FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
                                if (TheFile.Exists)
                                {
                                    string img = dr["empphoto"].ToString();
                                    img = Server.MapPath(img);
                                    Byte[] img_Byte = File.ReadAllBytes(img);
                                    dr["Image"] = img_Byte;
                                }
                            }
                        }

                        Session["rptFilename"] = "Reports/Rpt_Hr_BioDataForm.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_Hr_BioDataForm.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "2": //Appointment Letter                    
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETAPPOINTMENTDATA", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(DDBranchName.SelectedValue) == 2)
                        {
                            Session["rptFilename"] = "Reports/rpt_HrappointmentForPanipat.rpt";
                        }
                        else if (DS.Tables[0].Rows[0]["Wagescalculation"].ToString() == "3") //PCS Wise
                        {
                            Session["rptFilename"] = "Reports/rpt_Hrappointmentworkcontact.rpt";
                        }
                        else
                        {
                            Session["rptFilename"] = "Reports/rpt_Hrappointment.rpt";
                        }
                        Session["dsFilename"] = "~\\ReportSchema\\rpt_Hrappointment.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "3": //FOrm F
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FORMF_NOMINATION", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        Session["rptFilename"] = "Reports/Rpt_HR_FormFNomination.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_FormFNomination.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "4": //Form 16
                case "21": //Form D
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FORM16", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(DDBranchName.SelectedValue) == 1)
                        {
                            Session["rptFilename"] = "Reports/Rpt_HR_Form16.rpt";
                        }
                        else
                        {
                            Session["rptFilename"] = "Reports/Rpt_HR_Form16ForPanipat.rpt";
                        }
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_Form16.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "7": // EmploymentBackgroundscreening
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_HR_EmploymentBackgroundscreening", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        Session["rptFilename"] = "Reports/Rpt_Hr_EmployementBackgroundScreening.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_Hr_EmployementBackgroundScreening.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "8": // Application Form
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_APPLICATIONFORM", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        Session["rptFilename"] = "Reports/Rpt_Hr_Applicationform.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_Hr_Applicationform.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "9": //FOrm 2
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FORM2_NOMINATIONANDDECLARATION", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        Session["rptFilename"] = "Reports/Rpt_HR_NominationAndDeclarationForm.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_NominationAndDeclarationForm.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "10": //FOrm 11
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FORM11", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFilename"] = "Reports/Rpt_HR_Form_11.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_Form_11.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "12": //INDUCTION FORM
                   arr[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@REPORTNAME", "INDUCTIONFORM");
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEECHECKLIST", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFilename"] = "Reports/Rpt_HR_INDUCTIONFORM.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\PRO_HR_GETEMPLOYEECHECKLIST.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "16": // Previous employment
                    arr[0] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_PREVIOUSEMPLOYMENTHISTORY", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {

                        Session["rptFilename"] = "Reports/Rpt_HR_PreEmploymentHistory.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_PreEmploymentHistory.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "17": // Service Record
                    arr[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETSERVICERECORD", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(DDBranchName.SelectedValue) == 2)
                        {
                            Session["rptFilename"] = "Reports/Rpt_HR_ServiceRecordForPanipat.rpt";
                        }
                        else
                        {
                            Session["rptFilename"] = "Reports/Rpt_HR_ServiceRecord.rpt";
                        }
                        
                        Session["dsFilename"] = "~\\ReportSchema\\Rpt_HR_ServiceRecord.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "18": // Hand Book
                    arr[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@REPORTNAME", "HANDBOOK");
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEECHECKLIST", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFilename"] = "Reports/Rpt_HR_HANDBOOK.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\PRO_HR_GETEMPLOYEECHECKLIST.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "19": // EMPLOYEE BENIFIT DEDUCTION
                    arr[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@REPORTNAME", "EMPLOYEEBENIFITDEDUCTION");
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEECHECKLIST", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFilename"] = "Reports/Rpt_HR_EMPLOYEEBENIFITDEDUCTION.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\PRO_HR_GETEMPLOYEECHECKLIST.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "20": // EMPLOYEE PERSONAL DETAIL
                    arr[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@REPORTNAME", "EMPLOYEEPERSONALDETAIL");
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEECHECKLIST", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DS.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (Convert.ToString(dr["empphoto"]) != "")
                            {
                                FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
                                if (TheFile.Exists)
                                {
                                    string img = dr["empphoto"].ToString();
                                    img = Server.MapPath(img);
                                    Byte[] img_Byte = File.ReadAllBytes(img);
                                    dr["Image"] = img_Byte;
                                }
                            }
                        }

                        Session["rptFilename"] = "Reports/Rpt_HR_EMPLOYEEPERSONALDETAIL.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\PRO_HR_GETEMPLOYEECHECKLIST.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                case "22": // FORM V
                    arr[0] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                    arr[1] = new SqlParameter("@empid", txtempid.Text == "" ? "0" : txtempid.Text);
                    arr[2] = new SqlParameter("@REPORTNAME", "FORMV");
                    DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEECHECKLIST", arr);

                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DS.Tables[0].Columns.Add("Image", typeof(System.Byte[]));
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (Convert.ToString(dr["empphoto"]) != "")
                            {
                                FileInfo TheFile = new FileInfo(Server.MapPath(dr["empphoto"].ToString()));
                                if (TheFile.Exists)
                                {
                                    string img = dr["empphoto"].ToString();
                                    img = Server.MapPath(img);
                                    Byte[] img_Byte = File.ReadAllBytes(img);
                                    dr["Image"] = img_Byte;
                                }
                            }
                        }

                        Session["rptFilename"] = "Reports/Rpt_HR_FORM5.rpt";
                        Session["dsFilename"] = "~\\ReportSchema\\PRO_HR_GETEMPLOYEECHECKLIST.xsd";
                        Session["GetDataset"] = DS;
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
                    }
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {

            lblmsg.Text = ex.Message;
        }
    }
}