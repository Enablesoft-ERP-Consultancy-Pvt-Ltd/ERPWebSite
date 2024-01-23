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
                    Where CI.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName 
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

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, " Select customerid,customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by customercode", true, "--Select--");

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
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        if (chkedit.Checked == true)
        {
            string str3 = @"Select Distinct OM.OrderId,OM.CustomerOrderNo FROM LABELINSPECTIONMASTER RIM INNER JOIN LABELINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
                        LEFT JOIN OrderMaster OM ON RIM.OrderId=OM.OrderId 
                        Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue + "";
            if (DDCustomerCode.SelectedIndex > 0)
            {
                str3 = str3 + " and RIM.CustomerId=" + DDCustomerCode.SelectedValue + "";
            }
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str3, true, "--Select--");
        }
        else
        {
            string str = @"Select Distinct Orderid, CustomerOrderNo from OrderMaster Where CustomerID=" + DDCustomerCode.SelectedValue + " And CompanyId=" + DDcompanyName.SelectedValue + " and Status=0";

            str = str + "  Order by CustomerOrderNo";
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--Select--");
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hndocid.Value = "0";
        if (chkedit.Checked == true)
        {
            fillDocno();
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
        dt.Columns.Add(new DataColumn("SampleSize", typeof(string)));//Sample Plan
        dt.Columns.Add(new DataColumn("QTYReceived", typeof(string)));
        dt.Columns.Add(new DataColumn("QTYInspection", typeof(string)));
        dt.Columns.Add(new DataColumn("Color", typeof(string)));
        dt.Columns.Add(new DataColumn("Size", typeof(string)));
        dt.Columns.Add(new DataColumn("ConformedQTY", typeof(string)));
        dt.Columns.Add(new DataColumn("NonConformingQTY", typeof(string)));
        dt.Columns.Add(new DataColumn("DefectiveAllowed", typeof(string)));
        dt.Columns.Add(new DataColumn("SampleLabelAvailable", typeof(string)));
        dt.Columns.Add(new DataColumn("IsPrintingClear", typeof(string)));
        dt.Columns.Add(new DataColumn("AreLabelsSeparatelyPacked", typeof(string)));
        dt.Columns.Add(new DataColumn("IsLabelSizeFinishingOK", typeof(string)));
        dt.Columns.Add(new DataColumn("AnySpellingMistakeInPrinting", typeof(string)));
        dt.Columns.Add(new DataColumn("IsBarcodeScanAble", typeof(string)));
        dt.Columns.Add(new DataColumn("Acceptance", typeof(string)));
        dt.Columns.Add(new DataColumn("Lotresult", typeof(string)));

        dr = dt.NewRow();
        dr["Srno"] = 1;
        dr["Description"] = string.Empty;
        dr["Totalqty"] = string.Empty;
        dr["Samplesize"] = string.Empty;
        dr["QTYReceived"] = string.Empty;
        dr["QTYInspection"] = string.Empty;
        dr["Color"] = string.Empty;
        dr["Size"] = string.Empty;
        dr["ConformedQTY"] = string.Empty;
        dr["NonConformingQTY"] = string.Empty;
        dr["DefectiveAllowed"] = string.Empty;
        dr["SampleLabelAvailable"] = string.Empty;
        dr["IsPrintingClear"] = string.Empty;
        dr["AreLabelsSeparatelyPacked"] = string.Empty;
        dr["IsLabelSizeFinishingOK"] = string.Empty;
        dr["AnySpellingMistakeInPrinting"] = string.Empty;
        dr["IsBarcodeScanAble"] = string.Empty;
        dr["Acceptance"] = string.Empty;
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
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txttotalqty");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsamplesize");
                    TextBox txtQTYReceived = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtQTYReceived");
                    TextBox txtQTYInspection = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtQTYInspection");
                    TextBox txtcolor = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtcolor");
                    TextBox txtsize = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtsize");
                    TextBox txtConformedQTY = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtConformedQTY");
                    TextBox txtNonConformingQTY = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtNonConformingQTY");
                    TextBox txtDefectiveAllowed = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtDefectiveAllowed");
                    TextBox txtSampleLabelAvailable = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtSampleLabelAvailable");
                    TextBox txtIsPrintingClear = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtIsPrintingClear");
                    TextBox txtAreLabelsSeparatelyPacked = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtAreLabelsSeparatelyPacked");
                    TextBox txtIsLabelSizeFinishingOK = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtIsLabelSizeFinishingOK");
                    TextBox txtAnySpellingMistakeInPrinting = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtAnySpellingMistakeInPrinting");
                    TextBox txtIsBarcodeScanAble = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtIsBarcodeScanAble");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[17].FindControl("txtacceptance");
                    DropDownList ddresult = (DropDownList)Dgdetail.Rows[rowIndex].Cells[18].FindControl("ddresult");
                    Label lbllotresult = (Label)Dgdetail.Rows[rowIndex].FindControl("lbllotresult");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["srno"] = i + 1;
                    drCurrentRow["Lotresult"] = "Pass";

                    dtCurrentTable.Rows[i - 1]["description"] = txtdescription.Text;
                    dtCurrentTable.Rows[i - 1]["totalqty"] = txttotalqty.Text;
                    dtCurrentTable.Rows[i - 1]["samplesize"] = txtsamplesize.Text;
                    dtCurrentTable.Rows[i - 1]["QTYReceived"] = txtQTYReceived.Text;
                    dtCurrentTable.Rows[i - 1]["QTYInspection"] = txtQTYInspection.Text;
                    dtCurrentTable.Rows[i - 1]["Color"] = txtcolor.Text;
                    dtCurrentTable.Rows[i - 1]["Size"] = txtsize.Text;
                    dtCurrentTable.Rows[i - 1]["ConformedQTY"] = txtConformedQTY.Text;
                    dtCurrentTable.Rows[i - 1]["NonConformingQTY"] = txtNonConformingQTY.Text;
                    dtCurrentTable.Rows[i - 1]["DefectiveAllowed"] = txtDefectiveAllowed.Text;
                    dtCurrentTable.Rows[i - 1]["SampleLabelAvailable"] = txtSampleLabelAvailable.Text;
                    dtCurrentTable.Rows[i - 1]["IsPrintingClear"] = txtIsPrintingClear.Text;
                    dtCurrentTable.Rows[i - 1]["AreLabelsSeparatelyPacked"] = txtAreLabelsSeparatelyPacked.Text;
                    dtCurrentTable.Rows[i - 1]["IsLabelSizeFinishingOK"] = txtIsLabelSizeFinishingOK.Text;
                    dtCurrentTable.Rows[i - 1]["AnySpellingMistakeInPrinting"] = txtAnySpellingMistakeInPrinting.Text;
                    dtCurrentTable.Rows[i - 1]["IsBarcodeScanAble"] = txtIsBarcodeScanAble.Text;
                    dtCurrentTable.Rows[i - 1]["Acceptance"] = txtacceptance.Text;
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
                    TextBox txttotalqty = (TextBox)Dgdetail.Rows[rowIndex].Cells[2].FindControl("txttotalqty");
                    TextBox txtsamplesize = (TextBox)Dgdetail.Rows[rowIndex].Cells[3].FindControl("txtsamplesize");
                    TextBox txtQTYReceived = (TextBox)Dgdetail.Rows[rowIndex].Cells[4].FindControl("txtQTYReceived");
                    TextBox txtQTYInspection = (TextBox)Dgdetail.Rows[rowIndex].Cells[5].FindControl("txtQTYInspection");
                    TextBox txtcolor = (TextBox)Dgdetail.Rows[rowIndex].Cells[6].FindControl("txtcolor");
                    TextBox txtSize = (TextBox)Dgdetail.Rows[rowIndex].Cells[7].FindControl("txtSize");
                    TextBox txtConformedQTY = (TextBox)Dgdetail.Rows[rowIndex].Cells[8].FindControl("txtConformedQTY");
                    TextBox txtNonConformingQTY = (TextBox)Dgdetail.Rows[rowIndex].Cells[9].FindControl("txtNonConformingQTY");
                    TextBox txtDefectiveAllowed = (TextBox)Dgdetail.Rows[rowIndex].Cells[10].FindControl("txtDefectiveAllowed");
                    TextBox txtSampleLabelAvailable = (TextBox)Dgdetail.Rows[rowIndex].Cells[11].FindControl("txtSampleLabelAvailable");
                    TextBox txtIsPrintingClear = (TextBox)Dgdetail.Rows[rowIndex].Cells[12].FindControl("txtIsPrintingClear");
                    TextBox txtAreLabelsSeparatelyPacked = (TextBox)Dgdetail.Rows[rowIndex].Cells[13].FindControl("txtAreLabelsSeparatelyPacked");
                    TextBox txtIsLabelSizeFinishingOK = (TextBox)Dgdetail.Rows[rowIndex].Cells[14].FindControl("txtIsLabelSizeFinishingOK");
                    TextBox txtAnySpellingMistakeInPrinting = (TextBox)Dgdetail.Rows[rowIndex].Cells[15].FindControl("txtAnySpellingMistakeInPrinting");
                    TextBox txtIsBarcodeScanAble = (TextBox)Dgdetail.Rows[rowIndex].Cells[16].FindControl("txtIsBarcodeScanAble");
                    TextBox txtacceptance = (TextBox)Dgdetail.Rows[rowIndex].Cells[17].FindControl("txtacceptance");
                    DropDownList ddresult = (DropDownList)Dgdetail.Rows[rowIndex].Cells[18].FindControl("ddresult");

                    txtsrno.Text = dt.Rows[i]["srno"].ToString();
                    txtdescription.Text = dt.Rows[i]["Description"].ToString();
                    txttotalqty.Text = dt.Rows[i]["totalqty"].ToString();
                    txtsamplesize.Text = dt.Rows[i]["samplesize"].ToString();
                    txtQTYReceived.Text = dt.Rows[i]["QTYReceived"].ToString();
                    txtQTYInspection.Text = dt.Rows[i]["QTYInspection"].ToString();
                    txtcolor.Text = dt.Rows[i]["color"].ToString();
                    txtSize.Text = dt.Rows[i]["Size"].ToString();
                    txtConformedQTY.Text = dt.Rows[i]["ConformedQTY"].ToString();
                    txtNonConformingQTY.Text = dt.Rows[i]["NonConformingQTY"].ToString();
                    txtDefectiveAllowed.Text = dt.Rows[i]["DefectiveAllowed"].ToString();
                    txtSampleLabelAvailable.Text = dt.Rows[i]["SampleLabelAvailable"].ToString();
                    txtIsPrintingClear.Text = dt.Rows[i]["IsPrintingClear"].ToString();
                    txtAreLabelsSeparatelyPacked.Text = dt.Rows[i]["AreLabelsSeparatelyPacked"].ToString();
                    txtIsLabelSizeFinishingOK.Text = dt.Rows[i]["IsLabelSizeFinishingOK"].ToString();
                    txtAnySpellingMistakeInPrinting.Text = dt.Rows[i]["AnySpellingMistakeInPrinting"].ToString();
                    txtIsBarcodeScanAble.Text = dt.Rows[i]["IsBarcodeScanAble"].ToString();
                    txtacceptance.Text = dt.Rows[i]["acceptance"].ToString();
                    foreach (ListItem item in ddresult.Items)
                    {
                        if (string.Equals(item.Value, dt.Rows[i]["lotresult"].ToString()))
                        {
                            item.Selected = true;
                        }
                    }

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
            SqlParameter[] param = new SqlParameter[15];
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
            param[7] = new SqlParameter("@standared", txtstandared.Text);
            param[8] = new SqlParameter("@suppliername", txtsuppliername.Text);
            param[9] = new SqlParameter("@challanNodate", txtchallannodate.Text);
            param[10] = new SqlParameter("@Size", txtsize.Text);
            param[11] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);
            param[12] = new SqlParameter("@OrderID", DDCustomerOrderNo.SelectedValue);
            param[13] = new SqlParameter("@CustomerID", DDCustomerCode.SelectedValue);
            param[14] = new SqlParameter("@Week", txtWeek.Text);

            //*********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_SAVELABLEINSPECTION]", param);
            lblmsg.Text = param[4].Value.ToString();
            txtdocno.Text = param[2].Value.ToString();
            hndocid.Value = param[0].Value.ToString();

            // at the time of update delete all the data in tables
            string str1 = @"DELETE FROM LabelInspectionDetail WHERE DOCID=" + hndocid.Value;
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
                TextBox txtQTYReceived = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtQTYReceived");
                TextBox txtQTYInspection = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtQTYInspection");
                TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtcolor");
                TextBox txtSize = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtSize");
                TextBox txtConformedQTY = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtConformedQTY");
                TextBox txtNonConformingQTY = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtNonConformingQTY");
                TextBox txtDefectiveAllowed = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtDefectiveAllowed");
                TextBox txtSampleLabelAvailable = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtSampleLabelAvailable");
                TextBox txtIsPrintingClear = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtIsPrintingClear");
                TextBox txtAreLabelsSeparatelyPacked = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtAreLabelsSeparatelyPacked");
                TextBox txtIsLabelSizeFinishingOK = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtIsLabelSizeFinishingOK");
                TextBox txtAnySpellingMistakeInPrinting = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtAnySpellingMistakeInPrinting");
                TextBox txtIsBarcodeScanAble = (TextBox)Dgdetail.Rows[i].Cells[16].FindControl("txtIsBarcodeScanAble");
                TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[17].FindControl("txtacceptance");
                DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[18].FindControl("ddresult");

                if (txtsrno.Text != "")
                {
                    str = str + @" Insert into LabelInspectionDetail(Docid,Srno,Description,totalqty,samplesize,QTYReceived,QTYInspection,Color,Size,ConformedQTY,NonConformingQTY,DefectiveAllowed,SampleLabelAvailable,IsPrintingClear,AreLabelsSeparatelyPacked,IsLabelSizeFinishingOK,AnySpellingMistakeInPrinting,IsBarcodeScanAble,Acceptance,Lotresult)
                   values (" + hndocid.Value + ",'" + txtsrno.Text + "','" + txtdescription.Text.Replace("'", "''") + "','" + txttotalqty.Text.Replace("'", "''") + @"','" + txtsamplesize.Text.Replace("'", "''")
                   + "','" + txtQTYReceived.Text.Replace("'", "''") + "','" + txtQTYInspection.Text.Replace("'", "''") + "','" + txtcolor.Text.Replace("'", "''") + "','" + txtSize.Text.Replace("'", "''") + "','" + txtConformedQTY.Text.Replace("'", "''") 
                   + @"','" + txtNonConformingQTY.Text.Replace("'", "''") + "','" + txtDefectiveAllowed.Text.Replace("'", "''") + "','" + txtSampleLabelAvailable.Text.Replace("'", "''") + "','" + txtIsPrintingClear.Text.Replace("'", "''") 
                   + "','" + txtAreLabelsSeparatelyPacked.Text.Replace("'", "''") + "','" + txtIsLabelSizeFinishingOK.Text.Replace("'", "''") + "','" + txtAnySpellingMistakeInPrinting.Text.Replace("'", "''") + "','" + txtIsBarcodeScanAble.Text.Replace("'", "''")
                   + "','" + txtacceptance.Text.Replace("'", "''") + "','" + ddresult.SelectedItem.Text.Replace("'", "''") + "')";
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

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_GETLABELINSPECTIONREPORT]", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\rptLabelinspectionKaysons.rpt";
                Session["Getdataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptLabelinspectionKaysons.xsd";
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_DELETELABELINSPECTION]", param);
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

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_APPROVELABELINSPECTION]", param);
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
        TDCustomerOrderNo.Visible = false;
        TDSupplierSearch.Visible = false;
        TDDocno.Visible = false;
        hndocid.Value = "0";
        DDDocNo.Items.Clear();
        refreshcontrol();
        if (chkedit.Checked == true)
        {
            TDDocno.Visible = true;
            TDSupplierSearch.Visible = true;
            TDCustomerOrderNo.Visible = true;

            fillOrderNo();
            fillDocno();

        }
    }
    private void refreshcontrol()
    {
        txtsuppliername.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtchallannodate.Text = "";
        txtstandared.Text = "";
        txtWeek.Text = "";
        txtsize.Text = "";
        SetInitialRow();
    }
    private void fillOrderNo()
    {
        string str2 = @"Select Distinct CI.customerid,CI.CustomerCode FROM LABELINSPECTIONMASTER RIM INNER JOIN LABELINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
                        LEFT JOIN CustomerInfo CI ON RIM.CustomerId=CI.CustomerId
                        LEFT JOIN OrderMaster OM ON RIM.OrderId=OM.OrderID
                        Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue + " and CI.CustomerCode<>''";
        if (txtsuppliersearch.Text != "")
        {
            str2 = str2 + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtCustomerOrderNo.Text != "")
        {
            str2 = str2 + " and OM.CustomerOrderNo like '" + txtCustomerOrderNo.Text.Trim() + "%'";
        }

        UtilityModule.ConditionalComboFill(ref DDCustomerCode, str2, true, "--Select--");

        string str3 = @"Select Distinct OM.OrderId,OM.CustomerOrderNo FROM LABELINSPECTIONMASTER RIM INNER JOIN LABELINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID 
                        LEFT JOIN OrderMaster OM ON RIM.OrderId=OM.OrderId 
                        Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue + " and OM.CustomerOrderNo<>''";
        if (txtsuppliersearch.Text != "")
        {
            str3 = str3 + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtCustomerOrderNo.Text != "")
        {
            str3 = str3 + " and OM.CustomerOrderNo like '" + txtCustomerOrderNo.Text.Trim() + "%'";
        }

        UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str3, true, "--Select--");


        string str = @"SELECT Distinct isnull(RIM.OrderId,0) as OrderId,isnull(CI.CustomerId,0) as CustomerId FROM LABELINSPECTIONMASTER RIM INNER JOIN LABELINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID                       
                       LEFT JOIN CustomerInfo CI ON RIM.CustomerID=CI.CustomerId
                       LEFT JOIN OrderMaster OM ON RIM.OrderId=OM.OrderID
                      Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue;
        if (txtsuppliersearch.Text != "")
        {
            str = str + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtCustomerOrderNo.Text != "")
        {
            str = str + " and OM.CustomerOrderNo like '" + txtCustomerOrderNo.Text.Trim() + "%'";
        }

        //str = str + " order by DOCID";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
            DDCustomerOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderId"].ToString();
        }
        else
        {
            DDCustomerCode.SelectedValue = "0";
            DDCustomerOrderNo.SelectedValue = "0";
        }

    }
    private void fillDocno()
    {
        string str = @"SELECT Distinct RIM.DOCID,RIM.DOCNO +' # ' +Replace(convert(nvarchar(11),RIM.Reportdate,106),' ','-') as DocNo 
                        FROM LABELINSPECTIONMASTER RIM INNER JOIN LABELINSPECTIONDETAIL RID ON RIM.DOCID=RID.DOCID
                        LEFT JOIN OrderMaster OM ON RIM.OrderId=OM.OrderID
                      Where RIM.COMPANYID=" + DDcompanyName.SelectedValue + " And RIM.BranchID = " + DDBranchName.SelectedValue;
        if (txtsuppliersearch.Text != "")
        {
            str = str + " and RIM.Suppliername like '" + txtsuppliersearch.Text.Trim() + "%'";
        }
        if (txtCustomerOrderNo.Text != "")
        {
            str = str + " and OM.CustomerOrderNo like '" + txtCustomerOrderNo.Text.Trim() + "%'";
        }
        if (DDCustomerOrderNo.SelectedIndex > 0)
        {
            str = str + " and RIM.OrderId=" + DDCustomerOrderNo.SelectedValue + "";
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
        string str = @"SELECT * FROM LABELINSPECTIONMASTER RIM Where RIM.DocId=" + hndocid.Value + @"
                       SELECT * FROM LABELINSPECTIONDETAIL RID Where RID.DocId=" + hndocid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdocno.Text = ds.Tables[0].Rows[0]["docNo"].ToString();
            txtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Reportdate"]).ToString("dd-MMM-yyyy");
            txtstandared.Text = ds.Tables[0].Rows[0]["standared"].ToString();
            txtsize.Text = ds.Tables[0].Rows[0]["Size"].ToString();
            txtsuppliername.Text = ds.Tables[0].Rows[0]["suppliername"].ToString();
            txtchallannodate.Text = ds.Tables[0].Rows[0]["challanNodate"].ToString();
            txtcomments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
            txtWeek.Text = ds.Tables[0].Rows[0]["Week"].ToString();

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
                        TextBox txtQTYReceived = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtQTYReceived");
                        TextBox txtQTYInspection = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtQTYInspection");
                        TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtcolor");
                        TextBox txtSize = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtSize");
                        TextBox txtConformedQTY = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtConformedQTY");
                        TextBox txtNonConformingQTY = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtNonConformingQTY");
                        TextBox txtDefectiveAllowed = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtDefectiveAllowed");
                        TextBox txtSampleLabelAvailable = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtSampleLabelAvailable");
                        TextBox txtIsPrintingClear = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtIsPrintingClear");
                        TextBox txtAreLabelsSeparatelyPacked = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtAreLabelsSeparatelyPacked");
                        TextBox txtIsLabelSizeFinishingOK = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtIsLabelSizeFinishingOK");
                        TextBox txtAnySpellingMistakeInPrinting = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtAnySpellingMistakeInPrinting");
                        TextBox txtIsBarcodeScanAble = (TextBox)Dgdetail.Rows[i].Cells[16].FindControl("txtIsBarcodeScanAble");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[17].FindControl("txtacceptance");
                        DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[18].FindControl("ddresult");


                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["totalqty"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtQTYReceived.Text = ds.Tables[1].Rows[i]["QTYReceived"].ToString();
                        txtQTYInspection.Text = ds.Tables[1].Rows[i]["QTYInspection"].ToString();
                        txtcolor.Text = ds.Tables[1].Rows[i]["color"].ToString();
                        txtSize.Text = ds.Tables[1].Rows[i]["Size"].ToString();
                        txtConformedQTY.Text = ds.Tables[1].Rows[i]["ConformedQTY"].ToString();
                        txtNonConformingQTY.Text = ds.Tables[1].Rows[i]["NonConformingQTY"].ToString();
                        txtDefectiveAllowed.Text = ds.Tables[1].Rows[i]["DefectiveAllowed"].ToString();
                        txtSampleLabelAvailable.Text = ds.Tables[1].Rows[i]["SampleLabelAvailable"].ToString();
                        txtIsPrintingClear.Text = ds.Tables[1].Rows[i]["IsPrintingClear"].ToString();
                        txtAreLabelsSeparatelyPacked.Text = ds.Tables[1].Rows[i]["AreLabelsSeparatelyPacked"].ToString();
                        txtIsLabelSizeFinishingOK.Text = ds.Tables[1].Rows[i]["IsLabelSizeFinishingOK"].ToString();
                        txtAnySpellingMistakeInPrinting.Text = ds.Tables[1].Rows[i]["AnySpellingMistakeInPrinting"].ToString();
                        txtIsBarcodeScanAble.Text = ds.Tables[1].Rows[i]["IsBarcodeScanAble"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        //ddresult.SelectedItem.Text = ds.Tables[1].Rows[i]["lotresult"].ToString();
                        foreach (ListItem item in ddresult.Items)
                        {
                            if (string.Equals(item.Value, ds.Tables[1].Rows[i]["lotresult"].ToString()))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        AddNewRowToGrid();

                        TextBox txtsrno = (TextBox)Dgdetail.Rows[i].Cells[0].FindControl("txtsrno");
                        TextBox txtdescription = (TextBox)Dgdetail.Rows[i].Cells[1].FindControl("txtdescription");
                        TextBox txttotalqty = (TextBox)Dgdetail.Rows[i].Cells[2].FindControl("txttotalqty");
                        TextBox txtsamplesize = (TextBox)Dgdetail.Rows[i].Cells[3].FindControl("txtsamplesize");
                        TextBox txtQTYReceived = (TextBox)Dgdetail.Rows[i].Cells[4].FindControl("txtQTYReceived");
                        TextBox txtQTYInspection = (TextBox)Dgdetail.Rows[i].Cells[5].FindControl("txtQTYInspection");
                        TextBox txtcolor = (TextBox)Dgdetail.Rows[i].Cells[6].FindControl("txtcolor");
                        TextBox txtSize = (TextBox)Dgdetail.Rows[i].Cells[7].FindControl("txtSize");
                        TextBox txtConformedQTY = (TextBox)Dgdetail.Rows[i].Cells[8].FindControl("txtConformedQTY");
                        TextBox txtNonConformingQTY = (TextBox)Dgdetail.Rows[i].Cells[9].FindControl("txtNonConformingQTY");
                        TextBox txtDefectiveAllowed = (TextBox)Dgdetail.Rows[i].Cells[10].FindControl("txtDefectiveAllowed");
                        TextBox txtSampleLabelAvailable = (TextBox)Dgdetail.Rows[i].Cells[11].FindControl("txtSampleLabelAvailable");
                        TextBox txtIsPrintingClear = (TextBox)Dgdetail.Rows[i].Cells[12].FindControl("txtIsPrintingClear");
                        TextBox txtAreLabelsSeparatelyPacked = (TextBox)Dgdetail.Rows[i].Cells[13].FindControl("txtAreLabelsSeparatelyPacked");
                        TextBox txtIsLabelSizeFinishingOK = (TextBox)Dgdetail.Rows[i].Cells[14].FindControl("txtIsLabelSizeFinishingOK");
                        TextBox txtAnySpellingMistakeInPrinting = (TextBox)Dgdetail.Rows[i].Cells[15].FindControl("txtAnySpellingMistakeInPrinting");
                        TextBox txtIsBarcodeScanAble = (TextBox)Dgdetail.Rows[i].Cells[16].FindControl("txtIsBarcodeScanAble");
                        TextBox txtacceptance = (TextBox)Dgdetail.Rows[i].Cells[17].FindControl("txtacceptance");
                        DropDownList ddresult = (DropDownList)Dgdetail.Rows[i].Cells[18].FindControl("ddresult");


                        txtsrno.Text = ds.Tables[1].Rows[i]["srno"].ToString();
                        txtdescription.Text = ds.Tables[1].Rows[i]["description"].ToString();
                        txttotalqty.Text = ds.Tables[1].Rows[i]["totalqty"].ToString();
                        txtsamplesize.Text = ds.Tables[1].Rows[i]["samplesize"].ToString();
                        txtQTYReceived.Text = ds.Tables[1].Rows[i]["QTYReceived"].ToString();
                        txtQTYInspection.Text = ds.Tables[1].Rows[i]["QTYInspection"].ToString();
                        txtcolor.Text = ds.Tables[1].Rows[i]["color"].ToString();
                        txtSize.Text = ds.Tables[1].Rows[i]["Size"].ToString();
                        txtConformedQTY.Text = ds.Tables[1].Rows[i]["ConformedQTY"].ToString();
                        txtNonConformingQTY.Text = ds.Tables[1].Rows[i]["NonConformingQTY"].ToString();
                        txtDefectiveAllowed.Text = ds.Tables[1].Rows[i]["DefectiveAllowed"].ToString();
                        txtSampleLabelAvailable.Text = ds.Tables[1].Rows[i]["SampleLabelAvailable"].ToString();
                        txtIsPrintingClear.Text = ds.Tables[1].Rows[i]["IsPrintingClear"].ToString();
                        txtAreLabelsSeparatelyPacked.Text = ds.Tables[1].Rows[i]["AreLabelsSeparatelyPacked"].ToString();
                        txtIsLabelSizeFinishingOK.Text = ds.Tables[1].Rows[i]["IsLabelSizeFinishingOK"].ToString();
                        txtAnySpellingMistakeInPrinting.Text = ds.Tables[1].Rows[i]["AnySpellingMistakeInPrinting"].ToString();
                        txtIsBarcodeScanAble.Text = ds.Tables[1].Rows[i]["IsBarcodeScanAble"].ToString();
                        txtacceptance.Text = ds.Tables[1].Rows[i]["acceptance"].ToString();
                        //ddresult.SelectedItem.Text = ds.Tables[1].Rows[i]["lotresult"].ToString();
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
        fillOrderNo();
        fillDocno();
    }
    protected void btnSearchOrderNo_Click(object sender, EventArgs e)
    {

        fillOrderNo();
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