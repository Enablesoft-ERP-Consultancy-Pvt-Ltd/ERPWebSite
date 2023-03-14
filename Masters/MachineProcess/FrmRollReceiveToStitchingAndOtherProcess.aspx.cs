using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollReceiveToStitchingAndOtherProcess : System.Web.UI.Page
{
    string str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(nolock)
            JOIN Company_Authentication CA(nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CI.Companyname 

            Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From PROCESS_NAME_MASTER PNM(nolock) 
            Where PNM.MasterCompanyid = " + Session["varcompanyid"] + @" Order By PNM.Process_Name_ID ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");

            if (DDProcessName.Items.Count > 0)
            {
                if (Convert.ToInt32(Session["varcompanyid"]) == 21)
                {
                    DDProcessName.SelectedValue = "17";
                }
                else
                {
                    DDProcessName.SelectedIndex = 1;
                }
                DDProcessName_SelectedIndexChanged(sender, new EventArgs());
            }
            TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnReceiveID.Value = "0";
        }
    }

    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollIssueOtherProcessMatser a(Nolock)
            JOIN Empinfo EI ON EI.EmpID = a.EmpID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            Order By EI.EmpName ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "Select Emp Name");

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblRollIssueOtherProcessID = ((Label)DG.Rows[i].FindControl("LblRollIssueOtherProcessID"));
                Label LblRollIssueOtherProcessDetailID = ((Label)DG.Rows[i].FindControl("LblRollIssueOtherProcessDetailID"));
                TextBox TxtRejectPcs = ((TextBox)DG.Rows[i].FindControl("TxtRejectPcs"));
                if (DetailData == "")
                {
                    DetailData = LblRollIssueOtherProcessID.Text + "|" + LblRollIssueOtherProcessDetailID.Text + "|" + TxtRejectPcs.Text + "~";
                }
                else
                {
                    DetailData = DetailData + LblRollIssueOtherProcessID.Text + "|" + LblRollIssueOtherProcessDetailID.Text + "|" + TxtRejectPcs.Text + "~";
                }
            }
        }
        if (DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[11];
            arr[0] = new SqlParameter("@RollReceiveOtherProcessID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@EmpID", SqlDbType.Int);
            arr[4] = new SqlParameter("@RollIssueOtherProcessID", SqlDbType.Int);
            arr[5] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
            arr[6] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            arr[7] = new SqlParameter("@DetailData", SqlDbType.NVarChar);
            arr[8] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[9] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnReceiveID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Value = DDEmployeeName.SelectedValue;
            arr[4].Value = DDIssueNo.SelectedValue;
            arr[5].Direction = ParameterDirection.InputOutput;
            arr[5].Value = TxtReceiveNo.Text;
            arr[6].Value = TxtReceiveDate.Text;
            arr[7].Value = DetailData;
            arr[8].Value = Session["varuserid"];
            arr[9].Value = Session["varCompanyId"];
            arr[10].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollReceiveOtherProcess]", arr);

            if (arr[10].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[10].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnReceiveID.Value = arr[0].Value.ToString();
                TxtReceiveNo.Text = Convert.ToString(arr[5].Value);
                tran.Commit();
            }
            FillDGGrid();
            fill_grid();
            btnPreview.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label LblRollReceiveOtherProcessID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveOtherProcessID");
            Label LblRollReceiveOtherProcessDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveOtherProcessDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollReceiveOtherProcessID", LblRollReceiveOtherProcessID.Text);
            param[1] = new SqlParameter("@RollReceiveOtherProcessDetailID", LblRollReceiveOtherProcessDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollReceiveOtherProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            FillDGGrid();
            fill_grid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDReceiveNo.Items.Clear();
        DDIssueNo.Items.Clear();
        TxtReceiveNo.Text = "";
        TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Td3.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            EditCheckedChanged();
            Td3.Visible = true;
            btnPreview.Visible = true;
        }
    }
    private void EditCheckedChanged()
    {
        HnReceiveID.Value = "0";
        TxtReceiveNo.Text = "";
        //string str = @"";

        string str = @"Select Distinct EI.EmpID, EI.EmpName 
            From RollReceiveOtherProcessMatser a(Nolock)
            JOIN Empinfo EI ON EI.EmpID = a.EmpID 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @"
            Order By EI.EmpName ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDEmployeeName, ds, 0, true, "-Select Employee-");
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        string Str = @"Select a.RollReceiveOtherProcessID, b.RollReceiveToNextDetailID RollNo, CI.CompanyName, CI.CompAddr1 + ', ' + CI.CompAddr2 + ', ' + CI.CompAddr3 CompanyAddress, 
            CI.CompTel, CI.GSTNo, PNM.PROCESS_NAME ProcessName, a.ReceiveNo, 
            REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate, U1.UnitName, 
            VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName ItemDescription, 
            OM.CustomerOrderNo OrderNo, b.Qty-b.RejectQty RecQty 
            From RollReceiveOtherProcessMatser a(Nolock) 
            JOIN RollReceiveOtherProcessDetail b(Nolock) ON b.RollReceiveOtherProcessID = a.RollReceiveOtherProcessID 
            JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            JOIN Unit U1(Nolock) ON U1.UnitId = b.UnitID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID 
            Where a.RollReceiveOtherProcessID = " + HnReceiveID.Value + @" 
            Order By b.RollReceiveOtherProcessDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollReceiveToStitchingAndOtherProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollReceiveToStitchingAndOtherProcess.xsd";
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
    protected void DDIssue_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_grid();

       
    }
    private void fill_grid()
    {
        string str = @"Select a.RollIssueOtherProcessID, b.RollIssueOtherProcessDetailID, b.RollReceiveToNextDetailID, b.Qty, U.UnitName, OM.CustomerOrderNo OrderNo, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, 0 RejectPcs  
            From RollIssueOtherProcessMatser a(Nolock)
            JOIN RollIssueOtherProcessDetail b(Nolock) ON b.RollIssueOtherProcessID = a.RollIssueOtherProcessID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = b.OrderID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID 
            LEFT JOIN RollReceiveOtherProcessDetail c(Nolock) ON c.RollIssueOtherProcessID = a.RollIssueOtherProcessID And c.RollIssueOtherProcessDetailID = b.RollIssueOtherProcessDetailID 
            Where c.RollIssueOtherProcessID Is null And a.RollIssueOtherProcessID = " + DDIssueNo.SelectedValue + @" 
            Order By b.RollIssueOtherProcessDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DDEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select Distinct a.RollIssueOtherProcessID, a.IssueNo 
            From RollIssueOtherProcessMatser a(Nolock)
            JOIN RollIssueOtherProcessDetail b(Nolock) ON b.RollIssueOtherProcessID = a.RollIssueOtherProcessID 
            LEFT JOIN RollReceiveOtherProcessDetail c(Nolock) ON c.RollIssueOtherProcessID = a.RollIssueOtherProcessID And c.RollIssueOtherProcessDetailID = b.RollIssueOtherProcessDetailID 
            Where c.RollIssueOtherProcessID Is null And a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.EmpID = " + DDEmployeeName.SelectedValue + " Order By a.RollIssueOtherProcessID Desc ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");

        if (ChKForEdit.Checked == true)
        {
            string str2 = @" Select distinct a.RollReceiveOtherProcessID, a.ReceiveNo 
                From RollReceiveOtherProcessMatser a(Nolock) JOIN RollReceiveOtherProcessDetail b(Nolock) ON a.RollReceiveOtherProcessID=b.RollReceiveOtherProcessID
                Where a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + " And a.EmpID = " + DDEmployeeName.SelectedValue + @"                  
                Order By a.RollReceiveOtherProcessID";

            DataSet ds2 = SqlHelper.ExecuteDataset(str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDReceiveNo, ds2, 0, true, "-Select Issue No-");
        }
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        string str = @"Select a.RollReceiveOtherProcessID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate            
            From RollReceiveOtherProcessMatser a(Nolock) 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            And a.RollReceiveOtherProcessID = " + DDReceiveNo.SelectedValue + @"
            Order By a.RollReceiveOtherProcessID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
            TxtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            HnReceiveID.Value = ds.Tables[0].Rows[0]["RollReceiveOtherProcessID"].ToString();           
        }
        FillDGGrid();

//        string str = @"Select a.RollIssueOtherProcessID, a.IssueNo, REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate 
//            From RollIssueOtherProcessMatser a(Nolock) 
//            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
//            And a.RollIssueOtherProcessID = " + DDIssueNo.SelectedValue + @"
//            Order By a.RollIssueOtherProcessID Desc";

//        DataSet ds = SqlHelper.ExecuteDataset(str);
//        if (ds.Tables[0].Rows.Count > 0)
//        {
//            TxtReceiveNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
//            TxtReceiveDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
//            HnReceiveID.Value = ds.Tables[0].Rows[0]["RollIssueOtherProcessID"].ToString();
//        }
    }

    private void FillDGGrid()
    {
        string str = @"Select b.RollReceiveOtherProcessID, b.RollReceiveOtherProcessDetailID, b.RollReceiveToNextDetailID, U.UnitName, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription, b.Qty - b.RejectQty Qty 
            From RollReceiveOtherProcessMatser a(Nolock)
            JOIN RollReceiveOtherProcessDetail b(Nolock) ON b.RollReceiveOtherProcessID = a.RollReceiveOtherProcessID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID 
            Where a.RollReceiveOtherProcessID = " + HnReceiveID.Value + @"  
            Order By b.RollReceiveOtherProcessDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
}
