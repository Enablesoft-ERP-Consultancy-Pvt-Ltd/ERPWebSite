using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_TasselMaking_FrmTasselMakingOrderRowRecieve : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        DataSet DSQ = null; string Qry = "";
        if (!IsPostBack)
        {
            ViewState["Prmid"] = 0;
            Qry = @" Select Distinct CI.CompanyId,Companyname 
                From Companyinfo CI(Nolock) 
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 

                Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                From ProcessIssueToTasselMakingMaster HFMO(Nolock) 
                JOIN PROCESS_NAME_MASTER PNM (Nolock) ON PNM.PROCESS_NAME_ID = HFMO.PROCESSID 
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                Order By PROCESS_NAME 
                Select ConeType, ConeType From ConeMaster Order By SrNo 
                Select Distinct GM.GodownId,GM.GodownName 
                From GodownMaster GM(Nolock) 
                JOIN Godown_Authentication GA(Nolock) ON GM.GodownId=GA.GodownId 
                Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by GodownName";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, DSQ, 3, true, "--Select--");

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
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
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;

        string str = @"SELECT DISTINCT HMOM.PROCESSID, HMOM.EMPID, 
                    HMOM.ISSUEORDERID, IsNull(HMOM.IssueNo, 0) CHALLANNO 
                    FROM ProcessIssueToTasselMakingMaster HMOM
                    Where HMOM.STATUS <> 'CANCELED' And HMOM.COMPANYID = " + ddCompName.SelectedValue + " And HMOM.IssueNo='" + VarPOrderNo + "'";
        if (ddProcessName.SelectedIndex > 0)
        {
            str = str + " and HMOM.ProcessID = " + ddProcessName.SelectedValue;
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
            fill_qty();
            Fill_GodownSelectedChange();
        }
        else
        {
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 0;
            }
            TxtPOrderNo.Text = "";
            TxtPOrderNo.Focus();
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT EMP.ProcessId, EI.EmpId 
                FROM ProcessIssueToTasselMakingMaster EMP(Nolock) 
                JOIN EMPINFO EI ON EMP.EMPID = EI.EMPID 
                WHERE EI.EMPCODE = '" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddProcessName.Items.FindByValue(ds.Tables[0].Rows[0]["Processid"].ToString()) != null)
            {
                ddProcessName.SelectedValue = ds.Tables[0].Rows[0]["Processid"].ToString();
                if (sender != null)
                {
                    ddProcessName_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (ddempname.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                ddempname.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                if (sender != null)
                {
                    ddempname_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            ddOrderNo.Focus();
        }
        else
        {
            ddProcessName.SelectedIndex = -1;
            ddOrderNo.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "fillemp", "alert('Please Enter correct Emp. Code or No entry from this employee')", true);
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChange();
    }
    private void ProcessNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        Fill_Grid();
        string str = @"Select Distinct EI.EmpID,  EI.EmpName + ' / ' + EI.EmpCode EmpName 
        From ProcessIssueToTasselMakingMaster a(Nolock) 
        JOIN Empinfo EI(Nolock) ON EI.EmpID = a.EmpID 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varMasterCompanyIDForERP"] + @" 
        Order By EmpName ";

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
        Fill_Grid();

        string str = @"Select Distinct a.ISSUEORDERID, a.IssueNo CHALLANNO 
        From ProcessIssueToTasselMakingMaster a(Nolock) 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varMasterCompanyIDForERP"] + @" 
        And a.EmpID = " + ddempname.SelectedValue + @" And a.STATUS = 'PENDING' 
        Order By a.ISSUEORDERID Desc";
        UtilityModule.ConditionalComboFill(ref ddOrderNo, str, true, "Select order no");
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChange();
    }
    private void OrderNoSelectedIndexChange()
    {
        if (ChKForEdit.Checked == true)
        {
            EditCheckedChanged();
        }
        else
        {
            Fill_Grid();
        }

        UtilityModule.ConditionalComboFill(ref ddCatagory, @" Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME 
            From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Category Name");

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
            From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
			Order By VF.ITEM_NAME ", true, "--Select Item--");

            if (dditemname.Items.Count > 0)
            {
                dditemname.SelectedIndex = 1;
                ItemName_SelectChange();
            }
        }
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID], [CATEGORY_ID], IPM.[PARAMETER_ID], PARAMETER_NAME 
                    FROM [ITEM_CATEGORY_PARAMETERS] IPM(Nolock) 
                    JOIN PARAMETER_MASTER PM(Nolock) ON PM.[PARAMETER_ID] = IPM.[PARAMETER_ID] 
                    Where [CATEGORY_ID] = " + ddCatagory.SelectedValue + " And PM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
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
                        UtilityModule.ConditionalComboFill(ref dddesign, @"Select Distinct VF.DesignID 
                        From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorID, VF.ColorName 
                        From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeID, VF.ShapeName 
                        From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Shape--");
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeID, VF.SizeFt 
                        From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.ShapeID = " + ddshape.SelectedValue + @"
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Size in Ft");
                        break;
                    case "6":
                        shd.Visible = true;
                        break;
                }
            }
        }
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
        Td7.Visible = false;
        if (ChKForEdit.Checked == true)
        {
            Td7.Visible = true;
            if (ddOrderNo.Items.Count > 0)
            {
                UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId, ChalanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
                From ProcessRawMaster(Nolock) 
                Where TranType = 1 And TypeFlag = 2 And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + @" And 
                EmpID = " + ddempname.SelectedValue + " And ProrderId = " + ddOrderNo.SelectedValue + " And MasterCompanyId = " + Session["varMasterCompanyIDForERP"], true, "Select Challan No");
            }
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

            string strsql2 = @"Select PRMID, ChalanNo, Remark 
            From ProcessRawMaster PRM(Nolock) 
            Where PRM.Prmid = " + DDChallanNo.SelectedValue + " And PRM.TypeFlag = 2 And PRM.ProcessID = " + ddProcessName.SelectedValue + " And PRM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChalanNo"].ToString();
                txtremark.Text = ds2.Tables[0].Rows[0]["Remark"].ToString();
            }
        }
        Fill_Grid();
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Category_SelectedChange();
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemName_SelectChange();
    }
    private void ItemName_SelectChange()
    {
        if (dditemname.SelectedIndex >= 0)
        {
            string Qry = @" SELECT U.UnitId, U.UnitName 
            FROM ITEM_MASTER IM(Nolock) 
            JOIN Unit U(Nolock) ON U.UnitTypeID = IM.UnitTypeID Where IM.ITEM_ID = " + dditemname.SelectedValue + "  And IM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @"
            Select Distinct VF.QualityID, VF.QualityName 
            From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];

            DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
            if (ddlunit.Items.Count > 0)
            {
                ddlunit.SelectedIndex = 1;
            }

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
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct VF.ShadeColorID, VF.ShadeColorName 
                    From PROCESS_TASSELMAKING_CONSUMPTION_DETAIL HCD(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
                        VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @"
                    Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + @" And 
                        VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By VF.ShadeColorName", true, "Select Shadecolor");
        }
        fill_qty();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_qty(sender);
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(ddgodown.SelectedValue), Convert.ToInt32(hnitemfinishedid.Value), New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster where GODOWNID=" + ddgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            }
        }
    }
    protected void FillTagNo(object sender = null)
    {
        string str = @"Select Distinct Prt.Tagno,PRT.Tagno 
                From ProcessRawMaster PRM(Nolock)
                JOIN ProcessRawTran PRT (Nolock) ON PRM.PRMid=PRT.PRMid 
                Where TranType=0 And PRM.TypeFlag = 2 And PRT.Finishedid=" + hnitemfinishedid.Value + " And PRM.Prorderid=" + ddOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and prt.Lotno='" + ddlotno.SelectedItem.Text + "'";

        UtilityModule.ConditionalComboFill(ref DDTagno, str, true, "--Plz Select--");

        if (DDTagno.Items.Count > 0)
        {
            DDTagno.SelectedIndex = 1;
            if (sender != null)
            {
                DDTagno_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
    private void Fill_GodownSelectedChange(object sender = null)
    {
        string str = "Select Distinct lotno,lotno from stock Where CompanyId=" + ddCompName.SelectedValue + " And Godownid=" + ddgodown.SelectedValue + " and item_finished_id=" + hnitemfinishedid.Value;
        if (MySession.Stockapply == "True" && ChKForEdit.Checked == false)
        {
            str = str + " and Round(QtyInHand,3)>0";
        }

        UtilityModule.ConditionalComboFill(ref ddlotno, str, true, "--Select--");
        ddlotno.SelectedIndex = -1;
        if (ddlotno.Items.Count > 0)
        {
            ddlotno.SelectedIndex = 1;

            if (sender != null && ddlotno.SelectedIndex > 0)
            {
                ddlotno_SelectedIndexChanged(sender, new EventArgs());
            }
        }
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
            FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
        }
    }
    protected void DuplicateChallanNo()
    {
        LblError.Text = "";
        LblError.Visible = true;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtchalanno.Text != "")
            {
                string str = "Select ChalanNo From ProcessRawMaster Where ChalanNo<>'' And TranType=0 And TypeFlag = 2 And ChalanNo='" + txtchalanno.Text + "' and Empid>0 And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
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
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        if (Convert.ToDouble(txtRecieveQty.Text) > 0)
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
                    SqlParameter[] arr = new SqlParameter[37];

                    arr[0] = new SqlParameter("@PrmID", SqlDbType.Int);
                    arr[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                    arr[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                    arr[3] = new SqlParameter("@ProcessId", SqlDbType.Int);
                    arr[4] = new SqlParameter("@OrderId", SqlDbType.Int);
                    arr[5] = new SqlParameter("@IssueDate", SqlDbType.SmallDateTime);
                    arr[6] = new SqlParameter("@ChalanNo", SqlDbType.NVarChar, 50);
                    arr[7] = new SqlParameter("@TranType", SqlDbType.Int);
                    arr[8] = new SqlParameter("@userid", SqlDbType.Int);
                    arr[9] = new SqlParameter("@mastercompanyid", SqlDbType.Int);
                    arr[10] = new SqlParameter("@Prtid", SqlDbType.Int);
                    arr[11] = new SqlParameter("@CategoryId", SqlDbType.Int);
                    arr[12] = new SqlParameter("@Itemid", SqlDbType.Int);
                    arr[13] = new SqlParameter("@FinishedId", SqlDbType.Int);
                    arr[14] = new SqlParameter("@GodownId", SqlDbType.Int);
                    arr[15] = new SqlParameter("@IssueQuantity", SqlDbType.Float);
                    arr[16] = new SqlParameter("@lotNo", SqlDbType.NVarChar, 50);
                    arr[17] = new SqlParameter("@UnitId", SqlDbType.Int);
                    arr[18] = new SqlParameter("@PrmIdOutPut", SqlDbType.Int);
                    arr[19] = new SqlParameter("@PrtIdOutPut", SqlDbType.Int);
                    arr[20] = new SqlParameter("@UpdateFlag", SqlDbType.Int);
                    arr[21] = new SqlParameter("@BinNo", SqlDbType.VarChar, 50);
                    arr[22] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
                    arr[23] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                    arr[24] = new SqlParameter("@TanaBana", SqlDbType.VarChar, 50);
                    arr[25] = new SqlParameter("@TransportName", SqlDbType.VarChar, 50);
                    arr[26] = new SqlParameter("@BiltyNo", SqlDbType.VarChar, 50);
                    arr[27] = new SqlParameter("@VehicleNo", SqlDbType.VarChar, 20);
                    arr[28] = new SqlParameter("@EstimatedRate", SqlDbType.Float);
                    arr[29] = new SqlParameter("@Conetype", SqlDbType.VarChar, 50);
                    arr[30] = new SqlParameter("@Noofcone", SqlDbType.Int);
                    arr[31] = new SqlParameter("@Remark", txtremark.Text);
                    arr[32] = new SqlParameter("@FolioChallanNo", SqlDbType.VarChar, 50);
                    arr[33] = new SqlParameter("@BellWt", SqlDbType.Float);
                    arr[34] = new SqlParameter("@CGSTSGST", SqlDbType.Float);
                    arr[35] = new SqlParameter("@ItemDesignID", SqlDbType.Int);
                    arr[36] = new SqlParameter("@TypeFlag", SqlDbType.Int);

                    arr[0].Value = ViewState["Prmid"];
                    arr[1].Value = ddCompName.SelectedValue;
                    arr[2].Value = ddempname.SelectedValue;
                    arr[3].Value = ddProcessName.SelectedValue;
                    arr[4].Value = ddOrderNo.SelectedValue;
                    arr[5].Value = txtdate.Text;
                    arr[6].Value = txtchalanno.Text;
                    arr[6].Direction = ParameterDirection.InputOutput;
                    arr[7].Value = 1;
                    arr[8].Value = Session["varuserid"].ToString();
                    arr[9].Value = Session["varMasterCompanyIDForERP"].ToString();
                    arr[10].Value = 0;
                    arr[20].Value = 0;
                    if (btnsave.Text == "Update")
                    {
                        arr[10].Value = gvdetail.SelectedDataKey.Value;
                        arr[20].Value = 1;
                    }
                    arr[11].Value = ddCatagory.SelectedValue;
                    arr[12].Value = dditemname.SelectedValue;
                    arr[13].Value = hnitemfinishedid.Value;
                    arr[14].Value = ddgodown.SelectedValue;
                    arr[15].Value = txtRecieveQty.Text;
                    arr[16].Value = ddlotno.SelectedItem.Text;
                    arr[17].Value = ddlunit.SelectedValue;
                    arr[18].Direction = ParameterDirection.Output;
                    arr[19].Direction = ParameterDirection.Output;
                    string BinNo = TDBinNo.Visible == false ? "" : (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "");
                    arr[21].Value = BinNo;
                    arr[22].Value = TDTagno.Visible == false ? "Without Tag No" : DDTagno.SelectedItem.Text;
                    arr[23].Direction = ParameterDirection.Output;
                    arr[24].Value = "";
                    arr[25].Value = "";
                    arr[26].Value = "";
                    arr[27].Value = "";
                    arr[28].Value = 0;
                    arr[29].Value = DDconetype.SelectedItem.Text;
                    arr[30].Value = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                    arr[32].Value = ddOrderNo.SelectedIndex > 0 ? ddOrderNo.SelectedItem.Text : "";
                    arr[33].Value = TxtBellWt.Text == "" ? "0" : TxtBellWt.Text;
                    arr[34].Value = 0;
                    arr[35].Value = 0;
                    arr[36].Value = 2;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE", arr);

                    Tran.Commit();
                    txtchalanno.Text = arr[6].Value.ToString();
                    ViewState["Prmid"] = arr[18].Value;
                    LblError.Visible = true;
                    LblError.Text = arr[23].Value.ToString();
                    Fill_Grid();
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
    }
    private void SaveReferece()
    {
        if (ddlshade.Items.Count > 0 && shd.Visible == true)
        {
            ddlshade.SelectedIndex = 0;
        }
        TxtPendQty.Text = "";
        txtissue.Text = "";
        txtRecieveQty.Text = "";
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
            if (Convert.ToInt32(ViewState["Prmid"]) != 0)
            {
                strsql = @"Select PrtId,CATEGORY_NAME,ITEM_NAME,QualityName+ Space(2)+DesignName+ Space(2)+ColorName+ Space(2)+ShapeName+ Space(2)+SizeFt+ Space(2)+ShadeColorName DESCRIPTION,
                    IssueQuantity Qty,LotNo,GodownName,Pt.BinNo,PT.TagNo 
                    From ProcessRawTran PT(Nolock)
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.Item_Finished_id = PT.Finishedid And VF.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 
                    JOIN GodownMaster GM ON GM.GodownId = PT.GodownId 
                    Where PT.PrmID=" + ViewState["Prmid"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            }
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters_process_ProcessRawIssue|fill_Data_grid|" + ex.Message);
        }
        return ds;
    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(ChalanNo,0) asd from ProcessRawMaster where TypeFlag = 2 And ChalanNo='" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]));
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
            for (int i = 0; i < gvdetail.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (gvdetail.Columns[i].HeaderText.ToUpper() == "BIN NO.")
                    {
                        gvdetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }

    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "" && ddOrderNo.SelectedIndex > 0)
        {

            Str = "select IPM.*,IM.CATEGORY_ID  from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,PROCESS_CONSUMPTION_DETAIL PCD  WHERE IPM.ITEM_FINISHED_ID = PCD.IFINISHEDID and PCD.ISSUEORDERID =" + ddOrderNo.SelectedValue + " and IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Qry = @"select category_id,category_name from item_category_master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                Qry = Qry + " Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + "  select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ";
                Qry = Qry + "  SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by colorid";
                Qry = Qry + "  select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid  ";
                Qry = Qry + "  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " ANd SHAPEID=" + ddshape.SelectedValue + "";
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 0, true, "select");
                ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dditemname, DSQ, 1, true, "--Select Item--");
                dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "--Select Design--");
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();

                Session["finishedid"] = ds.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;

                }
                else
                {
                    ql.Visible = false;

                }
                if (Convert.ToInt32(dddesign.SelectedValue) > 0)
                {
                    dsn.Visible = true;
                }
                else
                {

                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;

                }
                else
                {
                    clr.Visible = false;
                }


                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }

                else
                {
                    shp.Visible = false;
                }

                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Unit");
            }
            else
            {
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddCatagory.Items.Clear();
        }
        fill_qty();
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("prmid");
        Session.Remove("finishedid");
        Session.Remove("inhand");
        Session.Remove("stocktranid");
        Session.Remove("stockid");
    }
    private void fill_qty(object sender = null)
    {
        TxtPendQty.Text = "0";
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
            ddlotno.Items.Clear();

            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

            hnitemfinishedid.Value = Varfinishedid.ToString();

            UtilityModule.ConditionalComboFill(ref ddlotno, @"Select Distinct Prt.LotNo,PRT.Lotno 
                From ProcessRawMaster PRM(Nolock)
                JOIN ProcessRawTran PRT(Nolock) ON PRT.PRMid = PRM.PRMid 
                Where PRM.TranType=0 And PRM.TypeFlag = 2 And PRT.Finishedid=" + Varfinishedid + " And PRM.Prorderid=" + ddOrderNo.SelectedValue + " And PRM.MasterCompanyId= " + Session["varMasterCompanyIDForERP"], true, "--Select--");
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
        if (UtilityModule.VALIDDROPDOWNLIST(ddgodown) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlotno) == false)
        {
            goto a;
        }
        if (TDBinNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDBinNo) == false)
            {
                goto a;
            }
        }
        if (TDTagno.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDTagno) == false)
            {
                goto a;
            }
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
    B:;
    }
    private void WayChallanFormatReport()
    {
        SqlParameter[] _array = new SqlParameter[3];
        _array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["Prmid"];
        _array[1].Value = ddProcessName.SelectedValue;
        _array[2].Value = 0; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RawMaterialIssuedDetail_Hafizia", _array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialIssueWayChallanHafizia.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialIssueWayChallanHafizia.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    private void WayChallanFormatBackReport()
    {
        SqlParameter[] _array = new SqlParameter[3];
        _array[0] = new SqlParameter("@prmId", SqlDbType.Int);
        _array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@Trantype", SqlDbType.Int);

        _array[0].Value = ViewState["Prmid"];
        _array[1].Value = ddProcessName.SelectedValue;
        _array[2].Value = 0; //For Issue

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_RawMaterialIssuedWayChallanBackFormat_Hafizia", _array);

        if (ds.Tables[1].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawMaterialIssueWayChallanBackFormatHafizia.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawMaterialIssueWayChallanBackFormatHafizia.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }

    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarPrtID = Convert.ToInt32(gvdetail.DataKeys[e.RowIndex].Value);
            SqlParameter[] arr = new SqlParameter[7];

            arr[0] = new SqlParameter("@PrtID", SqlDbType.Int);
            arr[1] = new SqlParameter("@RowCount", SqlDbType.Int);
            arr[2] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[6] = new SqlParameter("@TypeFlag", 2);

            arr[0].Value = VarPrtID;
            arr[1].Value = 2;
            arr[2].Value = 0;
            if (gvdetail.Rows.Count == 1)
            {
                arr[1].Value = 1;
            }
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE_RECEIVE_DELETE", arr);
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
        FIllissuedqty(Convert.ToInt32(hnitemfinishedid.Value));
    }

    protected void FIllissuedqty(int ItemFinishedId)
    {
        txtissue.Text = "";
        string str = @"Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
                     Where PRM.PRMid=PRT.PRMid And TranType=0 And PRM.TypeFlag = 2 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        string str1 = @" Select IsNull(Sum(IssueQuantity),0) AS Qty From ProcessRawMaster PRM,ProcessRawTran PRT
                     Where PRM.PRMid=PRT.PRMid And TranType=1 And PRM.TypeFlag = 2 And PRT.Finishedid=" + ItemFinishedId + " And PRM.Prorderid=" + ddOrderNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddlotno.SelectedIndex > 0)
        {
            str = str + "  and Prt.LotNo='" + ddlotno.SelectedItem.Text + "'";
            str1 = str1 + "  and Prt.LotNo='" + ddlotno.SelectedItem.Text + "'";
        }
        if (DDTagno.SelectedIndex > 0)
        {
            str = str + "  and Prt.Tagno='" + DDTagno.SelectedItem.Text + "'";
            str1 = str1 + "  and Prt.Tagno='" + DDTagno.SelectedItem.Text + "'";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str + str1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtissue.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]).ToString();
            TxtPendQty.Text = (Convert.ToDouble(ds.Tables[0].Rows[0]["Qty"]) - Convert.ToDouble(ds.Tables[1].Rows[0]["Qty"])).ToString();
        }
    }

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = @" select PM.Date, PM.ChalanNo, PM.trantype, PT.IssueQuantity, 
                    PT.Lotno, GM.GodownName, EI.EmpName, EI.Address, CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                    '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                    vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                    PM.Prorderid, EI.GSTNo as empgstin, CI.GSTNo,PT.TagNo,PT.BinNo, BM.GstNo BranchGstNo, 
                    0 ReportType 
                    From ProcessRawMaster PM(Nolock) 
                    inner join ProcessRawTran PT(Nolock) on PM.PRMid=PT.PRMid
                    JOIN BranchMaster BM(Nolock) ON BM.ID = PM.BranchID 
                    inner join CompanyInfo ci(Nolock) on PM.Companyid=ci.CompanyId
                    inner join V_FinishedItemDetail vf(Nolock) on PT.Finishedid=vf.ITEM_FINISHED_ID
                    inner join GodownMaster GM(Nolock) on PT.Godownid=GM.GoDownID
                    inner join EmpInfo Ei(Nolock) on PM.Empid=ei.EmpId
                    inner join PROCESS_NAME_MASTER PNM(Nolock) on PM.Processid=PNM.PROCESS_NAME_ID 
                    Where PM.TypeFlag = 2 And PM.Prmid=" + ViewState["Prmid"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%'";
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
}

