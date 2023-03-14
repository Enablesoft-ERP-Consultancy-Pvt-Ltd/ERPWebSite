using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Masters_RawMaterial_Frmfinishingmaterialpreparation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {

            string str = @"SELECT CI.COMPANYID,CI.COMPANYNAME FROM COMPANYINFO CI INNER JOIN COMPANY_AUTHENTICATION CA ON  CI.COMPANYID=CA.COMPANYID WHERE CA.USERID=" + Session["varuserid"] + @" ORDER BY COMPANYNAME
                        SELECT UNITSID,UNITNAME FROM UNITS ORDER BY UNITNAME
                        SELECT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESS_NAME_MASTER WHERE PROCESSTYPE=1 AND PROCESS_NAME_ID!=1 order by Process_name";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyname, ds, 0, false, "");
            if (DDCompanyname.Items.Count > 0)
            {
                DDCompanyname.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyname.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDjobname, ds, 2, true, "--Plz Select--");
            if (DDunit.Items.Count > 0)
            {
                DDunit.SelectedIndex = 1;
            }
            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDcheckedit.Visible = true;
            }
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Companyid", DDCompanyname.SelectedValue);
            param[1] = new SqlParameter("@Unitid", DDunit.SelectedValue);
            param[2] = new SqlParameter("@Jobid", DDjobname.SelectedValue);
            param[3] = new SqlParameter("@date", txtdate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFINISHINGMATERIALPREPDETAIL", param);
            DGDetail.DataSource = ds;
            DGDetail.DataBind();
            txttotalpcs.Text = "";
            txttotalarea.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalpcs.Text = ds.Tables[0].Compute("count(stockno)", "").ToString();
                txttotalarea.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(Area)", "")), 3).ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrcd", "alert('No data fetched for this combination!!!')", true);
            }


        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DDjobname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {

            Fillissueno();
        }

        DGDetail.DataSource = null;
        DGDetail.DataBind();

        DGIssueDetail.DataSource = null;
        DGIssueDetail.DataBind();

        DGConsmpdetail.DataSource = null;
        DGConsmpdetail.DataBind();

    }
    protected void Fillissueno()
    {
        string str = @"SELECT ID,ISSUENO+ ' # ' + REPLACE(CONVERT(NVARCHAR(11),DATE,106),' ','-') AS ISSUENO FROM FINISHINGMATERIALPREPMASTER WHERE COMPANYID=" + DDCompanyname.SelectedValue + " AND UNITID=" + DDunit.SelectedValue + " AND PROCESSID=" + DDjobname.SelectedValue + " ORDER BY ISSUENO";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("STOCKNO", typeof(long));
        dt.Columns.Add("ITEM_FINISHED_ID", typeof(int));
        dt.Columns.Add("WIDTH", typeof(string));
        dt.Columns.Add("LENGTH", typeof(string));
        dt.Columns.Add("AREA", typeof(float));
        dt.Columns.Add("ISSUEORDERID", typeof(int));
        dt.Columns.Add("ISSUE_DETAIL_ID", typeof(int));

        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            Label lblstockno = (Label)DGDetail.Rows[i].FindControl("lblstockno");
            Label lblitemfinishedid = (Label)DGDetail.Rows[i].FindControl("lblitemfinishedid");
            Label lblwidth = (Label)DGDetail.Rows[i].FindControl("lblwidth");
            Label lbllength = (Label)DGDetail.Rows[i].FindControl("lbllength");
            Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
            Label lblissueorderid = (Label)DGDetail.Rows[i].FindControl("lblissueorderid");
            Label lblissuedetailid = (Label)DGDetail.Rows[i].FindControl("lblissuedetailid");
            CheckBox chkitem = (CheckBox)DGDetail.Rows[i].FindControl("chkitem");
            if (chkitem.Checked == true)
            {

                DataRow dr = dt.NewRow();
                dr["stockno"] = lblstockno.Text;
                dr["item_finished_id"] = lblitemfinishedid.Text;
                dr["width"] = lblwidth.Text;
                dr["length"] = lbllength.Text;
                dr["Area"] = lblarea.Text;
                dr["issueorderid"] = lblissueorderid.Text;
                dr["issue_detail_id"] = lblissuedetailid.Text;
                dt.Rows.Add(dr);

            }
        }
        if (dt.Rows.Count > 0)
        {


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnid.Value;
                param[1] = new SqlParameter("@Companyid", DDCompanyname.SelectedValue);
                param[2] = new SqlParameter("@Unitid", DDunit.SelectedValue);
                param[3] = new SqlParameter("@Processid", DDjobname.SelectedValue);
                param[4] = new SqlParameter("@Date", txtdate.Text);
                param[5] = new SqlParameter("@issueno", SqlDbType.VarChar, 50);
                param[5].Direction = ParameterDirection.InputOutput;
                param[5].Value = txtissueno.Text;
                param[6] = new SqlParameter("@dt", dt);
                param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[7].Direction = ParameterDirection.Output;
                param[8] = new SqlParameter("@userid", Session["varuserid"]);
                param[9] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEFINISHINGMATERIALPREPARATION", param);

                if (param[7].Value.ToString() != "")
                {
                    lblmsg.Text = param[7].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    hnid.Value = param[0].Value.ToString();
                    txtissueno.Text = param[5].Value.ToString();
                    lblmsg.Text = "Data Saved Successfully...";
                    Tran.Commit();
                    DGDetail.DataSource = null;
                    DGDetail.DataBind();
                    FillSavedDetails();
                }

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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please select atleast one checkbox to save data?')", true);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", hnid.Value);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFINISHINGMATERIALPREPARATIONREPORT", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\rptfinishingmatpreparation.rpt";


                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptfinishingmatpreparation.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altrpt", "alert('No records Found!!!')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDissueno.Visible = false;
        hnid.Value = "0";
        DDjobname.SelectedIndex = -1;
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        txtdate.Enabled = true;
        if (chkedit.Checked == true)
        {
            TDissueno.Visible = true;
            txtdate.Enabled = false;
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDissueno.SelectedValue;
        FillSavedDetails();
    }
    protected void FillSavedDetails()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", hnid.Value);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFINISHINGMATERIALPREPSAVEDETAIL", param);
            lblissue.Visible = false;
            lblconsmp.Visible = false;

            DGIssueDetail.DataSource = ds.Tables[0];
            DGIssueDetail.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblissue.Visible = true;
                txtdate.Text = ds.Tables[0].Rows[0]["Issuedate"].ToString();
                txtissueno.Text = ds.Tables[0].Rows[0]["issueno"].ToString();
            }

            DGConsmpdetail.DataSource = ds.Tables[1];
            DGConsmpdetail.DataBind();
            if (ds.Tables[1].Rows.Count > 0)
            {
                lblconsmp.Visible = true;
            }



        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void lnkdelClick(object sender, EventArgs e)
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
            LinkButton lnkdel = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)lnkdel.NamingContainer;

            Label lbldetailid = (Label)gvr.FindControl("lbldetailid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Detailid", lbldetailid.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@MastercompanyId", Session["varcompanyNo"]);
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);

            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEFINISHINGMATERIALPREPARATION", param);
            lblmsg.Text = param[1].Value.ToString();
            Tran.Commit();
            FillSavedDetails();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
}