using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Carpet_DefineBomAndConsumption : System.Web.UI.Page
{
    int PROCESSCONSUMPTIONMASTER_ID = 0;
    string SizeString = "SizeFt";
    static int MasterCompanyid;
    string msg = "";
    static string WithBuyerCode = "";
    static int israwmat = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyid = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
        // WithBuyerCode = dr["WithBuyerCode"].ToString();
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            DataSet ds = new DataSet();
            logo();
            SizeString = "SizeFt";
            LblSub_Quality.Visible = false;
            dddSub_Quality.Visible = false;
            ddFinished.Visible = false;
            ddFinishedIN.Visible = false;
            BtnOpenOtherExpense.Visible = false;
            BtnOpenPackingCost.Visible = false;
            if (MasterCompanyid == 2)
            {
                UtilityModule.ConditionalComboFill(ref ddProcessName, "Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  AND PROCESS_NAME_ID in (6,8,9) Order BY Process_Name", true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddProcessName, "Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order BY Process_Name", true, "--SELECT--");
            }
            //";
            string str = "";
            str = @"Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Category_Name
                    Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by Category_Name
                    select val,Type from SizeType Order by val";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCategoryName, ds, 0, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddInCategoryName, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddOutCategoryName, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 2, false, "");
            if (DDsizetype.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDsizetype.SelectedValue = variable.VarDefaultSizeId;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDInSizeType, ds, 2, false, "");
            if (DDInSizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDInSizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDOutSizeType, ds, 2, false, "");
            if (DDOutSizeType.Items.FindByValue(variable.VarDefaultSizeId) != null)
            {
                DDOutSizeType.SelectedValue = variable.VarDefaultSizeId;
            }
            //*************
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select WithBuyerCode,VarProdCode from Mastersetting");
            ViewState["WithBuyercode"] = ds.Tables[0].Rows[0]["WithBuyerCode"].ToString();
            

            int VarProdCode = Convert.ToInt16(ds.Tables[0].Rows[0]["VarProdCode"]);
            //*************
            ViewState["SelectedIweight"] = 0;
            ViewState["FinishedID"] = 0;
            Session["PCMID"] = PROCESSCONSUMPTIONMASTER_ID;
            LblQty.Text = "0";
            if (Request.QueryString["ZZZ"] == "1")
            {
                zzz.Style.Add("display", "none");
                BtnNew.Visible = false;
                BtncClose.Visible = false;
            }
            if (Request.QueryString["finishedid"] != null)
            {
                fillMasterParameter();
            }
            lablechange();
            // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            HDF1.Value = Session["varMasterCompanyIDForERP"].ToString();
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 1:
                    LblSub_Quality.Visible = true;
                    dddSub_Quality.Visible = true;
                    LblQty.Visible = true;
                    // btnopen.Visible = false;
                    BtnOpenOtherExpense.Visible = false;
                    BtnOpenPackingCost.Visible = false;
                    Btn_FINIE_TYPE.Visible = false;
                    LblIFinish.Visible = false;
                    LblOFinish.Visible = false;
                    lblItemCode.Visible = false;
                    TxtProdCode.Visible = false;
                    LblInItemCode.Visible = false;
                    TxtInProdCode.Visible = false;
                    LblOutItemCode.Visible = false;
                    TxtOutProdCode.Visible = false;
                    btnsubquality.Visible = true;
                    BtnPreview.Visible = true;
                    break;
                case 2:

                    ChkForOneToOne.Checked = true;
                    ddFinished.Visible = true;
                    ddFinishedIN.Visible = true;
                    LblQty.Visible = false;
                    btnopen.Visible = true;
                    BtnOpenOtherExpense.Visible = true;
                    BtnOpenPackingCost.Visible = true;
                    LblIFinish.Visible = true;
                    LblOFinish.Visible = true;
                    lblItemCode.Visible = true;
                    TxtProdCode.Visible = true;
                    LblInItemCode.Visible = true;
                    TxtInProdCode.Visible = true;
                    LblOutItemCode.Visible = true;
                    TxtOutProdCode.Visible = true;
                    btnsubquality.Visible = false;
                    TdRemarks.Visible = true;
                    DDICALTYPE.SelectedValue = "1";
                    ddOCalType.SelectedValue = "1";
                    TblApproval.Visible = true;
                    ChkForMtr.Visible = false;
                    ChkForLossPercentage.Visible = false;
                    TDIloss.Visible = false;
                    TDRecLooss.Visible = false;
                    BtnShow.Visible = false;
                    break;
                case 3:
                    ChkForOneToOne.Checked = true;
                    ddFinished.Visible = false;
                    ddFinishedIN.Visible = false;
                    LblQty.Visible = false;
                    btnopen.Visible = true;
                    //BtnOpenOtherExpense.Visible = true;
                    //BtnOpenPackingCost.Visible = true;
                    LblIFinish.Visible = false;
                    LblOFinish.Visible = false;
                    lblItemCode.Visible = false;
                    TxtProdCode.Visible = false;
                    LblInItemCode.Visible = false;
                    TxtInProdCode.Visible = false;
                    LblOutItemCode.Visible = false;
                    TxtOutProdCode.Visible = false;
                    btnsubquality.Visible = false;
                    break;
                case 6:
                    ChkForOneToOne.Checked = true;
                    ddFinished.Visible = false;
                    ddFinishedIN.Visible = false;
                    LblQty.Visible = false;
                    btnopen.Visible = true;
                    LblIFinish.Visible = false;
                    LblOFinish.Visible = false;
                    lblItemCode.Visible = true;
                    TxtProdCode.Visible = true;
                    LblInItemCode.Visible = true;
                    TxtInProdCode.Visible = true;
                    LblOutItemCode.Visible = true;
                    TxtOutProdCode.Visible = true;
                    btnsubquality.Visible = false;
                    TdRemarks.Visible = true;
                    TDMtrSize.Visible = true;
                    break;
                case 12:
                    //LblQty.Visible = false;
                    // btnopen.Visible = false;
                    BtnOpenOtherExpense.Visible = false;
                    BtnOpenPackingCost.Visible = false;
                    Btn_FINIE_TYPE.Visible = false;
                    LblIFinish.Visible = false;
                    LblOFinish.Visible = false;
                    lblItemCode.Visible = false;
                    TxtProdCode.Visible = true;
                    LblInItemCode.Visible = false;
                    TxtInProdCode.Visible = false;
                    LblOutItemCode.Visible = false;
                    TxtOutProdCode.Visible = false;
                    btnsubquality.Visible = false;
                    BtnPreview.Visible = true;
                    ddIUnit.SelectedValue = "2";
                    ddOUnit.SelectedValue = "2";
                    DDICALTYPE.SelectedValue = "0";
                    ddOCalType.SelectedValue = "0";
                    // TxtProdCode.Text = "0";
                    break;
                case 9:
                    LblQty.Visible = false;
                    break;
                default:
                    //LblQty.Visible = false;
                    // btnopen.Visible = false;
                    BtnOpenOtherExpense.Visible = false;
                    BtnOpenPackingCost.Visible = false;
                    Btn_FINIE_TYPE.Visible = false;
                    LblIFinish.Visible = false;
                    LblOFinish.Visible = false;
                    lblItemCode.Visible = false;
                    TxtProdCode.Visible = false;
                    LblInItemCode.Visible = false;
                    TxtInProdCode.Visible = false;
                    LblOutItemCode.Visible = false;
                    TxtOutProdCode.Visible = false;
                    btnsubquality.Visible = false;
                    BtnPreview.Visible = true;
                    ddIUnit.SelectedValue = "2";
                    ddOUnit.SelectedValue = "2";
                    DDICALTYPE.SelectedValue = "0";
                    ddOCalType.SelectedValue = "0";
                    //TxtProdCode.Text = "0";
                    break;
            }

            switch (VarProdCode)
            {
                case 0:
                    lblItemCode.Visible = false;
                    TxtProdCode.Visible = false;
                    LblInItemCode.Visible = false;
                    TxtInProdCode.Visible = false;
                    LblOutItemCode.Visible = false;
                    TxtOutProdCode.Visible = false;
                    // TxtInProdCode.Text = "0";
                    break;
                case 1:
                    lblItemCode.Visible = true;
                    TxtProdCode.Visible = true;
                    LblInItemCode.Visible = true;
                    TxtInProdCode.Visible = true;
                    LblOutItemCode.Visible = true;
                    TxtOutProdCode.Visible = true;
                    break;
            }
            if (Session["varMasterCompanyIDForERP"].ToString()=="21")
            {
                ChkForMtr.Checked = true;
                DDICALTYPE.SelectedValue = "1";
                ddOCalType.SelectedValue = "1";
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblcategoryname.Text = ParameterList[5];
        lbloutcategoryname.Text = ParameterList[5];
        lblincategoryname.Text = ParameterList[5];

        lblitemname.Text = ParameterList[6];
        lbloutitemname.Text = ParameterList[6];
        lblinitemname.Text = ParameterList[6];

        lblqualityname.Text = ParameterList[0];
        lbloutqualiltlyname.Text = ParameterList[0];
        lblinqualityname.Text = ParameterList[0];

        lbldesignname.Text = ParameterList[1];
        lbloutdesignname.Text = ParameterList[1];
        lblindesignname.Text = ParameterList[1];

        lblcolorname.Text = ParameterList[2];
        lbloutcolorname.Text = ParameterList[2];
        lblincolorname.Text = ParameterList[2];

        lblshapename.Text = ParameterList[3];
        lbloutshapename.Text = ParameterList[3];
        lblinshapename.Text = ParameterList[3];

        lblsizename.Text = ParameterList[4];
        lbloutsizename.Text = ParameterList[4];
        lblinsizename.Text = ParameterList[4];

        lblshadename.Text = ParameterList[7];
        lbloutshadename.Text = ParameterList[7];
        lblinshade.Text = ParameterList[7];
    }
    protected void ddcategoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
        if (ChkForFillSame.Checked == true)
        {
            ddInCategoryName.SelectedValue = ddCategoryName.SelectedValue;
            IN_CATEGORY_DEPENDS_CONTROLS();
            ddOutCategoryName.SelectedValue = ddCategoryName.SelectedValue;
            OUT_CATEGORY_DEPENDS_CONTROLS();
        }
        btnaddcategory.Focus();
    }
    private void CATEGORY_DEPENDS_CONTROLS()
    {
        Quality.Visible = false;
        Design.Visible = false;
        Color.Visible = false;
        Shape.Visible = false;
        Size.Visible = false;
        Shade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        Quality.Visible = true;
                        break;
                    case "2":
                        Design.Visible = true;
                        break;
                    case "3":
                        Color.Visible = true;
                        break;
                    case "4":
                        Shape.Visible = true;
                        break;
                    case "5":
                        Size.Visible = true;
                        break;
                    case "6":
                        Shade.Visible = true;
                        break;
                }
            }
        }
        if (Session["varMasterCompanyIDForERP"].ToString() == "16")
        {
            if (ddCategoryName.SelectedItem.Text != "CARPET")
            {
                ViewState["WithBuyercode"] = "0";
            }
        }
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
        if (ChkForFillSame.Checked == true)
        {
            ddInItemName.SelectedValue = ddItemName.SelectedValue;
            InItemNameSelectedIndexChange();
            ddOutItemName.SelectedValue = ddItemName.SelectedValue;
            OutItemSelectedIndexChange();
        }
        btnadditem.Focus();
    }

    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN", true, "--SELECT DESIGN--");
        Fill_Sub_Quality();
        if (ChkForFillSame.Checked == true)
        {
            ddInQuality.SelectedValue = ddQuality.SelectedValue;
            ddOutQuality.SelectedValue = ddQuality.SelectedValue;
            if (ViewState["WithBuyercode"].ToString() == "1")
            {
                FillDesign(ddOutItemName, ddOutQuality, ddOutDesign, OutDesign);
            }
            //if (Session["varMasterCompanyIDForERP"].ToString() == "4")
            //{
            //    FillDesign(ddOutItemName, ddOutQuality, ddOutDesign, OutDesign);
            //}
        }
        btnaddquality.Focus();
        if (Session["VarcompanyNo"].ToString() == "2")
        {
            TxtProdCode.Text = ddQuality.SelectedItem.Text.Trim();
        }
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            FillDesign(ddItemName, ddQuality, ddDesign, Design);
        }
        //if (Session["varMasterCompanyIDForERP"].ToString() == "4")//Deepak
        //{
        //    FillDesign(ddItemName, ddQuality, ddDesign, Design);
        //}
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForFillSame.Checked == true)
        {
            if (CHKFORALLDESIGN.Checked == true)
            {
                ddInDesign.SelectedIndex = 1;
                ddOutDesign.SelectedIndex = 1;
            }
            else
            {
                ddInDesign.SelectedValue = ddDesign.SelectedValue;
                ddOutDesign.SelectedValue = ddDesign.SelectedValue;
            }
        }
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            fillColor(ddItemName, ddQuality, ddDesign, ddColor, Color);
        }
        //if (Session["varMasterCompanyIDForERP"].ToString() == "4")
        //{
        //    fillColor(ddItemName, ddQuality, ddDesign, ddColor, Color);
        //}
        //UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR", true, "--SELECT COLOR--");
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddShape, "SELECT SHAPEID,SHAPENAME FROM SHAPE", true, "--SELECT SHAPE--");
        if (ChkForFillSame.Checked == true)
        {
            if (CHKFORALLCOLOR.Checked == true)
            {
                ddInColor.SelectedIndex = 1;
                ddOutColor.SelectedIndex = 1;
            }
            else
            {
                ddInColor.SelectedValue = ddColor.SelectedValue;
                ddOutColor.SelectedValue = ddColor.SelectedValue;
            }
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (WithBuyerCode == "1")
        //{
        //    FillSize();
        //}
        ////if (Session["varMasterCompanyIDForERP"].ToString() == "4")
        ////{
        ////    FillSize();
        ////}
        //else
        //{
        //    ShapeSelectedChange();
        //}

        if (Session["varMasterCompanyIDForERP"].ToString() == "30")
        {
            CHKFORALLSIZE.Checked = true;
            CHKFORALLSIZE_CheckedChanged(sender, new EventArgs()); 
        }
        else
        {

            FillSize();
        }
    }
    private void ShapeSelectedChange()
    {
        TDMtrSize.Visible = true;
        if (ChMeteerSize.Checked)
        {
            SizeString = "SizeMtr";
        }
        else
        {
            SizeString = "SIZEFT";
        }
        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order By SIZEFT", true, "--SELECT--");
        if (ChkForFillSame.Checked == true)
        {
            ddInShape.SelectedValue = ddShape.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
            ddOutShape.SelectedValue = ddShape.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
        }
    }
    protected void ddOutCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        israwmat = checkcategory(ddOutCategoryName);
        OUT_CATEGORY_DEPENDS_CONTROLS();
        ddOutItemName.Focus();
        //For Deepak rugs
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "4":
                if (ddProcessName.SelectedValue == "5")
                {
                    TDDyeingMatch.Visible = true;
                    TDDyingType.Visible = true;
                    TDDyeing.Visible = true;
                }
                else
                {
                    TDDyeingMatch.Visible = false;
                    TDDyingType.Visible = false;
                    TDDyeing.Visible = false;
                }
                break;
            default:
                break;
        }
        //
    }
    private void Fill_Sub_Quality()
    {
        if (ddItemName.SelectedIndex > 0 && ddQuality.SelectedIndex > 0 && dddSub_Quality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddSub_Quality, "Select QualityCodeId,SubQuantity from qualityCodeMaster Where Item_Id=" + ddItemName.SelectedValue + " And QualityId=" + ddQuality.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order By SubQuantity", true, "--SELECT SUB_QUALITY--");
        }
    }
    private void OUT_CATEGORY_DEPENDS_CONTROLS()
    {
        if (ddFinished.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddFinished, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE where  MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  ORDER BY FINISHED_TYPE_NAME", true, "--SELECT--");
        }
        OutQuality.Visible = false;
        OutDesign.Visible = false;
        OutColor.Visible = false;
        OutShape.Visible = false;
        OutSize.Visible = false;
        OutShade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddOutItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddOutCategoryName.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "--SELECT--");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddOutCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        OutQuality.Visible = true;
                        break;
                    case "2":
                        OutDesign.Visible = true;
                        break;
                    case "3":
                        OutColor.Visible = true;
                        break;
                    case "4":
                        OutShape.Visible = true;
                        break;
                    case "5":
                        OutSize.Visible = true;
                        break;
                    case "6":
                        OutShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddOutItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        OutItemSelectedIndexChange();
        ddOutQuality.Focus();
    }
    private void OutItemSelectedIndexChange()
    {
        QDCSDDFill(ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutShade, Convert.ToInt32(ddOutItemName.SelectedValue), 1);
        //UtilityModule.ConditionalComboFill(ref ddOutQuality , "Select QUALITYID,QUALITYNAME from QUALITY WHERE ITEM_ID=" + ddOutItemName.SelectedValue + "", true, "--SELECT QUALITY--");        
        UtilityModule.ConditionalComboFill(ref ddOUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddOutItemName.SelectedValue + " Order BY U.UNITNAME", true, "--SELECT--");
        if (ddOUnit.Items.Count > 0)
        {
            ddOUnit.SelectedIndex = 1;
        }
        //FILLGRID();
    }
    protected void ddOutShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_RECEIVE();
    }
    private void FILL_SIZE_RECEIVE()
    {
        string Str = "";

        //if (CheckBox2.Checked)
        //{
        //    SizeString = "SizeMtr";
        //}
        //else
        //{
        //    SizeString = "SIZEFT";
        //}
        switch (DDOutSizeType.SelectedValue)
        {
            case "1":
                SizeString = "Sizemtr";
                break;
            case "0":
                SizeString = "Sizeft";
                break;
            case "2":
                SizeString = "Sizeinch";
                break;
            default:
                SizeString = "Sizeft";
                break;
        }
        Str = "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "";
        if (ViewState["WithBuyercode"].ToString() == "1")
        {

            Str = @"select Distinct S.Sizeid,S." + SizeString + " As  " + SizeString + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                      Im.SIZE_ID=S.SizeId inner join CustomerSize CS on cs.Sizeid=S.SizeId
                      and S.shapeId=" + ddOutShape.SelectedValue + "  and Im.Item_Id=" + ddOutItemName.SelectedValue + " and IM.QUALITY_ID=" + ddOutQuality.SelectedValue + "  and S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + SizeString + "";

        }
        UtilityModule.ConditionalComboFill(ref ddOutSize, Str, true, "--SELECT--");

    }
    protected void ddInCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        israwmat = checkcategory(ddInCategoryName);
        IN_CATEGORY_DEPENDS_CONTROLS();
        ddInItemName.Focus();
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "9":
                if (ddProcessName.SelectedValue == "5")
                {
                    ddOutCategoryName.SelectedValue = ddInCategoryName.SelectedValue;
                    ddOutCategoryName_SelectedIndexChanged(sender, e);
                }
                break;
        }
    }

    private void IN_CATEGORY_DEPENDS_CONTROLS()
    {
        if (ddFinishedIN.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddFinishedIN, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE where  MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " ORDER BY FINISHED_TYPE_NAME", true, "--SELECT--");
            ddFinishedIN.SelectedValue = "5";
        }
        InQuality.Visible = false;
        InDesign.Visible = false;
        InColor.Visible = false;
        InShape.Visible = false;
        InSize.Visible = false;
        InShade.Visible = false;
        if (ChkForProcessInPut.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddInItemName, "SELECT DISTINCT IM.ITEM_ID,ITEM_NAME from ITEM_MASTER IM,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.ITEM_ID=IM.ITEM_ID AND CATEGORY_ID=" + ddInCategoryName.SelectedValue + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ITEM_NAME", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddInItemName, "Select DISTINCT ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddInCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order By ITEM_NAME", true, "SELECT--");
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from ITEM_CATEGORY_PARAMETERS Where CATEGORY_ID=" + ddInCategoryName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        InQuality.Visible = true;
                        break;
                    case "2":
                        InDesign.Visible = true;
                        break;
                    case "3":
                        InColor.Visible = true;
                        break;
                    case "4":
                        InShape.Visible = true;
                        break;
                    case "5":
                        InSize.Visible = true;
                        break;
                    case "6":
                        InShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddInItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        InItemNameSelectedIndexChange();
        //FILLGRID();
        ddInQuality.Focus();
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "9":
                if (ddProcessName.SelectedValue == "5")
                {
                    if (ddOutCategoryName.SelectedIndex > 0)
                    {
                        ddOutItemName.SelectedValue = ddInItemName.SelectedValue;
                        ddOutItemName_SelectedIndexChanged(sender, e);
                    }
                }
                break;
        }
    }
    private void InItemNameSelectedIndexChange()
    {
        if (ChkForProcessInPut.Checked == true)
        {
            QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
            //UtilityModule.ConditionalComboFill(ref ddInQuality, "SELECT DISTINCT Q.QUALITYID,Q.QUALITYNAME from QUALITY Q,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.IFINISHEDID AND IPM.QUALITY_ID=Q.QUALITYID AND IPM.ITEM_ID=" + ddInItemName.SelectedValue + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "", true, "--SELECT QUALITY--");
            UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD WHERE PM.PCMID=PD.PCMID AND U.UNITID=PD.IUNITID AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " Order By U.UNITNAME", true, "--SELECT--");
        }
        else
        {
            QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
            UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddInItemName.SelectedValue + " Order BY U.UNITNAME", true, "--SELECT--");
        }
        if (ddIUnit.Items.Count > 0)
        {
            ddIUnit.SelectedIndex = 1;
        }
        int finishedid = GetItemFinishedID();
        newPreview1.ImageUrl = "~/ImageHandler.ashx?Id=" + finishedid + "&img=3";
    }
    protected void fill_grid1()
    {
        DGInPutProcess.DataSource = getdetail();
        DGInPutProcess.DataBind();
        //for (int i = 0; i < DGInPutProcess.Rows.Count; i++)
        //{
        //   ((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text=DGInPutProcess.Rows[i].Cells[2].Text;
        //}
    }
    protected DataSet getdetail()
    {
        DataSet ds = null;
        DataSet ds1 = null;
        string Str = "";
        //string Str1 = "";
        int VarNum = 0;
        if (ChkForMtr.Checked == true)
        {
            VarNum = 1;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            Str = @"SELECT COUNT(*) COUNT,OFINISHEDID,INOUTTYPEID from PROCESSCONSUMPTIONMASTER PCM,PROCESSCONSUMPTIONDETAIL PCD WHERE PCM.PCMID=PCD.PCMID AND 
                   PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " And PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " GROUP BY OFINISHEDID,INOUTTYPEID HAVING COUNT(*)>1 ORDER BY COUNT(*)";
            ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);

            Str = @"SELECT DISTINCT PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,Case When " + VarNum + @"=1 then Round(OQty*1.196,5) Else OQty End PreQty,
                    Case When " + VarNum + @"=1 then Round(OQty*1.196,5) Else OQty End Qty,case When PCM.MasterCompanyId<>9 Then 0 Else  OLoss End Loss,PCD.Icaltype FROM PROCESSCONSUMPTIONMASTER PCM,
                 PROCESSCONSUMPTIONDETAIL PCD,ITEM_PARAMETER_MASTER IPCM,VIEWFINDFINISHEDID1 VF1 WHERE PCM.PCMID=PCD.PCMID AND 
                 PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " And PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            #region
            //if (ddQuality.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND QUALITY_ID=" + ddQuality.SelectedValue + "";
            //}

            //if (CHKFORALLDESIGN.Checked == true)
            //{
            //    Str = Str + @" AND DESIGN_ID=-1";
            //}
            //else if (ddDesign.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND DESIGN_ID=" + ddDesign.SelectedValue + "";

            //}
            //if (CHKFORALLCOLOR.Checked == true)
            //{
            //    Str = Str + @" AND COLOR_ID=-1";
            //}
            //else if (ddColor.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND COLOR_ID=" + ddColor.SelectedValue + "";
            //}
            //if (ddShape.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND SHAPE_ID=" + ddShape.SelectedValue + "";
            //}
            //if (CHKFORALLSIZE.Checked == true)
            //{
            //    Str = Str + @" AND SIZE_ID=-1";
            //}
            //else if (ddSize.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND SIZE_ID=" + ddSize.SelectedValue + "";
            //}
            //if (ddProcessName.SelectedIndex > 0)
            //{
            //    Str = Str + @" AND PCM.PROCESSID=" + ddProcessName.SelectedValue + "";
            //}
            #endregion
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds1.Tables[0].Rows[0]["INOUTTYPEID"]) == 1)
                {
                    Str = @"SELECT DISTINCT top(1)  PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,Round(case When PCM.flagmtrft=1 Then OQTY*1.196 Else OQTY ENd,5) PreQty,
                    Round(case When PCM.flagmtrft=1 Then OQTY*1.196 Else OQTY ENd,5) Qty,case When PCM.MasterCompanyId<>9 Then 0  Else OLoss End Loss,PCD.Icaltype FROM PROCESSCONSUMPTIONMASTER PCM,ITEM_PARAMETER_MASTER IPCM,
                    VIEWFINDFINISHEDID1 VF1,PROCESSCONSUMPTIONDETAIL PCD
                    WHERE PCM.PCMID=PCD.PCMID AND PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND 
                    PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " AND PCD.OFINISHEDID=" + ds1.Tables[0].Rows[0]["OFINISHEDID"] + @"
                    UNION  SELECT DISTINCT PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,Round(OQTY,5) PreQty,
                    Round(OQTY,5) Qty,0 Loss,PCD.Icaltype FROM PROCESSCONSUMPTIONMASTER PCM,ITEM_PARAMETER_MASTER IPCM,
                    VIEWFINDFINISHEDID1 VF1,PROCESSCONSUMPTIONDETAIL PCD
                    WHERE PCM.PCMID=PCD.PCMID AND PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND 
                    PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " And PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OFINISHEDID<>" + ds1.Tables[0].Rows[0]["OFINISHEDID"];
                }
            }
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
            Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;

        //        DataSet ds = null;
        //        string Str="";
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        try
        //        {
        //            con.Open();
        //            //int Varfinishedid = UtilityModule.getItemFinishedIdForConsumption(ddQuality, ddDesign, ddColor, ddShape, ddSize, ddItemName, ddCategoryName, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE);
        //            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE);

        //            Str = @"SELECT DISTINCT PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,OQty PreQty,OQty Qty FROM PROCESSCONSUMPTIONMASTER PCM,
        //                 PROCESSCONSUMPTIONDETAIL PCD,ITEM_PARAMETER_MASTER IPCM,VIEWFINDFINISHEDID1 VF1 WHERE PCM.PCMID=PCD.PCMID AND 
        //                 PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + "";
        //            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str); 
        //        }

        //        catch (Exception ex)
        //        {
        //            Logs.WriteErrorLog("error");
        //        }
        //        finally
        //        {
        //            con.Close();
        //            con.Dispose();
        //        }

        //        return ds;
    }
    protected void ddInQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtIweight.Text = "0";
        DataSet ds;
        if (Session["VarcompanyNo"].ToString() == "2")
        {
            int FinishedID = GetInItemFinishedID();
            //string Str = "SELECT isnull(max(NETWEIGHT),0)   From Item_Parameter_Master IPM inner join MAIN_ITEM_IMAGE MI on ipm.item_finished_id=MI.finishedid where  IPM.quality_id =" + ddInQuality.SelectedValue  ;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(NETWEIGHT,0) NETWEIGHT from MAIN_ITEM_IMAGE  where FINISHEDID=" + FinishedID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtIweight.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            TotalAmount(FinishedID, 0);
        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select loss from quality where qualityid=" + ddInQuality.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtLoss.Text = ds.Tables[0].Rows[0][0].ToString();
        }

        if (ChkForOneToOne.Checked == true)
        {
            TxtInProdCode.Text = ddInQuality.SelectedItem.Text;
            TxtOutProdCode.Text = TxtInProdCode.Text;
            Out_ProdCode_TextChanges();
        }
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "9":
                if (ddProcessName.SelectedValue == "5")
                {
                    if (ddOutItemName.SelectedIndex > 0)
                    {
                        ddOutQuality.SelectedValue = ddInQuality.SelectedValue;
                        ddOutQuality_SelectedIndexChanged(sender, e);
                    }
                }
                break;
        }
        // TxtLoss.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select loss from quality where qualityid=" + ddInQuality.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "").ToString();
    }

    private int GetInItemFinishedID()
    {
        int Quality = 0;
        int design = 0;
        int color = 0;
        int shape = 0;
        int size = 0;
        int shade = 0;
        if (ddInQuality.SelectedIndex > 0)
        {
            Quality = Convert.ToInt32(ddInQuality.SelectedValue);

        }
        if (ddInDesign.SelectedIndex > 0)
        {
            design = Convert.ToInt32(ddInDesign.SelectedValue);

        }

        if (ddInColor.SelectedIndex > 0)
        {
            color = Convert.ToInt32(ddInColor.SelectedValue);
        }
        if (ddInShape.SelectedIndex > 0)
        {
            shape = Convert.ToInt32(ddInShape.SelectedValue);
        }
        if (CheckBox1.Checked == true)
        {
            size = -1;
        }
        else if (ddInSize.SelectedIndex > 0)
        {
            size = Convert.ToInt32(ddInSize.SelectedValue);
        }
        if (ddInShade.SelectedIndex > 0)
        {
            shade = Convert.ToInt32(ddInShade.SelectedValue);
        }

        int FinishedID = UtilityModule.getItemFinishedId(Convert.ToInt32(ddInItemName.SelectedValue), Quality, design, color, shape, size, shade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        return FinishedID;
    }
    private int GetItemFinishedID()
    {
        int Item = 0;
        int Quality = 0;
        int design = 0;
        int color = 0;
        int shape = 0;
        int size = 0;
        int shade = 0;
        if (ddItemName.SelectedIndex > 0)
        {
            Item = Convert.ToInt32(ddItemName.SelectedValue);

        }
        if (ddQuality.SelectedIndex > 0)
        {
            Quality = Convert.ToInt32(ddQuality.SelectedValue);

        }

        if (CHKFORALLDESIGN.Checked == true)
        {
            design = -1;
        }
        else if (ddDesign.SelectedIndex > 0)
        {
            design = Convert.ToInt32(ddDesign.SelectedValue);

        }

        if (CHKFORALLCOLOR.Checked == true)
        {
            color = -1;
        }
        else if (ddColor.SelectedIndex > 0)
        {
            color = Convert.ToInt32(ddColor.SelectedValue);
        }
        if (ddShape.SelectedIndex > 0)
        {
            shape = Convert.ToInt32(ddShape.SelectedValue);
        }
        if (CHKFORALLSIZE.Checked == true)
        {
            size = -1;
        }
        else if (ddSize.SelectedIndex > 0)
        {
            size = Convert.ToInt32(ddSize.SelectedValue);
        }
        if (ddInShade.SelectedIndex > 0)
        {
            shade = Convert.ToInt32(ddShade.SelectedValue);
        }

        int FinishedID = UtilityModule.getItemFinishedId(Item, Quality, design, color, shape, size, shade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        return FinishedID;
    }
    protected void ddInShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_ISSUE();
    }
    private void FILL_SIZE_ISSUE()
    {
        string size = "";
        string str = "";

        switch (DDInSizeType.SelectedValue)
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
        //size Query
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "4":
                case "9":
                    str = @"select Distinct S.Sizeid,S." + size + " As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                      Im.SIZE_ID=S.SizeId inner join CustomerSize CS on cs.Sizeid=S.SizeId
                      and S.shapeId=" + ddInShape.SelectedValue + "  and Im.Item_Id=" + ddInItemName.SelectedValue + " and IM.QUALITY_ID=" + ddInQuality.SelectedValue + "  and S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";
                    break;
                default:
                    str = "Select Distinct S.Sizeid,S." + size + " As  " + size + @" From Size S Inner Join CustomerSize CS on S.SizeId=CS.SizeId 
                      Where shapeid=" + ddInShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";
                    break;
            }
        }
        else
        {
            str = "Select Distinct S.Sizeid,S." + size + "+Space(2)+isnull(SizeNameAToC,'') As  " + size + @" From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId 
                 Where shapeid=" + ddInShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";
        }
        UtilityModule.ConditionalComboFill(ref ddInSize, str, true, "--SELECT--");
        //if (CheckBox1.Checked)
        //{
        //    SizeString = "SizeMtr";
        //}
        //else
        //{
        //    SizeString = "SIZEFT";
        //}
        //UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--SELECT--");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid, int Type_Flag, System.Web.UI.HtmlControls.HtmlTableCell tdItem = null, System.Web.UI.HtmlControls.HtmlTableCell tdQuality = null, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null)
    {
        #region
        //        if (ChkForProcessInPut.Checked == true)
        //        {
        //            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade,0);
        //            string Str = @"SELECT DISTINCT Q.QUALITYID,Q.QUALITYNAME FROM QUALITY Q,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM 
        //                        WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.QUALITY_ID=Q.QUALITYID AND IPM.ITEM_ID=" + Itemid + @"
        //                        AND PM.FINISHEDID=" + Varfinishedid + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "";
        //            UtilityModule.ConditionalComboFill(ref Quality, Str, true, "--SELECT--");
        //            Str = @"SELECT DISTINCT D.DESIGNID,D.DESIGNNAME from DESIGN D,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM 
        //                   WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.DESIGN_ID=D.DESIGNID AND IPM.ITEM_ID=" + Itemid + @"
        //                   AND PM.FINISHEDID=" + Varfinishedid + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "";
        //            UtilityModule.ConditionalComboFill(ref Design, Str, true, "--SELECT--");
        //            Str = @"SELECT DISTINCT C.COLORID,C.COLORNAME FROM COLOR C,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM 
        //                   WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.COLOR_ID=C.COLORID AND IPM.ITEM_ID=" + Itemid + @"
        //                   AND PM.FINISHEDID=" + Varfinishedid + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "";
        //            UtilityModule.ConditionalComboFill(ref Color, Str, true, "--SELECT--");
        //            Str = @"SELECT DISTINCT SH.SHAPEID,SH.SHAPENAME FROM SHAPE SH,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM 
        //                   WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.SHAPE_ID=SH.SHAPEID AND IPM.ITEM_ID=" + Itemid + @"
        //                   AND PM.FINISHEDID=" + Varfinishedid + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "";
        //            UtilityModule.ConditionalComboFill(ref Shape, Str, true, "--SELECT--");
        //            Str = @"SELECT DISTINCT SC.SHADECOLORID,SC.SHADECOLORNAME FROM SHADECOLOR SC,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM 
        //                   WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.OFINISHEDID AND IPM.SHADECOLOR_ID=SC.SHADECOLORID AND IPM.ITEM_ID=" + Itemid + @"
        //                   AND PM.FINISHEDID=" + Varfinishedid + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + "";
        //            UtilityModule.ConditionalComboFill(ref Shade, Str, true, "--SELECT--");
        //            //string Str= "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + "";
        //            //if (dddSub_Quality.Visible == true && Type_Flag == 1 && ChkForProcessInPut.Checked == false)
        //            //{
        //            //    Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY Q,qualitycodeDetail QD WHERE Q.QualityId=QD.Quality_ID And Q.ITEM_ID=" + Itemid + " And QD.qualitycodeid=" + ddSub_Quality.SelectedValue +"";
        //            //}
        //            //UtilityModule.ConditionalComboFill(ref Quality,Str, true, "--SELECT--");
        //            //UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN", true, "--SELECT--");
        //            //UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR", true, "--SELECT--");
        //            //UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE", true, "--SELECT--");
        //            //UtilityModule.ConditionalComboFill(ref Shade, "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR", true, "--SELECT--");
        //        }
        //        else
        //{
        #endregion
        string Str = "";
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            if (israwmat == 1)
            {
                Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By QUALITYNAME";
            }
            else
            {
                Str = @"SELECT Distinct Q.QualityId,Q.QualityName As QualityName
                        from Quality Q Inner join CustomerQuality CQ on Q.QualityId=Cq.QualityId  Where Item_Id=" + Itemid + " And Q.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by QualityName";
            }

        }
        else
        {
            Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By QUALITYNAME";
        }


        if (dddSub_Quality.Visible == true && Type_Flag == 1)
        {
            Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY Q,qualitycodeDetail QD WHERE Q.QualityId=QD.Quality_ID And Q.ITEM_ID=" + Itemid + " And QD.qualitycodeid=" + ddSub_Quality.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  Order By QUALITYNAME";
        }
        Str = Str + @"
            SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By DESIGNNAME
            SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By COLORNAME
            SELECT SHAPEID,SHAPENAME FROM SHAPE Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By SHAPENAME
            SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref Quality, ds, 0, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Design, ds, 1, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Color, ds, 2, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shape, ds, 3, true, "--SELECT--");
        UtilityModule.ConditionalComboFillWithDS(ref Shade, ds, 4, true, "--SELECT--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (BtnSave.Text == "UpDate" && ChkForInputConsmpQtyIntoOutputConsmpQty.Checked == true)
            {
                int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                int VarInfinishedid = UtilityModule.getItemFinishedId(ddInItemName, ddInQuality, ddInDesign, ddInColor, ddInShape, ddInSize, TxtOutProdCode, Tran, ddInShade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                int VarOutfinishedid = UtilityModule.getItemFinishedId(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutSize, TxtOutProdCode, Tran, ddOutShade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

                SqlParameter[] _arrpara = new SqlParameter[8];
                _arrpara[0] = new SqlParameter("@ProcessID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@FinishedID", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@IFinishedID", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@OFinishedID", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@InputConsumptionQty", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

                _arrpara[0].Value = ddProcessName.SelectedValue;
                _arrpara[1].Value = Varfinishedid;
                _arrpara[2].Value = VarInfinishedid;
                _arrpara[3].Value = VarOutfinishedid;
                _arrpara[4].Value = TxtInPutQty.Text;
                _arrpara[5].Value = Session["varuserid"];
                _arrpara[6].Value = Session["varMasterCompanyIDForERP"];
                _arrpara[7].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Prp_Update_ProcessConsumptionInputToOutputQty]", _arrpara);

                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + _arrpara[7].Value.ToString() + "');", true);
                Tran.Commit();
                Save_Refresh();
                FILLGRID();
                lblMessage.Visible = true;
                lblMessage.Text = "DATA SUCCESSFULLY SAVED.....";
                BtnSave.Text = "Save";
                
            }
            else if (Session["varMasterCompanyIDForERP"].ToString() == "2")
            {
                Save_ProcessInPut_Not_Check(Tran);
            }
            else
            {
                if (ChkForProcessInPut.Checked == true)
                {
                    Save_ProcessInPut_Check(Tran);
                }
                else
                {
                    Save_ProcessInPut_Not_Check(Tran);
                }
            }
            chksamereceiveitem.Checked = false;
            ChkForInputConsmpQtyIntoOutputConsmpQty.Checked = false;
            ChkForInputConsmpQtyIntoOutputConsmpQty.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
            Tran.Rollback();
            lblMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Save_ProcessInPut_Check(SqlTransaction Tran)
    {
        string str;
        int num = 0;
        CHECKVALIDCONTROLFOR_INPUT_PROCESS();

        if (lblMessage.Text == "")
        {
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESSCONSUMPTIONMASTER Where PCMID Not In (Select PCMID From PROCESSCONSUMPTIONDETAIL(Nolock))");

            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "SELECT PCMID FROM PROCESSCONSUMPTIONMASTER WHERE PROCESSID=" + ddProcessName.SelectedValue + " AND FINISHEDID=" + Varfinishedid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Session["PCMID"] = Ds.Tables[0].Rows[0]["PCMID"];
            }
            else
            {
                Session["PCMID"] = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMID ),0)+1 FROM PROCESSCONSUMPTIONMASTER"));
                num = 1;
            }
            if (BtnSave.Text == "UpDate" && num == 0)
            {
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE PROCESSCONSUMPTIONDETAIL WHERE PCMID=" + Session["PCMID"] + " And ProcessInputId<>0");
            }
            if (num == 1)
            {
                SqlParameter[] _arrpara = new SqlParameter[10];
                _arrpara[0] = new SqlParameter("@PCMID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@CalType", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@Rate", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@VarFlagMtrFt", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@VarPercenageFlag", SqlDbType.Int);
                _arrpara[9] = new SqlParameter("@Sizeflag", SqlDbType.TinyInt);

                _arrpara[0].Value = (Session["PCMID"]);
                _arrpara[1].Value = ddProcessName.SelectedValue;
                _arrpara[2].Value = Varfinishedid;
                _arrpara[3].Value = 0;
                _arrpara[4].Value = 0;
                _arrpara[5].Value = Session["varuserid"];
                _arrpara[6].Value = Session["varMasterCompanyIDForERP"];
                _arrpara[7].Value = ChkForMtr.Checked == true ? 1 : 2;
                _arrpara[8].Value = ChkForLossPercentage.Checked == true ? 1 : 0;
                _arrpara[9].Value = getSizeType(DDsizetype, Size);

                str = @"INSERT INTO PROCESSCONSUMPTIONMASTER(PCMID,PROCESSID,FINISHEDID,CALTYPE,RATE,userid,MasterCompanyid,FlagMtrFt,LossPercentageFlag,Sizeflag) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[3].Value + "," + _arrpara[4].Value + "," + _arrpara[5].Value + "," + _arrpara[6].Value + "," + _arrpara[7].Value + "," + _arrpara[8].Value + "," + _arrpara[9].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            int Valuseinserted = 0;
            string Icaltype = "0";
            for (int i = 0; i < DGInPutProcess.Rows.Count; i++)
            {
                CheckBox chk = ((CheckBox)DGInPutProcess.Rows[i].FindControl("Chkbox"));

                if (chk.Checked == true)
                {
                    int VarOutfinishedid = UtilityModule.getItemFinishedId(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutSize, TxtOutProdCode, ddOutShade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                    Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from PROCESSCONSUMPTIONDetail Where PCMID=" + Session["PCMID"] + " And IFINISHEDID in (Select OFINISHEDID from PROCESSCONSUMPTIONDetail Where PCMDID=" + DGInPutProcess.Rows[i].Cells[1].Text + ") And OFINISHEDID=" + VarOutfinishedid + "");
                    if (BtnSave.Text != "UpDate" && Ds.Tables[0].Rows.Count > 0)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "DATA ALLREADY EXISTS.....";
                    }
                    else
                    {
                        SqlParameter[] _arrpara = new SqlParameter[17];
                        _arrpara[0] = new SqlParameter("@PCMID", SqlDbType.Int);
                        _arrpara[1] = new SqlParameter("@PCMDID", SqlDbType.Int);
                        _arrpara[2] = new SqlParameter("@OFINISHEDID", SqlDbType.Int);
                        _arrpara[3] = new SqlParameter("@OUNITID", SqlDbType.Int);
                        _arrpara[4] = new SqlParameter("@OQTY", SqlDbType.Float);
                        _arrpara[5] = new SqlParameter("@ORATE", SqlDbType.Float);
                        _arrpara[6] = new SqlParameter("@PROCESSINPUTID", SqlDbType.Int);
                        _arrpara[7] = new SqlParameter("@INOUTTYPEID", SqlDbType.Int);
                        _arrpara[8] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);
                        _arrpara[9] = new SqlParameter("@SUB_QUALITY_ID", SqlDbType.Int);
                        _arrpara[10] = new SqlParameter("@OCALTYPE", SqlDbType.Int);
                        _arrpara[11] = new SqlParameter("@Loss", SqlDbType.Float);
                        _arrpara[12] = new SqlParameter("@IQTY", SqlDbType.Float);
                        _arrpara[13] = new SqlParameter("@RecLoss", SqlDbType.Float);
                        _arrpara[14] = new SqlParameter("@IWeight", SqlDbType.Float);
                        _arrpara[15] = new SqlParameter("@OSizeflag", SqlDbType.Int);
                        _arrpara[16] = new SqlParameter("@OQtyPercentage", SqlDbType.Float);


                        _arrpara[0].Value = (Session["PCMID"]);
                        _arrpara[1].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMDID ),0)+1 FROM PROCESSCONSUMPTIONDETAIL"));
                        _arrpara[2].Value = VarOutfinishedid;
                        _arrpara[3].Value = ddOUnit.SelectedValue;
                        _arrpara[5].Value = TxtOutPutRate.Text;
                        _arrpara[6].Value = ChkForProcessInPut.Checked == true ? Convert.ToInt32(ddInProcessName.SelectedValue) : 0;
                        _arrpara[7].Value = ChkForManyOutPut.Checked == true ? 2 : ChkForOneToOne.Checked == true ? 0 : 1;
                        _arrpara[8].Value = 0;
                        _arrpara[9].Value = 0;
                        _arrpara[14].Value = Math.Round(Convert.ToDecimal(TxtIweight.Text == "" ? "0" : TxtIweight.Text), 3);

                        if (ddSub_Quality.Visible == true)
                        {
                            _arrpara[9].Value = ddSub_Quality.SelectedValue;
                        }
                        if (ddFinished.Visible == true)
                        {
                            _arrpara[8].Value = ddFinished.SelectedValue; ;
                        }
                        _arrpara[10].Value = ddOCalType.SelectedValue;
                        if (((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text != "0" && ((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text != "")
                        {
                            if (ChkForMtr.Checked == true)
                            {
                                if (Valuseinserted == 0)
                                {
                                    _arrpara[4].Value = Math.Round(Convert.ToDouble(TxtOutPutQty.Text) / 1.196, 5);
                                    _arrpara[13].Value = Math.Round(Convert.ToDouble(TxtRecLoss.Text) / 1.196, 5);
                                }
                                else
                                {
                                    _arrpara[4].Value = 0;
                                    _arrpara[13].Value = 0;

                                }
                                if (((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text != "")
                                {
                                    _arrpara[11].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text) / 1.196, 5);
                                }
                                else
                                {
                                    _arrpara[4].Value = "0";
                                }
                                _arrpara[12].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text) / 1.196, 5);
                            }
                            else
                            {
                                if (Valuseinserted == 0)
                                {
                                    _arrpara[4].Value = Math.Round(Convert.ToDouble(TxtOutPutQty.Text), 5);
                                    _arrpara[13].Value = Math.Round(Convert.ToDouble(TxtRecLoss.Text), 5);
                                }
                                else
                                {
                                    _arrpara[4].Value = 0;
                                    _arrpara[13].Value = 0;
                                }

                                if (TxtLoss.Text != "")
                                {
                                    if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                                    {
                                        _arrpara[11].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text), 5);
                                    }
                                    else
                                    {

                                        _arrpara[11].Value = Math.Round(Convert.ToDouble(TxtLoss.Text), 5);
                                    }
                                }
                                else
                                {
                                    _arrpara[11].Value = "0";
                                }
                                _arrpara[12].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text), 5);
                                _arrpara[11].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text), 5);
                            }
                            if (ChkForLossPercentage.Checked == true)
                            {
                                _arrpara[11].Value = Math.Round(Convert.ToDouble(_arrpara[12].Value) * Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text == "" ? "0" : ((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text) / 100, 5);
                                _arrpara[13].Value = Math.Round(Convert.ToDouble(_arrpara[4].Value) * Convert.ToDouble(TxtRecLoss.Text == "" ? "0" : TxtRecLoss.Text) / 100, 5);
                            }
                            _arrpara[15].Value = getSizeType(DDOutSizeType, OutSize);
                            _arrpara[16].Value = txtOutputQtyPercentage.Text == "" ? "0" : txtOutputQtyPercentage.Text;
                            DropDownList ddlicaltypedginput = (DropDownList)DGInPutProcess.Rows[i].FindControl("ddlicaltypedginput");
                            Icaltype = ddlicaltypedginput.SelectedValue;
                            str = @"INSERT INTO PROCESSCONSUMPTIONDETAIL (PCMID,PCMDID,IFINISHEDID,IUNITID,IQTY,ILOSS,IRATE,OFINISHEDID,OUNITID,OQTY,ORATE,PROCESSINPUTID,INOUTTYPEID,ICALTYPE,I_FINISHED_Type_ID,
                                       O_FINISHED_Type_ID,SUB_QUALITY_ID,OCALTYPE,OLoss,iweight,ISizeflag,OSizeflag,OQtyPercentage,AddedDate) Select " + _arrpara[0].Value + "," + _arrpara[1].Value + ",OFINISHEDID,OUNITID," + _arrpara[12].Value + "," + _arrpara[11].Value + ",ORATE," + _arrpara[2].Value + "," + _arrpara[3].Value + "," + _arrpara[4].Value + "," + _arrpara[5].Value + "," + _arrpara[6].Value + "," + _arrpara[7].Value + "," + Icaltype + ",O_FINISHED_Type_ID," + _arrpara[8].Value + "," + _arrpara[9].Value + "," + _arrpara[10].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + ",ISizeflag," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' From PROCESSCONSUMPTIONDetail Where PCMDID=" + DGInPutProcess.Rows[i].Cells[1].Text + "";
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                            Valuseinserted = 1;
                        }
                    }
                }
            }
            Tran.Commit();
            lblMessage.Visible = true;
            if (lblMessage.Text == "")
            {
                lblMessage.Text = "DATA SUCCESSFULLY SAVED.....";
            }
            FILLGRID();
            DGInPutProcess.Visible = false;
            INPROCESSNAME.Visible = false;
            ChkForProcessInPut.Checked = false;
            Tr1.Disabled = false;
            Tr3.Disabled = false;
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
            Save_Refresh();
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Category_Name", true, "--SELECT--");
        }
    }
    private void Save_ProcessInPut_Not_Check(SqlTransaction Tran)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Delete PROCESSCONSUMPTIONMASTER Where PCMID Not In (Select PCMID From PROCESSCONSUMPTIONDETAIL(Nolock))");
                string str;
                DataSet Ds;
                SqlParameter[] _arrpara = new SqlParameter[34];
                _arrpara[0] = new SqlParameter("@PCMID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@CalType", SqlDbType.Int);
                _arrpara[16] = new SqlParameter("@Rate", SqlDbType.Int);

                _arrpara[3] = new SqlParameter("@PCMDID", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@IFINISHEDID", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@IUNITID", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@IQTY", SqlDbType.Float);
                _arrpara[7] = new SqlParameter("@ILOSS", SqlDbType.Float);
                _arrpara[8] = new SqlParameter("@IRATE", SqlDbType.Float);

                _arrpara[9] = new SqlParameter("@OFINISHEDID", SqlDbType.Int);
                _arrpara[10] = new SqlParameter("@OUNITID", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@OQTY", SqlDbType.Float);
                _arrpara[12] = new SqlParameter("@ORATE", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@PROCESSINPUTID", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@INOUTTYPEID", SqlDbType.Int);
                _arrpara[17] = new SqlParameter("@ICALTYPE", SqlDbType.Int);
                _arrpara[18] = new SqlParameter("@I_FINISHED_TYPE_ID", SqlDbType.Int);
                _arrpara[19] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);
                _arrpara[20] = new SqlParameter("@SUB_QUALITY_ID", SqlDbType.Int);
                _arrpara[21] = new SqlParameter("@OCALTYPE", SqlDbType.Int);
                _arrpara[22] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[23] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrpara[24] = new SqlParameter("@VarFlagMtrFt", SqlDbType.Int);
                _arrpara[25] = new SqlParameter("@RecLoss", SqlDbType.Float);
                _arrpara[26] = new SqlParameter("@VarPercentageFlag", SqlDbType.Int);
                _arrpara[27] = new SqlParameter("@IWeight", SqlDbType.Float);
                _arrpara[28] = new SqlParameter("@NetWeight", SqlDbType.Float);
                _arrpara[29] = new SqlParameter("@GrossWeight", SqlDbType.Float);
                _arrpara[30] = new SqlParameter("@ISizeflag", SqlDbType.Int);
                _arrpara[31] = new SqlParameter("@OSizeflag", SqlDbType.Int);
                _arrpara[32] = new SqlParameter("@Sizeflag", SqlDbType.TinyInt);
                _arrpara[33] = new SqlParameter("@OQtyPercentage", SqlDbType.Float);
                int num = 0;

                int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"SELECT PCMID FROM PROCESSCONSUMPTIONMASTER 
                WHERE PROCESSID=" + ddProcessName.SelectedValue + " AND FINISHEDID=" + Varfinishedid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Session["PCMID"] = Ds.Tables[0].Rows[0]["PCMID"];
                }
                else
                {
                    Session["PCMID"] = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMID ),0)+1 FROM PROCESSCONSUMPTIONMASTER"));
                    num = 1;
                }
                //IN TABLE PROCESSCONSUMPTIONDETAIL Field INOUTTYPEID 0 For ONE-TO-ONE,1 FOR MANY INPUT,2 FOR MANY OUTPUT

                _arrpara[0].Value = (Session["PCMID"]);
                _arrpara[1].Value = ddProcessName.SelectedValue;
                _arrpara[2].Value = Varfinishedid;

                int VarInfinishedid = UtilityModule.getItemFinishedId(ddInItemName, ddInQuality, ddInDesign, ddInColor, ddInShape, ddInSize, TxtOutProdCode, Tran, ddInShade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                _arrpara[4].Value = VarInfinishedid;
                _arrpara[5].Value = ddIUnit.SelectedValue;
                _arrpara[28].Value = 0;
                _arrpara[29].Value = 0;
                //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
                if (Session["varCompanyNo"].ToString() == "2")
                {
                    _arrpara[6].Value = TxtInPutQty.Text == "" ? "0" : TxtInPutQty.Text;
                    _arrpara[7].Value = TxtLoss.Text;
                    _arrpara[11].Value = TxtOutPutQty.Text;
                    _arrpara[25].Value = Math.Round(Convert.ToDouble(TxtRecLoss.Text), 5);
                    if (ChkForProcessInPut.Checked == true)
                    {
                        _arrpara[27].Value = 0;

                    }
                    else
                    {
                        _arrpara[27].Value = Math.Round(Convert.ToDouble(TxtIweight.Text == "" ? "0" : TxtIweight.Text), 5);
                        Decimal Iweight = Convert.ToDecimal(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select isnull(max(netweight),0)netweight from processconsumptionmaster where FINISHEDID=" + Varfinishedid));

                        Iweight = Iweight + (Convert.ToDecimal(TxtIweight.Text) * Convert.ToDecimal(TxtInPutQty.Text == "" ? "0" : TxtInPutQty.Text)) - Convert.ToDecimal(ViewState["SelectedIweight"]);
                        _arrpara[28].Value = Math.Round(Iweight, 5);
                        txtGrossweight.Text = txtGrossweight.Text == "0" ? (Convert.ToDecimal(TxtIweight.Text == "" ? "0" : TxtIweight.Text) * Convert.ToDecimal(TxtInPutQty.Text == "" ? "0" : TxtInPutQty.Text)).ToString() : txtGrossweight.Text;
                        _arrpara[29].Value = Math.Round(Convert.ToDecimal(txtGrossweight.Text == "" ? "0" : txtGrossweight.Text), 5);
                    }
                }
                else
                {

                    if (ChkForMtr.Checked == true)
                    {
                        _arrpara[6].Value = Math.Round(Convert.ToDouble(TxtInPutQty.Text) / 1.196, 5);
                        _arrpara[7].Value = Math.Round(Convert.ToDouble(TxtLoss.Text) / 1.196, 5);
                        _arrpara[11].Value = Math.Round(Convert.ToDouble(TxtOutPutQty.Text) / 1.196, 5);
                        _arrpara[25].Value = Math.Round(Convert.ToDouble(TxtRecLoss.Text) / 1.196, 5);
                    }
                    else
                    {
                        _arrpara[6].Value = Math.Round(Convert.ToDouble(TxtInPutQty.Text), 5);
                        _arrpara[7].Value = Math.Round(Convert.ToDouble(TxtLoss.Text), 5);
                        _arrpara[11].Value = Math.Round(Convert.ToDouble(TxtOutPutQty.Text), 5);
                        _arrpara[25].Value = Math.Round(Convert.ToDouble(TxtRecLoss.Text), 5);
                    }
                    _arrpara[27].Value = Math.Round(Convert.ToDouble(TxtIweight.Text == "" ? "0" : TxtIweight.Text) * Convert.ToDouble(_arrpara[6].Value), 5);
                }
                _arrpara[8].Value = TxtInPutRate.Text;

                int VarOutfinishedid = UtilityModule.getItemFinishedId(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutSize, TxtOutProdCode, Tran, ddOutShade, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                _arrpara[9].Value = VarOutfinishedid;
                _arrpara[10].Value = ddOUnit.SelectedValue;
                _arrpara[12].Value = TxtOutPutRate.Text;
                _arrpara[13].Value = ChkForProcessInPut.Checked == true ? Convert.ToInt32(ddInProcessName.SelectedValue) : 0;
                _arrpara[14].Value = ChkForManyOutPut.Checked == true ? 2 : ChkForOneToOne.Checked == true ? 0 : 1;
                _arrpara[15].Value = 0;
                _arrpara[16].Value = 0;
                _arrpara[17].Value = DDICALTYPE.SelectedValue;
                _arrpara[18].Value = 0;
                _arrpara[19].Value = 0;
                _arrpara[20].Value = 0;
                if (ddSub_Quality.Visible == true)
                {
                    _arrpara[18].Value = 0;
                    _arrpara[19].Value = 0;
                    _arrpara[20].Value = ddSub_Quality.SelectedValue;
                }
                if (ddFinishedIN.Visible == true)
                {
                    _arrpara[18].Value = ddFinishedIN.SelectedValue;
                    _arrpara[19].Value = ddFinished.SelectedValue;
                }
                _arrpara[21].Value = ddOCalType.SelectedValue;
                _arrpara[22].Value = Session["varuserid"].ToString();
                _arrpara[23].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrpara[24].Value = ChkForMtr.Checked == true ? 1 : 2;
                _arrpara[26].Value = ChkForLossPercentage.Checked == true ? 1 : 0;
                _arrpara[30].Value = getSizeType(DDInSizeType, InSize);
                _arrpara[31].Value = getSizeType(DDOutSizeType, OutSize);
                _arrpara[32].Value = getSizeType(DDsizetype, Size);
                _arrpara[33].Value = txtOutputQtyPercentage.Text == "" ? "0" : txtOutputQtyPercentage.Text;

                // LblNetWeightVal.Text = LblNetWeightVal.Text == "0" ? (Convert.ToDecimal(TxtIweight.Text == "" ? "0" : TxtIweight.Text) * Convert.ToDecimal(TxtInPutQty.Text==""?"0":TxtInPutQty.Text)).ToString() : LblNetWeightVal.Text;
                //txtGrossweight.Text = txtGrossweight.Text == "0" ? (Convert.ToDecimal(TxtIweight.Text == "" ? "0" : TxtIweight.Text) * Convert.ToDecimal(TxtInPutQty.Text == "" ? "0" : TxtInPutQty.Text)).ToString() : txtGrossweight.Text;
                //_arrpara[29].Value = Math.Round(Convert.ToDecimal(txtGrossweight.Text == "" ? "0" : txtGrossweight.Text), 3);

                if (Session["varMasterCompanyIDForERP"].ToString() == "2")
                {

                    if (Convert.ToDecimal(_arrpara[28].Value) > Convert.ToDecimal(_arrpara[29].Value))
                    {
                        if (BtnSave.Text == "UpDate")
                        {
                            msg = "Gross weight can't be less than Net weight !";
                            LblNetWeightVal.Text = _arrpara[29].Value.ToString();
                            lblMessage.Text = "Gross weight can't be less than Net weight !";
                            MessageSave(msg);
                            return;
                        }
                        else
                        {
                            _arrpara[29].Value = _arrpara[28].Value;
                        }

                    }
                }
                if (BtnSave.Text == "Save")
                {
                    _arrpara[3].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMDID ),0)+1 FROM PROCESSCONSUMPTIONDETAIL"));

                }
                else
                {
                    _arrpara[3].Value = DG.SelectedValue;
                }
                if (TxtRemarks.Text != "" && TxtRemarks.Visible == true)
                {
                    Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "SELECT * FROM MAIN_ITEM_IMAGE WHERE FINISHEDID=" + _arrpara[2].Value + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "Update MAIN_ITEM_IMAGE Set Remarks='" + TxtRemarks.Text.ToUpper() + "' WHERE FINISHEDID=" + _arrpara[2].Value + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "INSERT INTO MAIN_ITEM_IMAGE(FINISHEDID,REMARKS,MasterCompanyId) VALUES(" + _arrpara[2].Value + ",'" + TxtRemarks.Text.ToUpper() + "'," + Session["varMasterCompanyIDForERP"] + ")");
                    }
                }
                if (num == 1)
                {
                    //SELECT PCMID,PROCESSID,FINISHEDID FROM PROCESSCONSUMPTIONMASTER
                    //SELECT PCMID,PCMDID,IFINISHEDID,IUNITID,IQTY,ILOSS,IRATE,OFINISHEDID,OUNITID,OQTY,ORATE,PROCESSINPUTID,INOUTTYPEID FROM PROCESSCONSUMPTIONDETAIL
                    str = @"INSERT INTO PROCESSCONSUMPTIONMASTER(PCMID,PROCESSID,FINISHEDID,CALTYPE,RATE,userid,MasterCompanyid,FlagMtrFt,LossPercentageFlag,NETWEIGHT,GROSSWEIGHT,Sizeflag) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + "," + _arrpara[22].Value + "," + _arrpara[23].Value + "," + _arrpara[24].Value + "," + _arrpara[26].Value + "," + _arrpara[28].Value + "," + _arrpara[29].Value + "," + _arrpara[32].Value + ")";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }

                if (ChkForLossPercentage.Checked == true)
                {
                    _arrpara[7].Value = Math.Round(Convert.ToDouble(_arrpara[6].Value) * Convert.ToDouble(TxtLoss.Text == "" ? "0" : TxtLoss.Text) / 100, 5);
                    _arrpara[25].Value = Math.Round(Convert.ToDouble(_arrpara[11].Value) * Convert.ToDouble(TxtRecLoss.Text == "" ? "0" : TxtRecLoss.Text) / 100, 5);
                }

                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "SELECT IFINISHEDID FROM PROCESSCONSUMPTIONDETAIL WHERE IFINISHEDID=" + VarInfinishedid +
                    " AND OFINISHEDID=" + VarOutfinishedid + " AND PCMID=" + Session["PCMID"] + " AND I_FINISHED_TYPE_ID=" + _arrpara[18].Value + " AND O_FINISHED_TYPE_ID=" + _arrpara[19].Value + " AND PCMDID!=" + _arrpara[3].Value + "");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string StrQry = " ";
                    StrQry = "Update processconsumptionmaster SET GROSSWEIGHT=" + _arrpara[29].Value + ", NETWEIGHT=" + _arrpara[28].Value + " WHERE FINISHEDID=" + Varfinishedid + " AND PROCESSID=" + ddProcessName.SelectedValue;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrQry);
                    Tran.Commit();
                    lblMessage.Visible = true;
                    lblMessage.Text = "DATA ALLREADY EXISTS.....";
                }
                else
                {
                    string StrQry = " ";
                    StrQry = "Update processconsumptionmaster SET LossPercentageFlag=" + _arrpara[26].Value + ",GROSSWEIGHT=" + _arrpara[29].Value + ", NETWEIGHT=" + _arrpara[28].Value + " WHERE FINISHEDID=" + Varfinishedid + " AND PROCESSID=" + ddProcessName.SelectedValue;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrQry);
                    #region
                    //LblNetWeightVal.Text = Convert.ToDecimal(LblNetWeightVal.Text) >= 0 ? (Convert.ToDecimal(TxtIweight.Text == "" ? "0" : TxtIweight.Text) * Convert.ToDecimal(TxtInPutQty.Text == "" ? "0" : TxtInPutQty.Text)).ToString() : LblNetWeightVal.Text;
                    // _arrpara[28].Value = LblNetWeightVal.Text;
                    //if (Convert.ToDecimal(_arrpara[28].Value) <= Convert.ToDecimal(_arrpara[29].Value))
                    //{

                    //    StrQry = "Update processconsumptionmaster SET GROSSWEIGHT=" + _arrpara[29].Value + ", NETWEIGHT=" + _arrpara[28].Value + " WHERE FINISHEDID=" + Varfinishedid + " AND PROCESSID=" + ddProcessName.SelectedValue;
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrQry);
                    //}
                    //else
                    //{
                    //    if (BtnSave.Text == "UpDate")
                    //    {
                    //        msg = "Gross weight can't be less than Net weight !";
                    //        MessageSave(msg);
                    //          return;
                    //    }
                    //    else
                    //    {
                    //        StrQry = "Update processconsumptionmaster SET GROSSWEIGHT=" + _arrpara[28].Value + ", NETWEIGHT=" + _arrpara[28].Value + " WHERE FINISHEDID=" + Varfinishedid + " AND PROCESSID=" + ddProcessName.SelectedValue;
                    //        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrQry);
                    //    }

                    //}
                    #endregion
                    if (BtnSave.Text == "UpDate")
                    {
                        StrQry = " ";
                        StrQry = "  DELETE PROCESSCONSUMPTIONDETAIL WHERE PCMDID=" + _arrpara[3].Value;
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, StrQry);
                    }
                    string DyingMatch = "", DyingType = "", Dyeing = "";
                    if (TDDyeingMatch.Visible == true)
                    {
                        DyingMatch = DDDyeingMatch.SelectedItem.Text.Trim();
                        DyingType = DDDyingType.SelectedItem.Text.Trim();
                        Dyeing = DDDyeing.SelectedItem.Text.Trim();
                    }
                    str = @"INSERT INTO PROCESSCONSUMPTIONDETAIL(PCMID,PCMDID,IFINISHEDID,IUNITID,IQTY,ILOSS,IRATE,OFINISHEDID,OUNITID,OQTY,ORATE,PROCESSINPUTID,INOUTTYPEID,ICALTYPE,I_FINISHED_Type_ID,O_FINISHED_Type_ID,SUB_QUALITY_ID,OCALTYPE,OLoss,IWEIGHT,ISizeflag,OSizeflag,DyingMatch,DyingType,Dyeing,OQtyPercentage,AddedDate) VALUES(
                    " + _arrpara[0].Value + "," + _arrpara[3].Value + "," + _arrpara[4].Value + "," + _arrpara[5].Value + "," + _arrpara[6].Value +
                    "," + _arrpara[7].Value + "," + _arrpara[8].Value + "," + _arrpara[9].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value +
                    "'," + _arrpara[12].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[17].Value + "," + _arrpara[18].Value + "," + _arrpara[19].Value +
                    "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[25].Value + "," + _arrpara[27].Value + "," + _arrpara[30].Value + "," + _arrpara[31].Value +
                    ",'" + DyingMatch + "','" + DyingType + "','" + Dyeing + "'," + _arrpara[33].Value + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    //Process Include Qty 
                    switch (ddProcessName.SelectedItem.Text.ToUpper())
                    {
                        case "PURCHASE":
                            if (variable.BOM_PurchaseIncludeProcess == "1")
                            {

                                //*****sql Table
                                DataTable dtrecords = new DataTable();
                                dtrecords.Columns.Add("Processid", typeof(int));
                                dtrecords.Columns.Add("Consmpqty", typeof(float));
                                //*****
                                for (int i = 0; i < GDPurchaseinclude.Rows.Count; i++)
                                {
                                    CheckBox Chkbox = (CheckBox)GDPurchaseinclude.Rows[i].FindControl("Chkbox");
                                    TextBox txtincludeqty = (TextBox)GDPurchaseinclude.Rows[i].FindControl("txtincludeqty");
                                    if (Chkbox.Checked == true && Convert.ToDouble(txtincludeqty.Text == "" ? "0" : txtincludeqty.Text) > 0)
                                    {
                                        Label lblprocessid = (Label)GDPurchaseinclude.Rows[i].FindControl("lblprocessid");
                                        DataRow dr = dtrecords.NewRow();
                                        dr["Processid"] = lblprocessid.Text;
                                        dr["Consmpqty"] = txtincludeqty.Text;
                                        dtrecords.Rows.Add(dr);
                                    }
                                }
                                //***************Check
                                if (dtrecords.Rows.Count > 0)
                                {
                                    SqlParameter[] Param = new SqlParameter[11];
                                    Param[0] = new SqlParameter("@Finishedid", Varfinishedid);
                                    Param[1] = new SqlParameter("@Sizeflag", _arrpara[32].Value);
                                    Param[2] = new SqlParameter("@ProcessId", ddProcessName.SelectedValue);
                                    Param[3] = new SqlParameter("@IFinishedid", VarInfinishedid);
                                    Param[4] = new SqlParameter("@Iunitid", ddIUnit.SelectedValue);
                                    Param[5] = new SqlParameter("@ISizeflag", _arrpara[30].Value);
                                    Param[6] = new SqlParameter("@ICaltype", DDICALTYPE.SelectedValue);
                                    Param[7] = new SqlParameter("@userid", Session["varuserid"]);
                                    Param[8] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                                    Param[9] = new SqlParameter("@dtrecords", dtrecords);
                                    Param[10] = new SqlParameter("@mtrflag", ChkForMtr.Checked == true ? 1 : 2);
                                    //**************
                                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVEBOM_ProcessItemInclude", Param);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    //********************
                    Tran.Commit();
                    LblQty.Text = (Convert.ToDouble(LblQty.Text) - Convert.ToDouble(TxtOutPutQty.Text)).ToString();
                    lblMessage.Visible = true;
                    lblMessage.Text = "DATA SUCCESSFULLY SAVED.....";
                    BtnSave.Text = "Save";
                }
                FillNet_GrossWeight(Varfinishedid);
                //                string Qry = @"SELECT DISTINCT Sum(isnull(PCD.IWEIGHT,0)) NetWeight,ISNULL(PCM.GROSSWEIGHT,0) GROSSWEIGHT  FROM PROCESSCONSUMPTIONMASTER PCM,PROCESSCONSUMPTIONdetail PCD where PCM.PCMID=PCD.PCMID AND 
                //                                PCD.PCMID in (select Distinct PCMID From PROCESSCONSUMPTIONMASTER Where FINISHEDID=" + Varfinishedid + " AND PROCESSID=" + ddProcessName.SelectedValue + ") group by PCM.GROSSWEIGHT";
                //                Ds = SqlHelper.ExecuteDataset (ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
                //                if (Ds.Tables[0].Rows.Count > 0)
                //                {
                //                    LblNetWeightVal.Text = Ds.Tables[0].Rows[0]["NetWeight"].ToString();
                //                    txtGrossweight.Text = Ds.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();
                //                }
                VISIABLE_TRUE_FALSE_ACC_TO_MANY_REC();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
                Tran.Rollback();
                lblMessage.Visible = true;
                //lblMessage.Text =
            }
            finally
            {
                Save_Refresh();
                FILLGRID();
                ViewState["SelectedIweight"] = 0;
                //Process Include Qty 
                if (ddProcessName.SelectedItem.Text.ToUpper() == "PURCHASE")
                {
                    if (variable.BOM_PurchaseIncludeProcess == "1")
                    {
                        FillProcessForpurchaseinclude();
                    }
                }
            }
        }
    }
    private void FillNet_GrossWeight(int Varfinishedid)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "2")
        {
            string Qry = @"select isnull(max(NETWEIGHT),0) NETWEIGHT,isnull(max(GROSSWEIGHT),0) GROSSWEIGHT from processconsumptionmaster where FINISHEDID=" + Varfinishedid;
            //        string Qry = @"SELECT DISTINCT Sum(isnull(PCD.IWEIGHT,0)) NetWeight,ISNULL(PCM.GROSSWEIGHT,0) GROSSWEIGHT  FROM PROCESSCONSUMPTIONMASTER PCM,PROCESSCONSUMPTIONdetail PCD where PCM.PCMID=PCD.PCMID AND 
            //                                PCD.PCMID in (select Distinct PCMID From PROCESSCONSUMPTIONMASTER Where FINISHEDID=" + Varfinishedid + ") group by PCM.GROSSWEIGHT";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                LblNetWeightVal.Text = Ds.Tables[0].Rows[0]["NetWeight"].ToString();
                txtGrossweight.Text = Ds.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();

                if (txtGrossweight.Text == "" || txtGrossweight.Text == "0")
                {
                    txtGrossweight.Text = LblNetWeightVal.Text;
                }

            }

        }
    }
    private void CHECKVALIDCONTROLFOR_INPUT_PROCESS()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;

        }
        if (ddQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (ddDesign.Visible == true && CHKFORALLDESIGN.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true && CHKFORALLCOLOR.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        if (ddShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
            {
                goto a;
            }
        }
        if (ddSize.Visible == true && CHKFORALLSIZE.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (ddShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddProcessName) == false)
        {
            goto a;
        }
        if (ddSub_Quality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSub_Quality) == false)
            {
                goto a;
            }
        }
        if (ChkForProcessInPut.Checked == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInProcessName) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOutCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOutItemName) == false)
        {
            goto a;
        }
        if (ddOutQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutQuality) == false)
            {
                goto a;
            }
        }
        if (ddOutDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutDesign) == false)
            {
                goto a;
            }
        }
        if (ddOutColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutColor) == false)
            {
                goto a;
            }
        }
        if (ddOutShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutShape) == false)
            {
                goto a;
            }
        }
        if (ddOutSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutSize) == false)
            {
                goto a;
            }
        }
        if (ddOutShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutShade) == false)
            {
                goto a;
            }
        }
        if (ddSub_Quality.Visible == false && ddFinished.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddFinished) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtOutPutQty) == false)
        {
            goto a;
        }
        if (TxtOutPutRate.Text == "")
        {
            TxtOutPutRate.Text = "0";
        }
        if (TxtRecLoss.Text == "")
        {
            TxtRecLoss.Text = "0";
        }
        if (TxtLoss.Text == "")
        {
            TxtLoss.Text = "0";
        }
        if (UtilityModule.VALIDTEXTBOX(TxtOutPutRate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddItemName) == false)
        {
            goto a;

        }
        if (ddQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddQuality) == false)
            {
                goto a;
            }
        }
        if (ddDesign.Visible == true && CHKFORALLDESIGN.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true && CHKFORALLCOLOR.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddColor) == false)
            {
                goto a;
            }
        }
        //if (ddShape.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(ddShape) == false)
        //    {
        //        goto a;
        //    }
        //}
        if (ddSize.Visible == true && CHKFORALLSIZE.Checked == false)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSize) == false)
            {
                goto a;
            }
        }
        if (ddShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddShade) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddProcessName) == false)
        {
            goto a;
        }
        if (ddSub_Quality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddSub_Quality) == false)
            {
                goto a;
            }
        }
        //if (UtilityModule.VALIDDROPDOWNLIST(ddCalType) == false)
        //{
        //    goto a;
        //}

        //if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        //{
        //    goto a;
        //}

        if (UtilityModule.VALIDDROPDOWNLIST(ddOutCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOutItemName) == false)
        {
            goto a;
        }
        if (ddOutQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutQuality) == false)
            {
                goto a;
            }
        }
        if (ddOutDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutDesign) == false)
            {
                goto a;
            }
        }
        if (ddOutColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutColor) == false)
            {
                goto a;
            }
        }
        if (ddOutShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutShape) == false)
            {
                goto a;
            }
        }
        if (ddOutSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutSize) == false)
            {
                goto a;
            }
        }
        if (ddOutShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddOutShade) == false)
            {
                goto a;
            }
        }
        if (ddFinished.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddFinished) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddOUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtOutPutQty) == false)
        {
            goto a;
        }
        if (TxtOutPutRate.Text == "")
        {
            TxtOutPutRate.Text = "0";
        }
        if (TxtRecLoss.Text == "")
        {
            TxtRecLoss.Text = "0";
        }
        if (UtilityModule.VALIDTEXTBOX(TxtOutPutRate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddInCategoryName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddInItemName) == false)
        {
            goto a;
        }
        if (ddInQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInQuality) == false)
            {
                goto a;
            }
        }
        if (ddInDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInDesign) == false)
            {
                goto a;
            }
        }
        if (ddInColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInColor) == false)
            {
                goto a;
            }
        }
        if (ddInShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInShape) == false)
            {
                goto a;
            }
        }
        if (ddInSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInSize) == false)
            {
                goto a;
            }
        }
        if (ddInShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddInShade) == false)
            {
                goto a;
            }
        }
        if (ddFinishedIN.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddFinishedIN) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddIUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInPutQty) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(TxtInPutRate) == false)
        //{
        //    goto a;
        //}        
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessage);
    B: ;
        if (TxtLoss.Text == "")
        {
            TxtLoss.Text = "0";
        }
        if (TxtInPutRate.Text == "")
        {
            TxtInPutRate.Text = "0";
        }
    }
    private void Save_Refresh()
    {
        TxtInProdCode.Text = "";
        TxtOutProdCode.Text = "";
        if (ChkForManyOutPut.Checked == true)
        {
            int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            switch (VarCompanyNo)
            {
                case 1:
                    ddOutShade.SelectedIndex = 0;
                    TxtOutPutQty.Text = "";
                    TxtOutPutRate.Text = "";
                    ddOutShade.Focus();
                    break;
                case 2:
                    ddOutCategoryName.SelectedIndex = 0;
                    OUT_CATEGORY_DEPENDS_CONTROLS();
                    ddOUnit.SelectedIndex = 0;
                    TxtOutPutQty.Text = "";
                    TxtOutPutRate.Text = "";
                    break;
                case 4:
                    ddOutShade.SelectedIndex = 0;
                    TxtOutPutQty.Text = "";
                    TxtOutPutRate.Text = "";
                    break;
                case 9:
                    ddOutShade.SelectedIndex = 0;
                    TxtOutPutQty.Text = "";
                    TxtOutPutRate.Text = "";
                    break;
            }


        }
        else if (ChkForOneToOne.Checked == true)
        {
            if (ChkForFillSame.Checked == false)
            {
                ddInCategoryName.SelectedIndex = 0;
                IN_CATEGORY_DEPENDS_CONTROLS();
                ddIUnit.SelectedIndex = 0;
                TxtInPutQty.Text = "";
                TxtLoss.Text = "";
                TxtInPutRate.Text = "";
                ddOutCategoryName.SelectedIndex = 0;
                OUT_CATEGORY_DEPENDS_CONTROLS();
                ddOUnit.SelectedIndex = 0;
                TxtOutPutQty.Text = "";
                TxtOutPutRate.Text = "";
            }
        }
        else
        {
            ddInCategoryName.SelectedIndex = 0;
            IN_CATEGORY_DEPENDS_CONTROLS();
            ddIUnit.SelectedIndex = 0;
            TxtInPutQty.Text = "";
            TxtLoss.Text = "";
            TxtInPutRate.Text = "";
        }
    }
    private void VISIABLE_TRUE_FALSE_ACC_TO_MANY_REC()
    {
        if (ChkForManyOutPut.Checked == true)
        {
            Tr1.Disabled = true;
            Tr2.Disabled = true;
            Tr3.Disabled = true;
            //TrOUT.Disabled = false;
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
        }
        else if (ChkForOneToOne.Checked == true)
        {
            ChkForManyOutPut.Checked = false;
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
            Tr1.Disabled = false;
            Tr2.Disabled = false;
            Tr3.Disabled = false;
        }
        else
        {
            Tr1.Disabled = false;
            Tr2.Disabled = false;
            Tr3.Disabled = false;
            //TrOUT.Disabled = true;
            TrOUT1.Disabled = true;
            TrOUT2.Disabled = true;
        }
    }

    protected void ChkForProcessInPut_CheckedChanged(object sender, EventArgs e)
    {
        ProcessInputChecked();
    }
    private void ProcessInputChecked()
    {
        if (ChkForProcessInPut.Checked == true)
        {
            INPROCESSNAME.Visible = true;
            UtilityModule.ConditionalComboFill(ref ddInProcessName, "SELECT DISTINCT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PCM,PROCESS_NAME_MASTER PM WHERE PCM.PROCESSID=PM.PROCESS_NAME_ID AND PCM.PROCESSID<>" + ddProcessName.SelectedValue + " And PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PROCESS_NAME", true, "--SELECT--");
            DGInPutProcess.Visible = true;
            //Tr1.Disabled = true;
            //Tr3.Disabled = true;
            //TrOUT1.Disabled = false;
            //TrOUT2.Disabled = false;
            ddInProcessName.Focus();
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER  Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Category_Name", true, "--SELECT--");
            INPROCESSNAME.Visible = false;
            DGInPutProcess.Visible = false;
            Tr1.Disabled = false;
            Tr3.Disabled = false;
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
        }
    }
    protected void ddInProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {

        InProcessNameSelectedIndChange();
        TxtInProdCode.Focus();
        //Session["varMasterCompanyIDForERP"] = "4";
        switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
        {
            case 6:
                break;
            default:

                //ScriptManager.RegisterClientScriptBlock(this, typeof(string), "checkbox", "CheckAllCheckBoxes", true);

                //ClientScript.RegisterStartupScript(this.GetType(), "JSScript", "CheckAllCheckBoxes", true);
                break;
        }
        // Session["varMasterCompanyIDForERP"] = "6";
    }
    private void InProcessNameSelectedIndChange()
    {
        UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT ICM.Category_Id,ICM.Category_Name FROM ITEM_CATEGORY_MASTER ICM,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=IFINISHEDID AND IPM.ITEM_ID=IM.ITEM_ID AND IM.CATEGORY_ID=ICM.CATEGORY_ID AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By ICM.Category_Name", true, "--SELECT--");
        fill_grid1();
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddProcessName.SelectedIndex > 0 && ddQuality.SelectedIndex > 0 && ddDesign.SelectedIndex > 0 && ddColor.SelectedIndex > 0 && ddShape.SelectedIndex > 0 && ddSize.SelectedIndex > 0)
        {
            //FILLGRID();
        }
        if (ChkForFillSame.Checked == true)
        {
            ddInSize.SelectedValue = ddSize.SelectedValue;
            ddOutSize.SelectedValue = ddSize.SelectedValue;
        }
    }
    private void FILLGRID()
    {
        DG.DataSource = "";
        String STR = @"select PCD.PCMDID,PM.PROCESS_NAME,VD1.Category_Name+space(2)+VD1.Item_Name+space(2)+VD1.QualityName+ Space(2) +VD1.DesignName+ Space(2) +VD1.ColorName+ Space(2) +VD1.ShapeName+SPACE(2)+ 
                        case When PCM.sizeflag=0 Then  VD1.SIZEFT When PCM.sizeflag=1 Then VD1.SizeMtr When PCM.sizeflag=2 Then VD1.sizeinch Else VD2.Sizeft End+SPACE(2)+VD1.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+' '+case When vD1.SizeId>0 Then ST.Type Else '' End as ITEMCODE,
                        VD2.Category_Name+space(2)+VD2.Item_Name+space(2)+VD2.QualityName+ Space(2) +VD2.DesignName+ Space(2) +VD2.ColorName+ Space(2) +VD2.ShapeName+SPACE(2)+ case When PCD.ISizeflag=0 Then  VD2.SIZEFT 
                        When pcd.ISizeflag=1 Then VD2.SizeMtr When PCD.ISizeflag=2 Then VD2.sizeinch  Else VD2.sizeft End+SPACE(2)+VD2.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+case When VD2.sizeid>0 Then STI.type else '' End INPUT_ITEM,Round(case When PCM.FlagMtrFt=1 then IQTY*1.196 else IQTY End,3) IQTY,
                        Round(Case When PCM.LossPercentageFlag=1 Then Round(PCD.ILOSS/PCD.IQTY*100,0) Else Case When PCM.FlagMtrFt=1 then ILOSS*1.196 else ILOSS End End,3) ILOSS,
                        IRATE,IU.UNITNAME I_UNIT,VD3.Category_Name+space(2)+VD3.Item_Name+space(2)+VD3.QualityName+ Space(2) +VD3.DesignName+ Space(2) +VD3.ColorName+ Space(2) +VD3.ShapeName+SPACE(2)+case When PCD.OSizeflag=0 Then  VD2.SIZEFT 
                        When pcd.OSizeflag=1 Then VD3.SizeMtr When PCD.OSizeflag=2 Then VD3.sizeinch  Else VD3.sizeft End+SPACE(2)+VD3.SHADECOLORName+
                        SPACE(5)+Isnull(FT.FINISHED_TYPE_NAME,'')+case When VD3.sizeid>0 Then STO.type else '' End OUTPUT_ITEM,Round(Case When PCM.FlagMtrFt=1 then OQTY*1.196 else OQTY End,3) OQTY,ORATE,
                        OU.UNITNAME O_UNIT,QCM.QUALITYCODEID,PCM.FlagMtrFt,Dyingmatch,DyingType,Dyeing,case When PM.Process_Name='PURCHASE' and " + variable.BOM_PurchaseIncludeProcess + @"=1 Then dbo.F_GetBOMProcessIncluding(PCM.Finishedid,PCD.Ifinishedid,PCM.Processid) Else '' End as INCLUDING_PROCESS,
                        Round(Case When PCM.FlagMtrFt=1 then OLOSS*1.196 else OLOSS End,3) OLOSS,isnull(PCD.OQtyPercentage,'') OQtyPercentage
                        from PROCESSCONSUMPTIONMASTER PCM inner Join PROCESSCONSUMPTIONDETAIL PCD
                        on PCm.PCMID=PCD.PCMID
                        inner join ITEM_PARAMETER_MASTER IPM on IPM.ITEM_FINISHED_ID=PCM.FINISHEDID
                        inner join V_FinishedItemDetail VD1  on IPM.ITEM_FINISHED_ID=VD1.Item_FINISHED_ID and PCM.FINISHEDID=VD1.Item_Finished_id
                        inner join V_FinishedItemDetail VD2  on PCD.IFINISHEDID=Vd2.ITEM_FINISHED_ID
                        inner join V_FinishedItemDetail VD3 on  pCd.OFINISHEDID=vd3.ITEM_FINISHED_ID
                        inner join UNIT IU on PCD.IUNITID=IU.UnitId
                        inner join Unit OU on Pcd.OUNITID=ou.UnitId
                        LEFT OUTER JOIN qualityCodeMaster QCM ON  PCD.SUB_QUALITY_ID=QCM.QUALITYCODEID
                        LEFT OUTER JOIN FINISHED_TYPE FT ON PCD.I_FINISHED_TYPE_ID=FT.ID 
                        LEFT OUTER JOIN FINISHED_TYPE FT1  ON PCD.O_FINISHED_TYPE_ID=FT1.ID 
                        inner join PROCESS_NAME_MASTER PM on PM.PROCESS_NAME_ID=PCM.PROCESSID
                        inner join SizeType ST on PCM.sizeflag=ST.Val
                        inner join Sizetype STI on PCD.ISizeflag=STI.Val
                        inner join SizeType STO on Pcd.OSizeflag=STO.Val where PCM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (ddQuality.SelectedIndex > 0)
        {
            STR = STR + @" AND QUALITY_ID=" + ddQuality.SelectedValue + "";
        }

        if (CHKFORALLDESIGN.Checked == true)
        {
            STR = STR + @" AND DESIGN_ID=-1";
        }
        else if (ddDesign.SelectedIndex > 0)
        {
            STR = STR + @" AND DESIGN_ID=" + ddDesign.SelectedValue + "";

        }
        if (CHKFORALLCOLOR.Checked == true)
        {
            STR = STR + @" AND COLOR_ID=-1";
        }
        else if (ddColor.SelectedIndex > 0)
        {
            STR = STR + @" AND COLOR_ID=" + ddColor.SelectedValue + "";
        }
        if (ddShape.SelectedIndex > 0)
        {
            STR = STR + @" AND SHAPE_ID=" + ddShape.SelectedValue + "";
        }
        if (CHKFORALLSIZE.Checked == true)
        {
            STR = STR + @" AND SIZE_ID=-1";
        }
        else if (ddSize.SelectedIndex > 0)
        {
            STR = STR + @" AND SIZE_ID=" + ddSize.SelectedValue + "";
        }
        if (ddProcessName.SelectedIndex > 0)
        {
            STR = STR + @" AND PCM.PROCESSID=" + ddProcessName.SelectedValue + "";
        }
        STR = STR + " order by PCD.PCMDID";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
        DG.DataSource = Ds;
        DG.DataBind();
        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["FlagMtrFt"]) == 1)
            {
                ChkForMtr.Checked = true;
            }
        }
        int VARFINISHEDID = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        //Comment on 10-Dec-2016 MK
        //GET_FORMULAFEILD(VARFINISHEDID);
        //End

        //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (Session["varCompanyNo"].ToString() == "2")
        {
            #region
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, @" Delete From Temp_MAIN_ITEM_IMAGE
                    Insert INTO Temp_MAIN_ITEM_IMAGE (FINISHEDID,PHOTO,Remarks,brass,Iron,Glass,NETWEIGHT,MasterCompanyId) select FINISHEDID,PHOTO,Remarks,brass,Iron,Glass,NETWEIGHT,MasterCompanyId from MAIN_ITEM_IMAGE where FINISHEDID=" + VARFINISHEDID + @"
                    select FINISHEDID,PHOTO,Remarks,brass,Iron,Glass,NETWEIGHT,MasterCompanyId from MAIN_ITEM_IMAGE where FINISHEDID=" + VARFINISHEDID);

            ds1.Tables[0].Columns.Add("ImageThumbNail", typeof(System.Byte[]));
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                if (Convert.ToString(dr["Photo"]) != "")
                {
                    //FileInfo TheFile = new FileInfo(Server.MapPath("~/PurchaseImage/") + dr["PIndentDetailId"] + "_PindentImage.gif");
                    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                    if (TheFile.Exists)
                    {
                        string img = dr["Photo"].ToString();
                        img = Server.MapPath(img);
                        Byte[] img_Byte = File.ReadAllBytes(img);
                        dr["ImageThumbNail"] = img_Byte;
                        SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                        myConnection.Open();
                        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, "UPDATE  Temp_MAIN_ITEM_IMAGE SET ImageThumbNail =" + ds1.Tables[0].Rows[0]["ImageThumbNail"] + " Where FINISHEDID=" + VARFINISHEDID + "");
                        SqlCommand storeimage = new SqlCommand("UPDATE  Temp_MAIN_ITEM_IMAGE SET ImageThumbNail =@image Where FINISHEDID=" + VARFINISHEDID + "", myConnection);
                        storeimage.Parameters.Add("@image", SqlDbType.Image, img_Byte.Length).Value = img_Byte;
                        //System.Drawing.Image img = System.Drawing.Image.FromStream(PhotoImage.PostedFile.InputStream);
                        storeimage.ExecuteNonQuery();
                    }
                }
                //if (Convert.ToString(dr["PHOTO"]) != ""
                //{
                //    FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                //    if (TheFile.Exists)
                //    {
                //        string img = dr["Photo"].ToString();
                //        img = Server.MapPath(img);
                //        Byte[] img_Byte = File.ReadAllBytes(img);
                //        dr["ImageThumbNail"] = img_Byte;
                //        SqlCommand storeimage = new SqlCommand("UPDATE  Temp_MAIN_ITEM_IMAGE SET ImageThumbNail = @image Where FINISHEDID=" + VARFINISHEDID, con);
                //        storeimage.Parameters.Add("@image", SqlDbType.Image, img.Length).Value = img_Byte;
                //       // System.Drawing.Image ee = System.Drawing.Image.FromFile(img_Byte);
                //        //System.Drawing.Image image = System.Drawing.Image.FromStream(img_Byte);
                //        storeimage.ExecuteNonQuery();
                //    }
                //}
            }
            //                string Qry = @"Insert INTO Temp_MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,Remarks,brass,Iron,Glass,NETWEIGHT,MasterCompanyId,ImageThumbNail ) Values
            //                       " + (ds1.Tables[0].Rows[0]["FINISHEDID"].ToString() + "," + ds1.Tables[0].Rows[0]["PHOTO"].ToString() + "," + ds1.Tables[0].Rows[0]["Remarks"].ToString() + "," + @"
            //" + ds1.Tables[0].Rows[0]["brass"].ToString() + "," + ds1.Tables[0].Rows[0]["Iron"].ToString() + "," + ds1.Tables[0].Rows[0]["Glass"].ToString() + "," + ds1.Tables[0].Rows[0]["NETWEIGHT"].ToString() + @",
            //                " + ds1.Tables[0].Rows[0]["MasterCompanyId"].ToString() + "," + ds1.Tables[0].Rows[0]["ImageThumbNail"].ToString());

            //    select FINISHEDID,PHOTO,Remarks,brass,Iron,Glass,NETWEIGHT,MasterCompanyId from MAIN_ITEM_IMAGE where FINISHEDID=" + VARFINISHEDID +@"
            //string Qry = @" Update Temp_MAIN_ITEM_IMAGE SET ImageThumbNail=" + Ds.Tables[0].Rows[0]["ImageThumbNail"].ToString();

            // SqlHelper.ExecuteNonQuery(con, CommandType.Text, Qry);
            #endregion
            {
                Session["ReportPath"] = "Reports/DefineCostingOfAItem2.rpt";
                Session["CommanFormula"] = "{VIEW_COSTINGORDERDETAIL.ITEM_FINISHED_ID}= " + VARFINISHEDID + "";
            }
        }
        else if (Session["varCompanyNo"].ToString() == "43")
        {
            Session["ReportPath"] = "Reports/DefineBomAndComsumptionCarpetInternational.rpt";
            Session["CommanFormula"] = "{PROCESSCONSUMPTIONMASTER.FINISHEDID}= " + VARFINISHEDID + "";
        }
        else
        {
            Session["ReportPath"] = "Reports/DefineBomAndComsumption.rpt";
            Session["CommanFormula"] = "{PROCESSCONSUMPTIONMASTER.FINISHEDID}= " + VARFINISHEDID + "";
        }
        //}
    }
    private void ApprovalDetail(int finishedID)
    {
        string Qry = "select Distinct CurrencyID,CurRate,NetCost,ApprovedAmount from Item_Approval_Detail Where FinishedID=" + finishedID;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtTotalCost.Text = ds.Tables[0].Rows[0]["NetCost"].ToString();
            DDCurrency.SelectedValue = ds.Tables[0].Rows[0]["CurrencyID"].ToString();
            TxtCurRate.Text = ds.Tables[0].Rows[0]["CurRate"].ToString();
            TxtApprovalCost.Text = Math.Round((Convert.ToDecimal(ds.Tables[0].Rows[0]["NetCost"].ToString()) / Convert.ToDecimal(ds.Tables[0].Rows[0]["CurRate"].ToString())), 2).ToString();
        }
    }
    private void TotalAmount(int finishedID, int flag)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "2")
        {
            ViewState["FinishedID"] = finishedID;
            string Qry = @"Select isnull( sum(isnull(VT.Amt,0)* CS.Percentage/100) + isnull(VT.Amt ,0) + isnull(PAC.CONTAINERAMT,0),0) AS NetCost
                        From EXPENSENAME ES,CUSTWISEOTHEREXPENCE CS,View_TotalCost VT 
						left outer join  PACKING_AND_OTHERMATERIAL_COST PAC on VT.FINISHEDID =  PAC.FINISHEDID
						Where ES.Expid=CS.Expid And CS.FINISHEDID=VT.Finishedid   AND  CS.Finishedid=" + finishedID + @"
                        group by CS.Finishedid,VT.Amt, PAC.CONTAINERAMT
                        select Distinct CurrencyID,CurRate,NetCost,ApprovedAmount from Item_Approval_Detail Where FinishedID=" + finishedID;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtTotalCost.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString()), 2).ToString();
            }
            if (flag == 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    TxtTotalCost.Text = ds.Tables[1].Rows[0]["NetCost"].ToString();
                    DDCurrency.SelectedValue = ds.Tables[1].Rows[0]["CurrencyID"].ToString();
                    TxtCurRate.Text = ds.Tables[1].Rows[0]["CurRate"].ToString();
                    TxtApprovalCost.Text = ds.Tables[1].Rows[0]["ApprovedAmount"].ToString();
                }
            }
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        lblMessage.Text = "";
        if (TxtProdCode.Text != "")
        {
            //ddCategoryName.SelectedIndex = 0;
            TxtRemarks.Text = "";
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 Order by Category_Name", true, "--SELECT--");
            Str = "Select IPM.*,IM.CATEGORY_ID,IsNull(MII.REMARKS,'') REMARKS,IsNull(MII.Photo,'') Photo from ITEM_MASTER IM,CategorySeparate CS,ITEM_PARAMETER_MASTER IPM Left Outer Join MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  And Id=0 and ProductCode='" + TxtProdCode.Text + "'";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtFinishedid.Text = ds.Tables[0].Rows[0]["ITEM_FINISHED_ID"].ToString();
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
                if (ddQuality.Items.Count > 0)
                {
                    ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                }
                Fill_Sub_Quality();
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT FROM SIZE WHERE SHAPEID=" + ddShape.SelectedValue + "", true, "--SELECT--");
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                newPreview1.ImageUrl = ds.Tables[0].Rows[0]["Photo"].ToString();
                int Finishedid = GetItemFinishedID();

                //newPreview1.ImageUrl = "~/ImageHandler.ashx?Id=" + Finishedid + "&img=3";

                FillNet_GrossWeight(Finishedid);
                FILLGRID();
                TotalAmount(Finishedid, 0);
                BtnOpenOtherExpense.Visible = true;
                BtnOpenPackingCost.Visible = true;
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddCategoryName.SelectedIndex = 0;
                CATEGORY_DEPENDS_CONTROLS();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
                BtnOpenOtherExpense.Visible = false;
                BtnOpenPackingCost.Visible = false;
                DG.DataSource = "";
                DG.DataBind();
                ddCategoryName.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }
    protected void TxtOutProdCode_TextChanged(object sender, EventArgs e)
    {
        Out_ProdCode_TextChanges();
    }
    private void Out_ProdCode_TextChanges()
    {
        DataSet ds;
        string Str;
        lblMessage.Text = "";
        if (TxtOutProdCode.Text != "")
        {
            //ddOutCategoryName.SelectedIndex = 0;
            UtilityModule.ConditionalComboFill(ref ddOutCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Order by Category_Name", true, "--SELECT--");
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  where ProductCode='" + TxtOutProdCode.Text + "'";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddOutCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                OUT_CATEGORY_DEPENDS_CONTROLS();
                ddOutItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                FILLGRID();
                QDCSDDFill(ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutShade, Convert.ToInt32(ddOutItemName.SelectedValue), 1);
                UtilityModule.ConditionalComboFill(ref ddOUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddOutItemName.SelectedValue + " Order BY U.UNITNAME", true, "--SELECT--");
                ddOUnit.SelectedIndex = 1;
                ddOutQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddOutDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddOutColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddOutShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + "", true, "--SELECT--");
                ddOutSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                TxtOutPutQty.Text = "1";
                TxtOutPutRate.Text = "0";
                //newPreview1.ImageUrl = "~/ImageHandler.ashx?Id=" + id + "&img=3";

            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddOutCategoryName.SelectedIndex = 0;
                OUT_CATEGORY_DEPENDS_CONTROLS();
                TxtOutProdCode.Text = "";
                TxtOutProdCode.Focus();
            }
        }
        else
        {
            ddOutCategoryName.SelectedIndex = 0;
            OUT_CATEGORY_DEPENDS_CONTROLS();
        }
    }
    protected void TxtInProdCode_TextChanged(object sender, EventArgs e)
    {
        In_ProdCode_TextChanges();

        if (ChkForOneToOne.Checked == true)
        {
            TxtOutProdCode.Text = TxtInProdCode.Text;
            Out_ProdCode_TextChanges();
        }
    }
    private void In_ProdCode_TextChanges()
    {
        DataSet ds;
        string Str;
        lblMessage.Text = "";
        if (TxtInProdCode.Text != "")
        {
            //ddInCategoryName.SelectedIndex = 0;
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Order by Category_Name", true, "--SELECT--");
            if (ChkForProcessInPut.Checked == true)
            {
                Str = "select IPM.*,IM.CATEGORY_ID,O_Finished_Type_Id,OQTY from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  inner join  PROCESSCONSUMPTIONDetail PCMD on PCMD.OFINISHEDID=IPM.Item_Finished_Id  where IPM.ITEM_ID=IM.ITEM_ID  and ProductCode='" + TxtInProdCode.Text + "'and PCMid in (select pcmid from PROCESSCONSUMPTIONMaster,Item_Parameter_Master where Productcode='" + TxtProdCode.Text + "' and processid=" + ddInProcessName.SelectedValue + "  and Item_Finished_Id=FinishedId)";
            }
            else
            {
                Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  where ProductCode='" + TxtInProdCode.Text + "'";

                //SELECT isnull(max(NETWEIGHT),0) NETWEIGHT   From Item_Parameter_Master IPM inner join MAIN_ITEM_IMAGE MI on ipm.item_finished_id=MI.finishedid where  IPM.ProductCode ='" + TxtInProdCode.Text+"'";
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    TxtIweight.Text = ds.Tables[1].Rows[0][0].ToString();
            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddInCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                IN_CATEGORY_DEPENDS_CONTROLS();
                ddInItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
                UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddInItemName.SelectedValue + " Order By U.UNITNAME", true, "--SELECT--");
                ddIUnit.SelectedIndex = 1;
                ddInQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddInDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddInColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddInShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + "", true, "--SELECT--");
                ddInSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                int Finishedid = GetInItemFinishedID();
                string Qry = "select Isnull(NETWEIGHT,0) from MAIN_ITEM_IMAGE where FINISHEDID=" + Finishedid;
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(NETWEIGHT,0) from MAIN_ITEM_IMAGE where FINISHEDID=" + Finishedid);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    TxtIweight.Text = ds1.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    TxtIweight.Text = "0";
                }
                // TxtIweight.Text = SqlHelper.ExecuteNonQuery (ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Isnull(NETWEIGHT,0) from MAIN_ITEM_IMAGE where FINISHEDID=" + Finishedid).ToString();
                if (ChkForProcessInPut.Checked == true)
                {
                    ddFinishedIN.SelectedValue = ds.Tables[0].Rows[0]["O_Finished_Type_Id"].ToString();
                    TxtInPutQty.Text = ds.Tables[0].Rows[0]["OQTY"].ToString();
                    TxtInPutRate.Text = "0.00";
                }
                FILLGRID();
                ddInCategoryName.Focus();
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                ddInCategoryName.SelectedIndex = 0;
                IN_CATEGORY_DEPENDS_CONTROLS();
                TxtInProdCode.Text = "";
                TxtInProdCode.Focus();
            }
        }
        else
        {
            ddInCategoryName.SelectedIndex = 0;
            IN_CATEGORY_DEPENDS_CONTROLS();
        }
    }

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("DefineBomAndConsumption.aspx");
        //ddCategoryName.SelectedIndex = 0;
        //CATEGORY_DEPENDS_CONTROLS();
        //ddOutCategoryName.SelectedIndex = 0;
        //OUT_CATEGORY_DEPENDS_CONTROLS();
        //TxtOutPutQty.Text = "";
        //TxtOutPutRate.Text = "";
        //ddInCategoryName.SelectedIndex = 0;
        //IN_CATEGORY_DEPENDS_CONTROLS();
        //TxtInPutQty.Text = "";
        //TxtLoss.Text = "";
        //TxtInPutRate.Text = "";
        //BtnSave.Text = "Save";
    }
    protected void BtnPostBack_Click(object sender, EventArgs e)
    {

    }
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Order by Category_Name", true, "--SELECT--");
    }
    protected void refreshitem_Click(object sender, EventArgs e)
    {
        CATEGORY_DEPENDS_CONTROLS();
        //IN_CATEGORY_DEPENDS_CONTROLS();
        //OUT_CATEGORY_DEPENDS_CONTROLS();
    }
    protected void ddCalType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddQuality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + ddItemName.SelectedValue + " Order By QUALITYNAME", true, "--SELECT--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Order By DESIGNNAME", true, "--SELECT--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Order By COLORNAME", true, "--SELECT--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Order By SHAPENAME", true, "--SELECT--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + "", true, "--SELECT--");
        ShapeSelectedChange();
        FillSize();
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Order By ShadeColorName", true, "--SELECT--");

    }
    protected void btnrefreshprocess_Click(object sender, EventArgs e)
    {
        if (MasterCompanyid == 2)
        {
            UtilityModule.ConditionalComboFill(ref ddProcessName, "Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " AND PROCESS_NAME_ID in (6,8,9) order by PROCESS_NAME", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddProcessName, "Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "  order by PROCESS_NAME", true, "--SELECT--");
        }
    }
    protected void ChkForOneToOne_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForOneToOne.Checked == true)
        {
            ChkForManyOutPut.Checked = false;
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
            Tr1.Disabled = false;
            Tr2.Disabled = false;
            Tr3.Disabled = false;
        }
    }
    protected void ChkForManyOutPut_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForManyOutPut.Checked == true)
        {
            TrOUT1.Disabled = false;
            TrOUT2.Disabled = false;
            ChkForOneToOne.Checked = false;
        }
        else
        {
            Tr1.Disabled = false;
            Tr2.Disabled = false;
            Tr3.Disabled = false;
        }
    }
    protected void TxtInPutQty_TextChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyNo"].ToString() == "4")
            {
                //LblQty.Text = TxtInPutQty.Text;
                //int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                //DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "SELECT * FROM PROCESSCONSUMPTIONMASTER WHERE PROCESSID=" + ddProcessName.SelectedValue + " AND FINISHEDID=" + Varfinishedid + "");
                //if (Ds.Tables[0].Rows.Count > 0)
                //{
                //    LblQty.Text = (Convert.ToDouble(TxtInPutQty.Text) - Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Round(Isnull(Sum(OQty),0),5) Qty From PROCESSCONSUMPTIONDETAIL Where PCMID=" + Ds.Tables[0].Rows[0]["PCMID"] + ""))).ToString();
                //}
                //TxtOutPutQty.Text = LblQty.Text;
            }
            else if (Session["varCompanyNo"].ToString() == "2")
            {
                TxtOutPutQty.Text = TxtInPutQty.Text;
            }
            if (Session["varCompanyNo"].ToString() == "2")
            {
                TxtInPutRate.Focus();
            }
            else
            {
                TxtLoss.Focus();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
            Tran.Rollback();
            lblMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void refreshfinishedin_Click(object sender, EventArgs e)
    {
        //UtilityModule.ConditionalComboFill(ref ddFinishedIN, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE ORDER BY ID", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref ddFinished, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE ORDER BY FINISHED_TYPE_NAME", true, "--SELECT--");
    }
    //    protected void BtnPreview_Click(object sender, EventArgs e)
    //    {
    //        OpenNewWidow("../../ReportViewer.aspx");
    //        if (ddCategoryName.SelectedIndex>0 && ddItemName.SelectedIndex>0)

    //        {
    //                int VARFINISHEDID = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade);
    //                Session["ReportPath"] = "Reports/DefineCostingOfAItem1.rpt";
    //                Session["CommanFormula"] = "{VIEW_COSTINGORDERDETAIL.ITEM_FINISHED_ID}= " + VARFINISHEDID + "";
    //                GET_FORMULAFEILD(VARFINISHEDID);
    //                OpenNewWidow("../../ReportViewer.aspx");
    //                //OpenNewWidow("../../ReportViewer.aspx");
    //        }
    //    }
    //    public void OpenNewWidow(string url)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format(@"<script> confirm('Do you want to save data?');
    //        window.open('{0}');</script>", url));
    //    }
    private void GET_FORMULAFEILD(int VARFINISHEDID)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[2];
            _arrpara[0] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@AMT", SqlDbType.Float);
            _arrpara[0].Value = VARFINISHEDID;
            _arrpara[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[dbo].[PRO_CAL_TOTAL_AMT]", _arrpara);
            RefreshApproval();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select Round((Amt+FRT+ sum((Percentage*Amt)/ 100)),2) as TotalCost from View_CalOtherExpense group by  Amt,FRT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtTotalCost.Text = ds.Tables[0].Rows[0][0].ToString();
                TxtApprovalCost.Text = TxtTotalCost.Text;
            }
            UtilityModule.ConditionalComboFill(ref DDCurrency, "select DISTINCT CurrencyId, CurrencyName from currencyinfo", true, "--select--");
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedValue = "1";
                DDCurrencySelectedChange();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = "Some Importent fields are missing.....";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ddSub_Quality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddSub_Quality.SelectedIndex > 0)
        {
            LblQty.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Isnull(Round(Quantity/1.196,5),0) from QualityCodeMaster Where QualityCodeId=" + ddSub_Quality.SelectedValue + "").ToString();
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        imgLogo.ImageUrl.DefaultIfEmpty();
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
        {
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void btnaddshade_Click(object sender, EventArgs e)
    {
        refreshshade.Visible = true;
    }
    protected void btnshade_Click(object sender, EventArgs e)
    {
        btnrefreshshadecolorform.Visible = true;
    }
    protected void btnrefreshshadecolorform_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddInShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Order By ShadeColorName", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref ddOutShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Order By ShadeColorName", true, "--SELECT--");
    }
    protected void btnrefreshsbqlt_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSub_Quality, "Select QualityCodeId,SubQuantity from qualityCodeMaster Order By SubQuantity", true, "--SELECT SUB_QUALITY--");
    }
    protected void CHKFORALLDESIGN_CheckedChanged(object sender, EventArgs e)
    {
        //#region
        //if (CHKFORALLDESIGN.Checked == true)
        //{
        //    UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Order By DESIGNNAME", true, "--ALL--");
        //    ddInDesign.SelectedIndex = 1;
        //    ddOutDesign.SelectedIndex = 1;

        //}
        //else
        //{
        //    UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Order By DESIGNNAME", true, "--SELECT--");
        //    ddInDesign.SelectedValue = ddDesign.SelectedValue;
        //    ddOutDesign.SelectedValue = ddDesign.SelectedValue;
        //}
        //#endregion
        if (CHKFORALLDESIGN.Checked == true)
        {
            ddDesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            ddDesign.Items.Insert(0, li);
            //ddInDesign.SelectedIndex = 1;
            //ddOutDesign.SelectedIndex = 1;
        }
        else
        {
            ddDesign.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--SELECT--";
            li.Value = "0";
            ddDesign.Items.Insert(0, li);
            //ddInDesign.SelectedValue = ddDesign.SelectedValue;
            //ddOutDesign.SelectedValue = ddDesign.SelectedValue;
        }
    }
    protected void CHKFORALLCOLOR_CheckedChanged(object sender, EventArgs e)
    {
        #region
        //if (CHKFORALLCOLOR.Checked == true)
        //{
        //    UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Order By COLORNAME", true, "--ALL--");
        //    //ddInColor.SelectedIndex = 1;
        //    //ddOutColor.SelectedIndex = 1;
        //}
        //else
        //{
        //    UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Order By COLORNAME", true, "--SELECT--");
        //    ddInColor.SelectedValue = ddColor.SelectedValue;
        //    ddOutColor.SelectedValue = ddColor.SelectedValue;
        //}
        #endregion
        if (CHKFORALLCOLOR.Checked == true)
        {
            ddColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--ALL--";
            li.Value = "0";
            ddColor.Items.Insert(0, li);

        }
        else
        {
            ddColor.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "--SELECT--";
            li.Value = "0";
            ddColor.Items.Insert(0, li);

        }

    }
    protected void CHKFORALLSIZE_CheckedChanged(object sender, EventArgs e)
    {
        if (ddShape.SelectedIndex > 0)
        {
            fil__Size();
        }
    }
    protected void ChMeteerSize_CheckedChanged(object sender, EventArgs e)
    {
        fil__Size();
    }
    private void fil__Size()
    {
        if (ChMeteerSize.Checked)
        {
            SizeString = "SizeMtr";
        }
        else
        {
            SizeString = "SIZEFT";
        }
        if (CHKFORALLSIZE.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + "", true, "--ALL--");
            if(Session["varMasterCompanyIDForERP"].ToString()=="30")
            {
                if (ddSize.Items.Count <= 0)
                {
                    CHKFORALLSIZE.Checked = false;                    
                }
            }

            if (ChkForFillSame.Checked == true)
            {
                ddInSize.SelectedIndex = 1;
                ddOutSize.SelectedIndex = 1;
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + "", true, "--SELECT--");
            if (ChkForFillSame.Checked == true)
            {
                ddInSize.SelectedValue = ddSize.SelectedValue;
                ddOutSize.SelectedValue = ddSize.SelectedValue;
            }
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "select$" + e.Row.RowIndex);

        }


    }
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (Convert.ToInt32(Session["Type"]) != 1)
        {
            string Str = @"SELECT PM.PROCESSID,PM.FINISHEDID,PM.FlagMtrFt,PD.PCMID,PD.PCMDID,PD.IFINISHEDID,PD.IUNITID,Case When PM.FlagMtrFt=1 Then Round(PD.IQTY*1.196,5) Else PD.IQTY End IQTY,
                        Case When PM.LossPercentageFlag=1 Then Round(PD.ILOSS/PD.IQTY*100,0) Else Case When PM.FlagMtrFt=1 Then Round(PD.ILOSS*1.196,5) Else PD.ILOSS End End ILOSS,
                        PD.IRATE,PD.OFINISHEDID,PD.OUNITID,Case When PM.FlagMtrFt=1 Then Round(PD.OQTY*1.196,5) Else PD.OQTY End OQTY,PD.ORATE,PD.PROCESSINPUTID,
                        PD.INOUTTYPEID,PD.ICALTYPE,PD.I_FINISHED_Type_ID,PD.O_FINISHED_Type_ID,PD.SUB_QUALITY_ID,PD.OCALTYPE,VF.CATEGORY_ID ICATEGORY_ID,
                        VF.ITEM_ID IITEM_ID,VF.QUALITY_ID IQUALITY_ID,VF.DESIGN_ID IDESIGN_ID,VF.COLOR_ID ICOLOR_ID,VF.SHAPE_ID ISHAPE_ID,VF.SIZE_ID ISIZE_ID,
                        VF.SHADECOLOR_ID ISHADECOLOR_ID,VF.PRODUCTCODE IPRODUCTCODE,VF1.CATEGORY_ID OCATEGORY_ID,VF1.ITEM_ID OITEM_ID,VF1.QUALITY_ID OQUALITY_ID,
                        VF1.DESIGN_ID ODESIGN_ID,VF1.COLOR_ID OCOLOR_ID,VF1.SHAPE_ID OSHAPE_ID,VF1.SIZE_ID OSIZE_ID,VF1.SHADECOLOR_ID OSHADECOLOR_ID,VF1.PRODUCTCODE OPRODUCTCODE,
                        Case When PM.LossPercentageFlag=1 Then Round(PD.OLoss/PD.OQTY*100,0) Else Case When PM.FlagMtrFt=1 Then Round(PD.OLoss*1.196,5) Else PD.OLoss End End OLoss,LossPercentageFlag,
						isnull(PD.IWEIGHT,0) IWEIGHT,isnull(PM.NETWEIGHT,0) NETWEIGHT,isnull(PM.GROSSWEIGHT,0) GROSSWEIGHT,IsizeFlag,OSizeflag,isnull(Dyingmatch,'') as Dyingmatch,
                        isnull(DyingType,'') as DyingType,isnull(dyeing,'') as Dyeing,isnull(Sizeflag,0) as Sizeflag,IsNull(PD.OQtyPercentage,'') as OQtyPercentage
                        FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,VIEWFINDCIQDCSSSHID VF,VIEWFINDCIQDCSSSHID VF1 
                        WHERE PM.PCMID=PD.PCMID AND PD.IFINISHEDID=VF.ITEM_FINISHED_ID AND PD.OFINISHEDID=VF1.ITEM_FINISHED_ID AND PD.PCMDID=" + DG.SelectedValue;
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["PROCESSID"].ToString();
                DDsizetype.SelectedValue = Ds.Tables[0].Rows[0]["Sizeflag"].ToString();
                ChkForProcessInPut.Checked = false;
                ProcessInputChecked();
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["PROCESSINPUTID"]) != 0)
                {
                    ChkForProcessInPut.Checked = true;
                    ProcessInputChecked();
                    ddInProcessName.SelectedValue = Ds.Tables[0].Rows[0]["PROCESSINPUTID"].ToString();
                    ddInProcessName_SelectedIndexChanged(sender, e);
                    //InProcessNameSelectedIndChange();

                }
                ChkForMtr.Checked = false;
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["FlagMtrFt"]) == 1)
                {
                    ChkForMtr.Checked = true;
                }
                ChkForLossPercentage.Checked = false;
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["LossPercentageFlag"]) == 1)
                {
                    ChkForLossPercentage.Checked = true;
                }
                ddInCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["ICATEGORY_ID"].ToString();
                ddInCategoryName_SelectedIndexChanged(sender, e);
                //                IN_CATEGORY_DEPENDS_CONTROLS();
                ddInItemName.SelectedValue = Ds.Tables[0].Rows[0]["IITEM_ID"].ToString();
                InItemNameSelectedIndexChange();
                if (ddInQuality.Items.Count > 0)
                {
                    ddInQuality.SelectedValue = Ds.Tables[0].Rows[0]["IQUALITY_ID"].ToString();
                }
                ddInDesign.SelectedValue = Ds.Tables[0].Rows[0]["IDESIGN_ID"].ToString();
                ddInColor.SelectedValue = Ds.Tables[0].Rows[0]["ICOLOR_ID"].ToString();
                ddInShape.SelectedValue = Ds.Tables[0].Rows[0]["ISHAPE_ID"].ToString();
                DDInSizeType.SelectedValue = Ds.Tables[0].Rows[0]["ISizeflag"].ToString();
                DDInSizeType_SelectedIndexChanged(sender, e);
                if (ddInSize.Items.FindByValue(Ds.Tables[0].Rows[0]["ISIZE_ID"].ToString()) != null)
                {
                    ddInSize.SelectedValue = Ds.Tables[0].Rows[0]["ISIZE_ID"].ToString();
                }
                ddInShade.SelectedValue = Ds.Tables[0].Rows[0]["ISHADECOLOR_ID"].ToString();
                ddIUnit.SelectedValue = Ds.Tables[0].Rows[0]["IUNITID"].ToString();
                if (ddFinishedIN.Visible == true)
                {
                    ddFinishedIN.SelectedValue = Ds.Tables[0].Rows[0]["I_FINISHED_TYPE_ID"].ToString();
                    TxtInProdCode.Text = Ds.Tables[0].Rows[0]["IPRODUCTCODE"].ToString();
                }
                DDICALTYPE.SelectedValue = Ds.Tables[0].Rows[0]["ICALTYPE"].ToString();
                TxtInPutQty.Text = Ds.Tables[0].Rows[0]["IQTY"].ToString();
                TxtLoss.Text = Ds.Tables[0].Rows[0]["ILOSS"].ToString();
                TxtInPutRate.Text = Ds.Tables[0].Rows[0]["IRATE"].ToString();
                TxtIweight.Text = Ds.Tables[0].Rows[0]["IWEIGHT"].ToString();
                txtGrossweight.Text = Ds.Tables[0].Rows[0]["GROSSWEIGHT"].ToString();
                LblNetWeightVal.Text = Ds.Tables[0].Rows[0]["NETWEIGHT"].ToString();

                ViewState["SelectedIweight"] = Convert.ToDecimal(TxtInPutQty.Text) * Convert.ToDecimal(TxtIweight.Text);
                FillNet_GrossWeight(Convert.ToInt32(Ds.Tables[0].Rows[0]["FINISHEDID"].ToString()));
                switch (Convert.ToInt32(Ds.Tables[0].Rows[0]["INOUTTYPEID"]))
                {
                    case 0:
                        ChkForOneToOne.Checked = true;
                        break;
                    case 2:
                        ChkForManyOutPut.Checked = true;
                        break;
                }
                ddOutCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["OCATEGORY_ID"].ToString();
                ddOutCategoryName_SelectedIndexChanged(sender, e);
                //OUT_CATEGORY_DEPENDS_CONTROLS();
                ddOutItemName.SelectedValue = Ds.Tables[0].Rows[0]["OITEM_ID"].ToString();
                OutItemSelectedIndexChange();
                ddOutQuality.SelectedValue = Ds.Tables[0].Rows[0]["OQUALITY_ID"].ToString();
                ddOutDesign.SelectedValue = Ds.Tables[0].Rows[0]["ODESIGN_ID"].ToString();
                ddOutColor.SelectedValue = Ds.Tables[0].Rows[0]["OCOLOR_ID"].ToString();
                ddOutShape.SelectedValue = Ds.Tables[0].Rows[0]["OSHAPE_ID"].ToString();
                DDOutSizeType.SelectedValue = Ds.Tables[0].Rows[0]["OSizeflag"].ToString();
                DDOutSizeType_SelectedIndexChanged(sender, e);

                if (ddOutSize.Items.FindByValue(Ds.Tables[0].Rows[0]["OSIZE_ID"].ToString()) != null)
                {
                    ddOutSize.SelectedValue = Ds.Tables[0].Rows[0]["OSIZE_ID"].ToString();
                }
                ddOutShade.SelectedValue = Ds.Tables[0].Rows[0]["OSHADECOLOR_ID"].ToString();
                ddOUnit.SelectedValue = Ds.Tables[0].Rows[0]["OUNITID"].ToString();
                if (ddFinished.Visible == true)
                {
                    ddFinished.SelectedValue = Ds.Tables[0].Rows[0]["O_FINISHED_TYPE_ID"].ToString();
                    TxtOutProdCode.Text = Ds.Tables[0].Rows[0]["OPRODUCTCODE"].ToString();
                }
                ddOCalType.SelectedValue = Ds.Tables[0].Rows[0]["OCALTYPE"].ToString();
                TxtOutPutQty.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Ds.Tables[0].Rows[0]["OQTY"].ToString()), 3));
                TxtRecLoss.Text = Ds.Tables[0].Rows[0]["Oloss"].ToString();
                TxtOutPutRate.Text = Ds.Tables[0].Rows[0]["ORATE"].ToString();
                txtOutputQtyPercentage.Text = Ds.Tables[0].Rows[0]["OQTYPercentage"].ToString();
                BtnSave.Text = "UpDate";
                int finishedid = GetItemFinishedID();
                TotalAmount(finishedid, 0);
                //For Deepak rugs
                if (Session["varMasterCompanyIDForERP"].ToString() == "4")
                {
                    if (ddProcessName.SelectedValue == "5") //Dying
                    {
                        TDDyeingMatch.Visible = true;
                        TDDyingType.Visible = true;
                        TDDyeing.Visible = true;
                        if (Ds.Tables[0].Rows[0]["dyingmatch"].ToString() != "")
                        {
                            DDDyeingMatch.SelectedValue = Ds.Tables[0].Rows[0]["Dyingmatch"].ToString();
                        }
                        if (Ds.Tables[0].Rows[0]["DyingType"].ToString() != "")
                        {
                            DDDyingType.SelectedValue = Ds.Tables[0].Rows[0]["DyingType"].ToString();
                        }
                        if (Ds.Tables[0].Rows[0]["Dyeing"].ToString() != "")
                        {
                            DDDyeing.SelectedValue = Ds.Tables[0].Rows[0]["Dyeing"].ToString();
                        }

                    }
                }
                else if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                {
                    ChkForInputConsmpQtyIntoOutputConsmpQty.Visible = true;
                }
                //Purchase_Include Process
                if (variable.BOM_PurchaseIncludeProcess == "1")
                {
                    ddProcessName_SelectedIndexChanged1(ddProcessName, new EventArgs());
                }
                switch (ddProcessName.SelectedItem.Text.ToUpper())
                {
                    case "PURCHASE":
                        if (variable.BOM_PurchaseIncludeProcess == "1")
                        {
                            //Uncheck all Checkbox
                            for (int P = 0; P < GDPurchaseinclude.Rows.Count; P++)
                            {
                                CheckBox Chkbox = (CheckBox)GDPurchaseinclude.Rows[P].FindControl("Chkbox");
                                TextBox txtincludeqty = (TextBox)GDPurchaseinclude.Rows[P].FindControl("txtincludeqty");
                                Chkbox.Checked = false;
                                txtincludeqty.Text = "";
                            }
                            //*******
                            string str1;
                            int IProcessid = 0;
                            Double IQTY = 0;
                            str1 = "select IProcessId,IQTY from BOM_ProcessItemInclude Where Finishedid=" + Ds.Tables[0].Rows[0]["Finishedid"] + " and IFinishedid=" + Ds.Tables[0].Rows[0]["IFinishedid"] + " and ProcessId=" + ddProcessName.SelectedValue;
                            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int P = 0; P < ds1.Tables[0].Rows.Count; P++)
                                {
                                    IProcessid = Convert.ToInt16(ds1.Tables[0].Rows[P]["IProcessid"]);
                                    IQTY = Convert.ToDouble(ds1.Tables[0].Rows[P]["IQTY"]);
                                    //Mtr flag
                                    if (ChkForMtr.Checked == true)
                                    {
                                        IQTY = Math.Round(IQTY * 1.196, 3);
                                    }
                                    for (int J = 0; J < GDPurchaseinclude.Rows.Count; J++)
                                    {
                                        CheckBox Chkbox = (CheckBox)GDPurchaseinclude.Rows[J].FindControl("Chkbox");
                                        Label lblprocessid = (Label)GDPurchaseinclude.Rows[J].FindControl("lblprocessid");
                                        TextBox txtincludeqty = (TextBox)GDPurchaseinclude.Rows[J].FindControl("txtincludeqty");
                                        //Check PRocessid
                                        if (IProcessid == Convert.ToInt16(lblprocessid.Text))
                                        {
                                            Chkbox.Checked = true;
                                            txtincludeqty.Text = IQTY.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                //*****************END
            }
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        Session["Type"] = 1;
    }
    protected void DG_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (Convert.ToInt32(Session["Type"]) == 1)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = DG.Rows[index];
                SqlParameter[] _param = new SqlParameter[2];
                _param[0] = new SqlParameter("@PCMDID", row.Cells[0].Text);
                _param[1] = new SqlParameter("@FinishedID", SqlDbType.Int);
                _param[1].Direction = ParameterDirection.Output;
                int i = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_BOMConsumption_Delete", _param);
                //SqlHelper.ExecuteNonQuery(con, CommandType.Text, "DELETE PROCESSCONSUMPTIONDETAIL WHERE PCMDID=" + row.Cells[0].Text);

                msg = "Record(s) has been deleted successfully!";

                MessageSave(msg);
                if (Session["varMasterCompanyIDForERP"].ToString() == "2")
                {
                    FillNet_GrossWeight(Convert.ToInt32(_param[1].Value));
                }               
                FILLGRID();
                Session["Type"] = 0;
                lblMessage.Text = "";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/DefineBomAndConsumption.aspx");
                lblMessage.Visible = true;
                lblMessage.Text = "Error In Deleting";
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();

        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=1 And IM.MasterCompanyid=" + MasterCompanyid + " And ProductCode Like '" + prefixText + "%' Order by ProductCode";
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality1(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + MasterCompanyid + "  And ProductCode Like '" + prefixText + "%' order by ProductCode";
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
    protected void txtOutputQtyPercentage_TextChanged(object sender, EventArgs e)
    {
        if (txtOutputQtyPercentage.Text != "")
        {
            TxtOutPutQty.Text = Convert.ToString(Math.Round(Convert.ToDouble(TxtInPutQty.Text) * Convert.ToDouble(txtOutputQtyPercentage.Text) / 100, 3));
            TxtOutPutQty_TextChanged(sender, new EventArgs());
        }
    }
    protected void TxtOutPutQty_TextChanged(object sender, EventArgs e)
    {
        // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (Session["varCompanyNo"].ToString() == "4" && INPROCESSNAME.Visible == false)
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
            //if (Convert.ToDouble(TxtOutPutQty.Text) > Math.Round(Convert.ToDouble(LblQty.Text), 3))
            //{
            //    TxtOutPutQty.Text = "";
            //    TxtOutPutQty.Focus();
            //    lblMessage.Visible = true;
            //    lblMessage.Text = "Rec Qty Cann't Be Greater Than Pending Qty";
            //}
        }
        if (TxtRecLoss.Visible == true)
        {
            TxtRecLoss.Focus();
        }
        else
        {
            TxtOutPutRate.Focus();
        }
    }
    protected void BtncClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["ZZZ"] == "1")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "CloseForm();", true);
        }
        else
        {
            Response.Redirect("~/main.aspx");
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ChkForFillSame_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForFillSame.Checked == true)
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
    //protected void DGInPutProcess_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //Add CSS class on header row.
    //    if (e.Row.RowType == DataControlRowType.Header)
    //        e.Row.CssClass = "header";

    //    //Add CSS class on normal row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Normal)
    //        e.Row.CssClass = "normal";

    //    //Add CSS class on alternate row.
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //              e.Row.RowState == DataControlRowState.Alternate)
    //        e.Row.CssClass = "alternate";
    //}
    private void fillMasterParameter()
    {
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select CATEGORY_ID,ITEM_ID,QualityId,designId,ColorId,ShapeId,SizeId from V_FinishedItemDetail where item_finished_id=" + Request.QueryString["finishedid"].ToString() + "");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddCategoryName.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
            CATEGORY_DEPENDS_CONTROLS();
            ddItemName.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();

            QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
            ddQuality.SelectedValue = ds1.Tables[0].Rows[0]["QualityId"].ToString();

            Fill_Sub_Quality();
            if (Design.Visible == true)
            {
                ddDesign.SelectedValue = ds1.Tables[0].Rows[0]["designId"].ToString();
            }
            if (Color.Visible == true)
            {
                ddColor.SelectedValue = ds1.Tables[0].Rows[0]["ColorId"].ToString();
            }
            if (Shape.Visible == true)
            {
                ddShape.SelectedValue = ds1.Tables[0].Rows[0]["ShapeId"].ToString();
                //ShapeSelectedChange();
                if (Request.QueryString["flagSize"] != null)
                {
                    DDsizetype.SelectedValue = Request.QueryString["flagSize"];
                }
                FillSize();
            }
            if (Size.Visible == true)
            {
                ddSize.SelectedValue = ds1.Tables[0].Rows[0]["SizeId"].ToString();
            }
        }
    }
    protected void TxtLoss_TextChanged(object sender, EventArgs e)
    {
        TxtRecLoss.Text = TxtLoss.Text;
    }

    protected void CHKFORISSUE_CheckedChanged(object sender, EventArgs e)
    {
        FILL_SIZE_ISSUE();
    }
    protected void CHKFOReceive_CheckedChanged(object sender, EventArgs e)
    {
        FILL_SIZE_RECEIVE();
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        FILLGRID();
        // ddInQuality.Focus();
    }
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }

    protected void DDCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCurrencySelectedChange();
    }

    private void DDCurrencySelectedChange()
    {
        if (DDCurrency.SelectedIndex > 0)
        {
            string Str = "select DISTINCT CurrentRateRefRs from currencyinfo Where CurrencyId=" + DDCurrency.SelectedValue + "AND MasterCompanyid=" + Session["varMasterCompanyIDForERP"];
            TxtCurRate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString();
            TxtApprovalCost.Text = (Math.Round((Convert.ToDouble(TxtTotalCost.Text == "" ? "0" : TxtTotalCost.Text) / Convert.ToDouble(TxtCurRate.Text == "" ? "0" : TxtCurRate.Text)), 2)).ToString();
        }
    }
    protected void BtnApproval_Click(object sender, EventArgs e)
    {
        int Finishedid = GetItemFinishedID();
        SqlParameter[] _param = new SqlParameter[6];
        _param[0] = new SqlParameter("@FinishedID", Finishedid);
        _param[1] = new SqlParameter("@CurrencyID", DDCurrency.SelectedValue);
        _param[2] = new SqlParameter("@CurRate", TxtCurRate.Text == "" ? "0" : TxtCurRate.Text);
        _param[3] = new SqlParameter("@NetCost", TxtTotalCost.Text == "" ? "0" : TxtTotalCost.Text);
        _param[4] = new SqlParameter("@ApprovedAmount", TxtApprovalCost.Text == "" ? "0" : TxtApprovalCost.Text);
        _param[5] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
        _param[5].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_SaveItemCostApproval", _param);
        msg = _param[5].Value.ToString();
        MessageSave(msg);

    }
    protected void TxtCurRate_TextChanged(object sender, EventArgs e)
    {
        TxtApprovalCost.Text = (Math.Round((Convert.ToDouble(TxtTotalCost.Text == "" ? "0" : TxtTotalCost.Text) / Convert.ToDouble(TxtCurRate.Text == "" ? "0" : TxtCurRate.Text)), 2)).ToString();
    }
    private void RefreshApproval()
    {
        TxtTotalCost.Text = "";
        TxtCurRate.Text = "";
        TxtApprovalCost.Text = "";
        if (DDCurrency.Items.Count > 0)
        {
            DDCurrency.SelectedValue = "1";
        }
    }
    //public void Save_Image(int VarFinishedid)
    //{
    //    //SqlConnection myConnection = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    //myConnection.Open();
    //    if (PhotoImage.FileName != "")
    //    {
    //        //SqlHelper.ExecuteNonQuery(myConnection, CommandType.Text, "Delete MAIN_ITEM_IMAGE Where FINISHEDID=" + VarFinishedid);
    //        //int id = 0;
    //        int id = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT DISTINCT isnull(FINISHEDID,0) as FinishedID FROM MAIN_ITEM_IMAGE Where FINISHEDID=" + VarFinishedid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ""));
    //        if (id > 0)
    //        {
    //            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
    //            string targetPath = Server.MapPath("../../Item_Image/" + VarFinishedid + "_Item.gif");
    //            string img = "~\\Item_Image\\" + VarFinishedid + "_Item.gif";
    //            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
    //            Stream strm = PhotoImage.PostedFile.InputStream;
    //            var targetFile = targetPath;

    //            FileInfo TheFile = new FileInfo(Server.MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
    //            if (TheFile.Exists)
    //            {
    //                File.Delete(MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
    //            }
    //            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
    //            {
    //                GenerateThumbnails(0.3, strm, targetFile);
    //            }
    //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update MAIN_ITEM_IMAGE Set Photo='" + img + "' Where FINISHEDID= " + VarFinishedid + "");
    //            //storeimage = new SqlCommand("Update MAIN_ITEM_IMAGE SET PHOTO = @image WHERE FINISHEDID= "+ VarFinishedid + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + ")", myConnection);
    //            //storeimage.Parameters.Add("@image", SqlDbType.Image, myimage.Length).Value = myimage;
    //        }
    //        else
    //        {
    //            string filename = Path.GetFileName(PhotoImage.PostedFile.FileName);
    //            string targetPath = Server.MapPath("../../Item_Image/" + VarFinishedid + "_Item.gif");
    //            string img = "~\\Item_Image\\" + VarFinishedid + "_Item.gif";
    //            //string img = "ImageDraftorder/d"+OrderDetailId+"" + filename;
    //            Stream strm = PhotoImage.PostedFile.InputStream;
    //            var targetFile = targetPath;

    //            FileInfo TheFile = new FileInfo(Server.MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
    //            if (TheFile.Exists)
    //            {
    //                File.Delete(MapPath("~\\Item_Image\\") + VarFinishedid + "_Item.gif");
    //            }
    //            if (PhotoImage.FileName != null && PhotoImage.FileName != "")
    //            {
    //                GenerateThumbnails(0.3, strm, targetFile);
    //            }
    //            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId) values(" + VarFinishedid + ",'" + img + "'," + Session["varMasterCompanyIDForERP"] + ")");
    //            //storeimage = new SqlCommand("Insert into MAIN_ITEM_IMAGE(FINISHEDID,PHOTO,MasterCompanyId) values(" + VarFinishedid + ","+img+"," + Session["varMasterCompanyIDForERP"] + ")", myConnection);
    //        }

    //    }
    //    //myConnection.Close();
    //    //myConnection.Dispose();
    //}

    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        ViewState["FinishedID"] = ViewState["FinishedID"] == "" ? "0" : ViewState["FinishedID"];
        TotalAmount(Convert.ToInt32(ViewState["FinishedID"].ToString()), 1);
    }

    protected void FillDesign(DropDownList dditem, DropDownList ddquality, DropDownList DDDesign, System.Web.UI.HtmlControls.HtmlTableCell tdDesign = null)
    {
        // string str = "";        
        if (tdDesign.Visible == true)
        {
            if (ViewState["WithBuyercode"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName As Designname  
                                from ITEM_PARAMETER_MASTER  IM inner join Design V on Im.DESIGN_ID=V.designId 
                                inner join CustomerDesign CD on CD.DesignId=V.designId and Im.Item_Id=" + dditem.SelectedValue + @" and 
                                    IM.QUALITY_ID=" + ddquality.SelectedValue + " and V.masterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");

            }
            else
            {
                //                UtilityModule.ConditionalComboFill(ref DDDesign, @"select Distinct V.DesignId,DesignName+Space(2)+isnull(DesignNameAtoC,'') As Designname
                //                                                             from Design V Left Outer Join CustomerDesign CD on V.DesignId=CD.DesignId And CustomerId=" + DDCustomerCode.SelectedValue + " Where V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by designname", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By DESIGNNAME", true, "--SELECT--");
            }
        }

    }
    protected void FillShadecolor(DropDownList ddquality, DropDownList ddShade)
    {
        string str = "";
        if (OutShade.Visible == true)
        {
            if (ViewState["WithBuyercode"].ToString() == "1")
            {
                switch (Session["varMasterCompanyIDForERP"].ToString())
                {
                    case "9":
                        str = "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";
                        break;
                    default:
                        str = @"select  distinct sc.ShadecolorId,sc.ShadeColorName from ITEM_PARAMETER_MASTER Im 
                                inner join ShadeColor sc on im.SHADECOLOR_ID=sc.ShadecolorId where im.quality_id=" + ddquality.SelectedValue + " and sc.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sc.ShadeColorName";
                        break;
                }

            }
            else
            {
                str = "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By SHADECOLORNAME";

            }
            UtilityModule.ConditionalComboFill(ref ddShade, str, true, "--SELECT--");
        }

    }
    protected void fillColor(DropDownList dditem, DropDownList ddquality, DropDownList DDDesign, DropDownList DDColor, System.Web.UI.HtmlControls.HtmlTableCell tdcolor = null)
    {
        if (tdcolor.Visible == true)
        {
            if (ViewState["WithBuyercode"].ToString() == "1")
            {

                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName As ColorName from ITEM_PARAMETER_MASTER  IM inner join Color c on
                                                                     Im.COLOR_ID=C.ColorId inner join CustomerColor  CC on CC.ColorId=C.ColorId
                                                                      and Im.Item_Id=" + dditem.SelectedValue + " and IM.QUALITY_ID=" + ddquality.SelectedValue + " and DESIGN_ID=" + DDDesign.SelectedValue + " and C.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--SELECT--");

            }
            else
            {
                //                UtilityModule.ConditionalComboFill(ref DDColor, @"select Distinct C.ColorId,C.ColorName+space(2)+isnull(CC.ColorNameToC,'') As ColorName
                //                                                             from Color C Left Outer Join CustomerColor CC on C.ColorId=CC.ColorId And CustomerId=" + DDCustomerCode.SelectedValue + " Where C.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--SELECT--");
                UtilityModule.ConditionalComboFill(ref DDColor, "SELECT COLORID,COLORNAME FROM COLOR Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By COLORNAME", true, "--SELECT--");
            }
        }
    }
    protected void FillSize()
    {
        string size = "";
        string str = "";

        switch (DDsizetype.SelectedValue)
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
        //size Query
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            str = @"select Distinct S.Sizeid,S." + size + " As  " + size + @" from ITEM_PARAMETER_MASTER  IM inner join Size  S on
                    Im.SIZE_ID=S.SizeId inner join CustomerSize CS on cs.Sizeid=S.SizeId
                    and S.shapeId=" + ddShape.SelectedValue + "  and Im.Item_Id=" + ddItemName.SelectedValue + " and IM.QUALITY_ID=" + ddQuality.SelectedValue + "  and S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";
        }
        else
        {
            str = "Select Distinct S.Sizeid,S." + size + "+Space(2)+isnull(SizeNameAToC,'') As  " + size + @" From Size S Left Outer Join CustomerSize CS on S.SizeId=CS.SizeId 
                   Where shapeid=" + ddShape.SelectedValue + " And S.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by " + size + "";
        }
        UtilityModule.ConditionalComboFill(ref ddSize, str, true, "--SELECT--");
        //
        if (ChkForFillSame.Checked == true)
        {
            ddInShape.SelectedValue = ddShape.SelectedValue;
            DDInSizeType.SelectedValue = DDsizetype.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddInSize, str, true, "--SELECT--");
            ddOutShape.SelectedValue = ddShape.SelectedValue;
            DDOutSizeType.SelectedValue = DDsizetype.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddOutSize, str, true, "--SELECT--");
        }
    }
    protected void ddOutQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            FillDesign(ddOutItemName, ddOutQuality, ddOutDesign, OutDesign);
            FillShadecolor(ddOutQuality, ddOutShade);
        }
    }
    protected void ddOutDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["WithBuyercode"].ToString() == "1")
        {
            fillColor(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, OutColor);
        }
    }
    protected void ddOutShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"].ToString() == "4")
        {
            TxtOutPutRate.Text = Convert.ToString(UtilityModule.getshaderate(Convert.ToInt16(ddOutQuality.SelectedValue), Convert.ToInt16(ddOutShade.SelectedValue)));
        }
    }
    protected static int checkcategory(DropDownList ddcategory)
    {
        int id = 0;
        string str = "Select ID from CategorySeparate where categoryid=" + ddcategory.SelectedValue;
        id = Convert.ToInt16(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str));
        return id;
    }

    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    private int getSizeType(DropDownList ddsizetypeval, System.Web.UI.HtmlControls.HtmlTableCell td = null)
    {
        int sizetype;

        switch (ddsizetypeval.SelectedValue)
        {
            case "0":
                sizetype = 0;
                break;
            case "1":
                sizetype = 1;
                break;
            case "2":
                sizetype = 2;
                break;
            default:
                sizetype = 0;
                break;
        }
        return sizetype;
    }
    protected void DDOutSizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_RECEIVE();
    }
    protected void DDInSizeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILL_SIZE_ISSUE();
    }
    protected void ddProcessName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (variable.BOM_PurchaseIncludeProcess == "1")
        {

            switch (ddProcessName.SelectedItem.Text.ToUpper())
            {
                case "PURCHASE":
                    ChkForProcessInPut.Visible = false;
                    FillProcessForpurchaseinclude();
                    break;
                default:
                    ChkForProcessInPut.Visible = true;
                    GDPurchaseinclude.DataSource = null;
                    GDPurchaseinclude.DataBind();
                    break;
            }
        }
    }
    protected void ddlicaltypedginput_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList ddlicaltypedginput = sender as DropDownList;
        GridViewRow gvr = ddlicaltypedginput.NamingContainer as GridViewRow;

        int rowindex = gvr.RowIndex;
        string icaltype = ddlicaltypedginput.SelectedValue;
        for (int i = 0; i < DGInPutProcess.Rows.Count; i++)
        {
            Label lblicaltypedginput = (Label)DGInPutProcess.Rows[i].FindControl("lblicaltypedginput");
            DropDownList ddlicaltypedginputrow = (DropDownList)DGInPutProcess.Rows[i].FindControl("ddlicaltypedginput");
            if (i >= rowindex)
            {
                ddlicaltypedginputrow.SelectedValue = icaltype;
            }
        }
    }
    protected void FillProcessForpurchaseinclude()
    {
        string str = "select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master  where PROCESS_NAME_ID<>" + ddProcessName.SelectedValue + " and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by SeqNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDPurchaseinclude.DataSource = ds.Tables[0];
        GDPurchaseinclude.DataBind();
    }
    protected void chksamereceiveitem_CheckedChanged(object sender, EventArgs e)
    {
        if (chksamereceiveitem.Checked == true)
        {
            ddOutCategoryName.SelectedValue = ddInCategoryName.SelectedValue;
            ddOutCategoryName_SelectedIndexChanged(sender, e);
            //ItemName
            ddOutItemName.SelectedValue = ddInItemName.SelectedValue;
            ddOutItemName_SelectedIndexChanged(sender, e);
            //Quality
            if (InQuality.Visible == true)
            {
                ddOutQuality.SelectedValue = ddInQuality.SelectedValue;
            }
            //Design
            if (InDesign.Visible == true)
            {
                ddOutDesign.SelectedValue = ddInDesign.SelectedValue;
            }
            //Color
            if (InColor.Visible == true)
            {
                ddOutColor.SelectedValue = ddInColor.SelectedValue;
            }
            //Shape
            if (InShape.Visible == true)
            {
                ddOutShape.SelectedValue = ddInShape.SelectedValue;
            }
            //Size
            if (InSize.Visible == true)
            {
                DDOutSizeType.SelectedValue = DDInSizeType.SelectedValue;
                DDOutSizeType_SelectedIndexChanged(sender, e);
                ddOutSize.SelectedValue = ddInSize.SelectedValue;
            }
            //Shade
            if (InShade.Visible == true)
            {
                ddOutShade.SelectedValue = ddInShade.SelectedValue;
            }
            TxtOutPutQty.Text = TxtInPutQty.Text;
            TxtRecLoss.Text = TxtLoss.Text;
            TxtOutPutRate.Text = TxtInPutRate.Text;
            ddOUnit.SelectedValue = ddIUnit.SelectedValue;
            ddOCalType.SelectedValue = DDICALTYPE.SelectedValue;
        }
    }
    protected int GetGridColumnId(string ColName)
    {
        int columnid = -1;
        foreach (DataControlField col in DG.Columns)
        {
            if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
            {
                columnid = DG.Columns.IndexOf(col);
                break;
            }
        }
        return columnid;
    }
    //protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    //e.Row.Cells[GetGridColumnId("QUALITYCODEID")].Visible = false;        
    //    ////**      
    //    //switch (Session["varMasterCompanyIDForERP"].ToString())
    //    //{
    //    //    case "4":
    //    //        break;
    //    //    default:
    //    //        e.Row.Cells[GetGridColumnId("DyingMatch")].Visible = false;
    //    //        e.Row.Cells[GetGridColumnId("Dyeing")].Visible = false;
    //    //        e.Row.Cells[GetGridColumnId("DyingType")].Visible = false;
    //    //        break;
    //    //}
    //    //if (variable.BOM_PurchaseIncludeProcess == "0")
    //    //{
    //    //    e.Row.Cells[GetGridColumnId("INCLUDING_PROCESS")].Visible = false;
    //    //}
    //    //else
    //    //{
    //    //    if (ddProcessName.SelectedItem.Text.ToUpper() != "PURCHASE")
    //    //    {
    //    //        e.Row.Cells[GetGridColumnId("INCLUDING_PROCESS")].Visible = false;
    //    //    }
    //    //}
    //}

    protected void DGInPutProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblicaltypedginput = (Label)e.Row.FindControl("lblicaltypedginput");
            DropDownList ddlicaltypedginput = (DropDownList)e.Row.FindControl("ddlicaltypedginput");
            ddlicaltypedginput.SelectedValue = lblicaltypedginput.Text;
        }
    }
    private void Report()
    {
        DataSet ds = new DataSet();
       
        string STR = "";

        if (ddQuality.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.QualityId=" + ddQuality.SelectedValue + "";
        }       
        if (ddDesign.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.designId=" + ddDesign.SelectedValue + "";

        }
        if (ddColor.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.ColorId=" + ddColor.SelectedValue + "";
        }
        if (ddShape.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.ShapeId=" + ddShape.SelectedValue + "";
        }
        if (ddSize.SelectedIndex > 0)
        {
            STR = STR + @" AND VF.SizeId=" + ddSize.SelectedValue + "";
        }
        if (ddProcessName.SelectedIndex > 0)
        {
            STR = STR + @" AND PCM.PROCESSID=" + ddProcessName.SelectedValue + "";
        }
        ////STR = STR + " order by PCD.PCMDID";


        SqlParameter[] array = new SqlParameter[4];
        array[0] = new SqlParameter("@CategoryId", ddCategoryName.SelectedValue);
        array[1] = new SqlParameter("@ItemId", ddItemName.SelectedValue);
        array[2] = new SqlParameter("@where", STR);   
        array[3] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_DefineBombAndConsumptionExcelReport", array);

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

            sht.Range("A1").Value = "DEFINE BOMB AND CONSUMPTION DETAILS";
            sht.Range("A1:M1").Style.Font.Bold = true;
            sht.Range("A1:M1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:M1").Merge();

            //Headers
            sht.Range("A3").Value = "PROCESS NAME";
            sht.Range("B3").Value = "CATEGORY";
            sht.Range("C3").Value = "ITEM NAME";
            sht.Range("D3").Value = "QUALITY NAME";
            sht.Range("E3").Value = "DESIGN NAME";
            sht.Range("F3").Value = "COLOR NAME";
            sht.Range("G3").Value = "SHAPE NAME";
            sht.Range("H3").Value = "SIZE NAME";

            sht.Range("I3").Value = "ITEM NAME";
            sht.Range("J3").Value = "QUALITY NAME";
            sht.Range("K3").Value = "SHADE COLORNAME";
            sht.Range("L3").Value = "CONSUMPTION QTY";
            sht.Range("M3").Value = "DATE";
           

            //sht.Range("I1:R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            sht.Range("A3:M3").Style.Font.Bold = true;

            row = 4;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["Process_Name"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["Category_Name"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["Item_Name"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["QualityName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["FinishingSize"]);

                sht.Range("I" + row).SetValue(ds.Tables[0].Rows[i]["InputOutputItemName"]);
                sht.Range("J" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutQualityName"]);
                sht.Range("K" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutShadeColorName"]);
                sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["InputOutPutConsumptionQty"]);
                sht.Range("M" + row).SetValue(ds.Tables[0].Rows[i]["AddedDate"]);
                
                //sht.Range("L" + row).SetValue(ds.Tables[0].Rows[i]["JobIssueULLNo"]);
                //sht.Range("L" + row).Style.NumberFormat.Format = "@";                

                row = row + 1;

            }
            sht.Range("I" + row + ":R" + row).Style.Font.Bold = true;
            sht.Columns(1, 20).AdjustToContents();
            //********************
            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("DefineBombAndConsumptionReport:" + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + "." + Fileextension);
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


            //////Export to excel
            ////GridView GridView1 = new GridView();
            ////GridView1.AllowPaging = false;

            ////GridView1.DataSource = ds;
            ////GridView1.DataBind();

            ////Response.Clear();
            ////Response.Buffer = true;
            ////Response.AddHeader("content-disposition",
            //// "attachment;filename=DefineBombAndConsumptionReport" + DateTime.Now + ".xls");
            ////Response.Charset = "";
            ////Response.ContentType = "application/vnd.ms-excel";
            ////StringWriter sw = new StringWriter();
            ////HtmlTextWriter hw = new HtmlTextWriter(sw);

            ////for (int i = 0; i < GridView1.Rows.Count; i++)
            ////{
            ////    //Apply text style to each Row
            ////    GridView1.Rows[i].Attributes.Add("class", "textmode");
            ////}
            ////GridView1.RenderControl(hw);

            //////style to format numbers to string
            ////string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            ////Response.Write(style);
            ////Response.Output.Write(sw.ToString());
            ////Response.Flush();
            ////Response.End();

        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void BtnExcelFormat_Click(object sender, EventArgs e)
    {
        Report();
    }
}