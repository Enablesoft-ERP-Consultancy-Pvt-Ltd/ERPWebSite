using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
public partial class Masters_ReportForms_frmJobcard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //SqlhelperEnum.FillDropDown(AllEnums.MasterTables.PROCESS_NAME_MASTER, DDjob, pWhere: "MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " And Process_name_Id<>1", pID: "Process_name_Id", pName: "Process_name", pFillBlank: true, Selecttext: "--Plz select Job");

            UtilityModule.ConditionalComboFill(ref DDjob, @"select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                                                          order by PROCESS_NAME", true, "--Plz Select Process--");

            //UtilityModule.ConditionalComboFill(ref DDyear, "select Year,Year As Year1 From YearData Order by Year desc", false, "");
            //DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            //DDyear.SelectedValue = DateTime.Now.Year.ToString();
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (chkpaymentslip.Checked==true)
        {
            StockNowiseReport();
            return;
        }
        //**************************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            lblErrorMsg.Text = "";
            SqlParameter[] _array = new SqlParameter[8];
            _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
            _array[2] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            _array[3] = new SqlParameter("@EmpCode", SqlDbType.VarChar, 20);
            _array[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[6] = new SqlParameter("@processName", SqlDbType.VarChar, 30);
            _array[7] = new SqlParameter("@ForSummary", SqlDbType.Int);



            _array[0].Value = DDjob.SelectedValue;// DDProcessName.SelectedValue;
            _array[1].Value = txtfromDate.Text;
            _array[2].Value = txttodate.Text;
            _array[3].Value = txtIdNo.Text;
            _array[4].Value = Session["varMasterCompanyIDForERP"];
            _array[5].Value = 1;// DDCompany.SelectedValue;
            _array[6].Value = DDjob.SelectedItem.Text;
            _array[7].Value = chkForSummary.Checked == true ? 1 : 0;

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "[Pro_ForJobCard]", _array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                // Table 1 For Show image in crystal Report
                ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (Convert.ToString(dr["Photo"]) != "")
                    {
                        FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                        if (TheFile.Exists)
                        {
                            string img = dr["Photo"].ToString();
                            img = Server.MapPath(img);
                            Byte[] img_Byte = File.ReadAllBytes(img);
                            dr["Image"] = img_Byte;
                        }
                    }
                }
                if (chkForSummary.Checked == false)
                {
                    Session["dsFileName"] = "~\\ReportSchema\\RptProcessJobCard.xsd";
                    Session["rptFileName"] = "Reports/RptProcessJobCard.rpt";
                }
                else
                {
                    Session["dsFileName"] = "~\\ReportSchema\\RptProcessJobCardForSummary.xsd";
                    Session["rptFileName"] = "Reports/RptProcessJobCardForSummary.rpt";

                }
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);

            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void StockNowiseReport()
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {

            SqlCommand cmd = new SqlCommand("GET_PROCESS_HISSAB_STOCKNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@HISSABNO", ViewState["Hissab_No"]);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (Session["varcompanyNo"].ToString())
                {
                    case "16":
                        if (DDjob.SelectedValue == "1")
                        {
                            Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
                        }
                        else
                        {
                            Session["rptFileName"] = "Reports/rptProcesshissabstocknowise_otherjob.rpt";
                        }

                        break;
                    default:
                        Session["rptFileName"] = "Reports/rptprocesshissabstocknowise.rpt";
                        break;
                }

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptprocesshissabstocknowise.xsd";
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", ex.Message, true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkpaymentslip_CheckedChanged(object sender, EventArgs e)
    {
        TDslipno.Visible = false;
        if (chkpaymentslip.Checked == true)
        {
            TDslipno.Visible = true;
            FillSlipno();
        }
    }
    protected void FillSlipno()
    {
        if (TDslipno.Visible == true)
        {
            string str = @"Select Distinct PH.HissabNo,PH.HissabNo as HissabNo1 from PROCESS_HISSAB PH inner join empinfo ei on PH.empid=ei.empid
                      Where CommPaymentFlag=0  And ProcessID=" + DDjob.SelectedValue + " And EI.EMpcode='" + txtIdNo.Text + "'";
            if (txtfromDate.Text != "")
            {
                str = str + " and PH.FromDate>='" + txtfromDate.Text + "'";
            }
            if (txttodate.Text != "")
            {
                str = str + " and PH.Todate<='" + txttodate.Text + "'";
            }
            str = str + " order by HissabNo1";
            UtilityModule.ConditionalComboFill(ref DDslipNo, str, true, "--SELECT--");
        }
    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        FillSlipno();
    }
    protected void DDslipNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Hissab_No"] = DDslipNo.SelectedValue;

    }
}