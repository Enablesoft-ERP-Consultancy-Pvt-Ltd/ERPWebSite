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
using ClosedXML.Excel;

public partial class Masters_Packing_FrmPacking : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
 @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Packing_FrmPacking),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            logo();
            tdsubquality.Visible = false;
            string Qry = @" select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                    Select CustomerId,CustomerCode + SPACE(5)+CompanyName From CustomerInfo Where MasterCompanyId=" + Session["varCompanyId"] + @" order by CustomerCode
                    Select CurrencyId,CurrencyName from CurrencyInfo Where MasterCompanyId=" + Session["varCompanyId"] + @"
                    Select UnitId,UnitName from Unit where UnitId in(1,2)";
            DataSet ds1 = null;
            ds1 = SqlHelper.ExecuteDataset(Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds1, 0, true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds1, 1, true, "--SELECT--");
            CustomerCodeSelectedIndexChange();
            UtilityModule.ConditionalComboFillWithDS(ref DDCurrency, ds1, 2, true, "--SELECT--");
            if (DDCurrency.Items.Count > 0)
            {
                DDCurrency.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds1, 3, true, "--SELECT--");
            if (DDUnit.Items.Count > 0)
            {
                DDUnit.SelectedIndex = 1;
            }
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ParameteLabel();
            RDAreaWise.Checked = true;
            int VarProdCode = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VarProdCode From MasterSetting"));
            if (VarProdCode == 1)
            {
                TDProdCode.Visible = true;
            }
            else
            {
                TDProdCode.Visible = false;
            }
            CheckedMultipleRolls();
            // ViewState["PackingID"] = 0;
            //Session["PackingID"] = 0;
            ViewState["PACKINGDETAILID"] = 0;
            //DDCustomerCode.Focus();
            hnid.Value = "0";
            hnpackingid.Value = "0";
            hnfinished.Value = "";

            //if (Session["varcompanyId"].ToString() == "19")
            //{
            //    Label36.Text = "Bale Nt Wt";
            //    Label37.Text = "Bale Gr Wt";
            //}
            //else
            //{
            //    Label36.Text = "One Pcs Nt Wt";
            //    Label37.Text = "One Pcs Gr Wt";
            //}

            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 19:
                    Label36.Text = "Bale Nt Wt";
                    Label37.Text = "Bale Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = false;
                    break;
                case 30:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = false;
                    break;
                case 16:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = true;
                    break;
                case 36:
                    Label39.Text = "Collection No.";
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = false;                    
                    break;
                case 42:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = false;
                    BtnPreview.Visible = true;
                    break;
                default:
                    Label36.Text = "One Pcs Nt Wt";
                    Label37.Text = "One Pcs Gr Wt";
                    TDRatePerPcs.Visible = false;
                    TDCheckForWithoutStockNo.Visible = false;
                    break;

            }
        }
    }
    private void logo()
    {
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    private void ParameteLabel()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varCompanyId"]));
        lblQualityName.Text = ParameterList[0];
        lblDesignName.Text = ParameterList[1];
        lblColorName.Text = ParameterList[2];
        lblShapeName.Text = ParameterList[3];
        lblSizeName.Text = ParameterList[4];
        lblCategoryName.Text = ParameterList[5];
        lblItemName.Text = ParameterList[6];
        lblShade.Text = ParameterList[7];
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChange();
        // TxtInvoiceNo.Focus();
    }
    private void CustomerCodeSelectedIndexChange()
    {
        UtilityModule.ConditionalComboFill(ref DDConsignee, "Select CustomerId,CompanyName From  CustomerInfo where CustomerID=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By Companyname", true, "--SELECT--");
        if (DDConsignee.Items.Count > 0)
        {
            DDConsignee.SelectedIndex = 1;
        }
        FillorderNo();

        if (ChkForEdit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddInvoiceNo, "Select PackingID,TPackingNo+' / '+Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingNo from Packing Where ConsignorId=" + DDCompanyName.SelectedValue + " And ConsigneeId=" + DDCustomerCode.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
        }
    }
    protected void FillorderNo()
    {
        if (variable.Carpetcompany == "1")
        {
            if (Session["varCompanyId"].ToString() == "30")
            {
                string str = "";
                str = @"Select distinct OM.OrderId, OM.CustomerOrderNo CustomerOrderNo from OrderMaster OM JOIN OrderDetail OD ON OM.OrderId=OD.OrderId
                        JOIN V_OrderQtyAndPackedQty VO ON OD.OrderId=VO.OrderId and OD.Item_Finished_Id=VO.Item_Finished_ID 
                        Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + @" and (VO.OrderQty-VO.PackedQty)>0  order by CustomerOrderNo";

                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, str, true, "--SELECT--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select OrderId,CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + "  order by CustomerOrderNo", true, "--SELECT--");
            }
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, "Select OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster Where CompanyId=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " order by OrderId", true, "--SELECT--");
        }
    }
    protected void ChkForMulipleRolls_CheckedChanged(object sender, EventArgs e)
    {
        ChkForMulipleRolls_CheckedChanged();
    }
    private void ChkForMulipleRolls_CheckedChanged()
    {
        TxtRollNoFrom.Text = "";
        TxtRollNoTo.Text = "0";
        TxtTotalPcs.Text = "0";
        TxtPcsPerRoll.Text = "0";
        TxtBales.Text = "";
        CheckedMultipleRolls();
        FillorderNo();
    }
    private void CheckedMultipleRolls()
    {
        if (ChkForMulipleRolls.Checked == true)
        {
            TDStockNo.Visible = false;
            TxtRollNoTo.Enabled = true;

            TxtPcsPerRoll.Enabled = true;

            TxtBales.Enabled = true;
        }
        else
        {
            TDStockNo.Visible = true;
            TxtRollNoTo.Enabled = false;
            TxtTotalPcs.Enabled = false;
            TxtBales.Enabled = false;
        }
    }
    protected void ChkForWithoutOrder_CheckedChanged(object sender, EventArgs e)
    {
        ChkForWithoutOrder_CheckedChanged();
    }
    private void ChkForWithoutOrder_CheckedChanged()
    {
        if (ChkForWithoutOrder.Checked == true)
        {
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
                DDCustomerOrderNo.Enabled = false;
            }
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct Category_ID,Category_Name from CarpetNumber CR ,V_FinishedItemDetail VF  Where CR.Item_Finished_ID=VF.Item_Finished_Id And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "-Select-");
        }
        else
        {
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
            }
            DDCustomerOrderNo.Enabled = true;
            ddCategoryName.Items.Clear();
        }
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        hnCarpetNoTypeId.Value = "1";
        string Str = "";
        int VarNumber = 0;
        DataSet Ds;
        LblErrorMessage.Text = "";
        hnsampletype.Value = "1";
        Trstockmsg.Visible = false;
        if (TxtStockNo.Text != "")
        {
            //2 ordercategory means sample as well as Direct stock
            Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Distinct CN.Pack,CN.IssRecStatus,CN.Orderid,CN.TypeId,VF.*,Om.CustomerId,isnull(OD.CQID,0) as CQID,isnull(OD.DSRNO,0) as DSRNO,isnull(OD.CSRNO,0) as CSRNO,
                 isnull(OM.OrderCategoryId,2) as OrderCategory,CAST(CONVERT(CHAR(11), CN.REC_DATE, 113) AS DATETIME) as Rec_Date,isnull(Ci.customercode,'STOCK') as Customercode,isnull(Om.customerorderno,'STOCK') as OrderNo,isnull(PRD.QualityTYpe,1) as QualityType
                 from V_FinishedItemDetail VF,CarpetNumber CN left outer join OrderMaster OM on CN.orderId=OM.OrderId 
                 left join orderDetail od on OM.orderid=OD.OrderId and cn.Item_Finished_Id=od.Item_Finished_Id
                 left join customerinfo Ci on Om.customerid=CI.customerid
                 Left join Process_receive_detail_1 PRD on CN.Process_rec_detail_id=PRD.Process_rec_detail_id
                 Where CN.Item_Finished_Id=VF.Item_Finished_Id And TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + " And CN.Item_Finished_Id=Vf.Item_Finished_Id");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                Trstockmsg.Visible = true;
                lblstockmsg.Text = "Customer Code : " + Ds.Tables[0].Rows[0]["Customercode"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + " Order No.  : " + Ds.Tables[0].Rows[0]["orderno"].ToString();

                if (Ds.Tables[0].Rows[0]["Pack"].ToString() == "1")
                {
                    VarNumber = 1;
                    LblErrorMessage.Text = "Already Packed";
                    TxtStockNo.Text = "";
                    //TxtStockNo.Focus();
                    return;
                }
                if (Ds.Tables[0].Rows[0]["QualityType"].ToString() == "3" || Ds.Tables[0].Rows[0]["QualityType"].ToString() == "2")
                {
                    VarNumber = 1;
                    LblErrorMessage.Text = "This Stock No. is Rejected Or Hold.";
                    TxtStockNo.Text = "";
                    //TxtStockNo.Focus();
                    return;
                }
                if (Ds.Tables[0].Rows[0]["IssRecStatus"].ToString() == "1")
                {
                    VarNumber = 1;
                    LblErrorMessage.Text = "This Stock No Issue To Any Process Pls Receive It";
                    TxtStockNo.Text = "";
                    //TxtStockNo.Focus();
                    return;
                }
                //if (Convert.ToDateTime(Ds.Tables[0].Rows[0]["Rec_Date"]) > Convert.ToDateTime(TxtDate.Text))
                //{
                //    VarNumber = 1;
                //    LblErrorMessage.Text = "Invoice Date can not be greater than Stock Receive Date";
                //    TxtStockNo.Text = "";
                //    //TxtStockNo.Focus();
                //    return;
                //}
                if (variable.Withbuyercode == "1" && ChkForWithoutOrder.Checked == false)
                {
                    hnsampletype.Value = "1";
                }
                else
                {
                    hnsampletype.Value = Ds.Tables[0].Rows[0]["OrderCategory"].ToString();
                    hnCarpetNoTypeId.Value = Ds.Tables[0].Rows[0]["TypeId"].ToString();
                }
                //*********Pack For Sample
                if (chksamplepack.Checked == true)
                {
                    hnsampletype.Value = "2";
                }

                if (VarNumber == 0)
                {
                    Str = "Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo CustomerOrderNo from OrderMaster OM,OrderDetail OD  Where Om.OrderId=OD.OrderId And CompanyId=" + DDCompanyName.SelectedValue + " And Item_Finished_id=" + Ds.Tables[0].Rows[0]["Item_Finished_id"];
                    if (ChkForWithoutOrder.Checked != true)
                    {
                        Str = Str + @" And Customerid=" + DDCustomerCode.SelectedValue;
                    }
                    UtilityModule.ConditionalComboFill(ref DDCustomerOrderNo, Str, true, "--SELECT--");

                    if (DDCustomerOrderNo.Items.Count > 0)
                    {
                        if (Convert.ToInt32(Ds.Tables[0].Rows[0]["orderid"]) > 0)
                        {
                            if (DDCustomerOrderNo.Items.FindByValue(Ds.Tables[0].Rows[0]["orderid"].ToString()) != null)
                            {
                                DDCustomerOrderNo.SelectedValue = Ds.Tables[0].Rows[0]["orderid"].ToString();
                            }
                        }
                        else
                        {

                            DDCustomerOrderNo.SelectedIndex = 1;
                        }
                    }
                    //***************With Buyer Detail
                    if (variable.Withbuyercode == "1" && ChkForWithoutOrder.Checked == false && DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = @"select OD.OrderId,OD.Item_Finished_Id,vf.category_id,vf.item_id,OD.CQID,vf.QualityId,OD.DSRNO,vf.designId,od.csrno,vf.colorid,vf.shapeid,
                            vf.SizeId,vf.ShadecolorId
                            From orderdetail OD inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID 
                            Where Od.OrderId=" + DDCustomerOrderNo.SelectedValue + " and OD.Item_Finished_Id=" + Ds.Tables[0].Rows[0]["Item_Finished_Id"];
                        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

                    }
                    //Str = "Select Distinct Category_ID,Category_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
                    Str = "Select Distinct Category_ID,Category_Name from V_FinishedItemDetail VF left outer join OrderDetail OD on OD.Item_Finished_Id=VF.Item_Finished_Id Where  VF.MasterCompanyId=" + Session["varCompanyId"] + " And Vf.Item_Finished_Id=" + Ds.Tables[0].Rows[0]["Item_Finished_Id"];
                    if (ChkForWithoutOrder.Checked != true)
                    {
                        if (DDCustomerOrderNo.SelectedIndex > 0)
                        {
                            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
                        }
                    }
                    UtilityModule.ConditionalComboFill(ref ddCategoryName, Str, true, "--SELECT--");
                    if (ddCategoryName.Items.Count > 0)
                    {
                        ddCategoryName.SelectedIndex = 1;
                        ddCategoryName.SelectedValue = Ds.Tables[0].Rows[0]["Category_ID"].ToString();
                    }
                    Category_SelectedIndex_Change();
                    ddItemName.SelectedValue = Ds.Tables[0].Rows[0]["Item_ID"].ToString();
                    ItemNameSelectedIndexChange();
                    if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                    {
                        if (ddQuality.Items.FindByValue(Ds.Tables[0].Rows[0]["CQID"].ToString()) != null)
                        {
                            ddQuality.SelectedValue = Ds.Tables[0].Rows[0]["CQID"].ToString();
                        }
                    }
                    else
                    {
                        if (ddQuality.Items.FindByValue(Ds.Tables[0].Rows[0]["QUALITYID"].ToString()) != null)
                        {
                            ddQuality.SelectedValue = Ds.Tables[0].Rows[0]["QUALITYID"].ToString();
                        }
                    }
                    ComboFill();
                    if (TDDesign.Visible == true)
                    {
                        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                        {
                            if (ddDesign.Items.FindByValue(Ds.Tables[0].Rows[0]["Dsrno"].ToString()) != null)
                            {
                                ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["Dsrno"].ToString();
                            }
                        }
                        else
                        {
                            if (ddDesign.Items.FindByValue(Ds.Tables[0].Rows[0]["DesignID"].ToString()) != null)
                            {
                                ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["DesignID"].ToString();
                            }
                        }

                        DesignSelectedChange();
                    }
                    if (TDColor.Visible == true)
                    {
                        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                        {
                            if (ddColor.Items.FindByValue(Ds.Tables[0].Rows[0]["CSRNO"].ToString()) != null)
                            {
                                ddColor.SelectedValue = Ds.Tables[0].Rows[0]["CSRNO"].ToString();
                            }


                        }
                        else
                        {
                            if (ddColor.Items.FindByValue(Ds.Tables[0].Rows[0]["ColorID"].ToString()) != null)
                            {
                                ddColor.SelectedValue = Ds.Tables[0].Rows[0]["ColorID"].ToString();
                            }

                        }
                        ddColor_SelectedIndexChanged(sender, new EventArgs());

                    }
                    
                    if (TDShape.Visible == true)
                    {
                        ddShape.SelectedValue = Ds.Tables[0].Rows[0]["ShapeID"].ToString();
                        ShapeSelectedChange(sizeid: Convert.ToInt16(Ds.Tables[0].Rows[0]["SizeID"]));
                        ddSize.SelectedValue = Ds.Tables[0].Rows[0]["SizeID"].ToString();
                    }
                    if (TDShade.Visible == true)
                    {
                        ddDesign.SelectedValue = Ds.Tables[0].Rows[0]["shadeColorID"].ToString();
                    }
                    SizeSelectedIndexChange();
                    TxtPackQty.Text = "1";
                    //BtnSave.Focus();
                }
            }
            else
            {
                LblErrorMessage.Text = "Stock No Does Not exists Plz Enter Correct Stock No";
                TxtStockNo.Text = "";
                // TxtStockNo.Focus();
            }
        }
        else
        {
            ddCategoryName.SelectedIndex = 0;
            Category_SelectedIndex_Change();
        }
    }
    protected void DDCustomerOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerOrderNo_SelectedIndexChanged();
        // ddCategoryName.Focus();

        if (Session["varCompanyId"].ToString() == "30" || Session["varCompanyId"].ToString() == "16")
        {
            if (DGOrderDetail.Rows.Count == 0)
            {
                string VarUnitID = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
                    @"Select Top 1 OrderUnitID From OrderDetail Where OrderID = " + DDCustomerOrderNo.SelectedValue).ToString();

                DDUnit.SelectedValue = VarUnitID;
            }
        }
    }
    private void DDCustomerOrderNo_SelectedIndexChanged()
    {
        if (TxtStockNo.Text == "")
        {
            UtilityModule.ConditionalComboFill(ref ddCategoryName, "Select Distinct Category_ID, Category_Name from OrderDetail OD,V_FinishedItemDetail VF Where OD.Item_Finished_Id=VF.Item_Finished_Id And Orderid=" + DDCustomerOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");
            if (ChkForMulipleRolls.Checked == true)
            {
                // ddCategoryName.Focus();
            }
            else
            {
                //TxtStockNo.Focus();
            }
        }
        else
        {
            if (ddSize.SelectedIndex > 0)
            {
                Fill_Price();
            }
        }
    }
    protected void ddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Category_SelectedIndex_Change();
        //ddItemName.Focus();
    }
    private void Category_SelectedIndex_Change()
    {
        ddlcategorycange();
        string Str = @"Select Distinct VF.Item_ID,VF.Item_Name from V_FinishedItemDetail VF 
        left Outer join V_OrderPackDetail OD on OD.Item_Finished_Id=VF.Item_Finished_Id 
        Where  Category_Id=" + ddCategoryName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        if (ChkForWithoutOrder.Checked != true)
        {
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue;
            }

        }
        Str = Str + " order by Item_Name";
        UtilityModule.ConditionalComboFill(ref ddItemName, Str, true, "--SELECT--");
    }
    private void ddlcategorycange()
    {
        ddQuality.Items.Clear();
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShade.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;

        string strsql = @"SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME 
                      FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on 
                      IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCategoryName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void ddItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemNameSelectedIndexChange();
        // ddQuality.Focus();
    }
    private void ItemNameSelectedIndexChange()
    {
        string Str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            Str = @"select Distinct CQ.SrNo,CQ.QualityNameAToC 
                    from CustomerQuality CQ 
                    inner join V_FinishedItemDetail vf on CQ.QualityId=vf.QualityId and CQ.CustomerId=" + DDCustomerCode.SelectedValue + @" 
                    left join V_OrderPackDetail Od on Od.Item_Finished_Id=vf.ITEM_FINISHED_ID 
                    Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + " and Od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            Str = Str + " order by QualityNameAToC";
        }
        else
        {
            Str = @"select Distinct vf.QualityId,vf.QualityName 
                    from V_FinishedItemDetail vf 
                    left join V_OrderPackDetail OD on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    Where  VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
            if (ChkForWithoutOrder.Checked != true)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + @" And OD.Orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            Str = Str + " order by QualityName";
        }
        UtilityModule.ConditionalComboFill(ref ddQuality, Str, true, "--SELECT--");
    }
    protected void ddQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboFill();
        Fill_Price();
        // ddDesign.Focus();
    }
    protected void ComboFill()
    {
        ddDesign.Items.Clear();
        ddColor.Items.Clear();
        ddShape.Items.Clear();
        ddSize.Items.Clear();
        ddShade.Items.Clear();
        
        if (TDDesign.Visible == true)
        {
            FillDesign();
        }
        if (TDColor.Visible == true)
        {
            FillColor();
        }
        if (TDShape.Visible == true)
        {
            FillShape();
        }
        if (TDShade.Visible == true)
        {
            FillShadeColor();
        }
    }

    protected void FillDesign()
    {
        string str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            str = @"select Distinct cd.SrNo,cd.DesignNameAToC 
                    From CustomerDesign cd 
                    inner join V_FinishedItemDetail vf on cd.DesignId=vf.designId 
                    left join V_OrderPackDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    Where cd.CustomerId=" + DDCustomerCode.SelectedValue + @" And vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + "  and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            if (TDQuality.Visible == true)
            {
                if (ddQuality.SelectedIndex > 0)
                {
                    str = str + " and cd.CQSRNO=" + ddQuality.SelectedValue;
                }
            }
            str = str + " order by DesignNameAToC";
        }
        else
        {
            str = @"select Distinct vf.designId,vf.designName 
                        From V_FinishedItemDetail vf 
                        left join V_OrderPackDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                        Where vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }

            if (TDQuality.Visible == true)
            {
                if (ddQuality.SelectedIndex > 0)
                {
                    str = str + " and vf.qualityid=" + ddQuality.SelectedValue;
                }
            }
            str = str + "  order by designName";
        }
        UtilityModule.ConditionalComboFill(ref ddDesign, str, true, "--Select--");
    }
    protected void FillColor()
    {
        hndesignid.Value = "0";
        string str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            str = @"select Distinct CC.SrNo,CC.ColorNameToC 
                    From CustomerColor cc 
                    inner join V_FinishedItemDetail vf on CC.ColorId=vf.ColorId 
                    left join V_OrderPackDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id 
                    Where cc.CustomerId=" + DDCustomerCode.SelectedValue + " And vf.item_id=" + ddItemName.SelectedValue + " and vf.mastercompanyid=" + Session["varcompanyid"];

            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            if (ddDesign.SelectedIndex > 0)
            {

                hndesignid.Value = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select designid from customerdesign where srno=" + ddDesign.SelectedValue + "").ToString();
                if (variable.VarMasterBuyercodeSeqWise == "1")
                {
                    str = str + " and CC.CDSRNO=" + ddDesign.SelectedValue + "";
                }
                else
                {
                    str = str + " and vf.DesignID=" + (hndesignid.Value == "" ? "0" : hndesignid.Value) + "";
                }
               // str = str + " and cc.CDSRNO=" + ddDesign.SelectedValue;
            }
            str = str + " order by ColorNameToC";
        }
        else
        {
            str = @"select Distinct vf.ColorId,vf.ColorName 
                    From V_FinishedItemDetail vf 
                    left join V_OrderPackDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id and vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.MasterCompanyId=" + Session["varcompanyId"];
            if (ChkForWithoutOrder.Checked == false)
            {
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    str = str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            str = str + " order by ColorName";
        }
        UtilityModule.ConditionalComboFill(ref ddColor, str, true, "--Select--");
    }
    private void FillShape()
    {
        string Str = "";
        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
        {
            Str = @"Select Distinct VF.ShapeId, VF.ShapeName 
                    From V_FinishedItemDetail VF";

            if (TDQuality.Visible == true)
            {
                Str = Str + " JOIN CustomerQuality CQ ON CQ.QualityID = VF.QualityID And CQ.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddQuality.SelectedIndex > 0)
                {
                    Str = Str + " And CQ.SrNo = " + ddQuality.SelectedValue;
                }
            }
            if (TDDesign.Visible == true)
            {
                Str = Str + " JOIN CustomerDesign CD ON CD.DesignID = VF.DesignID And CD.CQSRNO = CQ.SRNO And CD.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddDesign.SelectedIndex > 0)
                {
                    Str = Str + " And CD.SrNo = " + ddDesign.SelectedValue;
                }
            }
            if (TDColor.Visible == true)
            {
                Str = Str + " Left JOIN CustomerColor CC ON CC.ColorID = VF.ColorID And CC.CDSRNO = CD.SRNO And CC.CustomerID = " + DDCustomerCode.SelectedValue;
                if (ddColor.SelectedIndex > 0)
                {
                    Str = Str + " And CC.SrNo = " + ddColor.SelectedValue;
                }
            }
            Str = Str + " LEFT JOIN V_OrderPackDetail OD ON OD.Item_Finished_Id = VF.ITEM_FINISHED_ID ";
            Str = Str + " Where 1 = 1";
        }
        else
        {
            Str = @"Select Distinct VF.ShapeId, VF.ShapeName 
                From V_FinishedItemDetail VF 
                LEFT JOIN V_OrderPackDetail OD ON OD.Item_Finished_Id=VF.ITEM_FINISHED_ID 
                Where VF.Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + " And VF.MasterCompanyId=" + Session["varCompanyId"];
            if (TDQuality.Visible == true)
            {
                Str = Str + " And VF.QualityID = " + ddQuality.SelectedValue;
            }
            if (TDDesign.Visible == true)
            {
                Str = Str + " And VF.DesignID = " + ddDesign.SelectedValue;
            }
            if (TDColor.Visible == true)
            {
                Str = Str + " And VF.ColorID = " + ddColor.SelectedValue;
            }
        }
        
        if (ChkForWithoutOrder.Checked != true)
        {
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
            }
        }
        UtilityModule.ConditionalComboFill(ref ddShape, Str, true, "--Select--");
    }
    protected void FillShadeColor()
    {
        string Str = @"Select Distinct VF.ShadeColorId, VF.ShadeColorName From V_OrderPackDetail OD, V_FinishedItemDetail VF 
                Where OD.Item_Finished_Id=VF.Item_Finished_Id And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + @" And 
                VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        if (ChkForWithoutOrder.Checked != true)
        {
            if (DDCustomerOrderNo.SelectedIndex > 0)
            {
                Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
            }
        }
        UtilityModule.ConditionalComboFill(ref ddShade, Str, true, "--Select--");
    }
    protected void ddShape_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();
        // ddSize.Focus();
    }
    private void ShapeSelectedChange(int sizeid = 0)
    {

        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtArea.Text = "";
        //TxtPrice.Text = "";
        string Str = "", size = "SizeFt", custsize = "SizeNameAToC";

        if (DDUnit.SelectedValue == "1")
        {
            size = "SizeMtr";
            custsize = "MtSizeAToC";
        }

        if (Session["varcompanyId"].ToString() == "30")
        {
            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                Str = @"select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName  
                from CustomerSize CS 
                inner join V_FinishedItemDetail vf on CS.SizeID=vf.SizeID And vf.item_id=" + ddItemName.SelectedValue + @" 
                left join V_OrderPackDetail od on vf.ITEM_FINISHED_ID=od.Item_Finished_Id";

                if (ddDesign.SelectedIndex > 0)
                {
                    Str = Str + " JOIN CustomerDesign CD ON CD.DesignID = VF.DesignID And CD.Srno = " + ddDesign.SelectedValue;
                }

                if (ddColor.SelectedIndex > 0)
                {
                    Str = Str + " JOIN CustomerColor CC ON CC.ColorID = VF.ColorID And CC.Srno = " + ddColor.SelectedValue;
                }

                Str = Str + " Where CS.CustomerId=" + DDCustomerCode.SelectedValue + @" and vf.mastercompanyid=" + Session["varcompanyid"];
                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " and od.orderid=" + DDCustomerOrderNo.SelectedValue;
                    }
                }
                Str = Str + " order by SizeName";
            }
            else
            {
                Str = @"select sizeid," + size + " as SIze From V_finisheditemdetail vf where vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + "  and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }
            }
        }
        else
        {
            if (hnsampletype.Value == "1")
            {
                if (hnCarpetNoTypeId.Value == "0")
                {
                    Str = @"Select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName 
                    from CustomerSize CS(NoLock) 
                    inner join V_FinishedItemDetail vf(NoLock)  on CS.SizeID=vf.SizeID  And CustomerId=" + DDCustomerCode.SelectedValue + @"
                    left join V_OrderPackDetail od(NoLock)  on vf.ITEM_FINISHED_ID=od.Item_Finished_Id
                    Where Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + "  And VF.ShapeId=" + ddShape.SelectedValue + "   And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
                    
                }
                else
                {
                    Str = @"Select Distinct VF.SizeId,Case when " + custsize + " is null Then " + size + @" Else " + custsize + @" End SizeName 
                  from V_OrderPackDetail OD,V_FinishedItemDetail VF Left outer join CustomerSize CS on CS.Sizeid=Vf.Sizeid And CustomerId=" + DDCustomerCode.SelectedValue + @" Where OD.Item_Finished_Id=VF.Item_Finished_Id
                  And Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + "  And VF.ShapeId=" + ddShape.SelectedValue + "   And VF.MasterCompanyId=" + Session["varCompanyId"] + "";

                }
                if (ChkForWithoutOrder.Checked != true)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        if (DDCustomerOrderNo.SelectedIndex > 0)
                        {
                            Str = Str + @" And Orderid=" + DDCustomerOrderNo.SelectedValue + "";
                        }
                    }
                }
            }
            else
            {
                Str = @"select distinct sizeid," + size + " as SIze From V_finisheditemdetail vf where vf.CATEGORY_ID=" + ddCategoryName.SelectedValue + " and vf.ITEM_ID=" + ddItemName.SelectedValue + " and vf.ShapeId=" + ddShape.SelectedValue;
                if (sizeid > 0)
                {
                    Str = Str + " and  vf.sizeid=" + sizeid;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddSize, Str, true, "--Select--");
    }
    protected void ddSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        SizeSelectedIndexChange();
        //TxtPrice.Focus();

        if (Session["varCompanyId"].ToString() == "30")
        {
            ChangeRate();
        }
    }
    private void SizeSelectedIndexChange()
    {
        if (Session["varCompanyId"].ToString() == "30")
        {
            string DDSizeValue = ddSize.SelectedItem.Text;
            //string stringBeforeChar = DDSizeValue.Substring(0, DDSizeValue.IndexOf("["));

            if (ddSize.SelectedIndex > 0)
            {
                string[] splitString = DDSizeValue.Split('x');

                string Width = splitString[0].Trim();
                string Length = splitString[1].Trim();

                TxtWidth.Text = Width;
                TxtLength.Text = Length;
                Check_Length_Width_Format();
                //TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
            }
        }
        else
        {
            string Width = "WidthFt", Length = "LengthFt";
            if (DDUnit.SelectedValue == "1")
            {
                Width = "WidthMtr";
                Length = "LengthMtr";
            }
            DataSet Ds = new DataSet();
            if (Session["varCompanyNo"].ToString() == "39")
            {
                if (ddShape.SelectedItem.Text.ToUpper() == "ROUND")
                {
                    Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + " Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then ProdAreaMtr Else ProdAreaFt End Area from Size Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
                }
                else
                {
                    Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + " Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then AreaMtr Else AreaFt End Area from Size Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
                }
            }
            else
            {
                Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Case When 1=" + DDUnit.SelectedValue + @" Then " + Width + " Else " + Width + @" End Width,
                     Case When 1=" + DDUnit.SelectedValue + @" Then " + Length + " Else " + Length + @" End Length,
                     Case When 1=" + DDUnit.SelectedValue + @" Then AreaMtr Else AreaFt End Area from Size Where SizeId=" + ddSize.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "");
            }

             
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtWidth.Text = Ds.Tables[0].Rows[0]["Width"].ToString();
                TxtLength.Text = Ds.Tables[0].Rows[0]["Length"].ToString();
                TxtArea.Text = Ds.Tables[0].Rows[0]["Area"].ToString();
            }
        }
       

        
        Fill_Price();
    }
    protected void TxtLength_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    protected void TxtWidth_TextChanged(object sender, EventArgs e)
    {
        Check_Length_Width_Format();
    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";
        if (TxtLength.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtLength.Text));
                TxtLength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtLength.Text = "";
                    TxtLength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    LblErrorMessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (TxtLength.Text != "" && TxtWidth.Text != "")
        {
            if (Convert.ToInt32(DDUnit.SelectedValue) == 1)
            {
                if (Session["VarCompanyNo"].ToString() == "39")
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, Convert.ToInt32(ddShape.SelectedIndex>0 ? ddShape.SelectedValue : "0")));
                }
                else
                {
                    TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, 0));
                }

               
            }
            if (Convert.ToInt32(DDUnit.SelectedValue) == 2)
            {
                //TxtArea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(TxtLength.Text), Convert.ToDouble(TxtWidth.Text), 1, 0));
                FtAreaCalculate(TxtLength, TxtWidth, TxtArea, 1);
            }
        }
    }
    protected void ddDesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        DesignSelectedChange();
    }
    private void DesignSelectedChange()
    {
        FillColor();
        //        if (TDColor.Visible == true)
        //        {
        //            string Str = @"Select Distinct VF.ColorId,case When ColorNameToC is null then VF.ColorName Else ColorNameToC End As ColorName 
        //                        From OrderDetail OD,
        //                        V_FinishedItemDetail VF 
        //                        left outer join CustomerColor CC on CC.ColorId=VF.ColorId And Customerid=" + DDCustomerCode.SelectedValue + @" 
        //                        Where OD.Item_Finished_Id=VF.Item_Finished_Id And VF.Category_Id=" + ddCategoryName.SelectedValue + " And VF.Item_Id=" + ddItemName.SelectedValue + @" 
        //                        And VF.MasterCompanyId=" + Session["varCompanyId"] + "";
        //            if (ChkForWithoutOrder.Checked != true)
        //            {
        //                Str = Str + @" And OD.Orderid=" + DDCustomerOrderNo.SelectedValue + "";
        //            }
        //            UtilityModule.ConditionalComboFill(ref ddColor, Str, true, "--Select--");
        //        }
        Fill_Price();
        //ddColor.Focus();
    }
    protected void ddColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillShape();
        Fill_Price();
        ShapeSelectedChange();
        // ddShape.Focus();
    }
    
    protected void ddShade_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Price();
    }
    private void Fill_Price()
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        int color = 0;
        int quality = 0;
        int design = 0;
        int shape = 0;
        int size = 0;
        int shadeColor = 0;
        if ((TDQuality.Visible == true && ddQuality.SelectedIndex > 0) || TDQuality.Visible != true)
        {
            quality = 1;
        }
        if (TDDesign.Visible == true && ddDesign.SelectedIndex > 0 || TDDesign.Visible != true)
        {
            design = 1;
        }
        if (TDColor.Visible == true && ddColor.SelectedIndex > 0 || TDColor.Visible != true)
        {
            color = 1;
        }
        if (TDShape.Visible == true && ddShape.SelectedIndex > 0 || TDShape.Visible != true)
        {
            shape = 1;
        }
        if (TDSize.Visible == true && ddSize.SelectedIndex > 0 || TDSize.Visible != true)
        {
            size = 1;
        }
        if (TDShade.Visible == true && ddShade.SelectedIndex > 0 || TDShade.Visible != true)
        {
            shadeColor = 1;
        }
        int VarQuality = 0, VarDesign = 0, VarColor = 0;
        if (quality == 1 && design == 1 && color == 1 && shape == 1 && size == 1 && shadeColor == 1)
        {
            //if (TxtStockNo.Text != "")
            //{

            //    if (ddQuality.Visible == true)
            //    {
            //        VarQuality = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.QualityId from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
            //    }

            //    if (ddDesign.Visible == true)
            //    {
            //        VarDesign = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.DesignID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));

            //    }

            //    if (ddColor.Visible == true)
            //    {
            //        VarColor = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.colorID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
            //    }
            //}
            //else
            //{
            VarQuality = ddQuality.Visible == true ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
            VarDesign = ddDesign.Visible == true ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
            VarColor = ddColor.Visible == true ? Convert.ToInt32(ddColor.SelectedValue) : 0;
            // }
            int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
            int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
            int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;
            int finishedid = 0;
            if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
            {
                finishedid = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
            }
            else
            {
                finishedid = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"]));
            }
            if (finishedid > 0)
            {
                if (ChkForWithoutOrder.Checked == false)
                {
                    DataSet Ds = null;
                    if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                    {
                        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Sum(QtyRequired) As QtyRequired,UnitRate,Item_Finished_id,ordercaltype from OrderDetail Where Orderid=" + DDCustomerOrderNo.SelectedValue + " And Item_Finished_Id=" + finishedid + " and CQID=" + VarQuality + " and DSRNO=" + VarDesign + " and CSRNO=" + VarColor + " group by Unitrate,Item_Finished_id,ordercaltype");
                    }
                    else
                    {
                        Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Sum(QtyRequired) As QtyRequired,UnitRate,Item_Finished_id,ordercaltype from OrderDetail Where Orderid=" + DDCustomerOrderNo.SelectedValue + " And Item_Finished_Id=" + finishedid + " group by Unitrate,Item_Finished_id,ordercaltype");
                    }

                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        TxtTotalQty.Text = Ds.Tables[0].Rows[0]["QtyRequired"].ToString();
                        TxtPrice.Text = Ds.Tables[0].Rows[0]["UnitRate"].ToString();
                        //if (Ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "1")
                        //{
                        //    if ((RDAreaWise.Checked))
                        //    {
                        //        TxtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) / Convert.ToDecimal(TxtArea.Text), 2));

                        //    }
                        //}
                        if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                        {
                            TxtPrePackQty.Text = (Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Sum(Pcs),0) from PackingInformation Where OrderId=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + " and CQSrno=" + VarQuality + " and CDSRNO=" + VarDesign + " and CCSRNO=" + VarColor + ""))).ToString();
                        }
                        else
                        {
                            TxtPrePackQty.Text = (Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select IsNull(Sum(Pcs),0) from PackingInformation Where OrderId=" + DDCustomerOrderNo.SelectedValue + " And FinishedId=" + finishedid + ""))).ToString();
                        }
                    }
                    else
                    {
                        TxtTotalQty.Text = TxtTotalPcs.Text;
                        TxtPrePackQty.Text = "0";
                        //TxtPrice.Text = "";
                    }
                }
                ////if (ChkForMulipleRolls.Checked == true)
                //{
                TxtPackQty.Text = TxtTotalPcs.Text;

                //if (ChkForWithoutStockNo.Checked == false)
                //{
                    Fill_StockGrid(finishedid);
                //}

               

                if (Session["varCompanyId"].ToString() == "30")
                {
                    TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
                }

                //}
            }
        }
    }
    private void Fill_StockGrid(int VarFinishedID)
    {
        string Str = "";
        //if (ChkForEdit.Checked == false)
        //{
        if (TxtStockNo.Text == "")
        {
            if (Session["varcompanyId"].ToString() == "4")//Deepak rugs
            {
                Str = @"Select StockNo Sr_No,TStockNo StockNo,Pack From CarpetNumber Where Pack=0 And IssRecStatus=0 
                      And CompanyID=" + DDCompanyName.SelectedValue + " And Item_Finished_ID=" + VarFinishedID + " ";
                //And CAST(CONVERT(CHAR(11),REC_DATE, 113) AS DATETIME)<='" + TxtDate.Text + "'";
                if (DDCustomerOrderNo.SelectedIndex > 0)
                {
                    Str = Str + " And OrderId=" + DDCustomerOrderNo.SelectedValue;
                }
            }
            else
            {
                Str = @"SELECT CN.STOCKNO AS SR_NO,CN.TSTOCKNO as STOCKNO,CN.PACK 
                    FROM CARPETNUMBER CN(Nolock) 
                    JOIN PROCESS_RECEIVE_DETAIL_1 PRD(Nolock) ON CN.PROCESS_REC_DETAIL_ID=PRD.PROCESS_REC_DETAIL_ID
                    WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + @" AND 
                    CN.ITEM_FINISHED_ID=" + VarFinishedID + @" AND PRD.QUALITYTYPE=1";

                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
                    }
                }
                if (Session["varcompanyId"].ToString() == "38")
                {
                    Str = Str + " And CN.CurrentProStatus = 15";
                }
                Str = Str + "   UNION ";
                Str = Str + @"  SELECT CN.STOCKNO AS SR_NO,CN.TSTOCKNO as STOCKNO,CN.PACK 
                FROM CARPETNUMBER CN(Nolock) 
                WHERE CN.PACK=0 AND CN.ISSRECSTATUS=0 AND CN.COMPANYID=" + DDCompanyName.SelectedValue + @" AND 
                CN.ITEM_FINISHED_ID=" + VarFinishedID + " AND CN.PROCESS_REC_DETAIL_ID=0";

                if (ChkForWithoutOrder.Checked == false)
                {
                    if (DDCustomerOrderNo.SelectedIndex > 0)
                    {
                        Str = Str + " And CN.OrderId=" + DDCustomerOrderNo.SelectedValue;
                    }
                }
                if (Session["varcompanyId"].ToString() == "38")
                {
                    Str = Str + " And CN.CurrentProStatus = 15";
                }
            }
        }
        else
        {

            Str = @"Select StockNo Sr_No,TStockNo StockNo,Pack From CarpetNumber Where Pack=0 And IssRecStatus=0 And CompanyID=" + DDCompanyName.SelectedValue + @"
                   And Item_Finished_ID=" + VarFinishedID + " and TStockNo='" + TxtStockNo.Text + "' ";

        }
       
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGStock.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["varCompanyNo"].ToString()=="16" && ChkForWithoutOrder.Checked==false)
            {
                int balancestockqty=0;
                int totalqty = 0;
                int prepackqty = 0;
                int balancepackqty = 0;

                totalqty = Convert.ToInt32(TxtTotalQty.Text == "" ? "0" : TxtTotalQty.Text);
                prepackqty = Convert.ToInt32(TxtPrePackQty.Text == "" ? "0" : TxtPrePackQty.Text);
                balancepackqty = Convert.ToInt32(TxtTotalQty.Text == "" ? "0" : TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text == "" ? "0" : TxtPrePackQty.Text);

                balancestockqty = ds.Tables[0].Rows.Count;


                if ((Convert.ToInt32(TxtTotalPcs.Text) > balancestockqty) || (Convert.ToInt32(TxtTotalPcs.Text) > balancepackqty))
                {
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "Pls Pack Correct Qty";
                    TxtPackQty.Text = "";
                    TxtPackQty.Focus();
                    DGStock.DataSource = null;
                    DGStock.DataBind();

                }
                else
                {
                    if (ChkForWithoutStockNo.Checked == false)
                    {
                        DGStock.DataBind();
                        DGStock.Visible = true;
                        for (int i = 0; i < DGStock.Rows.Count; i++)
                        {
                            if (((TextBox)DGStock.Rows[i].FindControl("TxtPack")).Text == "1")
                            {
                                ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = true;
                            }
                            else
                            {
                                ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
                            }
                        }
                    }
                    
                }
            }
            else
            {
                DGStock.DataBind();
                DGStock.Visible = true;
                for (int i = 0; i < DGStock.Rows.Count; i++)
                {
                    if (((TextBox)DGStock.Rows[i].FindControl("TxtPack")).Text == "1")
                    {
                        ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
                    }
                }
            }
        }
        else
        {
            DGStock.Visible = false;
            TxtPackQty.Text = "";
            TxtPackQty.Focus();
        }
        if (ChkForWithoutStockNo.Checked == false)
        {
            ForCheckAllRows();
        }
    }
    private void ForCheckAllRows()
    {
        if (TxtPackQty.Text != "")
        {
            for (int i = 0; i < DGStock.Rows.Count; i++)
            {
                ((CheckBox)DGStock.Rows[i].FindControl("Chkbox")).Checked = false;
            }

            if (DGStock.Rows.Count >= Convert.ToInt32(TxtPackQty.Text))
            {
                for (int i = 0; i < Convert.ToInt32(TxtPackQty.Text); i++)
                {
                    GridViewRow row = DGStock.Rows[i];
                    ((CheckBox)row.FindControl("Chkbox")).Checked = true;
                }
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Value";
                TxtPcsPerRoll.Text = "";
                TxtTotalPcs.Text = "";
                if (ChkForEdit.Checked == false)
                {
                    TxtPackQty.Text = "";
                    TxtPackQty.Focus();
                }
            }
        }
    }
    protected void TxtRollNoFrom_TextChanged(object sender, EventArgs e)
    {
        RollNoFromTextChanged();
    }
    private void RollNoFromTextChanged()
    {
        if (TxtRollNoFrom.Text != "")
        {
            TxtBales.Text = "1";
            if (TxtPcsPerRoll.Text == "")
            {
                TxtPcsPerRoll.Text = "1";
            }
            TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(TxtBales.Text) - 1).ToString();
            //TxtTotalPcs.Text =(Convert.ToDouble(TxtPcsPerRoll.Text) * Convert.ToDouble(TxtBales.Text)).ToString ();
        }
        TextBoxIndexChange();
        //TxtSrNo.Focus();
    }
    private void TextBoxIndexChange()
    {
        if (TxtRollNoFrom.Text != "" && TxtRollNoTo.Text != "" && TxtPcsPerRoll.Text != "" && TxtPcsPerRoll.Text != "0")
        {
            TxtTotalPcs.Text = (Convert.ToInt32(TxtPcsPerRoll.Text) * (1 + (Convert.ToInt32(TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text)))).ToString();
            if (Session["varcompanyId"].ToString() != "30")
            {
                TxtBales.Text = (Convert.ToInt32(TxtTotalPcs.Text) / Convert.ToInt32(TxtPcsPerRoll.Text)).ToString();
            }

            TxtPackQty.Text = TxtTotalPcs.Text;

            if (Session["VarCompanyNo"].ToString() == "36")
            {
                if (ChkForMulipleRolls.Checked==true)
                {
                    ForCheckAllRows();
                }
            }
            else
            {
                ForCheckAllRows();
            }                
            

            //if (ChkForEdit.Checked == true)
            //{
            //    TxtPackQty.Text = TxtTotalPcs.Text;
            //    ForCheckAllRows();
            //    //TxtPrePackQty.Text =Convert.ToString(Convert.ToDouble(TxtPrePackQty.Text) - Convert.ToDouble(TxtPackQty.Text));
            //}
        }
    }
    protected void TxtBales_TextChanged(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "30" || Session["varcompanyId"].ToString() == "38")
        {
            //(Convert.ToInt32(TxtPcsPerRoll.Text) * (1 + (Convert.ToInt32(TxtRollNoTo.Text) - Convert.ToInt32(TxtRollNoFrom.Text)))).ToString();
            TxtRollNoTo.Text = (Convert.ToInt32(TxtRollNoFrom.Text) + Convert.ToInt32(TxtBales.Text) - 1).ToString();
            TextBoxIndexChange();
        }
    }
    protected void TxtRollNoTo_TextChanged(object sender, EventArgs e)
    {
        TextBoxIndexChange();
    }
    protected void TxtPcsPerRoll_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "36" && ChkForWithoutOrder.Checked==true)
        {
            TxtTotalPcs.Text = (Convert.ToDouble(TxtPcsPerRoll.Text) * Convert.ToDouble(TxtBales.Text)).ToString();
        }
        else
        {
            TextBoxIndexChange();   
        }           
       
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "You Are Successfully LoggedOut..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void DDUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShapeSelectedChange();

        //if (Session["VarCompanyId"].ToString() == "30")
        //{
        //    ChangeRate();
        //}
    }
    private void Validation_Check()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;

        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerOrderNo) == false)
        {
            goto a;

        }
        if (UtilityModule.VALIDTEXTBOX(TxtStockNo) == false)
        {
            goto a;
        }
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
        if (ddDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true)
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
        if (ddSize.Visible == true)
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
        if (UtilityModule.VALIDTEXTBOX(TxtPrice) == false)
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (LblErrorMessage.Text == "")
        {
            ViewState["PACKINGDETAILID"] = 0;
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                int num = 0;
                string VarInvoiceYear = "";
                if (Convert.ToInt32(ViewState["PackingID"]) == 0)
                {
                    num = 1;
                }
                if (num == 1)
                {
                    SqlParameter[] _arrpara1 = new SqlParameter[11];

                    _arrpara1[0] = new SqlParameter("@PackingId", SqlDbType.Int);
                    _arrpara1[1] = new SqlParameter("@ConsignorId", SqlDbType.Int);
                    _arrpara1[2] = new SqlParameter("@ConsigneeId", SqlDbType.Int);
                    _arrpara1[3] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
                    _arrpara1[4] = new SqlParameter("@CurrencyId", SqlDbType.Int);
                    _arrpara1[5] = new SqlParameter("@UnitId", SqlDbType.Int);
                    _arrpara1[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                    _arrpara1[7] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
                    _arrpara1[8] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                    _arrpara1[9] = new SqlParameter("@InvoiceYear", SqlDbType.Int);
                    _arrpara1[10] = new SqlParameter("@Msg", SqlDbType.NVarChar, 50);


                    if (ddInvoiceNo.Visible == true)
                    {
                        _arrpara1[0].Value = ddInvoiceNo.SelectedValue;
                    }
                    else
                    {
                        _arrpara1[0].Value = 0;
                    }
                    _arrpara1[0].Direction = ParameterDirection.InputOutput;
                    _arrpara1[1].Value = DDCompanyName.SelectedValue;
                    _arrpara1[2].Value = DDCustomerCode.SelectedValue;
                    _arrpara1[3].Value = TxtDate.Text;
                    _arrpara1[4].Value = DDCurrency.SelectedValue;
                    _arrpara1[5].Value = DDUnit.SelectedValue;
                    _arrpara1[6].Value = Session["varuserid"];
                    _arrpara1[7].Value = TxtInvoiceNo.Text;
                    _arrpara1[8].Value = Session["varCompanyId"];
                    string VarInvoiceMonth = DateTime.Now.ToString("MM");
                    if (Convert.ToInt32(VarInvoiceMonth) >= 04)
                    {
                        VarInvoiceYear = DateTime.Now.ToString("yyyy");
                    }
                    else
                    {
                        VarInvoiceYear = DateTime.Now.ToString("yyyy");
                        VarInvoiceYear = (Convert.ToInt32(VarInvoiceYear) - 1).ToString();
                    }
                    _arrpara1[9].Value = VarInvoiceYear;
                    _arrpara1[10].Direction = ParameterDirection.Output;


                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKING_AND_INVOICE", _arrpara1);
                    ViewState["PackingID"] = _arrpara1[0].Value;
                    if (_arrpara1[10].Value.ToString() != "")  //Check For Duplicate InvoiceNo
                    {
                        LblErrorMessage.Visible = true;
                        LblErrorMessage.Text = _arrpara1[10].Value.ToString();
                        Tran.Rollback();
                        return;
                    }
                    LblErrorMessage.Text = "";
                    BtnSave.Text = "Save";
                }
                SqlParameter[] _arrpara = new SqlParameter[55];
                _arrpara[0] = new SqlParameter("@PackingId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@RollNo", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@PcsFrom", SqlDbType.Int);
                _arrpara[3] = new SqlParameter("@PcsTo", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@ArticleNo", SqlDbType.NVarChar, 20);
                _arrpara[5] = new SqlParameter("@Extra1", SqlDbType.NVarChar, 20);
                _arrpara[6] = new SqlParameter("@Extra2", SqlDbType.NVarChar, 20);
                _arrpara[7] = new SqlParameter("@FinishedID", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("@Width", SqlDbType.NVarChar, 10);
                _arrpara[9] = new SqlParameter("@Length", SqlDbType.NVarChar, 10);
                _arrpara[10] = new SqlParameter("@Pcs", SqlDbType.Int);
                _arrpara[11] = new SqlParameter("@Area", SqlDbType.Float);
                _arrpara[12] = new SqlParameter("@Price", SqlDbType.Float);
                _arrpara[13] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrpara[14] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[15] = new SqlParameter("@StockNo", SqlDbType.Int);
                _arrpara[16] = new SqlParameter("@TStockNo", SqlDbType.NVarChar, 8000);
                _arrpara[17] = new SqlParameter("@Id", SqlDbType.Int);
                _arrpara[18] = new SqlParameter("@TPackingNo", SqlDbType.NVarChar, 50);
                _arrpara[19] = new SqlParameter("@RollFrom", SqlDbType.Int);
                _arrpara[20] = new SqlParameter("@RollTo", SqlDbType.Int);
                _arrpara[21] = new SqlParameter("@RPcs", SqlDbType.Int);
                _arrpara[22] = new SqlParameter("@TotalPcs", SqlDbType.Int);
                _arrpara[23] = new SqlParameter("@TotalRoll", SqlDbType.Int);
                _arrpara[24] = new SqlParameter("@CompStockNo", SqlDbType.Int);
                _arrpara[25] = new SqlParameter("@CustStockNo", SqlDbType.Int);
                _arrpara[26] = new SqlParameter("@Pack", SqlDbType.Int);
                _arrpara[27] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 50);
                _arrpara[28] = new SqlParameter("@PackingDate", SqlDbType.SmallDateTime);
                _arrpara[29] = new SqlParameter("@Quality", SqlDbType.NVarChar, 50);
                _arrpara[30] = new SqlParameter("@Design", SqlDbType.NVarChar, 50);
                _arrpara[31] = new SqlParameter("@Color", SqlDbType.NVarChar, 50);
                _arrpara[32] = new SqlParameter("@CQSRNO", SqlDbType.Int);
                _arrpara[33] = new SqlParameter("@CDSRNO", SqlDbType.Int);
                _arrpara[34] = new SqlParameter("@CCSRNO", SqlDbType.Int);
                _arrpara[35] = new SqlParameter("@CalTypeAmt", SqlDbType.Int);
                _arrpara[36] = new SqlParameter("@MulipleRollFlag", SqlDbType.Int);
                _arrpara[37] = new SqlParameter("@PakingDetailID", SqlDbType.Int);
                _arrpara[38] = new SqlParameter("@QualityCodeID", SqlDbType.Int);
                _arrpara[39] = new SqlParameter("@Sub_Quality", SqlDbType.NVarChar, 450);
                _arrpara[40] = new SqlParameter("@NetWt", SqlDbType.Float);
                _arrpara[41] = new SqlParameter("@GrossWt", SqlDbType.Float);
                _arrpara[42] = new SqlParameter("@PurchaseCode", SqlDbType.NVarChar, 250);
                _arrpara[43] = new SqlParameter("@BuyerCode", SqlDbType.NVarChar, 250);
                _arrpara[44] = new SqlParameter("@UCCNumber", SqlDbType.VarChar, 50);
                _arrpara[45] = new SqlParameter("@RUGID", SqlDbType.VarChar, 50);
                _arrpara[46] = new SqlParameter("@Withbuyercode", SqlDbType.TinyInt);
                _arrpara[47] = new SqlParameter("@BaleL", SqlDbType.VarChar, 10);
                _arrpara[48] = new SqlParameter("@BaleW", SqlDbType.VarChar, 10);
                _arrpara[49] = new SqlParameter("@BaleH", SqlDbType.VarChar, 10);
                _arrpara[50] = new SqlParameter("@CBM", SqlDbType.Float);
                _arrpara[51] = new SqlParameter("@SinglePcsNetWt", SqlDbType.Float);
                _arrpara[52] = new SqlParameter("@SinglePcsGrossWt", SqlDbType.Float);
                _arrpara[53] = new SqlParameter("@RatePerPcs", SqlDbType.Float);
                _arrpara[54] = new SqlParameter("@StyleNo", SqlDbType.VarChar, 20);

                //Select PackingId,RollNo,PcsFrom,PcsTo,ArticleNo,Extra1,Extra2,FinishedId,Width,Length,Pcs,Area,Price,SrNo,OrderId,StockNo,TStockNo,Id,TPackingNo,
                //RollFrom,RollTo,RPcs,TotalPcs,TotalRoll,CompStockNo,CustStockNo,Pack,Remarks From PackingInformation
                _arrpara[0].Value = ViewState["PackingID"];
                _arrpara[1].Value = TxtRollNoFrom.Text;
                _arrpara[2].Value = TxtBales.Text;
                _arrpara[3].Value = 0;
                _arrpara[4].Value = "";
                _arrpara[5].Value = "";
                _arrpara[6].Value = "";

                _arrpara[8].Value = TxtWidth.Text;
                _arrpara[9].Value = TxtLength.Text;
                _arrpara[10].Value = TxtTotalPcs.Text;
                _arrpara[11].Value = Convert.ToDouble(TxtArea.Text) * Convert.ToDouble(TxtTotalPcs.Text);
                _arrpara[12].Value = TxtPrice.Text;
                _arrpara[13].Value = TxtSrNo.Text == "" ? "0" : TxtSrNo.Text;
                _arrpara[14].Value = ChkForWithoutOrder.Checked == true ? "0" : DDCustomerOrderNo.SelectedValue;
                _arrpara[15].Value = 0;
                if (hnid.Value == "0")
                    _arrpara[17].Value = ViewState["PACKINGDETAILID"];

                else
                    _arrpara[17].Value = hnid.Value;
                _arrpara[18].Value = TxtInvoiceNo.Text;
                _arrpara[19].Value = TxtRollNoFrom.Text;
                _arrpara[20].Value = TxtRollNoTo.Text;
                _arrpara[21].Value = TxtPcsPerRoll.Text;
                _arrpara[22].Value = TxtTotalPcs.Text;
                _arrpara[23].Value = TxtBales.Text;
                _arrpara[24].Value = 0;
                _arrpara[25].Value = 0;
                _arrpara[26].Value = 0;
                _arrpara[27].Value = TxtRemarks.Text;
                _arrpara[28].Value = TxtDate.Text;
                _arrpara[29].Value = ddQuality.SelectedItem.Text;
                _arrpara[30].Value = ddDesign.SelectedItem.Text;
                //_arrpara[31].Value = ddColor.SelectedItem.Text;
                _arrpara[31].Value = ddColor.SelectedItem == null ? "" : ddColor.SelectedItem.Text;
                _arrpara[32].Value = ddQuality.SelectedValue;
                _arrpara[33].Value = ddDesign.SelectedValue;
                _arrpara[34].Value = ddColor.SelectedValue;
                _arrpara[35].Value = RDAreaWise.Checked == true ? 0 : 1;
                _arrpara[36].Value = ChkForMulipleRolls.Checked == true ? 1 : 0;
                _arrpara[37].Direction = ParameterDirection.Output;
                //_arrpara[38].Value = DDSubQuality.SelectedValue;
                //_arrpara[39].Value = DDSubQuality.SelectedItem.Text;
                _arrpara[38].Value = 0;
                _arrpara[39].Value = "";
                _arrpara[40].Value = 0;
                _arrpara[41].Value = 0;
                _arrpara[42].Value = Txtpurchasecode.Text;
                _arrpara[43].Value = TxtBuyer.Text;
                _arrpara[44].Value = txtuccnumber.Text;
                _arrpara[45].Value = txtrugid.Text;
                _arrpara[46].Value = variable.Withbuyercode;

                _arrpara[47].Value = txtlengthBale.Text;
                _arrpara[48].Value = txtwidthBale.Text;
                _arrpara[49].Value = txtheightbale.Text;
                _arrpara[50].Value = txtcbmbale.Text == "" ? "0" : txtcbmbale.Text;
                _arrpara[51].Value = txtSinglePcsNetWt.Text == "" ? "0" : txtSinglePcsNetWt.Text;
                _arrpara[52].Value = txtSinglePcsGrossWt.Text == "" ? "0" : txtSinglePcsGrossWt.Text;
                _arrpara[53].Value = TxtRatePerPcs.Text == "" ? "0" : TxtRatePerPcs.Text;
                _arrpara[54].Value = txtStyleNo.Text;

                int VarQuality = 0; int VarDesign = 0; int VarColor = 0;
                #region
                //if (TxtStockNo.Text != "" && TxtStockNo.Text != "0")
                //{
                //    if (ddQuality.Visible == true)
                //    {
                //        VarQuality = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.QualityId from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
                //    }

                //    if (ddDesign.Visible == true)
                //    {
                //        VarDesign = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.DesignID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
                //    }

                //    if (ddColor.Visible == true)
                //    {
                //        VarColor = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VF.colorID from V_FinishedItemDetail VF, CarpetNumber CR where CR.Item_Finished_ID= VF.Item_Finished_Id AND TStockNo ='" + TxtStockNo.Text + "' And VF.MasterCompanyId=" + Session["varCompanyId"] + ""));
                //    }
                //}
                //else
                //{
                #endregion
                VarQuality = Convert.ToInt32(ddQuality.SelectedValue) > 0 ? Convert.ToInt32(ddQuality.SelectedValue) : 0;
                VarDesign = Convert.ToInt32(ddDesign.SelectedValue) > 0 ? Convert.ToInt32(ddDesign.SelectedValue) : 0;
                VarColor = ddColor.Visible == true && Convert.ToInt32(ddColor.SelectedValue) > 0 ? Convert.ToInt32(ddColor.SelectedValue) : 0;
                //}
                int VarShape = ddShape.Visible == true ? Convert.ToInt32(ddShape.SelectedValue) : 0;
                int VarSize = ddSize.Visible == true ? Convert.ToInt32(ddSize.SelectedValue) : 0;
                int VarShadeColor = ddShade.Visible == true ? Convert.ToInt32(ddShade.SelectedValue) : 0;

                if (variable.Withbuyercode == "1" && hnsampletype.Value == "1")
                {
                    _arrpara[7].Value = UtilityModule.getItemFinishedIdWithBuyercode(ddItemName, ddQuality, ddDesign, ddColor, ddShape, ddSize, TxtProdCode, ddShade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                }
                else
                {
                    //_arrpara[7].Value = UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"]));
                    _arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"])));
                }

                //_arrpara[7].Value = Convert.ToInt32(UtilityModule.getItemFinishedId(Convert.ToInt32(ddItemName.SelectedValue), VarQuality, VarDesign, VarColor, VarShape, VarSize, VarShadeColor, "", Convert.ToInt32(Session["varCompanyId"])));
                string stockno11 = "";
                //if (ChkForMulipleRolls.Checked == true)
                //{
                for (int i = 0; i < DGStock.Rows.Count; i++)
                {
                    GridViewRow row = DGStock.Rows[i];
                    if (((CheckBox)row.FindControl("Chkbox")).Checked == true)
                    {
                        if (stockno11 == "")
                        {

                            stockno11 = DGStock.DataKeys[i].Value.ToString();
                        }
                        else
                        {
                            stockno11 = stockno11 + ',' + DGStock.DataKeys[i].Value.ToString();
                        }

                    }
                }
                _arrpara[16].Value = stockno11;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
                ViewState["PACKINGDETAILID"] = _arrpara[37].Value;
                //}
                //else
                //{
                //    _arrpara[16].Value = TxtStockNo.Text.ToUpper();
                //    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION", _arrpara);
                //}
                Tran.Commit();
                Fill_Grid();
                Save_Referce();
                DGStock.DataSource = null;
                DGStock.DataBind();
                TxtStockNo.Focus();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
                Tran.Rollback();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = ex.Message;
                if (DGOrderDetail.Rows.Count == 0)
                {
                    ViewState["PackingID"] = 0;
                }
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompanyName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDConsignee) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtInvoiceNo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDCurrency) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDUnit) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRollNoFrom) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtBales) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtRollNoTo) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPcsPerRoll) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtTotalPcs) == false)
        {
            goto a;
        }
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
        if (ddDesign.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddDesign) == false)
            {
                goto a;
            }
        }
        if (ddColor.Visible == true)
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
        if (ddSize.Visible == true)
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
        if (UtilityModule.VALIDTEXTBOX(TxtWidth) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtLength) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPrice) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDTEXTBOX(TxtArea) == false)
        {
            goto a;
        }
        if (ChkForWithoutOrder.Checked == false)
        {
            if (UtilityModule.VALIDTEXTBOX(TxtTotalQty) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDTEXTBOX(TxtPackQty) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        LblErrorMessage.Visible = true;
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
    private void Save_Referce()
    {
        TxtStockNo.Text = "";
        if (Session["varcompanyno"].ToString() != "30")
        {
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedIndex = 0;
                Category_SelectedIndex_Change();
                ItemNameSelectedIndexChange();
                DDSubQuality.Items.Clear();
            }
            if (DDCustomerOrderNo.Items.Count > 0)
            {
                DDCustomerOrderNo.SelectedIndex = 0;
            }
        }

        ShapeSelectedChange();
        if (ddSize.Items.Count == 0)
        {
            ComboFill();
        }
        DDUnit.Enabled = false;
        TxtWidth.Text = "";
        TxtLength.Text = "";
        TxtProdCode.Text = "";
        TxtArea.Text = "";
        //TxtPrice.Text = "";
        //TxtTotalPcs.Text = "";
        TxtPackQty.Text = "";
        TxtRemarks.Text = "";
        hnid.Value = "0";
        hnpackingid.Value = "0";
        Txtpurchasecode.Text = "";
        TxtBuyer.Text = "";
        //TxtRollNoFrom.Text = "";
        //TxtRollNoTo.Text = "";
        //TxtBales.Text = "";
        //TxtPcsPerRoll.Text = "";
        //TxtTotalPcs.Text = "";
        TxtTotalQty.Text = "0";
        txtuccnumber.Text = "";
        txtrugid.Text = "";
        txtlengthBale.Text = "";
        txtwidthBale.Text = "";
        txtheightbale.Text = "";
        txtcbmbale.Text = "";
        txtSinglePcsNetWt.Text = "";
        txtSinglePcsGrossWt.Text = "";
        TxtRatePerPcs.Text = "";
        txtStyleNo.Text = "";
        if (Session["varcompanyno"].ToString() == "30" || Session["varcompanyId"].ToString() == "38")
        {
            TxtRollNoFrom.Text = (Convert.ToInt32(TxtRollNoTo.Text == "" ? "0" : TxtRollNoTo.Text) + 1).ToString();
            TxtBales.Text = "";
            TxtRollNoTo.Text = "0";
            TxtPcsPerRoll.Text = "";
            RollNoFromTextChanged();            
        }
        hnsampletype.Value = "1";
    }
    private void Fill_Grid()
    {
        DGOrderDetail.DataSource = GetDetail();
        DGOrderDetail.DataBind();
        if (DGOrderDetail.Rows.Count > 0)
        {
            DGOrderDetail.Visible = true;
        }
        else
        {
            DGOrderDetail.Visible = true;
        }
    }
    //********************************Function To Get Data to fill Gride*********************************************************
    private DataSet GetDetail()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            int VarCalType = 0;
            if (RDAreaWise.Checked == true)
            {
                VarCalType = 0;
            }
            else
            {
                VarCalType = 1;
            }
            string strsql = @"Select ID Sr_No,PackingId,Category_Name Category,Item_Name ItemName,PI.Quality,PI.Design,PI.Color,ShapeName, 
                              PI.Width+'x'+PI.Length as Size,Pcs Qty,Price Rate,Area,
                              Case When " + VarCalType + @"=0 Then Area*Price Else Pcs*Price End Amount ,dbo.F_GetstockNo(ID)  As StockNo,RollFrom,RollTo,UCCNumber,RUGID,
                              isnull(OM.customerorderNo,'')as CustomerorderNo,isnull(PI.RatePerPcs,0) as RatePerPcs,isnull(PI.SinglePcsNetWt,0) as SinglePcsNetWt
                              From PackingInformation PI inner join V_FinishedItemDetail VF on PI.Finishedid=VF.Item_Finished_id left join ordermaster OM on PI.Orderid=OM.orderid Where PI.PackingId=" + ViewState["PackingID"] + " And VF.MasterCompanyId=" + Session["varCompanyId"] + " order by RollFrom desc";

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            txttotalareagrid.Text = "";
            txttotalpcsgrid.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalpcsgrid.Text = ds.Tables[0].Compute("sum(Qty)", "").ToString();
                txttotalareagrid.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(Area)", "")), 4).ToString();
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
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

    protected void TxtPackQty_TextChanged(object sender, EventArgs e)
    {
        if (TxtTotalPcs.Text != "" && TxtPrePackQty.Text != "" && TxtPackQty.Text != "")
        {
            if (Convert.ToInt32(TxtPackQty.Text) > (Convert.ToInt32(TxtTotalQty.Text) - Convert.ToInt32(TxtPrePackQty.Text)))
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Pending Qty";
                TxtPackQty.Text = "";
                TxtPackQty.Focus();
            }
            else
            {
                ForCheckAllRows();
            }
        }
    }
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        lnkchnginvoice.Visible = false;
        TRSearchInvoiceNo.Visible = false;
        if (ChkForEdit.Checked == true)
        {
            TRSearchInvoiceNo.Visible = true;
            TDDDInvoiceNo.Visible = true;
            ddInvoiceNo.Items.Clear();
            TDChkForChangeQDC.Visible = true;
            CustomerCodeSelectedIndexChange();
            DGStock.Visible = false;
            Save_Referce();
            ViewState["PackingID"] = 0;
            lnkchnginvoice.Visible = true;
        }
        else
        {
            TRSearchInvoiceNo.Visible = false;
            TDChkForChangeQDC.Visible = false;
            TDDDInvoiceNo.Visible = false;
            ddInvoiceNo.Items.Clear();
            DGStock.Visible = false;
            Save_Referce();
            TxtInvoiceNo.Text = "";
            TxtDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            DGOrderDetail.Visible = false;
            hnid.Value = "0";
            hnpackingid.Value = "0";
            ViewState["PackingID"] = 0;
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 0;
            }
            if (DDConsignee.Items.Count > 0)
            {
                DDConsignee.SelectedIndex = 0;
            }
        }
    }
    protected void ddInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strsql = "";
        strsql = "select CalTypeAmt from packinginformation where packingid=" + ddInvoiceNo.SelectedValue + @"";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //VarCalType = Convert.ToInt16(ds.Tables[0].Rows[0]["caltypeamt"]);
            if (Convert.ToInt16(ds.Tables[0].Rows[0]["caltypeamt"]) == 0)
            {
                RDAreaWise.Checked = true;
            }
            else
            {
                RDPcsWise.Checked = true;
            }

        }

        string str = @"Select PackingId,InvoiceNo,Replace(Convert(VarChar(11),PackingDate,106), ' ','-') PackingDate,CurrencyId,UnitId,TinvoiceNo 
                    From Packing (Nolock) 
                    Where PackingId = " + ddInvoiceNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + @"
                    Select Distinct RollFrom, RollFrom Rollfrom1 
                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @" order by Rollfrom1
                    Select IsNull(Max(RollTo), 0) + 1 MaxRollNo, MulipleRollFlag 
                    From Packinginformation(Nolock) Where PackingId=" + ddInvoiceNo.SelectedValue + @"
                    Group by MulipleRollFlag";

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDCurrency.SelectedValue = Ds.Tables[0].Rows[0]["CurrencyId"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
            TxtInvoiceNo.Text = Ds.Tables[0].Rows[0]["TinvoiceNo"].ToString();
            ViewState["PackingID"] = Ds.Tables[0].Rows[0]["PackingId"].ToString();
            TxtDate.Text = Ds.Tables[0].Rows[0]["PackingDate"].ToString();
            Fill_Grid();
            ChkForChangeQDCCheckedChange();
            TRcarpetSet.Visible = true;
            //From Roll

            UtilityModule.ConditionalComboFillWithDS(ref DDfromroll, Ds, 1, true, "--SELECT--");
        }
        if (Session["varcompanyId"].ToString() == "30" || Session["varcompanyId"].ToString() == "38" )
        {
            if (Ds.Tables[2].Rows.Count > 0)
            {
                if (Ds.Tables[2].Rows[0]["MulipleRollFlag"].ToString() == "1")
                {
                    ChkForMulipleRolls.Checked = true;
                    ChkForMulipleRolls_CheckedChanged();
                }
                TxtRollNoFrom.Text = Ds.Tables[2].Rows[0]["MaxRollNo"].ToString();
                RollNoFromTextChanged();
            }
        }

       
    }
    protected void ChkForChangeQDC_CheckedChanged(object sender, EventArgs e)
    {
        ChkForChangeQDCCheckedChange();
    }
    private void ChkForChangeQDCCheckedChange()
    {
        if (ChkForChangeQDC.Checked == true && ddInvoiceNo.SelectedIndex > 0)
        {
            TrDownGrid.Visible = true;
            string Str = @"Select Replace(Str(PackingId)+'|'+Str(Finishedid)+'|'+Str(CQSRNO)+'|'+Str(CDSRNO)+'|'+Str(CCSRNO),' ','') SrNo,
                       Category_Name Category,Item_Name Item,Quality,Design,Color
                       From PackingInFormation PI,V_FinishedItemDetail VF Where PI.Finishedid=VF.Item_Finished_id And PI.PackingId=" + ddInvoiceNo.SelectedValue + "  And VF.MasterCompanyId=" + Session["varCompanyId"] + @"
                       Group By PackingId,Finishedid,CQSRNO,CDSRNO,CCSRNO,Category_Name,Item_Name,Quality,Design,Color,Price";
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            DGChangeQDC.DataSource = Ds;
            DGChangeQDC.DataBind();
        }
        else
        {
            TrDownGrid.Visible = false;
        }
    }
    protected void BtnSaveChangeQDC_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara1 = new SqlParameter[20];
            for (int i = 0; i < DGChangeQDC.Rows.Count; i++)
            {
                string StrID = DGChangeQDC.DataKeys[i].Value.ToString();
                //TextBox TxtCATEGORY = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtCATEGORY");
                //TextBox TxtITEM = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtITEM");
                TextBox TxtQUALITY = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtQUALITY");
                TextBox TxtDESIGN = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtDESIGN");
                TextBox TxtCOLOR = (TextBox)DGChangeQDC.Rows[i].FindControl("TxtCOLOR");

                _arrpara1[0] = new SqlParameter("@VarPackingId", SqlDbType.Int);
                _arrpara1[1] = new SqlParameter("@VarFinishedid", SqlDbType.Int);
                _arrpara1[2] = new SqlParameter("@VarCQSRNO", SqlDbType.Int);
                _arrpara1[3] = new SqlParameter("@VarCDSRNO", SqlDbType.Int);
                _arrpara1[4] = new SqlParameter("@VarCCSRNO", SqlDbType.Int);
                _arrpara1[5] = new SqlParameter("@Quality", SqlDbType.NVarChar, 250);
                _arrpara1[6] = new SqlParameter("@Design", SqlDbType.NVarChar, 250);
                _arrpara1[7] = new SqlParameter("@Color", SqlDbType.NVarChar, 250);

                _arrpara1[0].Value = Convert.ToInt32(StrID.Split('|')[0]);
                _arrpara1[1].Value = Convert.ToInt32(StrID.Split('|')[1]);
                _arrpara1[2].Value = Convert.ToInt32(StrID.Split('|')[2]);
                _arrpara1[3].Value = Convert.ToInt32(StrID.Split('|')[3]);
                _arrpara1[4].Value = Convert.ToInt32(StrID.Split('|')[4]);
                _arrpara1[5].Value = TxtQUALITY.Text;
                _arrpara1[6].Value = TxtDESIGN.Text;
                _arrpara1[7].Value = TxtCOLOR.Text;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PACKINGINFORMATION_QDC_Change", _arrpara1);
            }
            Tran.Commit();
            ChkForChangeQDC.Checked = false;
            ChkForChangeQDCCheckedChange();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmPacking.aspx");
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
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
            {
                if (Session["varcompanyId"].ToString() == "30")
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
                    {
                        DGOrderDetail.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGOrderDetail.Columns[i].HeaderText == "RATE PER PCS")
                    {
                        DGOrderDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select distinct CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId from V_FinishedItemDetail v ,orderdetail od Where OD.Item_Finished_Id=V.Item_Finished_Id and od.orderid=" + DDCustomerOrderNo.SelectedValue + " and ProductCode='" + TxtProdCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ddCategoryName.Items.Count > 0)
            {
                ddCategoryName.SelectedValue = ds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                Category_SelectedIndex_Change();
            }
            if (ddItemName.Items.Count > 0)
            {
                ddItemName.SelectedValue = ds.Tables[0].Rows[0]["ITEM_ID"].ToString();
                ItemNameSelectedIndexChange();
            }
            if (ddQuality.Items.Count > 0 && ddQuality.Visible == true)
            {
                ddQuality.SelectedValue = ds.Tables[0].Rows[0]["QualityId"].ToString();
                ComboFill();
                Fill_Price();
            }
            if (ddDesign.Items.Count > 0 && ddDesign.Visible == true)
            {
                ddDesign.SelectedValue = ds.Tables[0].Rows[0]["designId"].ToString();
                DesignSelectedChange();
            }
            if (ddColor.Items.Count > 0 && ddColor.Visible == true)
            {
                ddColor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                Fill_Price();
            }
            if (ddShape.Items.Count > 0 && ddShape.Visible == true)
            {
                ddShape.SelectedValue = ds.Tables[0].Rows[0]["ShapeId"].ToString();
                ShapeSelectedChange();
            }
            if (ddSize.Items.Count > 0 && ddSize.Visible == true)
            {
                ddSize.SelectedValue = ds.Tables[0].Rows[0]["SizeId"].ToString();
                SizeSelectedIndexChange();
            }
            if (ddShade.Items.Count > 0 && ddShade.Visible == true)
            {
                ddShade.SelectedValue = ds.Tables[0].Rows[0]["ShadecolorId"].ToString();
                Fill_Price();
            }
        }
        else
        {
            LblErrorMessage.Text = "Product Code Does Not Exist";
            LblErrorMessage.Visible = true;
        }
    }
    protected void TxtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        string str = "";
        if (ChkForEdit.Checked == true)
        {
            str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And PackingId<>" + ddInvoiceNo.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + ""));
        }
        else
        {
            str = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select TInvoiceNo from Packing Where TInvoiceNo='" + TxtInvoiceNo.Text.Trim() + "' And MasterCompanyId=" + Session["varCompanyId"] + ""));
        }
        if (str != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Opn1", "alert('Invoice Number " + str + " Already Exists...');", true);
            TxtInvoiceNo.Text = "";
        }
    }


    protected void DGOrderDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            SqlParameter[] array = new SqlParameter[3];
            array[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            array[1] = new SqlParameter("@PackingDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("Lblpacking")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeletePacking", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Delete", "alert('" + array[2].Value + "');", true);
            Fill_Grid();
            if (DGOrderDetail.Rows.Count == 0)
            {
                DDUnit.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    public string getStockNo(string strVal, string strval1)
    {
        string val = "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select strcarpets From [dbo].[GetTStockNosPack](" + strVal + ",0)");
        val = ds.Tables[0].Rows[0]["strcarpets"].ToString();
        return val;
    }

    protected void DGOrderDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGOrderDetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGOrderDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGOrderDetail.EditIndex = -1;
        Fill_Grid();
    }
    protected void DGOrderDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            SqlParameter[] array = new SqlParameter[7];
            array[0] = new SqlParameter("@PackingId", SqlDbType.Int);
            array[1] = new SqlParameter("@PackingDetailid", SqlDbType.Int);
            array[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[3] = new SqlParameter("@UCCNumber", SqlDbType.VarChar, 100);
            array[4] = new SqlParameter("@RUGID", SqlDbType.VarChar, 100);
            array[5] = new SqlParameter("@Rate", SqlDbType.Float);
            array[6] = new SqlParameter("@SinglePcsNetWt", SqlDbType.Float);

            array[0].Value = ((Label)DGOrderDetail.Rows[e.RowIndex].FindControl("Lblpacking")).Text;
            array[1].Value = DGOrderDetail.DataKeys[e.RowIndex].Value;
            array[2].Direction = ParameterDirection.Output;
            array[3].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtUccNumber")).Text;
            array[4].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtrugid")).Text;
            array[5].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtRate")).Text;
            array[6].Value = ((TextBox)DGOrderDetail.Rows[e.RowIndex].FindControl("txtSinglePcsNetWt")).Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_UPdatePacking]", array);
            Tran.Commit();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "update", "alert('" + array[2].Value + "');", true);
            DGOrderDetail.EditIndex = -1;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }


    }
    private void FtAreaCalculate(TextBox VarLengthNew, TextBox VarWidthNew, TextBox VarAreaNew, int VarFactor)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootHeight = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        int FootHeightInch = 0;
        int InchLength = 0;
        int InchWidth = 0;
        int InchHeight = 0;
        double VarArea = 0;
        double VarVolume = 0;
        string Str = "";

        Str = string.Format("{0:#0.00}", Convert.ToDouble(VarLengthNew.Text == "" ? "0" : VarLengthNew.Text));
        switch (Session["varcompanyNo"].ToString())
        {
            case "6":
            case "12":
                InchLength = Convert.ToInt32(Convert.ToDouble(VarLengthNew.Text) * 12);
                InchWidth = Convert.ToInt32(Convert.ToDouble(VarWidthNew.Text) * 12);
                break;
            default:
                if (VarLengthNew.Text != "")
                {
                    if (VarLengthNew.Text != "")
                    {
                        FootLength = Convert.ToInt32(Str.Split('.')[0]);
                        FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootLengthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12";
                        VarLengthNew.Text = "";
                        VarLengthNew.Focus();
                    }
                }
                if (VarWidthNew.Text != "")
                {
                    Str = string.Format("{0:#0.00}", Convert.ToDouble(VarWidthNew.Text));
                    if (VarWidthNew.Text != "")
                    {
                        FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                        FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                    }
                    if (FootWidthInch > 11)
                    {
                        LblErrorMessage.Text = "Inch value must be less than 12";
                        VarWidthNew.Text = "";
                        VarWidthNew.Focus();
                    }
                }
                InchLength = (FootLength * 12) + FootLengthInch;
                InchWidth = (FootWidth * 12) + FootWidthInch;
                break;
        }
        VarArea = Math.Round((Convert.ToDouble(InchLength) * Convert.ToDouble(InchWidth)) / 144 * VarFactor, 4);
        VarAreaNew.Text = Convert.ToString(VarArea);


    }
    protected void DDfromroll_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDtoroll, "select Distinct RollTo,RollTo as RollTo1 From Packinginformation Where PackingId=" + ddInvoiceNo.SelectedValue + " and RollFrom>=" + DDfromroll.SelectedValue + " order by RollTo1", true, "--Plz Select--");
    }
    protected void BuyerQualityDesignOrderWiseExportExcel()
    {
        try
        {            
            string str = "";

            string reporttitle = "";
            if (DDfromroll.SelectedIndex > 0)
            {
                reporttitle = reporttitle + " From Roll : " + DDfromroll.SelectedValue;
            }
            if (DDtoroll.SelectedIndex > 0)
            {
                reporttitle = reporttitle + " To Roll : " + DDtoroll.SelectedValue;
            }
           

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_BuyerQualityDesignOrderWisePackingEXCELREPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@RollFrom", DDfromroll.SelectedValue);
            cmd.Parameters.AddWithValue("@RollTo", DDtoroll.SelectedValue);
            cmd.Parameters.AddWithValue("@ReportTitle", reporttitle);            
            cmd.Parameters.AddWithValue("@PackingId", ddInvoiceNo.SelectedValue);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyId"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            ////cmd.Parameters.AddWithValue("@Where", str);           

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            //ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
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

                sht.Column("A").Width = 10.00;
                sht.Column("B").Width = 10.00;
                sht.Column("C").Width = 25.00;
                sht.Column("D").Width = 20.00;
                sht.Column("E").Width = 25.00;
                sht.Column("F").Width = 20.00;
                sht.Column("G").Width = 20.00;
                sht.Column("H").Width = 20.00;
                sht.Column("I").Width = 25.00;
                sht.Column("J").Width = 20.00;
                sht.Column("K").Width = 10.00;

                sht.Column("L").Width = 15.00;
                sht.Column("M").Width = 10.00;
               

                sht.Range("A1:M1").Merge();
                sht.Range("A1").Value = "PACKING DETAIL";
                sht.Range("A2:M2").Merge();
                sht.Range("A2").Value = "INVOICE NO:"+" "+ ddInvoiceNo.SelectedItem.Text;
                sht.Range("A3:M3").Merge();
                sht.Range("A3").Value = reporttitle;
                //sht.Row(1).Height = 30;
                sht.Range("A1:M3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A1:M3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:M1").Style.Alignment.SetWrapText();
                sht.Range("A2:M2").Style.Alignment.SetWrapText();
                sht.Range("A3:M3").Style.Alignment.SetWrapText();
                sht.Range("A1:M3").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A1:M3").Style.Font.FontSize = 15;
                sht.Range("A1:M3").Style.Font.Bold = true;

                //*******Header
                sht.Range("A4:B4").Merge();
                sht.Range("A4").Value = "Roll From -To";                
                sht.Range("C4").Value = "Buyer Quality";
                sht.Range("D4").Value = "Local Quality";
                sht.Range("E4").Value = "Buyer Design";
                sht.Range("F4").Value = "Local Design";
                sht.Range("G4").Value = "Color";
                sht.Range("H4").Value = "Size";
                sht.Range("I4").Value = "Customer OrderNo";
                sht.Range("J4").Value = "Local OrderNo";
                sht.Range("K4").Value = "Qty";

                if (Session["VarCompanyNo"].ToString() == "36")
                {
                    sht.Range("L4").Value = "Collection";
                    sht.Range("M4").Value = "StockNo";
                }

                // sht.Column("D").Width = 9.33;


                sht.Range("A4:M4").Style.Font.FontName = "Arial Unicode MS";
                sht.Range("A4:M4").Style.Font.FontSize = 10;
                sht.Range("A4:M4").Style.Font.Bold = true;
                //sht.Range("O3:T3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A4:M4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                sht.Range("A4:M4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A4:M4").Style.Alignment.SetWrapText();

                row = 5;
                int rowfrom = 5;

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                   
                    sht.Range("A" + row + ":B" + row).Merge();
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[j]["RollFrom"] + " - " + ds.Tables[0].Rows[j]["RollTo"]);                 
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[j]["BuyerQuality"]);
                    sht.Range("C" + row).Style.Alignment.SetWrapText();
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[j]["LocalQuality"]);
                    sht.Range("D" + row).Style.Alignment.SetWrapText();
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[j]["BuyerDesign"]);
                    sht.Range("E" + row).Style.Alignment.SetWrapText();
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[j]["LocalDesign"]);
                    sht.Range("F" + row).Style.Alignment.SetWrapText();
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[j]["LocalColor"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[j]["Size"]);
                    sht.Range("I" + row).SetValue(ds.Tables[0].Rows[j]["CustomerOrderNo"]);
                    sht.Range("J" + row).SetValue(ds.Tables[0].Rows[j]["LocalOrderNo"]);
                    sht.Range("K" + row).SetValue(ds.Tables[0].Rows[j]["Qty"]);

                    if (Session["VarCompanyNo"].ToString() == "36")
                    {
                        sht.Range("L" + row).SetValue(ds.Tables[0].Rows[j]["StyleNo"]);
                        sht.Range("M" + row).SetValue(ds.Tables[0].Rows[j]["StockNo"]);
                    }

                     row = row + 1;

                }
                sht.Range("J" + row).Value = "Total";
                sht.Range("K" + row).FormulaA1 = "=SUM(K" + (rowfrom) + ":K" + (row - 1) + ")";
                sht.Range("A" + row + ":J" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":J" + row).Style.Font.SetBold();
                row = row + 1;               

                
                //*************
                using (var a = sht.Range("A1:M" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }

                //sht.Columns(1, 30).AdjustToContents();

                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("BuyerQualityDesignPackingDetail_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "altre", "alert('No data fetched.')", true);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        if (ChkForExcel.Checked == true)
        {
            BuyerQualityDesignOrderWiseExportExcel();
        }
        else
        {
            string reporttitle = "";
            if (DDfromroll.SelectedIndex > 0)
            {
                reporttitle = reporttitle + " From Roll : " + DDfromroll.SelectedValue;
            }
            if (DDtoroll.SelectedIndex > 0)
            {
                reporttitle = reporttitle + " To Roll : " + DDtoroll.SelectedValue;
            }
            string str = @"select Vf.QualityName as Quality,vf.designName as Design,vf.ColorName as Color,Width+'x'+Length as Size,Sum(Pd.pcs) as Qty,dbo.F_GetstockNo(PD.ID)  As StockNo,pd.StyleNo,
                    P.TInvoiceNo,'" + reporttitle + @"' as Reporttitle";

            if (Session["varcompanyId"].ToString() == "16")
            {
                str = str + " ,'' RollFrom, '' RollTo";
            }
            else
            {
                str = str + " ,PD.RollFrom,PD.RollTo";
            }
            str = str + @" From Packing P inner Join PackingInformation Pd on P.PackingId=pd.PackingId
                     inner join V_FinishedItemDetail vf on PD.FinishedId=vf.ITEM_FINISHED_ID left join ordermaster OM on PD.Orderid=OM.orderid 
                     Where P.Packingid=" + ddInvoiceNo.SelectedValue;

            if (DDfromroll.SelectedIndex > 0)
            {
                str = str + " and RollFrom>=" + DDfromroll.SelectedValue + "";
            }
            if (DDtoroll.SelectedIndex > 0)
            {
                str = str + " and RollTO<=" + DDtoroll.SelectedValue + "";
            }
            str = str + " group by Vf.QualityName,vf.designName,vf.ColorName,Width,Length,P.TInvoiceNo,PD.StyleNo,PD.id";
            if (Session["varcompanyId"].ToString() != "16")
            {
                str = str + " ,PD.RollFrom,PD.RollTo";
                str = str + " Order By PD.RollFrom,PD.RollTo";
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["VarCompanyId"].ToString() == "36")
                {
                    Session["rptFileName"] = "reports/rptcarpetsetprasad.rpt";
                }
                else
                {
                    Session["rptFileName"] = "reports/rptcarpetset.rpt";
                
                
                }
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\rptcarpetset.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }

        
    }
    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void lnkchnginvoice_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Visible = false;
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Invoiceid", ddInvoiceNo.SelectedValue);
            param[1] = new SqlParameter("@InvoiceNo", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.InputOutput;
            param[1].Value = TxtInvoiceNo.Text;
            param[2] = new SqlParameter("@Invoicedate", SqlDbType.VarChar, 50);
            param[2].Direction = ParameterDirection.InputOutput;
            param[2].Value = TxtDate.Text;
            param[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_CHANGEINVOICENODATE", param);
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = param[3].Value.ToString();

        }
        catch (Exception ex)
        {
            Tran.Commit();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGOrderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGOrderDetail.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void TxtSearchInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtSearchInvoiceNo.Text != "")
        {
            try
            {
                string str = @"select consignorId,cosigneeId,InvoiceYear,InvoiceId,TInvoiceNo From INVOICE With (Nolock) Where Consignorid = " + Session["CurrentWorkingCompanyID"] + " And Tinvoiceno='" + TxtSearchInvoiceNo.Text + "'";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DDCustomerCode.Items.FindByValue(ds.Tables[0].Rows[0]["cosigneeId"].ToString()) != null)
                    {
                        DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["cosigneeId"].ToString();
                        CustomerCodeSelectedIndexChange();
                    }
                    if (ddInvoiceNo.Items.FindByValue(ds.Tables[0].Rows[0]["InvoiceId"].ToString()) != null)
                    {
                        ddInvoiceNo.SelectedValue = ds.Tables[0].Rows[0]["InvoiceId"].ToString();
                        ddInvoiceNo_SelectedIndexChanged(sender, new EventArgs());
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altinv", "alert('Invalid Invoice No. !!!')", true);
                }
                TxtSearchInvoiceNo.Text = "";
                TxtSearchInvoiceNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    protected void ChangeRate()
    {
        string OrderUnitId = "", PackingUnitid = "";
        string str = @"Select Distinct OM.OrderId,OD.OrderUnitid,OD.ordercaltype From OrderMaster(NoLock) OM JOIN OrderDetail (NoLock) OD ON OM.OrderId=OD.OrderId Where OM.OrderId=" + DDCustomerOrderNo.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            OrderUnitId = ds.Tables[0].Rows[0]["OrderUnitid"].ToString();
        }
        if (DDUnit.SelectedIndex > 0)
        {
            PackingUnitid = DDUnit.SelectedValue;
        }
        if (ds.Tables[0].Rows[0]["ordercaltype"].ToString() == "1")
        {
            if ((RDAreaWise.Checked))
            {
                TxtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text == "" ? "0" : TxtPrice.Text) / Convert.ToDecimal(TxtArea.Text), 2));
            }
        }
        if (OrderUnitId != "" && PackingUnitid != "")
        {
            if (OrderUnitId == "2" && PackingUnitid == "1")
            {
                TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) / 10.764), 2));

                TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
            }
            if (OrderUnitId == "1" && PackingUnitid == "2")
            {
                TxtPrice.Text = Convert.ToString(Math.Round((Convert.ToDouble(TxtPrice.Text) * 10.764), 2));

                TxtRatePerPcs.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TxtPrice.Text) * Convert.ToDecimal(TxtArea.Text), 2));
            }
        }
    }
    protected void PackingDetailReportInPDF()
    {
        LblErrorMessage.Text = "";
        try
        {

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GetPackingDetailReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
            cmd.Parameters.AddWithValue("@PackingId", ViewState["PackingID"]);
            cmd.Parameters.AddWithValue("@MasterCompanyId", Session["VarCompanyNo"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();
            //***********
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "Reports/RptPackingDetailReportNew.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptPackingDetailReportNew.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {        
       PackingDetailReportInPDF();       
    }
}