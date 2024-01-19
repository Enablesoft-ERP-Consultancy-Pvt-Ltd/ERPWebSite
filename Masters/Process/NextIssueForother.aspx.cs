using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

public partial class Masters_Process_NextIssueForother : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected static string Focus = "";
    static string btnclickflag = "";
    static decimal WashingByWeight = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        if (Focus != "")
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            sm.SetFocus(Focus);
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order By CompanyName", true, "--SelectCompany");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            logo();
            string str;
            if (Session["varcompanyId"].ToString() == "8")
            {
                str = @"select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 order by Process_Name  ";
            }
            else
            {
                //str = @"select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 and Processtype=1 order by Process_Name";
                str = @"select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                        WHere PNM.ProcessType=1 and PNM.PROCESS_NAME_ID<>1 order by PROCESS_NAME";

            }
            str = str + @" Select UnitId,unitName from Unit where UnitId in(1,2,6) order by UnitId
                        Select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                        Select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME
                        Select ID, BranchName From BRANCHMASTER BM(nolock) JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDTOProcess, ds, 0, true, "--Plz Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--Plz Select Unit--");
            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 2, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "--Plz Select--");
            //if (DDCategory.Items.Count > 0)
            //{
            //    DDCategory.SelectedIndex = 1;
            //    DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
            //}

            DDUnit.SelectedIndex = 1;

            if (Session["VarCompanyNo"].ToString() == "43")
            {
                if (Session["CurrentWorkingCompanyID"].ToString() == "2")
                {
                    ddUnits.SelectedIndex = 1;
                }
                else
                {
                    ddUnits.SelectedIndex = 0;
                }
            }          


            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            string str2 = "";
            str2 = @"select CustomerId,CustomerCode from customerinfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by Customercode ";
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds2, 0, true, "--Plz Select--");


            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtDataFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtgetdataupto.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            BtnPreview.Enabled = false;
            DDFromProcess.Focus();
            ViewState["IssueOrderid"] = 0;
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyId"].ToString());
            switch (VarCompanyNo)
            {
                case 1:
                    ProCod1.Visible = false;
                    break;
                case 2:
                    ProCod1.Visible = true;
                    break;
                case 4:
                    ProCod1.Visible = false;
                    DDUnit.SelectedValue = "2";
                    break;
                case 5:
                    ProCod1.Visible = false;
                    TDRateNew.Visible = true;
                    TDAreaNew.Visible = true;
                    if (Session["VarDepartMent"].ToString() == "0")
                    {
                        TDRateNew.Visible = false;
                    }
                    break;
                case 8:
                    ChkForIssue_Receive.Visible = true;
                    //Tdissuedate.Visible = false;
                    //Tdreqdate.Visible = false;
                    TDCategory.Visible = false;
                    tdqualityname1.Visible = false;
                    tddesign1.Visible = false;
                    TDgetstockdetail.Visible = false;
                    tdColor1.Visible = false;
                    tdsize1.Visible = false;
                    TDButtonsavegrid.Visible = false;
                    TDDatestamp.Visible = false;
                    TDRecchallanNo.Visible = false;
                    TDgetDataupto.Visible = false;
                    break;
                case 30:
                    lblStockCarpetNo.Text = "Enter Carpet No";
                    break;

                case 44:
                    DIVStockDetail.Visible = true;
                    btngetstock.Visible = true;
                    btnsavegrid.Visible = true;
                    TDCustomerCode.Visible = true;
                    TDCustomerOrderNo.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    Td2.Visible = true;
                    break;
                case 16:
                case 28:
                    DIVStockDetail.Visible = true;
                    btngetstock.Visible = true;
                    btnsavegrid.Visible = true;
                    TDCustomerCode.Visible = true;
                    TDCustomerOrderNo.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    TdDyingLotNo.Visible = true;
                    break;
                case 27:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;
                            TDCustomerCode.Visible = true;
                            TDCustomerOrderNo.Visible = true;

                            break;
                        default:
                            btnsavegrid.Visible = true;
                            TDCustomerCode.Visible = true;
                            TDCustomerOrderNo.Visible = true;
                            break;
                    }
                    break;
                case 38:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;
                            TDCustomerCode.Visible = true;
                            TDCustomerOrderNo.Visible = true;
                            ChkForWithoutRate.Visible = true;
                            TxtIssueQty.Enabled = true;
                            TxtIssueQty.Text = "200";
                            DGStockDetail.PageSize = 200;
                            break;
                        default:
                            btnsavegrid.Visible = false;
                            TDCustomerCode.Visible = true;
                            TDCustomerOrderNo.Visible = true;
                            ChkForWithoutRate.Visible = true;
                            break;
                    }
                    break;
                case 42:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;
                            TDExportSize.Visible = true;
                            Tr3.Visible = true;
                            break;
                        default:
                            //DIVStockDetail.Visible = false;
                            //btngetstock.Visible = false;
                            btnsavegrid.Visible = false;
                            TDCustomerCode.Visible = false;
                            TDCustomerOrderNo.Visible = false;
                            TDExportSize.Visible = false;
                            Tr3.Visible = true;
                            break;
                    }
                    break;
                case 43:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;
                            TrBindRecChallanNoJobWise.Visible = true;

                            break;
                        default:
                            //DIVStockDetail.Visible = false;
                            //btngetstock.Visible = false;
                            btnsavegrid.Visible = true;
                            TDCustomerCode.Visible = false;
                            TDCustomerOrderNo.Visible = false;
                            TrBindRecChallanNoJobWise.Visible = true;
                            break;
                    }
                    break;
                case 46:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;
                            TDExportSize.Visible = true;
                            break;
                        default:
                            //DIVStockDetail.Visible = false;
                            //btngetstock.Visible = false;
                            btnsavegrid.Visible = true;
                            TDCustomerCode.Visible = false;
                            TDCustomerOrderNo.Visible = false;
                             TDExportSize.Visible  = false;
                            break;
                    }
                    break;
                default:
                    switch (Session["Usertype"].ToString())
                    {
                        case "1":
                            DIVStockDetail.Visible = true;
                            btngetstock.Visible = true;
                            btnsavegrid.Visible = true;

                            break;
                        default:
                            //DIVStockDetail.Visible = false;
                            //btngetstock.Visible = false;
                            btnsavegrid.Visible = false;
                            TDCustomerCode.Visible = false;
                            TDCustomerOrderNo.Visible = false;
                            break;
                    }

                    break;
            }
            //user type
            switch (Session["Usertype"].ToString())
            {
                case "1":
                    TDRateNew.Visible = true;
                    //TDAreaNew.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void logo()
    {
        imgLogo.ImageUrl.DefaultIfEmpty();
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;

    }
    protected void DDFromProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;
        UtilityModule.ConditionalComboFill(ref DDTOProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER where PROCESS_Name_ID != " + ViewState["FromProcessId"] + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");
    }
    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {

        Enablecontrols();
        lstWeaverName.Items.Clear();
        txtWeaverIdNo_AutoCompleteExtender.ContextKey = DDTOProcess.SelectedValue;
        //For grid fill

        hnfromprocessid.Value = "0";
        //
        LblErrorMessage.Text = "";
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        if (tdqualityname1.Visible == true)
        {
            DDQuality.SelectedIndex = -1;
        }
        ////string str = "Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid And EP.Processid=" + DDTOProcess.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"];
        //str = "Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME";
        //DataSet ds = new DataSet();
        //ds = SqlHelper.ExecuteDataset(str);

        //// UtilityModule.ConditionalComboFillWithDS(ref DDContractor, ds, 0, true, "--Select Employee--");
        //UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 0, true, "--Select Category--");
        TDReworkof.Visible = false;
        //  TDRateNew.Visible = false;
        TDcheckjobseq.Visible = false;
        TxtRateNew.Text = "";
        ChkForIssue_Receive.Checked = false;
        chkjobseqno.Checked = false;
        TDDatestamp.Visible = false;
        TDRecchallanNo.Visible = false;
        txtdatestamp.Text = "";
        TDPacktype.Visible = false;
        TDarticleNo.Visible = false;
        TDBatchNo.Visible = false;
        TDAreaNew.Visible = false;
        TxtAreaNew.Text = "";

        ////****************************
        if (Session["varCompanyNo"].ToString() == "43")
        {     
            string strNew = "";
            strNew = @"Select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                        WHere PNM.ProcessType=1 and PNM.PROCESS_NAME_ID!=" + DDTOProcess.SelectedValue + " order by PROCESS_NAME";

            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strNew);
            UtilityModule.ConditionalComboFillWithDS(ref DDFromProcessForRecChallanNo, ds2, 0, true, "--Plz Select Process--");
        }

        ////***************************


        //**************
        string str = "select isnull(AreaEditable,0) as AreaEditable,isnull(issreconetime,0) as issreconetime  From  Process_Name_master Where Process_name_id=" + DDTOProcess.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //switch (Session["Usertype"].ToString())
            //{
            //    case "1":
            //        TDAreaNew.Visible = true;
            //        break;
            //    default:
            //        break;
            //}
            switch (ds.Tables[0].Rows[0]["AreaEditable"].ToString())
            {
                case "1":
                    TDAreaNew.Visible = true;
                    break;
                default:
                    TDAreaNew.Visible = false;
                    break;
            }
            switch (ds.Tables[0].Rows[0]["issreconetime"].ToString())
            {
                case "1":
                    ChkForIssue_Receive.Checked = true;
                    break;
                default:
                    break;
            }
        }
        //**************
        switch (DDTOProcess.SelectedItem.Text.ToUpper())
        {

            case "REPAIRING":
                TDRateNew.Visible = true;
                TxtRateNew.Text = "";
                break;
            case "WASHING BY WEIGHT":
                switch (Session["varcompanyid"].ToString())
                {
                    case "16":
                    case "44":
                    case "28":
                        TDWeight.Visible = true;
                        break;
                    default:
                        TDWeight.Visible = false;
                        txtWeight.Text = "";
                        break;
                }
                break;
            case "FOR DISPATCHED":
                switch (Session["varcompanyid"].ToString())
                {
                    case "16":
                    case "44":
                    case "28":
                        TDEWayBillNo.Visible = true;
                        TDGSTType.Visible = true;
                        break;
                    default:
                        TDEWayBillNo.Visible = false;
                        TDGSTType.Visible = false;
                        txtEWayBillNo.Text = "";
                        DDGSTType.SelectedIndex = 0;
                        break;
                }
                break;
            case "RE-WORK":
                TDRateNew.Visible = true;
                TxtRateNew.Text = "";
                TDReworkof.Visible = true;
                UtilityModule.ConditionalComboFill(ref DDreworkof, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME not in('Weaving','RE-WORK') order by Process_Name", false, "");
                break;
            case "PACKING":
                switch (Session["varcompanyid"].ToString())
                {
                    case "8":
                        TDDatestamp.Visible = false;
                        break;
                    case "16":
                        TDDatestamp.Visible = false;
                        break;
                    case "44":
                        TDDatestamp.Visible = false;
                        break;
                    case "39":
                        TDDatestamp.Visible = false;
                        TDRecchallanNo.Visible = false;
                        break;
                    case "247":
                        TDDatestamp.Visible = false;
                        TDRecchallanNo.Visible = false;
                        break;
                    default:
                        if (Session["varuserid"].ToString() == "1")
                        {
                            TDcheckjobseq.Visible = true;
                        }
                        TDDatestamp.Visible = true;
                        TDRecchallanNo.Visible = true;
                        TDPacktype.Visible = true;
                        TDarticleNo.Visible = true;
                        TDBatchNo.Visible = true;
                        break;
                }
                break;
            default:
                TDWeight.Visible = false;
                txtWeight.Text = "";
                TDEWayBillNo.Visible = false;
                TDGSTType.Visible = false;
                txtEWayBillNo.Text = "";
                DDGSTType.SelectedIndex = 0;
                break;

        }
        //get Job sequence;

        //
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlcategorycange();
        if (TDitemName.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Item_Name", true, "---Select Item----");
        }
        string Str = @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName 
                    From ITEM_MASTER IM(Nolock) 
                    join CategorySeparate CS(Nolock) on IM.CATEGORY_ID=cs.Categoryid and cs.id=0  and Cs.Categoryid=" + DDCategory.SelectedValue + @" 
                    join Quality Q(Nolock) on IM.ITEM_ID=q.Item_Id  
                    Where Im.mastercompanyid=" + Session["varcompanyid"] + " order by Qualityname";
        if (TDCustomerOrderNo.Visible == true && DDorderNo.SelectedIndex > 0)
        {
            Str = @"Select Distinct VF.QualityId, VF.QualityName + ' [' + VF.Item_Name + ']' QualityName 
                From OrderDetail OD(Nolock)
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" 
                Where OD.OrderId = " + DDorderNo.SelectedValue + @"
                Order BY QualityName";
        }
        UtilityModule.ConditionalComboFill(ref DDQuality, Str, true, "--Plz Select--");
        //fill_gride();
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FindFromProcessId(Convert.ToInt16(DDItemName.SelectedValue), Convert.ToInt16(DDTOProcess.SelectedValue));
        UtilityModule.ConditionalComboFill(ref DDQuality, "Select QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QualityName", true, "--Select Quality--");
        //fill_gride();

    }
    private void ddlcategorycange()
    {
        tdqualityname1.Visible = false;
        tddesign1.Visible = false;
        tdColor1.Visible = false;
        tdShape1.Visible = false;
        tdsize1.Visible = false;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT PARAMETER_ID FROM ITEM_CATEGORY_PARAMETERS where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        tdqualityname1.Visible = true;
                        break;
                    case "2":
                        tddesign1.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDDesign, "Select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order by DesignName", true, "--Select Design--");
                        break;
                    case "3":
                        tdColor1.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDColor, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorName", true, "--Select Color--");
                        break;
                    case "4":
                        tdShape1.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order By ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        tdsize1.Visible = true;
                        break;
                    case "6":
                        tdshadecolor.Visible = true;
                        break;
                }
            }
        }
    }
    protected void BtnStockFill_Click(object sender, EventArgs e)
    {
        ViewState["Function"] = 2;
    }
    protected void BtnAllQty_Click(object sender, EventArgs e)
    {
        ViewState["Function"] = 1;
    }

    private void Fill_Grid_Total()
    {
        int varTotalPcs = 0;
        double varTotalArea = 0;
        double varTotalAmount = 0;
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            varTotalPcs = varTotalPcs + Convert.ToInt32(DGDetail.Rows[i].Cells[6].Text);
            varTotalArea = varTotalArea + Convert.ToDouble(DGDetail.Rows[i].Cells[8].Text);
            varTotalAmount = varTotalAmount + Convert.ToDouble(DGDetail.Rows[i].Cells[9].Text);
        }
        TxtTotalPcs.Text = varTotalPcs.ToString();
        TxtArea.Text = varTotalArea.ToString();
        TxtAmount.Text = varTotalAmount.ToString();
    }
    //private void Fill_StockGride()
    //{
    //    DGStock.DataSource = GetStock();
    //    DGStock.DataBind();
    //}
    private DataSet GetStock()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"Select StockNo,TStockNo,[dbo].[GET_Process_Rate](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate from CarpetNumber where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and IssRecStatus=0 And CompanyId=1 And CurrentProStatus=" + ViewState["FromProcessId"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/NextIssue.aspx");
            //Logs.WriteErrorLog("error");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds;
    }
    //    private void fill_gride()
    //    {
    //        int VarUnit = Convert.ToInt32(DDUnit.SelectedValue);
    //        string strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.ProdSizeMtr Else VF.ProdSizeFt End Description,
    //                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + "=1 Then VF.ProdWidthMtr Else VF.ProdWidthFt End Width,Case When " + VarUnit + @"=1 Then VF.ProdLengthMtr Else VF.ProdLengthFt End Length,
    //                      Round(Case When " + VarUnit + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End,2) Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate                      
    //                      From Process_Receive_MASTER_" + ViewState["FromProcessId"] + " PRM,Process_Receive_Detail_" + ViewState["FromProcessId"] + @" PRD,
    //                      V_FinishedItemDetail VF,Process_Stock_Detail PSD,CarpetNumber CN Where CN.Pack=0 And PRD.QualityType<>3 And PRM.Process_Rec_ID=PRD.Process_Rec_ID And 
    //                      PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And VF.Item_Finished_Id=PRD.Item_Finished_Id And CN.Item_Finished_id=PRD.Item_Finished_Id And 
    //                      PSD.StockNo=CN.StockNo And VF.Category_Id=" + DDCategory.SelectedValue + " And CN.CurrentProStatus=" + ViewState["FromProcessId"] + " And VF.MasterCompanyId=" + Session["varCompanyId"];
    //        string str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.ProdSizeMtr,VF.ProdSizeFt,VF.ProdLengthMtr,VF.ProdLengthFt,VF.ProdWidthMtr,VF.ProdWidthFt,VF.ProdAreaMtr,VF.ProdAreaFt";
    //        if (Session["varCompanyId"].ToString() == "5")
    //        {
    //            strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.SizeMtr Else VF.SizeFt End Description,
    //                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + "=1 Then VF.WidthMtr Else VF.WidthFt End Width,Case When " + VarUnit + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,
    //                      Round(Case When " + VarUnit + @"=1 Then VF.AreaMtr Else VF.AreaFt End,2) Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate                      
    //                      From Process_Receive_MASTER_" + ViewState["FromProcessId"] + " PRM,Process_Receive_Detail_" + ViewState["FromProcessId"] + @" PRD,
    //                      V_FinishedItemDetail VF,Process_Stock_Detail PSD,CarpetNumber CN Where CN.Pack=0 And PRD.QualityType<>3 And PRM.Process_Rec_ID=PRD.Process_Rec_ID And 
    //                      PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And VF.Item_Finished_Id=PRD.Item_Finished_Id And CN.Item_Finished_id=PRD.Item_Finished_Id And
    //                      PSD.StockNo=CN.StockNo And VF.Category_Id=" + DDCategory.SelectedValue + " And CN.CurrentProStatus=" + ViewState["FromProcessId"] + " And ToProcessId=" + ViewState["FromProcessId"] + " And PRM.CompanyId=1 And VF.MasterCompanyId=" + Session["varCompanyId"];
    //            str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.SizeMtr,VF.SizeFt,VF.LengthMtr,VF.LengthFt,VF.WidthMtr,VF.WidthFt,VF.AreaMtr,VF.AreaFt";
    //        }
    //        if (DDItemName.SelectedIndex > 0)
    //        {
    //            strsql = strsql + " And VF.ITEM_ID=" + DDItemName.SelectedValue;
    //        }
    //        if (DDQuality.SelectedIndex > 0 && tdqualityname1.Visible == true)
    //        {
    //            strsql = strsql + " And VF.QualityId=" + DDQuality.SelectedValue;
    //        }
    //        if (DDDesign.SelectedIndex > 0 && tddesign1.Visible == true)
    //        {
    //            strsql = strsql + " And VF.DesignId=" + DDDesign.SelectedValue;
    //        }
    //        if (DDColor.SelectedIndex > 0 && tdColor1.Visible == true)
    //        {
    //            strsql = strsql + " And VF.ColorId=" + DDColor.SelectedValue;
    //        }
    //        if (DDShape.SelectedIndex > 0 && tdShape1.Visible == true)
    //        {
    //            strsql = strsql + " And VF.ShapeId=" + DDShape.SelectedValue;
    //        }
    //        if (DDSize.SelectedIndex > 0 && tdsize1.Visible == true)
    //        {
    //            strsql = strsql + " And VF.SizeId=" + DDItemName.SelectedValue;
    //        }
    //        if (ddlshade.SelectedIndex > 0 && tdshadecolor.Visible == true)
    //        {
    //            strsql = strsql + " And VF.ShadecolorId=" + ddlshade.SelectedValue;
    //        }
    //        strsql = strsql + str;

    //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
    //        DGItemDetail.DataSource = ds;
    //        DGItemDetail.DataBind();
    //    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //  e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fillsize();
    }

    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string StrSize = "vf.SizeMtr + ' ' + vf.shapename";
        string str = "";
        if (Session["varcompanyId"].ToString() == "44")
        {
            str = @"select top(1) OrderUnitId From OrderDetail Where OrderId=" + DDorderNo.SelectedValue;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["orderunitid"].ToString() == "1")
                {
                    StrSize = "vf.SizeMtr + ' ' + vf.shapename";
                }
                else if (ds.Tables[0].Rows[0]["orderunitid"].ToString() == "2")
                {
                    StrSize = "vf.Sizeft + ' ' + vf.shapename";
                }
                else if (ds.Tables[0].Rows[0]["orderunitid"].ToString() == "6")
                {

                    StrSize = "vf.SizeInch + ' ' + vf.shapename";
                }
                else
                {
                    StrSize = "vf.Sizeft + ' ' + vf.shapename";
                }

            }
        }
        if (Session["varcompanyId"].ToString() == "16" && DDTOProcess.SelectedValue == "12")
        {
            StrSize = "vf.Sizeft + ' ' + vf.shapename";

        }

        if (Session["varcompanyId"].ToString() == "38")
        {
            StrSize = "vf.Sizeft + ' ' + vf.shapename";

        }

        str = @"select Distinct vf.sizeid, " + StrSize + @" size From V_FinishedItemDetail vf 
            where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid=" + DDColor.SelectedValue + @" and 
            vf.sizeid<>0 order by Size";

        if (TDCustomerOrderNo.Visible == true && DDorderNo.SelectedIndex > 0)
        {
            str = @"Select Distinct vf.sizeid, " + StrSize + @" size 
                From OrderDetail OD(Nolock)
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.CATEGORY_ID = " + DDCategory.SelectedValue + @" And 
                    vf.QualityId=" + DDQuality.SelectedValue + "  and vf.designid=" + DDDesign.SelectedValue + " And vf.colorid=" + DDColor.SelectedValue + @" 
                Where OD.OrderId = " + DDorderNo.SelectedValue + @"
                Order BY Size";
        }

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Plz Select--");

        Focus = "DDSize";
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
        // fill_gride();
    }
    private void fillsize()
    {
        if (DDUnit.SelectedValue == "2")
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "Select Sizeid,SizeFt from Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By SizeFt", true, "--Select Size--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDSize, "Select Sizeid,SizeMtr from Size where ShapeId=" + DDShape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By SizeFt", true, "--Select Size--");
        }
    }
    private void fill_DetailGride()
    {
        DGDetail.DataSource = GetSaveDetail();
        DGDetail.DataBind();
    }
    private DataSet GetSaveDetail()
    {
        DataSet DS = null;
        string sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
                        Length + 'x' + Width Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) StockNo,
                        isnull(PD.Bonus,0) as Bonus,isnull(PD.BonusAmt,0) as BonusAmt  
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id desc";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
            LblErrorMessage.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/NextIssue.aspx");
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    protected void DDQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_gride();
        string str = "select Distinct vf.designId,vf.designName From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + @" and vf.designid<>0 order by vf.designName";

        if (TDCustomerOrderNo.Visible == true && DDorderNo.SelectedIndex > 0)
        {
            str = @"Select Distinct vf.designId,vf.designName 
                From OrderDetail OD(Nolock)
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.CATEGORY_ID = " + DDCategory.SelectedValue + " And vf.QualityId=" + DDQuality.SelectedValue + @" 
                Where OD.OrderId = " + DDorderNo.SelectedValue + @"
                Order BY vf.designName";
        }

        str = str + @" Select Distinct IsNull((select Top 1 processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Plz Select--");
        //***Get From Processid
        if (ds.Tables[1].Rows.Count > 0)
        {
            hnfromprocessid.Value = ds.Tables[1].Rows[0]["FromProcessid"].ToString();
        }
        else
        {
            hnfromprocessid.Value = "0";
        }
        Focus = "DDDesign";
    }
    protected void DDSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_gride();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string str = "";
        DataSet ds;
        try
        {
            //            str = @"Select isnull(PIM.ChallanNo,'') as ChallanNo, Ci.CompanyId,BM.BranchName CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, '' CompAddr3,BM.PhoneNo CompTel,CI.CompFax,CI.GSTNo
            //                        ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PIM.issueorderid
            //                        ,PIM.assigndate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
            //                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
            //                        PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
            //                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction,PID.Item_Finished_Id,
            //                        case when " + Session["varcompanyId"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '''' end as FolioNo,
            //                        isnull(PID.JobIssueWeight,0) as JobIssueWeight, PID.Amount 
            //                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM 
            //                        Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId
            //                        JOIN BranchMaster BM ON BM.ID = PIM.BranchID 
            //                        inner join CompanyInfo CI on BM.Companyid=CI.CompanyId
            //                        inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
            //                        cross apply(select * From dbo.F_GetJobIssueEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.issueorderid)) EI
            //                        Where PIM.issueorderid=" + ViewState["IssueOrderid"] + " order by Issue_Detail_id";

            str = @"Select isnull(PIM.ChallanNo,'') as ChallanNo, Ci.CompanyId,BM.BranchName CompanyName, BM.BranchAddress CompAddr1, '' CompAddr2, '' CompAddr3,BM.PhoneNo CompTel,CI.CompFax,CI.GSTNo
                        ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PIM.issueorderid
                        ,PIM.assigndate,PID.reqbydate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                        PID.Width+' x ' +PID.Length as Size,PID.qty,----PID.Qty*PID.area as Area,
                        Case When " + Session["varcompanyId"].ToString() + @"=43 Then Case When (Select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @")='Binding' Then cast(PID.Qty*PID.area as int) Else PID.Qty*PID.area End
						Else PID.Qty*PID.area End as Area,
                        PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction,PIM.Remarks,PID.Item_Finished_Id,
                        case when " + Session["varcompanyId"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '' end as FolioNo,
                        isnull(PID.JobIssueWeight,0) as JobIssueWeight, PID.Amount,PID.GSTTYPE,
                        Case When PID.GSTTYPE=1 Then isnull(round(PID.AMOUNT*PID.CGST/100+  PID.AMOUNT*PID.SGST/100,3),0) else 0 end SGST,
                        Case When PID.GSTTYPE=2 Then isnull(round(PID.AMOUNT*PID.IGST/100,2),0) else 0 end IGST,
                        isnull(VF.HSNCode,'') as HSNCode,isnull(PIM.EWayBillNo,'') as EWayBillNo,
                       (Select Distinct OM.CustomerOrderNo+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID1(NoLock) JOIN  OrderMaster OM(NoLock) ON PID1.OrderID=OM.OrderID 
                            Where PIM.IssueOrderID=PID1.IssueOrderId and PID.ITEM_FINISHED_ID=PID1.Item_Finished_Id For XML PATH('')) as CustomerOrderNo,
                        (Select Distinct CustIn.CustomerCode+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID2(NoLock) JOIN  OrderMaster OM2(NoLock) ON OM2.OrderID = PID2.OrderID 
                            JOIN CustomerInfo CustIn(NoLock) ON OM2.CustomerId=CustIn.CustomerId  Where PID2.IssueOrderId=PIM.IssueOrderID For XML PATH('')) as CustomerCode
                        ,isnull(NU.UserName,'') as UserName,isnull(PID.Bonus,0) as Bonus,isnull(PID.BonusAmt,0) as BonusAmt, VF.MasterCompanyID  
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock) 
                        Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID(NoLock) on PIM.IssueOrderId=PID.IssueOrderId
                        JOIN BranchMaster BM(NoLock) ON BM.ID = PIM.BranchID 
                        inner join CompanyInfo CI(NoLock) on BM.Companyid=CI.CompanyId
                        inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                        cross apply(select * From dbo.F_GetJobIssueEmployeeDetail(" + DDTOProcess.SelectedValue + @",PIM.issueorderid)) EI
                        JOIN NewUserDetail NU(NoLock) ON PIM.UserId=NU.UserId
                        Where PIM.issueorderid=" + ViewState["IssueOrderid"] + " order by Issue_Detail_id";

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(str, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    if (ChkForWithoutRate.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryWithoutRate.rpt";
                    }
                    else
                    {
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "27":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryForAntique.rpt";
                                break;
                            case "30":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummarySamara.rpt";
                                break;
                            case "16":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary_barcode.rpt";
                                break;
                            case "28":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummaryWithHSN.rpt";
                                break;
                            case "44":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary_agni.rpt";
                                break;
                            default:
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary.rpt";
                                break;
                        }
                        //Session["rptFileName"] = "~\\Reports\\RptNextissueNewSummary.rpt";
                    }
                }
                else
                {
                    if (ChkForWithoutRate.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptNextissueNew2WithoutRate.rpt";
                    }
                    else
                    {
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "16":
                            case "28":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2_barcode.rpt";
                                break;
                            case "42":
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2_VikramMirzapur.rpt";
                                break;
                            default:
                                Session["rptFileName"] = "~\\Reports\\RptNextissueNew2.rpt";
                                break;

                        }
                        // Session["rptFileName"] = "~\\Reports\\RptNextissueNew2.rpt";
                    }
                }

                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextissueNew2.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
    }
    private void Report()
    {
        string qry = @"SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_NextProcessIssueReport.QualityName,V_NextProcessIssueReport.designName,V_NextProcessIssueReport.ColorName,V_NextProcessIssueReport.ShadeColorName,V_NextProcessIssueReport.ShapeName,V_NextProcessIssueReport.Instruction,V_NextProcessIssueReport.Qty,V_NextProcessIssueReport.Rate,V_NextProcessIssueReport.Areamtr as area,V_NextProcessIssueReport.IssueOrderId,V_NextProcessIssueReport.AssignDate,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessIssueReport.UnitId,V_NextProcessIssueReport.Length,V_NextProcessIssueReport.Width,V_NextProcessIssueReport.TStockNo,CustomerOrderNo," + Session["VarcompanyNo"] + @" As CompanyNo,CustomerCode
        FROM   PROCESS_NAME_MASTER 
        JOIN V_NextProcessIssueReport ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessIssueReport.PROCESSID 
        JOIN V_Companyinfo ON V_NextProcessIssueReport.Companyid=V_Companyinfo.CompanyId 
        JOIN V_EmployeeInfo ON V_NextProcessIssueReport.Empid=V_EmployeeInfo.EmpId
        where V_NextProcessIssueReport.IssueOrderId= " + ViewState["IssueOrderid"] + " And PROCESS_NAME_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And V_Companyinfo.CompanyId=" + DDCompanyName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\NextProcessIssueNEW.rpt";
            //Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\NextProcessIssueNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    public void OpenNewWidow(string url)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
    }
    protected void BtnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }

    //protected void DGStock_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    int index = Convert.ToInt32(e.CommandArgument);
    //    GridViewRow row = DGStock.Rows[index];
    //    string carpetNo = row.Cells[0].Text;
    //    string Rate = ((TextBox)row.Cells[1].FindControl("TxtCarpetRate")).Text;
    //    if (Rate == "" || Rate == "0")
    //    {
    //        LblErrorMessage.Visible = true;
    //        LblErrorMessage.Text = "Pls Enter Rate....";
    //    }
    //    else
    //    {
    //        Issue_CarpetWise(carpetNo, Rate);
    //    }
    //}

    private void Issue_CarpetWise(string carpetNo, string Rate, DataSet DS)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            double Length = 0;
            double Width = 0;
            double Area = 0;

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                if (DS.Tables[0].Rows.Count > 0)
                {
                    int VarStockNo = Convert.ToInt32(DS.Tables[0].Rows[0]["StockNo"]);
                    if (ChkForArea.Checked == true && TDAreaNew.Visible == true)
                    {
                        Area = Convert.ToDouble(TxtAreaNew.Text == "" ? "0" : TxtAreaNew.Text);
                    }
                    if (ChkForRate.Checked == true)
                    {
                        Rate = TxtRateNew.Text == "" ? "0" : TxtRateNew.Text;
                    }
                    SqlParameter[] _arrpara = new SqlParameter[50];

                    _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.SmallDateTime);
                    _arrpara[3] = new SqlParameter("@Status", SqlDbType.NVarChar);
                    _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
                    _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar);
                    _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar);
                    _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

                    _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
                    _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                    _arrpara[11] = new SqlParameter("@Length", SqlDbType.NVarChar);
                    _arrpara[12] = new SqlParameter("@Width", SqlDbType.NVarChar);
                    _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
                    _arrpara[14] = new SqlParameter("@Rate", SqlDbType.Float);
                    _arrpara[15] = new SqlParameter("@Amount", SqlDbType.Float);
                    _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Int);
                    _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.SmallDateTime);
                    _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Int);

                    _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
                    _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.Float);
                    _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
                    _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);

                    _arrpara[23] = new SqlParameter("@StockNo", SqlDbType.Int);
                    _arrpara[24] = new SqlParameter("@FromProcessId", SqlDbType.Int);
                    _arrpara[25] = new SqlParameter("@ToProcessId", SqlDbType.Int);
                    _arrpara[26] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                    _arrpara[27] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
                    _arrpara[28] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
                    _arrpara[29] = new SqlParameter("@VarTypeId", SqlDbType.Int);
                    _arrpara[30] = new SqlParameter("@Iss_RecFlag", SqlDbType.TinyInt);
                    _arrpara[31] = new SqlParameter("@Weight", SqlDbType.Float);
                    _arrpara[32] = new SqlParameter("@process_rec_id", SqlDbType.Int);
                    _arrpara[33] = new SqlParameter("@StrEmpId", SqlDbType.VarChar, 500);
                    _arrpara[34] = new SqlParameter("@UnitsId", SqlDbType.Int);
                    _arrpara[35] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    ////For Packing job only
                    _arrpara[36] = new SqlParameter("@Date_Stamp", SqlDbType.VarChar, 100);
                    _arrpara[37] = new SqlParameter("@Recchallanno", SqlDbType.VarChar, 100);
                    _arrpara[38] = new SqlParameter("@Packingtypeid", SqlDbType.Int);
                    _arrpara[39] = new SqlParameter("@Articleno", SqlDbType.VarChar, 50);
                    _arrpara[40] = new SqlParameter("@BatchNo", SqlDbType.VarChar, 100);
                    _arrpara[41] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                    _arrpara[42] = new SqlParameter("@Saveflag", SqlDbType.Int);
                    _arrpara[43] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
                    _arrpara[44] = new SqlParameter("@JobIssueWeight", SqlDbType.Float);
                    _arrpara[45] = new SqlParameter("@BranchID", SqlDbType.Float);
                    ////For Dispatched job only
                    _arrpara[46] = new SqlParameter("@GSTType", SqlDbType.Int);
                    _arrpara[47] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 15);
                    _arrpara[48] = new SqlParameter("@ChkForExportSize", SqlDbType.Int);
                    _arrpara[49] = new SqlParameter("@DyingLotNo", SqlDbType.VarChar, 100);

                    //
                    //  int num = 1;
                    #region "Comment on '06-Feb-2014"

                    //if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || (DDTOProcess.SelectedValue != (ViewState["processId"]).ToString()))
                    //{
                    //    int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                    //    ViewState["IssueOrderid"] = a;
                    //    TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();
                    //    num = 1;
                    //}
                    //else
                    //{
                    //    num = 0;
                    //}

                    #endregion
                    if (ViewState["IssueOrderid"].ToString() == "0" || ViewState["IssueOrderid"] == null)
                    {
                        ViewState["IssueOrderid"] = 0;
                        ViewState["@process_rec_id"] = 0;
                    }
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[0].Value = (ViewState["IssueOrderid"]);
                    _arrpara[1].Value = DDContractor.SelectedIndex <= 0 ? "0" : DDContractor.SelectedValue;
                    _arrpara[2].Value = TxtIssueDate.Text;
                    _arrpara[3].Value = "Pending";
                    _arrpara[4].Value = DDUnit.SelectedValue;
                    _arrpara[5].Value = Session["varuserid"];
                    _arrpara[6].Value = TDReworkof.Visible == true ? "Rework-of " + DDreworkof.SelectedItem.Text : TxtRemarks.Text.ToUpper();
                    _arrpara[7].Value = TxtInsructions.Text.ToUpper();
                    _arrpara[8].Value = DS.Tables[0].Rows[0]["Companyid"];//Fix CompanyId
                    #region "COmment on 06-Feb-2014"
                    //_arrpara[9].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Issue_Detail_Id),0)+1 from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue);
                    #endregion
                    _arrpara[9].Direction = ParameterDirection.InputOutput;
                    _arrpara[9].Value = 0;  //IssueDetailId
                    _arrpara[10].Value = DS.Tables[0].Rows[0]["Item_Finished_id"];
                    ViewState["Item_Finished_Id"] = DS.Tables[0].Rows[0]["Item_Finished_id"];
                    _arrpara[11].Value = Length;
                    _arrpara[12].Value = Width;
                    _arrpara[13].Value = Area;
                    _arrpara[14].Value = Rate == "" ? "0" : Rate;
                    if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
                    {
                        _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Area) * Convert.ToDouble(Rate)));
                    }
                    if (DDcaltype.SelectedValue == "1")
                    {
                        _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Rate)));
                    }
                    _arrpara[16].Value = 1;
                    _arrpara[17].Value = TxtReqDate.Text;
                    _arrpara[18].Value = 1;
                    _arrpara[19].Value = "0";
                    _arrpara[20].Value = "0";
                    _arrpara[21].Value = DS.Tables[0].Rows[0]["OrderId"];
                    _arrpara[22].Value = DDcaltype.SelectedValue;

                    _arrpara[23].Value = VarStockNo;
                    _arrpara[24].Value = ViewState["FromProcessId"];
                    _arrpara[25].Value = DDTOProcess.SelectedValue;
                    _arrpara[26].Value = TxtIssueDate.Text;
                    _arrpara[27].Value = TxtReqDate.Text;
                    _arrpara[28].Value = 0;
                    _arrpara[29].Value = 1;
                    _arrpara[30].Value = ChkForIssue_Receive.Checked == true ? 1 : 0; //1 For Issue Receive 
                    _arrpara[31].Value = DS.Tables[0].Rows[0]["Weight"];
                    _arrpara[32].Direction = ParameterDirection.InputOutput;
                    _arrpara[32].Value = ViewState["@process_rec_id"];
                    ViewState["processId"] = Convert.ToInt32(DDTOProcess.SelectedValue);

                    //Find EmployeeId
                    string StrEmpid = "";
                    for (int i = 0; i < lstWeaverName.Items.Count; i++)
                    {
                        if (StrEmpid == "")
                        {
                            StrEmpid = lstWeaverName.Items[i].Value;
                        }
                        else
                        {
                            StrEmpid = StrEmpid + "," + lstWeaverName.Items[i].Value;
                        }
                    }
                    //Check Employee Entry
                    if (StrEmpid == "")
                    {
                        LblErrorMessage.Text = "Plz Enter Weaver ID No...";
                        Tran.Commit();
                        return;
                    }
                    _arrpara[33].Value = StrEmpid;
                    _arrpara[34].Value = ddUnits.SelectedValue;
                    _arrpara[35].Direction = ParameterDirection.Output;
                    _arrpara[36].Value = TDDatestamp.Visible == true ? txtdatestamp.Text : "";
                    _arrpara[37].Direction = ParameterDirection.Output;
                    _arrpara[38].Value = TDPacktype.Visible == false ? "0" : DDPacktype.SelectedValue;
                    _arrpara[39].Value = TDarticleNo.Visible == false ? "" : DDarticleno.SelectedValue;
                    _arrpara[40].Value = TDBatchNo.Visible == false ? "" : DDbatchNo.SelectedItem.Text;
                    _arrpara[41].Value = Session["varcompanyid"];
                    _arrpara[42].Direction = ParameterDirection.Output;
                    _arrpara[43].Value = TxtChallanNO.Text;
                    _arrpara[43].Direction = ParameterDirection.InputOutput;

                    if (Session["VarCompanyNo"].ToString() == "16" || Session["VarcompanyNo"].ToString() == "28")
                    {
                        if (DDTOProcess.SelectedItem.Text.ToUpper() == "WASHING BY WEIGHT")
                        {
                            _arrpara[44].Value = WashingByWeight;
                        }
                        else
                        {
                            _arrpara[44].Value = TDWeight.Visible == false ? "0" : txtWeight.Text == "" ? "0" : txtWeight.Text;
                        }
                    }
                    else
                    {
                        _arrpara[44].Value = TDWeight.Visible == false ? "0" : txtWeight.Text == "" ? "0" : txtWeight.Text;
                    }
                    _arrpara[45].Value = DDBranchName.SelectedValue;

                    if (Session["VarCompanyNo"].ToString() == "16" || Session["VarcompanyNo"].ToString() == "28")
                    {
                        if (DDTOProcess.SelectedItem.Text.ToUpper() == "FOR DISPATCHED")
                        {
                            _arrpara[46].Value = DDGSTType.SelectedIndex > 0 ? DDGSTType.SelectedValue : "0";
                            _arrpara[47].Value = txtEWayBillNo.Text;
                        }
                        else
                        {
                            _arrpara[46].Value = TDGSTType.Visible == false ? "0" : DDGSTType.SelectedIndex > 0 ? DDGSTType.SelectedValue : "0";
                            _arrpara[47].Value = TDEWayBillNo.Visible == false ? "0" : txtEWayBillNo.Text == "" ? "0" : txtEWayBillNo.Text;
                        }
                    }
                    else
                    {
                        _arrpara[46].Value = TDGSTType.Visible == false ? "0" : DDGSTType.SelectedIndex > 0 ? DDGSTType.SelectedValue : "0";
                        _arrpara[47].Value = TDEWayBillNo.Visible == false ? "0" : txtEWayBillNo.Text == "" ? "0" : txtEWayBillNo.Text;
                    }
                    _arrpara[48].Value = TDExportSize.Visible == false ? "0" : ChkForExportSize.Checked == true ? "1" : "0";
                    _arrpara[49].Value = TdDyingLotNo.Visible == false ? "" : TxtDyingLotNo.Text;

                    //Insert into Process_Issue_Master And Process_Issue_Detail 
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessIssueForOther", _arrpara);

                    ViewState["IssueOrderid"] = _arrpara[0].Value;
                    //TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();
                    TxtChallanNO.Text = _arrpara[43].Value.ToString();
                    ViewState["@process_rec_id"] = _arrpara[32].Value;
                    //TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();
                    txtrecchallanNo.Text = _arrpara[37].Value.ToString();
                    TxtIssueDate.Enabled = false;
                    if (_arrpara[35].Value.ToString() != "")
                    {
                        LblErrorMessage.Text = _arrpara[35].Value.ToString();
                        LblErrorMessage.Visible = true;
                        Tran.Rollback();
                    }
                    else
                    {
                        if (TDGSTType.Visible == true)
                        {
                            DDGSTType.Enabled = false;
                        }

                        TxtStockNo.Text = "";
                        TxtStockNo.Focus();
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Data Successfully Saved.......";
                        if (_arrpara[42].Value.ToString() == "1") //Data Not Saved in case of packing
                        {
                            Tran.Rollback();
                        }
                        else
                        {
                            Tran.Commit();
                        }
                    }
                    BtnPreview.Enabled = true;

                    FillGridDetails();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/NextIssue.aspx");
                Tran.Rollback();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void FillGridDetails()
    {
        SqlParameter[] _array = new SqlParameter[10];
        _array[0] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
        _array[1] = new SqlParameter("@FromProcessId", SqlDbType.Int);
        _array[2] = new SqlParameter("@ToProcessId", SqlDbType.Int);
        _array[3] = new SqlParameter("@Finishedid", SqlDbType.Int);
        _array[4] = new SqlParameter("@CompanyId", SqlDbType.Int);
        _array[5] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        _array[6] = new SqlParameter("@RowsCount", SqlDbType.Int);

        _array[0].Value = ViewState["IssueOrderid"];
        _array[1].Value = ViewState["FromProcessId"];
        _array[2].Value = DDTOProcess.SelectedValue;
        _array[3].Value = ViewState["Item_Finished_Id"];
        _array[4].Value = DDCompanyName.SelectedValue;
        _array[5].Value = Session["varcompanyNo"].ToString();
        //_array[6].Value = DGStock.Rows.Count;
        _array[6].Value = 0;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillGridDetail_NextIssueForOther", _array);

        DGDetail.DataSource = ds.Tables[0];
        DGDetail.DataBind();
        //if (DGStock.Rows.Count > 0)
        //{
        //    DGStock.DataSource = ds.Tables[1];
        //    DGStock.DataBind();
        //}
        //TxtTotalPcs.Text = ds.Tables[1].Rows[0]["Qty"].ToString();
        //TxtArea.Text = ds.Tables[1].Rows[0]["Area"].ToString();
        //TxtAmount.Text = ds.Tables[1].Rows[0]["Amount"].ToString();

        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
            if (Session["VarCompanyNo"].ToString() == "43")
            {
                TxtArea.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("Sum(Area)", "")), 2).ToString();
                TxtAmount.Text = Math.Round(Convert.ToDouble(ds.Tables[0].Compute("Sum(Amount)", "")), 2).ToString();
            }
            else
            {
                TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
                TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();
            }
        }
        else
        {
            TxtTotalPcs.Text = "";
            TxtArea.Text = "";
            TxtAmount.Text = "";
        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (DGDetail.Columns[i].HeaderText == "Bonus")
                {
                    if (Convert.ToInt32(Session["varcompanyId"]) == 42)
                    {
                        DGDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
            long stockno = 0;
            Label lbltstockno = (Label)DGDetail.Rows[e.RowIndex].FindControl("lbltstockno");
            string str = "select stockno From Carpetnumber Where TstockNo in('" + lbltstockno.Text + "')";
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);

            if (ds.Tables[0].Rows.Count > 0)
            {
                stockno = Convert.ToInt64(ds.Tables[0].Rows[0]["stockno"]);
            }

            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@issueorderid", ViewState["IssueOrderid"]);
            param[1] = new SqlParameter("@IssueDetailid", VarProcess_Issue_Detail_Id);
            param[2] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
            param[3] = new SqlParameter("@StockNo", stockno);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@mastercompanyId", Session["varcompanyid"]);
            param[7] = new SqlParameter("@issrecstatus", ChkForIssue_Receive.Checked == true ? 1 : 0);
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETENEXTISSUEFOROTHER", param);
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = param[4].Value.ToString();
            FillGridDetails();

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where ProductCode Like  '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId;
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
    protected void DDContractor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;
    }
    

    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "27" && Session["usertype"].ToString() != "1")
        {
            btnclickflag = "";
            btnclickflag = "BtnSaveScanCarpetNumber";
            Popup(true);
            txtpwd.Focus();
        }
        else
        {
            SaveScanCarpetNumber();
        }
    }
    protected void SaveScanCarpetNumber()
    {
        saveDetail();
        disablecontrols();
        Focus = "TxtStockNo";
    }
    protected void saveDetail()
    {
        LblErrorMessage.Text = "";

        SqlParameter[] param = new SqlParameter[13];
        param[0] = new SqlParameter("@TStockNo", TxtStockNo.Text);
        param[1] = new SqlParameter("@ProcessId", DDTOProcess.SelectedValue);
        param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        param[2].Direction = ParameterDirection.Output;
        param[3] = new SqlParameter("@FromProcessid", SqlDbType.Int);        
        param[3].Direction = ParameterDirection.Output;
        param[4] = new SqlParameter("@notmaintainjobseq", chkjobseqno.Checked == true ? "1" : "0");
        param[5] = new SqlParameter("@unitid", DDUnit.SelectedValue);
        param[6] = new SqlParameter("@Item_Name", SqlDbType.VarChar, 50);
        param[6].Direction = ParameterDirection.Output;
        param[7] = new SqlParameter("@Quality", SqlDbType.VarChar, 50);
        param[7].Direction = ParameterDirection.Output;
        param[8] = new SqlParameter("@Design", SqlDbType.VarChar, 50);
        param[8].Direction = ParameterDirection.Output;
        param[9] = new SqlParameter("@Color", SqlDbType.VarChar, 50);
        param[9].Direction = ParameterDirection.Output;
        param[10] = new SqlParameter("@Shape", SqlDbType.VarChar, 50);
        param[10].Direction = ParameterDirection.Output;
        param[11] = new SqlParameter("@Size", SqlDbType.VarChar, 50);
        param[11].Direction = ParameterDirection.Output;
        param[12] = new SqlParameter("@CompanyId", DDCompanyName.SelectedValue);
        //Execute p-roc 
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_CheckStockNoForIssueOther", param);
        TRItemdetail.Visible = true;
        if (TRItemdetail.Visible == true)
        {
            txtitem.Text = param[6].Value.ToString();
            txtQuality.Text = param[7].Value.ToString();
            txtDesign.Text = param[8].Value.ToString();
            txtcolor.Text = param[9].Value.ToString();
            txtshape.Text = param[10].Value.ToString();
            txtsize.Text = param[11].Value.ToString();
        }
        LblErrorMessage.Text = param[2].Value.ToString();
        ViewState["FromProcessId"] = param[3].Value.ToString();

        if (LblErrorMessage.Text == "")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Issue_CarpetWise(TxtStockNo.Text, "0", ds);
            }
            //if (TDgetstockdetail.Visible==true)
            //{
            //    Fillstockdetail();
            //}
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = param[2].Value.ToString();
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDTOProcess) == false)
        {
            goto a;
        }
        if (TDDatestamp.Visible == true)
        {
            if (UtilityModule.VALIDTEXTBOX(txtdatestamp) == false)
            {
                goto a;
            }
        }
        //if (tdqualityname1.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (tddesign1.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (tdColor1.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
        //    {
        //        goto a;
        //    }
        //}
        //if (tdsize1.Visible == true)
        //{
        //    if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
        //    {
        //        goto a;
        //    }
        //}
        if (TDPacktype.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDPacktype) == false)
            {
                goto a;
            }
        }
        if (TDarticleNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDarticleno) == false)
            {
                goto a;
            }
        }
        if (TDBatchNo.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDbatchNo) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtIssueDate) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtIssueDate) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DGItemDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGStock_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_gride();
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"].ToString() != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select UnitId from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"]);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (DDUnit.SelectedValue != Ds.Tables[0].Rows[0]["UnitId"].ToString())
                {
                    DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Folio no has only one unitid !');", true);
                }
            }
        }
        fillsize();
    }
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"].ToString() != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Caltype from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"]);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (DDcaltype.SelectedValue != Ds.Tables[0].Rows[0]["CalType"].ToString())
                {
                    DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["CalType"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Folio no has only one caltype !');", true);
                }
            }
        }
    }


    protected void ChkForIssue_Receive_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        FillGridDetails();
    }

    protected void btnDeleteName_Click(object sender, EventArgs e)
    {
        lstWeaverName.Items.Remove(lstWeaverName.SelectedItem);
    }
    protected void FindFromProcessId(int ItemId, int ProcessId)
    {
        SqlParameter[] array = new SqlParameter[5];
        array[0] = new SqlParameter("@Itemid", SqlDbType.Int);
        array[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
        array[2] = new SqlParameter("@FromProcessId", SqlDbType.Int);

        array[0].Value = ItemId;
        array[1].Value = ProcessId;
        array[2].Direction = ParameterDirection.Output;

        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getFromProcessId", array);

        ViewState["FromProcessId"] = array[2].Value.ToString();

    }
    protected void ddUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"].ToString() != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Units from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"]);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (ddUnits.SelectedValue != Ds.Tables[0].Rows[0]["Units"].ToString())
                {
                    ddUnits.SelectedValue = Ds.Tables[0].Rows[0]["Units"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Folio no has only one Units !');", true);
                }
            }
        }
    }

    protected void btngetstock_Click(object sender, EventArgs e)
    {
        if (hnfromprocessid.Value == "0" && variable.VarFinishingIssueSeqWise == "1")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Last Job is not defined for this Quality.');", true);
        }

        Fillstockdetail();
    }
    protected void Fillstockdetail()
    {
        DataSet ds = new DataSet();

        if (MasterCompanyId == 16 && MasterCompanyId == 28)
        {
            if (DDorderNo.Items.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Please select customer order no');", true);
                return;
            }
            if (DDorderNo.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Please select order no');", true);
                return;
            }
        }

        string str = "";
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And VF.Designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            str = str + " And VF.Colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            str = str + " And VF.Sizeid=" + DDSize.SelectedValue;
        }
        if (DDorderNo.SelectedIndex > 0)
        {
            str = str + " And CN.OrderID=" + DDorderNo.SelectedValue;
        }

        //SqlParameter[] param = new SqlParameter[9];
        //param[0] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
        //param[1] = new SqlParameter("@FromProcess", hnfromprocessid.Value);
        //param[2] = new SqlParameter("@ToProcess", DDTOProcess.SelectedValue);
        //param[3] = new SqlParameter("@FromDate", TxtDataFromDate.Text);
        //param[4] = new SqlParameter("@Where", str);
        //param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //param[5].Direction = ParameterDirection.Output;
        //param[6] = new SqlParameter("@userid", Session["varuserid"]);
        //param[7] = new SqlParameter("@MastercompanyId", Session["varcompanyid"]);
        //param[8] = new SqlParameter("@ToDate", txtgetdataupto.Text);

        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETSTOCKDETAIL_FORFINISHER", param);

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("PRO_GETSTOCKDETAIL_FORFINISHER", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
        cmd.Parameters.AddWithValue("@FromProcess", hnfromprocessid.Value);
        cmd.Parameters.AddWithValue("@ToProcess", DDTOProcess.SelectedValue);
        cmd.Parameters.AddWithValue("@FromDate", TxtDataFromDate.Text);
        cmd.Parameters.AddWithValue("@Where", str);
        cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
        cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
        cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
        cmd.Parameters.AddWithValue("@MastercompanyId", Session["varcompanyid"]);
        cmd.Parameters.AddWithValue("@ToDate", txtgetdataupto.Text);
        cmd.Parameters.AddWithValue("@QualityID", DDQuality.SelectedValue);
        cmd.Parameters.AddWithValue("@DesignID", DDDesign.SelectedValue);
        cmd.Parameters.AddWithValue("@OrderID", DDorderNo.SelectedValue);
        cmd.Parameters.AddWithValue("@JobWiseRecChallanNo", DDRecChallanNoJobWise.SelectedValue);
        cmd.Parameters.AddWithValue("@FromProcessForRecChallanNo", DDFromProcessForRecChallanNo.SelectedValue);

        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();
        //**********

        DGStockDetail.DataSource = ds.Tables[0];
        DGStockDetail.DataBind();
        txttotalpcsgrid.Text = ds.Tables[0].Compute("count(Stockno)", "").ToString();
    }
    protected void DGStockDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGStockDetail.PageIndex = e.NewPageIndex;
        Fillstockdetail();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Distinct vf.ColorId,vf.ColorName From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid<>0 order by vf.colorname";
        if (TDCustomerOrderNo.Visible == true && DDorderNo.SelectedIndex > 0)
        {
            str = @"Select Distinct vf.ColorId,vf.ColorName 
                From OrderDetail OD(Nolock)
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id And VF.CATEGORY_ID = " + DDCategory.SelectedValue + " And vf.QualityId=" + DDQuality.SelectedValue + "  and vf.designid=" + DDDesign.SelectedValue + @" 
                Where OD.OrderId = " + DDorderNo.SelectedValue + @"
                Order BY vf.ColorName";
        }

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Plz Select--");
        Focus = "DDColor";
    }
    protected void SaveGridCarpetData()
    {
        if (TDWeight.Visible == true && (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28"))
        {
            int count = 0;
            foreach (GridViewRow row in this.DGStockDetail.Rows)
            {
                CheckBox chkId = (row.FindControl("Chkboxitem") as CheckBox);
                if (chkId.Checked)
                {
                    count++;
                }
                //this.lblCountedCheckBox.Text = count.ToString();
            }

            if (count > 0)
            {
                WashingByWeight = Math.Round(Convert.ToDecimal(txtWeight.Text == "" ? "0" : txtWeight.Text) / Convert.ToDecimal(count), 3);
            }
            else
            {
                WashingByWeight = 0;
            }
        }

        //Grid Loop
        for (int i = 0; i < DGStockDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                saveDetail();
            }
        }
        Fillstockdetail();
        //
    }
    protected void btnsavegrid_Click(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "27" && Session["usertype"].ToString() != "1")
        {
            btnclickflag = "";
            btnclickflag = "BtnSaveUpdate";
            Popup(true);
            txtpwd.Focus();
        }
        else
        {
            SaveGridCarpetData();
        }

    }
    protected void DDSize_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (TDPacktype.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDPacktype, "select ID,Packingtype From packingtype order by PackingType", true, "--Plz Select--");
        }
    }
    protected void DDPacktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDarticleno, "select Distinct ArticleNo,ArticleNo as articleno1 From Packingarticle Where QualityId=" + DDQuality.SelectedValue + " and Designid=" + DDDesign.SelectedValue + " and Colorid=" + DDColor.SelectedValue + " and sizeid=" + DDSize.SelectedValue + " and Packingtypeid=" + DDPacktype.SelectedValue + " order by articleno1", true, "--Plz Select--");
    }

    protected void DDarticleno_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDbatchNo, @"select PM.Id,PM.BatchNo From PackingPlanMaster PM inner join PackingPlanDetail PD on PM.ID=PD.Masterid
                                                         and PD.Articleno='" + DDarticleno.SelectedItem.Text + "' and Pd.Packtypeid=" + DDPacktype.SelectedValue + " and PM.Status='OPEN' order by PM.Id desc", true, "--Plz Select--");
        if (DDbatchNo.Items.Count > 0)
        {
            DDbatchNo.SelectedIndex = 1;
        }
    }

    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        string str = "";
        DataSet ds = null;
        try
        {
            if (txtWeaverIdNo.Text != "")
            {
                switch (Session["varcompanyId"].ToString())
                {
                    case "8":
                        str = @"select Emp.Empid from process_issue_Master_" + DDTOProcess.SelectedValue + " PM,Process_issue_Detail_" + DDTOProcess.SelectedValue + @" PD,Employee_ProcessOrderNo EMP,Empinfo EI
                      Where PM.IssueOrderid=PD.IssueOrderid And PM.issueOrderid=EMP.IssueOrderid And Processid=" + DDTOProcess.SelectedValue + @" And EI.EMpId=EMP.EmpId 
                      And EI.EmpCode='" + txtWeaverIdNo.Text + @"' And pqty>0
                      select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'";

                        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                        if (Session["varcompanyid"].ToString() != "14")
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Stock No. Already pending at this ID No..');", true);
                                return;
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (lstWeaverName.Items.FindByValue(ds.Tables[1].Rows[0]["Empid"].ToString()) == null)
                            {

                                lstWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[0]["Empname"].ToString(), ds.Tables[1].Rows[0]["Empid"].ToString()));
                            }
                            txtWeaverIdNo.Text = "";
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                        }
                        break;
                    default:
                        str = "select Empid,Empname, IsNull(EmployeeType, 0) Emptype from empinfo Where EMpid='" + txtgetvalue.Text + "' and isnull(blacklist,0)=0";

                        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if ((Session["varCompanyId"].ToString() == "28" || Session["varCompanyId"].ToString() == "16") && ds.Tables[0].Rows[0]["emptype"].ToString() == "0")
                            {
                                SqlParameter[] param = new SqlParameter[4];
                                param[0] = new SqlParameter("@CardNo", txtWeaverIdNo.Text);
                                param[1] = new SqlParameter("@UserID", Session["varuserid"]);
                                param[2] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);
                                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                                param[3].Direction = ParameterDirection.Output;
                                //*************
                                DataSet dsnew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetEmployeeIsPresentOrNot", param);

                                if (dsnew.Tables[0].Rows.Count == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee is absent so please process attendance');", true);
                                    if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281")
                                    {
                                        txtWeaverIdNoscan.Text = "";
                                        txtWeaverIdNoscan.Focus();
                                        return;
                                    }
                                }
                                if (dsnew.Tables[1].Rows.Count == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee resign from company so can not process order');", true);
                                    return;
                                }
                            }
                            if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                            {
                                lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                            }
                            txtWeaverIdNo.Text = "";
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                        }
                        break;
                }
                ds.Dispose();
            }
            Focus = "txtWeaverIdNo";
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }

    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        fillemployee();
    }
    protected void fillemployee()
    {
        string str = "select Empid,Empname, IsNull(EmployeeType, 0) Emptype From empinfo Where EMPCODE='" + txtWeaverIdNoscan.Text + "' and ISNULL(EMPCODE,'')<>'' and isnull(blacklist,0)=0";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if ((Session["varCompanyId"].ToString() == "28" || Session["varCompanyId"].ToString() == "16") && ds.Tables[0].Rows[0]["emptype"].ToString() == "0")
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CardNo", txtWeaverIdNoscan.Text);
                param[1] = new SqlParameter("@UserID", Session["varuserid"]);
                param[2] = new SqlParameter("@MasterCompanyID", Session["varcompanyId"]);
                param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;
                //*************
                DataSet dsnew = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetEmployeeIsPresentOrNot", param);

                if (dsnew.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee is absent so please process attendance');", true);
                    if (Session["varCompanyId"].ToString() == "28" && Session["varSubCompanyId"].ToString() == "281")
                    {
                        txtWeaverIdNoscan.Text = "";
                        txtWeaverIdNoscan.Focus();
                        return;
                    }
                }
                if (dsnew.Tables[1].Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('Employee resign from company so can not process order');", true);
                    return;
                }
            }

            if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
            {

                lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
            }
            txtWeaverIdNoscan.Text = "";
        }
        Focus = "txtWeaverIdNoscan";
    }
    protected void chkscan_CheckedChanged(object sender, EventArgs e)
    {
        txtWeaverIdNoscan.Visible = false;
        txtWeaverIdNo.Visible = true;
        if (chkscan.Checked == true)
        {
            txtWeaverIdNoscan.Visible = true;
            txtWeaverIdNo.Visible = false;
        }
    }
    protected void disablecontrols()
    {
        chkscan.Enabled = false;
        txtWeaverIdNo.Enabled = false;
        txtWeaverIdNoscan.Enabled = false;
        lstWeaverName.Enabled = false;
        btnDeleteName.Enabled = false;
    }
    protected void Enablecontrols()
    {
        chkscan.Enabled = true;
        txtWeaverIdNo.Enabled = true;
        txtWeaverIdNoscan.Enabled = true;
        lstWeaverName.Enabled = true;
        btnDeleteName.Enabled = true;
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (variable.VarFinishingOrderIssuePassword == txtpwd.Text)
        {
            if (btnclickflag == "BtnSaveUpdate")
            {
                SaveGridCarpetData();
            }
            if (btnclickflag == "BtnSaveScanCarpetNumber")
            {
                SaveScanCarpetNumber();
            }
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }

    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";

        str = @"select Distinct OM.OrderId,OM.CustomerOrderNo as OrderNo from OrderDetail OD  inner join  OrderMaster OM on  OM.OrderId=OD.OrderId
                    And Om.ORDERFROMSAMPLE=0                   
                    Where OM.CompanyId=" + DDCompanyName.SelectedValue + " and OM.status='0' and OM.CustomerId=" + DDcustcode.SelectedValue + " order by OM.OrderId";

        UtilityModule.ConditionalComboFill(ref DDorderNo, str, true, "--Plz Select--");

    }
    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        if (Session["varcompanyId"].ToString() == "44")
        {
            str = @"select distinct b.CATEGORY_ID ,b.CATEGORY_NAME from OrderDetail a join V_FinishedItemDetail b on a.Item_Finished_Id=b.ITEM_FINISHED_ID
	join ITEM_CATEGORY_MASTER ICM on icm.CATEGORY_ID=b.CATEGORY_ID inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0
	where orderid=" + DDorderNo.SelectedValue;

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 0, true, "--Plz Select--");

            str = @"select top(1) OrderUnitId From OrderDetail Where OrderId=" + DDorderNo.SelectedValue;
            DataSet dsORDER = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            if (dsORDER.Tables[0].Rows.Count > 0)
            {
                //if (dsORDER.Tables[0].Rows[0]["orderunitid"].ToString() == "1")
                //{
                if (DDUnit.Items.FindByValue(dsORDER.Tables[0].Rows[0]["orderunitid"].ToString()) != null)
                {
                    DDUnit.SelectedValue = dsORDER.Tables[0].Rows[0]["orderunitid"].ToString();
                }
                // }
            }

        }

    }
    protected void TxtIssueQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        if (hnfromprocessid.Value == "0" && variable.VarFinishingIssueSeqWise == "1")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Last Job is not defined for this Quality.');", true);
        }
        if (TxtIssueQty.Text != "")
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtIssueQty.Text);
            Fillstockdetail();
        }
    }

    protected void DDFromProcessForRecChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        str = @"Select EI.EmpId,EI.EmpName+' - '+EI.EmpCode from EmpInfo EI(NoLock) JOIN EmpProcess EP(NoLock) ON EI.EmpId=EP.EmpId 
                Where EP.ProcessID=" + DDFromProcessForRecChallanNo.SelectedValue + " Order by EI.EmpName";

//        str = @"Select EI.EmpId,EI.EmpName+' - '+EI.Empcode as EmpName From PROCESS_RECEIVE_MASTER_" + DDFromProcessForRecChallanNo.SelectedValue + " PRM(NoLock) JOIN PROCESS_RECEIVE_DETAIL_" + DDFromProcessForRecChallanNo.SelectedValue + @" PRD(NoLock) ON PRM.PROCESS_REC_ID=PRD.PROCESS_REC_ID
//                JOIN Employee_ProcessReceiveNo EPR(NoLock) ON PRM.PROCESS_REC_ID=EPR.Process_Rec_id and EPR.ProcessId=" + DDFromProcessForRecChallanNo.SelectedValue + @"
//                JOIN EmpInfo EI(NoLock) ON EPR.Empid=EI.EmpId
//                Group by EI.EmpId,EI.EmpName,EI.Empcode
//                Order by EI.EmpName";        

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDProcessWiseEmpName, ds, 0, true, "--Select Emp--"); 
    }
    protected void DDProcessWiseEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        TxtIssueDate.Enabled = true;

        string str = "";
        str = @"Select PRM.Process_Rec_Id,PRM.ChallanNo From PROCESS_RECEIVE_MASTER_" + DDFromProcessForRecChallanNo.SelectedValue + " PRM(NoLock) JOIN PROCESS_RECEIVE_DETAIL_" + DDFromProcessForRecChallanNo.SelectedValue + @" PRD(NoLock) ON PRM.PROCESS_REC_ID=PRD.PROCESS_REC_ID
                JOIN Employee_ProcessReceiveNo EPR(NoLock) ON PRM.PROCESS_REC_ID=EPR.Process_Rec_id and EPR.ProcessId=" + DDFromProcessForRecChallanNo.SelectedValue + @"
                JOIN EmpInfo EI(NoLock) ON EPR.Empid=EI.EmpId
                Where EI.EmpId=" + DDProcessWiseEmpName.SelectedValue + " and PRM.CompanyId=" + DDCompanyName.SelectedValue + @"
                Group by PRM.Process_Rec_Id,PRM.ChallanNo
                Order by PRM.Process_Rec_Id";
       
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDRecChallanNoJobWise, ds, 0, true, "--Plz RecChallanNo--");

        //str = @"Select PRM.Process_Rec_Id,PRM.ChallanNo From PROCESS_RECEIVE_MASTER_" + DDFromProcessForRecChallanNo.SelectedValue + " PRM(NoLock) Where CompanyId=" + DDCompanyName.SelectedValue + "";

    }
    
}
