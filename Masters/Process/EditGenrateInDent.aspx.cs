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

public partial class Masters_Process_EditGenrateInDent : System.Web.UI.Page
{
    static int ReProcessType = 0;
    static int ChkReDeyingStatus = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["othershadefinishedid"] = "0";
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname", true, "--SelectCompany");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                CompanyNameSelectedChange();
            }
            UtilityModule.ConditionalComboFill(ref DDBranchName, @"Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"], false, "");

            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,Customercode+ SPACE(5)+CompanyName from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + " order by Customercode", true, "--SELECT--");

            ParameteLabel();
            CommanFunction.FillCombo(DDcaltype, "select CalID,CalType from Process_CalType");
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            switch (VarProdCode)
            {
                case 0:
                    q3.Visible = false;
                    break;
                case 1:
                    q3.Visible = true;
                    break;
            }
            //hnorderid.Value = "0";
            hncomp.Value = Session["varCompanyId"].ToString();
            //WithOut BOM
            if (Session["WithoutBOM"].ToString() == "1" || ChkForOrder.Checked == true)
            {
                TDChkForOrder.Visible = true;
                // TDLblReqDate.Visible = true;
                //tdrate.Visible = false;
                ChkForOrder.Checked = true;
                ChkEditOrder_CheckedChanged(sender, e);
                TxtDate.Enabled = false;
            }
            //
            switch (Convert.ToInt16(Session["varCompanyId"]))
            {
                case 3:
                    TDChkForOrder.Visible = true;
                    TDLblReqDate.Visible = true;
                    tdrate.Visible = false;
                    ChkForOrder.Checked = true;
                    ChkEditOrder_CheckedChanged(sender, e);
                    TxtDate.Enabled = false;
                    break;
                case 4:
                    TDCaltype.Visible = false;
                    break;
                case 6:
                    TDChkForOrder.Visible = true;
                    TxtRate.Enabled = true;
                    break;
                case 7:
                    TDChkForOrder.Visible = true;
                    TDLblReqDate.Visible = true;
                    tdrate.Visible = false;
                    ChkForOrder.Checked = true;
                    ChkEditOrder_CheckedChanged(sender, e);
                    TxtDate.Enabled = false;
                    break;
                case 10:
                    TDChkForOrder.Visible = true;
                    TDLblReqDate.Visible = true;
                    tdrate.Visible = false;
                    ChkForOrder.Checked = true;
                    ChkEditOrder_CheckedChanged(sender, e);
                    TxtDate.Enabled = false;
                    TDLotNo.Visible = false;
                    break;
                case 12:
                    TDLotNo.Visible = false;
                    TDStockQty.Visible = false;
                    TxtRate.Enabled = true;
                    break;
                case 14:
                    TDTagNo.Visible = true;
                    break;
                case 16:
                    TxtLoss.Enabled = false;
                    txtextraQty.Enabled = false;
                    ChkForWithoutRate.Visible = true;
                    BtnEmployeeWisePPDetail.Visible = true;
                    BtnRateUpdate.Visible = true;
                    break;
                case 21:
                    TxtDate.Enabled = false;
                    break;
                case 28:
                    txtextraQty.Enabled = false;
                    break;
                case 42:
                    TDGodownName.Visible = true;
                    if (Session["usertype"].ToString() != "1")
                    {
                        TDBtnAddRate.Visible = false;
                    }
                    break;
                case 43:
                    //TDGodownName.Visible = true;
                    TDDDTagNo.Visible = true;
                    break;
                default:
                    if (ChkForOrder.Checked == true)
                    {
                        DDCategory.Enabled = false;
                        DDItem.Enabled = false;
                        DDQuality.Enabled = false;
                        DDDesign.Enabled = false;
                        DDColor.Enabled = false;
                        DDShape.Enabled = false;
                        DDColorShade.Enabled = false;
                        DDSize.Enabled = false;
                        ddUnit.Enabled = false;
                    }
                    break;
            }
            if (MySession.TagNowise == "1")
            {
                if (Convert.ToInt16(Session["varCompanyId"]) == 43)
                {
                    TDTagNo.Visible = false;
                }
                else
                {
                    TDTagNo.Visible = true;
                }
            }
            if (variable.VarGENERATEINDENTTAGNOAUTOGENERATED == "1")
            {
                txtTagno.Enabled = false;
            }
            if (Convert.ToInt16(Session["varCompanyId"]) == 6 || Convert.ToInt16(Session["varCompanyId"]) == 7 || Convert.ToInt16(Session["varCompanyId"]) == 3 || Convert.ToInt16(Session["varCompanyId"]) == 10 || Session["WithoutBOM"].ToString() == "1")
            {
                UtilityModule.ConditionalComboFill(ref DDProcessName, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=0 And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Process--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDProcessName, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=0 And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Process--");
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
            }
            //show edit button
            if (Session["canedit"].ToString() == "1") //non authenticated person
            {
                Tdcomplete.Visible = true;
            }
            if (variable.VarGENRATEINDENTWITHOUTLOT_STOCK == "1")
            {
                TDLotNo.Visible = false;
                TDTagNo.Visible = false;
                TDStockQty.Visible = false;
            }
            //

            if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
            {
                BtnSave.Visible = false;
                DGIndentDetail.Columns[11].Visible = false;
                DGIndentDetail.Columns[12].Visible = false;

            }
        }

    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedChange();
    }
    private void CompanyNameSelectedChange()
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select Distinct EI.EmpId,EmpName from IndentMaster IM inner join EmpInfo EI ON IM.PartyId=EI.EmpId inner join EmpProcess EP on EI.EmpId=EP.EmpId Where IM.Companyid=" + DDCompanyName.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + " order by empname", true, "--Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str, Status = "";
        if (chkcomplete.Checked == true)
        {
            Status = "Complete";
        }
        else
        {
            Status = "Pending";
        }
        if (Session["WithoutBOM"].ToString() == "1" || ChkForOrder.Checked == true)
        {

            str = @"select Distinct IM.IndentID,IM.IndentNo 
                  from IndentMaster IM inner join IndentDetail ID on IM.IndentID=ID.IndentId 
                  where IM.Status<>'Cancelled' and Im.Status='" + Status + "' and IM.MasterCompanyId=" + Session["varcompanyId"] + @"
                  And id.ORDERID=" + DDOrderNo.SelectedValue + " and IM.PartyId=" + DDPartyName.SelectedValue + " and Im.ProcessID=" + DDProcessName.SelectedValue + "";

            UtilityModule.ConditionalComboFill(ref DDIndentNo, str, true, "--Select--");
        }

        else
        {
            ProcessNameSelectedChanged();
        }
        // PartyNameSelectedChanged();
    }
    private void PartyNameSelectedChanged()
    {
        ProcessNameSelectedChanged();

    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DDProcessName.SelectedItem.Text.ToUpper())
        {
            case "DYEING":
                TDReDyeing.Visible = true;
                break;
            default:
                TDReDyeing.Visible = false;
                break;
        }
        UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + DDProcessName.SelectedValue + " AND EI.MasterCompanyId=" + Session["varCompanyId"] + " order by ei.empname", true, "--Select--");

    }
    private void ProcessNameSelectedChanged()
    {
        string str = @"select Distinct ID.PPNo,
                        case When " + Session["varcompanyId"] + @"=9 Then (select strcarpets From [Get_ProcessLocalOrderNo](PP.PPID)) + ' # '+cast(PP.ChallanNo as varchar(50)) Else PP.ChallanNo end 
                        From IndentMaster IM 
                        INNER JOIN IndentDetail ID ON IM.IndentId=Id.IndentId 
                        INNER JOIN ProcessProgram PP ON ID.PPNO=PP.PPID 
                        INNER JOIN OrderMaster OM ON PP.Order_id=OM.OrderId
                        Where IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.ProcessId=" + DDProcessName.SelectedValue + " And IM.PartyID=" + DDPartyName.SelectedValue + " ";

        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, str, true, "--Select--");

        ////        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, 
        ////            @"select Distinct PPNo,PP.ChallanNo from IndentMaster IM INNER JOIN IndentDetail ID ON IM.IndentId=Id.IndentId
        ////            INNER JOIN ProcessProgram PP ON ID.PPNO=PP.PPID 
        ////            where IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.ProcessId=" + DDProcessName.SelectedValue + " And IM.PartyID=" + DDPartyName.SelectedValue, true, "--Select--");
    }
    protected void DDProcessProgramNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessProgramNoSelectedChange();
    }
    private void ProcessProgramNoSelectedChange()
    {
        string str = @"Select Distinct IM.IndentId,IndentNo from IndentMaster IM inner join IndentDetail ID on IM.IndentId=ID.IndentId Where IM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.Status <> 'Cancelled' And ID.PPNo=" + DDProcessProgramNo.SelectedValue + @" And 
                     IM.MasterCompanyId=" + Session["varCompanyId"] + " and IM.Partyid=" + DDPartyName.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + "  and Im.status='Complete'";
        }
        else
        {
            str = str + "  and Im.status='Pending'";
        }
        UtilityModule.ConditionalComboFill(ref DDIndentNo, str, true, "--Select--");
    }
    protected void DDIndentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        IndentNoSelectedChange();
    }
    private void IndentNoSelectedChange()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Replace(convert(nvarchar(11),Date,106),' ',' -') Date,Replace(convert(nvarchar(11),ReqDate,106),' ',' -') ReqDate,isnull(Gremark,'') as Gremark From IndentMaster Where IndentId=" + DDIndentNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtDate.Text = Ds.Tables[0].Rows[0]["Date"].ToString();
            TxtReqDate.Text = Ds.Tables[0].Rows[0]["ReqDate"].ToString();
            txtremarks.Text = Ds.Tables[0].Rows[0]["Gremark"].ToString();
            Fill_Grid();
        }
        string strcategory;
        if (Session["WithoutBOM"].ToString() == "1" || ChkForOrder.Checked == true)
        {
            strcategory = @"select distinct vf.CATEGORY_ID,vf.CATEGORY_NAME 
                        from IndentDetail Id inner join V_FinishedItemDetail Vf on Id.OFinishedId=vf.ITEM_FINISHED_ID
                        Where ID.IndentId=" + DDIndentNo.SelectedValue + @"
                        order by vf.CATEGORY_NAME";
        }
        else
        {
            strcategory = @"Select Distinct Category_Id,Category_Name From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
        VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }
        UtilityModule.ConditionalComboFill(ref DDCategory, strcategory, true, "--Select Category--");
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
        string stritem;

        if (ChkForOrder.Checked == true)
        {
            stritem = "select distinct IM.Item_Id,IM.Item_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Item_Category_Master ICM on ICM.Category_Id=IM.Category_Id where  IM.Category_Id=" + DDCategory.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            UtilityModule.ConditionalComboFill(ref DDItem, stritem, true, "---Select Item----");
        }
        else
        {
            stritem = @"Select Distinct ITEM_ID,ITEM_NAME From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And CATEGORY_ID=" + DDCategory.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
        }

        UtilityModule.ConditionalComboFill(ref DDItem, stritem, true, "---Select Item----");
        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + DDCategory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
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
                    case "10":
                        TdColor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void DDItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddUnit, "select Distinct UnitId,unitName From  Unit U,Item_Master IM where U.UnitTypeId=IM.UnitTypeId And ITEM_ID=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"], true, " Select ");
        if (ddUnit.SelectedIndex > 0)
        {
            ddUnit.SelectedIndex = 1;
        }
        if (TdQuality.Visible == true)
        {
            string strquality;
            if (ChkForOrder.Checked == true)
            {
                strquality = "select distinct Q.QualityId,QualityName from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Quality Q on Q.QualityId=IPM.Quality_Id where  IM.Item_Id=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strquality = @"Select Distinct QualityId,QualityName From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            UtilityModule.ConditionalComboFill(ref DDQuality, strquality, true, "--Select Quality--");
        }
        ComboFill();
    }
    protected void ComboFill()
    {
        if (TdDesign.Visible == true)
        {
            string strDesign;
            if (ChkForOrder.Checked == true)
            {
                strDesign = "select distinct D.DesignId,DesignName from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Design D on D.DesignId=IPM.Design_Id where  IM.Item_Id=" + DDItem.SelectedValue + "  And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strDesign = @"Select Distinct designId,designName From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            UtilityModule.ConditionalComboFill(ref DDDesign, strDesign, true, "--Select Design--");
        }
        if (TdColor.Visible == true)
        {
            string strColor;
            if (ChkForOrder.Checked == true)
            {
                strColor = "select distinct C.ColorId,ColorName from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Color C on C.ColorId=IPM.Color_Id where IM.Item_Id=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strColor = @"Select Distinct ColorId,ColorName From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            UtilityModule.ConditionalComboFill(ref DDColor, strColor, true, "--Select Color--");
        }
        if (TdColorShade.Visible == true)
        {
            string strShadeColor;
            if (ChkForOrder.Checked == true)
            {
                strShadeColor = "select distinct SC.ShadeColorId,ShadeColorName from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join ShadeColor SC on SC.ShadeColorId=IPM.ShadeColor_Id where  IM.Item_Id=" + DDItem.SelectedValue + "  And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strShadeColor = @"Select Distinct ShadecolorId,ShadeColorName From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
                if (TdQuality.Visible == true && DDQuality.SelectedIndex > 0)
                {
                    strShadeColor = strShadeColor + " and vf.qualityid=" + DDQuality.SelectedValue;
                }
            }
            UtilityModule.ConditionalComboFill(ref DDColorShade, strShadeColor, true, "--Select ShadeColor--");
        }
        if (TdShape.Visible == true)
        {
            string strShape;
            if (ChkForOrder.Checked == true)
            {
                strShape = "select distinct SH.ShapeId,ShapeName from  Item_Parameter_Master IPM inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Shape SH on SH.ShapeId=IPM.Shape_Id where  IM.Item_Id=" + DDItem.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strShape = @"Select Distinct ShapeId,ShapeName From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }
            UtilityModule.ConditionalComboFill(ref DDShape, strShape, true, "--Select Shape--");
        }
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ChkForOrder.Checked == false)
        {
            ComboFill();
            Fill_Quantity();
        }

    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForOrder.Checked == false)
        {
            Fill_Quantity();
        }
    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForOrder.Checked == false)
        {
            Fill_Quantity();
        }
    }
    protected void FillIssueShade()
    {
        string str = "";
        if (Session["varcompanyNo"].ToString() == "9")
        {
            str = @"Select Distinct VF1.ShadeColorID, VF1.ShadeColorName 
                    From PP_Consumption a
                    JOIN V_FINISHEDITEMDETAIL VF ON VF.ITEM_FINISHED_ID = a.FinishedID And ITEM_ID=" + DDItem.SelectedValue + " and QualityId=" + DDQuality.SelectedValue + @" 
                                And VF.ShadeColorID = " + DDColorShade.SelectedValue + @" 
                    JOIN V_FINISHEDITEMDETAIL VF1 ON VF1.ITEM_FINISHED_ID = a.IFinishedID 
                    Where PPID = " + DDProcessProgramNo.SelectedValue + " Order By VF1.ShadeColorName";
        }
        else
        {
            str = "select Distinct Shadecolorid,ShadeColorName From V_FinishedItemdetail Where ITEM_ID=" + DDItem.SelectedValue + " and QualityId=" + DDQuality.SelectedValue + "";
            if (chkredyeing.Checked == true)
            {
                str = str + " and  ShadecolorId<>" + DDColorShade.SelectedValue;
            }
            else
            {
                str = str + " and ShadeColorName like 'Undyed%'";
            }
        }
        UtilityModule.ConditionalComboFill(ref DDISSUESHADE, str, true, "--Plz select--");

        // string str = "select Distinct Shadecolorid,ShadeColorName From V_FinishedItemdetail Where ITEM_ID=" + DDItem.SelectedValue + " and QualityId=" + DDQuality.SelectedValue + " and ShadeColorName like 'Undyed%'";
        // UtilityModule.ConditionalComboFill(ref DDISSUESHADE, str, true, "--Plz select--");
    }
    protected void DDColorShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForOrder.Checked == false)
        {
            //*****************Fill Issue Shade
            if (variable.Carpetcompany == "1" && DDProcessName.SelectedItem.Text.ToUpper() == "DYEING")
            {
                if (variable.VarDyeingIssueOthershade == "1" || chkredyeing.Checked == true)
                {
                    TDISSUESHADE.Visible = true;
                    //Auto select issue shade Id
                    string Shadecolorid = "0";
                    string str = @"select Top(1) vf.ShadecolorId From PP_Consumption PC inner Join v_finisheditemdetail vf on PC.IFinishedid=vf.ITEM_FINISHED_ID inner join V_finisheditemdetail vf1 on PC.Finishedid=vf1.ITEM_FINISHED_ID
                            Where PPId=" + DDProcessProgramNo.SelectedValue + " and Vf1.Item_id=" + DDItem.SelectedValue + " and Vf1.qualityid=" + DDQuality.SelectedValue + " and vf1.ShadecolorId=" + DDColorShade.SelectedValue;
                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Shadecolorid = Convert.ToString(ds.Tables[0].Rows[0]["Shadecolorid"]);
                    }

                    //**********
                    FillIssueShade();
                    if (DDISSUESHADE.Items.FindByValue(Shadecolorid) != null)
                    {
                        DDISSUESHADE.SelectedValue = Shadecolorid;
                        DDISSUESHADE_SelectedIndexChanged(sender, new EventArgs());
                    }

                }
                else
                {
                    TDISSUESHADE.Visible = false;
                }
            }

            //****************
            Fill_Quantity();
            if (Session["VarCompanyId"].ToString() == "16")
            {
                if (TxtLoss.Text == "0" || TxtLoss.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Javascript", "javascript:confirmSubmit(); ", true);
                }
            }
            DyeingTypeTrueorfalse();
            switch (Session["varcompanyid"].ToString())
            {
                case "4":
                    GetdyingTypeswithRates();
                    break;
                default:
                    break;
            }
        }
    }
    protected void DyeingTypeTrueorfalse()
    {
        switch (Session["varcompanyid"].ToString())
        {
            case "4":
                //TRdyingTypes.Visible = true;
                TDDyeingMatch.Visible = true;
                TDDyingType.Visible = true;
                TDDyeing.Visible = true;
                break;
            case "16":
                // TRdyingTypes.Visible = true;
                TDDyingType.Visible = false;
                TDDyeing.Visible = false;
                TDDyeingMatch.Visible = true;
                DDDyeingMatch.Enabled = true;
                break;
            default:
                break;
        }
    }
    protected void GetdyingTypeswithRates()
    {
        string str;
        str = @"select DyingMatch,DyingType,Dyeing,ORate from PP_Consumption PC inner join ORDER_CONSUMPTION_DETAIL OCD
                on PC.OrderId=OCD.ORDERID and PC.OrderDetailId=OCD.ORDERDETAILID 
                and pc.FinishedId=ocd.OFINISHEDID  
                inner join ITEM_PARAMETER_MASTER IPM on IPM.ITEM_FINISHED_ID=PC.FinishedId
                where PC.PPId=" + DDProcessProgramNo.SelectedValue + " and ipm.ITEM_ID=" + DDItem.SelectedValue + " and IPM.SHADECOLOR_ID=" + DDColorShade.SelectedValue + "";
        if (TdQuality.Visible == true)
        {
            str = str + " and IPM.QUALITY_ID= " + DDQuality.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DDDyeing.SelectedItem.Text = ds.Tables[0].Rows[0]["Dyeing"].ToString();
            DDDyeingMatch.SelectedItem.Text = ds.Tables[0].Rows[0]["DyingMatch"].ToString(); ;
            DDDyingType.SelectedItem.Text = ds.Tables[0].Rows[0]["DyingType"].ToString();
            TxtRate.Text = ds.Tables[0].Rows[0]["ORate"].ToString();
        }
        else
        {
            DDDyeing.SelectedItem.Text = "";
            DDDyeingMatch.SelectedItem.Text = "";
            DDDyingType.SelectedItem.Text = "";
            TxtRate.Text = "";
        }
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkForOrder.Checked == false)
        {

            Fill_Quantity();
        }
    }
    private void Fill_Quantity()
    {
        int color = 0;
        int quality = 0;
        int design = 0;
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
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"])));
                ViewState["finishedid"] = finishedid;
                if (finishedid > 0)
                {
                    SqlParameter[] _arrpara = new SqlParameter[6];
                    _arrpara[0] = new SqlParameter("@FinishedId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@PPID", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
                    _arrpara[3] = new SqlParameter("@TotalQty", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@PreQty", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@Loss", SqlDbType.Float);
                    // _arrpara[6] = new SqlParameter("@TotalAssignedQTY", SqlDbType.Float);

                    _arrpara[0].Value = finishedid;
                    _arrpara[1].Value = DDProcessProgramNo.SelectedValue;
                    _arrpara[2].Value = DDProcessName.SelectedValue;
                    _arrpara[3].Direction = ParameterDirection.Output;
                    _arrpara[4].Direction = ParameterDirection.Output;
                    _arrpara[5].Direction = ParameterDirection.Output;
                    // _arrpara[6].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Get_TotalConsmp_Indent_LossQty", _arrpara);

                    txtTotalQty.Text = _arrpara[3].Value.ToString();
                    TxtPreQty.Text = _arrpara[4].Value.ToString();
                    TxtLoss.Text = _arrpara[5].Value.ToString();
                    // ViewState["TotalAssignedQty"] = _arrpara[6].Value.ToString();
                    Tran.Commit();
                    TxtFinishedid.Text = "a1=" + DDCompanyName.SelectedValue + "&a2=" + DDPartyName.SelectedValue + "&a3=" + finishedid + "&a4=" + DDcaltype.SelectedValue;
                    UtilityModule.ConditionalComboFill(ref ddUnit, "SELECT DISTINCT U.UNITID,U.UNITNAME FROM ORDER_CONSUMPTION_DETAIL OCD,V_FINISHEDITEMDETAIL VF,UNIT U WHERE OCD.OFINISHEDID=VF.ITEM_FINISHED_ID AND U.UNITID=OCD.OUNITID AND OCD.PROCESSID=" + DDProcessName.SelectedValue + " AND ORDERID in (Select Order_id from ProcessProgram Where PPID=" + DDProcessProgramNo.SelectedValue + ") AND VF.ITEM_ID=" + DDItem.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, " Select ");
                    if (ddUnit.Items.Count > 0)
                    {
                        ddUnit.SelectedIndex = 1;
                    }

                    if (Convert.ToInt16(Session["varCompanyId"]) == 42)
                    {
                        FillGodownName();
                    }
                    else
                    {
                        FillLotNo();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            TxtPreQty.Text = "";
            txtTotalQty.Text = "";
        }
    }
    private void fillsize()
    {
        if (TdSize.Visible == true)
        {
            string strSize;
            if (DDsizetype.SelectedIndex == 0)
            {
                strSize = " Sizeft";
            }
            else if (DDsizetype.SelectedIndex == 1)
            {
                strSize = "Sizemtr";
            }
            else
            {
                strSize = "Sizeinch";
            }
            if (ChkForOrder.Checked == true)
            {
                strSize = "select distinct SZ.SizeID," + strSize + " from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id inner join Size SZ on SZ.SizeId=IPM.Size_Id where  IM.Item_Id=" + DDItem.SelectedValue + " and SZ.ShapeId=" + DDShape.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                strSize = @"Select Distinct SizeId," + strSize + @" From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And ShapeId=" + DDShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            }

            //            else
            //            {
            //                strSize = @"Select Distinct SizeId,SizeMtr From PP_Consumption OCD,ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And 
            //                           VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ITEM_ID=" + DDItem.SelectedValue + " And ShapeId=" + DDShape.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            //            }
            UtilityModule.ConditionalComboFill(ref DDSize, strSize, true, "--Select Size--");
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "16")
        {
            if (hnRetunTypeValue.Value == "0")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please fill loss percentage";
                return;
            }
        }
        double Qty = Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text);
        CHECKVALIDCONTROL();

        if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
        {
            string ChkMsg = CheckStockQty();
            if (ChkMsg == "G")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Qty should not be greater than assigned stock";
                return;
            }
        }
        if (ChkForOrder.Checked == false)
        {
            check_qty();
        }
        if (lblMessage.Text != "")
        {
            return;
        }
        if (Qty == 0.0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Qty Cann't be Zero";
        }
        CheckReDyeingProcessType();
        if (lblMessage.Visible == false && lblMessage.Text == "")
        {
            Save_indent();
        }
        if (Convert.ToInt32(Session["varCompanyId"]) == 16)
        {
            TxtRate.Enabled = false;
        }
    }
    private void CheckReDyeingProcessType()
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ChkReDeyingStatus = Convert.ToInt32(chkredyeing.Checked == true ? "1" : "0");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Indentid,Re_Process from indentdetail where IndentId=" + DDIndentNo.SelectedValue + " order by Indentdetailid ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            ReProcessType = Convert.ToInt32(Ds.Tables[0].Rows[0]["Re_Process"].ToString());

            if (ChkReDeyingStatus != ReProcessType)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please generate another indent for dyeing status change";
                return;
            }
        }
    }
    private void Save_indent()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[40];
            if (BtnSave.Text != "Update")
            {
                Session["IndentDetailId"] = 0;
            }
            _arrpara[0] = new SqlParameter("@IndentId", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@IndentDetailId", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@PPNo", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@IFinishedId", SqlDbType.Int);
            _arrpara[4] = new SqlParameter("@OFinishedId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@Quantity", SqlDbType.Float, 50);
            _arrpara[6] = new SqlParameter("@Rate", SqlDbType.Float);
            _arrpara[7] = new SqlParameter("@DyingType", SqlDbType.Int);

            _arrpara[8] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@PartyId", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@ProcessID", SqlDbType.Int);
            _arrpara[11] = new SqlParameter("@Date", SqlDbType.DateTime);
            _arrpara[12] = new SqlParameter("@IndentNo", SqlDbType.NVarChar, 50);
            _arrpara[13] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrpara[14] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpara[15] = new SqlParameter("@Id", SqlDbType.Int);
            _arrpara[16] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            _arrpara[17] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[18] = new SqlParameter("@O_FINISHED_TYPE_ID", SqlDbType.Int);
            _arrpara[19] = new SqlParameter("@UNITID", SqlDbType.Int);
            _arrpara[20] = new SqlParameter("@ReqDate", SqlDbType.DateTime);
            _arrpara[21] = new SqlParameter("@Loss", SqlDbType.Float);
            _arrpara[22] = new SqlParameter("@OrderWiseFlag", SqlDbType.Int);
            _arrpara[23] = new SqlParameter("@orderdetailid", SqlDbType.Int);
            _arrpara[24] = new SqlParameter("@Remark", SqlDbType.NVarChar, 250);
            _arrpara[25] = new SqlParameter("@ItemRemark", SqlDbType.NVarChar, 250);
            _arrpara[26] = new SqlParameter("@Sizeflag", SqlDbType.Int);
            _arrpara[27] = new SqlParameter("@ExtraQty", SqlDbType.Float);
            _arrpara[28] = new SqlParameter("@CancelQty", SqlDbType.Float);
            _arrpara[29] = new SqlParameter("@editflag", SqlDbType.TinyInt);
            _arrpara[30] = new SqlParameter("@DyingMatch", SqlDbType.VarChar, 20);
            _arrpara[31] = new SqlParameter("@DyeingType", SqlDbType.VarChar, 20);
            _arrpara[32] = new SqlParameter("@Dyeing", SqlDbType.VarChar, 20);
            _arrpara[33] = new SqlParameter("@TagNo", SqlDbType.VarChar, 50);
            _arrpara[34] = new SqlParameter("@Othershadefinishedid", SqlDbType.Int);
            _arrpara[35] = new SqlParameter("@Re_Process", SqlDbType.Int);
            _arrpara[36] = new SqlParameter("@OldTagNowise", SqlDbType.Int);
            _arrpara[37] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            _arrpara[38] = new SqlParameter("@BranchID", SqlDbType.Int);
            _arrpara[39] = new SqlParameter("@GodownID", SqlDbType.Int);

            _arrpara[0].Value = DDIndentNo.SelectedValue;
            _arrpara[1].Value = Session["IndentDetailId"];

            _arrpara[4].Value = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"]));
            if (ChkForOrder.Checked == true)
            {
                _arrpara[2].Value = 0;
                _arrpara[3].Value = 0;
                _arrpara[16].Value = "Without Lot No";
                _arrpara[17].Value = DDOrderNo.SelectedValue;
                _arrpara[21].Value = 0;
                _arrpara[22].Value = 1;// For OrderWise GenerateIndent
                _arrpara[7].Value = 1;
            }
            else
            {
                _arrpara[2].Value = DDProcessProgramNo.SelectedValue;
                _arrpara[3].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IFinishedId from Order_Consumption_Detail OCD inner join ProcessProgram PP on OCD.OrderId=PP.Order_Id  where PP.PPId=" + DDProcessProgramNo.SelectedValue + " and OCD.OFinishedId=" + _arrpara[4].Value + " And PP.MasterCompanyId=" + Session["varCompanyId"] + "");
                _arrpara[16].Value = (TDLotNo.Visible == true ? ddllotno.SelectedItem.Text : "Without Lot No");
                int orderId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Order_Id From ProcessProgram where PPID=" + DDProcessProgramNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
                _arrpara[17].Value = orderId;
                _arrpara[21].Value = Convert.ToDouble(TxtLoss.Text);
                _arrpara[22].Value = 0;
                _arrpara[7].Value = DDcaltype.SelectedValue;
            }
            _arrpara[5].Value = txtQty.Text;
            _arrpara[6].Value = TxtRate.Text == "" ? "0" : TxtRate.Text;

            _arrpara[8].Value = DDCompanyName.SelectedValue;
            _arrpara[9].Value = DDPartyName.SelectedValue;
            _arrpara[10].Value = DDProcessName.SelectedValue;
            _arrpara[11].Value = TxtDate.Text;
            _arrpara[12].Direction = ParameterDirection.InputOutput;
            _arrpara[12].Value = DDIndentNo.SelectedItem.Text.ToUpper();
            _arrpara[13].Value = Session["varCompanyId"];
            _arrpara[14].Value = Session["varuserid"];
            _arrpara[15].Direction = ParameterDirection.Output;
            _arrpara[18].Value = 0;
            _arrpara[19].Value = ddUnit.SelectedValue;
            _arrpara[20].Value = TxtReqDate.Text;
            if (ChkForOrder.Checked == true)
            {
                _arrpara[23].Value = hnorderid.Value;
            }
            else
            {
                _arrpara[23].Value = 0;
            }

            _arrpara[24].Value = txtremarks.Text;
            _arrpara[25].Value = txtitemremark.Text;
            _arrpara[26].Value = DDsizetype.Visible == true ? DDsizetype.SelectedValue : "0";
            _arrpara[27].Value = txtextraQty.Text == "" ? "0" : txtextraQty.Text;
            _arrpara[28].Value = txtCanQty.Text == "" ? "0" : txtCanQty.Text;
            _arrpara[29].Value = 1; // Edit flag
            _arrpara[30].Value = TDDyeingMatch.Visible == true ? DDDyeingMatch.SelectedItem.Text : "";
            _arrpara[31].Value = TDDyingType.Visible == true ? DDDyingType.SelectedItem.Text : "";
            _arrpara[32].Value = TDDyeing.Visible == true ? DDDyeing.SelectedItem.Text : "";
            
            if (TDDDTagNo.Visible == true)
            {
                _arrpara[33].Value = DDTagNo.SelectedItem.Text;
            }
            else
            {
                _arrpara[33].Value = txtTagno.Text == "" ? "Without Tag No" : txtTagno.Text;
            }

            _arrpara[34].Value = TDISSUESHADE.Visible == true ? ViewState["othershadefinishedid"] : "0";
            _arrpara[35].Value = chkredyeing.Checked == true ? "1" : "0";
            _arrpara[36].Value = "0";
            _arrpara[37].Direction = ParameterDirection.Output;
            _arrpara[38].Value = DDBranchName.SelectedValue;
            _arrpara[39].Value = TDGodownName.Visible == true ? DDGodownName.SelectedValue : "0";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndent", _arrpara);
            if (_arrpara[37].Value.ToString() != "")
            {
                lblMessage.Visible = true;
                lblMessage.Text = _arrpara[37].Value.ToString();
            }
            Tran.Commit();
            Fill_Grid();
            SaveRefresh();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/GenrateInDent.aspx");
            lblMessage.Visible = true;
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessage.Text = "";
        lblMessage.Visible = false;
        if (UtilityModule.VALIDDROPDOWNLIST(DDCategory) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItem) == false)
        {
            goto a;

        }
        if (TdQuality.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
        }

        if (TdDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
            {
                goto a;
            }
        }
        if (TdColor.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
            {
                goto a;
            }
        }
        if (TdColorShade.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDColorShade) == false)
            {
                goto a;
            }
        }
        if (TDISSUESHADE.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDISSUESHADE) == false)
            {
                goto a;
            }
        }
        if (TdShape.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDShape) == false)
            {
                goto a;
            }
        }
        if (TdSize.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddUnit) == false)
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
    private void Fill_Grid()
    {
        BtnPreview.Enabled = true;
        DGIndentDetail.DataSource = GetDetail();
        DGIndentDetail.DataBind();
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
                Session["ReportPath"] = "Reports/GenrateIndentApproval.rpt";
                break;
            case "4":
                Session["ReportPath"] = "Reports/GenrateIndentDeepak.rpt";
                break;
            case "14":
                Session["ReportPath"] = "Reports/GenrateIndentEMIKEA.rpt";
                break;
            case "27":
                Session["ReportPath"] = "Reports/GenrateIndentAntique.rpt";
                break;
            case "21":
                if (Session["UserType"].ToString() == "1")
                {
                    Session["ReportPath"] = "Reports/GenrateIndentKaysons.rpt";
                }
                else
                {
                    Session["ReportPath"] = "Reports/GenrateIndentWithoutRateKaysons.rpt";
                }

                break;
            case "44":
                Session["ReportPath"] = "Reports/GenrateIndentAgni.rpt";
                break;
            default:
                Session["ReportPath"] = "Reports/GenrateIndent.rpt";
                break;
        }

        Session["CommanFormula"] = "{GenrateIndentReport.IndentID}=" + DDIndentNo.SelectedValue;
    }
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";
        if (ChkForOrder.Checked == false)
        {

            sqlstr = @"Select IND.IndentDetailId,PPNo,IndentNo,IndentQty Quantity,Rate,VF1.CATEGORY_NAME+space(3)+VF1.ITEM_NAME+space(3)+VF1.QualityName+ Space(3)+VF1.designName+ 
                      Space(3)+VF1.ColorName+ Space(3)+VF1.ShadeColorName+ Space(3)+VF1.ShapeName+ Space(3)+VF1.SizeMtr InDescription,VF.CATEGORY_NAME+space(3)+VF.ITEM_NAME+space(3)+
                      VF.QualityName+ Space(3)+VF.designName+ Space(3)+VF.ColorName+ Space(3)+VF.ShadeColorName+ Space(3)+VF.ShapeName+ Space(3)+case when IND.flagsize=1 Then VF.SizeMtr Else case When IND.flagSize=0 Then vf.SizeFt else vf.Sizeinch End ENd OutDescription,ExtraQty,CancelQty,IND.LotNo,IND.TagNo
                      From IndentMaster INM
                      inner join IndentDetail IND on INM.indentid=IND.IndentId
                      inner join V_FinishedItemDetail VF on vf.ITEM_FINISHED_ID=ind.OFinishedId
                      left join V_FinishedItemDetail VF1 on vf1.ITEM_FINISHED_ID=ind.IFinishedId
                      Where IND.IndentId=" + DDIndentNo.SelectedValue + " And INM.MasterCompanyId=" + Session["varCompanyId"] + " And INM.CompanyId=" + DDCompanyName.SelectedValue + " order by IndentDetailId";

        }
        else
        {

            sqlstr = @"Select IND.IndentDetailId,PPNo,IndentNo,IndentQty Quantity,Rate,'' as InDescription,VF.CATEGORY_NAME+space(3)+VF.ITEM_NAME+space(3)+
        VF.QualityName+ Space(3)+VF.designName+ Space(3)+VF.ColorName+ Space(3)+VF.ShadeColorName+ Space(3)+VF.ShapeName+ Space(3)+VF.SizeMtr OutDescription,ExtraQty,CancelQty,IND.LotNo,IND.TagNo
        From IndentMaster INM,IndentDetail IND,V_FinishedItemDetail VF
        Where IND.IndentId=INM.IndentId And IND.OFinishedId=VF.ITEM_FINISHED_ID And IND.IndentId=" + DDIndentNo.SelectedValue + " And INM.MasterCompanyId=" + Session["varCompanyId"];
        }
        try
        {
            DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
        }
        return DS;
    }
    private void SaveRefresh()
    {
        lblMessage.Text = "";
        BtnSave.Text = "Save";
        Session["IndentDetailId"] = 0;

        DDCompanyName.Enabled = false;
        DDPartyName.Enabled = false;
        DDProcessName.Enabled = false;
        DDProcessProgramNo.Enabled = false;
        TxtDate.Enabled = false;
        DDDesign.SelectedIndex = -1;
        DDColor.SelectedIndex = -1;
        DDShape.SelectedIndex = -1;
        DDSize.SelectedIndex = -1;
        DDColorShade.SelectedIndex = -1;

        if (TDDyeingMatch.Visible == true)
        {
            DDDyeingMatch.SelectedIndex = -1;
        }
        if (TDDyingType.Visible == true)
        {
            DDDyingType.SelectedIndex = -1;
        }
        if (TDDyeing.Visible == true)
        {
            DDDyeing.SelectedIndex = -1;
        }

        txtTotalQty.Text = "";
        TxtPreQty.Text = "";
        txtQty.Text = "";
        TxtRate.Text = "";
        txtstock.Text = "";
        ddllotno.SelectedIndex = -1;
        txtitemremark.Text = "";
        txtCanQty.Text = "";
        txtTagno.Text = "";
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds;
        string Str;
        if (TxtProdCode.Text != "")
        {
            IndentNoSelectedChange();
            Str = @"Select Distinct VF.CATEGORY_ID,VF.ITEM_ID,VF.QualityId,VF.designId,VF.ColorId,VF.ShapeId,VF.SizeId,VF.ShadecolorId From PP_Consumption OCD,
            ProcessProgram PP,V_FinishedItemDetail VF Where OCD.OrderId=PP.Order_Id And VF.Item_Finished_Id=OCD.FinishedId And PP.PPId=" + DDProcessProgramNo.SelectedValue + " And ProductCode='" + TxtProdCode.Text + "' And PP.MasterCompanyId=" + Session["varCompanyId"];

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                DDItem.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ComboFill();
                if (TdQuality.Visible == true)
                {
                    DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                }
                if (TdDesign.Visible == true)
                {
                    DDDesign.SelectedValue = ds.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                }
                if (TdColor.Visible == true)
                {
                    DDColor.SelectedValue = ds.Tables[0].Rows[0]["COLOR_ID"].ToString();
                }
                if (TdColorShade.Visible == true)
                {
                    DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadeColor_Id"].ToString();
                }
                if (TdShape.Visible == true)
                {
                    DDShape.SelectedValue = ds.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                    fillsize();
                }
                if (TdSize.Visible == true)
                {
                    DDSize.SelectedValue = ds.Tables[0].Rows[0]["SIZE_ID"].ToString();
                }
                Fill_Quantity();
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDCategory.SelectedIndex = 0;
                ddlcategorycange();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            DDCategory.SelectedIndex = 0;
            ddlcategorycange();
        }
    }
    protected void DGIndentDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["IndentDetailId"] = DGIndentDetail.SelectedValue;
        try
        {
            string Str = @"Select IndentDetailId,IndentId,PPNo,OFinishedId,VF.CATEGORY_ID,VF.ITEM_ID,VF.QualityId,VF.designId,VF.ColorId,VF.ShapeId,VF.SizeId,
                        VF.ShadecolorId,IndentQty Quantity,Rate,DyingType,lotNo,ORDERID,UnitId,id.remark as itemremark,flagsize,ExtraQty,CancelQty,ID.OrderDetailId,
                        id.DyingMatch,id.Dyeing,id.DyeingType,TagNo,isnull(vfi.shadecolorid,0) as Ishadecolorid,isnull(Id.Re_Process,0) as Re_Process, ID.GodownID  
                        From IndentDetail ID(Nolock) 
                        inner join V_FinishedItemDetail VF(Nolock) on ID.ofinishedid=vf.item_finished_id 
                        left join  v_finisheditemDetail VfI(Nolock) on id.ifinishedid=vfi.item_finished_id
                        Where  IndentDetailId=" + DGIndentDetail.SelectedValue + @" 
                        And VF.MasterCompanyId=" + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hnorderid.Value = ds.Tables[0].Rows[0]["OrderDetailId"].ToString();
                DDCategory.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                ddlcategorycange();
                DDItem.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                DDItem_SelectedIndexChanged(sender, e);
                chkredyeing.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Re_Process"]);
                if (TdQuality.Visible == true)
                {
                    DDQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                }
                if (TdDesign.Visible == true)
                {
                    DDDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
                }
                if (TdColor.Visible == true)
                {
                    DDColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                }
                if (TdColorShade.Visible == true)
                {
                    DDColorShade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
                    //**************Other Shade issue
                    if (variable.Carpetcompany == "1" && DDProcessName.SelectedItem.Text.ToUpper() == "DYEING")
                    {
                        if (variable.VarDyeingIssueOthershade == "1" || ds.Tables[0].Rows[0]["Re_Process"].ToString() == "1")
                        {
                            TDISSUESHADE.Visible = true;
                            FillIssueShade();
                            if (DDISSUESHADE.Items.FindByValue(ds.Tables[0].Rows[0]["IShadecolorId"].ToString()) != null)
                            {
                                DDISSUESHADE.SelectedValue = ds.Tables[0].Rows[0]["IShadecolorId"].ToString();
                            }
                            int VarOthershadefinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDISSUESHADE, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                            ViewState["othershadefinishedid"] = VarOthershadefinishedid;
                        }

                    }
                    //*************
                    DyeingTypeTrueorfalse();
                    if (TDDyeingMatch.Visible == true)
                    {
                        if (DDDyeingMatch.Items.FindByValue(ds.Tables[0].Rows[0]["DyingMatch"].ToString()) != null)
                        {
                            DDDyeingMatch.SelectedValue = ds.Tables[0].Rows[0]["DyingMatch"].ToString();
                        }
                    }
                    if (TDDyeing.Visible == true)
                    {
                        if (DDDyeing.Items.FindByValue(ds.Tables[0].Rows[0]["Dyeing"].ToString()) != null)
                        {
                            DDDyeing.SelectedValue = ds.Tables[0].Rows[0]["Dyeing"].ToString();
                        }
                    }
                    if (TDDyingType.Visible == true)
                    {
                        if (DDDyingType.Items.FindByValue(ds.Tables[0].Rows[0]["DyeingType"].ToString()) != null)
                        {
                            DDDyingType.SelectedValue = ds.Tables[0].Rows[0]["DyeingType"].ToString();
                        }
                    }
                }
                if (TdShape.Visible == true)
                {
                    DDShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                    DDsizetype.SelectedValue = ds.Tables[0].Rows[0]["flagsize"].ToString();
                    fillsize();
                }
                if (TdSize.Visible == true)
                {
                    DDSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                }
                if (ChkForOrder.Checked == false)
                {
                    Fill_Quantity();
                }
                if (ddUnit.Items.FindByValue(ds.Tables[0].Rows[0]["UnitId"].ToString()) != null)
                {
                    ddUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                }
                DDcaltype.SelectedValue = ds.Tables[0].Rows[0]["DyingType"].ToString();

                if (Convert.ToInt16(Session["varCompanyId"]) == 42)
                {
                    DDGodownName.SelectedValue = ds.Tables[0].Rows[0]["GodownID"].ToString();
                    FillLotNo();
                }

                if (TDLotNo.Visible == true)
                {
                    if (ddllotno.Items.FindByValue(ds.Tables[0].Rows[0]["lotNo"].ToString()) != null)
                    {
                        ddllotno.SelectedValue = ds.Tables[0].Rows[0]["lotNo"].ToString();
                    }
                }
                if (ChkForOrder.Checked == false && TDLotNo.Visible == true)
                {
                    LotNoSelectedChange();
                }
                txtQty.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                TxtRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                txtstock.Text = (Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text) + Convert.ToDouble(ds.Tables[0].Rows[0]["Quantity"])).ToString();
                TxtPreQty.Text = (Convert.ToDouble(TxtPreQty.Text == "" ? "0" : TxtPreQty.Text) - Convert.ToDouble(ds.Tables[0].Rows[0]["Quantity"])).ToString();
                txtitemremark.Text = ds.Tables[0].Rows[0]["itemremark"].ToString();
                txtCanQty.Text = ds.Tables[0].Rows[0]["CancelQty"].ToString();
                txtextraQty.Text = ds.Tables[0].Rows[0]["ExtraQty"].ToString();
                txtTagno.Text = ds.Tables[0].Rows[0]["TagNo"].ToString();

                if (Convert.ToInt32(Session["varCompanyId"]) == 16 && Session["usertype"].ToString() == "1")
                {
                    TxtRate.Enabled = true;
                }
            }
            else
            {
                lblMessage.Text = "ITEM CODE DOES NOT EXISTS....";
                DDCategory.SelectedIndex = 0;
                ddlcategorycange();
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
            Fillamount();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
        BtnSave.Text = "Update";
    }
    private void Fill_Rate()
    {
        try
        {
            string st = "select Rate from DyeingRateMaster where PartyId=" + DDPartyName.SelectedValue + " and FromoQty <=" + txtQty.Text + " and ToQty>=" + txtQty.Text + " and  FINISHEDID=" + ViewState["finishedid"] + " And MasterCompanyId=" + Session["varCompanyId"] + " And CompanyId=" + DDCompanyName.SelectedValue;
            TxtRate.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, st).ToString();
            if (TxtRate.Text == null)
            {
                lblMessage.Text = "Please add rate first for desired QTY..........";
            }
            else
            {
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
        }
    }
    protected void txtidnt_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (txtidnt.Text != "")
        {
            try
            {
                string str = @"Select distinct IM.CompanyId,PartyId,ProcessId,IM.IndentId,PPNo,ID.orderid,CI.customerid 
                    From IndentMaster IM 
                    inner Join IndentDetail ID on IM.IndentId=ID.IndentId 
                    Left outer join OrderMaster OM on OM.orderid=ID.orderid 
                    left outer join Customerinfo Ci on Ci.customerid=OM.customerid 
                    Where IM.CompanyID = " + DDCompanyName.SelectedValue + " And IM.BranchID = " + DDBranchName.SelectedValue + @" And 
                    IM.IndentNo='" + txtidnt.Text + "' And IM.MasterCompanyId=" + Session["varCompanyId"];
                if (chkcomplete.Checked == true)
                {
                    str = str + " and Im.status='Complete'";
                }
                else
                {
                    str = str + " and Im.status='Pending'";
                }
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ChkForOrder.Checked == true)
                    {
                        DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["Customerid"].ToString();
                        DDCustomerCode_SelectedIndexChanged(sender, e);
                        DDOrderNo.SelectedValue = ds.Tables[0].Rows[0]["Orderid"].ToString();
                    }
                    DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
                    PartyNameSelectedChanged();
                    DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
                    switch (DDProcessName.SelectedItem.Text.ToUpper())
                    {
                        case "DYEING":
                            TDReDyeing.Visible = true;
                            break;
                        default:
                            TDReDyeing.Visible = false;
                            break;
                    }
                    ProcessNameSelectedChanged();
                    DDProcessProgramNo.SelectedValue = ds.Tables[0].Rows[0]["PPNo"].ToString();
                    ProcessProgramNoSelectedChange();
                    DDIndentNo.SelectedValue = ds.Tables[0].Rows[0]["IndentId"].ToString();
                    IndentNoSelectedChange();
                }
                else
                {
                    txtidnt.Text = "";
                    txtidnt.Focus();
                    lblMessage.Visible = true;
                    lblMessage.Text = "Indent No does not exists or Indent is (Complete or Pending)";
                }

            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }
    }
    protected void ddllotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstock.Text = "";
        if (TDDDTagNo.Visible == false)
        {
            LotNoSelectedChange();
        }
        else
        {
            if (variable.Carpetcompany == "1" && variable.VarDyeingIssueOthershade == "1" && DDProcessName.SelectedItem.Text.ToUpper() == "DYEING")
            {
                FillTagNoOtherIssueShadeWise();
            }
            else
            {

                FillTagNo();
            }
        }        
    }
    private void FillTagNo()
    {
        string str = @"select distinct s.TagNo, s.TagNo 
                        From stock s(Nolock) 
                        join ORDER_CONSUMPTION_DETAIL OCD(Nolock) on S.ITEM_FINISHED_ID=OCD.IFINISHEDID 
                        join ProcessProgram(Nolock) PR on OCD.ORDERID=pr.Order_ID 
                        where pr.PPID=" + DDProcessProgramNo.SelectedValue + " and ocd.OFINISHEDID=" + Session["FinishedId"] + @" and 
                        Companyid=" + DDCompanyName.SelectedValue;

        if (ddllotno.SelectedIndex > 0)
        {
            str = str + " And s.LotNo = '" + ddllotno.SelectedItem.Text + "' ";
        }
        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(S.Qtyinhand,3)>0";
        }
        if (chkredyeing.Checked == false)
        {
            str = str + " and Godownid in(" + variable.VarGENERATEINDENTGODOWNID + ")";
        }
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Select--");
    }
    private void FillTagNoOtherIssueShadeWise()
    {
        string str = @"select Distinct S.TagNo,S.TagNo From stock S inner Join V_FinishedItemDetail vf on s.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID
                                Where S.Companyid=" + DDCompanyName.SelectedValue + " and  vf.ITEM_ID=" + DDItem.SelectedValue + @" And 
                                Vf.QualityId=" + DDQuality.SelectedValue + " and vf.ShadecolorId=" + DDISSUESHADE.SelectedValue;

        if (ddllotno.SelectedIndex > 0)
        {
            str = str + " And s.LotNo = '" + ddllotno.SelectedItem.Text + "' ";
        }
        if (MySession.Stockapply == "True")
        {
            str = str + " and Round(S.Qtyinhand,3)>0";
        }
        if (chkredyeing.Checked == false)
        {
            str = str + " and Godownid in(" + variable.VarGENERATEINDENTGODOWNID + ")";
        }
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Select--");
    }
    private void LotNoSelectedChange()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[11];

            _arrpara[0] = new SqlParameter("@lotno", SqlDbType.NVarChar, 50);
            _arrpara[1] = new SqlParameter("@finishedid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@companyid", SqlDbType.Int);
            _arrpara[3] = new SqlParameter("@stockqty", SqlDbType.Float, 50);
            _arrpara[4] = new SqlParameter("@OFinishedid", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@PPID", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@Processid", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Othershadefinishedid", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@Re_Process", SqlDbType.Int);
            _arrpara[9] = new SqlParameter("@GodownID", SqlDbType.Int);
            _arrpara[10] = new SqlParameter("@TagNo", SqlDbType.NVarChar, 50);

            _arrpara[0].Value = ddllotno.SelectedItem.Text;
            _arrpara[1].Value = 0;
            _arrpara[2].Value = DDCompanyName.SelectedValue;
            _arrpara[3].Direction = ParameterDirection.Output;
            _arrpara[4].Value = ViewState["finishedid"];
            _arrpara[5].Value = DDProcessProgramNo.SelectedValue;
            _arrpara[6].Value = DDProcessName.SelectedValue;
            _arrpara[7].Value = TDISSUESHADE.Visible == true && variable.Carpetcompany == "1" ? ViewState["othershadefinishedid"] : "0";
            _arrpara[8].Value = chkredyeing.Checked == true ? "1" : "0";
            _arrpara[9].Value = TDGodownName.Visible == true ? DDGodownName.SelectedValue : "0";
            _arrpara[10].Value = TDDDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_Stockqty", _arrpara);
            Tran.Commit();
            txtstock.Text = _arrpara[3].Value.ToString();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private string CheckStockQty()
    {
        string Message = "";
        try
        {
            SqlParameter[] _param = new SqlParameter[7];
            _param[0] = new SqlParameter("@OFinishedID", ViewState["finishedid"]);
            _param[1] = new SqlParameter("@ProcessProgramNo", DDProcessProgramNo.SelectedValue);
            _param[2] = new SqlParameter("@TXTQTY", txtQty.Text);
            _param[3] = new SqlParameter("@Message", SqlDbType.NVarChar, 10);
            _param[3].Direction = ParameterDirection.Output;
            _param[4] = new SqlParameter("@NewEditFlag", BtnSave.Text == "Update" ? DGIndentDetail.SelectedDataKey.Value : 0);
            _param[5] = new SqlParameter("@LotNo", ddllotno.SelectedItem.Text);
            _param[6] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "CheckStockQty_indent", _param);
            Message = _param[3].Value.ToString();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
            UtilityModule.MessageAlert(ex.Message, "Masters/Process/GenrateInDent");
        }
        return Message;
    }
    private void check_qty()
    {
        double stockqty = Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
        double totalQty = Convert.ToDouble(txtTotalQty.Text == "" ? "0" : txtTotalQty.Text);
        double VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndent from MasterSetting"));
        if (VarPercentQty <= 0.0)
        {
            VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                    "Select PercentageExecssQtyForIndent From ProcessProgramExcessPercentage(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " And PPID = " + DDProcessProgramNo.SelectedValue));
        }
        totalQty = totalQty * (100.0 + VarPercentQty) / 100;
        double PreQty = Convert.ToDouble(TxtPreQty.Text == "" ? "0" : TxtPreQty.Text);
        double Qty = Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text);
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
            case "12":
                if (Qty + PreQty > totalQty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Qty is greater than Stock or Total Qty";
                    txtQty.Text = "";
                    txtQty.Focus();
                }
                else
                {
                    lblMessage.Visible = false;
                }
                break;
            default:
                if (Qty + PreQty > totalQty || Qty > stockqty && variable.VarGENRATEINDENTWITHOUTLOT_STOCK == "0")
                {

                    lblMessage.Visible = true;
                    lblMessage.Text = "Qty is greater than Stock or Total Qty";
                    txtQty.Text = "";
                    txtQty.Focus();
                }
                else
                {
                    lblMessage.Visible = false;
                }
                break;
        }
    }
    protected void DGIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblMessage.Text = "";
        int VarIndentDetailId = Convert.ToInt32(DGIndentDetail.DataKeys[e.RowIndex].Value);
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[3];
            arr[0] = new SqlParameter("@indentdetailid", VarIndentDetailId);
            arr[1] = new SqlParameter("@carpetcompany", variable.Carpetcompany);
            arr[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[2].Direction = ParameterDirection.Output;
            //**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteindentdetail", arr);
            lblMessage.Text = arr[2].Value.ToString();
            lblMessage.Visible = true;
            Tran.Commit();
            BtnSave.Text = "Save";
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
        #region
        //DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IndentId from PP_ProcessRawTran Where IndentID in (Select IndentId From IndentDetail where IndentDetailId=" + VarIndentDetailId + ")");
        //if (Ds.Tables[0].Rows.Count > 0)
        //{
        //    lblMessage.Text = "This Indent No already Issued So You Can't Delete this row.";
        //    lblMessage.Visible = true;
        //}
        //else
        //{
        //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //    con.Open();
        //    SqlTransaction tran = con.BeginTransaction();
        //    try
        //    {
        //        if (DGIndentDetail.Rows.Count == 1)
        //        {
        //            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Delete from IndentMaster Where IndentId in (Select IndentId From IndentDetail where IndentDetailId=" + VarIndentDetailId + ")");
        //        }
        //        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Delete from IndentDetail where IndentDetailId=" + VarIndentDetailId);
        //        lblMessage.Text = "Data Deleted ..............";
        //        tran.Commit();
        //        BtnSave.Text = "Save";
        //        Fill_Grid();
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
        //        lblMessage.Text = ex.Message;
        //        lblMessage.Visible = true;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        #endregion

    }
    protected void DGIndentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGIndentDetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGIndentDetail.Columns.Count; i++)
            {
                if (variable.VarGENERATEINDENTTAGNOAUTOGENERATED == "1")
                {
                    if (DGIndentDetail.Columns[i].HeaderText.ToUpper() == "UPDATE TAG NO.")
                    {
                        Label lbltagno = (Label)e.Row.FindControl("lbltagno");
                        if (lbltagno.Text.ToUpper() == "WITHOUT TAG NO")
                        {
                            DGIndentDetail.Columns[i].Visible = true;
                        }
                        else
                        {
                            DGIndentDetail.Columns[i].Visible = false;
                        }
                    }
                }
                else
                {
                    if (DGIndentDetail.Columns[i].HeaderText.ToUpper() == "UPDATE TAG NO.")
                    {
                        DGIndentDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
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
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForOrder.Checked == true)
        {
            TDCaltype.Visible = false;
            //TDLblReqDate.Visible = true;
            TDLotNo.Visible = false;
            TDStockQty.Visible = false;
            TDLoss.Visible = false;
            TDTotalQty.Visible = false;
            TDPreQty.Visible = false;
            BtnAddRate.Visible = false;
            TDppno.Visible = false;
            LblKg.Visible = false;
            TDCustCode.Visible = true;
            TxtRate.Enabled = true;
            TDOrderNo.Visible = true;
        }
        else
        {
            LblKg.Visible = true;
            TDLblReqDate.Visible = false;
            TDOrderNo.Visible = false;
            TxtRate.Enabled = false;
            TDCustCode.Visible = false;
            TDCaltype.Visible = true;
            TDLotNo.Visible = true;
            TDStockQty.Visible = true;
            TDLoss.Visible = true;
            TDTotalQty.Visible = true;
            TDPreQty.Visible = true;
            BtnAddRate.Visible = true;
            TDppno.Visible = true;
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    private void CustomerCodeSelectedChange()
    {
        if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDOrderNo, @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster OM,OrderLocalConsumption OC Where OM.OrderID=OC.OrderId And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue, true, "--Select--");
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //1 For Final  
        string process = "";
        if (Session["WithoutBom"].ToString() == "1" || ChkForOrder.Checked == true)
        {
            process = "select Distinct Process_Name_Id,Process_Name From Process_Name_Master PM where MasterCompanyId=" + Session["varCompanyId"] + " order by Process_Name Asc";
        }
        switch (Session["varcompanyNo"].ToString())
        {
            case "7":
                process = "select Distinct Process_Name_Id,Process_Name From Process_Name_Master PM,OrderProcessPlanning PP where PM.Process_Name_Id=PP.ProcessId And OrderId=" + DDOrderNo.SelectedValue + " And FinalStatus=1 And PM.MasterCompanyId=" + Session["varCompanyId"] + " order by Process_Name Asc";
                break;

        }
        UtilityModule.ConditionalComboFill(ref DDProcessName, process, true, "--Select Process--");
        string CateG = @"Select distinct ICM.Category_Id,ICM.Category_Name from  Item_Parameter_Master IPM  inner Join Item_Master IM on IM.Item_Id=IPM.Item_Id And IM.MasterCompanyId=" + Session["varCompanyId"] + @" inner join Item_Category_Master ICM  on 
                     ICM.Category_Id=IM.Category_Id inner join UserRights_Category UC on ICM.Category_Id=UC.CategoryId And UC.UserId=" + Session["varuserid"];
        UtilityModule.ConditionalComboFill(ref DDCategory, CateG, true, "--Select Category--");
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 7)
        {
            Btnorder.Visible = false;
        }
    }
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDColorShade, 0, "", Convert.ToInt32(Session["varCompanyId"])));
        TxtFinishedid.Text = "a1=" + DDCompanyName.SelectedValue + "&a2=" + DDPartyName.SelectedValue + "&a3=" + finishedid + "&a4=" + DDcaltype.SelectedValue;
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] parparam = new SqlParameter[1];
            parparam[0] = new SqlParameter("@IndentID", DDIndentNo.SelectedValue);
            int i = SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_IndentCancel", parparam);
            if (i > 0)
            {
                lblMessage.Text = "Record(s) has been Cancelled !";
            }
            else
            {
                lblMessage.Text = "This Indent no. has been Issued, So You Can't Cancel";
            }
            tran.Commit();
            Fill_Grid();
        }

        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditGenrateInDent.aspx");
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    protected void TxtReqDate_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(TxtReqDate.Text) < Convert.ToDateTime(TxtDate.Text))
        {
            lblMessage.Visible = true;
            TxtReqDate.Text = TxtDate.Text;
            lblMessage.Text = "Req. Date Can not be shorter than OrderDate..";
        }
        else
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (MySession.IndentAsProduction == "1")
        {
            Showreport();
        }
        else if (Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "43")
        {
            string qry = @"select * from v_indentreportForCarpetCompany where indentid=" + DDIndentNo.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForWithoutRate.Checked == true)
                {
                    Session["rptFileName"] = "~\\Reports\\RptIndentReportNewWithoutRate.rpt";
                }
                else
                {
                    if (Session["varcompanyid"].ToString() == "43")
                    {
                        Session["rptFileName"] = "~\\Reports\\rptindentCarpetInternational.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\rptindentreportnew.rpt";
                    }
                }
                //Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptindentreportnew.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }
        else if (ChkForOrder.Checked == false)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            string qry = @"select * from v_IndentOrderWise where indentid=" + DDIndentNo.SelectedValue + "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            if (ds.Tables[0].Rows.Count > 0)
            {

                Session["rptFileName"] = "~\\Reports\\Rptgenerateindetwithorder.rpt";

                //Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\Rptgenerateindetwithorder.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
        }

    }
    protected void Showreport()
    {
        if (DDIndentNo.SelectedValue == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "alert", "alert('Please select or Type indent No!!!');", true);
            return;
        }
        string str = "select * from [GenrateIndentReport] where indentid=" + DDIndentNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Update reportcount status
            str = "update indentmaster set reportcount=isnull(reportcount,0)+1 Where indentid=" + DDIndentNo.SelectedValue;
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //end
            Session["rptFileName"] = "~\\Reports\\rptgenerateindentapprovalnew.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptgenerateindentapprovalnew.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }

    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        QtyTextChanged();
    }
    private void QtyTextChanged()
    {
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
        {
            string ChkMsg = CheckStockQty();
            if (ChkMsg == "G")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Qty should not be greater than assigned stock";
                return;
            }
        }
        if (ChkForOrder.Checked == false)
        {
            double stockqty = Convert.ToDouble(txtstock.Text == "" ? "0" : txtstock.Text);
            double totalQty = Convert.ToDouble(txtTotalQty.Text == "" ? "0" : txtTotalQty.Text);
            double VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select PercentageExecssQtyForIndent from MasterSetting"));
            if (VarPercentQty <= 0.0)
            {
                VarPercentQty = Convert.ToDouble(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                        "Select PercentageExecssQtyForIndent From ProcessProgramExcessPercentage(Nolock) Where MasterCompanyID = " + Session["varCompanyId"] + " And PPID = " + DDProcessProgramNo.SelectedValue));
            }
            totalQty = totalQty * (100.0 + VarPercentQty) / 100;
            double PreQty = Convert.ToDouble(TxtPreQty.Text == "" ? "0" : TxtPreQty.Text);
            double Qty = Convert.ToDouble(txtQty.Text);
            if (Session["VarcompanyNo"].ToString() == "6" || MySession.IndentAsProduction == "1")
            {
                if (Qty + PreQty > totalQty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Qty is greater than Stock or Total Qty";
                    txtQty.Text = "";
                    txtQty.Focus();
                }
                else
                {

                    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                    con.Open();
                    SqlTransaction Tran = con.BeginTransaction();
                    try
                    {
                        lblMessage.Text = "";
                        int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"])));
                        string st = "select round(isnull(Rate,0),2) from DyeingRateMaster where PartyId=" + DDPartyName.SelectedValue + " and FromoQty <=" + txtQty.Text + " and ToQty>=" + txtQty.Text + " and  FINISHEDID=" + finishedid + " And MasterCompanyId=" + Session["varCompanyId"] + " And CompanyId=" + DDCompanyName.SelectedValue;
                        double rate = Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, st));
                        Tran.Commit();
                        if (rate == 0)
                        {
                            //lblMessage.Visible = true;
                            //lblMessage.Text = "Error:- Rate not define please define rate..........";
                            //txtQty.Text = "0";
                        }
                        else
                        {
                            lblMessage.Text = "";
                            TxtRate.Text = rate.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        UtilityModule.MessageAlert(ex.Message, "Master/Process/GenrateInDent.aspx");
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
            else
            {
                if (Qty + PreQty > totalQty || Qty > stockqty && variable.VarGENRATEINDENTWITHOUTLOT_STOCK == "0")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Qty is greater than Stock Qty or Total Qty";
                    txtQty.Text = "";
                    txtQty.Focus();
                }
                else
                {
                    if (Session["varcompanyNo"].ToString() != "4")
                    {
                        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                        con.Open();
                        SqlTransaction Tran = con.BeginTransaction();
                        try
                        {
                            lblMessage.Text = "";
                            double QtyNew = 0;
                            if (Session["varcompanyNo"].ToString() == "30")
                            {
                                //QtyNew = txtQty.Text == "" ? 0 : Convert.ToDouble(txtQty.Text) + txtextraQty.Text == "" ? 0 : Convert.ToDouble(txtextraQty.Text);
                                QtyNew = (txtQty.Text == "" ? 0 : Convert.ToDouble(txtQty.Text)) + (txtextraQty.Text == "" ? 0 : Convert.ToDouble(txtextraQty.Text));
                            }
                            else
                            {
                                QtyNew = txtQty.Text == "" ? 0 : Convert.ToDouble(txtQty.Text);
                            }

                            int finishedid = Convert.ToInt32(UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, Tran, DDColorShade, "", Convert.ToInt32(Session["varCompanyId"])));
                            string st = "select round(isnull(Rate,0),2) from DyeingRateMaster where PartyId=" + DDPartyName.SelectedValue + @" And 
                                        FromoQty <=" + QtyNew + " and ToQty>=" + QtyNew + " and  FINISHEDID=" + finishedid + " And MasterCompanyId=" + Session["varCompanyId"] + @" And 
                                        CompanyId=" + DDCompanyName.SelectedValue;
                            double rate = Convert.ToDouble(SqlHelper.ExecuteScalar(Tran, CommandType.Text, st));
                            Tran.Commit();
                            //if (rate == 0 && Convert.ToInt32(Session["varcompanyNo"]) != 16)
                            if (rate == 0)
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Error:- Rate not define please define rate..........";
                                txtQty.Text = "0";
                            }
                            else
                            {
                                lblMessage.Text = "";
                                TxtRate.Text = rate.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            UtilityModule.MessageAlert(ex.Message, "Master/Process/GenrateInDent.aspx");
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }
                }
            }
        }
        Fillamount();
    }
    protected void DDISSUESHADE_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotnoIssueshade();
        int VarOthershadefinishedid = UtilityModule.getItemFinishedId(DDItem, DDQuality, DDDesign, DDColor, DDShape, DDSize, TxtProdCode, DDISSUESHADE, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        ViewState["othershadefinishedid"] = VarOthershadefinishedid;
    }
    protected void chkredyeing_CheckedChanged(object sender, EventArgs e)
    {
        TDISSUESHADE.Visible = false;
        if (chkredyeing.Checked == true)
        {
            TDISSUESHADE.Visible = true;
        }
    }
    protected void lnkupdatetagno_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        lblMessage.Visible = true;
        GridViewRow gvr = ((LinkButton)sender).NamingContainer as GridViewRow;

        try
        {
            Label lbltagno = (Label)gvr.FindControl("lbltagno");
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@IndentDetailId", DGIndentDetail.DataKeys[gvr.RowIndex].Value);
            param[1] = new SqlParameter("@TagNo", lbltagno.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Userid", Session["varuserid"]);
            param[4] = new SqlParameter("@MastercompanyId", Session["varcompanyid"]);
            //*********
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_UPDATEINDENTTAGNO", param);
            lblMessage.Text = param[2].Value.ToString();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.Visible = true;
        }
    }
    protected void Fillamount()
    {
        double Tqyt = Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) + Convert.ToDouble(txtextraQty.Text == "" ? "0" : txtextraQty.Text) - Convert.ToDouble(txtCanQty.Text == "" ? "0" : txtCanQty.Text);
        txtamt.Text = Convert.ToString(Tqyt * Convert.ToDouble(TxtRate.Text == "" ? "0" : TxtRate.Text));
    }
    protected void txtextraQty_TextChanged(object sender, EventArgs e)
    {
        QtyTextChanged();
        //Fillamount();
    }
    protected void txtCanQty_TextChanged(object sender, EventArgs e)
    {
        Fillamount();
    }
    protected void TxtRate_TextChanged(object sender, EventArgs e)
    {
        Fillamount();
    }
    protected void BtnEmployeeWisePPDetail_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlCommand cmd = new SqlCommand("PRO_GET_EMPLOYEEWISE_PP_CONSUMPTION_DETAIL", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@PPID", DDProcessProgramNo.SelectedValue);
        cmd.Parameters.AddWithValue("@EmpID", DDPartyName.SelectedIndex == -1 ? "0" : DDPartyName.SelectedValue);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ad.Fill(ds);
        if (ds.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('No data found for this selection !!!')", true);
            return;
        }
        else
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;

            GridView1.DataSource = ds;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EmployeeWisePP_ConsumptionDetail" + DateTime.Now + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //*************
        }
    }
    protected void BtnRateUpdate_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["IndentDetailId"]) == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('Please select atleast one row for update rate !!!')", true);
            return;
        }
        string Str = "Update IndentDetail Set Rate = " + TxtRate.Text + " Where IndentID = " + DDIndentNo.SelectedValue + " And IndentDetailID = " + Session["IndentDetailId"];
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        ScriptManager.RegisterStartupScript(Page, GetType(), "altform12", "alert('Rate updated successfully !!!')", true);
        Fill_Grid();
        SaveRefresh();
    }
    private void FillGodownName()
    {
        string str = @"Select Distinct GM.GodownID, GM.GodownName 
            From ProcessProgram PR(Nolock)
            JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.ORDERID = PR.Order_ID And OCD.OFINISHEDID = " + ViewState["finishedid"] + @" 
            JOIN Stock S(Nolock) ON S.ITEM_FINISHED_ID = OCD.IFINISHEDID 
            JOIN GodownMaster GM (Nolock) ON GM.GoDownID = S.GodownID 
            JOIN GodownWiseEmp GE(Nolock) ON GE.GodownID = GM.GodownID And GE.EmpID = " + DDPartyName.SelectedValue + @" 
            Where PR.PPID = " + DDProcessProgramNo.SelectedValue + @" And S.Companyid = " + DDCompanyName.SelectedValue;

        if (MySession.Stockapply == "True")
        {
            str = str + " And Round(S.Qtyinhand, 3) > 0 ";
        }
        UtilityModule.ConditionalComboFill(ref DDGodownName, str, true, "--Select--");
    }
    protected void DDGodownName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLotNo();
    }
    private void FillLotNo()
    {
        string str = "";
        DataSet ds;
        if (variable.ORDER_STOCK_ASSIGN == "1")
        {
            str = @"Select Distinct OSA.LotNo, OSA.LotNo 
                                From ProcessProgram PP(Nolock) 
                                JOIN OrderStockAssign OSA(Nolock) ON OSA.OrderID = PP.Order_ID 
                                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OSA.FinishedID And VF.ITEM_ID = " + DDItem.SelectedValue + @" And 
                                            VF.QualityId = " + DDQuality.SelectedValue + "  And VF.ShadecolorId = " + DDISSUESHADE.SelectedValue + @" 
                                Where PP.PPID = " + DDProcessProgramNo.SelectedValue;

            UtilityModule.ConditionalComboFill(ref ddllotno, str, true, "--Select--");
        }
        else
        {
            if (Session["varcompanyNo"].ToString() == "6")
            {
                str = @"select Distinct lotno,lotno From Stock 
                Where Item_Finished_id in(Select IFinishedid 
                        From Order_Consumption_Detail 
                        Where OrderId in(select Order_Id from ProcessProgram Where PPID=" + DDProcessProgramNo.SelectedValue + ") And OFInishedid= " + ViewState["finishedid"] + ") and CompanyId=" + DDCompanyName.SelectedValue + "";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    UtilityModule.ConditionalComboFill(ref ddllotno, @"select Distinct lotno,lotno From Stock 
                    Where Item_Finished_id in(select IFInishedid from Order_Consumption_Detail 
                            Where OrderId in(select Order_id from ProcessProgram where PPID=" + DDProcessProgramNo.SelectedValue + ") And OFInishedid=" + ViewState["finishedid"] + ") And CompanyId=" + DDCompanyName.SelectedValue + "", true, "select");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddllotno, "select 1,1", true, "select");
                }
            }
            else
            {
                if (variable.Carpetcompany == "1" && (variable.VarDyeingIssueOthershade == "1" || chkredyeing.Checked == true) && DDProcessName.SelectedItem.Text.ToUpper() == "DYEING")
                {
                    str = @"select Distinct S.lotno,S.lotno From stock S inner Join V_FinishedItemDetail vf on s.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID
                                Where S.Companyid=" + DDCompanyName.SelectedValue + " and  vf.ITEM_ID=" + DDItem.SelectedValue + @" And 
                                Vf.QualityId=" + DDQuality.SelectedValue + " and vf.ShadecolorId=" + DDISSUESHADE.SelectedValue;
                    if (MySession.Stockapply == "True")
                    {
                        str = str + " And Round(S.Qtyinhand,3)>0";
                    }
                    if (chkredyeing.Checked == false)
                    {
                        str = str + " and S.Godownid in(" + variable.VarGENERATEINDENTGODOWNID + ")";
                    }
                    UtilityModule.ConditionalComboFill(ref ddllotno, str, true, "--Select--");
                }
                else
                {
                    str = @"select distinct Lotno,lotno 
                        From stock s(Nolock) 
                        join ORDER_CONSUMPTION_DETAIL OCD(Nolock) on S.ITEM_FINISHED_ID=OCD.IFINISHEDID 
                        join ProcessProgram(Nolock) PR on OCD.ORDERID=pr.Order_ID 
                        where pr.PPID=" + DDProcessProgramNo.SelectedValue + " and ocd.OFINISHEDID=" + ViewState["finishedid"] + @" and 
                        Companyid=" + DDCompanyName.SelectedValue;

                    if (MySession.Stockapply == "True")
                    {
                        str = str + " and Round(S.qtyinhand,3)>0";
                    }
                    if (Convert.ToInt16(Session["varCompanyId"]) == 42)
                    {
                        str = str + " and S.Godownid = " + DDGodownName.SelectedValue;
                    }
                    else
                    {
                        if (chkredyeing.Checked == false)
                        {
                            str = str + " and S.Godownid in(" + variable.VarGENERATEINDENTGODOWNID + ")";
                        }
                    }
                    UtilityModule.ConditionalComboFill(ref ddllotno, str, true, "--Select--");
                }
            }
        }
    }

    protected void FillLotnoIssueshade()
    {
        if (variable.Carpetcompany == "1")
        {
            if (variable.VarDyeingIssueOthershade == "1" || chkredyeing.Checked == true)
            {
                string str = @"select Distinct S.lotno,S.lotno From stock S inner Join V_FinishedItemDetail vf on s.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID
                      Where S.Companyid=" + DDCompanyName.SelectedValue + " and  vf.ITEM_ID=" + DDItem.SelectedValue + " and Vf.QualityId=" + DDQuality.SelectedValue + " and vf.ShadecolorId=" + DDISSUESHADE.SelectedValue + " ";
                if (MySession.Stockapply == "True")
                {
                    str = str + " and Round(Qtyinhand,3)>0";
                }
                str = str + " and S.godownid in(" + variable.VarGENERATEINDENTGODOWNID + ")";
                UtilityModule.ConditionalComboFill(ref ddllotno, str, true, "--Select--");
            }
        }
    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LotNoSelectedChange();
    }
}
