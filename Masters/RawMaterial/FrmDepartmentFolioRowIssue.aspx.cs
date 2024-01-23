using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_process_FrmDepartmentFolioRowIssue : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        DataSet DSQ = null; string Qry = "";
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            Qry = @" Select Distinct CI.CompanyId, Companyname 
                    From Companyinfo CI(Nolock)
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 
                    Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                    From DEPARTMENTRAWISSUEMASTER DIM(Nolock)
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = DIM.PROCESSID 
                    JOIN UserRightsProcess URP(Nolock) on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserId"] + @"
                    Where DIM.TYPEFLAG = 1 
                    Order by PROCESS_NAME
                    Select VarProdCode, VarCompanyNo From MasterSetting(Nolock) 
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                    Select ConeType, ConeType From ConeMaster(Nolock) Order By SrNo ";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 3, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            if (ddProcessName.Items.Count > 0)
            {
                ddProcessName.SelectedIndex = 1;
                ProcessNameSelectedIndexChange();
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 4, false, "");

            int VarProdCode = Convert.ToInt32(DSQ.Tables[2].Rows[0]["VarProdCode"]);
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            switch (VarProdCode)
            {
                case 0:
                    procode.Visible = false;
                    break;
                case 1:
                    procode.Visible = true;
                    break;
            }
            lablechange();
            if (MySession.TagNowise == "1")
            {
                TDTagno.Visible = true;
            }

            if (variable.VarCompanyWiseChallanNoGenerated == "1")
            {
                txtchalanno.Enabled = false;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
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
        ViewState["Prmid"] = 0;
        string str = @"Select Distinct EI.EmpId, EI.EmpName 
            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(Nolock) 
            JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = PIM.IssueOrderId And ProcessID = " + ddProcessName.SelectedValue + @" 
            JOIN EmpInfo EI(Nolock) ON EI.EmpID = EPO.Empid  
            Where PIM.DEPARTMENTTYPE = 1 And PIM.Companyid = " + ddCompName.SelectedValue + " And PIM.BRANCHID = " + DDBranchName.SelectedValue + " Order By EI.EmpName ";
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Select--");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalanno.Text = "";
        EmpNameSelectedIndexChange();
    }
    private void EmpNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        string str = @"Select Distinct PIM.IssueOrderId, PIM.IssueOrderId 
            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(Nolock) 
            JOIN Employee_ProcessOrderNo EPO(Nolock) ON EPO.IssueOrderId = PIM.IssueOrderId And ProcessID = " + ddProcessName.SelectedValue + " And EPO.EmpID = " + ddempname.SelectedValue + @"
            Where IsNull(PIM.DEPARTMENTTYPE, 0) = 1 And PIM.Companyid = " + ddProcessName.SelectedValue + " And PIM.BRANCHID = " + ddProcessName.SelectedValue + @" 
                  And PIM.Status <> 'Complete' Order By PIM.IssueOrderId";

        UtilityModule.ConditionalComboFill(ref ddOrderNo, str, true, "Select order no");
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChange();
    }
    private void OrderNoSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        if (ChKForEdit.Checked == false)
        {
            Fill_Grid();
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId, ChallanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
            From DepartmentProcessRawMaster 
            Where FlagType = 1 And TranType = 0 And IssueOrderID = " + ddOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Challan No");
        }
        UtilityModule.ConditionalComboFill(ref ddCatagory, @"
                    Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
                    From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID 
                    JOIN CategorySeparate CS(Nolock) on CS.CategoryID = VF.CATEGORY_ID And CS.ID = 1 
                    Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + @" And PCD.PROCESSID = " + ddProcessName.SelectedValue + @"
                    Order BY VF.CATEGORY_NAME", true, "Select Category Name");

        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
        }
        Fill_Category_SelectedChange();
        fill_Grid_ShowConsmption();
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        string strsql = "";
        try
        {
            if (variable.VarMANYFOLIORAWISSUE_SINGLECHALAN == "1")
            {
                strsql = @"Select PrtId,CATEGORY_NAME,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeFt+ Space(2)+ShadeColorName DESCRIPTION,
                             Qty,LotNo,'' GodownName,'' BinNo,PT.TagNo From DepartmentProcessRawMaster Pm,DepartmentProcessRawTran PT,V_FinishedItemDetail VF Where pm.prmid=pt.prmid and  PT.Item_Finished_id=VF.Item_Finished_id And 
                             PM.challanno='" + txtchalanno.Text + "' and PM.Trantype=0 And PM.FlagType = 1 And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                if (ChKForEdit.Checked == true && ddOrderNo.SelectedIndex > 0)
                {

                    strsql = strsql + " and PM.IssueOrderID=" + ddOrderNo.SelectedValue + "";
                }
            }
            else
            {
                strsql = @"Select PrtId,CATEGORY_NAME,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeFt+ Space(2)+ShadeColorName DESCRIPTION,
                             Qty,LotNo,'' GodownName,'' BinNo,PT.TagNo From DepartmentProcessRawTran PT,V_FinishedItemDetail VF Where PT.Item_Finished_id=VF.Item_Finished_id And 
                             PT.PrmID=" + ViewState["Prmid"] + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
        }
        return ds;
    }
    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();

            UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
                From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID AND VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @" 
                Order BY VF.ITEM_NAME ", true, "--Select Item--");

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
        dsn.Visible = false;
        clr.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM(Nolock) inner join PARAMETER_MASTER PM(Nolock) on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
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
                        UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct VF.DesignID, VF.DesignName 
                            From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID 
                            Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @"
                            Order BY VF.DesignName ", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorId, VF.ColorName 
                            From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID 
                            Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @"
                            Order BY VF.ColorName ", true, "--Select Color--");
                        break;

                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeId, VF.ShapeName 
                            From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID 
                            Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @"
                            Order BY VF.ShapeName ", true, "--Select Shape--");
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
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
            string Qry = "";
                Qry = @" SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i(Nolock) INNER JOIN  Unit u(Nolock) ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + @" Select Distinct VF.QualityID, VF.QualityName 
                        From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID AND VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
                        Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @" 
                        Order BY VF.QualityName";

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
            UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 1, true, "Select Quallity");
            if (dquality.Items.Count > 0)
            {
                if (dquality.Items.Count == 1)
                {
                    dquality.SelectedIndex = 0;
                }
                else
                {
                    dquality.SelectedIndex = 1;
                }

                QualitySelectedIndexChange();
            }
            if (ddlunit.Items.Count > 0)
            {
                ddlunit.SelectedIndex = 1;
            }
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
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct VF.ShadecolorID, VF.ShadeColorName 
                From PROCESS_CONSUMPTION_DETAIL PCD(Nolock) 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = PCD.IFINISHEDID AND VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
                    VF.ITEM_ID = " + dditemname.SelectedValue + @" And VF.QualityId = " + dquality.SelectedValue + @" 
                Where PCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And PCD.PROCESSID = " + ddProcessName.SelectedValue + @" 
                Order BY VF.ShadeColorName", true, "Select Shadecolor");
        }
        FillLotNo();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDTagno.Visible == true)
        {
            DDTagno.SelectedIndex = -1;
            FillTagNo(sender);
        }
        else
        {
            fill_qty();
        }
    }
    private void FillLotNo()
    {
        int Item_Finished_ID = Get_Item_Finished_ID();
        string Str = @" Select Distinct DRIT.LOTNO, DRIT.LOTNO 
            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(Nolock)
            JOIN DEPARTMENTRAWISSUEMASTER DRIM(Nolock) ON DRIM.ISSUEORDERID = PIM.DepartmentIssueOrderID 
            JOIN DEPARTMENTRAWISSUETRAN DRIT(Nolock) ON DRIT.PRMID = DRIM.PRMID AND DRIT.ITEM_FINISHED_ID = " + Item_Finished_ID + @" 
            JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = DRIT.ITEM_FINISHED_ID 
            Where PIM.TYPEFLAG = 1 And PIM.ISSUEORDERID = " + ddOrderNo.SelectedValue + @"
            Order By DRIT.LOTNO ";
        UtilityModule.ConditionalComboFill(ref ddlotno, Str, true, "Select lot No");
    }
    protected void FillTagNo(object sender = null)
    {
        int Item_Finished_ID = Get_Item_Finished_ID();
        string Str = @" Select Distinct DRIT.TAGNO, DRIT.TAGNO 
            From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PIM(Nolock)
            JOIN DEPARTMENTRAWISSUEMASTER DRIM(Nolock) ON DRIM.ISSUEORDERID = PIM.DepartmentIssueOrderID 
            JOIN DEPARTMENTRAWISSUETRAN DRIT(Nolock) ON DRIT.PRMID = DRIM.PRMID AND DRIT.ITEM_FINISHED_ID = " + Item_Finished_ID + " AND DRIT.LotNo = '" + ddlotno.SelectedItem.Text + @"'
            JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = DRIT.ITEM_FINISHED_ID 
            Where PIM.TYPEFLAG = 1 And PIM.ISSUEORDERID = " + ddOrderNo.SelectedValue + @" 
            Order By DRIT.TAGNO ";
        UtilityModule.ConditionalComboFill(ref DDTagno, Str, true, "Select Tag No");
    }
    
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();

        if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
        {
            btnsave.Visible = false;
            gvdetail.Columns[8].Visible = false;

        }
    }
    private void EditCheckedChanged()
    {
        if (ChKForEdit.Checked == true)
        {
            Td7.Visible = true;
            if (ddOrderNo.Items.Count > 0)
            {

                UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId, ChallanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
                    from DepartmentProcessRawMaster 
                    Where FlagType = 1 And TranType=0 And CompanyID = " + ddCompName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + @" And 
                    IssueOrderID=" + ddOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Challan No");
            }
        }
        else
        {
            Td7.Visible = false;
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedIndexChange();
    }
    private void ChallanNoSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        txtchalanno.Text = "";
        if (DDChallanNo.SelectedIndex > 0)
        {
            ViewState["Prmid"] = DDChallanNo.SelectedValue;

            string strsql2 = "select PRMID,ChallanNo from DepartmentProcessRawMaster PRM where PRM.FlagType = 1 And PRM.Prmid = " + DDChallanNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChallanNo"].ToString();
            }
        }
        Fill_Grid();
    }
    
    protected void DuplicateChallanNo()
    {
        LblError.Text = "";
        LblError.Visible = true;
        try
        {
            if (txtchalanno.Text != "")
            {
                string str = "Select ChallanNo From DepartmentProcessRawMaster Where ChallanNo<>'' And TranType=0 And FlagType = 1 And ChallanNo='" + txtchalanno.Text + "' and Empid>0 And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblError.Text = "Challan no. already exists.....";
                }
            }
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblError.Text == "")
        {
            if (variable.VarMANYFOLIORAWISSUE_SINGLECHALAN == "0")
            {
                DuplicateChallanNo();
            }
        }
        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[22];

                arr[0] = new SqlParameter("@PRMID", SqlDbType.Int);
                arr[1] = new SqlParameter("@COMPANYID", SqlDbType.Int);
                arr[2] = new SqlParameter("@BRANCHID", SqlDbType.Int);
                arr[3] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                arr[4] = new SqlParameter("@ISSUEORDERID", SqlDbType.Int);
                arr[5] = new SqlParameter("@EMPID", SqlDbType.Int);
                arr[6] = new SqlParameter("@DATE", SqlDbType.SmallDateTime);
                arr[7] = new SqlParameter("@CHALLANNO", SqlDbType.NVarChar, 300);
                arr[8] = new SqlParameter("@TRANTYPE", SqlDbType.Int);
                arr[9] = new SqlParameter("@UserID", SqlDbType.Int);
                arr[10] = new SqlParameter("@MAsterCompanyID", SqlDbType.Int);
                arr[11] = new SqlParameter("@FLAGTYPE", SqlDbType.Int);
                arr[12] = new SqlParameter("@PRTID", SqlDbType.Int);
                arr[13] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
                arr[14] = new SqlParameter("@UNITID", SqlDbType.Int);                
                arr[15] = new SqlParameter("@LOTNO", SqlDbType.NVarChar, 200);
                arr[16] = new SqlParameter("@TAGNO", SqlDbType.VarChar, 200);                
                arr[17] = new SqlParameter("@CONETYPE", SqlDbType.VarChar, 50);
                arr[18] = new SqlParameter("@NOOFCONE", SqlDbType.Int);
                arr[19] = new SqlParameter("@QTY", SqlDbType.Float);
                arr[20] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);               

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

                arr[0].Value = ViewState["Prmid"];
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = DDBranchName.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = ddOrderNo.SelectedValue;
                arr[5].Value = ddempname.SelectedValue;
                arr[6].Value = txtdate.Text;
                arr[7].Value = txtchalanno.Text;
                arr[7].Direction = ParameterDirection.InputOutput;
                arr[8].Value = 0;
                arr[9].Value = Session["varuserid"].ToString();
                arr[10].Value = Session["varMasterCompanyIDForERP"].ToString();
                arr[11].Value = 1;
                arr[12].Direction = ParameterDirection.InputOutput;
                arr[13].Value = Varfinishedid;
                arr[14].Value = ddlunit.SelectedValue;                
                arr[15].Value = ddlotno.SelectedItem.Text;
                arr[16].Value = TDTagno.Visible == false ? "Without Tag No" : DDTagno.SelectedItem.Text;
                arr[17].Value = DDconetype.SelectedItem.Text;
                arr[18].Value = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                arr[19].Value = txtissue.Text;
                arr[20].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_DEPARTMENT_PROCESS_RAW_ISSUE", arr);

                Tran.Commit();
                txtchalanno.Text = arr[7].Value.ToString();
                ViewState["Prmid"] = arr[0].Value;
                LblError.Visible = true;
                LblError.Text = arr[20].Value.ToString();
                Fill_Grid();
                fill_Grid_ShowConsmption();
                SaveReferece();
                btnsave.Text = "Save";
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LblError.Visible = true;
                LblError.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void SaveReferece()
    {
        if (ddlshade.Items.Count > 0 && shd.Visible == true)
        {
            ddlshade.SelectedIndex = 0;
        }
        TxtTotalDepartmentIssQty.Text = "";
        txtconqty.Text = "";
        TxtPendQty.Text = "";
        txtissue.Text = "";
        txtnoofcone.Text = "";
    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(ChallanNo,0) asd from DepartmentProcessRawMaster where FlagType = 1 And ChallanNo='" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]));
        if (ChalanNo != "")
        {
            txtchalanno.Text = "";
            txtchalanno.Focus();
            LblError.Visible = true;
            LblError.Text = "Challan No already exist";
        }
        else
        {
            LblError.Visible = false;
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

    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IPM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
        da.Fill(ds);
        count = ds.Tables[0].Rows.Count;
        con.Close();
        List<string> ProductCode = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ProductCode.Add(ds.Tables[0].Rows[i][0].ToString());
        }
        con.Close();
        return ProductCode.ToArray();
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("prmid");
        Session.Remove("finishedid");
        Session.Remove("inhand");
        Session.Remove("stocktranid");
        Session.Remove("stockid");
    }
    protected void txtissue_TextChanged(object sender, EventArgs e)
    {
        double totalQty = Convert.ToDouble(txtconqty.Text == "" ? "0" : txtconqty.Text);
        double PreQty = Math.Round(totalQty - Convert.ToDouble(TxtPendQty.Text == "" ? "0" : TxtPendQty.Text), 3);
        double VarExcessQty = 0;
        if (Session["varcompanyNo"].ToString() == "16" && ddProcessName.SelectedValue != "1")
        {
            VarExcessQty = 0;
        }
        else
        {
            string Str = "Select PercentageExecssQtyForProcessIss From MasterSetting(Nolock)";
            VarExcessQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str));
        }

        int VarEmployeeType = 0;
        if (Session["varcompanyNo"].ToString() == "16" || Session["varcompanyNo"].ToString() == "28")
        {
            VarEmployeeType = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct EI.EmployeeType 
                From Employee_ProcessOrderNo EPO(Nolock) 
                JOIN Empinfo EI(Nolock) ON EI.EmpID = EPO.EmpID 
                Where EPO.ProcessID = " + ddProcessName.SelectedValue + " And EPO.IssueOrderId = " + ddOrderNo.SelectedValue));

            if (VarEmployeeType == 1)
            {
                VarExcessQty = 0;
            }
        }

        totalQty = (totalQty * (100.0 + VarExcessQty) / 100);

        double Qty = Convert.ToDouble(txtissue.Text == "" ? "0" : txtissue.Text);
        double coneweight = UtilityModule.Getconeweight(DDconetype.SelectedItem.Text, Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text));
        Qty = Qty - coneweight;
        if (Qty + PreQty > totalQty)
        {
            txtissue.Text = "";
            LblError.Text = "Pls Enter Correct Qty ";
            LblError.Visible = true;
            txtissue.Focus();
            return;
        }
        else
        {
            LblError.Visible = false;
        }

    }
   
    private Int32 Get_Item_Finished_ID()
    {
        int Varfinishedid = 0;
        int quality = 0;
        int design = 0;
        int color = 0;
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
            Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        }
        return Varfinishedid;
    }
    private void fill_qty(object sender = null)
    {
        txtconqty.Text = "";
        TxtPendQty.Text = "";
        TxtTotalDepartmentIssQty.Text = "";
        if (DDTagno.SelectedIndex > 0)
        {
            int Item_Finished_ID = Get_Item_Finished_ID();

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@ProcessID", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@Item_Finished_ID", Item_Finished_ID);
            param[2] = new SqlParameter("@LotNo", ddlotno.SelectedItem.Text);
            param[3] = new SqlParameter("@TagNo", DDTagno.SelectedItem.Text);
            param[4] = new SqlParameter("@FolioNo", ddOrderNo.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetDepartmentBalQtyConsmpPendingQty", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtTotalDepartmentIssQty.Text = ds.Tables[0].Rows[0]["DepartmentIssQty"].ToString();
                txtconqty.Text = ds.Tables[0].Rows[0]["CONSMPQTY"].ToString();
                TxtPendQty.Text = ((txtconqty.Text == "" ? 0 : Convert.ToDouble(txtconqty.Text)) - Convert.ToDouble(ds.Tables[0].Rows[0]["ISSQTY"])).ToString();
            }
        }
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForMtr.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Mtr");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblError.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddCompName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddProcessName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddempname) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOrderNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtdate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddCatagory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(dditemname) == false)
        {
            goto a;
        }
        if (ql.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (dsn.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dddesign) == false)
            {
                goto a;
            }
        }
        if (clr.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (shp.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
            {
                goto a;
            }
        }
        if (sz.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
            {
                goto a;
            }
        }
        if (shd.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlshade) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlunit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlotno) == false)
        {
            goto a;
        }
        if (TDTagno.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDTagno) == false)
            {
                goto a;
            }

        }
        if (UtilityModule.VALIDTEXTBOX(txtconqty) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(txtissue) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblError);
    B: ;
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        Boolean Istqueryflag = true;
        string str = "";
        if (Session["varcompanyNo"].ToString() == "9")
        {
            Istqueryflag = true;

        }
        else
        {
            if (ddProcessName.SelectedValue == "1")
            {
                Istqueryflag = true;
            }
            else
            {
                if (variable.VarFinishingNewModuleWise == "1")
                {
                    Istqueryflag = false;
                }
                else
                {
                    Istqueryflag = true;
                }
            }
        }
        if (Istqueryflag == true)
        {
            str = @" Select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, 
                                PT.Lotno, '' GodownName, Case When IsNull(EI.EmpName, '') = '' Then 
	                                (Select Distinct EII.EmpName + ', ' 
		                                From Employee_ProcessOrderNo EPO(Nolock) 
		                                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.IssueOrderId And EPO.ProcessId = PM.Processid) 
                                Else EI.EmpName End EmpName, 
                                Case When IsNull(EI.Address, '') = '' Then 
	                                (Select Distinct EII.Address + ', ' 
		                                From Employee_ProcessOrderNo EPO(Nolock) 
		                                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.IssueOrderId And EPO.ProcessId = PM.Processid) 
                                Else EI.Address End Address,
                                CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                                '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                                PM.IssueOrderId Prorderid, 
                                Case When IsNull(EI.GSTNo, '') = '' Then 
	                                (Select Distinct EII.GSTNo + ', ' 
		                                From Employee_ProcessOrderNo EPO(Nolock) 
		                                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.IssueOrderId And EPO.ProcessId = PM.Processid) 
                                Else EI.GSTNo End empgstin,                               
                                CI.GSTNo,PT.TAGNO,'' BINNO, 
                                (Select Distinct CII.CustomerCode + ', '
		                                From PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PID(Nolock) 
		                                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                                        Where PID.IssueOrderId = PM.IssueOrderId For XML Path('')) OrderNo, BM.GstNo BranchGstNo 
                                From DepartmentProcessRawMaster PM 
                                join DepartmentProcessRawTran PT on PM.PRMid=PT.PRMid 
                                JOIN BranchMaster BM ON BM.ID = PM.BranchID 
                                join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                                join V_FinishedItemDetail vf on PT.Item_Finished_id=vf.ITEM_FINISHED_ID 
                                LEFT join EmpInfo Ei on PM.Empid=ei.EmpId 
                                join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                                Where PM.FlagType = 1 And PM.Prmid=" + ViewState["Prmid"];
        }
        else
        {
            str = @" select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, 
                           PT.Lotno, '' GodownName, EI.EmpName, '' Address, CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                           '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                           vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                           PM.IssueOrderId Prorderid, '' as empgstin, CI.GSTNo,PT.TAGNO,'' BINNO, PM.IssueOrderId OrderNo, BM.GstNo BranchGstNo, 
                           0 ReportType 
                           From DepartmentProcessRawMaster PM 
                           JOIN BranchMaster BM ON BM.ID = PM.BranchID 
                           inner join DepartmentProcessRawTran PT on PM.PRMid=PT.PRMid 
                           inner join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                           inner join V_FinishedItemDetail vf on PT.Item_Finished_id=vf.ITEM_FINISHED_ID 
                           inner join V_GetCommaSeparateEmployee Ei on PM.IssueOrderId=ei.Issueorderid and ei.Processid=" + ddProcessName.SelectedValue + @" 
                           inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                           Where PM.FlagType = 1 And PM.Prmid=" + ViewState["Prmid"] + " and PM.Processid=" + ddProcessName.SelectedValue;
        }
       DataSet  ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawIssueRecDuplicateNew.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }
    }
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;

        string str = @"SELECT DISTINCT VIM.COMPANYID, OM.CUSTOMERID, VID.ORDERID, VIM.PROCESSID, 
            CASE WHEN VIM.EMPID = 0 THEN (Select Top 1 EmpID From Employee_ProcessOrderNo EPO(Nolock) 
                                                Where EPO.IssueOrderId = VIM.IssueOrderId And EPO.ProcessId = VIM.PROCESSID
                                         ) ELSE VIM.EMPID END EMPID, 
            VIM.ISSUEORDERID, IsNull(VIM.CHALLANNO, 0) CHALLANNO 
            FROM VIEW_PROCESS_ISSUE_MASTER VIM(nolock) 
            INNER JOIN VIEW_PROCESS_ISSUE_DETAIL VID(nolock) ON VIM.ISSUEORDERID=VID.ISSUEORDERID AND VIM.PROCESSID=VID.PROCESS_NAME_ID AND VIM.STATUS<>'CANCELED'
            INNER JOIN ORDERMASTER OM ON VID.ORDERID=OM.ORDERID 
            WHere IsNull(VIM.DEPARTMENTTYPE, 0) = 1 And VIM.COMPANYID = " + ddCompName.SelectedValue + " And VIm.ChallanNo='" + VarPOrderNo + "'";
        if (Session["varMasterCompanyIDForERP"].ToString() == "16")
        {
            str = str + " and vim.processid=1";
        }
        if (ddProcessName.SelectedIndex > 0)
        {
            str = str + " and vim.processid=" + ddProcessName.SelectedValue;
        }

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
            ProcessNameSelectedIndexChange();
            ddempname.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
            EmpNameSelectedIndexChange();
            if (ddOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["ISSUEOrderId"].ToString()) != null)
            {
                ddOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
            }
            else
            {
                LblError.Text = "This Po No. does not exists or Sample Po.";
                LblError.Visible = true;
                return;

            }
            OrderNoSelectedIndexChange();
            if (DDChallanNo.Items.Count > 0)
            {
                DDChallanNo.SelectedIndex = 1;
                ChallanNoSelectedIndexChange();
            }
        }
        else
        {
            if (ddOrderNo.Items.Count > 0)
            {
                ddOrderNo.SelectedIndex = 0;
                fill_Grid_ShowConsmption();
            }
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 0;
            }
            TxtPOrderNo.Text = "";
            TxtPOrderNo.Focus();
        }
    }
    private void fill_Grid_ShowConsmption()
    {
        DataSet ds = null;

        string Str = @"SELECT VF1.CATEGORY_NAME,VF1.ITEM_NAME,VF1.QUALITYNAME+SPACE(2)+VF1.DESIGNNAME+SPACE(2)+VF1.COLORNAME+SPACE(2)+VF1.SHAPENAME+SPACE(2)+
        CASE WHEN PM.UNITID=1 THEN VF1.SIZEMTR ELSE VF1.SIZEFT END+SPACE(2)+VF1.SHADECOLORNAME DESCRIPTION,
        ISNULL(ROUND(SUM(CASE WHEN PM.CALTYPE=0 OR PM.CALTYPE=2 THEN CASE WHEN PM.UNITID=1 THEN PD.QTY*OCD.IQTY*1.196 * 
        Case When VF.MasterCompanyId in (28, 16) Then VF.AreaMtr Else PD.Area End 
        ELSE PD.QTY*OCD.IQTY * Case When VF.MasterCompanyId in (28, 16) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else PD.Area End END ELSE 
        CASE WHEN PM.UNITID=1 THEN PD.QTY*OCD.IQTY*1.196 ELSE PD.QTY*OCD.IQTY END END),3),0) CONSMPQTY,
        IsNull((SELECT ISNULL(SUM(case When a.trantype=0 Then b.QTY ELse - b.Qty End), 0) 
	         FROM DepartmentProcessRawMaster a(nolock)
	         JOIN DepartmentProcessRawTran b(nolock) ON b.PRMID = a.PRMID 
	         WHERE a.FlagType = 1 And a.IssueOrderID = PM.IssueOrderId AND b.Item_FINISHED_ID = OCD.IFINISHEDID AND a.PROCESSID = " + ddProcessName.SelectedValue + @"), 0) ISSQTY,
        Round(ISNULL(ROUND(SUM(CASE WHEN PM.CALTYPE=0 OR PM.CALTYPE=2 THEN CASE WHEN PM.UNITID=1 THEN PD.QTY*OCD.IQTY*1.196 * Case When VF.MasterCompanyId in (28, 16) Then VF.AreaMtr Else PD.Area End 
        ELSE PD.QTY*OCD.IQTY * Case When VF.MasterCompanyId in (28, 16) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else PD.Area End END ELSE 
        CASE WHEN PM.UNITID=1 THEN PD.QTY*OCD.IQTY*1.196 ELSE PD.QTY*OCD.IQTY END END),3),0)-IsNull((SELECT ISNULL(SUM(case When a.trantype=0 Then b.QTY ELse - b.Qty End), 0) 
	         FROM DepartmentProcessRawMaster a(nolock)
	         JOIN DepartmentProcessRawTran b(nolock) ON b.PRMID = a.PRMID 
	         WHERE a.FlagType = 1 And a.IssueOrderID = PM.IssueOrderId AND b.Item_FINISHED_ID = OCD.IFINISHEDID AND a.PROCESSID = " + ddProcessName.SelectedValue + @"), 0),3) PENDQTY 
        FROM PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + @" PM 
        JOIN PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PD ON PM.ISSUEORDERID=PD.ISSUEORDERID 
        JOIN PROCESS_CONSUMPTION_DETAIL OCD ON OCD.ISSUEORDERID=PD.ISSUEORDERID AND OCD.ISSUE_DETAIL_ID=PD.ISSUE_DETAIL_ID AND OCD.PROCESSID=" + ddProcessName.SelectedValue + @"
        JOIN V_FINISHEDITEMDETAIL VF1 ON VF1.ITEM_FINISHED_ID=OCD.IFINISHEDID 
        JOIN CATEGORYSEPARATE CS ON VF1.CATEGORY_ID=CS.CATEGORYID AND CS.ID=1  AND VF1.MASTERCOMPANYID= " + Session["varMasterCompanyIDForERP"] + @" 
        JOIN V_FINISHEDITEMDETAIL VF ON PD.ITEM_FINISHED_ID = VF.ITEM_FINISHED_ID 
        WHERE PM.ISSUEORDERID=" + ddOrderNo.SelectedValue + @"
        GROUP BY VF1.CATEGORY_NAME,VF1.ITEM_NAME,VF1.QUALITYNAME,VF1.DESIGNNAME,VF1.COLORNAME,VF1.SHAPENAME,PM.UNITID,VF1.SIZEMTR,VF1.SIZEFT,
        VF1.SHADECOLORNAME,OCD.IFINISHEDID,PM.ISSUEORDERID";
        
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        GDGridShow.DataSource = ds;
        GDGridShow.DataBind();
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPrtID = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            ViewState["Prmid"] = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select PrmId from DepartmentProcessRawTran Where PrtId=" + VarPrtID);
            
            SqlParameter[] arr = new SqlParameter[7];

            arr[0] = new SqlParameter("@PrtID", SqlDbType.Int);
            arr[1] = new SqlParameter("@RowCount", SqlDbType.Int);
            arr[2] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[6] = new SqlParameter("@FlagType", SqlDbType.Int);

            arr[0].Value = VarPrtID;
            arr[1].Value = 2;
            arr[2].Value = 0;
            if (gvdetail.Rows.Count == 1)
            {
                arr[1].Value = 1;
            }
            arr[3].Direction = ParameterDirection.Output;
            arr[6].Value = 1;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_DEPARTMENT_PROCESS_RAW_ISSUE", arr);
            if (arr[3].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + arr[3].Value.ToString() + "');", true);
            }
            else
            {
                LblError.Text = "Row Item Deleted successfully.";
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        Fill_Grid();
    }

    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
}