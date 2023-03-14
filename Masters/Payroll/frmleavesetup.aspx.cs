using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Payroll_frmleavesetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (!Page.IsPostBack)
            {
                SetInitialRowGrid();
            }
            string str = @"SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE With (nolock) ORDER BY MONTH_ID
                          SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA With (nolock)
                          select GroupId,GroupName From HR_GroupMaster order by GroupName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDfrommonth, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDfromyear, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDtomonth, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDtoyear, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDempgroup, ds, 2, true, "--Plz Select--");

            if (DDfromyear.Items.Count > 0)
            {
                if (DDfromyear.Items.FindByValue(System.DateTime.Now.Year.ToString()) != null)
                {
                    DDfromyear.SelectedValue = System.DateTime.Now.Year.ToString();
                }
            }
            if (DDtomonth.Items.FindByValue("12") != null)
            {
                DDtomonth.SelectedValue = "12";
            }
            if (DDtoyear.Items.FindByValue(System.DateTime.Now.Year.ToString()) != null)
            {
                DDtoyear.SelectedValue = System.DateTime.Now.Year.ToString();
            }
        }
    }

    private void SetInitialRowGrid()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("typeofleave", typeof(int)));
        dt.Columns.Add(new DataColumn("maxleaveallot", typeof(float)));
        dt.Columns.Add(new DataColumn("daysworkedinmonth", typeof(float)));
        dt.Columns.Add(new DataColumn("Durationofleaveearned", typeof(float)));
        dt.Columns.Add(new DataColumn("leaveid", typeof(int)));
        dt.Columns.Add(new DataColumn("maxleaveallotid", typeof(int)));
        dt.Columns.Add(new DataColumn("balleaveid", typeof(int)));
        dt.Columns.Add(new DataColumn("leavegenderid", typeof(int)));

        dr = dt.NewRow();
        dr["typeofleave"] = 1;
        dr["maxleaveallot"] = DBNull.Value;
        dr["daysworkedinmonth"] = DBNull.Value;
        dr["Durationofleaveearned"] = DBNull.Value;
        dr["leaveid"] = 0;
        dr["maxleaveallotid"] = 1;
        dr["balleaveid"] = 1;
        dr["leavegenderid"] = 1;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["dgdetail"] = dt;

        dgdetail.DataSource = dt;
        dgdetail.DataBind();
    }
    protected void btnaddnewrow_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["dgdetail"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["dgdetail"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values   
                    Label lbltypeofleave = (Label)dgdetail.Rows[rowIndex].FindControl("lbltypeofleave");
                    RadioButton rdfixed = (RadioButton)dgdetail.Rows[rowIndex].FindControl("rdfixed");
                    RadioButton rdcalculate = (RadioButton)dgdetail.Rows[rowIndex].FindControl("rdcalculate");
                    DropDownList ddleavename = (DropDownList)dgdetail.Rows[rowIndex].FindControl("ddleavename");
                    TextBox txtmaxleaveallot = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtmaxleaveallot");
                    DropDownList ddmaxleavetype = (DropDownList)dgdetail.Rows[rowIndex].FindControl("ddmaxleavetype");
                    TextBox txtdaysworkedinmonth = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtdaysworkedinmonth");
                    TextBox txtdurationofleaveearned = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtdurationofleaveearned");
                    DropDownList DDballeave = (DropDownList)dgdetail.Rows[rowIndex].FindControl("DDballeave");
                    DropDownList DDleavegender = (DropDownList)dgdetail.Rows[rowIndex].FindControl("DDleavegender");

                    drCurrentRow = dtCurrentTable.NewRow();

                    //dr["maxleaveallot"] = DBNull.Value;
                    //dr["daysworkedinmonth"] = DBNull.Value;
                    //dr["Durationofleaveearned"] = DBNull.Value;
                    //dr["leaveid"] = 0;
                    //dr["maxleaveallotid"] = 1;
                    //dr["balleaveid"] = 1;
                    //dr["leavegenderid"] = 1;
                    drCurrentRow["Typeofleave"] = 1;
                    drCurrentRow["leaveid"] = 0;
                    drCurrentRow["maxleaveallotid"] = 1;
                    drCurrentRow["balleaveid"] = 1;
                    drCurrentRow["leavegenderid"] = 1;

                    dtCurrentTable.Rows[i - 1]["Typeofleave"] = rdfixed.Checked == true ? "1" : "2";
                    dtCurrentTable.Rows[i - 1]["maxleaveallot"] = txtmaxleaveallot.Text == "" ? DBNull.Value : (object)txtmaxleaveallot.Text;
                    dtCurrentTable.Rows[i - 1]["daysworkedinmonth"] = txtdaysworkedinmonth.Text == "" ? DBNull.Value : (object)txtdaysworkedinmonth.Text;
                    dtCurrentTable.Rows[i - 1]["Durationofleaveearned"] = txtdurationofleaveearned.Text == "" ? DBNull.Value : (object)txtdurationofleaveearned.Text;
                    dtCurrentTable.Rows[i - 1]["leaveid"] = ddleavename.SelectedIndex > 0 ? ddleavename.SelectedValue : "0";
                    dtCurrentTable.Rows[i - 1]["maxleaveallotid"] = ddmaxleavetype.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["balleaveid"] = DDballeave.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["leavegenderid"] = DDleavegender.SelectedValue;



                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["dgdetail"] = dtCurrentTable;

                dgdetail.DataSource = dtCurrentTable;
                dgdetail.DataBind();
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
        if (ViewState["dgdetail"] != null)
        {
            DataTable dt = (DataTable)ViewState["dgdetail"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lbltypeofleave = (Label)dgdetail.Rows[rowIndex].FindControl("lbltypeofleave");
                    RadioButton rdfixed = (RadioButton)dgdetail.Rows[rowIndex].FindControl("rdfixed");
                    RadioButton rdcalculate = (RadioButton)dgdetail.Rows[rowIndex].FindControl("rdcalculate");
                    DropDownList ddleavename = (DropDownList)dgdetail.Rows[rowIndex].FindControl("ddleavename");
                    TextBox txtmaxleaveallot = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtmaxleaveallot");
                    DropDownList ddmaxleavetype = (DropDownList)dgdetail.Rows[rowIndex].FindControl("ddmaxleavetype");
                    TextBox txtdaysworkedinmonth = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtdaysworkedinmonth");
                    TextBox txtdurationofleaveearned = (TextBox)dgdetail.Rows[rowIndex].FindControl("txtdurationofleaveearned");
                    DropDownList DDballeave = (DropDownList)dgdetail.Rows[rowIndex].FindControl("DDballeave");
                    DropDownList DDleavegender = (DropDownList)dgdetail.Rows[rowIndex].FindControl("DDleavegender");


                    if (ddleavename.Items.FindByValue(dt.Rows[i]["leaveid"].ToString()) != null)
                    {
                        ddleavename.SelectedValue = dt.Rows[i]["leaveid"].ToString();
                    }
                    rdfixed.Checked = false;
                    rdcalculate.Checked = false;
                    switch (dt.Rows[i]["typeofleave"].ToString())
                    {
                        case "1":
                            rdfixed.Checked = true;
                            break;
                        case "2":
                            rdcalculate.Checked = true;
                            break;
                        default:
                            break;
                    }
                    //dr["maxleaveallot"] = DBNull.Value;
                    //dr["daysworkedinmonth"] = DBNull.Value;
                    //dr["Durationofleaveearned"] = DBNull.Value;
                    //dr["leaveid"] = 0;
                    //dr["maxleaveallotid"] = 1;
                    //dr["balleaveid"] = 1;
                    //dr["leavegenderid"] = 1;
                    txtmaxleaveallot.Text = dt.Rows[i]["maxleaveallot"].ToString();
                    if (ddmaxleavetype.Items.FindByValue(dt.Rows[i]["maxleaveallotid"].ToString()) != null)
                    {
                        ddmaxleavetype.SelectedValue = dt.Rows[i]["maxleaveallotid"].ToString();
                    }
                    txtdaysworkedinmonth.Text = dt.Rows[i]["daysworkedinmonth"].ToString();
                    txtdurationofleaveearned.Text = dt.Rows[i]["Durationofleaveearned"].ToString();
                    if (DDballeave.Items.FindByValue(dt.Rows[i]["balleaveid"].ToString()) != null)
                    {
                        DDballeave.SelectedValue = dt.Rows[i]["balleaveid"].ToString();
                    }
                    if (DDleavegender.Items.FindByValue(dt.Rows[i]["leavegenderid"].ToString()) != null)
                    {
                        DDleavegender.SelectedValue = dt.Rows[i]["leavegenderid"].ToString();
                    }

                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);
                    rowIndex++;
                }

                //InsertRecords(sc);
            }
        }
    }
    protected void dgdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddleavename = (DropDownList)e.Row.FindControl("ddleavename");
            DropDownList ddmaxleavetype = (DropDownList)e.Row.FindControl("ddmaxleavetype");
            DropDownList DDballeave = (DropDownList)e.Row.FindControl("DDballeave");
            DropDownList DDleavegender = (DropDownList)e.Row.FindControl("DDleavegender");

            UtilityModule.ConditionalComboFill(ref ddleavename, "select Leaveid,Name+'  '+Code From Hr_LeaveType", true, "--Plz Select--");
            Label lblleaveid = (Label)e.Row.FindControl("lblleaveid");
            Label lblmaxleaveallotid = (Label)e.Row.FindControl("lblmaxleaveallotid");
            Label lblballeaveid = (Label)e.Row.FindControl("lblballeaveid");
            Label lblleavegenderid = (Label)e.Row.FindControl("lblleavegenderid");

            if (ddleavename.Items.FindByValue(lblleaveid.Text) != null)
            {
                ddleavename.SelectedValue = lblleaveid.Text;
            }
            if (ddmaxleavetype.Items.FindByValue(lblballeaveid.Text) != null)
            {
                ddmaxleavetype.SelectedValue = lblballeaveid.Text;
            }

            if (DDballeave.Items.FindByValue(lblballeaveid.Text) != null)
            {
                DDballeave.SelectedValue = lblballeaveid.Text;
            }

            if (DDleavegender.Items.FindByValue(lblleavegenderid.Text) != null)
            {
                DDleavegender.SelectedValue = lblleavegenderid.Text;
            }


        }
    }
    protected void btnrefreshgrouponleavetype_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDempgroup, "select GroupId,GroupName From HR_GroupMaster order by GroupName", true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        //*********************
        DataTable dt = new DataTable();
        dt.Columns.Add("LEAVEID", typeof(int));
        dt.Columns.Add("TYPEOFLEAVE", typeof(int));
        dt.Columns.Add("MAXLEAVEALLOTED", typeof(float));
        dt.Columns.Add("MAXLEAVEALLOTID", typeof(int));
        dt.Columns.Add("DAYSWORKEDINMONTH", typeof(float));
        dt.Columns.Add("DURATIONOFLEAVEEARNED", typeof(float));
        dt.Columns.Add("BALLEAVEID", typeof(int));
        dt.Columns.Add("LEAVEGENDERID", typeof(int));
        for (int i = 0; i < dgdetail.Rows.Count; i++)
        {

            DropDownList ddleavename = (DropDownList)dgdetail.Rows[i].FindControl("ddleavename");


            if (ddleavename.SelectedIndex > 0)
            {
                RadioButton rdfixed = (RadioButton)dgdetail.Rows[i].FindControl("rdfixed");
                RadioButton rdcalculate = (RadioButton)dgdetail.Rows[i].FindControl("rdcalculate");
                TextBox txtmaxleaveallot = (TextBox)dgdetail.Rows[i].FindControl("txtmaxleaveallot");
                DropDownList ddmaxleavetype = (DropDownList)dgdetail.Rows[i].FindControl("ddmaxleavetype");
                TextBox txtdaysworkedinmonth = (TextBox)dgdetail.Rows[i].FindControl("txtdaysworkedinmonth");
                TextBox txtdurationofleaveearned = (TextBox)dgdetail.Rows[i].FindControl("txtdurationofleaveearned");
                DropDownList DDballeave = (DropDownList)dgdetail.Rows[i].FindControl("DDballeave");
                DropDownList DDleavegender = (DropDownList)dgdetail.Rows[i].FindControl("DDleavegender");

                DataRow dr = dt.NewRow();
                dr["LEAVEID"] = ddleavename.SelectedValue;
                dr["TYPEOFLEAVE"] = rdfixed.Checked == true ? 1 : 2;
                dr["MAXLEAVEALLOTED"] = txtmaxleaveallot.Text == "" ? "0" : txtmaxleaveallot.Text;
                dr["MAXLEAVEALLOTID"] = ddmaxleavetype.SelectedValue;
                dr["DAYSWORKEDINMONTH"] = txtdaysworkedinmonth.Text == "" ? "0" : txtdaysworkedinmonth.Text;
                dr["DURATIONOFLEAVEEARNED"] = txtdurationofleaveearned.Text == "" ? "0" : txtdurationofleaveearned.Text;
                dr["BALLEAVEID"] = DDballeave.SelectedValue;
                dr["LEAVEGENDERID"] = DDleavegender.SelectedValue;
                dt.Rows.Add(dr);
            }

        }
        if (dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please Fill atleast one row in Data Grid !!!')", true);
            return;
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
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@id", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hnid.Value;
            param[1] = new SqlParameter("@Frommonthid", DDfrommonth.SelectedValue);
            param[2] = new SqlParameter("@ToMonthid", DDtomonth.SelectedValue);
            param[3] = new SqlParameter("@Fromyearid", DDfromyear.SelectedValue);
            param[4] = new SqlParameter("@ToYearid", DDtoyear.SelectedValue);
            param[5] = new SqlParameter("@empgroupid", DDempgroup.SelectedValue);
            param[6] = new SqlParameter("@userid", Session["varuserid"]);
            param[7] = new SqlParameter("@dt", dt);
            param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_HR_SAVELEAVESETUP", param);
            Tran.Commit();
            lblmsg.Text = param[8].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsavep", "alert('" + param[8].Value.ToString() + "')", true);

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
    protected void DDempgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@FromMonthid", DDfrommonth.SelectedValue);
            param[1] = new SqlParameter("@ToMonthid", DDtomonth.SelectedValue);
            param[2] = new SqlParameter("@FromYearid", DDfromyear.SelectedValue);
            param[3] = new SqlParameter("@ToYearid", DDtoyear.SelectedValue);
            param[4] = new SqlParameter("@Empgroupid", DDempgroup.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HR_GETLEAVESETUP", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnid.Value = ds.Tables[0].Rows[0]["id"].ToString();
                ViewState["dgdetail"] = ds.Tables[0];
                dgdetail.DataSource = ds.Tables[0];
                dgdetail.DataBind();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Label lbltypeofleave = (Label)dgdetail.Rows[i].FindControl("lbltypeofleave");
                    RadioButton rdfixed = (RadioButton)dgdetail.Rows[i].FindControl("rdfixed");
                    RadioButton rdcalculate = (RadioButton)dgdetail.Rows[i].FindControl("rdcalculate");
                    DropDownList ddleavename = (DropDownList)dgdetail.Rows[i].FindControl("ddleavename");
                    TextBox txtmaxleaveallot = (TextBox)dgdetail.Rows[i].FindControl("txtmaxleaveallot");
                    DropDownList ddmaxleavetype = (DropDownList)dgdetail.Rows[i].FindControl("ddmaxleavetype");
                    TextBox txtdaysworkedinmonth = (TextBox)dgdetail.Rows[i].FindControl("txtdaysworkedinmonth");
                    TextBox txtdurationofleaveearned = (TextBox)dgdetail.Rows[i].FindControl("txtdurationofleaveearned");
                    DropDownList DDballeave = (DropDownList)dgdetail.Rows[i].FindControl("DDballeave");
                    DropDownList DDleavegender = (DropDownList)dgdetail.Rows[i].FindControl("DDleavegender");


                    if (ddleavename.Items.FindByValue(ds.Tables[0].Rows[i]["leaveid"].ToString()) != null)
                    {
                        ddleavename.SelectedValue = ds.Tables[0].Rows[i]["leaveid"].ToString();
                    }
                    rdfixed.Checked = false;
                    rdcalculate.Checked = false;
                    switch (ds.Tables[0].Rows[i]["typeofleave"].ToString())
                    {
                        case "1":
                            rdfixed.Checked = true;
                            break;
                        case "2":
                            rdcalculate.Checked = true;
                            break;
                        default:
                            break;
                    }
                    //dr["maxleaveallot"] = DBNull.Value;
                    //dr["daysworkedinmonth"] = DBNull.Value;
                    //dr["Durationofleaveearned"] = DBNull.Value;
                    //dr["leaveid"] = 0;
                    //dr["maxleaveallotid"] = 1;
                    //dr["balleaveid"] = 1;
                    //dr["leavegenderid"] = 1;
                    txtmaxleaveallot.Text = ds.Tables[0].Rows[i]["maxleaveallot"].ToString();
                    if (ddmaxleavetype.Items.FindByValue(ds.Tables[0].Rows[i]["maxleaveallotid"].ToString()) != null)
                    {
                        ddmaxleavetype.SelectedValue = ds.Tables[0].Rows[i]["maxleaveallotid"].ToString();
                    }
                    txtdaysworkedinmonth.Text = ds.Tables[0].Rows[i]["daysworkedinmonth"].ToString();
                    txtdurationofleaveearned.Text = ds.Tables[0].Rows[i]["Durationofleaveearned"].ToString();
                    if (DDballeave.Items.FindByValue(ds.Tables[0].Rows[i]["balleaveid"].ToString()) != null)
                    {
                        DDballeave.SelectedValue = ds.Tables[0].Rows[i]["balleaveid"].ToString();
                    }
                    if (DDleavegender.Items.FindByValue(ds.Tables[0].Rows[i]["leavegenderid"].ToString()) != null)
                    {
                        DDleavegender.SelectedValue = ds.Tables[0].Rows[i]["leavegenderid"].ToString();
                    }

                }
            }
            else
            {
                SetInitialRowGrid();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}