using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_HomeFurnishing_FrmHomeFurnishingProductionOrderRowIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
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
                Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By Companyname 

                Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                From HomeFurnishingOrderMaster HFMO(Nolock) 
                JOIN PROCESS_NAME_MASTER PNM (Nolock) ON PNM.PROCESS_NAME_ID = HFMO.PROCESSID 
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                Order By PROCESS_NAME 
                Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 2, false, "");

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
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;

        string str = @"SELECT DISTINCT HMOM.PROCESSID, (Select Top 1 EmpID From Employee_HomeFurnishingOrderMaster EPO(Nolock) 
                                                    Where EPO.IssueOrderId = HMOM.IssueOrderId And EPO.ProcessId = HMOM.PROCESSID) EMPID, 
                    HMOM.ISSUEORDERID, IsNull(HMOM.CHALLANNO, 0) CHALLANNO 
                    FROM HomeFurnishingOrderMaster HMOM
                    Where HMOM.STATUS <> 'CANCELED' And HMOM.COMPANYID = " + ddCompName.SelectedValue + " And HMOM.ChallanNo='" + VarPOrderNo + "'";
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
            Fill_LotNoSelectedChange();
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
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillProcess_Employee(sender);
    }
    protected void FillProcess_Employee(object sender = null)
    {
        string str = @"SELECT Top(1) EMP.ProcessId, EI.EmpId 
                FROM Employee_HomeFurnishingOrderMaster EMP(Nolock) 
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
        From HomeFurnishingOrderMaster a(Nolock) 
        JOIN Employee_HomeFurnishingOrderMaster EMP(Nolock) ON EMP.IssueOrderID = a.ISSUEORDERID 
        JOIN Empinfo EI(Nolock) ON EI.EmpID = EMP.EmpID 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
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

        string str = @"Select Distinct a.ISSUEORDERID, a.CHALLANNO 
        From HomeFurnishingOrderMaster a(Nolock) 
        JOIN Employee_HomeFurnishingOrderMaster EMP(Nolock) ON EMP.IssueOrderID = a.ISSUEORDERID And EMP.EmpID = " + ddempname.SelectedValue + @" 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + " And a.MASTERCOMPANYID = " + Session["varcompanyId"] + @" 
         And a.STATUS = 'PENDING' 
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
            From HomeFurnishingConsumptionDetail HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Category Name");

        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
        }
        Fill_Category_SelectedChange();
        fill_Grid_ShowConsmption();
    }
    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();

            UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME 
            From HomeFurnishingConsumptionDetail HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + @"
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
                    Where [CATEGORY_ID] = " + ddCatagory.SelectedValue + " And PM.MasterCompanyId = " + Session["varCompanyId"];
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
                        From HomeFurnishingConsumptionDetail HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorID, VF.ColorName 
                        From HomeFurnishingConsumptionDetail HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeID, VF.ShapeName 
                        From HomeFurnishingConsumptionDetail HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Shape--");
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeID, VF.SizeFt 
                        From HomeFurnishingConsumptionDetail HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.ShapeID = " + ddshape.SelectedValue + @"
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Size in Ft");
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
                Where TranType = 0 And TypeFlag = 1 And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + @" And 
                EmpID = " + ddempname.SelectedValue + " And ProrderId = " + ddOrderNo.SelectedValue + " And MasterCompanyId = " + Session["varCompanyId"], true, "Select Challan No");
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
            Where PRM.Prmid = " + DDChallanNo.SelectedValue + " And PRM.TypeFlag = 1 And PRM.ProcessID = " + ddProcessName.SelectedValue + " And PRM.MasterCompanyId = " + Session["varCompanyId"];
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
            JOIN Unit U(Nolock) ON U.UnitTypeID = IM.UnitTypeID Where IM.ITEM_ID = " + dditemname.SelectedValue + "  And IM.MasterCompanyId = " + Session["varCompanyId"] + @"
            Select Distinct VF.QualityID, VF.QualityName 
            From HomeFurnishingConsumptionDetail HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And HCD.MasterCompanyId=" + Session["varCompanyId"];

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
                    From HomeFurnishingConsumptionDetail HCD(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
                        VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @"
                    Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + @" And 
                        HCD.MasterCompanyId=" + Session["varCompanyId"] + " Order By VF.ShadeColorName", true, "Select Shadecolor");
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
        Fill_GodownSelectedChange(sender);
    }
    protected void FillBinNo(object sender = null)
    {
        txtstock.Text = "";
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string str = "Select Distinct BinNo,BinNo from stock Where CompanyId=" + ddCompName.SelectedValue + " And Godownid=" + ddgodown.SelectedValue + "   and item_finished_id=" + Varfinishedid + " and LotNo='" + ddlotno.SelectedItem.Text + "'";
            if (MySession.Stockapply == "True" && ChKForEdit.Checked == false)
            {
                str = str + "  And QtyInHand>0";
            }
            if (TDTagno.Visible == true)
            {
                str = str + "  And TagNo='" + DDTagno.SelectedItem.Text + "'";
            }
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
                if (sender != null)
                {
                    DDBinNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
    }
    protected void FillTagNo(object sender = null)
    {
        txtstock.Text = "";
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string str = "Select Distinct TagNo,Tagno from stock Where CompanyId=" + ddCompName.SelectedValue + " And Godownid=" + ddgodown.SelectedValue + "   and item_finished_id=" + Varfinishedid + " and LotNo='" + ddlotno.SelectedItem.Text + "'";
            if (MySession.Stockapply == "True" && ChKForEdit.Checked == false)
            {
                str = str + " and Round(Qtyinhand,3)>0";
            }

            UtilityModule.ConditionalComboFill(ref DDTagno, str, true, "--Select--");
            if (DDTagno.Items.Count > 0)
            {

                DDTagno.SelectedIndex = 1;
                if (sender != null)
                {
                    DDTagno_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
    }
    private void Fill_GodownSelectedChange(object sender = null)
    {
        txtstock.Text = "";
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            string str = "Select Distinct lotno,lotno from stock Where CompanyId=" + ddCompName.SelectedValue + " And Godownid=" + ddgodown.SelectedValue + " and item_finished_id=" + Varfinishedid;
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
            Fill_LotNoSelectedChange();
        }
    }
    private void Fill_LotNoSelectedChange()
    {
        if (ddlotno.Items.Count > 0)
        {
            txtstock.Text = "0";
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
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                ViewState["FinishedID"] = Varfinishedid;
                string TagNo = "Without Tag No";
                string BinNo = "";
                if (TDTagno.Visible == true)
                {
                    TagNo = DDTagno.SelectedIndex > 0 ? DDTagno.SelectedItem.Text : "Without Tag No";
                }
                if (TDBinNo.Visible == true)
                {
                    BinNo = DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
                }
                txtstock.Text = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.SelectedItem.Text, Varfinishedid, TagNo: TagNo, BinNo: BinNo).ToString();
            }
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
                string str = "Select ChalanNo From ProcessRawMaster Where ChalanNo<>'' And TranType=0 And TypeFlag = 1 And ChalanNo='" + txtchalanno.Text + "' and Empid>0 And PRMID<>" + ViewState["Prmid"] + " And MasterCompanyId=" + Session["varCompanyId"];
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
        CHECKVALIDCONTROL();
        if (LblError.Text == "")
        {
            if (variable.VarMANYFOLIORAWISSUE_SINGLECHALAN == "0")
            {
                DuplicateChallanNo();
            }
        }
        if (Session["VarcompanyNo"].ToString() == "5")
        {
            if (ddProcessName.SelectedValue == "2" || ddProcessName.SelectedValue == "6")
            {
                string ChkMsg = CheckOderStockAssign();
                if (ChkMsg == "G")
                {
                    LblError.Visible = true;
                    LblError.Text = "Qty should not be greater than assigned stock";
                    txtissue.Text = "";
                    txtissue.Focus();
                    return;
                }
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

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));

                arr[0].Value = ViewState["Prmid"];
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = ddempname.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = ddOrderNo.SelectedValue;
                arr[5].Value = txtdate.Text;
                arr[6].Value = txtchalanno.Text;
                arr[6].Direction = ParameterDirection.InputOutput;
                arr[7].Value = 0;
                arr[8].Value = Session["varuserid"].ToString();
                arr[9].Value = Session["varCompanyId"].ToString();
                arr[10].Value = 0;
                arr[20].Value = 0;
                if (btnsave.Text == "Update")
                {
                    arr[10].Value = gvdetail.SelectedDataKey.Value;
                    arr[20].Value = 1;
                }
                arr[11].Value = ddCatagory.SelectedValue;
                arr[12].Value = dditemname.SelectedValue;
                arr[13].Value = Varfinishedid;
                arr[14].Value = ddgodown.SelectedValue;
                arr[15].Value = txtissue.Text;
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
                arr[36].Value = 1;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PROCESS_RAW_ISSUE", arr);

                Tran.Commit();
                txtchalanno.Text = arr[6].Value.ToString();
                ViewState["Prmid"] = arr[18].Value;
                LblError.Visible = true;
                LblError.Text = arr[23].Value.ToString();
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
        txtstock.Text = "";
        txtconqty.Text = "";
        TxtPendQty.Text = "";
        txtissue.Text = "";
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
                    From ProcessRawTran PT
                    JOIN V_FinishedItemDetail VF ON VF.Item_Finished_id = PT.Finishedid And VF.MasterCompanyId = " + Session["varCompanyId"] + @" 
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
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(ChalanNo,0) asd from ProcessRawMaster where TypeFlag = 1 And ChalanNo='" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varCompanyId"]));
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
    protected void gvdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            string sql = @"SELECT DISTINCT PRM.PRMid,PRM.Companyid,PRM.Processid,PRM.Prorderid,PRM.Empid,PRM.Date,PRM.ChalanNo,PRT.PRTid,PRT.Finishedid,PRT.IssueQuantity+prt.coneweight as Issuequantity,
                           PRT.Godownid,PRT.Lotno,PRT.UnitId,CATEGORY_ID,ITEM_ID,QualityId,ColorId,designId,ShapeId,SizeId,ShadecolorId,isnull(Prt.BinNo,'') as BinNo,Prt.Tagno,
                           isnull(PRT.TanaBana,'') as TanaBana,isnull(PRT.EstimatedRate,0) as EstimatedRate,prt.conetype,prt.noofcone,prt.coneweight,isnull(PRT.CGSTSGST,0) as CGSTSGST
                           FROM ProcessRawMaster PRM,ProcessRawTran PRT,V_FinishedItemDetail IPM
                           Where PRM.PRMid=PRT.PRMid And PRT.Finishedid=IPM.ITEM_FINISHED_ID And PRM.TypeFlag = 1 And PRT.PRTid=" + gvdetail.SelectedValue + " ANd PRM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);

            ddCatagory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            Fill_Category_SelectedChange();
            dditemname.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();

            ItemName_SelectChange();
            dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            //ItemName_SelectChange();

            UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"], true, "Select Unit");
            ddlunit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();

            //            if (ql.Visible == true)
            //            {
            //                UtilityModule.ConditionalComboFill(ref dquality, @"SELECT DISTINCT dbo.Quality.QualityId,dbo.Quality.QualityName
            //                FROM  dbo.ITEM_PARAMETER_MASTER INNER JOIN PROCESS_CONSUMPTION_DETAIL ON dbo.ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID = PROCESS_CONSUMPTION_DETAIL.IFinishedId
            //                INNER JOIN dbo.Quality ON dbo.ITEM_PARAMETER_MASTER.QUALITY_ID = dbo.Quality.QualityId
            //                Where PROCESS_CONSUMPTION_DETAIL.issueorderid=" + ddOrderNo.SelectedValue + " and quality.item_id=" + dditemname.SelectedValue + " And ITEM_PARAMETER_MASTER.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Quallity");

            //                dquality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            //                QualitySelectedIndexChange();
            //            }
            if (dsn.Visible == true)
            {
                dddesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
            }
            if (clr.Visible == true)
            {
                ddcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (shp.Visible == true)
            {
                ddshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
            }
            if (shd.Visible == true)
            {
                ddlshade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
            }
            if (procode.Visible == true)
            {
                TxtProdCode.Text = ds.Tables[0].Rows[0]["productcode"].ToString();
            }
            if (sz.Visible == true)
            {
                if (ddlunit.SelectedValue == "1")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
                }
                else if (ddlunit.SelectedValue == "2")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"], true, "select size");
                }
                ddsize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            string Str;
            switch (Session["varcompanyid"].ToString())
            {
                case "9":
                    Str = @"SELECT ROUND(SUM(CASE WHEN CalType=0 or Caltype=2 Then Case When UnitId=1 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*(PCD.IQTY+PCD.ILoss) Else (PD.Qty-Isnull(CancelQty,0))*PD.Area*(PCD.IQTY+PCD.ILOss)/10.76391 End Else 
                    Case When UnitId=1 Then (PD.Qty-Isnull(CancelQty,0))*(PCD.IQTY+PCD.ILoss) Else (PD.Qty-isnull(CancelQty,0))*(PCD.IQTY+PCD.ILoss) End End),3) Qty,[dbo].[Get_ProcessIssueQty] (PCD.IFinishedid,PM.IssueOrderId) IssQty 
                    From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PD,
                    PROCESS_CONSUMPTION_DETAIL PCD Where PM.IssueOrderId=PD.IssueOrderId And PD.Issue_Detail_Id=PCD.Issue_Detail_Id And 
                    PCD.ProcessId=" + ddProcessName.SelectedValue + " And PCD.IssueOrderID=" + ddOrderNo.SelectedValue + @" And PCD.IFinishedid=" + ds.Tables[0].Rows[0]["Finishedid"] + @"
                    Group By PM.IssueOrderId,PCD.IFinishedid";
                    break;
                case "15":
                    Str = @"SELECT ROUND(SUM(Case When UnitId=1 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*PCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*PCD.IQTY End ),3)  Qty,[dbo].[Get_ProcessIssueQty] (PCD.IFinishedid,PM.IssueOrderId) IssQty 
                    From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PD,
                    PROCESS_CONSUMPTION_DETAIL PCD Where PM.IssueOrderId=PD.IssueOrderId And PD.Issue_Detail_Id=PCD.Issue_Detail_Id And 
                    PCD.ProcessId=" + ddProcessName.SelectedValue + " And PCD.IssueOrderID=" + ddOrderNo.SelectedValue + @" And PCD.IFinishedid=" + ds.Tables[0].Rows[0]["Finishedid"] + @"
                    Group By PM.IssueOrderId,PCD.IFinishedid";
                    break;
                default:
                    Str = @"SELECT ROUND(SUM(CASE WHEN CalType=0 or Caltype=2 Then Case When UnitId=1 Then (PD.Qty-isnull(CancelQty,0))*PD.Area*PCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*PD.Area*PCD.IQTY End Else 
                    Case When UnitId=1 Then (PD.Qty-isnull(CancelQty,0))*PCD.IQTY*1.196 Else (PD.Qty-isnull(CancelQty,0))*PCD.IQTY End End),3) Qty,[dbo].[Get_ProcessIssueQty] (PCD.IFinishedid,PM.IssueOrderId) IssQty 
                    From PROCESS_ISSUE_MASTER_" + ddProcessName.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + ddProcessName.SelectedValue + @" PD,
                    PROCESS_CONSUMPTION_DETAIL PCD Where PM.IssueOrderId=PD.IssueOrderId And PD.Issue_Detail_Id=PCD.Issue_Detail_Id And 
                    PCD.ProcessId=" + ddProcessName.SelectedValue + " And PCD.IssueOrderID=" + ddOrderNo.SelectedValue + @" And PCD.IFinishedid=" + ds.Tables[0].Rows[0]["Finishedid"] + @"
                    Group By PM.IssueOrderId,PCD.IFinishedid";
                    break;
            }

            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                txtconqty.Text = (ds1.Tables[0].Rows[0]["qty"].ToString());
                TxtPendQty.Text = (Math.Round(Convert.ToDouble(ds1.Tables[0].Rows[0]["qty"]) - Convert.ToDouble(ds1.Tables[0].Rows[0]["IssQty"]), 3)).ToString();
            }

            ////UtilityModule.ConditionalComboFill(ref ddgodown, "Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM,Stock S Where GM.GodownID=S.GodownID And QtyInHand>0 And CompanyId=" + ddCompName.SelectedValue + " And item_finished_id=" + ds.Tables[0].Rows[0]["Finishedid"] + " And GM.MasterCompanyId=" + Session["varCompanyId"] + " Order By GodownName", true, "--Select--");

            string str1 = "";
            str1 = "Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GoDownID=GA.GodownID and GA.UserID=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @"
                    JOIN Stock S ON GM.GodownID=S.GodownID
                    Where QtyInHand>0 And CompanyId=" + ddCompName.SelectedValue + " And item_finished_id=" + ds.Tables[0].Rows[0]["Finishedid"] + " And GM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Order By GodownName";

            UtilityModule.ConditionalComboFill(ref ddgodown, str1, true, "--Select--");
            ddgodown.SelectedValue = ds.Tables[0].Rows[0]["Godownid"].ToString();
            ddgodown_SelectedIndexChanged(sender, new EventArgs());
            if (ddlotno.Items.FindByValue(ds.Tables[0].Rows[0]["Lotno"].ToString()) != null)
            {
                ddlotno.SelectedValue = ds.Tables[0].Rows[0]["Lotno"].ToString();
                ddlotno_SelectedIndexChanged(sender, new EventArgs());
            }
            if (TDTagno.Visible == true)
            {
                if (DDTagno.Items.FindByValue(ds.Tables[0].Rows[0]["Tagno"].ToString()) != null)
                {
                    DDTagno.SelectedValue = ds.Tables[0].Rows[0]["Tagno"].ToString();
                    DDTagno_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (TDBinNo.Visible == true)
            {
                if (DDBinNo.Items.FindByText(ds.Tables[0].Rows[0]["BinNo"].ToString()) != null)
                {
                    DDBinNo.SelectedValue = ds.Tables[0].Rows[0]["BinNo"].ToString();
                    DDBinNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            if (DDconetype.Items.FindByText(ds.Tables[0].Rows[0]["Conetype"].ToString()) != null)
            {
                DDconetype.SelectedValue = ds.Tables[0].Rows[0]["Conetype"].ToString();
            }
            txtnoofcone.Text = ds.Tables[0].Rows[0]["noofcone"].ToString();
            txtissue.Text = ds.Tables[0].Rows[0]["issuequantity"].ToString();
            TxtPendQty.Text = (Convert.ToDouble(TxtPendQty.Text) + Convert.ToDouble(txtissue.Text)).ToString();
            txtstock.Text = (Convert.ToDouble(txtstock.Text) + Convert.ToDouble(txtissue.Text)).ToString();
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        btnsave.Text = "Update";
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "" && ddOrderNo.SelectedIndex > 0)
        {

            Str = "select IPM.*,IM.CATEGORY_ID  from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,PROCESS_CONSUMPTION_DETAIL PCD  WHERE IPM.ITEM_FINISHED_ID = PCD.IFINISHEDID and PCD.ISSUEORDERID =" + ddOrderNo.SelectedValue + " and IPM.ITEM_ID=IM.ITEM_ID and ProductCode='" + TxtProdCode.Text + "' And IPM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string Qry = @"select category_id,category_name from item_category_master Where MasterCompanyId=" + Session["varCompanyId"];
                Qry = Qry + " Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varCompanyId"] + " And Category_Id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varCompanyId"] + " And item_id=" + Convert.ToInt32(ds.Tables[0].Rows[0]["ITEM_ID"].ToString());
                Qry = Qry + "  select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName ";
                Qry = Qry + "  SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by colorid";
                Qry = Qry + "  select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Shapeid  ";
                Qry = Qry + "  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varCompanyId"] + " ANd SHAPEID=" + ddshape.SelectedValue + "";
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
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + dditemname.SelectedValue + " And i.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Unit");
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
    protected void txtissue_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarcompanyNo"].ToString() == "5")
        {
            if (ddProcessName.SelectedValue == "2" || ddProcessName.SelectedValue == "6")
            {
                string ChkMsg = CheckOderStockAssign();
                if (ChkMsg == "G")
                {
                    LblError.Visible = true;
                    LblError.Text = "Qty should not be greater than assigned stock";
                    txtissue.Text = "";
                    txtissue.Focus();
                    return;
                }
            }
        }

        double stockqty = Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
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

            if (Session["varcompanyNo"].ToString() == "27")
            {
                Str = @"Select ExtraPercentage PercentageExecssQtyForProcessIss From ProcessConsumptionExtraPercentage(Nolock) 
                Where CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + @" And 
                IssueOrderID = " + ddOrderNo.SelectedValue + " And MasterCompanyID = " + Session["varCompanyId"];
            }
            VarExcessQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str));
        }
        totalQty = (totalQty * (100.0 + VarExcessQty) / 100);

        double Qty = Convert.ToDouble(txtissue.Text == "" ? "0" : txtissue.Text);
        double coneweight = UtilityModule.Getconeweight(DDconetype.SelectedItem.Text, Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text));
        Qty = Qty - coneweight;
        if (Qty + PreQty > totalQty || Qty > stockqty)
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
    private string CheckOderStockAssign()
    {
        string str = "";
        try
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            SqlParameter[] parparam = new SqlParameter[7];
            parparam[0] = new SqlParameter("@PrOrderid", ddOrderNo.SelectedValue);
            parparam[1] = new SqlParameter("@FinishedID", Varfinishedid);
            parparam[2] = new SqlParameter("@TxtQty", txtissue.Text);
            if (btnsave.Text == "Update")
            {
                parparam[3] = new SqlParameter("@PrtId", gvdetail.SelectedDataKey.Value);
            }
            else
            {
                parparam[3] = new SqlParameter("@PrtId", 0);
            }
            parparam[4] = new SqlParameter("@Message", SqlDbType.NVarChar, 2);
            parparam[5] = new SqlParameter("@LotNo", ddlotno.SelectedItem.Text);
            parparam[6] = new SqlParameter("@CompanyId", ddCompName.SelectedValue);
            parparam[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_CheckOrderStockAssign_Weaver]", parparam);
            str = parparam[4].Value.ToString();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Masters/RawMaterial/IndentRawIssue");
        }
        return str;
    }
    private void fill_qty(object sender = null)
    {
        txtconqty.Text = "0";
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
            txtstock.Text = "";

            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Processid", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@Issueorderid", ddOrderNo.SelectedValue);
            param[2] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@Item_finished_id", Varfinishedid);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FILLHomeFurnishingProductionOrderRowIssue", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtconqty.Text = (ds.Tables[0].Rows[0]["CONSMPQTY"].ToString());
                TxtPendQty.Text = (Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["PENDQTY"]), 3)).ToString();
            }

            string str = @"Select Distinct GM.GodownID,GM.GodownName 
                        From GodownMaster GM 
                        JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @"
                        JOIN Stock S ON GM.GodownID=S.GodownID  Where S.QtyInHand>0 And S.CompanyId=" + ddCompName.SelectedValue + @" And 
                                S.item_finished_id=" + Varfinishedid + " And GM.MasterCompanyId=" + Session["varCompanyId"] + @" Order By GM.GodownName
                        Select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 0, true, "--Select--");

            if (ddgodown.Items.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ddgodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        ddgodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                        if (sender != null)
                        {
                            ddgodown_SelectedIndexChanged(sender, new EventArgs());
                        }

                    }
                }
                else
                {
                    ddgodown.SelectedIndex = 1;
                    if (sender != null)
                    {
                        ddgodown_SelectedIndexChanged(sender, new EventArgs());
                    }
                }
            }
        }
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForMtr.Checked == false)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "Size in Mtr");
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

        if (UtilityModule.VALIDTEXTBOX(txtstock) == false)
        {
            goto a;
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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "";
        if (Session["varCompanyId"].ToString() == "44")
        {

            str = @" Select PM.Date, PM.ChalanNo, PM.trantype, PT.IssueQuantity, 
                                PT.Lotno, GM.GodownName, Case When IsNull(EI.EmpName, '') = '' Then 
	                                (Select Distinct EII.EmpName + ', ' 
		                                From Employee_HomeFurnishingOrderMaster  EPO(Nolock) 
		                                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.Prorderid And EPO.ProcessId = PM.Processid) 
                                Else EI.EmpName End EmpName, EI.Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, 
                                CI.CompAddr3, CI.CompTel,vf.CATEGORY_NAME, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                                hfom.CHALLANNO as Prorderid, EI.GSTNo as empgstin, CI.GSTNo,PT.TAGNO,PT.BINNO, 
                                (Select Distinct CII.CustomerCode + ', '
		                                From HomeFurnishingOrderDetail PID(Nolock) 
		                                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                                        Where PID.IssueOrderId = PM.Prorderid For XML Path('')) CustomerCode,(Select Distinct cast(om.CustomerOrderNo as varchar) + ', '
		                                From HomeFurnishingOrderDetail PID(Nolock) 
		                                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                                        Where PID.IssueOrderId = PM.Prorderid For XML Path('')) OrderNo, 
                                1 ReportType,pm.REMARK 
                                From ProcessRawMaster PM(Nolock) 
                                join ProcessRawTran PT(Nolock) on PM.PRMid=PT.PRMid 
                                join HomeFurnishingOrderMaster hfom on pm.Prorderid=hfom.ISSUEORDERID
                                join CompanyInfo ci(Nolock) on PM.Companyid=ci.CompanyId 
                                join V_FinishedItemDetail vf(Nolock) on PT.Finishedid=vf.ITEM_FINISHED_ID 
                                join GodownMaster GM(Nolock) on PT.Godownid=GM.GoDownID 
                                LEFT join EmpInfo Ei(Nolock) on PM.Empid=ei.EmpId 
                                join PROCESS_NAME_MASTER PNM(Nolock) on PM.Processid=PNM.PROCESS_NAME_ID 
                                Where PM.TypeFlag = 1 AND PM.Prmid=" + ViewState["Prmid"];




        }
        else
        {
            str = @" Select PM.Date, PM.ChalanNo, PM.trantype, PT.IssueQuantity, 
                                PT.Lotno, GM.GodownName, Case When IsNull(EI.EmpName, '') = '' Then 
	                                (Select Distinct EII.EmpName + ', ' 
		                                From Employee_HomeFurnishingOrderMaster  EPO(Nolock) 
		                                JOIN Empinfo EII(Nolock) ON EII.EmpID = EPO.EmpID And EPO.IssueOrderId = PM.Prorderid And EPO.ProcessId = PM.Processid) 
                                Else EI.EmpName End EmpName, EI.Address, CI.CompanyName, CI.CompAddr1, CI.CompAddr2, 
                                CI.CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                                PM.Prorderid, EI.GSTNo as empgstin, CI.GSTNo,PT.TAGNO,PT.BINNO, 
                                (Select Distinct CII.CustomerCode + ', '
		                                From HomeFurnishingOrderDetail PID(Nolock) 
		                                JOIN OrderMaster OM(Nolock) ON OM.OrderiD = PID.OrderiD 
                                        JOIN CustomerInfo CII(Nolock) ON CII.CustomerID = OM.CustomerID 
                                        Where PID.IssueOrderId = PM.Prorderid For XML Path('')) OrderNo, 
                                1 ReportType, PM.Prorderid IssueOrderID 
                                From ProcessRawMaster PM(Nolock) 
                                join ProcessRawTran PT(Nolock) on PM.PRMid=PT.PRMid 
                                join CompanyInfo ci(Nolock) on PM.Companyid=ci.CompanyId 
                                join V_FinishedItemDetail vf(Nolock) on PT.Finishedid=vf.ITEM_FINISHED_ID 
                                join GodownMaster GM(Nolock) on PT.Godownid=GM.GoDownID 
                                LEFT join EmpInfo Ei(Nolock) on PM.Empid=ei.EmpId 
                                join PROCESS_NAME_MASTER PNM(Nolock) on PM.Processid=PNM.PROCESS_NAME_ID 
                                Where PM.TypeFlag = 1 AND PM.Prmid=" + ViewState["Prmid"];
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varCompanyId"].ToString() == "44")
            {
                Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNewagni.rpt";
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew.rpt";
            
            }
        
            
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawIssueRecDuplicateNew.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }
    }

    private void fill_Grid_ShowConsmption()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Processid", ddProcessName.SelectedValue);
            param[1] = new SqlParameter("@ISSUEORDERID", ddOrderNo.SelectedValue);
            param[2] = new SqlParameter("@MASTERCOMPANYID", Session["varcompanyId"]);
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FILLHomeFurnishingProductionOrderRowIssueCONSUMPTION", param);

            GDGridShow.DataSource = ds;
            GDGridShow.DataBind();
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters/HomeFurnishing/FrmHomeFurnishingProductionOrderRowIssue|fill_Data_grid|" + ex.Message);
        }
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
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            arr[6] = new SqlParameter("@TypeFlag", 1);

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
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_LotNoSelectedChange();
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            DDBinNo.SelectedIndex = -1;
            FillBinNo(sender);
        }
        else
        {
            Fill_LotNoSelectedChange();
        }
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
