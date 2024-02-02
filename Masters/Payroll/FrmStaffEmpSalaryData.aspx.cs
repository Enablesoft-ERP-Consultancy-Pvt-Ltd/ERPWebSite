using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

public partial class Masters_Payroll_FrmStaffEmpSalaryData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            //txtstartdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //txtcompdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                                       select UnitsId,UnitName from Units order by UnitName
                                        select Month_Id,Month_Name from MonthTable 
                                        select Year,Year from YearData";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyUnitM, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 3, true, "--Plz Select--");

            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            if (DDCompanyUnitM.Items.Count > 0)
            {
                DDCompanyUnitM.SelectedIndex = 1;
            }

            if (DDMonth.Items.Count > 0)
            {
                DDMonth.SelectedValue = currentMonth;
            }
            if (DDYear.Items.Count > 0)
            {
                DDYear.SelectedValue = currentYear;
            } 
        }
    }
    protected void fillReceiveNo()
    {
//        string str = @"select distinct AE.ReceiveNo,AE.ReceiveNo+' / '+cast(left(datename(month,dateadd(month, DATEPART(MONTH, AE.AttendanceDate) - 1, 0)),3) as varchar)+'-'+cast(DATEPART(YEAR, AE.AttendanceDate) as varchar) as ReceiveName 
//                      from AttendanceEmp AE where AE.AttendanceDate>='" + txtfromdate.Text + "'";
        string str = @"select distinct AE.ReceiptNo,AE.ReceiptNo+' / '+cast(left(datename(month,dateadd(month, AE.SalMonth - 1, 0)),3) as varchar)+'-'+cast(AE.SalYear as varchar) as ReceiveName 
                      from StaffEmpSalAllowDed AE where UnitId='" + DDCompanyUnit.SelectedValue + "'";
        UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
    }
    protected void bindCompanyUnit()
    {
        string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                                       select UnitsId,UnitName from Units order by UnitName";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //CommanFunction.FillComboWithDS(DDCompany, ds, 0);
        UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "");
        UtilityModule.ConditionalComboFillWithDS(ref DDCompanyUnit, ds, 1, true, "--Plz Select--");
        //if (DDCompanyUnit.Items.Count > 0)
        //{
        //    DDCompanyUnit.SelectedIndex = 1;
        //}
    }
    protected void DDCompanyUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillReceiveNo();
    }
    protected void chkDelete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDelete.Checked == true)
        {
            bindCompanyUnit();           
            TDreceiveno.Visible = true;
            TDCompanyName.Visible = true;
            TDCompanyUnit.Visible = true;
            TDDeleteBtn.Visible = true;
        }
        else
        {
            TDreceiveno.Visible = false;
            TDDeleteBtn.Visible = false;
            TDCompanyName.Visible = false;
            TDCompanyUnit.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDreceiveNo, "", true, "--Plz Select--");
            UtilityModule.ConditionalComboFill(ref DDCompanyUnit, "", true, "--Plz Select--");
        }
    }
    //protected void txtfromdate_TextChanged(object sender, EventArgs e)
    //{
    //    fillReceiveNo();
    //}
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        lblmsg.Text = "";
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
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (DDCompanyUnitM.SelectedIndex > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@UnitId", DDCompanyUnitM.SelectedValue);
                param[2] = new SqlParameter("@SalMonth", DDMonth.SelectedValue);
                param[3] = new SqlParameter("@SalYear", DDYear.SelectedValue);
                param[4] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
                param[5] = new SqlParameter("@UserId", Session["varuserId"]);
                param[6] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                param[6].Direction = ParameterDirection.Output;

                //**********
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Pro_GetStaffEmpSalAllowDed", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    GVStaffEmpSalary.DataSource = ds.Tables[0];
                    GVStaffEmpSalary.DataBind();
                    BtnSave.Visible = true;
                }
                else
                {
                    GVStaffEmpSalary.DataSource = null;
                    GVStaffEmpSalary.DataBind();
                    BtnSave.Visible = false;
                }
                lblMessage.Text = param[6].Value.ToString();
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblMessage.Text = ex.Message;
                con.Close();
            }
        }

    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main.aspx");
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //CHECKVALIDCONTROL();
        ViewState["StaffEmpSal"] = null;
        DataTable dtrecordsStaffEmpSal = new DataTable();

        if (ViewState["StaffEmpSal"] == null)
        {
            //DataTable dtrecords = new DataTable();
            dtrecordsStaffEmpSal.Columns.Add("EmpId", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("EmpName", typeof(string));
            dtrecordsStaffEmpSal.Columns.Add("FatherName", typeof(string));
            dtrecordsStaffEmpSal.Columns.Add("TotalPresent", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("TotalAbsent", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("TotalWeekOff", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("TotalHolidays", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("BasicSalary", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("PerDaySalary", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("Allowance", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("TotalGrossSalary", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("Deduction", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("NetPay", typeof(float));
            dtrecordsStaffEmpSal.Columns.Add("CompanyId", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("UnitId", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("SalMonth", typeof(int));
            dtrecordsStaffEmpSal.Columns.Add("SalYear", typeof(int));

            ViewState["StaffEmpSal"] = dtrecordsStaffEmpSal;
        }
        else
        {
            dtrecordsStaffEmpSal = (DataTable)ViewState["StaffEmpSal"];
        }

        for (int i = 0; i < GVStaffEmpSalary.Rows.Count; i++)
        {
            //CheckBox chkoutItem = ((CheckBox)GVStaffEmpSalary.Rows[i].FindControl("Chkboxitem"));
            //TextBox txtQty = ((TextBox)GVStaffEmpSalary.Rows[i].FindControl("txtQty"));
           

            //if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) >= 0) && Convert.ToDouble(txtAmt.Text == "" ? "0" : txtAmt.Text) > 0)
            //{
            Label lblEmpId = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblEmpId"));
            Label lblEmpName = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblEmpName"));
            Label lblFatherName = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblFatherName"));
            Label lblTotalPresent = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblTotalPresent"));
            Label lblTotalAbsent = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblTotalAbsent"));
            Label lblTotalWeekOff = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblTotalWeekOff"));
            Label lblTotalHolidays = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblTotalHolidays"));
            Label lblBasicSalary = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblBasicSalary"));
            Label lblAllowence = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblAllowence"));
            Label lblPerDaySalary = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblPerDaySalary"));
            Label lblTotalGrossSalary = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblTotalGrossSalary"));
            Label lblDeduction = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblDeduction"));
            Label lblNetSalary = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblNetSalary"));
            Label lblCompanyid = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblCompanyid"));
            Label lblUnitId = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblUnitId"));
            Label lblSalMonth = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblSalMonth"));
            Label lblSalYear = ((Label)GVStaffEmpSalary.Rows[i].FindControl("lblSalYear"));


            DataRow dr = dtrecordsStaffEmpSal.NewRow();
                dr["EmpId"] = lblEmpId.Text;
                dr["EmpName"] = lblEmpName.Text;
                dr["FatherName"] = lblFatherName.Text;
                dr["TotalPresent"] = lblTotalPresent.Text == "" ? "0" : lblTotalPresent.Text; ;
                dr["TotalAbsent"] = lblTotalAbsent.Text == "" ? "0" : lblTotalAbsent.Text;
                dr["TotalWeekOff"] = lblTotalWeekOff.Text == "" ? "0" : lblTotalWeekOff.Text;
                dr["TotalHolidays"] = lblTotalHolidays.Text == "" ? "0" : lblTotalHolidays.Text;
                dr["BasicSalary"] = lblBasicSalary.Text == "" ? "0" : lblBasicSalary.Text;
                dr["PerDaySalary"] = lblPerDaySalary.Text == "" ? "0" : lblPerDaySalary.Text;
                dr["Allowance"] = lblAllowence.Text == "" ? "0" : lblAllowence.Text;
                dr["TotalGrossSalary"] = lblTotalGrossSalary.Text == "" ? "0" : lblTotalGrossSalary.Text;
                dr["Deduction"] = lblDeduction.Text == "" ? "0" : lblDeduction.Text;
                dr["NetPay"] = lblNetSalary.Text == "" ? "0" : lblNetSalary.Text;

                dr["CompanyId"] = lblCompanyid.Text == "" ? "0" : lblCompanyid.Text;
                dr["UnitId"] = lblUnitId.Text == "" ? "0" : lblUnitId.Text;
                dr["SalMonth"] = lblSalMonth.Text == "" ? "0" : lblSalMonth.Text;
                dr["SalYear"] = lblSalYear.Text == "" ? "0" : lblSalYear.Text;

                dtrecordsStaffEmpSal.Rows.Add(dr);
                ViewState["StaffEmpSal"] = dtrecordsStaffEmpSal;
              
            //}
        }

         //********************
        if (dtrecordsStaffEmpSal.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[1] = new SqlParameter("@UnitId", SqlDbType.Int);
                param[2] = new SqlParameter("@SalMonth", SqlDbType.Int);
                param[3] = new SqlParameter("@SalYear", SqlDbType.Int);
                param[4] = new SqlParameter("@MastercompanyId", SqlDbType.Int);
                param[5] = new SqlParameter("@UserId", SqlDbType.Int);              
                param[6] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);

                param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@UnitId", DDCompanyUnitM.SelectedValue);
                param[2] = new SqlParameter("@SalMonth", DDMonth.SelectedValue);
                param[3] = new SqlParameter("@SalYear", DDYear.SelectedValue);
                param[4] = new SqlParameter("@MastercompanyId", Session["varMasterCompanyIDForERP"]);
                param[5] = new SqlParameter("@UserId", Session["varuserId"]);
                param[6] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 200);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@dtrecordsStaffEmpSal", dtrecordsStaffEmpSal); 

                //**********
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew2", param);
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveStaffEmpSalAllowDed", param);
                Tran.Commit();

                lblmsg.Text = param[6].Value.ToString();
                lblmsg.Visible = true;

                GVStaffEmpSalary.DataSource = null;
                GVStaffEmpSalary.DataBind();
                BtnSave.Visible = false;

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
    protected void GVStaffEmpSalary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVStaffEmpSalary, "select$" + e.Row.RowIndex);
        }
    }
    //protected void GVStaffEmpSalary_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //}
  
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (DDreceiveNo.SelectedIndex > 0)
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
                param[0] = new SqlParameter("@ReceiptNo", DDreceiveNo.SelectedValue);
                param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
                param[2] = new SqlParameter("@UserId", Session["varuserid"]);
                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                //*************************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteStaffEmpSalData", param);
                lblMessage.Text = param[3].Value.ToString();
                Tran.Commit();
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
            lblmsg.Text = "Please Select ReceiveNo";
        }
    }
   
}