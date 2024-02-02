using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Hissab_EditFrmBunkarPayment : System.Web.UI.Page
{
    static int MasterCompanyId;
    static int GVBunkarCarpetReceiveCount = 0;
    static int sum = 0;
    static string name = "";
    static decimal TotalPenalityAmt = 0;
    static int DeleteDetailId = 0, DeleteId = 0, DeleteReceiveDetailId = 0;
    static string btnclickflag = "";
    static string deletepassword = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            BindContractor();

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           Select VarCompanyNo,VarProdCode From MasterSetting 
                           SELECT MONTH_ID,MONTH_NAME FROM MONTHTABLE order by Month_Id
                           SELECT YEAR,YEAR AS YEAR1 FROM YEARDATA order by Year";

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMonth, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDYear, ds, 3, true, "--Select--");
            hncomp.Value = ds.Tables[1].Rows[0]["VarCompanyNo"].ToString();
            int VarProdCode = Convert.ToInt32(ds.Tables[1].Rows[0]["VarProdCode"]);

            if (DDMonth.Items.Count > 0)
            {
                DDMonth.SelectedValue = DateTime.Now.Month.ToString();
            }
            if (DDYear.Items.Count > 0)
            {
                DDYear.SelectedValue = DateTime.Now.Year.ToString();
            }

            //TxtRecDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                BindContractor();
                DDItemName.Items.Clear();
                DDBunkarName.Items.Clear();
                ClearAfterCompanyChange();
            }
        }
    }
    private void ClearAfterCompanyChange()
    {
        //DDEmployeeNamee.Focus();
        GVBunkarCarpetReceive.DataSource = null;
        GVBunkarCarpetReceive.DataBind();

        GVPenalty.DataSource = null;
        GVPenalty.DataBind();

        //dgorder.DataSource = null;
        //dgorder.DataBind(); 

        ViewState["BunkarCarpetReceive"] = null;
        ViewState["BunkarCarpetReceivePen"] = null;

    }
    protected void BindContractor()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand("PRO_DDBINDCONTRACTOR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            DDContractorName.DataSource = dr;
                            DDContractorName.DataTextField = "EmpName";
                            DDContractorName.DataValueField = "EmpId";
                            DDContractorName.DataBind();
                            DDContractorName.Items.Insert(0, new ListItem("--Select--", "0"));
                        }
                    }
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error:" + ex.Message.ToString());
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindContractor();
        DDItemName.Items.Clear();
        DDBunkarName.Items.Clear();
        ClearAfterCompanyChange();
    }
    private void BindItemName()
    {
        string str = "";

        str = @" select Distinct VF.Item_Id,VF.Item_Name from Process_Receive_Master_1 PRM JOIN Process_Receive_Detail_1 PRD ON PRM.Process_Rec_Id=PRD.Process_Rec_ID 
                JOIN V_finishedItemDetailNew VF ON PRD.Item_Finished_Id=VF.Item_Finished_Id Where PRM.EmpID=" + DDContractorName.SelectedValue + " order by VF.Item_Name";

        UtilityModule.ConditionalComboFill(ref DDItemName, str, true, "-----------Select------");
    }
    protected void DDContractorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemName();
        DDBunkarName.Items.Clear();
    }
    private void BindBunkarName()
    {
        string str = "";

        str = @" select BMID,BunkarName from bunkarmaster Where Status=0 and ContractorId=" + DDContractorName.SelectedValue + " Order by BunkarName";

        UtilityModule.ConditionalComboFill(ref DDBunkarName, str, true, "-----------Select------");
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBunkarName();
    }
    private void FillBunkarCarpetDetail()
    {
        ViewState["BunkarCarpetReceive"] = null;
        DataTable dtrecords = new DataTable();

        if (ViewState["BunkarCarpetReceive"] == null)
        {
            //dtrecords = new DataTable();
            dtrecords.Columns.Add("BunkarPaymentId", typeof(int));
            dtrecords.Columns.Add("BunkarPaymentDetailId", typeof(int));
            dtrecords.Columns.Add("ChallanNo", typeof(string));
            dtrecords.Columns.Add("ReceiveDate", typeof(DateTime));

            dtrecords.Columns.Add("ItemName", typeof(string));
            dtrecords.Columns.Add("Quality", typeof(string));
            dtrecords.Columns.Add("Design", typeof(string));
            dtrecords.Columns.Add("Color", typeof(string));
            dtrecords.Columns.Add("Shape", typeof(string));
            dtrecords.Columns.Add("Size", typeof(string));
            dtrecords.Columns.Add("Qty", typeof(int));
            dtrecords.Columns.Add("BZWeight", typeof(float));
            dtrecords.Columns.Add("StWeight", typeof(float));
            dtrecords.Columns.Add("PenalityName", typeof(string));
            dtrecords.Columns.Add("Area", typeof(float));
            dtrecords.Columns.Add("TArea", typeof(float));
            dtrecords.Columns.Add("Lagat", typeof(float));
            dtrecords.Columns.Add("DefWeight", typeof(float));
            dtrecords.Columns.Add("ExtraWeight", typeof(float));
            dtrecords.Columns.Add("ActualPercentage", typeof(float));
            dtrecords.Columns.Add("LessPercentage", typeof(float));
            dtrecords.Columns.Add("Rate", typeof(float));
            dtrecords.Columns.Add("WeightPRate", typeof(float));
            dtrecords.Columns.Add("WAmount", typeof(float));
            dtrecords.Columns.Add("PenalityAmount", typeof(float));
            dtrecords.Columns.Add("WeightPenality", typeof(float));
            dtrecords.Columns.Add("PaidAmount", typeof(float));
            dtrecords.Columns.Add("Item_Finished_Id", typeof(int));
            dtrecords.Columns.Add("Process_Rec_Id", typeof(int));
            dtrecords.Columns.Add("Process_Rec_Detail_Id", typeof(int));
            dtrecords.Columns.Add("HnQty", typeof(int));
            dtrecords.Columns.Add("UnitId", typeof(int));
            dtrecords.Columns.Add("ItemId", typeof(int));
            dtrecords.Columns.Add("QualityId", typeof(int));
            dtrecords.Columns.Add("ItemDescription", typeof(string));
            dtrecords.Columns.Add("CalType", typeof(int));

            ViewState["BunkarCarpetReceive"] = dtrecords;
        }
        else
        {
            dtrecords = (DataTable)ViewState["BunkarCarpetReceive"];
        }

        String str = "";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " and BPM.Item_ID=" + DDItemName.SelectedValue;
        }
        if (DDBunkarName.SelectedIndex > 0)
        {
            str = str + " and BPM.BunkarId=" + DDBunkarName.SelectedValue;
        }
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@ContractorId", DDContractorName.SelectedValue);
        param[1] = new SqlParameter("@MonthId", DDMonth.SelectedValue);
        param[2] = new SqlParameter("@Year", DDYear.SelectedValue);
        param[3] = new SqlParameter("@Where", str);

        //***********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETBUNKARPAYMENTDETAIL", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["BunkarCarpetReceive"] = ds.Tables[1];

            GVBunkarCarpetReceive.DataSource = ds;
            GVBunkarCarpetReceive.DataBind();
            GVBunkarCarpetReceive.Visible = true;

            ViewState["BunakrPaymentId"] = ds.Tables[0].Rows[0]["Id"].ToString();
            CreatePenEdit();
        }
        else
        {
            GVBunkarCarpetReceive.DataSource = null;
            GVBunkarCarpetReceive.DataBind();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }

    public string getgiven(string strval, string strval1, string strval2, string strval3)
    {
        string val = "0";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(Qty),0) from BunkarpaymentDetail where Item_Finished_Id=" + strval1 + "  and Process_Rec_Id=" + strval2 + " and Process_Rec_Detail_Id=" + strval3 + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            val = Convert.ToString(Convert.ToInt32(strval) - Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()));
        }
        return val;
    }
    public Boolean CheckBalQty(int Item_Finished_Id, int Process_Rec_Id, int Process_Rec_Detail_Id, int Qty, int hnQty, int rowindex)
    {
        int TotalIssueQty = 0;
        int TotalBazaarQty = 0;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(Qty),0) as Qty from BunkarpaymentDetail where Item_Finished_Id=" + Item_Finished_Id + "  and Process_Rec_Id=" + Process_Rec_Id + " and Process_Rec_Detail_Id=" + Process_Rec_Detail_Id + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            TotalIssueQty = Convert.ToInt32(ds.Tables[0].Rows[0]["Qty"].ToString());
        }
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(Qty,0) as BazaarQty from Process_Receive_Detail_1 Where Item_Finished_Id=" + Item_Finished_Id + "  and Process_Rec_Id=" + Process_Rec_Id + " and Process_Rec_Detail_Id=" + Process_Rec_Detail_Id + "");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            TotalBazaarQty = Convert.ToInt32(ds2.Tables[0].Rows[0]["BazaarQty"].ToString());
        }

        if (Qty > (TotalBazaarQty - TotalIssueQty + hnQty))
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Bunkar Qty can not be greater than Pending Qty...";
            //TextBox RecQty = (TextBox)GVBunkarCarpetReceive.Rows[rowindex].FindControl("txtBunkarQty");
            //RecQty.Text = Convert.ToString(hnQty);
            return false;
        }
        return true;
    }
    protected void CreatePenEdit()
    {
        ViewState["BunkarCarpetReceivePen"] = null;
        DataTable dtrecordsPenEdit = new DataTable();

        if (ViewState["BunkarCarpetReceivePen"] == null)
        {
            //DataTable dtrecords = new DataTable();
            dtrecordsPenEdit.Columns.Add("ID", typeof(int));
            dtrecordsPenEdit.Columns.Add("PenalityId", typeof(int));
            dtrecordsPenEdit.Columns.Add("PenalityName", typeof(string));
            dtrecordsPenEdit.Columns.Add("Qty", typeof(int));
            dtrecordsPenEdit.Columns.Add("Rate", typeof(float));
            dtrecordsPenEdit.Columns.Add("Amount", typeof(float));
            dtrecordsPenEdit.Columns.Add("PenalityType", typeof(string));
            dtrecordsPenEdit.Columns.Add("BunkarPaymentId", typeof(int));
            dtrecordsPenEdit.Columns.Add("BunkarPaymentDetailId", typeof(int));
            dtrecordsPenEdit.Columns.Add("Process_Rec_Detail_Id", typeof(int));

            ViewState["BunkarCarpetReceivePen"] = dtrecordsPenEdit;
        }
        else
        {
            dtrecordsPenEdit = (DataTable)ViewState["BunkarCarpetReceivePen"];
        }

        string sql = "";

        sql = @"select BPD.Id,PM.PenalityId,PM.PenalityName, Qty,PM.Rate,Amount,PM.PenalityType,BPD.BunkarPaymentId,BPD.BunkarPaymentDetailId,BPD.Process_Rec_Detail_Id from BunkarPenaltyDetail BPD
                INNER JOIN PenalityMaster PM ON BPD.PenaltyId=PM.PenalityId and PM.PenalityWF='W' where BPD.BunkarPaymentId=" + ViewState["BunakrPaymentId"] + " ";
        sql = sql + "Order By PM.PenalityName Desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["BunkarCarpetReceivePen"] = ds.Tables[0];
        }
    }
    decimal TotalQty2 = 0;
    decimal TotalArea2 = 0;
    decimal TotalWeight = 0;
    protected void CalQtyTotalArea()
    {

        for (int i = 0; i < GVBunkarCarpetReceive.Rows.Count; i++)
        {
            TextBox TxtRecDate = ((TextBox)GVBunkarCarpetReceive.Rows[i].FindControl("TxtRecDate"));
            TextBox txtBZWeight = ((TextBox)GVBunkarCarpetReceive.Rows[i].FindControl("txtBZWeight"));
            Label lblRate = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblRate"));
            Label lblBunkarQty = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblBunkarQty"));


            TextBox txtBunkarQty = ((TextBox)GVBunkarCarpetReceive.Rows[i].FindControl("txtBunkarQty"));
            TotalQty2 += Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text);
            Label lblArea = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblArea"));
            TotalArea2 += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text);

            //Label lblFinalWt = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblFinalWt"));
            //TotalWeight += Convert.ToDecimal(lblFinalWt.Text);

            Label lblTArea = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblTArea"));
            lblTArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text));

            Label lblLagat = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblLagat"));

            Label lblStWeight = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblStWeight"));
            lblStWeight.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text) * Convert.ToDecimal(lblLagat.Text));

            Label lblDefWeight = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblDefWeight"));
            lblDefWeight.Text = Convert.ToString(Convert.ToDecimal(txtBZWeight.Text == "" ? "0" : txtBZWeight.Text) - Convert.ToDecimal(lblStWeight.Text));

            Label lblPenalityName = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPenalityName"));

            Label lblSrNo = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblSrNo"));
            Label lblExtraWeight = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblExtraWeight"));
            Label lblDetailId = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblDetailId"));

            if (Convert.ToDouble(lblDefWeight.Text) < 0)
            {
                lblExtraWeight.Text = "0";
            }
            else
            {
                lblExtraWeight.Text = lblDefWeight.Text;
            }
            ////Actual perc
            Label lblActualPercentage = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblActualPercentage"));
            Label lblLessPercentage = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblLessPercentage"));
            if (Convert.ToDouble(lblStWeight.Text) == 0)
            {
                lblActualPercentage.Text = "0";
            }
            else
            {
                lblActualPercentage.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblDefWeight.Text) / Convert.ToDecimal(lblStWeight.Text) * 100, 2));
            }
            ////Less Perc
            if (Convert.ToDouble(lblActualPercentage.Text) < 0)
            {
                lblLessPercentage.Text = "0";
            }
            else
            {
                lblLessPercentage.Text = lblActualPercentage.Text;
            }

            ////Wamount
            Label lblWAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblWAmount"));
            lblWAmount.Text = Convert.ToString(Convert.ToDecimal(lblTArea.Text) * Convert.ToDecimal(lblRate.Text));

            Label lblItemId = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblItemId"));
            Label lblWeightPRate = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblWeightPRate"));
            Label lblWeightPenality = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblWeightPenality"));
            if (Convert.ToDouble(lblLessPercentage.Text) > 0 && TxtRecDate.Text != "")
            {
                lblWeightPRate.Text = Convert.ToString(UtilityModule.Get_WeightPenalty(Convert.ToInt32(lblItemId.Text), Convert.ToDouble(lblLessPercentage.Text), TxtRecDate.Text));
                lblWeightPenality.Text = Convert.ToString(Convert.ToDecimal(lblWeightPRate.Text) * Convert.ToDecimal(lblTArea.Text));
            }
            else
            {
                lblWeightPRate.Text = "0";
                lblWeightPenality.Text = "0";
            }
            Label lblPaidAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPaidAmount"));
            Label lblPenalityAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPenalityAmount"));
            lblPaidAmount.Text = Convert.ToString(Convert.ToDecimal(lblWAmount.Text) - Convert.ToDecimal(lblPenalityAmount.Text) - Convert.ToDecimal(lblWeightPenality.Text));

            DataTable dt3 = new DataTable();
            dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

            if (dt3.Rows.Count > 0)
            {
                foreach (DataRow row in dt3.Rows)
                {
                    if (row["BunkarPaymentDetailId"].ToString() == lblDetailId.Text)
                    {

                        row["ExtraWeight"] = lblExtraWeight.Text; ;
                        row["ActualPercentage"] = lblActualPercentage.Text;
                        row["LessPercentage"] = lblLessPercentage.Text;
                        row["WeightPRate"] = lblWeightPRate.Text;
                        row["PenalityAmount"] = lblPenalityAmount.Text;
                        row["WeightPenality"] = lblWeightPenality.Text;
                        row["PaidAmount"] = lblPaidAmount.Text;
                    }
                    dt3.AcceptChanges();
                    row.SetModified();
                    ViewState["BunkarCarpetReceive"] = dt3;
                }
            }

        }

    }
    protected void txtBunkarQty_TextChanged(object sender, EventArgs e)
    {
        llMessageBox.Visible = false;
        llMessageBox.Text = "";

        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");


        TextBox txtBunkarQty = (TextBox)gvRow.FindControl("txtBunkarQty");

        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label ProcessRecDetailId = (Label)gvRow.FindControl("lblProcess_Rec_Detail_ID");
        Label hnQty = (Label)gvRow.FindControl("hnQty");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        Label lblPenalityName = (Label)gvRow.FindControl("lblPenalityName");
        lblPenalityName.Text = "";
        Label lblCalType = (Label)gvRow.FindControl("lblCalType");
        Label lblPenalityAmount = (Label)gvRow.FindControl("lblPenalityAmount");
        lblPenalityAmount.Text = "0";

        Label lblItemFinishedId = (Label)gvRow.FindControl("lblItemFinishedId");
        Label lblProcess_Rec_Id = (Label)gvRow.FindControl("lblProcess_Rec_Id");
        Label lblProcess_Rec_Detail_ID = (Label)gvRow.FindControl("lblProcess_Rec_Detail_ID");
        Label lblDetailId = (Label)gvRow.FindControl("lblDetailId");


        if (Convert.ToInt32(txtBunkarQty.Text) > 0)
        {

            //if (CheckBalQty(Convert.ToInt32(lblItemFinishedId.Text), Convert.ToInt32(lblProcess_Rec_Id.Text), Convert.ToInt32(lblProcess_Rec_Detail_ID.Text), Convert.ToInt32(txtBunkarQty.Text), Convert.ToInt32(hnQty.Text), gvRow.RowIndex) == true)
            //{
            //    CalQtyTotalArea();
            //}


            DataTable dt3 = new DataTable();
            dt3 = (DataTable)ViewState["BunkarCarpetReceive"];



            if (CheckBalQty(Convert.ToInt32(lblItemFinishedId.Text), Convert.ToInt32(lblProcess_Rec_Id.Text), Convert.ToInt32(lblProcess_Rec_Detail_ID.Text), Convert.ToInt32(txtBunkarQty.Text), Convert.ToInt32(hnQty.Text), gvRow.RowIndex) == true)
            {
                if (dt3.Rows.Count > 0)
                {
                    foreach (DataRow row in dt3.Rows)
                    {
                        if (row["BunkarPaymentDetailId"].ToString() == lblDetailId.Text)
                        {
                            row["Qty"] = txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text;


                            if (row["CalType"].ToString() == "0" || row["CalType"].ToString() == "2" || row["CalType"].ToString() == "3" || row["CalType"].ToString() == "4")
                            {
                                row["WAmount"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtBunkarQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                                //row["CommAmt"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtBunkarQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                            }
                            if (row["CalType"].ToString() == "1")
                            {
                                row["WAmount"] = Convert.ToInt32(txtBunkarQty.Text) * Convert.ToDecimal(row["Rate"].ToString());
                                // row["CommAmt"] = Convert.ToInt32(txtReqQty.Text) * Convert.ToDecimal(row["Comm"].ToString());
                            }

                            row["TArea"] = Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtBunkarQty.Text);
                        }
                        dt3.AcceptChanges();
                        row.SetModified();
                        ViewState["BunkarCarpetReceive"] = dt3;
                    }
                }
                CalQtyTotalArea();
                CarpetPenalityDel(Convert.ToInt32(lblDetailId.Text));
            }
        }
        else
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Sorry Less Than Equal Zero Not Allowed !!";
        }

    }
    protected void txtBZWeight_TextChanged(object sender, EventArgs e)
    {


        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");


        TextBox txtBunkarQty = (TextBox)gvRow.FindControl("txtBunkarQty");
        TextBox txtBZWeight = (TextBox)gvRow.FindControl("txtBZWeight");

        Label Item = (Label)gvRow.FindControl("lblItemId");
        Label lblQualityid = (Label)gvRow.FindControl("lblQualityId");
        Label ProcessRecDetailId = (Label)gvRow.FindControl("lblProcess_Rec_Detail_ID");
        Label hnBalQty = (Label)gvRow.FindControl("hnBalQty");
        Label lblArea = (Label)gvRow.FindControl("lblArea");
        //Label lblPenalityName = (Label)gvRow.FindControl("lblPenalityName");
        //lblPenalityName.Text = "";
        Label lblCalType = (Label)gvRow.FindControl("lblCalType");
        Label lblLagat = (Label)gvRow.FindControl("lblLagat");
        Label lblDetailId = (Label)gvRow.FindControl("lblDetailId");


        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

        // int sum = 0;
        //sum = Convert.ToInt32(dt3.Compute("SUM(Qty)", "ItemId=" + Item.Text + " and QualityId=" + lblQualityid.Text + " and Issue_Detail_Id=" + IssueDetailId.Text));


        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row in dt3.Rows)
            {
                if (row["BunkarPaymentDetailId"].ToString() == lblDetailId.Text)
                {
                    row["BZWeight"] = txtBZWeight.Text == "" ? "0" : txtBZWeight.Text;
                    row["DefWeight"] = Math.Round(Convert.ToDecimal(txtBZWeight.Text == "" ? "0" : txtBZWeight.Text) - (Convert.ToDecimal(Convert.ToDecimal(lblArea.Text) * Convert.ToInt32(txtBunkarQty.Text) * Convert.ToDecimal(lblLagat.Text))), 4);

                }
                dt3.AcceptChanges();
                row.SetModified();
                ViewState["BunkarCarpetReceive"] = dt3;
            }
        }


        CalQtyTotalArea();
        ////CarpetPenalityDel(Convert.ToInt32(lblSrNo.Text));
    }
    protected void TxtRecDate_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        txt.Focus();

        //Label lblSrNo = (Label)gvRow.FindControl("lblSrNo");

        TextBox TxtRecDate = (TextBox)gvRow.FindControl("TxtRecDate");
        Label lblDetailId = (Label)gvRow.FindControl("lblDetailId");


        DataTable dt3 = new DataTable();
        dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

        // int sum = 0;
        //sum = Convert.ToInt32(dt3.Compute("SUM(Qty)", "ItemId=" + Item.Text + " and QualityId=" + lblQualityid.Text + " and Issue_Detail_Id=" + IssueDetailId.Text));


        if (dt3.Rows.Count > 0)
        {
            foreach (DataRow row in dt3.Rows)
            {
                if (row["BunkarPaymentDetailId"].ToString() == lblDetailId.Text)
                {
                    row["ReceiveDate"] = TxtRecDate.Text;

                }
                dt3.AcceptChanges();
                row.SetModified();
                ViewState["BunkarCarpetReceive"] = dt3;
            }
        }

        CalQtyTotalArea();

        ////CarpetPenalityDel(Convert.ToInt32(lblSrNo.Text));
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        FillBunkarCarpetDetail();
    }

    int TotalQty = 0, TotalArea = 0, TotalPenality = 0, TotalWeightPenality = 0, TotalPaidAmount = 0;
    protected void GVBunkarCarpetReceive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int GDCount = GVBunkarCarpetReceiveCount - 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.FindControl("txtKhapWidth").Focus();
            TextBox TxtRecDate = (TextBox)e.Row.FindControl("TxtRecDate");

            TextBox txtBunkarQty = (TextBox)e.Row.FindControl("txtBunkarQty");
            TextBox txtBZWeight = (TextBox)e.Row.FindControl("txtBZWeight");
            Label lblRate = (Label)e.Row.FindControl("lblRate");
            Label lblBunkarQty = (Label)e.Row.FindControl("lblBunkarQty");
            //TotalQty += Convert.ToDecimal(lblBunkarQty.Text);
            Label lblArea = (Label)e.Row.FindControl("lblArea");
            //TotalArea += Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(lblBunkarQty.Text);

            Label lblTArea = (Label)e.Row.FindControl("lblTArea");
            lblTArea.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text));

            Label lblLagat = (Label)e.Row.FindControl("lblLagat");

            Label lblStWeight = (Label)e.Row.FindControl("lblStWeight");
            lblStWeight.Text = Convert.ToString(Convert.ToDecimal(lblArea.Text) * Convert.ToDecimal(txtBunkarQty.Text == "" ? "0" : txtBunkarQty.Text) * Convert.ToDecimal(lblLagat.Text));

            Label lblDefWeight = (Label)e.Row.FindControl("lblDefWeight");
            lblDefWeight.Text = Convert.ToString(Convert.ToDecimal(txtBZWeight.Text == "" ? "0" : txtBZWeight.Text) - Convert.ToDecimal(lblStWeight.Text));

            Label lblPenalityName = (Label)e.Row.FindControl("lblPenalityName");

            Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
            Label lblExtraWeight = (Label)e.Row.FindControl("lblExtraWeight");

            Label lblDetailId = (Label)e.Row.FindControl("lblDetailId");

            if (Convert.ToDouble(lblDefWeight.Text) < 0)
            {
                lblExtraWeight.Text = "0";
            }
            else
            {
                lblExtraWeight.Text = lblDefWeight.Text;
            }
            ////Actual perc
            Label lblActualPercentage = (Label)e.Row.FindControl("lblActualPercentage");
            Label lblLessPercentage = (Label)e.Row.FindControl("lblLessPercentage");
            if (Convert.ToDouble(lblStWeight.Text) == 0)
            {
                lblActualPercentage.Text = "0";
            }
            else
            {
                lblActualPercentage.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblDefWeight.Text) / Convert.ToDecimal(lblStWeight.Text) * 100, 2));
            }
            ////Less Perc
            if (Convert.ToDouble(lblActualPercentage.Text) < 0)
            {
                lblLessPercentage.Text = "0";
            }
            else
            {
                lblLessPercentage.Text = lblActualPercentage.Text;
            }

            ////Wamount
            Label lblWAmount = (Label)e.Row.FindControl("lblWAmount");
            lblWAmount.Text = Convert.ToString(Convert.ToDecimal(lblTArea.Text) * Convert.ToDecimal(lblRate.Text));

            Label lblItemId = (Label)e.Row.FindControl("lblItemId");
            Label lblWeightPRate = (Label)e.Row.FindControl("lblWeightPRate");
            Label lblWeightPenality = (Label)e.Row.FindControl("lblWeightPenality");
            if (Convert.ToDouble(lblLessPercentage.Text) > 0 && TxtRecDate.Text != "")
            {
                lblWeightPRate.Text = Convert.ToString(UtilityModule.Get_WeightPenalty(Convert.ToInt32(lblItemId.Text), Convert.ToDouble(lblLessPercentage.Text), TxtRecDate.Text));
                lblWeightPenality.Text = Convert.ToString(Convert.ToDecimal(lblWeightPRate.Text) * Convert.ToDecimal(lblTArea.Text));
            }
            else
            {
                lblWeightPRate.Text = "0";
                lblWeightPenality.Text = "0";
            }
            Label lblPaidAmount = (Label)e.Row.FindControl("lblPaidAmount");
            Label lblPenalityAmount = (Label)e.Row.FindControl("lblPenalityAmount");
            //string PenalityAmt = ((Label)GVBunkarCarpetReceive.Rows[e.Row.RowIndex].FindControl("lblPenalityAmount")).Text;
            //lblPenalityAmount.Text = PenalityAmt;
            lblPaidAmount.Text = Convert.ToString(Convert.ToDecimal(lblWAmount.Text) - Convert.ToDecimal(lblPenalityAmount.Text) - Convert.ToDecimal(lblWeightPenality.Text));


            if (e.Row.RowIndex == GDCount)
            {
                if (e.Row.RowIndex > 0)
                {
                    string RecDate2 = ((TextBox)GVBunkarCarpetReceive.Rows[e.Row.RowIndex - 1].FindControl("TxtRecDate")).Text;
                    TxtRecDate.Text = RecDate2;

                    DataTable dt3 = new DataTable();
                    dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt3.Rows)
                        {
                            if (row["BunkarPaymentDetailId"].ToString() == lblDetailId.Text)
                            {

                                TxtRecDate.Text = Convert.ToDateTime(row["ReceiveDate"]).ToString("dd-MMM-yyyy");

                            }
                            dt3.AcceptChanges();
                            row.SetModified();
                            ViewState["BunkarCarpetReceive"] = dt3;
                        }
                    }
                }
            }

            TotalQty += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Qty"));
            TotalArea += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TArea"));
            TotalPenality += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PenaltyAmount"));
            TotalWeightPenality += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "WeightPAmount"));
            TotalPaidAmount += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PaidAmount"));
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            txtTotalQty.Text = Convert.ToString(string.Format("{0:#0.00}", TotalQty));
            txtTotalArea.Text = Convert.ToString(string.Format("{0:#0.0000}", TotalArea));
            txtTotalPenality.Text = Convert.ToString(string.Format("{0:#0.00}", TotalPenality));
            txtTotalWeightPenality.Text = Convert.ToString(string.Format("{0:#0.00}", TotalWeightPenality));
            txtTotalPaidAmount.Text = Convert.ToString(string.Format("{0:#0.00}", TotalPaidAmount));
        }
    }
    protected void DeleteRow(int DetailId, int ReceiveDetailId, int Id)
    {
        llMessageBox.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _param = new SqlParameter[8];

            _param[0] = new SqlParameter("@DetailId", SqlDbType.Int);
            _param[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            _param[2] = new SqlParameter("@ID", SqlDbType.Int);
            _param[3] = new SqlParameter("@RowCount", SqlDbType.Int);
            _param[4] = new SqlParameter("@msg", SqlDbType.NVarChar, 200);

            _param[0].Value = DetailId;
            _param[1].Value = ReceiveDetailId;
            _param[2].Value = Id;
            _param[3].Value = GVBunkarCarpetReceive.Rows.Count;
            _param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteBunkarPaymentDetail]", _param);

            llMessageBox.Visible = true;
            llMessageBox.Text = _param[4].Value.ToString();

            Tran.Commit();
            FillBunkarCarpetDetail();

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditFrmBunkarPayment.aspx");
            Tran.Rollback();
            ViewState["Id"] = 0;
            llMessageBox.Visible = true;
            llMessageBox.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void GVBunkarCarpetReceive_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //VarProcess_Issue_Detail_Id = Convert.ToInt32(GVBunkarCarpetReceive.DataKeys[e.RowIndex].Value);

        string lblDetailId = ((Label)GVBunkarCarpetReceive.Rows[e.RowIndex].FindControl("lblDetailId")).Text;

        DeleteReceiveDetailId = Convert.ToInt32(((Label)GVBunkarCarpetReceive.Rows[e.RowIndex].FindControl("lblProcess_Rec_Detail_ID")).Text);

        DeleteDetailId = Convert.ToInt32(((Label)GVBunkarCarpetReceive.Rows[e.RowIndex].FindControl("lblDetailId")).Text);
        DeleteId = Convert.ToInt32(((Label)GVBunkarCarpetReceive.Rows[e.RowIndex].FindControl("lblId")).Text);
        btnclickflag = "";

        if (deletepassword == "")
        {
            btnclickflag = "BtnDeleteRow";
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            txtpwd.Text = deletepassword;
            btnclickflag = "BtnDeleteRow";
            btnCheck_Click(sender, new EventArgs());
        }




        //int rowindex = e.RowIndex;

        //DataTable dt3 = new DataTable();
        //dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

        //if (dt3.Rows.Count > 0)
        //{
        //    dt3.Rows.Remove(dt3.Rows[rowindex]);
        //    dt3.AcceptChanges();
        //    ViewState["BunkarCarpetReceive"] = dt3;
        //}
        //if (dt3.Rows.Count <= 0)
        //{
        //    ViewState["CarpetReceivePen"] = null;
        //}

        if (GVBunkarCarpetReceive.Rows.Count <= 0)
        {
            ViewState["CarpetReceivePen"] = null;
        }

        ////GVBunkarCarpetReceive.DataSource = ViewState["BunkarCarpetReceive"];
        ////GVBunkarCarpetReceive.DataBind();        

        CarpetPenalityDel(Convert.ToInt32(lblDetailId));

        ////BQty(Convert.ToInt32(Process_Rec_Detail_ID), 0);
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
                //txtAmt.Text = "0";
                //cb.Checked = false;
            }
            else
            {
                //GVPenalty.Rows[currentRow.RowIndex].Enabled = true;
                txtAmt.Enabled = false;
            }

            if (ViewState["BunkarCarpetReceivePen"] != null)
            {
                DataTable dt = (DataTable)ViewState["BunkarCarpetReceivePen"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ViewState["BunkarPaymentDetailId"].ToString() == dt.Rows[i]["BunkarPaymentDetailId"].ToString() && lblPenalityId.Text == dt.Rows[i]["PenalityId"].ToString())
                    {
                        chkoutItem.Checked = true;
                        txtQty.Text = dt.Rows[i]["Qty"].ToString();
                        txtAmt.Text = dt.Rows[i]["Amount"].ToString();

                    }
                }
            }

        }

    }

    protected void popup_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        //Label lnk1 = (Label)gvRow.FindControl("lblText");
        //PenMode = lnk1.Text;

        if (ViewState["BunkarCarpetReceivePen"].ToString() != "")
        {
            ViewState["RQty"] = "";
            ViewState["Area"] = "";
            ViewState["ProcessRecDetailID"] = "";
            ViewState["BunkarPaymentId"] = "";
            ViewState["BunkarPaymentDetailId"] = "";

            // Show ModalPopUpExtender.
            ModalPopupExtender1.Show();

            //GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            //int index = gvRow.RowIndex;
            LinkButton lnk = (LinkButton)gvRow.FindControl("popup");
            string myScript = lnk.Text;

            Label lblProcessRecDetailId = (Label)gvRow.FindControl("lblProcess_Rec_Detail_ID");
            Label lblQualityId = (Label)gvRow.FindControl("lblQualityId");
            TextBox txtReqQty = (TextBox)gvRow.FindControl("txtBunkarQty");
            Label lblArea = (Label)gvRow.FindControl("lblArea");
            Label lblItemId = (Label)gvRow.FindControl("lblItemId");
            Label lblDetailId = (Label)gvRow.FindControl("lblDetailId");
            Label lblId = (Label)gvRow.FindControl("lblId");

            int Qualityid = Convert.ToInt32(lblQualityId.Text);
            int ItemId = Convert.ToInt32(lblItemId.Text);
            ViewState["RQty"] = txtReqQty.Text;
            ViewState["Area"] = lblArea.Text;
            ViewState["ProcessRecDetailID"] = lblProcessRecDetailId.Text;
            ViewState["BunkarPaymentId"] = lblId.Text;
            ViewState["BunkarPaymentDetailId"] = lblDetailId.Text;
            FillPenalityGrid(ItemId);
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        name = "";
        TotalPenalityAmt = 0;

        if (ViewState["BunkarCarpetReceivePen"] != null)
        {
            DataTable dtrecordsPen = new DataTable();

            dtrecordsPen = (DataTable)ViewState["BunkarCarpetReceivePen"];

            for (int i = 0; i < GVPenalty.Rows.Count; i++)
            {
                CheckBox chkoutItem = ((CheckBox)GVPenalty.Rows[i].FindControl("Chkboxitem"));
                TextBox txtQty = ((TextBox)GVPenalty.Rows[i].FindControl("txtQty"));
                TextBox txtAmt = ((TextBox)GVPenalty.Rows[i].FindControl("txtAmt"));
                //Label lblAmt = ((Label)GVPenalty.Rows[i].FindControl("lblAmt"));

                if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) >= 0) && Convert.ToDouble(txtAmt.Text == "" ? "0" : txtAmt.Text) > 0)
                {
                    Label lblRate = ((Label)GVPenalty.Rows[i].FindControl("lblRate"));
                    Label lblPenalityName = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityName"));
                    Label lblPenalityId = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityId"));
                    Label lblPenalityType = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityType"));

                    int m = 0;
                    for (int n = 0; n < dtrecordsPen.Rows.Count; n++)
                    {
                        if (dtrecordsPen.Rows[n]["BunkarPaymentDetailId"].ToString() == ViewState["BunkarPaymentDetailId"].ToString() && dtrecordsPen.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                        {
                            m = 1;
                            dtrecordsPen.Rows.RemoveAt(n);
                            dtrecordsPen.AcceptChanges();
                            break;
                        }
                    }

                    DataRow dr = dtrecordsPen.NewRow();
                    dr["ID"] = 0;
                    dr["PenalityId"] = lblPenalityId.Text;
                    dr["PenalityName"] = lblPenalityName.Text;
                    dr["Qty"] = txtQty.Text;
                    dr["Rate"] = lblRate.Text == "" ? "0" : lblRate.Text;
                    dr["Amount"] = txtAmt.Text == "" ? "0" : txtAmt.Text;
                    dr["PenalityType"] = lblPenalityType.Text;
                    dr["BunkarPaymentId"] = ViewState["BunkarPaymentId"];
                    dr["BunkarPaymentDetailId"] = ViewState["BunkarPaymentDetailId"];
                    dr["Process_Rec_Detail_Id"] = ViewState["ProcessRecDetailID"];

                    dtrecordsPen.Rows.Add(dr);
                    ViewState["BunkarCarpetReceivePen"] = dtrecordsPen;
                    name += lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                    TotalPenalityAmt = TotalPenalityAmt + Convert.ToDecimal(txtAmt.Text);
                    //name += lblPenalityId.Text + "_" + lblPenalityName.Text + " " + txtQty.Text + "-" + txtAmt.Text + ",";
                }
            }


            DataTable dt3 = new DataTable();
            dt3 = (DataTable)ViewState["BunkarCarpetReceive"];

            if (dt3.Rows.Count > 0)
            {
                foreach (DataRow row2 in dt3.Rows)
                {
                    if (row2["BunkarPaymentDetailId"].ToString() == ViewState["BunkarPaymentDetailId"].ToString())
                    {
                        row2["PenalityName"] = name;
                        row2["PenalityAmount"] = TotalPenalityAmt;
                        row2["PaidAmount"] = Convert.ToDecimal(row2["WAmount"]) - TotalPenalityAmt - Convert.ToDecimal(row2["WeightPenality"]);

                    }
                    dt3.AcceptChanges();
                    row2.SetModified();
                    ViewState["BunkarCarpetReceive"] = dt3;

                }
            }

            for (int i = 0; i < GVBunkarCarpetReceive.Rows.Count; i++)
            {
                Label lblPenalityName = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPenalityName"));
                Label lblProcessRecDetailId = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblProcessRecDetailId"));
                Label lblDetailId = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblDetailId"));

                Label lblPenalityAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPenalityAmount"));

                Label lblPaidAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblPaidAmount"));
                Label lblWAmount = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblWAmount"));
                Label lblWeightPenality = ((Label)GVBunkarCarpetReceive.Rows[i].FindControl("lblWeightPenality"));

                if (lblDetailId.Text == ViewState["BunkarPaymentDetailId"].ToString())
                {
                    lblPenalityName.Text = name;
                    lblPenalityName.ToolTip = name;
                    lblPenalityAmount.Text = Convert.ToString(Math.Round(TotalPenalityAmt, 4));
                    lblPaidAmount.Text = Convert.ToString(Convert.ToDecimal(lblWAmount.Text) - Convert.ToDecimal(lblPenalityAmount.Text) - Convert.ToDecimal(lblWeightPenality.Text));
                }
            }
        }

    }
    protected void CarpetPenalityDel(int detailid)
    {
        if (ViewState["BunkarCarpetReceivePen"] != null)
        {
            DataTable dtrecordsPen = (DataTable)ViewState["BunkarCarpetReceivePen"];

            DataView dv = new DataView(dtrecordsPen);
            dv.RowFilter = "BunkarPaymentDetailId not in('" + detailid + "')";
            ViewState["BunkarCarpetReceivePen"] = dv.ToTable();
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

            if (ViewState["BunkarCarpetReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["BunkarCarpetReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["BunkarPaymentDetailId"].ToString() == ViewState["BunkarPaymentDetailId"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId.Text)
                    {
                        m = 1;

                        dt4.Rows.RemoveAt(n);
                        dt4.AcceptChanges();
                        break;
                    }
                }
            }

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

            if (ViewState["BunkarCarpetReceivePen"] != null)
            {
                if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(ViewState["RQty"]))
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
            if (ViewState["BunkarCarpetReceivePen"] != null)
            {
                DataTable dt4 = new DataTable();
                dt4 = (DataTable)ViewState["BunkarCarpetReceivePen"];

                int m = 0;
                for (int n = 0; n < dt4.Rows.Count; n++)
                {
                    if (dt4.Rows[n]["BunkarPaymentDetailId"].ToString() == ViewState["BunkarPaymentDetailId"].ToString() && dt4.Rows[n]["PenalityId"].ToString() == lblPenalityId)
                    {
                        m = 1;

                        dt4.Rows[n]["Amount"] = txtAmt.Text;

                        dt4.AcceptChanges();
                        ViewState["BunkarCarpetReceivePen"] = dt4;
                    }
                }
            }
        }
        ModalPopupExtender1.Show();

    }
    protected void FillPenalityGrid(int ItemId)
    {
        string sql = "";
        if (variable.VarNewQualitySize == "1")
        {
            sql = @"select PenalityId,PenalityName,rate,PenalityType,QualityId from PenalityMaster where (Qualityid=" + ItemId + " or Qualityid=0) and PenalityWF='W' And PenalityId<>-1 ";
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (llMessageBox.Text == "")
        {
            ProcessIssue();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        llMessageBox.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDContractorName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(TxtRecDate) == false)
        //{
        //    goto a;
        //}

        else
        {
            goto B;
        }
    a:
        llMessageBox.Visible = true;
        UtilityModule.SHOWMSG(llMessageBox);
    B: ;
    }
    private void ChkGVDDValidation()
    {
        llMessageBox.Text = "";
        int count = GVBunkarCarpetReceive.Rows.Count;

        if (GVBunkarCarpetReceive.Rows.Count > 0)
        {
            for (int k = 0; k < count; k++)
            {
                int Qty = Convert.ToInt32(((TextBox)(GVBunkarCarpetReceive.Rows[k].FindControl("txtBunkarQty"))).Text);
                TextBox TxtRecDate = ((TextBox)GVBunkarCarpetReceive.Rows[k].FindControl("TxtRecDate"));

                if (Qty <= 0 || Qty == 0)
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Please fill qty";
                }
                else if (TxtRecDate.Text == "")
                {
                    llMessageBox.Visible = true;
                    llMessageBox.Text = "Please select Receive Date";
                }
            }
        }
    }
    //*********************************************Process Issue**************************************************************************
    private void ProcessIssue()
    {
        ChkGVDDValidation();
        if (llMessageBox.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();

            DataTable dtrecords = new DataTable();
            if (ViewState["BunkarCarpetReceive"] != null)
            {
                dtrecords = (DataTable)ViewState["BunkarCarpetReceive"];
            }

            dtrecords = (DataTable)ViewState["BunkarCarpetReceive"];

            DataTable dtrecordsPenSave = new DataTable();
            if (ViewState["BunkarCarpetReceivePen"] != null)
            {
                dtrecordsPenSave = (DataTable)ViewState["BunkarCarpetReceivePen"];
            }
            else if (ViewState["BunkarCarpetReceivePen"] == null)
            {
                dtrecordsPenSave.Columns.Add("ID", typeof(int));
                dtrecordsPenSave.Columns.Add("PenalityId", typeof(int));
                dtrecordsPenSave.Columns.Add("PenalityName", typeof(string));
                dtrecordsPenSave.Columns.Add("Qty", typeof(int));
                dtrecordsPenSave.Columns.Add("Rate", typeof(float));
                dtrecordsPenSave.Columns.Add("Amt", typeof(float));
                dtrecordsPenSave.Columns.Add("PenalityType", typeof(string));
                dtrecordsPenSave.Columns.Add("BunkarPaymentId", typeof(int));
                dtrecordsPenSave.Columns.Add("BunkarPaymentDetailId", typeof(int));
                dtrecordsPenSave.Columns.Add("Process_Rec_Detail_Id", typeof(int));

                DataRow dr = dtrecordsPenSave.NewRow();
                dr["ID"] = 0;
                dr["PenalityId"] = 0;
                dr["PenalityName"] = "";
                dr["Qty"] = 0;
                dr["Rate"] = 0;
                dr["Amt"] = 0;
                dr["PenalityType"] = null;
                dr["Amt"] = 0;
                dr["BunkarPaymentId"] = 0;
                dr["BunkarPaymentDetailId"] = 0;
                dr["Process_Rec_Detail_Id"] = 0;

                dtrecordsPenSave.Rows.Add(dr);
                ViewState["BunkarCarpetReceivePen"] = dtrecordsPenSave;
            }


            if (dtrecords.Rows.Count > 0)
            {
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    SqlParameter[] _arrpara = new SqlParameter[16];

                    if (ViewState["ID"] == null)
                    {
                        ViewState["ID"] = 0;
                    }
                    _arrpara[0] = new SqlParameter("@ID", SqlDbType.Int);
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[0].Value = ViewState["ID"];
                    _arrpara[1] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                    _arrpara[2] = new SqlParameter("@Item_Id", DDItemName.SelectedValue);
                    _arrpara[3] = new SqlParameter("@ContractorId", DDContractorName.SelectedValue);
                    _arrpara[4] = new SqlParameter("@BunkarId", DDBunkarName.SelectedValue);
                    _arrpara[5] = new SqlParameter("@MonthId", DDMonth.SelectedValue);
                    _arrpara[6] = new SqlParameter("@Year", DDYear.SelectedValue);
                    _arrpara[7] = new SqlParameter("@PaymentDate", DateTime.Now.ToShortDateString());
                    _arrpara[8] = new SqlParameter("@FinalFlag", chkForFinal.Checked == true ? "1" : "0");
                    _arrpara[9] = new SqlParameter("@UserId", Session["varUserId"]);
                    _arrpara[10] = new SqlParameter("@SaveFlag", "1");
                    _arrpara[11] = new SqlParameter("@dtrecords", dtrecords);
                    _arrpara[12] = new SqlParameter("@dtrecordsPen", dtrecordsPenSave);
                    _arrpara[13] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                    _arrpara[13].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UpdateBunkarPayment]", _arrpara);

                    llMessageBox.Visible = true;
                    llMessageBox.Text = _arrpara[13].Value.ToString();
                    //ViewState["PurchaseReceiveDetailId"] = _arrpara[8].Value;
                    llMessageBox.Text = "Data Successfully Saved.......";
                    ViewState["ID"] = 0;
                    //lblChallanNo.Text = _arrpara[5].Value.ToString();
                    //lblProcessRecID.Text = _arrpara[0].Value.ToString();
                    Tran.Commit();

                    TxtChallanNo.Text = _arrpara[9].Value.ToString();

                    ClearAfterSave();
                    FillBunkarCarpetDetail();
                    //FillBunkarPaymentDetail();

                }
                catch (Exception ex)
                {
                    UtilityModule.MessageAlert(ex.Message, "Master/Hissab/EditFrmBunkarPayment.aspx");
                    Tran.Rollback();
                    ViewState["ID"] = 0;
                    llMessageBox.Visible = true;
                    llMessageBox.Text = ex.Message;
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

        ////DDEmployeeNamee.Focus();
        ////fillorderdetail();
        //GVBunkarCarpetReceive.DataSource = null;
        //GVBunkarCarpetReceive.DataBind();

        //GVPenalty.DataSource = null;
        //GVPenalty.DataBind();

        //ViewState["BunkarCarpetReceive"] = null;
        //ViewState["BunkarCarpetReceivePen"] = null;

    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            deletepassword = txtpwd.Text;
            if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(DeleteDetailId, DeleteReceiveDetailId, DeleteId);
            }
            Popup(false);
        }
        else
        {
            llMessageBox.Visible = true;
            llMessageBox.Text = "Please Enter Correct Password..";
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
}