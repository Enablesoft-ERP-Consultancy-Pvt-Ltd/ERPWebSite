using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_TasselMaking_FrmTasselMakingDepartmentOrderRowIssue : System.Web.UI.Page
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
                From TasselMakingDepartmentOrderMaster HFMO(Nolock) 
                JOIN PROCESS_NAME_MASTER PNM (Nolock) ON PNM.PROCESS_NAME_ID = HFMO.PROCESSID 
                JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                Order By PROCESS_NAME 
                Select ConeType, ConeType From ConeMaster Order By SrNo
                Select ID, BranchName 
                From BRANCHMASTER BM(nolock) 
                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 2, false, "");

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 3, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lablechange();

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

        string str = @"SELECT DISTINCT HMOM.PROCESSID, HMOM.DepartmentID, 
                    HMOM.ISSUEORDERID, IsNull(HMOM.IssueNo, 0) CHALLANNO 
                    FROM TasselMakingDepartmentOrderMaster HMOM
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
            ddDepartmentName.SelectedValue = Ds.Tables[0].Rows[0]["DepartmentID"].ToString();
            DepartmentNameSelectedIndexChange();
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
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedIndexChange();
    }
    private void ProcessNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        Fill_Grid();
        string str = @"Select Distinct a.DepartmentID,  D.DepartmentName 
        From TasselMakingDepartmentOrderMaster a(Nolock) 
        JOIN Department D(Nolock) ON D.DepartmentID = a.DepartmentID 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + @" And 
        a.MASTERCOMPANYID = " + Session["varCompanyId"] + " Order By D.DepartmentName ";

        UtilityModule.ConditionalComboFill(ref ddDepartmentName, str, true, "--Select--");
    }
    protected void ddDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalanno.Text = "";
        DepartmentNameSelectedIndexChange();
    }
    private void DepartmentNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        Fill_Grid();

        string str = @"Select Distinct a.ISSUEORDERID, a.IssueNo CHALLANNO 
        From TasselMakingDepartmentOrderMaster a(Nolock) 
        Where a.COMPANYID = " + ddCompName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.PROCESSID = " + ddProcessName.SelectedValue + @" And 
        a.MASTERCOMPANYID = " + Session["varcompanyId"] + " And a.DepartmentID = " + ddDepartmentName.SelectedValue + @" And a.STATUS = 'PENDING' 
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
            From TasselMakingDepartmentOrder_Consumption_Detail HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Select Category Name");

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
            From TasselMakingDepartmentOrder_Consumption_Detail HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @"
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
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
                        From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorID, VF.ColorName 
                        From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeID, VF.ShapeName 
                        From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID 
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Shape--");
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
                        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeID, VF.SizeFt 
                        From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.ShapeID = " + ddshape.SelectedValue + @"
                        Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "Size in Ft");
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
                UtilityModule.ConditionalComboFill(ref DDChallanNo, @"Select PrmId, ChallanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
                From TasselMakingDepartmentOrderRawIssueMaster(Nolock) 
                Where TranType = 0 And TypeFlag = 1 And CompanyID = " + ddCompName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + @" And 
                DepartmentID = " + ddDepartmentName.SelectedValue + " And IssueOrderID = " + ddOrderNo.SelectedValue + @" And BranchID = " + DDBranchName.SelectedValue + @"
                And MasterCompanyId = " + Session["varCompanyId"], true, "Select Challan No");
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

            string strsql2 = @"Select PRMID, ChallanNo, Remark 
            From TasselMakingDepartmentOrderRawIssueMaster PRM(Nolock) 
            Where PRM.Prmid = " + DDChallanNo.SelectedValue + " And PRM.TranType = 0 And PRM.TypeFlag = 1 And PRM.ProcessID = " + ddProcessName.SelectedValue + " And PRM.MasterCompanyId = " + Session["varCompanyId"];
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChallanNo"].ToString();
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
            From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + " And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
            Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];

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
                    From TasselMakingDepartmentOrder_Consumption_Detail  HCD(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = HCD.IFINISHEDID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
                        VF.ITEM_ID = " + dditemname.SelectedValue + " And VF.QualityID = " + dquality.SelectedValue + @"
                    Where HCD.ISSUEORDERID = " + ddOrderNo.SelectedValue + " And HCD.PROCESSID = " + ddProcessName.SelectedValue + @" And 
                        VF.MasterCompanyId=" + Session["varCompanyId"] + " Order By VF.ShadeColorName", true, "Select Shadecolor");
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

    private void SaveButton()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[25];

            arr[0] = new SqlParameter("@PRMID", SqlDbType.Int);
            arr[1] = new SqlParameter("@COMPANYID", SqlDbType.Int);
            arr[2] = new SqlParameter("@BRANCHID", SqlDbType.Int);
            arr[3] = new SqlParameter("@PROCESSID", SqlDbType.Int);
            arr[4] = new SqlParameter("@ISSUEORDERID", SqlDbType.Int);
            arr[5] = new SqlParameter("@DEPARTMENTID", SqlDbType.Int);
            arr[6] = new SqlParameter("@DATE", SqlDbType.SmallDateTime);
            arr[7] = new SqlParameter("@CHALLANNO", SqlDbType.NVarChar, 150);
            arr[8] = new SqlParameter("@TRANTYPE", SqlDbType.Int);
            arr[9] = new SqlParameter("@USERID", SqlDbType.Int);
            arr[10] = new SqlParameter("@MASTERCOMPANYID", SqlDbType.Int);
            arr[11] = new SqlParameter("@TYPEFLAG", SqlDbType.Int);
            arr[12] = new SqlParameter("@REMARK", txtremark.Text);

            arr[13] = new SqlParameter("@PRTID", SqlDbType.Int);
            arr[14] = new SqlParameter("@ITEM_FINISHED_ID", SqlDbType.Int);
            arr[15] = new SqlParameter("@SIZEFLAG", SqlDbType.Int);
            arr[16] = new SqlParameter("@UNITID", SqlDbType.Int);
            arr[17] = new SqlParameter("@QTY", SqlDbType.Float);
            arr[18] = new SqlParameter("@GODOWNID", SqlDbType.Int);
            arr[19] = new SqlParameter("@LOTNO", SqlDbType.NVarChar, 200);
            arr[20] = new SqlParameter("@TAGNO", SqlDbType.VarChar, 200);
            arr[21] = new SqlParameter("@BINNO", SqlDbType.VarChar, 50);
            arr[22] = new SqlParameter("@Conetype", SqlDbType.VarChar, 200);
            arr[23] = new SqlParameter("@Noofcone", SqlDbType.Int);
            arr[24] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));

            arr[0].Value = ViewState["Prmid"];
            arr[0].Direction = ParameterDirection.InputOutput;
            arr[1].Value = ddCompName.SelectedValue;
            arr[2].Value = DDBranchName.SelectedValue;
            arr[3].Value = ddProcessName.SelectedValue;
            arr[4].Value = ddOrderNo.SelectedValue;
            arr[5].Value = ddDepartmentName.SelectedValue;
            arr[6].Value = txtdate.Text;
            arr[7].Value = txtchalanno.Text;
            arr[7].Direction = ParameterDirection.InputOutput;
            arr[8].Value = 0;
            arr[9].Value = Session["varuserid"].ToString();
            arr[10].Value = Session["varCompanyId"].ToString();
            arr[11].Value = 1;
            arr[12].Value = txtremark.Text;
            arr[13].Value = 0;
            if (btnsave.Text == "Update")
            {
                arr[13].Value = gvdetail.SelectedDataKey.Value;
            }
            arr[14].Value = Varfinishedid;
            arr[15].Value = ChkForMtr.Checked == true ? 1 : 2;
            arr[16].Value = ddlunit.SelectedValue;
            arr[17].Value = txtissue.Text;
            arr[18].Value = ddgodown.SelectedValue;
            arr[19].Value = ddlotno.SelectedItem.Text;
            arr[20].Value = TDTagno.Visible == false ? "Without Tag No" : DDTagno.SelectedItem.Text;
            string BinNo = TDBinNo.Visible == false ? "" : (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "");
            arr[21].Value = BinNo;
            arr[22].Value = DDconetype.SelectedItem.Text;
            arr[23].Value = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
            arr[24].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_TASSEL_MAKING_DEPARTMENT_RAW_ISSUE", arr);
            if (arr[24].Value.ToString() == "Data saved successfully")
            {
                Tran.Commit();
                txtchalanno.Text = arr[7].Value.ToString();
                ViewState["Prmid"] = arr[0].Value;
                Fill_Grid();
                SaveReferece();
                btnsave.Text = "Save";
            }
            else
            {
                Tran.Rollback();
            }
            LblError.Visible = true;
            LblError.Text = arr[24].Value.ToString();
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SaveButton();
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
                    PT.Qty,LotNo,GodownName,Pt.BinNo,PT.TagNo 
                    From TasselMakingDepartmentOrderRawIssueMaster PM(Nolock)
                    JOIN TasselMakingDepartmentOrderRawIssueTran PT(Nolock) ON PT.PRmID = PM.PrmID 
                    JOIN V_FinishedItemDetail VF ON VF.Item_Finished_id = PT.Item_Finished_id 
                    JOIN GodownMaster GM ON GM.GodownId = PT.GodownId 
                    Where VF.MasterCompanyId = " + Session["varCompanyId"] + " And PT.PrmID = " + ViewState["Prmid"];
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            }
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters_TasselMaking_FrmTasselMakingDepartmentOrderRowIssue|fill_Data_grid|" + ex.Message);
        }
        return ds;
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
        double stockqty = Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
        double totalQty = Convert.ToDouble(txtconqty.Text == "" ? "0" : txtconqty.Text);
        double PreQty = Math.Round(totalQty - Convert.ToDouble(TxtPendQty.Text == "" ? "0" : TxtPendQty.Text), 3);
        double VarExcessQty = 0;
        if (Session["varcompanyNo"].ToString() == "16" && ddProcessName.SelectedValue != "1")
        {
            VarExcessQty = 0;
        }
        //else
        //{
        //    string Str = "Select PercentageExecssQtyForProcessIss From MasterSetting(Nolock)";

        //    VarExcessQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str));
        //}
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
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FillTasselMakingDepartmentOrderRowIssueConsumption", param);

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

    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = @" select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, 
                PT.Lotno, GM.GodownName, D.DepartmentName EmpName, '' Address, CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                PM.IssueOrderID Prorderid, '' as empgstin, CI.GSTNo,PT.TAGNO,PT.BINNO, PM.ChallanNo OrderNo, BM.GstNo BranchGstNo, 
                0 ReportType, PM.IssueOrderID 
                From TasselMakingDepartmentOrderRawIssueMaster PM (Nolock)
                JOIN BranchMaster BM ON BM.ID = PM.BranchID 
                join TasselMakingDepartmentOrderRawIssueTran PT on PM.PRMid=PT.PRMid 
                join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                join V_FinishedItemDetail vf on PT.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID 
                join GodownMaster GM on PT.Godownid=GM.GoDownID 
                join Department D on D.DepartmentId = PM.DEPARTMENTID 
                join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                Where PM.TypeFlag = 1 And PM.Prmid = " + ViewState["Prmid"] + " and PM.Processid=" + ddProcessName.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptRawIssueRecDuplicateNew3.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRawIssueRecDuplicateNew.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }
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
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FillTasselMakingDepartmentOrderRowIssueConsumption", param);

            GDGridShow.DataSource = ds;
            GDGridShow.DataBind();
        }
        catch (Exception ex)
        {
            LblError.Visible = true;
            LblError.Text = ex.Message;
            Logs.WriteErrorLog("Masters/TasselMaking/FrmTasselMakingDepartmentOrderRowIssue|fill_Data_grid|" + ex.Message);
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
            if (gvdetail.Rows.Count == 1)
            {
                arr[1].Value = 1;
            }

            arr[2].Value = 0;
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_TASSEL_MAKING_DEPARTMENT_RAW_ISSUE", arr);
            if (arr[3].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + arr[3].Value.ToString() + "');", true);
                Tran.Rollback();
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
