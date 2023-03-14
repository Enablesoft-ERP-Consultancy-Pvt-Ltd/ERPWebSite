using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Drawing;

public partial class Masters_Carpet_FrmCostingMaster : System.Web.UI.Page
{
    Double Total = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@PageName", "FrmCostingMaster");
            param[1] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"]);
            param[2] = new SqlParameter("@UserID", Session["varuserId"]);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDATA_FOR_PAGELOAD", param);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategoryRD, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessRD, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 4, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDSizeTypeRD, ds, 4, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessUnit, ds, 5, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDDyingType, ds, 6, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessPMD, ds, 7, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessUnitPMD, ds, 8, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds, 9, true, "--Plz Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }
            if (DDCategoryname.Items.Count > 0)
            {
                DDCategoryname.SelectedIndex = 1;
                CategoryDependControls(DDCategoryname, DDItemName, TDQuality, TDDesign, TDColor, TDShape, TDSize, TDShade);
            }

            TDCBM.Attributes.Add("style", "display:none;");
            txtcostingDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DDProcessRD.SelectedValue = "9";
            //*************Create Directory
            string directorypath = Server.MapPath("~/CostingImage");
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            //*************

            if (Session["varcompanyId"].ToString() == "16")
            {
                lblSampleCodeForEdit.Text = "Edit Costing Code";
                lblddSampleCodeForEdit.Text = "Costing Code";
                lblSampleCode.Text = "Costing Code";
                TDWeightWithoutLatex.Visible = false;
                TDWeightWithLatex.Visible = false;
                TDUSDINR.Visible = false;
                TDTHCPercentage.Visible = false;
                TDRMUPercentage.Visible = false;
                TDlblItemInterestPercentage.Visible = true;
                TDFOB.Visible = true;
                TDOverHead.Visible = true;
                TDSalePrice.Visible = true;
                TDInterestPercentage.Visible = true;
                Label1.Text = "Process And Material Detail";
                TDCostingRemark.Visible = true;
                TDTxtDyingType.Visible = false;
                TDDDDyingType.Visible = true;
                TDProcessPMD.Visible = true;
                TDPUnitPMD.Visible = true;
                TDPRatePMD.Visible = true;
                TDCurrency.Visible = true;
                TDExchangeRate.Visible = true;
                TDPoNo.Visible = true;
                TDLicensePercentage.Visible = true;
                TDDrawbackPercentage.Visible = true;
            }
        }
    }
    protected void TxtSampleCodeForEdit_TextChanged(object sender, EventArgs e)
    {
        SampleCodeSearchForEdit();
    }
    protected void SampleCodeSearchForEdit()
    {
        ViewState["CostingItemMasterID"] = null;
        string str;
        str = @"Select a.CostingItemMasterID, a.SampleCode + Case When a.CostingType = 0 Then '' Else ' / ' + IsNull(CI.CustomerCode, '') End + ' / ' + replace(convert(varchar(11), a.CostingDate, 106), ' ', '-')  CostingDate 
                From CostingItemMaster a(Nolock) 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_id 
                LEFT JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = a.CustomerID 
                Where a.CompanyID = " + DDCompanyName.SelectedValue + " And a.MasterCompanyID = " + Session["varCompanyId"] + @" And 
                a.SampleCode Like '%" + TxtSampleCodeForEdit.Text + @"%'";
        if (DDItemName.SelectedIndex > 0)
        {
            str = str + " And VF.Item_ID = " + DDItemName.SelectedValue;
        }
        if (TDQuality.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityID = " + DDQuality.SelectedValue;
        }
        if (TDDesign.Visible == true && DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.DesignID = " + DDDesign.SelectedValue;
        }
        if (TDColor.Visible == true && DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.ColorID = " + DDColor.SelectedValue;
        }
        if (TDShade.Visible == true && DDshape.SelectedIndex > 0)
        {
            str = str + " And VF.ShapeID = " + DDshape.SelectedValue;
        }
        if (TDSize.Visible == true && DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.SizeID = " + DDSize.SelectedValue;
        }
        if (TDShade.Visible == true && DDshade.SelectedIndex > 0)
        {
            str = str + " And VF.ShadeColorID = " + DDshade.SelectedValue;
        }
        if (tdDDCustomerCode.Visible == true && DDCustomerCode.SelectedIndex > 0)
        {
            str = str + " And a.CustomerID = " + DDCustomerCode.SelectedValue;
        }

        str = str + " Order By a.CostingItemMasterID Desc";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblddSampleCodeForEdit.Visible = true;
            DDSampleCode.Visible = true;

            UtilityModule.ConditionalComboFillWithDS(ref DDSampleCode, ds, 0, true, "--Select--");
            DDSampleCode.SelectedIndex = 1;
            DDSampleCodeSelectedIndexChanged();
        }
        else
        {
            TxtSampleCodeForEdit.Text = "";
            TxtSampleCodeForEdit.Focus();
        }
    }
    protected void DDSampleCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAddImage.Visible = false;
        ViewState["CostingItemMasterID"] = null;
        if (DDSampleCode.SelectedIndex > 0)
        {
            DDSampleCodeSelectedIndexChanged();
        }
    }
    protected void DDSampleCodeSelectedIndexChanged()
    {
        ViewState["CostingItemMasterID"] = DDSampleCode.SelectedValue;
        btnAddImage.Visible = true;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select a.CostingItemMasterID, a.CompanyID, a.SizeType, VF.CATEGORY_ID, VF.ITEM_ID, VF.QualityId, VF.designId, 
                    VF.ColorId, VF.ShapeId, VF.SizeId, VF.ShadeColorid, a.CostingType, a.CustomerID, a.SampleCode, replace(convert(varchar(11), a.CostingDate, 106), ' ', '-') CostingDate, a.Description, a.WeightWithoutLatex, a.WeightWithLatex, 
                    a.USDVsINR, a.THCPercentage, a.RMUPercentage, a.Interest, a.FOB, a.OverHead, a.SalePrice, a.CurrencyID, a.ExchangeRate, a.PoNo, a.licensePercentage, a.DrawBackPercentage 
                    From CostingItemMaster a(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_id 
                    Where a.CompanyID = " + DDCompanyName.SelectedValue + " And a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CostingItemMasterID = " + DDSampleCode.SelectedValue + @"

                    Select Top 1 a.CostingItemMasterID, a.ProcessID, a.SizeType, VF.CATEGORY_ID, VF.ITEM_ID, VF.QualityId, VF.designId, VF.ColorId, VF.ShapeId, VF.SizeId, VF.ShadeColorid, 
                    a.Consumption, a.Rate, a.WastagePercentage, a.ProcessRate, a.ProcessType, a.Amount, a.Interest, a.DyingType 
                    From CostingItemProcessDetail a(Nolock) 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_id 
                    Where a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CostingItemMasterID = " + DDSampleCode.SelectedValue + @"

                    Select Top 1 a.CostingItemMasterID, a.ProcessID, a.UnitID, a.Rate, a.Amount, a.Remark 
                    From CostingProcessRateDetail a(Nolock)
                    Where CostingItemProcessDetailFinishedID = 0 And a.MasterCompanyID = " + Session["varCompanyId"] + " And a.CostingItemMasterID = " + DDSampleCode.SelectedValue);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["SizeType"].ToString();
            DDCostingFor.SelectedValue = ds.Tables[0].Rows[0]["CostingType"].ToString();
            if (DDCostingFor.SelectedValue == "1")
            {
                DDCostingForSelectedIndexChanged();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerID"].ToString();
            }
            DDCategoryname.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            CategoryDependControls(DDCategoryname, DDItemName, TDQuality, TDDesign, TDColor, TDShape, TDSize, TDShade);
            DDItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
            QDCSDDFill(DDQuality, DDDesign, DDColor, DDshape, DDsizetype, DDshade, Convert.ToInt16(DDItemName.SelectedValue), TDQuality, TDDesign, TDColor, TDShape, TDShade);
            if (TDQuality.Visible == true)
            {
                DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
            }
            if (TDDesign.Visible == true)
            {
                DDDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
            }
            if (TDColor.Visible == true)
            {
                DDColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (TDShape.Visible == true)
            {
                DDshape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                FillSize(DDsizetype, DDshape, DDSize);
            }
            if (TDSize.Visible == true)
            {
                DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
            }
            if (TDShade.Visible == true)
            {
                DDshade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColorid"].ToString();
            }
            txtsamplecode.Text = ds.Tables[0].Rows[0]["SampleCode"].ToString();
            txtcostingDate.Text = ds.Tables[0].Rows[0]["CostingDate"].ToString();
            TxtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            TxtWeightWithoutLatex.Text = ds.Tables[0].Rows[0]["WeightWithoutLatex"].ToString();
            TxtWeightWithLatex.Text = ds.Tables[0].Rows[0]["WeightWithLatex"].ToString();
            TxtUSDVsINR.Text = ds.Tables[0].Rows[0]["USDVsINR"].ToString();
            TxtTHCPercentage.Text = ds.Tables[0].Rows[0]["THCPercentage"].ToString();
            TxtRMUPercentage.Text = ds.Tables[0].Rows[0]["RMUPercentage"].ToString();
            TxtItemInterestPercentage.Text = ds.Tables[0].Rows[0]["Interest"].ToString();
            TxtFOB.Text = ds.Tables[0].Rows[0]["FOB"].ToString();
            TxtOverHead.Text = ds.Tables[0].Rows[0]["OverHead"].ToString();
            TxtSalePrice.Text = ds.Tables[0].Rows[0]["SalePrice"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyID"].ToString();
            TxtExchangeRate.Text = ds.Tables[0].Rows[0]["ExchangeRate"].ToString();
            TxtPoNo.Text = ds.Tables[0].Rows[0]["PoNo"].ToString();
            TxtLicensePercentage.Text = ds.Tables[0].Rows[0]["licensePercentage"].ToString();
            TxtDrawbackPercentage.Text = ds.Tables[0].Rows[0]["DrawBackPercentage"].ToString();
        }

        //Select Top 1 a.CostingItemMasterID, a.ProcessID, a.SizeType, VF.CATEGORY_ID, VF.ITEM_ID, VF.QualityId, VF.designId, VF.ColorId, VF.ShapeId, VF.SizeId, VF.ShadeColorid, 
        //a.Consumption, a.Rate, a.WastagePercentage, a.ProcessRate, a.ProcessType, a.Amount


        if (ds.Tables[1].Rows.Count > 0)
        {
            DDProcessRD.SelectedValue = ds.Tables[1].Rows[0]["ProcessID"].ToString();
            DDCategoryRD.SelectedValue = ds.Tables[1].Rows[0]["CATEGORY_ID"].ToString();
            CategoryDependControls(DDCategoryRD, DDItemNameRD, TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD, TDSizeRD, TDShadeRD);
            DDItemNameRD.SelectedValue = ds.Tables[1].Rows[0]["ITEM_ID"].ToString();
            QDCSDDFill(DDQualityRD, DDDesignRD, DDColorRD, DDShapeRD, DDSizeTypeRD, DDShadeRD, Convert.ToInt16(DDItemNameRD.SelectedValue), TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD, TDShadeRD);
            if (TDQualityRD.Visible == true)
            {
                DDQualityRD.SelectedValue = ds.Tables[1].Rows[0]["QualityId"].ToString();
            }
            if (TDDesignRD.Visible == true)
            {
                DDDesignRD.SelectedValue = ds.Tables[1].Rows[0]["designId"].ToString();
            }
            if (TDColorRD.Visible == true)
            {
                DDColorRD.SelectedValue = ds.Tables[1].Rows[0]["ColorId"].ToString();
            }
            if (TDShapeRD.Visible == true)
            {
                DDShapeRD.SelectedValue = ds.Tables[1].Rows[0]["ShapeId"].ToString();
                FillSize(DDSizeTypeRD, DDShapeRD, DDSizeRD);
            }
            if (TDSizeRD.Visible == true)
            {
                DDSizeRD.SelectedValue = ds.Tables[1].Rows[0]["SizeId"].ToString();
            }
            if (TDShadeRD.Visible == true)
            {
                DDShadeRD.SelectedValue = ds.Tables[1].Rows[0]["ShadeColorid"].ToString();
            }

            txtQty.Text = ds.Tables[1].Rows[0]["Consumption"].ToString();
            txtRate.Text = ds.Tables[1].Rows[0]["Rate"].ToString();
            TxtInterestPercentage.Text = ds.Tables[1].Rows[0]["Interest"].ToString();
            TxtWastagePercentage.Text = ds.Tables[1].Rows[0]["WastagePercentage"].ToString();
            TxtDyeingRate.Text = ds.Tables[1].Rows[0]["ProcessRate"].ToString();
            TxtProcessType.Text = ds.Tables[1].Rows[0]["ProcessType"].ToString();
            txtAmount.Text = ds.Tables[1].Rows[0]["Amount"].ToString();
           // DDDyingType.SelectedValue = ds.Tables[1].Rows[0]["DyingType"].ToString();
        }
        //Select Top 1 a.CostingItemMasterID, a.ProcessID, a.UnitID, a.Rate, a.Amount, a.Remark 
        if (ds.Tables[2].Rows.Count > 0)
        {
            DDProcess.SelectedValue = ds.Tables[2].Rows[0]["ProcessID"].ToString();
            DDProcessUnit.SelectedValue = ds.Tables[2].Rows[0]["UnitID"].ToString();
            txtProcessRate.Text = ds.Tables[2].Rows[0]["Rate"].ToString();
            txtProcessAmount.Text = ds.Tables[2].Rows[0]["Amount"].ToString();
            txtprocessremark.Text = ds.Tables[2].Rows[0]["Remark"].ToString();
        }
    }
    protected void DDCategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls(DDCategoryname, DDItemName, TDQuality, TDDesign, TDColor, TDShape, TDSize, TDShade);
    }
    protected void DDCategoryRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        CategoryDependControls(DDCategoryRD, DDItemNameRD, TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD, TDSizeRD, TDShadeRD);
    }

    protected void CategoryDependControls(DropDownList ddcategoryname, DropDownList dditemName, HtmlTableCell tdquality = null, HtmlTableCell tddesign = null, HtmlTableCell tdColor = null, HtmlTableCell tdshape = null, HtmlTableCell tdsize = null, HtmlTableCell tdShade = null)
    {
        tdquality.Visible = false;
        tddesign.Visible = false;
        tdColor.Visible = false;
        tdshape.Visible = false;
        tdsize.Visible = false;
        tdShade.Visible = false;
        UtilityModule.ConditionalComboFill(ref dditemName, "Select ITEM_ID, ITEM_NAME from ITEM_MASTER(Nolock) Where CATEGORY_ID = " + ddcategoryname.SelectedValue + " Order By ITEM_NAME", true, "--Select--");
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PARAMETER_ID From ITEM_CATEGORY_PARAMETERS(Nolock) Where CATEGORY_ID = " + ddcategoryname.SelectedValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["PARAMETER_ID"]))
                {
                    case "1":
                        tdquality.Visible = true;
                        break;
                    case "2":
                        tddesign.Visible = true;
                        break;
                    case "3":
                        tdColor.Visible = true;
                        break;
                    case "4":
                        tdshape.Visible = true;
                        break;
                    case "5":
                        tdsize.Visible = true;
                        break;
                    case "6":
                        tdShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Sizetype, DropDownList ShadeColor, int Itemid, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null, System.Web.UI.HtmlControls.HtmlTableCell tdshape = null, System.Web.UI.HtmlControls.HtmlTableCell tdshadeColor = null)
    {
        if (tdQuality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref Quality, "Select QualityId, QualityName From Quality(Nolock) Where Item_Id = " + Itemid + " Order By QualityName", true, "--Select--");
        }

        string str;
        str = @"SELECT DESIGNID, DESIGNNAME from DESIGN(Nolock) Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME
            SELECT COLORID,COLORNAME FROM COLOR(Nolock) Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME
            SELECT SHAPEID,SHAPENAME FROM SHAPE(Nolock) Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By SHAPENAME
            Select SC.ShadeColorID, SC.ShadeColorName From ShadeColor SC(Nolock) Where SC.MasterCompanyId = " + Session["varCompanyId"] + @" Order By SC.ShadeColorName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (tdDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 0, true, "--Select--");
        }
        if (tdcolor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 1, true, "--Select--");
        }
        if (tdshape.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 2, true, "--Select--");
            UtilityModule.ConditionalComboFill(ref Sizetype, "select val,type from sizetype", false, "");
        }
        if (tdshadeColor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref ShadeColor, ds, 3, true, "--Select--");
        }
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQuality, DDDesign, DDColor, DDshape, DDsizetype, DDshade, Convert.ToInt16(DDItemName.SelectedValue), TDQuality, TDDesign, TDColor, TDShape, TDShade);
    }
    protected void DDItemNameRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(DDQualityRD, DDDesignRD, DDColorRD, DDShapeRD, DDSizeTypeRD, DDShadeRD, Convert.ToInt16(DDItemNameRD.SelectedValue), TDQualityRD, TDDesignRD, TDColorRD, TDShapeRD, TDShadeRD);
    }
    protected void FillSize(DropDownList SizeType, DropDownList Shape, DropDownList Size)
    {
        string size = "";
        string str = "";

        switch (SizeType.SelectedValue)
        {
            case "1":
                size = "Sizemtr";
                break;
            case "0":
                size = "Sizeft";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "Sizeft";
                break;
        }

        str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S(Nolock) 
                 Where S.shapeid=" + Shape.SelectedValue + " And S.MasterCompanyId=" + Session["varCompanyId"] + " order by " + size + "";

        UtilityModule.ConditionalComboFill(ref Size, str, true, "--Select--");
    }
    protected void DDshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void DDShapeRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDSizeTypeRD, DDShapeRD, DDSizeRD);
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDsizetype, DDshape, DDSize);
    }
    protected void DDSizeTypeRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize(DDSizeTypeRD, DDShapeRD, DDSizeRD);
    }
    protected void DDCostingFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCostingForSelectedIndexChanged();
    }
    protected void DDCostingForSelectedIndexChanged()
    {
        tdDDCustomerCode.Visible = false;
        if (DDCostingFor.SelectedIndex == 1)
        {
            tdDDCustomerCode.Visible = true;
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select C.CustomerID, C.CustomerCode From Customerinfo C(Nolock) Where MasterCompanyid = " + Session["varcompanyId"] + " Order By C.CustomerCode", true, "--Select--");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() != "16")
        {
            if (txtsamplecode.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('Please enter sample code!');", true);
                txtsamplecode.Focus();
                return;
            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
       // SqlTransaction Tran1 = con.BeginTransaction();
        try
        {
            int item_finished_id = UtilityModule.getItemFinishedId(DDItemName, DDQuality, DDDesign, DDColor, DDshape, DDSize, txtprodcode, DDshade,0, "", Convert.ToInt32(Session["varCompanyId"])); 
            string CostingItemMasterData = "";
            string CostingItemProcessDetailData = "";
            string CostingProcessRateDetailData = "";

            CostingItemMasterData = CostingItemMasterData + item_finished_id + "|" + DDsizetype.SelectedValue + "|" + DDCostingFor.SelectedValue + "|";

            if (DDCostingFor.SelectedValue == "1")
                CostingItemMasterData = CostingItemMasterData + DDCustomerCode.SelectedValue + "|";
            else
                CostingItemMasterData = CostingItemMasterData + "0|";

            string WeightWithoutLatex = TxtWeightWithoutLatex.Text == "" ? "0" : TxtWeightWithoutLatex.Text;
            string WeightWithLatex = TxtWeightWithLatex.Text == "" ? "0" : TxtWeightWithLatex.Text;
            string USDVsINR = TxtUSDVsINR.Text == "" ? "0" : TxtUSDVsINR.Text;
            string THCPercentage = TxtTHCPercentage.Text == "" ? "0" : TxtTHCPercentage.Text;
            string RMUPercentage = TxtRMUPercentage.Text == "" ? "0" : TxtRMUPercentage.Text;
            string ItemInterestPercentage = TxtItemInterestPercentage.Text == "" ? "0" : TxtItemInterestPercentage.Text;
            string FOB = TxtFOB.Text == "" ? "0" : TxtFOB.Text;
            string OverHead = TxtOverHead.Text == "" ? "0" : TxtOverHead.Text;
            string SalePrice = TxtSalePrice.Text == "" ? "0" : TxtSalePrice.Text;
            string CostingRemark = TxtCostingRemark.Text == "" ? "" : TxtCostingRemark.Text;

            string ExchangeRate = TxtExchangeRate.Text == "" ? "0" : TxtExchangeRate.Text;
            string PoNo = TxtPoNo.Text == "" ? "0" : TxtPoNo.Text;
            string licensePercentage = TxtLicensePercentage.Text == "" ? "0" : TxtLicensePercentage.Text;
            string DrawbackPercentage = TxtDrawbackPercentage.Text == "" ? "0" : TxtDrawbackPercentage.Text;

            CostingItemMasterData = CostingItemMasterData + txtsamplecode.Text + "|" + txtcostingDate.Text + "|" + TxtDescription.Text + "|" + WeightWithoutLatex + "|";
            CostingItemMasterData = CostingItemMasterData + WeightWithLatex + "|" + USDVsINR + "|" + THCPercentage + "|" + RMUPercentage + "|" + DDCompanyName.SelectedValue + "|";
            CostingItemMasterData = CostingItemMasterData + ItemInterestPercentage + "|" + FOB + "|" + OverHead + "|" + SalePrice + "|" + CostingRemark + "|";
            CostingItemMasterData = CostingItemMasterData + DDCurrency.SelectedValue + "|" + ExchangeRate + "|" + PoNo + "|" + licensePercentage + "|" + DrawbackPercentage + "~";

            item_finished_id = 0;
            item_finished_id = UtilityModule.getItemFinishedId(DDItemNameRD, DDQualityRD, DDDesignRD, DDColorRD, DDShapeRD, DDSizeRD, txtItemCodeRD, DDShadeRD,0, "", Convert.ToInt32(Session["varCompanyId"]));

            string InterestPercentage = TxtInterestPercentage.Text == "" ? "0" : TxtInterestPercentage.Text;

            string ProcessType = "";
            string DyingType = "0";
            string ProcessPMD = "0";
            string ProcessUnitPMD = "0";
            if (TDTxtDyingType.Visible == true)
            {
                ProcessType = TxtProcessType.Text;
            }
            if (TDDDDyingType.Visible == true)
            {
                DyingType = DDDyingType.SelectedValue;
            }

            if (TDProcessPMD.Visible == true)
            {
                if (DDProcessPMD.SelectedIndex > 0)
                {
                    ProcessPMD = DDProcessPMD.SelectedValue;
                }
            }

            if (TDPUnitPMD.Visible == true)
            {
                if (DDProcessUnitPMD.SelectedIndex > 0)
                {
                    ProcessUnitPMD = DDProcessUnitPMD.SelectedValue;
                }
            }

            CostingItemProcessDetailData = DDProcessRD.SelectedValue + "|" + item_finished_id + "|" + DDSizeTypeRD.SelectedValue + "|" + txtQty.Text + "|" + txtRate.Text + "|" + TxtWastagePercentage.Text + "|";
            CostingItemProcessDetailData = CostingItemProcessDetailData + TxtDyeingRate.Text + "|" + ProcessType + "|" + txtAmount.Text + "|" + InterestPercentage + "|";
            CostingItemProcessDetailData = CostingItemProcessDetailData + DyingType + "|" + ProcessPMD + "|" + ProcessUnitPMD + "~";

            CostingProcessRateDetailData = DDProcess.SelectedValue + "|" + DDProcessUnit.SelectedValue + "|" + txtProcessRate.Text + "|";

            if (TDProcessAmount.Visible == true)
            {
                CostingProcessRateDetailData = CostingProcessRateDetailData + txtProcessAmount.Text + "|";
            }
            else
                CostingProcessRateDetailData = CostingProcessRateDetailData + "0|";

            CostingProcessRateDetailData = CostingProcessRateDetailData + txtprocessremark.Text + "|0~";

            if (TDProcessPMD.Visible == true)
            {
                if (DDProcessUnitPMD.SelectedIndex > 0)
                {
                    string AmountPMD = "0";

                    AmountPMD = (Convert.ToDouble(txtProcessRatePMD.Text == "" ? "0" : txtProcessRatePMD.Text) * Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text)).ToString();
                    CostingProcessRateDetailData = CostingProcessRateDetailData + DDProcessPMD.SelectedValue + "|" + DDProcessUnitPMD.SelectedValue + "|" + txtProcessRatePMD.Text + "|" + AmountPMD + "||" + item_finished_id + "~";
                }
            }

            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@CostingItemMasterID", SqlDbType.Int);
            param[1] = new SqlParameter("@CostingItemMasterData", SqlDbType.VarChar, 4000);
            param[2] = new SqlParameter("@CostingItemProcessDetailData", SqlDbType.VarChar, 4000);
            param[3] = new SqlParameter("@CostingProcessRateDetailData", SqlDbType.VarChar, 4000);
            param[4] = new SqlParameter("@UserID", SqlDbType.Int);
            param[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            param[6] = new SqlParameter("@Message", SqlDbType.VarChar, 100);
            param[7] = new SqlParameter("@SampleCodeReturn", SqlDbType.VarChar, 100);

            param[0].Direction = ParameterDirection.InputOutput;
            param[0].Value = ViewState["CostingItemMasterID"];
            param[1].Value = CostingItemMasterData;
            param[2].Value = CostingItemProcessDetailData;
            param[3].Value = CostingProcessRateDetailData;
            param[4].Value = Session["varuserid"];
            param[5].Value = Session["varcompanyId"];
            param[6].Direction = ParameterDirection.Output;
            param[7].Direction = ParameterDirection.InputOutput;
            param[7].Value = txtsamplecode.Text;

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_CostingSaveData", param);
            ViewState["CostingItemMasterID"] = param[0].Value;
            //**************Message
            if (param[6].Value.ToString() != "")
            {
                lblmessage.Text = param[6].Value.ToString();
                txtsamplecode.Text = param[7].Value.ToString();
            }

          //  Tran1.Commit();
            btnAddImage.Visible = true;
            FillGrid();
            //Refreshcontrol();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
           // Tran1.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Refreshcontrol()
    {
        DDCategoryRD.SelectedIndex = -1;
        DDItemNameRD.SelectedIndex = -1;
        DDQualityRD.SelectedIndex = -1;
        DDDesignRD.SelectedIndex = -1;
        DDColorRD.SelectedIndex = -1;
        DDShapeRD.SelectedIndex = -1;
        DDSizeRD.SelectedIndex = -1;
        DDShadeRD.SelectedIndex = -1;
        txtQty.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        DDProcess.SelectedIndex = -1;
        txtProcessRate.Text = "";
        //txtProcessQty.Text = "";
        txtProcessAmount.Text = "";
        txtprocessremark.Text = "";
    }
    protected void FillGrid()
    {
        string str = @"Select a.CostingItemProcessDetailID, a.CostingItemMasterID, PNM.PROCESS_NAME ProcessName, 
            VF.CATEGORY_NAME + ' ' + VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
            (Case When a.SizeType = 0 Then VF.SizeFt When a.SizeType = 1 Then VF.SizeMtr When a.SizeType = 2 Then VF.SizeInch Else '' End) + ' ' + VF.ShadeColorName ItemDetail,  
            a.Consumption, a.Rate, a.WastagePercentage, a.ProcessRate, a.ProcessType, a.Amount 
            From CostingItemProcessDetail a(nolock) 
            JOIN V_FinishedItemDetail VF(nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
            JOIN PROCESS_NAME_MASTER PNM(nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            Where CostingItemMasterID = " + ViewState["CostingItemMasterID"] + @" 
            Select a.CostingProcessRateDetailID, a.CostingItemMasterID, PNM.PROCESS_NAME ProcessName, U.UnitName, a.Rate, a.Amount, a.Remark 
            From CostingProcessRateDetail a(Nolock) 
            JOIN PROCESS_NAME_MASTER PNM(nolock) ON PNM.PROCESS_NAME_ID = a.ProcessID 
            JOIN Unit U(Nolock) ON U.UnitId = a.UnitID 
            Where a.CostingItemMasterID = " + ViewState["CostingItemMasterID"];

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
        GVProcessDetail.DataSource = ds.Tables[1];
        GVProcessDetail.DataBind();
    }
    protected void btnshowdetail_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GVDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CostingItemMasterID", SqlDbType.Int);
            param[1] = new SqlParameter("@DetailType", SqlDbType.TinyInt);      // For 0 ItemDetailTable(CostingItemProcessDetail), For 1 ProcessDetailTable(CostingProcessRateDetail) 
            param[2] = new SqlParameter("@TableDetailID", SqlDbType.Int);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4] = new SqlParameter("@Imgdelflag", SqlDbType.TinyInt);      //0 for No 1 for yes
            //*************

            param[0].Value = ViewState["CostingItemMasterID"];
            param[1].Value = 0;
            param[2].Value = Convert.ToInt32(GVDetail.DataKeys[e.RowIndex].Value);
            param[3].Direction = ParameterDirection.Output;
            param[4].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CostingDeleteData", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            //************imgdele
            if (param[4].Value.ToString() == "1")
            {
                //if (File.Exists(Server.MapPath("~/CostingImage/" + lblcostingid.Text)))
                {
                    //File.Delete(Server.MapPath("~/CostingImage/" + lblcostingid.Text));
                }
            }
            //**************

            FillGrid();
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
    protected void GVProcessDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CostingItemMasterID", SqlDbType.Int);
            param[1] = new SqlParameter("@DetailType", SqlDbType.TinyInt);      // For 0 ItemDetailTable(CostingItemProcessDetail), For 1 ProcessDetailTable(CostingProcessRateDetail) 
            param[2] = new SqlParameter("@TableDetailID", SqlDbType.Int);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4] = new SqlParameter("@Imgdelflag", SqlDbType.TinyInt);      //0 for No 1 for yes
            //*************

            param[0].Value = ViewState["CostingItemMasterID"];
            param[1].Value = 1;
            param[2].Value = Convert.ToInt32(GVProcessDetail.DataKeys[e.RowIndex].Value);
            param[3].Direction = ParameterDirection.Output;
            param[4].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_CostingDeleteData", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            //************imgdele
            if (param[4].Value.ToString() == "1")
            {
                //if (File.Exists(Server.MapPath("~/CostingImage/" + lblcostingid.Text)))
                {
                    //File.Delete(Server.MapPath("~/CostingImage/" + lblcostingid.Text));
                }
            }
            //**************

            FillGrid();
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
    protected void btnAddImage_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["CostingItemMasterID"]) > 0)
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../Carpet/AddPhotoRefImage1.aspx?SrNo=" + ViewState["CostingItemMasterID"] + "&img=FrmCostingMaster&PPI=yes', 'nwwin', 'toolbar=0, titlebar=1,  top=200px, left=100px, scrollbars=1, resizable = yes,width=550px,Height=200px');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "16")
        {
            PreviewForChampoClick();
        }
        else
        {
            PreviewClick();
        }
    }
    protected void PreviewClick()
    {
        lblmessage.Text = "";
        int Row;
        DataSet DS = new DataSet();
        String sQry = " ";
        string shadecolor = "";
        string FilterBy = "Filter By ";
        try
        {
            sQry = @"Select a.CostingItemMasterID, CI.CompanyName, CI.CompAddr1 + ' ' + CI.CompAddr2 + ' ' + CI.CompAddr3 + ' ' + CI.CompTel CompanyAddress, a.SampleCode, CII.CustomerCode, 
                    ST.[Type], Case When a.SizeType = 1 Then 'Sq Mtr' Else 'Sq ' + ST.Type End AreaType, a.Description, 
                    VF.CATEGORY_NAME, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.ShadeColorName ItemDescription, 
                    Case When a.SizeType = 0 Then SizeFt Else Case When a.SizeType = 1 Then VF.SizeMtr Else Case When a.SizeType = 2 Then VF.SizeInch End End End Size, 
                    Case When a.SizeType = 0 Then AreaFt Else Case When a.SizeType = 1 Then VF.AreaMtr Else Case When a.SizeType = 2 Then VF.AreaInch End End End Area, 
                    a.WeightWithoutLatex, a.WeightWithLatex,case when isnull(a.WeightWithoutLatex,0)=0 then 0 else Round(a.WeightWithoutLatex / AreaMtr, 3) end as QualityWithOutLatexKGPerSqMtr, case when isnull(a.WeightWithLatex,0)=0 then 0 else Round(a.WeightWithLatex / AreaMtr, 3) end as  QualityWithLatexLatexKGPerSqMtr, 
                    case when isnull(a.WeightWithoutLatex,0)=0 then 0 else Round(a.WeightWithoutLatex / AreaFt, 3) end as QualityWithOutLatexKGPerSqFt,case when isnull(a.WeightWithoutLatex,0)=0 then 0 else Round(a.WeightWithLatex / AreaFt, 3) end as QualityWithLatexLatexKGPerSqFt, 
                    a.USDVsINR, a.THCPercentage, a.RMUPercentage, a.TotalRMUPercentage, a.PcsAmount - a.TotalAmount TotalRMUVal, a.TotalAmount, a.PcsAmount, 
                    Round(a.PcsAmount / VF.AreaMtr, 2) [PriceFOBPerMt2INRs],case when isnull(a.USDVsINR,0)=0 then 0 else Round(a.PcsAmount / a.USDVsINR, 2) end as PriceFOBPerPieceUSD, 
                    case when isnull(a.USDVsINR,0)=0 then 0 else Round(a.PcsAmount / (VF.AreaMtr * a.USDVsINR), 2) end as PriceFOBPerMt2USD,case when isnull(a.USDVsINR,0)=0 then 0 else Round(a.PcsAmount / (VF.AreaFt * a.USDVsINR), 2) end as  PriceFOBPerFt2USD 
                    From CostingItemMaster a(Nolock) 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyID = a.CompanyID 
                    JOIN SizeType ST(Nolock) ON ST.Val = a.SizeType 
                    LEFT JOIN CustomerInfo CII(Nolock) ON CII.CustomerId = a.CustomerID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                    Where SampleCode = '" + txtsamplecode.Text + @"' 
                    Order By a.CostingItemMasterID ";

            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            if (DS.Tables[0].Rows.Count > 0)
            {
                string Path = "";
                FilterBy = FilterBy + "Sample Code - " + DS.Tables[0].Rows[0]["SampleCode"];

                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("CostSheet");
                //*************
                sht.Range("A1:E1").Merge();
                sht.Range("A1:E1").Style.Font.FontSize = 11;
                sht.Range("A1:E1").Style.Font.Bold = true;
                sht.Range("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:E1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A1").SetValue("Cost Sheet(" + DS.Tables[0].Rows[0]["CATEGORY_NAME"] + ")");
                sht.Range("A1:E1").Style.Font.FontColor = XLColor.Blue;
                sht.Row(1).Height = 21.75;
                //
                sht.Range("A2:E2").Merge();
                sht.Range("A2:E2").Style.Font.FontSize = 11;
                sht.Range("A2:E2").Style.Font.Bold = true;
                sht.Range("A2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:E2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A2").SetValue(FilterBy.TrimStart(','));
                sht.Row(2).Height = 21.75;
                //Company Name

                sht.Range("A3:E3").Merge();
                sht.Range("A3:E3").Style.Font.FontSize = 11;
                sht.Range("A3:E3").Style.Font.Bold = true;
                sht.Range("A3:E3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A3:E3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A3").SetValue(DS.Tables[0].Rows[0]["CompanyName"]);
                sht.Row(1).Height = 21.75;
                sht.Range("A3:E3").Style.Font.FontColor = XLColor.Red;
                sht.Range("A3:E3").Style.Fill.BackgroundColor = XLColor.LightGray;
                //
                sht.Range("A4:E4").Merge();
                sht.Range("A4:E4").Style.Font.FontSize = 9;
                sht.Range("A4:E4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A4:E4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A4").SetValue(DS.Tables[0].Rows[0]["CompanyAddress"]);
                sht.Row(2).Height = 21.75;
                sht.Column(1).Width = 45;
                sht.Column(2).Width = 15;
                sht.Column(3).Width = 4;
                sht.Column(4).Width = 2;
                sht.Column(5).Width = 4;

                Row = 5;

                for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    sht.Range("A" + Row).SetValue("Style #");
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 14;
                    //sht.Range("B" + Row + ":E" + Row).Merge();
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["SampleCode"]);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;

                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Buyer Reference -");
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    //sht.Range("B" + Row + ":E" + Row).Merge();
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["CustomerCode"]);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Description -");
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row + ":E" + Row).Merge();
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["Description"]);
                    //sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Size in " + DS.Tables[0].Rows[i]["Type"] + " -");
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + Row + ":E" + Row).Merge();
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[i]["Size"]);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Area in " + DS.Tables[0].Rows[i]["AreaType"] + " -");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + Row + ":E" + Row).Merge();
                    sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[i]["Area"]);
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Weight of Pcs In Kg");
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Font.Bold = true;

                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //sht.Range("A" + Row + ":E" + Row).Merge();
                    //sht.Range("A" + Row + ":E" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("(Without Latex & Primary Back) -");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["WeightWithoutLatex"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("(With Latex & Back / Finished) -");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["WeightWithLatex"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row + ":G" + Row).Merge();
                    sht.Range("F" + Row + ":G" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Quality -");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    //sht.Range("A" + Row + ":E" + Row).Merge();
                    //sht.Range("A" + Row + ":E" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("(Without Latex & Primary Back) - Kg/Sq.Mtr");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["QualityWithOutLatexKGPerSqMtr"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    sht.Range("C" + Row + ":E" + Row).Merge();
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[i]["QualityWithOutLatexKGPerSqFt"] + "Kg/Sq.Ft");
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("(With Latex & Back / Finished) - Kg/Sq.Mtr ");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;

                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["QualityWithLatexLatexKGPerSqMtr"]);
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;

                    sht.Range("C" + Row + ":E" + Row).Merge();
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    sht.Range("C" + Row + ":E" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[i]["QualityWithLatexLatexKGPerSqFt"] + "Kg/Sq.Ft");
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("(US$ Vs. INRs. - ) -");
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["USDVsINR"]);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;

                    Row = Row + 1;

                    DataSet dsDetail = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select a.CostingItemMasterID, PNM.PROCESS_NAME, ST.Type SizeType, 
                    VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + 
                    Case When a.SizeType = 0 Then SizeFt Else Case When a.SizeType = 1 Then VF.SizeMtr Else Case When a.SizeType = 2 Then VF.SizeInch End End End + ' ' + VF.ShadeColorName ItemDescription, 
                    case when isnull(a.WeightWithoutLatex,0)=0  then 0 else Round(b.Consumption / a.WeightWithoutLatex * 100, 2) end as ConsumptioninPercentage, b.Consumption, b.Rate, b.WastagePercentage, b.ProcessRate, b.ProcessType, b.Amount 
                    From CostingItemMaster a(Nolock) 
                    JOIN CostingItemProcessDetail b(Nolock) ON b.CostingItemMasterID = a.CostingItemMasterID 
                    JOIN SizeType ST(Nolock) ON ST.Val = b.SizeType 
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = b.ProcessID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                    Where a.SampleCode = '" + txtsamplecode.Text + "' And a.CostingItemMasterID = " + DS.Tables[0].Rows[i]["CostingItemMasterID"] + @" 
                    Order By b.CostingItemProcessDetailID 

                    Select a.CostingItemMasterID, PNM.PROCESS_NAME + Case When IsNull(VF.ITEM_NAME, '') = '' Then '' Else ' (' + VF.ITEM_NAME + ' ' + VF.QualityName + ')' End PROCESS_NAME, 
                    U.UnitName, b.Rate, Sum(b.Amount) Amount, b.Remark                     
                    From CostingItemMaster a(Nolock) 
                    JOIN CostingProcessRateDetail b(Nolock) ON b.CostingItemMasterID = a.CostingItemMasterID 
                    LEFT JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.CostingItemProcessDetailFinishedID 
                    JOIN PROCESS_NAME_MASTER PNM(Nolock) ON PNM.PROCESS_NAME_ID = b.ProcessID 
                    JOIN Unit U(Nolock) ON U.UnitId = b.UnitID 
                    Where a.SampleCode = '" + txtsamplecode.Text + "' And a.CostingItemMasterID = " + DS.Tables[0].Rows[i]["CostingItemMasterID"] + @" 
                    Group By a.CostingItemMasterID, PNM.PROCESS_NAME, U.UnitName, b.Rate, b.Remark, VF.ITEM_NAME, VF.QualityName ");

                    for (int j = 0; j < dsDetail.Tables[0].Rows.Count; j++)
                    {
                        sht.Range("A" + Row).SetValue(dsDetail.Tables[0].Rows[j]["PROCESS_NAME"]);
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ItemDescription"]);
                        sht.Range("B" + Row + ":E" + Row).Merge();
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Consumption in Percent -");
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row + ":B" + Row).Style.Font.FontColor = XLColor.Yellow;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ConsumptioninPercentage"]);
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Consumption in Kg -");
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Consumption"]);
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Price @ INRs./Kgs");
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Rate"]);
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Wastage % @");
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["WastagePercentage"]);
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Dying @ INRs./Kgs");
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ProcessRate"]);
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;

                        sht.Range("C" + Row + ":D" + Row).Merge();
                        sht.Range("C" + Row).Style.Font.Bold = true;
                        sht.Range("C" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("C" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ProcessType"]);
                        Row = Row + 1;

                        sht.Range("A" + Row).SetValue("Value in INRs.");
                        sht.Range("A" + Row).Style.Font.Bold = true;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.DarkBlue;
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        sht.Range("F" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Amount"]);
                        sht.Range("F" + Row).Style.Font.FontSize = 11;
                        sht.Range("F" + Row).Style.Font.Bold = true;
                        sht.Range("F" + Row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        sht.Range("F" + Row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        Row = Row + 1;
                    }
                    Row = Row + 1;
                    for (int k = 0; k < dsDetail.Tables[1].Rows.Count; k++)
                    {
                        sht.Range("A" + Row).SetValue(dsDetail.Tables[1].Rows[k]["PROCESS_NAME"] + " @ INR / " + dsDetail.Tables[1].Rows[k]["UnitName"]);
                        sht.Range("A" + Row + ":E" + Row).Style.Font.FontSize = 11;
                        sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                        sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                        sht.Range("B" + Row).SetValue(dsDetail.Tables[1].Rows[k]["Rate"]);
                        sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                        sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                        sht.Range("F" + Row).SetValue(dsDetail.Tables[1].Rows[k]["Amount"]);
                        //sht.Range("F" + Row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        //sht.Range("F" + Row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        Row = Row + 1;
                    }

                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Total -");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row).Style.Font.FontSize = 11;
                    //sht.Range("B" + Row + ":E" + Row).Merge();
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["TotalAmount"]);
                    sht.Range("F" + Row).Style.Font.Bold = true;
                    sht.Range("F" + Row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("F" + Row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("THC %");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["THCPercentage"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("RMU % @");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["RMUPercentage"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Total RMU %");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[i]["TotalRMUPercentage"]);
                    sht.Range("B" + Row).Style.Font.FontColor = XLColor.Red;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["TotalRMUVal"]);
                    sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Grand Total ( Price/Pcs INRs.)");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["PcsAmount"]);
                    sht.Range("F" + Row).Style.Font.Bold = true;
                    sht.Range("F" + Row).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    sht.Range("F" + Row).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Price FOB / Mt2 INRs.");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["PriceFOBPerMt2INRs"]);
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Price FOB / Piece US$");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["PriceFOBPerPieceUSD"]);

                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Price FOB / Mt2 US$");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["PriceFOBPerMt2USD"]);
                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Price FOB / Ft2 US$");
                    sht.Range("A" + Row).Style.Font.FontColor = XLColor.Blue;
                    sht.Range("A" + Row).Style.Font.Bold = true;
                    sht.Range("A" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sht.Range("A" + Row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    sht.Range("B" + Row + ":E" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).Style.Fill.BackgroundColor = XLColor.Gray;
                    sht.Range("F" + Row).SetValue(DS.Tables[0].Rows[i]["PriceFOBPerFt2USD"]);
                    Row = Row + 1;

                    Row = Row + 5;

                }

                //******SAVE FILE
                sht.Columns(1, 8).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("CostSheet(" + shadecolor + ")" + DateTime.Now + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void PreviewForChampoClick()
    {
        lblmessage.Text = "";
        int Row;
        DataSet DS = new DataSet();
        String sQry = "";
        try
        {
            sQry = @"Select a.CostingItemMasterID, CI.CompanyName, CI.CompAddr1 + ' ' + CI.CompAddr2 + ' ' + CI.CompAddr3 + ' ' + CI.CompTel CompanyAddress, a.SampleCode, 
                    IsNull(CII.CustomerCode, 'Sample') CustomerCode, REPLACE(CONVERT(NVARCHAR(11), CostingDate, 106), ' ', '-') CostingDate, ST.[Type], 
                    Case When a.SizeType = 1 Then 'Sq Mtr' Else 'Sq Yrd' End AreaType, a.Description, 
                    VF.CATEGORY_NAME, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.ShadeColorName ItemDescription, 
                    Case When a.SizeType = 0 Then SizeFt Else Case When a.SizeType = 1 Then VF.SizeMtr Else Case When a.SizeType = 2 Then VF.SizeInch End End End Size, 
                    Case When a.SizeType = 0 Then AreaFt Else Case When a.SizeType = 1 Then VF.AreaMtr Else Case When a.SizeType = 2 Then VF.AreaInch End End End Area, 
                    a.WeightWithoutLatex, a.WeightWithLatex, Round(a.WeightWithoutLatex / AreaMtr, 3) QualityWithOutLatexKGPerSqMtr, Round(a.WeightWithLatex / AreaMtr, 3) QualityWithLatexLatexKGPerSqMtr, 
                    Round(a.WeightWithoutLatex / AreaFt, 3) QualityWithOutLatexKGPerSqFt, Round(a.WeightWithLatex / AreaFt, 3) QualityWithLatexLatexKGPerSqFt, 
                    a.USDVsINR, a.THCPercentage, a.RMUPercentage, a.TotalRMUPercentage, a.PcsAmount - a.TotalAmount TotalRMUVal, a.TotalAmount, a.PcsAmount, --Round(a.PcsAmount / VF.AreaMtr, 2) 
                    '' [PriceFOBPerMt2INRs], --Round(a.PcsAmount / a.USDVsINR, 2) 
                    '' PriceFOBPerPieceUSD, --Round(a.PcsAmount / (VF.AreaMtr * a.USDVsINR), 2) 
                    '' PriceFOBPerMt2USD, --Round(a.PcsAmount / (VF.AreaFt * a.USDVsINR), 2) 
                    '' PriceFOBPerFt2USD, a.Interest, a.FOB, a.OverHead, a.SalePrice, a.CostingRemark, CU.CurrencyName, a.ExchangeRate, a.PoNo, a.licensePercentage, 
                    a.DrawBackPercentage, Round(a.SalePrice / a.ExchangeRate, 2) CostPricePerCurrency, Round(a.SalePrice * a.licensePercentage * 0.01, 2) licenseAmount, 
                    Round(a.SalePrice * a.DrawBackPercentage * 0.01, 2) DrawBackAmount 
                    From CostingItemMaster a(Nolock) 
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyID = a.CompanyID 
                    JOIN SizeType ST(Nolock) ON ST.Val = a.SizeType 
                    LEFT JOIN CustomerInfo CII(Nolock) ON CII.CustomerId = a.CustomerID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_ID 
                    LEFT JOIN CurrencyInfo CU(Nolock) ON CU.CurrencyId = a.CurrencyId 
                    Where SampleCode = '" + txtsamplecode.Text + @"' 
                    Order By a.CostingItemMasterID ";

            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sQry);
            if (DS.Tables[0].Rows.Count > 0)
            {
                string Path = "";
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("CostSheet");
                //*************

                sht.Range("A1").SetValue("Date :");
                sht.Range("A1").Style.Font.FontSize = 11;
                //sht.Range("A1").Style.Font.Bold = true;
                sht.Range("B1").SetValue(DS.Tables[0].Rows[0]["CostingDate"]);
                sht.Range("B1").Style.Font.FontSize = 11;

                sht.Range("C1").SetValue("Costing Code :");
                sht.Range("C1").Style.Font.FontSize = 11;
                //sht.Range("A1").Style.Font.Bold = true;
                sht.Range("D1").SetValue(DS.Tables[0].Rows[0]["SampleCode"]);
                sht.Range("D1").Style.Font.FontSize = 11;

                sht.Range("E1").SetValue("Production :");
                sht.Range("E1").Style.Font.FontSize = 11;

                sht.Range("A2").SetValue("Buyer :");
                sht.Range("A2").Style.Font.FontSize = 11;
                sht.Range("B2").SetValue(DS.Tables[0].Rows[0]["CustomerCode"]);
                sht.Range("B2").Style.Font.FontSize = 11;

                sht.Range("C2").SetValue("Costing Remark :");
                sht.Range("C2").Style.Font.FontSize = 11;
                sht.Range("D2").SetValue(DS.Tables[0].Rows[0]["CostingRemark"]);
                sht.Range("D2").Style.Font.FontSize = 11;

                sht.Range("E2").SetValue("Purchase :");
                sht.Range("E2").Style.Font.FontSize = 11;

                sht.Range("A3").SetValue("Design :");
                sht.Range("A3").Style.Font.FontSize = 11;
                sht.Range("B3").SetValue(DS.Tables[0].Rows[0]["DesignName"]);
                sht.Range("B3").Style.Font.FontSize = 11;

                sht.Range("A4").SetValue("Design No :");
                sht.Range("A4").Style.Font.FontSize = 11;

                sht.Range("C4").SetValue("Sale Price :");
                sht.Range("C4").Style.Font.FontSize = 11;
                sht.Range("D4").SetValue(DS.Tables[0].Rows[0]["SalePrice"]);
                sht.Range("D4").Style.Font.FontSize = 11;
                
                sht.Range("E4").SetValue(DS.Tables[0].Rows[0]["AreaType"].ToString() + "(" + DS.Tables[0].Rows[0]["CurrencyName"].ToString() + ")");
                sht.Range("E4").Style.Font.FontSize = 11;


                sht.Range("A5").SetValue("Product Discription :");
                sht.Range("A5").Style.Font.FontSize = 11;
                sht.Range("B5").SetValue(DS.Tables[0].Rows[0]["Description"]);
                sht.Range("B5").Style.Font.FontSize = 11;

                sht.Range("A6").SetValue("Quality :");
                sht.Range("A6").Style.Font.FontSize = 11;
                sht.Range("B6").SetValue(DS.Tables[0].Rows[0]["QualityName"]);
                sht.Range("B6").Style.Font.FontSize = 11;

                Row = 7;

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SampleCode", txtsamplecode.Text);
                param[1] = new SqlParameter("@CostingItemMasterID", DS.Tables[0].Rows[0]["CostingItemMasterID"]);

                DataSet dsDetail = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETDATA_CostingItem_Process_Rate_Detail", param);

                for (int j = 0; j < dsDetail.Tables[0].Rows.Count; j++)
                {
                    sht.Range("C" + Row).SetValue("Rate");
                    sht.Range("C" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    Row = Row + 1;

                    sht.Range("A" + Row).SetValue("Material " + (j + 1));
                    sht.Range("A" + Row).Style.Font.FontSize = 11;
                    sht.Range("A" + (Row + 1)).SetValue("Dye Type");
                    sht.Range("A" + ((Row + 1))).Style.Font.FontSize = 11;
                    sht.Range("A" + (Row + 2)).SetValue("Loss %");
                    sht.Range("A" + (Row + 2)).Style.Font.FontSize = 11;
                    sht.Range("A" + (Row + 3)).SetValue("Interest %");
                    sht.Range("A" + (Row + 3)).Style.Font.FontSize = 11;
                    sht.Range("A" + (Row + 4)).SetValue("Total Cost");
                    sht.Range("A" + (Row + 4)).Style.Font.FontSize = 11;

                    sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ItemQuality"]);
                    sht.Range("B" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessType"]);
                    sht.Range("B" + (Row + 1)).Style.Font.FontSize = 11;
                    sht.Range("B" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["WastagePercentage"]);
                    sht.Range("B" + (Row + 2)).Style.Font.FontSize = 11;
                    sht.Range("B" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestPercentage"]);
                    sht.Range("B" + (Row + 3)).Style.Font.FontSize = 11;

                    sht.Range("C" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Rate"]);
                    sht.Range("C" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessRate"]);
                    sht.Range("C" + (Row + 1)).Style.Font.FontSize = 11;
                    sht.Range("C" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["LossAmt"]);
                    sht.Range("C" + (Row + 2)).Style.Font.FontSize = 11;
                    sht.Range("C" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestAmt"]);
                    sht.Range("C" + (Row + 3)).Style.Font.FontSize = 11;
                    sht.Range("C" + (Row + 4)).SetValue(dsDetail.Tables[0].Rows[j]["MaterialAmt"]);
                    sht.Range("C" + (Row + 4)).Style.Font.FontSize = 11;

                    j = j + 1;
                    if (j < dsDetail.Tables[0].Rows.Count)
                    {
                        sht.Range("F" + (Row - 1)).SetValue("Rate");
                        sht.Range("F" + (Row - 1)).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("F" + (Row - 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        sht.Range("D" + Row).SetValue("Material " + (j + 1));
                        sht.Range("D" + Row).Style.Font.FontSize = 11;
                        sht.Range("D" + (Row + 1)).SetValue("Dye Type");
                        sht.Range("D" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("D" + (Row + 2)).SetValue("Loss %");
                        sht.Range("D" + (Row + 2)).Style.Font.FontSize = 11;
                        sht.Range("D" + (Row + 3)).SetValue("Interest %");
                        sht.Range("D" + (Row + 3)).Style.Font.FontSize = 11;
                        sht.Range("D" + (Row + 4)).SetValue("Total Cost");
                        sht.Range("D" + (Row + 4)).Style.Font.FontSize = 11;

                        sht.Range("E" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ItemQuality"]);
                        sht.Range("E" + Row).Style.Font.FontSize = 11;
                        sht.Range("E" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessType"]);
                        sht.Range("E" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("E" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["WastagePercentage"]);
                        sht.Range("E" + (Row + 2)).Style.Font.FontSize = 11;
                        sht.Range("E" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestPercentage"]);
                        sht.Range("E" + (Row + 3)).Style.Font.FontSize = 11;

                        sht.Range("F" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Rate"]);
                        sht.Range("F" + Row).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessRate"]);
                        sht.Range("F" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["LossAmt"]);
                        sht.Range("F" + (Row + 2)).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestAmt"]);
                        sht.Range("F" + (Row + 3)).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row + 4)).SetValue(dsDetail.Tables[0].Rows[j]["MaterialAmt"]);
                        sht.Range("F" + (Row + 4)).Style.Font.FontSize = 11;
                    }

                    j = j + 1;
                    if (j < dsDetail.Tables[0].Rows.Count)
                    {
                        sht.Range("F" + (Row - 1)).SetValue("Rate");
                        sht.Range("F" + (Row - 1)).Style.Font.FontSize = 11;
                        sht.Range("F" + (Row - 1)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        sht.Range("F" + (Row - 1)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        sht.Range("G" + Row).SetValue("Material " + (j + 1));
                        sht.Range("G" + Row).Style.Font.FontSize = 11;
                        sht.Range("G" + (Row + 1)).SetValue("Dye Type");
                        sht.Range("G" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("G" + (Row + 2)).SetValue("Loss %");
                        sht.Range("G" + (Row + 2)).Style.Font.FontSize = 11;
                        sht.Range("G" + (Row + 3)).SetValue("Interest %");
                        sht.Range("G" + (Row + 3)).Style.Font.FontSize = 11;
                        sht.Range("G" + (Row + 4)).SetValue("Total Cost");
                        sht.Range("G" + (Row + 4)).Style.Font.FontSize = 11;

                        sht.Range("H" + Row).SetValue(dsDetail.Tables[0].Rows[j]["ItemQuality"]);
                        sht.Range("H" + Row).Style.Font.FontSize = 11;
                        sht.Range("H" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessType"]);
                        sht.Range("H" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("H" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["WastagePercentage"]);
                        sht.Range("H" + (Row + 2)).Style.Font.FontSize = 11;
                        sht.Range("H" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestPercentage"]);
                        sht.Range("H" + (Row + 3)).Style.Font.FontSize = 11;

                        sht.Range("I" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Rate"]);
                        sht.Range("I" + Row).Style.Font.FontSize = 11;
                        sht.Range("I" + (Row + 1)).SetValue(dsDetail.Tables[0].Rows[j]["ProcessRate"]);
                        sht.Range("I" + (Row + 1)).Style.Font.FontSize = 11;
                        sht.Range("I" + (Row + 2)).SetValue(dsDetail.Tables[0].Rows[j]["LossAmt"]);
                        sht.Range("I" + Row + 2).Style.Font.FontSize = 11;
                        sht.Range("I" + (Row + 3)).SetValue(dsDetail.Tables[0].Rows[j]["InterestAmt"]);
                        sht.Range("I" + (Row + 3)).Style.Font.FontSize = 11;
                        sht.Range("I" + (Row + 4)).SetValue(dsDetail.Tables[0].Rows[j]["MaterialAmt"]);
                        sht.Range("I" + (Row + 4)).Style.Font.FontSize = 11;
                    }
                    Row = Row + 6;
                }

                sht.Range("A" + Row).SetValue("Consumption");
                sht.Range("A" + Row).Style.Font.FontSize = 11;

                sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[0]["AreaType"]);
                sht.Range("B" + Row).Style.Font.FontSize = 11;
                sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("B" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("C" + Row).SetValue("Sum");
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                Row = Row + 1;
                Double TotalAmount = 0;
                for (int j = 0; j < dsDetail.Tables[0].Rows.Count; j++)
                {
                    sht.Range("A" + Row).SetValue("Material " + (j + 1));
                    sht.Range("A" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Consumption"]);
                    sht.Range("B" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + Row).SetValue(dsDetail.Tables[0].Rows[j]["Amount"]);
                    sht.Range("C" + Row).Style.Font.FontSize = 11;
                    TotalAmount = TotalAmount + Convert.ToDouble(dsDetail.Tables[0].Rows[j]["Amount"]);

                    Row = Row + 1;
                }

                for (int j = 0; j < dsDetail.Tables[1].Rows.Count; j++)
                {
                    sht.Range("A" + Row).SetValue(dsDetail.Tables[1].Rows[j]["PROCESS_NAME"]);
                    sht.Range("A" + Row).Style.Font.FontSize = 11;
                    sht.Range("B" + Row).SetValue(dsDetail.Tables[1].Rows[j]["Rate"]);
                    sht.Range("B" + Row).Style.Font.FontSize = 11;
                    sht.Range("C" + Row).SetValue(dsDetail.Tables[1].Rows[j]["Amount"]);
                    sht.Range("C" + Row).Style.Font.FontSize = 11;
                    TotalAmount = TotalAmount + Convert.ToDouble(dsDetail.Tables[1].Rows[j]["Amount"]);
                    Row = Row + 1;
                }
                TotalAmount = Math.Round(TotalAmount + Convert.ToDouble(DS.Tables[0].Rows[0]["FOB"]) + Convert.ToDouble(DS.Tables[0].Rows[0]["OverHead"]), 2);

                Double TotalInterest = Math.Round(TotalAmount * Convert.ToDouble(DS.Tables[0].Rows[0]["Interest"]) * 0.01, 2);

                sht.Range("A" + Row).SetValue("FOB");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["FOB"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Overhead");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["Overhead"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Interest %");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("B" + Row).SetValue(DS.Tables[0].Rows[0]["Interest"]);
                sht.Range("B" + Row).Style.Font.FontSize = 11;
                sht.Range("B" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("B" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("C" + Row).SetValue(TotalInterest);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Cost Price");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(Math.Round((TotalInterest + TotalAmount), 2));
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Ex.Rate");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["ExchangeRate"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Cost Price (" + DS.Tables[0].Rows[0]["CurrencyName"] + ")");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["CostPricePerCurrency"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                Row = Row + 1;

                sht.Range("A" + Row).SetValue("PO#");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["PoNo"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                Row = Row + 1;

                sht.Range("A" + Row).SetValue("license " + DS.Tables[0].Rows[0]["licensePercentage"] + "%");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["licenseAmount"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                Row = Row + 1;

                sht.Range("A" + Row).SetValue("Drawback " + DS.Tables[0].Rows[0]["DrawBackPercentage"] + "%");
                sht.Range("A" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).SetValue(DS.Tables[0].Rows[0]["DrawBackAmount"]);
                sht.Range("C" + Row).Style.Font.FontSize = 11;
                sht.Range("C" + Row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("C" + Row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                //******SAVE FILE
                sht.Columns(1, 9).AdjustToContents();
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("CostSheet" + DateTime.Now + "." + Fileextension);
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
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT DESIGNID, DESIGNNAME from DESIGN(Nolock) Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By DESIGNNAME");
        if (TDDesign.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Select--");
        }
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT COLORID,COLORNAME FROM COLOR(Nolock) Where  MasterCompanyId=" + Session["varCompanyId"] + @" Order By COLORNAME");
        if (TDColor.Visible == true)
        {
            UtilityModule.ConditionalComboFillWithDS(ref DDColor, ds, 0, true, "--Select--");
        }
    }
    protected void btnGetCostingCode_Click(object sender, EventArgs e)
    {
        SampleCodeSearchForEdit();
    }
}