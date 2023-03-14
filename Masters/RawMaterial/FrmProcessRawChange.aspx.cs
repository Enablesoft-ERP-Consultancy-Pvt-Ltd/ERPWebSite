using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_FrmProcessRawChange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string Qry = @"Select Distinct CI.CompanyId, CI.Companyname 
                From Companyinfo CI(Nolock)
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CI.Companyname 

                Select PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                From PROCESS_NAME_MASTER PNM (Nolock)
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @"
                Where PNM.ProcessType = 1 Order By PNM.PROCESS_NAME
                
                Select VarProdCode, VarCompanyNo From MasterSetting(Nolock)";

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);

            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, false, "");
            if (ddProcessName.Items.Count > 0)
            {
                ddProcessName.SelectedValue = "1";
                ProcessNameSelectedIndexChange();
            }
            switch (Convert.ToInt32(DSQ.Tables[2].Rows[0]["VarProdCode"]))
            {
                case 0:
                    procode.Visible = false;
                    break;
                case 1:
                    procode.Visible = true;
                    break;
            }
            lablechange();
        }
    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        string str = @"Select Top 1 a.Companyid, " + ddProcessName.SelectedValue + @" ProcessID, Case When a.EmpID = 0 Then EPO.EmpID Else a.EmpID End EMPID, a.IssueOrderId, 
                Case When a.[Status] = 'Complete' Then 1 Else 0 End [Status] 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(Nolock) 
                LEFT JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = a.IssueOrderId And EPO.ProcessId = " + ddProcessName.SelectedValue + @" 
                Where a.Companyid = " + ddCompName.SelectedValue + " And a.IssueOrderId = '" + TxtPOrderNo.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ChKForComplete.Checked = false;
            ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessID"].ToString();
            ProcessNameSelectedIndexChange();
            ddempname.SelectedValue = ds.Tables[0].Rows[0]["EMPID"].ToString();
            if (ds.Tables[0].Rows[0]["Status"].ToString() == "1")
            {
                ChKForComplete.Checked = true;
            }
            EmpNameSelectedIndexChange();
            ddOrderNo.SelectedValue = ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
            OrderNoSelectedIndexChange();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('No record found !');", true);
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChange();
    }
    private void ProcessNameSelectedIndexChange()
    {
        string str = "";
        if (ddProcessName.SelectedIndex > 0)
        {
            //            str = @"Select a.EmpID, EI.EmpName 
            //                        From PROCESS_ISSUE_MASTER_1 a(nolock) 
            //                        JOIN EmpInfo EI(nolock) ON EI.EmpID = a.EmpID 
            //                        Where a.CompanyID = " + ddCompName.SelectedValue;
            //            if (ChKForComplete.Checked == true)
            //                str = str + " And a.Status = 'Pending'";
            //            else
            //                str = str + " And a.Status = 'Pending'";

            //            str = str + @" UNION 
            //                        Select EPO.EmpID, EI.EmpName 
            //                        From PROCESS_ISSUE_MASTER_1 a(nolock) 
            //                        JOIN Employee_ProcessOrderNo EPO(nolock) ON EPO.IssueOrderId = a.IssueOrderId 
            //                        JOIN EmpInfo EI(nolock) ON EI.EmpID = EPO.EmpID 
            //                        Where a.CompanyID = " + ddCompName.SelectedValue;
            //            if (ChKForComplete.Checked == true)
            //                str = str + " And a.Status = 'Pending'";
            //            else
            //                str = str + " And a.Status = 'Pending'";

            //            str = str + " Order By EmpName ";

            str = @"Select EI.EmpId, EI.EmpName + ' ' + Case When IsNull(EI.EmpCode, '') = '' Then '' ELse '[' + EI.EmpCode + ']' End EmployeeName
                    From Empinfo EI(Nolock)
                    JOIN EmpProcess EMP(Nolock) ON EMP.EmpId = EI.EmpId 
                    And EMP.Processid = " + ddProcessName.SelectedValue + @"
                    Order By EmployeeName ";

            UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Select--");
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpNameSelectedIndexChange();
    }
    private void EmpNameSelectedIndexChange()
    {
        string str = @"Select Distinct PIM.IssueOrderId, Pim.IssueOrderId 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM (Nolock) 
                JOIN EmpInfo ei on ei.EmpId = pim.Empid 
                WHERE PIm.Companyid = " + ddCompName.SelectedValue + @" And PIM.Empid = " + ddempname.SelectedValue;
        if (ChKForComplete.Checked == true)
            str = str + " And PIM.Status = 'Complete'";
        else
            str = str + " And PIM.Status = 'Pending'";

        str = str + @" UNION 
                Select Distinct pim.IssueOrderId, pim.IssueOrderId 
                From Process_issue_Master_" + ddProcessName.SelectedValue + @" pim (Nolock) 
                JOIN employee_processorderno emp(Nolock) ON emp.issueorderid = pim.issueorderid And emp.ProcessId = " + ddProcessName.SelectedValue + @" And 
                            EMP.Empid = " + ddempname.SelectedValue + @" 
                Where pim.Empid = 0 And pim.Companyid = " + ddCompName.SelectedValue;
        if (ChKForComplete.Checked == true)
            str = str + " And PIM.Status = 'Complete'";
        else
            str = str + " And PIM.Status = 'Pending'";


        UtilityModule.ConditionalComboFill(ref ddOrderNo, str, true, "Select order no");
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChange();
    }
    private void OrderNoSelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id 
                Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "Select Category Name");

        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
        }
        Fill_Category_SelectedChange();
    }
    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();

            UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
                Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Select Item--");

            if (dditemname.Items.Count > 0)
            {
                dditemname.SelectedIndex = 1;
                ItemName_SelectChange();
            }
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Category_SelectedChange();
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = @"SELECT IPM.CATEGORY_PARAMETERS_ID, IPM.CATEGORY_ID, IPM.PARAMETER_ID, PM.PARAMETER_NAME 
                FROM ITEM_CATEGORY_PARAMETERS IPM (Nolock)
                JOIN PARAMETER_MASTER PM(Nolock) ON PM.[PARAMETER_ID] = IPM.[PARAMETER_ID] And PM.MasterCompanyId = " + Session["varCompanyId"] + @" 
                Where IPM.[CATEGORY_ID] = " + ddCatagory.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        break;
                    case "2":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct VF.DesignId, VF.DesignName 
                            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                            JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
                            Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Select Design--");
                        if (dddesign.Items.Count > 0)
                        {
                            dddesign.SelectedIndex = 1;
                        }
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorId, VF.ColorName 
                            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                            JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
                            Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Select Color--");
                        if (ddcolor.Items.Count > 0)
                        {
                            ddcolor.SelectedIndex = 1;
                        }
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeId, VF.ShapeName 
                            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                            JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
                            Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Select Shape--");

                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeId, Case When a.UnitID = 2 Then VF.SizeFt Else SizeMtr End 
                            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                            JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
                                    And VF.ShapeID = " + ddshape.SelectedValue + @" 
                            Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Size--");
                        if (ddsize.Items.Count > 0)
                        {
                            ddsize.SelectedIndex = 1;
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemName_SelectChange();
    }
    private void ItemName_SelectChange()
    {
        if (dditemname.SelectedIndex >= 0)
        {
            UtilityModule.ConditionalComboFill(ref dquality, @"Select Distinct VF.QualityId, VF.QualityName 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id 
                Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "--Select Item--");
            if (dquality.Items.Count > 0)
            {
                dquality.SelectedIndex = 1;
            }
            QualitySelectedIndexChange();
        }
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        QualitySelectedIndexChange();
    }
    private void QualitySelectedIndexChange()
    {
        if (shd.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct VF.ShadecolorId, VF.ShadeColorName 
                From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_Id  
                And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityId = " + dquality.SelectedValue + @" 
                Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue, true, "Select Shadecolor");
        }
        Fill_Grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((ql.Visible == true && dquality.SelectedIndex > 0) || ql.Visible != true)
        {
            quality = 1;
        }
        if (dsn.Visible == true && dddesign.SelectedIndex > 0 || dsn.Visible != true)
        {
            design = 1;
        }
        if (clr.Visible == true && ddcolor.SelectedIndex > 0 || clr.Visible != true)
        {
            color = 1;
        }
        if (shp.Visible == true && ddshape.SelectedIndex > 0 || shp.Visible != true)
        {
            shape = 1;
        }
        if (sz.Visible == true && ddsize.SelectedIndex > 0 || sz.Visible != true)
        {
            size = 1;
        }
        if (shd.Visible == true && ddlshade.SelectedIndex > 0 || shd.Visible != true)
        {
            shadeColor = 1;
        }
        //*************************
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

            string str = @"Select Distinct a.ISSUEORDERID, PCD.IFinishedID Item_Finished_Id, 
                    Replace(VF1.ITEM_NAME + '  ' + VF1.QualityName + '  ' + VF1.DesignName + '  ' + VF1.ColorName + '  ' + VF1.ShapeName + '  ' + Case When a.UnitId = 1 Then VF1.SizeMtr Else VF1.SizeFt End + '  ' + VF1.ShadeColorName, '   ', '') [Description], 
                    PCD.IQty ConsmpQty
                    From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" a(nolock) 
                    JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" b(Nolock) ON b.IssueOrderId = a.IssueOrderId 
                    JOIN PROCESS_CONSUMPTION_DETAIL PCD(Nolock) ON PCD.ISSUEORDERID = a.IssueOrderId And PCD.ISSUE_DETAIL_ID = b.Issue_Detail_Id And ProcessID = " + ddProcessName.SelectedValue + @" 
                    JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = PCD.IFINISHEDID 
                    Where a.ISSUEORDERID = " + ddOrderNo.SelectedValue + @" And b.Item_Finished_Id = " + Varfinishedid + @" 
                    Order BY a.IssueOrderId ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdetail.DataSource = ds.Tables[0];
                gvdetail.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Consumption not attach this folio!');", true);
            }
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        string str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarItem_Finished_ID = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

            for (int i = 0; i < gvdetail.Rows.Count; i++)
            {
                string VarRaw_IFinished_ID = gvdetail.DataKeys[i].Value.ToString();
                TextBox txtConsmpQty = (TextBox)gvdetail.Rows[i].FindControl("txtConsmpQty");


                if (Convert.ToDecimal(txtConsmpQty.Text) > 0)
                {
                    if (str == "")
                    {
                        str = VarRaw_IFinished_ID + '|' + txtConsmpQty.Text + '~';
                    }
                    else
                    {
                        str = str + VarRaw_IFinished_ID + '|' + txtConsmpQty.Text + '~';
                    }
                }
            }

            SqlCommand cmd = new SqlCommand("PRO_UPDATE_FOLIO_CONSUMPTION", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@IssueOrderID", ddOrderNo.SelectedValue);
            cmd.Parameters.AddWithValue("@Item_Finished_ID", VarItem_Finished_ID);
            cmd.Parameters.AddWithValue("@ProcessID", ddProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@DetailData", str);
            cmd.Parameters.Add("@Message", SqlDbType.VarChar, 100);
            cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["varcompanyId"]);

            cmd.ExecuteNonQuery();

            Tran.Commit();

            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Record(s) has been saved successfully!');", true);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/FrmProcessRawChange.aspx");
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog(ex.Message);
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    
    protected void ChKForComplete_CheckedChanged(object sender, EventArgs e)
    {
        if (ddempname.SelectedIndex > 0)
        {
            EmpNameSelectedIndexChange();
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {

    }
    protected void btnclose_Click(object sender, EventArgs e)
    {

    }
}