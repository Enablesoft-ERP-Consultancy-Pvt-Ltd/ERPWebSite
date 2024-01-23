using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;
using System.IO;
using ClosedXML.Excel;

public partial class Masters_Order_frmOrderTnaPlan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str;
            str = @"select CI.CompanyId,CI.CompanyName from CompanyInfo CI inner join Company_Authentication CA on Ci.CompanyId=CA.CompanyId
                  WHere CI.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " and CA.UserId=" + Session["varuserid"] + @"  order by CompanyName
                  select CustomerId,customercode+'  '+companyname from customerinfo where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + "  order by CustomerCode";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDCustomer, ds, 1, true, "--Select Buyer--");
            ds.Dispose();
        }
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillOrderDetail();
    }
    protected void DDCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OM.OrderId,OM.LocalOrder+'/'+OM.CustomerOrderNo as CustomerOrderNo from ordermaster OM Where OM.CustomerId=" + DDCustomer.SelectedValue + " and OM.CompanyId=" + DDCompanyName.SelectedValue + " and OM.status=0 order by OM.OrderId", true, "--Select Order No.--");
    }
    protected void fillOrderDetail()
    {
        string str;
        str = @"select distinct vf.CATEGORY_NAME+'  '+vf.ITEM_NAME+'  '+vf.QualityName+'  '+vf.designName+'  '+vf.ColorName
                +'  '+vf.ShadeColorName+'  '+vf.ShapeName+'  '+case when OrderUnitId=1 Then vf.SizeMtr Else case When OrderUnitId=2 Then vf.SizeFt
                Else case When orderunitid=6 then vf.SizeInch else '' End End End as ItemDescription,
                Od.item_finished_id,od.OrderDetailId,od.QtyRequired,Replace(convert(nvarchar(11),Om.orderdate,106),' ','-') as Orderdate,Replace(convert(nvarchar(11),Om.dispatchdate,106),' ','-') as  DispatchDate,
                orp.orderdetailid as RawtnaOrderdetailid   from OrderMaster OM inner join
                OrderDetail OD on om.OrderId=od.OrderId inner join V_FinishedItemDetail vf on od.Item_Finished_Id=vf.ITEM_FINISHED_ID
                left join orderTna ORP on Orp.orderid=od.OrderId and orp.orderdetailid=od.OrderDetailId Where Od.orderid=" + DDOrderNo.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVOrderDetail.DataSource = ds;
        GVOrderDetail.DataBind();
        ds.Dispose();
    }

    protected void fillrawprocurement(string Orderdetailid)
    {
        string str;
        DataSet ds = null;

        str = @"select Distinct vf.CATEGORY_NAME+'  '+vf.ITEM_NAME+'  '+vf.QualityName+'  '+vf.designName+'  '+vf.ColorName
                +'  '+vf.ShadeColorName+'  '+vf.ShapeName+'  '+case when OrderUnitId=1 Then vf.SizeMtr Else case When OrderUnitId=2 Then vf.SizeFt
                Else case When orderunitid=6 then vf.SizeInch else '' End End End as ItemName
                ,ocd.IFINISHEDID as Item_finished_id,
                Replace(convert(nvarchar(11),RP.Targetdate,106),' ','-') as TargetDate,
                Replace(convert(nvarchar(11),RP.RevisedDate,106),' ','-') as RevisedDate,Replace(convert(nvarchar(11),RP.actualdate,106),' ','-')as ActualDate,RP.Remark,RP.Item_finished_id as RPFinishedid 
                from ORDER_CONSUMPTION_DETAIL OCD inner join PROCESS_NAME_MASTER PNM on
                OCD.PROCESSID=PNM.PROCESS_NAME_ID
                inner join OrderDetail OD on OD.OrderId=Ocd.ORDERID and od.OrderDetailId=ocd.ORDERDETAILID
                inner join V_FinishedItemDetail vf on vf.ITEM_FINISHED_ID=OCD.IFINISHEDID
                left join  OrderRawMaterialProcurement RP on Rp.orderid=OD.OrderId and RP.orderdetailid=OD.OrderDetailId
                and OCD.IFINISHEDID=RP.Item_finished_id
                Where pnm.PROCESS_NAME='PURCHASE' and od.orderid=" + DDOrderNo.SelectedValue + " and od.orderdetailid=" + Orderdetailid;

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVRaw.DataSource = ds;
        GVRaw.DataBind();
        ds.Dispose();
    }
    protected void chkRawMat_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRawMat.Checked == true)
        {
            divRawmat.Visible = true;
        }
        else
        {
            divRawmat.Visible = false;
        }
    }
    protected void chkItem_CheckedChanged(object sender, EventArgs e)
    {

        //
        int j = 0;
        for (int i = 0; i < GVOrderDetail.Rows.Count; i++)
        {
            CheckBox chkitem = ((CheckBox)GVOrderDetail.Rows[i].FindControl("ChkItem"));
            if (chkitem.Checked == true)
            {
                j = j + 1;
                if (j > 1)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "chk", "alert('You can select only one checkbox....');", true);
                    chkitem.Checked = false;
                    return;
                }
            }
        }
        //get item_finishedid       
        CheckBox chk = (CheckBox)sender;

        string order_detail_id = "0";
        if (chk.Checked == true)
        {
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;
            order_detail_id = ((Label)GVOrderDetail.Rows[gvr.RowIndex].FindControl("lblorderdetailid")).Text;

        }
        fillrawprocurement(order_detail_id);
        FillProductionPlan(order_detail_id);
        FillInspectionPlan(order_detail_id);
    }
    protected void FillProductionPlan(string Orderdetailid)
    {
        string str;
        DataSet ds;

        if (Session["VarCompanyNo"].ToString() == "27")
        {
            str = @"select distinct PNM.PROCESS_NAME_ID,pnm.PROCESS_NAME,
                replace(convert(nvarchar(11),opt.targetdate,106),' ','-') as Targetdate,
                replace(convert(nvarchar(11),opt.RevisedDate,106),' ','-') as RevisedDate,
                replace(convert(nvarchar(11),opt.ActualDate,106),' ','-') as ActualDate,
                opt.remark,opt.Processid as ptnaprocessid
                from PROCESS_NAME_MASTER PNM               
                left join orderProductionTna OPT on opt.orderid=" + DDOrderNo.SelectedValue + " ANd opt.orderdetailid=" + Orderdetailid + @"   and OPT.PROCESSID=PNM.Process_Name_ID
                Where pnm.PROCESS_NAME in('WEAVING','DYEING','LATEXING','BACKING','BINDING','FINISHING')";
        }
        else
        {
            str = @"select distinct PNM.PROCESS_NAME_ID,pnm.PROCESS_NAME,
                replace(convert(nvarchar(11),opt.targetdate,106),' ','-') as Targetdate,
                replace(convert(nvarchar(11),opt.RevisedDate,106),' ','-') as RevisedDate,
                replace(convert(nvarchar(11),opt.ActualDate,106),' ','-') as ActualDate,
                opt.remark,opt.Processid as ptnaprocessid
                 from ORDER_CONSUMPTION_DETAIL OCD inner join PROCESS_NAME_MASTER PNM on
                OCD.PROCESSID=PNM.PROCESS_NAME_ID
                inner join OrderDetail OD on OD.OrderId=Ocd.ORDERID and od.OrderDetailId=ocd.ORDERDETAILID
                left join orderProductionTna OPT on opt.orderid=od.OrderId and opt.orderdetailid=od.OrderDetailId
                and ocd.PROCESSID=opt.Processid
                Where pnm.PROCESS_NAME<>'PURCHASE' and od.OrderId=" + DDOrderNo.SelectedValue + " and od.orderdetailid=" + Orderdetailid;
        }

       
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVProduction.DataSource = ds;
        GVProduction.DataBind();
    }
    protected void FillInspectionPlan(string Orderdetailid)
    {
        string str;
        DataSet ds;
        str = @"select  ISp.id,ISP.DateName as InspectionType,replace(convert(nvarchar(11),OIT.targetdate,106),' ','-') as Targetdate,
                replace(convert(nvarchar(11),OIT.RevisedDate,106),' ','-') as RevisedDate,
                replace(convert(nvarchar(11),OIT.ActualDate,106),' ','-') as ActualDate,OIT.remark,OIT.inspectionId as inspectionIdtna
                from InspectionDateMaster  ISP
                left join  orderInspectionTna OIT on oit.inspectionId=isp.ID
                And oit.orderid=" + DDOrderNo.SelectedValue + " and oit.orderdetailid=" + Orderdetailid;
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVInspection.DataSource = ds;
        GVInspection.DataBind();
    }

    protected void ChkProduction_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkProduction.Checked == true)
        {
            divProduction.Visible = true;
        }
        else
        {
            divProduction.Visible = false;
        }
    }

    protected void chkinspection_CheckedChanged(object sender, EventArgs e)
    {
        if (chkinspection.Checked == true)
        { divinsp.Visible = true; }
        else
        { divinsp.Visible = false; }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        var sendmsg = 1;
        SqlConnection Con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (Con.State == ConnectionState.Closed)
        {
            Con.Open();
        }
        SqlTransaction Tran = Con.BeginTransaction();
        try
        {
            //Declare Table RawMatProcurement
            DataTable dtRawProcurement = new DataTable();
            System.Data.SqlTypes.SqlDateTime getdate = SqlDateTime.Null;
            dtRawProcurement.Columns.Add("Item_finished_id", typeof(String));
            dtRawProcurement.Columns.Add("TargetDate", typeof(DateTime));
            dtRawProcurement.Columns.Add("RevisedDate", typeof(DateTime));
            dtRawProcurement.Columns.Add("ActualDate", typeof(DateTime));
            dtRawProcurement.Columns.Add("Remark", typeof(string));

            //Loop for Rawmaterial Procurement
            for (int i = 0; i < GVRaw.Rows.Count; i++)
            {
                CheckBox ChkrawmatProcurement = ((CheckBox)GVRaw.Rows[i].FindControl("Chkrawmat"));
                if (ChkrawmatProcurement.Checked == true)
                {
                    DataRow drRawProcuement = dtRawProcurement.NewRow();
                    Label lblitem_Finished_id = ((Label)GVRaw.Rows[i].FindControl("lblfinishedid"));
                    TextBox txtrawtargetdate = ((TextBox)GVRaw.Rows[i].FindControl("txtrawtargetdate"));
                    TextBox txtrawreviseddate = ((TextBox)GVRaw.Rows[i].FindControl("txtrawreviseddate"));
                    TextBox txtrawactualdate = ((TextBox)GVRaw.Rows[i].FindControl("txtrawactualdate"));
                    TextBox txtrawremark = ((TextBox)GVRaw.Rows[i].FindControl("txtrawremark"));
                    drRawProcuement["Item_Finished_id"] = lblitem_Finished_id.Text;
                    if (txtrawtargetdate.Text != "")
                    {
                        drRawProcuement["TargetDate"] = DateTime.Parse(txtrawtargetdate.Text);
                    }
                    if (txtrawreviseddate.Text != "")
                    {
                        drRawProcuement["Reviseddate"] = DateTime.Parse(txtrawreviseddate.Text);
                    }
                    if (txtrawactualdate.Text != "")
                    {
                        drRawProcuement["ActualDate"] = DateTime.Parse(txtrawactualdate.Text);
                    }
                    drRawProcuement["Remark"] = txtrawremark.Text;
                    dtRawProcurement.Rows.Add(drRawProcuement);
                }
            }
            //end of Raw material Procurement

            //start Production Tna
            DataTable dtProductionTna = new DataTable();
            dtProductionTna.Columns.Add("Processid", typeof(int));
            dtProductionTna.Columns.Add("TargetDate", typeof(DateTime));
            dtProductionTna.Columns.Add("RevisedDate", typeof(DateTime));
            dtProductionTna.Columns.Add("ActualDate", typeof(DateTime));
            dtProductionTna.Columns.Add("Remark", typeof(string));
            //Loop For Production TNA
            for (int i = 0; i < GVProduction.Rows.Count; i++)
            {
                CheckBox ChkProcess = ((CheckBox)GVProduction.Rows[i].FindControl("chkprod"));
                if (ChkProcess.Checked == true)
                {
                    DataRow drproductionTna = dtProductionTna.NewRow();
                    Label lblProcessid = ((Label)GVProduction.Rows[i].FindControl("lblprocessid"));
                    TextBox txtprodtargetdate = ((TextBox)GVProduction.Rows[i].FindControl("txtprodtargetdate"));
                    TextBox txtprodreviseddate = ((TextBox)GVProduction.Rows[i].FindControl("txtprodreviseddate"));
                    TextBox txtprodactualdate = ((TextBox)GVProduction.Rows[i].FindControl("txtprodactualdate"));
                    TextBox txtprodremark = ((TextBox)GVProduction.Rows[i].FindControl("txtprodremark"));
                    drproductionTna["Processid"] = lblProcessid.Text;
                    if (txtprodtargetdate.Text != "")
                    {
                        drproductionTna["targetdate"] = DateTime.Parse(txtprodtargetdate.Text);
                    }
                    if (txtprodreviseddate.Text != "")
                    {
                        drproductionTna["RevisedDate"] = DateTime.Parse(txtprodreviseddate.Text);
                    }
                    if (txtprodactualdate.Text != "")
                    {
                        drproductionTna["actualdate"] = DateTime.Parse(txtprodactualdate.Text);
                    }
                    drproductionTna["Remark"] = txtprodremark.Text;
                    dtProductionTna.Rows.Add(drproductionTna);
                }
            }
            //End
            //Start Inspection TNA
            DataTable dtinspectionTna = new DataTable();
            dtinspectionTna.Columns.Add("Inspectionid", typeof(int));
            dtinspectionTna.Columns.Add("TargetDate", typeof(DateTime));
            dtinspectionTna.Columns.Add("RevisedDate", typeof(DateTime));
            dtinspectionTna.Columns.Add("ActualDate", typeof(DateTime));
            dtinspectionTna.Columns.Add("Remark", typeof(string));


            for (int i = 0; i < GVInspection.Rows.Count; i++)
            {
                CheckBox Chkinsp = ((CheckBox)GVInspection.Rows[i].FindControl("chkinsp"));
                if (Chkinsp.Checked == true)
                {
                    DataRow drinspectionTna = dtinspectionTna.NewRow();
                    Label lblinspectionId = ((Label)GVInspection.Rows[i].FindControl("lblinspection"));
                    TextBox txtInspectiontargetdate = ((TextBox)GVInspection.Rows[i].FindControl("txtInspectiontargetdate"));
                    TextBox txtInspectionreviseddate = ((TextBox)GVInspection.Rows[i].FindControl("txtInspectionreviseddate"));
                    TextBox txtInspectionactualdate = ((TextBox)GVInspection.Rows[i].FindControl("txtInspectionactualdate"));
                    TextBox txtInspectionremark = ((TextBox)GVInspection.Rows[i].FindControl("txtInspectionremark"));
                    drinspectionTna["Inspectionid"] = lblinspectionId.Text;
                    if (txtInspectiontargetdate.Text != "")
                    {
                        drinspectionTna["targetdate"] = DateTime.Parse(txtInspectiontargetdate.Text);
                    }
                    if (txtInspectionreviseddate.Text != "")
                    {
                        drinspectionTna["RevisedDate"] = DateTime.Parse(txtInspectionreviseddate.Text);
                    }
                    if (txtInspectionactualdate.Text != "")
                    {
                        drinspectionTna["actualdate"] = DateTime.Parse(txtInspectionactualdate.Text);
                    }
                    drinspectionTna["Remark"] = txtInspectionremark.Text;
                    dtinspectionTna.Rows.Add(drinspectionTna);
                }
            }
            //End
            if (dtRawProcurement.Rows.Count > 0 || dtProductionTna.Rows.Count > 0 || dtinspectionTna.Rows.Count > 0)
            {
                string orderdetailid = "";
                for (int i = 0; i < GVOrderDetail.Rows.Count; i++)
                {
                    CheckBox chkitem = ((CheckBox)GVOrderDetail.Rows[i].FindControl("ChkItem"));
                    if (chkitem.Checked == true)
                    {
                        Label lblorderdetailid = ((Label)GVOrderDetail.Rows[i].FindControl("lblorderdetailid"));
                        orderdetailid = lblorderdetailid.Text;
                    }
                }
                SqlParameter[] array = new SqlParameter[7];
                array[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
                array[1] = new SqlParameter("@orderdetailid", orderdetailid);
                array[2] = new SqlParameter("@userid", Session["varuserid"]);
                array[3] = new SqlParameter("@OrderRawmatProcurement", dtRawProcurement);
                array[4] = new SqlParameter("@OrderProductionTna", dtProductionTna);
                array[5] = new SqlParameter("@OrderInspectionTna", dtinspectionTna);
                array[6] = new SqlParameter("@msg", SqlDbType.VarChar, 200);
                array[6].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveTNA", array);
                Tran.Commit();

                if (array[6].Value.ToString() != "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "msg", "alert('" + array[6].Value.ToString() + "');", true);
                    sendmsg = 0;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('TNA Saved Successfully....');", true);
                    //sendmsg = 0;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('Any checkbox is not checked for TNA....');", true);
                Tran.Commit();
                sendmsg = 0;


            }
            //Send Message

            if (sendmsg == 1)
            {
                //Message
                string Number = "", From = "";
                switch (Session["varcompanyNo"].ToString())
                {
                    case "12":
                        //string str = "select Om.orderid,case When " + Session["varcompanyNo"] + "=6 Then CI.Customercode Else CI.CompanyName End as CustomerName,OM.customerorderno,replace(CONVERT(nvarchar(11),OM.dispatchdate,106),' ','-') as dispatchdate,Sum(QtyRequired) as Qty,Round(Sum(OD.Amount),2) as Amount,dbo.F_GetOrderItem(OM.orderid) as Product,isnull(c.CurrencyName,0) as CurrencyName from ordermaster OM inner join OrderDetail OD on OM.OrderId=OD.OrderId inner join Customerinfo ci on OM.customerid=CI.customerid left join currencyinfo c on c.currencyid=od.CurrencyId where OM.orderid=" + ViewState["orderid"] + " group by CI.customercode,CI.CompanyName,OM.customerorderno,OM.DispatchDate,OM.orderid,c.CurrencyName";
                        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        switch (Session["varcompanyNo"].ToString())
                        {
                            case "12":
                                //Number = "8447281984";
                                Number = "9717331705,8756266111,7065029921";
                                //From = Session["UserName"].ToString();
                                From = "";
                                break;
                        }
                        string message = "The TNA for Customer- " + DDCustomer.SelectedItem.Text + " and order No.- " + DDOrderNo.SelectedItem.Text + " has been updated by " + Session["username"] + ".";
                        UtilityModule.SendMessage(Number, message, Convert.ToInt16(Session["varcompanyNo"]), From: From);
                        //}
                        break;
                }
            }
        }

        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "Err", "alert(" + ex.Message + ");", true);

        }
        finally
        {
            Con.Dispose();
            Con.Close();
        }
    }
    protected void GVRaw_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblrpfinishedid = ((Label)e.Row.FindControl("lblrpfinishedid"));
            if (lblrpfinishedid.Text != "")
            {
                CheckBox chkraw = ((CheckBox)e.Row.FindControl("Chkrawmat"));
                chkraw.Checked = true;
                TextBox txtrawtargetdate = ((TextBox)e.Row.FindControl("txtrawtargetdate"));
                if (txtrawtargetdate.Text != "")
                {
                    txtrawtargetdate.Enabled = false;
                    txtrawtargetdate.BackColor = Color.LightGreen;
                }
            }
        }
    }
    protected void GVProduction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblptnaprocessid = ((Label)e.Row.FindControl("lblptnaprocessid"));
            if (lblptnaprocessid.Text != "")
            {
                CheckBox chkprod = ((CheckBox)e.Row.FindControl("chkprod"));
                chkprod.Checked = true;
                TextBox txtprodtargetdate = ((TextBox)e.Row.FindControl("txtprodtargetdate"));
                if (txtprodtargetdate.Text != "")
                {
                    txtprodtargetdate.Enabled = false;
                    txtprodtargetdate.BackColor = Color.LightGreen;
                }
            }
        }
    }
    protected void GVInspection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblinspectionidtna = ((Label)e.Row.FindControl("lblinspectionidtna"));
            if (lblinspectionidtna.Text != "")
            {
                CheckBox chkinsp = ((CheckBox)e.Row.FindControl("chkinsp"));
                chkinsp.Checked = true;
            }
        }
    }
    protected void GVOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRawtnaorderdetailid = ((Label)e.Row.FindControl("lblRawtnaorderdetailid"));
            if (lblRawtnaorderdetailid.Text != "")
            {
                e.Row.BackColor = Color.LightGreen;
            }
        }
    }
    private void OrderTnaExcelReport(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");
            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            //sht.PageSetup.AdjustTo(90);
            sht.PageSetup.FitToPages(1, 1);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;
            //sht.PageSetup.VerticalDpi = 300;
            //sht.PageSetup.HorizontalDpi = 300;
            sht.PageSetup.Margins.Top = 0.2;
            sht.PageSetup.Margins.Bottom = 0.2;
            sht.PageSetup.Margins.Right = 0.2;
            sht.PageSetup.Margins.Left = 0.2;
            sht.PageSetup.Margins.Header = 0.2;
            sht.PageSetup.Margins.Footer = 0.2;
            //sht.Style.Font.FontName = "Cambria";
            sht.PageSetup.SetScaleHFWithDocument();
            sht.PageSetup.CenterHorizontally = true;

            sht.Column("A").Width = 9.33;
            sht.Column("B").Width = 9.56;
            sht.Column("C").Width = 17.22;
            sht.Column("D").Width = 12.11;
            sht.Column("E").Width = 16.10;
            sht.Column("F").Width = 5.78;
            sht.Column("G").Width = 7.11;
            sht.Column("H").Width = 13.89;
            sht.Column("I").Width = 7.89;
            sht.Column("J").Width = 12.33;
            sht.Column("K").Width = 11.33;
            sht.Column("L").Width = 9.22;
            sht.Column("M").Width = 12.33;
            sht.Column("N").Width = 11.33;
            sht.Column("O").Width = 9.22;
            sht.Column("P").Width = 12.33;
            sht.Column("Q").Width = 11.33;
            sht.Column("R").Width = 9.22;
            sht.Column("S").Width = 12.33;
            sht.Column("T").Width = 11.33;
            sht.Column("U").Width = 9.22;
            sht.Column("V").Width = 12.33;
            sht.Column("W").Width = 11.33;
            sht.Column("X").Width = 9.22;
            sht.Column("Y").Width = 12.33;
            sht.Column("Z").Width = 11.33;
            sht.Column("AA").Width = 9.22;
            sht.Column("AB").Width = 12.33;

            sht.Row(1).Height = 22.8;

            int row = 0;

            //******************

            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            sht.Range("A1:AB1").Style.Font.FontName = "Arial";
            sht.Range("A1:AB1").Style.Font.FontSize = 12;
            sht.Range("A1:AB1").Style.Font.Bold = true;
            sht.Range("A1:AB1").Style.Alignment.SetWrapText();
            sht.Range("A1:AB1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:AB1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A1:AB1").Merge();

            sht.Range("C2").Value = "TNA CHART";
            sht.Range("C2:G2").Style.Font.FontName = "Arial";
            sht.Range("C2:G2").Style.Font.FontSize = 12;
            sht.Range("C2:G2").Style.Font.Bold = true;
            sht.Range("C2:G2").Style.Alignment.SetWrapText();
            sht.Range("C2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("C2:G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("C2:G2").Merge();

            sht.Range("C3").Value = "FILLED DATE:"+ " "+ds.Tables[0].Rows[0]["TNAPlanDate"];
            sht.Range("C3:G3").Style.Font.FontName = "Arial";
            sht.Range("C3:G3").Style.Font.FontSize = 12;
            sht.Range("C3:G3").Style.Font.Bold = true;
            sht.Range("C3:G3").Style.Alignment.SetWrapText();
            sht.Range("C3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("C3:G3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("C3:G3").Merge();

            using (var a = sht.Range("A1:AB1"))
            {
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("A1:A3"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("AB1:AB3"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("A3:AB3"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Range("A4").Value = "";
            sht.Range("A4:AB4").Style.Alignment.SetWrapText();
            sht.Range("A4:AB4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A4:AB4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A4:AB4").Merge();
           
            using (var a = sht.Range("A4:A4"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("AB4:AB4"))
            {
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            }
            using (var a = sht.Range("A4:AB4"))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            sht.Row(5).Height = 50.5;
            sht.Row(6).Height = 34.5;

            sht.Range("A5").Value = "BUYER";
            sht.Range("A5:A5").Style.Font.FontName = "Courier New";
            sht.Range("A5:A5").Style.Font.FontSize = 12;
            sht.Range("A5:A5").Style.Font.Bold = true;
            sht.Range("A5:A5").Merge();
            sht.Range("A5:A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A5:A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A5:A5").Style.Alignment.SetWrapText();

            sht.Range("B5").Value = "PO#";
            sht.Range("B5:B5").Style.Font.FontName = "Courier New";
            sht.Range("B5:B5").Style.Font.FontSize = 12;
            sht.Range("B5:B5").Style.Font.Bold = true;
            sht.Range("B5:B5").Merge();
            sht.Range("B5:B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("B5:B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("B5:B5").Style.Alignment.SetWrapText();

            sht.Range("C5").Value = "STYLE";
            sht.Range("C5:C5").Style.Font.FontName = "Courier New";
            sht.Range("C5:C5").Style.Font.FontSize = 12;
            sht.Range("C5:C5").Style.Font.Bold = true;
            sht.Range("C5:C5").Merge();
            sht.Range("C5:C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("C5:C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("C5:C5").Style.Alignment.SetWrapText();

            sht.Range("D5").Value = "COLOUR";
            sht.Range("D5:D5").Style.Font.FontName = "Courier New";
            sht.Range("D5:D5").Style.Font.FontSize = 12;
            sht.Range("D5:D5").Style.Font.Bold = true;
            sht.Range("D5:D5").Merge();
            sht.Range("D5:D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D5:D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("D5:D5").Style.Alignment.SetWrapText();

            sht.Range("E5").Value = "SIZE";
            sht.Range("E5:E5").Style.Font.FontName = "Courier New";
            sht.Range("E5:E5").Style.Font.FontSize = 12;
            sht.Range("E5:E5").Style.Font.Bold = true;
            sht.Range("E5:E5").Merge();
            sht.Range("E5:E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("E5:E5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("E5:E5").Style.Alignment.SetWrapText();

            sht.Range("F5").Value = "SHAPE";
            sht.Range("F5:F5").Style.Font.FontName = "Courier New";
            sht.Range("F5:F5").Style.Font.FontSize = 12;
            sht.Range("F5:F5").Style.Font.Bold = true;
            sht.Range("F5:F5").Merge();
            sht.Range("F5:F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("F5:F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("F5:F5").Style.Alignment.SetWrapText();

            sht.Range("G5").Value = "ORDER QTY";
            sht.Range("G5:G5").Style.Font.FontName = "Courier New";
            sht.Range("G5:G5").Style.Font.FontSize = 12;
            sht.Range("G5:G5").Style.Font.Bold = true;
            sht.Range("G5:G5").Merge();
            sht.Range("G5:G5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("G5:G5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("G5:G5").Style.Alignment.SetWrapText();

            sht.Range("H5").Value = "AREA IN" +" "+ ds.Tables[0].Rows[0]["UNITName"];
            sht.Range("H5:H5").Style.Font.FontName = "Courier New";
            sht.Range("H5:H5").Style.Font.FontSize = 12;
            sht.Range("H5:H5").Style.Font.Bold = true;
            sht.Range("H5:H5").Merge();
            sht.Range("H5:H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("H5:H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("H5:H5").Style.Alignment.SetWrapText();

            sht.Range("I5").Value = "VENDOR NAME";
            sht.Range("I5:I5").Style.Font.FontName = "Courier New";
            sht.Range("I5:I5").Style.Font.FontSize = 12;
            sht.Range("I5:I5").Style.Font.Bold = true;
            sht.Range("I5:I5").Merge();
            sht.Range("I5:I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("I5:I5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("I5:I5").Style.Alignment.SetWrapText();

            sht.Range("J5").Value = "YARN DYEING";
            sht.Range("J5:L5").Style.Font.FontName = "Courier New";
            sht.Range("J5:L5").Style.Font.FontSize = 12;
            sht.Range("J5:L5").Style.Font.Bold = true;
            sht.Range("J5:L5").Merge();
            sht.Range("J5:L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("J5:L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("J5:L5").Style.Alignment.SetWrapText();

            sht.Range("M5").Value = "WEAVING";
            sht.Range("M5:O5").Style.Font.FontName = "Courier New";
            sht.Range("M5:O5").Style.Font.FontSize = 12;
            sht.Range("M5:O5").Style.Font.Bold = true;
            sht.Range("M5:O5").Merge();
            sht.Range("M5:O5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("M5:O5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("M5:O5").Style.Alignment.SetWrapText();

            sht.Range("P5").Value = "LATEXING";
            sht.Range("P5:R5").Style.Font.FontName = "Courier New";
            sht.Range("P5:R5").Style.Font.FontSize = 12;
            sht.Range("P5:R5").Style.Font.Bold = true;
            sht.Range("P5:R5").Merge();
            sht.Range("P5:R5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P5:R5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("P5:R5").Style.Alignment.SetWrapText();

            sht.Range("S5").Value = "BACKING";
            sht.Range("S5:U5").Style.Font.FontName = "Courier New";
            sht.Range("S5:U5").Style.Font.FontSize = 12;
            sht.Range("S5:U5").Style.Font.Bold = true;
            sht.Range("S5:U5").Merge();
            sht.Range("S5:U5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("S5:U5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("S5:U5").Style.Alignment.SetWrapText();

            sht.Range("V5").Value = "BINDING";
            sht.Range("V5:X5").Style.Font.FontName = "Courier New";
            sht.Range("V5:X5").Style.Font.FontSize = 12;
            sht.Range("V5:X5").Style.Font.Bold = true;
            sht.Range("V5:X5").Merge();
            sht.Range("V5:X5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("V5:X5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("V5:X5").Style.Alignment.SetWrapText();

            sht.Range("Y5").Value = "FINISHING";
            sht.Range("Y5:AA5").Style.Font.FontName = "Courier New";
            sht.Range("Y5:AA5").Style.Font.FontSize = 12;
            sht.Range("Y5:AA5").Style.Font.Bold = true;
            sht.Range("Y5:AA5").Merge();
            sht.Range("Y5:AA5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Y5:AA5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("Y5:AA5").Style.Alignment.SetWrapText();

            sht.Range("AB5").Value = "Ex-FACTORY CONFIRMED";
            sht.Range("AB5:AB5").Style.Font.FontName = "Courier New";
            sht.Range("AB5:AB5").Style.Font.FontSize = 12;
            sht.Range("AB5:AB5").Style.Font.Bold = true;
            sht.Range("AB5:AB5").Merge();
            sht.Range("AB5:AB5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("AB5:AB5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("AB5:AB5").Style.Alignment.SetWrapText();

            sht.Range("J6").Value = "START DATE";
            sht.Range("J6:J6").Style.Font.FontName = "Courier New";
            sht.Range("J6:J6").Style.Font.FontSize = 12;
            sht.Range("J6:J6").Style.Font.Bold = true;
            sht.Range("J6:J6").Merge();
            sht.Range("J6:J6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("J6:J6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("J6:J6").Style.Alignment.SetWrapText();

            sht.Range("K6").Value = "COMPLETION DATE";
            sht.Range("K6:K6").Style.Font.FontName = "Courier New";
            sht.Range("K6:K6").Style.Font.FontSize = 12;
            sht.Range("K6:K6").Style.Font.Bold = true;
            sht.Range("K6:K6").Merge();
            sht.Range("K6:K6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("K6:K6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("K6:K6").Style.Alignment.SetWrapText();

            sht.Range("L6").Value = "REMARK";
            sht.Range("L6:L6").Style.Font.FontName = "Courier New";
            sht.Range("L6:L6").Style.Font.FontSize = 12;
            sht.Range("L6:L6").Style.Font.Bold = true;
            sht.Range("L6:L6").Merge();
            sht.Range("L6:L6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("L6:L6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("L6:L6").Style.Alignment.SetWrapText();

            sht.Range("M6").Value = "START DATE";
            sht.Range("M6:M6").Style.Font.FontName = "Courier New";
            sht.Range("M6:M6").Style.Font.FontSize = 12;
            sht.Range("M6:M6").Style.Font.Bold = true;
            sht.Range("M6:M6").Merge();
            sht.Range("M6:M6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("M6:M6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("M6:M6").Style.Alignment.SetWrapText();

            sht.Range("N6").Value = "COMPLETION DATE";
            sht.Range("N6:N6").Style.Font.FontName = "Courier New";
            sht.Range("N6:K6").Style.Font.FontSize = 12;
            sht.Range("N6:N6").Style.Font.Bold = true;
            sht.Range("N6:N6").Merge();
            sht.Range("N6:N6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("N6:N6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("N6:N6").Style.Alignment.SetWrapText();

            sht.Range("O6").Value = "REMARK";
            sht.Range("O6:O6").Style.Font.FontName = "Courier New";
            sht.Range("O6:O6").Style.Font.FontSize = 12;
            sht.Range("O6:O6").Style.Font.Bold = true;
            sht.Range("O6:O6").Merge();
            sht.Range("O6:O6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("O6:O6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("O6:O6").Style.Alignment.SetWrapText();

            sht.Range("P6").Value = "START DATE";
            sht.Range("P6:P6").Style.Font.FontName = "Courier New";
            sht.Range("P6:P6").Style.Font.FontSize = 12;
            sht.Range("P6:P6").Style.Font.Bold = true;
            sht.Range("P6:P6").Merge();
            sht.Range("P6:P6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P6:P6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("P6:P6").Style.Alignment.SetWrapText();

            sht.Range("Q6").Value = "COMPLETION DATE";
            sht.Range("Q6:Q6").Style.Font.FontName = "Courier New";
            sht.Range("Q6:Q6").Style.Font.FontSize = 12;
            sht.Range("Q6:Q6").Style.Font.Bold = true;
            sht.Range("Q6:Q6").Merge();
            sht.Range("Q6:Q6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q6:Q6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("Q6:Q6").Style.Alignment.SetWrapText();

            sht.Range("R6").Value = "REMARK";
            sht.Range("R6:R6").Style.Font.FontName = "Courier New";
            sht.Range("R6:R6").Style.Font.FontSize = 12;
            sht.Range("R6:R6").Style.Font.Bold = true;
            sht.Range("R6:R6").Merge();
            sht.Range("R6:R6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("R6:R6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("R6:R6").Style.Alignment.SetWrapText();

            sht.Range("S6").Value = "START DATE";
            sht.Range("S6:S6").Style.Font.FontName = "Courier New";
            sht.Range("S6:S6").Style.Font.FontSize = 12;
            sht.Range("S6:S6").Style.Font.Bold = true;
            sht.Range("S6:S6").Merge();
            sht.Range("S6:S6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("S6:S6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("S6:S6").Style.Alignment.SetWrapText();

            sht.Range("T6").Value = "COMPLETION DATE";
            sht.Range("T6:T6").Style.Font.FontName = "Courier New";
            sht.Range("T6:T6").Style.Font.FontSize = 12;
            sht.Range("T6:T6").Style.Font.Bold = true;
            sht.Range("T6:T6").Merge();
            sht.Range("T6:T6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("T6:T6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("T6:T6").Style.Alignment.SetWrapText();

            sht.Range("U6").Value = "REMARK";
            sht.Range("U6:U6").Style.Font.FontName = "Courier New";
            sht.Range("U6:U6").Style.Font.FontSize = 12;
            sht.Range("U6:U6").Style.Font.Bold = true;
            sht.Range("U6:U6").Merge();
            sht.Range("U6:U6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("U6:U6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("U6:U6").Style.Alignment.SetWrapText();

            sht.Range("V6").Value = "START DATE";
            sht.Range("V6:V6").Style.Font.FontName = "Courier New";
            sht.Range("V6:V6").Style.Font.FontSize = 12;
            sht.Range("V6:V6").Style.Font.Bold = true;
            sht.Range("V6:V6").Merge();
            sht.Range("V6:V6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("V6:V6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("V6:V6").Style.Alignment.SetWrapText();

            sht.Range("W6").Value = "COMPLETION DATE";
            sht.Range("W6:W6").Style.Font.FontName = "Courier New";
            sht.Range("W6:W6").Style.Font.FontSize = 12;
            sht.Range("W6:W6").Style.Font.Bold = true;
            sht.Range("W6:W6").Merge();
            sht.Range("W6:W6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("W6:W6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("W6:W6").Style.Alignment.SetWrapText();

            sht.Range("X6").Value = "REMARK";
            sht.Range("X6:X6").Style.Font.FontName = "Courier New";
            sht.Range("X6:X6").Style.Font.FontSize = 12;
            sht.Range("X6:X6").Style.Font.Bold = true;
            sht.Range("X6:X6").Merge();
            sht.Range("X6:X6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("X6:X6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("X6:X6").Style.Alignment.SetWrapText();

            sht.Range("Y6").Value = "START DATE";
            sht.Range("Y6:Y6").Style.Font.FontName = "Courier New";
            sht.Range("Y6:Y6").Style.Font.FontSize = 12;
            sht.Range("Y6:Y6").Style.Font.Bold = true;
            sht.Range("Y6:Y6").Merge();
            sht.Range("Y6:Y6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Y6:Y6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("Y6:Y6").Style.Alignment.SetWrapText();

            sht.Range("Z6").Value = "COMPLETION DATE";
            sht.Range("Z6:Z6").Style.Font.FontName = "Courier New";
            sht.Range("Z6:Z6").Style.Font.FontSize = 12;
            sht.Range("Z6:Z6").Style.Font.Bold = true;
            sht.Range("Z6:Z6").Merge();
            sht.Range("Z6:Z6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Z6:Z6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("Z6:Z6").Style.Alignment.SetWrapText();

            sht.Range("AA6").Value = "REMARK";
            sht.Range("AA6:AA6").Style.Font.FontName = "Courier New";
            sht.Range("AA6:AA6").Style.Font.FontSize = 12;
            sht.Range("AA6:AA6").Style.Font.Bold = true;
            sht.Range("AA6:AA6").Merge();
            sht.Range("AA6:AA6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("AA6:AA6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("AA6:AA6").Style.Alignment.SetWrapText();

            using (var a = sht.Range("A5:AB6"))
            {
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            row = 7;
            int rowcount = ds.Tables[0].Rows.Count;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sht.Range("A" + row + ":AB" + row).Style.Font.FontName = "Courier New";
                sht.Range("A" + row + ":AB" + row).Style.Font.FontSize = 10;
                //sht.Range("A" + row + ":AB" + row).Style.Font.SetBold();
                sht.Range("A" + row + ":AB" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sht.Range("A" + row + ":AB" + row).Style.Alignment.SetWrapText();
                sht.Range("A" + row + ":AB" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                sht.Range("A" + row).SetValue(ds.Tables[0].Rows[i]["CustomerCode"]);
                sht.Range("B" + row).SetValue(ds.Tables[0].Rows[i]["CustomerOrderNo"]);
                sht.Range("C" + row).SetValue(ds.Tables[0].Rows[i]["DesignName"]);
                sht.Range("D" + row).SetValue(ds.Tables[0].Rows[i]["ColorName"]);
                sht.Range("E" + row).SetValue(ds.Tables[0].Rows[i]["Size"]);
                sht.Range("F" + row).SetValue(ds.Tables[0].Rows[i]["ShapeName"]);
                sht.Range("G" + row).SetValue(ds.Tables[0].Rows[i]["QtyRequired"]);
                sht.Range("H" + row).SetValue(ds.Tables[0].Rows[i]["TotalOrderArea"]);
                sht.Range("I" + row).SetValue("");

                DataView dvitemdesc = new DataView(ds.Tables[2]);
                dvitemdesc.RowFilter = "OrderId='" + ds.Tables[0].Rows[i]["OrderId"] + "' and OrderDetailId='" + ds.Tables[0].Rows[i]["OrderDetailId"] + "'";
                DataSet dsitemdesc = new DataSet();
                dsitemdesc.Tables.Add(dvitemdesc.ToTable());
                //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];

                //DataTable dtdistinctitemdesc = dsitemdesc.Tables[0].DefaultView.ToTable(true, "OrderId", "OrderDetailId", "Process_Name", "TargetDate", "RevisedDate", "ActualDate", "Remark");
                    DataTable dtdistinctitemdesc = dsitemdesc.Tables[0];
                    foreach (DataRow dritemdesc in dtdistinctitemdesc.Rows)
                    {
                        if (dritemdesc["Process_Name"].ToString() == "DYEING")
                        {
                            sht.Range("J" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("K" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("L" + row).SetValue(dritemdesc["Remark"]);
                        }                       

                        if (dritemdesc["Process_Name"].ToString() == "WEAVING")
                        {
                            sht.Range("M" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("N" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("O" + row).SetValue(dritemdesc["Remark"]);
                        }                      

                        if (dritemdesc["Process_Name"].ToString() == "LATEXING")
                        {
                            sht.Range("P" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("Q" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("R" + row).SetValue(dritemdesc["Remark"]);
                        }                       

                        if (dritemdesc["Process_Name"].ToString() == "BACKING")
                        {
                            sht.Range("S" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("T" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("U" + row).SetValue(dritemdesc["Remark"]);
                        }                       

                        if (dritemdesc["Process_Name"].ToString() == "BINDING")
                        {
                            sht.Range("V" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("W" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("X" + row).SetValue(dritemdesc["Remark"]);
                        }                       

                        if (dritemdesc["Process_Name"].ToString() == "FINISHING")
                        {
                            sht.Range("Y" + row).SetValue(dritemdesc["TargetDate"]);
                            sht.Range("Z" + row).SetValue(dritemdesc["ActualDate"]);
                            sht.Range("AA" + row).SetValue(dritemdesc["Remark"]);
                        }                       
                       
                    }

                    sht.Range("AB" + row).SetValue(ds.Tables[0].Rows[i]["DispatchDate"]);
                    using (var a = sht.Range("A" + row + ":AB" + row))
                    {
                        a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                    row = row + 1;

                //sht.Range("C" + row).Style.Alignment.SetWrapText();

            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("TNAORDERPLANREPORT_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            return;
           
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
        
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@orderid", DDOrderNo.SelectedValue);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getOrderTnaPreview", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Session["VarCompanyNo"].ToString() == "27")
            {
                OrderTnaExcelReport(ds);
            }
            else
            {
                Session["rptFileName"] = "~\\Reports\\RptOrderTna.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptOrderTna.xsd";

                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            } 

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        }
    }
}