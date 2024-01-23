using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_process_FrmDepartmentRowReceive : System.Web.UI.Page
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
            Qry = @" select Distinct CI.CompanyId,Companyname from Companyinfo CI
                    JOIN Company_Authentication CA ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
                    Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname
                    Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                    Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                    From DEPARTMENTRAWISSUEMASTER a(Nolock)
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = a.ProcessID And URP.Userid = " + Session["varuserId"] + @"  
                    Where a.TYPEFLAG = 1 And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By a.ProcessID 
                    Select Distinct a.DepartmentId, D.DepartmentName 
                    From DEPARTMENTRAWISSUEMASTER a(Nolock)
                    JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentId 
                    Where a.TYPEFLAG = 1 And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
                    Order By D.DepartmentName 
                    Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DSQ = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, DSQ, 0, true, "--Select--");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, DSQ, 1, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, DSQ, 2, true, "--Select--");
            if (ddProcessName.Items.Count > 0)
            {
                ddProcessName.SelectedIndex = 1;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDepartmentName, DSQ, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, DSQ, 4, false, "");
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
        string str = @"Select Distinct a.DepartmentId, D.DepartmentName 
            From DEPARTMENTRAWISSUEMASTER a(Nolock)
            JOIN Department D(Nolock) ON D.DepartmentId = a.DepartmentId 
            Where a.TYPEFLAG = 1 And a.CompanyID = " + ddCompName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + " And ProcessID = " + ddProcessName.SelectedValue + " And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"  
            Order By D.DepartmentName ";
        UtilityModule.ConditionalComboFill(ref DDDepartmentName, str, true, "--Select--");
    }
    protected void DDDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchalanno.Text = "";
        DepartmentNameSelectedIndexChange();
    }
    private void DepartmentNameSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;
        string str = @"Select Distinct a.IssueOrderID, b.IssueNo 
        From DEPARTMENTRAWISSUEMASTER a(Nolock)
        JOIN ProcessIssueToDepartmentMaster b(Nolock) ON b.IssueOrderID = a.ISSUEORDERID 
        Where a.TYPEFLAG = 1 And a.CompanyID = " + ddCompName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And a.ProcessID = " + ddProcessName.SelectedValue + @" And 
        a.DepartmentID = " + DDDepartmentName.SelectedValue + " And a.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" 
        Order By a.IssueOrderID Desc";

        UtilityModule.ConditionalComboFill(ref ddOrderNo, str, true, "Select order no");
    }
    protected void ddOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChange();
    }
    private void OrderNoSelectedIndexChange()
    {
        ViewState["Prmid"] = 0;

        string Qry = @"Select Distinct PrmId, ChallanNo + ' / ' + REPLACE(CONVERT(NVARCHAR(11), Date, 106), ' ', '-') Challan 
            From DEPARTMENTRAWISSUEMASTER(Nolock) 
            Where TypeFlag = 1 And TranType=0 And IssueOrderID=" + ddOrderNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
            And CompanyID = " + ddCompName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + @" 
            Order By PrmId Desc ";

        DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, DSQ, 0, true, "--Select--");
    }

    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        IssueNoSelectedIndexChange();
    }
    private void IssueNoSelectedIndexChange()
    {
        string strsql2 = @"select PRMID, ChallanNo 
        From DEPARTMENTRAWISSUEMASTER PRM(Nolock) 
        Where PRM.TypeFlag = 1 And PRM.TranType = 1 And PRM.CompanyID = " + ddCompName.SelectedValue + " And PRM.BranchID = " + DDBranchName.SelectedValue + @" And 
        PRM.ProcessID = " + ddProcessName.SelectedValue + " And PRM.DepartmentID = " + DDDepartmentName.SelectedValue + " And PRM.IssueOrderID = " + ddOrderNo.SelectedValue + @" 
        And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
        Select Distinct VF.CATEGORY_ID, VF.CATEGORY_NAME  
        From DEPARTMENTRAWISSUEMASTER a(Nolock)
        JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID 
        Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue;

        DataSet DSQ = SqlHelper.ExecuteDataset(strsql2);

        if (ChKForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDChallanNo, DSQ, 0, true, "--Select--");
        }
        UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, DSQ, 1, true, "--Select--");

        if (ddCatagory.Items.Count > 0)
        {
            ddCatagory.SelectedIndex = 1;
        }
        Fill_Category_SelectedChange();
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Category_SelectedChange();
    }

    private void Fill_Category_SelectedChange()
    {
        if (ddCatagory.SelectedIndex >= 0)
        {
            ddlcategorycange();

            UtilityModule.ConditionalComboFill(ref dditemname, @"Select Distinct VF.ITEM_ID, VF.ITEM_NAME
            From DEPARTMENTRAWISSUEMASTER a(Nolock)
            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
            Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue + @" 
            Order By VF.ITEM_NAME", true, "--Select Item--");

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
        dsn.Visible = false;
        clr.Visible = false;        
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM 
                      inner join PARAMETER_MASTER PM on PM.[PARAMETER_ID] = IPM.[PARAMETER_ID] And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" 
                      where [CATEGORY_ID] = " + ddCatagory.SelectedValue;

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
                            From DEPARTMENTRAWISSUEMASTER a(Nolock)
                            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                            Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue, true, "--Select Design--");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, @"Select Distinct VF.ColorId, VF.ColorName 
                            From DEPARTMENTRAWISSUEMASTER a(Nolock)
                            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                            Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue, true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, @"Select Distinct VF.ShapeId, VF.ShapeName 
                            From DEPARTMENTRAWISSUEMASTER a(Nolock)
                            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" 
                            Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue, true, "--Select Shape--");
                        if (ddshape.Items.Count > 0)
                        {
                            ddshape.SelectedIndex = 1;
                        }
                        break;
                    case "5":
                        sz.Visible = true;
                        ChkForMtr.Checked = false;
                        FillSize();
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
            string Qry = @" SELECT U.UnitId, U.UnitName  
            FROM ITEM_MASTER IM(Nolock) 
            JOIN Unit U(Nolock) ON IM.UnitTypeID = U.UnitTypeID 
            Where IM.ITEM_ID = " + dditemname.SelectedValue + " And IM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" 

            Select Distinct VF.QualityID, VF.QualityName 
            From DEPARTMENTRAWISSUEMASTER a(Nolock)
            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + "  And VF.ITEM_ID = " + dditemname.SelectedValue + @" 
            Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue + @" 
            Order By VF.QualityName ";

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
            UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct VF.ShadecolorId, VF.ShadeColorName
                From DEPARTMENTRAWISSUEMASTER a(Nolock)
                JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + @" And 
                    VF.ITEM_ID = " + dditemname.SelectedValue + "  And VF.QualityId = " + dquality.SelectedValue + @" 
                Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue + @" 
                Order By VF.ShadeColorName", true, "Select Shadecolor");
        }
        fill_LotGodown();
    }

    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();
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
                    from DEPARTMENTRAWISSUEMASTER(Nolock) 
                    Where TypeFlag = 1 And TranType=1 And CompanyID = " + ddCompName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + @" And 
                    IssueOrderID=" + ddOrderNo.SelectedValue + " And IssuePRMID = " + DDIssueNo.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Challan No");
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

            string strsql2 = "select PRMID,ChallanNo from DEPARTMENTRAWISSUEMASTER PRM(Nolock) where PRM.TypeFlag = 1 And PRM.Prmid=" + DDChallanNo.SelectedValue + " And PRM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql2);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtchalanno.Text = ds2.Tables[0].Rows[0]["ChallanNo"].ToString();
            }
        }
        Fill_Grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_LotGodown();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_LotGodown();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_LotGodown();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_LotGodown();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_LotGodown(sender);
    }
    protected void ddgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_GodownSelectedChange(sender);
    }
    protected void FillBinNo(object sender = null)
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
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
        }
    }
    protected void FillTagNo(object sender = null)
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            
           string str = @"Select Distinct b.TagNo, b.TagNo 
                From DEPARTMENTRAWISSUEMASTER a(Nolock)
                JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue + " And b.ITEM_FINISHED_ID = " + Varfinishedid + @" 
                    And b.LotNo = '" + ddlotno.SelectedItem.Text + "'";

            UtilityModule.ConditionalComboFill(ref DDTagno, str, true, "--Select--");
        }
    }
    private void Fill_GodownSelectedChange(object sender = null)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                TextBox Txtprodcode = new TextBox();
                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, Txtprodcode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(ddgodown.SelectedValue), Varfinishedid, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster where GODOWNID=" + ddgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
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
    }

    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
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
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

            string str = @"Select IsNull(Sum(b.Qty), 0) IssQty 
                From DEPARTMENTRAWISSUEMASTER a(Nolock)
                JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                Where a.TranType = 0 And a.TypeFlag = 1 And a.PRMID = " + DDIssueNo.SelectedValue + " And b.ITEM_FINISHED_ID = " + Varfinishedid + @" 
                    And b.LotNo = '" + ddlotno.SelectedItem.Text + "' And b.TAGNO = '" + DDTagno.SelectedItem.Text + @"'
                Select IsNull(Sum(b.Qty), 0) RecQty 
                From DEPARTMENTRAWISSUEMASTER a(Nolock)
                JOIN DEPARTMENTRAWISSUETRAN b(Nolock) ON b.PRMID = a.PRMID 
                Where a.TranType = 1 And a.TypeFlag = 1 And a.IssuePRMID = " + DDIssueNo.SelectedValue + " And b.ITEM_FINISHED_ID = " + Varfinishedid + @" 
                    And b.LotNo = '" + ddlotno.SelectedItem.Text + "' And b.TAGNO = '" + DDTagno.SelectedItem.Text + @"'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            TxtIssueQty.Text = ds.Tables[0].Rows[0]["IssQty"].ToString();
            TxtPendQty.Text = (Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["IssQty"]) - Convert.ToDouble(ds.Tables[1].Rows[0]["RecQty"]), 3)).ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        LblError.Text = "";
        if (LblError.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[37];

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
                arr[15] = new SqlParameter("SIZEFLAG", SqlDbType.Int);
                arr[16] = new SqlParameter("@UNITID", SqlDbType.Int);
                arr[17] = new SqlParameter("@QTY", SqlDbType.Float);
                arr[18] = new SqlParameter("@GODOWNID", SqlDbType.Int);
                arr[19] = new SqlParameter("@LOTNO", SqlDbType.NVarChar, 200);
                arr[20] = new SqlParameter("@TAGNO", SqlDbType.VarChar, 200);
                arr[21] = new SqlParameter("@BINNO", SqlDbType.VarChar, 50);
                arr[22] = new SqlParameter("@Conetype", SqlDbType.VarChar, 200);
                arr[23] = new SqlParameter("@Noofcone", SqlDbType.Int);
                arr[24] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                arr[25] = new SqlParameter("@IssuePRMID", SqlDbType.Int);

                int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

                arr[0].Value = ViewState["Prmid"];
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[1].Value = ddCompName.SelectedValue;
                arr[2].Value = DDBranchName.SelectedValue;
                arr[3].Value = ddProcessName.SelectedValue;
                arr[4].Value = ddOrderNo.SelectedValue;
                arr[5].Value = DDDepartmentName.SelectedValue;
                arr[6].Value = txtdate.Text;
                arr[7].Value = txtchalanno.Text;
                arr[7].Direction = ParameterDirection.InputOutput;
                arr[8].Value = 1;
                arr[9].Value = Session["varuserid"].ToString();
                arr[10].Value = Session["varMasterCompanyIDForERP"].ToString();
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
                arr[17].Value = TxtRecQty.Text;
                arr[18].Value = ddgodown.SelectedValue;
                arr[19].Value = ddlotno.SelectedItem.Text;
                arr[20].Value = TDTagno.Visible == false ? "Without Tag No" : DDTagno.SelectedItem.Text;
                string BinNo = TDBinNo.Visible == false ? "" : (DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "");
                arr[21].Value = BinNo;
                arr[22].Value = DDconetype.SelectedItem.Text;
                arr[23].Value = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                arr[24].Direction = ParameterDirection.Output;
                arr[25].Value = DDIssueNo.SelectedValue;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVE_DEPARTMENT_RAW_ISSUE", arr);
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
        TxtIssueQty.Text = "";
        TxtRecQty.Text = "";
        TxtPendQty.Text = "";
        txtnoofcone.Text = "";
    }
    private void Fill_Grid()
    {
        gvdetail.DataSource = fill_Data_grid();
        gvdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        string strsql = @"Select b.PrtId, VF.CATEGORY_NAME, VF.ITEM_NAME, VF.QualityName + Space(2) + VF.DesignName + Space(2) + VF.ColorName + Space(2) + VF.ShapeName + Space(2) + VF.SizeFt + Space(2) + VF.ShadeColorName [DESCRIPTION],
            b.Qty, GM.GodownName, b.LotNo, b.TagNo, b.BinNo 
            From DEPARTMENTRAWISSUEMASTER a(Nolock)
            JOIN DEPARTMENTRAWISSUETRAN b(Nolock) ON b.PRMID = a.PRMID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.Item_Finished_id = b.Item_Finished_id 
            JOIN GodownMaster GM(Nolock) ON GM.GodownId = b.GodownId 
            Where a.TYPEFLAG = 1 And a.PrmID = " + ViewState["Prmid"] + " And a.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        return ds;
    }
    protected void txtchalan_ontextchange(object sender, EventArgs e)
    {
        string ChalanNo = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(CHALLANNO, 0) asd 
                From DEPARTMENTRAWISSUEMASTER(nolock) Where TypeFlag = 1 And CHALLANNO = '" + txtchalanno.Text + "' And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]));
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
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Session.Remove("prmid");
        Session.Remove("finishedid");
        Session.Remove("inhand");
        Session.Remove("stocktranid");
        Session.Remove("stockid");
    }

    protected void TxtRecQty_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtissue_TextChanged(object sender, EventArgs e)
    {
        double stockqty = 0;// Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
        double totalQty = 0;//Convert.ToDouble(txtconqty.Text == "" ? "0" : txtconqty.Text);
        double PreQty = Math.Round(totalQty - Convert.ToDouble(TxtPendQty.Text == "" ? "0" : TxtPendQty.Text), 3);
        double VarExcessQty = 0;
       
        totalQty = (totalQty * (100.0 + VarExcessQty) / 100);

        double Qty = 0;// Convert.ToDouble(txtissue.Text == "" ? "0" : txtissue.Text);
        double coneweight = UtilityModule.Getconeweight(DDconetype.SelectedItem.Text, Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text));
        Qty = Qty - coneweight;
        if (Qty + PreQty > totalQty || Qty > stockqty)
        {
            //txtissue.Text = "";
            LblError.Text = "Pls Enter Correct Qty ";
            LblError.Visible = true;
            //txtissue.Focus();
            return;
        }
        else
        {
            LblError.Text = "";
            LblError.Visible = false;
        }
    }
    private void fill_LotGodown(object sender = null)
    {
        TxtPendQty.Text = "0";

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

            int Varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            DataSet ds = null;

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct b.LotNo, b.LotNo 
                From DEPARTMENTRAWISSUEMASTER a(Nolock)
                JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue + " And b.ITEM_FINISHED_ID = " + Varfinishedid + @"
                Select Distinct GM.GodownID,GM.GodownName 
                From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                Order By GM.GodownName
                Select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'");
            
            UtilityModule.ConditionalComboFillWithDS(ref ddlotno, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 1, true, "--Select--");

//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                txtconqty.Text = (ds.Tables[0].Rows[0]["qty"].ToString());
//                TxtPendQty.Text = (Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["qty"]) - Convert.ToDouble(ds.Tables[0].Rows[0]["IssQty"]), 3)).ToString();
//            }

//            string str = @"Select Distinct GM.GodownID,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
//                            JOIN Stock S ON GM.GodownID=S.GodownID  Where S.QtyInHand>0 And S.CompanyId=" + ddCompName.SelectedValue + " And S.item_finished_id=" + Varfinishedid + " And GM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By GM.GodownName
//                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

//            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            //UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 0, true, "--Select--");

            //if (ddgodown.Items.Count > 0)
            //{
            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        if (ddgodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
            //        {
            //            ddgodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
            //            if (sender != null)
            //            {
            //                ddgodown_SelectedIndexChanged(sender, new EventArgs());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ddgodown.SelectedIndex = 1;
            //        if (sender != null)
            //        {
            //            ddgodown_SelectedIndexChanged(sender, new EventArgs());
            //        }
            //    }
            //}
        }
    }
    protected void ChkForMtr_CheckedChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        UtilityModule.ConditionalComboFill(ref ddsize, @"Select Distinct VF.SizeID, Case When '" + ChkForMtr.Checked + @"' == 'False' Then VF.SizeFt Else VF.SizeMtr End 
                    From DEPARTMENTRAWISSUEMASTER a(Nolock)
                    JOIN DEPARTMENTRAWISSUETRAN b(Nolock) on b.PRMID = a.PRMID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.ITEM_FINISHED_ID And VF.CATEGORY_ID = " + ddCatagory.SelectedValue + "  And VF.ShapeID = " + ddshape.SelectedValue + @" 
                    Where a.TYPEFLAG = 1 And a.PRMID = " + DDIssueNo.SelectedValue, true, "Select Size");
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = @" select PM.Date, PM.ChallanNo ChalanNo, PM.trantype, PT.Qty IssueQuantity, 
                           PT.Lotno, GM.GodownName, D.DepartmentName EmpName, '' Address, CI.CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, 
                           '' CompAddr3, CI.CompTel, vf.ITEM_NAME, vf.QualityName, vf.designName, 
                           vf.ColorName, vf.ShadeColorName, vf.ShapeName, vf.SizeMtr, PNM.PROCESS_NAME, 
                           PM.IssueOrderID Prorderid, '' as empgstin, CI.GSTNo,PT.TAGNO,PT.BINNO, BM.GstNo BranchGstNo, 
                           0 ReportType  
                           From DEPARTMENTRAWISSUEMASTER PM 
                           JOIN BranchMaster BM ON BM.ID = PM.BranchID 
                           inner join DEPARTMENTRAWISSUETRAN PT on PM.PRMid=PT.PRMid 
                           inner join CompanyInfo ci on PM.Companyid=ci.CompanyId 
                           inner join V_FinishedItemDetail vf on PT.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID 
                           inner join GodownMaster GM on PT.Godownid=GM.GoDownID 
                           inner join Department D on D.DepartmentId = PM.DEPARTMENTID 
                           inner join PROCESS_NAME_MASTER PNM on PM.Processid=PNM.PROCESS_NAME_ID 
                           Where PM.TypeFlag = 1 And PM.Prmid=" + ViewState["Prmid"] + " and PM.Processid=" + ddProcessName.SelectedValue;

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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
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
            ViewState["Prmid"] = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select PrmId from DEPARTMENTRAWISSUETRAN Where PrtId=" + VarPrtID);
            SqlParameter[] arr = new SqlParameter[7];

            arr[0] = new SqlParameter("@PrtID", SqlDbType.Int);
            arr[1] = new SqlParameter("@RowCount", SqlDbType.Int);
            arr[2] = new SqlParameter("@TranType", SqlDbType.Int);
            arr[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            arr[4] = new SqlParameter("@userid", Session["varuserid"]);
            arr[5] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[6] = new SqlParameter("@TypeFlag", 1);

            arr[0].Value = VarPrtID;
            arr[1].Value = 2;
            
            if (gvdetail.Rows.Count == 1)
            {
                arr[1].Value = 1;
            }
            arr[2].Value = 1;
            arr[3].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_DEPARTMENT_RAW_ISSUE", arr);
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
    protected void TxtPOrderNo_TextChanged(object sender, EventArgs e)
    {
        LblError.Text = "";
        String VarPOrderNo = TxtPOrderNo.Text == "" ? "0" : TxtPOrderNo.Text;

        string str = @"Select a.CompanyID, a.BranchID, a.ProcessID, a.DepartmentID, a.IssueOrderID, a.PRMID 
                From DEPARTMENTRAWISSUEMASTER a(Nolock) 
			    JOIN ProcessIssueToDepartmentMaster b(Nolock) ON b.IssueOrderID = a.ISSUEORDERID 
                Where a.TYPEFLAG = 1 And a.COMPANYID = " + ddCompName.SelectedValue + " And a.BranchID = " + DDBranchName.SelectedValue + " And b.IssueNo = '" + VarPOrderNo + "'"; 

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
            ProcessNameSelectedIndexChange();
            DDDepartmentName.SelectedValue = Ds.Tables[0].Rows[0]["DepartmentID"].ToString();
            DepartmentNameSelectedIndexChange();
            if (ddOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["IssueOrderID"].ToString()) != null)
            {
                ddOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
            }
            else
            {
                LblError.Text = "This Po No. does not exists";
                LblError.Visible = true;
                return;

            }
            OrderNoSelectedIndexChange();
            //if (DDIssueNo.Items.Count > 0)
            //{
            //    DDIssueNo.SelectedIndex = 1;
            //    IssueNoSelectedIndexChange();
            //}
        }
        else
        {
            if (ddOrderNo.Items.Count > 0)
            {
                ddOrderNo.SelectedIndex = 0;
            }
            if (DDIssueNo.Items.Count > 0)
            {
                DDIssueNo.SelectedIndex = 0;
            }
            if (ddCatagory.Items.Count > 0)
            {
                ddCatagory.SelectedIndex = 0;
            }
            TxtPOrderNo.Text = "";
            TxtPOrderNo.Focus();
        }
    }
}