using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using ClosedXML.Excel;

public partial class Masters_Carpet_ProcessProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            logo();

            Session["ViewState"] = "0";

            UtilityModule.ConditionalComboFill(ref ddcompany, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "--Select--");
            if (ddcompany.Items.FindByValue(Session["CurrentWorkingCompanyID"].ToString()) != null)
            {
                ddcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddcompany.Enabled = false;
                CompanySelectedIndexChange();
            }

            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                Tdcustcode.Visible = false;
            }
            string str = @"Select PROCESS_NAME_ID, PROCESS_NAME from PROCESS_NAME_MASTER Where MasterCompanyId=" + Session["varCompanyId"];
            if (Convert.ToInt16(Session["varcompanyId"]) == 16)
            {
                str = str + " and Process_name_id in (5, 143)";
            }
            else if (Convert.ToInt16(Session["varcompanyId"]) == 44)
            {
                str = str + " and Process_name_id in (12,11,5,18,8,29,54,34,56,55)";
            }
            else if (variable.Carpetcompany == "1")
            {
                str = str + " and Process_name_id=5";
            }

            str = str + " Order by PROCESS_NAME";
            UtilityModule.ConditionalComboFill(ref ddprocess, str, true, "--Select--");
            if (ddprocess.Items.Count > 0)
            {
                ddprocess.SelectedValue = "5";
                ProcessSelectedIndexChanged();
            }
            if (variable.VarWEAVERORDERWITHOUTCUSTCODE == "1")
            {
                editandwithoutedit();
            }

            if (Convert.ToInt32(Session["varCompanyId"]) == 16)
            {
                TDProcessEmployeeName.Visible = true;
            }

            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 9:  ///Hafizia
                    ChkForItemDetailInExcel.Visible = true;
                    break;
                default:
                    ChkForItemDetailInExcel.Visible = false;
                    break;
            }
        }
    }
    protected void ddcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChange();
    }
    private void CompanySelectedIndexChange()
    {
        if (ddcompany.SelectedIndex > 0)
        {
            switch (Convert.ToInt16(Session["varcompanyId"]))
            {
                case 6:  ///Art INdia
                    UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " order by Customercode", true, "--Select--");
                    break;
                case 42:
                    UtilityModule.ConditionalComboFill(ref ddcustomer, @"SELECT DISTINCT CI.Customerid, CI.Customercode + SPACE(5) + CI.CompanyName Custcode 
                    From Ordermaster OM(Nolock)
                    JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId And CI.MasterCompanyId = " + Session["varCompanyId"] + @" 
                    JOIN CompanyWiseCustomerDetail CCD(Nolock) ON CCD.CustomerID = CI.CustomerID And CCD.CompanyID = OM.CompanyID 
                    JOIN OrderDetail OD(Nolock) ON OD.OrderId = OM.OrderId And OD.Tag_Flag = 1 
                    JOIN JobAssigns JA(nolock) ON JA.OrderId = OD.OrderId And JA.Item_Finished_Id = OD.Item_Finished_Id 
                    Where (JA.PreProdAssignedQty + JA.INTERNALPRODASSIGNEDQTY) > 0 And OM.Companyid = " + ddcompany.SelectedValue + " Order By Custcode ", true, "--Select--");
                    break;
                case 44:
                    if (Tdcustcode.Visible == true)
                    {
                        UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode  as Custcode from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " order by Custcode", true, "--Select--");
                    }
                    break;
                default:
                    if (Tdcustcode.Visible == true)
                    {
                        UtilityModule.ConditionalComboFill(ref ddcustomer, "SELECT DISTINCT CI.Customerid,CI.Customercode + SPACE(5) +CompanyName as Custcode from customerinfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 and Companyid=" + ddcompany.SelectedValue + " And CI.MasterCompanyId=" + Session["varCompanyId"] + " order by Custcode", true, "--Select--");
                    }
                    break;
            }
            ProcessSelectedIndexChanged();
        }
    }
    protected void ddcustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        editandwithoutedit();
    }
    protected void FillProcessprogramNOEdit()
    {
        string str1 = "";
        if (Session["VarCompanyNo"].ToString() == "46")
        {
            str1 = @"select Distinct P.PPID, Orderno.OrderNo + ' # ' +  cast(P.ChallanNo as varchar) 
                    From ProcessProgram P(Nolock) 
                    JOIN OrderMaster om(Nolock) on p.Order_ID=OM.OrderId 
                    JOIN customerinfo CI(Nolock) ON OM.CustomerId=CI.CustomerId
                    cross apply (select OrderNo From F_GetPPNo_OrderNo(P.PPID)) Orderno
                    Where P.Process_id=" + ddprocess.SelectedValue + " and OM.CompanyiD=" + ddcompany.SelectedValue;
        }
        else
        {
             str1 = @"select Distinct P.PPID,cast(P.ChallanNo as varchar) + ' # ' + Orderno.OrderNo 
                    From ProcessProgram P(Nolock) 
                    JOIN OrderMaster om(Nolock) on p.Order_ID=OM.OrderId 
                    JOIN customerinfo CI(Nolock) ON OM.CustomerId=CI.CustomerId
                    cross apply (select OrderNo From F_GetPPNo_OrderNo(P.PPID)) Orderno
                    Where P.Process_id=" + ddprocess.SelectedValue + " and OM.CompanyiD=" + ddcompany.SelectedValue;
        }

        

        if (Tdcustcode.Visible == true)
        {
            str1 = str1 + " and OM.CustomerId=" + ddcustomer.SelectedValue + "";
        }
        if (ChkForAllPPNo.Checked == false)
        {
            str1 = str1 + " And OM.Orderdate > GETDATE() - 100 ";
        }
        str1 = str1 + " Order By P.PPID Desc ";
        UtilityModule.ConditionalComboFill(ref ddprocessprogram, str1, true, "--Select--");
    }
    private void editandwithoutedit()
    {
        if (ChekEdit.Checked == true)
        {

            //            string str1 = @"Select DISTINCT PPID,PPID From Ordermaster OM,CustomerInfo CI,ProcessProgram P 
            //                          Where OM.OrderID=P.Order_Id And OM.CustomerId=CI.CustomerId  
            //                           And CI.MasterCompanyId=" + Session["varCompanyId"] + @" 
            //                           and P.Process_id=" + ddprocess.SelectedValue + " and OM.CompanyiD=" + ddcompany.SelectedValue;

            FillProcessprogramNOEdit();

            txtprocessprogram.Visible = false;
            lblprocessprogram.Visible = false;
            //ddprocessprogram.Visible = true;
            //lblprocessprogram1.Visible = true;
            Tdprocessprogram.Visible = true;
        }
        else
        {
            DataSet ds = null;
            try
            {
                string strsql = null;
                strsql = @"Select OrderCategoryId,OrderCategory from OrderCategory where OrderCategory like 'Purchase%'";
                ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
            }

            string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 And OM.Companyid=" + ddcompany.SelectedValue + @" and
                           CI.MasterCompanyId=" + Session["varCompanyId"] + " And OM.OrderId Not IN(Select Order_Id From ProcessProgram WHERE Process_ID=" + ddprocess.SelectedValue + @") ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                str1 = str1 + " and OM.OrderCategoryId<>" + ds.Tables[0].Rows[0]["OrderCategoryId"];
            }
            if (Tdcustcode.Visible == true)
            {
                str1 = str1 + " and OM.CustomerId=" + ddcustomer.SelectedValue;
            }
            str1 = str1 + "  Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
            lblprocessprogram.Visible = true;
            txtprocessprogram.Visible = true;
            Tdprocessprogram.Visible = false;
        }
    }
    protected void ddprocessprogram_SelectedIndexChanged(object sender, EventArgs e)
    {
        Note1.Visible = true;
        string str1 = @"Select OM.OrderId,LocalOrder+' / '+CustomerOrderNo+' / '+CustomerCode+' / '+replace(convert(varchar(11),ProdReqDate,106), ' ','-') as OrderNo 
                          From CustomerInfo CI,Ordermaster OM,OrderDetail OD,JobAssigns JA Where CI.Customerid=OM.Customerid And OD.OrderId=OM.OrderId And 
                          OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 And 
                          OM.CustomerId=" + ddcustomer.SelectedValue + @" And CI.MasterCompanyId=" + Session["varCompanyId"] + @" And OM.OrderId Not IN(Select Order_Id From ProcessProgram) 
                          Group By  OM.Orderid,LocalOrder,CustomerOrderNo,CustomerCode,ProdReqDate Order By ProdReqDate ASC";
        UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str1);
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            fill_ConsumptionGride();
            string strsql = @"Select Distinct O.orderid,LocalOrder+' / '+Customerorderno +' / '+CustomerCode+' / '+replace(convert(varchar(11),isnull(ProdReqDate,''),106), ' ','-') as OrderNo from ordermaster o inner join OrderDetail OD on OD.OrderId=o.OrderId inner join Customerinfo C on o.Customerid=C.Customerid  inner join processprogram on o.orderid=order_id and ppid=" + ddprocessprogram.SelectedValue + " And C.MasterCompanyId=" + Session["varCompanyId"] + @" 
                            Select EmpID From ProcessProgramWithEmp(Nolock) Where PPID = " + ddprocessprogram.SelectedValue;
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
                                      JobAssigns JA Where OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id  And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 And 
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
            for (int j = 0; j < ChkBoxListProcessEmployeName.Items.Count; j++)
            {
                ChkBoxListProcessEmployeName.Items[j].Selected = false;
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    for (int j = 0; j < ChkBoxListProcessEmployeName.Items.Count; j++)
                    {
                        if (Convert.ToInt32(ChkBoxListProcessEmployeName.Items[j].Value) == Convert.ToInt32(ds.Tables[1].Rows[i]["EmpID"]))
                        {
                            ChkBoxListProcessEmployeName.Items[j].Selected = true;
                        }
                    }
                }
            }

            fill_OrderConsumption();
            BtnPreview.Visible = true;
            btndel.Visible = true;
            BtnLocalOcReport.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
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
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //*************
        int Chkcount = 0;
        int n = 0;
        n = chekboxlist.Items.Count;
        for (int i = 0; i < n; i++)
        {
            if (chekboxlist.Items[i].Selected)
            {
                Chkcount += 1;
            }
        }
        if (Chkcount == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Please check atleast one Order No. to save Data.')", true);
            return;
        }
        //*************
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
            SqlParameter[] _arrPara = new SqlParameter[9];
            _arrPara[0] = new SqlParameter("@PP_Detail_ID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@Process_Id", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@Order_Id", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@PPid", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@varuserid", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@varCompanyId", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@CHALLANNO", SqlDbType.VarChar, 150);
            _arrPara[8] = new SqlParameter("@EmpID", SqlDbType.VarChar, 4000);

            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddprocess.SelectedValue;
            _arrPara[3].Direction = ParameterDirection.InputOutput;
            _arrPara[3].Value = Convert.ToInt32(ppid);
            _arrPara[4].Value = Session["varuserid"].ToString();
            _arrPara[5].Value = Session["varCompanyId"].ToString();
            _arrPara[6].Value = ddcompany.SelectedValue;
            _arrPara[7].Direction = ParameterDirection.InputOutput;
            _arrPara[7].Value = "";

            string EmpID = "";
            for (int i = 0; i < ChkBoxListProcessEmployeName.Items.Count; i++)
            {
                if (ChkBoxListProcessEmployeName.Items[i].Selected)
                {
                    if (EmpID == "")
                    {
                        EmpID = ChkBoxListProcessEmployeName.Items[i].Value + '|';
                    }
                    else
                    {
                        EmpID = EmpID + ChkBoxListProcessEmployeName.Items[i].Value + '|';
                    }
                }
            }
            _arrPara[8].Value = EmpID;

            n = chekboxlist.Items.Count;
            for (int i = 0; i < n; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {
                    _arrPara[2].Value = chekboxlist.Items[i].Value;
                    SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "PRO_ProcessProgram", _arrPara);
                }
            }
            //txtprocessprogram.Text = _arrPara[3].Value.ToString();

            //if (Session["VarCompanyId"].ToString() == "30")
            //{
                Session["ViewState"] = _arrPara[3].Value.ToString();
            //}

            txtprocessprogram.Text = _arrPara[7].Value.ToString();
            BtnPreview.Visible = true;
            BtnLocalOcReport.Visible = false;
            //*************GET orderid
            string orderid = Convert.ToString(SqlHelper.ExecuteScalar(tran, CommandType.Text, @" SELECT DISTINCT  STUFF((SELECT distinct ',' + cast(p1.order_id as varchar)
                                                                             FROM ProcessProgram p1
                                                                             WHERE p.PPID = p1.PPID
                                                                                FOR XML PATH(''), TYPE
                                                                                ).value('.', 'NVARCHAR(MAX)')
                                                                            ,1,1,'') orderid
                                                                    FROM processprogram p where ppid=" + _arrPara[3].Value + ""));
            if (orderid == "")
            {
                orderid = "0";
            }
            //*********************GET orderid

            string Str = "insert into PP_Consumption (PPId ,FinishedId,OrderId,OrderDetailId,Qty,ExtraQty,UserId,MasterCompanyId,Loss,LossQty,ConsmpQty,IFinishedid,IUNITID) ";
            //carpetcompany
            switch (variable.Carpetcompany)
            {
                case "1":
                    switch (variable.DyingProgramWithFullArea)
                    {
                        case "1":
                            if ((Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "28" || Session["varcompanyid"].ToString() == "44" || Session["varcompanyid"].ToString() == "39") && ddprocess.SelectedValue.ToString() == "5")
                            {
                                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,
                                        Round(CASE WHEN ORDERUNITID in (1, 6) THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY * Case When VF.PoufTypeCategory = 1 Then 1 Else 
                                        Case When OCD.ICalType = 0 OR OCD.ICalType = 2 Then VF.AreaMtr * 1.196 Else 1 End END ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When VF.MasterCompanyId in (16, 28) Then 
                                        Case When VF.PoufTypeCategory = 1 Then 1 Else Case When OCD.ICalType = 0 Then Round(VF.AreaFt * 144.0 / 1296, 4, 1) Else 1 End End Else VF.Actualfullareasqyd END END ,5) QTY,
                                        0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
                                        Round(CASE WHEN ORDERUNITID=1  THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OLoss*1.196*Case When VF.PoufTypeCategory = 1 Then 1 Else 
                                        Case When OCD.ICalType = 0 Then VF.AreaMtr Else 1 End END ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) *OCD.OLoss*Case When VF.PoufTypeCategory = 1 Then 1 Else 
                                        Case When OCD.ICalType = 0 Then VF.Actualfullareasqyd Else 1 End End END, 5) LossQty,
                                        Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID 
                                        FROM PROCESSPROGRAM PP(Nolock) 
                                        JOIN ORDERMASTER OM(Nolock) ON OM.ORDERID = PP.ORDER_ID 
                                        JOIN ORDERDETAIL OD(Nolock) ON OM.ORDERID = OD.ORDERID 
                                        JOIN OrderDetailDetail ODD(Nolock) ON ODD.OrderID = OD.OrderID And ODD.ORDERDETAILID = OD.ORDERDETAILID 
                                        JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.ORDERDETAILID = OD.ORDERDETAILID AND OCD.ORDERID = OM.OrderId And OCD.PROCESSID = PP.Process_ID 
		                                        And OCD.ORDERDETAILDETAILID = ODD.ORDERDETAILDETAILID 
                                        JOIN JobAssigns js ON Js.OrderId = Od.OrderId And js.ITEM_FINISHED_ID = Od.Item_Finished_Id 
                                        JOIN V_FinishedItemDetail VF ON VF.ITEM_FINISHED_ID = ODD.OrderDetailDetail_Item_Finished_Id 
                                        WHERE PP.PPID = " + _arrPara[3].Value + " And PP.Process_ID = " + _arrPara[1].Value + " And PP.Order_Id in (" + orderid + ") And PP.MasterCompanyID = " + Session["varCompanyId"];
                            }
                            else if (Session["varcompanyid"].ToString() == "44")
                            {
                                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY * Case When VF.PoufTypeCategory = 1 Then 1 Else 
                                        Case When OCD.ICalType = 0 OR OCD.ICalType = 2 Then S.AreaMtr * 1.196 Else 1 End END ELSE case when vf.MasterCompanyId=44 then OD.QTYREQUIRED * (OCD.IQTY + OCD.ILoss) else (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When VF.MasterCompanyId in (16, 28) Then 
                                        Case When VF.PoufTypeCategory = 1 Then 1 Else Case When OCD.ICalType = 0 Then Round(S.AreaFt * 144.0 / 1296, 4, 1) Else 1 End End Else S.Actualfullareasqyd END END END ,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",
                                        Round(CASE WHEN ORDERUNITID=1 THEN 1.196 ELSE 1 End * Case When IPM.MasterCompanyId = 42 Then 
		                                IsNull((Select Top 1 LossPercentage from QualityLoss QL(Nolock) Where QL.MonthID = Month(GetDate()) And QL.QualityID = VF.QualityID), 0)
		                                Else OCD.OLoss End, 5) OLoss,
                                            Round(CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OQTY*1.196
                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When IPM.MasterCompanyId in (16, 28) Then Round(S.AreaFt * 144.0 / 1296, 4, 1) Else S.Actualfullareasqyd END END ,5) * 
                                        Case When IPM.MasterCompanyId = 42 Then 
		                                IsNull((Select Top 1 LossPercentage * 0.01 from QualityLoss QL(Nolock) Where QL.MonthID = Month(GetDate()) And QL.QualityID = VF.QualityID), 0) 
		                                Else OCD.OLoss End LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID  
                                        FROM PROCESSPROGRAM PP(Nolock)
                                        JOIN ORDERMASTER OM(Nolock) ON OM.ORDERID=PP.Order_ID And OM.OrderId in(" + orderid + @")
                                        JOIN ORDERDETAIL OD(Nolock) ON OD.ORDERID=OM.ORDERID 
                                        JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.ORDERDETAILID=OD.ORDERDETAILID 
                                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                                        JOIN JobAssigns js(Nolock) ON Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id 
                                        JOIN ITEM_PARAMETER_MASTER IPM(Nolock) ON OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And IPM.MasterCompanyId=" + Session["varCompanyId"] + @" 
                                        Left Outer Join SIZE S(Nolock) ON IPM.Size_id=S.Sizeid 
                                        WHERE PPID=" + _arrPara[3].Value + " And ProcessId= " + _arrPara[1].Value;



                            
                            
                            }
                            else
                            {
                                //                                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OQTY*1.196
                                //                                          ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When IPM.MasterCompanyId in (16, 28) Then Round(S.AreaFt * 144.0 / 1296, 4, 1) Else S.Actualfullareasqyd END END ,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
                                //                                          Round(CASE WHEN ORDERUNITID=1  THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OLoss*1.196 ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.Actualfullareasqyd *OCD.OLoss END ,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid 
                                //                                          FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,JobAssigns js,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S 
                                //                                          ON IPM.Size_id=S.Sizeid 
                                //                                          WHERE Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id and OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
                                //                                          PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId in(" + orderid + ") and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

                                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY * Case When VF.PoufTypeCategory = 1 Then 1 Else 
                                        Case When OCD.ICalType = 0 OR OCD.ICalType = 2 Then S.AreaMtr * 1.196 Else 1 End END ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When VF.MasterCompanyId in (16, 28) Then 
                                        Case When VF.PoufTypeCategory = 1 Then 1 Else Case When OCD.ICalType = 0 Then Round(S.AreaFt * 144.0 / 1296, 4, 1) Else 1 End End Else S.Actualfullareasqyd END END ,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",
                                        Round(CASE WHEN ORDERUNITID=1 THEN 1.196 ELSE 1 End * Case When IPM.MasterCompanyId = 42 Then 
		                                IsNull((Select Top 1 LossPercentage from QualityLoss QL(Nolock) Where QL.MonthID = Month(GetDate()) And QL.QualityID = VF.QualityID), 0)
		                                Else OCD.OLoss End, 5) OLoss,
                                            Round(CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OQTY*1.196
                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY) * OCD.OQTY * Case When IPM.MasterCompanyId in (16, 28) Then Round(S.AreaFt * 144.0 / 1296, 4, 1) Else S.Actualfullareasqyd END END ,5) * 
                                        Case When IPM.MasterCompanyId = 42 Then 
		                                IsNull((Select Top 1 LossPercentage * 0.01 from QualityLoss QL(Nolock) Where QL.MonthID = Month(GetDate()) And QL.QualityID = VF.QualityID), 0) 
		                                Else OCD.OLoss End LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID  
                                        FROM PROCESSPROGRAM PP(Nolock)
                                        JOIN ORDERMASTER OM(Nolock) ON OM.ORDERID=PP.Order_ID And OM.OrderId in(" + orderid + @")
                                        JOIN ORDERDETAIL OD(Nolock) ON OD.ORDERID=OM.ORDERID 
                                        JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.ORDERDETAILID=OD.ORDERDETAILID 
                                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
                                        JOIN JobAssigns js(Nolock) ON Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id 
                                        JOIN ITEM_PARAMETER_MASTER IPM(Nolock) ON OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And IPM.MasterCompanyId=" + Session["varCompanyId"] + @" 
                                        Left Outer Join SIZE S(Nolock) ON IPM.Size_id=S.Sizeid 
                                        WHERE PPID=" + _arrPara[3].Value + " And ProcessId= " + _arrPara[1].Value;
                            }
                            break;
                        default:
                            switch (Session["varcompanyid"].ToString())
                            {
                                case "9":
                                    Str = Str + @" Select PP.PPID, OFINISHEDID FinishedId, OM.OrderId, OD.OrderDetailId, 
                                                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196 
																		                                                    Else OD.QTYREQUIRED * OD.TotalArea * OCD.OQTY * 1.196 
																                                                     End 
									                                                    ELSE Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OQTY 
												                                                    Else OD.QTYREQUIRED * OD.TotalArea * OCD.OQTY * 1.196/10.76391  
											                                                    End 
									                                                    END 
	                                                    ELSE 
		                                                    CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OQTY*1.196 
																		                                                    Else OD.QTYREQUIRED * OCD.OQTY * 1.196 
										                                                    End
								                                                    ELSE Case When OCD.MasterCompanyId <>9 Then  OD.QTYREQUIRED*OCD.OQTY Else OD.QTYREQUIRED*OCD.OQTY/10.76391 End 
		                                                    END 
                                                    END,5) QTY, 0.00, 1, " + Session["varcompanyId"].ToString() + @", 
                                                    Round(CASE WHEN ORDERUNITID = 1 THEN Case When OCD.MasterCompanyId <>9 Then OCD.OLoss*1.196 Else OCD.OLoss*1.196 End  ELSE Case When OCD.Mastercompanyid <>9 Then  OLoss Else OLoss*1.196 End End,5) OLoss, 
                                                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
                                                    THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196 Else OD.QTYREQUIRED*OD.TotalArea*OCD.OLoss * 1.196 End
                                                    ELSE Case  When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OLoss Else OD.QTYREQUIRED*OD.TotalArea*OCD.OLoss * 1.196/10.76391 END END ELSE 
                                                    CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OLoss*1.196 Else OD.QTYREQUIRED*OCD.OLoss End
                                                    ELSE Case When OCD.Mastercompanyid <>9 Then  OD.QTYREQUIRED*OCD.OLoss Else OD.QTYREQUIRED*OCD.OLoss * 1.196/10.76391  END END END,5) LossQty, 
                                                    Round(CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY*1.196 Else OCD.OQTY * 1.196 End ELSE Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY  Else OCD.OQty * 1.196 End End,5) OQTY, OCD.IFinishedid,OCD.IUNITID  
                                                    FROM PROCESSPROGRAM PP(Nolock) 
                                                    JOIN ORDERMASTER OM(Nolock) ON OM.OrderID = PP.Order_ID 
                                                    JOIN ORDERDETAIL OD(Nolock) ON OD.OrderID = OM.OrderID 
                                                    JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.ORDERDETAILID = OD.OrderDetailId And OCD.PROCESSID = PP.Process_ID 
                                                    JOIN ITEM_PARAMETER_MASTER IPM(Nolock) ON IPM.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                                                    Left Outer Join SIZE S ON S.Sizeid = IPM.Size_id 
                                                    Where PP.PPID = " + _arrPara[3].Value + " and PP.Order_Id in (" + orderid + ") And PP.Process_ID = " + _arrPara[1].Value + " And PP.MasterCompanyid = " + Session["varcompanyId"];

                                    //                                    Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,
                                    //                                        Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1  
                                    //                                        THEN Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196 
                                    //                                        Else OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY End
                                    //                                        ELSE Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OQTY Else OD.QTYREQUIRED*S.AreaFt*OCD.OQTY/10.76391  End END ELSE CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OQTY*1.196 Else OD.QTYREQUIRED*OCD.OQTY End
                                    //                                        ELSE Case When OCD.MasterCompanyId <>9 Then  OD.QTYREQUIRED*OCD.OQTY Else OD.QTYREQUIRED*OCD.OQTY/10.76391 End END END,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN Case When OCD.MasterCompanyId <>9 Then OCD.OLoss*1.196 Else OCD.OLoss End  ELSE Case When OCD.Mastercompanyid <>9 Then  OLoss Else OLoss/10.76391 End End,5) OLoss,
                                    //                                        Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
                                    //                                        THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196 Else OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss End
                                    //                                        ELSE Case  When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OLoss Else OD.QTYREQUIRED*S.AreaFt*OCD.OLoss/10.76391 END END ELSE CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OLoss*1.196 Else OD.QTYREQUIRED*OCD.OLoss End
                                    //                                        ELSE Case When OCD.Mastercompanyid <>9 Then  OD.QTYREQUIRED*OCD.OLoss Else OD.QTYREQUIRED*OCD.OLoss/10.76391  END END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY*1.196 Else OCD.OQTY End ELSE Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY  Else OCD.OQty/10.76391 End End,5) OQTY,OCD.IFinishedid 
                                    //                                        FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
                                    //                                        WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
                                    //                                        PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And
                                    //                                        PPID=" + _arrPara[3].Value + " and OM.OrderId in(" + orderid + ") and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varcompanyId"];

                                    break;
                                default:
                                    switch (variable.VarDyeingprogramwithExportArea)
                                    {
                                        case "1":
                                            Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OQTY*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY END END,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
                                                        Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
                                                        THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaMtr*OCD.OLoss*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.AreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OLoss*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID  FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,JobAssigns js,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
                                                        WHERE Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id and OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
                                                        PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId in(" + orderid + ") and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

                                            break;

                                        default:
                                            string Caltycolum = "ordercaltype";
                                            if (variable.Varkatiwithbomcaltype == "1")
                                            {
                                                Caltycolum = "OCD.Icaltype";

                                            }
                                            Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN " + Caltycolum + @"=0  THEN CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.ProdAreaMtr*OCD.OQTY*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.ProdAreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OQTY END END,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
                                                        Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
                                                        THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.ProdAreaMtr*OCD.OLoss*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*S.ProdAreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OLoss*1.196
                                                        ELSE (js.PreProdAssignedQty+Js.INTERNALPRODASSIGNEDQTY)*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID  FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,JobAssigns js,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
                                                        WHERE Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id and OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
                                                        PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId in(" + orderid + ") and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

                                            break;
                                    }

                                    break;
                            }
                            break;
                    }
                    break;
                //No carpet Company
                default:
                    Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196
                                        ELSE OD.QTYREQUIRED*S.AreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OQTY
                                        ELSE OD.QTYREQUIRED*OCD.OQTY END END,5) QTY,0.00,1," + Session["varcompanyNo"] + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
                                        Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
                                        THEN OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196
                                        ELSE OD.QTYREQUIRED*S.AreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OLoss*1.196
                                        ELSE OD.QTYREQUIRED*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid,OCD.IUNITID  FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
                                        WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
                                        PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId in(" + orderid + ") and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
                    break;
            }
            //
            #region
            //            if (Session["VarcompanyNo"].ToString() == "5")
            //            {
            //                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN 
            //                    OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196 ELSE OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196 END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OQTY*1.196
            //                    ELSE OD.QTYREQUIRED*OCD.OQTY END END,5) QTY,0.00,1,5,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
            //                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
            //                    THEN OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196
            //                    ELSE OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196 END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OLoss*1.196
            //                    ELSE OD.QTYREQUIRED*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
            //                    WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
            //                    PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId=" + _arrPara[2].Value + " and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
            //            }
            //            else if (Session["VarcompanyNo"].ToString() == "4")
            //            {

            //                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*S.ProdAreaMtr*OCD.OQTY*1.196
            //                    ELSE js.PreProdAssignedQty*S.ProdAreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*OCD.OQTY*1.196
            //                    ELSE js.PreProdAssignedQty*OCD.OQTY END END,5) QTY,0.00,1,4,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
            //                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
            //                    THEN js.PreProdAssignedQty*S.ProdAreaMtr*OCD.OLoss*1.196
            //                    ELSE js.PreProdAssignedQty*S.ProdAreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*OCD.OLoss*1.196
            //                    ELSE js.PreProdAssignedQty*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,JobAssigns js,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
            //                    WHERE Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id and OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
            //                    PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId=" + _arrPara[2].Value + " and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //            }
            //            else if (Session["varcompanyNo"].ToString() == "15")
            //            {
            //                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*S.ProdAreaMtr*OCD.OQTY*1.196
            //                    ELSE js.PreProdAssignedQty*S.ProdAreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*OCD.OQTY*1.196
            //                    ELSE js.PreProdAssignedQty*OCD.OQTY END END,5) QTY,0.00,1," + Session["varcompanyno"] + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
            //                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
            //                    THEN js.PreProdAssignedQty*S.ProdAreaMtr*OCD.OLoss*1.196
            //                    ELSE js.PreProdAssignedQty*S.ProdAreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN js.PreProdAssignedQty*OCD.OLoss*1.196
            //                    ELSE js.PreProdAssignedQty*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,JobAssigns js,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
            //                    WHERE Js.OrderId=Od.OrderId and js.ITEM_FINISHED_ID=Od.Item_Finished_Id and OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
            //                    PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId=" + _arrPara[2].Value + " and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];

            //            }
            //            else if (Session["varcompanyNo"].ToString() == "9" || Session["varcompanyNo"].ToString() == "14" || Session["varcompanyNo"].ToString() == "8")
            //            {

            //                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,
            //                            Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1  
            //                            THEN Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196 
            //                            Else OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY End
            //                            ELSE Case When OCD.MasterCompanyId <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OQTY Else OD.QTYREQUIRED*S.AreaFt*OCD.OQTY/10.76391  End END ELSE CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OQTY*1.196 Else OD.QTYREQUIRED*OCD.OQTY End
            //                            ELSE Case When OCD.MasterCompanyId <>9 Then  OD.QTYREQUIRED*OCD.OQTY Else OD.QTYREQUIRED*OCD.OQTY/10.76391 End END END,5) QTY,0.00,1," + Session["varcompanyId"].ToString() + @",Round(CASE WHEN ORDERUNITID=1 THEN Case When OCD.MasterCompanyId <>9 Then OCD.OLoss*1.196 Else OCD.OLoss End  ELSE Case When OCD.Mastercompanyid <>9 Then  OLoss Else OLoss/10.76391 End End,5) OLoss,
            //                            Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
            //                            THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196 Else OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss End
            //                            ELSE Case  When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*S.AreaFt*OCD.OLoss Else OD.QTYREQUIRED*S.AreaFt*OCD.OLoss/10.76391 END END ELSE CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then OD.QTYREQUIRED*OCD.OLoss*1.196 Else OD.QTYREQUIRED*OCD.OLoss End
            //                            ELSE Case When OCD.Mastercompanyid <>9 Then  OD.QTYREQUIRED*OCD.OLoss Else OD.QTYREQUIRED*OCD.OLoss/10.76391  END END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY*1.196 Else OCD.OQTY End ELSE Case When OCD.Mastercompanyid <>9 Then  OCD.OQTY  Else OCD.OQty/10.76391 End End,5) OQTY,OCD.IFinishedid 
            //                            FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
            //                            WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
            //                            PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And
            //                            PPID=" + _arrPara[3].Value + " and OM.OrderId=" + _arrPara[2].Value + " and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varcompanyId"];

            //            }
            //            else
            //            {
            //                Str = Str + @"Select PP.PPID,OFINISHEDID FinishedId,OM.OrderId,OD.OrderDetailId,Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*S.AreaMtr*OCD.OQTY*1.196
            //                    ELSE OD.QTYREQUIRED*S.AreaFt*OCD.OQTY END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OQTY
            //                    ELSE OD.QTYREQUIRED*OCD.OQTY END END,5) QTY,0.00,1," + Session["varcompanyNo"] + @",Round(CASE WHEN ORDERUNITID=1 THEN OCD.OLoss*1.196 ELSE OLoss End,5) OLoss,
            //                    Round(CASE WHEN OrderCalType=0 THEN CASE WHEN ORDERUNITID=1 
            //                    THEN OD.QTYREQUIRED*S.AreaMtr*OCD.OLoss*1.196
            //                    ELSE OD.QTYREQUIRED*S.AreaFt*OCD.OLoss END ELSE CASE WHEN ORDERUNITID=1 THEN OD.QTYREQUIRED*OCD.OLoss*1.196
            //                    ELSE OD.QTYREQUIRED*OCD.OLoss END END,5) LossQty,Round(CASE WHEN ORDERUNITID=1 THEN OCD.OQTY*1.196 ELSE OCD.OQTY End,5) OQTY,OCD.IFinishedid FROM ORDER_CONSUMPTION_DETAIL OCD,ORDERDETAIL OD,ORDERMASTER OM,PROCESSPROGRAM PP,ITEM_PARAMETER_MASTER IPM Left Outer Join SIZE S ON IPM.Size_id=S.Sizeid 
            //                    WHERE OCD.ORDERDETAILID=OD.ORDERDETAILID And OM.ORDERID=OD.ORDERID AND 
            //                    PP.ORDER_ID=OD.ORDERID and OD.ITEM_FINISHED_ID=IPM.ITEM_FINISHED_ID And PPID=" + _arrPara[3].Value + " and OM.OrderId=" + _arrPara[2].Value + " and ProcessId=" + _arrPara[1].Value + " And IPM.MasterCompanyId=" + Session["varCompanyId"];
            //            }
            #endregion
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, Str);

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
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
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
                _arrPara[6].Value = Session["varCompanyId"].ToString();

                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "Update Top(1) PP_Consumption Set ExtraQty=" + _arrPara[4].Value + " Where PPID=" + _arrPara[0].Value + " and OrderId=(Select Order_Id From ProcessProgram Where PPID=" + _arrPara[0].Value + ") and FinishedId=" + _arrPara[1].Value);

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
        try
        {
            string strsql = null;
            strsql = @"Select P.PP_Detail_ID as Sr_No,o.LocalOrder+' / '+customerorderno CustomerOrderNo from processProgram p ,orderMaster o where p.order_id=o.orderid and o.CustomerId=0 And P.MasterCompanyId=" + Session["varCompanyId"];
            if (ChekEdit.Checked == true)
            {
                strsql = @"Select P.PP_Detail_ID as Sr_No,o.LocalOrder+' / '+customerorderno CustomerOrderNo from processProgram p ,orderMaster o where p.order_id=o.orderid and o.CustomerId=" + ddcustomer.SelectedValue + " And P.MasterCompanyId=" + Session["varCompanyId"];
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
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
            Tdprocessprogram.Visible = true;
            FillProcessprogramNOEdit();
            TDTxtOrderNo.Visible = true;
        }
        else
        {
            lblprocessprogram.Visible = true;
            txtprocessprogram.Visible = true;
            Tdprocessprogram.Visible = false;
            TDTxtOrderNo.Visible = false;
            string str = "select Distinct OD.orderid,LocalOrder+' / '+customerorderno as OrderNo from ordermaster o inner join OrderDetail OD on OD.OrderId=o.OrderId where OD.Tag_Flag=1 and  o.orderid not in(select order_id from processProgram)";
            if (Tdcustcode.Visible == true)
            {
                str = str + " and o.customerid='" + ddcustomer.SelectedValue + "'";
            }
            UtilityModule.ConditonalChkBoxListFill(ref chekboxlist, str);
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
                         INNER JOIN ITEM_MASTER IM ON IM.ITEM_ID=FI2.ITEM_ID inner join OrderMaster OM on OM.OrderId=PC.OrderId where PPID=" + ddprocessprogram.SelectedValue + " And IM.MasterCompanyId=" + Session["varCompanyId"] + " Group BY PC.FINISHEDID,OM.LocalOrder,ITEM_NAME,FI2.Quality,FI2.ShadeColor ";
            }
            else
            {
                strsql = "";
            }
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            txttotalconsmpqty.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalconsmpqty.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("sum(QTY)", "")), 3).ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
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
        BtnLocalOcReport.Visible = true;
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
            Label lblConsmpOrderDetailId = (Label)e.Row.FindControl("lblConsmpOrderDetailId");
            string CellValue = lblConsmpOrderDetailId.Text;
            if (CellValue == "0")
            {
                e.Row.BackColor = System.Drawing.Color.Red;
                //txtgreen.Visible = true;
                //Note.Text = "Consumption Not Define";
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.Green;
            }

        }
    }
    private DataSet Fill_OrderConsumption_Data()
    {
        DataSet ds = null;
        try
        {
            string orderid = "";
            //Get Orderid
            for (int i = 0; i < chekboxlist.Items.Count; i++)
            {
                if (chekboxlist.Items[i].Selected)
                {

                    orderid = orderid + "," + chekboxlist.Items[i].Value;
                }
            }
            orderid = orderid.TrimStart(',');
            //
            string strsql = null;
            if (Convert.ToInt32(Session["varCompanyId"]) == 9)
            {
                strsql = @"Select Distinct OM.OrderId OrderDetailId,Item_Name+' / '+ isnull(QualityName,'')+' / '+isnull(DesignName,'') +' / '+isnull(ColorName,'')+' / '+
                    isnull(ShapeName,'')+' / '+ Case When OD.flagsize=1 Then isnull(SizeMtr,'') 
                    When OD.flagsize=0 Then isnull(SizeFt,'')
                    When OD.flagsize=2 Then isnull(SizeInch,'') Else isnull(SizeFt,'') End+'   '+isnull(ShadeColorName,'') Description,
                    Sum((PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)) QTY,[dbo].[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT_PROCESSPROGRAM] (OM.Orderid,JA.Item_Finished_Id,'" + ddprocess.SelectedItem.Text + @"') ConsmpOrderDetailId,
                    Sum(OD.TotalArea * OD.QtyRequired) Area 
                    From OrderMaster OM,OrderDetail OD,V_FinishedItemDetail IPM,Jobassigns JA,Mastersetting ms
                    Where OM.Orderid=OD.Orderid And OD.Item_Finished_Id=JA.Item_Finished_Id And OM.Orderid=JA.Orderid And IPM.Item_Finished_Id=JA.Item_Finished_Id And 
                    IPM.Mastercompanyid=ms.varcompanyNo And
                    PreProdAssignedQty>0 And OM.Orderid in(" + orderid + " ) And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By OM.OrderId,Item_Name,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OrderUnitID,SizeMtr,SizeFt,SizeInch,Od.flagsize,JA.Item_Finished_Id";
            }

            else if (Convert.ToInt32(Session["varCompanyId"]) == 42)
            {
                strsql = @"Select OM.OrderId OrderDetailId,Item_Name+' / '+ isnull(QualityName,'')+' / '+isnull(DesignName,'') +' / '+isnull(ColorName,'')+' / '+
                    isnull(ShapeName,'')+' / '+ Case When OD.flagsize=1 Then isnull(SizeMtr,'') 
                    When OD.flagsize=0 Then isnull(SizeFt,'')
                    When OD.flagsize=2 Then isnull(SizeInch,'') Else isnull(SizeFt,'') End+'   '+isnull(ShadeColorName,'') Description,
                    Sum(OD.QtyRequired) QTY,
                    [dbo].[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT_PROCESSPROGRAM] (OM.Orderid,OD.Item_Finished_Id,'DYEING') ConsmpOrderDetailId, 
                    Sum(Case When OD.OrderUnitID = 1 Then ROUND(OD.Qtyrequired*IPM.AREAMTR,MS.RoundMtrFlag,Ms.RoundMtrFlag) Else 
                    ROUND(OD.Qtyrequired*Round(IPM.ACTUALFULLAREASQYD,MS.RoundFtFlag,MS.RoundFtFlag), MS.RoundFtFlag, Ms.RoundFtFlag) End) AREA 
                    From OrderMaster OM(Nolock)
                    JOIN OrderDetail OD(Nolock) ON OM.Orderid=OD.Orderid 
                    JOIN V_FinishedItemDetail IPM(Nolock) ON IPM.ITEM_FINISHED_ID = OD.Item_Finished_Id 
                    JOIN MasterSetting MS(Nolock) ON MS.VarCompanyNo = IPM.MasterCompanyId 
                    Where OM.Orderid in(" + orderid + ") And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By OM.OrderId,Item_Name,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OrderUnitID,SizeMtr,SizeFt,SizeInch,Od.flagsize, 
                    OD.Item_Finished_Id ";
            }
            else if (Convert.ToInt32(Session["varCompanyId"]) == 44)
            {
                strsql = @"Select Distinct OM.OrderId OrderDetailId,Item_Name+' / '+ isnull(QualityName,'')+' / '+isnull(DesignName,'') +' / '+isnull(ColorName,'')+' / '+
                    isnull(ShapeName,'')+' / '+ Case When OD.flagsize=1 Then cast(WidthMtr as varchar)+'x'+cast(LengthMtr as varchar) +case when HeightMtr>0 then 'x'+cast(HeightMtr as varchar) else '' end
                    When OD.flagsize=0 Then cast(WidthFt as varchar)+'x'+cast(LengthFt as varchar) +case when Heightft>0 then 'x'+cast(HeightFt as varchar) else ''  end
                    When OD.flagsize=2 Then cast(WidthInch as varchar)+'x'+cast(LengthInch as varchar) +case when HeightInch>0 then 'x'+cast(HeightInch as varchar) else ''  end Else isnull(SizeFt,'') End+'   '+isnull(ShadeColorName,'') Description,
                    Sum((PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)) QTY,[dbo].[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT_PROCESSPROGRAM] (OM.Orderid,JA.Item_Finished_Id,'" + ddprocess.SelectedItem.Text + @"') ConsmpOrderDetailId,
                    CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN SUM(CASE WHEN MS.DYINGPROGRAMWITHFULLAREA=1   THEN ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.ACTUALFULLAREASQYD,4) 
                    ELSE CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAFT,4)  
                    ELSE ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR*1.196,4) END END) ELSE 
                    CASE WHEN OD.ORDERUNITID=1 THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR),4) ELSE 
                    CASE WHEN MS.DYINGPROGRAMWITHFULLAREA=1   THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.ACTUALFULLAREASQYD),4) 
                    ELSE CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAFT),4)  
                    ELSE ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR*1.196),4) END END  END
                    END AS AREA 
                    From OrderMaster OM,OrderDetail OD,V_FinishedItemDetail IPM,Jobassigns JA,Mastersetting ms
                    Where OM.Orderid=OD.Orderid And OD.Item_Finished_Id=JA.Item_Finished_Id And OM.Orderid=JA.Orderid And IPM.Item_Finished_Id=JA.Item_Finished_Id And 
                    IPM.Mastercompanyid=ms.varcompanyNo And (JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY) > 0 And 
                    OM.Orderid in(" + orderid + " ) And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By OM.OrderId,Item_Name,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OrderUnitID,SizeMtr,SizeFt,SizeInch,Od.flagsize,
                    JA.Item_Finished_Id,MS.DYEINGPROGRAMWITHEXPORTAREA,MS.DYINGPROGRAMWITHFULLAREA,WidthInch,LengthInch,HeightInch,WidthMtr,LengthMtr,HeightMtr,WidthFt,HeightFt,LengthFt";
            }
            else
            {
                strsql = @"Select Distinct OM.OrderId OrderDetailId,Item_Name+' / '+ isnull(QualityName,'')+' / '+isnull(DesignName,'') +' / '+isnull(ColorName,'')+' / '+
                    isnull(ShapeName,'')+' / '+ Case When OD.flagsize=1 Then isnull(SizeMtr,'') 
                    When OD.flagsize=0 Then isnull(SizeFt,'')
                    When OD.flagsize=2 Then isnull(SizeInch,'') Else isnull(SizeFt,'') End+'   '+isnull(ShadeColorName,'') Description,
                    Sum((PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)) QTY,[dbo].[GET_ORDER_CONSUMPTION_DEFINE_OR_NOT_PROCESSPROGRAM] (OM.Orderid,JA.Item_Finished_Id,'" + ddprocess.SelectedItem.Text + @"') ConsmpOrderDetailId,
                    CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN SUM(CASE WHEN MS.DYINGPROGRAMWITHFULLAREA=1   THEN ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.ACTUALFULLAREASQYD,4) 
                    ELSE CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAFT,4)  
                    ELSE ROUND((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR*1.196,4) END END) ELSE 
                    CASE WHEN OD.ORDERUNITID=1 THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR),4) ELSE 
                    CASE WHEN MS.DYINGPROGRAMWITHFULLAREA=1   THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.ACTUALFULLAREASQYD),4) 
                    ELSE CASE WHEN MS.DYEINGPROGRAMWITHEXPORTAREA=1 THEN ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAFT),4)  
                    ELSE ROUND(SUM((JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY)*IPM.AREAMTR*1.196),4) END END  END
                    END AS AREA 
                    From OrderMaster OM,OrderDetail OD,V_FinishedItemDetail IPM,Jobassigns JA,Mastersetting ms
                    Where OM.Orderid=OD.Orderid And OD.Item_Finished_Id=JA.Item_Finished_Id And OM.Orderid=JA.Orderid And IPM.Item_Finished_Id=JA.Item_Finished_Id And 
                    IPM.Mastercompanyid=ms.varcompanyNo And (JA.PREPRODASSIGNEDQTY+JA.INTERNALPRODASSIGNEDQTY) > 0 And 
                    OM.Orderid in(" + orderid + " ) And IPM.MasterCompanyId=" + Session["varCompanyId"] + @"
                    Group By OM.OrderId,Item_Name,QualityName,DesignName,ColorName,ShadeColorName,ShapeName,OrderUnitID,SizeMtr,SizeFt,SizeInch,Od.flagsize,
                    JA.Item_Finished_Id,MS.DYEINGPROGRAMWITHEXPORTAREA,MS.DYINGPROGRAMWITHFULLAREA";
            }
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
            txttotalqty.Text = "";
            txttotalarea.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalqty.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                txttotalarea.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("Sum(area)", "")), 4).ToString();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
        }
        return ds;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varCompanyId"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varCompanyId"] + "_company.gif?" + DateTime.Now.ToString("dd-MMM-yyyy");
        }
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
                DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select OD.Item_Finished_Id,OD.Orderid,OD.Orderdetailid From OrderDetail OD,JobAssigns JA Where OD.OrderId=JA.OrderId And OD.Item_Finished_Id=JA.Item_Finished_Id  And (PreProdAssignedQty+INTERNALPRODASSIGNEDQTY)>0 And OD.Tag_Flag=1 And OD.OrderId in (" + chekboxlist.SelectedValue + ")");
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
            UtilityModule.MessageAlert(ex.Message, "Master/Carpet/ProcessProgram.aspx");
        }
        finally
        {
            con.Close();
        }
    }
    protected void ItemDetailInExcelReport(int PPID)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlCommand cmd = new SqlCommand("Pro_ProcessProgramItemDetailReportInExcel", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 300;

        cmd.Parameters.AddWithValue("@Companyid", ddcompany.SelectedValue);
        cmd.Parameters.AddWithValue("@PPID", PPID);
        cmd.Parameters.AddWithValue("@Processid", ddprocess.SelectedValue);

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
            if (!Directory.Exists(Server.MapPath("~/Tempexcel/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Tempexcel/"));
            }
            string Path = "";
            var xapp = new XLWorkbook();
            var sht = xapp.Worksheets.Add("sheet1");


            sht.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            sht.PageSetup.AdjustTo(95);
            sht.PageSetup.PaperSize = XLPaperSize.A4Paper;


            sht.PageSetup.Margins.Top = 1.21;
            sht.PageSetup.Margins.Left = 0.47;
            sht.PageSetup.Margins.Right = 0.36;
            sht.PageSetup.Margins.Bottom = 0.19;
            sht.PageSetup.Margins.Header = 1.20;
            sht.PageSetup.Margins.Footer = 0.3;
            sht.PageSetup.SetScaleHFWithDocument();

            sht.Column("A").Width = 10.22;
            sht.Column("B").Width = 6.56;
            sht.Column("C").Width = 9.11;
            sht.Column("D").Width = 6.67;
            sht.Column("E").Width = 10.22;
            sht.Column("F").Width = 8.11;
            sht.Column("G").Width = 8.56;
            sht.Column("H").Width = 5.89;
            sht.Column("I").Width = 5.89;
            sht.Column("J").Width = 5.89;
            sht.Column("K").Width = 5.89;
            sht.Column("L").Width = 5.89;
            sht.Column("M").Width = 5.89;
            sht.Column("N").Width = 5.89;
            sht.Column("O").Width = 5.89;
            sht.Column("P").Width = 11.78;
            sht.Column("Q").Width = 11.56;

            ////sht.ColumnWidth = 5.20;

            //sht.Row(1).Height = 29;
            //sht.Row(2).Height = 29;
            //sht.Row(3).Height = 29;
            //sht.Row(4).Height = 29;
            //sht.Row(5).Height = 29;
            //sht.Row(6).Height = 15;
            //sht.Row(7).Height = 33;
            //sht.Row(8).Height = 18;
            //sht.Row(9).Height = 18;
            //sht.Row(10).Height = 18;
            //sht.Row(11).Height = 18;
            //sht.Row(12).Height = 18;

            sht.Range("A1").Value = ds.Tables[0].Rows[0]["CompanyName"];
            sht.Range("A1:Q2").Style.Font.FontName = "Calibri";
            sht.Range("A1:Q2").Style.Font.Bold = true;
            sht.Range("A1:Q2").Style.Font.FontSize = 22;
            sht.Range("A1:Q2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A1:Q2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            sht.Range("A1:Q2").Merge();
            sht.Range("A1:Q2").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("A1:Q2").Style.Alignment.SetWrapText();

            sht.Range("A3").Value = ds.Tables[0].Rows[0]["OrderNo"];
            sht.Range("A3:C3").Style.Font.FontName = "Calibri";
            sht.Range("A3:C3").Style.Font.Bold = true;
            sht.Range("A3:C3").Style.Font.FontSize = 16;
            sht.Range("A3:C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A3:C3").Merge();
            sht.Range("A3:C3").Style.Fill.BackgroundColor = XLColor.LightGray;

            sht.Range("D3").Value = "";
            sht.Range("D3:Q3").Style.Font.FontName = "Calibri";
            sht.Range("D3:Q3").Style.Font.Bold = true;
            sht.Range("D3:Q3").Style.Font.FontSize = 16;
            sht.Range("D3:Q3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("D3:Q3").Merge();
            sht.Range("D3:Q3").Style.Fill.BackgroundColor = XLColor.LightGray;

            //*******Header
            sht.Range("A4").Value = "QUALITY";
            sht.Range("B4").Value = "COUNT";
            sht.Range("C4").Value = "UNDYED SHADE NO";
            sht.Range("D4").Value = "LOT NO";
            sht.Range("E4").Value = "DYED SHADENO/COLOR NO";
            sht.Range("F4").Value = "PROCESS";
            sht.Range("G4").Value = "WIGHT (K.G)";
            sht.Range("P4").Value = "ORDER DATE";
            sht.Range("Q4").Value = "DELIVERY DATE";

            sht.Row(4).Height = 53.3;

            sht.Range("A4:Q4").Style.Font.FontName = "Calibri";
            sht.Range("A4:Q4").Style.Font.FontSize = 11;
            sht.Range("A4:Q4").Style.Font.Bold = true;
            sht.Range("A4:Q4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("A4:Q4").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("A4:Q4").Style.Alignment.SetWrapText();


            int row = 5;
            int noofrows2 = 0;
            int i2 = 0;
            string OrderQuality = "";
            string OrderDesign = "";
            string OrderColor = "";

            DataTable DtDistinctOrderDesignColor = ds.Tables[0].DefaultView.ToTable(true, "OrderQuality", "OrderDesign", "OrderColor");
            noofrows2 = DtDistinctOrderDesignColor.Rows.Count;

            for (i2 = 0; i2 < noofrows2; i2++)
            {
                OrderQuality = DtDistinctOrderDesignColor.Rows[i2]["OrderQuality"].ToString();
                OrderDesign = DtDistinctOrderDesignColor.Rows[i2]["OrderDesign"].ToString();
                OrderColor = DtDistinctOrderDesignColor.Rows[i2]["OrderColor"].ToString();

                sht.Range("A" + row).SetValue("DESIGNNO:" + " " + DtDistinctOrderDesignColor.Rows[i2]["OrderDesign"] + " " + DtDistinctOrderDesignColor.Rows[i2]["OrderColor"]);
                sht.Range("A" + row + ":F" + row).Style.Font.Bold = true;
                sht.Range("A" + row + ":F" + row).Style.Font.FontName = "Calibri";
                sht.Range("A" + row + ":F" + row).Style.Font.FontSize = 11;
                sht.Range("A" + row + ":F" + row).Style.Font.FontColor = XLColor.White;
                sht.Range("A" + row + ":F" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //sht.Range("A" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                sht.Range("A" + row + ":F" + row).Style.Alignment.WrapText = true;
                sht.Range("A" + row + ":F" + row).Style.Fill.BackgroundColor = XLColor.Black;
                sht.Range("A" + row + ":F" + row).Merge();

                row = row + 1;

                string OItemName = "";
                string OQualityName = "";
                string IShadeColor = "";
                string OShadeColorName = "";
                decimal IssueQty = 0;

                DataRow[] foundRows;
                foundRows = ds.Tables[0].Select("OrderQuality='" + DtDistinctOrderDesignColor.Rows[i2]["OrderQuality"] + "' and OrderDesign='" + DtDistinctOrderDesignColor.Rows[i2]["OrderDesign"] + "' and OrderColor='" + DtDistinctOrderDesignColor.Rows[i2]["OrderColor"] + "'");

                if (foundRows.Length > 0)
                {
                    foreach (DataRow row3 in foundRows)
                    {
                        OItemName = row3["OItemName"].ToString();
                        OQualityName = row3["QualityName"].ToString();
                        IShadeColor = row3["IShadeColor"].ToString();
                        OShadeColorName = row3["OShadeColor"].ToString();
                        IssueQty = Convert.ToDecimal(row3["Qty"].ToString());

                        sht.Range("A" + row).SetValue(OItemName);
                        sht.Range("B" + row).SetValue(OQualityName);
                        sht.Range("C" + row).SetValue(IShadeColor);
                        sht.Range("E" + row).SetValue(OShadeColorName);
                        sht.Range("G" + row).SetValue(IssueQty);

                        //sht.Range("G" + row).SetValue(Convert.ToDecimal(ds.Tables[0].Compute("sum(Qty)", "OItemName='" + row3["OItemName"] + "' and QualityName='" + row3["QualityName"] + "' and IFinishedId='" + row3["IFinishedId"] + "' and FinishedId='" + row3["FinishedId"] + "' ")));


                        sht.Range("A" + row + ":G" + row).Style.Font.Bold = true;
                        sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Calibri";
                        sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 11;
                        sht.Range("A" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        sht.Range("A" + row + ":G" + row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        sht.Range("A" + row + ":G" + row).Style.Alignment.WrapText = true;

                        row = row + 1;

                        //break;
                    }
                }

            }

            row = row + 1;

            sht.Range("A" + row).Value = "";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":Q" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 20;
            sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":Q" + row).Style.Alignment.WrapText = true;

            row = row + 1;

            sht.Range("A" + row).Value = "";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":Q" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 20;
            sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":Q" + row).Style.Alignment.WrapText = true;

            row = row + 1;

            sht.Range("A" + row).Value = "गुच्छी निकालने के बाद ही लाट डाई करे";
            sht.Range("A" + row + ":G" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":G" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":G" + row).Style.Font.FontSize = 18;
            sht.Range("A" + row + ":G" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":G" + row).Style.Alignment.WrapText = true;
            sht.Range("A" + row + ":G" + row).Merge();

            sht.Range("H" + row).Value = "ओल्ड स्टाक को चेक कर ले उसके बाद मटेरियल इशू करे";
            sht.Range("H" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("H" + row + ":Q" + row).Style.Font.Bold = true;
            sht.Range("H" + row + ":Q" + row).Style.Font.FontSize = 18;
            sht.Range("H" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("H" + row + ":Q" + row).Style.Alignment.WrapText = true;
            sht.Range("H" + row + ":Q" + row).Merge();

            row = row + 1;

            sht.Range("A" + row).Value = "नोट %& कृपया जो डाई पैरामीटर दिया गया हैं उस पर खास ध्यान देकर टेस्टिंग करना है गलती नही होनी चाहिए  ";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":Q" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 18;
            sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":Q" + row).Style.Alignment.WrapText = true;
            sht.Range("A" + row + ":Q" + row).Merge();

            row = row + 1;

            sht.Range("A" + row).Value = " शेड का डाई करते समय कलर चेक करते रहे मैचिंग सही होनी चाहिए इसका खास ध्यान रहे  ";
            sht.Range("A" + row + ":Q" + row).Style.Font.FontName = "Arial Unicode MS";
            sht.Range("A" + row + ":Q" + row).Style.Font.Bold = true;
            sht.Range("A" + row + ":Q" + row).Style.Font.FontSize = 18;
            sht.Range("A" + row + ":Q" + row).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            sht.Range("A" + row + ":Q" + row).Style.Alignment.WrapText = true;
            sht.Range("A" + row + ":Q" + row).Merge();

            row = row + 1;


            sht.Range("P6").Value = "NAME OF THE PARAMETERS";
            sht.Range("P6").Style.Font.FontName = "Calibri";
            sht.Range("P6").Style.Font.Bold = true;
            sht.Range("P6").Style.Font.FontSize = 8;
            sht.Range("P6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P6").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P6").Style.Alignment.WrapText = true;

            sht.Range("Q6").Value = "SCOPE OF REQUIREMENT TEST";
            sht.Range("Q6").Style.Font.FontName = "Calibri";
            sht.Range("Q6").Style.Font.Bold = true;
            sht.Range("Q6").Style.Font.FontSize = 8;
            sht.Range("Q6").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q6").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q6").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q6").Style.Alignment.WrapText = true;

            sht.Range("P7").Value = "AZO DYES";
            sht.Range("P7").Style.Font.FontName = "Calibri";
            sht.Range("P7").Style.Font.Bold = true;
            sht.Range("P7").Style.Font.FontSize = 8;
            sht.Range("P7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P7").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P7").Style.Alignment.WrapText = true;

            sht.Range("Q7").Value = "AZO FREE";
            sht.Range("Q7").Style.Font.FontName = "Calibri";
            sht.Range("Q7").Style.Font.Bold = true;
            sht.Range("Q7").Style.Font.FontSize = 8;
            sht.Range("Q7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q7").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q7").Style.Alignment.WrapText = true;

            sht.Range("P8").Value = "FORMAL DEHYDE";
            sht.Range("P8").Style.Font.FontName = "Calibri";
            sht.Range("P8").Style.Font.Bold = true;
            sht.Range("P8").Style.Font.FontSize = 8;
            sht.Range("P8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P8").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P8").Style.Alignment.WrapText = true;

            sht.Range("Q8").Value = "75 mg/kg";
            sht.Range("Q8").Style.Font.FontName = "Calibri";
            sht.Range("Q8").Style.Font.Bold = true;
            sht.Range("Q8").Style.Font.FontSize = 8;
            sht.Range("Q8").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q8").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q8").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q8").Style.Alignment.WrapText = true;

            sht.Range("P9").Value = "PH VALUE";
            sht.Range("P9").Style.Font.FontName = "Calibri";
            sht.Range("P9").Style.Font.Bold = true;
            sht.Range("P9").Style.Font.FontSize = 8;
            sht.Range("P9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P9").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P9").Style.Alignment.WrapText = true;

            sht.Range("Q9").Value = "04-7.50";
            sht.Range("Q9").Style.Font.FontName = "Calibri";
            sht.Range("Q9").Style.Font.Bold = true;
            sht.Range("Q9").Style.Font.FontSize = 8;
            sht.Range("Q9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q9").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q9").Style.Alignment.WrapText = true;

            sht.Range("P10").Value = "COLOR FASTNESS TO WASHING";
            sht.Range("P10").Style.Font.FontName = "Calibri";
            sht.Range("P10").Style.Font.Bold = true;
            sht.Range("P10").Style.Font.FontSize = 8;
            sht.Range("P10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P10").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P10").Style.Alignment.WrapText = true;

            sht.Range("Q10").Value = "40 D";
            sht.Range("Q10").Style.Font.FontName = "Calibri";
            sht.Range("Q10").Style.Font.Bold = true;
            sht.Range("Q10").Style.Font.FontSize = 8;
            sht.Range("Q10").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q10").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q10").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q10").Style.Alignment.WrapText = true;

            sht.Range("P11").Value = "COLOR FASTNESS TO DRY ";
            sht.Range("P11").Style.Font.FontName = "Calibri";
            sht.Range("P11").Style.Font.Bold = true;
            sht.Range("P11").Style.Font.FontSize = 8;
            sht.Range("P11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P11").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P11").Style.Alignment.WrapText = true;

            sht.Range("Q11").Value = "4";
            sht.Range("Q11").Style.Font.FontName = "Calibri";
            sht.Range("Q11").Style.Font.Bold = true;
            sht.Range("Q11").Style.Font.FontSize = 8;
            sht.Range("Q11").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q11").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q11").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q11").Style.Alignment.WrapText = true;

            sht.Range("P12").Value = "COLOR FASTNESS TO WET";
            sht.Range("P12").Style.Font.FontName = "Calibri";
            sht.Range("P12").Style.Font.Bold = true;
            sht.Range("P12").Style.Font.FontSize = 8;
            sht.Range("P12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("P12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("P12").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("P12").Style.Alignment.WrapText = true;

            sht.Range("Q12").Value = "4";
            sht.Range("Q12").Style.Font.FontName = "Calibri";
            sht.Range("Q12").Style.Font.Bold = true;
            sht.Range("Q12").Style.Font.FontSize = 8;
            sht.Range("Q12").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sht.Range("Q12").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            sht.Range("Q12").Style.Fill.BackgroundColor = XLColor.LightGray;
            sht.Range("Q12").Style.Alignment.WrapText = true;



            ////*************
            //sht.Columns(1, 30).AdjustToContents();

            using (var a = sht.Range("A1" + ":Q" + (row - 1)))
            {
                a.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                a.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                a.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                a.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            }

            string Fileextension = "xlsx";
            string filename = UtilityModule.validateFilename("ProcessProgramItemDetailExcelReport_" + DateTime.Now.ToString("dd-MMM-yyyy") + "." + Fileextension);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "Fstatus", "alert('No Record Found!');", true);
        }
    }

    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        if (ddprocess.SelectedIndex > 0)
        {
            int PPID = 0;
            if (txtprocessprogram.Text != "")
            {
                if (Session["VarCompanyId"].ToString() == "30")
                {
                    PPID = Convert.ToInt32(Session["ViewState"].ToString());
                }
                else
                {
                    PPID = Convert.ToInt32(Session["ViewState"]);
                }
            }
            if (ChekEdit.Checked == true && ddprocessprogram.SelectedIndex > 0)
            {
                PPID = Convert.ToInt32(ddprocessprogram.SelectedValue);
            }
            if (Session["VarcompanyNo"].ToString() == "4")
            {
                Session["ReportPath"] = "Reports/RptDyeingProgram.rpt";
                Session["CommanFormula"] = "{VIEW_PP_CONSUMPTION.PPID}= " + PPID + " And {VIEW_PP_CONSUMPTION.ProcessId}= " + ddprocess.SelectedValue + "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
            else if (ChkForItemDetailInExcel.Checked == true)
            {
                ItemDetailInExcelReport(PPID);

                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@PPID", PPID);
                //param[1] = new SqlParameter("@Processid", ddprocess.SelectedValue);
                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Processprogramreport", param);
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                //}
                //else 
                //{ 
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Record Found!');", true); 
                //}
            }
            else if (variable.Carpetcompany == "1")
            {
                ////**************
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@PPID", PPID);
                //param[1] = new SqlParameter("@Processid", ddprocess.SelectedValue);
                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Processprogramreport", param);

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Pro_Processprogramreport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.AddWithValue("@PPID", PPID);
                cmd.Parameters.AddWithValue("@Processid", ddprocess.SelectedValue);

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                ad.Fill(ds);
                //*************
                con.Close();
                con.Dispose();


                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (variable.VarDyeingprogramwithExportArea == "1")
                    {
                        Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnewExportArea.rpt";
                    }
                    else if (variable.VarDyeingProgramReportWithOrderLagat == "1")
                    {
                        if (Session["VarcompanyNo"].ToString() == "30")
                        {
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnewWithOrderLagatsamara.rpt";
                        }
                        else if (Session["VarcompanyNo"].ToString() == "36")
                        {
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnewWithOrderLagatPrasad.rpt";
                        }
                        else
                        {
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnewWithOrderLagat.rpt";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["varCompanyId"]) == 44)
                        {
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramagni.rpt";
                        }
                        else if (Convert.ToInt32(Session["varCompanyId"]) == 45)
                        {
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnewMWS.rpt";
                        }
                        else 
                        { 
                            Session["rptFileName"] = "~\\Reports\\rptdyeingprogramnew.rpt"; 
                        }
                    }

                    //Session["rptFileName"] = Session["ReportPath"];
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\rptdyeingprogramnew.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Record Found!');", true); }

            }
            else
            {
                Session["ReportPath"] = "Reports/RptDyeingProgram1.rpt";
                Session["CommanFormula"] = "{VIEW_PP_CONSUMPTION.PPID}= " + PPID + " And {VIEW_PP_CONSUMPTION.ProcessId}= " + ddprocess.SelectedValue + "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
        }
    }
    protected void BtnLocalOcReport_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["varCompanyId"]) == 30)
        {
            Session["ReportPath"] = "Reports/LocalOC.rpt";
            Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + chekboxlist.SelectedValue + "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        }
        else
        {
            if (ChkCurrentConnsumption.Checked == true)
            {
                Session["ReportPath"] = "Reports/LocalOC.rpt";
                Session["CommanFormula"] = "{NewOrderReport.Orderid}=" + chekboxlist.SelectedValue + "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
            }
        }
    }
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
        ProcessSelectedIndexChanged();
    }
    private void ProcessSelectedIndexChanged()
    {
        editandwithoutedit();
        if (ddcustomer.Items.Count > 0)
        {
            ddcustomer.SelectedIndex = 0;
        }
        UtilityModule.ConditonalChkBoxListFill(ref ChkBoxListProcessEmployeName, @"Select EI.EmpId, EI.EmpName 
                From EmpInfo EI 
                join EmpProcess EP ON EI.EmpId = EP.EmpId 
                Where processId = " + ddprocess.SelectedValue + "  AND EI.MasterCompanyId = " + Session["varCompanyId"] + @" order by ei.empname");
    }
    protected void btndel_Click(object sender, EventArgs e)
    {
        lblerror.Text = "";
        if (Tdprocessprogram.Visible == true)
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

                param[0] = new SqlParameter("@PPid", ddprocessprogram.SelectedValue);
                param[1] = new SqlParameter("@Processid", ddprocess.SelectedValue);
                param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;
                param[3] = new SqlParameter("@userid", Session["varuserid"]);
                param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPROCESSPROGRAM", param);
                if (param[2].Value.ToString() != "")
                {
                    lblerror.Text = param[2].Value.ToString();
                    Tran.Commit();
                }
                else
                {
                    Tran.Commit();
                    lblerror.Text = "PPNo. Deleted Successfully.";
                    ddprocess_SelectedIndexChanged(sender, new EventArgs());
                }

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                Tran.Rollback();

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnPreviewOrderNotProcessProgram_Click(object sender, EventArgs e)
    {
        if (ddprocess.SelectedIndex > 0)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@CompanyID", ddcompany.SelectedValue);
            param[1] = new SqlParameter("@ProcessID", ddprocess.SelectedValue);
            param[2] = new SqlParameter("@CustomerID", ddcustomer.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_OrderDetailWithoutProcessProgram", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptOrderDetailWithoutProcessProgram.rpt";
                Session["dsFileName"] = "~\\ReportSchema\\RptOrderDetailWithoutProcessProgram.xsd";
                Session["GetDataset"] = ds;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('No Record Found!');", true);
            }
        }
    }
    protected void ForAllPPNo_CheckedChanged(object sender, EventArgs e)
    {
        if (ddcustomer.Items.Count > 0)
        {
            ddcustomer.SelectedIndex = 0;
            ddprocessprogram.Items.Clear();
        }
    }
    protected void TxtOrderNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtOrderNo.Text != "")
        {
            string str = @"Select OM.CompanyID, P.Process_ID, OM.CustomerID, P.PPID 
                From OrderMaster OM(Nolock) 
                JOIN ProcessProgram P(Nolock) ON P.Order_ID = OM.OrderId ";
            if (ddprocess.SelectedIndex > 0)
            {
                str = str + " And P.Process_ID = " + ddprocess.SelectedValue;
            }

            str = str + " Where OM.CompanyiD = " + ddcompany.SelectedValue + @" And 
                        (OM.CustomerOrderNo = '" + TxtOrderNo.Text + "' OR OM.LocalOrder='" + TxtOrderNo.Text + "')";
            if (ddcustomer.Items.Count > 0)
            {
                if (ddcustomer.SelectedIndex > 0)
                {
                    str = str + " And OM.CustomerID = " + ddcustomer.SelectedValue;
                }
            }

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddcustomer.SelectedIndex == 0)
                {
                    if (ddcustomer.Items.FindByValue(ds.Tables[0].Rows[0]["customerid"].ToString()) != null)
                    {
                        ddcustomer.SelectedValue = ds.Tables[0].Rows[0]["customerid"].ToString();
                        editandwithoutedit();
                    }
                }
                if (ddprocessprogram.Items.FindByValue(ds.Tables[0].Rows[0]["PPID"].ToString()) != null)
                {
                    ddprocessprogram.SelectedValue = ds.Tables[0].Rows[0]["PPID"].ToString();
                    ddprocessprogram_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altor", "alert('Invalid customer order No.!!!')", true);
            }
        }
    }
}