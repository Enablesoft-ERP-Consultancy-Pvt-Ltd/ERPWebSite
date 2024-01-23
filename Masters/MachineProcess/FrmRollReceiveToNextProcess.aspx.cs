using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollReceiveToNextProcess : System.Web.UI.Page
{
    string str = "";
    int varcombo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(nolock)
            JOIN Company_Authentication CA(nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CI.Companyname 

            Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
            From PROCESS_NAME_MASTER PNM(nolock) 
            JOIN RollIssueToNextProcessMaster a(nolock) ON a.ProcessID = PNM.Process_Name_ID 
            Where PNM.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + @" Order By PNM.Process_Name_ID ";

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
                DDProcessName.SelectedIndex = 1;
                ProcessNameSelectedIndexChanged();
            }
            //FillDGGrid();
            TxtReceiveDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnReceiveID.Value = "0";
        }
    }
    
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChanged();
    }

    private void ProcessNameSelectedIndexChanged()
    {
        string str = @"Select RollIssueToNextID, IssueNo 
            From RollIssueToNextProcessMaster(Nolock) 
            Where MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @" 
            Order By RollIssueToNextID Desc ";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");
    }

    protected void DDIssue_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDGGrid();
        if (ChKForEdit.Checked == true)
        {
            EditCheckedChanged();
        }
    }
    private void FillDGGrid()
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand("[Pro_getRollReceiveToNextProcess]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CompanyID", ddCompName.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@RollIssueToNextID", DDIssueNo.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            DG.DataSource = ds.Tables[0];
            DG.DataBind();

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "RollReceiveToNextProcess", "alert('No record found !!!')", true);
            //    return;
            //}
            //else
            //{
            //    DG.DataSource = ds.Tables[0];
            //    DG.DataBind();
            //}
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblRollIssueToNextDetailDetailID = ((Label)DG.Rows[i].FindControl("LblRollIssueToNextDetailDetailID"));
                TextBox TxtWidth = ((TextBox)DG.Rows[i].FindControl("TxtWidth"));
                TextBox TxtLength = ((TextBox)DG.Rows[i].FindControl("TxtLength"));
                TextBox TxtNoofPati = ((TextBox)DG.Rows[i].FindControl("TxtNoofPati"));
                TextBox TxtRejectPcs = ((TextBox)DG.Rows[i].FindControl("TxtRejectPcs"));
                Label LblUnitID = ((Label)DG.Rows[i].FindControl("LblUnitID"));
                Label LblOrderQty = ((Label)DG.Rows[i].FindControl("LblOrderQty"));

                if (DetailData == "")
                {
                    DetailData = LblRollIssueToNextDetailDetailID.Text + "|" + LblUnitID.Text + "|" + TxtWidth.Text + "|" + TxtNoofPati.Text + "|" + TxtRejectPcs.Text + "|" + TxtLength.Text + "|" + LblOrderQty.Text + "~";
                }
                else
                {
                    DetailData = DetailData + LblRollIssueToNextDetailDetailID.Text + "|" + LblUnitID.Text + "|" + TxtWidth.Text + "|" + TxtNoofPati.Text + "|" + TxtRejectPcs.Text + "|" + TxtLength.Text + "|" + LblOrderQty.Text + "~";
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
            SqlParameter[] arr = new SqlParameter[22];
            arr[0] = new SqlParameter("@RollReceiveToNextID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@ReceiveNo", SqlDbType.NVarChar, 50);
            arr[4] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("DetailData", SqlDbType.NVarChar);
            arr[6] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[7] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[8] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnReceiveID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Direction = ParameterDirection.InputOutput;
            arr[3].Value = TxtReceiveNo.Text;
            arr[4].Value = TxtReceiveDate.Text;
            arr[5].Value = DetailData;
            arr[6].Value = Session["varuserid"];
            arr[7].Value = Session["varMasterCompanyIDForERP"];
            arr[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollReceiveToNextProcess]", arr);

            if (arr[8].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[8].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnReceiveID.Value = arr[0].Value.ToString();
                TxtReceiveNo.Text = Convert.ToString(arr[3].Value);
                tran.Commit();
                FillDGGrid();
                fill_grid();
                btnPreview.Visible = true;
            }
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

    private void fill_grid()
    {
        string str = @"Select b.RollReceiveToNextDetailID, a.RollReceiveToNextID, b.MaterialReceiveInPcsID MainRollID, b.MaterialReceiveInPcsDetailID SubRollID, b.RollReceiveToNextDetailID SubSubRollID, 
            OM.CustomerOrderNo OrderNo, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName MainRollDescription, 
            VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.ShadeColorName + 
            Cast(b.SplitWidth as Nvarchar) + 'x' + Cast(b.SplitLength as Nvarchar) SubRollDescription, 
            VF2.ITEM_NAME + ' ' + VF2.QualityName + ' ' + VF2.DesignName + ' ' + VF2.ColorName + ' ' + VF2.ShapeName + ' ' + 
            Case When b.UnitID = 1 Then VF2.SizeMtr Else Case When b.UnitID = 2 Then VF2.SizeFt Else VF2.SizeInch End End + ' ' + VF2.ShadeColorName SubSubRollDescription, 
            b.Qty - b.RejectQty Qty, b.RejectQty 
            From RollReceiveToNextProcessMaster a(Nolock) 
            JOIN RollReceiveToNextProcessDetail b(Nolock) ON b.RollReceiveToNextID = a.RollReceiveToNextID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.MainRollFinishedID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = b.Item_Finished_ID 
            JOIN V_FinishedItemDetail VF2(Nolock) ON VF2.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            Where a.RollReceiveToNextID = " + HnReceiveID.Value + @" 
            Order By MaterialReceiveInPcsID, b.MaterialReceiveInPcsDetailID, b.RollReceiveToNextDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
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
            Label LblRollReceiveToNextID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveToNextID");
            Label LblRollReceiveToNextDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollReceiveToNextDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollReceiveToNextID", LblRollReceiveToNextID.Text);
            param[1] = new SqlParameter("@RollReceiveToNextDetailID", LblRollReceiveToNextDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollReceiveToNextProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
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
        string str = @"Select RollReceiveToNextID, ReceiveNo 
            From RollReceiveToNextProcessMaster(nolock) 
            Where MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @"
            Order By RollReceiveToNextID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDReceiveNo, ds, 0, true, "-Select Issue No-");        
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"Select a.RollReceiveToNextID, a.ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate 
            From RollReceiveToNextProcessMaster a(Nolock) 
            Where a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And a.CompanyID = " + ddCompName.SelectedValue + @" And 
            a.ProcessID = " + DDProcessName.SelectedValue + " And a.RollReceiveToNextID = " + DDReceiveNo.SelectedValue + @"
            Order By a.RollReceiveToNextID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
            TxtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            HnReceiveID.Value = ds.Tables[0].Rows[0]["RollReceiveToNextID"].ToString();
        }

        fill_grid();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        string Str = @"Select b.RollReceiveToNextDetailID, a.RollReceiveToNextID, b.MaterialReceiveInPcsID MainRollID, b.MaterialReceiveInPcsDetailID SubRollID, b.RollReceiveToNextDetailID SubSubRollID, 
            CI.CompanyName, CI.CompAddr1 + ', ' + CI.CompAddr2 + ', ' + CI.CompAddr3 CompanyAddress, 
            CI.CompTel, CI.GSTNo, PNM.PROCESS_NAME ProcessName, a.ReceiveNo, 
            REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') ReceiveDate, U1.UnitName, OM.CustomerOrderNo OrderNo, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName MainRollDescription, 
            VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.ShadeColorName SubRollDescription, 
            Cast(b.SplitWidth as Nvarchar) + 'x' + Cast(b.SplitLength as Nvarchar) SubRollSize,
            Case When b.UnitID = 1 Then VF2.SizeMtr Else Case When b.UnitID = 2 Then VF2.SizeFt Else VF2.SizeInch End End SubSubRollSize, 
            Case When b.UnitID = 1 Then VF1.SizeMtr Else Case When b.UnitID = 2 Then VF1.SizeFt Else VF1.SizeInch End End OrderSize, b.Qty - b.RejectQty Qty, b.RejectQty 
            From RollReceiveToNextProcessMaster a(Nolock) 
            JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            JOIN RollReceiveToNextProcessDetail b(Nolock) ON b.RollReceiveToNextID = a.RollReceiveToNextID 
            JOIN Unit U1(Nolock) ON U1.UnitId = b.UnitID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderID = b.OrderID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.MainRollFinishedID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = b.Item_Finished_ID 
            JOIN V_FinishedItemDetail VF2(Nolock) ON VF2.ITEM_FINISHED_ID = b.SubSubRollFinishedID 
            Where a.RollReceiveToNextID = " + HnReceiveID.Value + @" 
            Order By MaterialReceiveInPcsID, b.MaterialReceiveInPcsDetailID, b.RollReceiveToNextDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollReceiveToNextProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollReceiveToNextProcess.xsd";
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
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
