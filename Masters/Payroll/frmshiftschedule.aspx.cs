using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmshiftschedule : System.Web.UI.Page
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
                SetInitialRowGrid();
            }
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            txtfromdate.Text = startDate.ToString("dd/MM/yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

            string str = @"Select Distinct D.DepartmentId, D.DepartmentName 
                        From Department D(Nolock)
                        JOIN DepartmentBranch DB(Nolock) ON DB.DepartmentID = D.DepartmentId 
                        JOIN BranchUser BU(Nolock) ON BU.BranchID = DB.BranchID And BU.UserID = " + Session["varuserId"] + @" 
                        Where IsNull(ShowOrNotInHR, 0) = 1 And D.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                        Order By D.DepartmentName 
                        Select ID, BranchName 
                        From BRANCHMASTER BM(nolock) 
                        JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                        Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDdepartment, ds, 0, true, "--Select--");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
        }
    }
    private void SetInitialRowGrid()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Date", typeof(string)));
        dt.Columns.Add(new DataColumn("shiftid", typeof(int)));

        dr = dt.NewRow();
        dr["Date"] = string.Empty;
        dr["shiftid"] = 0;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["Dgdetail"] = dt;

        Dgdetail.DataSource = dt;
        Dgdetail.DataBind();
    }

    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        fillemployee();
    }
    protected void fillemployee()
    {
        try
        {
            string str = @"SELECT EI.EMPID,EI.EMPCODE + ' - ' + EI.EMPNAME EMPNAME 
                    FROM EMPINFO EI With(nolock) 
                    JOIN HR_EMPLOYEEINFORMATION HEI With(nolock) ON EI.EMPID=HEI.EMPID AND HEI.RESIGNSTATUS = 0 
                    Where EI.EMPCODE = '" + txtempcode.Text + "' And EI.BranchID = " + DDBranchName.SelectedValue;

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (listWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                {
                    listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Emp. Code does not exists in ERP or Status is Resigned !!!')", true);
            }
            txtempcode.Text = "";
            txtempcode.Focus();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        listWeaverName.Items.Remove(listWeaverName.SelectedItem);
    }
    protected void Dgdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddshift = (DropDownList)e.Row.FindControl("ddshift");
            UtilityModule.ConditionalComboFill(ref ddshift, "select ShiftId,SHIFTCODE +' - '+INTIME+'-'+OUTTIME From HR_ShiftMaster order by ShiftId", true, "--Plz Select--");
            Label lblshiftid = (Label)e.Row.FindControl("lblshiftid");
            if (ddshift.Items.FindByValue(lblshiftid.Text) != null)
            {
                ddshift.SelectedValue = lblshiftid.Text;
            }

        }
    }
    protected void btnaddnewrow_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["Dgdetail"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Dgdetail"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values                    
                    TextBox txtdate = (TextBox)Dgdetail.Rows[rowIndex].FindControl("txtdate");
                    Label lblshiftid = (Label)Dgdetail.Rows[rowIndex].FindControl("lblshiftid");
                    DropDownList ddshift = (DropDownList)Dgdetail.Rows[rowIndex].FindControl("ddshift");


                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["date"] = txtdate.Text;
                    dtCurrentTable.Rows[i - 1]["shiftid"] = ddshift.SelectedIndex > 0 ? ddshift.SelectedValue : "0";

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["Dgdetail"] = dtCurrentTable;

                Dgdetail.DataSource = dtCurrentTable;
                Dgdetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousDatagrid();
    }
    private void SetPreviousDatagrid()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["Dgdetail"] != null)
        {
            DataTable dt = (DataTable)ViewState["Dgdetail"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtdate = (TextBox)Dgdetail.Rows[rowIndex].FindControl("txtdate");
                    Label lblshiftid = (Label)Dgdetail.Rows[rowIndex].FindControl("lblshiftid");
                    DropDownList ddshift = (DropDownList)Dgdetail.Rows[rowIndex].FindControl("ddshift");


                    txtdate.Text = dt.Rows[i]["date"].ToString();
                    if (ddshift.Items.FindByValue(dt.Rows[i]["shiftid"].ToString()) != null)
                    {
                        ddshift.SelectedValue = dt.Rows[i]["shiftid"].ToString();
                    }
                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);
                    rowIndex++;
                }

                //InsertRecords(sc);
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //***********Employee Data
        DataTable dtemp = new DataTable();
        dtemp.Columns.Add("empid", typeof(int));
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            DataRow dr = dtemp.NewRow();
            dr["empid"] = listWeaverName.Items[i].Value;
            dtemp.Rows.Add(dr);
        }
        //***********

        //**********EMployee Data
        DataTable dtempdata = new DataTable();
        dtempdata.Columns.Add("shiftdate", typeof(string));
        dtempdata.Columns.Add("shiftid", typeof(int));
        for (int i = 0; i < Dgdetail.Rows.Count; i++)
        {
            DataRow dr = dtempdata.NewRow();
            TextBox txtdate = (TextBox)Dgdetail.Rows[i].FindControl("txtdate");
            DropDownList ddshift = (DropDownList)Dgdetail.Rows[i].FindControl("ddshift");
            if (txtdate.Text != "" && ddshift.SelectedIndex > 0)
            {
                dr["shiftdate"] = txtdate.Text;
                dr["shiftid"] = ddshift.SelectedValue;

                dtempdata.Rows.Add(dr);
            }
        }
        //*************End
        if (dtemp.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Please Fill atleast One Employee Code !!!')", true);
            return;
        }
        if (dtempdata.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altempdata", "alert('Please Fill Data in Grid to Save Data !!!')", true);
            return;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVESHIFTSCHEDULE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@DTEMP", dtemp);
            cmd.Parameters.AddWithValue("@DTEMPDATA", dtempdata);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            Tran.Commit();
            lblmsg.Text = cmd.Parameters["@msg"].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value + "')", true);
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
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //***********Employee Data
        DataTable dtemp = new DataTable();
        dtemp.Columns.Add("empid", typeof(int));
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            DataRow dr = dtemp.NewRow();
            dr["empid"] = listWeaverName.Items[i].Value;
            dtemp.Rows.Add(dr);
        }
        //**********
        if (dtemp.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Please Fill atleast One Employee Code !!!')", true);
            return;
        }

        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Dtemp", dtemp);
            param[1] = new SqlParameter("@Fromdate", txtfromdate.Text);
            param[2] = new SqlParameter("@Todate", txttodate.Text);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETSHIFTINSERTEDDATA", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Dgdetail"] = ds.Tables[0];
                Dgdetail.DataSource = ds.Tables[0];
                Dgdetail.DataBind();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TextBox txtdate = (TextBox)Dgdetail.Rows[i].FindControl("txtdate");
                    Label lblshiftid = (Label)Dgdetail.Rows[i].FindControl("lblshiftid");
                    DropDownList ddshift = (DropDownList)Dgdetail.Rows[i].FindControl("ddshift");

                    txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToString("dd/MM/yyyy");
                    lblshiftid.Text = ds.Tables[0].Rows[i]["shiftid"].ToString();

                    if (ddshift.Items.FindByValue(ds.Tables[0].Rows[i]["shiftid"].ToString()) != null)
                    {
                        ddshift.SelectedValue = ds.Tables[0].Rows[i]["shiftid"].ToString();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altgetdata", "alert('No records found in this range !!!')", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void DDdepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select ei.EmpId,ei.EmpCode,ei.EmpName 
            From EmpInfo ei(Nolock) 
            join Department d(Nolock) on ei.Departmentid=d.DepartmentId And D.departmentid=" + DDdepartment.SelectedValue + @" 
            Where EI.BlackList = 0 And ei.BranchID = " + DDBranchName.SelectedValue;

        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            str = str + " And ei.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
        }
        str = str + " order by ei.EmpCode";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        Dgemp.DataSource = ds.Tables[0];
        Dgemp.DataBind();
    }
    protected void btnset_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            if (Dgemp.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altassign", "alert('Please select atleast one Emp code in Data Grid !!!')", true);
                return;
            }
            string empname = "";
            for (int i = 0; i < Dgemp.Rows.Count; i++)
            {
                CheckBox chkitem = (CheckBox)Dgemp.Rows[i].FindControl("chkitem");
                Label lblempid = (Label)Dgemp.Rows[i].FindControl("lblempid");
                Label lblempcode = (Label)Dgemp.Rows[i].FindControl("lblempcode");
                Label lblempname = (Label)Dgemp.Rows[i].FindControl("lblempname");
                empname = lblempcode.Text + " - " + lblempname.Text;
                if (chkitem.Checked == true)
                {
                    if (listWeaverName.Items.FindByValue(lblempid.Text) == null)
                    {
                        listWeaverName.Items.Add(new ListItem(empname, lblempid.Text));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnassignbulk_Click(object sender, EventArgs e)
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
            SqlCommand cmd = new SqlCommand("PRO_HR_SAVESHIFTALL", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Fromdate", txtfromdate.Text);
            cmd.Parameters.AddWithValue("@Todate", txttodate.Text);
            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);

            cmd.ExecuteNonQuery();
            Tran.Commit();
            lblmsg.Text = cmd.Parameters["@Msg"].Value.ToString();
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
