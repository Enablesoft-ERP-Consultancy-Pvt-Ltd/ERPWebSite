using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Payroll_frmHolidaymaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId, CI.CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName
                            Select Year,Year as Year1 From YearData order  by Year1 desc 
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 2, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDyear, ds, 1, false, "");

            DDyear.SelectedValue = System.DateTime.Now.Year.ToString();

            txtchoosedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            Fill_Grid();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        save_update();
    }
    protected void lnklnkupdate(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int rowindex = row.RowIndex;

        Label lblcompanyid = (Label)GDDetail.Rows[rowindex].FindControl("lblcompanyid");
        Label lbldate = (Label)GDDetail.Rows[rowindex].FindControl("lbldate");
        Label lblholidaytype = (Label)GDDetail.Rows[rowindex].FindControl("lblholidaytype");
        Label lblid = (Label)GDDetail.Rows[rowindex].FindControl("lblid");
        hnid.Value = lblid.Text;
        //**
        DDCompanyName.SelectedValue = lblcompanyid.Text;
        txtchoosedate.Text = lbldate.Text;
        txtholidaytype.Text = lblholidaytype.Text;
        btnsave.Text = "Update";
    }
    protected void save_update()
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@Id", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnid.Value;
            param[1] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@choosedate", txtchoosedate.Text);
            param[3] = new SqlParameter("@Holidaytype", txtholidaytype.Text.ToString());
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

            //*********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveHRHolidaymaster", param);
            lblmsg.Text = param[6].Value.ToString();
            Tran.Commit();
            Fill_Grid();
            btnsave.Text = "Save";
            hnid.Value = "0";
            txtholidaytype.Text = "";
            txtchoosedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Fill_Grid()
    {
        string str = @"select Replace(convert(nvarchar(11),Date,106),' ','-') as Date,Holidaytype,ID,H.CompanyId 
            From HR_Holidaymaster H 
            WHere H.CompanyId=" + DDCompanyName.SelectedValue + " And H.BranchID = " + DDBranchName.SelectedValue + @" And 
            Year(H.Date)='" + DDyear.SelectedValue + "' order by H.Date";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDDetail.DataSource = ds.Tables[0];
        GDDetail.DataBind();
    }

    protected void DDyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select CI.companyName,HR.Date,HR.Holidaytype,'" + DDyear.SelectedValue + @"' as Year 
        From HR_Holidaymaster HR 
        inner Join CompanyInfo CI on HR.CompanyId=CI.CompanyId 
        where HR.BranchID = " + DDBranchName.SelectedValue + @" And Year(Date)=" + DDyear.SelectedValue + " and HR.CompanyId=" + DDCompanyName.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptHR_Holidaylist.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptHR_Holidaylist.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
    protected void GDDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = (Label)GDDetail.Rows[e.RowIndex].FindControl("lblid");
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@id", lblid.Text);
            param[1] = new SqlParameter("@Mastercompanyid", Session["varcompanyId"]);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //*********
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_DeleteHR_Holidaymaster", param);
            lblmsg.Text = param[3].Value.ToString();
            Tran.Commit();
            Fill_Grid();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}