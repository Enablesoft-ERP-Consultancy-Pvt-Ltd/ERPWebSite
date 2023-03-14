using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Masters_Carpet_FrmOrderWiseConsumption : System.Web.UI.Page
{
    int PROCESSCONSUMPTIONMASTER_ID = 0;
    string SizeString = "SizeFt";
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        MasterCompanyId = Convert.ToInt32(Session["varCompanyId"]);
        if (IsPostBack == false)
        {
            logo();
            SizeString = "SizeFt";
            LblSub_Quality.Visible = false;
            ddFinished.Visible = false;
            ddFinishedIN.Visible = false;
            BtnOpenOtherExpense.Visible = false;
            BtnOpenPackingCost.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by CompanyName", true, "--SELECT--");
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                CustomerCodeSelectedChange();
            }
            UtilityModule.ConditionalComboFill(ref ddProcessName, "Select Process_Name_Id,Process_Name from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order BY Process_Name", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddOutCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            Session["PCMID"] = PROCESSCONSUMPTIONMASTER_ID;
            LblQty.Text = "0";
            if (Request.QueryString["ZZZ"] == "1")
            {
                zzz.Style.Add("display", "none");
            }
            lablechange();
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            switch (Convert.ToInt16(Session["varCompanyId"]))
            {
                case 1:
                    LblSub_Quality.Visible = true;
                    LblQty.Visible = true;
                    btnopen.Visible = false;
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
                    break;
                case 3:
                    ChkForOneToOne.Checked = true;
                    ddFinished.Visible = false;
                    ddFinishedIN.Visible = false;
                    LblQty.Visible = false;
                    btnopen.Visible = true;
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
                    break;
                default:
                    btnopen.Visible = false;
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
                    break;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
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
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    private void CustomerCodeSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster Where customerid=" + DDCustomerCode.SelectedValue, true, "--SELECT--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FILLGRID();
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
        UtilityModule.ConditionalComboFill(ref ddItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "SELECT--");
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
    }

    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedChange();
    }
    private void ItemNameSelectedChange()
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
        Fill_Sub_Quality();
        if (ChkForFillSame.Checked == true)
        {
            ddInQuality.SelectedValue = ddQuality.SelectedValue;
            ddOutQuality.SelectedValue = ddQuality.SelectedValue;
        }
        btnaddquality.Focus();
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
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        TDMtrSize.Visible = true;
        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By SIZEFT", true, "--SELECT--");
        if (ChkForFillSame.Checked == true)
        {
            ddInShape.SelectedValue = ddShape.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
            ddOutShape.SelectedValue = ddShape.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        }
    }
    protected void ddOutCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        OUT_CATEGORY_DEPENDS_CONTROLS();
        ddOutItemName.Focus();
    }

    private void Fill_Sub_Quality()
    {
        if (ddItemName.SelectedIndex > 0 && ddQuality.SelectedIndex > 0 && LblSub_Quality.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddSub_Quality, "Select QualityCodeId,SubQuantity from qualityCodeMaster Where Item_Id=" + ddItemName.SelectedValue + " And QualityId=" + ddQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By SubQuantity", true, "--SELECT SUB_QUALITY--");
        }
    }

    private void OUT_CATEGORY_DEPENDS_CONTROLS()
    {
        if (ddFinished.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddFinished, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE Where MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY ID", true, "--SELECT--");
        }
        OutQuality.Visible = false;
        OutDesign.Visible = false;
        OutColor.Visible = false;
        OutShape.Visible = false;
        OutSize.Visible = false;
        OutShade.Visible = false;
        UtilityModule.ConditionalComboFill(ref ddOutItemName, "Select ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddOutCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "--SELECT--");
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
        UtilityModule.ConditionalComboFill(ref ddOUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddOutItemName.SelectedValue + " Order BY U.UNITNAME", true, "--SELECT--");
        if (ddOUnit.Items.Count > 0)
        {
            ddOUnit.SelectedIndex = 1;
        }
        FILLGRID();
    }

    protected void ddOutShape_SelectedIndexChanged(object sender, EventArgs e)
    {

        UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void ddInCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        IN_CATEGORY_DEPENDS_CONTROLS();
        ddInItemName.Focus();
    }

    private void IN_CATEGORY_DEPENDS_CONTROLS()
    {
        if (ddFinishedIN.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref ddFinishedIN, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE Where MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY FINISHED_TYPE_NAME", true, "--SELECT--");
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
            UtilityModule.ConditionalComboFill(ref ddInItemName, "SELECT DISTINCT IM.ITEM_ID,ITEM_NAME from ITEM_MASTER IM,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=PD.IFINISHEDID AND IPM.ITEM_ID=IM.ITEM_ID AND CATEGORY_ID=" + ddInCategoryName.SelectedValue + " AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "--SELECT--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddInItemName, "Select DISTINCT ITEM_ID,ITEM_NAME from ITEM_MASTER Where CATEGORY_ID=" + ddInCategoryName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ITEM_NAME", true, "SELECT--");
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
        FILLGRID();
        ddInQuality.Focus();
    }
    private void InItemNameSelectedIndexChange()
    {
        if (ChkForProcessInPut.Checked == true)
        {
            QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
            UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD WHERE PM.PCMID=PD.PCMID AND U.UNITID=PD.IUNITID AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"] + " Order By U.UNITNAME", true, "--SELECT--");
        }
        else
        {
            QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
            UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddInItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order BY U.UNITNAME", true, "--SELECT--");
        }
        if (ddIUnit.Items.Count > 0)
        {
            ddIUnit.SelectedIndex = 1;
        }

    }
    protected void fill_grid1()
    {
        DGInPutProcess.DataSource = getdetail();
        DGInPutProcess.DataBind();
    }
    protected DataSet getdetail()
    {
        DataSet ds = null;
        DataSet ds1 = null;
        string Str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varCompanyId"]));
            Str = @"SELECT COUNT(*) COUNT,OFINISHEDID,INOUTTYPEID from PROCESSCONSUMPTIONMASTER PCM,PROCESSCONSUMPTIONDETAIL PCD WHERE PCM.PCMID=PCD.PCMID AND 
                   PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " And PCM.MasterCompanyId=" + Session["varCompanyId"] + " GROUP BY OFINISHEDID,INOUTTYPEID HAVING COUNT(*)>1 ORDER BY COUNT(*)";
            ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);

            Str = @"SELECT DISTINCT PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,OQty PreQty,OQty Qty,0 Loss FROM PROCESSCONSUMPTIONMASTER PCM,
                 PROCESSCONSUMPTIONDETAIL PCD,ITEM_PARAMETER_MASTER IPCM,VIEWFINDFINISHEDID1 VF1 WHERE PCM.PCMID=PCD.PCMID AND 
                 PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " And PCM.MasterCompanyId=" + Session["varCompanyId"];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds1.Tables[0].Rows[0]["INOUTTYPEID"]) == 1)
                {
                    Str = @"SELECT DISTINCT Top (1) PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,Round(OQTY,3) PreQty,
                    Round(OQTY,3) Qty,0 Loss FROM PROCESSCONSUMPTIONMASTER PCM,ITEM_PARAMETER_MASTER IPCM,
                    VIEWFINDFINISHEDID1 VF1,PROCESSCONSUMPTIONDETAIL PCD
                    WHERE PCM.PCMID=PCD.PCMID AND PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND PCM.MasterCompanyId=" + Session["varCompanyId"] + @" And
                    PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + " AND PCD.OFINISHEDID=" + ds1.Tables[0].Rows[0]["OFINISHEDID"] + @"
                    UNION SELECT DISTINCT PCMDID,VF1.QDCS+Space(3)+SizeFt+Space(3)+ShadeColor Description,Round(OQTY,3) PreQty,
                    Round(OQTY,3) Qty,0 Loss FROM PROCESSCONSUMPTIONMASTER PCM,ITEM_PARAMETER_MASTER IPCM,
                    VIEWFINDFINISHEDID1 VF1,PROCESSCONSUMPTIONDETAIL PCD
                    WHERE PCM.PCMID=PCD.PCMID AND PCM.FINISHEDID=IPCM.ITEM_FINISHED_ID AND VF1.FINISHEDID=PCD.OFINISHEDID AND 
                    PCM.PROCESSID=" + ddInProcessName.SelectedValue + " AND PCM.FINISHEDID=" + Varfinishedid + @" And OFINISHEDID<>" + ds1.Tables[0].Rows[0]["OFINISHEDID"] + " And PCM.MasterCompanyId=" + Session["varCompanyId"];
                }
            }
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Str);
        }
        catch (Exception ex)
        {
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
    }
    protected void ddInQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtLoss.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select loss from quality where qualityid=" + ddInQuality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
    }

    protected void ddInShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + "", true, "--SELECT--");
    }
    private void QDCSDDFill(DropDownList Quality, DropDownList Design, DropDownList Color, DropDownList Shape, DropDownList Shade, int Itemid, int Type_Flag)
    {
        string Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + Itemid + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QUALITYNAME";
        if (LblSub_Quality.Visible == true && Type_Flag == 1)
        {
            Str = "SELECT QUALITYID,QUALITYNAME FROM QUALITY Q,qualitycodeDetail QD WHERE Q.QualityId=QD.Quality_ID And Q.ITEM_ID=" + Itemid + " And QD.qualitycodeid=" + ddSub_Quality.SelectedValue + " And Q.MasterCompanyId=" + Session["varCompanyId"] + " Order By QUALITYNAME";
        }
        UtilityModule.ConditionalComboFill(ref Quality, Str, true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Design, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Color, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SHAPENAME", true, "--SELECT--");
        UtilityModule.ConditionalComboFill(ref Shade, "SELECT SHADECOLORID,SHADECOLORNAME FROM SHADECOLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SHADECOLORNAME", true, "--SELECT--");
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyId"].ToString() == "2")
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
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
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
            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varCompanyId"]));
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "SELECT * FROM PROCESSCONSUMPTIONMASTER WHERE PROCESSID=" + ddProcessName.SelectedValue + " AND FINISHEDID=" + Varfinishedid + " And MasterCompanyId=" + Session["varCompanyId"] + "");
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
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE PROCESSCONSUMPTIONDETAIL WHERE PCMID=" + Session["PCMID"]);
            }
            if (num == 1)
            {
                SqlParameter[] _arrpara = new SqlParameter[7];
                _arrpara[0] = new SqlParameter("@PCMID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@FINISHEDID", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@CalType", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@Rate", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[6] = new SqlParameter("@varCompanyId", SqlDbType.Int);

                _arrpara[0].Value = (Session["PCMID"]);
                _arrpara[1].Value = ddProcessName.SelectedValue;
                _arrpara[2].Value = Varfinishedid;
                _arrpara[3].Value = 0;
                _arrpara[4].Value = 0;
                _arrpara[5].Value = Session["varuserid"];
                _arrpara[6].Value = Session["varCompanyId"];
                str = @"INSERT INTO PROCESSCONSUMPTIONMASTER(PCMID,PROCESSID,FINISHEDID,CALTYPE,RATE,userid,MasterCompanyid) Values (" + _arrpara[0].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[3].Value + "," + _arrpara[4].Value + "," + _arrpara[5].Value + "," + _arrpara[6].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            for (int i = 0; i < DGInPutProcess.Rows.Count; i++)
            {
                int VarOutfinishedid = UtilityModule.getItemFinishedId(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutSize, TxtOutProdCode, ddOutShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from PROCESSCONSUMPTIONDetail Where PCMID=" + Session["PCMID"] + " And IFINISHEDID in (Select OFINISHEDID from PROCESSCONSUMPTIONDetail Where PCMDID=" + DGInPutProcess.Rows[i].Cells[0].Text + ") And OFINISHEDID=" + VarOutfinishedid + "");
                if (BtnSave.Text != "UpDate" && Ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "DATA ALLREADY EXISTS.....";
                }
                else
                {
                    SqlParameter[] _arrpara = new SqlParameter[24];
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

                    _arrpara[0].Value = (Session["PCMID"]);
                    _arrpara[1].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMDID ),0)+1 FROM PROCESSCONSUMPTIONDETAIL"));
                    _arrpara[2].Value = VarOutfinishedid;
                    _arrpara[3].Value = ddOUnit.SelectedValue;
                    _arrpara[4].Value = TxtOutPutQty.Text;
                    _arrpara[5].Value = TxtOutPutRate.Text;
                    _arrpara[6].Value = ChkForProcessInPut.Checked == true ? Convert.ToInt32(ddInProcessName.SelectedValue) : 0;
                    _arrpara[7].Value = ChkForManyOutPut.Checked == true ? 2 : ChkForOneToOne.Checked == true ? 0 : 1;
                    _arrpara[8].Value = 0;
                    _arrpara[9].Value = 0;
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

                        _arrpara[12].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtp_tage")).Text), 3);
                        _arrpara[11].Value = Math.Round(Convert.ToDouble(((TextBox)DGInPutProcess.Rows[i].FindControl("txtDGLoss")).Text), 3);

                        str = @"INSERT INTO PROCESSCONSUMPTIONDETAIL (PCMID,PCMDID,IFINISHEDID,IUNITID,IQTY,ILOSS,IRATE,OFINISHEDID,OUNITID,OQTY,ORATE,PROCESSINPUTID,INOUTTYPEID,ICALTYPE,I_FINISHED_Type_ID,
                                O_FINISHED_Type_ID,SUB_QUALITY_ID,OCALTYPE,OLoss) Select " + _arrpara[0].Value + "," + _arrpara[1].Value + ",OFINISHEDID,OUNITID," + _arrpara[12].Value + "," + _arrpara[11].Value + ",ORATE," + _arrpara[2].Value + "," + _arrpara[3].Value + "," + _arrpara[4].Value + "," + _arrpara[5].Value + "," + _arrpara[6].Value + "," + _arrpara[7].Value + ",OCALTYPE,O_FINISHED_Type_ID," + _arrpara[8].Value + "," + _arrpara[9].Value + "," + _arrpara[10].Value + " from PROCESSCONSUMPTIONDetail Where PCMDID=" + DGInPutProcess.Rows[i].Cells[0].Text + "";
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
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
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
        }
    }
    private void Save_ProcessInPut_Not_Check(SqlTransaction Tran)
    {
        CHECKVALIDCONTROL();
        if (lblMessage.Text == "")
        {
            SqlParameter[] _arrpara = new SqlParameter[25];
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
            _arrpara[24] = new SqlParameter("@RecLoss", SqlDbType.Float);

            int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varCompanyId"]));

            _arrpara[1].Value = ddProcessName.SelectedValue;
            _arrpara[2].Value = Varfinishedid;

            int VarInfinishedid = UtilityModule.getItemFinishedId(ddInItemName, ddInQuality, ddInDesign, ddInColor, ddInShape, ddInSize, TxtOutProdCode, Tran, ddInShade, "", Convert.ToInt32(Session["varCompanyId"]));
            _arrpara[4].Value = VarInfinishedid;
            _arrpara[5].Value = ddIUnit.SelectedValue;
            // int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyId"].ToString() == "2")
            {
                _arrpara[6].Value = TxtInPutQty.Text;
                _arrpara[7].Value = TxtLoss.Text;
                _arrpara[11].Value = TxtOutPutQty.Text;
                _arrpara[24].Value = TxtRecLoss.Text;
            }
            else if (Session["varCompanyId"].ToString() == "1")
            {
                if (Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select UnitId from qualityCodeMaster Where QualityCodeid=" + ddSub_Quality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "")) == 1)
                {
                    _arrpara[6].Value = Convert.ToDouble(TxtInPutQty.Text) / 1.196;
                    _arrpara[7].Value = Convert.ToDouble(TxtLoss.Text) / 1.196;
                    _arrpara[11].Value = Convert.ToDouble(TxtOutPutQty.Text) / 1.196;
                    _arrpara[24].Value = Convert.ToDouble(TxtRecLoss.Text) / 1.196;
                }
                else
                {
                    _arrpara[6].Value = TxtInPutQty.Text;
                    _arrpara[7].Value = TxtLoss.Text;
                    _arrpara[11].Value = TxtOutPutQty.Text;
                    _arrpara[24].Value = TxtRecLoss.Text;
                }
            }
            else
            {
                _arrpara[6].Value = TxtInPutQty.Text;
                _arrpara[7].Value = TxtLoss.Text;
                _arrpara[11].Value = TxtOutPutQty.Text;
                _arrpara[24].Value = TxtRecLoss.Text;
            }

            _arrpara[8].Value = TxtInPutRate.Text;
            int VarOutfinishedid = UtilityModule.getItemFinishedId(ddOutItemName, ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutSize, TxtOutProdCode, Tran, ddOutShade, "", Convert.ToInt32(Session["varCompanyId"]));
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
            _arrpara[23].Value = Session["varCompanyId"].ToString();
            if (BtnSave.Text == "Save")
            {
                _arrpara[0].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMID ),0)+1 FROM PROCESSCONSUMPTIONMASTER"));
                _arrpara[3].Value = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "SELECT Isnull(Max(PCMDID ),0)+1 FROM PROCESSCONSUMPTIONDETAIL"));
            }
            else
            {
                _arrpara[3].Value = DG.SelectedValue;
            }

            if (BtnSave.Text == "UpDate")
            {
                string str = @"Update ORDER_CONSUMPTION_DETAIL Set IFINISHEDID=" + _arrpara[4].Value + ",IUNITID=" + _arrpara[5].Value + ",IQTY=" + _arrpara[6].Value + @",
                    ILOSS=" + _arrpara[7].Value + ",IRATE=" + _arrpara[8].Value + ",OFINISHEDID=" + _arrpara[9].Value + ",OUNITID=" + _arrpara[10].Value + ",OQTY=" + _arrpara[11].Value + @",
                    ORATE=" + _arrpara[12].Value + ",PROCESSINPUTID=" + _arrpara[13].Value + ",INOUTTYPEID=" + _arrpara[14].Value + ",ICALTYPE=" + _arrpara[17].Value + @",
                    I_FINISHED_Type_ID=" + _arrpara[18].Value + ",O_FINISHED_Type_ID=" + _arrpara[19].Value + ",SUB_QUALITY_ID=" + _arrpara[20].Value + ",OCALTYPE=" + _arrpara[21].Value + @", 
                    OLoss=" + _arrpara[24].Value + " From ORDER_CONSUMPTION_DETAIL Where ORDERID=" + DDOrderNo.SelectedValue + " And Processid=" + ddProcessName.SelectedValue + " And PCMDID=" + _arrpara[3].Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            else
            {
                DataSet Ds1;
                DataSet Ds3;
                Ds3 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * From ORDERDETAIL Where Orderid=" + DDOrderNo.SelectedValue);
                if (Ds3.Tables[0].Rows.Count > 0)
                {
                    for (int num = 0; num < Ds3.Tables[0].Rows.Count; num++)
                    {
                        string Str = "Select * from Item_ParaMeter_Master IPM,ITem_Master IM Where IPM.Item_Id=IM.Item_Id And IPM.Item_Finished_ID=" + Ds3.Tables[0].Rows[num]["ITEM_FINISHED_ID"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                        DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                            Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=" + Ds.Tables[0].Rows[0]["Design_ID"] + " And Color_ID=-1 And Size_ID=-1 And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=" + Ds.Tables[0].Rows[0]["Color_ID"] + " And Size_ID=-1 And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=" + Ds.Tables[0].Rows[0]["Size_ID"] + " And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count == 0)
                            {
                                Str = "Select IPCM.*,IM.CATEGORY_ID From ITEM_PARAMETER_MASTER IPCM,ITEM_MASTER IM Where IM.ITEM_ID=IPCM.ITEM_ID And ITEM_FINISHED_ID=" + _arrpara[2].Value + " And Category_ID=" + Ds.Tables[0].Rows[0]["Category_ID"] + " And IPCM.Item_ID=" + Ds.Tables[0].Rows[0]["Item_ID"] + " And Quality_ID=" + Ds.Tables[0].Rows[0]["Quality_ID"] + " And Shape_ID=" + Ds.Tables[0].Rows[0]["Shape_ID"] + " And Design_ID=-1 And Color_ID=-1 And Size_ID=-1 And IM.MasterCompanyId=" + Session["varCompanyId"];
                                Ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                            }
                            if (Ds1.Tables[0].Rows.Count > 0)
                            {
                                Str = @"Insert into ORDER_CONSUMPTION_DETAIL (PCMID,PCMDID,PROCESSID,FINISHEDID,IFINISHEDID,IUNITID,IQTY,ILOSS,IRATE,OFINISHEDID,OUNITID,OQTY,
                                         ORATE,PROCESSINPUTID,INOUTTYPEID,ICALTYPE,I_FINISHED_Type_ID,O_FINISHED_Type_ID,SUB_QUALITY_ID,OCALTYPE,ORDERID,ORDERDETAILID,OLoss) Values
                                         (" + _arrpara[0].Value + "," + _arrpara[3].Value + "," + _arrpara[1].Value + "," + _arrpara[2].Value + "," + _arrpara[4].Value + @",
                                          " + _arrpara[5].Value + "," + _arrpara[6].Value + "," + _arrpara[7].Value + "," + _arrpara[8].Value + "," + _arrpara[9].Value + @",
                                          " + _arrpara[10].Value + "," + _arrpara[11].Value + "," + _arrpara[12].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + @",
                                          " + _arrpara[17].Value + "," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + @",
                                          " + Ds3.Tables[0].Rows[num]["ORDERID"] + "," + Ds3.Tables[0].Rows[num]["ORDERDETAILID"] + "," + _arrpara[24].Value + ")";
                                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                            }
                        }
                    }
                }
            }
            Tran.Commit();
            lblMessage.Visible = true;
            Save_Refresh();
            FILLGRID();
            lblMessage.Text = "DATA SUCCESSFULLY SAVED.....";
            BtnSave.Text = "Save";
            VISIABLE_TRUE_FALSE_ACC_TO_MANY_REC();
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
    }
    private void Save_Refresh()
    {
        TxtInProdCode.Text = "";
        TxtOutProdCode.Text = "";
        if (ChkForManyOutPut.Checked == true)
        {
            //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            switch (Convert.ToInt16(Session["varCompanyId"]))
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
            UtilityModule.ConditionalComboFill(ref ddInProcessName, "SELECT DISTINCT PROCESS_NAME_ID,PROCESS_NAME FROM PROCESSCONSUMPTIONMASTER PCM,PROCESS_NAME_MASTER PM WHERE PCM.PROCESSID=PM.PROCESS_NAME_ID AND PCM.PROCESSID<>" + ddProcessName.SelectedValue + " And PCM.MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--SELECT--");
            DGInPutProcess.Visible = true;
            //Tr1.Disabled = true;
            //Tr3.Disabled = true;
            //TrOUT1.Disabled = false;
            //TrOUT2.Disabled = false;
            ddInProcessName.Focus();
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
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
    }
    private void InProcessNameSelectedIndChange()
    {
        UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select DISTINCT ICM.Category_Id,ICM.Category_Name FROM ITEM_CATEGORY_MASTER ICM,PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD,ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM WHERE PM.PCMID=PD.PCMID AND IPM.ITEM_FINISHED_ID=IFINISHEDID AND IPM.ITEM_ID=IM.ITEM_ID AND IM.CATEGORY_ID=ICM.CATEGORY_ID AND PM.PROCESSID=" + ddInProcessName.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"] + " Order By ICM.Category_Name", true, "--SELECT--");
        fill_grid1();
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddProcessName.SelectedIndex > 0 && ddQuality.SelectedIndex > 0 && ddDesign.SelectedIndex > 0 && ddColor.SelectedIndex > 0 && ddShape.SelectedIndex > 0 && ddSize.SelectedIndex > 0)
        {
            FILLGRID();
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
        if (DDOrderNo.SelectedIndex > 0)
        {
            string STR = @"SELECT DISTINCT OCD.PCMDID,PM.PROCESS_NAME, VD1.QDCS + SPACE(3) + VD1.SizeFt AS ITEMCODE, VD2.QDCS + SPACE(3) + VD2.SizeFt + SPACE(3) + VD2.ShadeColor + SPACE(10) 
                         + ISNULL(FT.FINISHED_TYPE_NAME, '') AS INPUT_ITEM, CASE WHEN QCM.UnitId = 1 THEN IQTY * 1.196 ELSE IQTY END AS IQTY, 
                         CASE WHEN QCM.UnitId = 1 THEN ILOSS * 1.196 ELSE ILOSS END AS ILOSS,OCD.IRATE, IU.UnitName AS I_UNIT, VD3.QDCS + SPACE(3) + VD3.SizeFt + SPACE(3) 
                         + VD3.ShadeColor + SPACE(10) + ISNULL(FT1.FINISHED_TYPE_NAME, '') AS OUTPUT_ITEM, CASE WHEN QCM.UnitId = 1 THEN OQTY * 1.196 ELSE OQTY END AS OQTY,OCD.ORATE, OU.UnitName AS O_UNIT,QCM.QualityCodeId
                         FROM ORDER_CONSUMPTION_DETAIL AS OCD INNER JOIN PROCESS_NAME_MASTER AS PM ON OCD.PROCESSID = PM.PROCESS_NAME_ID INNER JOIN
                         ViewFindFinishedid1 AS VD1 ON OCD.FINISHEDID = VD1.Finishedid INNER JOIN ViewFindFinishedid1 AS VD2 ON OCD.IFINISHEDID = VD2.Finishedid INNER JOIN
                         ViewFindFinishedid1 AS VD3 ON OCD.OFINISHEDID = VD3.Finishedid INNER JOIN Unit AS IU ON OCD.IUNITID = IU.UnitId INNER JOIN
                         Unit AS OU ON OCD.OUNITID = OU.UnitId LEFT OUTER JOIN FINISHED_TYPE AS FT1 ON OCD.O_FINISHED_Type_ID = FT1.Id LEFT OUTER JOIN
                         FINISHED_TYPE AS FT ON OCD.I_FINISHED_Type_ID = FT.Id LEFT OUTER JOIN QualityCodeMaster AS QCM ON OCD.SUB_QUALITY_ID = QCM.QualityCodeId
                         Where OCD.Orderid=" + DDOrderNo.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
            if (ddProcessName.SelectedIndex > 0)
            {
                STR = STR + " And OCD.PROCESSID=" + ddProcessName.SelectedValue;
            }
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, STR);
            DG.DataSource = Ds;
            DG.DataBind();
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
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            Str = "Select IPM.*,IM.CATEGORY_ID,IsNull(MII.REMARKS,'') REMARKS from ITEM_MASTER IM,CategorySeparate CS,ITEM_PARAMETER_MASTER IPM Left Outer Join MAIN_ITEM_IMAGE MII ON IPM.ITEM_FINISHED_ID=MII.FINISHEDID Where IM.Category_Id=CS.CategoryId And IPM.ITEM_ID=IM.ITEM_ID  And Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " and ProductCode='" + TxtProdCode.Text + "'";
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtFinishedid.Text = ds.Tables[0].Rows[0]["ITEM_FINISHED_ID"].ToString();
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddQuality, ddDesign, ddColor, ddShape, ddShade, Convert.ToInt32(ddItemName.SelectedValue), 0);
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                Fill_Sub_Quality();
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT FROM SIZE WHERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                TxtRemarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                FILLGRID();
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
            UtilityModule.ConditionalComboFill(ref ddOutCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  where ProductCode='" + TxtOutProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddOutCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                OUT_CATEGORY_DEPENDS_CONTROLS();
                ddOutItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                FILLGRID();
                QDCSDDFill(ddOutQuality, ddOutDesign, ddOutColor, ddOutShape, ddOutShade, Convert.ToInt32(ddOutItemName.SelectedValue), 1);
                UtilityModule.ConditionalComboFill(ref ddOUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddOutItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order BY U.UNITNAME", true, "--SELECT--");
                ddOUnit.SelectedIndex = 1;
                ddOutQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddOutDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddOutColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddOutShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddOutSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();

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
            UtilityModule.ConditionalComboFill(ref ddInCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
            if (ChkForProcessInPut.Checked == true)
            {
                Str = "select IPM.*,IM.CATEGORY_ID,O_Finished_Type_Id,OQTY from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  inner join  PROCESSCONSUMPTIONDetail PCMD on PCMD.OFINISHEDID=IPM.Item_Finished_Id  where IPM.ITEM_ID=IM.ITEM_ID  and ProductCode='" + TxtInProdCode.Text + "'and PCMid in (select pcmid from PROCESSCONSUMPTIONMaster,Item_Parameter_Master where Productcode='" + TxtProdCode.Text + "' and processid=" + ddInProcessName.SelectedValue + "  and Item_Finished_Id=FinishedId) And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                Str = "select IPM.*,IM.CATEGORY_ID from ITEM_PARAMETER_MASTER IPM inner join ITEM_MASTER IM  on IPM.Item_Id=IM.Item_Id  where ProductCode='" + TxtInProdCode.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddInCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                IN_CATEGORY_DEPENDS_CONTROLS();
                ddInItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                QDCSDDFill(ddInQuality, ddInDesign, ddInColor, ddInShape, ddInShade, Convert.ToInt32(ddInItemName.SelectedValue), 1);
                UtilityModule.ConditionalComboFill(ref ddIUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM UNIT U,UNIT_TYPE_MASTER UT,ITEM_MASTER IM WHERE U.UNITTYPEID=UT.UNITTYPEID AND UT.UNITTYPEID=IM.UNITTYPEID AND ITEM_ID=" + ddInItemName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By U.UNITNAME", true, "--SELECT--");
                ddIUnit.SelectedIndex = 1;
                ddInQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddInDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddInColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddInShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddInSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
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
    }

    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Category_Id,Category_Name from ITEM_CATEGORY_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Category_Name", true, "--SELECT--");
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
        UtilityModule.ConditionalComboFill(ref ddQuality, "SELECT QUALITYID,QUALITYNAME FROM QUALITY WHERE ITEM_ID=" + ddItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QUALITYNAME", true, "--SELECT--");
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--SELECT--");
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--SELECT--");
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShape, "SELECT SHAPEID,SHAPENAME FROM SHAPE Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SHAPENAME", true, "--SELECT--");
    }
    protected void refreshsize_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID,SIZEFT fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName", true, "--SELECT--");

    }
    protected void btnrefreshprocess_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddProcessName, "Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " order by PROCESS_NAME", true, "--SELECT--");
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
            // VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
            if (Session["varCompanyId"].ToString() == "4")
            {
                LblQty.Text = TxtInPutQty.Text;
                int Varfinishedid = UtilityModule.getItemFinishedId(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, Tran, ddShade, CHKFORALLDESIGN, CHKFORALLCOLOR, CHKFORALLSIZE, "", Convert.ToInt32(Session["varCompanyId"]));
                LblQty.Text = (Convert.ToDouble(TxtInPutQty.Text) - Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Round(Isnull(Sum(OQty),0),4) Qty From ORDER_CONSUMPTION_DETAIL Where PROCESSID=" + ddProcessName.SelectedValue + " And FINISHEDID=" + Varfinishedid + ""))).ToString();
                TxtOutPutQty.Text = LblQty.Text;
            }
            else if (Session["varCompanyId"] == "2")
            {
                TxtOutPutQty.Text = TxtInPutQty.Text;
            }
            TxtLoss.Focus();
        }
        catch
        {
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
        UtilityModule.ConditionalComboFill(ref ddFinished, "SELECT ID,FINISHED_TYPE_NAME FROM FINISHED_TYPE Where MasterCompanyId=" + Session["varCompanyId"] + " ORDER BY FINISHED_TYPE_NAME", true, "--SELECT--");
    }
    protected void ddSub_Quality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddSub_Quality.SelectedIndex > 0)
        {
            LblQty.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Isnull(Round(Quantity/1.196,3),0) from QualityCodeMaster Where QualityCodeId=" + ddSub_Quality.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "").ToString();
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
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
        UtilityModule.ConditionalComboFill(ref ddInShade, "SELECT ShadecolorId,ShadeColorName FROM ShadeColor Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ShadeColorName", true, "--SELECT--");
    }
    protected void btnrefreshsbqlt_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddSub_Quality, "Select QualityCodeId,SubQuantity from qualityCodeMaster Where MasterCompanyId=" + Session["varCompanyId"] + " Order By SubQuantity", true, "--SELECT SUB_QUALITY--");
    }
    protected void CHKFORALLDESIGN_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKFORALLDESIGN.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--ALL--");
            ddInDesign.SelectedIndex = 1;
            ddOutDesign.SelectedIndex = 1;
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddDesign, "SELECT DESIGNID,DESIGNNAME from DESIGN Where MasterCompanyId=" + Session["varCompanyId"] + " Order By DESIGNNAME", true, "--SELECT--");
            ddInDesign.SelectedValue = ddDesign.SelectedValue;
            ddOutDesign.SelectedValue = ddDesign.SelectedValue;
        }
    }
    protected void CHKFORALLCOLOR_CheckedChanged(object sender, EventArgs e)
    {
        if (CHKFORALLCOLOR.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--ALL--");
            ddInColor.SelectedIndex = 1;
            ddOutColor.SelectedIndex = 1;
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddColor, "SELECT COLORID,COLORNAME FROM COLOR Where MasterCompanyId=" + Session["varCompanyId"] + " Order By COLORNAME", true, "--SELECT--");
            ddInColor.SelectedValue = ddColor.SelectedValue;
            ddOutColor.SelectedValue = ddColor.SelectedValue;
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
            UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--ALL--");
            if (ChkForFillSame.Checked == true)
            {
                ddInSize.SelectedIndex = 1;
                ddOutSize.SelectedIndex = 1;
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
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
        if (Convert.ToInt32(Session["Type"]) != 1)
        {
            string Str = @"Select OCD.*,VF2.CATEGORY_ID,VF2.ITEM_ID,VF2.QUALITY_ID,VF2.DESIGN_ID,VF2.COLOR_ID,VF2.SHAPE_ID,VF2.SIZE_ID,VF2.SHADECOLOR_ID,VF2.PRODUCTCODE,
                         VF.CATEGORY_ID ICATEGORY_ID,VF.ITEM_ID IITEM_ID,VF.QUALITY_ID IQUALITY_ID,VF.DESIGN_ID IDESIGN_ID,VF.COLOR_ID ICOLOR_ID,
                         VF.SHAPE_ID ISHAPE_ID,VF.SIZE_ID ISIZE_ID,VF.SHADECOLOR_ID ISHADECOLOR_ID,VF.PRODUCTCODE IPRODUCTCODE,VF1.CATEGORY_ID OCATEGORY_ID,
                         VF1.ITEM_ID OITEM_ID,VF1.QUALITY_ID OQUALITY_ID,VF1.DESIGN_ID ODESIGN_ID,VF1.COLOR_ID OCOLOR_ID,VF1.SHAPE_ID OSHAPE_ID,VF1.SIZE_ID OSIZE_ID,
                         VF1.SHADECOLOR_ID OSHADECOLOR_ID,VF1.PRODUCTCODE OPRODUCTCODE From ORDER_CONSUMPTION_DETAIL OCD,VIEWFINDCIQDCSSSHID VF,VIEWFINDCIQDCSSSHID VF1,
                         VIEWFINDCIQDCSSSHID VF2 Where OCD.IFINISHEDID=VF.ITEM_FINISHED_ID AND OCD.OFINISHEDID=VF1.ITEM_FINISHED_ID AND OCD.FINISHEDID=VF2.ITEM_FINISHED_ID AND 
                         Orderid=" + DDOrderNo.SelectedValue + " And PCMDID=" + DG.SelectedValue;
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ddCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                CATEGORY_DEPENDS_CONTROLS();
                ddItemName.SelectedValue = Ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ItemNameSelectedChange();
                ddQuality.SelectedValue = Ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                ddColor.SelectedValue = Ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                ddShape.SelectedValue = Ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
                ddSize.SelectedValue = Ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                ddShade.SelectedValue = Ds.Tables[0].Rows[0]["SHADECOLOR_ID"].ToString();
                if (TxtProdCode.Visible == true)
                {
                    TxtProdCode.Text = Ds.Tables[0].Rows[0]["PRODUCTCODE"].ToString();
                }
                ddProcessName.SelectedValue = Ds.Tables[0].Rows[0]["PROCESSID"].ToString();
                ChkForProcessInPut.Checked = false;
                ProcessInputChecked();
                if (Convert.ToInt32(Ds.Tables[0].Rows[0]["PROCESSINPUTID"]) != 0)
                {
                    ChkForProcessInPut.Checked = true;
                    ProcessInputChecked();
                    ddInProcessName.SelectedValue = Ds.Tables[0].Rows[0]["PROCESSINPUTID"].ToString();
                    InProcessNameSelectedIndChange();
                }
                ddInCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["ICATEGORY_ID"].ToString();
                IN_CATEGORY_DEPENDS_CONTROLS();
                ddInItemName.SelectedValue = Ds.Tables[0].Rows[0]["IITEM_ID"].ToString();
                InItemNameSelectedIndexChange();
                ddInQuality.SelectedValue = Ds.Tables[0].Rows[0]["IQUALITY_ID"].ToString();
                ddInDesign.SelectedValue = Ds.Tables[0].Rows[0]["IDESIGN_ID"].ToString();
                ddInColor.SelectedValue = Ds.Tables[0].Rows[0]["ICOLOR_ID"].ToString();
                ddInShape.SelectedValue = Ds.Tables[0].Rows[0]["ISHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddInSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddInShape.SelectedValue + " And MasterCompanyId= " + Session["varCompanyId"] + "", true, "--SELECT--");
                ddInSize.SelectedValue = Ds.Tables[0].Rows[0]["ISIZE_ID"].ToString();
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
                OUT_CATEGORY_DEPENDS_CONTROLS();
                ddOutItemName.SelectedValue = Ds.Tables[0].Rows[0]["OITEM_ID"].ToString();
                OutItemSelectedIndexChange();
                ddOutQuality.SelectedValue = Ds.Tables[0].Rows[0]["OQUALITY_ID"].ToString();
                ddOutDesign.SelectedValue = Ds.Tables[0].Rows[0]["ODESIGN_ID"].ToString();
                ddOutColor.SelectedValue = Ds.Tables[0].Rows[0]["OCOLOR_ID"].ToString();
                ddOutShape.SelectedValue = Ds.Tables[0].Rows[0]["OSHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFill(ref ddOutSize, "SELECT SIZEID," + SizeString + " fROM SIZE WhERE SHAPEID=" + ddOutShape.SelectedValue + " And MasterCompanyId= " + Session["varCompanyId"] + "", true, "--SELECT--");
                ddOutSize.SelectedValue = Ds.Tables[0].Rows[0]["OSIZE_ID"].ToString();
                ddOutShade.SelectedValue = Ds.Tables[0].Rows[0]["OSHADECOLOR_ID"].ToString();
                ddOUnit.SelectedValue = Ds.Tables[0].Rows[0]["OUNITID"].ToString();
                if (ddFinished.Visible == true)
                {
                    ddFinished.SelectedValue = Ds.Tables[0].Rows[0]["O_FINISHED_TYPE_ID"].ToString();
                    TxtOutProdCode.Text = Ds.Tables[0].Rows[0]["OPRODUCTCODE"].ToString();
                }
                DDICALTYPE.SelectedValue = Ds.Tables[0].Rows[0]["OCALTYPE"].ToString();
                TxtOutPutQty.Text = Ds.Tables[0].Rows[0]["OQTY"].ToString();
                TxtOutPutRate.Text = Ds.Tables[0].Rows[0]["ORATE"].ToString();
                BtnSave.Text = "UpDate";
            }
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        Session["Type"] = 1;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=1 And ProductCode Like '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
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
        string strQuery = "SELECT ProductCode from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,ITEM_CATEGORY_MASTER ICM,CategorySeparate CS Where IPM.Item_Id=IM.Item_Id AND IM.Category_Id=ICM.Category_Id And ICM.Category_Id=CS.CategoryId And Id=0 And ProductCode Like '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
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
    protected void TxtOutPutQty_TextChanged(object sender, EventArgs e)
    {
        //int VarCompanyNo = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarCompanyNo From MasterSetting"));
        if (Session["varCompanyId"] == "4" && INPROCESSNAME.Visible == false)
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
            if (Convert.ToDouble(TxtOutPutQty.Text) > Convert.ToDouble(LblQty.Text))
            {
                TxtOutPutQty.Text = "";
                TxtOutPutQty.Focus();
                lblMessage.Visible = true;
                lblMessage.Text = "Rec Qty Cann't Be Greater Than Pending Qty";
            }
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
        FILLGRID();
    }
    protected void ChkForFillSame_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForFillSame.Checked == true)
        {
            ddCategoryName.SelectedIndex = 0;
            CATEGORY_DEPENDS_CONTROLS();
        }
    }

    protected void DGInPutProcess_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "DELETE ORDER_CONSUMPTION_DETAIL Where Orderid=" + DDOrderNo.SelectedValue + " And PCMDID=" + Convert.ToInt32(DG.DataKeys[e.RowIndex].Value) + "");
            FILLGRID();
        }
        catch (Exception)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Error In Deleting";
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnNew_Click1(object sender, EventArgs e)
    {
        Response.Redirect("FrmOrderWiseConsumption.aspx");
    }
    protected void TxtLoss_TextChanged(object sender, EventArgs e)
    {
        TxtLoss.Text = TxtRecLoss.Text;
    }
}