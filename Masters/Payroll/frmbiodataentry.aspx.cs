using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class Masters_Payroll_frmbiodataentry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (!Page.IsPostBack)
            {
                SetInitialRow_Experience();
                SetInitialRow_familymembers();
            }
            tcbiodata.ActiveTabIndex = 0;
            string str = @"SELECT DESIGNATIONID,DESIGNATION FROM HR_DESIGNATIONMASTER ORDER BY DISPSEQNO,DESIGNATION 
                            Select Distinct D.DepartmentId, D.DepartmentName 
                            From Department D(Nolock)
                            JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                            JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                            Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                            Order By D.DepartmentName 
                            SELECT SUBDEPTID,SUBDEPT FROM HR_SUBDEPT ORDER BY DISPSEQNO,SUBDEPT
                            SELECT DIVISIONID,DIVISION FROM HR_DIVISIONMASTER ORDER BY DISPSEQNO,DIVISION
                            SELECT CATEGORYID,CATEGORY FROM HR_CATEGORYMASTER ORDER BY DISPSEQNO,CATEGORY
                            SELECT CADREID,CADRE FROM HR_CADREMASTER ORDER BY DISPSEQNO,CADRE
                            SELECT CO.COMPANYID,CO.COMPANYNAME FROM COMPANYINFO CO INNER JOIN COMPANY_AUTHENTICATION CA ON CO.COMPANYID=CA.COMPANYID AND CA.USERID=" + Session["varuserid"] + @" ORDER BY COMPANYNAME
                            SELECT LOCATIONID,LOCATION FROM HR_LOCATIONMASTER ORDER BY DISPSEQNO,LOCATION
                            SELECT DOCTYPEID,DOCTYPE FROM HR_DOCTYPEMASTER ORDER BY DISPSEQNO,DOCTYPE
                            SELECT PAYROLLTYPEID,PAYROLLTYPENAME FROM HR_PAYROLLTYPEMASTER ORDER BY PAYROLLTYPENAME
                            select ShiftId,shiftcode + ' - ' + Intime+'-'+Outtime as Shiftcode From HR_ShiftMaster order by ShiftId
                            SELECT GROUPID,GROUPNAME FROM HR_GROUPMASTER
                            Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " Order By BranchName ";


            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref ddpostapplied, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddesignation, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddepartment, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddsubdept, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddivision, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcategory, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcadre, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcompany, ds, 6, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddlocation, ds, 7, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddoctype, ds, 8, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDpayrolltype, ds, 9, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddshiftoption, ds, 10, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddempgroup, ds, 11, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 12, false, "");

            switch (Session["usertype"].ToString())
            {
                case "1":
                case "2":
                case "5":
                    Tredit.Visible = true;
                    break;
                default:
                    Tredit.Visible = false;
                    break;
            }
            if (ddcompany.Items.Count > 0)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
            }
        }
    }
    private void SetInitialRow_Experience()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Srno", typeof(string)));
        dt.Columns.Add(new DataColumn("nameofemployer", typeof(string)));
        dt.Columns.Add(new DataColumn("postheld", typeof(string)));
        dt.Columns.Add(new DataColumn("from_to", typeof(string)));
        dt.Columns.Add(new DataColumn("DOJ", typeof(string)));
        dt.Columns.Add(new DataColumn("DOL", typeof(string)));
        dt.Columns.Add(new DataColumn("Reasonforleaving", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["nameofemployer"] = string.Empty;
        dr["postheld"] = string.Empty;
        dr["DOJ"] = string.Empty;
        dr["DOL"] = string.Empty;
        dr["from_to"] = string.Empty;
        dr["Reasonforleaving"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();
        //Store the DataTable in ViewState
        ViewState["dgexperience"] = dt;

        DGexperience.DataSource = dt;
        DGexperience.DataBind();
    }
    private void SetInitialRow_familymembers()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Srno", typeof(string)));
        dt.Columns.Add(new DataColumn("name", typeof(string)));
        dt.Columns.Add(new DataColumn("yearofbirth", typeof(string)));
        dt.Columns.Add(new DataColumn("Relationid", typeof(int)));
        dt.Columns.Add(new DataColumn("Address", typeof(string)));
        dt.Columns.Add(new DataColumn("SetasNominee", typeof(int)));
        dt.Columns.Add(new DataColumn("Share", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["name"] = string.Empty;
        dr["yearofbirth"] = string.Empty;
        dr["relationid"] = 0;
        dr["Address"] = string.Empty;
        dr["setasNominee"] = 0;
        dr["Share"] = string.Empty;
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["dgfamilymembers"] = dt;

        Dgfamilymembers.DataSource = dt;
        Dgfamilymembers.DataBind();
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid_Experience();
    }
    protected void ButtonAddfamilymembers_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid_familymembers();
    }
    private void AddNewRowToGrid_Experience()
    {
        int rowIndex = 0;

        if (ViewState["dgexperience"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["dgexperience"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtsrno = (TextBox)DGexperience.Rows[rowIndex].Cells[0].FindControl("txtsrno");
                    TextBox txtnameofemployer = (TextBox)DGexperience.Rows[rowIndex].Cells[1].FindControl("txtnameofemployer");
                    TextBox txtpostheld = (TextBox)DGexperience.Rows[rowIndex].Cells[2].FindControl("txtpostheld");
                    TextBox txtdoj = (TextBox)DGexperience.Rows[rowIndex].Cells[2].FindControl("txtdoj");
                    TextBox txtdol = (TextBox)DGexperience.Rows[rowIndex].Cells[2].FindControl("txtdol");
                    TextBox txtfromto = (TextBox)DGexperience.Rows[rowIndex].Cells[3].FindControl("txtfromto");
                    TextBox txtreasonforleaving = (TextBox)DGexperience.Rows[rowIndex].Cells[4].FindControl("txtreasonforleaving");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["nameofemployer"] = txtnameofemployer.Text;
                    dtCurrentTable.Rows[i - 1]["postheld"] = txtpostheld.Text;
                    dtCurrentTable.Rows[i - 1]["Doj"] = txtdoj.Text;
                    dtCurrentTable.Rows[i - 1]["Dol"] = txtdol.Text;
                    dtCurrentTable.Rows[i - 1]["from_to"] = txtfromto.Text;
                    dtCurrentTable.Rows[i - 1]["reasonforleaving"] = txtreasonforleaving.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["dgexperience"] = dtCurrentTable;

                DGexperience.DataSource = dtCurrentTable;
                DGexperience.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData_Experience();
    }
    private void AddNewRowToGrid_familymembers()
    {
        int rowIndex = 0;

        if (ViewState["dgfamilymembers"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["dgfamilymembers"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtsrno = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtsrno");
                    TextBox txtfamilymembername = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtfamilymembername");
                    TextBox txtyearofbirth = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtyearofbirth");
                    Label lblrelationid = (Label)Dgfamilymembers.Rows[rowIndex].FindControl("lblrelationid");
                    DropDownList ddrelation = (DropDownList)Dgfamilymembers.Rows[rowIndex].FindControl("ddrelation");
                    TextBox txtaddressnominee = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtaddressnominee");
                    TextBox txtshare = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtshare");
                    CheckBox chksetasnominee = (CheckBox)Dgfamilymembers.Rows[rowIndex].FindControl("chksetasnominee");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;
                    drCurrentRow["setasNominee"] = "0";

                    dtCurrentTable.Rows[i - 1]["name"] = txtfamilymembername.Text;
                    dtCurrentTable.Rows[i - 1]["yearofbirth"] = txtyearofbirth.Text;
                    dtCurrentTable.Rows[i - 1]["Relationid"] = ddrelation.SelectedIndex > 0 ? ddrelation.SelectedValue : "0";
                    dtCurrentTable.Rows[i - 1]["Address"] = txtaddressnominee.Text;
                    dtCurrentTable.Rows[i - 1]["setasnominee"] = chksetasnominee.Checked == true ? "1" : "0";
                    dtCurrentTable.Rows[i - 1]["share"] = txtshare.Text == "" ? DBNull.Value : (object)txtshare.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["dgfamilymembers"] = dtCurrentTable;

                Dgfamilymembers.DataSource = dtCurrentTable;
                Dgfamilymembers.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData_familymembers();
    }
    private void SetPreviousData_Experience()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["dgexperience"] != null)
        {
            DataTable dt = (DataTable)ViewState["dgexperience"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtsrno = (TextBox)DGexperience.Rows[rowIndex].Cells[0].FindControl("txtsrno");
                    TextBox txtnameofemployer = (TextBox)DGexperience.Rows[rowIndex].Cells[1].FindControl("txtnameofemployer");
                    TextBox txtpostheld = (TextBox)DGexperience.Rows[rowIndex].Cells[2].FindControl("txtpostheld");
                    TextBox txtdoj = (TextBox)DGexperience.Rows[rowIndex].Cells[3].FindControl("txtdoj");
                    TextBox txtdol = (TextBox)DGexperience.Rows[rowIndex].Cells[3].FindControl("txtdol");
                    TextBox txtfromto = (TextBox)DGexperience.Rows[rowIndex].Cells[3].FindControl("txtfromto");
                    TextBox txtreasonforleaving = (TextBox)DGexperience.Rows[rowIndex].Cells[4].FindControl("txtreasonforleaving");


                    txtsrno.Text = dt.Rows[i]["srno"].ToString();
                    txtnameofemployer.Text = dt.Rows[i]["nameofemployer"].ToString();
                    txtpostheld.Text = dt.Rows[i]["postheld"].ToString();
                    txtdoj.Text = dt.Rows[i]["doj"].ToString();
                    txtdol.Text = dt.Rows[i]["dol"].ToString();
                    txtfromto.Text = dt.Rows[i]["from_to"].ToString();
                    txtreasonforleaving.Text = dt.Rows[i]["Reasonforleaving"].ToString();

                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);

                    rowIndex++;


                }

                //InsertRecords(sc);
            }
        }
    }
    private void SetPreviousData_familymembers()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["dgfamilymembers"] != null)
        {
            DataTable dt = (DataTable)ViewState["dgfamilymembers"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtsrno = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtsrno");
                    TextBox txtfamilymembername = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtfamilymembername");
                    TextBox txtyearofbirth = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtyearofbirth");
                    TextBox txtaddressnominee = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtaddressnominee");
                    DropDownList ddrelation = (DropDownList)Dgfamilymembers.Rows[rowIndex].FindControl("ddrelation");
                    CheckBox chksetasnominee = (CheckBox)Dgfamilymembers.Rows[rowIndex].FindControl("chksetasnominee");
                    TextBox txtshare = (TextBox)Dgfamilymembers.Rows[rowIndex].FindControl("txtshare");

                    txtsrno.Text = dt.Rows[i]["srno"].ToString();
                    txtfamilymembername.Text = dt.Rows[i]["name"].ToString();
                    txtyearofbirth.Text = dt.Rows[i]["yearofbirth"].ToString();
                    txtaddressnominee.Text = dt.Rows[i]["address"].ToString();
                    if (ddrelation.Items.FindByValue(dt.Rows[i]["relationid"].ToString()) != null)
                    {
                        ddrelation.SelectedValue = dt.Rows[i]["relationid"].ToString();
                    }
                    chksetasnominee.Checked = Convert.ToBoolean(dt.Rows[i]["setasNominee"]);
                    txtshare.Text = dt.Rows[i]["share"].ToString();
                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);
                    rowIndex++;
                }

                //InsertRecords(sc);
            }
        }
    }
    protected void btnsameaddress_Click(object sender, EventArgs e)
    {
        txtvill_permanent.Text = txtvill_present.Text;
        txtpo_permanent.Text = txtpo_present.Text;
        txtpolicestation_permanent.Text = txtpolicestation_present.Text;
        txtsubdivision_permanent.Text = txtsubdivision_present.Text;
        txtdistt_permanent.Text = txtdistt_present.Text;
        txtstate_permanent.Text = txtstate_present.Text;
        txtpincode_permanent.Text = txtpincode_present.Text;
        txtphno_permanent.Text = txtphno_present.Text;
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
        decimal allowances = 0, deductions = 0, Amount = 0, GrossDeduction = 0, GrosssalDeduction = 0;
        if (basicpay > 0)
        {
            //*******Allowances
            foreach (GridViewRow gvr in DGallowances.Rows)
            {
                Label lblallowancetype = (Label)gvr.FindControl("lblallowance_type");
                Label lblallowancepercent_amount = (Label)gvr.FindControl("lblallowancepercent_amount");
                Label lblallowancemaxcapingamt = (Label)gvr.FindControl("lblallowancemaxcapingamt");
                TextBox txtallowanceamount = (TextBox)gvr.FindControl("txtallowanceamount");

                if (lblallowancetype.Text != "2")
                {
                    //0 for percentage of basic sal,1 fixed amt
                    if (lblallowancetype.Text == "0")
                    {
                        Amount = Math.Round(basicpay * Convert.ToDecimal(lblallowancepercent_amount.Text) / 100, 2, MidpointRounding.AwayFromZero);
                        if (Amount > Convert.ToDecimal(lblallowancemaxcapingamt.Text) && Convert.ToDecimal(lblallowancemaxcapingamt.Text) > 0)
                        {
                            Amount = Convert.ToDecimal(lblallowancemaxcapingamt.Text);
                        }
                        txtallowanceamount.Text = Amount.ToString();
                        allowances = allowances + Amount;
                    }
                    else
                    {
                        switch (lblallowancetype.Text)
                        {
                            case "1":
                                allowances = allowances + Convert.ToDecimal(txtallowanceamount.Text == "" ? "0" : txtallowanceamount.Text);
                                break;
                            case "2":
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            //*******Deductions
            foreach (GridViewRow gvr in DGDeductions.Rows)
            {
                Label lbldeductiontype = (Label)gvr.FindControl("lbldeduction_type");
                Label lbldeductionamount = (Label)gvr.FindControl("lbldeductionamount");
                Label lbldeductionpercent_amount = (Label)gvr.FindControl("lbldeductionpercent_amount");
                Label lbldeductionmaxcapingamt = (Label)gvr.FindControl("lbldeductionmaxcapingamt");
                TextBox txtdeductionamount = (TextBox)gvr.FindControl("txtdeductionamount");
                //0 for percentage of basic sal,1 fixed amt
                if (lbldeductiontype.Text != "2")
                {

                    if (lbldeductiontype.Text == "0")
                    {
                        Amount = Math.Round(basicpay * Convert.ToDecimal(lbldeductionpercent_amount.Text) / 100, 2, MidpointRounding.AwayFromZero);
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
                    Amount = Math.Round(GrosssalDeduction * Convert.ToDecimal(lbldeductionpercent_amount.Text) / 100, 2, MidpointRounding.AwayFromZero);
                    if (Amount > Convert.ToDecimal(lbldeductionmaxcapingamt.Text) && Convert.ToDecimal(lbldeductionmaxcapingamt.Text) > 0)
                    {
                        Amount = Convert.ToDecimal(lbldeductionmaxcapingamt.Text);
                    }
                    txtdeductionamount.Text = Amount.ToString();
                    GrossDeduction = GrossDeduction + Amount;
                }
            }
            txtnetsal.Text = Convert.ToString(basicpay + allowances - (deductions + GrossDeduction));
        }
    }
    protected void txtbasicpay_TextChanged(object sender, EventArgs e)
    {
        GetGross_Netsalary();

    }
    protected void DDpayrolltype_SelectedIndexChanged(object sender, EventArgs e)
    {

        Fill_allowances_Deductions();

        GetGross_Netsalary();
    }
    protected void Fill_allowances_DeductionsEdit(DataSet ds)
    {
        //***********Allowances
        DataView dvallowances = new DataView(ds.Tables[3]);
        dvallowances.RowFilter = "Allowance_deduction_id=1";
        DGallowances.DataSource = dvallowances;
        DGallowances.DataBind();
        //********deductions
        DataView dvdeductions = new DataView(ds.Tables[3]);
        dvdeductions.RowFilter = "Allowance_deduction_id=2";
        DGDeductions.DataSource = dvdeductions;
        DGDeductions.DataBind();
    }

    protected void Fill_allowances_Deductions()
    {
        string str = @"select AM.ParameterName,case when PD.Allowance_type=0 Then 'Percentage of Basic salary('+cast(PD.Percent_Amount as varchar)+'%)'  
                     When Pd.Allowance_type=2 Then   'Percentage of Gross salary('+cast(PD.Percent_Amount as varchar)+'%)'
                     else 'Fixed amount' End Allowance_deductiontype,
                    PD.ParameterId,PD.Allowance_Deduction_Id,Case WHen PD.Allowance_type=1 Then Percent_Amount else 0 End as Amount,PD.Maxcapingamount,PD.Mincapingamount,PD.Allowance_type,PD.Percent_Amount,Taxable 
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        DataTable dtfamilymembers = new DataTable();

        dtfamilymembers.Columns.Add("SrNo", typeof(string));
        dtfamilymembers.Columns.Add("Name", typeof(string));
        dtfamilymembers.Columns.Add("YearofBirth", typeof(string));
        dtfamilymembers.Columns.Add("Relation", typeof(int));
        dtfamilymembers.Columns.Add("address", typeof(string));
        dtfamilymembers.Columns.Add("setasnominee", typeof(int));
        dtfamilymembers.Columns.Add("share", typeof(string));

        for (int i = 0; i < Dgfamilymembers.Rows.Count; i++)
        {
            TextBox txtsrno = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtsrno");
            TextBox txtfamilymembername = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtfamilymembername");
            TextBox txtyearofbirth = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtyearofbirth");
            DropDownList ddrelation = (DropDownList)Dgfamilymembers.Rows[i].FindControl("ddrelation");
            TextBox txtaddressnominee = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtaddressnominee");
            CheckBox chksetasnominee = (CheckBox)Dgfamilymembers.Rows[i].FindControl("chksetasnominee");
            TextBox txtshare = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtshare");

            if (txtfamilymembername.Text != "")
            {
                DataRow dr = dtfamilymembers.NewRow();
                dr["srno"] = txtsrno.Text;
                dr["Name"] = txtfamilymembername.Text;
                dr["Yearofbirth"] = txtyearofbirth.Text;
                dr["Relation"] = ddrelation.SelectedIndex > 0 ? ddrelation.SelectedValue : "0";
                dr["address"] = txtaddressnominee.Text;
                dr["setasnominee"] = chksetasnominee.Checked == true ? "1" : "0";
                dr["share"] = txtshare.Text == "" ? DBNull.Value : (object)txtshare.Text;
                dtfamilymembers.Rows.Add(dr);
            }

        }
        //*************Experience Detail
        DataTable dtexperience = new DataTable();

        dtexperience.Columns.Add(new DataColumn("Srno", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("nameofemployer", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("postheld", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("Doj", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("Dol", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("from_to", typeof(string)));
        dtexperience.Columns.Add(new DataColumn("Reasonforleaving", typeof(string)));

        for (int i = 0; i < DGexperience.Rows.Count; i++)
        {
            TextBox txtsrno = (TextBox)DGexperience.Rows[i].FindControl("txtsrno");
            TextBox txtnameofemployer = (TextBox)DGexperience.Rows[i].FindControl("txtnameofemployer");
            TextBox txtpostheld = (TextBox)DGexperience.Rows[i].FindControl("txtpostheld");
            TextBox txtdoj = (TextBox)DGexperience.Rows[i].FindControl("txtdoj");
            TextBox txtdol = (TextBox)DGexperience.Rows[i].FindControl("txtdol");
            TextBox txtfromto = (TextBox)DGexperience.Rows[i].FindControl("txtfromto");
            TextBox txtreasonforleaving = (TextBox)DGexperience.Rows[i].FindControl("txtreasonforleaving");
            if (txtnameofemployer.Text != "")
            {
                DataRow dr = dtexperience.NewRow();

                dr["Srno"] = txtsrno.Text;
                dr["nameofemployer"] = txtnameofemployer.Text;
                dr["postheld"] = txtpostheld.Text;
                dr["doj"] = txtdoj.Text == "" ? DBNull.Value : (object)txtdoj.Text;
                dr["dol"] = txtdol.Text == "" ? DBNull.Value : (object)txtdol.Text;
                dr["from_to"] = txtfromto.Text;
                dr["Reasonforleaving"] = txtreasonforleaving.Text;
                dtexperience.Rows.Add(dr);
            }

        }
        //*************Payroll Parameter
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
            DataRow dr = dt.NewRow();
            Label lblallowanceid = (Label)DGallowances.Rows[i].FindControl("lblallowanceid");
            Label lbltaxableallowance = (Label)DGallowances.Rows[i].FindControl("lbltaxableallowance");
            Label lblallowance_type = (Label)DGallowances.Rows[i].FindControl("lblallowance_type");
            Label lblallowancepercent_amount = (Label)DGallowances.Rows[i].FindControl("lblallowancepercent_amount");
            Label lblallowancemincapingamt = (Label)DGallowances.Rows[i].FindControl("lblallowancemincapingamt");
            Label lblallowancemaxcapingamt = (Label)DGallowances.Rows[i].FindControl("lblallowancemaxcapingamt");
            Label lblAllowance_Deduction_Id = (Label)DGallowances.Rows[i].FindControl("lblAllowance_Deduction_Id");
            TextBox txtallowanceamount = (TextBox)DGallowances.Rows[i].FindControl("txtallowanceamount");

            dr["Payrolltypeid"] = DDpayrolltype.SelectedValue;
            dr["Parameterid"] = lblallowanceid.Text;
            dr["Taxable"] = lbltaxableallowance.Text;
            dr["Allowance_Type"] = lblallowance_type.Text;

            switch (lblallowance_type.Text)
            {
                case "1":
                    dr["Percentage_Amount"] = txtallowanceamount.Text == "" ? "0" : txtallowanceamount.Text;
                    break;
                default:
                    dr["Percentage_Amount"] = lblallowancepercent_amount.Text == "" ? "0" : lblallowancepercent_amount.Text;
                    break;
            }
            dr["Mincapingamt"] = lblallowancemincapingamt.Text == "" ? "0" : lblallowancemincapingamt.Text;
            dr["Maxcapingamt"] = lblallowancemaxcapingamt.Text == "" ? "0" : lblallowancemaxcapingamt.Text;
            dr["Allowance_Deduction_Id"] = lblAllowance_Deduction_Id.Text;
            dt.Rows.Add(dr);

        }
        //****Deductions
        for (int i = 0; i < DGDeductions.Rows.Count; i++)
        {

            DataRow dr = dt.NewRow();
            Label lbldeductionid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionid");
            Label lbldeductiontaxable = (Label)DGDeductions.Rows[i].FindControl("lbldeductiontaxable");
            Label lbldeduction_type = (Label)DGDeductions.Rows[i].FindControl("lbldeduction_type");
            Label lbldeductionpercent_amount = (Label)DGDeductions.Rows[i].FindControl("lbldeductionpercent_amount");
            Label lbldeductionmincapingamt = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmincapingamt");
            Label lbldeductionmaxcapingamt = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmaxcapingamt");
            Label lbldeductionmastertypeid = (Label)DGDeductions.Rows[i].FindControl("lbldeductionmastertypeid");
            TextBox txtdeductionamount = (TextBox)DGDeductions.Rows[i].FindControl("txtdeductionamount");

            dr["Payrolltypeid"] = DDpayrolltype.SelectedValue;
            dr["Parameterid"] = lbldeductionid.Text;
            dr["Taxable"] = lbldeductiontaxable.Text;
            dr["Allowance_Type"] = lbldeduction_type.Text;
            switch (lbldeduction_type.Text)
            {
                case "1":
                    dr["Percentage_Amount"] = txtdeductionamount.Text == "" ? "0" : txtdeductionamount.Text;
                    break;
                default:
                    dr["Percentage_Amount"] = lbldeductionpercent_amount.Text == "" ? "0" : lbldeductionpercent_amount.Text;
                    break;
            }
            dr["Mincapingamt"] = lbldeductionmincapingamt.Text == "" ? "0" : lbldeductionmincapingamt.Text;
            dr["Maxcapingamt"] = lbldeductionmaxcapingamt.Text == "" ? "0" : lbldeductionmaxcapingamt.Text;
            dr["Allowance_Deduction_Id"] = lbldeductionmastertypeid.Text;

            dt.Rows.Add(dr);

        }
        //*********************
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[129];
            arr[0] = new SqlParameter("@EMPID", SqlDbType.Int);
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = hnempid.Value;
            arr[1] = new SqlParameter("@DEPARTMENTID", dddepartment.SelectedValue);
            arr[2] = new SqlParameter("@EMPNAME", txtname.Text);
            arr[3] = new SqlParameter("@FATHERNAME", txtfather_husbandname.Text);
            arr[4] = new SqlParameter("@DOB", txtdateofbirth.Text == "" ? DBNull.Value : (object)txtdateofbirth.Text);
            arr[5] = new SqlParameter("@EMPCODE", SqlDbType.VarChar, 50);
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = txtpaycode.Text;
            arr[6] = new SqlParameter("@POSTAPPLIEDFOR", ddpostapplied.SelectedValue);
            arr[7] = new SqlParameter("@MOTHERNAME", txtmothername.Text);
            arr[8] = new SqlParameter("@GENDER", rdmale.Checked == true ? "1" : (rdfemale.Checked == true ? "2" : "0"));
            arr[9] = new SqlParameter("@MARITALSTATUS", rdmarried.Checked == true ? "1" : (rdunmarried.Checked == true ? "2" : "0"));
            arr[10] = new SqlParameter("@NATIONALITY", txtnationality.Text);
            arr[11] = new SqlParameter("@RELIGION", txtreligion.Text);
            arr[12] = new SqlParameter("@CASTE", txtcaster.Text);
            arr[13] = new SqlParameter("@REGION", txtregion.Text);
            arr[14] = new SqlParameter("@SHIFTTYPE", rdfixed.Checked == true ? "1" : (rdrotational.Checked == true ? "2" : "0"));
            arr[15] = new SqlParameter("@DESIGNATION", dddesignation.SelectedValue);
            arr[16] = new SqlParameter("@SUBDEPT", ddsubdept.SelectedValue);
            arr[17] = new SqlParameter("@DIVISION", dddivision.SelectedValue);
            arr[18] = new SqlParameter("@CATEGORYID", ddcategory.SelectedValue);
            arr[19] = new SqlParameter("@CADREID", ddcadre.SelectedValue);
            arr[20] = new SqlParameter("@COMPANYID", ddcompany.SelectedValue);
            arr[21] = new SqlParameter("@LOCATIONID", ddlocation.SelectedValue);
            arr[22] = new SqlParameter("@SHIFTOPTIONID", ddshiftoption.SelectedValue);
            arr[23] = new SqlParameter("@DOCTYPEID", dddoctype.SelectedValue);
            arr[24] = new SqlParameter("@DOCNO", txtdocno.Text);
            arr[25] = new SqlParameter("@VILL_PRESENT", txtvill_present.Text);
            arr[26] = new SqlParameter("@PO_PRESENT", txtpo_present.Text);
            arr[27] = new SqlParameter("@POLICESTATION_PRESENT", txtpolicestation_present.Text);
            arr[28] = new SqlParameter("@SUBDIVISION_PRESENT", txtsubdivision_present.Text);
            arr[29] = new SqlParameter("@DISTT_PRESENT", txtdistt_present.Text);
            arr[30] = new SqlParameter("@STATE_PRESENT", txtstate_present.Text);
            arr[31] = new SqlParameter("@PINCODE_PRESENT", txtpincode_present.Text);
            arr[32] = new SqlParameter("@PHNO_PRESENT", txtphno_present.Text);
            arr[33] = new SqlParameter("@VILL_PERMANENT", txtvill_permanent.Text);
            arr[34] = new SqlParameter("@POLICESTATION_PERMANENT", txtpolicestation_permanent.Text);
            arr[35] = new SqlParameter("@SUBDIVISION_PERMANENT", txtsubdivision_permanent.Text);
            arr[36] = new SqlParameter("@DISTT_PERMANENT", txtdistt_permanent.Text);
            arr[37] = new SqlParameter("@STATE_PERMANENT", txtstate_permanent.Text);
            arr[38] = new SqlParameter("@PINCODE_PERMANENT", txtpincode_permanent.Text);
            arr[39] = new SqlParameter("@PHNO_PERMANENT", txtphno_permanent.Text);
            arr[40] = new SqlParameter("@QUALIFICATION", txtqualification.Text);
            arr[41] = new SqlParameter("@TECHNICALQUALIFICATION", txttechnicalqualification.Text);
            arr[42] = new SqlParameter("@LANGUAGEKNOWN", txtlanguageknown.Text);
            arr[43] = new SqlParameter("@TOTALEXPERIENCE", txttotalexperience.Text);
            arr[44] = new SqlParameter("@RELATIVEINCOMPANY", rdrelativeworkyes.Checked == true ? "1" : (rdrelativeworkno.Checked == true ? "2" : "0"));
            arr[45] = new SqlParameter("@WITNESSNAME1", txtwitnessname_1.Text);
            arr[46] = new SqlParameter("@WITNESSRELATIONSHIP1", txtwitnessrelation_1.Text);
            arr[47] = new SqlParameter("@WITNESSADDRESS1", txtwitnessaddress_1.Text);
            arr[48] = new SqlParameter("@WITNESSNAME2", txtwitnessname_2.Text);
            arr[49] = new SqlParameter("@WITNESSRELATIONSHIP2", txtwitnessrelationship_2.Text);
            arr[50] = new SqlParameter("@WITNESSADDRESS2", txtwitnessaddress_2.Text);
            arr[51] = new SqlParameter("@APPOINTMENTDATE", txtappointmentdate.Text == "" ? DBNull.Value : (object)txtappointmentdate.Text);
            arr[52] = new SqlParameter("@PLACE", txtplace.Text);
            arr[53] = new SqlParameter("@INTERVIEWDATE", txtinterviewdate.Text == "" ? DBNull.Value : (object)txtinterviewdate.Text);
            arr[54] = new SqlParameter("@INTERVIEWBY", txtinterviewby.Text);
            arr[55] = new SqlParameter("@DATEOFJOINING", txtdateofjoining.Text == "" ? DBNull.Value : (object)txtdateofjoining.Text);
            arr[56] = new SqlParameter("@CONFIRMDATE", txtconfirmdate.Text == "" ? DBNull.Value : (object)txtconfirmdate.Text);
            arr[57] = new SqlParameter("@INDIVIDUALBIODATA", ddindividualdate.SelectedItem.Text);
            arr[58] = new SqlParameter("@PHOTO", ddphoto.SelectedItem.Text);
            arr[59] = new SqlParameter("@APPLICATIONOFEMPLOYMENT", ddapplicationforemployment.SelectedItem.Text);
            arr[60] = new SqlParameter("@PROOFOFAGE", ddproofofage.SelectedItem.Text);
            arr[61] = new SqlParameter("@PROOFNAME", txtproofname.Text);
            arr[62] = new SqlParameter("@CERTIFICATETESTIMONIALS", ddcertificates_testimonials.SelectedItem.Text);
            arr[63] = new SqlParameter("@CONTRACTOFEMPLOYMENT", ddcontractofemployment.SelectedItem.Text);
            arr[64] = new SqlParameter("@JOININGREPORT", ddjoiningreport.SelectedItem.Text);
            arr[65] = new SqlParameter("@NOMINATIONFORM", ddnominationform.SelectedItem.Text);
            arr[66] = new SqlParameter("@EMPLOYEETYPE", rdpermanent.Checked == true ? "1" : (rdcasual.Checked == true ? "2" : "0"));
            arr[67] = new SqlParameter("@WAGESCALCULATION", rdmonthly.Checked == true ? "1" : (rddaily.Checked == true ? "2" : (rdpcswise.Checked == true ? "3" : "0")));
            arr[68] = new SqlParameter("@PAYMENTTYPE", rdcash.Checked == true ? "1" : (rdcheque.Checked == true ? "2" : (rdbank.Checked == true ? "3" : "0")));
            arr[69] = new SqlParameter("@OVERTIME", rdenableovertime.Checked == true ? "1" : (rddisableovertime.Checked == true ? "2" : "0"));
            arr[70] = new SqlParameter("@FOODING", rdenablefooding.Checked == true ? "1" : (rddisbalefooding.Checked == true ? "2" : "0"));
            arr[71] = new SqlParameter("@SUN_HDPAY", rdenablesunhdpay.Checked == true ? "1" : (rddisablesunhdpay.Checked == true ? "2" : "0"));
            arr[72] = new SqlParameter("@ESI", rdesiyes.Checked == true ? "1" : (rdesino.Checked == true ? "2" : "0"));
            arr[73] = new SqlParameter("@ESI_INSURANCENO", txtesiinsuranceno.Text);
            arr[74] = new SqlParameter("@ESI_EMPLOYERCODE", txtesiemployercode.Text);
            arr[75] = new SqlParameter("@ESI_DISPENSARY", txtesidispensary.Text);
            arr[76] = new SqlParameter("@ESI_LOCALOFFICE", txtesilocaloffice.Text);
            arr[77] = new SqlParameter("@ESI_NOMINEEFORPAYMENT", txtesinomineeforpayment.Text);
            arr[78] = new SqlParameter("@ESI_PARTICULARSOFFAMILY", txtesiparticularsoffamily.Text);
            arr[79] = new SqlParameter("@ESI_FAMILYMEMBERRESIDINGWITHINSUREDPERSON", txtesifamilymemberresidingwithinsuredperson.Text);
            arr[80] = new SqlParameter("@PF", rdpfyes.Checked == true ? "1" : (rdpfno.Checked == true ? "2" : "0"));
            arr[81] = new SqlParameter("@PF_ACCOUNTNO", txtpfaccountno.Text);
            arr[82] = new SqlParameter("@PF_NOMINEE", txtpfnominee_nominees.Text);
            arr[83] = new SqlParameter("@PF_SHAREOFNOMINEE", txtpfsharepercentageofnominee.Text == "" ? "0" : txtpfsharepercentageofnominee.Text);
            arr[84] = new SqlParameter("@PF_CHILDPENSION", ddchildrenpension.SelectedItem.Text);
            arr[85] = new SqlParameter("@PF_WIDOWPENSION", ddwidowpension.SelectedItem.Text);
            arr[86] = new SqlParameter("@PF_UANNO", txtpfuanno.Text);
            arr[87] = new SqlParameter("@IFSCCODE", txtifsccode.Text);
            arr[88] = new SqlParameter("@BANKNAME", txtbankname.Text);
            arr[89] = new SqlParameter("@BRANCH", txtbranch.Text);
            arr[90] = new SqlParameter("@BANKADDRESS", txtbankaddress.Text);
            arr[91] = new SqlParameter("@BANKACNO", txtacno.Text);
            arr[92] = new SqlParameter("@NOMINEESFORGRATUITY", txtnomineeforgratuity.Text);
            arr[93] = new SqlParameter("@GRAUITYSHAREOFNOMINEE", txtgratuitysharepercentageofnominee.Text == "" ? "0" : txtgratuitysharepercentageofnominee.Text);
            arr[94] = new SqlParameter("@USERID", Session["varuserid"]);
            arr[95] = new SqlParameter("@MASTERCOMPANYID", Session["varMasterCompanyIDForERP"]);
            arr[96] = new SqlParameter("@DTEXPERIENCE", dtexperience);
            arr[97] = new SqlParameter("@DTFAMILYMEMBERS", dtfamilymembers);
            arr[98] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            arr[98].Direction = ParameterDirection.Output;
            arr[99] = new SqlParameter("@PO_PERMANENT", txtpo_permanent.Text);
            arr[100] = new SqlParameter("@MinimumWagesBasic", txtbasic.Text == "" ? DBNull.Value : (object)txtbasic.Text);
            arr[101] = new SqlParameter("@DA", txtda.Text == "" ? DBNull.Value : (object)txtda.Text);
            arr[102] = new SqlParameter("@Basicpay", txtbasicpay.Text == "" ? DBNull.Value : (object)txtbasicpay.Text);
            arr[103] = new SqlParameter("@Grosssalary", txtgrosssal.Text == "" ? DBNull.Value : (object)txtgrosssal.Text);
            arr[104] = new SqlParameter("@Netsalary", txtgrosssal.Text == "" ? DBNull.Value : (object)txtnetsal.Text);
            arr[105] = new SqlParameter("@PayrollTypeid", DDpayrolltype.SelectedValue);
            arr[106] = new SqlParameter("@DTPAYROLLPARAMETER", dt);
            arr[107] = new SqlParameter("@Minimumwagesdate", txtminimumwagesdate.Text == "" ? DBNull.Value : (object)txtminimumwagesdate.Text);
            arr[108] = new SqlParameter("@NAME_COMPANYEMP", txtname_verifiedby.Text);
            arr[109] = new SqlParameter("@MOBILENO_COMPANYEMP", txtmobileno_verifiedby.Text);
            arr[110] = new SqlParameter("@NAME1_REFERENCE", txtname1_verification.Text);
            arr[111] = new SqlParameter("@MOBILENO1_REFERENCE", txtmobileno1_verification.Text);
            arr[112] = new SqlParameter("@NAME2_REFERENCE", txtname2_verification.Text);
            arr[113] = new SqlParameter("@MOBILENO2_REFERENCE", txtmobileno2_verification.Text);
            arr[114] = new SqlParameter("@NEIGHBOURNAME", txtneighbourname.Text);
            arr[115] = new SqlParameter("@MOBILENO_NEIGHBOUR", txtmobileno_neighbour.Text);
            arr[116] = new SqlParameter("@GRAAMPRADHANMEMBER", txtgraampradhan_memeber.Text);
            arr[117] = new SqlParameter("@VERIFICATIONDONEBY", txtverificationdoneby.Text);
            arr[118] = new SqlParameter("@CRIMINALBACKGROUND", DDcriminalbackground.SelectedItem.Text);
            arr[119] = new SqlParameter("@DESIGNATION_VERIFICATIONDONEBY", txtdesignation_verification.Text);
            arr[120] = new SqlParameter("@DATE_VERIFICATION", txtdate_verification.Text == "" ? DBNull.Value : (object)txtdate_verification.Text);
            arr[121] = new SqlParameter("@Resigndate", txtresigndate.Text == "" ? DBNull.Value : (object)txtresigndate.Text);
            arr[122] = new SqlParameter("@Reasonremark", txtreasonremarks.Text.Trim());
            arr[123] = new SqlParameter("@empgroupid", ddempgroup.SelectedIndex > 0 ? ddempgroup.SelectedValue : "0");
            arr[124] = new SqlParameter("@accountholdername", txtaccountholdername.Text);
            arr[125] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            arr[126] = new SqlParameter("@IdentificationMark", TxtIdentificationMark.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVEEMPLOYEEBIODATA", arr);
            Tran.Commit();
            hnempid.Value = arr[0].Value.ToString();
            txtpaycode.Text = arr[5].Value.ToString();
            txtcardno.Text = arr[5].Value.ToString();
            if (arr[98].Value.ToString() != "")
            {
                lblmsg.Text = arr[98].Value.ToString();
                ScriptManager.RegisterStartupScript(Page, GetType(), "altmsg", "alert('" + lblmsg.Text + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Employee Information Saved !!!')", true);
                SaveImage(Convert.ToInt32(hnempid.Value));

                refreshcontrol();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmsg.Visible = true;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Dgfamilymembers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddrelation = (DropDownList)e.Row.FindControl("ddrelation");
            UtilityModule.ConditionalComboFill(ref ddrelation, "select RELATIONID,RELATION From HR_RELATIONMASTER order by DISPSEQNO,RELATION", true, "--Plz Select--");
            Label lblrelationid = (Label)e.Row.FindControl("lblrelationid");
            if (ddrelation.Items.FindByValue(lblrelationid.Text) != null)
            {
                ddrelation.SelectedValue = lblrelationid.Text;
            }
        }
    }
    protected void txtdateofbirth_TextChanged(object sender, EventArgs e)
    {
        txtage.Text = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
        @"Select (Convert(int,(convert(Float,convert(int,convert(varchar(10),GETDATE(),112))) - convert(Float,convert(int,convert(varchar(10),convert(date,ltrim(rtrim('" + txtdateofbirth.Text + "'))),112)))))/10000)"));
        if (Convert.ToInt32(Session["varMasterCompanyIDForERP"]) == 28 && Convert.ToInt32(Session["varSubCompanyId"]) == 281)
        {
            if (Convert.ToDecimal(txtage.Text) < 18)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "agemsg", "alert('Age Must be Greater than or equal to 18 !!! ')", true);
                txtdateofbirth.Text = "";
                txtdateofbirth.Focus();
            }
        }
        else
        {
            if (Convert.ToDecimal(txtage.Text) < 18 || Convert.ToDecimal(txtage.Text) > 57)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "agemsg", "alert('Age Must be Greater than or equal to 18 and less than or Equal to 58 !!! ')", true);
                txtdateofbirth.Text = "";
                txtdateofbirth.Focus();
            }
        }
    }
    protected void SaveImage(int Empid)
    {
        if (fileuploadsignature.FileName != "")
        {
            string filename = Path.GetFileName(fileuploadsignature.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../Hrdocs");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../Hrdocs/" + Empid + "_Sig.gif");

            FileInfo file = new FileInfo(targetPath);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

            string img = "~\\Hrdocs\\" + Empid + "_Sig.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileuploadsignature.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileuploadsignature.FileName != null && fileuploadsignature.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update empinfo Set Signatureimage='" + img + "' Where empid=" + Empid + "");
            lblsignatureimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString();
        }
        if (fileuploadphoto.FileName != "")
        {
            string filename = Path.GetFileName(fileuploadphoto.PostedFile.FileName);
            string Folderpath = Server.MapPath("../../Hrdocs");
            //Check folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //
            string targetPath = Server.MapPath("../../Hrdocs/" + Empid + "_photo.gif");
            string img = "~\\Hrdocs\\" + Empid + "_photo.gif";
            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
            Stream strm = fileuploadphoto.PostedFile.InputStream;
            var targetFile = targetPath;
            if (fileuploadphoto.FileName != null && fileuploadphoto.FileName != "")
            {
                GenerateThumbnails(0.3, strm, targetFile);
            }
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update empinfo Set Empphoto='" + img + "' Where empid=" + Empid + "");
            lblphotoimage.ImageUrl = img + "?" + DateTime.Now.Ticks.ToString(); ;
        }
    }
    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        Tdeditcardno.Visible = false;
        hnempid.Value = "0";
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            Tdeditcardno.Visible = true;
        }
    }
    protected void refreshcontrol()
    {
        if (chkedit.Checked == true)
        {
            btnRemoveResign.Visible = false;
        }
        tcbiodata.ActiveTabIndex = 0;
        ddpostapplied.SelectedIndex = -1;
        //txtpaycode.Text = "";
        //txtcardno.Text = "";
        txtname.Text = "";
        txtfather_husbandname.Text = "";
        txtmothername.Text = "";
        txtdateofbirth.Text = "";
        txtage.Text = "";
        rdmale.Checked = true;
        rdfemale.Checked = false;
        rdunmarried.Checked = true;
        txtnationality.Text = "INDIAN";
        txtreligion.Text = "";
        txtcaster.Text = "";
        txtregion.Text = "";
        dddesignation.SelectedIndex = -1;
        dddepartment.SelectedIndex = -1;
        ddsubdept.SelectedIndex = -1;
        dddivision.SelectedIndex = -1;
        ddcategory.SelectedIndex = -1;
        ddcadre.SelectedIndex = -1;
        ddlocation.SelectedIndex = -1;
        ddshiftoption.SelectedIndex = -1;
        dddoctype.SelectedIndex = -1;
        txtdocno.Text = "";
        txtvill_present.Text = "";
        txtvill_permanent.Text = "";
        txtpo_present.Text = "";
        txtpo_permanent.Text = "";
        txtpolicestation_present.Text = "";
        txtpolicestation_permanent.Text = "";
        txtsubdivision_present.Text = "";
        txtsubdivision_permanent.Text = "";
        txtdistt_present.Text = "";
        txtdistt_permanent.Text = "";
        txtstate_present.Text = "";
        txtstate_permanent.Text = "";
        txtpincode_present.Text = "";
        txtpincode_permanent.Text = "";
        txtphno_present.Text = "";
        txtphno_permanent.Text = "";
        //*******Experience/Qualification Tab
        txtqualification.Text = "";
        txttechnicalqualification.Text = "";
        txtlanguageknown.Text = "";
        txttotalexperience.Text = "";
        SetInitialRow_Experience();
        //*******Family Members/Witness
        SetInitialRow_familymembers();
        rdrelativeworkyes.Checked = false;
        rdrelativeworkno.Checked = false;
        txtwitnessaddress_1.Text = "";
        txtwitnessaddress_2.Text = "";
        txtwitnessname_1.Text = "";
        txtwitnessname_2.Text = "";
        txtwitnessrelation_1.Text = "";
        txtwitnessrelationship_2.Text = "";
        //********Joining/Salary Information
        ddempgroup.SelectedIndex = -1;
        txtappointmentdate.Text = "";
        txtplace.Text = "";
        txtinterviewdate.Text = "";
        txtinterviewby.Text = "";
        txtdateofjoining.Text = "";
        txtconfirmdate.Text = "";
        txtresigndate.Text = "";
        txtreasonremarks.Text = "";
        rdpermanent.Checked = false;
        rdcasual.Checked = false;
        rdmonthly.Checked = false;
        rddaily.Checked = false;
        rdpcswise.Checked = false;
        rdcash.Checked = false;
        rdcheque.Checked = false;
        rdbank.Checked = false;
        rdenableovertime.Checked = false;
        rddisableovertime.Checked = false;
        rdenablefooding.Checked = false;
        rddisbalefooding.Checked = false;
        rdenablesunhdpay.Checked = false;
        rddisablesunhdpay.Checked = false;

        txtbasic.Text = "";
        txtda.Text = "";
        txtbasicpay.Text = "";
        txtgrosssal.Text = "";
        txtnetsal.Text = "";
        DDpayrolltype.SelectedIndex = -1;
        Fill_allowances_Deductions();
        //******* Statutory
        rdesiyes.Checked = false;
        rdesino.Checked = false;
        txtesiinsuranceno.Text = "";
        txtesiemployercode.Text = "";
        txtesidispensary.Text = "";
        txtesilocaloffice.Text = "";
        txtesinomineeforpayment.Text = "";
        txtesiparticularsoffamily.Text = "";
        txtesifamilymemberresidingwithinsuredperson.Text = "";
        txtnomineeforgratuity.Text = "";
        txtgratuitysharepercentageofnominee.Text = "";
        rdpfno.Checked = false;
        rdpfyes.Checked = false;
        txtpfaccountno.Text = "";
        txtpfnominee_nominees.Text = "";
        txtpfsharepercentageofnominee.Text = "";
        ddchildrenpension.SelectedItem.Text = "No";
        ddwidowpension.SelectedItem.Text = "No";
        txtpfuanno.Text = "";
        txtifsccode.Text = "";
        txtbankname.Text = "";
        txtbranch.Text = "";
        txtbankaddress.Text = "";
        txtacno.Text = "";
        txtaccountholdername.Text = "";
        //Previous Employment
        txtname_verifiedby.Text = "";
        txtmobileno_verifiedby.Text = "";
        txtname1_verification.Text = "";
        txtmobileno1_verification.Text = "";
        txtname2_verification.Text = "";
        txtmobileno2_verification.Text = "";
        txtneighbourname.Text = "";
        txtmobileno_neighbour.Text = "";
        txtgraampradhan_memeber.Text = "";
        txtverificationdoneby.Text = "";
        DDcriminalbackground.SelectedItem.Text = "OK";
        txtdesignation_verification.Text = "";
        txtdate_verification.Text = "";
        //************
        lblsignatureimage.ImageUrl = null;
        lblphotoimage.ImageUrl = null;
        TxtIdentificationMark.Text = "";
    }
    protected void btngetdetail_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (txteditcardno.Text != "")
        {
            refreshcontrol();
            try
            {
                SqlParameter[] arr = new SqlParameter[2];
                arr[0] = new SqlParameter("@empcode", txteditcardno.Text.Trim());
                arr[1] = new SqlParameter("@UserID", Session["varuserid"]);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETEMPLOYEEBIODATA", arr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hnempid.Value = ds.Tables[0].Rows[0]["empid"].ToString();
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["ResignStatus"]) == 1)
                    {
                        btnRemoveResign.Visible = true;
                    }
                    if (ddpostapplied.Items.FindByValue(ds.Tables[0].Rows[0]["Postappliedfor"].ToString()) != null)
                    {
                        ddpostapplied.SelectedValue = ds.Tables[0].Rows[0]["Postappliedfor"].ToString();
                    }
                    txtpaycode.Text = ds.Tables[0].Rows[0]["empcode"].ToString();
                    txtcardno.Text = ds.Tables[0].Rows[0]["empcode"].ToString();
                    txtname.Text = ds.Tables[0].Rows[0]["empname"].ToString();
                    txtfather_husbandname.Text = ds.Tables[0].Rows[0]["Fathername"].ToString();
                    txtmothername.Text = ds.Tables[0].Rows[0]["Mothername"].ToString();
                    txtdateofbirth.Text = ds.Tables[0].Rows[0]["Dob"].ToString();
                    txtage.Text = ds.Tables[0].Rows[0]["Age"].ToString();

                    switch (ds.Tables[0].Rows[0]["Gender"].ToString())
                    {
                        case "1":
                            rdmale.Checked = true;
                            break;
                        case "2":
                            rdfemale.Checked = true;
                            break;
                        default:
                            rdmale.Checked = false;
                            rdfemale.Checked = false;
                            break;
                    }
                    rdmarried.Checked = false;
                    rdunmarried.Checked = false;
                    switch (ds.Tables[0].Rows[0]["Maritalstatus"].ToString())
                    {
                        case "1":
                            rdmarried.Checked = true;
                            break;
                        case "2":
                            rdunmarried.Checked = true;
                            break;
                        default:
                            rdmarried.Checked = false;
                            rdunmarried.Checked = false;
                            break;
                    }
                    txtnationality.Text = ds.Tables[0].Rows[0]["Nationality"].ToString();
                    txtreligion.Text = ds.Tables[0].Rows[0]["Religion"].ToString();
                    txtcaster.Text = ds.Tables[0].Rows[0]["caste"].ToString();
                    txtregion.Text = ds.Tables[0].Rows[0]["Region"].ToString();
                    if (ddshiftoption.Items.FindByValue(ds.Tables[0].Rows[0]["shiftoptionid"].ToString()) != null)
                    {
                        ddshiftoption.SelectedValue = ds.Tables[0].Rows[0]["shiftoptionid"].ToString();
                    }
                    if (dddoctype.Items.FindByValue(ds.Tables[0].Rows[0]["Doctypeid"].ToString()) != null)
                    {
                        dddoctype.SelectedValue = ds.Tables[0].Rows[0]["Doctypeid"].ToString();
                    }
                    txtdocno.Text = ds.Tables[0].Rows[0]["Docno"].ToString();
                    if (dddesignation.Items.FindByValue(ds.Tables[0].Rows[0]["Designation"].ToString()) != null)
                    {
                        dddesignation.SelectedValue = ds.Tables[0].Rows[0]["Designation"].ToString();
                    }
                    if (dddepartment.Items.FindByValue(ds.Tables[0].Rows[0]["Departmentid"].ToString()) != null)
                    {
                        dddepartment.SelectedValue = ds.Tables[0].Rows[0]["Departmentid"].ToString();
                    }
                    if (ddsubdept.Items.FindByValue(ds.Tables[0].Rows[0]["subdept"].ToString()) != null)
                    {
                        ddsubdept.SelectedValue = ds.Tables[0].Rows[0]["subdept"].ToString();
                    }
                    if (dddivision.Items.FindByValue(ds.Tables[0].Rows[0]["Division"].ToString()) != null)
                    {
                        dddivision.SelectedValue = ds.Tables[0].Rows[0]["Division"].ToString();
                    }
                    if (ddcategory.Items.FindByValue(ds.Tables[0].Rows[0]["Categoryid"].ToString()) != null)
                    {
                        ddcategory.SelectedValue = ds.Tables[0].Rows[0]["Categoryid"].ToString();
                    }
                    if (ddcadre.Items.FindByValue(ds.Tables[0].Rows[0]["cadreid"].ToString()) != null)
                    {
                        ddcadre.SelectedValue = ds.Tables[0].Rows[0]["cadreid"].ToString();
                    }
                    if (ddcompany.Items.FindByValue(ds.Tables[0].Rows[0]["Companyid"].ToString()) != null)
                    {
                        ddcompany.SelectedValue = ds.Tables[0].Rows[0]["companyid"].ToString();
                    }
                    if (ddlocation.Items.FindByValue(ds.Tables[0].Rows[0]["Locationid"].ToString()) != null)
                    {
                        ddlocation.SelectedValue = ds.Tables[0].Rows[0]["Locationid"].ToString();
                    }
                    txtvill_present.Text = ds.Tables[0].Rows[0]["vill_present"].ToString();
                    txtvill_permanent.Text = ds.Tables[0].Rows[0]["vill_permanent"].ToString();
                    txtpo_present.Text = ds.Tables[0].Rows[0]["Po_present"].ToString();
                    txtpo_permanent.Text = ds.Tables[0].Rows[0]["Po_permanent"].ToString();
                    txtpolicestation_present.Text = ds.Tables[0].Rows[0]["Policestation_present"].ToString();
                    txtpolicestation_permanent.Text = ds.Tables[0].Rows[0]["Policestation_permanent"].ToString();
                    txtsubdivision_present.Text = ds.Tables[0].Rows[0]["Subdivision_present"].ToString();
                    txtsubdivision_permanent.Text = ds.Tables[0].Rows[0]["Subdivision_permanent"].ToString();
                    txtdistt_present.Text = ds.Tables[0].Rows[0]["Distt_present"].ToString();
                    txtdistt_permanent.Text = ds.Tables[0].Rows[0]["Distt_permanent"].ToString();
                    txtstate_present.Text = ds.Tables[0].Rows[0]["State_present"].ToString();
                    txtstate_permanent.Text = ds.Tables[0].Rows[0]["State_permanent"].ToString();
                    txtpincode_present.Text = ds.Tables[0].Rows[0]["Pincode_present"].ToString();
                    txtpincode_permanent.Text = ds.Tables[0].Rows[0]["Pincode_permanent"].ToString();
                    txtphno_present.Text = ds.Tables[0].Rows[0]["Phno_present"].ToString();
                    txtphno_permanent.Text = ds.Tables[0].Rows[0]["Phno_permanent"].ToString();

                    //********************************Qualification/Experience
                    txtqualification.Text = ds.Tables[0].Rows[0]["Qualification"].ToString();
                    txttechnicalqualification.Text = ds.Tables[0].Rows[0]["Technicalqualification"].ToString();
                    txtlanguageknown.Text = ds.Tables[0].Rows[0]["Languageknown"].ToString();
                    txttotalexperience.Text = ds.Tables[0].Rows[0]["Totalexperience"].ToString();
                    TxtIdentificationMark.Text = ds.Tables[0].Rows[0]["IdentificationMark"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ViewState["dgexperience"] = ds.Tables[1];
                        DGexperience.DataSource = ds.Tables[1];
                        DGexperience.DataBind();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            TextBox txtsrno = (TextBox)DGexperience.Rows[i].FindControl("txtsrno");
                            TextBox txtnameofemployer = (TextBox)DGexperience.Rows[i].FindControl("txtnameofemployer");
                            TextBox txtpostheld = (TextBox)DGexperience.Rows[i].FindControl("txtpostheld");
                            TextBox txtdoj = (TextBox)DGexperience.Rows[i].FindControl("txtdoj");
                            TextBox txtdol = (TextBox)DGexperience.Rows[i].FindControl("txtdol");
                            TextBox txtfromto = (TextBox)DGexperience.Rows[i].FindControl("txtfromto");
                            TextBox txtreasonforleaving = (TextBox)DGexperience.Rows[i].FindControl("txtreasonforleaving");

                            txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                            txtnameofemployer.Text = ds.Tables[1].Rows[i]["nameofemployer"].ToString();
                            txtpostheld.Text = ds.Tables[1].Rows[i]["Postheld"].ToString();
                            txtdoj.Text = ds.Tables[1].Rows[i]["doj"].ToString();
                            txtdol.Text = ds.Tables[1].Rows[i]["dol"].ToString();
                            txtfromto.Text = ds.Tables[1].Rows[i]["from_to"].ToString();
                            txtreasonforleaving.Text = ds.Tables[1].Rows[i]["Reasonforleaving"].ToString();
                        }

                    }
                    //********************************Family Members/Witness
                    switch (ds.Tables[0].Rows[0]["Relativeincompany"].ToString())
                    {
                        case "1":
                            rdrelativeworkyes.Checked = true;
                            break;
                        case "2":
                            rdrelativeworkno.Checked = true;
                            break;
                        default:
                            rdrelativeworkyes.Checked = false;
                            rdrelativeworkno.Checked = false;
                            break;
                    }
                    txtwitnessname_1.Text = ds.Tables[0].Rows[0]["Witnessname1"].ToString();
                    txtwitnessname_2.Text = ds.Tables[0].Rows[0]["Witnessname2"].ToString();
                    txtwitnessrelation_1.Text = ds.Tables[0].Rows[0]["Witnessrelationship1"].ToString();
                    txtwitnessrelationship_2.Text = ds.Tables[0].Rows[0]["Witnessrelationship2"].ToString();
                    txtwitnessaddress_1.Text = ds.Tables[0].Rows[0]["Witnessaddress1"].ToString();
                    txtwitnessaddress_2.Text = ds.Tables[0].Rows[0]["Witnessaddress2"].ToString();
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ViewState["dgfamilymembers"] = ds.Tables[2];
                        Dgfamilymembers.DataSource = ds.Tables[2];
                        Dgfamilymembers.DataBind();

                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {

                            TextBox txtsrno = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtsrno");
                            TextBox txtfamilymembername = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtfamilymembername");
                            TextBox txtyearofbirth = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtyearofbirth");
                            Label lblrelationid = (Label)Dgfamilymembers.Rows[i].FindControl("lblrelationid");
                            DropDownList ddrelation = (DropDownList)Dgfamilymembers.Rows[i].FindControl("ddrelation");
                            TextBox txtaddressnominee = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtaddressnominee");
                            CheckBox chksetasnominee = (CheckBox)Dgfamilymembers.Rows[i].FindControl("chksetasnominee");
                            TextBox txtshare = (TextBox)Dgfamilymembers.Rows[i].FindControl("txtshare");

                            txtsrno.Text = ds.Tables[2].Rows[i]["srno"].ToString();
                            txtfamilymembername.Text = ds.Tables[2].Rows[i]["Name"].ToString();
                            txtyearofbirth.Text = ds.Tables[2].Rows[i]["Yearofbirth"].ToString();
                            lblrelationid.Text = ds.Tables[2].Rows[i]["Relationid"].ToString();
                            txtaddressnominee.Text = ds.Tables[2].Rows[i]["address"].ToString();
                            // UtilityModule.ConditionalComboFill(ref ddrelation, "select RELATIONID,RELATION From HR_RELATIONMASTER order by DISPSEQNO,RELATION", true, "--Plz Select--");

                            if (ddrelation.Items.FindByValue(ds.Tables[2].Rows[i]["Relationid"].ToString()) != null)
                            {
                                ddrelation.SelectedValue = ds.Tables[2].Rows[i]["Relationid"].ToString();
                            }
                            chksetasnominee.Checked = Convert.ToBoolean(ds.Tables[2].Rows[i]["setasNominee"]);
                            txtshare.Text = ds.Tables[2].Rows[i]["share"].ToString();
                        }
                    }
                    //*********************************Joining / Salary Information
                    if (ddempgroup.Items.FindByValue(ds.Tables[0].Rows[0]["empgroupid"].ToString()) != null)
                    {
                        ddempgroup.SelectedValue = ds.Tables[0].Rows[0]["empgroupid"].ToString();
                    }
                    txtappointmentdate.Text = ds.Tables[0].Rows[0]["Appointmentdate"].ToString();
                    txtplace.Text = ds.Tables[0].Rows[0]["Place"].ToString();
                    txtinterviewdate.Text = ds.Tables[0].Rows[0]["Interviewdate"].ToString();
                    txtinterviewby.Text = ds.Tables[0].Rows[0]["Interviewby"].ToString();
                    txtdateofjoining.Text = ds.Tables[0].Rows[0]["Dateofjoining"].ToString();
                    txtconfirmdate.Text = ds.Tables[0].Rows[0]["Confirmdate"].ToString();
                    txtminimumwagesdate.Text = ds.Tables[0].Rows[0]["minimumwagesdate"].ToString();
                    txtresigndate.Text = ds.Tables[0].Rows[0]["resigndate"].ToString();
                    txtreasonremarks.Text = ds.Tables[0].Rows[0]["reasonremark"].ToString();
                    if (ddindividualdate.Items.FindByText(ds.Tables[0].Rows[0]["individualbiodata"].ToString()) != null)
                    {
                        ddindividualdate.SelectedItem.Text = ds.Tables[0].Rows[0]["individualbiodata"].ToString();
                    }
                    if (ddphoto.Items.FindByText(ds.Tables[0].Rows[0]["photo"].ToString()) != null)
                    {
                        ddphoto.SelectedItem.Text = ds.Tables[0].Rows[0]["photo"].ToString();
                    }
                    if (ddapplicationforemployment.Items.FindByText(ds.Tables[0].Rows[0]["applicationofemployment"].ToString()) != null)
                    {
                        ddapplicationforemployment.SelectedItem.Text = ds.Tables[0].Rows[0]["applicationofemployment"].ToString();
                    }
                    if (ddproofofage.Items.FindByText(ds.Tables[0].Rows[0]["Proofofage"].ToString()) != null)
                    {
                        ddproofofage.SelectedItem.Text = ds.Tables[0].Rows[0]["Proofofage"].ToString();
                    }
                    txtproofname.Text = ds.Tables[0].Rows[0]["Proofname"].ToString();

                    if (ddcertificates_testimonials.Items.FindByText(ds.Tables[0].Rows[0]["certificatetestimonials"].ToString()) != null)
                    {
                        ddcertificates_testimonials.SelectedItem.Text = ds.Tables[0].Rows[0]["certificatetestimonials"].ToString();
                    }
                    if (ddcontractofemployment.Items.FindByText(ds.Tables[0].Rows[0]["contractofemployment"].ToString()) != null)
                    {
                        ddcontractofemployment.SelectedItem.Text = ds.Tables[0].Rows[0]["contractofemployment"].ToString();
                    }
                    if (ddjoiningreport.Items.FindByText(ds.Tables[0].Rows[0]["Joiningreport"].ToString()) != null)
                    {
                        ddjoiningreport.SelectedItem.Text = ds.Tables[0].Rows[0]["Joiningreport"].ToString();
                    }
                    if (ddnominationform.Items.FindByText(ds.Tables[0].Rows[0]["Nominationform"].ToString()) != null)
                    {
                        ddnominationform.SelectedItem.Text = ds.Tables[0].Rows[0]["Nominationform"].ToString();
                    }
                    switch (ds.Tables[0].Rows[0]["EmployeeType"].ToString())
                    {
                        case "1":
                            rdpermanent.Checked = true;
                            break;
                        case "2":
                            rdcasual.Checked = true;
                            break;
                        default:
                            rdpermanent.Checked = false;
                            rdcasual.Checked = false;
                            break;
                    }
                    switch (ds.Tables[0].Rows[0]["Wagescalculation"].ToString())
                    {
                        case "1":
                            rdmonthly.Checked = true;
                            break;
                        case "2":
                            rddaily.Checked = true;
                            break;
                        case "3":
                            rdpcswise.Checked = true;
                            break;
                        default:
                            rdmonthly.Checked = false;
                            rddaily.Checked = false;
                            rdpcswise.Checked = false;
                            break;
                    }
                    switch (ds.Tables[0].Rows[0]["PaymentType"].ToString())
                    {
                        case "1":
                            rdcash.Checked = true;
                            break;
                        case "2":
                            rdcheque.Checked = true;
                            break;
                        case "3":
                            rdbank.Checked = true;
                            break;
                        default:
                            rdcash.Checked = false;
                            rdcheque.Checked = false;
                            rdbank.Checked = false;
                            break;
                    }
                    switch (ds.Tables[0].Rows[0]["Overtime"].ToString())
                    {
                        case "1":
                            rdenableovertime.Checked = true;
                            break;
                        case "2":
                            rddisableovertime.Checked = true;
                            break;
                        default:
                            rdenableovertime.Checked = false;
                            rddisableovertime.Checked = false;
                            break;
                    }
                    switch (ds.Tables[0].Rows[0]["Fooding"].ToString())
                    {
                        case "1":
                            rdenablefooding.Checked = true;
                            break;
                        case "2":
                            rddisbalefooding.Checked = true;
                            break;
                        default:
                            rdenablefooding.Checked = false;
                            rddisbalefooding.Checked = false;
                            break;
                    }
                    switch (ds.Tables[0].Rows[0]["Sun_Hdpay"].ToString())
                    {
                        case "1":
                            rdenablesunhdpay.Checked = true;
                            break;
                        case "2":
                            rddisablesunhdpay.Checked = true;
                            break;
                        default:
                            rdenablesunhdpay.Checked = false;
                            rddisablesunhdpay.Checked = false;
                            break;
                    }
                    txtbasic.Text = ds.Tables[0].Rows[0]["Minimumwagesbasic"].ToString();
                    txtda.Text = ds.Tables[0].Rows[0]["Da"].ToString();
                    txtbasicpay.Text = ds.Tables[0].Rows[0]["Basicpay"].ToString();
                    txtgrosssal.Text = ds.Tables[0].Rows[0]["Grosssalary"].ToString();
                    txtnetsal.Text = ds.Tables[0].Rows[0]["NetSalary"].ToString();

                    if (DDpayrolltype.Items.FindByValue(ds.Tables[0].Rows[0]["PayrollTypeid"].ToString()) != null)
                    {
                        DDpayrolltype.SelectedValue = ds.Tables[0].Rows[0]["PayrollTypeid"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["Signatureimage"].ToString() != "")
                    {
                        lblsignatureimage.ImageUrl = ds.Tables[0].Rows[0]["SignatureImage"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                    }
                    if (ds.Tables[0].Rows[0]["EMpphoto"].ToString() != "")
                    {
                        lblphotoimage.ImageUrl = ds.Tables[0].Rows[0]["EMpphoto"].ToString() + "?" + DateTime.Now.Ticks.ToString();
                    }

                    //**************************SALARY
                    Fill_allowances_DeductionsEdit(ds);

                    GetGross_Netsalary();
                    //*****************************Previous Employment
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        txtname_verifiedby.Text = ds.Tables[5].Rows[0]["NAME_COMPANYEMP"].ToString();
                        txtmobileno_verifiedby.Text = ds.Tables[5].Rows[0]["MOBILENO_COMPANYEMP"].ToString();
                        txtname1_verification.Text = ds.Tables[5].Rows[0]["NAME1_REFERENCE"].ToString();
                        txtmobileno1_verification.Text = ds.Tables[5].Rows[0]["MOBILENO1_REFERENCE"].ToString();
                        txtname2_verification.Text = ds.Tables[5].Rows[0]["NAME2_REFERENCE"].ToString();
                        txtmobileno2_verification.Text = ds.Tables[5].Rows[0]["MOBILENO2_REFERENCE"].ToString();
                        txtneighbourname.Text = ds.Tables[5].Rows[0]["NEIGHBOURNAME"].ToString();
                        txtmobileno_neighbour.Text = ds.Tables[5].Rows[0]["MOBILENO_NEIGHBOUR"].ToString();
                        txtgraampradhan_memeber.Text = ds.Tables[5].Rows[0]["GRAAMPRADHANMEMBER"].ToString();
                        txtverificationdoneby.Text = ds.Tables[5].Rows[0]["VERIFICATIONDONEBY"].ToString();
                        //
                        if (DDcriminalbackground.Items.FindByText(ds.Tables[5].Rows[0]["CRIMINALBACKGROUND"].ToString()) != null)
                        {
                            DDcriminalbackground.SelectedItem.Text = ds.Tables[5].Rows[0]["CRIMINALBACKGROUND"].ToString();
                        }
                        txtdesignation_verification.Text = ds.Tables[5].Rows[0]["DESIGNATION_VERIFICATIONDONEBY"].ToString();
                        txtdate_verification.Text = ds.Tables[5].Rows[0]["DATE_VERIFICATION"].ToString();
                    }

                    //***************Statutory
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        switch (ds.Tables[4].Rows[0]["Esi"].ToString())
                        {
                            case "1":
                                rdesiyes.Checked = true;
                                break;
                            case "2":
                                rdesino.Checked = true;
                                break;
                            default:
                                rdesiyes.Checked = false;
                                rdesino.Checked = false;
                                break;
                        }
                        txtesiinsuranceno.Text = ds.Tables[4].Rows[0]["Esi_Insuranceno"].ToString();
                        txtesiemployercode.Text = ds.Tables[4].Rows[0]["Esi_Employercode"].ToString();
                        txtesidispensary.Text = ds.Tables[4].Rows[0]["Esi_dispensary"].ToString();
                        txtesilocaloffice.Text = ds.Tables[4].Rows[0]["Esi_Localoffice"].ToString();
                        txtesinomineeforpayment.Text = ds.Tables[4].Rows[0]["Esi_Nomineeforpayment"].ToString();
                        txtesiparticularsoffamily.Text = ds.Tables[4].Rows[0]["Esi_Particularsoffamily"].ToString();
                        txtesifamilymemberresidingwithinsuredperson.Text = ds.Tables[4].Rows[0]["Esi_familymemberresidingWithinsuredperson"].ToString();

                        switch (ds.Tables[4].Rows[0]["pf"].ToString())
                        {
                            case "1":
                                rdpfyes.Checked = true;
                                break;
                            case "2":
                                rdpfno.Checked = true;
                                break;
                            default:
                                rdpfyes.Checked = false;
                                rdpfno.Checked = false;
                                break;
                        }
                        txtpfaccountno.Text = ds.Tables[4].Rows[0]["pf_accountno"].ToString();
                        txtpfnominee_nominees.Text = ds.Tables[4].Rows[0]["pf_Nominee"].ToString();
                        txtpfsharepercentageofnominee.Text = ds.Tables[4].Rows[0]["pf_shareofNominee"].ToString();
                        if (ddchildrenpension.Items.FindByText(ds.Tables[4].Rows[0]["pf_childpension"].ToString()) != null)
                        {
                            ddchildrenpension.SelectedItem.Text = ds.Tables[4].Rows[0]["pf_childpension"].ToString();
                        }
                        if (ddwidowpension.Items.FindByText(ds.Tables[4].Rows[0]["pf_Widowpension"].ToString()) != null)
                        {
                            ddwidowpension.SelectedItem.Text = ds.Tables[4].Rows[0]["pf_Widowpension"].ToString();
                        }
                        txtpfuanno.Text = ds.Tables[4].Rows[0]["pf_UanNo"].ToString();
                        txtifsccode.Text = ds.Tables[4].Rows[0]["Ifsccode"].ToString();
                        txtbankname.Text = ds.Tables[4].Rows[0]["Bankname"].ToString();
                        txtbranch.Text = ds.Tables[4].Rows[0]["Branch"].ToString();
                        txtbankaddress.Text = ds.Tables[4].Rows[0]["Bankaddress"].ToString();
                        txtacno.Text = ds.Tables[4].Rows[0]["BankAcno"].ToString();
                        txtnomineeforgratuity.Text = ds.Tables[4].Rows[0]["Nomineesforgratuity"].ToString();
                        txtgratuitysharepercentageofnominee.Text = ds.Tables[4].Rows[0]["GrauityshareofNominee"].ToString();
                        txtaccountholdername.Text = ds.Tables[4].Rows[0]["accountholdername"].ToString();
                    }
                }

                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "getdata", "alert('No records fetched !!!')", true);
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }
    }
    protected void txtdateofjoining_TextChanged(object sender, EventArgs e)
    {
        txtconfirmdate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select convert(nvarchar(11),DATEADD(dd,-1,DATEADD (MM,6,convert(date,CONVERT(varchar(11),'" + txtdateofjoining.Text + "',103),104))),103)").ToString();
    }
    protected void btnrefreshgrouponBiodata_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddempgroup, "select GroupId,GroupName From HR_GroupMaster order by GroupName", true, "--Plz Select--");
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
    protected void btnRemoveResign_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string Str = @"Update HR_EmployeeInformation Set RESIGNDATE = null, RESIGNSTATUS = 0 Where EmpID = " + hnempid.Value + @" 
                Update Empinfo Set Blacklist = 0 Where EmpID = " + hnempid.Value;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Employee Resign Successfully Remove !!!')", true);

        refreshcontrol();
    }
}