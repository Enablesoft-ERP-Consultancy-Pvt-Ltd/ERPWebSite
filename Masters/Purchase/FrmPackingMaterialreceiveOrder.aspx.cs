using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Purchase_FrmPackingMaterialreceiveOrder : System.Web.UI.Page
{
    DataSet dt3 = new DataSet();
    string str2;
    string msg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
            if (!IsPostBack)
            {
              
                ViewState["pac_mas_id"] = 0;
               ViewState["pac_det_id"] = 0;
                txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                string qry= @"select distinct ci.customerid,ci.Customercode + SPACE(5)+CI.CompanyName from customerinfo ci 
                inner join OrderMaster om on om.customerid=ci.customerid And ci.MasterCompanyId=" + Session["varCompanyId"] + @" inner join ORDER_CONSUMPTION_DETAIL ocd on ocd.orderid=om.orderid
                inner join Jobassigns JA ON OM.Orderid=JA.Orderid
                 select distinct ei.empid ,ei.empname from empinfo ei inner join  PurchaseIndentMaster pim on ei.empid=pim.partyid And ei.MasterCompanyId=" + Session["varCompanyId"] + @"
                Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 

                select distinct e1.empid,e1.empname from empinfo e1 Where e1.MasterCompanyId=" + Session["varCompanyId"] + @"
                select GoDownID,GodownName from godownmaster where MasterCompanyid=" + Session["varCompanyId"];
                DataSet ds = SqlHelper.ExecuteDataset(qry);
                UtilityModule.ConditionalComboFillWithDS(ref ddcustomercode,ds,0, true, "Select CustomerCode");
                UtilityModule.ConditionalComboFillWithDS(ref ddempname, ds, 1, true, "Select Party");
                UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 2, true, "Select Comp Name");
                UtilityModule.ConditionalComboFillWithDS(ref ddempname, ds, 3, true, "Select Party");
                UtilityModule.ConditionalComboFillWithDS(ref ddgodown, ds, 4, true, "--Select--");

                if (ddCompName.Items.Count > 0)
                {
                    ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                    ddCompName.Enabled = false;
                }

                ddcustomercode.Focus();
                imgLogo.ImageUrl.DefaultIfEmpty();
                imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
                LblCompanyName.Text = Session["varCompanyName"].ToString();
                LblUserName.Text = Session["varusername"].ToString();  
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
    protected void ddcustomercode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And  om.orderid  in (select distinct orderno from pakingprocessreceivedetail ) Order BY OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo", true, "Select OrderNo.");           
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And om.orderid  in (select orderid from PurchaseOrderMasterPacking where pid in(select distinct pid from PurchaseOrdeDetailPacking where pqty<>0)) Order BY OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo", true, "Select OrderNo.");           
        }
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //con.Open();
        //try
        //{
        //    if (ChkEditOrder.Checked == false)
        //    {
        //        str2 = "select isnull(max(PackingReceiveId),0)+1 from packingprocessreceivemaster";
        //        dt3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        //        txtBillno.Text = dt3.Tables[0].Rows[0][0].ToString();
        //    }
        //    else
        //    {
        //        txtBillno.Text = "";
        //    }
        //}
        //catch(Exception ex)
        //{
        //    UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialreceiveOrder.aspx");
        //}
        //finally
        //{
        //    con.Close();
        //    con.Dispose();
        //}
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dt5 = new DataSet();
        str2 = "select chalanno from PurchaseOrderMasterPacking where orderid=" + ddorderno.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"];
        dt5 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
        // txtchalanno.Text = dt5.Tables[0].Rows[0][0].ToString();
        if (ChkEditOrder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "select distinct empid,empname from empinfo where MasterCompanyId=" + Session["varCompanyId"] + " And  empid in(select distinct partyid from pakingprocessreceivedetail pd,packingprocessreceivemaster pm where pd.packingreceiveid=pm.packingreceiveid and pd.orderno=" + ddorderno.SelectedValue + " ) Order BY empname", true, "Select Party");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddempname, "select distinct empid,empname from empinfo e,PurchaseOrderMasterPacking pm where pm.partyid=e.empid and orderid=" + ddorderno.SelectedValue + " And e.MasterCompanyId=" + Session["varCompanyId"] + " Order By empname", true, "Select Party");
        }
        //Fill_Grid_Show();
    }
    private void Fill_Grid_Show()
   { 
        string str4 = "";
         //TxtOrderId.Text = ddorderno.SelectedValue;
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
         con.Open();
         try
         {
             if (ChkEditOrder.Checked == true)
             {
                 str4 = @"select distinct pid.packingprocessreceivedetailid ,pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,
                          pd.height,pd.gsm,pd.ply,pd.pcs unit,pd.pqty pqty,pd.qty qty,pd.rate,amt,pd.pcs,pd.detailid,isnull(pd.weight,0) weight,pid.remarks remark,pd.gsm2 gsm2,IPM.ProductCode,isnull(pim.godownid,0)godownid
                        From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om,pakingprocessreceivedetail pid,packingprocessreceivemaster pim, item_parameter_master IPM
                        Where pm.pid=pd.pid and om.orderid=pm.orderid and pid.pdetailid=pd.detailid and pim.PackingReceiveId=pid.PackingReceiveId AND IPM.ITEM_FINISHED_ID=pd.FINISHEDID and 
                        pim.PackingReceiveId  = " + ddBillno.SelectedValue + " and pm.chalanno=" + ddchalanno.SelectedValue + " And pm.MasterCompanyId=" + Session["varCompanyId"];
             }
             else
             {
                 str4 = @"select 0 as packingprocessreceivedetailid, pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,pd.height,pd.gsm,pd.ply,pd.pcs unit,pd.pqty pqty,pd.qty qty,pd.rate,(pd.pqty*pd.rate) amt,pd.pcs,pd.detailid,isnull(pd.weight,0) weight,pd.remarks remark,pd.gsm2 gsm2, IPM.ProductCode 
                        From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om, item_parameter_master IPM
                        Where pm.pid=pd.pid and om.orderid=pm.orderid AND IPM.ITEM_FINISHED_ID=pd.FINISHEDID AND pd.pqty > 0 and pm.chalanno=" + ddchalanno.SelectedValue + " And pm.MasterCompanyId=" + Session["varCompanyId"];
             }
              DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str4);
              if (ChkEditOrder.Checked == true)
              {
                  if (Ds.Tables[0].Rows.Count > 0)
                  {
                      ddgodown.SelectedValue = Ds.Tables[0].Rows[0]["godownid"].ToString();
                  }
                  //ddgodown.Enabled = false;
              }
              DGSHOWDATA.DataSource = Ds;
              DGSHOWDATA.DataBind();
              if (DGSHOWDATA.Rows.Count > 0)
              {
                  selectall.Visible = true;
              }
              else
              {
                  selectall.Visible = false;
              }
              if (ChkEditOrder.Checked == true)
              {
                  DGSHOWDATA.Columns[17].Visible = true;
              }
             
         }
         catch (Exception ex)
         {
             UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialreceiveOrder.aspx");
         }
         finally
         {
             con.Close();
             con.Dispose();
         }
    }
    private void Fill_GridData_Show()
    {
        string str4 = "";
        //TxtOrderId.Text = ddorderno.SelectedValue;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            if (ChkEditOrder.Checked == true)
            {
                str4 = @"select distinct pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,pd.height,pd.gsm,pd.ply,pd.pcs unit,pd.pqty pqty,pd.qty qty,pd.rate,amt,pd.pcs,pd.detailid,isnull(pd.weight,0) weight,pd.remarks remark,pd.gsm2 gsm2
                        From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om,pakingprocessreceivedetail pid,packingprocessreceivemaster pim
                        Where pm.pid=pd.pid and om.orderid=pm.orderid and pid.pdetailid=pd.detailid and pim.PackingReceiveId=pim.PackingReceiveId  and pim.partyid=" + ddempname.SelectedValue + "  and pm.chalanno=" + ddchalanno.SelectedValue + " And pm.MasterCompanyId=" + Session["varCompanyId"];
            }
            else
            {
                str4 = @"select pd.packingcostid srno,OM.OrderId,LocalOrder+ ' / ' +om.CustomerOrderNo orderno,Case When pd.PackingType=1 Then 'INNER' Else Case When pd.PackingType=2 
                Then 'MIDDLE' Else 'MASTER' END END PackingType,pd.length,pd.width,pd.height,pd.gsm,pd.ply,pd.pcs unit,pd.pqty pqty,pd.qty qty,pd.rate,amt,pd.pcs,pd.detailid,
                isnull(pd.weight,0) weight,pd.remarks remark,pd.gsm2 gsm2, replace(convert(varchar(11),GETDATE(),106), ' ','-')  as Recdate, IPM.ProductCode
                From PurchaseOrderMasterPacking pm,PurchaseOrdeDetailPacking pd,ordermaster om ,item_parameter_master IPM
                Where pm.pid=pd.pid  and om.orderid=pm.orderid AND IPM.ITEM_FINISHED_ID=pd.FINISHEDID  and pm.partyid=" + ddempname.SelectedValue + " and  pm.chalanno =" + ddchalanno.SelectedValue + " And pm.MasterCompanyId=" + Session["varCompanyId"];
            }
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str4);
            //GDVSHOWORDER.DataSource = Ds;
            //GDVSHOWORDER.DataBind();
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialreceiveOrder.aspx");
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void Txtrate_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DGSHOWDATA.Rows.Count; i++)
        {
            string qnt = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
            string rate = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
            string pc = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
            //((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Enabled = true;
            DGSHOWDATA.Rows[i].Cells[13].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
        }
    }
    protected void Txtqty_TextChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < DGSHOWDATA.Rows.Count; i++)
        {
            string qnt = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text;
            string rate = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRAte")).Text;
            if (rate == "")
            {
                rate = DGSHOWDATA.Rows[i].Cells[11].Text;
            }
            string pc = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Text;
            //((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTPCS")).Enabled = true;
            DGSHOWDATA.Rows[i].Cells[13].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
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
            DGSHOWDATA.Rows[i].Cells[13].Text = Convert.ToString(Math.Round(((Convert.ToDecimal(qnt) / Convert.ToDecimal(pc)) * Convert.ToDecimal(rate)), 2));
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        DataSet dt4 = new DataSet();
       
         SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ddCompName.SelectedIndex > 0 && ddcustomercode.SelectedIndex > 0 && ddempname.SelectedIndex > 0 && ddchalanno.SelectedIndex >0)
                 {
                     LblErrorMessage.Visible = false;
                    SqlParameter[] arr = new SqlParameter[30];
                    arr[0] = new SqlParameter("@packingreceiveid", SqlDbType.Int);
                    arr[1] = new SqlParameter("@Companyid", SqlDbType.Int);
                    arr[2] = new SqlParameter("@Partyid", SqlDbType.Int);
                    arr[3] = new SqlParameter("@receiveno", SqlDbType.NVarChar, 50);
                    arr[4] = new SqlParameter("@receivedate", SqlDbType.DateTime);
                    arr[5] = new SqlParameter("@userid", SqlDbType.Int);
                    arr[6] = new SqlParameter("@billno", SqlDbType.NVarChar, 50);
                    arr[7] = new SqlParameter("@packingprocessreceivedetailid", SqlDbType.Int);
                    arr[8] = new SqlParameter("@orderno", SqlDbType.Int);
                    arr[9] = new SqlParameter("@packingType", SqlDbType.Int);
                    arr[10] = new SqlParameter("@length", SqlDbType.Float);
                    arr[11] = new SqlParameter("@width", SqlDbType.Float);
                    arr[12] = new SqlParameter("@gsm", SqlDbType.Float);
                    arr[13] = new SqlParameter("@ply", SqlDbType.Float);
                    arr[14] = new SqlParameter("@qty", SqlDbType.Float);
                    arr[15] = new SqlParameter("@rate", SqlDbType.Float);
                    arr[16] = new SqlParameter("@amount", SqlDbType.Float);
                    arr[17] = new SqlParameter("@height", SqlDbType.Float);
                    arr[18] = new SqlParameter("@openqty", SqlDbType.Float);
                    arr[19] = new SqlParameter("@addqty", SqlDbType.Float);
                    arr[20] = new SqlParameter("@pstockno", SqlDbType.Int);
                    arr[21] = new SqlParameter("@pdetailid", SqlDbType.Int);
                    arr[22] = new SqlParameter("@pqty", SqlDbType.Int);
                    arr[23] = new SqlParameter("@weight", SqlDbType.Float);
                    arr[24] = new SqlParameter("@remark", SqlDbType.NVarChar, 250);
                    arr[25] = new SqlParameter("@gsm2", SqlDbType.Float);
                    arr[26] = new SqlParameter("@godownID", SqlDbType.Float);
                   
                   int n = DGSHOWDATA.Rows.Count;
                    for (int i = 0; i < n; i++)
                    {
                        ViewState["pac_det_id"] = ((Label)DGSHOWDATA.Rows[i].FindControl("LblpkgdetailID")).Text;
                        
                        GridViewRow row = DGSHOWDATA.Rows[i];
                        if (((CheckBox)row.FindControl("Chkbox")).Checked == true)
                        {
                            arr[0].Value = ViewState["pac_mas_id"];
                            arr[0].Direction = ParameterDirection.InputOutput;
                            arr[1].Value = ddCompName.SelectedValue;
                            arr[2].Value = ddempname.SelectedValue;
                            //arr[3].Value = ddBillno.SelectedValue;
                            arr[3].Value = ddchalanno.SelectedValue;
                            arr[4].Value = txtdate.Text;
                            arr[5].Value = Session["varuserid"];
                            if (ChkEditOrder.Checked == false)
                            {
                                arr[6].Value = txtBillno.Text.Trim().ToUpper();
                            }
                            else
                            {
                                arr[6].Value = ddBillno.SelectedItem.ToString();
                            }
                            arr[7].Value = ViewState["pac_det_id"];
                            arr[8].Value = ddorderno.SelectedValue;
                            string a = ((Label)DGSHOWDATA.Rows[i].FindControl("lblpacktype")).Text;
                            if (a == "INNER")
                                arr[9].Value = 1;
                            else if (a == "MIDDLE")
                                arr[9].Value = 2;
                            else
                                arr[9].Value = 3;
                            arr[10].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[4].Text);
                            arr[11].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[5].Text);
                            arr[12].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[7].Text);
                            arr[13].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[9].Text);
                            arr[14].Value = Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text);
                            arr[15].Value = Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTRATE")).Text);
                            arr[16].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[13].Text);
                            arr[17].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[6].Text);
                            arr[18].Value = 0;
                            string str1 = "select pstockid,qty from pakingmetalstock where companyid=" + ddCompName.SelectedValue + " and length=" + DGSHOWDATA.Rows[i].Cells[4].Text + " and width=" + DGSHOWDATA.Rows[i].Cells[5].Text + " and heigth=" + DGSHOWDATA.Rows[i].Cells[6].Text + " and gsm=" + DGSHOWDATA.Rows[i].Cells[7].Text + " and gsm2=" + DGSHOWDATA.Rows[i].Cells[8].Text + " and ply=" + DGSHOWDATA.Rows[i].Cells[9].Text + "";
                            dt4 = SqlHelper.ExecuteDataset(tran, CommandType.Text, str1);
                            if (dt4.Tables[0].Rows.Count > 0)
                            {
                                arr[20].Value = dt4.Tables[0].Rows[0][0].ToString();
                                arr[19].Value = Convert.ToDouble(dt4.Tables[0].Rows[0][1].ToString()) + Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text);
                            }
                            else
                            {
                                arr[20].Value = 0;
                                arr[19].Value = Convert.ToDouble(((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTQTY")).Text);
                            }
                            arr[21].Value = ((Label)DGSHOWDATA.Rows[i].FindControl("lbldetailid")).Text;
                            arr[23].Value = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTweight")).Text;
                            arr[24].Value = ((TextBox)DGSHOWDATA.Rows[i].FindControl("TXTrem")).Text;
                            arr[25].Value = Convert.ToDouble(DGSHOWDATA.Rows[i].Cells[8].Text);
                            arr[26].Value = ddgodown.SelectedValue;
                          
                            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[PRO_ReceiveOrderPacking]", arr);
                            ViewState["pac_mas_id"] = arr[0].Value;
                        }
                    }
                    tran.Commit();
                    report();
                    UtilityModule.ConditionalComboFill(ref ddorderno, "Select Distinct OM.OrderId,LocalOrder+ ' / ' +CustomerOrderNo from OrderMaster OM,Jobassigns JA Where OM.Orderid=JA.Orderid And CustomerId=" + ddcustomercode.SelectedValue + "and om.CompanyId=" + ddCompName.SelectedValue + " And om.orderid  in (select orderid from PurchaseOrderMasterPacking where pid in(select distinct pid from PurchaseOrdeDetailPacking where pqty<>0))", true, "Select OrderNo.");
                    btnpriview.Visible = true;
                    // txtchalanno.Text = "";
                    ddchalanno.SelectedIndex = 0;
                    // TxtOrderId.Text = "";
                    Fill_Grid_Show();
                    refresh();
                    msg = "Record(s) has been saved successfully !";
                    MessageSave(msg);
                }
                else
                {
                    LblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Purchase/FrmPackingMaterialreceiveOrder.aspx");
                tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            string str = @"select Distinct PM.ReceiveNo, PM.ReceiveNo ChallanNo from packingprocessreceivemaster PM, pakingprocessreceivedetail PD
                                    where PM.PackingReceiveId=PD.PackingReceiveId AND OrderNo=" + ddorderno.SelectedValue + "  AND CompanyID=" + Session["varCompanyId"] + " AND PartyID=" + ddempname.SelectedValue + " Order BY ChallanNo";
            UtilityModule.ConditionalComboFill(ref ddchalanno, str , true, "Select Chalanno.");
            //UtilityModule.ConditionalComboFill(ref ddchalanno, "select DISTINCT chalanno,chalanno ChallanText from PurchaseOrderMasterPacking where MasterCompanyId=" + Session["varCompanyId"] + " And pid in(select distinct pid from PurchaseOrdeDetailPacking where detailid in(select distinct pdetailid from pakingprocessreceivedetail)) and companyid=" + ddCompName.SelectedValue + " and orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " Order BY ChallanText ", true, "Select Chalanno.");
        }
        else
            UtilityModule.ConditionalComboFill(ref ddchalanno, "select DISTINCT chalanno,chalanno  ChallanText from PurchaseOrderMasterPacking where MasterCompanyId=" + Session["varCompanyId"] + " And companyid=" + ddCompName.SelectedValue + " and orderid=" + ddorderno.SelectedValue + " and partyid=" + ddempname.SelectedValue + " Order By ChallanText", true, "Select Chalanno.");
       
    }
    
    protected void ddchalanno_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TxtOrderId.Text = ddchalanno.SelectedValue;
        // txtchalanno.Text = ddchalanno.SelectedValue;
        if (ChkEditOrder.Checked == false)
        {
            Fill_Grid_Show();
            //Fill_GridData_Show();
        }
        if (ChkEditOrder.Checked == true)
        {
            string str = @"select DISTINCT PM.packingreceiveid, PM.BillNo  from packingprocessreceivemaster PM, pakingprocessreceivedetail PD
                            where PM.PackingReceiveId=PD.PackingReceiveId AND OrderNo=" + ddorderno.SelectedValue + "  AND CompanyID=" + Session["varCompanyId"] + " AND PartyID= " + ddempname.SelectedValue + " AND ReceiveNo= " + ddchalanno.SelectedValue + "  Order By PM.BillNo";
            //UtilityModule.ConditionalComboFill(ref ddBillno, "select distinct pm.packingreceiveid,pm.billno from pakingprocessreceivedetail pd,packingprocessreceivemaster pm ,PurchaseOrdeDetailPacking po where pd.packingreceiveid=pm.packingreceiveid and po.detailid=pd.pdetailid and pd.orderno=" + ddorderno.SelectedValue + " and po.pid=" + ddchalanno.SelectedValue + " And pm.CompanyId=" + ddCompName.SelectedValue + " Order BY pm.billno", true, "Select");
            UtilityModule.ConditionalComboFill(ref ddBillno, str, true, "Select");

        }        
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            tdrec.Visible = true;
            txtBillno.Text = "";
            btnpriview.Visible = false;
            tdbillno.Visible = false;
        }
        else
        {
            tdbillno.Visible = true;
            tdrec.Visible = false;
            txtBillno.Text = "";
            btnpriview.Visible = false;
        }
        refresh();
    }
    protected void ddBillno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtBillno.Text = ddBillno.SelectedItem.Text;
       
        Fill_Grid_Show();
        ViewState["pac_mas_id"] = ddBillno.SelectedValue;
        btnpriview.Visible = true;
        report();
    }
    public void refresh()
    {
        ddcustomercode.SelectedIndex = 0;
        ddorderno.Items.Clear();
        ddempname.SelectedIndex = 0;
        ddchalanno.Items.Clear();
        ddBillno.Items.Clear();
        txtBillno.Text = "";
        ddgodown.SelectedIndex = 0;
        DGSHOWDATA.DataSource = "";
        DGSHOWDATA.DataBind();
        ChkForAllSelect.Checked = false;
        selectall.Visible = false;
    }
    private void report()
    {
        Session["ReportPath"] = "Reports/Rpt_Packing_receive_detailNEW.rpt";
        Session["CommanFormula"] = "{pakingprocessreceivedetail.packingreceiveid}=" + ViewState["pac_mas_id"] + "";
    }
    protected void btnpriview_Click(object sender, EventArgs e)
    {
        Report1();
    }
    private void Report1()
    {
        string qry = "";
        if (Convert.ToString(Session["ReportPath"]) == "Reports/Rpt_Packing_receive_detailNEW.rpt")
        {
            qry = @"SELECT CompanyInfo.CompanyName,OrderMaster.CustomerOrderNo,packingprocessreceivemaster.ReceiveNo,packingprocessreceivemaster.ReceiveDate,PakingProcessReceiveDetail.PackingReceiveId,PakingProcessReceiveDetail.Length,PakingProcessReceiveDetail.Width,
                  PakingProcessReceiveDetail.Ply,PakingProcessReceiveDetail.Qty,PakingProcessReceiveDetail.Rate,PakingProcessReceiveDetail.height,PakingProcessReceiveDetail.remarks,OrderMaster.LocalOrder,PakingProcessReceiveDetail.PackingProcessReceiveDetailId,
                  CompanyInfo.CompAddr1,CompanyInfo.CompAddr2,CompanyInfo.CompAddr3,CompanyInfo.CompFax,CompanyInfo.CompTel,CompanyInfo.Email,PakingProcessReceiveDetail.PackingType, Gd.GodownName
                  FROM   PakingProcessReceiveDetail INNER JOIN CompanyInfo INNER JOIN packingprocessreceivemaster ON CompanyInfo.CompanyId=packingprocessreceivemaster.CompanyId ON PakingProcessReceiveDetail.PackingReceiveId=packingprocessreceivemaster.PackingReceiveId
                  INNER JOIN OrderMaster ON PakingProcessReceiveDetail.OrderNo=OrderMaster.OrderId inner join godownmaster Gd on Gd.GoDownID=packingprocessreceivemaster.GodownID
                  Where pakingprocessreceivedetail.packingreceiveid=" + ViewState["pac_mas_id"] + " And Companyinfo.CompanyId=" + ddCompName.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\PGenrateIndentNEW.rpt";
            Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\Rpt_Packing_receive_detailNEW.xsd";
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

    protected void GDVSHOWORDER_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int r = Convert.ToInt32(GDVSHOWORDER.SelectedIndex.ToString());
        //ViewState["PackingCostID"] = GDVSHOWORDER.SelectedDataKey.Value;
        //ViewState["DetailID"] = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lbldetailid")).Text;
        //txtlen.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lbllength")).Text;
        //txtwidth.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblwidth")).Text;
        //txtheight.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblHeight")).Text;
        //txtgsm.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblgsm")).Text;
        //txtgsm2.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblgsm2")).Text;
        //txtply.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblply")).Text;
        //txtweight.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblunit")).Text;
        //txtremarks.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lblremark")).Text;
        //if (ChkEditOrder.Checked == true)
        //{
        //    txtdeldate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("Lbldeldate")).Text;
        //    txtdate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblodate")).Text;
        //  //  txtduedate.Text = ((Label)GDVSHOWORDER.Rows[r].FindControl("lblduedate")).Text;
        //}



        //string PkgType = GDVSHOWORDER.Rows[r].Cells[3].Text;
        //if (PkgType == "MIDDLE")
        //{
        //    DDpackingType.SelectedIndex = 1;
        //}
        //else if (PkgType == "MASTER")
        //{
        //    DDpackingType.SelectedIndex = 2;
        //}
        //else
        //{
        //    DDpackingType.SelectedIndex = 0;
        //}
        //txtItemCode.Text = GDVSHOWORDER.Rows[r].Cells[1].Text;
        //txtqty.Text = GDVSHOWORDER.Rows[r].Cells[5].Text;
        //txtrate.Text = GDVSHOWORDER.Rows[r].Cells[6].Text;
        //Txtamount.Text = GDVSHOWORDER.Rows[r].Cells[7].Text;
        //txtpcs.Text = GDVSHOWORDER.Rows[r].Cells[8].Text;
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
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GDVSHOWORDER, "Select$" + e.Row.RowIndex);
        }
    }
    protected void txtBillno_TextChanged(object sender, EventArgs e)
    {
        string billno = txtBillno.Text == "" ? "0" : txtBillno.Text.Trim().ToUpper();
        string Qry = "select  Distinct PackingReceiveId from  packingprocessreceivemaster where PartyId=" + ddempname.SelectedValue + " AND BillNo='" + billno + "'" ;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Qry);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtBillno.Text = "";
            msg = "Duplicate Bill no is not permitted !";
            MessageSave(msg);
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
    protected void DGSHOWDATA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        int i = Convert.ToInt32(DGSHOWDATA.DataKeys[e.RowIndex].Value);
        SqlParameter[] _param = new SqlParameter[3];
        _param[0] = new SqlParameter("@PRECDetailID", i);
        _param[1] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"].ToString());
        _param[2] = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
        _param[2].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_PackingReceive_Delete", _param);
        msg = _param[2].Value.ToString();
        MessageSave(msg);
        Fill_Grid_Show();
        Fill_GridData_Show();
    }
}