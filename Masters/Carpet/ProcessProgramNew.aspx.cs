using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;

public partial class Masters_Carpet_ProcessProgramNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();

            UtilityModule.ConditionalComboFill(ref ddcompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");
            if (ddcompany.Items.FindByValue(Session["CurrentWorkingCompanyID"].ToString()) != null)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
                CompanySelectedChange();
            }

            UtilityModule.ConditionalComboFill(ref ddprocess, "Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by PROCESS_NAME", true, "--Select--");
            if (ddprocess.Items.Count > 0)
            {
                ddprocess.SelectedValue = "5";
            }
        }
    }
    protected void ddcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedChange();
    }
    private void CompanySelectedChange()
    {
        if (ddcompany.SelectedIndex > 0)
        {
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 6:  ///Art INdia
                    UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
                    break;
                default:
                    UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5) +CompanyName from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
                    break;
            }
            //UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5)+CompanyName from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CustomerId", true, "--Select--");
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        editandwithoutedit();
    }
    private void editandwithoutedit()
    {
        if (ChekEdit.Checked == true)
        {
            string str1 = "Select DISTINCT PPID,PPID From Ordermaster OM,CustomerInfo CI,ProcessProgram P Where OM.OrderID=P.Order_Id And OM.CustomerId=CI.CustomerId And OM.CustomerId=" + ddcustomer.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " and P.Process_id=" + ddprocess.SelectedValue;
            UtilityModule.ConditionalComboFill(ref ddprocessprogram, str1, true, "--Select--");
            txtprocessprogram.Visible = false;
            lblprocessprogram.Visible = false;
            ddprocessprogram.Visible = true;
            lblprocessprogram1.Visible = true;
        }
        else
        {
            string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 And 
                          OM.CustomerId=" + ddcustomer.SelectedValue + @" And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And OM.OrderId Not IN(Select Order_Id From ProcessProgram WHERE Process_ID=" + ddprocess.SelectedValue + @") 
                          Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
            lblprocessprogram.Visible = true;
            txtprocessprogram.Visible = true;
            ddprocessprogram.Visible = false;
            lblprocessprogram1.Visible = false;
        }
    }
    protected void ddprocessprogram_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And PreProdAssignedQty>0 And OD.Tag_Flag=1 And 
                          OM.CustomerId=" + ddcustomer.SelectedValue + @" And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" And OM.OrderId Not IN(Select Order_Id From ProcessProgram) 
                          Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
        UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            fill_ConsumptionGride();
            string strsql = @"Select Distinct O.orderid,LocalOrder+' / '+Customerorderno +' / '+CustomerCode+' / '+replace(convert(varchar(11),isnull(ProdReqDate,''),106), ' ','-') as OrderNo from ordermaster o inner join OrderDetail OD on OD.OrderId=o.OrderId inner join Customerinfo C on o.Customerid=C.Customerid  inner join processprogram on o.orderid=order_id and ppid=" + ddprocessprogram.SelectedValue + " And C.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            con.Open();

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            int n = ds.Tables[0].Rows.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    chekboxlist.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                    int a = Convert.ToInt32(chekboxlist.Items.Count);
                    chekboxlist.Items[a - 1].Selected = true;
                    if (ChkCurrentConnsumption.Checked)
                    {
                        DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, @"Select OD.Item_Finished_Id,OD.Orderid,OD.Orderdetailid From OrderDetail OD,
                                      JobAssigns JA Where OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id  And PreProdAssignedQty>0 And OD.Tag_Flag=1 And 
                                      OD.OrderId in (" + dr[0].ToString() + ")");
                        int m = ds1.Tables[0].Rows.Count;
                        if (m > 0)
                        {
                            for (int i = 0; i < m; i++)
                            {
                                UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(ds1.Tables[0].Rows[i]["Item_Finished_Id"]), Convert.ToInt32(ds1.Tables[0].Rows[i]["Orderid"]), Convert.ToInt32(ds1.Tables[0].Rows[i]["Orderdetailid"]), 1, ChkCurrentConnsumption.Checked == true ? 1 : 0);
                            }
                        }
                    }
                }
            }
            fill_OrderConsumption();
            BtnPreview.Visible = true;
            // BtnLocalOcReport.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
            lblerror.Visible = true;
            lblerror.Text = ex.Message;
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

    public string OrderId = "";
    public double vartotalrawcal, varqtyinin, varqtyinincal, vartotalqty, varsize;

    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            string ppid = null;
            if (ChekEdit.Checked == true)
            {
                SqlHelper.ExecuteScalar(tran, CommandType.Text, "delete from processprogram where ppid=" + ddprocessprogram.SelectedValue);
                ppid = ddprocessprogram.SelectedValue;
            }
            else
            {
                ppid = (SqlHelper.ExecuteScalar(tran, CommandType.Text, "Select isnull(max(PPID),0)+1 from ProcessProgram").ToString());
            }
            SqlParameter[] _arrPara = new SqlParameter[7];
            _arrPara[0] = new SqlParameter("@PP_Detail_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Process_Id", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@Order_Id", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@PPid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);

            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddprocess.SelectedValue;
            _arrPara[3].Direction = ParameterDirection.InputOutput;
            _arrPara[3].Value = Convert.ToInt32(ppid);
            _arrPara[4].Value = Session["varuserid"].ToString();
            _arrPara[5].Value = Session["varMasterCompanyIDForERP"].ToString();
            int n = chekboxlist.Items.Count;
            for (int i = 0; i < n; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {
                    _arrPara[2].Value = chekboxlist.Items[i].Value;
                    OrderId += chekboxlist.Items[i].Value + ",";
                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_ProcessProgram", _arrPara);
                }
            }
            txtprocessprogram.Text = _arrPara[3].Value.ToString();

            SqlParameter[] _arrPara2 = new SqlParameter[4];
            _arrPara2[0] = new SqlParameter("@Order_Id", SqlDbType.VarChar, 50);
            _arrPara2[1] = new SqlParameter("@PPid", SqlDbType.Int);
            _arrPara2[2] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara2[3] = new SqlParameter("@varCompanyId", SqlDbType.Int);

            _arrPara2[0].Value = OrderId;
            _arrPara2[1].Value = Convert.ToInt32(ppid);
            _arrPara2[2].Value = Session["varuserid"].ToString();
            _arrPara2[3].Value = Session["varMasterCompanyIDForERP"].ToString();

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_GetProcessProgQtyDetails", _arrPara2);

            BtnPreview.Visible = true;
            // BtnLocalOcReport.Visible = false; 

            if (ChekEdit.Checked == true && DgConsumption.Rows.Count > 0)
            {
                save_Consumption(tran);
            }
            tran.Commit();
            lblerror.Text = "Save Details.................";
            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, "Select Distinct OD.orderid,LocalOrder+' / '+customerorderno as OrderNo from ordermaster o inner join OrderDetail OD on OD.OrderId=o.OrderId where o.customerid='" + ddcustomer.SelectedValue + "' and  OD.Tag_Flag=1  and orderid not in(select order_id from processProgram)");
            fill_grid();
            if (ChekEdit.Checked)
            {
                //ddprocessprogram.SelectedIndex = 0;
                fill_ConsumptionGride();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
            tran.Rollback();
            lblerror.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void save_Consumption(SqlTransaction tran)
    {
        SqlParameter[] _arrPara = new SqlParameter[7];
        _arrPara[0] = new SqlParameter("@PPId", SqlDbType.Int);
        _arrPara[1] = new SqlParameter("@FinishedId", SqlDbType.Int);
        _arrPara[2] = new SqlParameter("@OrderNo", SqlDbType.NVarChar, 50);
        _arrPara[3] = new SqlParameter("@Qty", SqlDbType.Float);
        _arrPara[4] = new SqlParameter("@ExtraQty", SqlDbType.Float);
        _arrPara[5] = new SqlParameter("@UserId", SqlDbType.Int);
        _arrPara[6] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

        int n = DgConsumption.Rows.Count;
        for (int i = 0; i < n; i++)
        {
            string ExQty = ((TextBox)DgConsumption.Rows[i].FindControl("TxtExtraQty")).Text;
            ExQty = ExQty == "" ? "0" : ExQty;
            if (ExQty != "0")
            {
                GridViewRow row = DgConsumption.Rows[i];
                string FID = DgConsumption.Rows[i].Cells[0].Text;
                string ONO = DgConsumption.Rows[i].Cells[3].Text;
                string Qty = DgConsumption.Rows[i].Cells[2].Text;

                _arrPara[0].Value = ddprocessprogram.SelectedValue;
                _arrPara[1].Value = FID;
                _arrPara[2].Value = ONO;
                _arrPara[3].Value = Convert.ToDouble(Qty);
                _arrPara[4].Value = Convert.ToDouble(ExQty);
                _arrPara[5].Value = Session["varuserid"].ToString();
                _arrPara[6].Value = Session["varMasterCompanyIDForERP"].ToString();

                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Update Top(1) PP_Consumption Set ExtraQty=@ExtraQty Where PPID=" + _arrPara[0].Value + " and OrderId=(Select Order_Id From ProcessProgram Where PPID=" + _arrPara[0].Value + ") and FinishedId=" + _arrPara[1].Value);

                //SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_PP_Consumption1", _arrPara);
            }
        }
        DgOrderConsumption.Visible = false;
        Note1.Visible = false;
    }
    private void fill_grid()
    {
        Dgprocessprogram.DataSource = Fill_Grid_Data();
        Dgprocessprogram.DataBind();
    }

    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = null;
            strsql = @"Select P.PP_Detail_ID as Sr_No,o.LocalOrder+' / '+customerorderno CustomerOrderNo from processProgram p ,orderMaster o where p.order_id=o.orderid and o.CustomerId=0 And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            if (ChekEdit.Checked == true)
            {
                strsql = @"Select P.PP_Detail_ID as Sr_No,o.LocalOrder+' / '+customerorderno CustomerOrderNo from processProgram p ,orderMaster o where p.order_id=o.orderid and o.CustomerId=" + ddcustomer.SelectedValue + " And P.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
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
    protected void ChekEdit_CheckedChanged(object sender, EventArgs e)
    {
        fill_grid();
        if (ChekEdit.Checked == true)
        {
            txtprocessprogram.Visible = false;
            lblprocessprogram.Visible = false;
            txtprocessprogram.Text = "";
            ddprocessprogram.Visible = true;
            lblprocessprogram1.Visible = true;
            UtilityModule.ConditionalComboFill(ref ddprocessprogram, "select distinct PPID,PPID from processprogram PP inner join OrderMaster  OM on OM.OrderId=PP.Order_Id where Process_Id=" + ddprocess.SelectedValue + " and CustomerId=" + ddcustomer.SelectedValue + " And PP.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select--");
        }
        else
        {
            lblprocessprogram.Visible = true;
            txtprocessprogram.Visible = true;
            ddprocessprogram.Visible = false;
            lblprocessprogram1.Visible = false;
            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, "select Distinct OD.orderid,LocalOrder+' / '+customerorderno as OrderNo from ordermaster o inner join OrderDetail OD on OD.OrderId=o.OrderId where o.customerid='" + ddcustomer.SelectedValue + "' and  OD.Tag_Flag=1 and  o.orderid not in(select order_id from processProgram)");
            fill_ConsumptionGride();
        }
    }
    private void fill_ConsumptionGride()
    {
        DgConsumption.DataSource = Fill_ConsumptionGrid_Data();
        DgConsumption.DataBind();
    }
    private DataSet Fill_ConsumptionGrid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = null;
            if (ChekEdit.Checked == true)
            {
                strsql = @"SELECT PC.FINISHEDID,sum(QTY) QTY,sum(ExtraQty) ExtraQty,IM.ITEM_NAME+'/'+FI2.Quality+'/'+ FI2.ShadeColor Description,OM.LocalOrder ORDERNO  FROM   PP_CONSUMPTION PC INNER JOIN ViewFindFinishedId2 FI2 ON PC.FINISHEDID=FI2.Finishedid 
                         INNER JOIN ITEM_MASTER IM ON IM.ITEM_ID=FI2.ITEM_ID inner join OrderMaster OM on OM.OrderId=PC.OrderId where PPID=" + ddprocessprogram.SelectedValue + " And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Group BY PC.FINISHEDID,OM.LocalOrder,ITEM_NAME,FI2.Quality,FI2.ShadeColor ";
            }
            else
            {
                strsql = "";
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
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
    protected void chekboxlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Note.Text = "";
        // BtnLocalOcReport.Visible = true;
        BtnPreview.Visible = false;
        save_consumption();
        fill_OrderConsumption();
    }
    private void fill_OrderConsumption()
    {
        // txtgreen.Visible = false;
        DgOrderConsumption.Visible = true;
        DgOrderConsumption.DataSource = Fill_OrderConsumption_Data();
        DgOrderConsumption.DataBind();
    }
    protected void DgOrderConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //string CellValue = e.Row.Cells[2].Text;
            //if (CellValue == "0")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Red;
            //    //txtgreen.Visible = true;
            //    //Note.Text = "Consumption Not Define";
            //}
            //else
            //{
            //    e.Row.BackColor = System.Drawing.Color.Green;
            //}

            e.Row.BackColor = System.Drawing.Color.Green;
            e.Row.Cells[2].Visible = false;
        }
    }
    private DataSet Fill_OrderConsumption_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {


            int orderid = chekboxlist.SelectedValue == "" ? 0 : Convert.ToInt32(chekboxlist.SelectedValue);
            string strsql = null;
            strsql = @"Select Distinct OM.OrderId OrderDetailId, Item_Name+' / '+ isnull(QualityName,'')+' / '+isnull(DesignName,'') +' / '+isnull(ColorName,'')+' / '+
                       isnull(ShapeName,'')+' / '+ Case When OD.flagsize=1 Then isnull(SizeMtr,'') 
                       When OD.flagsize=0 Then isnull(SizeFt,'')
                       When OD.flagsize=2 Then isnull(SizeInch,'') Else isnull(SizeFt,'') End+'   '+isnull(ShadeColorName,'') Description,
                       Sum(PreProdAssignedQty) QTY,[dbo].[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT] (OM.Orderid,JA.Item_Finished_Id) ConsmpOrderDetailId
                       From OrderMaster OM,OrderDetail OD,V_FinishedItemDetailNew IPM,Jobassigns JA 
                       Where OM.Orderid=OD.Orderid And OD.Item_Finished_Id=JA.Item_Finished_Id And OM.Orderid=JA.Orderid And IPM.Item_Finished_Id=JA.Item_Finished_Id And 
                       PreProdAssignedQty>0 And OM.Orderid=" + orderid + "  And IPM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @"
                       Group By OM.OrderId,Item_Name,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OrderUnitID,SizeMtr,SizeFt,SizeInch,Od.flagsize,JA.Item_Finished_Id";
            con.Open();

            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        imgLogo.ImageUrl.DefaultIfEmpty();
        imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        if (Session["varCompanyName"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }

    protected void ChkCurrentConnsumption_CheckedChanged(object sender, EventArgs e)
    {
        if (ddprocessprogram.Items.Count > 0)
        {
            ddprocessprogram.SelectedIndex = 0;
        }
        save_consumption();
    }
    private void save_consumption()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            if (ChkCurrentConnsumption.Checked)
            {
                DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select OD.Item_Finished_Id,OD.Orderid,OD.Orderdetailid From OrderDetail OD,JobAssigns JA Where OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id  And PreProdAssignedQty>0 And OD.Tag_Flag=1 And OD.OrderId in (" + chekboxlist.SelectedValue + ")");
                int n = ds1.Tables[0].Rows.Count;
                if (n > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        UtilityModule.ORDER_CONSUMPTION_DEFINE(Convert.ToInt32(ds1.Tables[0].Rows[i]["Item_Finished_Id"]), Convert.ToInt32(ds1.Tables[0].Rows[i]["Orderid"]), Convert.ToInt32(ds1.Tables[0].Rows[i]["Orderdetailid"]), 1, ChkCurrentConnsumption.Checked == true ? 1 : 0);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgramNew.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (ddprocess.SelectedIndex > 0)
        {
            report();
        }
    }
    private void report()
    {
        Session["ReportPath"] = "Reports/ConsumpNew.rpt";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {

            SqlParameter[] _arrPara = new SqlParameter[2];
            _arrPara[0] = new SqlParameter("@PPID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@varCompanyId", SqlDbType.Int);


            _arrPara[0].Value = ddprocessprogram.SelectedValue;
            _arrPara[1].Value = Session["varMasterCompanyIDForERP"].ToString();

            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, "Pro_GetReportNewConsumpOrder", _arrPara);

            Session["dsFileName"] = "~\\ReportSchema\\ConsumpNew.xsd";
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
            lblerror.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }

    }
    //protected void BtnLocalOcReport_Click(object sender, EventArgs e)
    //{
    //    if (ChkCurrentConnsumption.Checked == true)
    //    {
    //        Session["ReportPath"] = "Reports/LocalOC.rpt";
    //        Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + chekboxlist.SelectedValue + "";
    //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);

    //    }
    //}

    //protected void DgOrderConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void Dgprocessprogram_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DgConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void ddprocess_SelectedIndexChanged(object sender, EventArgs e)
    {

        editandwithoutedit();
        if (ddcustomer.Items.Count > 0)
        {
            ddcustomer.SelectedIndex = 0;
        }

    }
}