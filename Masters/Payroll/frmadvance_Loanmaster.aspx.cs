using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmadvance_Loanmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"SELECT Year,Year as Year1 FROM YEARDATA
                          select Month_Id,Month_Name From MonthTable";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDdeductionyear, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDyearedit, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDeductionmonthedit, ds, 1, false, "");
            DDdeductionyear.SelectedValue = System.DateTime.Now.Year.ToString();
            DDMonth.SelectedValue = System.DateTime.Now.Month.ToString();

            DDyearedit.SelectedValue = System.DateTime.Now.Year.ToString();
            DDDeductionmonthedit.SelectedValue = System.DateTime.Now.Month.ToString();


            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            DDcrdr.SelectedValue = "2";
            DDtypeofamount.SelectedValue = "3";
            DDpaymentmode.SelectedValue = "2";

        }
    }
    protected void fillemployee()
    {
        try
        {
            txtempname.Text = "";
            hnempid.Value = "0";
            string str = @"SELECT EI.EMPID,EI.EMPNAME FROM EMPINFO EI With (nolock) INNER JOIN HR_EMPLOYEEINFORMATION HEI With (nolock) ON EI.EMPID=HEI.EMPID
                         AND EI.EMPCODE='" + txtempcode.Text + "' AND HEI.RESIGNSTATUS=0";
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnempid.Value = ds.Tables[0].Rows[0]["empid"].ToString();
                txtempname.Text = ds.Tables[0].Rows[0]["empname"].ToString();

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Emp. Code does not exists in ERP or Status is Resigned !!!')", true);
                txtempcode.Text = "";
            }
            txtempcode.Focus();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        fillemployee();

    }
    protected void btnsave_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[20];
            param[0] = new SqlParameter("@ID", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = 0;
            param[1] = new SqlParameter("@ADV_LOANNO", SqlDbType.VarChar, 20);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Value = "";
            param[2] = new SqlParameter("@EMPID", hnempid.Value);
            param[3] = new SqlParameter("@CR_DR", DDcrdr.SelectedValue);
            param[4] = new SqlParameter("@TYPE", DDtypeofamount.SelectedValue);
            param[5] = new SqlParameter("@AMOUNT", txtamount.Text == "" ? "0" : txtamount.Text);
            param[6] = new SqlParameter("@ADV_LOANDATE", txtdate.Text);
            param[7] = new SqlParameter("@NOOFINSTALLMENT", txtnoofinstallment.Text == "" ? "0" : txtnoofinstallment.Text);
            param[8] = new SqlParameter("@INSTALLMENTAMOUNT", txtinstallmentamount.Text == "" ? "0" : txtinstallmentamount.Text);
            param[9] = new SqlParameter("@DEDUCTIONPERIOD", DDDeductionperiod.SelectedValue);
            param[10] = new SqlParameter("@DEDUCTIONYEAR", DDdeductionyear.SelectedValue);
            param[11] = new SqlParameter("@DEDUCTIONMONTHID", DDMonth.SelectedValue);
            param[12] = new SqlParameter("@PAYMENTMODE", DDpaymentmode.SelectedValue);
            param[13] = new SqlParameter("@CHEQUENO", txtchequeno.Text);
            param[14] = new SqlParameter("@BANKNAME", txtbankname.Text);
            param[15] = new SqlParameter("@REMARK", txtremark.Text);
            param[16] = new SqlParameter("@DETAILS", txtdetails.Text);
            param[17] = new SqlParameter("@USERID", Session["varuserid"]);
            param[18] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[18].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVEADVANCE_LOAN", param);
            Tran.Commit();

            if (param[18].Value.ToString() != "")
            {
                lblmsg.Text = param[18].Value.ToString();
            }
            else
            {
                lblmsg.Text = "Data Saved Successfully.";
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + lblmsg.Text + "')", true);
                txtadvance_LoanNo.Text = param[1].Value.ToString();
                Refreshcontrol();
            }
        }
        catch (SqlException sql)
        {
            Tran.Rollback();
            lblmsg.Text = sql.Message + sql.LineNumber;
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
    protected void Refreshcontrol()
    {
        txtempcode.Text = "";
        txtempname.Text = "";
        hnempid.Value = "0";
        //DDcrdr.SelectedValue = "1";
        //DDtypeofamount.SelectedValue = "1";
        txtamount.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtnoofinstallment.Text = "1";
        txtinstallmentamount.Text = "";
        DDDeductionperiod.SelectedValue = "1";
        DDdeductionyear.SelectedValue = System.DateTime.Now.Year.ToString();
        DDMonth.SelectedValue = System.DateTime.Now.Month.ToString();
        //DDpaymentmode.SelectedValue = "1";
        txtchequeno.Text = "";
        txtbankname.Text = "";
        txtremark.Text = "";
        txtdetails.Text = "";
    }
    protected void txtnoofinstallment_TextChanged(object sender, EventArgs e)
    {
        Fillinstallmentamount();
    }
    protected void Fillinstallmentamount()
    {
        txtinstallmentamount.Text = (Convert.ToDecimal(txtamount.Text == "" ? "0" : txtamount.Text) / Convert.ToDecimal(txtnoofinstallment.Text == "" ? "0" : txtnoofinstallment.Text)).ToString();
    }
    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        Fillinstallmentamount();
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            string str = @"SELECT HAD.ID,HAD.EMPID,HAD.DEDUCTIONMONTHID,Replace(convert(nvarchar(11),HAD.ADV_LOANDATE,106),' ','-') as ADV_LOANDATE,EI.EMPCODE,EI.EMPNAME,DP.DEPARTMENTNAME,CR_DR=CASE WHEN HAD.CR_DR=1 THEN 'CR' WHEN CR_DR=2 THEN 'DR' ELSE '' END,
                        TYPE=CASE WHEN HAD.TYPE=1 THEN 'ADVANCE' WHEN HAD.TYPE=2 THEN 'LOAN' WHEN HAD.TYPE=3 THEN 'KHARCHA' ELSE '' END,
                        HAD.AMOUNT,HAD.NOOFINSTALLMENT,HAD.INSTALLMENTAMOUNT,DEDUCTIONPERIOD=CASE WHEN HAD.DEDUCTIONPERIOD=1 THEN 'MONTHLY' WHEN HAD.DEDUCTIONPERIOD=2 THEN 'QUATERLY' WHEN HAD.DEDUCTIONPERIOD=3 THEN
                        'HALF-YEARLY' WHEN HAD.DEDUCTIONPERIOD=4 THEN 'YEARLY' ELSE '' END,HAD.DEDUCTIONYEAR,MT.MONTH_NAME,
                        PAYMENTMODE=CASE WHEN HAD.PAYMENTMODE=1 THEN 'CASH' WHEN HAD.PAYMENTMODE=2 THEN 'CHEQUE' WHEN HAD.PAYMENTMODE=3 THEN 'OTHER' ELSE '' END,
                        HAD.CHEQUENO,HAD.BANKNAME,HAD.REMARK
                        FROM EMPINFO EI INNER JOIN HR_EMPLOYEEINFORMATION HEI ON EI.EMPID=HEI.EMPID
                        INNER JOIN DEPARTMENT DP ON EI.DEPARTMENTID=DP.DEPARTMENTID
                        INNER JOIN HR_DESIGNATIONMASTER HDM ON HEI.DESIGNATION=HDM.DESIGNATIONID
                        INNER JOIN HR_ADVANCE_LOAN HAD ON EI.EMPID=HAD.EMPID
                        LEFT JOIN MONTHTABLE MT ON HAD.DEDUCTIONMONTHID=MT.MONTH_ID WHere 1=1";
            if (txtempcodeedit.Text != "")
            {
                str = str + "  and ei.empcode='" + txtempcodeedit.Text + "'";
            }
            if (DDcrdredit.SelectedValue != "-1")
            {
                str = str + "  and HAD.Cr_dr=" + DDcrdredit.SelectedValue;
            }
            if (DDtypeedit.SelectedValue != "-1")
            {
                str = str + "  and HAD.Type=" + DDtypeedit.SelectedValue;
            }
            if (DDyearedit.SelectedIndex != -1)
            {
                str = str + "  and HAD.Deductionyear=" + DDyearedit.SelectedValue;
            }
            if (DDDeductionmonthedit.SelectedIndex != -1)
            {
                str = str + "  and HAD.Deductionmonthid=" + DDDeductionmonthedit.SelectedValue;
            }

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }

            str = str + " ORDER BY HAD.ID";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DGDetail.DataSource = ds.Tables[0];
            DGDetail.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            if (Session["usertype"].ToString() == "1")
            {
                lnkDelete.Visible = true;
            }
            else
            {
                lnkDelete.Visible = false;
            }
        }
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lblid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblid");
            Label lblempid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lblempid");
            Label lbldeductionmonthid = (Label)DGDetail.Rows[e.RowIndex].FindControl("lbldeductionmonthid");
            Label lbldeductionyear = (Label)DGDetail.Rows[e.RowIndex].FindControl("lbldeductionyear");

            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", lblid.Text);
            param[1] = new SqlParameter("@empid", lblempid.Text);
            param[2] = new SqlParameter("@Deductionmonthid", lbldeductionmonthid.Text);
            param[3] = new SqlParameter("@Deductionyear", lbldeductionyear.Text);
            param[4] = new SqlParameter("@userid", Session["varuserid"]);
            param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyNo"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_DELETEADVANCELOAN", param);
            Tran.Commit();            
            btngetdata_Click(sender, new EventArgs());
            lblmsg.Text = param[6].Value.ToString();
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