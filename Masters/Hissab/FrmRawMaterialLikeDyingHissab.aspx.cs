using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmRawMaterialLikeDyingHissab : System.Web.UI.Page
{
    int k;
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            CommanFunction.FillCombo(DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanySelectedChange();
            }

            TDBillNo.Visible = false;
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtfromdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["Hissabid"] = 0;

            if (Convert.ToInt32(Session["varCompanyId"]) == 16 || Convert.ToInt32(Session["varCompanyId"]) == 28)
            {
                BtnDeductionAmountDetail.Visible = true;
            }
            if (Session["VarCompanyNo"].ToString() == "42")
            {
                Textpamt.ReadOnly = true;
            }
            if (Session["VarCompanyNo"].ToString() == "44")
            {
                Btnpreview.Text = "Payment Voucher";
                Btnpreview.Width = 150;
            }
            else
            {

                Btnpreview.Text = "Preview";
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        if (chksample.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDProcess, @"select Distinct PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From SampleDyeingmaster SM inner Join PROCESS_NAME_MASTER PNM on SM.processid=PNM.PROCESS_NAME_ID
                               and SM.Mastercompanyid=" + Session["varcompanyid"] + " and SM.Companyid=" + DDCompanyName.SelectedValue + " order by Process_name", true, "--Select Process--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDProcess, @"Select Distinct PROCESS_NAME_ID,PROCESS_NAME from ProcessProgram PP,OrderMaster OM,Process_Name_Master PNM
        Where PP.Order_Id=OM.OrderId And PP.Process_Id=PNM.Process_Name_Id And OM.Companyid=" + DDCompanyName.SelectedValue + " And PNM.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Process--");
        }
        DDProcess.SelectedValue = "5";
        FillEmployee();
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployee();
    }
    protected void FillEmployee()
    {
        if (chksample.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDPartyName, "select Distinct Ei.EmpId,Ei.EmpName From SampleDyeingmaster SM inner join EmpInfo EI on SM.empid=Ei.EmpId and Sm.Companyid=" + DDCompanyName.SelectedValue + " AND Sm.processid=" + DDProcess.SelectedValue + " and sm.Mastercompanyid=" + Session["varcompanyid"] + " order by Ei.empname", true, "--Select Employee--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDPartyName, "Select Distinct EI.EmpId,EI.EmpName from EmpInfo EI,View_Indent_Rec_Detail VIRD Where EI.EmpId=VIRD.EmpId And ProcessId=" + DDProcess.SelectedValue + " And CompanyId=" + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by Empname", true, "--Select Employee--");
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Hissabid"] = 0;
        PartyNameSelectedChange();
        fillgrid();
    }
    private void PartyNameSelectedChange()
    {
        if (DDPartyName.SelectedIndex > 0 && ChkEditOrder.Checked == true)
        {
            string str = "Select HissabId,BillNo From RawMaterialPreprationHissab Where billstatus =0 And CompanyId=" + DDCompanyName.SelectedValue + " And Processid=" + DDProcess.SelectedValue + " And PartyID=" + DDPartyName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
            if (chksample.Checked == true)
            {
                str = str + "  and hissabtype=1";
            }
            else
            {
                str = str + "  and hissabtype=0";
            }
            str = str + " Order By BillNo";
            UtilityModule.ConditionalComboFill(ref DDBillNo, str, true, "--Select Bill No--");
        }
    }
    private void fillgrid()
    {
        if (chksample.Checked == true)
        {
            Fillgrid_Sample();
        }
        else
        {
            FillGrid_Dyeing();
        }
    }
    protected void FillGrid_Dyeing()
    {
        double tot = 0;

        string Str = @" Select VHD.ChallanNo,isnull(Round(sum(VHD.Total),0),0) Total,VHD.IndentId ,0 Flag,VHD.ProcessRec_PrmId
                        From V_GetRawmaterialPreparationHissabDetail VHD
                        Left outer join V_RawmaterialHissabdone RMD on VHD.ProcessRec_Prmid=RMD.ProcessRec_PrmId And Rmd.IndentID = VHD.INDENTID and RMD.Hissabtype=0
                        Where Rmd.ProcessRec_PrmId is null  And VHD.CompanyId=" + DDCompanyName.SelectedValue + " And VHD.ProcessID=" + DDProcess.SelectedValue + " And VHD.EmpId=" + DDPartyName.SelectedValue;
        switch (Session["varcompanyId"].ToString())
        {
            case "27":
                break;
            default:
                Str = Str + " and VHD.status='Complete'";
                break;
        }
        Str = Str + " Group By VHD.IndentId,VHD.ChallanNo,VHD.ProcessRec_Prmid";

        if (ChkEditOrder.Checked == false)
        {
            Str = Str + " Order by VHD.IndentId,VHD.ProcessRec_Prmid";
        }
        if (ChkEditOrder.Checked == true && DDBillNo.SelectedIndex > 0)
        {

            Str = Str + @" Union All Select VIRD.ChallanNo,isnull(Round(Sum(VIRD.Total),0),0) Total,VIRD.IndentId ,1 Flag,VIRD.ProcessRec_PrmId
                            From V_GetRawmaterialPreparationHissabDetail VIRD Left outer join RawMaterialPreprationHissabdetail RMD on VIRD.ProcessRec_Prmid=RMD.ProcessRec_PrmId
                            inner join V_RawmaterialHissabdone VHD on VIRD.ProcessRec_Prmid=VHD.ProcessRec_PrmId And VHD.IndentID = VIRD.INDENTID And VHD.Hissabtype=0 and vhd.HissabId=" + DDBillNo.SelectedValue + @"
                            Where VIRD.CompanyId=" + DDCompanyName.SelectedValue + " And VIRD.ProcessID=" + DDProcess.SelectedValue + " And VIRD.EmpId=" + DDPartyName.SelectedValue + " Group By VIRD.IndentId,VIRD.ChallanNo,VIRD.ProcessRec_Prmid Order by IndentId,ProcessRec_Prmid";
        }
        //  And VIRD.IndentID=VHD.IndentID
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGIndentDetail.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIndentDetail.DataBind();
            if (ChkEditOrder.Checked == true)
            {
                for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
                {
                    if (Convert.ToInt32(DGIndentDetail.Rows[i].Cells[3].Text) == 1)
                    {
                        ((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked = true;
                        //check if indentQty is greater than or equal to Actual Rec Qty than Amount should be RecQty*Rate Else (RecQty+LossQty)*Rate
                        SqlParameter[] _array = new SqlParameter[8];
                        _array[0] = new SqlParameter("@IndentId", SqlDbType.Int);
                        _array[1] = new SqlParameter("@PrmId", SqlDbType.Int);
                        _array[2] = new SqlParameter("@totalAmount", SqlDbType.Float);
                        _array[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                        _array[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                        _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
                        _array[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                        _array[7] = new SqlParameter("@DebitAmt", SqlDbType.Float);

                        //get Indentid And PrmId
                        string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                        string Prmid = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;

                        _array[0].Value = indentid;
                        _array[1].Value = Prmid;
                        _array[2].Direction = ParameterDirection.Output;
                        _array[3].Value = DDProcess.SelectedValue;
                        _array[4].Value = DDPartyName.SelectedValue;
                        _array[5].Value = DDCompanyName.SelectedValue;
                        _array[6].Value = Session["varcompanyId"];
                        _array[7].Direction = ParameterDirection.Output;//For DebitAmt

                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_GetDyingAMount", _array);
                        tot = tot + Convert.ToDouble(_array[2].Value);
                    }
                }
            }
            Txttotamt.Text = tot.ToString();
            DGIndentDetail.Visible = true;
        }
        else
        {
            DGIndentDetail.Visible = false;
        }
    }
    protected void Fillgrid_Sample()
    {
        double tot = 0;

        string Str = @"select vs.ChallanNo,Sum(vs.Total) as Total,
                vs.Indentid,0 as flag,vs.ProcessRec_prmid
                From V_sampleHissabDetail VS left join  V_RawmaterialHissabdone VHD on VHD.ProcessRec_PrmId=VS.ProcessRec_Prmid and VHD.Hissabtype=1 and Vs.Indentid=VHD.IndentID
                Where vs.Companyid=" + DDCompanyName.SelectedValue + " and vs.Processid=" + DDProcess.SelectedValue + " and vs.empid=" + DDPartyName.SelectedValue + @" and VHD.ProcessRec_PrmId is null ";
        switch (Session["varcompanyid"].ToString())
        {
            case "27":
                break;
            default:
                Str = Str + " and vs.Status='Complete'";
                break;
        }
        Str = Str + " group by vs.ChallanNo,vs.Indentid,vs.ProcessRec_prmid";

        if (ChkEditOrder.Checked == false)
        {
            Str = Str + " Order by vs.Indentid,vs.ProcessRec_prmid";
        }
        if (ChkEditOrder.Checked == true && DDBillNo.SelectedIndex > 0)
        {
            Str = Str + @" UNION ALL select vs.ChallanNo,isnull(Sum(vs.Total),0) As Total,
                                        Vs.Indentid,1 as flag,vs.ProcessRec_prmid
                                        From V_sampleHissabDetail Vs inner join  V_RawmaterialHissabdone VHD on VHD.ProcessRec_PrmId=vs.ProcessRec_Prmid and VHD.Hissabtype=1 and Vs.Indentid=VHD.IndentID and VHD.HissabId=" + DDBillNo.SelectedValue + @"
                                        Where vs.Companyid=" + DDCompanyName.SelectedValue + " and vs.Processid=" + DDProcess.SelectedValue + " and vs.empid=" + DDPartyName.SelectedValue + @" 
                                        group by vs.ChallanNo,Vs.Indentid,vs.ProcessRec_prmid order by Indentid,ProcessRec_prmid";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGIndentDetail.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIndentDetail.DataBind();
            if (ChkEditOrder.Checked == true)
            {
                for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
                {
                    if (Convert.ToInt32(DGIndentDetail.Rows[i].Cells[3].Text) == 1)
                    {
                        ((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked = true;
                        //check if indentQty is greater than or equal to Actual Rec Qty than Amount should be RecQty*Rate Else (RecQty+LossQty)*Rate
                        SqlParameter[] _array = new SqlParameter[8];
                        _array[0] = new SqlParameter("@IndentId", SqlDbType.Int);
                        _array[1] = new SqlParameter("@PrmId", SqlDbType.Int);
                        _array[2] = new SqlParameter("@totalAmount", SqlDbType.Float);
                        _array[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                        _array[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                        _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
                        _array[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                        _array[7] = new SqlParameter("@DebitAmt", SqlDbType.Float);

                        //get Indentid And PrmId
                        string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                        string Prmid = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;

                        _array[0].Value = indentid;
                        _array[1].Value = Prmid;
                        _array[2].Direction = ParameterDirection.Output;
                        _array[3].Value = DDProcess.SelectedValue;
                        _array[4].Value = DDPartyName.SelectedValue;
                        _array[5].Value = DDCompanyName.SelectedValue;
                        _array[6].Value = Session["varcompanyId"];
                        _array[7].Direction = ParameterDirection.Output;//For DebitAmt

                        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_GetDyingAMountSample", _array);
                        tot = tot + Convert.ToDouble(_array[2].Value);
                    }
                }
            }
            Txttotamt.Text = tot.ToString();
            DGIndentDetail.Visible = true;
        }
        else
        {
            DGIndentDetail.Visible = false;
        }
    }
    protected void Chkchallan_CheckedChanged(object sender, EventArgs e)
    {
        if (chksample.Checked == true)
        {
            fillAmtSample();
        }
        else
        {
            fillAmt();
        }

    }
    private void fillAmtSample()
    {
        double tot = 0;
        double DebitAmt = 0;
        for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {

                //check if indentQty is greater than or equal to Actual Rec Qty than Amount should be RecQty*Rate Else (RecQty+LossQty)*Rate
                //if (Session["varcompanyNo"].ToString() == "5")
                //{
                SqlParameter[] _array = new SqlParameter[8];
                _array[0] = new SqlParameter("@IndentId", SqlDbType.Int);
                _array[1] = new SqlParameter("@PrmId", SqlDbType.Int);
                _array[2] = new SqlParameter("@totalAmount", SqlDbType.Float);
                _array[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _array[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _array[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _array[7] = new SqlParameter("@DebitAmt", SqlDbType.Float);

                //get Indentid And PrmId
                string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                string Prmid = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;

                _array[0].Value = indentid;
                _array[1].Value = Prmid;
                _array[2].Direction = ParameterDirection.Output;
                _array[3].Value = DDProcess.SelectedValue;
                _array[4].Value = DDPartyName.SelectedValue;
                _array[5].Value = DDCompanyName.SelectedValue;
                _array[6].Value = Session["varcompanyId"];
                _array[7].Direction = ParameterDirection.Output;//For DebitAmt

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_GetDyingAMountSample", _array);
                tot = tot + Convert.ToDouble(_array[2].Value);
                DebitAmt = DebitAmt + Convert.ToDouble(_array[7].Value);

            }
        }
        Txttotamt.Text = tot.ToString();
        Textpamt.Text = Txttotamt.Text;
        txtDebitamt.Text = DebitAmt.ToString();
        //Fill_Amount();
        Fill_AmountWithGst();
    }
    private void fillAmt()
    {
        string IndentIDs = "";
        double tot = 0;
        double DebitAmt = 0;
        for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {

                //check if indentQty is greater than or equal to Actual Rec Qty than Amount should be RecQty*Rate Else (RecQty+LossQty)*Rate
                //if (Session["varcompanyNo"].ToString() == "5")
                //{
                SqlParameter[] _array = new SqlParameter[8];
                _array[0] = new SqlParameter("@IndentId", SqlDbType.Int);
                _array[1] = new SqlParameter("@PrmId", SqlDbType.Int);
                _array[2] = new SqlParameter("@totalAmount", SqlDbType.Float);
                _array[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _array[4] = new SqlParameter("@EmpId", SqlDbType.Int);
                _array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _array[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
                _array[7] = new SqlParameter("@DebitAmt", SqlDbType.Float);

                //get Indentid And PrmId
                string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                string Prmid = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;

                _array[0].Value = indentid;
                _array[1].Value = Prmid;
                _array[2].Direction = ParameterDirection.Output;
                _array[3].Value = DDProcess.SelectedValue;
                _array[4].Value = DDPartyName.SelectedValue;
                _array[5].Value = DDCompanyName.SelectedValue;
                _array[6].Value = Session["varcompanyId"];
                _array[7].Direction = ParameterDirection.Output;//For DebitAmt

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_GetDyingAMount", _array);
                tot = tot + Convert.ToDouble(_array[2].Value);
                DebitAmt = DebitAmt + Convert.ToDouble(_array[7].Value);
            }
        }
        if ((Convert.ToInt32(Session["varCompanyId"]) == 16) || (Convert.ToInt32(Session["varCompanyId"]) == 28))
        {
            for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;

                    if (IndentIDs == "")
                    {
                        IndentIDs = indentid + "|";
                    }
                    else
                    {
                        IndentIDs = IndentIDs + indentid + "|";
                    }
                }
            }

            SqlParameter[] _array = new SqlParameter[3];
            _array[0] = new SqlParameter("@IndentId", SqlDbType.NVarChar);
            _array[1] = new SqlParameter("@DeductionAmount", SqlDbType.Float);
            _array[2] = new SqlParameter("@TypeID", SqlDbType.Int);

            _array[0].Value = IndentIDs;
            _array[1].Direction = ParameterDirection.Output;
            _array[2].Value = 0;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDyingDeductionAmountForShortQty", _array);
            txtDeductionAmt.Text = _array[1].Value.ToString();
        }
        Txttotamt.Text = tot.ToString();
        Textpamt.Text = Txttotamt.Text;
        txtDebitamt.Text = DebitAmt.ToString();
        //Fill_Amount();
        Fill_AmountWithGst();
    }
    private void CheckDuplicateBillNo()
    {
        string Str = "Select * From RawMaterialPreprationHissab Where BillNo='" + Textbillno.Text + "' And PartyID=" + DDPartyName.SelectedValue + " And HissabId <> " + ViewState["Hissabid"] + " And MasterCompanyId=" + Session["varCompanyId"];
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Text = "Duplicate bill no exists..";
            Textbillno.Text = "";
            Textbillno.Focus();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Duplicate bill no exists..!');", true);
        }
    }
    protected void DGIndentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGIndentDetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGIndentDetail.Columns.Count; i++)
            {
                if (DGIndentDetail.Columns[i].HeaderText == "Total Amount")
                {
                    if (Session["varcompanyId"].ToString() == "42")
                    {
                        DGIndentDetail.Columns[i].Visible = false;
                    }
                    else
                    {
                        DGIndentDetail.Columns[i].Visible = true;
                    }
                }
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Str = "";
        string Str1 = "";

        string Str2 = "select * from ProcessHissabApproved PA,ProcessHissabApprovedDetail PAD  where PA.ID=PAD.ID And PA.CompanyId=" + DDCompanyName.SelectedValue + @"
               And PA.EmpId=" + DDPartyName.SelectedValue + " And PA.ProcessId=" + DDProcess.SelectedValue + " And PAD.HissabId=" + ViewState["Hissabid"] + "";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str2);
        if (ds.Tables[0].Rows.Count > 0)
        {
            MessageSave("Bill No. Already Approved ......");
            return;
        }
        LblErrorMessage.Text = "";
        CheckDuplicateBillNo();
        if (LblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                SqlParameter[] _arrPara = new SqlParameter[22];

                _arrPara[0] = new SqlParameter("@HissabId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Companyid", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@Processid", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@PartyID", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@BillNo", SqlDbType.NVarChar);
                _arrPara[5] = new SqlParameter("@Amount", SqlDbType.Float);
                _arrPara[6] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                _arrPara[7] = new SqlParameter("@Remark", SqlDbType.NVarChar);
                _arrPara[8] = new SqlParameter("@Userid", SqlDbType.Int);
                _arrPara[9] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@Str", SqlDbType.NVarChar);
                _arrPara[11] = new SqlParameter("@Str1", SqlDbType.NVarChar);
                _arrPara[12] = new SqlParameter("@ConsumedDyes", SqlDbType.Float);
                _arrPara[13] = new SqlParameter("@vat", SqlDbType.Float);
                _arrPara[14] = new SqlParameter("@Sat", SqlDbType.Float);
                _arrPara[15] = new SqlParameter("@TotalAmt", SqlDbType.Float);
                _arrPara[16] = new SqlParameter("@DebitAmt", SqlDbType.Float);
                _arrPara[17] = new SqlParameter("@Hissabtype", SqlDbType.TinyInt);
                _arrPara[18] = new SqlParameter("@AdditionAmt", SqlDbType.Float);
                _arrPara[19] = new SqlParameter("@DeductionAmt", SqlDbType.Float);
                _arrPara[20] = new SqlParameter("@Gst", SqlDbType.Float);
                _arrPara[21] = new SqlParameter("@Msg", SqlDbType.NVarChar, 100);

                _arrPara[0].Direction = ParameterDirection.InputOutput;
                _arrPara[0].Value = ViewState["Hissabid"];
                _arrPara[1].Value = DDCompanyName.SelectedValue;
                _arrPara[2].Value = DDProcess.SelectedValue;
                _arrPara[3].Value = DDPartyName.SelectedValue;
                _arrPara[4].Value = Textbillno.Text;
                _arrPara[5].Value = Textpamt.Text != "" ? Convert.ToDouble(Textpamt.Text) : 0;
                _arrPara[6].Value = TxtDate.Text;
                _arrPara[7].Value = txtremark.Text;
                _arrPara[8].Value = Session["varuserid"];
                _arrPara[9].Value = Session["varCompanyId"];
                for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
                {
                    if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        if (Str == "")
                        {
                            Str = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                            Str1 = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;
                        }
                        else
                        {
                            Str = Str + ',' + ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                            Str1 = Str1 + ',' + ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;
                        }
                    }
                }
                _arrPara[10].Value = Str;
                _arrPara[11].Value = Str1;
                _arrPara[12].Value = txtConsDyes.Text != "" ? txtConsDyes.Text : "0";
                _arrPara[13].Value = txtvat.Text != "" ? txtvat.Text : "0";
                _arrPara[14].Value = txtsat.Text != "" ? txtsat.Text : "0";
                _arrPara[15].Value = Txttotamt.Text != "" ? Txttotamt.Text : "0";
                _arrPara[16].Value = txtDebitamt.Text != "" ? txtDebitamt.Text : "0";
                _arrPara[17].Value = chksample.Checked == true ? "1" : "0";

                _arrPara[18].Value = txtAdditionAmt.Text != "" ? txtAdditionAmt.Text : "0";
                _arrPara[19].Value = txtDeductionAmt.Text != "" ? txtDeductionAmt.Text : "0";
                _arrPara[20].Value = txtGst.Text != "" ? txtGst.Text : "0";
                _arrPara[21].Direction = ParameterDirection.InputOutput;
                string sp = string.Empty;
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    sp = "PRO_RawMaterialPreprationHissab_AGNI";
                }
                else
                {

                    sp = "PRO_RawMaterialPreprationHissab";
                }
               
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, sp, _arrPara);
                ViewState["Hissabid"] = _arrPara[0].Value;
                Tran.Commit();
                if (_arrPara[21].Value.ToString() != "")
                {
                    Msg = _arrPara[21].Value.ToString();
                    MessageSave(Msg);
                    LblErrorMessage.Text = _arrPara[21].Value.ToString();
                }
                else
                {
                    refresh();
                    Msg = "Record(s) has been saved successfully!";
                    MessageSave(Msg);
                    LblErrorMessage.Text = "Data Saved Successfully";
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Hissab/purchase_hisab.aspx");
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
    private void refresh()
    {
        txtremark.Text = "";
        Txttotamt.Text = "";
        txtDebitamt.Text = "0";
        Textbillno.Text = "";
        Textpamt.Text = "";
        txtConsDyes.Text = "";
        txtvat.Text = "";
        txtsat.Text = "";
        txtAdditionAmt.Text = "";
        txtDeductionAmt.Text = "";
        txtGst.Text = "";
        fillgrid();
    }
    //protected void DGIndentDetail_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    //protected void DGBILLDETAIL_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    protected void DDBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Hissabid"] = DDBillNo.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select BillNo,Round(Amount,2) Amount,replace(convert(varchar(11),Date,106), ' ','-') as Date,Remark,ConsumedDyes,Vat,Sat,Round(DebitAmt,2) DebitAmt,isnull(AdditionAmt,0) as AdditionAmt,isnull(DeductionAmt,0) as DeductionAmt,isnull(Gst,0) as Gst From RawMaterialPreprationHissab Where Hissabid=" + DDBillNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Textbillno.Text = Ds.Tables[0].Rows[0]["BillNo"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            Textpamt.Text = Ds.Tables[0].Rows[0]["Amount"].ToString();
            txtremark.Text = Ds.Tables[0].Rows[0]["Remark"].ToString();
            txtConsDyes.Text = Ds.Tables[0].Rows[0]["ConsumedDyes"].ToString();
            txtvat.Text = Ds.Tables[0].Rows[0]["vat"].ToString();
            txtsat.Text = Ds.Tables[0].Rows[0]["sat"].ToString();
            txtDebitamt.Text = Ds.Tables[0].Rows[0]["DebitAmt"].ToString();
            txtAdditionAmt.Text = Ds.Tables[0].Rows[0]["AdditionAmt"].ToString();
            txtDeductionAmt.Text = Ds.Tables[0].Rows[0]["DeductionAmt"].ToString();
            txtGst.Text = Ds.Tables[0].Rows[0]["Gst"].ToString();
        }
        fillgrid();
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            TDBillNo.Visible = true;
            PartyNameSelectedChange();
            BtnSave.Visible = false;
        }
        else
        {
            TDBillNo.Visible = false;
            BtnSave.Visible = true;
        }
        fillgrid();
        Txttotamt.Text = "";
        Textbillno.Text = "";
        Textpamt.Text = "";
        txtremark.Text = "";
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void Fill_Amount()
    {
        Double ConDyesAmount = 0;
        Double VatAmount = 0;
        Double SatAmount = 0;
        if (txtConsDyes.Text != "")
        {
            ConDyesAmount = Convert.ToDouble(Txttotamt.Text) * Convert.ToDouble(txtConsDyes.Text) / 100;
        }
        if (txtvat.Text != "")
        {
            VatAmount = ConDyesAmount * Convert.ToDouble(txtvat.Text) / 100;
        }
        if (txtsat.Text != "")
        {
            SatAmount = ConDyesAmount * Convert.ToDouble(txtsat.Text) / 100;
        }
        if (ConDyesAmount != 0 || VatAmount != 0 || SatAmount != 0)
        {
            Textpamt.Text = Math.Round((Convert.ToDouble(Txttotamt.Text) + VatAmount + SatAmount), 0).ToString();
        }
        else
        {
            Textpamt.Text = Txttotamt.Text;
            //Textpamt.Text = (Convert.ToDouble(Textpamt.Text) - Convert.ToDouble(txtDebitamt.Text)).ToString();
        }
        Textpamt.Text = (Convert.ToDouble(Textpamt.Text) - Convert.ToDouble(txtDebitamt.Text)).ToString();
    }
    protected void txtConsDyes_TextChanged(object sender, EventArgs e)
    {
        Fill_Amount();
        txtvat.Focus();
    }
    protected void txtvat_TextChanged(object sender, EventArgs e)
    {
        Fill_Amount();
        txtsat.Focus();
    }
    protected void txtsat_TextChanged(object sender, EventArgs e)
    {
        Fill_Amount();
    }
    protected void chksample_CheckedChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    protected void Btnpreview_Click(object sender, EventArgs e)
    {
        string str = "";
        DataSet ds = new DataSet();
        if (chksample.Checked == true)
        {
            str = @"select VH.Companyname,Vh.CompAddr1,Vh.CompAddr2,Vh.CompAddr3,vh.CompTel,
                    VRH.BillNo,VH.ReceiveDate,VH.ChallanNo as RecchallanNo,VH.EmpName,vh.ITEM_NAME,vh.QualityName,vh.designName,vh.ColorName,vh.ShadeColorName,
                    vh.ShapeName,vh.Size,Vh.Recqty,Vh.Lossqty,vh.rate,vh.Total,Vh.Processrec_prmid,VH.Process_Name,VRH.AdditionAmt,VRH.DeductionAmt,VRH.GST
                    From V_sampleHissabDetail VH  inner join V_RawMaterialHissabDetail VRH
                    on VH.ProcessRec_Prmid=VRH.ProcessRec_PrmId and Vh.Indentid=Vrh.IndentID
                    Where VRH.Hissabtype=1 and VRH.Hissabid=" + ViewState["Hissabid"];
            Session["rptFileName"] = "~\\Reports\\rptSampleBillDetails.rpt";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        }
        else
        {
            if (Convert.ToInt32(Session["varCompanyId"]) == 16 || Convert.ToInt32(Session["varCompanyId"]) == 28)
            {
//                str = @" Select CI.Companyname, CI.CompAddr1, CI.CompAddr2, CI.CompAddr3, CI.CompTel, a.BillNo, a.Date ReceiveDate, EI.EmpName, '' ITEM_NAME, '' QualityName, '' designName, '' ColorName, '' ShadeColorName,
//                        '' ShapeName, '' Size, 0 Recqty, 0 Lossqty, 0 rate, a.TotalAmt Total,'' Processrec_prmid, PNM.Process_Name, a.AdditionAmt, a.DeductionAmt, a.GST, 
//                        (Select Distinct IM.IndentNo + '/' + PRM.ChallanNo + ', '
//		                        From RawMaterialPreprationHissabDetail b(nolock) 
//		                        JOIN IndentMaster IM(Nolock) ON IM.IndentID = b.IndentID 
//		                        JOIN PP_ProcessRecMaster PRM(Nolock) ON PRM.PRMID = b.ProcessRec_PrmId
//		                        Where b.HissabId = a.HissabId For XML Path('')) RecchallanNo
//                        From RawMaterialPreprationHissab a(nolock) 
//                        JOIN CompanyInfo CI ON CI.CompanyId = a.Companyid 
//                        JOIN EmpInfo EI ON EI.EmpId = a.PartyID 
//                        JOIN PROCESS_NAME_MASTER PNM ON PNM.PROCESS_NAME_ID = a.Processid 
//                        Where a.HissabType = 0 And a.HissabId = " + ViewState["Hissabid"];
            
//                Session["rptFileName"] = "~\\Reports\\rptBillDetailsNew.rpt";

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@HissabID", ViewState["Hissabid"]);

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialPreprationHissab", param);

                Session["rptFileName"] = "~\\Reports\\rptBillDetailsNewChampo.rpt";
            }
            else if (Convert.ToInt32(Session["varCompanyId"]) == 42)
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@HissabID", ViewState["Hissabid"]);

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRawMaterialPreprationHissabReportVikramMirzapur", param);

                Session["rptFileName"] = "~\\Reports\\rptBillDetailsNewVikramMirzapur.rpt";
            }
            else
            {
                str = @" Select VH.Companyname,Vh.CompAddr1,Vh.CompAddr2,Vh.CompAddr3,vh.CompTel,
                    VRH.BillNo,VH.ReceiveDate,VH.ChallanNo as RecchallanNo,VH.EmpName,vh.ITEM_NAME,vh.QualityName,vh.designName,vh.ColorName,vh.ShadeColorName,
                    vh.ShapeName,vh.Size,Vh.Recqty,Vh.Lossqty,vh.rate,vh.Total,Vh.Processrec_prmid,VH.Process_Name,VRH.AdditionAmt,VRH.DeductionAmt,VRH.GST,
                    VRH.DebitAmt,VH.unitname,VH.CustomerOrderNo,VH.CustomerCode,VRH.TDS 
                    From V_GetRawmaterialPreparationHissabDetail VH  inner join V_RawMaterialHissabDetail VRH
                    on VH.ProcessRec_Prmid=VRH.ProcessRec_PrmId and Vh.Indentid=Vrh.IndentID
                    Where VRH.Hissabtype=0 and VRH.Hissabid=" + ViewState["Hissabid"];
                if (Session["VarCompanyNo"].ToString() == "44")
                {
                    Session["rptFileName"] = "~\\Reports\\rptBillDetails_agni.rpt";
                }
                else {
                    Session["rptFileName"] = "~\\Reports\\rptBillDetails.rpt";
                }
                
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            }            
        }
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptbilldetails.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
        }
    }
    protected void btnbilldetails_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
        param[1] = new SqlParameter("@fromdate", txtfromdate.Text);
        param[2] = new SqlParameter("@Todate", txttodate.Text);
        param[3] = new SqlParameter("@Dyerid", DDPartyName.SelectedIndex > 0 ? DDPartyName.SelectedValue : "0");
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getDyerBillDetails", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["dsFilename"] = "~\\ReportSchema\\Rptdyerbilldetails.xsd";
            Session["rptFilename"] = "Reports/Rptdyerbilldetails.rpt";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Records found...')", true);

        }


    }
    protected void Fill_AmountWithGst()
    {
        Double Deduction = 0;
        Double Addition = 0;
        Double Gst = 0;
        Double totalamt = Convert.ToDouble(Txttotamt.Text);
        Double debitnote = Convert.ToDouble(txtDebitamt.Text == "" ? "0" : txtDebitamt.Text);

        if (txtAdditionAmt.Text != "")
        {
            Addition = Convert.ToDouble(txtAdditionAmt.Text == "" ? "0" : txtAdditionAmt.Text);
        }
        if (txtDeductionAmt.Text != "")
        {
            Deduction = Convert.ToDouble(txtDeductionAmt.Text == "" ? "0" : txtDeductionAmt.Text);
        }

        totalamt = totalamt + Addition - Deduction;

        if (txtGst.Text != "")
        {
            Gst = (totalamt) * Convert.ToDouble(txtGst.Text == "" ? "0" : txtGst.Text) / 100;

        }

        //if (Addition != 0 || Deduction != 0 || Gst != 0)
        //{
        //    Textpamt.Text = Math.Round((Convert.ToDouble(Txttotamt.Text) + Addition - Deduction + Gst), 0).ToString();
        //}
        //else
        //{
        //    Textpamt.Text = Txttotamt.Text;
        //    //Textpamt.Text = (Convert.ToDouble(Textpamt.Text) - Convert.ToDouble(txtDebitamt.Text)).ToString();
        //}

        Textpamt.Text = Math.Round((totalamt + Gst - debitnote), 0).ToString();
    }
    protected void txtAdditionAmt_TextChanged(object sender, EventArgs e)
    {
        Fill_AmountWithGst();
    }
    protected void txtDeductionAmt_TextChanged(object sender, EventArgs e)
    {
        Fill_AmountWithGst();
    }
    protected void txtGst_TextChanged(object sender, EventArgs e)
    {
        Fill_AmountWithGst();
    }
    protected void BtnIndentPreview_Click(object sender, EventArgs e)
    {
        string str = "";
        string IndentID = "";
        string PRMID = "";
        for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                if (IndentID == "")
                {
                    IndentID = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                    PRMID = ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;
                }
                else
                {
                    IndentID = IndentID + ',' + ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;
                    PRMID = PRMID + ',' + ((Label)DGIndentDetail.Rows[i].FindControl("lblProcessRec_PrmId")).Text;
                }
            }
        }

        if (IndentID == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
            return;
        }
        str = @"select Process_Name,case When " + DDCompanyName.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,
                IndentNo,PM.GatePassNo As GateNo,ChallanNo,PM.Date,Finishedid,LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName 
                As Description,case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch Else Sizemtr End End End As Size,
                Sum(issueQuantity-isnull(canQty,0)) As IssueQty,0 As RecQty,0 As LossQty,0 As RetQty,'20-Apr-2020 ' As FromDate,'20-Apr-2020' As ToDate,
                0 As dateflag,OM.LocalOrder,OM.CustomerOrderNo,0 as Lshort,0 as shrinkage,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,
                case when IM.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,0 AS UNDYEDQTY
                From PP_ProcessRawMaster PM INNER JOIN PP_ProcessRawTran PT ON PM.PrmId=PT.PrmId
                INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentId
                INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.Item_Finished_Id
                INNER JOIN Empinfo E ON Pm.EmpId=E.EmpId
                INNER JOIN OrderMaster OM ON OM.Orderid=IM.OrderId
                INNER JOIN Companyinfo CI ON PM.CompanyId=CI.CompanyId
                INNER JOIN Process_Name_Master PNM ON PM.ProcessId=PNM.PROCESS_NAME_ID
                Where PM.MasterCompanyId=" + Session["varCompanyId"] + " And IM.CompanyId=" + DDCompanyName.SelectedValue + " And PM.Processid=" + DDProcess.SelectedValue + "  And PM.EmpId= " + DDPartyName.SelectedValue;
        if (IndentID != "")
        {
            str = str + " And PT.IndentID in (" + IndentID + ")";
        }
        str = str + @" group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GatePassNo,ChallanNo,PM.Date,
            Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,
            OM.LocalOrder,OM.CustomerOrderNo,VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,IM.Re_Process   

        Union All   

        Select Process_Name,case When " + DDCompanyName.SelectedIndex + @">0 Then CI.CompanyName Else 'ALL' End As CompanyName,Empname,IM.IndentId,IndentNo,PM.GateINNo As GateNo,
        PM.ChallanNo,PM.Date,Finishedid,LotNo,CATEGORY_NAME+' '+ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName As Description,
        case When unitId=1 Then Sizemtr Else Case When UnitId=2 Then Sizeft Else case When UnitId=6 Then Sizeinch Else Sizemtr End End End As Size, 
        0 As IssueQty,SUM(CASE WHEN REC_ISS_ITEMFLAG=0 THEN RECQUANTITY ELSE 0 END) As RecQty,Sum(LossQty) As LossQty,isnull(Sum(RetQty),0) As RetQty,'" + TxtDate.Text + @"' As FromDate,
        '" + TxtDate.Text + @"' As ToDate,0  As dateflag,OM.LocalOrder,OM.CustomerOrderNo,isnull(Sum(PT.Lshort),0) as Lshort,isnull(sum(shrinkage),0) as Shrinkage,
        VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,case when ID.Re_Process=1 then 'Re-Dyeing' else 'Dyeing' end as Re_Process,
        SUM(CASE WHEN REC_ISS_ITEMFLAG=1 THEN RECQUANTITY ELSE 0 END) AS UNDYEDQTY 
        FROM PP_ProcessRecMaster PM 
        inner join PP_ProcessRecTran PT on PM.PRMid=PT.PRMid
        INNER JOIN V_Indent_OredrId IM ON PT.IndentId=IM.IndentID
        INNER JOIN OrderMaster OM ON OM.OrderId=IM.OrderId
        INNER JOIN V_FinishedItemDetail VF ON PT.Finishedid=VF.ITEM_FINISHED_ID
        INNER JOIN EmpInfo E on PM.Empid=E.EmpId
        INNER JOIN CompanyInfo CI on IM.CompanyId=CI.CompanyId
        inner join PROCESS_NAME_MASTER PNM on PM.processid=PNM.PROCESS_NAME_ID
        inner join PP_ProcessRawMaster PRM ON PRM.prmId=PT.IssPrmId
        left outer Join V_IndentRawReturnQty V  on V.prmId=Pt.PrmId and V.prtId=PT.prtId
        INNER JOIN V_REPROCESSINDENTID ID ON PT.IndentId=ID.IndentId 
        Where PRM.MasterCompanyId=" + Session["varCompanyId"] + " And IM.CompanyId = " + DDCompanyName.SelectedValue + " And PM.Processid=" + DDProcess.SelectedValue + "  And PM.EmpId= " + DDPartyName.SelectedValue;
        if (IndentID != "")
        {
            str = str + " And PT.IndentID in (" + IndentID + ")";
        }
        str = str + @" group by Process_Name,CI.CompanyName,Empname,IM.IndentId,IndentNo,PM.GateINNo,PM.ChallanNo,PM.Date,
        Finishedid,Lotno,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,unitId,Sizemtr,Sizeft,Sizeinch,OM.LocalOrder,OM.CustomerOrderNo,
        VF.ITEM_ID,VF.QualityId,VF.ShadecolorId,ID.Re_Process";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["Getdataset"] = ds;
            Session["rptFilename"] = "Reports/RptIndentRawIss_RecDetailIndentWise.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\RptIndentRawIss_RecDetailIndentWise.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
        }
    }
    protected void BtnDeductionAmountDetail_Click(object sender, EventArgs e)
    {
        string IndentIDs = "";
        if ((Convert.ToInt32(Session["varCompanyId"]) == 16) || (Convert.ToInt32(Session["varCompanyId"]) == 28))
        {
            for (int i = 0; i < DGIndentDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGIndentDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    string indentid = ((Label)DGIndentDetail.Rows[i].FindControl("lblIndentId")).Text;

                    if (IndentIDs == "")
                    {
                        IndentIDs = indentid + "|";
                    }
                    else
                    {
                        IndentIDs = IndentIDs + indentid + "|";
                    }
                }
            }
            if (IndentIDs != "")
            {
                SqlParameter[] _array = new SqlParameter[3];
                _array[0] = new SqlParameter("@IndentId", SqlDbType.NVarChar);
                _array[1] = new SqlParameter("@DeductionAmount", SqlDbType.Float);
                _array[2] = new SqlParameter("@TypeID", SqlDbType.Int);

                _array[0].Value = IndentIDs;
                _array[1].Direction = ParameterDirection.Output;
                _array[2].Value = 1;

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDyingDeductionAmountForShortQty", _array);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["Getdataset"] = ds;
                    Session["rptFilename"] = "Reports/RptIndentBalQtyAmountDetail.rpt";
                    Session["dsFileName"] = "~\\ReportSchema\\RptIndentBalQtyAmountDetail.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No Records found')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please select atleast one check box')", true);
            }
        }
    }
}