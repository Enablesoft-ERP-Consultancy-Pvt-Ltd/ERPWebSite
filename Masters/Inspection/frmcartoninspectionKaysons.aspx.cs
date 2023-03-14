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
using System.Text;

public partial class Masters_Inspection_frmcartoninspection : System.Web.UI.Page
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
                SetInitialRow();
            }
            string str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                    WHere CI.MasterCompanyid=" + Session["varcompanyid"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompanyName, ds, 0, true, "Plz Select--");

            if (DDcompanyName.Items.Count > 0)
            {
                DDcompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            switch (Session["usertype"].ToString())
            {
                case "1":
                case "2":
                    btnApprove.Visible = true;
                    Changeapprovebuttoncolor(0);
                    break;
                default:
                    btnApprove.Visible = false;
                    break;
            }
        }
    }
    protected void Changeapprovebuttoncolor(int approvestatus = 0)
    {
        switch (approvestatus)
        {
            case 1:
                btnApprove.BackColor = System.Drawing.Color.Green;
                break;
            default:
                btnApprove.BackColor = System.Drawing.Color.Red;
                break;
        }
    }
    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Srno", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("TestReport", typeof(string)));
        dt.Columns.Add(new DataColumn("size", typeof(string)));
        dt.Columns.Add(new DataColumn("SampleSize", typeof(string)));
        dt.Columns.Add(new DataColumn("noofply", typeof(string)));
        dt.Columns.Add(new DataColumn("IKEYCode", typeof(string)));
        dt.Columns.Add(new DataColumn("Moisture", typeof(string)));
        dt.Columns.Add(new DataColumn("Burstingstrength", typeof(string)));
        dt.Columns.Add(new DataColumn("Weight", typeof(string)));
        dt.Columns.Add(new DataColumn("Found", typeof(string)));
        dt.Columns.Add(new DataColumn("Acceptance", typeof(string)));
        dt.Columns.Add(new DataColumn("PastingOfTtheFeet", typeof(string)));
        dt.Columns.Add(new DataColumn("GapBtweenFeet", typeof(string)));
        dt.Columns.Add(new DataColumn("FourWayEntry", typeof(string)));
        dt.Columns.Add(new DataColumn("FeetPattern", typeof(string)));
        dt.Columns.Add(new DataColumn("CAPAonObservation", typeof(string)));
        dt.Columns.Add(new DataColumn("Lotresult", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["Description"] = string.Empty;
        dr["TestReport"] = string.Empty;
        dr["size"] = string.Empty;
        dr["Samplesize"] = string.Empty;
        dr["noofply"] = string.Empty;
        dr["IKEYCode"] = string.Empty;
        dr["Moisture"] = string.Empty;
        dr["Burstingstrength"] = string.Empty;
        dr["Weight"] = string.Empty;
        dr["Found"] = string.Empty;
        dr["Acceptance"] = string.Empty;
        dr["PastingOfTtheFeet"] = string.Empty;
        dr["GapBtweenFeet"] = string.Empty;
        dr["FourWayEntry"] = string.Empty;
        dr["FeetPattern"] = string.Empty;
        dr["CAPAonObservation"] = string.Empty;
        dr["Lotresult"] = "Pass";
        dt.Rows.Add(dr);
        //dr = dt.NewRow();

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        Dgdetail.DataSource = dt;
        Dgdetail.DataBind();
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtsrno = (TextBox)Dgdetail.Rows[rowIndex].Cells[0].FindControl("txtsrno");
                    TextBox txtdescription = (TextBox)Dgdetail.Rows[rowIndex].Cells[1].FindControl("txtdescription");
                    TextBox txtTestReport = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txtTestReport");
                    TextBox txtsize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsize");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtsamplesize");
                    TextBox txtnoofply = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtnoofply");
                    TextBox txtIkeyCode = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtIkeyCode");
                    TextBox txtmoisture = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtmoisture");
                    TextBox txtburstingstrength = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtburstingstrength");
                    TextBox txtweight = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtweight");
                    TextBox txtfound = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtfound");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtacceptance");
                    TextBox txtPastingOfTheFeet = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtPastingOfTheFeet");
                    TextBox txtGapBtweenFeet = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtGapBtweenFeet");
                    TextBox txtFourWayEntry = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtFourWayEntry");
                    TextBox txtFeetPattern = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtFeetPattern");
                    TextBox txtCAPAonObservation = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtCAPAonObservation");
                    DropDownList ddresult = (DropDownList)Dgdetail.Rows[rowIndex].Cells[17].FindControl("ddresult");
                    Label lbllotresult = (Label)Dgdetail.Rows[rowIndex].FindControl("lbllotresult");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["description"] = txtdescription.Text;
                    dtCurrentTable.Rows[i - 1]["TestReport"] = txtdescription.Text;
                    dtCurrentTable.Rows[i - 1]["size"] = txtsize.Text;
                    dtCurrentTable.Rows[i - 1]["samplesize"] = txtsamplesize.Text;
                    dtCurrentTable.Rows[i - 1]["noofply"] = txtnoofply.Text;
                    dtCurrentTable.Rows[i - 1]["IKEYCode"] = txtIkeyCode.Text;
                    dtCurrentTable.Rows[i - 1]["Moisture"] = txtmoisture.Text;
                    dtCurrentTable.Rows[i - 1]["burstingstrength"] = txtburstingstrength.Text;
                    dtCurrentTable.Rows[i - 1]["Weight"] = txtweight.Text;
                    dtCurrentTable.Rows[i - 1]["Found"] = txtfound.Text;
                    dtCurrentTable.Rows[i - 1]["Acceptance"] = txtacceptance.Text;
                    dtCurrentTable.Rows[i - 1]["PastingOfTtheFeet"] = txtPastingOfTheFeet.Text;
                    dtCurrentTable.Rows[i - 1]["GapBtweenFeet"] = txtGapBtweenFeet.Text;
                    dtCurrentTable.Rows[i - 1]["FourWayEntry"] = txtFourWayEntry.Text;
                    dtCurrentTable.Rows[i - 1]["FeetPattern"] = txtFeetPattern.Text;
                    dtCurrentTable.Rows[i - 1]["CAPAonObservation"] = txtCAPAonObservation.Text;
                    dtCurrentTable.Rows[i - 1]["Lotresult"] = ddresult.SelectedItem.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                Dgdetail.DataSource = dtCurrentTable;
                Dgdetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        //StringCollection sc = new StringCollection();
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtsrno = (TextBox)Dgdetail.Rows[rowIndex].Cells[0].FindControl("txtsrno");
                    TextBox txtdescription = (TextBox)Dgdetail.Rows[rowIndex].Cells[1].FindControl("txtdescription");
                    TextBox txtTestReport = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txtTestReport");
                    TextBox txtsize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsize");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtsamplesize");
                    TextBox txtnoofply = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtnoofply");
                    TextBox txtIkeyCode = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtIkeyCode");
                    TextBox txtmoisture = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtmoisture");
                    TextBox txtburstingstrength = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtburstingstrength");
                    TextBox txtweight = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtweight");
                    TextBox txtfound = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtfound");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtacceptance");
                    TextBox txtPastingOfTheFeet = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtPastingOfTheFeet");
                    TextBox txtGapBtweenFeet = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtGapBtweenFeet");
                    TextBox txtFourWayEntry = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtFourWayEntry");
                    TextBox txtFeetPattern = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtFeetPattern");
                    TextBox txtCAPAonObservation = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtCAPAonObservation");
                    DropDownList ddresult = (DropDownList)Dgdetail.Rows[rowIndex].Cells[17].FindControl("ddresult");

                    txtsrno.Text = dt.Rows[i]["srno"].ToString();
                    txtdescription.Text = dt.Rows[i]["Description"].ToString();
                    txtTestReport.Text = dt.Rows[i]["TestReport"].ToString();
                    txtsize.Text = dt.Rows[i]["size"].ToString();
                    txtsamplesize.Text = dt.Rows[i]["samplesize"].ToString();
                    txtnoofply.Text = dt.Rows[i]["noofply"].ToString();
                    txtIkeyCode.Text = dt.Rows[i]["IKEYCode"].ToString();
                    txtmoisture.Text = dt.Rows[i]["moisture"].ToString();
                    txtburstingstrength.Text = dt.Rows[i]["burstingstrength"].ToString();
                    txtweight.Text = dt.Rows[i]["weight"].ToString();
                    txtfound.Text = dt.Rows[i]["found"].ToString();
                    txtacceptance.Text = dt.Rows[i]["acceptance"].ToString();
                    txtPastingOfTheFeet.Text = dt.Rows[i]["PastingOfTtheFeet"].ToString();
                    txtGapBtweenFeet.Text = dt.Rows[i]["GapBtweenFeet"].ToString();
                    txtFourWayEntry.Text = dt.Rows[i]["FourWayEntry"].ToString();
                    txtFeetPattern.Text = dt.Rows[i]["FeetPattern"].ToString();
                    txtCAPAonObservation.Text = dt.Rows[i]["CAPAonObservation"].ToString();
                    //ddresult.SelectedItem.Text = dt.Rows[i]["lotresult"].ToString();
                    /*if (!string.IsNullOrWhiteSpace(dt.Rows[i]["lotresult"].ToString()))
                    {
                        ddresult.Items.FindByValue(dt.Rows[i]["lotresult"].ToString()).Selected = true;
                    }*/
                    foreach (ListItem item in ddresult.Items)
                    {
                        if (string.Equals(item.Value, dt.Rows[i]["lotresult"].ToString()))
                        {
                            item.Selected = true;
                        }
                    }
                    rowIndex++;


                }

                //InsertRecords(sc);
            }
        }
    }
    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
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
            SqlParameter[] param = new SqlParameter[19];
            param[0] = new SqlParameter("@Docid", SqlDbType.Int);
            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = hndocid.Value;
            param[1] = new SqlParameter("@Companyid", DDcompanyName.SelectedValue);
            param[2] = new SqlParameter("@DocNo", SqlDbType.VarChar, 50);
            param[2].Value = txtdocno.Text;
            param[2].Direction = ParameterDirection.InputOutput;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@Comments", txtcomments.Text.Trim());
            param[6] = new SqlParameter("@ReportDate", txtdate.Text);
            //param[7] = new SqlParameter("@standared", txtstandared.Text);
            param[7] = new SqlParameter("@suppliername", txtsuppliername.Text);
            param[8] = new SqlParameter("@challanNodate", txtchallannodate.Text);
            param[9] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            param[10] = new SqlParameter("@InspectedBy", txtInspectedBy.Text);
            param[11] = new SqlParameter("@OrderQty", txtOrderQty.Text);
            param[12] = new SqlParameter("@InspectedQty", txtInspectedQty.Text);
            param[13] = new SqlParameter("@AcceptableQty", txtAcceptableQty.Text);
            param[14] = new SqlParameter("@SamplingPlanAQL", txtSamplingPlanAQL.Text);
            param[15] = new SqlParameter("@UIDNo", txtUIDNo.Text);
            param[16] = new SqlParameter("@Symbol1", chkSymbolList.Items[0].Selected);
            param[17] = new SqlParameter("@Symbol2", chkSymbolList.Items[1].Selected);
            param[18] = new SqlParameter("@Symbol3", chkSymbolList.Items[2].Selected);

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVECARTONINSPECTIONKaysons]", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM CartonInspectionDetail WHERE DOCID=" + hndocid.Value;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            insertrecordDetail(Tran);

            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[4].Value.ToString() + "')", true);

            Tran.Commit();
            //**********

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
    private void insertrecordDetail(SqlTransaction Tran)
    {

        string str = "";
        if (Dgdetail.Rows.Count != 0)
        {

            for (int i = 0; i < Dgdetail.Rows.Count; i++)
            {
                TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                TextBox txtdescription = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtdescription");
                TextBox txtTestReport = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtTestReport");
                TextBox txtsize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtsize");
                TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                TextBox txtnoofply = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtnoofply");
                TextBox txtIkeyCode = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtIkeyCode");
                TextBox txtmoisture = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtmoisture");
                TextBox txtburstingstrength = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtburstingstrength");
                TextBox txtweight = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtweight");
                TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtfound");
                TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtacceptance");
                TextBox txtPastingOfTheFeet = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtPastingOfTheFeet");
                TextBox txtGapBtweenFeet = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtGapBtweenFeet");
                TextBox txtFourWayEntry = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtFourWayEntry");
                TextBox txtFeetPattern = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtFeetPattern");
                TextBox txtCAPAonObservation = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtCAPAonObservation");
                DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[16].FindControl("ddresult");

                if (txtsrno.Text != "")
                {
                    str = str + @"  Insert into CartonInspectionDetail(Docid,Srno,Description,TestReport,size,samplesize,Noofply,IKEYCode,Moisture,Burstingstrength,Weight,Found,Acceptance,PastingOfTtheFeet,GapBtweenFeet,FourWayEntry,FeetPattern,CAPAonObservation,Lotresult)
                   values (" + hndocid.Value + ",'" + txtsrno.Text + "','" + txtdescription.Text.Replace("'", "''") + "','" + txtTestReport.Text.Replace("'", "''") + "','" + txtsize.Text.Replace("'", "''") + "','" + txtsamplesize.Text.Replace("'", "''") + "','" + txtnoofply.Text.Replace("'", "''") + "','" + txtIkeyCode.Text.Replace("'", "''") + "','" + txtmoisture.Text.Replace("'", "''") + @"',
                          '" + txtburstingstrength.Text.Replace("'", "''") + "','" + txtweight.Text.Replace("'", "''") + "','" + txtfound.Text.Replace("'", "''") + "','" + txtacceptance.Text.Replace("'", "''") + "','" + txtPastingOfTheFeet.Text.Replace("'", "''")
                              + "','" + txtGapBtweenFeet.Text.Replace("'", "''") + "','" + txtFourWayEntry.Text.Replace("'", "''") + "','" + txtFeetPattern.Text.Replace("'", "''") + "','" + txtCAPAonObservation.Text.Replace("'", "''") + "','" + ddresult.SelectedItem.Text.Replace("'", "''") + "')";
                }


            }
            if (str != "")
            {
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
        }

    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Docid", hndocid.Value);

            //            string str = @"SELECT Ci.companyName,* FROM RAWYARNINSPECTIONMASTER RIM INNER JOIN RAWYARNINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
            //                           inner join CompanyInfo ci on RIM.COMPANYID=ci.CompanyId Where RIM.Docid=" + hndocid.Value;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETCARTONINSPECTIONREPORT", param);

            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["rptFileName"] = "~\\Reports\\rptcartoninspectionKaysons.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptcartoninspectionKaysons.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No records found.')", true);
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETECARTONINSPECTION", param);
            Tran.Commit();
            if (param[3].Value.ToString() != "")
            {
                lblmsg.Text = param[3].Value.ToString();
            }
            else
            {
                lblmsg.Text = "DOC No. Deleted Successfully.";
                fillDocno();
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
    protected void btnApprove_Click(object sender, EventArgs e)
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
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Docid", hndocid.Value);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_APPROVECARTONINSPECTION", param);
            Tran.Commit();
            lblmsg.Text = param[3].Value.ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altappv", "alert('" + param[3].Value.ToString() + "')", true);
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDSupplierSearch.Visible = false;
        TDDocno.Visible = false;
        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;
            TDSupplierSearch.Visible = true;
            fillDocno();
        }
    }
    private void refreshcontrol()
    {
        txtsuppliername.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtchallannodate.Text = "";
        txtUIDNo.Text = "";
        foreach (ListItem item in chkSymbolList.Items)
        {
            item.Selected = false;
        }

        SetInitialRow();
    }
    private void fillDocno()
    {
        string str = @"SELECT Distinct RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                FROM CartonInspectionMaster RIM(Nolock) 
                INNER JOIN CartonInspectionDetail RID(Nolock) ON RIM.DOCID=RID.DOCID
                Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue;
        if (txtsuppliersearch.Text != "")
        {
            str = str + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        str = str + " order by DOCID";
        UtilityModule.ConditionalComboFill(ref DDDocNo, str, true, "--Plz Select--");
    }
    protected void DDDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = DDDocNo.SelectedValue;
        refreshcontrol();
        FillDataback();
    }
    protected void FillDataback()
    {
        string str = @"SELECT * FROM CartonInspectionMaster RIM Where RIM.DocId=" + hndocid.Value + @"
                       SELECT * FROM CartonInspectionDetail RID Where RID.DocId=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["docNo"].ToString();
            txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Reportdate"]).ToString("dd-MMM-yyyy");
            txtInspectedBy.Text = ds.Tables[0].Rows[0]["InspectedBy"].ToString();
            txtOrderQty.Text = ds.Tables[0].Rows[0]["OrderQty"].ToString();
            txtAcceptableQty.Text = ds.Tables[0].Rows[0]["AcceptableQty"].ToString();
            txtInspectedQty.Text = ds.Tables[0].Rows[0]["InspectedQty"].ToString();
            txtSamplingPlanAQL.Text = ds.Tables[0].Rows[0]["SamplingPlanAQL"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["suppliername"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["challanNodate"].ToString();
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            txtUIDNo.Text = ds.Tables[0].Rows[0]["UIDNo"].ToString();
            foreach (ListItem item in chkSymbolList.Items)
            {
                item.Selected = Convert.ToBoolean(ds.Tables[0].Rows[0][item.Value]);
            }

            Changeapprovebuttoncolor(Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));
            EditRights_Button(Convert.ToInt16(Session["usertype"]), Convert.ToInt16(ds.Tables[0].Rows[0]["approvestatus"]));

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                        TextBox txtdescription = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtdescription");
                        TextBox txtTestReport = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtTestReport");
                        TextBox txtsize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtsize");
                        TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                        TextBox txtnoofply = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtnoofply");
                        TextBox txtIkeyCode = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtIkeyCode");
                        TextBox txtmoisture = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtmoisture");
                        TextBox txtburstingstrength = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtburstingstrength");
                        TextBox txtweight = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtweight");
                        TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtfound");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtacceptance");
                        TextBox txtPastingOfTheFeet = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtPastingOfTheFeet");
                        TextBox txtGapBtweenFeet = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtGapBtweenFeet");
                        TextBox txtFourWayEntry = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtFourWayEntry");
                        TextBox txtFeetPattern = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtFeetPattern");
                        TextBox txtCAPAonObservation = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtCAPAonObservation");
                        DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[16].FindControl("ddresult");

                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txtTestReport.Text = ds.Tables[1].Rows[i]["TestReport"].ToString();
                        txtsize.Text = ds.Tables[1].Rows[i]["size"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtnoofply.Text = ds.Tables[1].Rows[i]["noofply"].ToString();
                        txtIkeyCode.Text = ds.Tables[1].Rows[i]["IKEYCode"].ToString();
                        txtmoisture.Text = ds.Tables[1].Rows[i]["Moisture"].ToString();
                        txtburstingstrength.Text = ds.Tables[1].Rows[i]["burstingstrength"].ToString();
                        txtweight.Text = ds.Tables[1].Rows[i]["Weight"].ToString();
                        txtfound.Text = ds.Tables[1].Rows[i]["found"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        txtPastingOfTheFeet.Text = ds.Tables[1].Rows[i]["PastingOfTtheFeet"].ToString();
                        txtGapBtweenFeet.Text = ds.Tables[1].Rows[i]["GapBtweenFeet"].ToString();
                        txtFourWayEntry.Text = ds.Tables[1].Rows[i]["FourWayEntry"].ToString();
                        txtFeetPattern.Text = ds.Tables[1].Rows[i]["FeetPattern"].ToString();
                        txtCAPAonObservation.Text = ds.Tables[1].Rows[i]["CAPAonObservation"].ToString();
                        foreach (ListItem item in ddresult.Items)
                        {
                            if (string.Equals(item.Value, ds.Tables[1].Rows[i]["lotresult"].ToString()))
                            {
                                item.Selected = true;
                            }
                        }
                        /*if (!string.IsNullOrWhiteSpace(ds.Tables[1].Rows[i]["lotresult"].ToString()))
                        {
                            ddresult.Items.FindByValue(ds.Tables[1].Rows[i]["lotresult"].ToString()).Selected = true;
                        }*/
                        ddresult.SelectedItem.Text = ds.Tables[1].Rows[i]["lotresult"].ToString();
                    }
                    else
                    {
                        AddNewRowToGrid();

                        TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                        TextBox txtdescription = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtdescription");
                        TextBox txtTestReport = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtTestReport");
                        TextBox txtsize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtsize");
                        TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                        TextBox txtnoofply = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtnoofply");
                        TextBox txtIkeyCode = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtIkeyCode");
                        TextBox txtmoisture = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtmoisture");
                        TextBox txtburstingstrength = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtburstingstrength");
                        TextBox txtweight = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtweight");
                        TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtfound");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtacceptance");
                        TextBox txtPastingOfTheFeet = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtPastingOfTheFeet");
                        TextBox txtGapBtweenFeet = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtGapBtweenFeet");
                        TextBox txtFourWayEntry = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtFourWayEntry");
                        TextBox txtFeetPattern = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtFeetPattern");
                        TextBox txtCAPAonObservation = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtCAPAonObservation");
                        DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[16].FindControl("ddresult");

                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txtTestReport.Text = ds.Tables[1].Rows[i]["TestReport"].ToString();
                        txtsize.Text = ds.Tables[1].Rows[i]["size"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtnoofply.Text = ds.Tables[1].Rows[i]["noofply"].ToString();
                        txtIkeyCode.Text = ds.Tables[1].Rows[i]["IKEYCode"].ToString();
                        txtmoisture.Text = ds.Tables[1].Rows[i]["Moisture"].ToString();
                        txtburstingstrength.Text = ds.Tables[1].Rows[i]["burstingstrength"].ToString();
                        txtweight.Text = ds.Tables[1].Rows[i]["Weight"].ToString();
                        txtfound.Text = ds.Tables[1].Rows[i]["found"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        txtPastingOfTheFeet.Text = ds.Tables[1].Rows[i]["PastingOfTtheFeet"].ToString();
                        txtGapBtweenFeet.Text = ds.Tables[1].Rows[i]["GapBtweenFeet"].ToString();
                        txtFourWayEntry.Text = ds.Tables[1].Rows[i]["FourWayEntry"].ToString();
                        txtFeetPattern.Text = ds.Tables[1].Rows[i]["FeetPattern"].ToString();
                        txtCAPAonObservation.Text = ds.Tables[1].Rows[i]["CAPAonObservation"].ToString();
                        foreach (ListItem item in ddresult.Items)
                        {
                            if (string.Equals(item.Value, ds.Tables[1].Rows[i]["lotresult"].ToString()))
                            {
                                item.Selected = true;
                            }
                        }
                    }


                }
            }

        }
    }
    protected void EditRights_Button(int usertype, int approvestatus = 0)
    {
        switch (usertype)
        {
            case 1:
            case 2:
                btnsave.Visible = true;
                btndelete.Visible = true;
                break;
            default:
                if (approvestatus == 1)
                {
                    btnsave.Visible = false;
                    btndelete.Visible = false;
                }
                else
                {
                    btnsave.Visible = true;
                    btndelete.Visible = true;
                }
                break;
        }
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        fillDocno();
    }
    protected void Dgdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbllotresult = (Label)e.Row.FindControl("lbllotresult");
            DropDownList ddresult = (DropDownList)e.Row.FindControl("ddresult");
            if (ddresult != null)
            {
                foreach (ListItem item in ddresult.Items)
                {
                    if (string.Equals(item.Value, lbllotresult))
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}