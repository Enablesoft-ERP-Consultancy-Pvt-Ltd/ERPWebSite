using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Carpet_FrmGateInOfAllItems : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string Str = @"Select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And 
                                CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + @" Order by CompanyName 
                            SELECT DepartmentId,DepartmentName FROM Department Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By DepartmentName
                            Select VarProdCode From MasterSetting Where VarCompanyNo=" + Session["varCompanyId"];
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, Ds, 0, true, "Select Compompany");
            UtilityModule.ConditionalComboFillWithDS(ref DDDepartment, Ds, 1, true, "Select Department");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            if (DDDepartment.Items.Count > 0)
            {
                DDDepartment.SelectedIndex = 1;
                DepartmentSelectedChange();
            }
            ParameteLabel();
            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            int VarProdCode = Convert.ToInt32(Ds.Tables[2].Rows[0]["VarProdCode"]);
            switch (VarProdCode)
            {
                case 0:
                    TDProductCode.Visible = false;
                    break;
                case 1:
                    TDProductCode.Visible = true;
                    break;
            }
            CategoryTypeSelectedChange();
        }
    }
    protected void DDDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DepartmentSelectedChange();
    }
    private void DepartmentSelectedChange()
    {
        string Str = @"Select EI.EmpId,EI.EmpName From EmpInfo EI,Department D Where EI.DepartmentId=D.DepartmentId And D.DepartmentId=" + DDDepartment.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"];
        UtilityModule.ConditionalComboFill(ref DDPartyName, Str, true, "--Select Department--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DDCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryTypeSelectedChange();
    }
    private void CategoryTypeSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDCategory, @"Select Distinct VF.CATEGORY_ID,VF.CATEGORY_NAME From V_FinishedItemDetail VF,CategorySeparate CS Where VF.CATEGORY_ID=CS.Categoryid And VF.MasterCompanyId=" + Session["varCompanyId"] + " And CS.ID=" + DDCategoryType.SelectedValue, true, "--Select--");
        if (DDCategory.Items.Count > 0)
        {
            DDCategory.SelectedIndex = 1;
            ddlcategorycange();
        }
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {

    }
    protected void TxtProductCode_TextChanged(object sender, EventArgs e)
    {

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    private void ddlcategorycange()
    {
        TdQuality.Visible = false;
        TdDesign.Visible = false;
        TdColor.Visible = false;
        TdColorShade.Visible = false;
        TdShape.Visible = false;
        TdSize.Visible = false;

        string strsql = @"SELECT distinct IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] Where [CATEGORY_ID]=" + DDCategory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TdQuality.Visible = true;
                        break;
                    case "2":
                        TdDesign.Visible = true;
                        break;
                    case "3":
                        TdColor.Visible = true;
                        break;
                    case "6":
                        TdColorShade.Visible = true;
                        break;
                    case "4":
                        TdShape.Visible = true;
                        break;
                    case "5":
                        TdSize.Visible = true;
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref DDItem, @"Select Distinct VF.ITEM_ID,Vf.ITEM_NAME From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.CATEGORY_ID=" + DDCategory.SelectedValue, true, "--Select Item--");
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDQuality, @"Select Distinct VF.QualityId,VF.QualityName From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue, true, "--Select Item--");
        UtilityModule.ConditionalComboFill(ref DDUnit, "SELECT Distinct U.UnitId,U.UnitName FROM ITEM_MASTER I,Unit U Where I.UnitTypeID=U.UnitTypeID And ITEM_ID=" + DDItem.SelectedValue, true, "Select Unit");
        if (DDUnit.Items.Count > 0)
        {
            DDUnit.SelectedIndex = 1;
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        QualitySelectedChanged();
        FillGodown();
    }
    private void QualitySelectedChanged()
    {
        if (TdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDDesign, @"Select Distinct VF.DesignId,VF.DesignName From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue, true, "--Select Design--");
        }
        if (TdColor.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColor, @"Select Distinct VF.ColorId,VF.ColorName From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue, true, "--Select Color--");
        }
        if (TdShape.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDShape, @"Select Distinct VF.ShapeId,VF.ShapeName From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue, true, "--Select Shape--");
        }
        if (TdColorShade.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDColorShade, @"Select Distinct VF.ShadeColorId,VF.ShadeColorName From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue, true, "--Select ShadeColor--");
        }
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGodown();
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGodown();
    }
    protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGodown();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Size();
    }
    private void Fill_Size()
    {
        string str = "";
        if (ChkFt.Checked == true)
        {
            str = @"Select Distinct VF.SizeId,VF.SizeFt From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        else
        {
            str = @"Select Distinct VF.SizeId,VF.SizeMtr From V_FinishedItemDetail VF Where VF.MasterCompanyId=" + Session["varCompanyId"] + " And VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.QualityId=" + DDQuality.SelectedValue + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Select Size--");
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        Fill_Size();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGodown();
    }
    private void FillGodown()
    {
        if (DDCategoryType.SelectedValue == "1")
        {
            DDGodownName.Items.Clear();
            DDLotNo.Items.Clear();
            TxtStock.Text = "";
            int quality = 0;
            int design = 0;
            int color = 0;
            int shape = 0;
            int size = 0;
            int shadeColor = 0;
            if ((TdQuality.Visible == true && DDQuality.SelectedIndex > 0) || TdQuality.Visible != true)
            {
                quality = 1;
            }
            if (TdDesign.Visible == true && DDDesign.SelectedIndex > 0 || TdDesign.Visible != true)
            {
                design = 1;
            }
            if (TdColor.Visible == true && DDColor.SelectedIndex > 0 || TdColor.Visible != true)
            {
                color = 1;
            }
            if (TdShape.Visible == true && DDShape.SelectedIndex > 0 || TdShape.Visible != true)
            {
                shape = 1;
            }
            if (TdSize.Visible == true && DDSize.SelectedIndex > 0 || TdSize.Visible != true)
            {
                size = 1;
            }
            if (TdColorShade.Visible == true && DDColorShade.SelectedIndex > 0 || TdColorShade.Visible != true)
            {
                shadeColor = 1;
            }
            if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
            {
                try
                {
                    int Finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProductCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
                    UtilityModule.ConditionalComboFill(ref DDGodownName, "Select Distinct S.GodownId,GM.GodownName From Stock S,GodownMaster GM Where S.Godownid=GM.Godownid And S.CompanyId=" + DDCompanyName.SelectedValue + " And ITEM_FINISHED_ID=" + Finishedid + " And GM.MasterCompanyid=" + Session["varCompanyId"] + " And QtyinHand>0 Order By GM.GodownName", true, "--Select Godown--");
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    LblMessage.Visible = true;
                }
            }
        }
    }
    protected void DDGodownName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProductCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));

        UtilityModule.ConditionalComboFill(ref DDLotNo, "Select Distinct S.LotNo,S.LotNo From Stock S Where S.CompanyId=" + DDCompanyName.SelectedValue + " And ITEM_FINISHED_ID=" + Finishedid + " And S.GodownId=" + DDGodownName.SelectedValue + " And QtyinHand>0 Order By S.LotNo", true, "--Select Godown--");
    }
    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProductCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));

        TxtStock.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Isnull(Round(Sum(QtyInHand),3),0) StockQty From Stock Where CompanyId=" + DDCompanyName.SelectedValue + " And ITEM_FINISHED_ID=" + Finishedid + " And GodownId=" + DDGodownName.SelectedValue + " And LotNo='" + DDLotNo.SelectedItem.Text + "'").ToString();
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {

    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        LblQuality.Text = ParameterList[0];
        LblDesign.Text = ParameterList[1];
        LblColor.Text = ParameterList[2];
        LblShape.Text = ParameterList[3];
        LblSize.Text = ParameterList[4];
        LblCategory.Text = ParameterList[5];
        LblItemName.Text = ParameterList[6];
        LblColorShade.Text = ParameterList[7];
    }
}