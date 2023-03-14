using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_MachineProcess_FrmRollIssueToNextProcess : System.Web.UI.Page
{
    string str = "";
    int varcombo = 0;
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
                    DDProcessName.SelectedValue = "16";
                }
                else
                {
                    DDProcessName.SelectedIndex = 1;
                }
            }
            FillDGGrid();
            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            HnIssueID.Value = "0";
        }
    }
    private void FillDGGrid()
    {
        string str = @"Select a.MaterialReceiveInPcsID, a.MainRollFinishedID, U.UnitName, a.UnitID, 
        VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
        Case When a.UnitID = 1 Then VF.SizeMtr Else Case When a.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription 
        From MaterialReceiveInPcsMaster a(Nolock)
        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.MainRollFinishedID 
        JOIN Unit U(Nolock) ON U.UnitID = a.UnitID 
        LEFT JOIN View_RollIssueToNextProcessDetail VRINPD(Nolock) ON VRINPD.MaterialReceiveInPcsID = a.MaterialReceiveInPcsID
        Where VRINPD.MaterialReceiveInPcsID Is Null ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblMaterialReceiveInPcsID = ((Label)DG.Rows[i].FindControl("LblMaterialReceiveInPcsID"));
                Label LblMainRollFinishedID = ((Label)DG.Rows[i].FindControl("LblMainRollFinishedID"));
                Label LblUnitID = ((Label)DG.Rows[i].FindControl("LblUnitID"));
                if (DetailData == "")
                {
                    DetailData = LblMaterialReceiveInPcsID.Text + "|" + LblMainRollFinishedID.Text + "|" + LblUnitID.Text + "~";
                }
                else
                {
                    DetailData = DetailData + LblMaterialReceiveInPcsID.Text + "|" + LblMainRollFinishedID.Text + "|" + LblUnitID.Text + "~";
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
            arr[0] = new SqlParameter("@RollIssueToNextID", SqlDbType.Int);
            arr[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
            arr[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
            arr[3] = new SqlParameter("@IssueNo", SqlDbType.NVarChar, 50);
            arr[4] = new SqlParameter("@IssueDate", SqlDbType.DateTime);
            arr[5] = new SqlParameter("@DetailData", SqlDbType.NVarChar);
            arr[6] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[7] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[8] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnIssueID.Value;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDProcessName.SelectedValue;
            arr[3].Direction = ParameterDirection.InputOutput;
            arr[3].Value = TxtIssueNo.Text;
            arr[4].Value = TxtIssueDate.Text;
            arr[5].Value = DetailData;
            arr[6].Value = Session["varuserid"];
            arr[7].Value = Session["varCompanyId"];
            arr[8].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveRollIssueToNextProcess]", arr);

            if (arr[8].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[8].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnIssueID.Value = arr[0].Value.ToString();
                TxtIssueNo.Text = Convert.ToString(arr[3].Value);
                tran.Commit();
            }
            FillDGGrid();
            fill_grid();
            FillConsumptionQty();
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

    private void fill_grid()
    {
        string str = @"Select a.RollIssueToNextID, b.RollIssueToNextDetailID, b.MaterialReceiveInPcsID, U.UnitName, 
            VF.ITEM_NAME + ' / ' + VF.QualityName + ' / ' + VF.DesignName + ' / ' + VF.ColorName + ' / ' + VF.ShapeName + ' / ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + 
            Case WHen VF.ShadeColorName <> '' Then ' / ' + VF.ShadeColorName Else '' End ItemDescription 
            From RollIssueToNextProcessMaster a(Nolock)
            JOIN RollIssueToNextProcessDetail b(Nolock) ON b.RollIssueToNextID = a.RollIssueToNextID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.MainRollFinishedID 
            JOIN Unit U(Nolock) ON U.UnitID = b.UnitID 
            Where a.RollIssueToNextID = " + HnIssueID.Value + @" 
            Order By b.RollIssueToNextDetailID ";

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
            Label LblRollIssueToNextID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextID");
            Label LblRollIssueToNextDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollIssueToNextID", LblRollIssueToNextID.Text);
            param[1] = new SqlParameter("@RollIssueToNextDetailID", LblRollIssueToNextDetailID.Text);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["VarCompanyId"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollIssueToNextProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
            FillConsumptionQty();
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
        DDIssueNo.Items.Clear();
        TxtIssueNo.Text = "";
        TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
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
        HnIssueID.Value = "0";
        TxtIssueNo.Text = "";
        string str = @"Select RollIssueToNextID, IssueNo 
            From RollIssueToNextProcessMaster
            Where MasterCompanyID = " + Session["VarCompanyId"] + " And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + DDProcessName.SelectedValue + @"
            Order By RollIssueToNextID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "-Select Issue No-");
        
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        report();
    }
    private void report()
    {
        string Str = @"Select b.MaterialReceiveInPcsID RollNo, CI.CompanyName, CI.CompAddr1 + ', ' + CI.CompAddr2 + ', ' + CI.CompAddr3 CompanyAddress, 
            CI.CompTel, CI.GSTNo, PNM.PROCESS_NAME ProcessName, a.IssueNo, 
            REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate, U1.UnitName, 
            VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 2 Then VF.SizeFt Else VF.SizeInch End End + ' ' + VF.ShadeColorName MainRollDescription, 
            OM.CustomerOrderNo OrderNo, VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.ShadeColorName SubRollDescription, 
            Cast(c.SplitWidth as Nvarchar) + 'x' + Cast(c.SplitLength as Nvarchar) SubRollSize, 
            Case When b.UnitID = 1 Then VF1.SizeMtr Else Case When b.UnitID = 2 Then VF1.SizeFt Else VF1.SizeInch End End OrderSize, c.Qty OrderQty,
            c.MaterialReceiveInPcsDetailID as SubRollNo 
            From RollIssueToNextProcessMaster a
            JOIN RollIssueToNextProcessDetail b ON b.RollIssueToNextID = a.RollIssueToNextID 
            JOIN RollIssueToNextProcessDetailDetail c ON c.RollIssueToNextDetailID = b.RollIssueToNextDetailID And c.RollIssueToNextID = b.RollIssueToNextID 
            JOIN MaterialReceiveInPcsMaster d(Nolock) ON d.MaterialReceiveInPcsID = b.MaterialReceiveInPcsID 
            JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
            JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            JOIN Unit U1(Nolock) ON U1.UnitId = b.UnitID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.MainRollFinishedID 
            JOIN OrderMaster OM(Nolock) ON OM.OrderId = c.OrderID 
            JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = c.Item_Finished_ID 
            Where a.RollIssueToNextID =  " + HnIssueID.Value + @" 
            Order By c.RollIssueToNextDetailDetailID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRollIssueToNextProcess.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRollIssueToNextProcess.xsd";
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
        string str = @"Select a.RollIssueToNextID, a.IssueNo, REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate 
            From RollIssueToNextProcessMaster a(Nolock) 
            Where a.MasterCompanyID = " + Session["VarCompanyId"] + " And a.CompanyID = " + ddCompName.SelectedValue + " And a.ProcessID = " + DDProcessName.SelectedValue + @" 
            and a.IssueNo="+DDIssueNo.SelectedValue+@"
            Order By a.RollIssueToNextID Desc";

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtIssueNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
            TxtIssueDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
            HnIssueID.Value = ds.Tables[0].Rows[0]["RollIssueToNextID"].ToString();
        }
        fill_grid();
        FillConsumptionQty();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void FillConsumptionQty()
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@RollIssueToNextID", HnIssueID.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILL_LATEXING_ROLLCONSUMPTION", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGConsumption.DataSource = ds.Tables[0];
            DGConsumption.DataBind();
        }
        else
        {
            DGConsumption.DataSource = null;
            DGConsumption.DataBind();
        }
        
        


        //        string str = @"SELECT VF1.Item_Name+Space(2)+VF1.QualityName+Space(2)+VF1.DesignName+Space(2)+VF1.ColorName+Space(2)+VF1.ShapeName+Space(2)+
        //          CASE WHEN PM.UnitId=1 Then VF1.SizeMtr else VF1.SizeFt END+Space(2)+VF1.ShadeColorName ItemDescription,
        //          Isnull(Round(Sum(CASE WHEN OCD.ICalType=0 or OCD.ICalType=2 THEN CASE WHEN PM.UnitId=1 Then PD.Qty*PD.Area*OCD.IQTY*1.196 else PD.Qty*PD.Area*OCD.IQTY END ELSE 
        //          CASE WHEN PM.UnitId=1 Then PD.Qty*OCD.IQTY*1.196 else PD.Qty*OCD.IQTY END END),3),0) ConsmpQTY,OCD.IFinishedid,U.UnitName
        //          FROM PROCESS_ISSUE_MASTER_1 PM inner join PROCESS_ISSUE_DETAIL_1 PD on PM.IssueOrderId=PD.IssueOrderId
        //          inner join PROCESS_CONSUMPTION_DETAIL OCD on OCD.ISSUEORDERID=PM.IssueOrderId and OCD.ISSUE_DETAIL_ID=PD.Issue_Detail_Id and ocd.PROCESSID=1
        //          inner join V_FinishedItemDetail VF1 on Vf1.ITEM_FINISHED_ID=OCD.IFINISHEDID inner join Unit U on OCD.iunitid=U.Unitid Where OCD.Issueorderid=" + hnissueorderid.Value + @" 
        //          Group By VF1.Category_Name,VF1.Item_Name,VF1.QualityName,VF1.DesignName,VF1.ColorName,VF1.ShapeName,PM.UnitId,VF1.SizeMtr,VF1.SizeFt,
        //          VF1.ShadeColorName,OCD.IFINISHEDID,PM.Issueorderid,U.UnitName";

        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        DGConsumption.DataSource = ds.Tables[0];
        //        DGConsumption.DataBind();

    }
}
