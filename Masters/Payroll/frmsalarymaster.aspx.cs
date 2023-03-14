using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Payroll_frmsalarymaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock) 
                        JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" And 
                            CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CI.CompanyName 
                        Select Distinct D.DepartmentId, D.DepartmentName 
                        From Department D(Nolock)
                        JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                        JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                        Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varCompanyId"] + @" 
                        Order By D.DepartmentName 
                        SELECT Designationid, Designation From HR_Designationmaster(Nolock) order by Designation 
                        Select EmpId, EmpCode + '-' + EmpName Emp 
                        From EmpInfo EI(Nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = EI.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                        Where EI.PartyType = 1 and IsNull(EI.empcode, '') <> '' ";

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }
            str = str + " Order By Emp ";

            str = str + " Select Payrolltypeid,Payrolltypename From HR_PayrollTypeMaster order by Payrolltypename";

             
            str = str + @" Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDDesignation, ds, 2, true, "--Plz Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDemp, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDpayrolltype, ds, 4, false, "");

            txteffectivefrom.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 5, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        Show_Click();
    }
    protected void Show_Click()
    {
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@Companyid", DDCompanyName.SelectedIndex == -1 ? "0" : DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@DepartmentID", DDDepartment.SelectedIndex == -1 ? "0" : DDDepartment.SelectedValue);
        param[2] = new SqlParameter("@DesgnationID", DDDesignation.SelectedIndex == -1 ? "0" : DDDesignation.SelectedValue);
        param[3] = new SqlParameter("@PayRollType", DDpayrolltype.SelectedIndex == -1 ? "0" : DDpayrolltype.SelectedValue);
        param[4] = new SqlParameter("@EmpID", DDemp.SelectedIndex == -1 ? "0" : DDemp.SelectedValue);
        param[5] = new SqlParameter("@EmpCode", txtempcode.Text);

        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            param[6] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 1);
        }
        else
        {
            param[6] = new SqlParameter("@USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR", 0);
        }

        param[7] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FILLDETAILFORSALARYMASTERNEW", param);

        if (ds.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
            return;
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDpayrolltype.Items.FindByValue(ds.Tables[0].Rows[0]["payrolltypeid"].ToString()) != null)
            {
                DDpayrolltype.SelectedValue = ds.Tables[0].Rows[0]["payrolltypeid"].ToString();
            }
            txtbasicpay.Text = ds.Tables[0].Rows[0]["basic_pay"].ToString();
            txtgrosssal.Text = ds.Tables[0].Rows[0]["Grosssal"].ToString();
            txtnetsal.Text = ds.Tables[0].Rows[0]["Netsal"].ToString();
            //txteffectivefrom.Text = ds.Tables[0].Rows[0]["Effectivefrom"].ToString();
            //***********Allowances
            DataView dvallowances = new DataView(ds.Tables[0]);
            dvallowances.RowFilter = "Allowance_deduction_id=1 and payrolltypeid=" + ds.Tables[0].Rows[0]["payrolltypeid"].ToString() + "";
            DGallowances.DataSource = dvallowances;
            DGallowances.DataBind();
            //********deductions
            //DataView dvdeductions = new DataView(ds.Tables[0]);
            //dvdeductions.RowFilter = "Allowance_deduction_id=2 and payrolltypeid=" + ds.Tables[0].Rows[0]["payrolltypeid"].ToString() + "";
            //DGDeductions.DataSource = dvdeductions;
            //DGDeductions.DataBind();

            GetGross_Netsalary();
        }

        DG.DataSource = ds.Tables[1];
        DG.DataBind();
    }
    protected void DDpayrolltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_allowances_Deductions();
        GetGross_Netsalary();
    }
    protected void Fill_allowances_Deductions()
    {
        try
        {
            string str = @"select AM.ParameterName,case when PD.Allowance_type=0 Then 'Percentage of Basic salary('+cast(PD.Percent_Amount as varchar)+'%)'
                    when PD.Allowance_type=2 Then 'Percentage of Gross salary('+cast(PD.Percent_Amount as varchar)+'%)'
                    else 'Fixed amount' End Allowance_deductiontype,
                    PD.ParameterId,PD.Allowance_Deduction_Id,Case WHen PD.Allowance_type=1 Then Percent_Amount else 0 End as Amount,PD.Maxcapingamount,PD.Mincapingamount,
                    PD.Allowance_type,PD.Percent_Amount,PD.Taxable 
                    From HR_PayrollParameterDesc PD inner join HR_AllowancesMaster AM on PD.ParameterId=Am.ID
                    Where PD.PayrollTypeid=" + DDpayrolltype.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //***********Allowances
            DataView dvallowances = new DataView(ds.Tables[0]);
            dvallowances.RowFilter = "Allowance_deduction_id=1";
            DGallowances.DataSource = dvallowances;
            DGallowances.DataBind();
            //********deductions
            DataView dvdeductions = new DataView(ds.Tables[0]);
            dvdeductions.RowFilter = "Allowance_deduction_id=2";
            DGDeductions.DataSource = dvdeductions;
            DGDeductions.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void txtbasicpay_TextChanged(object sender, EventArgs e)
    {
        GetGross_Netsalary();
    }
    protected void txtallowanceamount_TextChanged(object sender, EventArgs e)
    {
        GetGross_Netsalary();
    }
    protected void txtdeductionamount_TextChanged(object sender, EventArgs e)
    {
        GetGross_Netsalary();
    }
    protected void GetGross_Netsalary()
    {
        decimal basicpay = txtbasicpay.Text == "" ? 0 : Convert.ToDecimal(txtbasicpay.Text);
        decimal allowances = 0, deductions = 0, Amount = 0, GrosssalDeduction = 0, GrossDeduction = 0;

        if (basicpay > 0)
        {
            //*******Allowances
            foreach (GridViewRow gvr in DGallowances.Rows)
            {
                Label lblallowancetype = (Label)gvr.FindControl("lblallowance_type");
                //Label lblallowanceamount = (Label)gvr.FindControl("lblallowanceamount");
                Label lblallowancepercent_amount = (Label)gvr.FindControl("lblallowancepercent_amount");
                Label lblallowancemaxcapingamt = (Label)gvr.FindControl("lblallowancemaxcapingamt");
                TextBox txtallowanceamount = (TextBox)gvr.FindControl("txtallowanceamount");
                //0 for percentage of basic sal,1 fixed amt
                if (lblallowancetype.Text != "2")
                {
                    if (lblallowancetype.Text == "0")
                    {
                        Amount = Math.Round(basicpay * Convert.ToDecimal(lblallowancepercent_amount.Text) / 100, 0, MidpointRounding.AwayFromZero);
                        if (Amount > Convert.ToDecimal(lblallowancemaxcapingamt.Text) && Convert.ToDecimal(lblallowancemaxcapingamt.Text) > 0)
                        {
                            Amount = Convert.ToDecimal(lblallowancemaxcapingamt.Text);
                        }
                        // lblallowanceamount.Text = Amount.ToString();
                        txtallowanceamount.Text = Amount.ToString();
                        allowances = allowances + Amount;
                    }
                    else
                    {
                        allowances = allowances + Convert.ToDecimal(txtallowanceamount.Text == "" ? "0" : txtallowanceamount.Text);
                    }
                }
            }
            //*******Deductions
            foreach (GridViewRow gvr in DGDeductions.Rows)
            {
                Label lbldeductiontype = (Label)gvr.FindControl("lbldeduction_type");
                //Label lbldeductionamount = (Label)gvr.FindControl("lbldeductionamount");
                Label lbldeductionpercent_amount = (Label)gvr.FindControl("lbldeductionpercent_amount");
                Label lbldeductionmaxcapingamt = (Label)gvr.FindControl("lbldeductionmaxcapingamt");
                TextBox txtdeductionamount = (TextBox)gvr.FindControl("txtdeductionamount");
                //0 for percentage of basic sal,1 fixed amt
                if (lbldeductiontype.Text != "2")
                {
                    if (lbldeductiontype.Text == "0")
                    {
                        Amount = Math.Round(basicpay * Convert.ToDecimal(lbldeductionpercent_amount.Text) / 100, 0, MidpointRounding.AwayFromZero);
                        if (Amount > Convert.ToDecimal(lbldeductionmaxcapingamt.Text) && Convert.ToDecimal(lbldeductionmaxcapingamt.Text) > 0)
                        {
                            Amount = Convert.ToDecimal(lbldeductionmaxcapingamt.Text);
                        }
                        //lbldeductionamount.Text = Amount.ToString();
                        txtdeductionamount.Text = Amount.ToString();
                        deductions = deductions + Amount;
                    }
                    else
                    {
                        deductions = deductions + Convert.ToDecimal(txtdeductionamount.Text == "" ? "0" : txtdeductionamount.Text);
                    }
                }

            }
            txtgrosssal.Text = Convert.ToString(basicpay + allowances);
            txtnetsal.Text = Convert.ToString(basicpay + allowances - deductions);
            GrosssalDeduction = Convert.ToDecimal(txtgrosssal.Text == "" ? "0" : txtgrosssal.Text);
            //*************Loop For Gross Salary
            foreach (GridViewRow gvr in DGDeductions.Rows)
            {
                Label lbldeductiontype = (Label)gvr.FindControl("lbldeduction_type");
                Label lbldeductionamount = (Label)gvr.FindControl("lbldeductionamount");
                Label lbldeductionpercent_amount = (Label)gvr.FindControl("lbldeductionpercent_amount");
                Label lbldeductionmaxcapingamt = (Label)gvr.FindControl("lbldeductionmaxcapingamt");
                TextBox txtdeductionamount = (TextBox)gvr.FindControl("txtdeductionamount");

                if (lbldeductiontype.Text == "2")
                {
                    Amount = Math.Round(GrosssalDeduction * Convert.ToDecimal(lbldeductionpercent_amount.Text) / 100, 0, MidpointRounding.AwayFromZero);
                    if (Amount > Convert.ToDecimal(lbldeductionmaxcapingamt.Text) && Convert.ToDecimal(lbldeductionmaxcapingamt.Text) > 0)
                    {
                        Amount = Convert.ToDecimal(lbldeductionmaxcapingamt.Text);
                    }
                    //lbldeductionamount.Text = Amount.ToString();
                    txtdeductionamount.Text = Amount.ToString();
                    GrossDeduction = GrossDeduction + Amount;
                }
            }
            txtnetsal.Text = Convert.ToString(basicpay + allowances - (deductions + GrossDeduction));
        }
    }
    protected void txtempcode_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string str = @"select empid From Empinfo where BranchID = " + DDBranchName.SelectedValue + " And empcode='" + txtempcode.Text + "'";
        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            str = str + " And USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDemp.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
            {
                DDemp.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDemp_SelectedIndexChanged(sender, new EventArgs());
            }
            else
            {
                lblmsg.Text = "Employee code does not exists.";
            }
        }
        else
        {
            lblmsg.Text = "Employee code does not exists.";
        }
    }
    protected void FillgridDetail()
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@empid", DDemp.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_FILLDETAILFORSALARYMASTER", param);

        //DGdetails.DataSource = ds.Tables[0];
        //DGdetails.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {
            if (DDpayrolltype.Items.FindByValue(ds.Tables[1].Rows[0]["payrolltypeid"].ToString()) != null)
            {
                DDpayrolltype.SelectedValue = ds.Tables[1].Rows[0]["payrolltypeid"].ToString();
            }
            txtbasicpay.Text = ds.Tables[1].Rows[0]["basic_pay"].ToString();
            txtgrosssal.Text = ds.Tables[1].Rows[0]["Grosssal"].ToString();
            txtnetsal.Text = ds.Tables[1].Rows[0]["Netsal"].ToString();
            txteffectivefrom.Text = ds.Tables[1].Rows[0]["Effectivefrom"].ToString();
            //***********Allowances
            DataView dvallowances = new DataView(ds.Tables[1]);
            dvallowances.RowFilter = "Allowance_deduction_id=1 and payrolltypeid=" + ds.Tables[1].Rows[0]["payrolltypeid"].ToString() + "";
            DGallowances.DataSource = dvallowances;
            DGallowances.DataBind();
            //********deductions
            DataView dvdeductions = new DataView(ds.Tables[1]);
            dvdeductions.RowFilter = "Allowance_deduction_id=2 and payrolltypeid=" + ds.Tables[1].Rows[0]["payrolltypeid"].ToString() + "";
            DGDeductions.DataSource = dvdeductions;
            DGDeductions.DataBind();

            GetGross_Netsalary();
        }
        ds.Tables[2].Columns.Remove("ID");
        ds.Tables[2].Columns.Remove("Pk");
        DgallowancesDetail.DataSource = ds.Tables[2];
        DgallowancesDetail.DataBind();
    }
    protected void DDemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillgridDetail();
    }
    protected void DGallowances_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblallowance_type = (Label)e.Row.FindControl("lblallowance_type");
            TextBox txtallowanceamount = (TextBox)e.Row.FindControl("txtallowanceamount");

            switch (lblallowance_type.Text)
            {
                case "1":
                    txtallowanceamount.Enabled = true;
                    break;
                default:
                    txtallowanceamount.Enabled = false;
                    break;
            }

        }
    }
    protected void DGDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbldeduction_type = (Label)e.Row.FindControl("lbldeduction_type");
            TextBox txtdeductionamount = (TextBox)e.Row.FindControl("txtdeductionamount");

            switch (lbldeduction_type.Text)
            {
                case "1":
                    txtdeductionamount.Enabled = true;
                    break;
                default:
                    txtdeductionamount.Enabled = false;
                    break;
            }
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SaveClick();
    }
    protected void SaveClick()
    {
        string Str = "";
        string EmpID = "";
        lblmsg.Text = "";
        //****allowances
        for (int i = 0; i < DGallowances.Rows.Count; i++)
        {
            Label lblallowanceid = (Label)DGallowances.Rows[i].FindControl("lblallowanceid");
            Label lbltaxableallowance = (Label)DGallowances.Rows[i].FindControl("lbltaxableallowance");
            Label lblallowance_type = (Label)DGallowances.Rows[i].FindControl("lblallowance_type");
            Label lblallowancepercent_amount = (Label)DGallowances.Rows[i].FindControl("lblallowancepercent_amount");
            Label lblallowancemincapingamt = (Label)DGallowances.Rows[i].FindControl("lblallowancemincapingamt");
            Label lblallowancemaxcapingamt = (Label)DGallowances.Rows[i].FindControl("lblallowancemaxcapingamt");
            Label lblAllowance_Deduction_Id = (Label)DGallowances.Rows[i].FindControl("lblAllowance_Deduction_Id");
            TextBox txtallowanceamount = (TextBox)DGallowances.Rows[i].FindControl("txtallowanceamount");

            Str = Str + DDpayrolltype.SelectedValue + "~";
            Str = Str + lblallowanceid.Text + "~";
            Str = Str + lbltaxableallowance.Text + "~";
            Str = Str + lblallowance_type.Text + "~";

            switch (lblallowance_type.Text)
            {
                case "1":
                    if (txtallowanceamount.Text == "")
                        Str = Str + "0~";
                    else
                        Str = Str + txtallowanceamount.Text + "~";
                    break;
                default:
                    if (lblallowancepercent_amount.Text == "")
                        Str = Str + "0~";
                    else
                        Str = Str + lblallowancepercent_amount.Text + "~";
                    break;
            }

            if (lblallowancemincapingamt.Text == "")
                Str = Str + "0~";
            else
                Str = Str + lblallowancemincapingamt.Text + "~";

            if (lblallowancemaxcapingamt.Text == "")
                Str = Str + "0~";
            else
                Str = Str + lblallowancemaxcapingamt.Text + "~";

            Str = Str + lblAllowance_Deduction_Id.Text + "|";
        }

        //****Deductions
        for (int i = 0; i < DGDeductions.Rows.Count; i++)
        {
            Label lbldeductionid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionid");
            Label lbldeductiontaxable = (Label)DGDeductions.Rows[i].FindControl("lbldeductiontaxable");
            Label lbldeduction_type = (Label)DGDeductions.Rows[i].FindControl("lbldeduction_type");
            Label lbldeductionpercent_amount = (Label)DGDeductions.Rows[i].FindControl("lbldeductionpercent_amount");
            Label lbldeductionmincapingamt = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmincapingamt");
            Label lbldeductionmaxcapingamt = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmaxcapingamt");
            Label lbldeductionmastertypeid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmastertypeid");
            TextBox txtdeductionamount = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionamount");

            Str = Str + DDpayrolltype.SelectedValue + "~";
            Str = Str + lbldeductionid.Text + "~";
            Str = Str + lbldeductiontaxable.Text + "~";
            Str = Str + lbldeduction_type.Text + "~";

            switch (lbldeduction_type.Text)
            {
                case "1":
                    if (txtdeductionamount.Text == "")
                        Str = Str + "0~";
                    else
                        Str = Str + txtdeductionamount.Text + "~";
                    break;
                default:
                    if (lbldeductionpercent_amount.Text == "")
                        Str = Str + "0~";
                    else
                        Str = Str + lbldeductionpercent_amount.Text + "~";
                    break;
            }

            if (lbldeductionmincapingamt.Text == "")
                Str = Str + "0~";
            else
                Str = Str + lbldeductionmincapingamt.Text + "~";

            if (lbldeductionmaxcapingamt.Text == "")
                Str = Str + "0~";
            else
                Str = Str + lbldeductionmaxcapingamt.Text + "~";

            Str = Str + lbldeductionmastertypeid.Text + "|";
        }
        //*********************

        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            if (Chkboxitem.Checked == true)
            {
                Label lblID = ((Label)DG.Rows[i].FindControl("lblID"));
                Label lblEmpID = ((Label)DG.Rows[i].FindControl("lblEmpID"));
                EmpID = EmpID + lblID.Text + "~" + lblEmpID.Text + "|";
            }
        }
        if (EmpID != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[11];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = 0;
                param[1] = new SqlParameter("@EMPDETAILID", EmpID);
                param[2] = new SqlParameter("@Payrolltypeid", DDpayrolltype.SelectedValue);
                param[3] = new SqlParameter("@basicpay", txtbasicpay.Text == "" ? "0" : txtbasicpay.Text);
                param[4] = new SqlParameter("@effectivefrom", txteffectivefrom.Text);
                param[5] = new SqlParameter("@userid", Session["varuserid"]);
                param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[6].Direction = ParameterDirection.Output;
                param[7] = new SqlParameter("@GrossSal", txtgrosssal.Text == "" ? "0" : txtgrosssal.Text);
                param[8] = new SqlParameter("@Netsal", txtnetsal.Text == "" ? "0" : txtnetsal.Text);
                param[9] = new SqlParameter("@Detail", Str);
                param[10] = new SqlParameter("@AllowanceChangeFlag", ChkForAllowanceChange.Checked == true ? 1 : 0);
                //*********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_HR_SaveSalaryMasterNew", param);
                lblmsg.Text = param[6].Value.ToString();
                Tran.Commit();
                //FillgridDetail();
                Show_Click();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
}