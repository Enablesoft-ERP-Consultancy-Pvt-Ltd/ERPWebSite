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

public partial class Masters_RawMaterial_DirectStock : System.Web.UI.Page
{
    int ItemFinishedId = 0;
    string remainstock = null;
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);

        hnVarNewQualitySize.Value = variable.VarNewQualitySize;
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        lblerror.Text = "";
        Label2.Text = "";
        lblMessageVal.Text = "";
        lblsave.Text = "";
        lblmessage.Text = "";
        txtstockid.Text = "0";
        if (!IsPostBack)
        {
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txttrandate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                        Select VarProdCode from MasterSetting";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddlcompany, ds, 0, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddlgodown, ds, 1, true, "Select Godown");

            if (ddlcompany.Items.Count > 0)
            {
                ddlcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddlcompany.Enabled = false;
            }

            int VarProdCode = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
            if (VarProdCode == 1)
            {
                code.Visible = true;
            }
            else
            {
                code.Visible = false;
            }
            lablechange();
            //****************************************************
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "7":
                    tdlotno.Visible = false;
                    tdrate.Visible = false;
                    break;
                case "8":
                    LblLotNo.Text = "Lot No./Batch No.";
                    break;
                case "9":
                    ChkForExcelExport.Visible = true;
                    break;
                case "10":
                    tdlotno.Visible = false;
                    break;
                case "14":
                    TDnoofCone.Visible = true;
                    break;
                case "16":
                    TxtShadeColorFill.Visible = true;
                    break;
                case "21":
                    ChkMtr.Checked = true;
                    TDDirectStockRemark.Visible = true;
                    break;
                case "28":
                    TxtShadeColorFill.Visible = true;
                    break;
                case "36":
                    TDDirectStockRemark.Visible = true;
                    break;
                case "42":
                    BtnStockNoToVCMSale.Visible = true;
                    TDUpdatePrice.Visible = true;
                    ChkForInchSize.Visible = true;
                    break;
                case "44":
                    ChkForExcelExport.Visible = false;
                    ChkForInchSize.Visible = true;
                    break;
                default:
                    ChkForExcelExport.Visible = false;
                    break;
            }
            //TagNo
            if (MySession.TagNowise == "1")
            {
                TDTagNo.Visible = true;
            }
        }
    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];
        lblshadecolor.Text = ParameterList[7];
    }
    protected void ddlcatagoryname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        TxtFinishedid.Text = "Category=" + ddlcatagoryname.SelectedValue;
    }
    private void ddlcategorycange()
    {
        ql.Visible = false;
        clr.Visible = false;
        dsn.Visible = false;
        shp.Visible = false;
        sz.Visible = false;
        shd.Visible = false;
        string strsql = "SELECT [CATEGORY_PARAMETERS_ID],[CATEGORY_ID],IPM.[PARAMETER_ID],PARAMETER_NAME " +
                      " FROM [ITEM_CATEGORY_PARAMETERS] IPM inner join PARAMETER_MASTER PM on " +
                      " IPM.[PARAMETER_ID]=PM.[PARAMETER_ID] where [CATEGORY_ID]=" + ddlcatagoryname.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        ql.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dquality, "select Qualityid,Qualityname from Quality Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by qualityname", true, "--Select Quality--");
                        break;
                    case "2":
                        dsn.Visible = true;
                        UtilityModule.ConditionalComboFill(ref dddesign, "select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName ", true, "Select Design");
                        break;
                    case "3":
                        clr.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolor, "SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName", true, "--Select Color--");
                        break;
                    case "4":
                        shp.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName", true, "--Select Shape--");
                        break;
                    case "5":
                        sz.Visible = true;
                        if (variable.VarNewQualitySize == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew order by sizeid ", true, "Size in Ft");
                        }
                        else
                        {
                            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "Size in Mtr");
                            }
                            else if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft+'  '+'['+Prodsizeft+']' from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "Size in Mtr");
                            }
                            else
                            {
                                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeid ", true, "Size in Ft");
                            }
                        }
                        break;
                    case "6":
                        shd.Visible = true;
                        FillShadeColorName();
                        break;
                }
            }
        }
        UtilityModule.ConditionalComboFill(ref ddlitemname, "Select Distinct Item_Id,Item_Name from Item_Master where Category_Id=" + ddlcatagoryname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Item_Name", true, "--Select Item--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        CheckBackDateEntryStop();
        if (lblMessageVal.Text == "")
        {
            if (txtopeningstock.Text != "")
            {
                ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                if (ddlcatagorytype.SelectedValue == "1")
                {
                    string BinNO = "", TagNo = "", Lotno = "";
                    if (btnsave.Text == "Save")
                    {
                        if (TDBinnowise.Visible == true)
                        {
                            BinNO = DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
                        }

                        Lotno = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text;

                        TagNo = txttagno.Text == "" ? "Without Tag No" : txttagno.Text;

                        if (variable.VarCHECKBINCONDITION == "1")
                        {

                            SqlParameter[] param = new SqlParameter[7];
                            param[0] = new SqlParameter("@BinNo", BinNO);
                            param[1] = new SqlParameter("@GODOWNID", ddlgodown.SelectedValue);
                            param[2] = new SqlParameter("@ITEMFINISHEDID", ItemFinishedId);
                            param[3] = new SqlParameter("@QTY", txtopeningstock.Text == "" ? "0" : txtopeningstock.Text);
                            param[4] = new SqlParameter("@TAGNO", TagNo);
                            param[5] = new SqlParameter("@Lotno", Lotno);
                            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                            param[6].Direction = ParameterDirection.Output;
                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_BinNovalidate", param);
                            if (param[6].Value.ToString() != "")
                            {
                                lblmessage.Visible = true;
                                lblmessage.Text = param[6].Value.ToString();
                                return;
                            }


                        }
                        else if (btnsave.Text == "Update")
                        {
                            SqlParameter[] param = new SqlParameter[11];
                            param[0] = new SqlParameter("@BinNo", BinNO);
                            param[1] = new SqlParameter("@GODOWNID", ddlgodown.SelectedValue);
                            param[2] = new SqlParameter("@ITEMFINISHEDID", ItemFinishedId);
                            param[3] = new SqlParameter("@QTY", txtopeningstock.Text == "" ? "0" : txtopeningstock.Text);
                            param[4] = new SqlParameter("@TAGNO", TagNo);
                            param[5] = new SqlParameter("@Lotno", Lotno);
                            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                            param[6].Direction = ParameterDirection.Output;
                            param[7] = new SqlParameter("@UPDATE_DEL", SqlDbType.Int);
                            param[7].Value = 0;
                            param[8] = new SqlParameter("@TRANTYPE", SqlDbType.Int);
                            param[8].Value = 1;
                            param[9] = new SqlParameter("@TABLENAME", SqlDbType.VarChar, 20);
                            param[9].Value = "Opening stock";
                            param[10] = new SqlParameter("@PRTID", SqlDbType.Int);
                            param[10].Value = 0;


                            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_BINNOVALIDATE_UPDATEDEL", param);
                            if (param[6].Value.ToString() != "")
                            {
                                lblmessage.Visible = true;
                                lblmessage.Text = param[6].Value.ToString();
                                return;
                            }
                        }
                    }
                    saverawdetail();
                }
                else
                {
                    savecarpetdetail();
                }
                if (ddlcatagorytype.SelectedValue == "0" && variable.Carpetcompany == "1")
                {
                    FillGridFinisheditem();
                }
                else
                {
                    fill_grid();
                }
                if (ddlcatagorytype.SelectedValue != "0")
                {
                    Refresh();
                }

            }
            else
            {
                lblsave.Visible = true;
            }
            hnstockid.Value = "0";
        }
    }
    private void saverawdetail()
    {
        int varfinishtype_Id = 0;
        string BinNo = "";
        if (TDBinnowise.Visible == true)
        {
            BinNo = DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
        }
        if (txtlotno.Text == "")
        {
            txtlotno.Text = "Without Lot No";
        }
        if (txttagno.Text == "")
        {
            txttagno.Text = "Without Tag No";
        }
        if (TdFINISHED_TYPE.Visible == false)
        {
            varfinishtype_Id = 0;
        }
        else
        {
            varfinishtype_Id = Convert.ToInt32(ddFINISHED_TYPE.SelectedValue);
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "and lotno='" + txtlotno.Text + "' and TagNo='" + txttagno.Text + "' And Finished_Type_Id=" + varfinishtype_Id + " and BinNo='" + BinNo + "'";
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, strstock);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int openstock = Convert.ToInt32(ds.Tables[0].Rows[0]["openstock"].ToString());
                int stockid = Convert.ToInt32(ds.Tables[0].Rows[0]["StockID"]);
                string str = @"Select StockId,Quantity,isnull(coneuse,0) From StockTran where TableName='Opening stock' And StockID=" + stockid + " and Unitid=" + ddlunit.SelectedValue;
                DataSet ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    Double Qty = Convert.ToDouble(ds1.Tables[0].Rows[0]["Quantity"]);
                    Double coneuse = Convert.ToDouble(ds1.Tables[0].Rows[0]["Quantity"]);
                    string str1 = @"Update Stock set QtyinHand=QtyinHand-(" + Qty + "),OpenStock=OpenStock-(" + Qty + "),Noofcone=noofcone-(" + coneuse + ") where stockId=" + stockid + "";
                    str1 = str1 + @" Delete From Stocktran where StockId=" + stockid + " And TableName='Opening stock' and unitid=" + ddlunit.SelectedValue;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

                }
            }
            UtilityModule.StockStockTranTableUpdateWithOpeningStock(ItemFinishedId, Convert.ToInt32(ddlgodown.SelectedValue), Convert.ToInt32(ddlcompany.SelectedValue), txtlotno.Text.ToString(), Convert.ToDouble(txtopeningstock.Text), txtdate.Text.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Opening stock", 0, Tran, 1, true, Convert.ToInt32(ddlcatagorytype.SelectedValue), varfinishtype_Id, Convert.ToDouble(TxtRate.Text), Unitid: Convert.ToInt16(ddlunit.SelectedValue), Noofcone: Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text), TagNo: txttagno.Text, userid: Convert.ToInt16(Session["varuserid"]), BinNo: BinNo);
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        lblmessage.Visible = true;
    }
    private void savecarpetdetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            int VarOpeningQty = Convert.ToInt32(txtopeningstock.Text);
            int a = 0;
            //for (int i = 1; i <= VarOpeningQty; i++)
            //{
            //    string VarTStockNoCheck = txtprefix.Text + Txtpostfix.Text;
            //    DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select TstockNo from CarpetNumber Where TstockNo='" + VarTStockNoCheck + "'");
            //    if (Ds.Tables[0].Rows.Count > 0)
            //    {
            //        a = 1;
            //    }
            //}
            if (a == 0)
            {
                if (btnsave.Text == "Update")
                {
                    saverawdetail2();
                }
                else
                {
                    saverawdetailcarpet();
                }
                ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
                string VarTStockNo = txtprefix.Text + Txtpostfix.Text;
                int VarTStockNo1 = Convert.ToInt32(Txtpostfix.Text);
                SqlParameter[] arr = new SqlParameter[12];
                arr[0] = new SqlParameter("@finishedid", SqlDbType.Int);
                arr[1] = new SqlParameter("@companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@TStockNo", SqlDbType.NVarChar);
                arr[3] = new SqlParameter("@TStockNo1", SqlDbType.Int);
                arr[4] = new SqlParameter("@prefix", SqlDbType.NVarChar);
                arr[5] = new SqlParameter("@recdate", SqlDbType.DateTime);
                arr[6] = new SqlParameter("@stock", SqlDbType.Int);
                arr[7] = new SqlParameter("@Userid", SqlDbType.Int);
                arr[8] = new SqlParameter("@Unitid", SqlDbType.Int);
                arr[9] = new SqlParameter("@Recqty", SqlDbType.Int);
                arr[10] = new SqlParameter("@Postfix", SqlDbType.Int);
                arr[11] = new SqlParameter("@DirectStockRemark", SqlDbType.VarChar, (100));

                arr[0].Value = ItemFinishedId;
                arr[1].Value = ddlcompany.SelectedValue;
                arr[2].Value = VarTStockNo;
                arr[3].Value = VarTStockNo1;
                arr[4].Value = txtprefix.Text;
                arr[5].Value = txtdate.Text;
                arr[6].Value = txtstockid.Text;
                arr[7].Value = Session["varuserid"].ToString();
                arr[8].Value = ddlunit.SelectedValue;
                arr[9].Value = txtopeningstock.Text == "" ? "0" : txtopeningstock.Text;
                arr[10].Direction = ParameterDirection.InputOutput;
                arr[10].Value = Txtpostfix.Text == "" ? "0" : Txtpostfix.Text;
                arr[11].Value = txtDirectStockRemark.Text;


                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_directstock]", arr);
                Txtpostfix.Text = arr[10].Value.ToString();
                //for (int i = 1; i <= VarOpeningQty; i++)
                //{
                //    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "[PRO_directstock]", arr);
                //    Txtpostfix.Text = Convert.ToString(VarTStockNo1 + 1);
                //    VarTStockNo = txtprefix.Text + Txtpostfix.Text;
                //    VarTStockNo1 = Convert.ToInt32(Txtpostfix.Text);
                //    arr[2].Value = VarTStockNo;
                //    arr[3].Value = VarTStockNo1;
                //}
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Visible = true;
                lblmessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void ddlcatagorytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddlcatagoryname, @"SELECT  Distinct dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
        dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID 
        WHERE dbo.CategorySeparate.id =" + ddlcatagorytype.SelectedValue + " And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Catagory");
        if (ddlcatagorytype.SelectedValue == "0")
        {
            TDGodownId.Visible = false;
            Txtpostfix.Visible = true;
            txtprefix.Visible = true;
            txtprefix.Text = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            string postfix = (SqlHelper.ExecuteScalar(con, CommandType.Text, "sELECT IsNull(Max(Postfix),0)+1 from CarpetNumber Where prefix Like '%'")).ToString();
            Txtpostfix.Text = postfix;
            TRcurrentstock.Visible = false;
            if (variable.Carpetcompany == "1")
            {
                TbFinished.Visible = true;
                TDgvcarpetdetail.Visible = false;

                switch (Session["varMasterCompanyIDForERP"].ToString())
                {
                    case "21":
                        TDDirectStockRemark.Visible = true;
                        break;
                    case "36":
                        TDDirectStockRemark.Visible = true;
                        break;
                    case "42":
                        TRFinishedStockReport.Visible = true;
                        break;
                }
            }

        }
        else
        {
            TDGodownId.Visible = true;
            TRFinishedStockReport.Visible = false;
            TDDirectStockRemark.Visible = false;
            Txtpostfix.Visible = false;
            txtprefix.Visible = false;
            if (ddlcatagoryname.Items.Count > 0)
            {
                ddlcatagoryname.SelectedIndex = 1;
                ddlcatagoryname_SelectedIndexChanged(sender, e);
            }
            //***********
            if (Session["Usertype"].ToString() == "1")
            {
                TRcurrentstock.Visible = true;
            }
            if (variable.Carpetcompany == "1")
            {
                TbFinished.Visible = false;
                TDgvcarpetdetail.Visible = true;
            }

            //***********
            //Bin NO
            if (variable.VarBINNOWISE == "1")
            {
                TDBinnowise.Visible = true;
            }
        }

    }
    protected void txtprefix_TextChanged(object sender, EventArgs e)
    {
        Txtpostfix.Text = UtilityModule.CalculatePostFix(txtprefix.Text).ToString();
    }
    private void fill_grid()
    {
        gvcarpetdetail.DataSource = fill_Data_grid();
        gvcarpetdetail.DataBind();
    }
    private DataSet fill_Data_grid()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            int category = Convert.ToInt32(ddlcatagoryname.SelectedIndex);
            int item = ddlitemname.SelectedIndex > 0 ? Convert.ToInt32(ddlitemname.SelectedValue) : 0;
            int quality = dquality.SelectedIndex > 0 ? Convert.ToInt32(dquality.SelectedValue) : 0;
            int design = dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0;
            int color = ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0;
            int shape = ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0;
            int size = ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0;
            int ShadeColor = ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0;
            int godownId = ddlgodown.SelectedIndex > 0 ? Convert.ToInt32(ddlgodown.SelectedValue) : 0;
            int Unitid = ddlunit.SelectedIndex > 0 ? Convert.ToInt16(ddlunit.SelectedValue) : 0;

            string str1 = "", str2 = "", str3 = "", str4 = "", str5 = "", str6 = "", str7 = "", str8 = "", str9 = "", str10 = "";
            if (item > 0)
            {
                str1 = "and VF.Item_Id=" + item;
            }
            else
            {
                str1 = "";
            }
            if (quality > 0)
            {
                str2 = " and VF.QualityId=" + quality;
            }
            else
            {
                str2 = "";
            }
            if (design > 0)
            {
                str3 = " and VF.DesignId=" + design;
            }
            else
            {
                str3 = "";
            }
            if (color > 0)
            {
                str4 = " and VF.ColorId=" + color;
            }
            else
            {
                str4 = "";
            }
            if (shape > 0)
            {
                str5 = " and VF.ShapeId=" + shape;
            }
            else
            {
                str5 = "";
            }
            if (size > 0)
            {
                str6 = "and VF.SizeId=" + size;
            }
            else
            {
                str6 = "";
            }
            if (ShadeColor > 0)
            {
                str7 = "and VF.ShadecolorId=" + ShadeColor;
            }
            else
            {
                str7 = "";
            }
            if (godownId > 0)
            {
                str8 = "and S.godownId=" + godownId;
            }
            else
            {
                str8 = "";
            }
            if (Unitid > 0)
            {
                str9 = "and St.unitid=" + Unitid;
            }
            else
            {
                str9 = "";
            }
            if (txtlotno.Text != "")
            {
                str10 = "and S.LotNo='" + txtlotno.Text + "'";
            }
            else
            {
                str10 = "";
            }

            string strsql1;
            string view = "V_FinishedItemDetail";
            if (variable.VarNewQualitySize == "1")
            {
                view = "V_FinishedItemDetailNew";
            }

            //////            strsql1 = @"select S.StockId,CATEGORY_NAME,ITEM_NAME,QualityName+'  '+DesignName+'  '+ColorName+'  '+ShapeName+'  '+isnull(SizeMtr,'')+'  '+ShadeColorName DESCRIPTION,
            //////                            LotNo,gm.GodownName,isnull(U.UnitName,'') as Unitname,isnull(U.unitid,0) as Unitid,
            //////                            Sum(case When St.TableName='Opening stock' Then St.quantity Else 0 End) as OpenStock,
            //////                            Round(sum(case when St.trantype=1 Then St.quantity else -St.quantity End),3) as Qtyinhand,S.TagNo,S.noofcone,S.BinNo from stock S inner join stocktran St on S.stockid=St.stockid
            //////                            inner join " + view + @" vf on s.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID
            //////                            inner join GodownMaster gm on gm.GoDownID=s.Godownid
            //////                            left join unit U on U.UnitId=St.Unitid
            //////                            Where S.companyid=" + ddlcompany.SelectedValue + " and  TypeId=" + ddlcatagorytype.SelectedValue + " " + str1 + " " + str2 + " " + str3 + " " + str4 + " " + str5 + " " + str6 + " " + str7 + " " + str8 + " " + str10 + @"
            //////                            group by S.StockId,CATEGORY_NAME,ITEM_NAME,QualityName,DesignName,ColorName,ShapeName,SizeMtr,ShadeColorName ,
            //////                            LotNo,gm.GodownName,U.UnitName,U.unitid,S.TagNo,S.noofcone,S.BinNo order by DESCRIPTION";

            strsql1 = @"select S.StockId,CATEGORY_NAME,ITEM_NAME,QualityName+'  '+DesignName+'  '+ColorName+'  '+ShapeName+'  '+isnull(SizeMtr,'')+'  '+ShadeColorName DESCRIPTION,
                            LotNo,gm.GodownName,isnull(U.UnitName,'') as Unitname,isnull(U.unitid,0) as Unitid,                            
                            sum(case when " + Session["varcompanyNo"] + @"=22 then ((case When St.TableName='Opening stock' Then St.quantity Else 0 End)+(case When St.trantype=1 and St.TableName='pp_processrectran' Then St.quantity Else 0 End)) 
                            else case When St.TableName='Opening stock' Then St.quantity Else 0 End end) as OpenStock,                           
                            Round(sum(case when St.trantype=1 Then St.quantity else -St.quantity End),3) as Qtyinhand,S.TagNo,S.noofcone,S.BinNo from stock S inner join stocktran St on S.stockid=St.stockid
                            inner join " + view + @" vf on s.ITEM_FINISHED_ID=vf.ITEM_FINISHED_ID
                            inner join GodownMaster gm on gm.GoDownID=s.Godownid
                            left join unit U on U.UnitId=St.Unitid
                            Where S.companyid=" + ddlcompany.SelectedValue + " and  TypeId=" + ddlcatagorytype.SelectedValue + " " + str1 + " " + str2 + " " + str3 + " " + str4 + " " + str5 + " " + str6 + " " + str7 + " " + str8 + " " + str10 + @"
                            group by S.StockId,CATEGORY_NAME,ITEM_NAME,QualityName,DesignName,ColorName,ShapeName,SizeMtr,ShadeColorName ,
                            LotNo,gm.GodownName,U.UnitName,U.unitid,S.TagNo,S.noofcone,S.BinNo order by DESCRIPTION";

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql1);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
            Logs.WriteErrorLog("Masters_process_DerictStock|Fill_Grid_Data|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void gvcarpetdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvcarpetdetail, "Select$" + e.Row.RowIndex);
        }
        //**************
        e.Row.Cells[GetGridColumnId("TagNo")].Visible = false;
        e.Row.Cells[GetGridColumnId("Noofcone")].Visible = false;
        e.Row.Cells[GetGridColumnId("BinNo")].Visible = false;
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "8":
                e.Row.Cells[GetGridColumnId("Opening Stock")].Visible = false;
                break;
            case "10":
                e.Row.Cells[GetGridColumnId("Lotno")].Visible = false;
                break;
            case "14":
                e.Row.Cells[GetGridColumnId("TagNo")].Visible = true;
                e.Row.Cells[GetGridColumnId("Noofcone")].Visible = true;
                break;
        }
        if (TDTagNo.Visible == true)
        {
            e.Row.Cells[GetGridColumnId("TagNo")].Visible = true;
        }
        if (TDBinnowise.Visible == true)
        {
            e.Row.Cells[GetGridColumnId("BinNo")].Visible = true;
        }
        //**************
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlunit.SelectedValue == "1")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by MtrSize", true, "select size");
            }
            else
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr+'  '+'['+ProdSizemtr+']' from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizemtr", true, "select size");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizemtr", true, "select size");
                }
                
            }
        }
        else if (ddlunit.SelectedValue == "2")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by Export_Format", true, "select size");
            }
            else
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft+'  '+'['+Prodsizeft+']' from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeft", true, "select size");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeft", true, "select size");
                }
                
            }
        }
        // fill_grid();
    }
    protected void ddlitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        itemchanged();
        TxtFinishedid.Text = "Category=" + ddlcatagoryname.SelectedValue + "&Item=" + ddlitemname.SelectedValue;
    }
    private void itemchanged()
    {
        if (ddlcatagorytype.SelectedValue == "0")
        {
            if (ddlitemname.SelectedIndex > 0)
            {
                if (variable.VarCarpetPrefixauto == "1")
                {
                    txtprefix.Text = SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Item_Code from Item_Master Where Item_Id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"]).ToString();
                    string get_year = DateTime.Now.ToString("dd-MMM-yyyy");
                    string lastTwoChars = get_year.Substring(get_year.Length - 2);
                    if (txtprefix.Text != "")
                    {
                        txtprefix.Text = (txtprefix.Text + "-" + lastTwoChars).Replace(" ", "");
                    }
                    else
                    {
                        txtprefix.Text = "";
                    }
                }

            }
            else
            {
                txtprefix.Text = "";
            }
            Txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((txtprefix.Text).ToUpper()));
        }
        string str = @"SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        str = str + @" select qualityid,qualityname from quality where item_id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by qualityname";
        DataSet ds = SqlHelper.ExecuteDataset(str);
        UtilityModule.ConditionalComboFillWithDS(ref ddlunit, ds, 0, true, "Select Unit");
        UtilityModule.ConditionalComboFillWithDS(ref dquality, ds, 1, true, "Select Quallity");
        if (ddlunit.Items.Count > 0)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "21")
            {
                ddlunit.SelectedIndex = 1;
            }
            else
            {
                if (ddlunit.Items.FindByValue("2") != null)
                {
                    ddlunit.SelectedValue = "2";
                }
                else
                {
                    ddlunit.SelectedIndex = 1;
                }
            }

        }
        //  fill_grid();
    }
    private void saverawdetailcarpet()
    {
        int varfinishtype_Id = 0;
        if (txtlotno.Text == "")
        {
            txtlotno.Text = "Without Lot No";
        }
        if (txttagno.Text == "")
        {
            txttagno.Text = "Without Tag No";
        }
        if (TdFINISHED_TYPE.Visible == false)
        {
            varfinishtype_Id = 0;
        }
        else
        {
            varfinishtype_Id = Convert.ToInt32(ddFINISHED_TYPE.SelectedValue);
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "and lotno='" + txtlotno.Text + "' and TagNo='" + txttagno.Text + "' And Finished_Type_Id=" + varfinishtype_Id;
            //DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, strstock);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    //int openstock = Convert.ToInt32(ds.Tables[0].Rows[0]["openstock"].ToString());
            //    int stockid = Convert.ToInt32(ds.Tables[0].Rows[0]["StockID"]);
            //    string str = @"Select StockId,Quantity,isnull(coneuse,0) From StockTran where TableName='Opening stock' And StockID=" + stockid + " and Unitid=" + ddlunit.SelectedValue;
            //    DataSet ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            //    if (ds1.Tables[0].Rows.Count > 0)
            //    {
            //        Double Qty = Convert.ToDouble(ds1.Tables[0].Rows[0]["Quantity"]);
            //        Double coneuse = Convert.ToDouble(ds1.Tables[0].Rows[0]["Quantity"]);
            //        string str1 = @"Update Stock set QtyinHand=QtyinHand-(" + Qty + "),OpenStock=OpenStock-(" + Qty + "),Noofcone=noofcone-(" + coneuse + ") where stockId=" + stockid + "";
            //        str1 = str1 + @" Delete From Stocktran where StockId=" + stockid + " And TableName='Opening stock' and unitid=" + ddlunit.SelectedValue;
            //        SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);

            //    }
            //}
            UtilityModule.StockStockTranTableUpdateWithOpeningStock(ItemFinishedId, Convert.ToInt32(ddlgodown.SelectedValue), Convert.ToInt32(ddlcompany.SelectedValue), txtlotno.Text.ToString(), Convert.ToDouble(txtopeningstock.Text), txtdate.Text.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Opening stock", 0, Tran, 1, true, Convert.ToInt32(ddlcatagorytype.SelectedValue), varfinishtype_Id, Convert.ToDouble(TxtRate.Text), Unitid: Convert.ToInt16(ddlunit.SelectedValue), Noofcone: Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text), TagNo: txttagno.Text);
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        lblmessage.Visible = true;
    }
    private void saverawdetail2()
    {
        int varfinishtype_Id = 0;
        if (txtlotno.Text == "")
        {
            txtlotno.Text = "Without Lot No";
        }
        if (TdFINISHED_TYPE.Visible == false)
        {
            varfinishtype_Id = 0;
        }
        else
        {
            varfinishtype_Id = Convert.ToInt32(ddFINISHED_TYPE.SelectedValue);
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Open)
        {
            con.Open();
        }
        ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string strstock = "select * from stock where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue + "and lotno='" + txtlotno.Text + "'  And Finished_Type_Id=" + varfinishtype_Id;
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, strstock);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int stockid = Convert.ToInt32(ds.Tables[0].Rows[0]["StockID"]);
                string str = @"Select StockId,Quantity From StockTran where TableName='Opening stock' And StockID=" + stockid;
                DataSet ds1 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    Double Qty = Convert.ToDouble(ds1.Tables[0].Rows[0]["Quantity"]);
                    string str1 = @"Update Stock set QtyinHand=QtyinHand-" + Qty + ",OpenStock=OpenStock-" + Qty + " where stockId=" + stockid + "";
                    str1 = str1 + @" Delete From Stocktran where StockId=" + stockid + " And TableName='Opening stock' ";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                }
                string update_carpet = "delete from carpetnumber where item_finished_id=" + ItemFinishedId;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, update_carpet);
                string postfix = (SqlHelper.ExecuteScalar(Tran, CommandType.Text, "sELECT IsNull(Max(Postfix),0)+1 from CarpetNumber Where prefix Like '" + txtprefix.Text + "%'")).ToString();
                Txtpostfix.Text = postfix;
            }
            UtilityModule.StockStockTranTableUpdateWithOpeningStock(ItemFinishedId, Convert.ToInt32(ddlgodown.SelectedValue), Convert.ToInt32(ddlcompany.SelectedValue), txtlotno.Text.ToString(), Convert.ToDouble(txtopeningstock.Text), txtdate.Text.ToString(), DateTime.Now.ToString("dd-MMM-yyyy"), "Opening stock", 0, Tran, 1, true, Convert.ToInt32(ddlcatagorytype.SelectedValue), varfinishtype_Id, Convert.ToDouble(TxtRate.Text));
            Tran.Commit();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        lblmessage.Visible = true;
    }
    protected void gvcarpetdetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcatagorytype.SelectedValue == "1")
        {
            DataSet ds = null;
            try
            {
                ds = null;
                Label lblunitid = ((Label)gvcarpetdetail.Rows[gvcarpetdetail.SelectedIndex].FindControl("lblunitid"));

                //                string str = @"SELECT DISTINCT s.StockID, (Select quantity From stocktran stt(Nolock) Where stt.Stockid = s.StockID And stt.tablename = 'Opening stock') OpenStock, s.Qtyinhand, s.lotno, 
                //                (SELECT TStockNos FROM  Get_StockNoRec_carpet_Wise(s.ITEM_FINISHED_ID) Get_StockNoRec_carpet_Wise_1) StockNos, 
                //                IPM.QUALITY_ID, IPM.DESIGN_ID, IPM.COLOR_ID, IPM.shadecolor_id, IPM.SHAPE_ID, IPM.SIZE_ID, IM.ITEM_ID, IM.CATEGORY_ID, IsNull(st.stocktranid, 0) stocktranid, 
                //                s.Companyid, s.Godownid, s.TypeId, " + lblunitid.Text + @" UnitId, s.ITEM_FINISHED_ID, s.Price, IsNull(st.coneuse, 0) coneuse, s.TagNo, s.BinNo 
                //                FROM stock s(Nolock)
                //                Left Join stocktran st(Nolock) on st.stockid = s.stockid And st.tablename = 'Opening stock'
                //                JOIN ITEM_PARAMETER_MASTER IPM(Nolock) ON IPM.ITEM_FINISHED_ID = s.ITEM_FINISHED_ID 
                //                JOIN ITEM_MASTER IM(Nolock) ON IM.ITEM_ID = IPM.ITEM_ID 
                //                Where s.StockID=" + gvcarpetdetail.SelectedValue + " And s.Companyid = " + ddlcompany.SelectedValue + @" And 
                //                IM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];

                string str = @"SELECT DISTINCT s.StockID, (Select quantity From stocktran stt(Nolock) Where stt.Stockid = s.StockID And stt.tablename = 'Opening stock') OpenStock, s.Qtyinhand, s.lotno, 
                '' StockNos, 
                IPM.QUALITY_ID, IPM.DESIGN_ID, IPM.COLOR_ID, IPM.shadecolor_id, IPM.SHAPE_ID, IPM.SIZE_ID, IM.ITEM_ID, IM.CATEGORY_ID, IsNull(st.stocktranid, 0) stocktranid, 
                s.Companyid, s.Godownid, s.TypeId, " + lblunitid.Text + @" UnitId, s.ITEM_FINISHED_ID, s.Price, IsNull(st.coneuse, 0) coneuse, s.TagNo, s.BinNo 
                FROM stock s(Nolock)
                Left Join stocktran st(Nolock) on st.stockid = s.stockid And st.tablename = 'Opening stock'
                JOIN ITEM_PARAMETER_MASTER IPM(Nolock) ON IPM.ITEM_FINISHED_ID = s.ITEM_FINISHED_ID 
                JOIN ITEM_MASTER IM(Nolock) ON IM.ITEM_ID = IPM.ITEM_ID 
                Where s.StockID=" + gvcarpetdetail.SelectedValue + " And s.Companyid = " + ddlcompany.SelectedValue + @" And 
                IM.MasterCompanyId = " + Session["varMasterCompanyIDForERP"];

                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int stockid = Convert.ToInt32(ds.Tables[0].Rows[0]["stockid"]);
                    hnstockid.Value = stockid.ToString();
                    txtstockid.Text = ds.Tables[0].Rows[0]["stockid"].ToString();
                    ddlcatagorytype.SelectedValue = ds.Tables[0].Rows[0]["typeid"].ToString();
                    UtilityModule.ConditionalComboFill(ref ddlcatagoryname, @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate(Nolock) JOIN 
                    dbo.ITEM_CATEGORY_MASTER(Nolock) ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID 
                    WHERE dbo.CategorySeparate.id =" + ddlcatagorytype.SelectedValue + " And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "Select Catagory");

                    ddlcatagoryname.SelectedValue = ds.Tables[0].Rows[0]["category_id"].ToString();

                    UtilityModule.ConditionalComboFill(ref ddlitemname, "Select Distinct Item_Id,Item_Name from Item_Master(Nolock) where Category_Id=" + ddlcatagoryname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Item_Name", true, "--select Item--");

                    ddlitemname.SelectedValue = ds.Tables[0].Rows[0]["item_id"].ToString();

                    string Qry = @"SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i(Nolock) JOIN Unit u(Nolock) ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    Qry = Qry + @" select qualityid,qualityname from quality(Nolock) where item_id=" + ddlitemname.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by qualityname";
                    Qry = Qry + @" select distinct Designid,DesignName from Design(Nolock) Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName  ";
                    Qry = Qry + "  SELECT ColorId,ColorName FROM Color(Nolock) Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ColorName";
                    Qry = Qry + " select Shapeid,ShapeName from Shape(Nolock) Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Shapeid ";
                    Qry = Qry + " select shadecolorid,shadecolorname from shadecolor(Nolock) Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by shadecolorname";
                    DataSet DSQ = SqlHelper.ExecuteDataset(Qry);
                    ddlunit.SelectedValue = ds.Tables[0].Rows[0]["unitid"].ToString();

                    UtilityModule.ConditionalComboFillWithDS(ref ddlunit, DSQ, 0, true, "Select Unit");
                    if (ql.Visible == true)
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 1, true, "Select Quallity");
                        dquality.SelectedValue = ds.Tables[0].Rows[0]["quality_id"].ToString();
                    }

                    if (dsn.Visible == true)
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 2, true, "--Select Design--");
                        dddesign.SelectedValue = ds.Tables[0].Rows[0]["design_id"].ToString();
                    }
                    if (clr.Visible == true)
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 3, true, "--Select Color--");
                        ddcolor.SelectedValue = ds.Tables[0].Rows[0]["color_id"].ToString();
                    }
                    if (shp.Visible == true)
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 4, true, "--Select Shape--");
                        ddshape.SelectedValue = ds.Tables[0].Rows[0]["shape_id"].ToString();
                    }
                    if (shd.Visible == true)
                    {
                        UtilityModule.ConditionalComboFillWithDS(ref ddlshade, DSQ, 5, true, "Select Shadecolor");
                    }
                    ddlshade.SelectedValue = ds.Tables[0].Rows[0]["shadecolor_id"].ToString();
                    txtlotno.Text = ds.Tables[0].Rows[0]["lotno"].ToString();
                    int finishedid = Convert.ToInt32(ds.Tables[0].Rows[0]["item_finished_id"].ToString());
                    if (ddlunit.SelectedValue == "1")
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " ", true, "select size");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "select size");
                        }
                    }
                    else if (ddlunit.SelectedValue == "2")
                    {
                        if (variable.VarNewQualitySize == "1")
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " ", true, "select size");
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "select size");
                        }
                    }
                    ddsize.SelectedValue = ds.Tables[0].Rows[0]["size_id"].ToString();
                    ddlgodown.SelectedValue = ds.Tables[0].Rows[0]["godownid"].ToString();
                    ddlgodown_SelectedIndexChanged(sender, new EventArgs());
                    txtopeningstock.Text = ds.Tables[0].Rows[0]["openstock"].ToString();
                    txtnoofcone.Text = ds.Tables[0].Rows[0]["coneuse"].ToString();
                    if (DDBinNo.Items.FindByText(ds.Tables[0].Rows[0]["BinNo"].ToString()) != null)
                    {
                        DDBinNo.SelectedValue = ds.Tables[0].Rows[0]["BinNo"].ToString();
                    }
                    TxtRate.Text = ds.Tables[0].Rows[0]["Price"].ToString();
                    txttagno.Text = ds.Tables[0].Rows[0]["TagNo"].ToString();
                    txtcurrentstock.Text = ds.Tables[0].Rows[0]["qtyinhand"].ToString();
                    hnqtyinhand.Value = ds.Tables[0].Rows[0]["qtyinhand"].ToString();
                    btnupdatestock.Enabled = true;

                    if (Convert.ToInt32(dquality.SelectedValue) > 0)
                    {
                        ql.Visible = true;
                    }
                    else
                    {
                        ql.Visible = false;
                    }
                    int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt16(dddesign.SelectedValue) : 0);
                    if (d > 0)
                    {
                        dsn.Visible = true;
                    }
                    else
                    {
                        dsn.Visible = false;
                    }
                    int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                    if (c > 0)
                    {
                        clr.Visible = true;
                    }
                    else
                    {
                        clr.Visible = false;
                    }
                    int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                    if (s > 0)
                    {
                        shp.Visible = true;
                    }
                    else
                    {
                        shp.Visible = false;
                    }
                    int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                    if (si > 0)
                    {
                        sz.Visible = true;
                    }
                    else
                    {
                        sz.Visible = false;
                    }
                    int sc = (ddlshade.SelectedIndex > 0 ? Convert.ToInt32(ddlshade.SelectedValue) : 0);
                    if (sc > 0)
                    {
                        shd.Visible = true;
                    }
                    else
                    {
                        shd.Visible = false;
                    }
                }
                else
                {
                    hnstockid.Value = "0";
                    txtcurrentstock.Text = "";
                    hnqtyinhand.Value = "0";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "stock", "alert('No opening stock is available to Update..')", true);
                    btnupdatestock.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");
            }

            btnsave.Text = "Update";
            txtprefix.Text = "";
            Txtpostfix.Text = "";
            //pnl1.Enabled = false;
            //Panel2.Enabled = false;
            ddlgodown.Enabled = false;
            txtopeningstock.Focus();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
        //string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER  where ProductCode Like  '" + prefixText + "%'";
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
    protected void TxtProdCode_TextChanged(object sender, EventArgs e)
    {
        DataSet ds1;
        string Str;
        if (TxtProdCode.Text != "")
        {
            ddlcatagoryname.SelectedIndex = -1;
            Str = @"select IPM.*,IM.CATEGORY_ID,cs.id from ITEM_PARAMETER_MASTER IPM,ITEM_MASTER IM,CategorySeparate cs where IPM.ITEM_ID=IM.ITEM_ID and im.CATEGORY_ID=cs.Categoryid
                  and ProductCode='" + TxtProdCode.Text + "' And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string Qry;
                if (variable.VarNewQualitySize == "1")
                {
                    Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                    dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " Order by Item_Name";
                    Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " order by qualityname";
                    Qry = Qry + @" select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName";
                    Qry = Qry + @" SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    Qry = Qry + @" select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName";
                    Qry = Qry + @"  SELECT SIZEID,Export_Format fROM QualitySizeNew WhERE SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SIZE_ID"].ToString());
                }
                else
                {
                    Qry = @"SELECT dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID, dbo.ITEM_CATEGORY_MASTER.CATEGORY_NAME  FROM  dbo.CategorySeparate INNER JOIN
                    dbo.ITEM_CATEGORY_MASTER ON dbo.CategorySeparate.Categoryid = dbo.ITEM_CATEGORY_MASTER.CATEGORY_ID And ITEM_CATEGORY_MASTER.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    Qry = Qry + "  Select Distinct Item_Id,Item_Name from Item_Master where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And  Category_Id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString()) + " Order by Item_Name";
                    Qry = Qry + "  select qualityid,qualityname from quality where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And item_id=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["ITEM_ID"].ToString()) + " order by qualityname";
                    Qry = Qry + @" select distinct Designid,DesignName from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order  by DesignName";
                    Qry = Qry + @" SELECT ColorId,ColorName FROM Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    Qry = Qry + @" select Shapeid,ShapeName from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by ShapeName";
                    Qry = Qry + @"  SELECT SIZEID,SIZEFT fROM SIZE WhERE MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And SHAPEID=" + Convert.ToInt32(ds1.Tables[0].Rows[0]["SIZE_ID"].ToString());
                }
                DataSet DSQ = SqlHelper.ExecuteDataset(Qry);

                UtilityModule.ConditionalComboFillWithDS(ref ddlcatagoryname, DSQ, 0, true, "Select Catagory");
                ddlcatagoryname.SelectedValue = ds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddlitemname, DSQ, 1, true, "--Select Item--");
                ddlitemname.SelectedValue = ds1.Tables[0].Rows[0]["ITEM_ID"].ToString();

                UtilityModule.ConditionalComboFillWithDS(ref dquality, DSQ, 2, true, "Select Quallity");
                dquality.SelectedValue = ds1.Tables[0].Rows[0]["QUALITY_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref dddesign, DSQ, 3, true, "Select Design");
                dddesign.SelectedValue = ds1.Tables[0].Rows[0]["DESIGN_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddcolor, DSQ, 4, true, "--Select Color--");
                ddcolor.SelectedValue = ds1.Tables[0].Rows[0]["COLOR_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddshape, DSQ, 5, true, "--Select Shape--");
                ddshape.SelectedValue = ds1.Tables[0].Rows[0]["SHAPE_ID"].ToString();
                UtilityModule.ConditionalComboFillWithDS(ref ddsize, DSQ, 6, true, "--SELECT SIZE--");
                ddsize.SelectedValue = ds1.Tables[0].Rows[0]["SIZE_ID"].ToString();

                Session["finishedid"] = ds1.Tables[0].Rows[0]["Item_Finished_id"].ToString();
                if (Convert.ToInt32(dquality.SelectedValue) > 0)
                {
                    ql.Visible = true;
                }
                else
                {
                    ql.Visible = false;
                }
                int d = (dddesign.SelectedIndex > 0 ? Convert.ToInt32(dddesign.SelectedValue) : 0);
                if (d > 0)
                {
                    dsn.Visible = true;
                }
                else
                {
                    dsn.Visible = false;
                }
                int c = (ddcolor.SelectedIndex > 0 ? Convert.ToInt32(ddcolor.SelectedValue) : 0);
                if (c > 0)
                {
                    clr.Visible = true;
                }
                else
                {
                    clr.Visible = false;
                }
                int s = (ddshape.SelectedIndex > 0 ? Convert.ToInt32(ddshape.SelectedValue) : 0);
                if (s > 0)
                {
                    shp.Visible = true;
                }
                else
                {
                    shp.Visible = false;
                }
                int si = (ddsize.SelectedIndex > 0 ? Convert.ToInt32(ddsize.SelectedValue) : 0);
                if (si > 0)
                {
                    sz.Visible = true;
                }
                else
                {
                    sz.Visible = false;
                }
                UtilityModule.ConditionalComboFill(ref ddlunit, "SELECT u.UnitId,u.UnitName  FROM ITEM_MASTER i INNER JOIN  Unit u ON i.UnitTypeID = u.UnitTypeID where item_id=" + ddlitemname.SelectedValue + " And i.MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "Select Unit");
                Label2.Visible = false;
            }
            else
            {
                Label2.Visible = true;
                TxtProdCode.Text = "";
                TxtProdCode.Focus();
            }
        }
        else
        {
            ddlcatagoryname.SelectedIndex = 0;
        }
    }
    private void raw_stock_update2()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
            string strupdate = "update stock set Qtyinhand=" + remainstock + "where ITEM_FINISHED_ID=" + ItemFinishedId + "and companyid=" + ddlcompany.SelectedValue + "and Godownid=" + ddlgodown.SelectedValue;
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, strupdate);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/RawMaterial/DirectStock.aspx");

        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../../main.aspx");
    }
    protected void dquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "30")
        {
            if (ddlcatagorytype.SelectedValue == "0")
            {
                if (ddlitemname.SelectedIndex > 0)
                {
                    string str = @"select distinct D.DesignId,D.DesignName from Design D JOIN V_FinishedItemDetail Vf ON D.designId=Vf.designId Where VF.CATEGORY_ID=" + ddlcatagoryname.SelectedValue + " and VF.ITEM_ID=" + ddlitemname.SelectedValue + " and Vf.QualityId=" + dquality.SelectedValue + " Order  by DesignName";
                    DataSet ds = SqlHelper.ExecuteDataset(str);
                    UtilityModule.ConditionalComboFillWithDS(ref dddesign, ds, 0, true, "Select Design");
                }
            }
        }
        // fill_grid();
    }
    protected void dddesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddcolor_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        // fill_grid();
    }
    private void CHECKVALIDCONTROL()
    {
        lblMessageVal.Text = "";
        lblMessageVal.Visible = true;
        if (UtilityModule.VALIDDROPDOWNLIST(ddlcompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlcatagorytype) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlcatagoryname) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlitemname) == false)
        {
            goto a;
        }
        if (ql.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(dquality) == false)
            {
                goto a;
            }
        }
        if (clr.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddcolor) == false)
            {
                goto a;
            }
        }
        if (shp.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddshape) == false)
            {
                goto a;
            }
        }
        if (sz.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddsize) == false)
            {
                goto a;
            }
        }
        if (shd.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlshade) == false)
            {
                goto a;
            }
        }
        if (TDGodownId.Visible == true)
        {
            if (UtilityModule.VALIDDROPDOWNLIST(ddlgodown) == false)
            {
                goto a;
            }
        }
        if (UtilityModule.VALIDDROPDOWNLIST(ddlunit) == false)
        {
            goto a;
        }
        //if (UtilityModule.VALIDTEXTBOX(txtRate) == false)
        //{
        //    goto a;
        //
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(lblMessageVal);
    B: ;
    }
    protected void Refresh()
    {
        lblsave.Visible = false;
        txtopeningstock.Text = "";
        TxtProdCode.Text = "";
        txtlotno.Text = "";
        txttagno.Text = "";
        lblerror.Visible = false;
        txtprefix.Visible = false;
        Txtpostfix.Visible = false;
        btnsave.Text = "Save";
        ddlgodown.Enabled = true;

        TxtRate.Text = "0";
        Txtminstock.Text = "0";

        switch (Session["varMasterCompanyIDForERP"].ToString())
        {
            case "30":
            case "27":
                break;
            default:
                ddlunit.SelectedValue = null;
                break;
        }

        if (ql.Visible == true)
        {
            //if (Session["varMasterCompanyIDForERP"].ToString() != "27") 
            //{
            //    dquality.SelectedValue = null;
            //}

            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "30":
                case "27":
                    break;
                default:
                    dquality.SelectedValue = null;
                    break;
            }

        }
        if (clr.Visible == true)
        {
            ddcolor.SelectedValue = null;
        }
        if (shp.Visible == true)
        {
            ddshape.SelectedValue = null;
        }
        if (sz.Visible == true)
        {
            ddsize.SelectedValue = null;
        }
        if (shd.Visible == true)
        {
            ddlshade.SelectedValue = null;
        }
        txtnoofcone.Text = "";
        hnstockid.Value = "0";
        hnqtyinhand.Value = "0";
        btnupdatestock.Enabled = false;
        txtcurrentstock.Text = "";
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        if (ChkForExcelExport.Checked == true)
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                ExportExcelReport();
            }
        }
        else
        {
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                Session["ReportPath"] = "reports/OpenStockHafizia.rpt";
            }
            else
            {
                Session["ReportPath"] = "reports/openstock.rpt";
            }
            string qry = "";
            qry = @" SELECT CATEGORY_NAME,ITEM_NAME,DESCRIPTION,OpenStock,Qtyinhand,LotNo,GodownName FROM  openstock
                 Where OpenStock.CompanyID=" + ddlcompany.SelectedValue + " And CompanyId=" + ddlcompany.SelectedValue + " And (Round(OpenStock,3)<>0 or Round(QtyinHand,3)<>0)";
            //Session["CommanFormula"] = "{OpenStock.CompanyID}=" + ddlcompany.SelectedValue + "";
            if (ddlcatagorytype.SelectedIndex > 0)
            {
                qry = qry + "and OpenStock.TypeID=" + ddlcatagorytype.SelectedValue + "";
                // Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.TypeID}=" + ddlcatagorytype.SelectedValue + "";
            }
            if (ddlcatagoryname.SelectedIndex > 0)
            {
                qry = qry + "and OpenStock.Category_id=" + ddlcatagoryname.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Category_id}=" + ddlcatagoryname.SelectedValue + "";
            }
            if (ddlitemname.SelectedIndex > 0)
            {
                qry = qry + " and OpenStock.Item_id=" + ddlitemname.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Item_id}=" + ddlitemname.SelectedValue + "";
            }
            if (dquality.SelectedIndex > 0 && ql.Visible == true)
            {
                qry = qry + "and OpenStock.Qualityid=" + dquality.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Qualityid}=" + dquality.SelectedValue + "";
            }
            if (dddesign.SelectedIndex > 0 && dsn.Visible == true)
            {
                qry = qry + " and OpenStock.DesignID=" + dddesign.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.DesignID}=" + dddesign.SelectedValue + "";
            }
            if (ddcolor.SelectedIndex > 0 && clr.Visible == true)
            {
                qry = qry + " and OpenStock.Colorid=" + ddcolor.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Colorid}=" + ddcolor.SelectedValue + "";
            }
            if (ddshape.SelectedIndex > 0 && shp.Visible == true)
            {
                qry = qry + " and OpenStock.Shapeid=" + ddshape.SelectedValue + " ";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Shapeid}=" + ddshape.SelectedValue + "";
            }
            if (ddsize.SelectedIndex > 0 && sz.Visible == true)
            {
                qry = qry + " and OpenStock.Sizeid=" + ddsize.SelectedValue + "";
                //Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.Sizeid}=" + ddsize.SelectedValue + "";
            }
            if (ddlshade.SelectedIndex > 0 && shd.Visible == true)
            {
                qry = qry + " And  OpenStock.ShadeColorid=" + ddlshade.SelectedValue + "";
                Session["CommanFormula"] = Session["CommanFormula"] + " And {OpenStock.ShadeColorid}=" + ddlshade.SelectedValue + "";
            }
            if (ddlgodown.SelectedIndex > 0)
            {
                qry = qry + " And OpenStock.GodownId=" + ddlgodown.SelectedValue + "";
            }
            qry = qry + " ORDER BY CATEGORY_NAME,DESCRIPTION";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
            //Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\openstock.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
    }
    //protected void gvcarpetdetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void refreshcategory_Click(object sender, EventArgs e)
    {
        ddlcategorycange();
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    protected void BtnRefreshItem_Click(object sender, EventArgs e)
    {
        itemchanged();
    }
    protected void refreshquality_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void refreshdesign_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        //  fill_grid();
    }
    protected void refreshshape_Click(object sender, EventArgs e)
    {
        //  fill_grid();
    }
    protected void BtnRefreshSize_Click(object sender, EventArgs e)
    {
        //fill_grid();
    }
    protected void refreshshade_Click(object sender, EventArgs e)
    {
        // fill_grid();
    }
    protected void ChkMtr_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkMtr.Checked == true)
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue, true, "select size");
            }
            else
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr+'  '+'['+ProdSizemtr+']' from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizemtr from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
                }                
            }
        }
        else
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,Export_Format from QualitySizeNew where Shapeid=" + ddshape.SelectedValue + " order by Export_Format", true, "select size");
            }
            else
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "43")
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft+'  '+'['+Prodsizeft+']' from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeft", true, "select size");
                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeft from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by sizeft", true, "select size");
                }                
            }
        }

    }
    protected void btnshowDetail_Click(object sender, EventArgs e)
    {
        //System.Threading.Thread.Sleep(3000);
        lblloading.Visible = true;
        if (ddlcatagorytype.SelectedValue == "0")
        {
            FillGridFinisheditem();
        }
        else
        {
            fill_grid();
        }

        lblloading.Visible = false;
    }
    protected void FillGridFinisheditem()
    {
        string view = "V_FinishedItemDetail";
        if (variable.VarNewQualitySize == "1")
        {
            view = "V_FinishedItemDetailNew";
        }
        string str = string.Empty;
        if (Session["varMasterCompanyIDForERP"].ToString() == "44")
        {
            str = @"select vf.item_name,Vf.QualityName,Vf.designname,vf.ColorName,vf.ShapeName,
                case when CN.StockUnitid=1 then vf.SizeMtr when CN.StockUnitid=6 then vf.SizeInch else vf.SizeFt end as SizeFt,CN.TStockNo,CN.StockNo, IsNull(CN.Price, 0) Price 
                From  CarpetNumber CN Inner Join " + view + @" Vf on CN.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                Where CN.TypeId=0 and CN.Pack=0 ";
        }
        else
        {
            str = @"select vf.item_name,Vf.QualityName,Vf.designname,vf.ColorName,vf.ShapeName,
                case when CN.StockUnitid=1 then vf.SizeMtr  else vf.SizeFt end as SizeFt,CN.TStockNo,CN.StockNo, IsNull(CN.Price, 0) Price 
                From  CarpetNumber CN Inner Join " + view + @" Vf on CN.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                Where CN.TypeId=0 and CN.Pack=0 ";



        }
        if (ddlitemname.SelectedIndex > 0)
        {
            str = str + " and Vf.Item_id=" + ddlitemname.SelectedValue;
        }
        if (dquality.SelectedIndex > 0)
        {
            str = str + " and Vf.QualityId=" + dquality.SelectedValue;
        }
        if (dddesign.SelectedIndex > 0)
        {
            str = str + " and Vf.DesignId=" + dddesign.SelectedValue;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            str = str + " and Vf.Colorid=" + ddcolor.SelectedValue;
        }
        if (ddshape.SelectedIndex > 0)
        {
            str = str + "  and Vf.Shapeid=" + ddshape.SelectedValue;
        }
        if (ddsize.SelectedIndex > 0)
        {
            str = str + " and Vf.Sizeid=" + ddsize.SelectedValue;
        }
        str = str + "  order by CN.stockNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GDFinishedDetail.DataSource = ds.Tables[0];
        GDFinishedDetail.DataBind();

    }
    protected int GetGridColumnId(string ColName)
    {
        int columnid = -1;
        foreach (DataControlField col in gvcarpetdetail.Columns)
        {
            if (col.HeaderText.ToUpper().Trim() == ColName.ToUpper())
            {
                columnid = gvcarpetdetail.Columns.IndexOf(col);
                break;
            }
        }
        return columnid;
    }
    protected void btnupdatestock_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        //*******Update current stock
        if ((hnstockid.Value != "0" || hnstockid.Value != string.Empty) && string.Compare(btnsave.Text, "Update", true) == 0)
        {
            ItemFinishedId = UtilityModule.getItemFinishedId(ddlitemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, ddlshade, 0, "", Convert.ToInt32(Session["varMasterCompanyIDForERP"]));

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                if (Convert.ToDecimal(txtcurrentstock.Text == "" ? "0" : txtcurrentstock.Text) != Convert.ToDecimal(txtopeningstock.Text == "" ? "0" : txtopeningstock.Text))
                {
                    //Check Bin Validation
                    if (variable.VarCHECKBINCONDITION == "1" && (Convert.ToDecimal(txtcurrentstock.Text) > Convert.ToDecimal(hnqtyinhand.Value)))
                    {
                        string BinNO = "", Lotno = "", TagNo = "";
                        if (TDBinnowise.Visible == true)
                        {
                            BinNO = DDBinNo.SelectedIndex > 0 ? DDBinNo.SelectedItem.Text : "";
                        }

                        Lotno = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text;

                        TagNo = txttagno.Text == "" ? "Without Tag No" : txttagno.Text;


                        SqlParameter[] param = new SqlParameter[11];
                        param[0] = new SqlParameter("@BinNo", BinNO);
                        param[1] = new SqlParameter("@GODOWNID", ddlgodown.SelectedValue);
                        param[2] = new SqlParameter("@ITEMFINISHEDID", ItemFinishedId);
                        param[3] = new SqlParameter("@QTY", txtopeningstock.Text == "" ? "0" : txtopeningstock.Text);
                        param[4] = new SqlParameter("@TAGNO", TagNo);
                        param[5] = new SqlParameter("@Lotno", Lotno);
                        param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                        param[6].Direction = ParameterDirection.Output;
                        param[7] = new SqlParameter("@UPDATE_DEL", SqlDbType.Int);
                        param[7].Value = 0;
                        param[8] = new SqlParameter("@TRANTYPE", SqlDbType.Int);
                        param[8].Value = 1;
                        param[9] = new SqlParameter("@TABLENAME", SqlDbType.VarChar, 20);
                        param[9].Value = "Opening stock";
                        param[10] = new SqlParameter("@PRTID", SqlDbType.Int);
                        param[10].Value = 0;


                        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_BINNOVALIDATE_UPDATEDEL", param);
                        if (param[6].Value.ToString() != "")
                        {
                            lblmessage.Visible = true;
                            lblmessage.Text = param[6].Value.ToString();
                            Tran.Commit();
                            return;
                        }
                    }
                    //********
                    string strsql = "update stock set qtyinhand=" + txtcurrentstock.Text + " where stockid=" + hnstockid.Value;
                    int TranId = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select IsNull(Max(StockTranId),0)+1 From StockTran"));
                    strsql = strsql + @" Insert Into StockTran(Stockid,StockTranId,TranType,Quantity,TranDate,Userid,RealDate,TableName,PRTId,Unitid,coneuse) 
                    Values (" + hnstockid.Value + "," + TranId + ",'1'," + (Convert.ToDecimal(txtcurrentstock.Text) - Convert.ToDecimal(hnqtyinhand.Value)) + ",'" + txttrandate.Text + "'," + Session["varuserid"] + ",'" + DateTime.Now.ToString("dd-MMM-yyyy") + "','Stockadjust',0," + ddlunit.SelectedValue + "," + (txtnoofcone.Text == "" ? "0" : txtnoofcone.Text) + ")";
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strsql);
                    Tran.Commit();
                    fill_grid();
                    Refresh();
                    if (Session["varMasterCompanyIDForERP"].ToString() == "30")
                    {
                        btnshowDetail_Click(sender, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        //**********
    }
    protected void GDFinishedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblMessageVal.Visible = false;
        lblMessageVal.Text = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        try
        {
            Label lblstockno = (Label)GDFinishedDetail.Rows[e.RowIndex].FindControl("lblstockno");
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@StockNo", lblstockno.Text);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDirectcarpetstockEntry", param);
            Tran.Commit();
            lblMessageVal.Visible = true;
            lblMessageVal.Text = param[1].Value.ToString();
            FillGridFinisheditem();
        }
        catch (Exception ex)
        {
            lblMessageVal.Visible = true;
            lblMessageVal.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ddlgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinnowise.Visible == true)
        {
            string str = "select BINNO,BINNO as BINNO1,BINid From BINMASTER Where Godownid=" + ddlgodown.SelectedValue + " order by BINID";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
        }
    }
    private void CheckBackDateEntryStop()
    {
        //Check StopBackEntryDate
        //// if (variable.VarStopBackDateEntryOnAllForms == "1" && Session["varuserid"].ToString() != "1")
        //// {
        if (variable.VarStopBackDateEntryOnAllForms == "1")
        {
            string currentdate = DateTime.Now.ToString("dd-MMM-yyyy");

            //if (Convert.ToDateTime(txtdate.Text) < Convert.ToDateTime(currentdate))
            //{
            //    lblMessageVal.Visible = true;
            //    lblMessageVal.Text = "DATE NOT BE LESS THAN CURRENT DATE";
            //    return;
            //}

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();


            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@COMPANYID", ddlcompany.SelectedValue);
            param[1] = new SqlParameter("@DATE", txtdate.Text);
            param[2] = new SqlParameter("@MASTERCOMPANYID", Session["VarcompanyNo"]);
            param[3] = new SqlParameter("@ReturnDate", SqlDbType.VarChar, 30);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@ReturnHolidayType", SqlDbType.VarChar, 50);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@UserTypeId", Session["usertype"]);
            param[6] = new SqlParameter("@UserTypeNew", SqlDbType.Int);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@UserID", Session["varuserId"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_GETRETURNDATE_HOLIDAYNAME", param);

            if (param[6].Value.ToString() == "1")
            {
                if (Convert.ToDateTime(txtdate.Text) < Convert.ToDateTime(currentdate))
                {
                    lblMessageVal.Visible = true;
                    lblMessageVal.Text = "DATE NOT BE LESS THAN CURRENT DATE";
                    return;
                }
            }

            if (txtdate.Text == param[3].Value.ToString())
            {
                lblMessageVal.Visible = true;
                lblMessageVal.Text = "PLEASE SELECT ANOTHER DATE DUE TO HOLIDAY.";
                Tran.Commit();
                return;
            }
            else
            {
                if (param[3].Value.ToString() != "")
                {
                    txtdate.Text = param[3].Value.ToString();
                }
            }
        }
    }

    protected void ExportExcelReport()
    {
        if (ddlcatagorytype.SelectedValue == "1")
        {
            #region Where Condition
            string Where = "";
            if (ddlitemname.SelectedIndex > 0)
            {
                Where = Where + " And VF.Item_Id=" + ddlitemname.SelectedValue;
                // filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
            }
            if (dquality.SelectedIndex > 0)
            {
                Where = Where + " And VF.QualityId=" + dquality.SelectedValue;
                //filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
            }
            if (dddesign.SelectedIndex > 0)
            {
                Where = Where + " And VF.DesignId=" + dddesign.SelectedValue;
                //filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
            }
            if (ddcolor.SelectedIndex > 0)
            {
                Where = Where + " And VF.ColorId=" + ddcolor.SelectedValue;
                //filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
            }
            if (ddshape.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShapeId=" + ddshape.SelectedValue;
                //filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
            }
            if (ddsize.SelectedIndex > 0)
            {
                Where = Where + " And VF.SizeId=" + ddsize.SelectedValue;
                // filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
            }
            if (ddlshade.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShadecolorId=" + ddlshade.SelectedValue;
                //filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
            }
            if (ddlgodown.SelectedIndex > 0)
            {
                Where = Where + " And S.godownId=" + ddlgodown.SelectedValue;
                //filterby = filterby + " design : " + dddesign.SelectedItem.Text;
            }
            if (ddlunit.SelectedIndex > 0)
            {
                Where = Where + " And St.unitid=" + ddlunit.SelectedValue;
                //filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
            }
            if (txtlotno.Text != "")
            {
                Where = Where + " And S.LotNo='" + txtlotno.Text + "'";

            }
            #endregion

            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", ddlcompany.SelectedValue);
            //param[1] = new SqlParameter("@TypeId", ddlcatagorytype.SelectedValue);
            //param[2] = new SqlParameter("@Where", Where);
            //param[3] = new SqlParameter("@MasterCompayId", Session["varMasterCompanyIDForERP"]);

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQtyInHandStockExcelReport", param);


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETQtyInHandStockExcelReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", ddlcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@TypeId", ddlcatagorytype.SelectedValue);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@MasterCompayId", Session["varMasterCompanyIDForERP"]);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                //Decimal TQty = 0, TAmount = 0, TBillQty = 0, TBillAmt = 0, TTransportAmt = 0, TUnloadingAmt = 0, ActualRecQty = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:N1").Merge();
                //sht.Range("A1").Value = "Purchase Material Received " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A1").Value = "Stock Report " + "For - " + ddlcompany.SelectedItem.Text;
                sht.Range("A2:N2").Merge();
                //sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Range("A2").Value = "";
                sht.Row(2).Height = 30;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N2").Style.Font.Bold = true;
                //***********Show Data
                row = 3;

                sht.Range("A" + row).Value = "Yarn Detail";
                sht.Range("B" + row).Value = "Yarn Count/Ply";
                sht.Range("C" + row).Value = "Shade Color";
                sht.Range("D" + row).Value = "Lot No";
                sht.Range("E" + row).Value = "Tag No";
                sht.Range("F" + row).Value = "Qty InHand ";
                sht.Range("G" + row).Value = "Rate";
                sht.Range("H" + row).Value = "Rack No";

                //sht.Range("O" + row + ":AA" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":H" + row).Style.Font.SetBold();
                using (var a = sht.Range("A" + row + ":H" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //******
                row = row + 1;
                int Rowfrom = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["ShadeColorName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["LotNo"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["TagNo"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["QtyinHand"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["Price"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["BinNo"]);

                    using (var a = sht.Range("A" + row + ":H" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                row = row + 1;
                ds.Dispose();

                ////*************GRAND TOTAL
                //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
                //sht.Range("N" + row).Value = "Grand Total";
                //sht.Range("O" + row).SetValue(TBillQty);
                //sht.Range("Q" + row).SetValue(TQty);
                //sht.Range("R" + row).SetValue(TBillAmt);
                //sht.Range("S" + row).SetValue(TAmount);
                //sht.Range("T" + row).SetValue(TTransportAmt);
                //sht.Range("U" + row).SetValue(TUnloadingAmt);
                ////*************
                sht.Columns(1, 30).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("QtyInHandStockReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);
            }
        }
    }
    protected void BtnStockNoToVCMSale_Click(object sender, EventArgs e)
    {
        int TypeFlag = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[1] = new SqlParameter("@USERID", 1);
            param[2] = new SqlParameter("@MASTERCOMPANYID", 42);
            param[3] = new SqlParameter("@TYPEFLAG", TypeFlag);
            param[4] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Save_StockNo_InVCMSale", param);
            if (param[4].Value.ToString() == "Successfully Inserted")
            {
                Tran.Commit();
            }
            else
            {
                Tran.Rollback();
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + param[4].Value + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('" + ex.Message + "')", true);
            Tran.Rollback();
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnUpdatePrice_Click(object sender, EventArgs e)
    {
        lblMessageVal.Visible = false;
        lblMessageVal.Text = "";
        string Str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();

        double VarPrice, VarPriceNew;
        try
        {
            for (int i = 0; i < GDFinishedDetail.Rows.Count; i++)
            {
                Label lblstockno = (Label)GDFinishedDetail.Rows[i].FindControl("lblstockno");
                Label lblPrice = (Label)GDFinishedDetail.Rows[i].FindControl("LblPrice");
                TextBox TxtPrice = (TextBox)GDFinishedDetail.Rows[i].FindControl("TxtPrice");

                VarPrice = lblPrice.Text == "" ? 0 : Convert.ToDouble(lblPrice.Text);
                VarPriceNew = TxtPrice.Text == "" ? 0 : Convert.ToDouble(TxtPrice.Text);
                if (VarPrice != VarPriceNew)
                {
                    Str = Str + lblstockno.Text + '|' + VarPriceNew + '~';
                }
            }

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@StrDetail", Str);
            param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[1].Direction = ParameterDirection.Output;
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);

            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateRateInCarpetNumber", param);
            if (param[1].Value.ToString() == "Date Successfully updated")
            {
                Tran.Commit();
            }
            else
            {
                Tran.Rollback();
            }

            lblMessageVal.Visible = true;
            lblMessageVal.Text = param[1].Value.ToString();
        }
        catch (Exception ex)
        {
            lblMessageVal.Visible = true;
            lblMessageVal.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DirectFinishedStockExcelReport()
    {
        if (ddlcatagorytype.SelectedValue == "0")
        {
            #region Where Condition
            string Where = "";
            if (ddlitemname.SelectedIndex > 0)
            {
                Where = Where + " And VF.Item_Id=" + ddlitemname.SelectedValue;
                // filterby = filterby + " Customer : " + ddcustomer.SelectedItem.Text;
            }
            if (dquality.SelectedIndex > 0)
            {
                Where = Where + " And VF.QualityId=" + dquality.SelectedValue;
                //filterby = filterby + " Order No : " + ddOrderno.SelectedItem.Text;
            }
            if (dddesign.SelectedIndex > 0)
            {
                Where = Where + " And VF.DesignId=" + dddesign.SelectedValue;
                //filterby = filterby + " Supp Name : " + dsuppl.SelectedItem.Text;
            }
            if (ddcolor.SelectedIndex > 0)
            {
                Where = Where + " And VF.ColorId=" + ddcolor.SelectedValue;
                //filterby = filterby + " PO No : " + DDPONo.SelectedItem.Text;
            }
            if (ddshape.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShapeId=" + ddshape.SelectedValue;
                //filterby = filterby + " Category : " + ddCatagory.SelectedItem.Text;
            }
            if (ddsize.SelectedIndex > 0)
            {
                Where = Where + " And VF.SizeId=" + ddsize.SelectedValue;
                // filterby = filterby + " Item : " + dditemname.SelectedItem.Text;
            }
            if (ddlshade.SelectedIndex > 0)
            {
                Where = Where + " And VF.ShadecolorId=" + ddlshade.SelectedValue;
                //filterby = filterby + " Quality : " + dquality.SelectedItem.Text;
            }
            if (ddlgodown.SelectedIndex > 0)
            {
                Where = Where + " And S.godownId=" + ddlgodown.SelectedValue;
                //filterby = filterby + " design : " + dddesign.SelectedItem.Text;
            }
            if (ddlunit.SelectedIndex > 0)
            {
                Where = Where + " And St.unitid=" + ddlunit.SelectedValue;
                //filterby = filterby + " Color : " + ddcolor.SelectedItem.Text;
            }
            if (txtlotno.Text != "")
            {
                Where = Where + " And S.LotNo='" + txtlotno.Text + "'";

            }
            #endregion

            //SqlParameter[] param = new SqlParameter[4];
            //param[0] = new SqlParameter("@CompanyId", ddlcompany.SelectedValue);
            //param[1] = new SqlParameter("@TypeId", ddlcatagorytype.SelectedValue);
            //param[2] = new SqlParameter("@Where", Where);
            //param[3] = new SqlParameter("@MasterCompayId", Session["varMasterCompanyIDForERP"]);

            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQtyInHandStockExcelReport", param);


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("PRO_GETDirectFinishedStockExcelReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Companyid", ddlcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@TypeId", ddlcatagorytype.SelectedValue);
            cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);
            cmd.Parameters.AddWithValue("@ToDate", TxtToDate.Text);
            cmd.Parameters.AddWithValue("@Where", Where);
            cmd.Parameters.AddWithValue("@MasterCompayId", Session["varMasterCompanyIDForERP"]);
            cmd.Parameters.AddWithValue("@UserId", Session["VarUserId"]);


            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
                }
                string Path = "";
                //Decimal TQty = 0, TAmount = 0, TBillQty = 0, TBillAmt = 0, TTransportAmt = 0, TUnloadingAmt = 0, ActualRecQty = 0;
                var xapp = new XLWorkbook();
                var sht = xapp.Worksheets.Add("sheet1");
                int row = 0;
                //***********
                sht.Range("A1:N1").Merge();
                //sht.Range("A1").Value = "Purchase Material Received " + "For - " + ddCompName.SelectedItem.Text;
                sht.Range("A1").Value = "Direct Finished Stock Report " + "For - " + ddlcompany.SelectedItem.Text;
                sht.Range("A2:N2").Merge();
                //sht.Range("A2").Value = "Filter By :  " + filterby;
                sht.Range("A2").Value = "";
                sht.Row(2).Height = 30;
                sht.Range("A1:N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A2:N2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                sht.Range("A1:N2").Style.Font.Bold = true;
                //***********Show Data
                row = 3;

                sht.Range("A" + row).Value = "ITEM NAME";
                sht.Range("B" + row).Value = "QUALITY";
                sht.Range("C" + row).Value = "DESIGN";
                sht.Range("D" + row).Value = "COLOR";
                sht.Range("E" + row).Value = "SHAPE";
                sht.Range("F" + row).Value = "SIZE ";
                sht.Range("G" + row).Value = "STOCKNO";
                sht.Range("H" + row).Value = "PRICE";

                //sht.Range("O" + row + ":AA" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                sht.Range("A" + row + ":H" + row).Style.Font.SetBold();
                using (var a = sht.Range("A" + row + ":H" + row))
                {
                    a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                //******
                row = row + 1;
                int Rowfrom = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Rowfrom == 0)
                    {
                        Rowfrom = row;
                    }
                    sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["ITEM_NAME"]);
                    sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["QUALITYNAME"]);
                    sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                    sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                    sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                    sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["SizeFt"]);
                    sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["TStockNo"]);
                    sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["Price"]);

                    using (var a = sht.Range("A" + row + ":H" + row))
                    {
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;
                }

                row = row + 1;
                ds.Dispose();

                ////*************GRAND TOTAL
                //sht.Range("N" + row + ":U" + row).Style.Font.SetBold();
                //sht.Range("N" + row).Value = "Grand Total";
                //sht.Range("O" + row).SetValue(TBillQty);
                //sht.Range("Q" + row).SetValue(TQty);
                //sht.Range("R" + row).SetValue(TBillAmt);
                //sht.Range("S" + row).SetValue(TAmount);
                //sht.Range("T" + row).SetValue(TTransportAmt);
                //sht.Range("U" + row).SetValue(TUnloadingAmt);
                ////*************
                sht.Columns(1, 30).AdjustToContents();
                //********************
                string Fileextension = "xlsx";
                string filename = UtilityModule.validateFilename("DirectFinishedStockReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "purchaserec", "alert('No Record Found!');", true);
            }
        }
    }
    protected void BtnExcelPreview_Click(object sender, EventArgs e)
    {
        DirectFinishedStockExcelReport();
    }
    protected void ChkForInchSize_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForInchSize.Checked == true)
        {
            ChkMtr.Checked = false;
        }
        if (ChkForInchSize.Checked == true)
        {
            //if (variable.VarNewQualitySize == "1")
            //{
            //    UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,MtrSize from QualitySizeNew where Shapeid=" + ddshape.SelectedValue, true, "select size");
            //}
            //else
            //{
            UtilityModule.ConditionalComboFill(ref ddsize, "select sizeid,sizeInch from size where Shapeid=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"], true, "select size");
            //}
        }
    }
    protected void TxtShadeColorFill_TextChanged(object sender, EventArgs e)
    {
        FillShadeColorName();
    }
    private void FillShadeColorName()
    {
        string Str = @"Select distinct ShadecolorId, ShadeColorName 
        From ShadeColor(Nolock) 
        Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        if (TxtShadeColorFill.Text != "")
        {
            Str = Str + " And ShadeColorName Like '" + TxtShadeColorFill.Text + @"%' ";
        }
        Str = Str + " Order By ShadeColorName ";
        UtilityModule.ConditionalComboFill(ref ddlshade, Str, true, "Select ShadeColor");
    }
}