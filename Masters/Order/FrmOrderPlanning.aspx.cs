using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Order_FrmOrderPlanning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "SELECT customerid,CompanyName + SPACE(5)+Customercode from customerinfo Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--SELECT--");
            UtilityModule.ConditionalComboFill(ref DDProcessName, "Select PROCESS_NAME_ID,PROCESS_NAME From Process_Name_Master Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By PROCESS_NAME", true, "--SELECT--");
            //TxtProcessReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (DDCustomerCode.Items.Count > 0)
            {
                DDCustomerCode.SelectedIndex = 1;
                CustomerCodeSelectedChange();
            }
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
            if (Session["varcompanyno"].ToString() == "7")
            {
                if (ChkEditOrder.Checked == false)
                {
                    str = @"select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo from OrderMaster om inner join orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join 
                UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  left outer join OrderProcessPlanning pm On om.orderid=pm.orderid Where om.status=0 and om.orderid not in (select distinct orderid from JobAssigns where supplierqty<>0) and isnull(finalstatus,0)<>1 and uc.userid=" + Session["varuserid"] + " and customerid=" + DDCustomerCode.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    str = @" select Distinct om.OrderId,om.LocalOrder+ ' / ' +om.CustomerOrderNo from OrderMaster om inner join orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail v On od.Item_Finished_Id= v.Item_Finished_Id inner join 
                UserRights_Category uc On v.CATEGORY_ID=uc.CategoryId  left outer join OrderProcessPlanning pm On om.orderid=pm.orderid Where om.status=0 and uc.userid=" + Session["varuserid"] + " and customerid=" + DDCustomerCode.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
            }
            else
            {
                str = @"SELECT Distinct OrderId,LocalOrder+ ' / ' +CustomerOrderNo 
            FROM OrderMaster Where status=0 and Companyid=" + DDCompanyName.SelectedValue + " And Customerid=" + DDCustomerCode.SelectedValue;
            }
            UtilityModule.ConditionalComboFill(ref DDOrderNo, str, true, "--Select--");
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds1 = new DataSet();
        LblOrderDate.Text = "";
        LblOrderReqDate.Text = "";
        lblMessage.Text = "";
        GDNew.DataSource = ds1;
        DG.DataBind();
        Lblsave.Text = "";
        if (DDCompanyName.SelectedIndex > 0 && DDCustomerCode.SelectedIndex > 0 && DDOrderNo.SelectedIndex > 0)
        {
            fillgridnew();
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select Replace(Convert(varchar(11),OrderDate,106), ' ','-') as OrderDate,
            Replace(Convert(varchar(11),DispatchDate,106), ' ','-') as DispatchDate From OrderMaster Where OrderId=" + DDOrderNo.SelectedValue);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                //TxtProcessReqDate.Text = Ds.Tables[0].Rows[0]["OrderDate"].ToString();
                LblOrderDate.Text = Ds.Tables[0].Rows[0]["OrderDate"].ToString();
                LblOrderReqDate.Text = Ds.Tables[0].Rows[0]["DispatchDate"].ToString();
                Fill_Grid();
            }
        }
        checkbtn();
    }
    private void fillgridnew()
    {
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select OM.OrderId ID,od.orderdetailid,Item_Name+'  '+QualityName+'  '+Designname+'  '+ColorName+'  '+ShadeColorName+'  '+Case When OrderUnitId=4 Then SizeMtr Else  SizeMtr End +Space(4)+Cast(Sum(QtyRequired) As Nvarchar) As Description,Sum(QtyRequired) as qty,od.photo
                     From OrderMaster OM,OrderDetail OD,V_FinishedItemDetail V  where OM.OrderId=OD.OrderId 
                     And V.Item_Finished_Id=OD.Item_Finished_Id And OM.OrderId=" + DDOrderNo.SelectedValue + " And V.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by OM.OrderId,Item_Name,QualityName,Designname,ColorName,ShadeColorName,OrderUnitId,SizeMtr,SizeFt,od.orderdetailid,od.photo");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            GDNew.DataSource = ds1;
            GDNew.DataBind();
            lblqty.Text = ds1.Tables[0].Compute("sum(Qty)", " ").ToString();
        }
    }
    private void Fill_Grid()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT ID,LocalOrder+ ' / ' +CustomerOrderNo OrderNo,PROCESS_NAME ProcessName,Replace(Convert(varchar(11),Date,106), ' ','-') AS Date,Qty ,Replace(Convert(varchar(11),FinalDate,106), ' ','-') AS FinalDate, case when FinalStatus=1 then 'Approved' else 'Unapproved' end as status,FinalStatus as FinalStatus,op.planRemark as Remark,op.DepRemark as depRemark
                              From OrderProcessPlanning OP,OrderMaster OM,Process_Name_Master PNM Where OP.OrderId=OM.OrderId And OP.PROCESSID=PNM.PROCESS_NAME_ID  And
                              OP.OrderId=" + DDOrderNo.SelectedValue + " And PNM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        DG.DataSource = ds;
        DG.DataBind();
    }
    protected void TxtProcessReqDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dt1 = Convert.ToDateTime(TxtProcessReqDate.Text);
        DateTime dt2 = Convert.ToDateTime(LblOrderDate.Text);
        DateTime dt3 = Convert.ToDateTime(LblOrderReqDate.Text);
        if (dt1 < dt2 || dt1 > dt3)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Pls Select Correct Date";
            TxtProcessReqDate.Text = LblOrderDate.Text;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrpara = new SqlParameter[9];
                _arrpara[0] = new SqlParameter("@OrderId", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _arrpara[2] = new SqlParameter("@OrderDate", SqlDbType.DateTime);
                _arrpara[3] = new SqlParameter("@ID", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@Qty", SqlDbType.Float);
                _arrpara[5] = new SqlParameter("@FinalDate", SqlDbType.SmallDateTime);
                _arrpara[6] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrpara[7] = new SqlParameter("@CompanyId", SqlDbType.Int);
                _arrpara[8] = new SqlParameter("Remark", SqlDbType.NVarChar, 250);
                _arrpara[0].Value = DDOrderNo.SelectedValue;
                _arrpara[1].Value = DDProcessName.SelectedValue;
                _arrpara[2].Value = TxtProcessReqDate.Text;
                if (BtnSave.Text == "Save")
                {
                    _arrpara[3].Value = 0;
                }
                else
                {
                    _arrpara[3].Value = HiddenID.Value;
                }
                _arrpara[4].Value = TXTQty.Text;
                _arrpara[6].Value = Session["varuserid"].ToString();
                _arrpara[7].Value = Session["varMasterCompanyIDForERP"].ToString();
                _arrpara[8].Value = txtremark.Text;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_ProcessPlanning", _arrpara);
                Tran.Commit();
                TXTQty.Text = "";
                lblmsg.Text = "";
                lblmsg.Visible = false;
                txtremark.Text = "";
                BtnSave.Text = "Save";
                TxtProcessReqDate.Text = "";
                //TxtProcessReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                Fill_Grid();
            }
        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderPlanning.aspx");
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
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }

    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillOrderBack();
    }
    private void fillOrderBack()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            ds = null;
            BtnSave.Text = "Update";
            string sql = @"select OP.ID,CI.CustomerId,OM.OrderId,ProcessId,Qty,Replace(Convert(varchar(11),OrderDate,106), ' ','-') As Date from OrderProcessPlanning OP,OrderMaster OM,CustomerInfo CI where OP.OrderId=OM.OrderId
                           And OM.CustomerId=CI.CustomerId And OP.ID=" + DG.SelectedValue + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TXTQty.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                DDCustomerCode.SelectedValue = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["ProcessId"].ToString();
                TxtProcessReqDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
                HiddenID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
            }
        }
        catch (Exception ex)
        {
            // UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderPlanning.aspx");
            LblErrorMessage.Text = ex.Message;
            LblErrorMessage.Visible = true;
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
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_GridForLocalConsump();
    }
    private void Fill_GridForLocalConsump()
    {
        DGShowConsumption.DataSource = GetDetail();
        DGShowConsumption.DataBind();
    }
    private DataSet GetDetail()
    {
        string sql;
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            sql = @"select Category_Name,Item_Name,QualityName+'  '+Designname+'  '+ColorName+' '+ShapeName+'  '+SizeMtr As Description, Process_Name,
             Round(Sum(CASE WHEN ORDERCalTYPE=0 THEN CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OD.TOTALAREA*OCD.OQTY*1.196 ELSE OD.QTYREQUIRED*OD.TOTALAREA*OCD.OQTY End
             ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OQTY*1.196 ELSE OD.QTYREQUIRED*OCD.OQTY END END),3) QTY
             from V_FinishedItemDetail VI,Process_Name_Master PM,
            ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID 
            And OM.ORDERID=OD.ORDERID And OCD.OFinishedid=VI.Item_Finished_id And OCD.ProcessId=Process_Name_Id And OM.OrderId=" + DDOrderNo.SelectedValue + " And OCD.ProcessId=" + DDProcessName.SelectedValue + " And VI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by Category_Name,Item_Name,QualityName,Designname,ColorName,ShapeName,SizeMtr,Process_Name";
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                sql = @"select Category_Name,Item_Name,QualityName+'  '+Designname+'  '+ColorName+' '+ShapeName+'  '+SizeMtr As Description, ' ' As Process_Name,Sum(Qty) As Qty
                      from V_FinishedItemDetail VI,OrderLocalConsumption OC WHERE  OC.Finishedid=VI.Item_Finished_id And OC.OrderId=" + DDOrderNo.SelectedValue + " And VI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " group by Category_Name,Item_Name,QualityName,Designname,ColorName,ShapeName,SizeMtr";
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sql);
            }
            else
            {
                TXTQty.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
            }

        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderPlanning.aspx");
            // LblErrorMessage.Visible = true;
            //LblErrorMessage.Text = ex.Message;
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
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        //DDCustomerCode.SelectedIndex = -1;
        DDOrderNo.SelectedIndex = -1;
        DDProcessName.SelectedIndex = -1;
        TXTQty.Text = "";
        TxtProcessReqDate.Text = "";
        //TxtProcessReqDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        BtnSave.Text = "Save";
    }
    protected void BtnFinalsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LblErrorMessage.Text = "";
            Lblsave.Text = "";
            for (int j = 0; j < DG.Rows.Count; j++)
            {
                if (((TextBox)DG.Rows[j].FindControl("FinalDate")).Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Final Date can not be blank....";
                }
            }
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select id from OrderProcessPlanning where orderid=" + DDOrderNo.SelectedValue + " ");
            if (ds.Tables[0].Rows.Count < 2)
            {
                lblMessage.Text = "First Planed all Process";
                lblMessage.Visible = true;
            }
            if (lblMessage.Text == "")
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update OrderProcessPlanning set FinalStatus=1 , Appdate=getdate() where orderid=" + DDOrderNo.SelectedValue + " and FinalStatus<>1");
                Fill_Grid();
                checkbtn();
            }
         
        }
        catch (Exception ex)
        {
            //UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderPlanning.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    protected void Link_Del_Click(object sender, EventArgs e)
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete OrderProcessPlanning Where Id=" + DG.SelectedDataKey.Value + "");
        Fill_Grid();
    }
    protected void DGShowConsumption_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DG_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void GDNew_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int VarIndentDetailId = Convert.ToInt32(DG.DataKeys[e.RowIndex].Value);
        string finish = ((Label)DG.Rows[e.RowIndex].FindControl("lblstatus")).Text;
        if (finish == "0")
        {
            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Delete OrderProcessPlanning Where Id=" + VarIndentDetailId + "");
            Fill_Grid();
            lblmsg.Text = "";
            lblmsg.Visible = false;
        }
        else
        {
            lblmsg.Text = "You cannot delete approved entry";
            lblmsg.Visible = true;
        }
    }
    private void checkbtn()
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select * from OrderProcessPlanning where orderid=" + DDOrderNo.SelectedValue + " and finalstatus=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            btnExcelExport.Visible = true;
        }
        else
        {
            btnExcelExport.Visible = false;
        }
    }
    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        string str = @"SELECT ID,Category_Name +'  '+VF.ITEM_NAME+'  '+QualityName+'  '+DesignName+'  '+ColorName+'  '+ShadeColorName+'  '+ShapeName+'  '+
                   CASE WHEN OD.SizeUnit=1 Then SizeMtr Else SizeFt End  Description,U1.Unitname As Unit,Qty,Od.thanlength,od.remark FROM OrderLocalConsumption OD Inner JOIN V_FinishedItemDetail VF ON OD.FinishedId=VF.Item_Finished_Id 
                   INNER JOIN ITEM_PARAMETER_MASTER IPM ON OD.FinishedId=IPM.Item_Finished_Id INNER JOIN Unit U On OD.SizeUnit=U.UnitId inner Join Unit U1 on OD.UnitId=U1.UnitId  Where OrderId=" + DDOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGConsumption.DataSource = ds;
            DGConsumption.DataBind();
        }
        string str1 = @"select Distinct om.orderid,om.localorder,vf.CATEGORY_NAME,om.customerorderNO,Replace(convert(varchar(11),OrderDate,106),' ','-') as OrderDate ,Replace(convert(varchar(11),om.DispatchDate,106),' ','-') AS SRC,Replace(convert(varchar(11),DueDate,106),' ','-') as StoreDElDAte,(select  case when FinalDate is not null then Replace(convert(varchar(11),FinalDate,106),' ','-') else ' ' end from OrderProcessPlanning where orderid=om.orderid and processid=1) as DelDate 
        from ordermaster om inner join 
        orderdetail od On om.orderid=od.orderid inner join V_FinishedItemDetail VF On OD.Item_Finished_Id=VF.Item_Finished_Id 
        where om.orderid=" + DDOrderNo.SelectedValue + " And VF.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
        SqlDataAdapter sda = new SqlDataAdapter(str1, ErpGlobal.DBCONNECTIONSTRING);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        ds.Tables.Add(dt);
        if (ds.Tables[1].Rows.Count > 0)
        {
            DGConsumption.Style.Add("font-size", "1em");
            Response.Clear();
            string attachment = "attachment; filename=Internal Fabric Sheet.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            DGConsumption.RenderControl(htmlWrite);
            //Response.Write(@"<TABLE><tr><td align=left colspan=3>Internal Fabric Procurement Sheet</td></tr><tr><td align=left colspan=3>Dilli Karigari Limited</td></tr><tr align=center style=background-color:Silver;><td>Genral Detail</td></tr><tr><td></td><td>SR No.</td><td>" + ds.Tables[1].Rows[0]["localorder"].ToString() + "</td></tr><tr><td></td><td>Category</td><td>" + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + "</td></tr><tr><td></td><td>Order Number</td><td>" + ds.Tables[1].Rows[0]["customerorderNO"].ToString() + "</td></tr><tr><td></td><td>FAB ODER DATE</td><td>" + ds.Tables[1].Rows[0]["OrderDate"].ToString() + "</td></tr><tr><td></td><td>Fab SRC</td><td>" + ds.Tables[1].Rows[0]["SRC"].ToString() + "</td></tr><tr><td></td><td>Fab Cost</td><td></td></tr><tr><td></td><td>Fab Del Date</td><td>" + ds.Tables[1].Rows[0]["DelDate"] + "</td></tr><tr><td></td><td>Stock at Store</td><td>" + ds.Tables[1].Rows[0]["StoreDElDAte"].ToString() + "</td></tr><tr><td></td><td>Garment Description</td><td></td></tr><tr><td></td><td>Consumption</td><td></td></tr></table>" + stringWrite.ToString() + "<table><tr><td align=right colspan=2>Total</td><td>" + ds.Tables[0].Compute("sum(Qty)", " ").ToString() + "</td></tr><tr></tr><tr></tr><tr><td align=left colspan=3>Signature of Category Head</td></tr></table>");
            Response.Write(@"<TABLE><tr><td align=left style=font-weight:bold; colspan=3>Internal Fabric Procurement Sheet</td></tr><tr><td align=left colspan=3 style=font-weight:bold;>Dilli Karigari Limited</td></tr><tr align=center style=background-color:Silver;><td style=font-weight:bold;>General Detail</td></tr><tr><td></td><td style=font-weight:bold;>SR No.</td><td align=right>" + ds.Tables[1].Rows[0]["localorder"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Category</td><td align=right>" + ds.Tables[1].Rows[0]["CATEGORY_NAME"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Order Number</td><td align=right>" + ds.Tables[1].Rows[0]["customerorderNO"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>FAB ODER DATE</td><td align=right>" + ds.Tables[1].Rows[0]["OrderDate"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Fab SRC</td><td align=right>" + ds.Tables[1].Rows[0]["SRC"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Fab Cost</td><td align=right></td></tr><tr><td></td><td style=font-weight:bold;>Fab Del Date</td><td align=right>" + ds.Tables[1].Rows[0]["DelDate"] + "</td></tr><tr><td></td><td style=font-weight:bold;>Stock at Store</td><td align=right>" + ds.Tables[1].Rows[0]["StoreDElDAte"].ToString() + "</td></tr><tr><td></td><td style=font-weight:bold;>Garment Description</td><td></td></tr><tr><td></td><td style=font-weight:bold;>Consumption</td><td></td></tr><tr></tr><tr></tr></table>" + stringWrite.ToString() + "<table><tr><td align=right colspan=2 style=font-weight:bold;>Total</td><td>" + ds.Tables[0].Compute("sum(Qty)", " ").ToString() + "</td></tr><tr></tr><tr></tr><tr><td align=left colspan=3 style=font-weight:bold;>Signature of Category Head</td></tr></table>");
            Response.End();
        }
    }  
    protected void Page_Init(object sender, EventArgs e)
    {
        PostBackTrigger trigger = new PostBackTrigger();
        trigger.ControlID = btnExcelExport.ID;
        updatepanal.Triggers.Add(trigger);
    }
    protected void refreshPhotoRefImage_Click(object sender, EventArgs e)
    {
        fillgridnew();
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedChange();
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }
}