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
        if (Session["varMasterCompanyIDForERP"] == null)
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
                  WHere CI.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

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
        dt.Columns.Add(new DataColumn("ArticleName", typeof(string)));
        dt.Columns.Add(new DataColumn("ArticleSize", typeof(string)));
        dt.Columns.Add(new DataColumn("TOTALQTY", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalOKQty", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalNotOKQty", typeof(string)));
        dt.Columns.Add(new DataColumn("PolybagSize", typeof(string)));
        dt.Columns.Add(new DataColumn("PolybagGuage", typeof(string)));
        dt.Columns.Add(new DataColumn("PresenceOfRefSample", typeof(string)));
        dt.Columns.Add(new DataColumn("ThicknessAndStrength", typeof(string)));
        dt.Columns.Add(new DataColumn("LengthWidthDia", typeof(string)));
        dt.Columns.Add(new DataColumn("TransportConditionsAndDamages", typeof(string)));
        dt.Columns.Add(new DataColumn("RESULT", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["ArticleName"] = string.Empty;
        dr["ArticleSize"] = string.Empty;
        dr["TOTALQTY"] = string.Empty;
        dr["TotalOKQty"] = string.Empty;
        dr["TotalNotOKQty"] = string.Empty;
        dr["PolybagSize"] = string.Empty;
        dr["PolybagGuage"] = string.Empty;
        dr["PresenceOfRefSample"] = string.Empty;
        dr["ThicknessAndStrength"] = string.Empty;
        dr["LengthWidthDia"] = string.Empty;
        dr["TransportConditionsAndDamages"] = string.Empty;
        dr["RESULT"] = string.Empty;
        dt.Rows.Add(dr);

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
                    TextBox txtArticleName = (TextBox)Dgdetail.Rows[rowIndex].Cells[1].FindControl("txtArticleName");
                    TextBox txtArticleSize = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txtArticleSize");
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txttotalqty");
                    TextBox txtTotalOKQty = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtTotalOKQty");
                    TextBox txtTotalNotOKQty = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtTotalNotOKQty");
                    TextBox txtPolybagSize = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtPolybagSize");
                    TextBox txtPolybagGauge = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtPolybagGauge");
                    TextBox txtPresenceOfRefSample = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtPresenceOfRefSample");
                    TextBox txtThicknessAndStrength = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtThicknessAndStrength");
                    TextBox txtLengthWidthDia = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtLengthWidthDia");
                    TextBox txtTransportConditionsAndDamages = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtTransportConditionsAndDamages");
                    TextBox txtResult = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtResult");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["ArticleName"] = txtArticleName.Text;
                    dtCurrentTable.Rows[i - 1]["ArticleSize"] = txtArticleSize.Text;
                    dtCurrentTable.Rows[i - 1]["TOTALQTY"] = txttotalqty.Text;
                    dtCurrentTable.Rows[i - 1]["TotalOKQty"] = txtTotalOKQty.Text;
                    dtCurrentTable.Rows[i - 1]["TotalNotOKQty"] = txtTotalNotOKQty.Text;
                    dtCurrentTable.Rows[i - 1]["PolybagSize"] = txtPolybagSize.Text;
                    dtCurrentTable.Rows[i - 1]["PolybagGuage"] = txtPolybagGauge.Text;
                    dtCurrentTable.Rows[i - 1]["PresenceOfRefSample"] = txtPresenceOfRefSample.Text;
                    dtCurrentTable.Rows[i - 1]["ThicknessAndStrength"] = txtThicknessAndStrength.Text;
                    dtCurrentTable.Rows[i - 1]["LengthWidthDia"] = txtLengthWidthDia.Text;
                    dtCurrentTable.Rows[i - 1]["TransportConditionsAndDamages"] = txtTransportConditionsAndDamages.Text;
                    dtCurrentTable.Rows[i - 1]["RESULT"] = txtResult.Text;


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
                    TextBox txtArticleName = (TextBox)Dgdetail.Rows[rowIndex].Cells[1].FindControl("txtArticleName");
                    TextBox txtArticleSize = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txtArticleSize");
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txttotalqty");
                    TextBox txtTotalOKQty = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtTotalOKQty");
                    TextBox txtTotalNotOKQty = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtTotalNotOKQty");
                    TextBox txtPolybagSize = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtPolybagSize");
                    TextBox txtPolybagGauge = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtPolybagGauge");
                    TextBox txtPresenceOfRefSample = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtPresenceOfRefSample");
                    TextBox txtThicknessAndStrength = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtThicknessAndStrength");
                    TextBox txtLengthWidthDia = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtLengthWidthDia");
                    TextBox txtTransportConditionsAndDamages = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtTransportConditionsAndDamages");
                    TextBox txtResult = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtResult");


                    txtsrno.Text = dt.Rows[i]["SRNO"].ToString();
                    txtArticleName.Text = dt.Rows[i]["ArticleName"].ToString();
                    txtArticleSize.Text = dt.Rows[i]["ArticleSize"].ToString();
                    txttotalqty.Text = dt.Rows[i]["TOTALQTY"].ToString();
                    txtTotalOKQty.Text = dt.Rows[i]["TotalOKQty"].ToString();
                    txtTotalNotOKQty.Text = dt.Rows[i]["TotalNotOKQty"].ToString();
                    txtPolybagSize.Text = dt.Rows[i]["PolybagSize"].ToString();
                    txtPolybagGauge.Text = dt.Rows[i]["PolybagGuage"].ToString();
                    txtPresenceOfRefSample.Text = dt.Rows[i]["PresenceOfRefSample"].ToString();
                    txtThicknessAndStrength.Text = dt.Rows[i]["ThicknessAndStrength"].ToString();
                    txtLengthWidthDia.Text = dt.Rows[i]["LengthWidthDia"].ToString();
                    txtTransportConditionsAndDamages.Text = dt.Rows[i]["TransportConditionsAndDamages"].ToString();
                    txtResult.Text = dt.Rows[i]["RESULT"].ToString();

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
            SqlParameter[] param = new SqlParameter[16];
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
            param[7] = new SqlParameter("@TypeOfMaterial", txtTypeOfMaterial.Text);
            param[8] = new SqlParameter("@StrenghtGauge", txtStrenghtGauge.Text);
            param[9] = new SqlParameter("@SamplingPlan", txtSamplingPlan.Text);
            param[10] = new SqlParameter("@SUPPLIERNAME", txtsuppliername.Text);
            param[11] = new SqlParameter("@challanNodate", txtchallannodate.Text);
            param[12] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            param[13] = new SqlParameter("@InvoiceNo", txtInvoiceNo.Text);
            param[14] = new SqlParameter("@AcceptedAreaStatus", DDAcceptedArea.SelectedValue);
            param[15] = new SqlParameter("@RejectedAreaStatus", DDRejectedArea.SelectedValue);

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVEPOLYBAGINSPECTIONKaysons]", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM POLYBAGINSPECTIONDETAILKaysons WHERE DOCID=" + hndocid.Value;
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
                TextBox txtArticleName = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtArticleName");
                TextBox txtArticleSize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtArticleSize");
                TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txttotalqty");
                TextBox txtTotalOKQty = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtTotalOKQty");
                TextBox txtTotalNotOKQty = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtTotalNotOKQty");
                TextBox txtPolybagSize = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtPolybagSize");
                TextBox txtPolybagGauge = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtPolybagGauge");
                TextBox txtPresenceOfRefSample = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtPresenceOfRefSample");
                TextBox txtThicknessAndStrength = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtThicknessAndStrength");
                TextBox txtLengthWidthDia = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtLengthWidthDia");
                TextBox txtTransportConditionsAndDamages = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtTransportConditionsAndDamages");
                TextBox txtResult = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtResult");

                if (txtsrno.Text != "")
                {
                    str = str + @"  Insert into POLYBAGINSPECTIONDETAILKaysons(Docid,Srno,ArticleName,ArticleSize,totalqty,TotalOKQty,
                                TotalNotOKQty,PolybagSize,PolybagGuage,PresenceOfRefSample,ThicknessAndStrength,LengthWidthDia,
                                TransportConditionsAndDamages,Result) values ("
                             + hndocid.Value + ",'" + txtsrno.Text + "','"
                             + txtArticleName.Text.Replace("'", "''") + "','"
                             + txtArticleSize.Text.Replace("'", "''") + "','"
                             + txttotalqty.Text.Replace("'", "''") + "','"
                             + txtTotalOKQty.Text.Replace("'", "''") + "','"
                             + txtTotalNotOKQty.Text.Replace("'", "''") + "','"
                             + txtPolybagSize.Text.Replace("'", "''") + "','"
                             + txtPolybagGauge.Text.Replace("'", "''") + "','"
                             + txtPresenceOfRefSample.Text.Replace("'", "''") + "','" 
                             + txtThicknessAndStrength.Text.Replace("'", "''") + "','"
                             + txtLengthWidthDia.Text.Replace("'", "''") + "','"
                             + txtTransportConditionsAndDamages.Text.Replace("'", "''") + "','"
                             + txtResult.Text.Replace("'", "''") + "')";
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

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_GETPOLYBAGINSPECTIONREPORTKaysons]", param);

            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["rptFileName"] = "~\\Reports\\rptPolyBaginspectionKaysons.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptPolyBaginspectionKaysons.xsd";
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
            param[2] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPOLYBAGINSPECTIONKaysons", param);
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
            param[2] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_APPROVEPOLYBAGINSPECTIONKaysons]", param);
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
        txtSamplingPlan.Text = "";
        txtStrenghtGauge.Text = "";
        txtTypeOfMaterial.Text = "";
        SetInitialRow();
    }
    private void fillDocno()
    {
        string str = @"SELECT Distinct RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo FROM POLYBAGINSPECTIONMASTERKaysons RIM INNER JOIN POLYBAGINSPECTIONDETAILKaysons RID ON RIM.DOCID=RID.DOCID
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
        string str = @"SELECT * FROM POLYBAGINSPECTIONMASTERKaysons RIM Where RIM.DocId=" + hndocid.Value + @"
                       SELECT * FROM POLYBAGINSPECTIONDETAILKaysons RID Where RID.DocId=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["docNo"].ToString();
            txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Reportdate"]).ToString("dd-MMM-yyyy");
            txtTypeOfMaterial.Text = ds.Tables[0].Rows[0]["TypeOfMaterial"].ToString();
            txtSamplingPlan.Text = ds.Tables[0].Rows[0]["SamplingPlan"].ToString();
            txtStrenghtGauge.Text = ds.Tables[0].Rows[0]["StrenghtGauge"].ToString();
            txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["suppliername"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["challanNodate"].ToString();
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();

            if (DDAcceptedArea.Items.FindByText(ds.Tables[0].Rows[0]["AcceptedAreaStatus"].ToString()) != null)
            {
                DDAcceptedArea.SelectedValue = ds.Tables[0].Rows[0]["AcceptedAreaStatus"].ToString();
            }
            if (DDRejectedArea.Items.FindByText(ds.Tables[0].Rows[0]["RejectedAreaStatus"].ToString()) != null)
            {
                DDRejectedArea.SelectedValue = ds.Tables[0].Rows[0]["RejectedAreaStatus"].ToString();
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
                        TextBox txtArticleName = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtArticleName");
                        TextBox txtArticleSize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtArticleSize");
                        TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txttotalqty");
                        TextBox txtTotalOKQty = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtTotalOKQty");
                        TextBox txtTotalNotOKQty = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtTotalNotOKQty");
                        TextBox txtPolybagSize = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtPolybagSize");
                        TextBox txtPolybagGauge = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtPolybagGauge");
                        TextBox txtPresenceOfRefSample = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtPresenceOfRefSample");
                        TextBox txtThicknessAndStrength = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtThicknessAndStrength");
                        TextBox txtLengthWidthDia = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtLengthWidthDia");
                        TextBox txtTransportConditionsAndDamages = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtTransportConditionsAndDamages");
                        TextBox txtResult = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtResult");


                        txtsrno.Text = ds.Tables[1].Rows[i]["SRNO"].ToString();
                        txtArticleName.Text = ds.Tables[1].Rows[i]["ArticleName"].ToString();
                        txtArticleSize.Text = ds.Tables[1].Rows[i]["ArticleSize"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["TOTALQTY"].ToString();
                        txtTotalOKQty.Text = ds.Tables[1].Rows[i]["TotalOKQty"].ToString();
                        txtTotalNotOKQty.Text = ds.Tables[1].Rows[i]["TotalNotOKQty"].ToString();
                        txtPolybagSize.Text = ds.Tables[1].Rows[i]["PolybagSize"].ToString();
                        txtPolybagGauge.Text = ds.Tables[1].Rows[i]["PolybagGuage"].ToString();
                        txtPresenceOfRefSample.Text = ds.Tables[1].Rows[i]["PresenceOfRefSample"].ToString();
                        txtThicknessAndStrength.Text = ds.Tables[1].Rows[i]["ThicknessAndStrength"].ToString();
                        txtLengthWidthDia.Text = ds.Tables[1].Rows[i]["LengthWidthDia"].ToString();
                        txtTransportConditionsAndDamages.Text = ds.Tables[1].Rows[i]["TransportConditionsAndDamages"].ToString();
                        txtResult.Text = ds.Tables[1].Rows[i]["RESULT"].ToString();
                    }
                    else
                    {
                        AddNewRowToGrid();

                        TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                        TextBox txtArticleName = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtArticleName");
                        TextBox txtArticleSize = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txtArticleSize");
                        TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txttotalqty");
                        TextBox txtTotalOKQty = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtTotalOKQty");
                        TextBox txtTotalNotOKQty = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtTotalNotOKQty");
                        TextBox txtPolybagSize = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtPolybagSize");
                        TextBox txtPolybagGauge = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtPolybagGauge");
                        TextBox txtPresenceOfRefSample = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtPresenceOfRefSample");
                        TextBox txtThicknessAndStrength = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtThicknessAndStrength");
                        TextBox txtLengthWidthDia = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtLengthWidthDia");
                        TextBox txtTransportConditionsAndDamages = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtTransportConditionsAndDamages");
                        TextBox txtResult = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtResult");

                        txtsrno.Text = ds.Tables[1].Rows[i]["SRNO"].ToString();
                        txtArticleName.Text = ds.Tables[1].Rows[i]["ArticleName"].ToString();
                        txtArticleSize.Text = ds.Tables[1].Rows[i]["ArticleSize"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["TOTALQTY"].ToString();
                        txtTotalOKQty.Text = ds.Tables[1].Rows[i]["TotalOKQty"].ToString();
                        txtTotalNotOKQty.Text = ds.Tables[1].Rows[i]["TotalNotOKQty"].ToString();
                        txtPolybagSize.Text = ds.Tables[1].Rows[i]["PolybagSize"].ToString();
                        txtPolybagGauge.Text = ds.Tables[1].Rows[i]["PolybagGuage"].ToString();
                        txtPresenceOfRefSample.Text = ds.Tables[1].Rows[i]["PresenceOfRefSample"].ToString();
                        txtThicknessAndStrength.Text = ds.Tables[1].Rows[i]["ThicknessAndStrength"].ToString();
                        txtLengthWidthDia.Text = ds.Tables[1].Rows[i]["LengthWidthDia"].ToString();
                        txtTransportConditionsAndDamages.Text = ds.Tables[1].Rows[i]["TransportConditionsAndDamages"].ToString();
                        txtResult.Text = ds.Tables[1].Rows[i]["RESULT"].ToString();
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