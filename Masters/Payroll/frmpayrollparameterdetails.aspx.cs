using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Payroll_frmpayrollparameterdetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDpayrolltype, "select Payrolltypeid,Payrolltypename From HR_PayrollTypeMaster order by Payrolltypename", true, "--Plz Select--");
            FillAllowanes_Deductions();
        }
    }
    protected void DDpayrolltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillAllowanes_Deductions();
        string str = @"select * From HR_PayrollParameterDesc Where Payrolltypeid=" + DDpayrolltype.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                if (dr["Allowance_Deduction_id"].ToString() == "1")
                {

                    //*******Allowances
                    for (int i = 0; i < DGallowances.Rows.Count; i++)
                    {
                        Label lblallowanceid = (Label)DGallowances.Rows[i].FindControl("lblallowanceid");
                        if (dr["parameterid"].ToString() == lblallowanceid.Text)
                        {
                            CheckBox chkallowances = (CheckBox)DGallowances.Rows[i].FindControl("chkallowances");
                            DropDownList ddallowancetaxable = (DropDownList)DGallowances.Rows[i].FindControl("ddallowancetaxable");
                            DropDownList ddallowancetype = (DropDownList)DGallowances.Rows[i].FindControl("ddallowancetype");
                            TextBox txtallowanceamount = (TextBox)DGallowances.Rows[i].FindControl("txtallowanceamount");
                            TextBox txtallowancemincapingamt = (TextBox)DGallowances.Rows[i].FindControl("txtallowancemincapingamt");
                            TextBox txtallowancemaxcapingamt = (TextBox)DGallowances.Rows[i].FindControl("txtallowancemaxcapingamt");

                            chkallowances.Checked = true;
                            ddallowancetaxable.SelectedItem.Text = dr["Taxable"].ToString();
                            ddallowancetype.SelectedValue = dr["Allowance_Type"].ToString();
                            txtallowanceamount.Text = dr["Percent_AMount"].ToString();
                            txtallowancemincapingamt.Text = dr["Mincapingamount"].ToString();
                            txtallowancemaxcapingamt.Text = dr["maxcapingamount"].ToString();


                        }
                    }

                }
                else if (dr["Allowance_Deduction_id"].ToString() == "2")
                {

                    //****Deductions
                    for (int i = 0; i < DGDeductions.Rows.Count; i++)
                    {
                        Label lbldeductionid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionid");
                        if (dr["parameterid"].ToString() == lbldeductionid.Text)
                        {

                            CheckBox chkdeductions = (CheckBox)DGDeductions.Rows[i].FindControl("chkdeductions");
                            DropDownList dddeductiontaxable = (DropDownList)DGDeductions.Rows[i].FindControl("dddeductiontaxable");
                            DropDownList dddeductiontype = (DropDownList)DGDeductions.Rows[i].FindControl("dddeductiontype");
                            TextBox txtdeductionamount = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionamount");
                            TextBox txtdeductionmincapingamt = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionmincapingamt");
                            TextBox txtdeductionmaxcapingamt = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionmaxcapingamt");

                            chkdeductions.Checked = true;
                            dddeductiontaxable.SelectedItem.Text = dr["Taxable"].ToString();
                            dddeductiontype.SelectedValue = dr["Allowance_Type"].ToString();
                            txtdeductionamount.Text = dr["Percent_AMount"].ToString();
                            txtdeductionmincapingamt.Text = dr["Mincapingamount"].ToString();
                            txtdeductionmaxcapingamt.Text = dr["maxcapingamount"].ToString();

                        }

                    }

                }
            }
        }

    }
    protected void FillAllowanes_Deductions()
    {
        string str = @"select ID as Parameterid,ParameterName,Typeid From HR_AllowancesMaster order by ParameterName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //**********Allowances
        DataView dvallowances = new DataView(ds.Tables[0]);
        dvallowances.RowFilter = "Typeid=1";
        DGallowances.DataSource = dvallowances;
        DGallowances.DataBind();
        //******Deductions
        DataView dvdeductions = new DataView(ds.Tables[0]);
        dvdeductions.RowFilter = "Typeid=2";
        DGDeductions.DataSource = dvdeductions;
        DGDeductions.DataBind();

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //**********sql Table Types
        DataTable dt = new DataTable();
        dt.Columns.Add("PayrollTypeid", typeof(int));
        dt.Columns.Add("Parameterid", typeof(int));
        dt.Columns.Add("Taxable", typeof(string));
        dt.Columns.Add("Allowance_Type", typeof(int));
        dt.Columns.Add("Percentage_Amount", typeof(double));
        dt.Columns.Add("Mincapingamt", typeof(double));
        dt.Columns.Add("Maxcapingamt", typeof(double));
        dt.Columns.Add("Allowance_Deduction_id", typeof(int));
        //****allowances
        for (int i = 0; i < DGallowances.Rows.Count; i++)
        {
            CheckBox chkallowances = (CheckBox)DGallowances.Rows[i].FindControl("chkallowances");
            if (chkallowances.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblallowanceid = (Label)DGallowances.Rows[i].FindControl("lblallowanceid");
                DropDownList ddallowancetaxable = (DropDownList)DGallowances.Rows[i].FindControl("ddallowancetaxable");
                DropDownList ddallowancetype = (DropDownList)DGallowances.Rows[i].FindControl("ddallowancetype");
                TextBox txtallowanceamount = (TextBox)DGallowances.Rows[i].FindControl("txtallowanceamount");
                TextBox txtallowancemincapingamt = (TextBox)DGallowances.Rows[i].FindControl("txtallowancemincapingamt");
                TextBox txtallowancemaxcapingamt = (TextBox)DGallowances.Rows[i].FindControl("txtallowancemaxcapingamt");
                Label lblallowancemastertypeid = (Label)DGallowances.Rows[i].FindControl("lblallowancemastertypeid");

                dr["Payrolltypeid"] = DDpayrolltype.SelectedValue;
                dr["Parameterid"] = lblallowanceid.Text;
                dr["Taxable"] = ddallowancetaxable.SelectedItem.Text;
                dr["Allowance_Type"] = ddallowancetype.SelectedValue;
                dr["Percentage_Amount"] = txtallowanceamount.Text == "" ? "0" : txtallowanceamount.Text;
                dr["Mincapingamt"] = txtallowancemincapingamt.Text == "" ? "0" : txtallowancemincapingamt.Text;
                dr["Maxcapingamt"] = txtallowancemaxcapingamt.Text == "" ? "0" : txtallowancemaxcapingamt.Text;
                dr["Allowance_Deduction_Id"] = lblallowancemastertypeid.Text;
                dt.Rows.Add(dr);
            }
        }
        //****Deductions
        for (int i = 0; i < DGDeductions.Rows.Count; i++)
        {
            CheckBox chkdeductions = (CheckBox)DGDeductions.Rows[i].FindControl("chkdeductions");
            if (chkdeductions.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lbldeductionid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionid");
                DropDownList dddeductiontaxable = (DropDownList)DGDeductions.Rows[i].FindControl("dddeductiontaxable");
                DropDownList dddeductiontype = (DropDownList)DGDeductions.Rows[i].FindControl("dddeductiontype");
                TextBox txtdeductionamount = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionamount");
                TextBox txtdeductionmincapingamt = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionmincapingamt");
                TextBox txtdeductionmaxcapingamt = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionmaxcapingamt");
                Label lbldeductionmastertypeid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmastertypeid");

                dr["Payrolltypeid"] = DDpayrolltype.SelectedValue;
                dr["Parameterid"] = lbldeductionid.Text;
                dr["Taxable"] = dddeductiontaxable.SelectedItem.Text;
                dr["Allowance_Type"] = dddeductiontype.SelectedValue;
                dr["Percentage_Amount"] = txtdeductionamount.Text == "" ? "0" : txtdeductionamount.Text;
                dr["Mincapingamt"] = txtdeductionmincapingamt.Text == "" ? "0" : txtdeductionmincapingamt.Text;
                dr["Maxcapingamt"] = txtdeductionmaxcapingamt.Text == "" ? "0" : txtdeductionmaxcapingamt.Text;
                dr["Allowance_Deduction_Id"] = lbldeductionmastertypeid.Text;

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
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[1] = new SqlParameter("@userid", Session["varuserid"]);
                param[2] = new SqlParameter("@dt", dt);
                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                param[4] = new SqlParameter("@Payrolltypeid", DDpayrolltype.SelectedValue);
                //*********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_HR_SaveParameterDesc", param);
                lblmsg.Text = param[3].Value.ToString();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please select atleast one check box to save data?')", true);
        }
    }
    protected void refreshpayrolltypemaster_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDpayrolltype, "select Payrolltypeid,Payrolltypename From HR_PayrollTypeMaster order by Payrolltypename", true, "--Plz Select--");
    }
}