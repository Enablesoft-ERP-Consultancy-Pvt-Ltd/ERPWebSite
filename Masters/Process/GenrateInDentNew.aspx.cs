using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_GenrateInDentNew : System.Web.UI.Page
{
    static int MasterCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {

        MasterCompanyId = Convert.ToInt16(Session["varMasterCompanyIDForERP"]);
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            hnorderid.Value = "0";
            ViewState["IndentId"] = 0;
            ViewState["FinalDate"] = "";
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By CompanyName", true, "--SelectCompany");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,Customercode+ SPACE(5)+CompanyName from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Customercode", true, "--SELECT--");
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                CustomerCodeSelectedChange();
            }


            TDBtnAddEmp.ColSpan = 5;
            int VarCompanyNo = Convert.ToInt32(Session["varMasterCompanyIDForERP"]);
            hncomp.Value = VarCompanyNo.ToString();

            //SizeType

            //
            string str = @"select Val,Type from SizeType Order  by Val
                       select flagforsampleorder From mastersetting";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            // UtilityModule.ConditionalComboFillWithDS(ref DDsizetype, ds, 0, false, "");

            //DataRow dr = AllEnums.MasterTables.Mastersetting.ToTable().Select()[0];
            if (Convert.ToInt16(ds.Tables[1].Rows[0]["flagforsampleorder"]) == 1)
            {
                //TDBtnAddEmp.ColSpan = 1;
                TDChkForOrder.Visible = true;
                //TDLblReqDate.Visible = true;
                // ChkEditOrder_CheckedChanged(sender, e);
                //TxtDate.Enabled = false;
                //BtnPreview.Visible = true;
                //lblfinalDate.Text = "";

            }
            else if (Session["varcompanyNo"].ToString() == "7")
            {
                //ChkForOrder.Visible = true;
            }
            else
            {
                // ChkForOrder.Visible = false;
            }


            switch (VarCompanyNo)
            {
                case 7:
                    TDBtnAddEmp.ColSpan = 1;
                    TDChkForOrder.Visible = true;
                    TDLblReqDate.Visible = true;
                    // tdrate.Visible = false;
                    // ChkForOrder.Checked = true;
                    //  ChkEditOrder_CheckedChanged(sender, e);
                    TxtDate.Enabled = false;
                    BtnPreview.Visible = false;
                    break;
                case 6:
                    BtnPreview.Visible = false;
                    //  TxtRate.Enabled = true;
                    break;
                case 4:
                    //  TDCaltype.Visible = false;
                    break;
                case 12:
                    // TDLotNo.Visible = false;
                    // TDStockQty.Visible = false;
                    // TxtRate.Enabled = true;
                    break;
                case 14:
                    // TDTagNo.Visible = true;
                    break;

            }
            //Without Define BOM
            if (Session["WithoutBOM"].ToString() == "1")
            {
                TDBtnAddEmp.ColSpan = 1;
                TDChkForOrder.Visible = true;
                TDLblReqDate.Visible = true;
                // ChkForOrder.Checked = true;
                // ChkEditOrder_CheckedChanged(sender, e);
                TxtDate.Enabled = false;

                lblfinalDate.Text = "";


            }
            if (VarCompanyNo == 6 || VarCompanyNo == 7 || VarCompanyNo == 3 || VarCompanyNo == 10 || Session["withoutBOM"].ToString() == "1")
            {
                // ChkForOrder.Text = "for Order Wise";
                UtilityModule.ConditionalComboFill(ref DDProcessName, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=0 And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Process--");

            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDProcessName, "select distinct PROCESS_Name_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=0 And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Process--");
            }
        }
    }

    protected void DDProcessProgramNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillReceiveDetails();

        if (Convert.ToString(ViewState["canedit"]) == "1")
        {
            ViewState["Indentid"] = "0";
            UtilityModule.ConditionalComboFill(ref DDIndentNo, "select distinct IM.IndentID,  IndentNo From IndentMaster IM INNER JOIN IndentDetail ID ON IM.IndentID=ID.IndentId Where IM.CompanyId=" + DDCompanyName.SelectedValue + " and PartyId=" + DDPartyName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + "  and ID.PPNo=" + DDProcessProgramNo.SelectedValue + " order by IndentNo", true, "--Select--");
        }
        else
        {
            DDIndentNo.Items.Clear();
        }

    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ////comment by sp

        // UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, "Select PPID,case When " + Session["varMasterCompanyIDForERP"] + @"=9 Then  om.localOrder+' / '+Replace(Str(PPID),'  ','')+' / '+CustomerOrderNo Else Replace(Str(PPID),'  ','')+' / '+CustomerOrderNo End  from ProcessProgram PP,OrderMaster OM where PP.Order_ID=OM.OrderId And Process_Id=" + DDProcessName.SelectedValue + " And PP.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " Order By PPID", true, "--Select--");

        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, "Select distinct PPID, dbo.f_getProcessProgramOrderNo(PP.PPId) as CustomerOrderNo  from ProcessProgram PP,OrderMaster OM where PP.Order_ID=OM.OrderId And Process_Id=" + DDProcessName.SelectedValue + " And PP.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " Order By PPID", true, "--Select--");
        if (Convert.ToInt32(Session["VarcompanyNo"]) == 7)
        {
            Btnorder.Visible = true;
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddpurchase_change();
    }
    private void ddpurchase_change()
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + DDProcessName.SelectedValue + " AND EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");
        if (DDOrderNo.SelectedIndex > 0)
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    string FinalDate;
                    FinalDate = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Replace(Convert(nvarchar(11),FinalDate,106),' ','-') As Finaldate From OrderProcessPlanning where OrderId=" + DDOrderNo.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + ""));
                    ViewState["FinalDate"] = FinalDate;
                    lblfinalDate.Text = "";
                    lblfinalDate.Text = "Stock At PH Date :" + FinalDate;
                    TxtReqDate.Text = Convert.ToString(Convert.ToDateTime(FinalDate).AddDays(-7).ToString("dd-MMM-yyyy"));
                    break;
            }

        }
        if (Convert.ToInt32(Session["varcompanyno"]) == 7)
        {
            if (DDProcessName.SelectedValue == "2" && DDOrderNo.SelectedIndex > 0)
            {
                fillorderdetail();
                tdordergrid.Visible = true;

            }
            //else if (ChkForOrder.Checked == true)
            //{
            //    fillorderdetail();
            //    tdordergrid.Visible = true;
            //    DDCategory.Enabled = true;
            //    DDItem.Enabled = false;
            //    DDQuality.Enabled = false;
            //    DDDesign.Enabled = false;
            //    DDColor.Enabled = false;
            //    DDShape.Enabled = false;
            //    DDColorShade.Enabled = false;
            //    DDSize.Enabled = false;
            //}
            else
            {
                tdordergrid.Visible = false;
            }
        }
        switch (Session["varcompanyNo"].ToString())
        {
            case "3":
                fillorderdetail();
                tdordergrid.Visible = true;
                break;
            case "10":
                fillorderdetail();
                tdordergrid.Visible = true;
                break;
            default:
                //if (ChkForOrder.Checked == true)
                //{
                //    fillorderdetail();
                //    tdordergrid.Visible = true;
                //    DDCategory.Enabled = false;
                //    DDItem.Enabled = false;
                //    DDQuality.Enabled = false;
                //    DDDesign.Enabled = false;
                //    DDColor.Enabled = false;
                //    DDShape.Enabled = false;
                //    DDColorShade.Enabled = false;
                //    DDSize.Enabled = false;
                //}
                break;
        }

    }


    protected void FillReceiveDetails()
    {
        string str = @"SELECT PC.PPId, PC.OrderId,PC.OrderDetailId, PC.FINISHEDID,PC.IFinishedid, ROUND(sum(QTY),3) QTY,FI2.CATEGORY_NAME+'/'+FI2.ITEM_NAME+'/'+FI2.QualityName+'/'+ FI2.ShadeColorName Description,OM.LocalOrder ORDERNO,UTM.UnitTypeID  
                        FROM   PP_CONSUMPTION PC INNER JOIN V_FinishedItemDetailNew FI2 ON PC.FINISHEDID=FI2.ITEM_FINISHED_ID 
                        INNER JOIN ITEM_MASTER IM ON FI2.ITEM_ID=IM.ITEM_ID 
                        INNER JOIN UNIT_TYPE_MASTER UTM ON IM.UnitTypeID=UTM.UnitTypeID                       
						 inner join OrderMaster OM on OM.OrderId=PC.OrderId 
						 where PPID='" + DDProcessProgramNo.SelectedValue + "' Group BY PC.PPId, PC.OrderId,PC.OrderDetailId, PC.FINISHEDID,PC.IFinishedid, OM.LocalOrder,FI2.CATEGORY_NAME,FI2.ITEM_NAME,FI2.QualityName,FI2.ShadeColorName,UTM.UnitTypeID";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGIndent.DataSource = ds.Tables[0];
        DGIndent.DataBind();
    }


    protected void BindStock()
    {

        int j = DGIndent.Rows.Count;
        float bqty = 0;

        if (j > 0)
        {
            for (int i = 0; i < j; i++)
            {

                string OFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblOFinishedId"))).Text;
                string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
                string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
                string ReqQty = ((TextBox)(DGIndent.Rows[i].FindControl("txtQty"))).Text;
                Label lblTotalStockQty = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));
                Label lbTotalStock = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStock")));

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@lotno", LotNo);
                    param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                    param[2] = new SqlParameter("@IFinishedId", IFinishedId);
                    param[3] = new SqlParameter("@stockqty", SqlDbType.Float);
                    param[3].Direction = ParameterDirection.Output;
                    //**********

                    DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_StockqtyNew", param);
                    Tran.Commit();
                    //((TextBox)(DGIndent.Rows[i].FindControl("txtStockQty"))).Text=param[3].Value.ToString();
                    lbTotalStock.Text = param[3].Value.ToString();
                    lblTotalStockQty.Text = param[3].Value.ToString();

                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    Tran.Rollback();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
    protected void BindPreLossAndTotalStock()
    {
        int j = DGIndent.Rows.Count;
        float bqty = 0;

        if (j > 0)
        {
            for (int i = 0; i < j; i++)
            {

                string OFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblOFinishedId"))).Text;
                string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
                string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
                string ReqQty = ((TextBox)(DGIndent.Rows[i].FindControl("txtQty"))).Text;
                Label lblTotalStockQty = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));
                Label lbTotalStock = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStock")));

                Label lblTotalQty = ((Label)(DGIndent.Rows[i].FindControl("lblTotalQty")));
                Label lblPreQty = ((Label)(DGIndent.Rows[i].FindControl("lblPreQty")));
                TextBox txtLoss = ((TextBox)(DGIndent.Rows[i].FindControl("txtLoss")));

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {

                    SqlParameter[] _arrpara = new SqlParameter[6];
                    _arrpara[0] = new SqlParameter("@FinishedId", SqlDbType.Int);
                    _arrpara[1] = new SqlParameter("@PPID", SqlDbType.Int);
                    _arrpara[2] = new SqlParameter("@ProcessID", SqlDbType.Int);
                    _arrpara[3] = new SqlParameter("@TotalQty", SqlDbType.Float);
                    _arrpara[4] = new SqlParameter("@PreQty", SqlDbType.Float);
                    _arrpara[5] = new SqlParameter("@Loss", SqlDbType.Float);
                    //_arrpara[6] = new SqlParameter("@TotalAssignedQTY", SqlDbType.Float);
                    _arrpara[0].Value = OFinishedId;
                    _arrpara[1].Value = DDProcessProgramNo.SelectedValue;
                    _arrpara[2].Value = DDProcessName.SelectedValue;
                    _arrpara[3].Direction = ParameterDirection.Output;
                    _arrpara[4].Direction = ParameterDirection.Output;
                    _arrpara[5].Direction = ParameterDirection.Output;
                    //  _arrpara[6].Direction = ParameterDirection.Output;
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Get_TotalConsmp_Indent_LossQty", _arrpara);
                    lblTotalQty.Text = _arrpara[3].Value.ToString();
                    lblPreQty.Text = _arrpara[4].Value.ToString();
                    txtLoss.Text = _arrpara[5].Value.ToString();

                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    Tran.Rollback();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

    }
    protected void DGIndent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label UnitTypeID = ((Label)e.Row.FindControl("lblUnitTypeId"));
            Label lblIFinishedId = ((Label)e.Row.FindControl("lblIFinishedId"));
            Label lblOFinishedId = ((Label)e.Row.FindControl("lblOFinishedId"));

            DropDownList DDUnit = ((DropDownList)e.Row.FindControl("DDUnit"));
            UtilityModule.ConditionalComboFill(ref DDUnit, "select UnitId,UnitName,UnitTypeId from Unit where UnitTypeId=" + UnitTypeID.Text, true, "");

            DropDownList DDCalType = ((DropDownList)e.Row.FindControl("DDCalType"));
            UtilityModule.ConditionalComboFill(ref DDCalType, "select CalID,CalType from Process_CalType order by caltype", true, "");

            DropDownList DDLotNo = ((DropDownList)e.Row.FindControl("DDLotNo"));

            string str1 = "Select TagNoWise from MasterSetting";
            DataSet ds1;
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows[0]["TagNoWise"].ToString() == "1")
                {
                    this.DGIndent.Columns[5].Visible = true;
                }
                else
                {
                    this.DGIndent.Columns[5].Visible = false;
                }
            }

            if (Session["varcompanyNo"].ToString() == "6")
            {
                string str = "select Distinct lotno,lotno From Stock Where Item_Finished_id in(select IFInishedid from PP_Consumption Where OrderId in(select Order_id from ProcessProgram where PPID=" + DDProcessProgramNo.SelectedValue + ") And FInishedid=" + lblOFinishedId.Text + ") And CompanyId=" + DDCompanyName.SelectedValue + "";
                DataSet ds;
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    UtilityModule.ConditionalComboFill(ref DDLotNo, "select Distinct lotno,lotno From Stock Where Item_Finished_id in(select IFInishedid from PP_Consumption Where OrderId in(select Order_id from ProcessProgram where PPID=" + DDProcessProgramNo.SelectedValue + ") And FInishedid=" + lblOFinishedId.Text + ") And CompanyId=" + DDCompanyName.SelectedValue + "", true, "");

                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDLotNo, "select 1,1", true, "select");
                }
            }
            else
            {
                //UtilityModule.ConditionalComboFill(ref ddllotno, "Select Distinct lotno,lotno from stock where item_finished_id in (Select IFinishedid from ORDER_CONSUMPTION_DETAIL Where Orderid in (Select Order_id from ProcessProgram Where PPID=" + DDProcessProgramNo.SelectedValue + ") And OFinishedid=" + finishedid + ") and companyid=" + DDCompanyName.SelectedValue + "", true, " Select ");
                if (MySession.Stockapply == "True")
                {
                    UtilityModule.ConditionalComboFill(ref DDLotNo, @"select distinct Lotno,lotno from stock s inner join PP_Consumption OCD 
                                          on S.ITEM_FINISHED_ID=OCD.IFINISHEDID and S.qtyinhand>0 inner join ProcessProgram PR
                                          on OCD.ORDERID=pr.Order_ID where pr.PPID=" + DDProcessProgramNo.SelectedValue + " and ocd.FINISHEDID=" + lblOFinishedId.Text + " and Companyid=" + DDCompanyName.SelectedValue + "", true, "");

                }
                else
                {
                    UtilityModule.ConditionalComboFill(ref DDLotNo, @"select distinct Lotno,lotno from stock s inner join PP_Consumption OCD 
                                          on S.ITEM_FINISHED_ID=OCD.IFINISHEDID inner join ProcessProgram PR
                                          on OCD.ORDERID=pr.Order_ID where pr.PPID=" + DDProcessProgramNo.SelectedValue + " and ocd.FINISHEDID=" + lblOFinishedId.Text + " and Companyid=" + DDCompanyName.SelectedValue + "", true, "");

                }
            }

            //UtilityModule.ConditionalComboFill(ref DDLotNo, "select Distinct lotno,lotno From Stock Where Item_Finished_id in(select IFInishedid from PP_Consumption Where OrderId in(select Order_id from ProcessProgram where PPID=" + DDProcessProgramNo.SelectedValue + ") And FInishedid=" + lblOFinishedId.Text + ") And CompanyId=" + DDCompanyName.SelectedValue + "", true, "select");
        }
        BindStock();
        BindPreLossAndTotalStock();
    }

    protected void DDLotNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddLotNo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddLotNo.Parent.Parent;
            int idx = row.RowIndex;

            float bqty = 0;
            String lblIFinishedId = ((Label)row.FindControl("lblIFinishedId")).Text;
            String lblOFinishedId = ((Label)row.FindControl("lblOFinishedId")).Text;
            DropDownList ddl = (DropDownList)row.FindControl("DDLotNo");

            Label lbTotalStock = (Label)row.FindControl("lblTotalStock");

            TextBox txt = (TextBox)row.FindControl("txtQty");

            Label lblTotalStockQty = (Label)row.FindControl("lblTotalStockQty");

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@lotno", ddl.SelectedValue);
                param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@IFinishedId", lblIFinishedId);
                param[3] = new SqlParameter("@stockqty", SqlDbType.Float);
                param[3].Direction = ParameterDirection.Output;
                //**********

                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_StockqtyNew", param);
                Tran.Commit();
                lbTotalStock.Text = param[3].Value.ToString();

                //string strsql = @"select isnull(sum(qtyinhand),0) as TotalStock from stock where lotno='" + ddl.SelectedValue + "' And companyid=" + DDCompanyName.SelectedValue + " and item_finished_id=" + lblIFinishedId;
                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
                if (lbTotalStock.Text != "" || lbTotalStock.Text != null)
                {

                    for (int i = 0; i < DGIndent.Rows.Count; i++)
                    {

                        string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
                        string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
                        Label lblTotalStockQty1 = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));
                        if (IFinishedId == lblIFinishedId && LotNo == ddl.SelectedValue)
                        {
                            TextBox ReqQty = (TextBox)DGIndent.Rows[i].FindControl("txtQty");
                            bqty = bqty + float.Parse(ReqQty.Text == "" ? "0" : ReqQty.Text);

                        }
                    }
                    for (int i = 0; i < DGIndent.Rows.Count; i++)
                    {
                        string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
                        string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
                        Label lblTotalStockQty1 = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));


                        if (IFinishedId == lblIFinishedId && LotNo == ddl.SelectedValue)
                        {
                            lblTotalStockQty1.Text = Convert.ToString(Math.Round(float.Parse(lbTotalStock.Text) - bqty, 3));
                        }
                    }
                    if (float.Parse(lblTotalStockQty.Text) < 0)
                    {
                        txt.Focus();
                        txt.Text = "";
                        txtQty_TextChanged(sender, e);
                        lblMessage1.Text = "Qty is greater than Stock or Total Qty";
                        lblMessage1.Visible = true;
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please add stock because total stock value is zero');", true);
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        lblMessage1.Text = "";
        lblMessage1.Visible = false;
        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        float bqty = 0;
        String lblIFinishedId = ((Label)currentRow.FindControl("lblIFinishedId")).Text;
        String lblOFinishedId = ((Label)currentRow.FindControl("lblOFinishedId")).Text;
        DropDownList ddl = (DropDownList)currentRow.FindControl("DDLotNo");
        Label lblTotalQty = (Label)currentRow.FindControl("lblTotalQty");
        Label lblPreQty = (Label)currentRow.FindControl("lblPreQty");

        Label lbTotalStock = (Label)currentRow.FindControl("lblTotalStock");

        TextBox txt = (TextBox)currentRow.FindControl("txtQty");

        Label lblTotalStockQty = (Label)currentRow.FindControl("lblTotalStockQty");

        for (int i = 0; i < DGIndent.Rows.Count; i++)
        {
            //if (i != currentRow.RowIndex)
            //{
            string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
            string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
            Label lblTotalStockQty1 = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));
            if (IFinishedId == lblIFinishedId && LotNo == ddl.SelectedValue)
            {
                TextBox ReqQty = (TextBox)DGIndent.Rows[i].FindControl("txtQty");
                bqty = bqty + float.Parse(ReqQty.Text == "" ? "0" : ReqQty.Text);
                //}
            }
        }
        for (int i = 0; i < DGIndent.Rows.Count; i++)
        {
            string IFinishedId = ((Label)(DGIndent.Rows[i].FindControl("lblIFinishedId"))).Text;
            string LotNo = ((DropDownList)(DGIndent.Rows[i].FindControl("DDLotNo"))).SelectedValue;
            Label lblTotalStockQty1 = ((Label)(DGIndent.Rows[i].FindControl("lblTotalStockQty")));

            if (IFinishedId == lblIFinishedId && LotNo == ddl.SelectedValue)
            {
                lblTotalStockQty1.Text = Convert.ToString(Math.Round(float.Parse(lbTotalStock.Text) - bqty, 3));
            }
        }

        //if (float.Parse(tbStockTotalQty.Text) < 0 || ((Convert.ToDouble(txt.Text == "" ? "0" : txt.Text) + (Convert.ToDouble(bqty) - Convert.ToDouble(txt.Text == "" ? "0" : txt.Text))) > (Convert.ToDouble(lblTotalQty.Text) - Convert.ToDouble(txtPreQty.Text == "" ? "0" : txtPreQty.Text))))

        if (float.Parse(lblTotalStockQty.Text) < 0 || (Convert.ToDouble(txt.Text == "" ? "0" : txt.Text) > (Convert.ToDouble(lblTotalQty.Text) - Convert.ToDouble(lblPreQty.Text == "" ? "0" : lblPreQty.Text))))
        {
            txt.Focus();
            txt.Text = "";
            txtQty_TextChanged(sender, e);
            lblMessage1.Text = "Qty is greater than Stock or Total Qty";
            lblMessage1.Visible = true;
            return;
        }

    }

    //protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    //{
    //    fillsize();
    //}

    protected void BtnSave_Click(object sender, EventArgs e)
    {

        //if (Convert.ToInt32(Session["VarcompanyNo"]) == 5)
        //{
        //    string ChkMsg = CheckStockQty();
        //    if (ChkMsg == "G")
        //    {
        //        lblMessage.Visible = true;
        //        lblMessage.Text = "Qty should not be greater than assigned stock";
        //        return;
        //    }
        //}
        //if (ChkForOrder.Checked == false)
        //{
        //    check_qty();
        //}

        if (lblMessage.Visible == false && lblMessage.Text == "" && lblMessage1.Text == "")
        {
            Save_indent();

            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    fillorderdetail();
                    break;
            }
            if (Session["WithoutBOM"].ToString() == "1")
            {
                fillorderdetail();
            }
        }
    }
    private void Save_indent()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("PPNo", typeof(int));
        dtrecords.Columns.Add("OFinishedid", typeof(int));
        dtrecords.Columns.Add("IFinishedid", typeof(int));
        dtrecords.Columns.Add("Quantity", typeof(float));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("DyingType", typeof(int));
        dtrecords.Columns.Add("LotNo", typeof(string));
        dtrecords.Columns.Add("orderid", typeof(int));
        dtrecords.Columns.Add("Unitid", typeof(int));
        dtrecords.Columns.Add("LossPercentage", typeof(float));
        dtrecords.Columns.Add("orderdetailid", typeof(int));
        dtrecords.Columns.Add("Remark", typeof(string));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("IndentQty", typeof(float));
        dtrecords.Columns.Add("TagNo", typeof(string));
        for (int i = 0; i < DGIndent.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)DGIndent.Rows[i].FindControl("Chkboxitem"));
            TextBox txtqty = ((TextBox)DGIndent.Rows[i].FindControl("txtQty"));
            if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            {
                DataRow dr = dtrecords.NewRow();
                //***********
                Label lblPPId = ((Label)DGIndent.Rows[i].FindControl("lblPPId"));
                Label lblOFinishedId = ((Label)DGIndent.Rows[i].FindControl("lblOFinishedId"));
                Label lblIFinishedId = ((Label)DGIndent.Rows[i].FindControl("lblIFinishedId"));
                TextBox txtRate = ((TextBox)DGIndent.Rows[i].FindControl("txtRate"));
                DropDownList ddlCalType = ((DropDownList)DGIndent.Rows[i].FindControl("DDCalType"));
                DropDownList ddlLotNo = ((DropDownList)DGIndent.Rows[i].FindControl("DDLotNo"));
                Label lblOrderId = ((Label)DGIndent.Rows[i].FindControl("lblOrderId"));
                DropDownList DDUnit = ((DropDownList)DGIndent.Rows[i].FindControl("DDUnit"));
                TextBox txtLoss = ((TextBox)DGIndent.Rows[i].FindControl("txtLoss"));
                Label lblOrderDetailId = ((Label)DGIndent.Rows[i].FindControl("lblOrderDetailId"));
                TextBox txtItemRemark = ((TextBox)DGIndent.Rows[i].FindControl("txtItemRemark"));
                TextBox txtTagNo = ((TextBox)DGIndent.Rows[i].FindControl("txtTagNo"));

                //**************
                dr["PPno"] = lblPPId.Text;
                dr["Ofinishedid"] = lblOFinishedId.Text;
                dr["IFinishedid"] = lblIFinishedId.Text;
                dr["Quantity"] = txtqty.Text;
                dr["Rate"] = txtRate.Text == "" ? "0" : txtRate.Text;
                dr["DyingType"] = ddlCalType.SelectedValue == "" ? "0" : ddlCalType.SelectedValue;
                dr["LotNo"] = ddlLotNo.SelectedValue == "" ? "0" : ddlLotNo.SelectedValue;
                dr["orderid"] = lblOrderId.Text;
                dr["Unitid"] = DDUnit.SelectedValue == "" ? "0" : DDUnit.SelectedValue;
                dr["LossPercentage"] = txtLoss.Text == "" ? "0" : txtLoss.Text;
                dr["orderdetailid"] = lblOrderDetailId.Text;
                dr["Remark"] = txtItemRemark.Text;
                dr["flagsize"] = "0";
                dr["IndentQty"] = txtqty.Text;
                dr["TagNo"] = txtTagNo.Text == "" ? "0" : txtTagNo.Text;

                dtrecords.Rows.Add(dr);
            }
        }
        //********************
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@IndentId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = ViewState["Indentid"];
                param[1] = new SqlParameter("@Companyid", DDCompanyName.SelectedValue);
                param[2] = new SqlParameter("@PartyId", DDPartyName.SelectedValue);
                param[3] = new SqlParameter("@ProcessId", DDProcessName.SelectedValue);
                param[4] = new SqlParameter("@Date", TxtDate.Text);
                param[5] = new SqlParameter("@IndentNo", SqlDbType.VarChar, 50);
                param[5].Direction = ParameterDirection.InputOutput;
                param[5].Value = TxtIndentNo.Text.ToUpper();
                param[6] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[7] = new SqlParameter("@userid", Session["varuserid"]);
                param[8] = new SqlParameter("@ReqDate", TxtReqDate.Text);
                param[9] = new SqlParameter("@orderwiseflag", (TDppno.Visible == true ? 0 : 1));
                param[10] = new SqlParameter("@Gremark", txtremarks.Text);
                param[11] = new SqlParameter("@Indentdetailid", 0);
                param[12] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                param[12].Direction = ParameterDirection.Output;
                param[13] = new SqlParameter("@dtrecords", dtrecords);
                //**********

                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew2", param);
                int rowscount;
                rowscount = SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew2", param);
                Tran.Commit();
                if (rowscount == -1)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Please Check Indent No.if already exists then Change the Indentno And refresh the page.";
                }
                string intid = param[0].Value.ToString();
                ViewState["Indentid"] = param[0].Value.ToString();
                TxtIndentNo.Text = param[5].Value.ToString();
                lblMessage1.Text = param[12].Value.ToString();
                lblMessage1.Visible = true;
                BtnSave.Text = "Save";
                Fill_Grid();
                SaveRefresh();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }
    private void Fill_Grid()
    {
        BtnPreview.Enabled = true;
        DGIndentDetail.DataSource = GetDetail();
        DGIndentDetail.DataBind();
        switch (Session["varcompanyNo"].ToString())
        {
            case "4":
                Session["ReportPath"] = "Reports/GenrateIndentDeepak.rpt";
                break;
            case "14":
                Session["ReportPath"] = "Reports/GenrateIndentEMIKEA.rpt";
                break;
            default:
                Session["ReportPath"] = "Reports/GenrateIndent.rpt";
                break;
        }
        Session["CommanFormula"] = "{GenrateIndentReport.IndentID}=" + ViewState["Indentid"];
    }
    private DataSet GetDetail()
    {
        DataSet DS = null;
        string sqlstr = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (ChkForOrder.Checked == false)
        //{
        sqlstr = @"Select IND.IndentDetailId,INM.IndentID,IND.OFinishedId,IND.IFinishedId,PPNo,IndentNo,IndentQty Quantity,Rate,VF1.CATEGORY_NAME+space(3)+VF1.ITEM_NAME+space(3)+VF1.QualityName+ Space(3)+VF1.designName+ 
                              Space(3)+VF1.ColorName+ Space(3)+VF1.ShadeColorName+ Space(3)+VF1.ShapeName+ Space(3)+VF1.SizeMtr InDescription,VF.CATEGORY_NAME+space(3)+VF.ITEM_NAME+space(3)+
                              VF.QualityName+ Space(3)+VF.designName+ Space(3)+VF.ColorName+ Space(3)+VF.ShadeColorName+ Space(3)+VF.ShapeName+ Space(3)+case when IND.flagsize=1 Then VF.SizeMtr Else case When IND.flagSize=0 Then vf.SizeFt else vf.Sizeinch End ENd OutDescription,ExtraQty,CancelQty,IND.Lotno,IND.TagNo
                              From IndentMaster INM
                              inner join IndentDetail IND on INM.indentid=IND.IndentId
                              inner join V_FinishedItemDetailNew VF on vf.ITEM_FINISHED_ID=ind.OFinishedId
                              left join V_FinishedItemDetailNew VF1 on vf1.ITEM_FINISHED_ID=ind.IFinishedId
                              Where IND.IndentId=" + ViewState["Indentid"] + " And INM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And INM.CompanyId=" + DDCompanyName.SelectedValue;


        //                }
        //                else
        //                {
        //        sqlstr = @"Select  IND.IndentDetailId,PPNo,IndentNo,Quantity,Rate,'' InDescription,
        //                    ICM.Category_Name+space(5)+IM.Item_Name+space(5)+IPM.QDCS + Space(5)+IPM.SizeMtr OutDescription,ExtraQty,IND.Lotno,IND.TagNo From IndentMaster INM inner join IndentDetail IND on 
        //                    IND.IndentId=INM.IndentId inner join ViewFindFinishedidItemidQDCSSNew IPM on IND.OFinishedId=IPM.Finishedid  inner join Item_Master IM on IPM.Item_Id=IM.Item_Id inner join ITEM_CATEGORY_MASTER ICM on IM.Category_Id=ICM.Category_Id  Where  IND.IndentId=" + ViewState["IndentId"] + " And INM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And INM.CompanyId=" + DDCompanyName.SelectedValue;
        //          }
        try
        {
            DS = SqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/GenrateInDentNew.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return DS;
    }
    private void SaveRefresh()
    {
        DDCompanyName.Enabled = false;
        DDPartyName.Enabled = false;
        DDProcessName.Enabled = false;
        DDProcessProgramNo.Enabled = false;
        DDIndentNo.Enabled = false;
        TxtIndentNo.Enabled = false;
        TxtDate.Enabled = false;
        TxtIndentNo.Enabled = false;
        DDCustomerCode.Enabled = false;
        DDOrderNo.Enabled = false;
        if (hncomp.Value != "7")
        {

        }
        txtremarks.Text = "";
        hnorderid.Value = "0";
        //txtitemremark.Text = "";     
        DDDyingType.SelectedItem.Text = "";
        DDDyeingMatch.SelectedItem.Text = "";
        DDDyeing.SelectedItem.Text = "";

    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        Response.RedirectLocation = "../../GenrateInDentNew.aspx";
    }

    //protected void DDcaltype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    TxtFinishedid.Text = "a1=" + DDCompanyName.SelectedValue + "&a2=" + DDPartyName.SelectedValue + "&a3=" + Session["FinishedId"] + "&a4=" + DDcaltype.SelectedValue;
    //}
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }


    //---------------------------------------Product code autocomplete--------------
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetQuality(string prefixText, int count)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        string strQuery = "Select ProductCode from ITEM_PARAMETER_MASTER IPM inner join item_Master IM on IM.Item_Id=IPM.Item_Id inner join CategorySeparate CS on CS.CategoryId=IM.Category_Id  where id=0 and ProductCode Like  '" + prefixText + "%' And IM.MasterCompanyId=" + MasterCompanyId;
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

    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForEdit.Checked == true)
        {
            TDIndentNo.Visible = true;
            ViewState["canedit"] = "1";
            DDIndentNo.Items.Clear();
        }
        else
        {
            TDIndentNo.Visible = false;
            ViewState["canedit"] = null;
            DDIndentNo.Items.Clear();
            ViewState["Indentid"] = "0";
            TxtIndentNo.Text = "";
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtremarks.Text = "";
            DDProcessName.SelectedValue = "";
            DDProcessProgramNo.SelectedValue = "";
            DDPartyName.SelectedValue = "";
            DGIndentDetail.DataSource = null;
            DGIndentDetail.DataBind();
        }
    }
    protected void DDIndentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Indentid,IndentNo,Replace(convert(nvarchar(11),Date,106),' ','-') as IndentDate,
                     Replace(convert(nvarchar(11),ReqDate,106),' ','-') as ReqDate,Gremark from Indentmaster where Indentid=" + DDIndentNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Indentid"] = ds.Tables[0].Rows[0]["Indentid"].ToString();
            TxtIndentNo.Text = ds.Tables[0].Rows[0]["IndentNo"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["IndentDate"].ToString();
            TxtReqDate.Text = ds.Tables[0].Rows[0]["ReqDate"].ToString();
            txtremarks.Text = ds.Tables[0].Rows[0]["Gremark"].ToString();
            Fill_Grid();
        }
        else
        {
            ViewState["Indentid"] = "0";
            TxtIndentNo.Text = "";
            TxtDate.Text = "";
            TxtReqDate.Text = "";
            txtremarks.Text = "";
            DDProcessName.SelectedValue = "";
            DDProcessProgramNo.SelectedValue = "";
            DDPartyName.SelectedValue = "";
        }
    }
    protected void TxtIndentNo_TextChanged(object sender, EventArgs e)
    {
        string str = @" select distinct IM.Indentid,IndentNo,Replace(convert(nvarchar(11),Date,106),' ','-') as IndentDate,
                        Replace(convert(nvarchar(11),ReqDate,106),' ','-') as ReqDate,Gremark,CompanyId,PartyId,ProcessID,PPNo from Indentmaster IM
                        INNER JOIN IndentDetail ID ON IM.IndentID=ID.IndentId where IndentNo='" + TxtIndentNo.Text.Trim() + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ChkForEdit.Checked = true;
            ChkForEdit_CheckedChanged(sender, new EventArgs());
            ViewState["Indentid"] = ds.Tables[0].Rows[0]["Indentid"].ToString();
            TxtIndentNo.Text = ds.Tables[0].Rows[0]["IndentNo"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["IndentDate"].ToString();
            TxtReqDate.Text = ds.Tables[0].Rows[0]["ReqDate"].ToString();
            txtremarks.Text = ds.Tables[0].Rows[0]["Gremark"].ToString();
            DDCompanyName.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
            DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessID"].ToString();
            ddpurchase_change();
            DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["PartyId"].ToString();
            UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, "Select distinct PPID, dbo.f_getProcessProgramOrderNo(PP.PPId) as CustomerOrderNo  from ProcessProgram PP,OrderMaster OM where PP.Order_ID=OM.OrderId And Process_Id=" + DDProcessName.SelectedValue + " And PP.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OM.CompanyId=" + DDCompanyName.SelectedValue + " Order By PPID", true, "--Select--");
            DDProcessProgramNo.SelectedValue = ds.Tables[0].Rows[0]["PPNo"].ToString();
            UtilityModule.ConditionalComboFill(ref DDIndentNo, "select distinct IM.IndentID,  IndentNo From IndentMaster IM INNER JOIN IndentDetail ID ON IM.IndentID=ID.IndentId Where IM.CompanyId=" + DDCompanyName.SelectedValue + " and PartyId=" + DDPartyName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + "  and ID.PPNo=" + DDProcessProgramNo.SelectedValue + " order by IndentNo", true, "--Select--");
            DDIndentNo.SelectedValue = ds.Tables[0].Rows[0]["Indentid"].ToString();
            FillReceiveDetails();
            Fill_Grid();
        }
        else
        {
            ViewState["Indentid"] = "0";
            TxtIndentNo.Text = "";
            TxtDate.Text = "";
            TxtReqDate.Text = "";
            txtremarks.Text = "";
            DDProcessName.SelectedValue = "";
            DDProcessProgramNo.SelectedValue = "";
            DDPartyName.SelectedValue = "";
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
            string str = "";
            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster OM,OrderLocalConsumption OC ,OrderProcessPlanning PP,orderdetail od,V_FinishedItemDetailNew v,UserRights_Category uc,PurchaseIndentIssue pii ,v_purchase_receive_report prr 
            Where om.orderid=od.orderid and od.Item_Finished_Id= v.Item_Finished_Id and v.CATEGORY_ID=uc.CategoryId and Om.status=0 and OM.OrderID=OC.OrderId and pp.orderid=om.orderid and pii.Orderid=om.orderid and prr.PIndentIssueId=pii.PIndentIssueId and pp.ProcessId=2 and pp.FinalStatus=1  and uc.userid=" + Session["varuserid"] + " And  om.Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                    break;
                case "3":
                    str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster OM,OrderLocalConsumption OC 
            Where Om.status=0 and OM.OrderID=OC.OrderId And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue;
                    break;
                case "10":
                    str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster OM,OrderLocalConsumption OC 
            Where Om.status=0 and OM.OrderID=OC.OrderId And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue;
                    break;
                case "6":
                    str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster OM,OrderLocalConsumption OC 
            Where Om.status=0 and OM.OrderID=OC.OrderId And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue + " And OrderCategoryid=2";
                    break;
                default:
                    if (Session["WithoutBOM"].ToString() == "1")
                    {
                        str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
                                    FROM OrderMaster OM,OrderLocalConsumption OC 
                                    Where Om.status=0 and OM.OrderID=OC.OrderId And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue;

                    }
                    else
                    {
                        str = @"SELECT Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
                            FROM OrderMaster OM,OrderLocalConsumption OC ,OrderProcessPlanning PP
                            Where Om.status=0 and OM.OrderID=OC.OrderId and pp.orderid=om.orderid and pp.ProcessId=2 and pp.FinalStatus=1 And  Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue;

                    }
                    break;

            }
            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string process = "";
        //1 For Final         
        switch (Session["varMasterCompanyIDForERP"].ToString())
        {

            case "7":
                process = "select Distinct Process_Name_Id,Process_Name From Process_Name_Master PM,OrderProcessPlanning PP where PM.Process_Name_Id=PP.ProcessId And OrderId=" + DDOrderNo.SelectedValue + " And FinalStatus=1 And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Process_Name Asc";
                break;
            default:
                process = "select Distinct Process_Name_Id,Process_Name From Process_Name_Master PM Where PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by Process_Name Asc";
                break;
        }

        UtilityModule.ConditionalComboFill(ref DDProcessName, process, true, "--Select Process--");

        if (DDProcessName.Items.Count > 0)
        {
            switch (Session["varcompanyNo"].ToString())
            {
                case "7":
                    DDProcessName.SelectedValue = "2";
                    ddpurchase_change();
                    break;
            }
        }
    }

    //protected void CheckReqDate()
    //{
    //    lblMessage1.Text = "";
    //    lblMessage1.Visible = false;
    //    DateTime dt1 = Convert.ToDateTime(TxtReqDate.Text);
    //    DateTime dt2 = Convert.ToDateTime(TxtDate.Text); //IssueDate 
    //    if (Convert.ToInt32(Session["VarcompanyNo"]) == 7)
    //    {
    //        DateTime dt3 = Convert.ToDateTime(ViewState["FinalDate"]);
    //        if (dt1 < dt2 || dt1 > dt3)
    //        {
    //            lblMessage1.Visible = true;
    //            lblMessage1.Text = "Pls Select Correct Date";
    //        }
    //    }
    //    else
    //    {
    //        if (dt1 < dt2)
    //        {
    //            lblMessage1.Visible = true;
    //            lblMessage1.Text = "Pls Select Correct Date";
    //        }
    //    }
    //}
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");

        UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, "", true, "--Select--");

    }
    //protected void DGIndentDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGIndentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    private void fillorderdetail()
    {
        string sql = @"select od.orderdetailid,CATEGORY_NAME+'  '+ Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName As Description,Sum(QtyRequired) As Qty,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,OD.ORDERUNITID AS UNIT
                     From OrderMaster OM,OrderDetail OD,V_FinishedItemDetailNew V  where OM.OrderId=OD.OrderId 
                     And V.Item_Finished_Id=OD.Item_Finished_Id And OM.OrderId=" + DDOrderNo.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by od.orderdetailid, Item_Name,QualityName,Designname,ColorName,ShadeColorName,OrderUnitId,SizeMtr,SizeFt,CATEGORY_NAME,CATEGORY_ID,v.ITEM_ID,QualityId,ColorId,designId,SizeId,ShapeId,ShadecolorId,OD.ORDERUNITID";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgorder.DataSource = ds;
            dgorder.DataBind();
            dgorder.Visible = true;
        }
        else
        {
            dgorder.Visible = false;
        }
    }
    protected void dgorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(dgorder.SelectedIndex.ToString());
        string category = ((Label)dgorder.Rows[r].FindControl("lblcategoryid")).Text;
        string Item = ((Label)dgorder.Rows[r].FindControl("lblitem_id")).Text;
        string Quality = ((Label)dgorder.Rows[r].FindControl("lblQualityid")).Text;
        string Color = ((Label)dgorder.Rows[r].FindControl("lblColorid")).Text;
        string design = ((Label)dgorder.Rows[r].FindControl("lbldesignid")).Text;
        string shape = ((Label)dgorder.Rows[r].FindControl("lblshapeid")).Text;
        string shadecolor = ((Label)dgorder.Rows[r].FindControl("lblshadecolorid")).Text;
        string size = ((Label)dgorder.Rows[r].FindControl("lblsizeid")).Text;
        string Qty = ((Label)dgorder.Rows[r].FindControl("lblqty")).Text;
        string orderdet = ((Label)dgorder.Rows[r].FindControl("lblorderdet")).Text;
        string UNIT = ((Label)dgorder.Rows[r].FindControl("LBLUNIT")).Text;
        string balqty = ((Label)dgorder.Rows[r].FindControl("lblbalnce")).Text;

        HNPENQTY.Value = balqty;
        hnorderid.Value = orderdet;
        Hnqty.Value = Qty;

    }
    protected void dgorder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dgorder, "Select$" + e.Row.RowIndex);
        }
    }
    public string getgiven(string strval, string Str)
    {
        string val = "";
        string qty = Convert.ToString(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(sum(Quantity),0) from Indentmaster IM,indentdetail ID where Im.IndentId=ID.IndentId And orderdetailid=" + strval + " And processId=" + DDProcessName.SelectedValue + ""));
        val = Convert.ToString(Convert.ToDouble(Str) - Convert.ToDouble(qty));
        return val;
    }
    protected void DGIndentDetail_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        //    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGIndentDetail, "Select$" + e.Row.RowIndex);
        //}
        //if (hncomp.Value == "7")
        //{
        //    e.Row.Cells[2].Visible = false;
        //    e.Row.Cells[3].Visible = false;
        //    e.Row.Cells[5].Visible = false;
        //}
    }
    protected void refreshEmp_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EmpName from EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId where processId=" + DDProcessName.SelectedValue + " And EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by ei.empname", true, "--Select--");
    }


    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (MySession.IndentAsProduction == "1")
        {
            Showreport();
            return;
        }

        Session["ReportPath"] = "Reports/GenerateNewIndentReport.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[2];
            _arrPara[0] = new SqlParameter("@IndentId", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@varCompanyId", SqlDbType.Int);


            _arrPara[0].Value = ViewState["Indentid"];
            _arrPara[1].Value = Session["varMasterCompanyIDForERP"].ToString();

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "Pro_GetReportNewGenerateIndent", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\GenerateNewIndentReport.xsd";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = Session["ReportPath"];
                Session["GetDataset"] = ds;
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
        catch (Exception ex)
        {
            tran.Rollback();
            lblMessage1.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Showreport()
    {
        if (ViewState["Indentid"].ToString() == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "alert", "alert('Please select or Type indent No!!!');", true);
            return;
        }
        string str = "select * from [GenrateIndentReport] where indentid=" + ViewState["IndentId"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

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
    protected void DGIndentDetail_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        //NewEditIndex property used to determine the index of the row being edited.  
        DGIndentDetail.EditIndex = e.NewEditIndex;
        Fill_Grid();
    }
    protected void DGIndentDetail_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        //Finding the controls from Gridview for the row which is going to update  
        Label lblIndentDetailId = DGIndentDetail.Rows[e.RowIndex].FindControl("lblIndentDetailId") as Label;
        Label lblIndentId = DGIndentDetail.Rows[e.RowIndex].FindControl("lblIndentId") as Label;
        Label lblLotNo = DGIndentDetail.Rows[e.RowIndex].FindControl("lblLotNo") as Label;
        TextBox txtRate = DGIndentDetail.Rows[e.RowIndex].FindControl("txtRate") as TextBox;
        TextBox txtQty = DGIndentDetail.Rows[e.RowIndex].FindControl("txtQty") as TextBox;
        Label lblOFinishedId = DGIndentDetail.Rows[e.RowIndex].FindControl("lblOFinishedId") as Label;
        Label lblIFinishedId = DGIndentDetail.Rows[e.RowIndex].FindControl("lblIFinishedId") as Label;
        Label lbltxtQty = DGIndentDetail.Rows[e.RowIndex].FindControl("lbltxtQty") as Label;

        double TotalStock = 0;
        double TotalReqQty = 0;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@lotno", lblLotNo.Text);
            param[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
            param[2] = new SqlParameter("@IFinishedId", lblIFinishedId.Text);
            param[3] = new SqlParameter("@stockqty", SqlDbType.Float);
            param[3].Direction = ParameterDirection.Output;
            //**********

            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_StockqtyNew", param);

            TotalStock = Convert.ToDouble(param[3].Value.ToString());

            SqlParameter[] param2 = new SqlParameter[3];
            param2[0] = new SqlParameter("@PPNo", DDProcessProgramNo.SelectedValue);
            param2[1] = new SqlParameter("@OFinishedId", lblOFinishedId.Text);
            param2[2] = new SqlParameter("@Requireqty", SqlDbType.Float);
            param2[2].Direction = ParameterDirection.Output;
            //**********

            DataSet ds2 = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "PRO_TotalIssueqtyNew", param2);
            Tran.Commit();
            TotalReqQty = Convert.ToDouble(param2[2].Value.ToString());

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        if (TotalStock + Convert.ToDouble(lbltxtQty.Text == "" ? "0" : lbltxtQty.Text) < Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) || Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text) > (TotalReqQty + Convert.ToDouble(lbltxtQty.Text == "" ? "0" : lbltxtQty.Text)))
        {
            txtQty.Focus();
            // txtQty.Text = "";
            //txtQty_TextChanged(sender, e);
            lblMessage1.Text = "Qty is greater than Stock or Total Qty";
            lblMessage1.Visible = true;
            return;
        }
        else
        {
            lblMessage1.Text = "";
            lblMessage1.Visible = false;

            SqlConnection con2 = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con2.State == ConnectionState.Closed)
            {
                con2.Open();
            }
            SqlTransaction Tran2 = con2.BeginTransaction();
            try
            {

                SqlParameter[] _arrpara = new SqlParameter[5];
                _arrpara[0] = new SqlParameter("@IndentId", lblIndentId.Text);
                _arrpara[1] = new SqlParameter("@IndentDetailId", lblIndentDetailId.Text);
                _arrpara[2] = new SqlParameter("@Quantity", txtQty.Text);
                _arrpara[3] = new SqlParameter("@Rate", txtRate.Text);
                _arrpara[4] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                _arrpara[4].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Tran2, CommandType.StoredProcedure, "Pro_UPdateGenerateIndentQty", _arrpara);
                lblMessage1.Text = _arrpara[4].Value.ToString();
                //txtPreQty.Text = _arrpara[4].Value.ToString();
                //txtLoss.Text = _arrpara[5].Value.ToString();
                Tran2.Commit();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran2.Rollback();
            }
            finally
            {
                con2.Close();
                con2.Dispose();
            }


        }
        //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
        DGIndentDetail.EditIndex = -1;
        //Call ShowData method for displaying updated data  
        Fill_Grid();
        FillReceiveDetails();
    }
    protected void DGIndentDetail_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
        DGIndentDetail.EditIndex = -1;
        Fill_Grid();
    }

    protected void DGIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lblIndentDetailId = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblIndentDetailId")).Text;

        string lblIndentId = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblIndentId")).Text;


        if (DGIndentDetail.Rows.Count >= 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IndentId", ViewState["Indentid"]);
                param[1] = new SqlParameter("@IndentDetailId", lblIndentDetailId);
                param[2] = new SqlParameter("@VarCompanyID", Session["varMasterCompanyIDForERP"]);
                param[3] = new SqlParameter("@VarUserID", Session["varuserid"]);

                param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.Output;

                //**********
                //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDesignRatioSizeWise", param);
                DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.StoredProcedure, "Proc_DeleteGenerateIndentNew", param);

                lblMessage.Text = param[4].Value.ToString();

                Tran.Commit();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        Fill_Grid();
        FillReceiveDetails();
    }
}
