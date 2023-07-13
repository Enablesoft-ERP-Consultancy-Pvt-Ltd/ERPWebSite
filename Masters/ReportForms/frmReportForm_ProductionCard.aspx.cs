using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Masters_ReportForms_frmReportForm_ProductionCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from YearData order by year desc", false, "");
            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();
            txtAdvancedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            switch (Session["varcompanyNo"].ToString())
            {
                case "8":
                    TRfromtoDate.Visible = false;
                    Trmonthyear.Visible = true;
                    Tradvamount.Visible = true;
                    tradvremark.Visible = true;
                    break;
                default:
                    TRfromtoDate.Visible = true;
                    Trmonthyear.Visible = false;
                    break;
            }
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            lblErrorMsg.Text = "";
            SqlParameter[] _array = new SqlParameter[14];
            _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[1] = new SqlParameter("@Month", SqlDbType.VarChar, 10);
            _array[2] = new SqlParameter("@Year", SqlDbType.VarChar, 10);
            _array[3] = new SqlParameter("@EmpCode", SqlDbType.VarChar, 20);
            _array[4] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[6] = new SqlParameter("@ForSummary", SqlDbType.Int);
            _array[7] = new SqlParameter("@AdvanceAmt", SqlDbType.Float);
            _array[8] = new SqlParameter("@AdvanceDate", SqlDbType.SmallDateTime);
            _array[9] = new SqlParameter("@varuserid", SqlDbType.Int);
            _array[10] = new SqlParameter("@advamountremark", SqlDbType.VarChar, 100);
            _array[11] = new SqlParameter("@fromdt", SqlDbType.DateTime);
            _array[12] = new SqlParameter("@ToDate", SqlDbType.DateTime);
            //_array[13] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", SqlDbType.Int);

            _array[0].Value = 1;// DDProcessName.SelectedValue;
            _array[1].Value = DDMonth.SelectedItem.Text;
            _array[2].Value = DDyear.SelectedItem.Text;
            _array[3].Value = txtIdNo.Text;
            _array[4].Value = Session["varCompanyId"];
            _array[5].Value = Session["CurrentWorkingCompanyID"];// DDCompany.SelectedValue;
            _array[6].Value = chksummary.Checked == true ? 1 : 0;//1 For show Summary ANd 0 For Detail
            _array[7].Value = txtAdvanceAmt.Text == "" ? "0" : txtAdvanceAmt.Text;
            _array[8].Value = txtAdvancedate.Text;
            _array[9].Value = Session["varuserid"].ToString();
            _array[10].Value = txtadvremark.Text;
            _array[11].Value = txtfromdate.Text;
            _array[12].Value = txttodate.Text;
            //_array[13].Value = 0;
            //if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            //{
            //    _array[13].Value = 1;
            //}
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_ForProductionCard", _array);

            if (ds.Tables[0].Rows.Count > 0)
            {
                // Table 2 For Show image in crystal Report
                ds.Tables[2].Columns.Add("Image", typeof(System.Byte[]));
                foreach (DataRow dr in ds.Tables[2].Rows)
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
                if (chksummary.Checked == false)
                {
                    if (Session["varcompanyNo"].ToString() == "8")
                    {
                        Session["rptFileName"] = "Reports/RptProductionCard.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "Reports/RptProductionCardNew.rpt";
                    }
                    Session["dsFileName"] = "~\\ReportSchema\\RptProductionCard.xsd";

                }
                else
                {
                    Session["dsFileName"] = "~\\ReportSchema\\RptProductionCardForSummary.xsd";
                    Session["rptFileName"] = "Reports/RptProductionCardForSummary.rpt";
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
            txtAdvanceAmt.Text = "";
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
}