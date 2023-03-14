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

public partial class Masters_Process_NextIssue : System.Web.UI.Page
{
    static int MasterCompanyId;
    public static string Focus = "";
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
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                         select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + @" Order By PROCESS_NAME
                         select unitid,unitname from unit where unitid in (1,2,6)";
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds1, 0, true, "--SelectCompany");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDFromProcess, ds1, 1, true, "--Select Process--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds1, 2, true, "--Select--");
            logo();
            //UtilityModule.ConditionalComboFill(ref DDFromProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");

            //UtilityModule.ConditionalComboFill(ref DDUnit, "select unitid,unitname from unit where unitid in (1,2,6)", true, "--Select--");
            DDUnit.SelectedIndex = 1;
            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            BtnPreview.Enabled = false;
            DDFromProcess.Focus();
            ViewState["IssueOrderid"] = 0;
            lablechange();
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select smssetting from mastersetting");
            //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            if (Convert.ToInt16(ds.Tables[0].Rows[0]["smssetting"]) == 1)
            {
                chkforsms.Visible = true;
            }
            else
            {
                chkforsms.Visible = false;
            }
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
                    TDChkForIssue_Receive.Visible = true;
                    break;
                case 9:
                    divstock.Visible = false;
                    TDsrno.Visible = true;
                    TDRateNew.Visible = true;
                    TDAreaNew.Visible = true;
                    break;
                case 11:
                    TDRateNew.Visible = true;
                    break;
                case 16:
                    TDRateNew.Visible = true;
                    TDChkForIssue_Receive.Visible = true;
                    break;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];

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
        if (variable.VarNextProcessUserAuthentication == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDTOProcess, "select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From UserRightsProcess UR inner Join PROCESS_NAME_MASTER PNM on UR.ProcessId=PNM.PROCESS_NAME_ID Where UR.Userid=" + Session["varuserid"] + " and PNM.Process_NAME_ID!=" + DDFromProcess.SelectedValue + " and PNM.Mastercompanyid=" + Session["VarcompanyId"] + " order by PNM.PROCESS_NAME", true, "--Select Process--");

        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDTOProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER where PROCESS_Name_ID != " + DDFromProcess.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");
        }
        if (TDsrno.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDsrno, "select orderid,LocalOrder From OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " and Status=0 order by OrderId", true, "--Plz Select--");
        }
    }
    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        ViewState["IssueOrderid"] = 0;
        string str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid And EP.Processid=" + DDTOProcess.SelectedValue + " And EI.Blacklist=0 and EI.MasterCompanyId=" + Session["varCompanyId"] + @" order by Ei.EmpName;
                      Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME";
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(str);

        UtilityModule.ConditionalComboFillWithDS(ref DDContractor, ds, 0, true, "--Select Employee--");
        UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Select Category--");

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT DISTINCT OCALTYPE FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD WHERE PM.PCMID=PD.PCMID And PROCESSID=" + DDTOProcess.SelectedValue + " ANd PM.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["OCALTYPE"].ToString();
        }
        switch (DDTOProcess.SelectedItem.Text.ToUpper())
        {
            case "BINDING":
                DDcaltype.SelectedValue = "2";
                break;
            default:
                if (Session["varcompanyid"].ToString() == "9")
                {
                    switch (DDTOProcess.SelectedItem.Text.ToUpper().Contains("INTERNAL PROCESS"))
                    {
                        case true:
                            ChkForIssue_Receive.Checked = true;
                            break;
                        default:
                            ChkForIssue_Receive.Checked = false;
                            break;
                    }
                }
                else
                {
                    ChkForIssue_Receive.Checked = false;
                }
                break;
        }
        //**********
        switch (Session["varcompanyid"].ToString())
        {
            case "16":
                if (DDUnit.Items.FindByValue("2") != null)
                {
                    DDUnit.SelectedValue = "2";
                }
                break;
        }

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

        string str;
        if (variable.VarNewQualitySize == "1")
        {
            str = @"select distinct vf.ITEM_ID,vf.ITEM_NAME from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetailNew vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID order by Item_name";
        }
        else
        {
            str = @"select distinct vf.ITEM_ID,vf.ITEM_NAME from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetail vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID order by Item_name";
        }
        UtilityModule.ConditionalComboFill(ref DDItemName, str, true, "---Select Item----");

        //UtilityModule.ConditionalComboFill(ref DDItemName, "select Item_id, Item_Name from Item_Master where Category_Id=" + DDCategory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Item_Name", true, "---Select Item----");
        //fill_gride();
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        if (variable.VarNewQualitySize == "1")
        {
            str = @"select distinct vf.QualityId,vf.QualityName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetailNew vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " order by qualityname";
        }
        else
        {
            str = @"select distinct vf.QualityId,vf.QualityName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetail vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " order by qualityname";
        }

        UtilityModule.ConditionalComboFill(ref DDQuality, str, true, "--Select Quality--");
        //UtilityModule.ConditionalComboFill(ref DDQuality, "Select QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QualityName", true, "--Select Quality--");
        ddlcategorycange();
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
                        FillDesign();
                        //         UtilityModule.ConditionalComboFill(ref DDDesign, "Select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order by DesignName", true, "--Select Design--");
                        break;
                    case "3":
                        tdColor1.Visible = true;
                        FillColor();
                        //UtilityModule.ConditionalComboFill(ref DDColor, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorName", true, "--Select Color--");
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
        LblErrorMessage.Text = "";
        if (DGItemDetail.Columns[8].Visible == true) //ALL QTY Button
        {
            Button btnall = (Button)sender;
            GridViewRow row = (GridViewRow)btnall.NamingContainer;
            Label lblitemfinishedidgrid = (Label)row.FindControl("lblitemfinishedid");
            Label lblorderidgrid = (Label)row.FindControl("lblorderid");
            TextBox TxtissueQty = (TextBox)row.FindControl("TxtissueQty");
            string str2 = @"select cn.StockNo,cn.Tstockno From CarpetNumber CN Left join Process_Stock_Detail Psd on cn.StockNo=psd.StockNo and psd.ToProcessId=" + DDTOProcess.SelectedValue + " Where cn.Pack=0 and psd.stockNo is null and cn.CompanyId=" + DDCompanyName.SelectedValue + " And cn.Item_Finished_Id=" + lblitemfinishedidgrid.Text + " and cn.Orderid=" + lblorderidgrid.Text + " and cn.CurrentProStatus=" + DDFromProcess.SelectedValue + " and cn.IssRecStatus=0";
            //string str2 = @"select StockNo,Tstockno From CarpetNumber Where Pack=0 and CompanyId=" + DDCompanyName.SelectedValue + " And Item_Finished_Id=" + lblitemfinishedidgrid.Text + " and Orderid=" + lblorderidgrid.Text + " and CurrentProStatus=" + DDFromProcess.SelectedValue + " and IssRecStatus=0";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int Qty = Convert.ToInt32(TxtissueQty.Text == "" ? "0" : TxtissueQty.Text);
                int stockqty = Convert.ToInt32(ds.Tables[0].Compute("count(tstockno)", ""));
                if (Qty > stockqty)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "op1", "alert('Issue quantity can not greater than Stock Qty.!');", true);
                    fill_gride();
                    return;
                }
                for (int i = 0; i < Qty; i++)
                {
                    TxtStockNo.Text = Convert.ToString(ds.Tables[0].Rows[i]["Tstockno"]);
                    save_detail();
                }
                fill_gride();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "op", "alert('No stock available for this Combination!');", true);
                fill_gride();
            }

        }
        //switch (Session["varcompanyNo"].ToString())
        //{
        //    case "9":
        //        ViewState["Function"] = 1;
        //        break;
        //    default:
        //        ScriptManager.RegisterStartupScript(Page, GetType(), "op", "alert('Please enter Stock No. One by One!');", true);
        //        break;

        //}

    }
    protected void DGItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGItemDetail.Rows[index];
        Label lblitemfinishedidgrid = (Label)row.FindControl("lblitemfinishedid");
        Label lblorderidgrid = (Label)row.FindControl("lblorderid");
        //ViewState["Item_Finished_Id"] = row.Cells[9].Text.ToString();
        //ViewState["OrderId"] = row.Cells[10].Text.ToString();
        ViewState["Item_Finished_Id"] = lblitemfinishedidgrid.Text;
        ViewState["OrderId"] = lblorderidgrid.Text;

        int VarShapeId = 0;
        if (variable.VarNewQualitySize == "1")
        {

            VarShapeId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetailNew Where MasterCompanyId=" + Session["varCompanyId"] + " And  Item_Finished_Id=" + ViewState["Item_Finished_Id"]));
        }
        else
        {
            VarShapeId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where MasterCompanyId=" + Session["varCompanyId"] + " And  Item_Finished_Id=" + ViewState["Item_Finished_Id"]));
        }
        if (Convert.ToInt32(ViewState["Function"]) == 2)
        {
            Fill_StockGride();
        }
        else if (Convert.ToInt32(ViewState["Function"]) == 1)
        {
            string Length = ((TextBox)row.Cells[4].FindControl("txtLength")).Text;
            string Width = ((TextBox)row.Cells[5].FindControl("txtWidth")).Text;
            string Area = ((TextBox)row.Cells[6].FindControl("txtArea")).Text;
            string Rate = ((TextBox)row.Cells[7].FindControl("txtRate")).Text;
            if (Rate == "" || Rate == "0" && Session["varcompanyId"].ToString() != "9")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Plz Enter Rate....";
            }
            else
            {
                //Calculate_Area(Length, Width, index, Area, Rate, VarShapeId);
            }
        }
    }
    private void Calculate_Area(string Length, string Width, int index, string Area, string Rate, int VarShapeId)
    {
        LblErrorMessage.Text = "";
        GridViewRow row = DGItemDetail.Rows[index];
        int Qty = Convert.ToInt32(((TextBox)row.Cells[7].FindControl("TxtissueQty")).Text);
        //int TQty = Convert.ToInt32(row.Cells[12].Text);
        int TQty = Convert.ToInt32(((Label)row.FindControl("lbltqty")).Text);
        if (Qty > TQty || Qty <= 0)
        {
            ((TextBox)row.Cells[7].FindControl("TxtissueQty")).Focus();
            LblErrorMessage.Text = "Issue Qty must greater then 0 and less or equals to " + TQty + "............";
        }
        if (LblErrorMessage.Text == "")
        {
            if (Length != "" && Width != "")
            {
                if (DDUnit.SelectedValue == "2")
                {
                    int FootLength = 0;
                    int FootLengthInch = 0;
                    Length = string.Format("{0:#0.00}", Length);
                    FootLength = Convert.ToInt32(Length.Split('.')[0]);
                    FootLengthInch = Convert.ToInt32(Length.Split('.')[1]);
                    Width = string.Format("{0:#0.00}", Width);
                    int FootWidth = Convert.ToInt32(Width.Split('.')[0]);
                    int FootWidthInch = Convert.ToInt32(Width.Split('.')[1]);

                    if (FootLengthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12..........";
                        ((TextBox)row.Cells[3].FindControl("txtLength")).Text = "";
                        ((TextBox)row.Cells[3].FindControl("txtLength")).Focus();
                    }

                    else if (FootWidthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12...........";
                        ((TextBox)row.Cells[4].FindControl("txtWidth")).Text = "";
                        ((TextBox)row.Cells[4].FindControl("txtWidth")).Focus();
                    }
                    else
                    {
                        ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(VarShapeId), Processid: Convert.ToInt16(DDTOProcess.SelectedValue)).ToString();
                        Area = ((TextBox)row.Cells[5].FindControl("txtArea")).Text;
                        ProcessIssue(Length, Width, Area, Rate, Qty);
                    }
                }
                else if (DDUnit.SelectedValue == "1")
                {
                    ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(VarShapeId)).ToString();
                    Area = ((TextBox)row.Cells[5].FindControl("txtArea")).Text;
                    ProcessIssue(Length, Width, Area, Rate, Qty);
                }
            }
            else
            {
                LblErrorMessage.Text = "Importent fields missing..........";
            }
        }
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
        TxtArea.Text = (varTotalArea * varTotalPcs).ToString();
        TxtAmount.Text = varTotalAmount.ToString();
    }
    private void Fill_StockGride()
    {
        DGStock.DataSource = GetStock();
        DGStock.DataBind();
    }
    private DataSet GetStock()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "";
            con.Open();

            strsql = @"Select StockNo,TStockNo,case WHen " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + @") else [dbo].[GET_PROCESS_RATE_OLDFORM](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") end Rate from CarpetNumber where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and IssRecStatus=0 And CompanyId=" + DDCompanyName.SelectedValue + " And CurrentProStatus=" + DDFromProcess.SelectedValue;


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
    private void fill_gride()
    {
        if (DDFromProcess.SelectedIndex <= 0)
        {
            return;
        }

        int VarUnit = 0;
        if (Session["varcompanyNO"].ToString() == "9")
        {
            string strquery = @"select OM.OrderId, OD.OrderUnitId from OrderMaster OM INNER JOIN OrderDetail OD ON OM.OrderId=OD.OrderId Where OM.LocalOrder='" + DDsrno.SelectedItem.Text + "'";
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strquery);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                DDUnit.SelectedValue = ds2.Tables[0].Rows[0]["OrderUnitId"].ToString();
                VarUnit = Convert.ToInt32(DDUnit.SelectedValue);
            }
            else
            {
                VarUnit = Convert.ToInt32(DDUnit.SelectedValue);
            }

        }
        else
        {
            VarUnit = Convert.ToInt32(DDUnit.SelectedValue);
        }



        string strsql = "";
        string str = "";
        //**************ItemDetailview
        string ViewName = "";
        if (variable.VarNewQualitySize == "1")
        {
            ViewName = "V_FinishedItemDetailNew";
        }
        else
        {
            ViewName = "V_FinishedItemDetail";
        }
        //********************       
        strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.ProdSizeMtr Else VF.ProdSizeFt End Description,
                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + "=1 Then VF.ProdWidthMtr Else VF.ProdWidthFt End Width,Case When " + VarUnit + @"=1 Then VF.ProdLengthMtr Else VF.ProdLengthFt End Length,
                      Round(Case When " + VarUnit + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End,2) Area,case when " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 THen [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Else [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") End Rate,OM.customerorderno,OM.localorder,isnull(OD.remark,'') as remark                      
                      From PROCESS_RECEIVE_MASTER_" + DDFromProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDFromProcess.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                      inner join OrderMaster om on Prd.OrderId=om.OrderId
                      inner JOIN OrderDetail Od ON OM.OrderId=OD.OrderId and OD.Item_Finished_Id=PRD.Item_Finished_Id
                      inner join V_FinishedItemDetail vf on PRD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                      inner join Process_Stock_Detail psd on PRd.Process_Rec_Detail_Id=PSd.ReceiveDetailId and Psd.ToProcessId=" + DDFromProcess.SelectedValue + @"
                      inner join CarpetNumber cn on psd.StockNo=cn.StockNo and CN.Pack=0 and CN.orderid=Om.orderid and CN.currentprostatus=" + DDFromProcess.SelectedValue + @"                      
                      left join Process_Stock_Detail PsdTo on cn.StockNo=PsdTo.StockNo and PsdTo.ToProcessId=" + DDTOProcess.SelectedValue + @" 
                      Where PRD.Qualitytype<>3 and PRM.Companyid=" + DDCompanyName.SelectedValue + " and Psdto.stockNo is null";
        str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.ProdSizeMtr,VF.ProdSizeFt,VF.ProdLengthMtr,VF.ProdLengthFt,VF.ProdWidthMtr,VF.ProdWidthFt,VF.ProdAreaMtr,VF.ProdAreaFt,OM.customerorderno,OM.localorder,OD.remark ";

        if (Session["varCompanyId"].ToString() == "9")
        {

            #region Comment for New Finisherjobrate
            //            strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.ProdSizeMtr Else Case When " + VarUnit + @"=6 Then IsNull(VF.SizeInch,0) Else VF.ProdSizeFt End End Description,
            //                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + @"=1 Then VF.ProdWidthMtr Else Case When " + VarUnit + @"=6 Then IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) Else VF.ProdWidthFt End End Width,
            //                      Case When " + VarUnit + @"=1 Then VF.ProdLengthMtr Else Case When " + VarUnit + @"=6 Then IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) Else VF.ProdLengthFt End End Length,
            //                      Round(Case When " + VarUnit + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End,2) Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate,OM.customerorderno,OM.localorder,isnull(OD.remark,'') as remark                      
            //                      From PROCESS_RECEIVE_MASTER_" + DDFromProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDFromProcess.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
            //                      inner join OrderMaster om on Prd.OrderId=om.OrderId
            //                      inner JOIN OrderDetail Od ON OM.OrderId=OD.OrderId and OD.Item_Finished_Id=PRD.Item_Finished_Id
            //                      inner join V_FinishedItemDetail vf on PRD.Item_Finished_Id=vf.ITEM_FINISHED_ID
            //                      inner join Process_Stock_Detail psd on PRd.Process_Rec_Detail_Id=PSd.ReceiveDetailId and Psd.ToProcessId=" + DDFromProcess.SelectedValue + @"
            //                      inner join CarpetNumber cn on psd.StockNo=cn.StockNo and CN.Pack=0 and CN.orderid=Om.orderid and CN.currentprostatus=" + DDFromProcess.SelectedValue + @"                      
            //                      left join Process_Stock_Detail PsdTo on cn.StockNo=PsdTo.StockNo and PsdTo.ToProcessId=" + DDTOProcess.SelectedValue + @" 
            //                      Where PRD.Qualitytype<>3 and PRM.Companyid=" + DDCompanyName.SelectedValue + " and Psdto.stockNo is null";

            //            str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.ProdSizeMtr,VF.SizeInch,VF.ProdSizeFt,VF.ProdLengthMtr,VF.ProdLengthFt,VF.ProdWidthMtr,VF.ProdWidthFt,VF.ProdAreaMtr,VF.ProdAreaFt,OM.customerorderno,OM.localorder,OD.remark ";
            #endregion
            strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.ProdSizeMtr Else Case When " + VarUnit + @"=6 Then IsNull(VF.SizeInch,0) Else VF.ProdSizeFt End End Description,
                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + @"=1 Then VF.ProdWidthMtr Else Case When " + VarUnit + @"=6 Then IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) Else VF.ProdWidthFt End End Width,
                      Case When " + VarUnit + @"=1 Then VF.ProdLengthMtr Else Case When " + VarUnit + @"=6 Then IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) Else VF.ProdLengthFt End End Length,
                      Round(Case When " + VarUnit + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End,2) Area,case when " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=1 THen [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") else [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") End Rate,OM.customerorderno,OM.localorder,isnull(OD.remark,'') as remark                      
                      From PROCESS_RECEIVE_MASTER_" + DDFromProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDFromProcess.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                      inner join OrderMaster om on Prd.OrderId=om.OrderId
                      inner JOIN OrderDetail Od ON OM.OrderId=OD.OrderId and OD.Item_Finished_Id=PRD.Item_Finished_Id
                      inner join V_FinishedItemDetail vf on PRD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                      inner join Process_Stock_Detail psd on PRd.Process_Rec_Detail_Id=PSd.ReceiveDetailId and Psd.ToProcessId=" + DDFromProcess.SelectedValue + @"
                      inner join CarpetNumber cn on psd.StockNo=cn.StockNo and CN.Pack=0 and CN.orderid=Om.orderid and CN.currentprostatus=" + DDFromProcess.SelectedValue + @"                      
                      left join Process_Stock_Detail PsdTo on cn.StockNo=PsdTo.StockNo and PsdTo.ToProcessId=" + DDTOProcess.SelectedValue + @" 
                      Where PRD.Qualitytype<>3 and PRM.Companyid=" + DDCompanyName.SelectedValue + " and Psdto.stockNo is null";

            str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.ProdSizeMtr,VF.SizeInch,VF.ProdSizeFt,VF.ProdLengthMtr,VF.ProdLengthFt,VF.ProdWidthMtr,VF.ProdWidthFt,VF.ProdAreaMtr,VF.ProdAreaFt,OM.customerorderno,OM.localorder,OD.remark ";


        }
        if (Session["varCompanyId"].ToString() == "5")
        {

            strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.SizeMtr Else VF.SizeFt End Description,
                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When " + VarUnit + "=1 Then VF.WidthMtr Else VF.WidthFt End Width,Case When " + VarUnit + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,
                      Round(Case When " + VarUnit + @"=1 Then VF.AreaMtr Else VF.AreaFt End,2) Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate,om.customerorderno,OM.Localorder,'' as remark                      
                      From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,
                      " + ViewName + @" VF,Process_Stock_Detail PSD,CarpetNumber CN,ordermaster om Where CN.Pack=0 And PRD.QualityType<>3 And PRM.Process_Rec_ID=PRD.Process_Rec_ID And 
                      PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And VF.Item_Finished_Id=PRD.Item_Finished_Id And CN.Item_Finished_id=PRD.Item_Finished_Id And
                      PSD.StockNo=CN.StockNo  And CN.CurrentProStatus=" + DDFromProcess.SelectedValue + " And ToProcessId=" + DDFromProcess.SelectedValue + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];

            str = " Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,CN.Item_Finished_Id,CN.OrderId,VF.SizeMtr,VF.SizeFt,VF.LengthMtr,VF.LengthFt,VF.WidthMtr,VF.WidthFt,VF.AreaMtr,VF.AreaFt,om.customerorderno,OM.localorder";

        }
        if (DDCategory.SelectedIndex > 0)
        {
            strsql = strsql + " And VF.Category_Id=" + DDCategory.SelectedValue;
        }
        if (DDItemName.SelectedIndex > 0)
        {
            strsql = strsql + " And VF.ITEM_ID=" + DDItemName.SelectedValue;
        }
        if (DDQuality.SelectedIndex > 0 && tdqualityname1.Visible == true)
        {
            strsql = strsql + " And VF.QualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0 && tddesign1.Visible == true)
        {
            strsql = strsql + " And VF.DesignId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0 && tdColor1.Visible == true)
        {
            strsql = strsql + " And VF.ColorId=" + DDColor.SelectedValue;
        }
        if (DDShape.SelectedIndex > 0 && tdShape1.Visible == true)
        {
            strsql = strsql + " And VF.ShapeId=" + DDShape.SelectedValue;
        }
        if (DDSize.SelectedIndex > 0 && tdsize1.Visible == true)
        {
            strsql = strsql + " And VF.SizeId=" + DDSize.SelectedValue;
        }
        if (ddlshade.SelectedIndex > 0 && tdshadecolor.Visible == true)
        {
            strsql = strsql + " And VF.ShadecolorId=" + ddlshade.SelectedValue;
        }
        if (DDsrno.SelectedIndex > 0)
        {
            strsql = strsql + " And OM.Localorder='" + DDsrno.SelectedItem.Text + "'";
        }
        strsql = strsql + str;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        DGItemDetail.DataSource = ds;
        DGItemDetail.DataBind();
        if (ds.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "data", "alert('No Data available!!!')", true);
        }
    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (Session["varcompanyNO"].ToString())
        {
            case "9":
                DGItemDetail.Columns[8].Visible = true;
                break;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //  e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGItemDetail.Columns.Count; i++)
            {
                if (Session["varcompanyId"].ToString() == "9")
                {
                    if (DGItemDetail.Columns[i].HeaderText == "Next Process")
                    {
                        DGItemDetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGItemDetail.Columns[i].HeaderText == "Next Process")
                    {
                        DGItemDetail.Columns[i].Visible = false;
                    }
                }
            }


        }
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    protected void DDDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillColor();
        //fill_gride();

    }
    protected void DDColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fill_gride();
    }
    protected void DDShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
        //fill_gride();
    }
    private void fillsize()
    {
        string str;
        string SizeType = "";
        if (DDUnit.SelectedValue == "1")
        {
            SizeType = "ProdSizeMtr";
        }
        else if (DDUnit.SelectedValue == "2")
        {
            SizeType = "ProdSizeFt";
        }
        else
        {
            SizeType = "Sizeinch";
        }

        if (variable.VarNewQualitySize == "1")
        {
            str = @"select distinct  vf.SizeId,vf." + SizeType + " from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetailNew vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyId=" + Session["varcompanyId"] + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        else
        {
            str = @"select distinct  vf.SizeId,vf." + SizeType + " from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetail vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyId=" + Session["varcompanyId"] + " and vf.shapeid=" + DDShape.SelectedValue;
        }
        if (tdqualityname1.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (tddesign1.Visible == true && DDDesign.SelectedIndex > 0)
        {
            str = str + " And vf.designId=" + DDDesign.SelectedValue;
        }
        if (tdColor1.Visible == true && DDColor.SelectedIndex > 0)
        {
            str = str + " And vf.colorId=" + DDColor.SelectedValue;
        }
        str = str + " order by " + SizeType;

        UtilityModule.ConditionalComboFill(ref DDSize, str, true, "--Select Size--");
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

            string str2 = @"select StockNo From CarpetNumber Where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and Orderid=" + ViewState["OrderId"] + " and CurrentProStatus=" + DDFromProcess.SelectedValue + " and IssRecStatus=0";
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

            _arrpar[1].Value = DDFromProcess.SelectedValue;
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
            BtnPreview.Enabled = true;

            fill_DetailGride();
            fill_gride();
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

        string sqlstr = "";
        if (Session["varCompanyId"].ToString() == "9")
        {
            if (variable.VarNewQualitySize == "1")
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case when PM.Unitid=6 then IsNull(SizeInch,0) Else SizeFt End End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,case When " + Session["varcompanyId"].ToString() + "=9 Then '' Else (Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) ENd as StockNo 
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id desc";
            }
            else
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case when PM.Unitid=6 then IsNull(SizeInch,0) Else SizeFt End End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,case When " + Session["varcompanyId"].ToString() + "=9 Then '' Else (Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) ENd as StockNo 
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id desc";
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,case When " + Session["varcompanyId"].ToString() + "=9 Then '' Else (Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) ENd as StockNo 
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id desc";
            }
            else
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,case When " + Session["varcompanyId"].ToString() + "=9 Then '' Else (Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) ENd as StockNo 
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id desc";
            }
        }


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
        if (tddesign1.Visible == true)
        {
            FillDesign();
        }
        //fill_gride();

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
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = @"select CI.CompanyName,E.EmpName,E.EMpid,PIM.AssignDate,PM.ReceiveDate,
                           (Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise](PD.Process_Rec_Detail_Id," + DDTOProcess.SelectedValue + @",Issue_Detail_Id)) TStockNo,VF.Item_name,Vf.QualityName,
                           case When PM.UnitId=1 Then vf.SizeMtr Else case When PM.UnitId=2 Then Vf.Sizeft Else vf.Sizeft End End As Size
                           ,Vf.ColorName,PD.Qty,PD.Rate,PD.Amount,PM.Remarks,CI.CompanyId,(select Process_Name from Process_Name_Master Where Process_Name_Id=" + DDTOProcess.SelectedValue + ") As job From Process_Issue_master_" + DDTOProcess.SelectedValue + " PIM,PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,V_FinishedItemDetailNew VF,Companyinfo CI,Empinfo E
                           Where PIM.IssueOrderid=PD.IssueOrderid And  PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id
                           And PM.Companyid=CI.CompanyId And PM.EmpId=E.EMpid And
                           vf.masterCompanyId=" + Session["varcompanyId"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " And PD.IssueOrderid=" + ViewState["IssueOrderid"] + " And PD.QualityType<>3";
                        str = str + "  select CompanyId,'~/Images/Logo/' + Cast(CompanyId as nvarchar) + '_company.gif' Photo From Companyinfo Where CompanyId=" + DDCompanyName.SelectedValue + "";
                    }
                    else
                    {
                        str = @"select CI.CompanyName,E.EmpName,E.EMpid,PIM.AssignDate,PM.ReceiveDate,
                           (Select * From [dbo].[Get_StockNoNext_Receive_Detail_Wise](PD.Process_Rec_Detail_Id," + DDTOProcess.SelectedValue + @",Issue_Detail_Id)) TStockNo,VF.Item_name,Vf.QualityName,
                           case When PM.UnitId=1 Then vf.SizeMtr Else case When PM.UnitId=2 Then Vf.Sizeft Else vf.Sizeft End End As Size
                           ,Vf.ColorName,PD.Qty,PD.Rate,PD.Amount,PM.Remarks,CI.CompanyId,(select Process_Name from Process_Name_Master Where Process_Name_Id=" + DDTOProcess.SelectedValue + ") As job From Process_Issue_master_" + DDTOProcess.SelectedValue + " PIM,PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,V_FinishedItemDetail VF,Companyinfo CI,Empinfo E
                           Where PIM.IssueOrderid=PD.IssueOrderid And  PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=VF.Item_Finished_id
                           And PM.Companyid=CI.CompanyId And PM.EmpId=E.EMpid And
                           vf.masterCompanyId=" + Session["varcompanyId"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " And PD.IssueOrderid=" + ViewState["IssueOrderid"] + " And PD.QualityType<>3";
                        str = str + "  select CompanyId,'~/Images/Logo/' + Cast(CompanyId as nvarchar) + '_company.gif' Photo From Companyinfo Where CompanyId=" + DDCompanyName.SelectedValue + "";
                    }
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
                    if (variable.VarNewQualitySize == "1")
                    {
                        str = @"select CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,(select Process_Name From Process_Name_Master where Process_Name_Id=" + DDTOProcess.SelectedValue + @")As Job,
                        vf.Item_name,vf.QualityName,case When U.UnitId=1 Then vf.Sizemtr Else case when U.UnitId=2 Then vf.Sizeft Else vf.sizeft End End Size,
                        Sum(Qty) As Qty  from Process_Issue_Master_" + DDTOProcess.SelectedValue + " PM,Process_Issue_Detail_" + DDTOProcess.SelectedValue + @" PD,V_FinisheditemDetailNew Vf,Empinfo E,Unit U,Companyinfo CI
                        Where PM.IssueOrderId=PD.IssueOrderId And vf.Item_Finished_id=PD.Item_Finished_id And E.Empid=PM.EmpId And CI.CompanyId=PM.CompanyId
                        And  U.Unitid=Pm.UnitID And CI.CompanyId=" + DDCompanyName.SelectedValue + " And  PM.IssueOrderId=" + ViewState["IssueOrderid"] + " group by CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,vf.Item_name,vf.QualityName,U.UnitId,vf.Sizemtr,vf.Sizeft";
                    }
                    else
                    {
                        str = @"select CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,(select Process_Name From Process_Name_Master where Process_Name_Id=" + DDTOProcess.SelectedValue + @")As Job,
                        vf.Item_name,vf.QualityName,case When U.UnitId=1 Then vf.Sizemtr Else case when U.UnitId=2 Then vf.Sizeft Else vf.sizeft End End Size,
                        Sum(Qty) As Qty  from Process_Issue_Master_" + DDTOProcess.SelectedValue + " PM,Process_Issue_Detail_" + DDTOProcess.SelectedValue + @" PD,V_FinisheditemDetail Vf,Empinfo E,Unit U,Companyinfo CI
                        Where PM.IssueOrderId=PD.IssueOrderId And vf.Item_Finished_id=PD.Item_Finished_id And E.Empid=PM.EmpId And CI.CompanyId=PM.CompanyId
                        And  U.Unitid=Pm.UnitID And CI.CompanyId=" + DDCompanyName.SelectedValue + " And  PM.IssueOrderId=" + ViewState["IssueOrderid"] + " group by CI.CompanyName,E.Empname,E.Empid,U.UnitName,PM.AssignDate,vf.Item_name,vf.QualityName,U.UnitId,vf.Sizemtr,vf.Sizeft";
                    }


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
            else if (Session["varcompanyId"].ToString() == "9")
            {
                str = @" Delete TEMP_PROCESS_ISSUE_MASTER 
                         Delete TEMP_PROCESS_ISSUE_DETAIL ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["processId"] + " from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight) Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                Report();
            }
            else
            {
                //                str = @" Delete TEMP_PROCESS_ISSUE_MASTER 
                //                         Delete TEMP_PROCESS_ISSUE_DETAIL ";
                //                str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["processId"] + " from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                //                str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight) Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                //                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);


                //                Session["ReportPath"] = "Reports/NextProcessIssue.rpt";
                //                Session["CommanFormula"] = "{V_NextProcessIssueReport.IssueOrderId}=" + ViewState["IssueOrderid"] + "";
                //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
                str = @"select Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
                        ,Ei.EmpId,EI.Empname,EI.address,EI.Address2,EI.Address3,EI.Mobile,Ei.GSTNo as Empgstin,PIM.issueorderid
                        ,PIM.assigndate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
                        Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                        PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Issue_Detail_Id,
                        (Select * from [dbo].[Get_StockNoIssue_Detail_Wise](PID.Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) TStockNo,PIM.Instruction
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " PIM inner Join PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId
                        inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
                        inner Join EmpInfo EI on PIM.Empid=EI.empid
                        inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                        Where PIM.issueorderid=" + ViewState["IssueOrderid"] + " order by Issue_Detail_id";

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = "~\\Reports\\RptNextissueNew2.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptNextissueNew2.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn2", "alert('No Record Found!');", true); }
                return;
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
    private void Report()
    {
        //        string qry = @"SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,V_NextProcessIssueReport.QualityName,V_NextProcessIssueReport.designName,V_NextProcessIssueReport.ColorName,V_NextProcessIssueReport.ShadeColorName,V_NextProcessIssueReport.ShapeName,V_NextProcessIssueReport.Instruction,V_NextProcessIssueReport.Qty,V_NextProcessIssueReport.Rate,area,V_NextProcessIssueReport.IssueOrderId,V_NextProcessIssueReport.AssignDate,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessIssueReport.UnitId,V_NextProcessIssueReport.Length,V_NextProcessIssueReport.Width,V_NextProcessIssueReport.TStockNo,CustomerOrderNo," + Session["VarcompanyNo"] + @" As CompanyNo,CustomerCode,Unitname,LocalOrder
        //        FROM   PROCESS_NAME_MASTER INNER JOIN V_NextProcessIssueReport ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessIssueReport.PROCESSID INNER JOIN V_Companyinfo ON V_NextProcessIssueReport.Companyid=V_Companyinfo.CompanyId INNER JOIN V_EmployeeInfo ON V_NextProcessIssueReport.Empid=V_EmployeeInfo.EmpId
        //        where V_NextProcessIssueReport.IssueOrderId= " + ViewState["IssueOrderid"] + " And PROCESS_NAME_MASTER.MasterCompanyId=" + Session["varCompanyId"] + " And V_Companyinfo.CompanyId=" + DDCompanyName.SelectedValue;
        string qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo,VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VPIR.QualityName,
                    VPIR.designName,VPIR.ColorName,VPIR.ShadeColorName,VPIR.ShapeName,VPIR.Instruction,Sum(VPIR.Qty) as Qty,VPIR.Rate,Sum(VPIR.Area) Area
                    ,VPIR.IssueOrderId,VPIR.AssignDate,
                    PNM.PROCESS_NAME,VPIR.UnitId,VPIR.Length,VPIR.Width,CustomerOrderNo," + Session["VarcompanyNo"] + @" As CompanyNo,Customercode,UnitName,LocalOrder
                    FROM PROCESS_NAME_MASTER PNM INNER JOIN V_NextProcessIssueReport VPIR ON PNM.PROCESS_NAME_ID=VPIR.PROCESSID 
                    INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId INNER JOIN V_EmployeeInfo VEI 
                    ON VPIR.Empid=VEI.EmpId Where VPIR.IssueOrderId=" + ViewState["IssueOrderid"] + " And PNM.MasterCompanyId=" + Session["VarcompanyNo"] + " And VCI.CompanyId=" + DDCompanyName.SelectedValue + @"
                    group by VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo,VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VPIR.QualityName,
                    VPIR.designName,VPIR.ColorName,VPIR.ShadeColorName,VPIR.ShapeName,VPIR.Instruction,VPIR.Rate,VPIR.IssueOrderId,VPIR.AssignDate,
                    PNM.PROCESS_NAME,VPIR.UnitId,VPIR.Length,VPIR.Width,CustomerOrderNo,Customercode,UnitName,LocalOrder";
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
    protected void DGStock_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGStock.Rows[index];
        string carpetNo = row.Cells[0].Text;
        string Rate = ((TextBox)row.Cells[1].FindControl("TxtCarpetRate")).Text;

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CompanyId,issRecStatus,CurrentProStatus,StockNo,Pack from CarpetNumber Where TStockNo='" + carpetNo + "' And CompanyId=" + DDCompanyName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            string Str = @"Select Distinct Replace(Str(PM.IssueOrderId),' ','')+'  /  '+EI.EmpName+'  /  '+replace(convert(varchar(11),AssignDate,106), ' ','-') EmpInFormation 
                                            From PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + @" PD,
                                            EmpInfo EI,Process_Stock_Detail PSD,CarpetNumber CN Where PM.IssueOrderid=PD.IssueOrderid And PM.EmpID=EI.EmpID And 
                                            PD.Issue_Detail_ID=PSD.IssueDetailID And PSD.StockNo=CN.StockNo And ReceiveDetailId=0 And CN.StockNo=" + Ds.Tables[0].Rows[0]["StockNo"] + @" And 
                                            CN.CurrentProStatus=" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + " And EI.MasterCompanyId=" + Session["varCompanyId"];

            LblErrorMessage.Text = "Issue To " + SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString();
            return;
        }

        if (Rate == "" || Rate == "0")
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Plz Enter Rate....";
        }
        else
        {
            Issue_CarpetWise(carpetNo, Rate);
            Fill_StockGride();
        }
    }
    private void Issue_CarpetWise(string carpetNo, string Rate)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            string Length = "0";
            string Width = "0";
            double Area = 0;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                DataSet DS = null;

                if (Session["varCompanyId"].ToString() == "4")
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select CN.StockNo,PRD.Length,PRD.Width,PRD.Area,case WHen " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @") else [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") end Rate,
                             CN.OrderId,CN.Item_Finished_ID,PRM.UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @"  PRD,Process_Stock_Detail PSD,CarpetNumber CN,V_FinishedItemDetailNew VF
                             Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.Item_Finished_Id And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select CN.StockNo,PRD.Length,PRD.Width,PRD.Area,case WHen " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @") else [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") end Rate,
                             CN.OrderId,CN.Item_Finished_ID,PRM.UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @"  PRD,Process_Stock_Detail PSD,CarpetNumber CN,V_FinishedItemDetail VF
                             Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.Item_Finished_Id And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                }
                else if (Session["varCompanyId"].ToString() == "5")
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then VF.WidthMtr Else VF.WidthFt End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.AreaMtr Else VF.AreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then VF.WidthMtr Else VF.WidthFt End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.AreaMtr Else VF.AreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetail VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                }
                else if (Session["varCompanyId"].ToString() == "9")
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) as varchar) Else cast(VF.ProdWidthFt  as varchar) End End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) as varchar) Else cast(VF.ProdLengthFt as varchar) End End Length,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id,2) Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) as varchar) Else cast(VF.ProdWidthFt  as varchar) End End Width,
                                                Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) as varchar) Else cast(VF.ProdLengthFt as varchar) End End Length,
                                                Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                                                [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id,2) Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                                                From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                                                CarpetNumber CN,V_FinishedItemDetail VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                                                PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");


                    }
                }
                else
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else cast(VF.ProdWidthFt  as varchar) End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else cast(VF.ProdLengthFt as varchar) End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id,2) Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else cast(VF.ProdWidthFt  as varchar) End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else cast(VF.ProdLengthFt as varchar) End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id,2) Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId,Round(Weight/Qty,3) Weight 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetail VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                }
                if (DS.Tables[0].Rows.Count > 0)
                {
                    Rate = UtilityModule.PROCESS_RATE(Convert.ToInt32(DS.Tables[0].Rows[0]["Item_Finished_ID"]), Convert.ToInt32(DS.Tables[0].Rows[0]["OrderId"]), Convert.ToInt32(DDTOProcess.SelectedValue), Tran, mastercompanyid: Convert.ToInt16(Session["varcompanyId"]), effectivedate: TxtIssueDate.Text, Caltype: Convert.ToInt16(DDcaltype.SelectedValue), OrderUnitId: Convert.ToInt16(DDUnit.SelectedValue)).ToString();
                    int VarStockNo = Convert.ToInt32(DS.Tables[0].Rows[0]["StockNo"]);
                    if (Session["varCompanyId"].ToString() == "4")
                    {
                        if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length == "" ? "0" : Length), Convert.ToDouble(Width == "" ? "0" : Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                        {
                            Length = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Length"])).ToString();
                            Width = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Width"])).ToString();
                            Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                        }
                    }
                    else if (Session["varCompanyId"].ToString() == "9")
                    {
                        string LengthInch = "0";
                        string WidthInch = "0";
                        if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length == "" ? "0" : Length), Convert.ToDouble(Width == "" ? "0" : Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 6)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);

                           ////Area = Convert.ToDouble(Convert.ToDouble(DS.Tables[0].Rows[0]["Length"]) * Convert.ToDouble(DS.Tables[0].Rows[0]["Width"]) / 144.0);

                            //LengthInch = UtilityModule.ConvertInchesToFt(Convert.ToInt32(DS.Tables[0].Rows[0]["Length"])).ToString();
                           //WidthInch = UtilityModule.ConvertInchesToFt(Convert.ToInt32(DS.Tables[0].Rows[0]["Width"])).ToString();

                            LengthInch = UtilityModule.ConvertInchesToFtHafizia(Convert.ToDouble(DS.Tables[0].Rows[0]["Length"])).ToString();
                            WidthInch = UtilityModule.ConvertInchesToFtHafizia(Convert.ToDouble(DS.Tables[0].Rows[0]["Width"])).ToString();
                           
                            Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(LengthInch == "" ? "0" : LengthInch), Convert.ToDouble(WidthInch == "" ? "0" : WidthInch), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length == "" ? "0" : Length), Convert.ToDouble(Width == "" ? "0" : Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length == "" ? "0" : Length), Convert.ToDouble(Width == "" ? "0" : Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                        }
                        else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                        {
                            Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                            Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                            Area = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length == "" ? "0" : Length), Convert.ToDouble(Width == "" ? "0" : Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                        }
                    }
                    if (ChkForArea.Checked == true)
                    {
                        Area = TxtAreaNew.Text == "" ? 0 : Convert.ToDouble(TxtAreaNew.Text);
                    }
                    if (ChkForRate.Checked == true)
                    {
                        Rate = TxtRateNew.Text == "" ? "0" : TxtRateNew.Text;
                    }
                    SqlParameter[] _arrpara = new SqlParameter[36];
                    _arrpara[0] = new SqlParameter("@IssueOrderid", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@Assign_Date", SqlDbType.SmallDateTime);
                    _arrpara[3] = new SqlParameter("@Status", SqlDbType.NVarChar,50);
                    _arrpara[4] = new SqlParameter("@UnitId", SqlDbType.Int);
                    _arrpara[5] = new SqlParameter("@User_Id", SqlDbType.Int);
                    _arrpara[6] = new SqlParameter("@Remarks", SqlDbType.NVarChar,500);
                    _arrpara[7] = new SqlParameter("@Instruction", SqlDbType.NVarChar,500);
                    _arrpara[8] = new SqlParameter("@Companyid", SqlDbType.Int);

                    _arrpara[9] = new SqlParameter("@Issue_Detail_Id", SqlDbType.Int);
                    _arrpara[10] = new SqlParameter("@Item_Finished_id", SqlDbType.Int);
                    _arrpara[11] = new SqlParameter("@Length", SqlDbType.NVarChar,250);
                    _arrpara[12] = new SqlParameter("@Width", SqlDbType.NVarChar,250);
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
                    _arrpara[33] = new SqlParameter("@ChallanNo", SqlDbType.VarChar,100);

                    // int num = 1;
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
                    _arrpara[0].Value = (ViewState["IssueOrderid"]);
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[1].Value = DDContractor.SelectedValue;
                    _arrpara[2].Value = TxtIssueDate.Text;
                    _arrpara[3].Value = "Pending";
                    _arrpara[4].Value = DDUnit.SelectedValue;
                    _arrpara[5].Value = Session["varuserid"];
                    _arrpara[6].Value = TxtRemarks.Text.ToUpper();
                    _arrpara[7].Value = TxtInsructions.Text.ToUpper();
                    _arrpara[8].Value = DDCompanyName.SelectedValue;
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
                    _arrpara[14].Value = Rate;
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
                    _arrpara[24].Value = DDFromProcess.SelectedValue;
                    _arrpara[25].Value = DDTOProcess.SelectedValue;
                    _arrpara[26].Value = TxtIssueDate.Text;
                    _arrpara[27].Value = TxtReqDate.Text;
                    _arrpara[28].Value = 0;
                    _arrpara[29].Value = 1;
                    _arrpara[30].Value = ChkForIssue_Receive.Checked == true ? 1 : 0; //1 For Issue Receive 
                    _arrpara[31].Value = DS.Tables[0].Rows[0]["Weight"];
                    _arrpara[32].Direction = ParameterDirection.InputOutput;
                    _arrpara[32].Value = ViewState["@process_rec_id"];
                    _arrpara[33].Direction = ParameterDirection.Output;
                    ViewState["processId"] = Convert.ToInt32(DDTOProcess.SelectedValue);
                    

                    #region "Comment on 06-Feb-2014"
                    //if (num == 1)
                    //{

                    //    string str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value;
                    //    str = str + @"Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);


                    //}

                    //#region TO DO.............
                    //DataSet DS1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from Process_Issue_Master_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + _arrpara[0].Value + "");
                    //if (DS1.Tables[0].Rows.Count == 0)
                    //{
                    //    string ABC = @"Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, ABC);
                    //}
                    //string str1 = @"Insert Into Process_Issue_Detail_" + DDTOProcess.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,CancelQty) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ",0)";
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                    // #endregion

                    //SqlParameter[] _arrpar = new SqlParameter[23];

                    //_arrpar[0] = new SqlParameter("@StockNo", SqlDbType.Int);
                    //_arrpar[1] = new SqlParameter("@FromProcessId", SqlDbType.Int);
                    //_arrpar[2] = new SqlParameter("@ToProcessId", SqlDbType.Int);
                    //_arrpar[3] = new SqlParameter("@OrderId", SqlDbType.Int);
                    //_arrpar[4] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                    //_arrpar[5] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);
                    //_arrpar[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
                    //_arrpar[7] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
                    //_arrpar[8] = new SqlParameter("@UserId", SqlDbType.Int);
                    //_arrpar[9] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
                    //_arrpar[10] = new SqlParameter("@VarTypeId", SqlDbType.Int);

                    //_arrpar[0].Value = VarStockNo;
                    //_arrpar[1].Value = DDFromProcess.SelectedValue;
                    //_arrpar[2].Value = DDTOProcess.SelectedValue;
                    //_arrpar[3].Value = DS.Tables[0].Rows[0]["OrderId"];
                    //_arrpar[4].Value = TxtIssueDate.Text;
                    //_arrpar[5].Value = TxtReqDate.Text;
                    //_arrpar[6].Value = DDCompanyName.SelectedValue;
                    //_arrpar[7].Value = 0;
                    //_arrpar[8].Value = Session["varuserid"];
                    //_arrpar[9].Value = _arrpara[9].Value;
                    //_arrpar[10].Value = 1;
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProcessStockDetail", _arrpar);
                    #endregion
                    //Insert into Process_Issue_Master And Process_Issue_Detail 
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessIssue", _arrpara);
                    ViewState["IssueOrderid"] = _arrpara[0].Value;
                    ViewState["@process_rec_id"] = _arrpara[32].Value;
                    ////TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();

                    TxtChallanNO.Text = _arrpara[33].Value.ToString();
                    ////UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value),Convert.ToInt16(DDTOProcess.SelectedValue),Convert.ToInt16(DS.Tables[0].Rows[0]["OrderId"]), Tran);
                    Tran.Commit();
                    //int ABCD = Convert.ToInt32(ViewState["Item_Finished_Id"]);
                    BtnPreview.Enabled = true;
                    switch (Session["varcompanyId"].ToString())
                    {
                        case "16":
                            FillGridDetails();
                            break;
                        default:
                            break;
                    }
                    //FillGridDetails();

                    //fill_DetailGride();
                    //if (DGStock.Rows.Count > 0)
                    //{
                    //    Fill_StockGride();
                    //}
                    //Fill_Grid_Total();
                    //fill_gride();
                    TxtStockNo.Text = "";
                    TxtStockNo.Focus();
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Data Successfully Saved.......";
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
        _array[1].Value = DDFromProcess.SelectedValue;
        _array[2].Value = DDTOProcess.SelectedValue;
        _array[3].Value = ViewState["Item_Finished_Id"];
        _array[4].Value = DDCompanyName.SelectedValue;
        _array[5].Value = Session["varcompanyNo"].ToString();
        _array[6].Value = DGStock.Rows.Count;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillGridDetail_NextIssue", _array);

        DGDetail.DataSource = ds.Tables[0];
        DGDetail.DataBind();
        if (DGStock.Rows.Count > 0)
        {
            DGStock.DataSource = ds.Tables[1];
            DGStock.DataBind();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
            TxtArea.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("Sum(Area)", "")), 4).ToString();
            TxtAmount.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("Sum(Amount)", "")), 2).ToString();
        }
        else
        {
            TxtTotalPcs.Text = "";
            TxtArea.Text = "";
            TxtAmount.Text = "";
        }

    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Issue_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
            if (0 != Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select ReceiveDetailId from Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + "")))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "You Have Received....";
            }
            else
            {
                if (DGDetail.Rows.Count == 1)
                {
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE Process_Issue_Master_" + DDTOProcess.SelectedValue + @" 
                Where IssueOrderID in (Select IssueOrderID from Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + ")");
                }
                string str = "Update CarpetNumber Set CurrentProStatus=" + DDFromProcess.SelectedValue + ",IssRecStatus=0 From Process_Stock_Detail PSD Where CarpetNumber.StockNo=PSD.StockNo And IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue;
                str = str + " Delete Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue;
                str = str + " Delete Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id;
                str = str + " Delete PROCESS_Consumption_Detail Where ProcessID=" + DDTOProcess.SelectedValue + " And IssueOrderId=" + ViewState["IssueOrderid"] + " And ISSUE_DETAIL_ID=" + VarProcess_Issue_Detail_Id;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

                Tran.Commit();
                fill_DetailGride();
                Fill_Grid_Total();
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
        save_detail();
        Focus = "TxtStockNo";
    }
    protected void save_detail()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CompanyId,issRecStatus,CurrentProStatus,StockNo,Pack from CarpetNumber Where TStockNo='" + TxtStockNo.Text + "' And CompanyId=" + DDCompanyName.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "";
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CompanyId"]) != Convert.ToInt32(DDCompanyName.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Company...";

            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["issRecStatus"]) != 0)
            {
                string Str = @"Select Distinct Replace(Str(PM.IssueOrderId),' ','')+'  /  '+EI.EmpName+'  /  '+replace(convert(varchar(11),AssignDate,106), ' ','-') EmpInFormation 
                                From PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + @" PD,
                                EmpInfo EI,Process_Stock_Detail PSD,CarpetNumber CN Where PM.IssueOrderid=PD.IssueOrderid And PM.EmpID=EI.EmpID And 
                                PD.Issue_Detail_ID=PSD.IssueDetailID And PSD.StockNo=CN.StockNo And ReceiveDetailId=0 And CN.StockNo=" + Ds.Tables[0].Rows[0]["StockNo"] + @" And 
                                CN.CurrentProStatus=" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + " And EI.MasterCompanyId=" + Session["varCompanyId"];
                LblErrorMessage.Text = "Issue To " + SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString();


            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CurrentProStatus"]) != Convert.ToInt32(DDFromProcess.SelectedValue))
            {
                string Str = @"Select Process_Name From Process_Name_Master Where Process_Name_Id=" + Ds.Tables[0].Rows[0]["CurrentProStatus"] + "";
                LblErrorMessage.Text = "This Stock No Belongs To " + SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str).ToString() + " Process";

            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Pack"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Packed....";
            }
        }
        else
        {
            LblErrorMessage.Text = "Pls Check Stock No....";
        }
        if (LblErrorMessage.Text == "")
        {
            Issue_CarpetWise(TxtStockNo.Text, "0");
        }
    }
    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDFromProcess) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDTOProcess) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDContractor) == false)
        {
            goto a;
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
    //protected void DGItemDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGStock_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fill_gride();
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"] != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"]);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (DDUnit.SelectedValue != Ds.Tables[0].Rows[0]["UnitId"])
                {
                    DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no has only one unitid !');", true);
                }
            }
        }
        fillsize();

    }
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"] != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"]);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (DDcaltype.SelectedValue != Ds.Tables[0].Rows[0]["CalType"])
                {
                    DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["CalType"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no has only one caltype !');", true);
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
    protected void chkforsms_CheckedChanged(object sender, EventArgs e)
    {
        if (chkforsms.Checked == true)
        {
            Btnsendsms.Visible = true;
        }
        else
        {
            Btnsendsms.Visible = false;
        }
    }
    protected void Btnsendsms_Click(object sender, EventArgs e)
    {
        try
        {
            Btnsendsms.Enabled = false;
            if (ViewState["IssueOrderid"] != "" || ViewState["IssueOrderid"] != "0")
            {
                UtilityModule.SendmessageToWeaver_Vendor_Finisher(MasterTableName: "Process_Issue_Master_" + DDTOProcess.SelectedValue + "", DetailTable: "Process_issue_Detail_" + DDTOProcess.SelectedValue + "", UniqueColName: "IssueOrderId", EmpIdColName: "Empid", OrderId: Convert.ToInt64(ViewState["IssueOrderid"]), OrderNo: "IssueOrderId", MasterCompanyId: Convert.ToInt16(Session["varcompanyId"]), FinishedidColName: "Item_Finished_id", QtyCOlName: "Qty", ReqByDate: "Reqbydate", JobName: "" + DDTOProcess.SelectedItem.Text + "", UnitName: "Pcs");
            }
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Message Sent Successfully";
            Btnsendsms.Enabled = true;
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;

        }
    }
    protected void FillDesign()
    {
        string str;

        if (variable.VarNewQualitySize == "1")
        {
            str = @"select distinct vf.designId,vf.designName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                               And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetailNew vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyid=" + Session["varcompanyId"];
        }
        else
        {
            str = @"select distinct vf.designId,vf.designName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                               And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetail vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyid=" + Session["varcompanyId"];
        }
        if (tdqualityname1.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " And vf.QualityId=" + DDQuality.SelectedValue;
        }
        str = str + " order by designname";

        UtilityModule.ConditionalComboFill(ref DDDesign, str, true, "--Select Design--");
    }
    protected void FillColor()
    {
        string str;
        if (variable.VarNewQualitySize == "1")
        {
            str = @"select distinct vf.ColorId,vf.ColorName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                               And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetailNew vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyid=" + Session["varcompanyId"];
        }
        else
        {
            str = @"select distinct vf.ColorId,vf.ColorName from PROCESS_ISSUE_MASTER_" + DDFromProcess.SelectedValue + " PM inner join PROCESS_ISSUE_DETAIL_" + DDFromProcess.SelectedValue + @" PD  on PM.IssueOrderId=PD.IssueOrderId
                               And PM.Companyid=" + DDCompanyName.SelectedValue + " inner join V_FinishedItemDetail vf on pd.Item_Finished_Id=vf.ITEM_FINISHED_ID and vf.item_id=" + DDItemName.SelectedValue + " Where vf.mastercompanyid=" + Session["varcompanyId"];
        }
        if (tdqualityname1.Visible == true && DDQuality.SelectedIndex > 0)
        {
            str = str + " And vf.QualityId=" + DDQuality.SelectedValue;
        }
        if (tddesign1.Visible == true && DDDesign.SelectedIndex > 0)
        {
            str = str + " And vf.designId=" + DDDesign.SelectedValue;
        }
        str = str + " order by ColorName";

        UtilityModule.ConditionalComboFill(ref DDColor, str, true, "--Select Color--");
    }


    protected void lnkgetitemdetail_Click(object sender, EventArgs e)
    {
        fill_gride();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        //******************sql Table Types
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Issue_Detail_id", typeof(int));
        int rowcount = DGDetail.Rows.Count;
        for (int i = 0; i < rowcount; i++)
        {
            CheckBox chkboxitem = (CheckBox)DGDetail.Rows[i].FindControl("chkboxitem");

            if (chkboxitem.Checked == true)
            {
                DataRow dr = dtrecords.NewRow();
                dr["Issue_Detail_id"] = Convert.ToInt32(DGDetail.DataKeys[i].Value);
                dtrecords.Rows.Add(dr);
            }

        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Processid", DDTOProcess.SelectedValue);
                param[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 500);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@dtrecords", dtrecords);
                param[3] = new SqlParameter("@issueorderid", ViewState["IssueOrderid"]);
                //****
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextIssueBulk", param);
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[1].Value.ToString();
                Tran.Commit();
                FillGridDetails();



            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "delete", "alert('Please check atleast one check box to delete data!!!')", true);
        }
        //******
    }

}