using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_WARP_FrmWarpCovertDescription : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                From CompanyInfo CI(Nolock)
                JOIN WARPLOOMMASTER WLM(Nolock) ON WLM.CompanyId = CI.CompanyId 
                JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And CA.MasterCompanyid = " + Session["varCompanyId"] + @" Order By CI.CompanyName 

                Select Distinct D.Departmentid, D.Departmentname 
                From Department D(Nolock) 
                JOIN WARPLOOMMASTER WLM(Nolock) ON WLM.DeptId = D.DepartmentId And CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                Order By Departmentname Desc 

                Select Distinct PROCESS_NAME_ID,PROCESS_NAME 
                From Process_Name_Master PNM(Nolock) 
                JOIN WARPLOOMMASTER WLM(Nolock) ON WLM.Processid = PNM.PROCESS_NAME_ID And WLM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
                Where MasterCompanyid = " + Session["varCompanyId"] + @" 

                Select CustomerID, CustomerCode + '  ' + CompanyName Customer From Customerinfo CI(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + @" Order By Customer ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 1, true, "--Plz Select--");
            if (DDDept.Items.Count > 0)
            {
                DDDept.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 3, true, "--Plz Select--");
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDProcess, @"Select Distinct PROCESS_NAME_ID,PROCESS_NAME 
                From Process_Name_Master PNM(Nolock) 
                JOIN WARPLOOMMASTER WLM(Nolock) ON WLM.Processid = PNM.PROCESS_NAME_ID And WLM.CompanyID = " + DDcompany.SelectedValue + @" 
                    And WLM.DeptId = " + DDDept.SelectedValue + @"
                Where MasterCompanyid = " + Session["varCompanyId"], true, "--Plz Select--");
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        if (chkedit.Checked == true)
        {
            Str = @"Select Distinct EI.EmpId, EI.EmpName + ' [' + EI.Empcode + ']' EmpName 
                    From WARPORDERMASTER WLM(Nolock) 
                    JOIN WARPORDERDETAIL b(Nolock) ON b.ID = WLM.ID 
                    JOIN WARPORDERDETAIL c(Nolock) ON IsNull(c.PostDetailID, 0) = b.Detailid 
                    JOIN Empinfo EI (Nolock) ON EI.EmpId = WLM.EmpId  
                    Where WLM.CompanyId = " + DDcompany.SelectedValue + " And WLM.DeptId = " + DDDept.SelectedValue + " And WLM.Processid = " + DDProcess.SelectedValue + @" Order By EmpName";
        }
        else
        {
            Str = @"Select EI.EmpId, EI.EmpName + ' [' + EI.Empcode + ']' Empname 
                From Empinfo EI 
                JOIN Department D ON EI.Departmentid = D.DepartmentId 
                Where EI.Departmentid = " + DDDept.SelectedValue + " Order by EI.EmpName";

//            Str = @"Select Distinct EI.EmpId, EI.EmpName + ' [' + EI.Empcode + ']' EmpName 
//                    From WARPLOOMMASTER WLM(Nolock) 
//                    JOIN Empinfo EI (Nolock) ON EI.EmpId = WLM.EmpId  
//                    Where WLM.CompanyId = " + DDcompany.SelectedValue + " And WLM.DeptId = " + DDDept.SelectedValue + " And WLM.Processid = " + DDProcess.SelectedValue + @" Order By EmpName";
        }
        
        Str = Str + @" Select Distinct VF.ITEM_FINISHED_ID, 
        VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.designName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr + ' ' + VF.ShadeColorName Description 
        From WARPLOOMMASTER a
        JOIN WARPLOOMDETAIL b ON b.ID = a.ID 
        JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = b.ofinishedid 
        Where b.Pcs > IsNull(IssuePcs, 0) 
        And a.CompanyId = " + DDcompany.SelectedValue + @" And a.DeptId = " + DDDept.SelectedValue + @" And a.Processid = " + DDProcess.SelectedValue + @" 
        Order By VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.designName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr + ' ' + VF.ShadeColorName ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDEmp, ds, 0, true, "--Plz Select--");
        UtilityModule.ConditionalComboFillWithDS(ref DDBeamDescription, ds, 1, true, "--Plz Select--");
    }
    protected void DDEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = @"Select Distinct VF.ITEM_FINISHED_ID, 
            VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.designName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr + ' ' + VF.ShadeColorName Description 
            From WARPLOOMMASTER a
            JOIN WARPLOOMDETAIL b ON b.ID = a.ID 
            JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = b.ofinishedid 
            Where b.Pcs > IsNull(IssuePcs, 0) 
            And a.CompanyId = " + DDcompany.SelectedValue + @" And a.DeptId = " + DDDept.SelectedValue + @" And a.Processid = " + DDProcess.SelectedValue + @" 
            And a.EmpId = " + DDEmp.SelectedValue + @" 
            Order By VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.designName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr + ' ' + VF.ShadeColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDBeamDescription, ds, 0, true, "--Plz Select--");
        if (chkedit.Checked == true)
        {
            FillIssueNo();
        }
        HNID.Value = "0";
        txtissueno.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDorderNo, @"Select OM.OrderID, OM.LocalOrder + ' ' + OM.CustomerOrderNo OrderNo 
            From OrderMaster OM 
            Where CompanyID = " + DDcompany.SelectedValue + " And CustomerId = " + DDcustcode.SelectedValue + " And OM.Status = 0 Order By OM.OrderID", true, "--Plz Select--");
    }

    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderDescription();
    }

    protected void FillOrderDescription()
    {
        string str = @"select OD.OrderDetailId, Vf.ITEM_NAME + ' ' + VF.QualityName + ' ' + Vf.designName + ' ' + Vf.ColorName + ' ' + vf.ShapeName + ' ' + 
                    case when OD.flagsize = 1 Then Vf.SizeMtr When OD.flagsize = 2 Then 
                    vf.sizeinch ELse vf.sizeft End + ' ' + case when Vf.SizeId > 0 Then Sz.Type Else '' End ItemDescription 
                    From OrderMaster OM 
                    JOIN OrderDetail OD on OM.OrderId = OD.OrderId 
                    JOIN V_FinishedItemDetail vf ON OD.Item_Finished_Id = vf.ITEM_FINISHED_ID 
                    Left join SizeType Sz ON Od.flagsize = Sz.val 
                    Where OM.Companyid = " + DDcompany.SelectedValue + " And Om.customerid = " + DDcustcode.SelectedValue + @" 
                    And OM.orderid = " + DDorderNo.SelectedValue + " Order By OD.orderdetailid";
        UtilityModule.ConditionalComboFill(ref DDitemdescription, str, true, "--Plz Select--");
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        BtnShowClick();
    }
    private void BtnShowClick()
    {
        GvBeamDesc.Visible = true;
        string str = @"Select ODD.CustomerID, ODD.OrderID, ODD.OrderDetailID, a.ProcessID, b.ofinishedid, a.ID, b.Detailid, b.Issuemasterid, b.IssueDetailid, ODD.CustomerCode, 
        ODD.LocalOrder + ' ' + ODD.CustomerOrderNo OrderNo, 
        VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.SizeMtr + ' ' + VF1.ShadeColorName OrderDescription, 
        VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeMtr + ' ' + VF.ShadeColorName BeamDescription, 
        b.Pcs - IsNull(IssuePcs, 0) Pcs 
        From WARPLOOMMASTER a
        JOIN WARPLOOMDETAIL b ON b.ID = a.ID 
        JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = b.ofinishedid 
        JOIN WARPORDERDETAIL WOD ON WOD.Detailid = b.IssueDetailid And WOD.ID = b.Issuemasterid 
        JOIN (Select Distinct OM.OrderID, OM.CustomerID, OD.OrderDetailID, CI.CustomerCode, OM.LocalOrder, OM.CustomerOrderNo 
		        From OrderMaster OM(Nolock) 
		        JOIN OrderDetail OD(Nolock) ON OD.OrderId = OM.OrderID 
		        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId) ODD ON ODD.OrderDetailID = WOD.OrderDetailID 
        JOIN V_FinishedItemDetail VF1 ON VF1.ITEM_FINISHED_ID = WOD.OrderItemFinishedid  
        --JOIN GodownMaster GM ON GM.GoDownID = b.GodownID 
        Where b.Pcs > IsNull(IssuePcs, 0)";

        if (DDcompany.SelectedIndex > 0)
        {
            str = str + " And a.CompanyID = " + DDcompany.SelectedValue;
        }
        if (DDDept.SelectedIndex > 0)
        {
            str = str + " And a.DeptID = " + DDDept.SelectedValue;
        }
        if (DDProcess.SelectedIndex > 0)
        {
            str = str + " And a.ProcessID = " + DDProcess.SelectedValue;
        }
        if (DDEmp.SelectedIndex > 0)
        {
            str = str + " And a.EmpID = " + DDEmp.SelectedValue;
        }
        if (DDcustcode.SelectedIndex > 0)
        {
            str = str + " And ODD.CustomerID = " + DDcustcode.SelectedValue;
        }
        if (DDorderNo.SelectedIndex > 0)
        {
            str = str + " And ODD.OrderID = " + DDorderNo.SelectedValue;
        }
        if (DDitemdescription.SelectedIndex > 0)
        {
            str = str + " And ODD.OrderDetailID = " + DDitemdescription.SelectedValue;
        }
        if (DDBeamDescription.SelectedIndex > 0)
        {
            str = str + " And b.ofinishedid = " + DDBeamDescription.SelectedValue;
        }
        str = str + " Order By VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, VF.SizeMtr, VF.SizeFt, VF.ShadeColorName ";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GvBeamDesc.DataSource = ds.Tables[0];
        GvBeamDesc.DataBind();
    }
    protected void Chkboxitem_CheckChanged(object sender, EventArgs e)
    {
        DDChangeOrderDescription.Items.Clear();
        Int32 NumberofCheck = 0;
        for (int i = 0; i < GvBeamDesc.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)GvBeamDesc.Rows[i].FindControl("Chkboxitem");

            if (Chkboxitem.Checked == true)
            {
                NumberofCheck = NumberofCheck + 1;
                if (NumberofCheck > 1)
                {
                    Chkboxitem.Checked = false;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Only one check box checked!!!');", true);
                    return;
                }

                Label LblID = (Label)GvBeamDesc.Rows[i].FindControl("LblID");
                Label LblDetailID = (Label)GvBeamDesc.Rows[i].FindControl("LblDetailID");
                Label LblIssueMasterID = (Label)GvBeamDesc.Rows[i].FindControl("LblIssueMasterID");
                Label LblIssueDetailID = (Label)GvBeamDesc.Rows[i].FindControl("LblIssueDetailID");
                Label LblProcessID = (Label)GvBeamDesc.Rows[i].FindControl("LblProcessID");

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@IssueDetailID", LblIssueDetailID.Text);
                param[1] = new SqlParameter("@ProcessID", LblProcessID.Text);

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_WarpCovertDescriptionOrderDetail", param);

                UtilityModule.ConditionalComboFillWithDS(ref DDChangeOrderDescription, ds, 0, true, "--Plz Select--");
            }
        }
    }

    protected void ChkBoxItemBeamDescription_CheckChanged(object sender, EventArgs e)
    {
        Int32 NumberofCheckBeamDescription = 0, NumberofCheck = 0, PendingPcs = 0, Pcs = 0, i = 0;

        for (i = 0; i < GDBeamDescription.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)GDBeamDescription.Rows[i].FindControl("ChkBoxItemBeamDescription");

            if (Chkboxitem.Checked == true)
            {
                NumberofCheckBeamDescription = 1;
                NumberofCheck = NumberofCheck + 1;
                if (NumberofCheck > 1)
                {
                    Chkboxitem.Checked = false;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Only one check box checked!!!');", true);
                    return;
                }

                PendingPcs = Convert.ToInt32(((Label)GDBeamDescription.Rows[i].FindControl("LblPendingPcs")).Text);
            }
        }
        NumberofCheck = 0;
        for (i = 0; i < GvBeamDesc.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)GvBeamDesc.Rows[i].FindControl("Chkboxitem");

            if (Chkboxitem.Checked == true)
            {
                NumberofCheck = NumberofCheck + 1;
                if (NumberofCheck > 1)
                {
                    Chkboxitem.Checked = false;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Only one check box checked!!!');", true);
                    return;
                }

                Pcs = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("lblPcs")).Text);
            }
        }
        if (Pcs > PendingPcs && NumberofCheckBeamDescription > 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('Pending pcs can not be less than pcs!!!');", true);

            for (i = 0; i < GDBeamDescription.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)GDBeamDescription.Rows[i].FindControl("ChkBoxItemBeamDescription");

                if (Chkboxitem.Checked == true)
                {
                    Chkboxitem.Checked = false;
                }
                return;
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";

        int FirstGDCheckFlag = 0, SecondGDCheckFlag = 0, i = 0, ISSUEID_OLD = 0, ISSUEDETAILID_OLD = 0, RECEIVEMASTERID_OLD = 0, RECEIVEDETAILID_OLD = 0,
            BEAMFINISHEDID = 0, PCS = 0, ProcessID = 0, OSIZEFLAG = 0;
        Label AREA = this.FindControl("AREA") as Label;

        for (i = 0; i < GvBeamDesc.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)GvBeamDesc.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                FirstGDCheckFlag = FirstGDCheckFlag + 1;
                RECEIVEMASTERID_OLD = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("LblID")).Text);
                RECEIVEDETAILID_OLD = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("LblDetailID")).Text);
                ISSUEID_OLD = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("LblIssueMasterID")).Text);
                ISSUEDETAILID_OLD = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("LblIssueDetailID")).Text);
                ProcessID = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("LblProcessID")).Text);
                PCS = Convert.ToInt32(((Label)GvBeamDesc.Rows[i].FindControl("lblPcs")).Text);
            }
        }

        for (i = 0; i < GDBeamDescription.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)GDBeamDescription.Rows[i].FindControl("ChkBoxItemBeamDescription");

            if (Chkboxitem.Checked == true)
            {
                SecondGDCheckFlag = SecondGDCheckFlag + 1;
                BEAMFINISHEDID = Convert.ToInt32(((Label)GDBeamDescription.Rows[i].FindControl("lblitemfinishedid")).Text);
                AREA = (Label)GDBeamDescription.Rows[i].FindControl("lblarea");
                OSIZEFLAG = Convert.ToInt32(((Label)GDBeamDescription.Rows[i].FindControl("lblosizeflag")).Text);
            }
        }
        if (FirstGDCheckFlag == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please check one checkbox in 1st table')", true);
            return;
        }
        if (SecondGDCheckFlag == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please check one checkbox in 2nd table')", true);
            return;
        }
        if (FirstGDCheckFlag > 0 && SecondGDCheckFlag > 0)
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[20];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@COMPANYID", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@DEPTID", DDDept.SelectedValue);
                param[3] = new SqlParameter("@PROCESSID", DDProcess.SelectedValue);
                param[4] = new SqlParameter("@EMPID", DDEmp.SelectedValue);
                param[5] = new SqlParameter("@IssueNo", SqlDbType.VarChar, 50);
                param[5].Value = txtissueno.Text;
                param[5].Direction = ParameterDirection.InputOutput;
                param[6] = new SqlParameter("@IssueDate", txtissuedate.Text);

                param[7] = new SqlParameter("@ISSUEID_OLD", ISSUEID_OLD);
                param[8] = new SqlParameter("@ISSUEDETAILID_OLD", ISSUEDETAILID_OLD);
                param[9] = new SqlParameter("@RECEIVEMASTERID_OLD", RECEIVEMASTERID_OLD);
                param[10] = new SqlParameter("@RECEIVEDETAILID_OLD", RECEIVEDETAILID_OLD);
                param[11] = new SqlParameter("@ORDERDETAILID", DDChangeOrderDescription.SelectedValue);
                param[12] = new SqlParameter("@BEAMFINISHEDID", BEAMFINISHEDID);
                param[13] = new SqlParameter("@PCS", PCS);
                param[14] = new SqlParameter("@AREA", AREA.Text);
                param[15] = new SqlParameter("@OSIZEFLAG", OSIZEFLAG);
                param[16] = new SqlParameter("@USERID", Session["varuserid"]);
                param[17] = new SqlParameter("@MASTERCOMPANYID", Session["varCompanyId"]);
                param[18] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
                param[18].Direction = ParameterDirection.Output;

                //**********************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_WARPORDER_RECEIVE_ONETIME", param);
                HNID.Value = param[0].Value.ToString();
                txtissueno.Text = param[5].Value.ToString();
                Tran.Commit();
                if (param[18].Value.ToString() != "")
                {
                    lblmessage.Text = param[18].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "Data saved successfully..";
                    BtnShowClick();
                    DDChangeOrderDescription.Items.Clear();
                    GDBeamDescription.Visible = false;
                    FillMaingrid();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please fill all checked Mandatory field data.')", true);
        }
    }

    protected void FillMaingrid()
    {
        string str = @"select dbo.F_getItemDescription(WD.item_finished_id,WD.flagsize) as ItemDescription,WD.Pcs,WD.area,WD.ID,WD.DetailId,Noofbeamreq,IssueNo,REPLACE(CONVERT(nvarchar(11),IssueDate,106),' ','-') as IssueDate,REPLACE(CONVERT(nvarchar(11),TargetDate,106),' ','-') as TargetDate
                      from Warpordermaster WM inner join Warporderdetail WD on WM.id=WD.Id Where WM.ID=" + HNID.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["Issueno"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
            }
            else
            {
                txtissueno.Text = "";
                txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", HNID.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_reportWarpingOrder", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rtpwarporder.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rtpwarporder.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
        //

    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = (Label)DG.Rows[e.RowIndex].FindControl("lblid");
            Label lblDetailid = (Label)DG.Rows[e.RowIndex].FindControl("lbldetailid");

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Id", lblid.Text);
            param[1] = new SqlParameter("@DetailId", lblDetailid.Text);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Delete_WarpOrder_Receive_OneTime", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            DDEmp_SelectedIndexChanged(DDEmp, e);
            FillMaingrid();
            
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        DDProcess.SelectedIndex = -1;
        DDProcess_SelectedIndexChanged(DDProcess, e);
        if (chkedit.Checked == true)
        {
            DDissueNo.SelectedIndex = -1;
            TDissueNo.Visible = true;
        }
        else
        {
            txtissueno.Text = "";
            HNID.Value = "0";
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDissueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        HNID.Value = DDissueNo.SelectedValue;
        FillMaingrid();
    }
    protected void DDChangeOrderDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        GDBeamDescription.Visible = true;
        FillBeamDescription();
    }
    protected void FillBeamDescription()
    {
        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@orderdetailid", DDChangeOrderDescription.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcess.SelectedValue);
            param[2] = new SqlParameter("@Totalpcs", SqlDbType.Int);
            param[3] = new SqlParameter("@Totalarea", SqlDbType.Float);
            param[2].Direction = ParameterDirection.Output;
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Effectivedate", txtissuedate.Text);
            param[5] = new SqlParameter("@EmpId", DDEmp.SelectedValue);
            param[6] = new SqlParameter("@MasterCompanyId", Session["VarCompanyId"]);
            //****************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETARTICLEBEAMDESCRIPTION", param);
            GDBeamDescription.DataSource = ds.Tables[0];
            GDBeamDescription.DataBind();
            txttotalpcs.Text = param[2].Value.ToString();
            txttotalarea.Text = param[3].Value.ToString();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    private void FillIssueNo()
    {
        string str = @"Select Distinct a.ID, a.IssueNo, a.CompanyId, a.DeptId, a.Processid, a.Empid 
                    From WARPORDERMASTER a(Nolock)
                    JOIN WARPORDERDETAIL b(Nolock) ON b.ID = a.ID 
                    JOIN WARPORDERDETAIL c(Nolock) ON IsNull(c.PostDetailID, 0) = b.Detailid 
                    Where a.CompanyID = " + DDcompany.SelectedValue + " And a.DeptId = " + DDDept.SelectedValue + " And a.Processid = " + DDProcess.SelectedValue + " And a.Empid = " + DDEmp.SelectedValue + @"
                    Order By a.ID Desc";
        UtilityModule.ConditionalComboFill(ref DDissueNo, str, true, "--Plz Select--");
    }
}