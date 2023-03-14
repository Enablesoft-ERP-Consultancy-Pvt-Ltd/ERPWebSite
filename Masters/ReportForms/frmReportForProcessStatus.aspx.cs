using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_frmReportForProcessStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId,CI.Companyname from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MastercompanyId=" + Session["varCompanyId"] + @" Order by Companyname 
                        Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By PROCESS_NAME
                        select CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER
                        select val,type from sizetype";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            CommanFunction.FillComboWithDS(DDCompany, ds, 0);
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "--select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 3, false, "");
            ds.Dispose();
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            string str2;
            str2 = "select SVM.SupervisorId,SVM.SupervisorName from SupervisorMaster SVM order by SVM.SupervisorName";
            UtilityModule.ConditionalComboFill(ref DDSupervisorName, str2, true, "--Select--");

            RDAll.Checked = true;
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName_SelectedIndexChanged(sender, e);
            }
            switch (Session["varcompanyId"].ToString())
            {
                case "16":
                case "14":
                    RDAll.Visible = false;
                    RDOrder.Visible = false;
                    RDReceive.Visible = false;
                    RDAll.Checked = false;
                    break;
                case "22":
                    RDAll.Visible = false;
                    RDOrder.Visible = false;
                    RDReceive.Visible = false;
                    RDAll.Checked = false;
                    RDRawMaterialSupervisorWise.Visible = true;
                    break;
                default:
                    RDRawMaterialSupervisorWise.Visible = false;
                    break;
            }

        }
    }
    protected void FillLoomNo()
    {
        if (DDSupervisorName.SelectedIndex > 0)
        {
            string str;
            str = "select PLM.UID,PLM.LoomNo from ProductionLoomMaster PLM where PLM.SupervisorId=" + DDSupervisorName.SelectedValue + " Order by PLM.LoomNo";
            UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Select--");
        }
        else
        {
            DDLoomNo.Items.Clear();
        }
    }
    protected void DDSupervisorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLoomNo();
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployee();
    }
    protected void FillEmployee()
    {
        string str;
        str = "select Distinct Ei.EmpId,ei.EmpName + case when Isnull(ei.empcode,'')<>'' Then ' ['+ei.empcode+']' else '' end as Empname from empinfo ei inner join  EmpProcess EP on ei.EmpId=Ep.EmpId";
        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + " Where ep.ProcessId=" + DDProcessName.SelectedValue;
        }
        str = str + " order by EmpName";
        UtilityModule.ConditionalComboFill(ref DDEmpName, str, true, "--Select--");
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        categoryselectedChange();
        ddItemName_SelectedIndexChanged(sender, e);
    }
    protected void categoryselectedChange()
    {
        TRDDQuality.Visible = false;
        TRDDDesign.Visible = false;
        TRDDColor.Visible = false;
        TRDDShape.Visible = false;
        TRDDSize.Visible = false;
        TRDDShadeColor.Visible = false;

        string str;
        if (RDRawMaterial.Checked == true)
        {
            str = "select distinct vf.ITEM_ID,vf.ITEM_NAME from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.CATEGORY_ID=" + DDCategory.SelectedValue + " order by Item_name";
        }
        else if (DDProcessName.SelectedIndex > 0)
        {
            str = "select distinct vf.ITEM_ID,vf.ITEM_NAME from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   vf.CATEGORY_ID=" + DDCategory.SelectedValue + " order by ITEM_NAME";
        }
        else
        {
            str = "select ITEM_ID,ITEM_NAME from ITEM_MASTER where CATEGORY_ID=" + DDCategory.SelectedValue + " order by Item_name";
        }
        UtilityModule.ConditionalComboFill(ref ddItemName, str, true, "--All--");


        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID  from item_category_parameters where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["Parameter_id"].ToString())
                {
                    case "1":
                        TRDDQuality.Visible = true;
                        break;
                    case "2":
                        TRDDDesign.Visible = true;
                        FillDesign();
                        break;
                    case "3":
                        TRDDColor.Visible = true;
                        FillColor();
                        break;
                    case "4":
                        TRDDShape.Visible = true;
                        FillShape();
                        break;
                    case "5":
                        TRDDSize.Visible = true;
                        FillSize();
                        break;
                    case "6":
                        TRDDShadeColor.Visible = true;
                        FillShadecolor();
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TRDDQuality.Visible == true)
        {
            string str;
            if (RDRawMaterial.Checked == true)
            {
                str = "select distinct vf.QualityId,vf.QualityName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where Vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And Vf.Item_Id=" + ddItemName.SelectedValue;
                }
                str = str + " order by QualityName";

            }
            else if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.QualityId,vf.QualityName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   Vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And Vf.Item_Id=" + ddItemName.SelectedValue;
                }
                str = str + " order by QualityName";
            }
            else
            {
                str = "select QualityId,QualityName from quality Where Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And Item_Id=" + ddItemName.SelectedValue;
                }
                str = str + " order by QualityName";
            }
            UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--All--");
        }
        if (ddItemName.SelectedIndex > 0)
        {
            FillDesign();
            FillColor();
            FillShape();
            FillShadecolor();
        }
    }
    protected void FillDesign()
    {
        string str;
        if (TRDDDesign.Visible == true)
        {
            if (RDRawMaterial.Checked == true)
            {
                str = "select distinct vf.designId,vf.designName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                str = str + " order by designname";
            }
            else if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.designId,vf.designName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where   vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                str = str + " order by designname";
            }
            else
            {
                str = "select distinct vf.designId,vf.designName from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                str = str + " order by designname";
            }
            UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--select--");
        }
    }
    protected void FillColor()
    {
        string str;
        if (TRDDColor.Visible == true)
        {
            if (RDRawMaterial.Checked == true)
            {
                str = "select distinct vf.colorid,vf.ColorName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                str = str + " order by colorname";
            }
            else if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.colorid,vf.colorname from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                str = str + " order by colorname";
            }
            else
            {
                str = "select distinct vf.colorid,vf.colorname from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                str = str + " order by colorname";
            }
            UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--select--");
        }
    }
    protected void FillShape()
    {
        string str;
        if (TRDDShape.Visible == true)
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.ShapeId,vf.ShapeName from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }

                str = str + " order by ShapeName";
            }
            else
            {
                str = "select distinct vf.ShapeId,vf.ShapeName from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                str = str + " order by shapename";
            }
            UtilityModule.ConditionalComboFill(ref DDShape, str, true, "--select--");
        }
    }

    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDesign();
        FillShadecolor();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillSize()
    {
        string str;
        string strSize;
        switch (DDsizetype.SelectedValue)
        {
            case "0":
                strSize = "Sizeft";
                break;
            case "1":
                strSize = "Sizemtr";
                break;
            case "2":
                strSize = "Sizeinch";
                break;
            default:
                strSize = "Sizeft";
                break;
        }
        if (TRDDSize.Visible == true)
        {
            if (DDProcessName.SelectedIndex > 0)
            {
                str = "select distinct vf.SizeId,vf." + strSize + " as Size from PROCESS_ISSUE_DETAIL_" + DDProcessName.SelectedValue + " PD inner join V_FinishedItemDetail vf on Pd.Item_Finished_Id=vf.ITEM_FINISHED_ID Where  vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                if (TRDDColor.Visible == true && DDColor.SelectedIndex > 0)
                {
                    str = str + " And vf.colorid=" + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and vf.ShapeId=" + DDShape.SelectedValue;
                }
                str = str + " order by Size";
            }
            else
            {
                str = "select distinct vf.SizeId,vf." + strSize + " as Size from V_FinishedItemDetail vf Where vf.Mastercompanyid=" + Session["varcompanyid"];
                if (ddItemName.SelectedIndex > 0)
                {
                    str = str + " And vf.Item_Id=" + ddItemName.SelectedValue;
                }
                if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
                }
                if (TRDDDesign.Visible == true && DDDesign.SelectedIndex > 0)
                {
                    str = str + " And vf.designid=" + DDDesign.SelectedValue;
                }
                if (TRDDColor.Visible == true && DDColor.SelectedIndex > 0)
                {
                    str = str + " And vf.colorid=" + DDColor.SelectedValue;
                }
                if (DDShape.SelectedIndex > 0)
                {
                    str = str + " and vf.ShapeId=" + DDShape.SelectedValue;
                }
                str = str + " order by Size";
            }
            UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--select--");
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    protected void FillShadecolor()
    {
        string str;
        if (TRDDShadeColor.Visible == true)
        {
            str = "select distinct vf.ShadecolorId,vf.ShadeColorName from ProcessRawTran PT inner Join V_FinishedItemDetail vf on Pt.Finishedid=vf.ITEM_FINISHED_ID Where vf.Item_Id=" + ddItemName.SelectedValue;
            if (TRDDQuality.Visible == true && DDQuality.SelectedIndex > 0)
            {
                str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
            }
            str = str + " order by vf.ShadeColorName";
            UtilityModule.ConditionalComboFill(ref DDShadeColor, str, true, "--select--");
        }
    }
    protected void ChkForDate_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForDate.Checked == true)
        {
            trDates.Visible = true;
            TxtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
        else
        {
            trDates.Visible = false;
        }
    }
    protected string OrderWhereCondition()
    {
        string Where = "";
        if (DDEmpName.SelectedIndex > 0)
        {
            Where = Where + " and Ei.EmpId=" + DDEmpName.SelectedValue;
        }
        if (DDStatus.SelectedIndex > 0)
        {
            Where = Where + " and PM.status in('pending','Partially Processed')";
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Where = Where + " and vf.item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Where = Where + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Where = Where + " and vf.SizeId=" + DDSize.SelectedValue;
        }
        if (txtlocalorderNo.Text != "")
        {
            Where = Where + " and OM.localOrder='" + txtlocalorderNo.Text + "'";
        }
        if (ChkForDate.Checked == true)
        {
            Where = Where + " and PM.AssignDate>='" + TxtFromDate.Text + "' and PM.assignDate<='" + TxtToDate.Text + "'";
        }
        return Where;
    }
    protected string ProcessRecCondition()
    {
        string Where = "";
        if (DDEmpName.SelectedIndex > 0)
        {
            Where = Where + " and Ei.EmpId=" + DDEmpName.SelectedValue;
        }
        if (DDStatus.SelectedIndex > 0)
        {
            Where = Where + " and PM.status in('pending','Partially Processed')";
        }
        if (ddItemName.SelectedIndex > 0)
        {
            Where = Where + " and vf.item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Where = Where + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Where = Where + " and vf.SizeId=" + DDSize.SelectedValue;
        }
        if (txtlocalorderNo.Text != "")
        {
            Where = Where + " and OM.localOrder='" + txtlocalorderNo.Text + "'";
        }
        if (ChkForDate.Checked == true)
        {
            Where = Where + " and PM.ReceiveDate>='" + TxtFromDate.Text + "' and PM.ReceiveDate<='" + TxtToDate.Text + "'";
        }
        return Where;
    }
    protected string ProcessRawmaterialCondition()
    {
        string Where = "";
        //if (DDEmpName.SelectedIndex > 0)
        //{
        //    Where = Where + " and Ei.EmpId=" + DDEmpName.SelectedValue;
        //}       
        if (ddItemName.SelectedIndex > 0)
        {
            Where = Where + " and vf.item_id=" + ddItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0)
        {
            Where = Where + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            Where = Where + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDShadeColor.SelectedIndex > 0)
        {
            Where = Where + " and vf.shadecolorid=" + DDShadeColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0)
        {
            Where = Where + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            Where = Where + " and vf.SizeId=" + DDSize.SelectedValue;
        }
        if (txtlocalorderNo.Text != "")
        {
            Where = Where + " and OM.localOrder='" + txtlocalorderNo.Text + "'";
        }
        if (ChkForDate.Checked == true)
        {
            Where = Where + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
        }
        if (DDRawTransaction.SelectedIndex > 0)
        {
            if (DDRawTransaction.SelectedIndex == 1)
            {
                Where = Where + " and PM.Trantype=0";
            }
            else
            {
                Where = Where + " and PM.Trantype=1";
            }

        }
        return Where;
    }
    protected string ProcessRawmaterialConditionSupervisorWise()
    {
        string Where = "";

        if (DDLoomNo.SelectedIndex > 0)
        {
            Where = Where + " and PLM.LoomNo=" + DDLoomNo.SelectedValue;
        }
        if (ChkForDate.Checked == true)
        {
            Where = Where + " and PM.Date>='" + TxtFromDate.Text + "' and PM.Date<='" + TxtToDate.Text + "'";
        }
        if (DDRawTransaction.SelectedIndex > 0)
        {
            if (DDRawTransaction.SelectedIndex == 1)
            {
                Where = Where + " and PM.Trantype=0";
            }
            else
            {
                Where = Where + " and PM.Trantype=1";
            }

        }
        return Where;
    }
    protected void show_all()
    {
        string Where = OrderWhereCondition();
        int Dateflag = 0;
        if (ChkForDate.Checked == true)
        {
            Dateflag = 1;
        }
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0"));
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        param[3] = new SqlParameter("@where", Where);
        param[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[5] = new SqlParameter("@ToDate", TxtToDate.Text);
        param[6] = new SqlParameter("@Dateflag", Dateflag);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetProcessOrderSummary]", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/RptProcessSummaryAll.rpt";
            Session["dsfileName"] = "~\\ReportSchema\\RptProcessSummaryAll.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No record found');", true);
        }

    }
    protected void ProcessOrderSummary()
    {
        string Where = OrderWhereCondition();
        int Dateflag = 0;
        if (ChkForDate.Checked == true)
        {
            Dateflag = 1;
        }
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0"));
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        param[3] = new SqlParameter("@where", Where);
        param[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[5] = new SqlParameter("@ToDate", TxtToDate.Text);
        param[6] = new SqlParameter("@Dateflag", Dateflag);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetProcessOrderSummary]", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/RptProcessOrderSummary.rpt";
            Session["dsfileName"] = "~\\ReportSchema\\RptProcessOrderSummary.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No record found');", true);
        }
    }

    protected void ProcessReceiveSummary()
    {
        string Where = ProcessRecCondition();
        int Dateflag = 0;
        if (ChkForDate.Checked == true)
        {
            Dateflag = 1;
        }
        SqlParameter[] param = new SqlParameter[7];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0"));
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        param[3] = new SqlParameter("@where", Where);
        param[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[5] = new SqlParameter("@ToDate", TxtToDate.Text);
        param[6] = new SqlParameter("@Dateflag", Dateflag);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetProcessReceiveSummary]", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (chksummary.Checked == true)
            {
                Session["rptFileName"] = "Reports/RptProcessReceiveSummarynew.rpt";
            }
            else
            {
                Session["rptFileName"] = "Reports/RptProcessReceiveSummary.rpt";
            }
            Session["dsfileName"] = "~\\ReportSchema\\RptProcessReceiveSummary.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No record found');", true);
        }
    }

    protected void ProcessRawMaterialDetail()
    {
        string Where = ProcessRawmaterialCondition();
        int Dateflag = 0;
        if (ChkForDate.Checked == true)
        {
            Dateflag = 1;
        }
        SqlParameter[] param = new SqlParameter[8];
        param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessId", (DDProcessName.SelectedIndex > 0 ? DDProcessName.SelectedValue : "0"));
        param[2] = new SqlParameter("@userid", Session["varuserid"]);
        param[3] = new SqlParameter("@where", Where);
        param[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
        param[5] = new SqlParameter("@ToDate", TxtToDate.Text);
        param[6] = new SqlParameter("@Dateflag", Dateflag);
        param[7] = new SqlParameter("@EMPID", DDEmpName.SelectedIndex > 0 ? DDEmpName.SelectedValue : "0");

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[Pro_GetProcessRawIssRecDetail]", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDRawTransaction.SelectedIndex > 0)
            {
                Session["rptFileName"] = "Reports/RptProcessrawIssRec.rpt";
                Session["dsfileName"] = "~\\ReportSchema\\RptProcessrawIssRecAll.xsd";
            }
            else
            {
                Session["rptFileName"] = "Reports/RptProcessrawIssRecAll.rpt";
                Session["dsfileName"] = "~\\ReportSchema\\RptProcessrawIssRecAll.xsd";
            }
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No record found');", true);
        }
    }
    protected void ProcessRawMaterialDetailSupervisorWise()
    {
        if (DDSupervisorName.SelectedIndex > 0)
        {
            string Where = ProcessRawmaterialConditionSupervisorWise();
            int Dateflag = 0;
            if (ChkForDate.Checked == true)
            {
                Dateflag = 1;
            }
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@companyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@SupervisorId", (DDSupervisorName.SelectedIndex > 0 ? DDSupervisorName.SelectedValue : "0"));
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@where", Where);
            param[4] = new SqlParameter("@FromDate", TxtFromDate.Text);
            param[5] = new SqlParameter("@ToDate", TxtToDate.Text);
            param[6] = new SqlParameter("@Dateflag", Dateflag);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "[PRO_GETPROCESSRAWISSRECDETAILSUPERVISORWISE]", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDRawTransaction.SelectedIndex > 0)
                {
                    Session["rptFileName"] = "Reports/RptProcessrawIssRecSupervisorWise.rpt";
                    Session["dsfileName"] = "~\\ReportSchema\\RptProcessrawIssRecAllSupervisorWise.xsd";
                }
                else
                {
                    Session["rptFileName"] = "Reports/RptProcessrawIssRecAllSupervisorWise.rpt";
                    Session["dsfileName"] = "~\\ReportSchema\\RptProcessrawIssRecAllSupervisorWise.xsd";
                }
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);


            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('No record found');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please select supervisor name');", true);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (RDAll.Checked == true)
        {
            show_all();
        }
        else if (RDOrder.Checked == true)
        {
            ProcessOrderSummary();
        }
        else if (RDReceive.Checked == true)
        {
            ProcessReceiveSummary();
        }
        else if (RDRawMaterial.Checked == true)
        {
            ProcessRawMaterialDetail();
        }
        else if (RDRawMaterialSupervisorWise.Checked == true)
        {
            ProcessRawMaterialDetailSupervisorWise();
        }
    }
}