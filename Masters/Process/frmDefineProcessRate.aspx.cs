using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Process_frmDefineProcessRate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyNo"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = null;
            DataSet ds = null;
            str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                    From Companyinfo CI(nolock)
                    JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                    Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 

                    Select UnitsId,UnitName from Units with(nolock) Where Mastercompanyid = " + Session["varcompanyid"] + @" 
                    Select ITEM_ID,ITEM_NAME from ITEM_MASTER IM with(nolock) Inner Join CategorySeparate CS with(nolock) on 
                        cs.Categoryid=IM.CATEGORY_ID and Cs.id=0 And IM.Mastercompanyid = " + Session["varcompanyid"] + @" 
                    Select ShapeId,ShapeName from Shape with(nolock) 
                    Select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER With(nolock) Where MasterCompanyid = " + Session["varcompanyid"] + @" Order By PROCESS_NAME 
                    Select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME 
                    Select val,Type From Sizetype
                    Select OrderCategoryId,OrderCategory from OrderCategory order by OrderCategory"; 

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                //DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDUnitName, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDArticleName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDShape, ds, 3, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddJob, ds, 4, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 5, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizeType, ds, 6, false, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDOrderType, ds, 7, false, "");

            FillBranchName();
            if (DDsizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            if (DDShape.Items.Count > 0)
            {
                DDShape.SelectedIndex = 0;
            }
            ds.Dispose();
            switch (Session["varcompanyId"].ToString())
            {
                case "8":
                    Divuniname.Visible = true;
                    break;
                default:
                    divQuality.Visible = true;
                    divDesign.Visible = true;
                    Divuniname.Visible = false;
                    divCategory.Visible = true;
                    if (DDCategory.Items.Count > 0)
                    {
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                    }
                    break;
            }
            if (Session["varcompanyId"].ToString() == "16")
            {
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            if (Session["varcompanyId"].ToString() == "28")
            {
                Divuniname.Visible = true;
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            if (Session["varcompanyId"].ToString() == "27")
            {
                if (DDRateLocation.SelectedValue == "1")
                {
                    DivWeavingEmployee.Visible = true;
                    BindWeavingEmp();
                }
                else
                {
                    DivWeavingEmployee.Visible = false;
                    DDWeavingEmp.SelectedIndex = 0;
                }
            }
            if (Session["varcompanyId"].ToString() == "42")
            {
                DivBonus.Visible = true;
                DivFinisherRate.Visible = true;
                DivDateRange.Visible = true;
                DivOrderType.Visible = true;
                if (DDRateLocation.SelectedValue == "1")
                {
                    DivWeavingEmployee.Visible = true;
                    BindWeavingEmp();
                }
                else
                {
                    DivWeavingEmployee.Visible = false;
                    DDWeavingEmp.SelectedIndex = 0;
                }
            }
            if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            {
                DivRateLocation.Visible = true;
            }
        }
    }

    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBranchName();
    }
    private void FillBranchName()
    {
        string str2 = @" Select ID, BranchName From BRANCHMASTER(Nolock) Where CompanyID = " + Session["CurrentWorkingCompanyID"] + " Order By ID ";
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);

        UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds2, 0, false, "");
    }

    protected void DDBranchName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddJob.SelectedIndex = 0;
        DGRateDetail.DataSource = null;
        DGRateDetail.DataBind();
    }
    protected void BindWeavingEmp()
    {
        string str2 = null;

        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "28")
        {
            str2 = @"Select EI.EmpID, Case When EI.Empcode <> '' Then EI.EmpCode Else EI.EmpName End Empname 
                From Empinfo EI(Nolock)
                JOIN EmpProcess EP(Nolock) ON EP.EmpId = EI.EmpId And EP.ProcessId = " + ddJob.SelectedValue + @" 
                Where EI.Blacklist = 0 
                Order By EI.EmpName";
        }
        else
        {
            str2 = @"select EI.EmpId, case When EI.Empcode<>'' then EI.EmpCode Else EI.EmpName End Empname,EmployeeType 
                from EmpInfo EI(Nolock) 
                inner join Department D(Nolock) on EI.Departmentid=D.DepartmentId and D.DepartmentName in ('PRODUCTION') 
	            JOIN EmpProcess EP(Nolock) ON EI.EmpId=EP.EmpId and EP.ProcessId=" + ddJob.SelectedValue + @"
                Where EI.Status='P' and EI.Blacklist=0 and EI.EmployeeType=" + DDRateLocation.SelectedValue + @"
	            Order by Empname";
        }
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        UtilityModule.ConditionalComboFillWithDS(ref DDWeavingEmp, ds2, 0, true, "--Plz Select--");
    }
    protected void DDArticleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (divQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct vf.Qualityid,vf.Qualityname from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And QualityId<>0 order by vf.Qualityname", true, "--Plz Select--");
        }
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct vf.ColorId,vf.ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " And ColorId<>0 order by ColorName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        string size = "SizeMtr";
        switch (DDsizeType.SelectedValue)
        {
            case "0":
                size = "sizeft";
                break;
            case "1":
                size = "SizeMtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                break;
        }
        string str = @"select Distinct SizeId," + size + @" as size from V_FinishedItemDetail vf with(nolock)
                       inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + @"
                       And ShapeId= " + DDShape.SelectedValue + " and Sizeid<>0";
        if (ddquality.SelectedIndex > 0)
        {
            str = str + " and  vf.QualityId=" + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " and  vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " and  vf.colorid=" + DDColor.SelectedValue;
        }
        str = str + " order by size";
        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--Plz Select--");
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, e);
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";

        if (UtilityModule.VALIDDROPDOWNLIST(ddquality) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
        {
            goto a;
        }        
        else
        {
            goto B;
        }
    a:
        lblMessage.Visible = true;
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (Session["VarCompanyNo"].ToString() == "22")
        {           
            CHECKVALIDCONTROL();            
        }
        if (lblMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int finishedid = 0;
                finishedid = UtilityModule.getItemFinishedId(DDArticleName, ddquality, DDDesign, DDColor, DDShape, ddSize, TxtProductCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
                
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
                param[1] = new SqlParameter("@UnitsId", Divuniname.Visible == true ? DDUnitName.SelectedValue : "0");
                param[2] = new SqlParameter("@Finishedid", finishedid);
                param[3] = new SqlParameter("@Unitrate", txtrate.Text);
                param[4] = new SqlParameter("@UserId", Session["varuserid"]);
                param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[6] = new SqlParameter("@JobId", ddJob.SelectedValue);
                param[7] = new SqlParameter("@Ratetype", DDRatetype.SelectedValue);
                param[8] = new SqlParameter("@CommRate", txtcommrate.Text == "" ? "0" : txtcommrate.Text);
                param[9] = new SqlParameter("@RateLocation", DivRateLocation.Visible == false ? "0" : DDRateLocation.SelectedValue);
                param[10] = new SqlParameter("@flagsize", DDsizeType.SelectedValue);
                param[11] = new SqlParameter("@EmpId", DivWeavingEmployee.Visible == false ? "0" : DDWeavingEmp.SelectedValue);
                param[12] = new SqlParameter("@Bonus", DivBonus.Visible == false ? "0" : TxtBouns.Text == "" ? "0" : TxtBouns.Text);
                param[13] = new SqlParameter("@FinisherRate", DivFinisherRate.Visible == false ? "0" : TxtFinisherRate.Text == "" ? "0" : TxtFinisherRate.Text);
                param[14] = new SqlParameter("@OrderTypeId", DivOrderType.Visible == false ? "0" : DDOrderType.SelectedValue);
                param[15] = new SqlParameter("@Remark", TxtRemark.Text);
                param[16] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

                //Save data
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveJobRate", param);
                //
                Tran.Commit();
                lblMessage.Text = "Unit Price entered successfully....";
                txtrate.Text = "";
                txtcommrate.Text = "";
                TxtBouns.Text = "";
                TxtFinisherRate.Text = "";
                TxtRemark.Text = "";
                fillGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        
        
    }
    protected void fillGrid()
    {
        DataSet ds = null;
        string str = @"select  vf.item_id,vf.ITEM_NAME+'  '+vf.QualityName+'  '+vf.designName as Articles,PNM.Process_name as JobName,vf.ColorName as Colour,case when tj.flagsize=1 then vf.SizeMtr else vf.sizeft end as Size,
                    Tj.Unitrate,Tj.Date,Vf.shapeName as Shape,Ratetype=case when tj.ratetype=1 then 'Pcs Wise' else 'Area Wise' End,tj.Commrate,
                    case when Tj.RateLocation=0 then 'InHouse' else 'OutSide' end as RateLocation,isnull(EI.EmpName,'') as EmpName, Tj.Bonus
                    ,isnull(TJ.FinisherRate,0) as FinisherRate,isnull(TJ.OrderTypeId,0) as OrderTypeId ,isnull(OC.Ordercategory,'') as OrderType, Tj.Remark 
                    from tbjobrate Tj(Nolock) 
                    inner join V_FinishedItemDetail vf(Nolock) on Tj.finishedid=vf.ITEM_FINISHED_ID  
                    LEFT JOIN EmpInfo EI(Nolock) ON TJ.EmpId=EI.EmpID                 
                    inner join PROCESS_NAME_MASTER PNM(Nolock) on PNM.PROCESS_NAME_ID=Tj.jobid 
                    LEFT JOIN OrderCategory OC(NoLock) ON TJ.OrderTypeId=OC.OrderCategoryId
                    Where Tj.mastercompanyId=" + Session["varcompanyId"] + " And Tj.companyId=" + DDCompanyName.SelectedValue + @"
                    And Tj.BranchID = " + DDBranchName.SelectedValue;

        if (Divuniname.Visible == true)
        {
            if (DDUnitName.SelectedIndex >= 0)
            {
                str = str + "  And Tj.Unitid= " + DDUnitName.SelectedValue;
            }
        }
        if (DivOrderType.Visible == true)
        {
            if (DDOrderType.SelectedIndex >= 0)
            {
                str = str + "  And Tj.OrderTypeId= " + DDOrderType.SelectedValue;
            }
        }
        if (ddJob.SelectedIndex > 0)
        {
            str = str + "  And Tj.Jobid= " + ddJob.SelectedValue;
        }
        if (DDArticleName.SelectedIndex > 0)
        {
            str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
        }
        if (ddquality.SelectedIndex > 0)
        {
            str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
        }
        if (divratetype.Visible == true)
        {
            if (DDRatetype.SelectedIndex != -1)
            {
                str = str + "  And TJ.Ratetype = " + DDRatetype.SelectedValue;
            }
        }
        if (DDRateLocation.SelectedIndex != -1)
        {
            str = str + "  And TJ.RateLocation = " + DDRateLocation.SelectedValue;
        }
        if (DivWeavingEmployee.Visible == true)
        {
            if (DDWeavingEmp.SelectedIndex > 0)
            {
                str = str + "  And Tj.EmpId = " + DDWeavingEmp.SelectedValue;
            }
        }

        str = str + " order by tj.id desc";
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGRateDetail.DataSource = ds;
        DGRateDetail.DataBind();
    }
    protected void ddJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddJob.SelectedItem.Text.ToUpper())
        {
            case "WEAVING":
                divratetype.Visible = true;
                break;
            case "PANEL MAKING":
                divratetype.Visible = true;
                break;
            case "FILLER MAKING":
                divratetype.Visible = true;
                break;
            default:
                divratetype.Visible = false;
                break;
        }
        fillGrid();
    }
    protected void DDUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }

    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct designId,designName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " And Designid<>0 order by designName", true, "--Plz Select--");
        fillGrid();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct Colorid,ColorName from V_FinishedItemDetail vf with(nolock)
                                           inner join CategorySeparate cs with(nolock) on vf.CATEGORY_ID=cs.Categoryid and cs.id=0 And ITEM_ID=" + DDArticleName.SelectedValue + " and QualityId=" + ddquality.SelectedValue + " and DesignId=" + DDDesign.SelectedValue + " And Colorid<>0 order by ColorName", true, "--Plz Select--");
        if (DDColor.Items.Count > 0)
        {
            DDColor_SelectedIndexChanged(sender, new EventArgs());
        }
        fillGrid();
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }

    private void ddlcategorycange()
    {
        divQuality.Visible = false;
        divDesign.Visible = false;
        divColor.Visible = false;
        divShape.Visible = false;
        divSize.Visible = false;
        divshade.Visible = false;

        UtilityModule.ConditionalComboFill(ref DDArticleName, @"select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM inner join CategorySeparate CS on IM.CATEGORY_ID=CS.Categoryid
                                                            and CS.id=0 and cs.categoryid=" + DDCategory.SelectedValue + " and IM.MasterCompanyid=" + Session["varcompanyId"] + " order by IM.ITEM_NAME", true, "--Plz Select--");

        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        divQuality.Visible = true;
                        break;
                    case "2":
                        divDesign.Visible = true;
                        break;
                    case "3":
                        divColor.Visible = true;
                        break;
                    case "4":
                        divShape.Visible = true;
                        break;
                    case "5":
                        divSize.Visible = true;
                        break;
                    case "6":
                        divshade.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, @"Select Distinct SC.ShadeColorID, SC.ShadeColorName 
                                From ShadeColor SC(nolock)
                                Order By ShadeColorName", true, "--Plz Select--");
                        break;
                }
            }
        }
    }
    protected void DDsizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDShape_SelectedIndexChanged(sender, new EventArgs());
    }
    protected void DDRatetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void DDRateLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "27" || Session["VarCompanyId"].ToString() == "42")
        {
            if (DDRateLocation.SelectedValue == "1")
            {
                DivWeavingEmployee.Visible = true;
                BindWeavingEmp();
            }
            else
            {
                DivWeavingEmployee.Visible = false;
                DDWeavingEmp.SelectedIndex = 0;
            }
        }
        if (Session["VarCompanyId"].ToString() == "16" || Session["VarCompanyId"].ToString() == "28")
        {
            DivWeavingEmployee.Visible = true;
            BindWeavingEmp();
        }
        fillGrid();
    }
    protected void DDWeavingEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void DGRateDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVFinisherJobRate, "select$" + e.Row.RowIndex);

            for (int i = 0; i < DGRateDetail.Columns.Count; i++)
            {
                if (DGRateDetail.Columns[i].HeaderText == "Rate Location")
                {
                    if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
                    {
                        DGRateDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGRateDetail.Columns[i].Visible = false;
                    }
                }
                if (DGRateDetail.Columns[i].HeaderText == "Bonus" || DGRateDetail.Columns[i].HeaderText == "Finisher Rate" || DGRateDetail.Columns[i].HeaderText == "Order Type")
                {
                    if (Convert.ToInt32(Session["varcompanyId"]) == 42)
                    {
                        DGRateDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGRateDetail.Columns[i].Visible = false;
                    }
                }

                //if (DGRateDetail.Columns[i].HeaderText == "Emp Name")
                //{
                //    if (Session["varCompanyId"].ToString() == "27")
                //    {
                //        DGRateDetail.Columns[i].Visible = true;
                //    }
                //    else
                //    {
                //        DGRateDetail.Columns[i].Visible = false;
                //    }
                //}
            }
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        try
        {
            string str = "";

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                str = str + "  And Tj.OrderTypeId= " + DDOrderType.SelectedValue;
            }

            if (Divuniname.Visible == true)
            {
                if (DDUnitName.SelectedIndex >= 0)
                {
                    str = str + "  And Tj.Unitid= " + DDUnitName.SelectedValue;
                }
            }
            if (ddJob.SelectedIndex > 0)
            {
                str = str + "  And Tj.Jobid= " + ddJob.SelectedValue;
            }
            if (DDArticleName.SelectedIndex > 0)
            {
                str = str + "  And vf.Item_Id= " + DDArticleName.SelectedValue;
            }
            if (ddquality.SelectedIndex > 0)
            {
                str = str + "  And vf.QualityId= " + ddquality.SelectedValue;
            }
            if (DDDesign.SelectedIndex > 0)
            {
                str = str + "  And vf.DesignId= " + DDDesign.SelectedValue;
            }

            if (Session["VarCompanyNo"].ToString() == "22")
            {
                if (DDColor.SelectedIndex > 0)
                {
                    str = str + "  And vf.ColorId= " + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + "  And vf.ShapeId= " + DDShape.SelectedValue;
                }
                if (ddSize.SelectedIndex > 0)
                {
                    str = str + "  And vf.SizeId= " + ddSize.SelectedValue;
                }
            }

            if (divratetype.Visible == true)
            {
                if (DDRatetype.SelectedIndex != -1)
                {
                    str = str + "  And TJ.Ratetype= " + DDRatetype.SelectedValue;
                }
            }
            if (DivRateLocation.Visible == true)
            {
                str = str + "  And TJ.RateLocation = " + DDRateLocation.SelectedValue;
            }

            if (DivDateRange.Visible == true)
            {
                if (ChkForDate.Checked == true)
                {
                    str = str + " and DATEADD(dd, 0, DATEDIFF(dd, 0, TJ.Date))>='" + txtFromDate.Text + "' and DATEADD(dd, 0, DATEDIFF(dd, 0, TJ.Date))<='" + txtToDate.Text + "'";
                }
            }

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[1] = new SqlParameter("@where", str);
            param[2] = new SqlParameter("@CurrentRate", chkcurrentrate.Checked == true ? "1" : "0");

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PRINTDEFINEJOBRATE", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28")
                {
                    if (chkcurrentrate.Checked == true)
                    {
                        ProcessJobRateInExcel(ds);
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
                    }

                }
                else if (Session["VarCompanyNo"].ToString() == "42")
                {
                    Session["rptFileName"] = "~\\Reports\\RptJobRateVikramMirzapur.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
                }
                else
                {
                    Session["rptFileName"] = "~\\Reports\\rptjobrate.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptjobrate.xsd";
                }
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    private void ProcessJobRateInExcel(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            int row = 0;

            sht.Range("A1:L1").Merge();
            sht.Range("A1").Value = ds.Tables[0].Rows[0]["JobName"] + " " + "RATE LIST FORMAT";
            //sht.Range("A2:X2").Merge();
            //sht.Range("A2").Value = "Filter By :  " + FilterBy;
            //sht.Row(2).Height = 30;
            sht.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A2:L2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A2:L2").Style.Alignment.SetWrapText();
            sht.Range("A1:L2").Style.Font.FontName = "Arial";
            sht.Range("A1:L2").Style.Font.FontSize = 10;
            sht.Range("A1:L2").Style.Font.Bold = true;

            //*******Header
            sht.Range("A3").Value = "SR NO.";
            sht.Range("B3").Value = "ITEM";
            sht.Range("C3").Value = "QUALITY";
            sht.Range("D3").Value = "DESIGN";
            sht.Range("E3").Value = "COLOR";
            sht.Range("F3").Value = "SHAPE";
            sht.Range("G3").Value = "SIZE";
            sht.Range("H3").Value = "RATE TYPE";
            sht.Range("I3").Value = "RATE";
            sht.Range("J3").Value = "COMM. RATE";
            sht.Range("K3").Value = "LOCATION";
            sht.Range("L3").Value = "DATE";


            sht.Range("A3:L3").Style.Font.FontName = "Arial";
            sht.Range("A3:L3").Style.Font.FontSize = 9;
            sht.Range("A3:L3").Style.Font.Bold = true;
            sht.Range("S3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:L3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A3:L3").Style.Alignment.SetWrapText();


            //DataView dv = new DataView(ds.Tables[0]);
            //dv.Sort = "FOLIONO";
            //DataSet ds1 = new DataSet();
            //ds1.Tables.Add(dv.ToTable());

            int srno = 0;
            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":L" + row).Style.Font.FontName = "Arial";
                sht.Range("A" + row + ":L" + row).Style.Font.FontSize = 8;

                srno = srno + 1;

                sht.Range("A" + row).SetValue(srno);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Colour"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["Shape"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["RateType"]);
                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["UnitRate"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["CommRate"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["RateLocation"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["Date"]);


                row = row + 1;

            }
            //*************
            sht.Columns(1, 26).AdjustToContents();

            //sht.Columns("K").Width = 13.43;

            using (var a = sht.Range("A1" + ":L" + row))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessJobRate_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
            Path = Server.MapPath("~/Tempexcel/" + filename);
            xapp.SaveAs(Path);
            xapp.Dispose();
            //Download File
            Response.ClearContent();
            Response.ClearHeaders();
            // Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.WriteFile(Path);
            // File.Delete(Path);
            Response.End();
        }
    }
}