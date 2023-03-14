using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Hissab_FrmAdvancePaymentFolioWise : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            string Str = @"Select CI.CompanyId,CompanyName 
                        From CompanyInfo CI(Nolock)
                        JOIN Company_Authentication CA (Nolock) ON CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserid"] + @" 
                        Where CI. MasterCompanyId=" + Session["varCompanyId"] + @" order by CI.CompanyId 
                        Select PROCESS_NAME_ID, PROCESS_NAME 
                        From PROCESS_NAME_MASTER (Nolock) 
                        Where MasterCompanyid = " + Session["varCompanyId"] + @" and Process_Name='Weaving' Order By PROCESS_NAME";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "--SELECT--");
            if (DDCompanyName.Items.FindByValue(Session["CurrentWorkingCompanyID"].ToString()) != null)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, Ds, 1, true, "--SELECT--");
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @" Select EI.EmpId, EI.EmpName + case when isnull(ei.empcode,'')<>'' then ' ['+ EI.empcode+']' else '' end + case when ei.Address<>'' then ' / '+ei.Address else '' end empname 
                        From EmpInfo EI(nolock) 
                        JOIN EmpProcess EP(nolock) ON EP.EmpID = EI.EmpID And EP.processID = " + DDProcessName.SelectedValue; 

            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                Str = Str + " Where ei.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }
            Str = Str + " order by ei.empname";

            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDEmployerName, Ds, 0, true, "--SELECT--");

    }
    protected void DDEmployerName_SelectedIndexChanged(object sender, EventArgs e)
    { 

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_BindFolioIssueNoForAdvancePayment", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 3000;

        cmd.Parameters.AddWithValue("@CompanyId", DDCompanyName.SelectedValue);
        cmd.Parameters.AddWithValue("@EMPID", DDEmployerName.SelectedValue);
        cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
        cmd.Parameters.AddWithValue("@Mastercompanyid", Session["VarCompanyNo"]);
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

        DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        UtilityModule.ConditionalComboFillWithDS(ref DDFolioNo, ds, 0, true, "--Plz Select--");
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DDPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        TxtAdvance.Text = "";
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[14];
            _array[0] = new SqlParameter("@ID", SqlDbType.Int);
            _array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
            _array[2] = new SqlParameter("@AdvanceAmt", SqlDbType.Float);
            _array[3] = new SqlParameter("@DeductAmt", SqlDbType.Float);
            _array[4] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            _array[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[7] = new SqlParameter("@UserId", SqlDbType.Int);
            _array[8] = new SqlParameter("@ChequeNo", SqlDbType.NVarChar, 250);
            _array[9] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 500);
            _array[10] = new SqlParameter("@MSG", SqlDbType.VarChar, 300);
            _array[11] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _array[12] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _array[13] = new SqlParameter("@PaymentType", SqlDbType.Int);

            _array[0].Direction = ParameterDirection.Output;
            _array[1].Value = DDEmployerName.SelectedValue;
            _array[2].Value = TxtAdvance.Text;
            _array[3].Value = 0;
            _array[4].Value = TxtDate.Text;
            _array[5].Value = Session["VarcompanyId"];
            _array[6].Value = DDCompanyName.SelectedValue;
            _array[7].Value = Session["VarUserId"];
            _array[8].Value = txtChequeno.Text;
            _array[9].Value = txtremarks.Text;
            _array[10].Direction = ParameterDirection.Output;
            _array[11].Value = DDProcessName.SelectedValue;
            _array[12].Value = DDFolioNo.SelectedValue;
            _array[13].Value = DDPaymentType.SelectedValue;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_AdvanceAmountByFolioNoWise", _array);
            TxtVoucherNo.Text = _array[0].Value.ToString();
            Tran.Commit();
            MessageSave(_array[10].Value.ToString());
            //MessageSave("Data Saved Successfully.......");
            TxtAdvance.Text = "";
            txtChequeno.Text = "";
            txtremarks.Text = "";
            FillGrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();

        }

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
    protected void FillGrid()
    {
        string str = @"select EmpName As Employee,AdvanceAmt,DeductAmt,RePlace(Convert(nvarchar(11),Date,106),' ','-') As Date,ID As VoucherNo,Hissab_VoucherNo,AD.ChequeNo,AD.Remarks,Processid,
                    isnull(PIM.ChallanNo,PIM.IssueOrderId) as FolioChallanNo,AD.IssueOrderId 
                    from AdvanceAmountByFolioNoWise AD(Nolock)
                    JOIN Empinfo EI(Nolock) ON EI.EmpId=AD.Empid 
                    JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM ON AD.IssueOrderId=PIM.IssueOrderId
                    Where AD.CompanyId=" + DDCompanyName.SelectedValue + " And AD.EmpId=" + DDEmployerName.SelectedValue + " And AD.ProcessID = " + DDProcessName.SelectedValue+@" 
                    And AD.PaymentType=" + DDPaymentType.SelectedValue + " And AD.IssueOrderId=" + DDFolioNo.SelectedValue;

        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
        }
        str = str + " Order by VoucherNo";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGAdvanceAmount.DataSource = ds.Tables[0];
        DGAdvanceAmount.DataBind();
    }
    protected void DGAdvanceAmount_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGAdvanceAmount, "Select$" + e.Row.RowIndex);
            for (int i = 0; i < DGAdvanceAmount.Columns.Count; i++)
            {

                if (Session["usertype"].ToString() == "1")
                {
                    if (DGAdvanceAmount.Columns[i].HeaderText == "Delete")
                    {
                        DGAdvanceAmount.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGAdvanceAmount.Columns[i].HeaderText == "Delete")
                    {
                        DGAdvanceAmount.Columns[i].Visible = false;
                    }
                }

                if (Session["VarCompanyNo"].ToString() == "42")
                {
                    if (DGAdvanceAmount.Columns[i].HeaderText == "Show")
                    {
                        DGAdvanceAmount.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGAdvanceAmount.Columns[i].HeaderText == "Show")
                    {
                        DGAdvanceAmount.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        string str = "";
        str = @"select CI.CompanyName,EmpName+Space(2)+'/'+EI.Address As Employee,AdvanceAmt,DeductAmt,RePlace(Convert(nvarchar(11),Date,106),' ','-') As Date,ID As VoucherNo,
                        Hissab_VoucherNo,AD.ChequeNo,AD.Remarks,isnull(EI.TypeId,0) as TdsType,
                        isnull((Select ISNULL(TDS,0) From TDS_Master TM Where TM.TYPEID=EI.TYPEID AND FromDate<=AD.Date And (EndDate is Null OR EndDate>=AD.Date)),0) as TDS,
                        isnull(PIM.ChallanNo,PIM.IssueOrderId) as FolioChallanNo,AD.IssueOrderId,isnull(EI.PanNo,'') as EmpPanNo  
                    from AdvanceAmountByFolioNoWise  AD(NoLock) JOIN EmpInfo EI ON EI.EmpId=AD.EmpId
                    JOIN CompanyInfo CI(NoLock) ON CI.CompanyId=AD.CompanyId
                    JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM(NoLock) ON AD.IssueOrderId=PIM.IssueOrderId
                    Where EI.EmpId=" + DDEmployerName.SelectedValue + " And AD.IssueOrderId=" + DDFolioNo.SelectedValue + " And AD.PaymentType=" + DDPaymentType.SelectedValue + @" 
                    And AD.CompanyId=" + DDCompanyName.SelectedValue;

        //if (Session["VarCompanyNo"].ToString() == "42")
        //{
           
        //}
//        else
//        {
//            str = @"select CI.CompanyName,EmpName+Space(2)+'/'+EI.Address As Employee,AdvanceAmt,DeductAmt,RePlace(Convert(nvarchar(11),Date,106),' ','-') As Date,ID As VoucherNo,
//                        Hissab_VoucherNo,AD.ChequeNo,AD.Remarks,isnull(EI.TypeId,0) as TdsType                        
//                    from AdvanceAmountByFolioNoWise  AD(NoLock) JOIN EmpInfo EI(NoLock) ON EI.EmpId=AD.EmpId
//                    JOIN CompanyInfo CI(NoLock) ON CI.CompanyId=AD.CompanyId
//                    JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM(NoLock) ON AD.IssueOrderId=PIM.IssueOrderId
//                    Where  EI.EmpId=" + DDEmployerName.SelectedValue + " And AD.IssueOrderId=" + DDFolioNo.SelectedValue + " And AD.CompanyId=" + DDCompanyName.SelectedValue;
//        }
        

        if (TxtVoucherNo.Text != "")
        {
            str = str + " And AD.ID=" + TxtVoucherNo.Text;
        }
        if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        {
            str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
        }

        str = str + " Order by VoucherNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDS.rpt";

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                if (DDPaymentType.SelectedValue == "1")
                {
                    Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDSNew.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptMaterialDeductionAmountFolioNoWise.rpt";
                }
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDS.rpt";
            }
            
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptAdvanceAmountFolioNoWiseWithTDS.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            MessageSave("No Record Found......");
        }
    }
    protected void btnsearchemp_Click(object sender, EventArgs e)
    {
        if (txtWeaverIdNoscan.Text != "")
        {
            string str = "select empid   From empinfo where empcode='" + txtWeaverIdNoscan.Text + "'";
            if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
            {
                str = str + " And USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDEmployerName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
                {
                    DDEmployerName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    DDEmployerName_SelectedIndexChanged(sender, new EventArgs());

                }
                txtWeaverIdNoscan.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('No Employee found on this Employee code.')", true);
                txtWeaverIdNoscan.Focus();
            }
        }
    }
    protected void lnkdel_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        LinkButton lnkdel = sender as LinkButton;
        GridViewRow gvr = lnkdel.NamingContainer as GridViewRow;
        rowindex = gvr.RowIndex;
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
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblErr.Text = "";
        if (DGAdvanceAmount.Rows.Count > 0)
        {

            if (variable.VarPAYMENTDEL_PWD == txtpwd.Text)
            {
                Rowdelete(rowindex, sender);
                Popup(false);
            }
            else
            {
                lblErr.Text = "Please Enter Correct Password..";
            }

        }
    }
    protected void Rowdelete(int rowindex, object sender = null)
    {
        lblErr.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = ((Label)DGAdvanceAmount.Rows[rowindex].FindControl("lblid"));
            Label lblprocessid = ((Label)DGAdvanceAmount.Rows[rowindex].FindControl("lblprocessid"));
            Label lblIssueOrderId = ((Label)DGAdvanceAmount.Rows[rowindex].FindControl("lblIssueOrderId"));

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@id", lblid.Text);
            param[1] = new SqlParameter("@Processid", lblprocessid.Text);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@IssueOrderId", lblIssueOrderId.Text);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEADAVANCEAMOUNTByFolioNoWise", param);
            if (param[4].Value.ToString() != "")
            {
                Tran.Rollback();
                lblErr.Text = param[4].Value.ToString();
            }
            else
            {
                Tran.Commit();
                lblErr.Text = "Record Deleted Successfully !";
            }

            FillGrid();

        }
        catch (Exception ex)
        {
            Tran.Rollback();

            lblErr.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BTNShowAmt_Click(object sender, EventArgs e)
    {
        //Determine the RowIndex of the Row whose Button was clicked.
        int rowIndex = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        ////Get the value of column from the DataKeys using the RowIndex.
        //int id = Convert.ToInt32(DGAdvanceAmount.DataKeys[rowIndex].Values[0]);

        Label lblid = ((Label)DGAdvanceAmount.Rows[rowIndex].FindControl("lblid"));
        int id = Convert.ToInt32(lblid.Text);

        string str = "";
        str = @"Select CI.CompanyName,EmpName+Space(2)+'/'+EI.Address As Employee,AdvanceAmt,DeductAmt,RePlace(Convert(nvarchar(11),Date,106),' ','-') As Date,ID As VoucherNo,
                        Hissab_VoucherNo,AD.ChequeNo,AD.Remarks,isnull(EI.TypeId,0) as TdsType,
                        isnull((Select ISNULL(TDS,0) From TDS_Master TM Where TM.TYPEID=EI.TYPEID AND FromDate<=AD.Date And (EndDate is Null OR EndDate>=AD.Date)),0) as TDS,
                        isnull(PIM.ChallanNo,PIM.IssueOrderId) as FolioChallanNo,AD.IssueOrderId,isnull(EI.PanNo,'') as EmpPanNo    
                    from AdvanceAmountByFolioNoWise  AD(NoLock) JOIN EmpInfo EI ON EI.EmpId=AD.EmpId
                    JOIN CompanyInfo CI(NoLock) ON CI.CompanyId=AD.CompanyId
                    JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @"  PIM(NoLock) ON AD.IssueOrderId=PIM.IssueOrderId
                    Where EI.EmpId=" + DDEmployerName.SelectedValue + " And AD.IssueOrderId=" + DDFolioNo.SelectedValue + @" 
                    And AD.PaymentType=" + DDPaymentType.SelectedValue + @"  And AD.CompanyId=" + DDCompanyName.SelectedValue + " And AD.ID=" + id;
       
        //if (Session["usertype"].ToString() == "5" && variable.HR_EMPLOYEE_SHOW_OR_NOT_USER_WISE == "1")
        //{
        //    str = str + " And EI.USER_WISE_EMPLOYEE_SHOW_OR_NOT_IN_HR = 1";
        //}

        str = str + " Order by VoucherNo";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDS.rpt";

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                if (DDPaymentType.SelectedValue == "1")
                {
                    Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDSNew.rpt";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\RptMaterialDeductionAmountFolioNoWise.rpt";
                }
                
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptAdvanceAmountFolioNoWiseWithTDS.rpt";
            }

            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptAdvanceAmountFolioNoWiseWithTDS.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            MessageSave("No Record Found......");
        }

    }
    protected void WeavingAdvanceAmtCalCulate()
    {
        lblErr.Text = "";
        decimal TotalAdvanceAmt = 0;
        decimal WeavingAmt = 0;
        decimal WeavingAmtAfterEightyPer = 0;
        string str = "";
        string str2 = "";

        str = @"Select Round(isnull(sum(Amount),0),2) as WeavingAmt  From Process_Issue_Master_" + DDProcessName.SelectedValue + @" PIM(Nolock)  
                        JOIN Process_Issue_Detail_" + DDProcessName.SelectedValue + @" PID(Nolock)  ON PIM.IssueOrderID=PID.IssueOrderId
                        Where PID.IssueOrderId=" + DDFolioNo.SelectedValue + @"  and PIM.CompanyId=" + DDCompanyName.SelectedValue + @"";

        str2 = @"select isnull(sum(AdvanceAmt),0) as AdvanceAmt
                    from AdvanceAmountByFolioNoWise AD(Nolock) 
                    Where AD.CompanyId=" + DDCompanyName.SelectedValue + " And AD.EmpId=" + DDEmployerName.SelectedValue + " And AD.ProcessID = " + DDProcessName.SelectedValue + @" 
                    And AD.PaymentType=" + DDPaymentType.SelectedValue + " And AD.IssueOrderId=" + DDFolioNo.SelectedValue; 
        

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            WeavingAmt =Convert.ToDecimal(ds.Tables[0].Rows[0]["WeavingAmt"]);

            WeavingAmtAfterEightyPer = Math.Round((WeavingAmt * 85 / 100), 2);
        }

        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            TotalAdvanceAmt = Convert.ToDecimal(ds2.Tables[0].Rows[0]["AdvanceAmt"]);            
        }

        if ((Convert.ToDecimal(TxtAdvance.Text=="" ? "0" : TxtAdvance.Text) + TotalAdvanceAmt) > WeavingAmtAfterEightyPer)
        {
            TxtAdvance.Text = "";
            lblErr.Text = "Total Advance Amount Can't Excess From 85% of Weaving Amt.";            
        }
    }
    protected void TxtAdvance_TextChanged(object sender, EventArgs e)
    {
        if (DDPaymentType.SelectedValue == "1")
        {
            WeavingAdvanceAmtCalCulate();
        }
    }
    protected void lnkVoucherPrint_Click(object sender, EventArgs e)
    {
        //lblErr.Text = "";
        //try
        //{
        //    LinkButton lnkVoucherPrint = sender as LinkButton;
        //    GridViewRow gvr = lnkVoucherPrint.NamingContainer as GridViewRow;
        //    rowindex = gvr.RowIndex;

        //    Label lblid = ((Label)DGAdvanceAmount.Rows[rowindex].FindControl("lblid"));

        //    SqlParameter[] param = new SqlParameter[5];
        //    param[0] = new SqlParameter("@ID", lblid.Text);
        //    param[1] = new SqlParameter("@processid", 0);
        //    param[2] = new SqlParameter("@PageName", "FrmAdvancePayment");
        //    param[3] = new SqlParameter("@ProcessTypeID", 0);
        //    param[4] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);

        //    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETVOUCHERDETAIL", param);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {

        //        Session["rptFileName"] = "~\\Reports\\RptVoucher.rpt";
        //        Session["GetDataset"] = ds;
        //        Session["dsFileName"] = "~\\ReportSchema\\RptVoucher.xsd";
        //        StringBuilder stb = new StringBuilder();
        //        stb.Append("<script>");
        //        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //    }
        //    else
        //    {
        //        MessageSave("No Record Found......");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblErr.Text = ex.Message;
        //}
    }
}