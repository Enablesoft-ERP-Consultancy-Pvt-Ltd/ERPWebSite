using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_FrmPackingMaterialPurchaseOrder : System.Web.UI.Page
{
    string msg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["PorderpackId"] = 0;
            ViewState["PorderpackdetailId"] = 0;
            ViewState["flag"] = 0;
            ViewState["PackingCostID"] = 0;
            ViewState["DetailID"] = 0;
            ViewState["ChallanNo"] = 0;
            ViewState["ddorderno"] = 0;
            
            txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtduedate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtdeldate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            string qry = @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName from customerinfo ci 
            inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
            inner join Jobassigns JA ON OM.Orderid=JA.Orderid Order By ci.Customercode + SPACE(5)+CI.CompanyName
            select distinct e1.empid,e1.empname from empinfo e1 Where e1.MasterCompanyId=" + Session["varCompanyId"] + @"
            Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
            Select p.PaymentId,p.PaymentName from Payment p Where p.MasterCompanyId=" + Session["varCompanyId"] + @" order by PaymentName
            select t.TermId,t.TermName from Term t Where t.MasterCompanyId=" + Session["varCompanyId"] + @" order by TermName
            select tm.TransModeid,tm.TransModeName from Transmode tm Where tm.MasterCompanyId=" + Session["varCompanyId"] + @" order by TransModename";
            //;
            //select distinct ei.empid ,ei.empname from empinfo ei inner join  PurchaseIndentMaster pim on ei.empid=pim.partyid And ei.MasterCompanyId=" + Session["varCompanyId"] + @"
            //        Order By ei.empname
            DataSet ds = SqlHelper.ExecuteDataset(qry);
            UtilityModule.ConditionalComboFillWithDS(ref ddcustomercode, ds, 0, true, "Select CustomerCode");
            UtilityModule.ConditionalComboFillWithDS(ref ddempname, ds, 1, true, "Select Party");
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 2, true, "Select Comp Name");
            UtilityModule.ConditionalComboFillWithDS(ref ddpayement, ds, 3, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref dddelivery, ds, 4, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddtransprt, ds, 5, true, "Select Mode");
            //UtilityModule.ConditionalComboFillWithDS(ref ddempname, ds, 6, true, "Select Party");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            ddpayement.SelectedIndex = 1;
            ddtransprt.SelectedIndex = 1;
            dddelivery.SelectedIndex = 1;
            ddcustomercode.Focus();
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
            LblCompanyName.Text = Session["varCompanyName"].ToString();
            LblUserName.Text = Session["varusername"].ToString();
        }
    }
    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ChkEditOrder.Checked == true)
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And  om.orderid  in (select orderid from PurchaseOrderMasterPacking) Order by LocalOrder+ ' / ' +CustomerOrderNo", true, "Select OrderNo.");
        else
        {
//            string strQry = @"Select distinct OM.OrderID,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo
//                   From ORDERMASTER OM,OrderDetail OD,PACKINGCOST PC  Left Outer Join Item_Parameter_Master IPM ON PC.ProdCode_FinishedID=IPM.Item_Finished_id
//                   Where OM.Orderid=OD.Orderid And OD.Item_Finished_id=PC.Finishedid And PC.ID not in (Select distinct PACKINGCOSTID from PurchaseOrdeDetailPacking ) And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue;
//            UtilityModule.ConditionalComboFill(ref ddorderno,strQry , true, "Select OrderNo.");
            //UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And om.orderid not in (select orderid from PurchaseOrderMasterPacking)", true, "Select OrderNo.");

            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " Order By LocalOrder+ ' / ' +CustomerOrderNo ", true, "Select OrderNo.");
        }
            
        
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ddorderno"] = ddorderno.SelectedValue;
        //DataSet ds3 = new DataSet();
        //TxtOrderId.Text = ddorderno.SelectedValue;
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "select distinct empid,empname from empinfo where MasterCompanyId=" + Session["varCompanyId"] + " And  empid in(select partyid from PurchaseOrderMasterPacking where orderid=" + ddorderno.SelectedValue + ") Order By empname", true, "--Select EMP--");
            //string st3 = "select chalanno from PurchaseOrderMasterPacking where orderid=" + ddorderno.Text + "";
            //ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, st3);
            //if (ds3.Tables[0].Rows.Count > 0)
            //{
            //    //txtchalanno.Text = ds3.Tables[0].Rows[0]["chalanno"].ToString();
            //}
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "select distinct empid,empname from empinfo Where MasterCompanyId=" + Session["varCompanyId"] + " Order By empname", true, "Select Party");
           // txtchalanno.Text = "";
            //Fill_Grid_Show();
            Fill_DataGridShow();
        }
    }
    private void Fill_DataGridShow()
    {
        string str4 = "";
       // int flag;
        if (ChkEditOrder.Checked == true)
        {
            //TxtOrderId.Text = ddorderno.SelectedValue;
            str4 = @"select Om.OrderID , pd.detailid pdetail, pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,pd.height,pd.gsm,pd.Gsm2 gsm2,pd.ply,pd.pcs unit,pd.qty,pd.rate,amt,pd.pcs,isnull(pd.weight,0) weight,pd.remarks remark,
                    CASE  WHEN PD.delivery_date IS NULL THEN replace(convert(varchar(11),GETDATE(),106), ' ','-') ELSE replace(convert(varchar(11),pd.delivery_date,106), ' ','-') END as ddate, isnull(IPM.ProductCode,'')ProductCode,
                    CASE  WHEN pm.Date IS NULL THEN replace(convert(varchar(11),GETDATE(),106), ' ','-') ELSE replace(convert(varchar(11),pm.Date,106), ' ','-') END as Odate  ,
                    CASE  WHEN pm.DueDate IS NULL THEN replace(convert(varchar(11),GETDATE(),106), ' ','-') ELSE replace(convert(varchar(11),pm.DueDate,106), ' ','-') END as Duedate 
                     From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om, Item_Parameter_Master IPM
                    Where pm.pid=pd.pid and om.orderid=pm.orderid and IPM.Item_Finished_id=pd.Finishedid and pm.chalanno=" + ddchalanno.SelectedValue + " And om.CompanyId=" + ddCompName.SelectedValue;
            ViewState["flag"] = 1;
        }
        else
        {
//            str4 = @"Select  pc.id pdetail, PC.ID SrNo,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo,Case When PackingType=1 Then 'INNER' Else Case When PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,
//                   PC.Length,PC.Width,PC.Height,PC.GSM1 GSM,pc.GSM2 GSM2,(ply+craft) PlY,'PCS' Unit, OD.QtyRequired/PC.Pcs Qty,netcost Rate,OD.QtyRequired/PC.Pcs*(netcost) Amt,PC.Pcs,0 weight, ' ' remark,replace(convert(varchar(11),getdate(),106), ' ','-') as ddate, isnull(IPM.ProductCode,'')ProductCode,
//                   replace(convert(varchar(11),GETDATE(),106), ' ','-') Odate, replace(convert(varchar(11),GETDATE(),106), ' ','-') Duedate
//                   From ORDERMASTER OM,OrderDetail OD,PACKINGCOST PC  Left Outer Join Item_Parameter_Master IPM ON PC.ProdCode_FinishedID=IPM.Item_Finished_id
//                   Where OM.Orderid=OD.Orderid And OD.Item_Finished_id=PC.Finishedid And PC.PackingType in (1,2,3) And  PC.ID not in (Select distinct POP.PACKINGCOSTID from PurchaseOrdeDetailPacking POP,PurchaseOrderMasterPacking PKM,V_PackingOrderIssuedQty v  
//                   where POP.PID=PKM.PID  AND V.ID=POP.PACKINGCOSTID And PKM.Orderid=" + ddorderno.SelectedValue + " group by POP.PACKINGCOSTID having Sum(OrderQty) > sum(IssuedQty)) And OM.Orderid=" + ddorderno.SelectedValue + " Order By PRODUCTCODE";
            str4 = @"Select Om.OrderID ,  pc.id pdetail, PC.ID SrNo,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo,Case When PackingType=1 Then 'INNER' Else Case When PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,
                    PC.Length,PC.Width,PC.Height,PC.GSM1 GSM,pc.GSM2 GSM2,(ply+craft) PlY,'PCS' Unit, OD.QtyRequired/PC.Pcs Qty,netcost Rate,OD.QtyRequired/PC.Pcs*(netcost) Amt,PC.Pcs,0 weight, ' ' remark,replace(convert(varchar(11),getdate(),106), ' ','-') as ddate, isnull(IPM.ProductCode,'')ProductCode,
                    replace(convert(varchar(11),GETDATE(),106), ' ','-') Odate, replace(convert(varchar(11),GETDATE(),106), ' ','-') Duedate
                    From ORDERMASTER OM,OrderDetail OD,PACKINGCOST PC  Left Outer Join Item_Parameter_Master IPM ON PC.FinishedID=IPM.Item_Finished_id
                    Where OM.Orderid=OD.Orderid And OD.Item_Finished_id=PC.Finishedid And PC.PackingType in (1,2,3) 
                    And  PC.ID  in (Select ID from V_PackingOrderIssuedQty  where Orderid=" + ViewState["ddorderno"].ToString() + @"  group by OrderID,ID having Sum(OrderQty)> sum(IssuedQty))
                     And OM.Orderid=" + ViewState["ddorderno"].ToString() +" Order By PRODUCTCODE";
            ViewState["flag"] = 0;
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str4);
        GDVSHOWORDER.DataSource = Ds;
        GDVSHOWORDER.DataBind();
        //DGSHOWDATA.DataSource = Ds;
        //DGSHOWDATA.DataBind();
        //if (DGSHOWDATA.Rows.Count > 0)
        //    DGSHOWDATA.Visible = true;
        //else
        //    DGSHOWDATA.Visible = false;
        //if (ViewState["flag"].ToString() == "1")
        //{
        //    DGSHOWDATA.Columns[16].Visible = true;
        //}
        ViewState["flag"] = 0;
    }

    private void Fill_Grid_Show()
    {
        string str4 = "";
       // int flag;
        //if (ChkEditOrder.Checked == true)
        //{
            //TxtOrderId.Text = ddorderno.SelectedValue;
        str4 = @"select  pd.detailid pdetail, pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,pd.height,pd.gsm,pd.Gsm2 gsm2,pd.ply,pd.pcs unit,pd.qty,pd.rate,amt,pd.pcs,isnull(pd.weight,0) weight,pd.remarks remark,
                    CASE  WHEN PD.delivery_date IS NULL THEN replace(convert(varchar(11),GETDATE(),106), ' ','-') ELSE replace(convert(varchar(11),pd.delivery_date,106), ' ','-') END as ddate, isnull(IPM.ProductCode,'')ProductCode
                     From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om, Item_Parameter_Master IPM
                   Where pm.pid=pd.pid and om.orderid=pm.orderid and IPM.Item_Finished_id=pd.Finishedid and pm.chalanno=" + ViewState["ChallanNo"].ToString() + " And om.CompanyId=" + ddCompName.SelectedValue;
            ViewState["flag"] = 1;
//        }
//        else
//        {
//            str4 = @"Select  pc.id pdetail, PC.ID SrNo,OM.LocalOrder+' / '+OM.CustomerOrderNo OrderNo,Case When PackingType=1 Then 'INNER' Else Case When PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,
//                   PC.Length,PC.Width,PC.Height,PC.GSM1 GSM,pc.GSM2 GSM2,(ply+craft) PlY,'PCS' Unit, OD.QtyRequired/PC.Pcs Qty,netcost Rate,OD.QtyRequired/PC.Pcs*(netcost) Amt,PC.Pcs,0 weight, ' ' remark,replace(convert(varchar(11),getdate(),106), ' ','-') as ddate, isnull(IPM.ProductCode,'')ProductCode
//                   From ORDERMASTER OM,OrderDetail OD,PACKINGCOST PC  Left Outer Join Item_Parameter_Master IPM ON PC.ProdCode_FinishedID=IPM.Item_Finished_id
//                   Where OM.Orderid=OD.Orderid And OD.Item_Finished_id=PC.Finishedid And PC.PackingType in (1,2,3) And OM.Orderid=" + ddorderno.SelectedValue + " Order By PRODUCTCODE";
//            ViewState["flag"] = 0;
//        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str4);
        //GDVSHOWORDER.DataSource = Ds;
        //GDVSHOWORDER.DataBind();
        DGSHOWDATA.DataSource = Ds;
        DGSHOWDATA.DataBind();
        if (DGSHOWDATA.Rows.Count > 0)
            DGSHOWDATA.Visible = true;
        else
            DGSHOWDATA.Visible = false;
       
            DGSHOWDATA.Columns[16].Visible = true;
       
        ViewState["flag"] = 0;
        
    }

    protected void txtfrieghtrate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        #region
        //if (ddCompName.SelectedIndex == 0 || ddcustomercode.SelectedIndex == 0 || ddorderno.SelectedIndex == 0 || ddempname.SelectedIndex == 0)
        //    LblErrorMessage.Visible = true;
        //else
        //    LblErrorMessage.Visible = false;
        //if (LblErrorMessage.Visible == false)
        //{
        //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //    con.Open();
        //    SqlTransaction tran = con.BeginTransaction();
        //    ViewState["PorderpackdetailId"] = 0;
        //    ViewState["PorderpackId"] = 0;
        //    try
        //    {
        //        SqlParameter[] arr = new SqlParameter[43];
        //        arr[0] = new SqlParameter("@Pid", SqlDbType.Int);
        //        arr[1] = new SqlParameter("@Companyid", SqlDbType.Int);
        //        arr[2] = new SqlParameter("@Partyid", SqlDbType.Int);
        //        arr[3] = new SqlParameter("@Orderid", SqlDbType.Int);
        //        arr[4] = new SqlParameter("@Chalanno", SqlDbType.NVarChar, 50);
        //        arr[5] = new SqlParameter("@Date", SqlDbType.DateTime);
        //        arr[6] = new SqlParameter("@Userid", SqlDbType.Int);
        //        arr[7] = new SqlParameter("@Duedate", SqlDbType.DateTime);
        //        arr[8] = new SqlParameter("@Destination", SqlDbType.NVarChar, 50);
        //        arr[9] = new SqlParameter("@PayementTermId", SqlDbType.Int);
        //        arr[10] = new SqlParameter("@Insurence", SqlDbType.NVarChar, 50);
        //        arr[11] = new SqlParameter("@Freight", SqlDbType.NVarChar, 50);
        //        arr[12] = new SqlParameter("@Feightrate", SqlDbType.Float);
        //        arr[13] = new SqlParameter("@TranportModeId", SqlDbType.Int);
        //        arr[14] = new SqlParameter("@DeliverTermid", SqlDbType.Int);
        //        arr[15] = new SqlParameter("@Formno", SqlDbType.NVarChar, 50);
        //        arr[16] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 150);
        //        arr[17] = new SqlParameter("@ExciseDuty", SqlDbType.Float);
        //        arr[18] = new SqlParameter("@EduCess", SqlDbType.Float);
        //        arr[19] = new SqlParameter("@CST", SqlDbType.Float);
        //        arr[20] = new SqlParameter("@NetAmount", SqlDbType.Float);
        //        arr[21] = new SqlParameter("@Agentname", SqlDbType.NVarChar, 50);
        //        arr[22] = new SqlParameter("@Packingcharge", SqlDbType.Int);
        //        arr[23] = new SqlParameter("@Finishedid", SqlDbType.Int);
        //        arr[24] = new SqlParameter("@Packingtype", SqlDbType.Int);
        //        arr[25] = new SqlParameter("@Packingcostid", SqlDbType.Int);
        //        arr[26] = new SqlParameter("@ProdCode_FinishedID", SqlDbType.Int);
        //        arr[27] = new SqlParameter("@Length", SqlDbType.Float);
        //        arr[28] = new SqlParameter("@Width", SqlDbType.Float);
        //        arr[29] = new SqlParameter("@Height", SqlDbType.Float);
        //        arr[30] = new SqlParameter("@Gsm", SqlDbType.Float);
        //        arr[31] = new SqlParameter("@Ply", SqlDbType.Float);
        //        arr[32] = new SqlParameter("@Qty", SqlDbType.Int);
        //        arr[33] = new SqlParameter("@Rate", SqlDbType.Float);
        //        arr[34] = new SqlParameter("@Amt", SqlDbType.Float);
        //        arr[35] = new SqlParameter("@Pcs", SqlDbType.Int);
        //        arr[36] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
        //        arr[37] = new SqlParameter("@Detailid", SqlDbType.Int);
        //        arr[38] = new SqlParameter("@pqty", SqlDbType.Int);
        //        arr[39] = new SqlParameter("@weight", SqlDbType.Float);
        //        arr[40] = new SqlParameter("@remark", SqlDbType.NVarChar, 250);
        //        arr[41] = new SqlParameter("@GSM2", SqlDbType.Float);
        //        arr[42] = new SqlParameter("@Delivery_Date", SqlDbType.DateTime);
        //        int n = DGSHOWDATA.Rows.Count;
        //        for (int i = 0; i < n; i++)
        //        {
        //            string ab = ((Label)DGSHOWDATA.Rows[i].FindControl("lbldetailid")).Text;
        //            arr[0].Direction = ParameterDirection.InputOutput;
        //            if (ChkEditOrder.Checked == true)
        //                arr[0].Value = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select pid from PurchaseOrdeDetailPacking where detailid=" + ab + "");
        //            else
        //                arr[0].Value = ViewState["PorderpackId"];
        //            arr[1].Value = ddCompName.SelectedValue;
        //            arr[2].Value = ddempname.SelectedValue;
        //            arr[3].Value = ddorderno.SelectedValue;
        //            arr[4].Direction = ParameterDirection.InputOutput;
        //            arr[4].Value = txtchalanno.Text;
        //            arr[5].Value = txtdate.Text;
        //            arr[6].Value = "1";
        //            arr[7].Value = txtduedate.Text;
        //            arr[8].Value = txtdestination.Text.ToUpper();
        //            arr[9].Value = ddpayement.SelectedIndex > 0 ? ddpayement.SelectedValue : "0";
        //            arr[10].Value = txtinsurence.Text;
        //            arr[11].Value = txtfrieght.Text;
        //            arr[12].Value = txtfrieghtrate.Text != "" ? txtfrieghtrate.Text : "0.00";
        //            arr[13].Value = ddtransprt.SelectedIndex > 0 ? ddtransprt.SelectedValue : "0";
        //            arr[14].Value = dddelivery.SelectedIndex > 0 ? dddelivery.SelectedValue : "0";
        //            arr[15].Value = txtform.Text.ToUpper();
        //            arr[16].Value = txtremarks.Text;
        //            arr[17].Value = TxtExceisDuty.Text != "" ? TxtExceisDuty.Text : "0";
        //            arr[18].Value = TxtEduCess.Text != "" ? TxtEduCess.Text : "0";
        //            arr[19].Value = TxtCst.Text != "" ? TxtCst.Text : "0";
        //            arr[20].Value = TxtNetAmount.Text != "" ? TxtNetAmount.Text : "0";
        //            arr[21].Value = TxtAgentName.Text.ToUpper();
        //            arr[22].Value = DDPackingCharges.SelectedValue;
        //            arr[23].Value = 0;
        //            arr[24].Value = 0;
        //            arr[25].Value = DGSHOWDATA.Rows[i].Cells[0].Text;
        //            arr[26].Value = 0;                    
        //            arr[27].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[3].Text);
        //            arr[28].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[4].Text);
        //            arr[29].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[5].Text);
        //            arr[30].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[6].Text);
        //            arr[31].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[8].Text);
        //            arr[32].Value = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
        //            arr[33].Value = Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text);
        //            arr[34].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[11].Text);
        //            arr[35].Value = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
        //            arr[36].Value = Session["varCompanyId"];
        //            if (ChkEditOrder.Checked == true)
        //                arr[37].Value = ((Label)DGSHOWDATA.Rows[i].FindControl("lbldetailid")).Text;
        //            else
        //                arr[37].Value = ViewState["PorderpackdetailId"];
        //            arr[38].Value = Convert.ToInt32(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text);
        //            arr[39].Value = Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTweight")).Text);
        //            arr[40].Value = Convert.ToString(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXtrem")).Text);
        //            arr[41].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[7].Text);
        //            arr[42].Value = Convert.ToDateTime(((TextBox)DGSHOWDATA.Rows[i].FindControl("txtdivery_date")).Text);
        //            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PurchaseOrderPacking]", arr);
        //            ViewState["PorderpackId"] = arr[0].Value;
        //            txtchalanno.Text = arr[4].Value.ToString();
        //        }
        //        tran.Commit();
        //        Session["ReportPath"] = "Reports/RptPackingMaterialPurchaseOrderNEW.rpt";
        //        Session["CommanFormula"] = "{PurchaseOrdeDetailPacking.PId}=" + ViewState["PorderpackId"] + "";
        //        ddorderno.SelectedIndex = 0;
        //        ddempname.SelectedIndex = 0;
        //        if (ChkEditOrder.Checked == true)
        //        {
        //            ddchalanno.SelectedIndex = 0;
        //        }
        //        //txtchalanno.Text = "0";
        //        //TxtOrderId.Text = "";
        //        btnpriview.Visible = true;
        //        Fill_Grid_Show();
        //        msg = "Detail Saved.";
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "YourFunctionName('" + msg + "');", true);
        //        LblErrorMessage.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialPurchaseOrder.aspx");
        //        tran.Rollback();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}

        #endregion

        if (ddCompName.SelectedIndex == 0 || ddcustomercode.SelectedIndex == 0 || ddorderno.SelectedIndex == 0 || ddempname.SelectedIndex == 0)
            LblErrorMessage.Visible = true;
        else
            LblErrorMessage.Visible = false;
        if (LblErrorMessage.Visible == false)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            ViewState["PorderpackdetailId"] = 0;
            ViewState["PorderpackId"] = 0;

            try
            {
                SqlParameter[] arr = new SqlParameter[43];
                arr[0] = new SqlParameter("@Pid", SqlDbType.Int);
                arr[1] = new SqlParameter("@Companyid", SqlDbType.Int);
                arr[2] = new SqlParameter("@Partyid", SqlDbType.Int);
                arr[3] = new SqlParameter("@Orderid", SqlDbType.Int);
                arr[4] = new SqlParameter("@Chalanno", SqlDbType.NVarChar, 50);
                arr[5] = new SqlParameter("@Date", SqlDbType.DateTime);
                arr[6] = new SqlParameter("@Userid", SqlDbType.Int);
                arr[7] = new SqlParameter("@Duedate", SqlDbType.DateTime);
                arr[8] = new SqlParameter("@Destination", SqlDbType.NVarChar, 50);
                arr[9] = new SqlParameter("@PayementTermId", SqlDbType.Int);
                arr[10] = new SqlParameter("@Insurence", SqlDbType.NVarChar, 50);
                arr[11] = new SqlParameter("@Freight", SqlDbType.NVarChar, 50);
                arr[12] = new SqlParameter("@Feightrate", SqlDbType.Float);
                arr[13] = new SqlParameter("@TranportModeId", SqlDbType.Int);
                arr[14] = new SqlParameter("@DeliverTermid", SqlDbType.Int);
                arr[15] = new SqlParameter("@Formno", SqlDbType.NVarChar, 50);
                arr[16] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 150);
                arr[17] = new SqlParameter("@ExciseDuty", SqlDbType.Float);
                arr[18] = new SqlParameter("@EduCess", SqlDbType.Float);
                arr[19] = new SqlParameter("@CST", SqlDbType.Float);
                arr[20] = new SqlParameter("@NetAmount", SqlDbType.Float);
                arr[21] = new SqlParameter("@Agentname", SqlDbType.NVarChar, 50);
                arr[22] = new SqlParameter("@Packingcharge", SqlDbType.Int);
                arr[23] = new SqlParameter("@Finishedid", SqlDbType.Int);
                arr[24] = new SqlParameter("@Packingtype", SqlDbType.Int);
                arr[25] = new SqlParameter("@Packingcostid", SqlDbType.Int);
                arr[26] = new SqlParameter("@ProdCode_FinishedID", SqlDbType.Int);
                arr[27] = new SqlParameter("@Length", SqlDbType.Float);
                arr[28] = new SqlParameter("@Width", SqlDbType.Float);
                arr[29] = new SqlParameter("@Height", SqlDbType.Float);
                arr[30] = new SqlParameter("@Gsm", SqlDbType.Float);
                arr[31] = new SqlParameter("@Ply", SqlDbType.Float);
                arr[32] = new SqlParameter("@Qty", SqlDbType.Int);
                arr[33] = new SqlParameter("@Rate", SqlDbType.Float);
                arr[34] = new SqlParameter("@Amt", SqlDbType.Float);
                arr[35] = new SqlParameter("@Pcs", SqlDbType.Int);
                arr[36] = new SqlParameter("@MasterCompanyid", SqlDbType.Int);
                arr[37] = new SqlParameter("@Detailid", SqlDbType.Int);
                arr[38] = new SqlParameter("@pqty", SqlDbType.Int);
                arr[39] = new SqlParameter("@weight", SqlDbType.Float);
                arr[40] = new SqlParameter("@remark", SqlDbType.NVarChar, 250);
                arr[41] = new SqlParameter("@GSM2", SqlDbType.Float);
                arr[42] = new SqlParameter("@Delivery_Date", SqlDbType.DateTime);
                //int n = DGSHOWDATA.Rows.Count;
                //for (int i = 0; i < n; i++)
                //{
                string ab = ViewState["DetailID"].ToString();
                    arr[0].Direction = ParameterDirection.InputOutput;
                    if (ChkEditOrder.Checked == true)
                        arr[0].Value = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select pid from PurchaseOrdeDetailPacking where detailid=" + ab + "");
                    else
                        arr[0].Value = ViewState["PorderpackId"];
                    arr[1].Value = ddCompName.SelectedValue;
                    arr[2].Value = ddempname.SelectedValue;
                    arr[3].Value = ddorderno.SelectedValue;
                    arr[4].Direction = ParameterDirection.InputOutput;
                    if (ChkEditOrder.Checked == true)
                    {
                        arr[4].Value = ddchalanno.SelectedValue;
                    }
                    else
                    {

                        arr[4].Value = ViewState["ChallanNo"];
                        //arr[4].Value = 0;
                    }
                    arr[5].Value = txtdate.Text;
                    arr[6].Value = Session["varuserid"];
                    arr[7].Value = txtduedate.Text;
                    arr[8].Value = txtdestination.Text.ToUpper();
                    arr[9].Value = ddpayement.SelectedIndex > 0 ? ddpayement.SelectedValue : "0";
                    arr[10].Value = txtinsurence.Text;
                    arr[11].Value = txtfrieght.Text;
                    arr[12].Value = txtfrieghtrate.Text != "" ? txtfrieghtrate.Text : "0.00";
                    arr[13].Value = ddtransprt.SelectedIndex > 0 ? ddtransprt.SelectedValue : "0";
                    arr[14].Value = dddelivery.SelectedIndex > 0 ? dddelivery.SelectedValue : "0";
                    arr[15].Value = txtform.Text.ToUpper();
                    arr[16].Value = txtremarks.Text;
                    arr[17].Value = TxtExceisDuty.Text != "" ? TxtExceisDuty.Text : "0";
                    arr[18].Value = TxtEduCess.Text != "" ? TxtEduCess.Text : "0";
                    arr[19].Value = TxtCst.Text != "" ? TxtCst.Text : "0";
                    arr[20].Value = TxtNetAmount.Text != "" ? TxtNetAmount.Text : "0";
                    arr[21].Value = TxtAgentName.Text.ToUpper();
                    arr[22].Value = DDPackingCharges.SelectedValue;
                    arr[23].Value = 0;
                    arr[24].Value = 0;
                    arr[25].Value = ViewState["PackingCostID"];
                    arr[26].Value = 0;
                    arr[27].Value = txtlen.Text != "" ? txtlen.Text : "0";
                    arr[28].Value = txtwidth.Text != "" ? txtwidth.Text : "0";
                    arr[29].Value = txtheight.Text != "" ? txtheight.Text : "0";
                    arr[30].Value = txtgsm.Text != "" ? txtgsm.Text : "0";
                    arr[31].Value = txtply.Text != "" ? txtply.Text : "0";
                    arr[32].Value = txtqty.Text != "" ? txtqty.Text : "0";
                    arr[33].Value = txtrate.Text != "" ? txtrate.Text : "0";
                    arr[34].Value = Txtamount.Text != "" ? Txtamount.Text : "0";
                    arr[35].Value = txtpcs.Text != "" ? txtpcs.Text : "0";
                    arr[36].Value = Session["varCompanyId"];
                    if (ChkEditOrder.Checked == true)
                        arr[37].Value = ViewState["DetailID"];
                    else
                        arr[37].Value = ViewState["PorderpackdetailId"];
                    arr[38].Value = txtqty.Text != "" ? txtqty.Text : "0";
                    arr[39].Value = txtwidth.Text != "" ? txtwidth.Text : "0";
                    arr[40].Value = txtremarks.Text != "" ? txtremarks.Text : "0";
                    arr[41].Value = txtgsm2.Text != "" ? txtgsm2.Text : "0";
                    arr[42].Value = txtdeldate.Text != "" ? txtdeldate.Text : "0";
                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_PurchaseOrderPacking]", arr);
                    ViewState["PorderpackId"] = arr[0].Value;
                    ViewState["ChallanNo"] = arr[4].Value.ToString();
              //  }
                tran.Commit();
                Session["ReportPath"] = "Reports/RptPackingMaterialPurchaseOrderNEW.rpt";
                Session["CommanFormula"] = "{PurchaseOrdeDetailPacking.PId}=" + ViewState["PorderpackId"] + "";
                //ddorderno.SelectedIndex = 0;
                //ddempname.SelectedIndex = 0;
                //if (ChkEditOrder.Checked == true)
                //{
                //    ddchalanno.SelectedIndex = 0;
                //}
                //txtchalanno.Text = "0";
                //TxtOrderId.Text = "";
                btnpriview.Visible = true;
                Fill_Grid_Show();
                Fill_DataGridShow();
                if (ChkEditOrder.Checked == false)
                {
                    ddcustomercode.Enabled = false;
                    ddorderno.Enabled = false;
                    ddempname.Enabled = false;
                }
                Refresh();
                //Fill_DataGridShow();
                msg = @"Record(s) has been saved successfully!  Challan Number is : " + ViewState["ChallanNo"].ToString();
//                        Challan Number is : " + ViewState["ChallanNo"].ToString();
                MessageSave(msg);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "YourFunctionName('" + msg + "');", true);
                LblErrorMessage.Visible = false;
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialPurchaseOrder.aspx");
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully logedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            ViewState["flag"] = 1;
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And  om.orderid  in (select orderid from PurchaseOrderMasterPacking) Order by LocalOrder+ ' / ' +CustomerOrderNo", true, "Select OrderNo.");
            DataSet ds3 = new DataSet();
            tdchalanno.Visible = true;
            Td6.Visible = false;
            //td_ord.Style.Add("display", "");
            DGSHOWDATA.Visible = false;
            GDVSHOWORDER.DataSource = "";
            GDVSHOWORDER.DataBind();

        }
        else
        {
            ViewState["flag"] = 0;
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + "  And om.orderid not in (select orderid from PurchaseOrderMasterPacking) LocalOrder+ ' / ' +CustomerOrderNo ", true, "Select OrderNo.");
            //td_ord.Style.Add("display", "none");
            Td6.Visible = true;
            tdchalanno.Visible = false;
            DGSHOWDATA.Visible = false;
        }
        Refresh();
        //TxtOrderId.Text = "";
       // txtchalanno.Text = "";
    }
    
    protected void Txtrate_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DGSHOWDATA.Rows.Count; i++)
        {
            string qnt = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
            string rate = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
            string pc = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
            //((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Enabled = true;
            DGSHOWDATA.Rows[i].Cells[11].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
        }
    }
    protected void Txtqty_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DGSHOWDATA.Rows.Count; i++)
        {
            string qnt = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
            string rate = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
            string pc = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
            //((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Enabled = true;
            DGSHOWDATA.Rows[i].Cells[11].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
        }
    }
    protected void Txtpc_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DGSHOWDATA.Rows.Count; i++)
        {
            string qnt = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
            string rate = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
            string pc = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
            //((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Enabled = true;
            DGSHOWDATA.Rows[i].Cells[11].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ChallanNo"] = 0;
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddchalanno, "select Distinct chalanno,chalanno ChalanText from PurchaseOrderMasterPacking where companyid=" + ddCompName.SelectedValue + " and orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + " Order By ChalanText", true, "Select Chalanno.");
        }
    }
    protected void ddchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //TxtOrderId.Text = ddchalanno.SelectedValue;
        //txtchalanno.Text = ddchalanno.SelectedValue;
        ViewState["ChallanNo"] = ddchalanno.SelectedValue;
        if (ddCompName.SelectedIndex > 0 && ddorderno.SelectedIndex > 0 && ddempname.SelectedIndex > 0 && ddchalanno.SelectedIndex > 0)
        {
            DataSet dt7 = new DataSet();
            string str2 = "select pid from PurchaseOrderMasterPacking where companyid=" + ddCompName.SelectedValue + " and partyid=" + ddempname.SelectedValue + " and orderid=" + ddorderno.SelectedValue + " and chalanno=" + ddchalanno.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "";
            dt7 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            ViewState["PorderpackId"] = dt7.Tables[0].Rows[0][0].ToString();
            Session["ReportPath"] = "Reports/RptPackingMaterialPurchaseOrderNEW.rpt";
            Session["CommanFormula"] = "{PurchaseOrdeDetailPacking.PId}=" + dt7.Tables[0].Rows[0][0].ToString() + "";
            btnpriview.Visible = true;
        }
        Fill_DataGridShow();
        if (ChkEditOrder.Checked == true)
        {
            Fill_Grid_Show();
        }
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        Report();
    }
    private void Report()
    {
        string qry = "";
        if (ChkEditOrder.Checked)
        {
            qry = @"SELECT CompanyInfo.CompanyId,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,EmpInfo.EmpName,ITEM_PARAMETER_MASTER.ProductCode,
                  PurchaseOrderMasterPacking.Date DispatchDate,OrderMaster.CustomerOrderNo,PurchaseOrdeDetailPacking.PACKINGTYPE,PurchaseOrdeDetailPacking.Length,
                  PurchaseOrdeDetailPacking.Width,PurchaseOrdeDetailPacking.Height,PurchaseOrdeDetailPacking.Ply,PurchaseOrdeDetailPacking.QTY,
                  PurchaseOrdeDetailPacking.RATE,PurchaseOrdeDetailPacking.AMT,PurchaseOrdeDetailPacking.PCS,PurchaseOrderMasterPacking.CHALANNO,
                  PurchaseOrdeDetailPacking.PID,PurchaseOrdeDetailPacking.weight,PurchaseOrdeDetailPacking.remarks,PurchaseOrderMasterPacking.DueDate,
                  OrderMaster.LocalOrder,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.Email,CompanyInfo.TinNo,
                  PurchaseOrdeDetailPacking.Delivery_Date
                  FROM   CompanyInfo INNER JOIN PurchaseOrderMasterPacking ON CompanyInfo.CompanyId=PurchaseOrderMasterPacking.COMPANYID
                  INNER JOIN PurchaseOrdeDetailPacking ON PurchaseOrderMasterPacking.PID=PurchaseOrdeDetailPacking.PID INNER JOIN OrderMaster ON PurchaseOrderMasterPacking.ORDERID=OrderMaster.OrderId 
                  INNER JOIN EmpInfo ON PurchaseOrderMasterPacking.PARTYID=EmpInfo.EmpId INNER JOIN ITEM_PARAMETER_MASTER ON PurchaseOrdeDetailPacking.FINISHEDID=ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID
                  Where PurchaseOrderMasterPacking.CHALANNO=" + ViewState["ChallanNo"] + " And PurchaseOrderMasterPacking.MasterCompanyId=" + Session["varCompanyId"] + " And Companyinfo.CompanyId=" + ddCompName.SelectedValue;
        }
        else
        {
            qry = @"SELECT CompanyInfo.CompanyId,CompanyInfo.CompanyName,CompanyInfo.CompAddr1,EmpInfo.EmpName,ITEM_PARAMETER_MASTER.ProductCode,
                  PurchaseOrderMasterPacking.Date DispatchDate,OrderMaster.CustomerOrderNo,PurchaseOrdeDetailPacking.PACKINGTYPE,PurchaseOrdeDetailPacking.Length,
                  PurchaseOrdeDetailPacking.Width,PurchaseOrdeDetailPacking.Height,PurchaseOrdeDetailPacking.Ply,PurchaseOrdeDetailPacking.QTY,
                  PurchaseOrdeDetailPacking.RATE,PurchaseOrdeDetailPacking.AMT,PurchaseOrdeDetailPacking.PCS,PurchaseOrderMasterPacking.CHALANNO,
                  PurchaseOrdeDetailPacking.PID,PurchaseOrdeDetailPacking.weight,PurchaseOrdeDetailPacking.remarks,PurchaseOrderMasterPacking.DueDate,
                  OrderMaster.LocalOrder,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.Email,CompanyInfo.TinNo,
                  PurchaseOrdeDetailPacking.Delivery_Date
                  FROM   CompanyInfo INNER JOIN PurchaseOrderMasterPacking ON CompanyInfo.CompanyId=PurchaseOrderMasterPacking.COMPANYID
                  INNER JOIN PurchaseOrdeDetailPacking ON PurchaseOrderMasterPacking.PID=PurchaseOrdeDetailPacking.PID INNER JOIN OrderMaster ON PurchaseOrderMasterPacking.ORDERID=OrderMaster.OrderId 
                  INNER JOIN EmpInfo ON PurchaseOrderMasterPacking.PARTYID=EmpInfo.EmpId INNER JOIN ITEM_PARAMETER_MASTER ON PurchaseOrdeDetailPacking.FINISHEDID=ITEM_PARAMETER_MASTER.ITEM_FINISHED_ID
                  Where PurchaseOrderMasterPacking.CHALANNO=" + ViewState["ChallanNo"] + " And PurchaseOrderMasterPacking.MasterCompanyId=" + Session["varCompanyId"] + " And Companyinfo.CompanyId=" + ddCompName.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptPackingMaterialPurchaseOrderNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void DGSHOWDATA_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGSHOWDATA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
           e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGSHOWDATA, "select$" + e.Row.RowIndex);
        }
    }
    //protected void BtnDelete_Click(object sender, EventArgs e)
    //{
    //    int i = Convert.ToInt32(DGSHOWDATA.DataKeys[e.RowIndex].Value);
    //   SqlParameter[] _param = new SqlParameter[3];
    //   _param[0] = new SqlParameter("@PDetailID", i);
    //   _param[1] = new SqlParameter("@MasterCompanyID", Session["CompanyID"].ToString());
    //   _param[2] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
    //   _param[2].Direction = ParameterDirection.Output;
    //   msg = _param[2].Value.ToString();
    //   MessageSave(msg);
    //}
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>"); 
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }

    protected void DGSHOWDATA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int i = Convert.ToInt32(DGSHOWDATA.DataKeys[e.RowIndex].Value);
        SqlParameter[] _param = new SqlParameter[3];
        _param[0] = new SqlParameter("@PDetailID", i);
        _param[1] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"].ToString());
        _param[2] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
        _param[2].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PackingPurchaseOrder_Delete", _param);
        msg = _param[2].Value.ToString();
        MessageSave(msg);
        Fill_Grid_Show();
        Fill_DataGridShow();
    }
    protected void GDVSHOWORDER_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
    protected void GDVSHOWORDER_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDVSHOWORDER, "Select$" + e.Row.RowIndex);
        }
    }
    protected void GDVSHOWORDER_SelectedIndexChanged(object sender, EventArgs e)
    {
        int r = Convert.ToInt32(GDVSHOWORDER.SelectedIndex.ToString());
        ViewState["PackingCostID"] = GDVSHOWORDER.SelectedDataKey.Value;
        ViewState["DetailID"] = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lbldetailid")).Text;
        txtlen.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lbllength")).Text;
        txtwidth.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblwidth")).Text;
        txtheight.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblHeight")).Text;
        txtgsm.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblgsm")).Text;
        txtgsm2.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblgsm2")).Text;
        txtply.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblply")).Text;
        txtweight.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblunit")).Text;
        txtremarks.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lblremark")).Text;

        string orderid = ((Label)GDVSHOWORDER.Rows[r].FindControl("LblOrderID")).Text;
        string str = @"select OrderQty, OrderQty-IssuedQty PQty from V_TotalPackingQty where OrderID=" + orderid + " AND ID=" + GDVSHOWORDER.SelectedDataKey.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        txtPqty.Text = ds.Tables[0].Rows[0]["PQty"].ToString();
        if (ChkEditOrder.Checked == true)
        {
            txtdeldate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lbldeldate")).Text;
            txtdate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblodate")).Text;
            txtduedate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblduedate")).Text;
            txtqty.Text = GDVSHOWORDER.Rows[r].Cells[5].Text;
        }
        else
        {
             txtqty.Text = ds.Tables[0].Rows[0]["OrderQty"].ToString();
            //string orderid = ((Label)GDVSHOWORDER.Rows[r].FindControl("LblOrderID")).Text;
            //string str = @"select OrderQty-IssuedQty PQty from V_TotalPackingQty where OrderID=" + orderid + " AND ID=" + GDVSHOWORDER.SelectedDataKey.Value;
            //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            //txtqty.Text = ds.Tables[0].Rows[0]["PQty"].ToString();
        }
        
        
        string PkgType = GDVSHOWORDER.Rows[r].Cells[3].Text;
        if (PkgType == "MIDDLE")
        {
            DDpackingType.SelectedIndex = 1;
        }
        else if (PkgType == "MASTER")
        {
            DDpackingType.SelectedIndex = 2;
        }
        else
        {
            DDpackingType.SelectedIndex = 0;
        }
        txtItemCode.Text = GDVSHOWORDER.Rows[r].Cells[1].Text;
      
        txtrate.Text = GDVSHOWORDER.Rows[r].Cells[6].Text;
        Txtamount.Text = GDVSHOWORDER.Rows[r].Cells[7].Text;
        txtpcs.Text = GDVSHOWORDER.Rows[r].Cells[8].Text;
        
    }
    protected void DGSHOWDATA_SelectedIndexChanged(object sender, EventArgs e)
    {
    //    int i = Convert.ToInt32(GDVSHOWORDER.SelectedIndex.ToString());
    //    ViewState["DetailID"] = ((Label)DGSHOWDATA.Rows[i].FindControl("lbldetailid")).Text;
    //    //arr[0].Direction = ParameterDirection.InputOutput;
    //    //if (ChkEditOrder.Checked == true)
    //    //    arr[0].Value = SqlHelper.ExecuteScalar(tran, CommandType.Text, "select pid from PurchaseOrdeDetailPacking where detailid=" + ab + "");
    //    //else
    //    //    arr[0].Value = ViewState["PorderpackId"];
       
    //    arr[5].Value = txtdate.Text;
    //    arr[6].Value = "1";
    //    arr[7].Value = txtduedate.Text;
    //    arr[8].Value = txtdestination.Text.ToUpper();
    //    arr[9].Value = ddpayement.SelectedIndex > 0 ? ddpayement.SelectedValue : "0";
    //    arr[10].Value = txtinsurence.Text;
    //    arr[11].Value = txtfrieght.Text;
    //    arr[12].Value = txtfrieghtrate.Text != "" ? txtfrieghtrate.Text : "0.00";
    //    arr[13].Value = ddtransprt.SelectedIndex > 0 ? ddtransprt.SelectedValue : "0";
    //    arr[14].Value = dddelivery.SelectedIndex > 0 ? dddelivery.SelectedValue : "0";
    //    arr[15].Value = txtform.Text.ToUpper();
    //    arr[16].Value = txtremarks.Text;
    //    arr[17].Value = TxtExceisDuty.Text != "" ? TxtExceisDuty.Text : "0";
    //    arr[18].Value = TxtEduCess.Text != "" ? TxtEduCess.Text : "0";
    //    arr[19].Value = TxtCst.Text != "" ? TxtCst.Text : "0";
    //    arr[20].Value = TxtNetAmount.Text != "" ? TxtNetAmount.Text : "0";
    //    arr[21].Value = TxtAgentName.Text.ToUpper();
    //    arr[22].Value = DDPackingCharges.SelectedValue;
    //    arr[23].Value = 0;
    //    arr[24].Value = 0;
    //    arr[25].Value = DGSHOWDATA.Rows[i].Cells[0].Text;
    //    arr[26].Value = 0;
    //    arr[27].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[3].Text);
    //    arr[28].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[4].Text);
    //    arr[29].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[5].Text);
    //    arr[30].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[6].Text);
    //    arr[31].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[8].Text);
    //   txtqty.Text = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
    //  txtrate.Text = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
    //    arr[34].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[11].Text);
    //txtpcs.Text= ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
    
    //   txtqty.Text = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
    //   txtweight.Text =((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTweight")).Text;
    //    txtremarks.Text = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXtrem")).Text;
    //    arr[41].Value = DGSHOWDATA.Rows[i].Cells[7].Text;
    //   txtdeldate.Text = ((TextBox)DGSHOWDATA.Rows[i].FindControl("txtdivery_date")).Text;
       

    }

    protected void Refresh()
    {
        TxtAgentName.Text = "";
        Txtamount.Text = "";
        TxtCst.Text = "";
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtdeldate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtduedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        TxtEduCess.Text = "";
        TxtExceisDuty.Text = "";
        txtfrieghtrate.Text = "";
        txtgsm.Text = "";
        txtgsm2.Text = "";
        txtheight.Text = "";
        txtinsurence.Text = "";
        txtItemCode.Text = "";
        txtlen.Text = "";
        TxtNetAmount.Text = "";
        txtpcs.Text = "";
        txtply.Text = "";
        txtqty.Text = "";
        txtrate.Text = "";
        txtremarks.Text = "";
        TxtTotalAmount.Text = "";
        txtweight.Text = "";
        txtwidth.Text = "";
        //GDVSHOWORDER.DataSource = "";
        //GDVSHOWORDER.DataBind();

    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        ddcustomercode.Enabled = true;
        ddorderno.Enabled = true;
        ddempname.Enabled = true;
        GDVSHOWORDER.DataSource = "";
        GDVSHOWORDER.DataBind();
        DGSHOWDATA.DataSource = "";
        DGSHOWDATA.DataBind();
        if (ddcustomercode.Items.Count > 0)
        {
            ddcustomercode.SelectedIndex = 0;
        }
        if (ddorderno.Items.Count > 0)
        {
            ddorderno.SelectedIndex = 0;
        }
        if (ddempname.Items.Count > 0)
        {
            ddempname.SelectedIndex = 0;
        }
    }
   
}