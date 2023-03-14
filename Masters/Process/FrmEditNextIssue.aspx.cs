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

public partial class Masters_Process_FrmEditNextIssue : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected static string Focus = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varCompanyId"]);
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Focus != "")
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            sm.SetFocus(Focus);
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " Order by Companyname", true, "--SelectCompany");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            CompanySelectedIndexChange();
            logo();
            UtilityModule.ConditionalComboFill(ref DDUnit, "select unitid,unitname from unit where unitid in (1,2,6)", true, "--Select--");
            TxtIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtChallanNO.Focus();
            lablechange();
            ViewState["IssueOrderid"] = 0;

            int VarCompanyNo = Convert.ToInt32(Session["varCompanyId"].ToString());
            switch (VarCompanyNo)
            {
                case 1:
                    ProCod1.Visible = false;
                    ProCod2.Visible = false;
                    break;
                case 2:
                    ProCod1.Visible = true;
                    ProCod2.Visible = true;
                    break;
                case 4:
                    ProCod1.Visible = false;
                    ProCod2.Visible = false;
                    TDChkReceiveDetail.Visible = true;
                    break;
                case 5:
                    ProCod1.Visible = false;
                    ProCod2.Visible = false;
                    TDRateNew.Visible = true;
                    TDAreaNew.Visible = true;
                    if (Session["VarDepartment"].ToString() == "0") //For POSH COLLECTION
                    {
                        TDRateNew.Visible = false;
                    }
                    break;
                case 9:
                    divstock.Visible = false;
                    TDRateNew.Visible = true;
                    TDAreaNew.Visible = true;
                    TDsrno.Visible = true;
                    break;
                case 11:
                    TDRateNew.Visible = true;
                    break;
                case 16:
                    TDRateNew.Visible = true;
                    break;
            }
            Fill_Temp_OrderNo();
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblQuality.Text = ParameterList[0];
        lblDesign.Text = ParameterList[1];
        lblColor.Text = ParameterList[2];
        lblShape.Text = ParameterList[3];
        lblSize.Text = ParameterList[4];
        lblCategory.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];

    }
    private void Fill_Temp_OrderNo()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_ISSUE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where Process_Name_Id<>1 And MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_ISSUE_MASTER_NEW SELECT PM.Companyid,OM.Customerid,PD.Orderid," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId,PM.Empid,PM.IssueOrderid,PM.ChallanNo FROM PROCESS_ISSUE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PM,PROCESS_ISSUE_DETAIL_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " PD,OrderMaster OM Where PM.IssueOrderid=PD.IssueOrderid And PD.Orderid=OM.Orderid And PM.Companyid=" + DDCompanyName.SelectedValue + "");
            }
        }
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
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChange();
    }
    private void CompanySelectedIndexChange()
    {
        ViewState["IssueOrderid"] = 0;
        if (variable.VarNextProcessUserAuthentication == "1")
        {
            UtilityModule.ConditionalComboFill(ref DDTOProcess, "select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From UserRightsProcess UR inner Join PROCESS_NAME_MASTER PNM on UR.ProcessId=PNM.PROCESS_NAME_ID Where UR.Userid=" + Session["varuserid"] + " and  PNM.Mastercompanyid=" + Session["VarcompanyId"] + " order by PNM.PROCESS_NAME", true, "--Select Process--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDTOProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");
        }
    }
    protected void DDTOProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToProcessSelectedChange();
        if (TDsrno.Visible == true)
        {
            UtilityModule.ConditionalComboFill(ref DDsrno, "select orderid,LocalOrder From OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " and Status=0 order by OrderId", true, "--Plz Select--");
        }
    }
    private void ToProcessSelectedChange()
    {
        ViewState["IssueOrderid"] = 0;
        ViewState["processId"] = DDTOProcess.SelectedValue;
        string str = @"Select EI.EmpId,EI.EmpName from EmpInfo EI,EmpProcess EP Where EI.Empid=EP.Empid And EP.Processid=" + DDTOProcess.SelectedValue + " And EI.MasterCompanyId=" + Session["varCompanyId"] + @" order by Ei.EmpName
                     Select Distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_NAME";
        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref DDContractor, ds, 0, true, "--Select Employee--");
        UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 1, true, "--Select Category--");
    }
    protected void DDContractor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ContractorSelectedChange();
    }
    private void ContractorSelectedChange()
    {
        ViewState["IssueOrderid"] = 0;
        UtilityModule.ConditionalComboFill(ref DDChallanNo, "Select IssueOrderId,ChallanNo From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where CompanyId=" + DDCompanyName.SelectedValue + " And EmpId=" + DDContractor.SelectedValue + "", true, "--Select ChallanNo--");
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
        //   UtilityModule.ConditionalComboFill(ref DDQuality, "Select QualityId,QualityName from Quality Where Item_Id=" + DDItemName.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By QualityName", true, "--Select Quality--");
        //fill_gride();
    }
    private void ddlcategorycange()
    {
        tdcolor.Visible = false;
        tdColor1.Visible = false;
        tddesign.Visible = false;
        tddesign1.Visible = false;
        tdshape.Visible = false;
        tdShape1.Visible = false;
        tdsize.Visible = false;
        tdsize1.Visible = false;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT PARAMETER_ID FROM ITEM_CATEGORY_PARAMETERS where CATEGORY_ID=" + DDCategory.SelectedValue);
        if (ds.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "2":
                        tddesign.Visible = true;
                        tddesign1.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDDesign, "Select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order by DesignName", true, "--Select Design--");
                        break;
                    case "3":
                        tdcolor.Visible = true;
                        tdColor1.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDColor, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order By ColorName", true, "--Select Color--");
                        break;
                    case "4":
                        tdShape1.Visible = true;
                        tdshape.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDShape, "select ShapeId,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " order By ShapeName", true, "--Select Shape--");
                        tdsize.Visible = true;
                        tdsize1.Visible = true;
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
        //ViewState["Function"] = 1;
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "op", "alert('No stock avilable for this Combination!');", true);
                fill_gride();
            }

        }
    }

    protected void DGItemDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // Retrieve the row index stored in the 
        // CommandArgument property.
        int index = Convert.ToInt32(e.CommandArgument);
        // Retrieve the row that contains the button 
        // from the Rows collection.
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
            VarShapeId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetailNew Where Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
        }
        else
        {
            VarShapeId = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select ShapeId From V_FinishedItemDetail Where Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
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
            if (Rate == "" || Rate == "0")
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Rate....";
            }
            else
            {
                // Calculate_Area(Length, Width, index, Area, Rate, VarShapeId);
            }
        }
    }
    private void Calculate_Area(string Length, string Width, int index, string Area, string Rate, int VarShapeId)
    {
        LblErrorMessage.Text = "";
        GridViewRow row = DGItemDetail.Rows[index];
        int Qty = Convert.ToInt32(((TextBox)row.Cells[7].FindControl("TxtissueQty")).Text);
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
                        ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), VarShapeId).ToString();
                        Area = ((TextBox)row.Cells[5].FindControl("txtArea")).Text;
                        ProcessIssue(Length, Width, Area, Rate, Qty);
                    }
                }
                else if (DDUnit.SelectedValue == "1")
                {
                    ((TextBox)row.Cells[5].FindControl("txtArea")).Text = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), VarShapeId).ToString();
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
            Label lblqty = (Label)DGDetail.Rows[i].FindControl("lblqty");
            Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
            Label lblamount = (Label)DGDetail.Rows[i].FindControl("lblamount");
            varTotalPcs = varTotalPcs + Convert.ToInt32(lblqty.Text);
            varTotalArea = varTotalArea + Convert.ToDouble(lblarea.Text);
            varTotalAmount = varTotalAmount + Convert.ToDouble(lblamount.Text);
        }
        TxtTotalPcs.Text = varTotalPcs.ToString();
        TxtArea.Text = Math.Round(varTotalArea, 4).ToString();
        TxtAmount.Text = Math.Round(varTotalAmount, 2).ToString();
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
            con.Open();
            string strsql = @"Select StockNo,TStockNo,case When " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + ") else [dbo].[GET_PROCESS_RATE_OLDFORM](OrderId,Item_Finished_Id," + DDTOProcess.SelectedValue + ",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") ENd Rate from CarpetNumber where Pack=0 And Item_Finished_Id=" + ViewState["Item_Finished_Id"] + " and IssRecStatus=0 And CompanyId=" + DDCompanyName.SelectedValue;
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
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
        DGItemDetail.DataSource = GetDetail();
        DGItemDetail.DataBind();
    }
    private DataSet GetDetail()
    {

        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = null;
            int category = DDCategory.SelectedIndex > 0 ? Convert.ToInt32(DDCategory.SelectedValue) : 0;
            int item = DDItemName.SelectedIndex > 0 ? Convert.ToInt32(DDItemName.SelectedValue) : 0;
            int quality = DDQuality.SelectedIndex > 0 ? Convert.ToInt32(DDQuality.SelectedValue) : 0;
            int design = DDDesign.SelectedIndex > 0 ? Convert.ToInt32(DDDesign.SelectedValue) : 0;
            int color = DDColor.SelectedIndex > 0 ? Convert.ToInt32(DDColor.SelectedValue) : 0;
            int shape = DDShape.SelectedIndex > 0 ? Convert.ToInt32(DDShape.SelectedValue) : 0;
            int size = DDSize.SelectedIndex > 0 ? Convert.ToInt32(DDSize.SelectedValue) : 0;

            int VarUnit = 0;
            if (DDUnit.Items.Count > 0)
            {
                VarUnit = Convert.ToInt32(DDUnit.SelectedValue);
            }


            string str1 = "", str2 = "", str3 = "", str4 = "", str5 = "", str6 = "", str7 = "", str8 = "";
            if (category > 0)
            {
                str8 = " And VF.Category_id=" + category;
            }
            if (item > 0)
            {
                str1 = " And VF.Item_Id=" + item;
            }

            if (quality > 0)
            {
                str2 = " And VF.QualityId=" + quality;
            }

            if (design > 0)
            {
                str3 = " And VF.DesignId=" + design;
            }

            if (color > 0)
            {
                str4 = " And VF.ColorId=" + color;
            }

            if (shape > 0)
            {
                str5 = " And VF.ShapeId=" + shape;
            }

            if (size > 0)
            {
                str6 = "And VF.SizeId=" + size;
            }
            if (DDsrno.SelectedIndex > 0)
            {
                str7 = "And OM.Localorder='" + DDsrno.SelectedItem.Text + "'";
            }
            string viewname = "";
            if (variable.VarNewQualitySize == "1")
            {
                viewname = "V_FinishedItemDetailNew";
            }
            else
            {
                viewname = "V_FinishedItemDetail";
            }
            if (Session["varCompanyId"].ToString() == "5")
            {

                strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When PRM.UnitID=1 Then VF.SizeMtr Else VF.SizeFt End Description,
                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,Case When PRM.UnitID=1 Then VF.LengthMtr Else VF.LengthFt End Length,Case When PRM.UnitID=1 Then VF.WidthMtr Else VF.WidthFt End Width,Case When PRM.UnitID=1 Then VF.AreaMtr Else VF.AreaFt End Area,
                      [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate,OM.customerorderno,OM.localorder
                      From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,
                      " + viewname + @" VF,Process_Stock_Detail PSD,CarpetNumber CN,ordermaster om
                      Where PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" And  CN.Pack=0 And PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And VF.Item_Finished_Id=PRD.Item_Finished_Id And PRD.Orderid=OM.orderid and
                      PSD.StockNo=CN.StockNo and CN.orderid=OM.orderid  And CN.CurrentProStatus=" + DDFromProcess.SelectedValue + " " + str8 + "  " + str1 + " " + str2 + @" 
                      " + str3 + " " + str4 + " " + str5 + " " + str6 + "  " + str7 + @" Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                      CN.Item_Finished_Id,CN.OrderId,VF.SizeMtr,VF.SizeFt,PRM.UnitID,VF.LengthMtr,VF.LengthFt,VF.WidthMtr,VF.WidthFt,VF.AreaMtr,VF.AreaFt,OM.customerorderno,OM.localorder";

            }
            else if (Session["varCompanyId"].ToString() == "9")
            {
                strsql = @"select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When " + VarUnit + @"=1 Then VF.ProdSizeMtr Else case when " + VarUnit + @"=6 then IsNull(VF.SizeInch,0) Else VF.ProdSizeFt End End Description,
                         CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,
                         Case When " + VarUnit + @"=1 Then VF.ProdLengthMtr Else case when " + VarUnit + @"=6 then IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) Else VF.ProdLengthFt End End Length,
                         Case When " + VarUnit + @"=1 Then VF.ProdWidthMtr Else Case When " + VarUnit + @"=6 Then IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) Else VF.ProdWidthFt End End Width,
                         Round(Case When " + VarUnit + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End,2) Area,
                         case when " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Else [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") end Rate,
                         om.customerorderno,OM.localorder From PROCESS_RECEIVE_MASTER_" + DDFromProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDFromProcess.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                         inner join V_FinishedItemDetail vf on PRD.Item_finished_id=vf.ITEM_FINISHED_ID
                         inner join OrderMaster om on prd.OrderId=om.OrderId
                         inner join CarpetNumber cn on om.OrderId=cn.OrderId and CN.CurrentProStatus=" + DDFromProcess.SelectedValue + @"
                         inner join Process_Stock_Detail psd on CN.StockNo=psd.StockNo and Prd.Process_Rec_Detail_Id=psd.ReceiveDetailId and psd.ToProcessId=" + DDFromProcess.SelectedValue + @"
                         left join Process_Stock_Detail PsdTo on cn.StockNo=PsdTo.StockNo and PsdTo.ToProcessId=" + DDTOProcess.SelectedValue + @"
                         Where PRm.CompanyId=" + DDCompanyName.SelectedValue + @" and Cn.Pack=0 and psdto.stockno is null  " + str8 + "  " + str1 + " " + str2 + @" 
                        " + str3 + " " + str4 + " " + str5 + " " + str6 + "  " + str7 + @"
                         Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                         CN.Item_Finished_Id,CN.OrderId,VF.SizeInch,VF.ProdLengthMtr,VF.ProdLengthFt,VF.ProdWidthMtr,VF.ProdWidthFt,VF.ProdAreaMtr,VF.ProdAreaFt, PRM.UnitID,VF.ProdSizeMtr,VF.ProdSizeFt,OM.customerorderno,OM.localorder";
            }
            else
            {
                strsql = @"select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When PRM.UnitID=1 Then VF.ProdSizeMtr Else VF.ProdSizeFt End Description,
                         CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,PRD.Length,PRD.Width,PRD.Area,case when " + variable.VarFINISHERJOBRATEFOR_OLDFORM + "=0 then [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") else [dbo].[GET_PROCESS_RATE_OLDFORM](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @",'" + TxtIssueDate.Text + "'," + DDcaltype.SelectedValue + "," + DDUnit.SelectedValue + @") end Rate,
                         om.customerorderno,OM.localorder From PROCESS_RECEIVE_MASTER_" + DDFromProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDFromProcess.SelectedValue + @" PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                         inner join V_FinishedItemDetail vf on PRD.Item_finished_id=vf.ITEM_FINISHED_ID
                         inner join OrderMaster om on prd.OrderId=om.OrderId
                         inner join CarpetNumber cn on om.OrderId=cn.OrderId and CN.CurrentProStatus=" + DDFromProcess.SelectedValue + @"
                         inner join Process_Stock_Detail psd on CN.StockNo=psd.StockNo and Prd.Process_Rec_Detail_Id=psd.ReceiveDetailId and psd.ToProcessId=" + DDFromProcess.SelectedValue + @"
                         left join Process_Stock_Detail PsdTo on cn.StockNo=PsdTo.StockNo and PsdTo.ToProcessId=" + DDTOProcess.SelectedValue + @"
                         Where PRm.CompanyId=" + DDCompanyName.SelectedValue + @" and Cn.Pack=0 and psdto.stockno is null  " + str8 + "  " + str1 + " " + str2 + @" 
                        " + str3 + " " + str4 + " " + str5 + " " + str6 + "  " + str7 + @"
                         Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                         CN.Item_Finished_Id,CN.OrderId,PRD.Length,PRD.Width,PRD.Area,PRM.UnitID,VF.ProdSizeMtr,VF.ProdSizeFt,OM.customerorderno,OM.localorder";

                //                strsql = @"Select '' Stock,VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName+'  '+VF.DesignName+'  '+VF.ColorName+'  '+VF.ShapeName+'  '+Case When PRM.UnitID=1 Then VF.ProdSizeMtr Else VF.ProdSizeFt End Description,
                //                      CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,0 RDI,PRD.Length,PRD.Width,PRD.Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + @") Rate,om.customerorderno,OM.localorder
                //                      From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,
                //                      " + viewname + @" VF,Process_Stock_Detail PSD,CarpetNumber CN,ordermaster om
                //                      Where PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + @" And  CN.Pack=0 And PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And VF.Item_Finished_Id=PRD.Item_Finished_Id And  Prd.orderid=om.orderid and
                //                      PSD.StockNo=CN.StockNo and CN.orderid=Om.orderid And CN.CurrentProStatus=" + DDFromProcess.SelectedValue + " " + str8 + "  " + str1 + " " + str2 + @" 
                //                      " + str3 + " " + str4 + " " + str5 + " " + str6 + "  " + str7 + @" Group By VF.CATEGORY_NAME,VF.Item_Name,VF.QualityName,VF.DesignName,VF.ColorName,VF.ShapeName,
                //                      CN.Item_Finished_Id,CN.OrderId,PRD.Length,PRD.Width,PRD.Area,PRM.UnitID,VF.ProdSizeMtr,VF.ProdSizeFt,OM.customerorderno,OM.localorder";

            }
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds;
    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (Session["varcompanyNO"].ToString())
        {
            case "9":
                DGItemDetail.Columns[8].Visible = true;
                break;
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
        if (DDQuality.SelectedIndex > 0)
        {
            str = str + " And vf.qualityId=" + DDQuality.SelectedValue;
        }
        if (DDDesign.SelectedIndex > 0)
        {
            str = str + " And vf.designId=" + DDDesign.SelectedValue;
        }
        if (DDColor.SelectedIndex > 0)
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
            if (DDcaltype.SelectedValue == "0" || DDcaltype.SelectedValue == "2" || DDcaltype.SelectedValue == "3" || DDcaltype.SelectedValue == "4")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Area) * Convert.ToDouble(Rate) * Convert.ToDouble(Qty)));
            }
            if (DDcaltype.SelectedValue == "1")
            {
                _arrpara[15].Value = String.Format("{0:#0.00}", (Convert.ToDouble(Rate) * Convert.ToDouble(Qty)));
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
                string str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value + " ";
                str = str + @" Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

                //Commented By: Rajeev
                //str = @"Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ")";
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            }
            string str1 = @"Insert Into Process_Issue_Detail_" + DDTOProcess.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ")";
            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

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
            }
            UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(_arrpar[2].Value), Convert.ToInt32(_arrpar[3].Value), Tran);
            Tran.Commit();
            fill_DetailGride();
            fill_gride();
            Fill_Grid_Total();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Data Successfully Saved.......";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
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
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) as StockNo,CancelQty,Om.Localorder,
                        case When psd.receiveDetailid=0 Then 'N' ELse 'Y' End as Isreceived
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,ordermaster Om,Process_stock_Detail psd Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id AND PD.orderid=Om.orderid And PD.issue_Detail_id=psd.issuedetailid and psd.toprocessid=" + DDTOProcess.SelectedValue + " And PM.IssueOrderid=" + ViewState["IssueOrderid"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + @"
                         And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
            }
            else
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else case when PM.Unitid=6 then IsNull(SizeInch,0) Else SizeFt End End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) as StockNo,CancelQty,OM.Localorder,case When psd.receiveDetailid=0 Then 'N' ELse 'Y' End as Isreceived 
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,orderMaster om,process_stock_detail psd Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id ANd PD.orderid=Om.orderid And PD.issue_Detail_id=psd.issuedetailid and psd.toprocessid=" + DDTOProcess.SelectedValue + " And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @"))  as StockNo,CancelQty,Om.Localorder,
                        case When psd.receiveDetailid=0 Then 'N' ELse 'Y' End as Isreceived
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,ordermaster Om,Process_stock_Detail psd  Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id AND PD.orderid=Om.orderid And PD.issue_Detail_id=psd.issuedetailid and psd.toprocessid=" + DDTOProcess.SelectedValue + " And PM.IssueOrderid=" + ViewState["IssueOrderid"] + "  And PM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
            }
            else
            {
                sqlstr = @"Select Issue_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * From [dbo].[Get_StockNoIssue_Detail_Wise](Issue_Detail_Id," + DDTOProcess.SelectedValue + @")) as StockNo,CancelQty,OM.Localorder,case When psd.receiveDetailid=0 Then 'N' ELse 'Y' End as Isreceived
                        From PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + @" PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + @" PD,
                        ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,orderMaster om,process_stock_detail psd Where PM.IssueOrderid=PD.IssueOrderid And PD.Item_Finished_id=IPM.Finishedid And 
                        IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id ANd PD.orderid=Om.orderid And PD.issue_Detail_id=psd.issuedetailid and psd.toprocessid=" + DDTOProcess.SelectedValue + " And PM.IssueOrderid=" + ViewState["IssueOrderid"] + " And PM.CompanyId=" + DDCompanyName.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order By Issue_Detail_Id Desc";
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
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
        FillDesign();
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
            if (Session["VarcompanyId"].ToString() == "8")//For SlipPrint For Anisha
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
            else if (Session["varcompanyId"].ToString() == "9")
            {
                str = @" Delete TEMP_PROCESS_ISSUE_MASTER 
                       Delete TEMP_PROCESS_ISSUE_DETAIL ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["processId"] + " from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight)Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                Report();
            }
            else
            {

                if (ChkReceiveDetail.Checked == false)
                {


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

                }
                else
                {
                    str = @"select Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
                                ,Ei.EmpId,EI.Empname,EI.address,EI.Address2,EI.Address3,EI.Mobile,Ei.GSTNo as Empgstin,PID.issueorderid
                                ,PIM.Receivedate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + DDTOProcess.SelectedValue + @") as Job,
                                Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                                PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
                                (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + DDTOProcess.SelectedValue + @",PID.issue_detail_id)) TStockNo
                                From PROCESS_Receive_MASTER_" + DDTOProcess.SelectedValue + " PIM inner Join PROCESS_Receive_DETAIL_" + DDTOProcess.SelectedValue + @" PID on PIM.Process_Rec_Id=PID.Process_Rec_Id
                                inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
                                inner Join EmpInfo EI on PIM.Empid=EI.empid
                                inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                                 Where PID.issueorderid=" + ViewState["IssueOrderid"] + " order by Issue_Detail_id";
                    ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                        Session["GetDataset"] = ds;
                        Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2.xsd";
                        StringBuilder stb = new StringBuilder();
                        stb.Append("<script>");
                        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                    }
                    else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }

                }
                //                str = @" Delete TEMP_PROCESS_ISSUE_MASTER 
                //                         Delete TEMP_PROCESS_ISSUE_DETAIL 
                //                         Delete from TempReceiveDetail";

                //                str = str + " Insert into TEMP_PROCESS_ISSUE_MASTER(IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight,PROCESSID) Select IssueOrderId,Empid,AssignDate,Status,UnitId,UserId,Remarks,Instruction,Companyid,CalType,Liasoning,Inspection,SampleNumber,Freight,Insurance,PaymentAt,Destination,SupplyOrderNo,FlagFixOrWeight," + ViewState["processId"] + " from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"] + " ";
                //                str = str + " Insert into TEMP_PROCESS_ISSUE_DETAIL(Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,EstimatedWeight)Select Issue_Detail_Id,IssueOrderId,Item_Finished_Id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,ArticalNo,QualityCodeId,RejectQty,CancelQty,Approvalflag,estimatedweight from PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " Where IssueOrderId=" + ViewState["IssueOrderid"];

                //                if (variable.VarNewQualitySize == "1")
                //                {
                //                    str = str + " insert into TempReceiveDetail(TStockNo,ReceiveDate,Issueorderid,IssueDetailid,Processid,ChallanNo,Rate,Area,ItemDescription) select CN.TStockNo,PSD.ReceiveDate,PRD.IssueOrderId,PRd.Issue_Detail_Id," + DDTOProcess.SelectedValue + " as ProcessId,PRM.ChallanNo,PRD.Rate,PRD.Area,vf.qualityName+' '+vf.designname+' '+vf.colorname+' '+vf.shapename+' '+PRD.Width+'x'+PRD.Length as ItemDescription from PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + " PRD on PRM.Process_Rec_Id=PRd.Process_Rec_Id inner join Process_Stock_Detail PSd on Psd.ToProcessId=" + DDTOProcess.SelectedValue + " and Psd.receivedetailid=PRd.Process_Rec_Detail_Id inner join CarpetNumber CN on PSd.StockNo=CN.stockNo inner join V_FinishedItemDetailNew vf on Prd.Item_Finished_Id=vf.ITEM_FINISHED_ID  Where PRD.IssueOrderid=" + ViewState["IssueOrderid"];
                //                }
                //                else
                //                {
                //                    str = str + " insert into TempReceiveDetail(TStockNo,ReceiveDate,Issueorderid,IssueDetailid,Processid,ChallanNo,Rate,Area,ItemDescription) select CN.TStockNo,PSD.ReceiveDate,PRD.IssueOrderId,PRd.Issue_Detail_Id," + DDTOProcess.SelectedValue + " as ProcessId,PRM.ChallanNo,PRD.Rate,PRD.Area,vf.qualityName+' '+vf.designname+' '+vf.colorname+' '+vf.shapename+' '+PRD.Width+'x'+PRD.Length as ItemDescription from PROCESS_RECEIVE_MASTER_" + DDTOProcess.SelectedValue + " PRM inner join PROCESS_RECEIVE_DETAIL_" + DDTOProcess.SelectedValue + " PRD on PRM.Process_Rec_Id=PRd.Process_Rec_Id inner join Process_Stock_Detail PSd on Psd.ToProcessId=" + DDTOProcess.SelectedValue + " and Psd.receivedetailid=PRd.Process_Rec_Detail_Id inner join CarpetNumber CN on PSd.StockNo=CN.stockNo inner join V_FinishedItemDetail vf on Prd.Item_Finished_Id=vf.ITEM_FINISHED_ID  Where PRD.IssueOrderid=" + ViewState["IssueOrderid"];
                //                }
                //                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

                //                if (ChkReceiveDetail.Checked == true)
                //                {
                //                    Session["ReportPath"] = "Reports/NextProcessIssueWithRecDetail.rpt";
                //                }
                //                else
                //                {
                //                    Session["ReportPath"] = "Reports/NextProcessIssue.rpt";
                //                }

                //                Session["CommanFormula"] = "{V_NextProcessIssueReport.IssueOrderId}=" + ViewState["IssueOrderid"] + "";
                //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
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
        //        string qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo,VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VPIR.QualityName,
        //        VPIR.designName,VPIR.ColorName,VPIR.ShadeColorName,VPIR.ShapeName,VPIR.Instruction,VPIR.Qty,VPIR.Rate,VPIR.Area,VPIR.IssueOrderId,VPIR.AssignDate,
        //        PNM.PROCESS_NAME,VPIR.UnitId,VPIR.Length,VPIR.Width,VPIR.TStockNo,CustomerOrderNo," + Session["VarcompanyNo"] + @" As CompanyNo,Customercode,UnitName,LocalOrder
        //        FROM PROCESS_NAME_MASTER PNM INNER JOIN V_NextProcessIssueReport VPIR ON PNM.PROCESS_NAME_ID=VPIR.PROCESSID 
        //        INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId INNER JOIN V_EmployeeInfo VEI ON VPIR.Empid=VEI.EmpId Where VPIR.IssueOrderId=" + ViewState["IssueOrderid"] + " And PNM.MasterCompanyId=" + Session["varCompanyId"] + " And VCI.CompanyId=" + DDCompanyName.SelectedValue;

        string qry = @"SELECT VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo,VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VPIR.QualityName,
                    VPIR.designName,VPIR.ColorName,VPIR.ShadeColorName,VPIR.ShapeName,VPIR.Instruction,Sum(VPIR.Qty) as Qty,VPIR.Rate,Sum(VPIR.Area) Area
                    ,VPIR.IssueOrderId,VPIR.AssignDate,
                    PNM.PROCESS_NAME,VPIR.UnitId,VPIR.Length,VPIR.Width,CustomerOrderNo," + Session["VarcompanyNo"] + @" As CompanyNo,Customercode,UnitName,LocalOrder,VCI.GSTIN,VEI.EMPGSTIN
                    FROM PROCESS_NAME_MASTER PNM INNER JOIN V_NextProcessIssueReport VPIR ON PNM.PROCESS_NAME_ID=VPIR.PROCESSID 
                    INNER JOIN V_Companyinfo VCI ON VPIR.Companyid=VCI.CompanyId INNER JOIN V_EmployeeInfo VEI 
                    ON VPIR.Empid=VEI.EmpId Where VPIR.IssueOrderId=" + ViewState["IssueOrderid"] + " And PNM.MasterCompanyId=" + Session["VarcompanyNo"] + " And VCI.CompanyId=" + DDCompanyName.SelectedValue + @"
                    group by VCI.CompanyName,VCI.CompanyAddress,VCI.CompanyPhoneNo,VCI.CompanyFaxNo,VCI.TinNo,VEI.EmpName,VEI.EmpAddress,VEI.EmpPhoneNo,VPIR.QualityName,
                    VPIR.designName,VPIR.ColorName,VPIR.ShadeColorName,VPIR.ShapeName,VPIR.Instruction,VPIR.Rate,VPIR.IssueOrderId,VPIR.AssignDate,
                    PNM.PROCESS_NAME,VPIR.UnitId,VPIR.Length,VPIR.Width,CustomerOrderNo,Customercode,UnitName,LocalOrder,VCI.GSTIN,VEI.EMPGSTIN";

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

    protected void BtnSaveCarpet_Click(object sender, EventArgs e)
    {
        string st = DGStock.SelectedIndex.ToString();
    }
    protected void DGStock_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = DGStock.Rows[index];
        string carpetNo = row.Cells[1].Text;
        string Rate = ((TextBox)row.Cells[2].FindControl("TxtCarpetRate")).Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from CarpetNumber Where TStockNo='" + carpetNo + "'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "";
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CompanyId"]) != Convert.ToInt32(DDCompanyName.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Company...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["issRecStatus"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Issued...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CurrentProStatus"]) != Convert.ToInt32(DDFromProcess.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Process...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Pack"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Packed....";
                TxtStockNo.Focus();
                return;
            }
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Pls Check Stock No....";
            TxtStockNo.Focus();
            return;
        }
        if (Rate == "" || Rate == "0")
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Pls Enter Rate....";
        }
        else
        {
            Issue_CarpetWise(carpetNo, Rate);
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
                SqlParameter[] _arrpara = new SqlParameter[23];
                DataSet DS = null;
                if (Session["varCompanyId"].ToString() == "4")
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select CN.StockNo,PRD.Length,PRD.Width,PRD.Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @") Rate,
                             CN.OrderId,CN.Item_Finished_ID,PRM.UnitID,VF.ShapeId From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @"  PRD,Process_Stock_Detail PSD,CarpetNumber CN,V_FinishedItemDetailNew VF
                             Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.Item_Finished_Id And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And  TSTOCKNO='" + carpetNo + "' And PRM.Companyid=" + DDCompanyName.SelectedValue + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select CN.StockNo,PRD.Length,PRD.Width,PRD.Area,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDFromProcess.SelectedValue + @") Rate,
                             CN.OrderId,CN.Item_Finished_ID,PRM.UnitID,VF.ShapeId From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @"  PRD,Process_Stock_Detail PSD,CarpetNumber CN,V_FinishedItemDetail VF
                             Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.Item_Finished_Id And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And  TSTOCKNO='" + carpetNo + "' And PRM.Companyid=" + DDCompanyName.SelectedValue + "");
                    }
                }
                else if (Session["varCompanyId"].ToString() == "5")
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then VF.WidthMtr Else VF.WidthFt End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.AreaMtr Else VF.AreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then VF.WidthMtr Else VF.WidthFt End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.LengthMtr Else VF.LengthFt End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.AreaMtr Else VF.AreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
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
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(LEFT(VF.SizeInch, CHARINDEX('x', VF.SizeInch) - 1),0) as varchar) Else cast(VF.ProdWidthFt  as varchar) End End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else Case When " + DDUnit.SelectedValue + @"=6 Then cast(IsNull(REPLACE(SUBSTRING(VF.SizeInch, CHARINDEX('x', VF.SizeInch), LEN(VF.SizeInch)), 'x', ''),0) as varchar) Else cast(VF.ProdLengthFt as varchar) End End Length,
                        Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetail VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And PRM.CompanyId=" + DDCompanyName.SelectedValue + "");
                    }
                }
                else
                {
                    if (variable.VarNewQualitySize == "1")
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else cast(VF.ProdWidthFt as varchar) End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else cast(VF.ProdLengthFt as varchar) End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetailNew VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                    else
                    {
                        DS = SqlHelper.ExecuteDataset(Tran, CommandType.Text, @"Select TSTOCKNO,CN.StockNo,Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdWidthMtr as varchar) Else cast(VF.ProdWidthFt as varchar) End Width,
                        Case When " + DDUnit.SelectedValue + @"=1 Then cast(VF.ProdLengthMtr as varchar) Else cast(VF.ProdLengthFt as varchar) End Length,Case When " + DDUnit.SelectedValue + @"=1 Then VF.ProdAreaMtr Else VF.ProdAreaFt End Area,
                        [dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + DDTOProcess.SelectedValue + ") Rate,CN.OrderId,CN.Item_Finished_ID," + DDUnit.SelectedValue + @" UnitID,VF.ShapeId 
                        From Process_Receive_MASTER_" + DDFromProcess.SelectedValue + " PRM,Process_Receive_Detail_" + DDFromProcess.SelectedValue + @" PRD,Process_Stock_Detail PSD,
                        CarpetNumber CN,V_FinishedItemDetail VF Where PRM.Process_Rec_ID=PRD.Process_Rec_ID And PRD.Process_Rec_Detail_Id=PSD.ReceiveDetailId And 
                        PSD.StockNo=CN.StockNo And PRD.Item_Finished_Id=VF.ITEM_FINISHED_ID And PSD.ToProcessID=" + DDFromProcess.SelectedValue + " And TSTOCKNO='" + carpetNo + "' And PRM.CompanyId=" + DDCompanyName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "");
                    }
                }
                //Rate = DS.Tables[0].Rows[0]["Rate"].ToString();
                Rate = UtilityModule.PROCESS_RATE(Convert.ToInt32(DS.Tables[0].Rows[0]["Item_Finished_ID"]), Convert.ToInt32(DS.Tables[0].Rows[0]["OrderId"]), Convert.ToInt32(DDTOProcess.SelectedValue), Tran, mastercompanyid: Convert.ToInt16(Session["varcompanyId"]), effectivedate: TxtIssueDate.Text, Caltype: Convert.ToInt16(DDcaltype.SelectedValue)).ToString();
                int VarStockNo = Convert.ToInt32(DS.Tables[0].Rows[0]["StockNo"]);
                if (Session["varCompanyId"].ToString() == "4")
                {
                    if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                    {
                        Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                        Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                        Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
                    }
                    else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                    {
                        Length = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Length"])).ToString();
                        Width = UtilityModule.ConvertMtrToFt(Convert.ToDouble(DS.Tables[0].Rows[0]["Width"])).ToString();
                        Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
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

                        LengthInch = UtilityModule.ConvertInchesToFt(Convert.ToInt32(DS.Tables[0].Rows[0]["Length"])).ToString();
                        WidthInch = UtilityModule.ConvertInchesToFt(Convert.ToInt32(DS.Tables[0].Rows[0]["Width"])).ToString();
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
                    Length = Convert.ToString(DS.Tables[0].Rows[0]["Length"]);
                    Width = Convert.ToString(DS.Tables[0].Rows[0]["Width"]);
                    if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 2)
                    {
                        Area = UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]), Processid: Convert.ToInt16(DDTOProcess.SelectedValue));
                    }
                    else if (Convert.ToInt32(DS.Tables[0].Rows[0]["UnitID"]) == 1)
                    {
                        Area = UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Length), Convert.ToDouble(Width), Convert.ToInt32(DDcaltype.SelectedValue), Convert.ToInt32(DS.Tables[0].Rows[0]["ShapeId"]));
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
                int num;
                string ChallanNo = "";
                if (Convert.ToUInt32(ViewState["IssueOrderid"]) == 0 || (DDTOProcess.SelectedValue != (ViewState["processId"]).ToString()))
                {
                    int a = 0;
                    switch (variable.VarMaintainProcessissueSeq)
                    {
                        case "0":
                            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from Process_issue_Master_" + DDTOProcess.SelectedValue + ""));
                            break;
                        default:
                            a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(IssueOrderid ),0)+1 from MasterSetting"));

                            break;
                    }
                    ViewState["IssueOrderid"] = a;


                    if (variable.VarCompanyWiseChallanNoGenerated == "1")
                    {
                        SqlConnection con2 = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                        if (con2.State == ConnectionState.Closed)
                        {
                            con2.Open();
                        }
                        SqlTransaction tran2 = con2.BeginTransaction();
                        try
                        {
                            SqlParameter[] param = new SqlParameter[6];
                            param[0] = new SqlParameter("@TranType", SqlDbType.Int);
                            param[1] = new SqlParameter("@IssueRecNo", SqlDbType.VarChar, 100);
                            param[2] = new SqlParameter("@TableName", SqlDbType.VarChar, 300);
                            param[3] = new SqlParameter("@TableId", SqlDbType.Int);
                            param[4] = new SqlParameter("@CompanyId", SqlDbType.Int);
                            param[5] = new SqlParameter("@VarCompanyId", SqlDbType.Int);

                            param[0].Value = 0;
                            param[1].Direction = ParameterDirection.Output;
                            param[2].Value = "Process_Issue_Master_" + DDTOProcess.SelectedValue;
                            param[3].Value = ViewState["IssueOrderid"];
                            param[4].Value = DDCompanyName.SelectedValue;
                            param[5].Value = Session["varCompanyNo"];

                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GatePassNew", param);

                            ChallanNo = param[1].Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            tran2.Rollback();
                            LblErrorMessage.Text = ex.Message;
                            LblErrorMessage.Visible = true;
                        }
                        finally
                        {
                            con2.Close();
                            con2.Dispose();
                        }

                    }
                    else
                    {
                        ChallanNo = ViewState["IssueOrderid"].ToString();
                    }

                    TxtChallanNO.Text = ChallanNo;

                    ////TxtChallanNO.Text = ViewState["IssueOrderid"].ToString();
                    num = 1;

                }
                else
                {
                    num = 0;

                    ChallanNo = ViewState["IssueOrderid"].ToString();
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
                _arrpara[10].Value = DS.Tables[0].Rows[0]["Item_Finished_id"];

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

                ViewState["processId"] = Convert.ToInt32(DDTOProcess.SelectedValue);

                if (num == 1)
                {
                    string str = "";
                    switch (variable.VarMaintainProcessissueSeq)
                    {
                        case "1":
                            str = @"Update MasterSetting Set IssueOrderid =" + _arrpara[0].Value + " ";
                            break;
                        default:
                            break;
                    }
                    str = str + @" Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,ChallanNo) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ",'" + ChallanNo + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);


                }
                DataSet DS1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from Process_Issue_Master_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + _arrpara[0].Value + "");
                if (DS1.Tables[0].Rows.Count == 0)
                {
                    string ABC = @"Insert Into Process_Issue_Master_" + DDTOProcess.SelectedValue + "(IssueOrderid,Empid,AssignDate,Status,UnitId,Userid,Remarks,Instruction,Companyid,CalType,ChallanNo) Values (" + _arrpara[0].Value + ",'" + _arrpara[1].Value + "','" + _arrpara[2].Value + "','" + _arrpara[3].Value + "'," + _arrpara[4].Value + "," + _arrpara[5].Value + ",'" + _arrpara[6].Value + "','" + _arrpara[7].Value + "'," + _arrpara[8].Value + "," + _arrpara[22].Value + ",'" + ChallanNo + "')";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, ABC);
                }
                string str1 = @"Insert Into Process_Issue_Detail_" + DDTOProcess.SelectedValue + "(Issue_Detail_Id,IssueOrderid,Item_Finished_id,Length,Width,Area,Rate,Amount,Qty,ReqByDate,PQty,Comm,CommAmt,Orderid,CancelQty) values(" + _arrpara[9].Value + "," + _arrpara[0].Value + "," + _arrpara[10].Value + ",'" + _arrpara[11].Value + "','" + _arrpara[12].Value + "'," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + ",'" + _arrpara[17].Value + "'," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + ",0)";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

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

                _arrpar[0].Value = VarStockNo;
                _arrpar[1].Value = DDFromProcess.SelectedValue;
                _arrpar[2].Value = DDTOProcess.SelectedValue;
                _arrpar[3].Value = DS.Tables[0].Rows[0]["OrderId"];
                _arrpar[4].Value = TxtIssueDate.Text;
                _arrpar[5].Value = TxtReqDate.Text;
                _arrpar[6].Value = DDCompanyName.SelectedValue;
                _arrpar[7].Value = 0;
                _arrpar[8].Value = Session["varuserid"];
                _arrpar[9].Value = _arrpara[9].Value;
                _arrpar[10].Value = 1;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProcessStockDetail", _arrpar);
                UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(_arrpara[10].Value), Convert.ToInt32(_arrpar[2].Value), Convert.ToInt32(_arrpar[3].Value), Tran);
                Tran.Commit();
                fill_DetailGride();
                Fill_StockGride();
                Fill_Grid_Total();
                TxtStockNo.Text = "";
                TxtStockNo.Focus();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Successfully Saved.......";
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
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
                string str = @"Update CarpetNumber Set CurrentProStatus=PSD.FromProcessId,IssRecStatus=0 From Process_Stock_Detail PSD Where CarpetNumber.StockNo=PSD.StockNo And IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + "";
                str = str + " Delete Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + "";
                str = str + " Delete Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where Issue_Detail_Id=" + VarProcess_Issue_Detail_Id + "";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * from Process_Stock_Detail Where StockNo in (Select StockNo From Process_Stock_Detail Where IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + ")");
                if (Ds.Tables[0].Rows.Count == 1)
                {
                    str = @"Update CarpetNumber Set CurrentProStatus=PSD.ToProcessId,IssRecStatus=0 From Process_Stock_Detail PSD Where CarpetNumber.StockNo=PSD.StockNo And IssueDetailId=" + VarProcess_Issue_Detail_Id + " And ToProcessId=" + DDTOProcess.SelectedValue + "";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                }
                Tran.Commit();
                fill_DetailGride();
                Fill_Grid_Total();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/FrmEditNextIssue.aspx");
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
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER Where ProductCode Like  '" + prefixText + "%' And MasterCompanyId=" + MasterCompanyId + "";
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
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        save_detail();
        Focus = "TxtStockNo";
    }
    protected void save_detail()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CompanyId,issRecStatus,CurrentProStatus,Pack from CarpetNumber Where TStockNo='" + TxtStockNo.Text + "'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "";
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CompanyId"]) != Convert.ToInt32(DDCompanyName.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Company...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["issRecStatus"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Issued...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CurrentProStatus"]) != Convert.ToInt32(DDFromProcess.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Process...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Pack"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Packed....";
                TxtStockNo.Focus();
            }
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Pls Check Stock No....";
            TxtStockNo.Focus();
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
    protected void TxtChallanNO_TextChanged(object sender, EventArgs e)
    {
        if (TxtChallanNO.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * From TEMP_PROCESS_ISSUE_MASTER_NEW Where CompanyId = " + DDCompanyName.SelectedValue + " And IssueOrderId=" + TxtChallanNO.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DDTOProcess.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                DDTOProcess_SelectedIndexChanged(sender, new EventArgs());
                DDContractor.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                ContractorSelectedChange();
                DDChallanNo.SelectedValue = Ds.Tables[0].Rows[0]["IssueOrderId"].ToString();
                ChallanNoSelectedChange();
            }
            else
            {
                if (DDChallanNo.Items.Count > 0)
                {
                    DDChallanNo.SelectedIndex = 0;
                    ChallanNoSelectedChange();
                }
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Challan No";
                TxtChallanNO.Text = "";
                TxtChallanNO.Focus();
            }
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        ViewState["IssueOrderid"] = DDChallanNo.SelectedValue;
        string Str = "Select replace(convert(varchar(11),AssignDate,106), ' ','-') AssignDate,UnitId,CalType,Remarks,Instruction,replace(convert(varchar(11),ReqbyDate,106), ' ','-') ReqbyDate,fromProcessId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " PM,PROCESS_ISSUE_DETAIL_" + DDTOProcess.SelectedValue + " PD,Process_Stock_Detail PSD Where PM.IssueOrderId=PD.IssueOrderId And PSD.IssueDetailID=PD.Issue_Detail_Id And PD.IssueOrderId=" + DDChallanNo.SelectedValue + " and PSD.ToProcessId=" + DDTOProcess.SelectedValue + " And PM.CompanyId=" + DDCompanyName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDFromProcess, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"] + " Order By PROCESS_NAME", true, "--Select Process--");
            DDFromProcess.SelectedValue = Ds.Tables[0].Rows[0]["fromProcessId"].ToString();
            TxtIssueDate.Text = Ds.Tables[0].Rows[0]["AssignDate"].ToString();
            TxtReqDate.Text = Ds.Tables[0].Rows[0]["ReqbyDate"].ToString();
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["CalType"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
        }
        fill_DetailGride();
        Fill_Grid_Total();
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
    protected void BtnCurrentConsumptionSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select * From Process_Issue_Detail_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + DDChallanNo.SelectedValue);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    UtilityModule.PROCESS_CONSUMPTION_DEFINE(Convert.ToInt32(Ds.Tables[0].Rows[i]["IssueOrderid"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["Issue_Detail_Id"]), Convert.ToInt32(Ds.Tables[0].Rows[i]["Item_Finished_id"]), Convert.ToInt32(DDTOProcess.SelectedValue), Convert.ToInt32(Ds.Tables[0].Rows[i]["Orderid"]), Tran);
                }
            }
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Consumption Successfully Changed.......";
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/EditProcessIssue.aspx");
            Tran.Rollback();
            LblErrorMessage.Text = "";
            LblErrorMessage.Visible = true;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"] != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"] + " And Companyid=" + DDCompanyName.SelectedValue + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                if (DDUnit.SelectedValue != Ds.Tables[0].Rows[0]["UnitId"])
                {
                    DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no has only one unitid !');", true);
                }
            }
        }
    }
    protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["IssueOrderid"] != "0")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_ISSUE_MASTER_" + DDTOProcess.SelectedValue + " Where IssueOrderid=" + ViewState["IssueOrderid"] + " And CompanyId=" + DDCompanyName.SelectedValue + "");
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
        if (DDQuality.SelectedIndex > 0)
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
        if (DDQuality.SelectedIndex > 0)
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
                param[3] = new SqlParameter("@issueorderid", DDChallanNo.SelectedValue);
                //****
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextIssueBulk", param);
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[1].Value.ToString();
                Tran.Commit();
                fill_DetailGride();
                Fill_Grid_Total();


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
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisreceived = (Label)e.Row.FindControl("lblisreceived");
            CheckBox chkboxitem = (CheckBox)e.Row.FindControl("chkboxitem");
            if (lblisreceived.Text == "Y")
            {
                e.Row.BackColor = System.Drawing.Color.Green;
                chkboxitem.Enabled = false;

            }
        }
    }
}