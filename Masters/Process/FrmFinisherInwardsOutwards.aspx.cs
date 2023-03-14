using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;

public partial class Masters_Process_FrmFinisherInwardsOutwards : System.Web.UI.Page
{
    protected static string Focus = "";
    static int GVReceiveCount = 0;
    static double FRate = 0;
    static int FRateid = 0;
    static double FRate2 = 0;
    static int FCalcOptionId = 0;
    static string name = "";
    static string mode = "Add";
    static string PenMode = "";
    static int PenProcessRecId = 0;
    static int PenProcessRecDetailId = 0;
    static int PenItemFinishedId = 0;
    static double RAmount = 0;
    static double RArea = 0;

    static string btnclickflag = "";

    static int lblIssuePcs = 0;
    static int lblProcessRecDetailId = 0;
    static int lblIItemId = 0;
    static int lblIQualityid = 0;
    static int Issue_Detail_Id = 0;
    static int lblFinisherJobId = 0;
    static int lblFinisherNameId = 0;
    static int lblIIssueOrderId = 0;
    static int lblProcessRecId = 0;
    static int lblStockNo = 0;
    static int NoOfRows = 0;

    static int StockNoMainBazaarChallanNo = 0;
    static int StockNoItemFinishedId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Focus != "")
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            sm.SetFocus(Focus);
        }

        if (!IsPostBack)
        {
            mode = "Add";
            ViewState["Process_Rec_Id"] = 0;
            ViewState["OldFJobName"] = 0;
            ViewState["OldFFinisherName"] = 0;
            ViewState["OldJobName"] = 0;
            ViewState["OldFinisherName"] = 0;
            ViewState["ChallanNo"] = 0;
            TxtTotalBalPcs.Enabled = false;
            txtTotalIssPcs.Enabled = false;
            txtTotalRecPcs.Enabled = false;

            string str = "select Distinct CI.CompanyId,CI.Companyname from CompanyInfo CI INNER JOIN Company_Authentication CA ON CI.CompanyId=CA.CompanyId Where CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname";
            str = str + " select process_name_id,process_name from process_name_master Where MasterCompanyId=" + Session["varCompanyId"] + " and ProcessType=1 and PROCESS_NAME_ID!=1 order by process_name ";
            str = str + " select unitid,unitname from unit where unitid in (1,2) ";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "--SELECT--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            ddprocess.Focus();
            ViewState["recid"] = 0;
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 1, true, "--SELECT--");

            string str1 = "select process_name_id,process_name from process_name_master Where  MasterCompanyId=" + Session["varCompanyId"] + " and ProcessType=1 and PROCESS_NAME_ID!=1 order by process_name";
            DataSet ds1 = SqlHelper.ExecuteDataset(str1);
            UtilityModule.ConditionalComboFillWithDS(ref DDIssueJobType, ds1, 0, true, "--SELECT--");

            TxtreceiveDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtIssueDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtRequiredDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyId"].ToString());

            hnorderid.Value = "0";
            StockNoMainBazaarChallanNo = 0;
            StockNoItemFinishedId = 0;
        }

    }
    private void ClearChangeCompEmpPro()
    {
        GVIssued.DataSource = null;
        GVIssued.DataBind();
        GVIssued.Visible = true;

        GVReceive.DataSource = null;
        GVReceive.DataBind();

        GVIssue.DataSource = null;
        GVIssue.DataBind();

        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();

        Trsave.Visible = false;
        txttotalpcsgrid.Text = "";

        LblErrorMessage.Text = "";
        ViewState["FinisherReceivePen"] = null;
        ViewState["Process_Rec_Id"] = 0;
        ViewState["process_rec_detail_id"] = 0;
        ViewState["OldFJobName"] = 0;
        ViewState["OldFFinisherName"] = 0;
        ViewState["OldJobName"] = 0;
        ViewState["OldFinisherName"] = 0;
        ViewState["ChallanNo"] = 0;
        txtTotalRecPcs.Text = "";
        txtTotalIssPcs.Text = "";
        lblLastReceiptNo.Text = "";

    }
    protected void CreatePenEdit()
    {
        ViewState["FinisherReceivePenEdit"] = null;
        DataTable dtrecordsPenEdit = new DataTable();

        if (ViewState["FinisherReceivePenEdit"] == null)
        {
            //DataTable dtrecords = new DataTable();
            dtrecordsPenEdit.Columns.Add("PenalityId", typeof(int));
            dtrecordsPenEdit.Columns.Add("PenalityName", typeof(string));
            dtrecordsPenEdit.Columns.Add("Qty", typeof(int));
            dtrecordsPenEdit.Columns.Add("Rate", typeof(float));
            dtrecordsPenEdit.Columns.Add("Amount", typeof(float));
            dtrecordsPenEdit.Columns.Add("PenalityType", typeof(string));

            ViewState["FinisherReceivePenEdit"] = dtrecordsPenEdit;
        }
        else
        {
            dtrecordsPenEdit = (DataTable)ViewState["FinisherReceivePenEdit"];
        }

        string sql = "";

        sql = @"select PM.PenalityId,PM.PenalityName, Qty,PM.Rate,Amount,PM.PenalityType from FinisherCarpetReceivePenality FCRP
        INNER JOIN PenalityMaster PM ON FCRP.PenalityId=PM.PenalityId and PM.PenalityWF='F' where Process_Rec_Id=" + ViewState["PenProcess_Rec_Id"] + " and Process_Rec_Detail_Id=" + ViewState["PenProcess_Rec_Detail_Id"] + "";
        sql = sql + "Order By PenalityName Desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["FinisherReceivePenEdit"] = ds.Tables[0];
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void ddprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChanges();

        ClearChangeCompEmpPro();
    }
    private void ProcessSelectedChanges()
    {
        string sql = "";
        ViewState["recid"] = 0;
        sql = @"Select Distinct EI.Empid,EI.EmpName From Empinfo EI,process_issue_master_" + ddprocess.SelectedValue + " PM Where EI.EmpId=PM.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "";
        //UtilityModule.ConditionalComboFill(ref ddemp, "Select Distinct EI.Empid,EI.EmpName From Empinfo EI,process_issue_master_" + ddprocess.SelectedValue + " PM Where EI.EmpId=PM.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by EI.EmpName", true, "--SELECT--");
        if (ddCompName.SelectedIndex > 0)
        {
            sql = sql + " And PM.CompanyId=" + ddCompName.SelectedValue;
        }
        sql = sql + "order by EI.EmpName";
        UtilityModule.ConditionalComboFill(ref ddemp, sql, true, "--SELECT--");

    }
    protected void ddemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanges();
        //ClearChangeCompEmpPro();
    }
    private void EmpSelectedChanges()
    {
        ViewState["recid"] = 0;

        if (ddprocess.SelectedIndex > 0 && ddemp.SelectedIndex > 0)
        {
            fillIssueGrid();
            //Fillstockno();
        }
        else
        {
            ClearChangeCompEmpPro();
        }
    }
    private void BindReceiveDataForUpdate()
    {
        LblErrorMessage.Text = "";
        if (TxtEditChallanNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, 
            @"select Process_Rec_Id,EmpId,ChallanNo,Companyid,ProcessId,replace(convert(nvarchar(13), ReceiveDate,106),'','-') as ReceiveDate 
            From VIEW_PROCESS_RECEIVE_MASTER Where ProcessId<>1 And CompanyId = " + ddCompName.SelectedValue + " And ChallanNo=" + TxtEditChallanNo.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtreceiveDate.Text = Ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
                ddprocess.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ProcessSelectedChanges();
                ddemp.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                EmpSelectedChanges();
                ViewState["Process_Rec_Id"] = Ds.Tables[0].Rows[0]["Process_Rec_Id"].ToString();

                ViewState["OldFJobName"] = 0;
                ViewState["OldFFinisherName"] = 0;

                ViewState["OldJobName"] = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ViewState["OldFinisherName"] = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                ViewState["ChallanNo"] = Ds.Tables[0].Rows[0]["ChallanNo"].ToString();

                mode = "Update";

                ddCompName.Enabled = false;
                ddprocess.Enabled = false;
                ddemp.Enabled = false;

            }
            else
            {
                //if (DDChallanNo.Items.Count > 0)
                //{
                //    DDChallanNo.SelectedIndex = 0;
                //    ChallanNoSelectedChange();
                //}
                ddCompName.SelectedIndex = 0;
                ddprocess.SelectedIndex = 0;
                ddemp.Items.Clear();
                GVIssued.DataSource = null;
                GVIssued.DataBind();

                GVReceive.DataSource = null;
                GVReceive.DataBind();

                GVIssue.DataSource = null;
                GVIssue.DataBind();

                TxtTotalBalPcs.Text = "";
                txtTotalIssPcs.Text = "";
                txtTotalRecPcs.Text = "";

                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Receipt No has been cancelled";
                //LblErrorMessage.Text = "Pls Enter Correct Receipt No";
                TxtEditChallanNo.Text = "";
                TxtEditChallanNo.Focus();
                mode = "Add";
                ViewState["Process_Rec_Id"] = 0;
                ViewState["OldFJobName"] = 0;
                ViewState["OldFFinisherName"] = 0;
                ViewState["OldJobName"] = 0;
                ViewState["OldFinisherName"] = 0;
                ViewState["ChallanNo"] = 0;
            }
        }
    }
    protected void TxtEditChallanNo_TextChanged(object sender, EventArgs e)
    {
        BindReceiveDataForUpdate();
        BindReceiveIssueGrid();
    }
    //protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    //{
    //    ViewState["recid"] = 0;       
    //}
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = 0;
        ClearChangeCompEmpPro();
        if (ddCompName.SelectedIndex > 0 && ddprocess.SelectedIndex > 0 && ddemp.SelectedIndex > 0)
        {
            EmpSelectedChanges();
        }
    }
    private void IssueJobTypeChanges()
    {
        string sql = "";
        ViewState["recid"] = 0;
        //sql = @"Select Distinct EI.Empid,EI.EmpName From Empinfo EI,process_issue_master_" + DDIssueJobType.SelectedValue + " PM Where EI.EmpId=PM.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "";
        //sql = sql + "order by EI.EmpName";

        sql = @"select EI.EmpId,EI.EmpName from EmpProcess EP INNER JOIN EmpInfo EI ON EP.EmpId=EI.EmpId  where Processid=" + DDIssueJobType.SelectedValue + "";
        sql = sql + "order by EI.EmpName";

        UtilityModule.ConditionalComboFill(ref DDIssueContractorName, sql, true, "--SELECT--");
    }
    protected void DDIssueJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        IssueJobTypeChanges();
    }
    protected void DDIssueContractorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //EmpSelectedChanges();
    }
    private DataSet fillIssueGrid()
    {
        string where = "";

        if (ddCompName.SelectedIndex > 0)
        {
            where = where + " and PIM.CompanyId=" + ddCompName.SelectedValue;

        }
        //if (ddemp.SelectedIndex > 0)
        //{
        //    where = where + " and PIM.Empid=" + ddemp.SelectedValue;
        //}       
        //where = where + " and PIM.AssignDate <='" + (TxtreceiveDate.Text == "" ? System.DateTime.Now.ToString("dd-MMM-yyyy") : TxtreceiveDate.Text) + "'";

        DataSet ds = new DataSet();
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[1] = new SqlParameter("@Date", SqlDbType.DateTime);
        array[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[3] = new SqlParameter("@Where", where);

        array[0].Value = Convert.ToInt32(ddemp.SelectedIndex <= 0 ? "0" : ddemp.SelectedValue);
        array[1].Value = TxtreceiveDate.Text;
        array[2].Value = ddprocess.SelectedValue;        

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherIssueDetails", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            GVIssued.DataSource = ds;
            GVIssued.DataBind();
            GVIssued.Visible = true;
            TxtTotalBalPcs.Text = Convert.ToString(Convert.ToDouble(ds.Tables[0].Compute("sum(ORDEREDQTY)", "")) - Convert.ToDouble(ds.Tables[0].Compute("sum(RECEIVEDQTY)", "")));           

            //TxtTotalBalPcs.Text = ds.Tables[0].Compute("sum(HBalPcs)", "").ToString();
        }
        else
        {
            GVIssued.Visible = false;
            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        return ds;
    }
    protected void GVIssued_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        //GVIssued.SelectedRow.BackColor = System.Drawing.Color.Green;  

        //int r = Convert.ToInt32(GVIssued.SelectedIndex.ToString());
        //if (r >= 0)
        //{
        //    string lblbazaarchallanno = ((Label)GVIssued.Rows[r].FindControl("lblBZRepNo")).Text;
        //}
    }
    protected void GVIssued_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVIssued, "Select$" + e.Row.RowIndex);
        }
    }
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }

    }
    protected void GVIssued_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet ddd = new DataSet();
        ddd = fillIssueGrid();
        DataTable dt = ddd.Tables[0];
        string sortingDirection = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            sortingDirection = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            sortingDirection = "Asc";
        }
        DataView sortedView = new DataView(dt);
        sortedView.Sort = e.SortExpression + " " + sortingDirection;
        //Session["objects"] = sortedView;
        GVIssued.DataSource = sortedView;
        GVIssued.DataBind();
    }

    protected void txtStockNo_TextChanged(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "Data saved successfully.....")
        {
            LblErrorMessage.Text = "";
            LblErrorMessage.Visible = true;
        }
        if (LblErrorMessage.Text == "")
        {
            ProcessIssue();
        }
    }
    protected void Chkboxitem_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.Parent.Parent;
        CheckBox cb = (CheckBox)gr.FindControl("Chkboxitem");

        TextBox txtQty = (TextBox)gr.FindControl("txtQty");
        TextBox txtAmt = (TextBox)gr.FindControl("txtAmt");
        //Label lblAmt = (Label)gr.FindControl("lblAmt");
        Label lblPenalityType = (Label)gr.FindControl("lblPenalityType");
        Label lblPenalityId = (Label)gr.FindControl("lblPenalityId");
        Label lblRate = (Label)gr.FindControl("lblRate");

        if (cb.Checked == false)
        {
            txtQty.Text = "0";
            txtAmt.Text = "0";

            if (PenMode == "Update" && ViewState["FinisherReceivePenEdit"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["FinisherReceivePenEdit"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;

                        dt4.Rows.RemoveAt(n);
                        dt4.AcceptChanges();
                        break;
                    }
                }
            }

            if (PenMode == "" && ViewState["FinisherReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["FinisherReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;

                        dt4.Rows.RemoveAt(n);
                        dt4.AcceptChanges();
                        break;
                    }
                }
            }
        }
        else if (cb.Checked == true && lblPenalityId.Text == "0")
        {
            txtAmt.Visible = true;
            txtAmt.Enabled = true;
        }
        else
        {
            txtAmt.Visible = false;
            txtAmt.Enabled = false;
        }

        ModalPopupExtender1.Show();
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        if (GVPenalty.Rows.Count > 0)
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            //int id =Convert.ToInt32(currentRow);

            String lblPenalityId = ((Label)currentRow.FindControl("lblPenalityId")).Text;

            TextBox txtQty = (TextBox)currentRow.FindControl("txtQty");
            // Label lblAmt = (Label)currentRow.FindControl("lblAmt");
            TextBox txtAmt = (TextBox)currentRow.FindControl("txtAmt");
            Label lblPenalityType = (Label)currentRow.FindControl("lblPenalityType");
            Label lblRate = (Label)currentRow.FindControl("lblRate");

            CheckBox cb = (CheckBox)currentRow.FindControl("Chkboxitem");

            if (lblPenalityId == "0")
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = false;
                txtQty.Enabled = false;
                txtQty.Text = "0";
                txtAmt.Enabled = true;
                //txtAmt.Text = "0";
                //cb.Checked = false;
            }
            else
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = true;
                txtAmt.Enabled = false;
            }

            if (cb.Checked == false)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Check First Penality !!');", true);
                txtQty.Text = "0";
            }

            if (PenMode == "Update" && ViewState["FinisherReceivePenEdit"] != null)
            {
                if (Convert.ToInt32(txtQty.Text) > 1)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Sorry You Can't Enter More Than Receive Quantity !!');", true);
                    txtQty.Text = "0";
                    //lblAmt.Text = "0";
                    txtAmt.Text = "0";
                }
                else
                {
                    if (Convert.ToInt32(lblPenalityId) > 0)
                    {
                        if (lblPenalityType.Text == "A")
                        {
                            double TAmt = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                            //lblAmt.Text = Convert.ToString(TAmt);
                            txtAmt.Text = Convert.ToString(TAmt);
                        }
                        else
                        {
                            double TAmt = Convert.ToDouble(ViewState["AreaEdit"]) * Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                            // lblAmt.Text = Convert.ToString(TAmt);
                            txtAmt.Text = Convert.ToString(TAmt);
                        }
                    }
                }
            }

            else if (PenMode == "")
            {
                if (Convert.ToInt32(txtQty.Text) > 1)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Sorry You Can't Enter More Than One Quantity !!');", true);
                    txtQty.Text = "0";
                    //lblAmt.Text = "0";
                    txtAmt.Text = "0";
                }
                else
                {
                    if (Convert.ToInt32(lblPenalityId) > 0)
                    {
                        if (lblPenalityType.Text == "A")
                        {
                            double TAmt = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                            //lblAmt.Text = Convert.ToString(TAmt);
                            txtAmt.Text = Convert.ToString(TAmt);
                        }
                        else
                        {
                            double TAmt = Convert.ToDouble(ViewState["Area"]) * Convert.ToDouble(txtQty.Text) * Convert.ToDouble(lblRate.Text);
                            // lblAmt.Text = Convert.ToString(TAmt);
                            txtAmt.Text = Convert.ToString(TAmt);
                        }
                    }
                }
            }


            ModalPopupExtender1.Show();
        }
    }
    protected void txtAmt_TextChanged(object sender, EventArgs e)
    {
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //int id =Convert.ToInt32(currentRow);

        String lblPenalityId = ((Label)currentRow.FindControl("lblPenalityId")).Text;

        TextBox txtQty = (TextBox)currentRow.FindControl("txtQty");
        // Label lblAmt = (Label)currentRow.FindControl("lblAmt");
        TextBox txtAmt = (TextBox)currentRow.FindControl("txtAmt");
        Label lblPenalityType = (Label)currentRow.FindControl("lblPenalityType");
        Label lblRate = (Label)currentRow.FindControl("lblRate");

        CheckBox cb = (CheckBox)currentRow.FindControl("Chkboxitem");

        if (cb.Checked == true)
        {
            if (PenMode == "Update" && ViewState["FinisherReceivePenEdit"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["FinisherReceivePenEdit"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId)
                    {
                        m = 1;

                        dt4.Rows[n]["Amt"] = txtAmt.Text;

                        dt4.AcceptChanges();
                        ViewState["FinisherReceivePenEdit"] = dt4;
                    }
                }
            }

            if (PenMode == "" && ViewState["FinisherReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["FinisherReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId)
                    {
                        m = 1;
                        dt4.Rows[n]["Amt"] = txtAmt.Text;

                        dt4.AcceptChanges();
                        ViewState["FinisherReceivePen"] = dt4;
                    }
                }
            }

        }
        ModalPopupExtender1.Show();
    }
    private void fill_PenalityQuality()
    {
        //CommanFunction.FillCombo(ddQualityName, "Select QualityId,QualityName from Quality Where MasterCompanyid=" + Session["varCompanyId"] + " order by QualityName");

        CommanFunction.FillCombo(ddQualityName, "Select ITEM_ID,ITEM_NAME from Item_master Where MasterCompanyid=" + Session["varCompanyId"] + " and CATEGORY_ID=1 order by ITEM_NAME");

    }
    protected void ddQualityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillPenalityGrid(Convert.ToInt32(ddQualityName.SelectedValue));
    }
    protected void FillPenalityGrid(int ItemId)
    {
        ModalPopupExtender1.Show();

        //if (ddQualityName.SelectedIndex > 0)    
        if (Convert.ToInt32(ddQualityName.SelectedValue) > 0)
        {
            string sql = "";
            if (variable.VarNewQualitySize == "1")
            {
                sql = @"select PenalityId,PenalityName,rate,PenalityType,QualityId from PenalityMaster where (Qualityid='" + ItemId + "' or Qualityid=0) and PenalityWF='F' And PenalityId<>-1 ";
            }
            sql = sql + "Order By PenalityName Desc";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GVPenalty.DataSource = ds;
                GVPenalty.DataBind();
                GVPenalty.Visible = true;
            }
            else
            {
                GVPenalty.Visible = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
    }
    protected void popup_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        Label lnk1 = (Label)gvRow.FindControl("lblText");
        PenMode = lnk1.Text;

        Label lblProcess_Rec_Id = (Label)gvRow.FindControl("lblProcess_Rec_Id");
        Label lblProcess_Rec_Detail_Id = (Label)gvRow.FindControl("lblProcess_Rec_Detail_Id");
        Label lblItemFinishedId = (Label)gvRow.FindControl("lblItemFinishedId");
        Label lblRecAmount = (Label)gvRow.FindControl("lblRecAmount");
        Label lblRecArea = (Label)gvRow.FindControl("lblRecArea");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityid");
        Label lblItemId = (Label)gvRow.FindControl("lblItemId");

        PenProcessRecId = Convert.ToInt32(lblProcess_Rec_Id.Text);
        PenProcessRecDetailId = Convert.ToInt32(lblProcess_Rec_Detail_Id.Text);
        PenItemFinishedId = Convert.ToInt32(lblItemFinishedId.Text);
        RAmount = Convert.ToDouble(lblRecAmount.Text);
        RArea = Convert.ToDouble(lblRecArea.Text);


        ModalPopupExtender1.Show();
        if (PenMode == "Update")
        {
            ViewState["PenProcess_Rec_Id"] = "";
            ViewState["PenProcess_Rec_Detail_Id"] = "";
            ViewState["PenProcess_Rec_Id"] = lblProcess_Rec_Id.Text;
            ViewState["PenProcess_Rec_Detail_Id"] = lblProcess_Rec_Detail_Id.Text;
            CreatePenEdit();
            fill_PenalityQuality();
            ddQualityName.Enabled = false;
            ddQualityName.SelectedValue = lblItemId.Text;
            FillPenalityGrid(Convert.ToInt32(lblItemId.Text));
            //ddQualityName.SelectedValue = lblQualityid.Text;
            //FillPenalityGrid(Convert.ToInt32(lblQualityid.Text));                

        }
        else
        {
            FillPenalityGrid(0);
            fill_PenalityQuality();
        }

    }
    protected void UpdatePenality(int ProcessRecId, int ProcessRecDetailId, int ItemFinishedId, double Amount, double Area)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        DataTable dtrecordsPenSave = new DataTable();
        if (ViewState["FinisherReceivePenEdit"] != null)
        {
            dtrecordsPenSave = (DataTable)ViewState["FinisherReceivePenEdit"];
        }
        else if (ViewState["FinisherReceivePenEdit"] == null)
        {
            dtrecordsPenSave.Columns.Add("PenalityId", typeof(int));
            dtrecordsPenSave.Columns.Add("PenalityName", typeof(string));
            dtrecordsPenSave.Columns.Add("Qty", typeof(int));
            dtrecordsPenSave.Columns.Add("Rate", typeof(float));
            dtrecordsPenSave.Columns.Add("Amount", typeof(float));
            dtrecordsPenSave.Columns.Add("PenalityType", typeof(string));

            DataRow dr = dtrecordsPenSave.NewRow();
            dr["PenalityId"] = 0;
            dr["PenalityName"] = "";
            dr["Qty"] = 0;
            dr["Rate"] = 0;
            dr["Amount"] = 0;
            dr["PenalityType"] = null;

            dtrecordsPenSave.Rows.Add(dr);
            ViewState["FinisherReceivePenEdit"] = dtrecordsPenSave;
        }


        if (ViewState["FinisherReceivePenEdit"] != null)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[13];

                _arrpara[0] = new SqlParameter("@Process_Rec_Id", ProcessRecId);
                _arrpara[1] = new SqlParameter("@process_rec_detail_id", ProcessRecDetailId);

                _arrpara[2] = new SqlParameter("@ProcessId", ddprocess.SelectedValue);
                _arrpara[3] = new SqlParameter("@Empid", ddemp.SelectedValue);
                _arrpara[4] = new SqlParameter("@Companyid", ddCompName.SelectedValue);
                _arrpara[5] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
                _arrpara[6] = new SqlParameter("@Penality", name);
                _arrpara[7] = new SqlParameter("@Userid", Session["varuserid"]);
                _arrpara[8] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                _arrpara[8].Direction = ParameterDirection.Output;
                _arrpara[9] = new SqlParameter("@dtrecordsPen", dtrecordsPenSave);
                _arrpara[10] = new SqlParameter("@ItemFinishedId", ItemFinishedId);
                _arrpara[11] = new SqlParameter("@Amount", Amount);
                _arrpara[12] = new SqlParameter("@Area", Area);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateFinisherInwardsOutwardsPenality]", _arrpara);

                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = _arrpara[8].Value.ToString();

                //lblLastReceiptNo.Text = Convert.ToString(ViewState["Process_Rec_Id"]);
                Tran.Commit();

                //fillIssueGrid();
                BindReceiveIssueGrid();

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
                Tran.Rollback();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        name = "";

        if (PenMode == "Update" && ViewState["FinisherReceivePenEdit"] != null)
        {
            DataTable dtrecordsPen = new DataTable();


            dtrecordsPen = (DataTable)ViewState["FinisherReceivePenEdit"];


            for (int i = 0; i < GVPenalty.Rows.Count; i++)
            {
                CheckBox chkoutItem = ((CheckBox)GVPenalty.Rows[i].FindControl("Chkboxitem"));
                TextBox txtQty = ((TextBox)GVPenalty.Rows[i].FindControl("txtQty"));
                TextBox txtAmt = ((TextBox)GVPenalty.Rows[i].FindControl("txtAmt"));
                //Label lblAmt = ((Label)GVPenalty.Rows[i].FindControl("lblAmt"));

                if ((chkoutItem.Checked == true))
                {
                    Label lblRate = ((Label)GVPenalty.Rows[i].FindControl("lblRate"));
                    Label lblPenalityName = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityName"));
                    Label lblPenalityId = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityId"));
                    Label lblPenalityType = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityType"));

                    int m = 0;
                    for (int n = 0; n < dtrecordsPen.Rows.Count; n++)
                    {
                        if (dtrecordsPen.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                        {
                            m = 1;
                            dtrecordsPen.Rows.RemoveAt(n);
                            dtrecordsPen.AcceptChanges();
                            break;
                        }
                    }

                    DataRow dr = dtrecordsPen.NewRow();
                    dr["PenalityId"] = lblPenalityId.Text;
                    dr["PenalityName"] = lblPenalityName.Text;
                    dr["Qty"] = txtQty.Text == "" ? "1" : txtQty.Text;
                    dr["Rate"] = lblRate.Text == "" ? "0" : lblRate.Text;
                    dr["Amount"] = txtAmt.Text == "" ? "0" : txtAmt.Text;
                    dr["PenalityType"] = lblPenalityType.Text;

                    dtrecordsPen.Rows.Add(dr);
                    ViewState["FinisherReceivePenEdit"] = dtrecordsPen;
                    name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";

                }
            }


            UpdatePenality(PenProcessRecId, PenProcessRecDetailId, PenItemFinishedId, RAmount, RArea);

        }
        else if (PenMode == "")
        {
            DataTable dtrecordsPen = new DataTable();

            if (ViewState["FinisherReceivePen"] == null)
            {
                //DataTable dtrecords = new DataTable();           
                dtrecordsPen.Columns.Add("PenalityId", typeof(int));
                dtrecordsPen.Columns.Add("PenalityName", typeof(string));
                dtrecordsPen.Columns.Add("Qty", typeof(int));
                dtrecordsPen.Columns.Add("Rate", typeof(float));
                dtrecordsPen.Columns.Add("Amt", typeof(float));
                dtrecordsPen.Columns.Add("PenalityType", typeof(string));

                ViewState["FinisherReceivePen"] = dtrecordsPen;
            }
            else
            {
                dtrecordsPen = (DataTable)ViewState["FinisherReceivePen"];
            }

            for (int i = 0; i < GVPenalty.Rows.Count; i++)
            {
                CheckBox chkoutItem = ((CheckBox)GVPenalty.Rows[i].FindControl("Chkboxitem"));
                TextBox txtQty = ((TextBox)GVPenalty.Rows[i].FindControl("txtQty"));
                TextBox txtAmt = ((TextBox)GVPenalty.Rows[i].FindControl("txtAmt"));
                //Label lblAmt = ((Label)GVPenalty.Rows[i].FindControl("lblAmt"));

                if ((chkoutItem.Checked == true))
                {
                    Label lblRate = ((Label)GVPenalty.Rows[i].FindControl("lblRate"));
                    Label lblPenalityName = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityName"));
                    Label lblPenalityId = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityId"));
                    Label lblPenalityType = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityType"));

                    int m = 0;
                    for (int n = 0; n < dtrecordsPen.Rows.Count; n++)
                    {
                        if (dtrecordsPen.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                        {
                            m = 1;
                            dtrecordsPen.Rows.RemoveAt(n);
                            dtrecordsPen.AcceptChanges();
                            break;
                        }
                    }

                    DataRow dr = dtrecordsPen.NewRow();
                    dr["PenalityId"] = lblPenalityId.Text;
                    dr["PenalityName"] = lblPenalityName.Text;
                    dr["Qty"] = txtQty.Text == "" ? "1" : txtQty.Text;
                    dr["Rate"] = lblRate.Text == "" ? "0" : lblRate.Text;
                    dr["Amt"] = txtAmt.Text == "" ? "0" : txtAmt.Text;
                    dr["PenalityType"] = lblPenalityType.Text;

                    dtrecordsPen.Rows.Add(dr);
                    ViewState["FinisherReceivePen"] = dtrecordsPen;
                    name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                }
            }

        }

    }
    protected void GVPenalty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkoutItem = ((CheckBox)e.Row.FindControl("Chkboxitem"));
            TextBox txtQty = ((TextBox)e.Row.FindControl("txtQty"));
            TextBox txtAmt = ((TextBox)e.Row.FindControl("txtAmt"));
            //Label lblAmt = ((Label)e.Row.FindControl("lblAmt"));

            Label lblRate = ((Label)e.Row.FindControl("lblRate"));
            Label lblPenalityName = ((Label)e.Row.FindControl("lblPenalityName"));
            Label lblPenalityId = ((Label)e.Row.FindControl("lblPenalityId"));
            Label lblPenalityType = ((Label)e.Row.FindControl("lblPenalityType"));

            if (lblPenalityId.Text == "0")
            {

                //GVPenalty.Rows[currentRow.RowIndex].Enabled = false;
                txtQty.Enabled = false;
                txtQty.Text = "0";
                txtAmt.Enabled = true;
                txtAmt.Visible = true;
                //txtAmt.Text = "0";
                //cb.Checked = false;
            }
            else
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = true;
                txtAmt.Enabled = false;
                txtAmt.Visible = false;
            }

            if (PenMode == "Update" && ViewState["FinisherReceivePenEdit"] != null)
            {
                DataTable dt = (DataTable)ViewState["FinisherReceivePenEdit"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        //txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amount"].ToString();

                    }
                }
            }

            if (PenMode == "" && ViewState["FinisherReceivePen"] != null)
            {
                DataTable dt = (DataTable)ViewState["FinisherReceivePen"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amt"].ToString();
                    }
                }
            }
        }
    }
    private void BindReceiveIssueGrid()
    {
        if (ddprocess.SelectedIndex > 0)
        {

            DataSet ds = new DataSet();
            SqlParameter[] array = new SqlParameter[2];
            array[0] = new SqlParameter("@ProcessRecId", SqlDbType.Int);
            array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);

            array[0].Value = ViewState["Process_Rec_Id"];
            array[1].Value = ddprocess.SelectedValue;

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetFinisherReceiveIssueDetails", array);

            txtTotalRecPcs.Text = "0";
            txtTotalIssPcs.Text = "0";

            if (ds.Tables[0].Rows.Count > 0)
            {
                GVReceive.DataSource = ds.Tables[0];
                GVReceive.DataBind();
                txtTotalRecPcs.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
            }
            else
            {
                GVReceive.DataSource = null;
                GVReceive.DataBind();
                txtTotalRecPcs.Text = "";
                lblLastReceiptNo.Text = "";
                ViewState["FinisherReceivePen"] = null;
                ViewState["Process_Rec_Id"] = 0;
                ViewState["process_rec_detail_id"] = 0;
                ViewState["OldFJobName"] = 0;
                ViewState["OldFFinisherName"] = 0;
                ViewState["OldJobName"] = 0;
                ViewState["OldFinisherName"] = 0;
                ViewState["ChallanNo"] = 0;
                TxtEditChallanNo.Text = "";
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                GVIssue.DataSource = ds.Tables[1];
                GVIssue.DataBind();
                txtTotalIssPcs.Text = ds.Tables[1].Compute("sum(Qty)", "").ToString();
            }
            else
            {
                GVIssue.DataSource = null;
                GVIssue.DataBind();
                txtTotalIssPcs.Text = "";
                lblLastReceiptNo.Text = "";
                ViewState["FinisherReceivePen"] = null;
                ViewState["Process_Rec_Id"] = 0;
                ViewState["process_rec_detail_id"] = 0;
                ViewState["OldFJobName"] = 0;
                ViewState["OldFFinisherName"] = 0;
                ViewState["OldJobName"] = 0;
                ViewState["OldFinisherName"] = 0;
                ViewState["ChallanNo"] = 0;
                TxtEditChallanNo.Text = "";
            }
        }
    }
    //protected void BtnSave_Click(object sender, EventArgs e)
    //{
    //    CHECKVALIDCONTROL();
    //    if (LblErrorMessage.Text == "")
    //    {
    //        ProcessIssue();            
    //    }
    //}
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        //if (UtilityModule.VALIDDROPDOWNLIST(ddCompName) == false)
        //{
        //    goto a;
        //}
        if (UtilityModule.VALIDDROPDOWNLIST(ddprocess) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddemp) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }

    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        if (LblErrorMessage.Text == "")
        {
            string where1 = "";

            if (ddCompName.SelectedIndex > 0)
            {
                where1 = where1 + " and PID.CompanyId=" + ddCompName.SelectedValue;
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();

            DataTable dtrecordsPenSave = new DataTable();
            if (ViewState["FinisherReceivePen"] != null)
            {
                dtrecordsPenSave = (DataTable)ViewState["FinisherReceivePen"];
            }
            else if (ViewState["FinisherReceivePen"] == null)
            {
                dtrecordsPenSave.Columns.Add("PenalityId", typeof(int));
                dtrecordsPenSave.Columns.Add("PenalityName", typeof(string));
                dtrecordsPenSave.Columns.Add("Qty", typeof(int));
                dtrecordsPenSave.Columns.Add("Rate", typeof(float));
                dtrecordsPenSave.Columns.Add("Amt", typeof(float));
                dtrecordsPenSave.Columns.Add("PenalityType", typeof(string));

                //DataRow dr = dtrecordsPenSave.NewRow();
                //dr["PenalityId"] = 0;
                //dr["PenalityName"] = "";
                //dr["Qty"] = 0;
                //dr["Rate"] = 0;
                //dr["Amt"] = 0;
                //dr["PenalityType"] = null;

                //dtrecordsPenSave.Rows.Add(dr);
                ViewState["FinisherReceivePen"] = dtrecordsPenSave;
            }


            if (Convert.ToInt32(txtStockNo.Text) > 0 && DDIssueJobType.SelectedIndex > 0 && DDIssueContractorName.SelectedIndex > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] _arrpara = new SqlParameter[22];

                    if (ViewState["Process_Rec_Id"] == null)
                    {
                        ViewState["Process_Rec_Id"] = 0;
                    }
                    _arrpara[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[0].Value = ViewState["Process_Rec_Id"];
                    _arrpara[1] = new SqlParameter("@process_rec_detail_id", SqlDbType.Int);
                    _arrpara[1].Direction = ParameterDirection.Output;
                    _arrpara[1].Value = 0;
                    _arrpara[2] = new SqlParameter("@ProcessId", ddprocess.SelectedValue);
                    _arrpara[3] = new SqlParameter("@Empid", ddemp.SelectedValue);
                    _arrpara[4] = new SqlParameter("@Companyid", ddCompName.SelectedValue);
                    _arrpara[5] = new SqlParameter("@TStockNo", txtStockNo.Text);
                    _arrpara[6] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
                    _arrpara[7] = new SqlParameter("@Penality", name);
                    _arrpara[8] = new SqlParameter("@JobName", DDIssueJobType.SelectedValue);
                    _arrpara[9] = new SqlParameter("@FinisherName", DDIssueContractorName.SelectedValue);
                    _arrpara[10] = new SqlParameter("@IssueDate", txtIssueDate.Text);
                    _arrpara[11] = new SqlParameter("@Userid", Session["varuserid"]);
                    _arrpara[12] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                    _arrpara[12].Direction = ParameterDirection.Output;
                    _arrpara[13] = new SqlParameter("@dtrecordsPen", dtrecordsPenSave);
                    _arrpara[14] = new SqlParameter("@Remarks", TxtRemarks.Text);
                    if (ViewState["OldFJobName"] == null)
                    {
                        ViewState["OldFJobName"] = 0;
                    }
                    _arrpara[15] = new SqlParameter("@OldFJobName", SqlDbType.Int);
                    _arrpara[15].Direction = ParameterDirection.InputOutput;
                    _arrpara[15].Value = ViewState["OldFJobName"];
                    if (ViewState["OldFFinisherName"] == null)
                    {
                        ViewState["OldFFinisherName"] = 0;
                    }
                    _arrpara[16] = new SqlParameter("@OldFFinisherName", SqlDbType.Int);
                    _arrpara[16].Direction = ParameterDirection.InputOutput;
                    _arrpara[16].Value = ViewState["OldFFinisherName"];

                    if (ViewState["OldJobName"] == null)
                    {
                        ViewState["OldJobName"] = 0;
                    }
                    _arrpara[17] = new SqlParameter("@OldJobName", SqlDbType.Int);
                    _arrpara[17].Direction = ParameterDirection.InputOutput;
                    _arrpara[17].Value = ViewState["OldJobName"];
                    if (ViewState["OldFinisherName"] == null)
                    {
                        ViewState["OldFinisherName"] = 0;
                    }
                    _arrpara[18] = new SqlParameter("@OldFinisherName", SqlDbType.Int);
                    _arrpara[18].Direction = ParameterDirection.InputOutput;
                    _arrpara[18].Value = ViewState["OldFinisherName"];
                    if (ViewState["ChallanNo"] == null)
                    {
                        ViewState["ChallanNo"] = 0;
                    }
                    _arrpara[19] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
                    _arrpara[19].Direction = ParameterDirection.InputOutput;
                    _arrpara[19].Value = ViewState["ChallanNo"];
                    _arrpara[20] = new SqlParameter("@Mode", mode);
                    _arrpara[21] = new SqlParameter("@Where", where1);

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveFinisherInwardsOutwards]", _arrpara);

                    ViewState["Process_Rec_Id"] = _arrpara[0].Value;
                    ViewState["process_rec_detail_id"] = _arrpara[1].Value;
                    ViewState["OldFJobName"] = _arrpara[15].Value;
                    ViewState["OldFFinisherName"] = _arrpara[16].Value;
                    ViewState["OldJobName"] = _arrpara[17].Value;
                    ViewState["OldFinisherName"] = _arrpara[18].Value;
                    ViewState["ChallanNo"] = _arrpara[19].Value;
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[12].Value.ToString();

                    lblLastReceiptNo.Text = Convert.ToString(ViewState["Process_Rec_Id"]);
                    Tran.Commit();

                    ClearAfterSave();
                    fillIssueGrid();
                    BindReceiveIssueGrid();

                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
                    Tran.Rollback();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    private void ClearAfterSave()
    {
        GVPenalty.DataSource = null;
        GVPenalty.DataBind();

        ViewState["FinisherReceivePen"] = null;

        txtStockNo.Text = "";
    }
    protected void GVReceive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQualityType = (Label)e.Row.FindControl("lblQualityType");
            DropDownList DDReceiveRejectPcs = ((DropDownList)e.Row.FindControl("DDReceiveRejectPcs"));

            DDReceiveRejectPcs.SelectedValue = lblQualityType.Text;

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVReceive, "Select$" + e.Row.RowIndex);
        }
    }
    protected void GVIssue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVIssue, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DeleteRow(int lblProcessRecDetailId, int Issue_Detail_Id, int lblIIssueOrderId, int NoOfRows, int lblFinisherJobId, int lblProcessRecId, int lblStockNo)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[9];

            _param[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _param[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            _param[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
            _param[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _param[4] = new SqlParameter("@RowCount", SqlDbType.Int);
            _param[5] = new SqlParameter("@FinishnerJobId", SqlDbType.Int);
            _param[6] = new SqlParameter("@ProcessRecId", SqlDbType.Int);
            _param[7] = new SqlParameter("@StockNo", SqlDbType.Int);

            _param[0].Value = ddprocess.SelectedValue;
            _param[1].Value = lblProcessRecDetailId;
            _param[2].Value = Issue_Detail_Id;
            _param[3].Value = lblIIssueOrderId;
            _param[4].Value = GVIssue.Rows.Count;
            _param[5].Value = lblFinisherJobId;
            _param[6].Value = lblProcessRecId;
            _param[7].Value = lblStockNo;
            _param[8] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
            _param[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteProcessFinisherReceiveDetail]", _param);

            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = _param[8].Value.ToString();
            Tran.Commit();

            fillIssueGrid();
            BindReceiveIssueGrid();
            //Fillstockno();
            Fillstockno(StockNoMainBazaarChallanNo, StockNoItemFinishedId);

            //fillorderdetail();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
            Tran.Rollback();
            //ViewState["Process_Rec_Id"] = 0;
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void GVIssue_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblIssuePcs = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblIssuePcs")).Text);

        lblProcessRecDetailId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblProcessRecDetailId")).Text);
        lblIItemId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblIItemId")).Text);
        lblIQualityid = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblIQualityid")).Text);
        Issue_Detail_Id = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("Issue_Detail_Id")).Text);
        lblFinisherJobId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblFinisherJobId")).Text);
        lblFinisherNameId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblFinisherNameId")).Text);
        lblIIssueOrderId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblIIssueOrderId")).Text);
        lblProcessRecId = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblProcessRecId")).Text);
        lblStockNo = Convert.ToInt32(((Label)GVIssue.Rows[e.RowIndex].FindControl("lblStockNo")).Text);
        NoOfRows = GVIssue.Rows.Count;

        if (TxtEditChallanNo.Text != "")
        {
            btnclickflag = "";

            btnclickflag = "BtnDeleteRow";
            txtpwd.Focus();
            Popup(true);
        }
        else if (TxtEditChallanNo.Text.Trim() == "")
        {
            DeleteRow(lblProcessRecDetailId, Issue_Detail_Id, lblIIssueOrderId, NoOfRows, lblFinisherJobId, lblProcessRecId, lblStockNo);
            #region

            //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
            //SqlTransaction Tran = con.BeginTransaction();
            //try
            //{
            //    SqlParameter[] _param = new SqlParameter[9];

            //    _param[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
            //    _param[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            //    _param[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
            //    _param[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            //    _param[4] = new SqlParameter("@RowCount", SqlDbType.Int);
            //    _param[5] = new SqlParameter("@FinishnerJobId", SqlDbType.Int);
            //    _param[6] = new SqlParameter("@ProcessRecId", SqlDbType.Int);
            //    _param[7] = new SqlParameter("@StockNo", SqlDbType.Int);

            //    _param[0].Value = ddprocess.SelectedValue;
            //    _param[1].Value = lblProcessRecDetailId;
            //    _param[2].Value = Issue_Detail_Id;
            //    _param[3].Value = lblIIssueOrderId;
            //    _param[4].Value = GVIssue.Rows.Count;
            //    _param[5].Value = lblFinisherJobId;
            //    _param[6].Value = lblProcessRecId;
            //    _param[7].Value = lblStockNo;
            //    _param[8] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);
            //    _param[8].Direction = ParameterDirection.Output;

            //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteProcessFinisherReceiveDetail]", _param);

            //    LblErrorMessage.Visible = true;
            //    LblErrorMessage.Text = _param[8].Value.ToString();
            //    Tran.Commit();

            //    fillIssueGrid();
            //    BindReceiveIssueGrid();

            //    //fillorderdetail();
            //}
            //catch (Exception ex)
            //{
            //    UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
            //    Tran.Rollback();
            //    //ViewState["Process_Rec_Id"] = 0;
            //    LblErrorMessage.Visible = true;
            //    LblErrorMessage.Text = ex.Message;
            //}
            //finally
            //{
            //    con.Close();
            //    con.Dispose();
            //}
            #endregion

        }

    }
    protected void UpdateRejectPcs(int ProcessRecId, int ProcessRecDetailId, int ItemFinishedId, int DDReceiveRejectPcsId, double Area, int CalType, double PenAmt)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[14];

            _arrpara[0] = new SqlParameter("@Process_Rec_Id", ProcessRecId);
            _arrpara[1] = new SqlParameter("@process_rec_detail_id", ProcessRecDetailId);
            _arrpara[2] = new SqlParameter("@DDReceiveRejectPcsId", DDReceiveRejectPcsId);

            _arrpara[3] = new SqlParameter("@ProcessId", ddprocess.SelectedValue);
            _arrpara[4] = new SqlParameter("@Empid", ddemp.SelectedValue);
            _arrpara[5] = new SqlParameter("@Companyid", ddCompName.SelectedValue);
            _arrpara[6] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);
            _arrpara[7] = new SqlParameter("@Userid", Session["varuserid"]);
            _arrpara[8] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
            _arrpara[8].Direction = ParameterDirection.Output;
            _arrpara[9] = new SqlParameter("@ItemFinishedId", ItemFinishedId);
            _arrpara[10] = new SqlParameter("@IssueDate", txtIssueDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : txtIssueDate.Text);
            _arrpara[11] = new SqlParameter("@Area", Area);
            _arrpara[12] = new SqlParameter("@CalType", CalType);
            _arrpara[13] = new SqlParameter("@PenAmt", PenAmt);


            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateRejectPcsInwardsOutwards]", _arrpara);

            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = _arrpara[8].Value.ToString();

            //lblLastReceiptNo.Text = Convert.ToString(ViewState["Process_Rec_Id"]);
            Tran.Commit();

            //fillIssueGrid();
            BindReceiveIssueGrid();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void DDReceiveRejectPcs_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddRejectPcs = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddRejectPcs.Parent.Parent;
        int idx = row.RowIndex;
        //ddFinisherName.Focus();

        DropDownList DDReceiveRejectPcs = (DropDownList)row.FindControl("DDReceiveRejectPcs");

        Label lblProcess_Rec_Id = (Label)row.FindControl("lblProcess_Rec_Id");
        Label lblProcess_Rec_Detail_Id = (Label)row.FindControl("lblProcess_Rec_Detail_Id");
        Label lblItemFinishedId = (Label)row.FindControl("lblItemFinishedId");
        Label lblRecAmount = (Label)row.FindControl("lblRecAmount");
        Label lblRecArea = (Label)row.FindControl("lblRecArea");
        Label lblQualityid = (Label)row.FindControl("lblQualityid");
        Label lblCalType = (Label)row.FindControl("lblCalType");
        Label lblOrderId = (Label)row.FindControl("lblOrderId");
        Label lblPenalityAmt = (Label)row.FindControl("lblPenalityAmt");

        //if (DDReceiveRejectPcs.SelectedIndex > 0)
        //{
        UpdateRejectPcs(Convert.ToInt32(lblProcess_Rec_Id.Text), Convert.ToInt32(lblProcess_Rec_Detail_Id.Text), Convert.ToInt32(lblItemFinishedId.Text),
            Convert.ToInt32(DDReceiveRejectPcs.SelectedValue), Convert.ToDouble(lblRecArea.Text), Convert.ToInt32(lblCalType.Text), Convert.ToDouble(lblPenalityAmt.Text));
        //}

    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            // UpdateDetails();
            //Popup(false);

            if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(lblProcessRecDetailId, Issue_Detail_Id, lblIIssueOrderId, NoOfRows, lblFinisherJobId, lblProcessRecId, lblStockNo);
            }
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void BtnInwards_Click(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "" || lblLastReceiptNo.Text != "")
        {
            Report3();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receipt no not showing!');", true);
        }
    }
    private void Report3()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        // array[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

        array[0].Value = Convert.ToInt32(TxtEditChallanNo.Text == "" ? lblLastReceiptNo.Text : TxtEditChallanNo.Text);
        array[1].Value = ddprocess.SelectedValue;
        array[2].Value = ddemp.SelectedValue;

        array[3].Value = Session["varcompanyId"];
        //array[4].Value = ddCompName.SelectedValue;


        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1;  

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForFinisherInwardsReport", array);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\FinisherInwards.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\FinisherInwards.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnOutwards_Click(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "" || lblLastReceiptNo.Text != "")
        {
            Report4();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receipt no not showing!');", true);
        }
    }
    private void Report4()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        // array[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

        array[0].Value = Convert.ToInt32(TxtEditChallanNo.Text == "" ? lblLastReceiptNo.Text : TxtEditChallanNo.Text);
        array[1].Value = ddprocess.SelectedValue;
        array[2].Value = ddemp.SelectedValue;

        array[3].Value = Session["varcompanyId"];
        //array[4].Value = ddCompName.SelectedValue;


        //array[1].Value = 4;
        //array[2].Value = 20;
        //array[3].Value = 1;  

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForFinisherOutWardsReport", array);


        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\FinisherOutwards.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\FinisherOutwards.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnShowStockNo_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int r = ((sender as Button).NamingContainer as GridViewRow).RowIndex;      
      
        //GVIssued.Rows[r].BackColor = System.Drawing.Color.Green;        

        StockNoMainBazaarChallanNo = Convert.ToInt32(((Label)GVIssued.Rows[r].FindControl("lblBZRepNo")).Text);
        StockNoItemFinishedId = Convert.ToInt32(((Label)GVIssued.Rows[r].FindControl("lblItemFinishedId")).Text);        

        Fillstockno(StockNoMainBazaarChallanNo, StockNoItemFinishedId);        

        ////Get the value of column from the DataKeys using the RowIndex.
        //int id = Convert.ToInt32(GVIssued.DataKeys[rowIndex].Values[0]);
    }
    protected void Fillstockno(int StockNoMainBazaarChallanNo1, int StockNoItemFinishedId1)
    {
        string str = @"select PID.MainBazaarChallanNo As BZRepNo,CN.StockNo,CN.TStockNo,VF.ITEM_NAME + space(4) + QualityName As Quality, vf.designName,vf.ColorName,vf.ShapeName,
                    VF.ITEM_NAME + space(2) + QualityName + space(2) + vf.designName + space(2) + vf.ColorName + space(2) + vf.ShapeName + space(2) + PID.FinishingMtSize As ItemDescription,
                    PIM.IssueOrderId,PIM.Empid as FinisherId,PID.Item_Finished_Id ,vf.ITEM_ID, vf.QualityId, vf.designId,vf.ColorId,vf.ShapeId,vf.SizeId,
                    PIM.Companyid
                    from PROCESS_ISSUE_Master_" + ddprocess.SelectedValue + @" PIM INNER JOIN PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PID ON PIM.IssueOrderId=PID.IssueOrderId
                    INNER JOIN V_FinishedItemDetailNew VF ON PID.Item_Finished_Id=VF.ITEM_FINISHED_ID
                    INNER JOIN Process_Stock_Detail PSD ON PID.Issue_Detail_Id=PSD.IssueDetailId and PSD.toProcessId=" + ddprocess.SelectedValue + @" and PSD.ReceiveDetailId=0
                    inner join carpetnumber CN on PSD.StockNo=CN.StockNo
                    where PIM.STATUS<>'CANCELED' AND PIM.Empid=" + ddemp.SelectedValue + @" and PIM.AssignDate<='" + TxtreceiveDate.Text + @"'
                    and PID.MainBazaarChallanNo=" + StockNoMainBazaarChallanNo1 + @" and PID.Item_Finished_Id=" + StockNoItemFinishedId1 + @"
                    ";

        if (ddCompName.SelectedIndex > 0)
        {
            str = str + " and PIM.CompanyId=" + ddCompName.SelectedValue;
        }
        str = str + " order by CN.StockNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DGStockDetail.DataSource = ds.Tables[0];
            DGStockDetail.DataBind();
            txttotalpcsgrid.Text = "0";
            Trsave.Visible = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Trsave.Visible = true;
                txttotalpcsgrid.Text = ds.Tables[0].Compute("count(Tstockno)", "").ToString();
            }
        }
        else
        {
            DGStockDetail.DataSource = null;
            DGStockDetail.DataBind();
        }
    }
    protected void DGStockDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGStockDetail.PageIndex = e.NewPageIndex;
        Fillstockno(StockNoMainBazaarChallanNo, StockNoItemFinishedId);
    }
    protected void btnsavefrmgrid_Click(object sender, EventArgs e)
    {
        //Grid Loop
        for (int i = 0; i < DGStockDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                txtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                txtStockNo_TextChanged(sender, new EventArgs());               
                //ProcessIssue();
            }
        }
        //fillIssueGrid();
        //BindReceiveIssueGrid();
       // Fillstockno();

        Fillstockno(StockNoMainBazaarChallanNo, StockNoItemFinishedId);

    }
    private void ReportRawMaterial()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

        array[0].Value = Convert.ToInt32(TxtEditChallanNo.Text == "" ? lblLastReceiptNo.Text : TxtEditChallanNo.Text);
        array[1].Value = ddprocess.SelectedValue;
        array[2].Value = ddemp.SelectedValue;
        array[3].Value = Session["varcompanyId"];
        array[4].Value = ddCompName.SelectedValue;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FinisherRawMaterialReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptFinisherRawMaterial.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinisherRawMaterial.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnRawMaterial_Click(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "" || lblLastReceiptNo.Text != "")
        {
            ReportRawMaterial();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receipt no not showing!');", true);
        }
    }
    private void SaveOutWardsConsumption()
    {
        //if (LblErrorMessage.Text == "")
        //{ 
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();

            if (ddCompName.SelectedIndex>0 && ddprocess.SelectedIndex > 0 && ddemp.SelectedIndex > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlParameter[] _arrpara = new SqlParameter[5];
                    _arrpara[0] = new SqlParameter("@ProcessRecId", ViewState["Process_Rec_Id"]);                   
                    _arrpara[1] = new SqlParameter("@ProcessId", ddprocess.SelectedValue);
                    _arrpara[2] = new SqlParameter("@Userid", Session["varuserid"]);
                    _arrpara[3] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);                   
                    _arrpara[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                    _arrpara[4].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveFinisherJobIssueConsumptionDetail]", _arrpara);
                   
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = _arrpara[4].Value.ToString();                  
                    Tran.Commit();

                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmFinisherInwardsOutwards.aspx");
                    Tran.Rollback();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
       // }

    }
    protected void BtnSaveOutWardsConsumption_Click(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "" || lblLastReceiptNo.Text != "")
        {
            SaveOutWardsConsumption();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receipt no not showing!');", true);
        }
    }
    private void ReportIssueRawMaterial()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@EmpId", SqlDbType.Int);
        array[3] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[4] = new SqlParameter("@CompanyId", SqlDbType.Int);

        array[0].Value = Convert.ToInt32(TxtEditChallanNo.Text == "" ? lblLastReceiptNo.Text : TxtEditChallanNo.Text);
        array[1].Value = ddprocess.SelectedValue;
        array[2].Value = ddemp.SelectedValue;
        array[3].Value = Session["varcompanyId"];
        array[4].Value = ddCompName.SelectedValue;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FinisherIssueRawMaterialReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptFinisherIssueRawMaterial.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptFinisherIssueRawMaterial.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
    protected void BtnIssueRawMaterial_Click(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "" || lblLastReceiptNo.Text != "")
        {
            ReportIssueRawMaterial();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Receipt no not showing!');", true);
        }
    }
}