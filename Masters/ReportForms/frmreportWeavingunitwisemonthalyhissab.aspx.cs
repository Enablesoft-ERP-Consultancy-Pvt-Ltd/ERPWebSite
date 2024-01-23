using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using System.Text;
public partial class Masters_ReportForms_frmreportWeavingunitwisemonthalyhissab : System.Web.UI.Page
{
    public static string Export = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                           
                           select UnitsId,UnitName from Units order by UnitName
                           SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESSTYPE=1 ORDER BY PROCESS_NAME
                           SELECT ICM.CATEGORY_ID,ICM.CATEGORY_NAME FROM ITEM_CATEGORY_MASTER ICM INNER JOIN CATEGORYSEPARATE CS ON ICM.CATEGORY_ID=CS.CATEGORYID AND CS.ID=0 order by CATEGORY_NAME
                           Select Distinct D.DepartmentId, D.DepartmentName 
                           From Department D(Nolock)
                           JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                           JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                           Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                           Order By D.DepartmentName
                           Select ID, BranchName 
                           From BRANCHMASTER BM(nolock) 
                           JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                           Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, ds, 4, true, "--Plz Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 5, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            str = @"select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME, 1 TypeID  
                From PROCESS_NAME_MASTER PNM(Nolock) 
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                Where PNM.ProcessType = 1";
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28)
            {
                str = str + @" UNION Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME, 2 TypeID  
                        From PROCESS_NAME_MASTER PNM(Nolock) 
                        JOIN MOTTELINGISSUEMASTER MIMA(Nolock) ON MIMA.ProcessID = PNM.PROCESS_NAME_ID 
                        UNION
                        Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME, 2 TypeID  
                        From PROCESS_NAME_MASTER PNM(Nolock) 
                        Where PROCESS_NAME_ID = 17 ";
            }
            str = str + @" Order By TypeID, PROCESS_NAME ";

            UtilityModule.ConditonalChkBoxListFill(ref chkprocessname, str);

            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //Export
            switch (variable.ReportWithpdf)
            {
                case "1":
                    chkexport.Visible = true;
                    Export = "N";
                    break;
                default:
                    chkexport.Visible = false;
                    Export = "Y";
                    break;
            }
            if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 && Convert.ToInt32(Session["varSubCompanyId"]) == 281 && (Convert.ToInt32(Session["varuserid"]) == 26 || Convert.ToInt32(Session["varuserid"]) == 89))
            {
                RDPAYMENTSAVE.Visible = true;
                BtnSave.Visible = true;
            }
            else if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 && Convert.ToInt32(Session["varSubCompanyId"]) == 282 && (Convert.ToInt32(Session["varuserid"]) == 4 || Convert.ToInt32(Session["varuserid"]) == 5 || Convert.ToInt32(Session["varuserid"]) == 6))
            {
                RDPAYMENTSAVE.Visible = true;
                BtnSave.Visible = true;
            }
            else if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 && Convert.ToInt32(Session["varSubCompanyId"]) == 283 && (Convert.ToInt32(Session["varuserid"]) == 9 || Convert.ToInt32(Session["varuserid"]) == 10 || Convert.ToInt32(Session["varuserid"]) == 11))
            {
                RDPAYMENTSAVE.Visible = true;
                BtnSave.Visible = true;
            }
            else if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 && Convert.ToInt32(Session["varSubCompanyId"]) == 283 && (Convert.ToInt32(Session["varuserid"]) == 4 || Convert.ToInt32(Session["varuserid"]) == 7 || Convert.ToInt32(Session["varuserid"]) == 8 || Convert.ToInt32(Session["varuserid"]) == 9))
            {
                RDPAYMENTSAVE.Visible = true;
                BtnSave.Visible = true;
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (RDProduction.Checked == true)
        {
            Production();
        }
        else if (RDPAYMENT.Checked == true)
        {
            Weaverwisepaymentfinal();
        }
        else if (RDPAYMENTSAVE.Checked == true)
        {
            Weaverwisepaymentfinal();
        }
    }

    protected void Production()
    {
        string str = "select *,'" + txtfromdate.Text + "' as FromDate,'" + txttodate.Text + "' as ToDate From V_weavingunitwisehissab Where ReceiveDate>='" + txtfromdate.Text + "' and Receivedate<='" + txttodate.Text + "'";
        if (DDcompany.SelectedIndex > 0)
        {
            str = str + " and companyid=" + DDcompany.SelectedValue;
        }
        if (DDProdunit.SelectedIndex > 0)
        {
            str = str + " and units=" + DDProdunit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and Loomid=" + DDLoomNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and Category_id=" + DDCategory.SelectedValue;
        }
        if (DDitemName.SelectedIndex > 0)
        {
            str = str + " and Item_id=" + DDitemName.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rptwagesverificationunitwise.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptwagesverificationunitwise.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            if (chkexport.Checked == true)
            {
                Export = "Y";
            }
            else
            {
                switch (variable.ReportWithpdf)
                {
                    case "1":
                        Export = "N";
                        break;
                    default:
                        Export = "Y";
                        break;
                }
            }
            stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);

        }
    }
    protected void Weaverwisepaymentfinal()
    {
        string str = "";
        if (DDProdunit.SelectedIndex > 0)
        {
            str = str + " and UnitsId=" + DDProdunit.SelectedValue;
        }
        if (DDLoomNo.SelectedIndex > 0)
        {
            str = str + " and UID=" + DDLoomNo.SelectedValue;
        }
        if (DDCategory.SelectedIndex > 0)
        {
            str = str + " and vf.category_id=" + DDCategory.SelectedValue;
        }
        //if (DDitemName.SelectedIndex > 0)
        //{
        //    str = str + " and vf.item_id=" + DDitemName.SelectedValue;
        //}
        string Processid = "";
        for (int i = 0; i < chkprocessname.Items.Count; i++)
        {
            if (chkprocessname.Items[i].Selected)
            {
                Processid = Processid == null ? chkprocessname.Items[i].Value : Processid + "," + chkprocessname.Items[i].Value;
            }
        }
        string ReportType = "", ReportTypeNew = "";
        if (Session["varMasterCompanyIDForERP"].ToString() == "28")
        {
            ReportType = DDReportType.SelectedIndex > 0 ? DDReportType.SelectedValue : "0";
            ReportTypeNew = RDPAYMENTSAVE.Checked == true ? "1" : "0";
        }
        else
        {
            ReportType = "0";
            ReportTypeNew = "0";
        }

        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETJOBWISEPAYEMENTDETAIL", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 30000;

        cmd.Parameters.AddWithValue("@CompanyId", DDcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
        cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.AddWithValue("@PROCESSIDCOMMSEP", Processid == "" ? "1" : Processid.TrimStart(','));
        cmd.Parameters.AddWithValue("@Itemid", DDitemName.SelectedIndex > 0 ? DDitemName.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@DepartmentID", DDDepartmentName.SelectedIndex > 0 ? DDDepartmentName.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@ReportType", ReportType);
        cmd.Parameters.AddWithValue("@ReportTypeNEW", ReportTypeNew);
        cmd.Parameters.AddWithValue("@BranchID", DDBranchName.SelectedValue);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDReportType.SelectedValue == "0")
            {
                switch (Session["varMasterCompanyIDForERP"].ToString())
                {
                    case "16":
                        Session["rptFileName"] = "~\\Reports\\rptweaverwisepaymentfinalforchampo.rpt";
                        break;
                    case "28":
                        DateTime FromDate = new DateTime(2020, 12, 15);
                        if (Convert.ToDateTime(txtfromdate.Text) >= FromDate)
                        {
                            Session["rptFileName"] = "~\\Reports\\rptweaverwisepaymentfinalforchampoWithPenalityNew.rpt";
                        }
                        else
                        {
                            Session["rptFileName"] = "~\\Reports\\rptweaverwisepaymentfinalforchampoWithPenality.rpt";
                            //rptweaverwisepaymentfinalforchampoWithPenality1
                        }
                        break;
                    default:
                        Session["rptFileName"] = "~\\Reports\\rptweaverwisepaymentfinal.rpt";
                        break;
                }
                Session["dsFileName"] = "~\\ReportSchema\\rptweaverwisepaymentfinal.xsd";
            }
            else
            {
                ds.Tables[0].Columns.Add("image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["CompanyLogo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["CompanyLogo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["CompanyLogo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["image"] = img_Byte;
                        }
                    }
                }

                if (DDReportType.SelectedValue == "1")
                {
                    Session["rptFileName"] = "~\\Reports\\RPT_HR_SALARYTRANSFER_EMPBANKDETAIL.rpt";
                    Session["dsFileName"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_EMPBANKDETAIL.xsd";
                }
                else if (DDReportType.SelectedValue == "2")
                {
                    Session["rptFilename"] = "Reports/RPT_HR_SALARYTRANSFER_EMPBANKDETAILSAME.rpt";
                    Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_EMPBANKDETAILSAME.xsd";
                }
                else if (DDReportType.SelectedValue == "3")
                {
                    Session["rptFilename"] = "Reports/RPT_HR_SALARYTRANSFER_CASHSALARY.rpt";
                    Session["dsFilename"] = "~\\ReportSchema\\RPT_HR_SALARYTRANSFER_CASHSALARY.xsd";
                }
            }
            Session["Getdataset"] = ds;

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            if (chkexport.Checked == true)
            {
                Export = "Y";
            }
            stb.Append("window.open('../../ViewReport.aspx?Export=" + Export + "', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void WeaverwisepaymentfinalSave()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_SAVE_HissabEmployeeProcessDateWise", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@CompanyId", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.AddWithValue("@DATAOVERWRITEFLAG", ChkForSaveCurrentData.Checked == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@Msg"].Value.ToString() + "');", true);
                Tran.Rollback();
            }
            else
            {
                Tran.Commit();
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Data Successfully saved');", true);
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + ex.Message + "');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDLoomNo, @"select  PM.UID,PM.loomno as LoomNo from ProductionLoomMaster PM                                             
                                            Where  PM.CompanyId=" + DDcompany.SelectedValue + " and PM.UnitId=" + DDProdunit.SelectedValue + " order by case when ISNUMERIC(PM.loomno)=1 Then CONVERT(int,PM.loomno) Else 9999999 End,PM.loomno", true, "--Plz Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDitemName, "SELECT ITEM_ID,ITEM_NAME FROM ITEM_MASTER WHERE CATEGORY_ID=" + DDCategory.SelectedValue + " ORDER BY ITEM_NAME", true, "--Plz Select--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        WeaverwisepaymentfinalSave();
    }
}