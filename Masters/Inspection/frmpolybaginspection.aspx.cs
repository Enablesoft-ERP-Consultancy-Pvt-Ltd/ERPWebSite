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
            CompanyWiseGridViewChange();
        }
    }

    private void CompanyWiseGridViewChange()
    {
        if (Convert.ToInt32(Session["varcompanyid"]) == 45)
        {
            Dgdetail.Columns[8].Visible = false;
            Dgdetail.Columns[11].Visible = false;
            Dgdetail.Columns[12].Visible = false;
        }
        else
        {
            Dgdetail.Columns[4].Visible = false;
            Dgdetail.Columns[13].Visible = false;
            Dgdetail.Columns[14].Visible = false;
            Dgdetail.Columns[15].Visible = false;
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
        dt.Columns.Add(new DataColumn("Totalqty", typeof(string)));
        dt.Columns.Add(new DataColumn("SampleSize", typeof(string)));
        dt.Columns.Add(new DataColumn("SizeInCM", typeof(string)));
        dt.Columns.Add(new DataColumn("Gauge", typeof(string)));
        dt.Columns.Add(new DataColumn("Sealing", typeof(string)));
        dt.Columns.Add(new DataColumn("Printing", typeof(string)));
        dt.Columns.Add(new DataColumn("Tearing", typeof(string)));
        dt.Columns.Add(new DataColumn("CLearity", typeof(string)));
        dt.Columns.Add(new DataColumn("Color", typeof(string)));
        dt.Columns.Add(new DataColumn("Found", typeof(string)));
        dt.Columns.Add(new DataColumn("Acceptance", typeof(string)));
        dt.Columns.Add(new DataColumn("SymbleONPolyBag", typeof(string)));
        dt.Columns.Add(new DataColumn("NoofPunture", typeof(string)));
        dt.Columns.Add(new DataColumn("Others", typeof(string)));
        dt.Columns.Add(new DataColumn("Lotresult", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["Description"] = string.Empty;
        dr["Totalqty"] = string.Empty;
        dr["Samplesize"] = string.Empty;
        dr["SizeInCM"] = string.Empty;
        dr["Gauge"] = string.Empty;
        dr["Sealing"] = string.Empty;
        dr["Printing"] = string.Empty;
        dr["Tearing"] = string.Empty;
        dr["Clearity"] = string.Empty;
        dr["color"] = string.Empty;
        dr["Found"] = string.Empty;
        dr["Acceptance"] = string.Empty;
        dr["SymbleONPolyBag"] = string.Empty;
        dr["NoofPunture"] = string.Empty;
        dr["Others"] = string.Empty;
        dr["Lotresult"] = string.Empty;
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
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txttotalqty");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsamplesize");
                    TextBox txtSizeInCM = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtSizeInCM");
                    TextBox txtgauge = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtgauge");
                    TextBox txtsealing = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtsealing");
                    TextBox txtprinting = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtprinting");
                    TextBox txttearing = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txttearing");
                    TextBox txtclearity = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtclearity");
                    TextBox txtcolor = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtcolor");
                    TextBox txtfound = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtfound");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtacceptance");
                    TextBox txtSymbleONPolyBag = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtSymbleONPolyBag");
                    TextBox txtNoofPunture = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtNoofPunture");
                    TextBox txtOthers = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtOthers");
                    TextBox txtlotresult = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtlotresult");
                    
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["description"] = txtdescription.Text;
                    dtCurrentTable.Rows[i - 1]["totalqty"] = txttotalqty.Text;
                    dtCurrentTable.Rows[i - 1]["samplesize"] = txtsamplesize.Text;
                    dtCurrentTable.Rows[i - 1]["SizeInCM"] = txtSizeInCM.Text;
                    dtCurrentTable.Rows[i - 1]["Gauge"] = txtgauge.Text;
                    dtCurrentTable.Rows[i - 1]["Sealing"] = txtsealing.Text;
                    dtCurrentTable.Rows[i - 1]["Printing"] = txtprinting.Text;
                    dtCurrentTable.Rows[i - 1]["Tearing"] = txttearing.Text;
                    dtCurrentTable.Rows[i - 1]["Clearity"] = txtclearity.Text;
                    dtCurrentTable.Rows[i - 1]["Color"] = txtcolor.Text;
                    dtCurrentTable.Rows[i - 1]["Found"] = txtfound.Text;
                    dtCurrentTable.Rows[i - 1]["Acceptance"] = txtacceptance.Text;
                    dtCurrentTable.Rows[i - 1]["SymbleONPolyBag"] = txtSymbleONPolyBag.Text;
                    dtCurrentTable.Rows[i - 1]["NoofPunture"] = txtNoofPunture.Text;
                    dtCurrentTable.Rows[i - 1]["Others"] = txtOthers.Text;
                    dtCurrentTable.Rows[i - 1]["Lotresult"] = txtlotresult.Text;

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
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txttotalqty");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsamplesize");
                    TextBox txtSizeInCM = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtSizeInCM");
                    TextBox txtgauge = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtgauge");
                    TextBox txtsealing = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtsealing");
                    TextBox txtprinting = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtprinting");
                    TextBox txttearing = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txttearing");
                    TextBox txtclearity = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtclearity");
                    TextBox txtcolor = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtcolor");
                    TextBox txtfound = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtfound");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtacceptance");
                    TextBox txtSymbleONPolyBag = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtSymbleONPolyBag");
                    TextBox txtNoofPunture = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtNoofPunture");
                    TextBox txtOthers = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtOthers");
                    TextBox txtlotresult = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtlotresult");

                    txtsrno.Text = dt.Rows[i]["srno"].ToString();
                    txtdescription.Text = dt.Rows[i]["Description"].ToString();
                    txttotalqty.Text = dt.Rows[i]["totalqty"].ToString();
                    txtsamplesize.Text = dt.Rows[i]["samplesize"].ToString();
                    txtSizeInCM.Text = dt.Rows[i]["SizeInCM"].ToString();
                    txtgauge.Text = dt.Rows[i]["Gauge"].ToString();
                    txtsealing.Text = dt.Rows[i]["sealing"].ToString();
                    txtprinting.Text = dt.Rows[i]["printing"].ToString();
                    txttearing.Text = dt.Rows[i]["Tearing"].ToString();
                    txtclearity.Text = dt.Rows[i]["clearity"].ToString();
                    txtcolor.Text = dt.Rows[i]["color"].ToString();
                    txtfound.Text = dt.Rows[i]["found"].ToString();
                    txtacceptance.Text = dt.Rows[i]["acceptance"].ToString();
                    txtSymbleONPolyBag.Text = dt.Rows[i]["SymbleONPolyBag"].ToString();
                    txtNoofPunture.Text = dt.Rows[i]["NoofPunture"].ToString();
                    txtOthers.Text = dt.Rows[i]["Others"].ToString();
                    txtlotresult.Text = dt.Rows[i]["lotresult"].ToString();

                    // sc.Add(box1.Text + "," + box2.Text + "," + box3.Text);

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
            SqlParameter[] param = new SqlParameter[12];
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
            param[7] = new SqlParameter("@standaredSize", txtstandaredsize.Text);
            param[8] = new SqlParameter("@suppliername", txtsuppliername.Text);
            param[9] = new SqlParameter("@challanNodate", txtchallannodate.Text);
            param[10] = new SqlParameter("@StandaredGauge", txtstandaredgauge.Text);
            param[11] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVEPOLYBAGINSPECTION]", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM PolybagInspectionDetail WHERE DOCID=" + hndocid.Value;
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
                TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txttotalqty");
                TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                TextBox txtSizeInCM = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtSizeInCM");
                TextBox txtgauge = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtgauge");
                TextBox txtsealing = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtsealing");
                TextBox txtprinting = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtprinting");
                TextBox txttearing = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txttearing");
                TextBox txtclearity = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtclearity");
                TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtcolor");
                TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtfound");
                TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtacceptance");
                TextBox txtSymbleONPolyBag = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtSymbleONPolyBag");
                TextBox txtNoofPunture = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtNoofPunture");
                TextBox txtOthers = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtOthers");
                TextBox txtlotresult = (TextBox)Dgdetail.Rows[i].Cells[16].FindControl("txtlotresult");

                if (txtsrno.Text != "")
                {
                    str = str + @"  Insert into PolybagInspectionDetail(Docid,Srno,Description,totalqty,
                    samplesize,Gauge,Sealing,Printing,Tearing,
                    Clearity,Color,Found,Acceptance,Lotresult,
                    SizeInCM,SymbleONPolyBag,NoofPunture,Others)
                    values (" + hndocid.Value + ",'" + txtsrno.Text + "','" + txtdescription.Text.Replace("'", "''") + "','" + txttotalqty.Text.Replace("'", "''") + @"',
                    '" + txtsamplesize.Text.Replace("'", "''") + "','" + txtgauge.Text.Replace("'", "''") + "','" + txtsealing.Text.Replace("'", "''") + "','" + txtprinting.Text.Replace("'", "''") + "','" + txttearing.Text.Replace("'", "''") + @"',
                    '" + txtclearity.Text.Replace("'", "''") + "','" + txtcolor.Text.Replace("'", "''") + "','" + txtfound.Text.Replace("'", "''") + "','" + txtacceptance.Text.Replace("'", "''") + "','" + txtlotresult.Text.Replace("'", "''") + @"',
                    '" + txtSizeInCM.Text.Replace("'", "''") + "','" + txtSymbleONPolyBag.Text.Replace("'", "''") + "','" + txtNoofPunture.Text.Replace("'", "''") + "','" + txtOthers.Text.Replace("'", "''") + "')";
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

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_GETPOLYBAGINSPECTIONREPORT]", param);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (Session["VarCompanyNo"].ToString() == "21")
                {
                    Session["rptFileName"] = "~\\Reports\\rptPolyBaginspectionKaysons.rpt";
                }
                else if (Session["VarCompanyNo"].ToString() == "45")
                {
                    Session["rptFileName"] = "~\\Reports\\rptPolyBaginspectionMWS.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptPolyBaginspection.rpt";
                }
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptPolyBaginspection.xsd";
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPOLYBAGINSPECTION", param);
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_APPROVEPOLYBAGINSPECTION]", param);
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
        txtstandaredsize.Text = "";
        txtstandaredgauge.Text = "";
        SetInitialRow();
    }
    private void fillDocno()
    {
        string str = @"SELECT Distinct RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
            FROM POLYBAGINSPECTIONMASTER RIM 
            --INNER JOIN PolybagInspectionDetail RID ON RIM.DOCID=RID.DOCID
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
        string str = @"SELECT * FROM PolybagInspectionMaster RIM Where RIM.DocId=" + hndocid.Value + @"
                       SELECT * FROM PolybagInspectionDetail RID Where RID.DocId=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["docNo"].ToString();
            txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Reportdate"]).ToString("dd-MMM-yyyy");
            txtstandaredsize.Text = ds.Tables[0].Rows[0]["standaredsize"].ToString();
            txtstandaredgauge.Text = ds.Tables[0].Rows[0]["standaredGauge"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["suppliername"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["challanNodate"].ToString();
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();

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
                        TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txttotalqty");
                        TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                        TextBox txtSizeInCM = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtSizeInCM");
                        TextBox txtgauge = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtgauge");
                        TextBox txtsealing = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtsealing");
                        TextBox txtprinting = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtprinting");
                        TextBox txttearing = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txttearing");
                        TextBox txtclearity = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtclearity");
                        TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtcolor");
                        TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtfound");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtacceptance");
                        TextBox txtSymbleONPolyBag = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtSymbleONPolyBag");
                        TextBox txtNoofPunture = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtNoofPunture");
                        TextBox txtOthers = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtOthers");
                        TextBox txtlotresult = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtlotresult");

                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["totalqty"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtSizeInCM.Text = ds.Tables[1].Rows[i]["SizeInCM"].ToString(); 
                        txtgauge.Text = ds.Tables[1].Rows[i]["Gauge"].ToString();
                        txtsealing.Text = ds.Tables[1].Rows[i]["Sealing"].ToString();
                        txtprinting.Text = ds.Tables[1].Rows[i]["Printing"].ToString();
                        txttearing.Text = ds.Tables[1].Rows[i]["Tearing"].ToString();
                        txtclearity.Text = ds.Tables[1].Rows[i]["Clearity"].ToString();
                        txtcolor.Text = ds.Tables[1].Rows[i]["COlor"].ToString();
                        txtfound.Text = ds.Tables[1].Rows[i]["found"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        txtSymbleONPolyBag.Text = ds.Tables[1].Rows[i]["SymbleONPolyBag"].ToString();
                        txtNoofPunture.Text = ds.Tables[1].Rows[i]["NoofPunture"].ToString();
                        txtOthers.Text = ds.Tables[1].Rows[i]["Others"].ToString();
                        txtlotresult.Text = ds.Tables[1].Rows[i]["lotresult"].ToString();
                    }
                    else
                    {
                        AddNewRowToGrid();

                        TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                        TextBox txtdescription = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtdescription");
                        TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txttotalqty");
                        TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                        TextBox txtSizeInCM= (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtSizeInCM");
                        TextBox txtgauge = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtgauge");
                        TextBox txtsealing = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtsealing");
                        TextBox txtprinting = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtprinting");
                        TextBox txttearing = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txttearing");
                        TextBox txtclearity = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtclearity");
                        TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtcolor");
                        TextBox txtfound = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtfound");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtacceptance");
                        TextBox txtSymbleONPolyBag = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtSymbleONPolyBag");
                        TextBox txtNoofPunture = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtNoofPunture");
                        TextBox txtOthers = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtOthers");
                        TextBox txtlotresult = (TextBox)Dgdetail.Rows[i].Cells[16].FindControl("txtlotresult");

                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["totalqty"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtSizeInCM.Text = ds.Tables[1].Rows[i]["SizeInCM"].ToString();
                        txtgauge.Text = ds.Tables[1].Rows[i]["Gauge"].ToString();
                        txtsealing.Text = ds.Tables[1].Rows[i]["Sealing"].ToString();
                        txtprinting.Text = ds.Tables[1].Rows[i]["Printing"].ToString();
                        txttearing.Text = ds.Tables[1].Rows[i]["Tearing"].ToString();
                        txtclearity.Text = ds.Tables[1].Rows[i]["Clearity"].ToString();
                        txtcolor.Text = ds.Tables[1].Rows[i]["COlor"].ToString();
                        txtfound.Text = ds.Tables[1].Rows[i]["found"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        txtSymbleONPolyBag.Text = ds.Tables[1].Rows[i]["SymbleONPolyBag"].ToString();
                        txtNoofPunture.Text = ds.Tables[1].Rows[i]["NoofPunture"].ToString();
                        txtOthers.Text = ds.Tables[1].Rows[i]["Others"].ToString();
                        txtlotresult.Text = ds.Tables[1].Rows[i]["lotresult"].ToString();
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
}