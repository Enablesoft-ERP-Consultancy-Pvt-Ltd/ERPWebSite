using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_frmEditadvanceamount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            UtilityModule.ConditionalComboFill(ref DDyear, "select year,year year1 from yearData order by year desc", false, "");
            DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            DDyear.SelectedValue = DateTime.Now.Year.ToString();
            txtAdvancedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }
    }
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {

        lblmsg.Text = "";

        if (txtIdNo.Text != "")
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
                param[0] = new SqlParameter("@Empcode", txtIdNo.Text);
                param[1] = new SqlParameter("@Advamount", txtAdvanceAmt.Text == "" ? "0" : txtAdvanceAmt.Text);
                param[2] = new SqlParameter("@AdvRemark", txtadvremark.Text);
                param[3] = new SqlParameter("@AdvDate", txtAdvancedate.Text);
                param[4] = new SqlParameter("@Userid", Session["varuserid"]);
                param[5] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
                param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@CompanyId", 1);
                param[8] = new SqlParameter("@JobId", 1);
                param[9] = new SqlParameter("@Voucherno", txtvoucherno.Text);
                //***********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEADVANCEAMOUNT", param);
                lblmsg.Text = param[6].Value.ToString();
                Tran.Commit();
                GetBalAmount();
                txtAdvanceAmt.Text = "";
                txtadvremark.Text = "";
                fillgrid();
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('Please Fill Emp Code!!!');", true);            
        }

    }
    protected void fillgrid()
    {
        string str;
        DataSet ds;
        str = @"select EI.EmpId,EmpCode,PM.PROCESS_NAME_ID as JobId,PM.PROCESS_NAME as Job,REPLACE(convert(nvarchar(11),Date,106),' ','-') as Date,
            AdvanceAmt as amount,AM.Id as DetailId,isnull(AM.Remark,'') as Remark From AdvanceAmountForAnisa AM inner join empinfo Ei on AM.Empid=EI.EmpId
            inner join PROCESS_NAME_MASTER PM on AM.Jobid=PM.PROCESS_NAME_ID
            Where Date>='" + txtfromdate.Text + "' and Date<='" + txttodate.Text + "'";//Where CONVERT(nvarchar(3),Date,0)='" + DDMonth.SelectedItem.Text + "' And year(Date)='" + DDyear.SelectedItem.Text + "'";
        if (txtIdNo.Text != "")
        {
            str = str + "  and empcode='" + txtIdNo.Text + "'";
        }
        str = str + " order by  case IsNumeric(Empcode) when 1 then Replicate(Char(0), 100 - Len(Empcode)) + Empcode else Empcode End";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvadvance.DataSource = ds;
            gvadvance.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('No advance amount is available for this ID No!!!');", true);
            gvadvance.DataSource = null;
            gvadvance.DataBind();
        }
    }
    protected void gvadvance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            int rowindex = e.RowIndex;
            //GetMasterData
            string Detailid = ((Label)gvadvance.Rows[rowindex].FindControl("lblDetailId")).Text;
            string empId = ((Label)gvadvance.Rows[rowindex].FindControl("lblEmpid")).Text;
            string Jobid = ((Label)gvadvance.Rows[rowindex].FindControl("lblJobid")).Text;
            string Amount = ((Label)gvadvance.Rows[rowindex].FindControl("lblamount")).Text;

            string str = @"Delete from advanceamountforanisa where EmpId=" + empId + " and JobId=" + Jobid + " and Id=" + Detailid + @"
                          insert into UpdateStatus(Id,CompanyId,UserId,Tablename,TableId,Date,Status)
                          values((select max(id)+1 from UpdateStatus)," + Session["varcompanyId"] + "," + Session["varuserid"] + ",'advanceamountforanisa'," + Detailid + ",GETDATE(),'Delete," + Amount + "')";
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            ScriptManager.RegisterStartupScript(Page, GetType(), "del", "alert('Data Deleted successfully!!!');", true);
            fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Empcode", txtIdNo.Text);
            param[1] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[2] = new SqlParameter("@ToDate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetReportDatafromadvanceamount", param);
            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["dsFileName"] = "~\\ReportSchema\\RptadvanceamountDetail.xsd";
                Session["rptFileName"] = "Reports/RptadvanceamountDetail.rpt";
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void FillEmpCode()
    {
        //string str = "select EmpCode From Empinfo Where Empcode='" + txtIdNo.Text + "' ";

        string str = "select EmpCode From Empinfo Where Empcode='" + txtIdNo.Text + "' and BlackList<>1 ";               
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtIdNo.Text = ds.Tables[0].Rows[0]["EmpCode"].ToString();
        }
        else
        {
            txtIdNo.Text = "";
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void txtIdNo_TextChanged(object sender, EventArgs e)
    {
        FillEmpCode();
        if (txtIdNo.Text != "")
        {
            string str = "select isnull(Guarantorname,'') as Guarantorname From Empinfo Where Empcode='" + txtIdNo.Text + "' ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblguarantorname.Text = "Guarantor Name : " + ds.Tables[0].Rows[0]["Guarantorname"].ToString();
            }
            else
            {
                lblguarantorname.Text = "";
            }
            GetBalAmount();
        }

    }
    protected void GetBalAmount()
    {
        //**************GET BAL AMOUNT
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@EMPCODE", txtIdNo.Text);
        param[1] = new SqlParameter("@PROCESSID", 1);
        param[2] = new SqlParameter("@FROMDATE", txtfromdate.Text);
        param[3] = new SqlParameter("@ToDATE", txttodate.Text);
        param[4] = new SqlParameter("@BALAMOUNT", SqlDbType.Float);
        param[4].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETEMPLOYEEPROCESSPENDINGAMOUNT", param);
        lblbal.Text = "BALANCE AMOUNT : " + param[4].Value.ToString();
    }
    protected void btnguarantorwise_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Empcode", txtIdNo.Text);
            param[1] = new SqlParameter("@Processid", 1);
            param[2] = new SqlParameter("@FromDate", txtfromdate.Text);
            param[3] = new SqlParameter("@ToDate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETGUARANTORWISEPAYMENTDETAIL", param);
            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["dsFileName"] = "~\\ReportSchema\\RptadvanceamountDetailGuarantorwise.xsd";
                Session["rptFileName"] = "Reports/RptadvanceamountDetailGuarantorwise.rpt";
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
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
}