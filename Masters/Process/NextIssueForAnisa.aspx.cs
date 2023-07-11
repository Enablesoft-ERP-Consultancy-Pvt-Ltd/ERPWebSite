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

public partial class Masters_Process_NextIssueForAnisa : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected static string Focus = "";
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
                str = @"Select PNM.PROCESS_NAME_ID, PNM.Process_name 
                    From PROCESS_NAME_MASTER PNM(Nolock) 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                    Where PNM.PROCESS_NAME_ID <> 1 and PNM.Processtype = 1 order by PNM.Process_Name ";
            }
            str = str + @" select UnitId,unitName from Unit where UnitId in(1,2) order by UnitId
                         select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId
                         select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDTOProcess, ds, 0, true, "--Plz Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--Plz Select Unit--");
            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 2, true, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 3, true, "--Plz Select--");
            if (DDCategory.Items.Count > 0)
            {
                switch (Session["varcompanyId"].ToString())
                {
                    case "14":
                        DDCategory.SelectedIndex = 1;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                        break;
                    default:
                        DDCategory.SelectedIndex = 0;
                        DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
                        break;
                }

            }
            DDUnit.SelectedIndex = 1;
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
                    TDitemremark.Visible = false;
                    break;
                case 22:
                    TDWeight.Visible = true;
                    TDFinishingDateStamp.Visible = true;
                    TDULLNo.Visible = true;
                    TDCottonMoisture.Visible = true;
                    TDWoolMoisture.Visible = true;
                    TDECISNO.Visible = true;
                    TDBulkIssueQty.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    break;
                case 14:
                    btnsavegrid.Visible = true;
                    TDBulkIssueQty.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    BtnPreview.Visible = true;
                    break;
                case 16:
                    TDBulkIssueQty.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "";
                    DGStockDetail.PageSize = 100;
                    break;
                case 21:
                    TDBulkIssueQty.Visible = true;
                    TxtIssueQty.Enabled = true;
                    TxtIssueQty.Text = "500";
                    DGStockDetail.PageSize = 500;
                    break;
                default:
                    TDBulkIssueQty.Visible = false;
                    TDCottonMoisture.Visible = false;
                    TDWoolMoisture.Visible = false;
                    TDECISNO.Visible = false;
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
                            break;
                    }
                    break;
            }

            if (variable.VarDEFINEPROCESSRATE_LOCATIONWISE == "1")
            {
                TDRateLocation.Visible = true;
            }
            //user type
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
        UtilityModule.ConditionalComboFill(ref DDFromProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");

    }
    protected void DDFromProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        UtilityModule.ConditionalComboFill(ref DDTOProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER where PROCESS_Name_ID != " + ViewState["FromProcessId"] + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");
    }
    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        //For grid fill
        hnfromprocessid.Value = "0";
        //
        LblErrorMessage.Text = "";
        ViewState["IssueOrderid"] = 0;
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
        TDRateNew.Visible = false;
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
        TDPackingUniqueNo.Visible = false;

        //***********************************************
        string str = "select isnull(AreaEditable,0) as AreaEditable,isnull(issreconetime,0) as issreconetime  From  Process_Name_master Where Process_name_id=" + DDTOProcess.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

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
                    case "21":
                        if (Session["varuserid"].ToString() == "1")
                        {
                            TDcheckjobseq.Visible = true;
                        }
                        TDDatestamp.Visible = true;
                        TDRecchallanNo.Visible = true;
                        TDPacktype.Visible = true;
                        TDarticleNo.Visible = true;
                        TDBatchNo.Visible = true;
                        TDPackingUniqueNo.Visible = false;
                        btnsavegrid.Visible = true;
                        break;

                    case "22":
                        if (Session["varuserid"].ToString() == "1")
                        {
                            TDcheckjobseq.Visible = true;
                        }
                        TDDatestamp.Visible = true;
                        TDRecchallanNo.Visible = true;
                        TDPacktype.Visible = true;
                        TDarticleNo.Visible = true;
                        TDBatchNo.Visible = true;
                        TDPackingUniqueNo.Visible = false;
                        if (TDPacktype.Visible == true)
                        {
                            UtilityModule.ConditionalComboFill(ref DDPacktype, "select ID,Packingtype From packingtype order by PackingType", true, "--Plz Select--");
                        }
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
                        TDPackingUniqueNo.Visible = false;
                        break;
                }
                break;
        }
        //get Job sequence;

        //
    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        if (TDitemName.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Item_Name", true, "---Select Item----");
        }
        UtilityModule.ConditionalComboFill(ref DDQuality, @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName From ITEM_MASTER IM inner join CategorySeparate CS on 
                                                         IM.CATEGORY_ID=cs.Categoryid and cs.id=0  inner join Quality Q on IM.ITEM_ID=q.Item_Id and Cs.Categoryid=" + DDCategory.SelectedValue + " and Im.mastercompanyid=" + Session["varcompanyid"] + " order by Qualityname", true, "--Plz Select--");
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
                        //UtilityModule.ConditionalComboFill(ref DDDesign, "Select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order by DesignName", true, "--Select Design--");
                        break;
                    case "3":
                        tdColor1.Visible = true;
                        // UtilityModule.ConditionalComboFill(ref DDColor, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorName", true, "--Select Color--");
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

    //protected void DGItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    int index = Convert.ToInt32(e.CommandArgument);
    //    GridViewRow row = DGItemDetail.Rows[index];
    //    ViewState["Item_Finished_Id"] = row.Cells[9].Text.ToString();
    //    ViewState["OrderId"] = row.Cells[10].Text.ToString();
    //    int VarShapeId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where MasterCompanyId=" + Session["varCompanyId"] + " And  Item_Finished_Id=" + ViewState["Item_Finished_Id"]));
    //    if (Convert.ToInt32(ViewState["Function"]) == 2)
    //    {
    //        Fill_StockGride();
    //    }
    //    else if (Convert.ToInt32(ViewState["Function"]) == 1)
    //    {
    //        string Length = ((TextBox)row.Cells[4].FindControl("txtLength")).Text;
    //        string Width = ((TextBox)row.Cells[5].FindControl("txtWidth")).Text;
    //        string Area = ((TextBox)row.Cells[6].FindControl("txtArea")).Text;
    //        string Rate = ((TextBox)row.Cells[7].FindControl("txtRate")).Text;
    //        if (Rate == "" || Rate == "0")
    //        {
    //            LblErrorMessage.Visible = true;
    //            LblErrorMessage.Text = "Pls Enter Rate....";
    //        }
    //        else
    //        {
    //            Calculate_Area(Length, Width, index, Area, Rate, VarShapeId);
    //        }
    //    }
    //}
    private void Calculate_Area(string Length, string Width, int index, string Area, string Rate, int VarShapeId)
    {
        //LblErrorMessage.Text = "";
        //GridViewRow row = DGItemDetail.Rows[index];
        //int Qty = Convert.ToInt32(((TextBox)row.Cells[7].FindControl("TxtissueQty")).Text);
        //int TQty = Convert.ToInt32(row.Cells[12].Text);
        //if (Qty > TQty || Qty <= 0)
        //{
        //    ((TextBox)row.Cells[7].FindControl("TxtissueQty")).Focus();
        //    LblErrorMessage.Text = "Issue Qty must greater then 0 and less or equals to " + TQty + "............";
        //}
        //if (LblErrorMessage.Text == "")
        //{
        //    if (Length != "" && Width != "")
        //    {
        //        if (DDUnit.SelectedValue == "2")
        //        {
        //            int FootLength = 0;
        //            int FootLengthInch = 0;
        //            Length = string.Format("{0:#0.00}", Length);
        //            FootLength = Convert.ToInt32(Length.Split('.')[0]);
        //            FootLengthInch = Convert.ToInt32(Length.Split('.')[1]);
        //            Width = string.Format("{0:#0.00}", Width);
        //            int FootWidth = Convert.ToInt32(Width.Split('.')[0]);
        //            int FootWidthInch = Convert.ToInt32(Width.Split('.')[1]);

        //            if (FootLengthInch > 11)
        //            {
        //                LblErrorMessage.Text = "Inch value must be less than 12..........";
        //                ((TextBox)row.Cells[3].FindControl("txtLength")).Text = "";
        //                ((TextBox)row.Cells[3].FindControl("txtLength")).Focus();
        //            }

        //            else if (FootWidthInch > 11)
        //            {
        //                LblErrorMessage.Text = "Inch value must be less than 12...........";
        //                ((TextBox)row.Cells[4].FindControl("txtWidth")).Text = "";
        //                ((TextBox)row.Cells[4].FindControl("txtWidth")).Focus();
        //            }
        //            else
        //            {
        //                ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(VarShapeId)).ToString();
        //                Area = ((TextBox)row.Cells[5].FindControl("txtArea")).Text;
        //                ProcessIssue(Length, Width, Area, Rate, Qty);
        //            }
        //        }
        //        else if (DDUnit.SelectedValue == "1")
        //        {
        //            ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(VarShapeId)).ToString();
        //            Area = ((TextBox)row.Cells[5].FindControl("txtArea")).Text;
        //            ProcessIssue(Length, Width, Area, Rate, Qty);
        //        }
        //    }
        //    else
        //    {
        //        LblErrorMessage.Text = "Importent fields missing..........";
        //    }
        //}
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
            string strsql = @"Select StockNo,TStockNo,[dbo].[GET_Process_Rate](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate from CarpetNumber where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and IssRecStatus=0 And CompanyId= " + DDCompanyName.SelectedValue + " And CurrentProStatus=" + ViewState["FromProcessId"];
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

    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        }
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fillsize();
    }

    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_gride();
        UtilityModule.ConditionalComboFill(ref DDSize, "select Distinct vf.sizeid,vf.SizeMtr+' '+ vf.shapename as size From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid=" + DDColor.SelectedValue + " and vf.sizeid<>0 order by Size", true, "--Plz Select--");
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
    private void ProcessIssue(string Length, string Width, string Area, string Rate, int Qty)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[23];
            if (Convert.ToInt32(Session["varCompanyId"]) == 5 && ChkForArea.Checked == true)
            {
                Area = TxtAreaNew.Text == "" ? "0" : TxtAreaNew.Text;
            }
            if (Convert.ToInt32(Session["varCompanyId"]) == 5 && ChkForRate.Checked == true)
            {
                Rate = TxtRateNew.Text == "" ? "0" : TxtRateNew.Text;
            }
            _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
            _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
            _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.NVarChar);
            _arrpara[3] = new SqlParameter("@Status", SqlDbType.DateTime);
            _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
            _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
            _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.Int);
            _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.Int);
            _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

            _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Float);
            _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.DateTime);
            _arrpara[11] = new SqlParameter("@Length", SqlDbType.Int);
            _arrpara[12] = new SqlParameter("@Width", SqlDbType.Int);
            _arrpara[13] = new SqlParameter("@Area", SqlDbType.Float);
            _arrpara[14] = new SqlParameter("@Rate", SqlDbType.NVarChar);
            _arrpara[15] = new SqlParameter("@Amount", SqlDbType.NVarChar);
            _arrpara[16] = new SqlParameter("@Qty", SqlDbType.Float);
            _arrpara[17] = new SqlParameter("@ReqByDate", SqlDbType.Int);
            _arrpara[18] = new SqlParameter("@PQty", SqlDbType.Float);

            _arrpara[19] = new SqlParameter("@Comm", SqlDbType.Float);
            _arrpara[20] = new SqlParameter("@CommAmt", SqlDbType.DateTime);
            _arrpara[21] = new SqlParameter("@Orderid", SqlDbType.Int);
            _arrpara[22] = new SqlParameter("@CalType", SqlDbType.Int);
            int num;
            if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || (DDTOProcess.SelectedValue != (ViewState["processId"]).ToString()))
            {
                int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));
                ViewState["IssueOrderid"] = a;
                TxtChallanNO.Text = a.ToString();
                num = 1;
            }
            else
            {
                num = 0;
            }
            _arrpara[0].Value = (ViewState["IssueOrderid"]);
            _arrpara[1].Value = DDContractor.SelectedValue;
            _arrpara[2].Value = TxtIssueDate.Text;
            _arrpara[3].Value = "Pending";
            _arrpara[4].Value = DDUnit.SelectedValue;
            _arrpara[5].Value = 0;
            _arrpara[6].Value = TxtRemarks.Text.ToUpper();
            _arrpara[7].Value = TxtInsructions.Text.ToUpper();
            _arrpara[8].Value = DDCompanyName.SelectedValue;

            _arrpara[9].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(Issue_Detail_Id),0)+1 from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue);
            _arrpara[10].Value = ViewState["Item_Finished_Id"];
            _arrpara[11].Value = Length;
            _arrpara[12].Value = Width;
            _arrpara[13].Value = Area;
            _arrpara[14].Value = Rate;

            if (DDcaltype.SelectedValue == "1")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Rate) * Convert.ToDouble(Qty)));
            }
            else
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Area) * Convert.ToDouble(Rate) * Convert.ToDouble(Qty)));
            }
            _arrpara[16].Value = Qty;
            _arrpara[17].Value = TxtReqDate.Text;
            _arrpara[18].Value = Qty;
            _arrpara[19].Value = "0";
            _arrpara[20].Value = "0";
            _arrpara[21].Value = ViewState["OrderId"];
            _arrpara[22].Value = DDcaltype.SelectedValue;

            ViewState["processId"] = Convert.ToInt32(DDTOProcess.SelectedValue);

            if (num == 1)
            {

                string str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                str = str + @"Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            string str1 = @"Insert Into Process_Issue_Detail_" + DDTOProcess.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(ViewState["processId"]), Convert.ToInt32(_arrpara[21].Value), Tran);

            string str2 = @"select StockNo From CarpetNumber Where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and Orderid=" + ViewState["OrderId"] + " and CurrentProStatus=" + ViewState["FromProcessId"] + " and IssRecStatus=0";
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str2);
            int n = ds.Tables[0].Rows.Count;
            SqlParameter[] _arrpar = new SqlParameter[23];

            _arrpar[0] = new SqlParameter("@StockNo", SqlDbType.Int);
            _arrpar[1] = new SqlParameter("@FromProcessId", SqlDbType.Int);
            _arrpar[2] = new SqlParameter("@ToProcessId", SqlDbType.Int);
            _arrpar[3] = new SqlParameter("@OrderId", SqlDbType.Int);
            _arrpar[4] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
            _arrpar[5] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
            _arrpar[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrpar[7] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
            _arrpar[8] = new SqlParameter("@UserId", SqlDbType.Int);
            _arrpar[9] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
            _arrpar[10] = new SqlParameter("@VarTypeId", SqlDbType.Int);

            _arrpar[1].Value = ViewState["FromProcessId"];
            _arrpar[2].Value = DDTOProcess.SelectedValue;
            _arrpar[3].Value = ViewState["OrderId"];
            _arrpar[4].Value = TxtIssueDate.Text;
            _arrpar[5].Value = TxtReqDate.Text;
            _arrpar[6].Value = DDCompanyName.SelectedValue;
            _arrpar[7].Value = 0;
            _arrpar[8].Value = Session["varuserid"];
            _arrpar[9].Value = _arrpara[9].Value;
            _arrpar[10].Value = 1;
            for (int i = 0; i < Qty; i++)
            {
                _arrpar[0].Value = ds.Tables[0].Rows[i]["StockNo"];
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProcessStockDetail", _arrpar);
                // SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Update CarpetNumber set CurrentProStatus=" + DDTOProcess.SelectedValue + " ,IssRecStatus=1 Where Pack=0 And StockNo=" + _arrpar[0].Value);
            }

            Tran.Commit();
            BtnPreview.Visible = true;

            fill_DetailGride();
            //  fill_gride();
            // Fill_StockGride();
            Fill_Grid_Total();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Data Successfully Saved.......";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/NextIssue.aspx");
            ViewState["IssueOrderid"] = 0;
            Tran.Rollback();
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
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
                        Length + 'x' + Width Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) StockNo 
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
        //                      select isnull((select processId From Item_Process Where QualityId=" + DDQuality.SelectedValue + " and SeqNo=IP.SeqNo-1),0) as FromProcessid From Item_Process IP Where QualityId=" + DDQuality.SelectedValue + " and processid=" + DDTOProcess.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDDesign, ds, 0, true, "--Plz Select--");
        //***********Get From Processid

        hnfromprocessid.Value = UtilityModule.GetItemProcessid(QualityId: DDQuality.SelectedValue, Processid: DDTOProcess.SelectedValue);

        ////***Get From Processid
        //if (ds.Tables[1].Rows.Count > 0)
        //{
        //    hnfromprocessid.Value = ds.Tables[1].Rows[0]["FromProcessid"].ToString();
        //}
        //else
        //{
        //    hnfromprocessid.Value = "0";
        //}
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
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (Session["varCompanyId"].ToString() == "8")//For SlipPrint For Anisha
            {
                if (ChkForIssue_Receive.Checked == true)
                {
                    str = @"select CI.CompanyName,E.EmpName,E.EMpid,PIM.AssignDate,PM.ReceiveDate,
                           (Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise](PD.Process_Rec_Detail_Id," + DDTOProcess.SelectedValue + @",Issue_Detail_Id)) TStockNo,VF.Item_name,Vf.QualityName,
                           case When PM.UnitId=1 Then vf.SizeMtr Else case When PM.UnitId=2 Then Vf.Sizeft Else vf.Sizeft End End As Size
                           ,Vf.ColorName,PD.Qty,PD.Rate,PD.Amount,PM.Remarks,CI.CompanyId,(select Process_Name from Process_Name_Master Where Process_Name_Id=" + DDTOProcess.SelectedValue + ") As job From Process_Issue_master_" + DDTOProcess.SelectedValue + " PIM,PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,V_FinishedItemDetail VF,Companyinfo CI,Empinfo E
                           Where PIM.IssueOrderid=PD.IssueOrderid And  PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id
                           And PM.Companyid=CI.CompanyId And PM.EmpId=E.EMpid And
                           vf.masterCompanyId=" + Session["varcompanyId"] + " And PM.CompanyId= " + DDCompanyName.SelectedValue + " And PD.IssueOrderid=" + ViewState["IssueOrderid"] + " And PD.QualityType<>3";
                    str = str + "  select CompanyId,'~/Images/Logo/' + Cast(CompanyId as nvarchar) + '_company.gif' Photo From Companyinfo Where CompanyId=" + DDCompanyName.SelectedValue + "";
                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[1].Columns.Add("Image", typeof(System.Byte[]));
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {

                            if (Convert.ToString(dr["Photo"]) != "")
                            {
                                FileInfo TheFile = new FileInfo(Server.MapPath(dr["photo"].ToString()));
                                if (TheFile.Exists)
                                {
                                    string img = dr["Photo"].ToString();
                                    img = Server.MapPath(img);
                                    Byte[] img_Byte = File.ReadAllBytes(img);
                                    dr["Image"] = img_Byte;
                                }
                            }
                        }
                        Session["rptFileName"] = "~\\Reports\\RptJobCard.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\RptJobCard.xsd";
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
                }
                else
                {
                    str = @"select CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,(select Process_Name From Process_Name_Master where Process_Name_Id=" + DDTOProcess.SelectedValue + @")As Job,
                vf.Item_name,vf.QualityName,case When U.UnitId=1 Then vf.Sizemtr Else case when U.UnitId=2 Then vf.Sizeft Else vf.sizeft End End Size,
                Sum(Qty) As Qty  from Process_Issue_Master_" + DDTOProcess.SelectedValue + " PM,Process_Issue_Detail_" + DDTOProcess.SelectedValue + @" PD,V_FinisheditemDetail Vf,Empinfo E,Unit U,Companyinfo CI
                Where PM.IssueOrderId=PD.IssueOrderId And vf.Item_Finished_id=PD.Item_Finished_id And E.Empid=PM.EmpId And CI.CompanyId=PM.CompanyId
                And  U.Unitid=Pm.UnitID And CI.CompanyId=" + DDCompanyName.SelectedValue + " And  PM.IssueOrderId=" + ViewState["IssueOrderid"] + " group by CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,vf.Item_name,vf.QualityName,U.UnitId,vf.Sizemtr,vf.Sizeft";


                    ds = SqlHelper.ExecuteDataset(con, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptJobSlip.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\RptJobSlip.xsd";
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
                }

            }
            else if (Session["varCompanyNo"].ToString() == "14")
            {
                EasternPreviewReport();
            }
            else
            {
                str = @" Delete TEMP_PROCESS_ISSUE_MASTER 
                       Delete TEMP_PROCESS_ISSUE_DETAIL ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["processId"] + ",units from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);


                Session["ReportPath"] = "Reports/NextProcessIssue.rpt";
                Session["CommanFormula"] = "{V_NextProcessIssueReport.IssueOrderId}=" + ViewState["IssueOrderid"] + "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
            //Report();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    private void EasternPreviewReport()
    {
        string str = "";
        DataSet ds;
        try
        {

            str = @"Select isnull(PIM.ChallanNo,'') as ChallanNo, Ci.CompanyId,CI.CompanyName, CI.CompAddr1, '' CompAddr2, '' CompAddr3,CompTel,CI.CompFax,CI.GSTNo
                        ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PIM.issueorderid
                        ,PIM.assigndate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                        PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction,PID.Item_Finished_Id,
                        case when " + Session["varcompanyId"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + DDTOProcess.SelectedValue + @") else '' end as FolioNo,
                        isnull(PID.JobIssueWeight,0) as JobIssueWeight, PID.Amount,PID.GSTTYPE,
                        Case When PID.GSTTYPE=1 Then isnull(round(PID.AMOUNT*PID.CGST/100+  PID.AMOUNT*PID.SGST/100,3),0) else 0 end SGST,
                        Case When PID.GSTTYPE=2 Then isnull(round(PID.AMOUNT*PID.IGST/100,2),0) else 0 end IGST,
                        isnull(VF.HSNCode,'') as HSNCode,isnull(PIM.EWayBillNo,'') as EWayBillNo,
                       (Select Distinct OM.CustomerOrderNo+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID1(NoLock) JOIN  OrderMaster OM(NoLock) ON PID1.OrderID=OM.OrderID 
                            Where PIM.IssueOrderID=PID1.IssueOrderId and PID.ITEM_FINISHED_ID=PID1.Item_Finished_Id For XML PATH('')) as CustomerOrderNo,
                        (Select Distinct CustIn.CustomerCode+',' from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID2(NoLock) JOIN  OrderMaster OM2(NoLock) ON OM2.OrderID = PID2.OrderID 
                            JOIN CustomerInfo CustIn(NoLock) ON OM2.CustomerId=CustIn.CustomerId  Where PID2.IssueOrderId=PIM.IssueOrderID For XML PATH('')) as CustomerCode
                        ,isnull(NU.UserName,'') as UserName,isnull(PID.Bonus,0) as Bonus,isnull(PID.BonusAmt,0) as BonusAmt,isnull(PID.ItemRemark,'') as ItemRemark  
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PIM(NoLock) 
                        Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID(NoLock) on PIM.IssueOrderId=PID.IssueOrderId
                        --JOIN BranchMaster BM(NoLock) ON BM.ID = PIM.BranchID 
                        inner join CompanyInfo CI(NoLock) on PIM.Companyid=CI.CompanyId
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

                Session["rptFileName"] = "~\\Reports\\RptNextissueNewEastern.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextissueNewEastern.xsd";
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
        FROM   PROCESS_NAME_MASTER INNER JOIN V_NextProcessIssueReport ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessIssueReport.PROCESSID INNER JOIN V_Companyinfo ON V_NextProcessIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN V_EmployeeInfo ON V_NextProcessIssueReport.Empid=V_EmployeeInfo.EmpId
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

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillGridDetail_NextIssueForAnisa", _array);

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
        TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
        TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
        TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();
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
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextIssue", param);
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
        #region
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        con.Open();
        //        SqlTransaction Tran = con.BeginTransaction();
        //        try
        //        {
        //            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
        //            if (0 != Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select ReceiveDetailId from Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + "")) && DDTOProcess.SelectedItem.Text.ToUpper() != "PACKING")
        //            {
        //                LblErrorMessage.Visible = true;
        //                LblErrorMessage.Text = "You Have Received....";
        //            }
        //            else
        //            {
        //                if (DGDetail.Rows.Count == 1)
        //                {
        //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Issue_Master_" + DDTOProcess.SelectedValue + @" 
        //                Where IssueOrderID in (Select IssueOrderID from Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")");
        //                }
        //                string str = "Update CarpetNumber Set CurrentProStatus=" + ViewState["FromProcessId"] + ",IssRecStatus=0 From Process_Stock_Detail PSD Where CarpetNumber.StockNo=PSD.StockNo And IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue;
        //                str = str + " Delete Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue;
        //                str = str + " Delete Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id;
        //                str = str + " Delete PROCESS_Consumption_Detail Where ProcessID=" + DDTOProcess.SelectedValue + " And IssueOrderId=" + ViewState["IssueOrderid"] + " And ISSUE_DETAIL_ID=" + VarProcess_Issue_Detail_Id;
        //                str = str + " Delete from Employee_ProcessOrderNo Where ProcessId=" + DDTOProcess.SelectedValue + " And IssueOrderId=" + ViewState["IssueOrderid"] + " And ISSUEDETAILID=" + VarProcess_Issue_Detail_Id;
        //                if (ChkForIssue_Receive.Checked == true)
        //                {
        //                    if (DGDetail.Rows.Count == 1)
        //                    {
        //                        str = str + " Delete from Process_Receive_Master_" + DDTOProcess.SelectedValue + " Where Process_Rec_id in(select Process_rec_id from Process_receive_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")";
        //                    }
        //                    str = str + " Delete from PROCESS_RECEIVE_CONSUMPTION Where Issueorderid=" + ViewState["IssueOrderid"] + " and Processid=" + DDTOProcess.SelectedValue + " and Process_Rec_Detail_Id in(select Process_rec_id from Process_receive_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")";
        //                    str = str + " Delete from Process_Receive_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id;

        //                }
        //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

        //                Tran.Commit();
        //                LblErrorMessage.Visible = true;
        //                LblErrorMessage.Text = "Data deleted successfully....";
        //                FillGridDetails();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            UtilityModule.MessageAlert(ex.Message, "Master/Process/NextIssue.aspx");
        //            Tran.Rollback();
        //            LblErrorMessage.Visible = true;
        //            LblErrorMessage.Text = ex.Message;
        //        }
        //        finally
        //        {
        //            con.Close();
        //            con.Dispose();
        //        }
        #endregion
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
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        string StrTStockNos = "";
        if (Session["varcompanyId"].ToString() == "14" || Session["varcompanyId"].ToString() == "21" && TxtStockNo.Text != "")
        {
            StrTStockNos = TxtStockNo.Text;
            saveDetail(StrTStockNos);
            Focus = "TxtStockNo";
        }
        else if (Session["varcompanyId"].ToString() == "22" && string.IsNullOrEmpty(txtFinishingDateStamp.Text) && DDTOProcess.SelectedItem.Text == "TABLE CHECKING")
        {
            LblErrorMessage.Text = "Please enter Date Stamp!!";
            return;
        }
        else if (Session["varcompanyId"].ToString() == "22" && string.IsNullOrEmpty(txtdatestamp.Text) && string.IsNullOrEmpty(txtEcisNo.Text) && DDTOProcess.SelectedItem.Text == "PACKING")
        {
            LblErrorMessage.Text = "Please enter Date Stamp and ECIS !!";
            return;
        }
        else
        {
            saveDetail("");
            Focus = "TxtStockNo";
        }

    }
    protected void saveDetail(string StrTStockNos)
    {
        LblErrorMessage.Text = "";
         if (Session["varcompanyId"].ToString() == "22" && string.IsNullOrEmpty(txtdatestamp.Text) && string.IsNullOrEmpty(txtEcisNo.Text) && DDTOProcess.SelectedItem.Text == "PACKING")
        {
            LblErrorMessage.Text = "Please enter Date Stamp and ECIS !!";
            return;
        }
        DataSet ds = new DataSet();

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_CheckStockNoForIssue", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@TStockNo", TxtStockNo.Text);
        cmd.Parameters.AddWithValue("@ProcessId", DDTOProcess.SelectedValue);
        cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
        cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@FromProcessid", SqlDbType.Int);
        cmd.Parameters["@FromProcessid"].Direction = ParameterDirection.Output;
        cmd.Parameters.AddWithValue("@qualityidforcheck", tdqualityname1.Visible == true ? DDQuality.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@Designidforcheck", tddesign1.Visible == true ? DDDesign.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@unitid", DDUnit.SelectedValue);
        cmd.Parameters.AddWithValue("@Coloridforcheck", tdColor1.Visible == true ? DDColor.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@Sizeidforcheck", tdsize1.Visible == true ? DDSize.SelectedValue : "0");
        cmd.Parameters.AddWithValue("@notmaintainjobseq", chkjobseqno.Checked == true ? "1" : "0");
        cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);


        // DataSet ds = new DataSet();
        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        ad.Fill(ds);
        //*************

        con.Close();
        con.Dispose();

        LblErrorMessage.Text = cmd.Parameters["@msg"].Value.ToString();
        ViewState["FromProcessId"] = cmd.Parameters["@FromProcessid"].Value.ToString();
        if (LblErrorMessage.Text == "")
        {
            Issue_CarpetWise(TxtStockNo.Text, "0", ds, StrTStockNos);
        }

        //SqlParameter[] param = new SqlParameter[10];
        //param[0] = new SqlParameter("@TStockNo", TxtStockNo.Text);
        //param[1] = new SqlParameter("@ProcessId", DDTOProcess.SelectedValue);
        //param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //param[3] = new SqlParameter("@FromProcessid", SqlDbType.Int);
        //param[2].Direction = ParameterDirection.Output;
        //param[3].Direction = ParameterDirection.Output;
        //param[4] = new SqlParameter("@qualityidforcheck", tdqualityname1.Visible == true ? DDQuality.SelectedValue : "0");
        //param[5] = new SqlParameter("@Designidforcheck", tddesign1.Visible == true ? DDDesign.SelectedValue : "0");
        //param[6] = new SqlParameter("@unitid", DDUnit.SelectedValue);
        //param[7] = new SqlParameter("@Coloridforcheck", tdColor1.Visible == true ? DDColor.SelectedValue : "0");
        //param[8] = new SqlParameter("@Sizeidforcheck", tdsize1.Visible == true ? DDSize.SelectedValue : "0");
        //param[9] = new SqlParameter("@notmaintainjobseq", chkjobseqno.Checked == true ? "1" : "0");

        ////Execute proc 
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_CheckStockNoForIssue", param);

        //LblErrorMessage.Text = param[2].Value.ToString();
        //ViewState["FromProcessId"] = param[3].Value.ToString();
        //if (LblErrorMessage.Text == "")
        //{
        //    Issue_CarpetWise(TxtStockNo.Text, "0", ds, StrTStockNos);
        //}
    }
    private void Issue_CarpetWise(string carpetNo, string Rate, DataSet DS, string StrTStockNos)
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
                    if (Session["varCompanyId"].ToString() == "4")
                    {
                        if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                        {
                            Length = Convert.ToDouble(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToDouble(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Ft(Length, Width, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                        {
                            Length = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Length"]));
                            Width = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Width"]));
                            Area = UtilityModule.Calculate_Area_Ft(Length, Width, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                        {
                            Length = Convert.ToDouble(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToDouble(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Ft(Length, Width, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                        {
                            Length = Convert.ToDouble(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToDouble(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Mtr(Length, Width, Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));

                        }
                    }
                    Rate = TxtRateNew.Text == "" ? "0" : TxtRateNew.Text;
                    SqlParameter[] _arrpara = new SqlParameter[55];
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
                    _arrpara[33] = new SqlParameter("@StrEmpId", SqlDbType.VarChar, 100);
                    _arrpara[34] = new SqlParameter("@UnitsId", SqlDbType.Int);
                    _arrpara[35] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    //For Packing job only
                    _arrpara[36] = new SqlParameter("@Date_Stamp", SqlDbType.VarChar, 100);
                    _arrpara[37] = new SqlParameter("@Recchallanno", SqlDbType.VarChar, 100);
                    _arrpara[38] = new SqlParameter("@Packingtypeid", SqlDbType.Int);
                    _arrpara[39] = new SqlParameter("@Articleno", SqlDbType.VarChar,50);
                    _arrpara[40] = new SqlParameter("@BatchNo", SqlDbType.VarChar, 100);
                    _arrpara[41] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                    _arrpara[42] = new SqlParameter("@Saveflag", SqlDbType.Int);
                    _arrpara[43] = new SqlParameter("@Itemremark", SqlDbType.VarChar, 300);
                    _arrpara[44] = new SqlParameter("@RateLocation", SqlDbType.Int);

                    _arrpara[45] = new SqlParameter("@JobIssueWeight", SqlDbType.Float);
                    _arrpara[46] = new SqlParameter("@JobIssueDateStamp", SqlDbType.VarChar, 50);
                    _arrpara[47] = new SqlParameter("@JobIssueULLNo", SqlDbType.VarChar, 50);
                    _arrpara[48] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 100);
                    _arrpara[49] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 15);
                    _arrpara[51] = new SqlParameter("@CottonMoisture", SqlDbType.Float);
                    _arrpara[52] = new SqlParameter("@WoolMoisture", SqlDbType.Float);
                    _arrpara[53] = new SqlParameter("@PackingUniqueNo", SqlDbType.VarChar, 50);
                    _arrpara[54] = new SqlParameter("@ECISNO", SqlDbType.VarChar, 50);

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
                    _arrpara[28].Direction = ParameterDirection.InputOutput;
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
                    _arrpara[36].Value = TDDatestamp.Visible == true ?  txtdatestamp.Text : "";
                    _arrpara[37].Direction = ParameterDirection.Output;
                    _arrpara[38].Value = TDPacktype.Visible == false ? "0" : DDPacktype.SelectedValue;
                    _arrpara[39].Value = TDarticleNo.Visible == false ? "" : DDarticleno.SelectedValue;
                    _arrpara[40].Value = TDBatchNo.Visible == false ? "" : DDbatchNo.SelectedItem.Text;
                    _arrpara[41].Value = Session["varcompanyid"];
                    _arrpara[42].Direction = ParameterDirection.Output;
                    _arrpara[43].Value = txtitemremark.Text.Trim();
                    _arrpara[44].Value = TDRateLocation.Visible == false ? "0" : DDRateLocation.SelectedValue;

                    _arrpara[45].Value = TDWeight.Visible == false ? "0" : txtWeight.Text == "" ? "0" : txtWeight.Text;
                    _arrpara[46].Value = TDDatestamp.Visible == true ? txtdatestamp.Text : TDFinishingDateStamp.Visible == false ? "" : txtFinishingDateStamp.Text; 
                    _arrpara[47].Value = TDULLNo.Visible == false ? "" : txtUllNo.Text;
                    _arrpara[48].Direction = ParameterDirection.Output;
                    _arrpara[49].Value = TxtEwayBillNo.Text;
                    _arrpara[50] = new SqlParameter("@TStockNos", StrTStockNos);
                    _arrpara[51].Value = TDCottonMoisture.Visible == false ? "0" : txtCottonMoisture.Text == "" ? "0" : txtCottonMoisture.Text;
                    _arrpara[52].Value = TDWoolMoisture.Visible == false ? "0" : txtWoolMoisture.Text == "" ? "0" : txtWoolMoisture.Text;
                    _arrpara[53].Value = TDPackingUniqueNo.Visible == false ? "" : txtPackingUniqueNo.Text;
                    _arrpara[54].Value = TDECISNO.Visible == false ? "" : txtEcisNo.Text;

                    //Insert into Process_Issue_Master And Process_Issue_Detail 
                    if (Session["varcompanyId"].ToString() == "14" || Session["varcompanyId"].ToString() == "21")
                    {
                        //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessIssueForAnisa", _arrpara);
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessIssueForAnisaBulkStockNos", _arrpara);
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessIssueForAnisa", _arrpara);
                    }

                    ViewState["IssueOrderid"] = _arrpara[0].Value;
                    ViewState["@process_rec_id"] = _arrpara[32].Value;
                    // TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();

                    TxtChallanNO.Text = _arrpara[48].Value.ToString();
                    txtrecchallanNo.Text = _arrpara[37].Value.ToString();
                    if (_arrpara[35].Value.ToString() != "")
                    {
                        LblErrorMessage.Text = _arrpara[35].Value.ToString();
                        LblErrorMessage.Visible = true;
                    }
                    else
                    {
                        TxtStockNo.Text = "";
                        TxtStockNo.Focus();
                        //txtWeight.Text = "";
                        //txtFinishingDateStamp.Text = "";
                        //txtUllNo.Text = "";
                        //txtCottonMoisture.Text = "";
                        //txtWoolMoisture.Text = "";
                        txtPackingUniqueNo.Text = "";

                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = "Data Successfully Saved.......";
                    }
                    if (_arrpara[42].Value.ToString() == "1") //Data Not Saved in case of packing
                    {
                        Tran.Rollback();
                    }
                    else
                    {
                        Tran.Commit();
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
        if (tdqualityname1.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDQuality) == false)
            {
                goto a;
            }
        }
        if (tddesign1.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDDesign) == false)
            {
                goto a;
            }
        }
        if (tdColor1.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDColor) == false)
            {
                goto a;
            }
        }
        if (tdsize1.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(DDSize) == false)
            {
                goto a;
            }
        }
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
        if (TDPackingUniqueNo.Visible == true)
        {
            if (UtilityModule.VALIDTEXTBOX(txtPackingUniqueNo) == false)
            {
                goto a;
            }
        }
        if (Session["VarCompanyId"].ToString() == "22" && DDTOProcess.SelectedItem.Text == "PACKAGING")
        {
            if (TDFinishingDateStamp.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtFinishingDateStamp) == false)
                {
                    goto a;
                }
            }
            if (TDWeight.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtWeight) == false)
                {
                    goto a;
                }
            }
            if (TDCottonMoisture.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtCottonMoisture) == false)
                {
                    goto a;
                }
            }
            if (TDWoolMoisture.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtWoolMoisture) == false)
                {
                    goto a;
                }
            }
        }
        else if (Session["VarCompanyId"].ToString() == "22" && DDTOProcess.SelectedItem.Text == "PACKING")
        {

            if (UtilityModule.VALIDTEXTBOX(txtdatestamp) == false)
            {
                goto a;
            }
            if (UtilityModule.VALIDTEXTBOX(txtEcisNo) == false)
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
    }
    protected void BtnShowData_Click(object sender, EventArgs e)
    {
        FillGridDetails();
    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {

        string str = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
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
                        //                        str = @"select Emp.Empid from process_issue_Master_" + DDTOProcess.SelectedValue + " PM,Process_issue_Detail_" + DDTOProcess.SelectedValue + @" PD,Employee_ProcessOrderNo EMP,Empinfo EI
                        //                      Where PM.IssueOrderid=PD.IssueOrderid And PM.issueOrderid=EMP.IssueOrderid And Processid=" + DDTOProcess.SelectedValue + @" And EI.EMpId=EMP.EmpId 
                        //                      And EI.EmpCode='" + txtWeaverIdNo.Text + @"' And pqty>0
                        str = "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "' and BlackList=0";

                        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                        //if (Session["varcompanyid"].ToString() != "14")
                        //{
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "AlertEmp", "alert('Stock No. Already pending at this ID No..');", true);
                        //        return;
                        //    }
                        //}
                        if (ds.Tables[0].Rows.Count > 0)
                        {
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
        finally
        {
            con.Dispose();
            con.Close();
        }
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
        if (hnfromprocessid.Value == "0")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Last Job is not defined for this Quality.');", true);
        }
        Fillstockdetail();
    }
    protected void Fillstockdetail()
    {
        string where = "";
        if (DDQuality.SelectedIndex > 0)
        {
            where = where + " and vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            where = where + " and vf.designid=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
        {
            where = where + " and vf.colorid=" + DDColor.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0)
        {
            where = where + " and vf.sizeid=" + DDSize.SelectedValue;
        }

        string str = @"select Distinct CN.StockNo,CN.TStockNo,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeMtr Itemdescription,
                    PNM.PROCESS_NAME CurrentJob,case when CN.IssRecStatus=0 Then 'Received' Else 'Issued' End currentstatus 
                    From Process_Stock_Detail PSD(Nolock) 
                    join carpetnumber CN(Nolock) on PSD.StockNo=CN.StockNo 
                    Join Process_Receive_Detail_1 PRD(Nolock) ON PRD.Process_Rec_Detail_ID = CN.Process_Rec_Detail_Id And PRD.QualityType <> 3 
                    join V_FinishedItemDetail vf(Nolock) on cn.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    join PROCESS_NAME_MASTER PNM(Nolock) on CN.CurrentProStatus=PNM.PROCESS_NAME_ID
                    left join (select stockno From Process_Stock_Detail PS(Nolock) Where  PS.ToProcessId=" + DDTOProcess.SelectedValue + @") as PID on CN.StockNo=PID.StockNo 
                    left join V_PackingJobstockNo PS on CN.StockNo=PS.StockNo 
                    Where CN.TypeID = 1 And PID.StockNo is null and PS.stockno is null and CN.CurrentProStatus=" + hnfromprocessid.Value + @" And CN.issrecstatus=0 And 
                    PSD.ToProcessId=" + hnfromprocessid.Value + @" And PSD.Receivedate>='" + TxtDataFromDate.Text + "' And PSD.Receivedate<='" + txtgetdataupto.Text + "' and CN.pack=0";
        if (where != "")
        {
            str = str + "   " + where;
        }
        str = str + @" Union Select Distinct CN.StockNo,CN.TStockNo,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeMtr Itemdescription,
                    PNM.PROCESS_NAME CurrentJob,case when CN.IssRecStatus=0 Then 'Received' Else 'Issued' End currentstatus 
                    From Process_Stock_Detail PSD(Nolock) 
                    join carpetnumber CN(Nolock) on PSD.StockNo=CN.StockNo 
                    join V_FinishedItemDetail vf(Nolock) on cn.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    join PROCESS_NAME_MASTER PNM(Nolock) on CN.CurrentProStatus=PNM.PROCESS_NAME_ID
                    left join (select stockno From Process_Stock_Detail PS(Nolock) Where  PS.ToProcessId=" + DDTOProcess.SelectedValue + @") as PID on CN.StockNo=PID.StockNo 
                    left join V_PackingJobstockNo PS on CN.StockNo=PS.StockNo 
                    Where CN.TypeID = 0 And PID.StockNo is null and PS.stockno is null and CN.CurrentProStatus=" + hnfromprocessid.Value + @" And CN.issrecstatus=0 And 
                    PSD.ToProcessId=" + hnfromprocessid.Value + @" And PSD.Receivedate>='" + TxtDataFromDate.Text + "' And PSD.Receivedate<='" + txtgetdataupto.Text + "' and CN.pack=0";
        if (where != "")
        {
            str = str + "   " + where;
        }

        //******FROM REPAIRING
        str = str + " UNION  ";
        str = str + @" SELECT DISTINCT CN.STOCKNO,CN.TSTOCKNO,VF.ITEM_NAME+' '+VF.QUALITYNAME+' '+VF.DESIGNNAME+' '+VF.COLORNAME+' '+VF.SHAPENAME+' '+VF.SIZEMTR AS ITEMDESCRIPTION,
                    PNM.PROCESS_NAME AS CURRENTJOB,CASE WHEN CN.ISSRECSTATUS=0 THEN 'RECEIVED' ELSE 'ISSUED' END AS CURRENTSTATUS
                    FROM PROCESS_STOCK_DETAIL PSD(Nolock) 
                    INNER JOIN PROCESS_NAME_MASTER PNM(Nolock)  ON PSD.TOPROCESSID=PNM.PROCESS_NAME_ID AND PSD.FROMPROCESSID=" + hnfromprocessid.Value + @" AND PNM.PROCESS_NAME='REPAIRING' 
                    INNER JOIN CARPETNUMBER CN(Nolock)  ON PSD.STOCKNO=CN.STOCKNO AND PSD.TOPROCESSID=CN.CURRENTPROSTATUS
                    INNER JOIN V_FINISHEDITEMDETAIL VF(Nolock)  ON CN.ITEM_FINISHED_ID=VF.ITEM_FINISHED_ID
                    LEFT JOIN (SELECT STOCKNO FROM PROCESS_STOCK_DETAIL PS(Nolock) WHERE  PS.TOPROCESSID=" + DDTOProcess.SelectedValue + @") AS PID ON CN.STOCKNO=PID.STOCKNO 
                    LEFT JOIN V_PACKINGJOBSTOCKNO PS(Nolock) ON CN.STOCKNO=PS.STOCKNO
                    WHERE PID.STOCKNO IS NULL AND PS.STOCKNO IS NULL  and CN.issrecstatus=0 and PSD.Receivedate>='" + TxtDataFromDate.Text + "' and PSD.Receivedate<='" + txtgetdataupto.Text + "' and CN.pack=0";
        if (where != "")
        {
            str = str + "   " + where;
        }
        //********DIRECT STOCK
        if (DDTOProcess.SelectedItem.Text.ToUpper() == "PACKING")
        {
            str = str + " UNION";
            str = str + @" select Distinct CN.StockNo,CN.TStockNo,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+vf.SizeMtr as Itemdescription,PNM.PROCESS_NAME as CurrentJob,case when CN.IssRecStatus=0 Then 'Received' Else 'Issued' End as currentstatus
                        From Process_Stock_Detail PSD(Nolock) 
                        inner join carpetnumber CN on PSD.StockNo=CN.StockNo and PSD.ToProcessId=1
                        inner join V_FinishedItemDetail vf(Nolock) on cn.Item_Finished_Id=vf.ITEM_FINISHED_ID
                        inner join PROCESS_NAME_MASTER PNM(Nolock) on CN.CurrentProStatus=PNM.PROCESS_NAME_ID                    
                        Where  CN.CurrentProStatus=1 and cn.TypeId=0
                        and CN.issrecstatus=0  and CN.pack=0";
            if (where != "")
            {
                str = str + "   " + where;
            }
        }

        str = str + "  order by CN.StockNo";
        //*********
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

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
        hnfromprocessid.Value = UtilityModule.GetItemProcessid(QualityId: DDQuality.SelectedValue, Processid: DDTOProcess.SelectedValue, designid: DDDesign.SelectedValue);
        UtilityModule.ConditionalComboFill(ref DDColor, "select Distinct vf.ColorId,vf.ColorName From V_FinishedItemDetail vf where vf.QualityId=" + DDQuality.SelectedValue + " and vf.designid=" + DDDesign.SelectedValue + " and vf.colorid<>0 order by vf.colorname", true, "--Plz Select--");
        Focus = "DDColor";
    }

    protected void btnsavegrid_Click(object sender, EventArgs e)
    {
        //btnsavegrid.ForeColor = System.Drawing.Color.Green;
        btnsavegrid.BackColor = System.Drawing.Color.Orange;

        if (Session["varcompanyId"].ToString() == "14" || Session["varcompanyId"].ToString() == "21")
        {
            string StrTStockNos = "";
            //Grid Loop
            if (Session["varcompanyId"].ToString() == "21" && TxtStockNo.Text != "")
            {
                StrTStockNos = StrTStockNos + TxtStockNo.Text;
            }
            for (int i = 0; i < DGStockDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                if (Chkboxitem.Checked == true)
                {
                    StrTStockNos = StrTStockNos + ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text + "~";

                    if (TxtStockNo.Text == "")
                    {
                        TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                    }
                }
            }

            saveDetail(StrTStockNos);
        }
        else
        {
            //Grid Loop
            for (int i = 0; i < DGStockDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                if (Chkboxitem.Checked == true)
                {
                    TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                    saveDetail("");
                }
            }
        }
        Fillstockdetail();
        //

    }
    protected void DDSize_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (TDPacktype.Visible == true)
        {
            if (Session["VarCompanyNo"].ToString() != "22")
            {

                UtilityModule.ConditionalComboFill(ref DDPacktype, "select ID,Packingtype From packingtype order by PackingType", true, "--Plz Select--");
            }
        }
    }
    protected void DDPacktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "22")
        {
            UtilityModule.ConditionalComboFill(ref DDarticleno, "select Distinct ArticleNo,ArticleNo as articleno1 From Packingarticle Where Packingtypeid=" + DDPacktype.SelectedValue + " order by articleno1", true, "--Plz Select--");
        }
        else
        {

            UtilityModule.ConditionalComboFill(ref DDarticleno, "select Distinct ArticleNo,ArticleNo as articleno1 From Packingarticle Where QualityId=" + DDQuality.SelectedValue + " and Designid=" + DDDesign.SelectedValue + " and Colorid=" + DDColor.SelectedValue + " and sizeid=" + DDSize.SelectedValue + " and Packingtypeid=" + DDPacktype.SelectedValue + " order by articleno1", true, "--Plz Select--");
        }
    }

    protected void DDarticleno_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDbatchNo, @"select PM.Id,PM.BatchNo From PackingPlanMaster PM inner join PackingPlanDetail PD on PM.ID=PD.Masterid
                                                         and PD.Articleno='" + DDarticleno.SelectedItem.Text + "' and Pd.Packtypeid=" + DDPacktype.SelectedValue + " and PM.Status='OPEN' order by PM.Id desc", true, "--Plz Select--");
        if (DDbatchNo.Items.Count > 0)
        {
            DDbatchNo.SelectedIndex = 1;
        }

        if (Session["varCompanyNo"].ToString() == "22")
        {
            FillData();
        }
    }
    protected void TxtIssueQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();


        if (hnfromprocessid.Value == "0")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Alt", "alert('Last Job is not defined for this Quality.');", true);
        }
        if (Session["varcompanyId"].ToString() == "14")
        {
            if (Convert.ToInt32(TxtIssueQty.Text) > 200)
            {
                TxtIssueQty.Text = "200";
            }
        }
        if (Session["varcompanyId"].ToString() == "21")
        {
            if (Convert.ToInt32(TxtIssueQty.Text) > 500)
            {
                TxtIssueQty.Text = "500";
            }
        }
        if (Session["varcompanyId"].ToString() == "22")
        {
            if (Convert.ToInt32(TxtIssueQty.Text) > 200)
            {
                TxtIssueQty.Text = "200";
            }
        }

        if (TxtIssueQty.Text != "")
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtIssueQty.Text);
            Fillstockdetail();
        }
    }
    protected void TxtIssueDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["IssueOrderid"] = 0;
        ViewState["@process_rec_id"] = 0;
    }

    protected void FillData()
    {
        string str = @"select PA.ArticleNo,PA.Itemid,PA.QualityId,PA.Designid,PA.Colorid,PA.shapeid,PA.sizeid,PA.descofgoods,PA.contents,PA.weight_roll,PA.Netwt,volume_roll,PA.pcs_roll,PA.packingtypeid,IM.category_id from Packingarticle PA(NoLock) inner join ITEM_MASTER IM(NoLock) on PA.itemid=IM.Item_id Where  PA.articleno='" + DDarticleno.SelectedItem.Text + @"'
                     select Replace(convert(nvarchar(11),EffDate,106),' ','-') as EffDate,Rate,Grwt,Netwt,Vol,Pcs from PackingarticleRate(NoLock) Where Articleno='" + DDarticleno.SelectedItem.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            DDPacktype.SelectedValue = ds.Tables[0].Rows[0]["packingtypeid"].ToString();

            DDCategory.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();
            DDCategory_SelectedIndexChanged(DDCategory, new EventArgs());
            //DDItemName.SelectedValue = ds.Tables[0].Rows[0]["Itemid"].ToString();
           // DDItemName_SelectedIndexChanged(DDItemName, new EventArgs());
            DDQuality.SelectedValue = ds.Tables[0].Rows[0]["qualityid"].ToString();
            
            DDQuality_SelectedIndexChanged(DDQuality, new EventArgs());
            DDDesign.SelectedValue = ds.Tables[0].Rows[0]["Designid"].ToString();
            DDDesign_SelectedIndexChanged(DDDesign, new EventArgs());            
            DDColor.SelectedValue = ds.Tables[0].Rows[0]["colorid"].ToString();           
            
            DDShape.SelectedValue = ds.Tables[0].Rows[0]["shapeid"].ToString();
           // FillSize();
            DDColor_SelectedIndexChanged(DDColor, new EventArgs());
            DDSize.SelectedValue = ds.Tables[0].Rows[0]["Sizeid"].ToString();

        }
        else
        {
            //refreshcontrol();
        }
    }
}