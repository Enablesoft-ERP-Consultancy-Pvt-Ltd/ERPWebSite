using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_RawMaterial_frmStocktransfer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname
                          Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                          SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                          dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID inner join UserRights_Category UC on(UC.CategoryId=ITEM_CATEGORY_MASTER.Category_Id And UC.UserId=" + Session["varuserid"] + @")
                          WHERE ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varCompanyId"] + @" And CategorySeparate.id=1
                        Select Distinct OM.CustomerId, CI.CustomerCode  
                        From OrderMaster OM(Nolock)
                        JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
                        Where OM.Status = 0 And OM.CompanyId = " + Session["CurrentWorkingCompanyID"].ToString();

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref DDFCompName, ds, 0, true, "Select Comp. Name");

            if (DDFCompName.Items.Count > 0)
            {
                DDFCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDFCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDFGodown, ds, 1, true, "Select From Godown");
            UtilityModule.ConditionalComboFillWithDS(ref DDTGodown, ds, 1, true, "Select TO Godown");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "Select Category");
            if (DDFCompName.Items.Count > 0)
            {
                //    DDFCompName.SelectedIndex = 1;
                CompanySelectedIndex();
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 3, true, "Select Customer");

            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            switch (Session["varcompanyId"].ToString())
            {
                case "9":
                    TxtChallanNo.Enabled = true;
                    break;
                case "16":
                    TDCustomerCode.Visible = true;
                    TDOrderNo.Visible = true;
                    break;
                case "21":
                    TDCustomerCode.Visible = true;
                    TDOrderNo.Visible = true;
                    break;
            }

            if (TDCustomerCode.Visible == true && DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                DDCustomerCodeSelectedChanged();
            }

            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDFBinNo.Visible = true;
                TDTBinNo.Visible = true;
            }
        }
    }
    protected void CompanySelectedIndex()
    {
        string str = "select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + "";
        //if (Session["varcompanyNo"].ToString() != "8")
        //{
        //    str = str + " And  CI.CompanyId<>" + DDFCompName.SelectedValue + "";
        //}
        str = str + "  Order By Companyname";
        UtilityModule.ConditionalComboFill(ref DDTCompName, str, true, "Select Comp. Name");
        if (DDTCompName.Items.Count > 0)
        {
            DDTCompName.SelectedIndex = 1;
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCatagorySelectedIndexChanged();
    }
    private void DDCatagorySelectedIndexChanged()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                     " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                     " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddCatagory.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dquality, "select Qualityid,Qualityname from Quality Where MasterCompanyId=" + Session["varCompanyId"], true, "--Select Quality--");
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order  by DesignName ", true, "Select Design");
                        break;
                    case "3":
                        TDColor.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varCompanyId"] + " order by colorname", true, "--Select Color--");
                        break;
                    case "4":
                        TDShape.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varCompanyId"] + " Order by ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varCompanyId"] + " order by sizeid ", true, "Size in Ft");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddlshade, "select shadecolorid,shadecolorname from shadecolor Where MasterCompanyId=" + Session["varCompanyId"] + " order by shadecolorname", true, "Select Shadecolor");
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref dditemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddCatagory.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " order by Item_Name", true, "--Select Item--");
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDItemNameSelectedChanged();
    }
    private void DDItemNameSelectedChanged()
    {
        string str = string.Empty;
        str = @"select QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + @" order by QualityName
                select Distinct U.UnitId,U.UnitName from Unit U inner join UNIT_TYPE_MASTER UT on U.UnitTypeID=UT.UnitTypeID inner join Item_master IM on Im.UnitTypeID=UT.UnitTypeID and Im.item_id=" + dditemname.SelectedValue + " order by unitname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 0, true, "Select Quality");
        UtilityModule.ConditionalComboFillWithDS(ref DDunit, ds, 1, true, "select unit");
        if (DDunit.Items.Count > 0)
        {
            DDunit.SelectedIndex = 1;
        }
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();
    }
    private void FillSize()
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
        if (ddshape.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid," + strSize + " from size Where MasterCompanyId=" + Session["varCompanyId"] + " And shapeid=" + ddshape.SelectedValue + " order by sizeid ", true, "Size in Ft");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid," + strSize + " from size Where MasterCompanyId=" + Session["varCompanyId"] + "  order by sizeid ", true, "Size in Ft");
        }
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSize();        
    }
    protected void DDFGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));

        if (TDFBinNo.Visible == true)
        {
            str = "select Distinct BinNo,BinNo From Stock  Where Companyid=" + DDFCompName.SelectedValue + " And Item_Finished_Id=" + ItemFinishedId + " And Godownid=" + DDFGodown.SelectedValue + @" and Round(Qtyinhand,3)>0";

            UtilityModule.ConditionalComboFill(ref DDFBinNo, str, true, "--Plz Select--");
        }
        else
        {
            //ViewState["FID"] = ItemFinishedId;

            str = "select Distinct LotNo,LotNo From Stock  Where Companyid=" + DDFCompName.SelectedValue + " And Item_Finished_Id=" + ItemFinishedId + " And Godownid=" + DDFGodown.SelectedValue + @" and Round(Qtyinhand,3)>0
               Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref ddlotno, ds, 0, true, "Select Lot No.");
            UtilityModule.ConditionalComboFillWithDS(ref DDTGodown, ds, 1, true, "Select ToGodown.");
        }

    }
    protected void ddlotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        if (MySession.TagNowise == "1")
        {
            DDTagNo.SelectedIndex = -1;
            str = "select Distinct TagNo,TagNo as TagNo1 From Stock  Where Companyid=" + DDFCompName.SelectedValue + " And Item_Finished_Id=" + ItemFinishedId + " And Godownid=" + DDFGodown.SelectedValue + " and LotNo='" + ddlotno.SelectedItem.Text + "' and Round(Qtyinhand,3)>0 ";
            if (TDFBinNo.Visible == true)
            {
                str = str + " and BinNo='" + DDFBinNo.SelectedItem.Text + "'";
            }
            str = str + " order by TagNo";
            UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "Select Tag No");
        }
        else
        {
            txtstock.Text = UtilityModule.getstockQty(DDFCompName.SelectedValue, DDFGodown.SelectedValue, ddlotno.SelectedItem.Text, ItemFinishedId, BinNo: (TDFBinNo.Visible == true ? DDFBinNo.SelectedItem.Text : "")).ToString();
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (TDOrderNo.Visible == true && DDOrderNo.Items.Count > 0 && Convert.ToInt32(DDOrderNo.SelectedValue) > 0)
        {
            if (Convert.ToDouble(txtissqty.Text) > Convert.ToDouble(TxtConsQty.Text))
            {
                MessageSave("Transfer qty can not be more than consmpqty....");
                txtissqty.Text = "";
                txtissqty.Focus();
                return;
            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[22];
            _array[0] = new SqlParameter("@Transferid", SqlDbType.Int);
            _array[1] = new SqlParameter("@FCompanyId", SqlDbType.Int);
            _array[2] = new SqlParameter("@TCompanyId", SqlDbType.Int);
            _array[3] = new SqlParameter("@FGodownid", SqlDbType.Int);
            _array[4] = new SqlParameter("@TGodownid", SqlDbType.Int);
            _array[5] = new SqlParameter("@FLotNo", SqlDbType.NVarChar, 500);
            _array[6] = new SqlParameter("@Item_Finishedid", SqlDbType.Int);
            _array[7] = new SqlParameter("@TransQty", SqlDbType.Float);
            _array[8] = new SqlParameter("@TransferDate", SqlDbType.DateTime);
            _array[9] = new SqlParameter("@DetailId", SqlDbType.Int);
            _array[10] = new SqlParameter("@ChallanNo", SqlDbType.NVarChar, 100);
            _array[11] = new SqlParameter("@Msgflag", SqlDbType.NVarChar, 100);
            _array[12] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[13] = new SqlParameter("@UnitId", SqlDbType.Int);
            _array[14] = new SqlParameter("@FTagNo", SqlDbType.NVarChar, 50);
            _array[15] = new SqlParameter("@Userid", SqlDbType.Int);
            _array[16] = new SqlParameter("@FBinNO", SqlDbType.VarChar, 50);
            _array[17] = new SqlParameter("@TBinNo", SqlDbType.VarChar, 50);
            _array[18] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 15);
            _array[19] = new SqlParameter("@Remarks", SqlDbType.VarChar, 150);
            _array[20] = new SqlParameter("@OrderID", SqlDbType.Int);
            _array[21] = new SqlParameter("@BellWt", SqlDbType.Float);

            _array[0].Direction = ParameterDirection.InputOutput;
            _array[0].Value = ViewState["Transferid"];
            _array[1].Value = DDFCompName.SelectedValue;
            _array[2].Value = DDTCompName.SelectedValue;
            _array[3].Value = DDFGodown.SelectedValue;
            _array[4].Value = DDTGodown.SelectedValue;
            _array[5].Value = ddlotno.SelectedItem.Text;
            _array[6].Value = ItemFinishedId;
            _array[7].Value = txtissqty.Text;
            _array[8].Value = txtdate.Text;
            _array[9].Direction = ParameterDirection.Output;
            _array[10].Direction = ParameterDirection.InputOutput;
            _array[10].Value = TxtChallanNo.Text;
            _array[11].Direction = ParameterDirection.Output;
            _array[12].Value = Session["VarcompanyId"];
            _array[13].Value = DDunit.SelectedValue;
            _array[14].Value = TDTagNo.Visible == true ? DDTagNo.SelectedItem.Text : "Without Tag No";
            _array[15].Value = Session["varuserid"];
            _array[16].Value = TDFBinNo.Visible == true ? DDFBinNo.SelectedItem.Text : "";
            _array[17].Value = TDTBinNo.Visible == true ? DDTBinNo.SelectedItem.Text : "";
            _array[18].Value = TxtEWayBillNo.Text;
            _array[19].Value = TxtRemarks.Text;
            if (TDOrderNo.Visible == true && DDOrderNo.Items.Count > 0)
            {
                _array[20].Value = TDOrderNo.Visible == true ? Convert.ToInt32(DDOrderNo.SelectedValue) > 0 ? Convert.ToInt32(DDOrderNo.SelectedValue) : 0 : 0;
            }
            else
            {
                _array[20].Value = 0;
            }
            _array[21].Value = TxtBellWeight.Text == "" ? "0" : TxtBellWeight.Text;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_StockTransfer", _array);

            ViewState["Transferid"] = _array[0].Value.ToString();
            ViewState["DetailId"] = _array[9].Value.ToString();
            TxtChallanNo.Text = _array[10].Value.ToString();
            if (_array[11].Value.ToString() != "")
            {
                MessageSave(_array[11].Value.ToString());
                Tran.Rollback();
                return;
            }
            //UtilityModule.StockStockTranTableUpdateNew(ItemFinishedId, Convert.ToInt32(DDFGodown.SelectedValue), Convert.ToInt32(DDFCompName.SelectedValue), ddlotno.SelectedItem.Text, Convert.ToDouble(txtissqty.Text), Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Stock_TransferDetail", Convert.ToInt32(ViewState["DetailId"]), Tran, 0, false, 1, 0, UnitId: Convert.ToInt16(DDunit.SelectedValue));
            //UtilityModule.StockStockTranTableUpdateNew(ItemFinishedId, Convert.ToInt32(DDTGodown.SelectedValue), Convert.ToInt32(DDTCompName.SelectedValue), ddlotno.SelectedItem.Text, Convert.ToDouble(txtissqty.Text), Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Stock_TransferDetail", Convert.ToInt32(ViewState["DetailId"]), Tran, 1, true, 1, 0, UnitId: Convert.ToInt16(DDunit.SelectedValue));

            Tran.Commit();
            MessageSave("Data Saved successfully....");
            txtissqty.Text = "";
            txtstock.Text = "";
            TxtBellWeight.Text = "";
            DDFGodown.SelectedIndex = 0;
            DDTGodown.SelectedIndex = 0;
            Fill_grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Fill_grid()
    {
        string str = @"select SM.TransferId,DetailId,FGodownid,TGodownid,FLotNo,TLotNo,Item_Finishedid,
                        CATEGORY_NAME,ITEM_NAME,QualityName+' '+designName+' '+ColorName+' '+ShapeName+' '+SizeFt+' '+ShadeColorName+'   '+U.unitname As DESCRIPTION,
                        Qty,FGM.GodownName as FromGodown,TGM.GodownName as ToGodown,FTagNo,isnull(SD.Remarks,'') as Remarks,isnull(SM.EwayBillNo,'') as EWayBillNo 
                        from  Stock_TransferMaster SM inner join Stock_TransferDetail SD on SM.TransferId=SD.TransferId
                        inner join V_FinishedItemDetail vf on vf.ITEM_FINISHED_ID=SD.Item_Finishedid
                        inner join GodownMaster FGM on FGM.GoDownID=sd.FGodownId 
                        inner join GodownMaster TGM on TGM.GoDownID=sd.TGodownId
                        left join unit u on u.unitid=sd.unitid
                        Where SM.Transferid=" + ViewState["Transferid"];

        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGStockTransfer.DataSource = ds.Tables[0];
            DGStockTransfer.DataBind();
        }
        else
        {
            DGStockTransfer.DataSource = null;
            DGStockTransfer.DataBind();
        }
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

    protected void DGStockTransfer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGStockTransfer.EditIndex = e.NewEditIndex;
        Fill_grid();
    }
    protected void DGStockTransfer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGStockTransfer.EditIndex = -1;
        Fill_grid();
    }

    protected void DDFCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Transferid"] = 0;
        ViewState["DetailId"] = 0;
        TxtChallanNo.Text = "";
        DGStockTransfer.DataSource = null;
        DGStockTransfer.DataBind();
        CompanySelectedIndex();
        DDTCompName_SelectedIndexChanged(sender, e);

    }
    protected void DDTCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGStockTransfer.DataSource = null;
        DGStockTransfer.DataBind();
        ViewState["Transferid"] = 0;
        ViewState["DetailId"] = 0;
        TxtChallanNo.Text = "";
        string str = "select Distinct TransferId,ChallanNo+' / ' + Replace(convert(varchar(11),TransferDate,106),' ','-') As ChallanNo  from Stock_TransferMaster Where FCompanyId=" + DDFCompName.SelectedValue + " And TCompanyId=" + DDTCompName.SelectedValue + "  Order by ChallanNo";
        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "Select Challan No...");
    }
    protected void ChkEdit_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["Transferid"] = 0;
        if (ChkEdit.Checked == true)
        {
            TxtChallanNo.Enabled = false;
            TDEditchallanNo.Visible = true;
            TxtChallanNo.Text = "";
            FillChallan();
        }
        else
        {
            TxtChallanNo.Text = "";
            TxtChallanNo.Enabled = true;
            DDChallanNo.Items.Clear();
            TDEditchallanNo.Visible = false;
        }
    }
    protected void FillChallan()
    {
        string str = "select Distinct SM.TransferId,SM.ChallanNo+' / ' + Replace(convert(varchar(11),SM.TransferDate,106),' ','-') As ChallanNo  from Stock_TransferMaster SM inner join Stock_TransferDetail SD on SM.TransferId=SD.TransferId Where SM.FCompanyId=" + DDFCompName.SelectedValue + " And SM.TCompanyId=" + DDTCompName.SelectedValue;
        if (DDTGodown.SelectedIndex > 0)
        {
            str = str + " And SD.TGodownId=" + DDTGodown.SelectedValue;
        }
        str = str + " Order by TransferId ";

        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "Select Challan No...");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Transferid"] = DDChallanNo.SelectedValue;
        string str = @"Select a.ChallanNo, Replace(convert(varchar(11), a.TransferDate, 106), '  ', '-') As TransferDate,  IsNull(OM.CustomerID, 0) CustomerID, IsNull(a.OrderID, 0) OrderID 
            From Stock_TransferMaster a(Nolock) 
            Left Join OrderMaster OM(Nolock) ON Om.OrderID = a.OrderID 
            Where a.TransferId = " + DDChallanNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtChallanNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();
            txtdate.Text = ds.Tables[0].Rows[0]["TransferDate"].ToString();
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["ChallanNo"]) > 0)
            {
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                DDCustomerCodeSelectedChanged();
                DDOrderNo.SelectedValue = ds.Tables[0].Rows[0]["OrderID"].ToString();
                if (Session["VarCompanyNo"].ToString() == "16")
                {
                    FillDGOrderConsmption();
                }
            }
        }
        else
        {
            TxtChallanNo.Text = "";
        }
        Fill_grid();
    }
    protected void DGStockTransfer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int DetailId = Convert.ToInt32(DGStockTransfer.DataKeys[e.RowIndex].Value);
            int ItemFinishedId = Convert.ToInt32(((Label)DGStockTransfer.Rows[e.RowIndex].FindControl("Finishedid")).Text);
            int FGodownid = Convert.ToInt32(((Label)DGStockTransfer.Rows[e.RowIndex].FindControl("FGodownId")).Text);
            int TGodownid = Convert.ToInt32(((Label)DGStockTransfer.Rows[e.RowIndex].FindControl("TGodownid")).Text);
            string LotNo = ((Label)DGStockTransfer.Rows[e.RowIndex].FindControl("FLotNo")).Text;
            Double Qty = Convert.ToDouble(((TextBox)DGStockTransfer.Rows[e.RowIndex].FindControl("Qty")).Text);

            string EWayBillNo = ((TextBox)DGStockTransfer.Rows[e.RowIndex].FindControl("txtEWayBillNo")).Text;
            string Remarks = ((TextBox)DGStockTransfer.Rows[e.RowIndex].FindControl("txtRemarks")).Text;

            SqlParameter[] _array = new SqlParameter[6];
            _array[0] = new SqlParameter("@DetailId", SqlDbType.Int);
            _array[1] = new SqlParameter("@NewQty", SqlDbType.Float);
            _array[2] = new SqlParameter("@userid", SqlDbType.Int);
            _array[3] = new SqlParameter("@msg", SqlDbType.VarChar, 50);
            _array[4] = new SqlParameter("@EWayBillNo", SqlDbType.VarChar, 15);
            _array[5] = new SqlParameter("@Remarks", SqlDbType.VarChar, 150);

            _array[0].Value = DetailId;
            _array[1].Value = Qty;
            _array[2].Value = Session["varuserid"];
            _array[3].Direction = ParameterDirection.Output;
            _array[4].Value = EWayBillNo;
            _array[5].Value = Remarks;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateStock_Transfer", _array);

            //UtilityModule.StockStockTranTableUpdate(ItemFinishedId, FGodownid, Convert.ToInt32(DDFCompName.SelectedValue), LotNo, Qty, Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Stock_TransferDetail", DetailId, Tran, 0, false, 1, 0);
            //UtilityModule.StockStockTranTableUpdate(ItemFinishedId, TGodownid, Convert.ToInt32(DDTCompName.SelectedValue), LotNo, Qty, Convert.ToDateTime(txtdate.Text).ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Stock_TransferDetail", DetailId, Tran, 1, true, 1, 0);
            if (_array[3].Value.ToString() != "")
            {
                MessageSave(_array[3].Value.ToString());
            }
            Tran.Commit();
            MessageSave("Data Updated successfully......");
            DGStockTransfer.EditIndex = -1;
            Fill_grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void DGStockTransfer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int DetailId = Convert.ToInt32(DGStockTransfer.DataKeys[e.RowIndex].Value);
            SqlParameter[] _array = new SqlParameter[3];
            _array[0] = new SqlParameter("@DetailId", SqlDbType.Int);
            _array[1] = new SqlParameter("@TransferId", SqlDbType.Int);
            _array[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            _array[0].Value = DetailId;
            _array[1].Value = ViewState["Transferid"];
            _array[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteStock_Transfer", _array);
            Tran.Commit();
            if (_array[2].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altdel", "alert('" + _array[2].Value.ToString() + "');", true);
            }
            else
            {
                MessageSave("Data Deleted successfully......");
            }
            Fill_grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            MessageSave(ex.Message);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void DDTGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDTBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
                UtilityModule.FillBinNO(DDTBinNo, Convert.ToInt32(DDTGodown.SelectedValue), ItemFinishedId, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDTBinNo, "SELECT BINNO,BINNO FROM BINMASTER WHERE GODOWNID=" + DDTGodown.SelectedValue + " ORDER BY BINID", true, "--Plz Select--");
            }
        }
        if (ChkEdit.Checked == true)
        {
            FillChallan();
        }
    }
    protected void DDTagNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        txtstock.Text = UtilityModule.getstockQty(DDFCompName.SelectedValue, DDFGodown.SelectedValue, ddlotno.SelectedItem.Text, ItemFinishedId, DDTagNo.SelectedItem.Text, BinNo: (TDFBinNo.Visible == true ? DDFBinNo.SelectedItem.Text : "")).ToString();
    }
    protected void DGStockTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[GetGridColumnId("TagNo")].Visible = false;
        if (MySession.TagNowise == "1")
        {
            e.Row.Cells[GetGridColumnId("TagNo")].Visible = true;
        }

    }
    protected int GetGridColumnId(string ColName)
    {
        int columnid = -1;
        foreach (DataControlField col in DGStockTransfer.Columns)
        {
            if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
            {
                columnid = DGStockTransfer.Columns.IndexOf(col);
                break;
            }
        }
        return columnid;
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from V_StockTransfer Where Transferid=" + ViewState["Transferid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyId"].ToString())
            {
                case "16":
                    Session["rptFileName"] = "~\\Reports\\RptstockTransfernew.rpt";
                    break;
                case "21":
                    Session["rptFileName"] = "~\\Reports\\RptstockTransferKaysons.rpt";
                    break;
                case "43":
                    Session["rptFileName"] = "~\\Reports\\RptstockTransferCarpetInternational.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\RptstockTransfer.rpt";
                    break;
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptstockTransfer.xsd";
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

    protected void DDFBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ItemFinishedId = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varCompanyId"]));
        //ViewState["FID"] = ItemFinishedId;
        string str = "";
        str = "select Distinct LotNo,LotNo From Stock  Where Companyid=" + DDFCompName.SelectedValue + " And Item_Finished_Id=" + ItemFinishedId + " And Godownid=" + DDFGodown.SelectedValue + @" and Round(Qtyinhand,3)>0";
        if (TDFBinNo.Visible == true)
        {
            str = str + " and BinNo='" + DDFBinNo.SelectedItem.Text + "'";
        }
        str = str + "  Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        UtilityModule.ConditionalComboFillWithDS(ref ddlotno, ds, 0, true, "Select Lot No.");
        UtilityModule.ConditionalComboFillWithDS(ref DDTGodown, ds, 1, true, "Select ToGodown.");
    }
    protected void DGOrderConsmption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderConsmption, "Select$" + e.Row.RowIndex);

            for (int i = 0; i < DGOrderConsmption.Columns.Count; i++)
            {
                //if (DGOrderConsmption.Columns[i].HeaderText == "Already TransferQty")
                //{
                //    DGOrderConsmption.Columns[i].Visible = false;
                //}
                //if (DGOrderConsmption.Columns[i].HeaderText == "Bal Qty")
                //{
                //    DGOrderConsmption.Columns[i].Visible = false;
                //}

                if (Session["varcompanyId"].ToString() == "21")
                {
                    if (DGOrderConsmption.Columns[i].HeaderText == "Already TransferQty" || DGOrderConsmption.Columns[i].HeaderText == "Bal Qty")
                    {
                        DGOrderConsmption.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DGOrderConsmption.Columns[i].HeaderText == "Already TransferQty" || DGOrderConsmption.Columns[i].HeaderText == "Bal Qty")
                    {
                        DGOrderConsmption.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDCustomerCodeSelectedChanged();
    }
    private void DDCustomerCodeSelectedChanged()
    {
        if (TDOrderNo.Visible == true)
        {
            string str = "";
            if (Session["VarCompanyNo"].ToString() == "21")
            {

                str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.ORDERDETAILID = OD.OrderDetailId And OCD.PROCESSID = 1 And OCD.IQty > 0 
                JOIN JobAssigns JA(Nolock) ON JA.OrderID = OM.OrderID AND JA.ITEM_FINISHED_ID = OD.Item_Finished_Id And JA.PreProdAssignedQty > 0 
                Where OM.Status = 0 And OM.CompanyId = " + DDFCompName.SelectedValue + " And CustomerID = " + DDCustomerCode.SelectedValue + @" 
                Order By OM.CustomerOrderNo ";

//                str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
//                From OrderMaster OM(Nolock) 
//                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID                 
//                JOIN JobAssigns JA(Nolock) ON JA.OrderID = OM.OrderID AND JA.ITEM_FINISHED_ID = OD.Item_Finished_Id  
//                Where OM.Status = 0 And OM.CompanyId = " + DDFCompName.SelectedValue + " And CustomerID = " + DDCustomerCode.SelectedValue + @" 
//                Order By OM.CustomerOrderNo ";
            }
            else  if (Session["VarCompanyNo"].ToString() == "16")
            {

                str = @"Select Distinct OM.OrderID, OM.CustomerOrderNo 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.ORDERDETAILID = OD.OrderDetailId And OCD.PROCESSID = 1 And OCD.IQty > 0 
                JOIN JobAssigns JA(Nolock) ON JA.OrderID = OM.OrderID AND JA.ITEM_FINISHED_ID = OD.Item_Finished_Id And JA.INTERNALPRODASSIGNEDQTY > 0 
                Where OM.Status = 0 And OM.CompanyId = " + DDFCompName.SelectedValue + " And CustomerID = " + DDCustomerCode.SelectedValue + @" 
                Order By OM.CustomerOrderNo ";
            }

            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "Select Order No");
            if (DDTCompName.Items.Count > 0)
            {
                DDTCompName.SelectedIndex = 1;
            }
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {        
            FillDGOrderConsmption();    
       
    }
    private void FillDGOrderConsmption()
    {
        TDDGOrderConsmption.Visible = true;

        string str = "";
        if (DDOrderNo.SelectedIndex > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "21")
            {
                str = @"Select OM.OrderID, OCD.IFINISHEDID, --OD.OrderDetailId, OD.Item_Finished_Id, OCD.ICALTYPE, 
                VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.SizeFt  + ' ' + VF1.ShadeColorName [Description], 
                ROUND(Sum(CASE WHEN OCD.ICALTYPE<>1 THEN CASE WHEN OD.OrderUnitID = 1 THEN VF.AreaMtr * JA.PreProdAssignedQty * OCD.IQTY * 1.196 ELSE 
	                CASE WHEN VF.MasterCompanyId in (16, 28) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else VF.Actualfullareasqyd End * JA.PreProdAssignedQty * OCD.IQTY END ELSE 
	                CASE WHEN OD.OrderUnitID = 1 THEN JA.PreProdAssignedQty * OCD.IQTY * 1.196 ELSE JA.PreProdAssignedQty * OCD.IQTY END END), 3) ConsQty, 
                isnull((Select sum(Qty) from Stock_TransferMaster STM(NoLock) JOIN Stock_TransferDetail STD(NoLock) on STM.TransferId=STD.TransferId
					Where STM.OrderID=" + DDOrderNo.SelectedValue + @" and STD.Item_Finishedid=OCD.IFINISHEDID),0) as AlreadyTransferQty,
                VF1.CATEGORY_ID CategoryID, VF1.Item_ID ItemID, VF1.QualityId, VF1.designId, VF1.ColorId, VF1.ShapeId, VF1.SizeId, VF1.ShadecolorId 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.ORDERDETAILID = OD.OrderDetailId And OCD.PROCESSID = 1 And OCD.IQty > 0 
                JOIN JobAssigns JA(Nolock) ON JA.OrderID = OM.OrderID AND JA.ITEM_FINISHED_ID = OD.Item_Finished_Id And JA.PreProdAssignedQty > 0 
                JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                Where OM.OrderID = " + DDOrderNo.SelectedValue + @"
                Group By OM.OrderID, OCD.IFINISHEDID, OCD.ICALTYPE, VF1.ITEM_NAME, VF1.QualityName, VF1.DesignName, VF1.ColorName, VF1.ShapeName, VF1.SizeFt, VF1.ShadeColorName, 
                VF1.CATEGORY_ID, VF1.Item_ID, VF1.QualityId, VF1.designId, VF1.ColorId, VF1.ShapeId, VF1.SizeId, VF1.ShadecolorId --, OD.OrderDetailId, OD.Item_Finished_Id 
                Order By VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.SizeFt  + ' ' + VF1.ShadeColorName  ";
            }
            else
            {
                str = @"Select OM.OrderID, OCD.IFINISHEDID, --OD.OrderDetailId, OD.Item_Finished_Id, OCD.ICALTYPE, 
                VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.SizeFt  + ' ' + VF1.ShadeColorName [Description], 
                ROUND(Sum(CASE WHEN OCD.ICALTYPE<>1 THEN CASE WHEN OD.OrderUnitID = 1 THEN VF.AreaMtr * JA.INTERNALPRODASSIGNEDQTY * OCD.IQTY * 1.196 ELSE 
	                CASE WHEN VF.MasterCompanyId in (16, 28) Then Round(VF.AreaFt * 144.0 / 1296.0, 4, 1) Else VF.Actualfullareasqyd End * JA.INTERNALPRODASSIGNEDQTY * OCD.IQTY END ELSE 
	                CASE WHEN OD.OrderUnitID = 1 THEN JA.INTERNALPRODASSIGNEDQTY * OCD.IQTY * 1.196 ELSE JA.INTERNALPRODASSIGNEDQTY * OCD.IQTY END END), 3) ConsQty,
                isnull((Select sum(Qty) from Stock_TransferMaster STM(NoLock) JOIN Stock_TransferDetail STD(NoLock) on STM.TransferId=STD.TransferId
					Where STM.OrderID=" + DDOrderNo.SelectedValue + @" and STD.Item_Finishedid=OCD.IFINISHEDID),0) as AlreadyTransferQty, 
                VF1.CATEGORY_ID CategoryID, VF1.Item_ID ItemID, VF1.QualityId, VF1.designId, VF1.ColorId, VF1.ShapeId, VF1.SizeId, VF1.ShadecolorId 
                From OrderMaster OM(Nolock) 
                JOIN OrderDetail OD(Nolock) ON OD.OrderID = OM.OrderID 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.ORDERDETAILID = OD.OrderDetailId And OCD.PROCESSID = 1 And OCD.IQty > 0 
                JOIN JobAssigns JA(Nolock) ON JA.OrderID = OM.OrderID AND JA.ITEM_FINISHED_ID = OD.Item_Finished_Id And JA.INTERNALPRODASSIGNEDQTY > 0 
                JOIN V_FinishedItemDetail VF1(Nolock) ON VF1.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                Where OM.OrderID = " + DDOrderNo.SelectedValue + @"
                Group By OM.OrderID, OCD.IFINISHEDID, OCD.ICALTYPE, VF1.ITEM_NAME, VF1.QualityName, VF1.DesignName, VF1.ColorName, VF1.ShapeName, VF1.SizeFt, VF1.ShadeColorName, 
                VF1.CATEGORY_ID, VF1.Item_ID, VF1.QualityId, VF1.designId, VF1.ColorId, VF1.ShapeId, VF1.SizeId, VF1.ShadecolorId --, OD.OrderDetailId, OD.Item_Finished_Id 
                Order By VF1.ITEM_NAME + ' ' + VF1.QualityName + ' ' + VF1.DesignName + ' ' + VF1.ColorName + ' ' + VF1.ShapeName + ' ' + VF1.SizeFt  + ' ' + VF1.ShadeColorName  ";
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            DGOrderConsmption.DataSource = ds.Tables[0];
            DGOrderConsmption.DataBind();
        }
    }
    protected void DGOrderConsmption_SelectedIndexChanged(object sender, EventArgs e)
    {
        TxtConsQty.Text = "";
        ddCatagory.Enabled = false;
        dditemname.Enabled = false;

        int n = DGOrderConsmption.SelectedIndex;
        ddCatagory.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("CategoryID")).Text;
        DDCatagorySelectedIndexChanged();
        dditemname.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("ItemID")).Text;
        DDItemNameSelectedChanged();
        if (TDQuality.Visible == true)
        {
            dquality.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("QualityID")).Text;
            dquality.Enabled = false;
        }
        if (TDDesign.Visible == true)
        {
            dddesign.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("designId")).Text;
            dddesign.Enabled = false;
        }
        if (TDColor.Visible == true)
        {
            ddcolor.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("ColorId")).Text;
            ddcolor.Enabled = false;
        }
        if (TDShape.Visible == true)
        {
            ddshape.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("ShapeId")).Text;
            ddshape.Enabled = false;
            FillSize();
        }
        if (TDSize.Visible == true)
        {
            ddsize.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("SizeId")).Text;
            ddsize.Enabled = false;
        }
        if (TDShade.Visible == true)
        {
            ddlshade.SelectedValue = ((Label)DGOrderConsmption.Rows[n].FindControl("ShadecolorId")).Text;
            ddlshade.Enabled = false;
        }
        TxtConsQty.Text = ((Label)DGOrderConsmption.Rows[n].FindControl("lblConsQty")).Text;
    }
}