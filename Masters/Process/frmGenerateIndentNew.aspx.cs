using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Masters_Process_frmGenerateIndentNew : System.Web.UI.Page
{
    public static string Item_Finished_id, Flagsize, Unitid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            TxtReqDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            string str;
            str = @"select CI.CompanyId,CI.CompanyName from companyinfo CI  
                    inner join company_authentication CA on CI.companyid=CA.companyId and CA.userid=" + Session["varuserid"] + @" order by CompanyName 
                    select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER order by Process_Name 
                    select CalId,Caltype from Process_CalType";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");            
            if (Session["canedit"].ToString() == "1")
            {
                TDChkEdit.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "select EI.EmpId,EI.EmpName from Empinfo EI inner join EmpProcess EMP on EI.EmpId=EMP.EmpId and EMP.processid=" + DDProcessName.SelectedValue + " order by Ei.empname", true, "--Plz Select--");
        ViewState["Indentid"] = "0";
        //****************Order wise/PPno wise
        if ((UtilityModule.PPNoWise(DDProcessName.SelectedValue) == 1))
        {
            TDppno.Visible = true;
            TDCustCode.Visible = false;
            TDOrderNo.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDProcessProgramNo, @"select distinct PPID,PPID as PPID1 from Processprogram PP
                                                                        inner Join OrderMaster OM on PP.Order_ID=OM.OrderId and OM.Status=0
                                                                        order by PPID desc", true, "--Plz Select--");

        }
        else
        {
            TDCustCode.Visible = true;
            TDOrderNo.Visible = true;
            TDppno.Visible = false;
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "select customerid,CustomerCode+'/'+CompanyName from customerinfo order by CustomerName", true, "--Plz Select--");
        }
        //****************
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OM.orderid,OM.LocalOrder+' # '+OM.CustomerOrderNo as orderNo from ordermaster OM where OM.CustomerId=" + DDCustomerCode.SelectedValue + " and OM.status=0  order by orderNo", true, "--Plz Select--");
    }
    protected void FillBuyerDescription()
    {
        string str = "";
        DataSet ds;
        if (TDOrderNo.Visible == true)
        {
            str = @"select Vf.category_Name+' '+vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+ 
                    vf.shadecolorname+' '+vf.ShapeName+' '+(case When OD.flagsize=0 Then VF.sizeft when OD.flagsize=1 Then vf.SizeMtr When OD.flagsize=2 Then 
                    Vf.SizeInch Else Vf.SizeFt End)+' '+case When vf.SizeId>0 Then St.type Else '' End as ItemDescription,
                    OM.CustomerOrderNo,OD.orderdetailid,QtyRequired,ISNULL(V.OrderIndentQty,0) as OrderedQty
                    from ordermaster OM inner join OrderDetail Od on Om.OrderId=OD.OrderId
                    inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                    inner join sizetype St on Od.flagsize=St.Val
                    left join (select ProcessID,OrderDetailID,sum(OrderIndentQty)as OrderindentQty from V_getorderIndentQty group by Processid,OrderDetailID  ) V on od.orderdetailid=V.OrderDetailID and  v.Processid=" + DDProcessName.SelectedValue + " where OM.orderid=" + DDOrderNo.SelectedValue;
        }
        else if (TDppno.Visible == true)
        {
            str = @"select Vf.category_Name+' '+vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+ 
                  vf.shadecolorname+' '+vf.ShapeName+' '+(case When OD.flagsize=0 Then VF.sizeft when OD.flagsize=1 Then vf.SizeMtr When OD.flagsize=2 Then 
                  Vf.SizeInch Else Vf.SizeFt End)+' '+case When vf.SizeId>0 Then St.type Else '' End as ItemDescription,
                  OM.CustomerOrderNo,OD.orderdetailid,QtyRequired  from ordermaster OM inner join OrderDetail Od on Om.OrderId=OD.OrderId
                  inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=Vf.ITEM_FINISHED_ID
                  inner join sizetype St on Od.flagsize=St.Val
                  inner Join ProcessProgram PP on OM.OrderId=PP.Order_ID where PP.PPID=" + DDProcessProgramNo.SelectedValue;
        }
        if (str != "")
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DGBuyerDesc.DataSource = ds.Tables[0];
            DGBuyerDesc.DataBind();
        }

    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBuyerDescription();
    }
    protected void DDProcessProgramNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBuyerDescription();
    }
    protected void FillOuterDescription()
    {
        //        string str = string.Empty;
        //        DataSet ds;
        //        if (hnorderDetailid.Value == "")
        //        {
        //            return;
        //        }
        //        if (TDppno.Visible == true)
        //        {
        //            str = @"select Distinct cast(Vf.ITEM_FINISHED_ID as varchar)+'/'+cast(OCD.OSizeflag as varchar)+'/'+cast(OCD.ounitid as varchar) as OutId,VF.category_Name+' '+vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+
        //                    vf.ColorName+' '+vf.ShadeColorName+' '+vf.ShapeName+' '+vf.SizeFt As OutDescription from PP_Consumption PP inner join Processprogram PR on PP.PPid=PR.PPId 
        //                    inner join V_FinishedItemDetail vf on PP.finishedid=vf.ITEM_FINISHED_ID 
        //                    inner join ORDER_CONSUMPTION_DETAIL OCD on PR.Order_ID=OCD.ORDERID and PP.ORDERDETAILID=ocd.ORDERDETAILID and PP.FinishedId=OCD.OFINISHEDID
        //                    Where PP.OrderDetailId in(" + hnorderDetailid.Value + ") and PP.ppid=" + DDProcessProgramNo.SelectedValue + " and PR.Process_id=" + DDProcessName.SelectedValue;
        //        }
        //        else if (TDOrderNo.Visible == true)
        //        {
        //            str = @"select Distinct cast(vf.ITEM_FINISHED_ID as varchar)+'/'+cast(OCD.OSizeflag as varchar)+'/'+cast(OCD.ounitid as varchar) as OutId,vf.CATEGORY_NAME+' '+vf.ITEM_NAME+' '+vf.QualityName+
        //                    ' '+vf.designName+' '+vf.ColorName+' '+vf.ShadeColorName+' '+vf.ShapeName+' '+vf.sizeft as OutDescription 
        //                    from ORDER_CONSUMPTION_DETAIL OCD inner join V_FinishedItemDetail vf on OCD.Ofinishedid=vf.ITEM_FINISHED_ID  
        //                    Where orderdetailid in(" + hnorderDetailid.Value + ") and PROCESSID=" + DDProcessName.SelectedValue + "  order by OutDescription";

        //        }
        //        if (str != "")
        //        {
        //            UtilityModule.ConditionalComboFill(ref DDoutdescription, str, true, "--Plz Select--");

        //        }

    }
    protected void btngetoutdesc_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Processid", typeof(int));
        dtrecords.Columns.Add("PPNoWise", typeof(int));
        dtrecords.Columns.Add("orderdetailid", typeof(int));
        dtrecords.Columns.Add("orderindentQty", typeof(int));
        dtrecords.Columns.Add("PPno", typeof(int));
        //********************
        for (int i = 0; i < DGBuyerDesc.Rows.Count; i++)
        {
            CheckBox chkitem = ((CheckBox)DGBuyerDesc.Rows[i].FindControl("Chkbox"));
            if (chkitem.Checked == true)
            {
                Label lblorderdetailid = ((Label)DGBuyerDesc.Rows[i].FindControl("lblorderdetailid"));
                TextBox txtindentQty = ((TextBox)DGBuyerDesc.Rows[i].FindControl("txtindentQty"));
                DataRow dr = dtrecords.NewRow();
                dr["Processid"] = DDProcessName.SelectedValue;
                dr["PPnowise"] = (TDppno.Visible == true ? 0 : 1);
                dr["Orderdetailid"] = lblorderdetailid.Text;
                dr["OrderindentQty"] = txtindentQty.Text == "" ? "0" : txtindentQty.Text;
                dr["PPno"] = (TDppno.Visible == true ? Convert.ToInt32(DDProcessProgramNo.SelectedValue) : 0);
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TT_GetindentQty", dtrecords);
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentQty", param);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No record select to save.');", true);
        }
        DGoutdescription.DataSource = ds.Tables[0];
        DGoutdescription.DataBind();
        //string orderdetailid = "";
        //for (int i = 0; i < DGBuyerDesc.Rows.Count; i++)
        //{
        //    CheckBox chkitem = ((CheckBox)DGBuyerDesc.Rows[i].FindControl("Chkbox"));
        //    Label lblorderdetailid = ((Label)DGBuyerDesc.Rows[i].FindControl("lblorderdetailid"));
        //    if (chkitem.Checked == true)
        //    {
        //        orderdetailid = orderdetailid + "," + lblorderdetailid.Text;
        //    }
        //}
        //hnorderDetailid.Value = orderdetailid.TrimStart(',');
        //FillOuterDescription();

    }
    protected void DDoutdescription_SelectedIndexChanged(object sender, EventArgs e)
    {

        //string[] Id = DDoutdescription.SelectedValue.Split('/');
        //Item_Finished_id = Id[0];
        //Flagsize = Id[1];
        //Unitid = Id[2];
        //hnofinishedid.Value = "a1=" + DDCompanyName.SelectedValue + "&a2=" + DDPartyName.SelectedValue + "&a3=" + Item_Finished_id + "&a4=" + DDcaltype.SelectedValue;

        ////****************
        //UtilityModule.ConditionalComboFill(ref ddUnit, "select unitid,unitname from Unit Where unitid=" + Unitid + "", false, "");
        //********FillQty

        //*********************
        //SqlParameter[] param = new SqlParameter[8];
        //param[0] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
        //param[1] = new SqlParameter("@PPnowise", (TDppno.Visible == true ? 0 : 1));
        //param[2] = new SqlParameter("@orderdetailid", hnorderDetailid.Value);
        //param[3] = new SqlParameter("@PPNo", (TDppno.Visible == true ? Convert.ToInt32(DDProcessProgramNo.SelectedValue) : 0));
        //param[4] = new SqlParameter("@Ofinishedid", Item_Finished_id);
        //param[5] = new SqlParameter("@TotalQty", SqlDbType.Float);
        //param[5].Direction = ParameterDirection.Output;
        //param[6] = new SqlParameter("@PreQty", SqlDbType.Float);
        //param[6].Direction = ParameterDirection.Output;
        //param[7] = new SqlParameter("@LossPercentage", SqlDbType.Float);
        //param[7].Direction = ParameterDirection.Output;
        ////**
        //SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIndentQty", param);

        //txtTotalQty.Text = param[5].Value.ToString();
        //TxtPreQty.Text = param[6].Value.ToString();
        //TxtLoss.Text = param[7].Value.ToString();
        //
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //****************sql Table Type 
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("PPNo", typeof(int));
        dtrecords.Columns.Add("oFinishedid", typeof(int));
        dtrecords.Columns.Add("Qty", typeof(float));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("Unitid", typeof(int));
        dtrecords.Columns.Add("LossPercentage", typeof(float));
        dtrecords.Columns.Add("Remark", typeof(string));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("orderid", typeof(int));
        dtrecords.Columns.Add("orderdetailid", typeof(int));
        dtrecords.Columns.Add("orderqty", typeof(int));
        for (int i = 0; i < DGoutdescription.Rows.Count; i++)
        {
            CheckBox chkoutItem = ((CheckBox)DGoutdescription.Rows[i].FindControl("Chkboxitem"));
            TextBox txtqty = ((TextBox)DGoutdescription.Rows[i].FindControl("txtoutQty"));
            if ((chkoutItem.Checked == true) && (Convert.ToDouble(txtqty.Text == "" ? "0" : txtqty.Text) > 0))
            {
                DataRow dr = dtrecords.NewRow();
                //***********
                Label lblppno = ((Label)DGoutdescription.Rows[i].FindControl("lblppno"));
                Label lblofinishedid = ((Label)DGoutdescription.Rows[i].FindControl("lblofinishedid"));
                TextBox txtrate = ((TextBox)DGoutdescription.Rows[i].FindControl("txtRate"));
                TextBox txtlossperc = ((TextBox)DGoutdescription.Rows[i].FindControl("txtlossperc"));
                TextBox txtitemremark = ((TextBox)DGoutdescription.Rows[i].FindControl("txtitemremark"));
                Label lblunitid = ((Label)DGoutdescription.Rows[i].FindControl("lblunitid"));
                Label lblflagsize = ((Label)DGoutdescription.Rows[i].FindControl("lblflagsize"));
                Label lblorderid = ((Label)DGoutdescription.Rows[i].FindControl("lblorderid"));
                Label lblorderdetailid = ((Label)DGoutdescription.Rows[i].FindControl("lblorderdetailid"));
                Label lblorderqty = ((Label)DGoutdescription.Rows[i].FindControl("lblorderqty"));
                //**************
                dr["PPno"] = lblppno.Text;
                dr["ofinishedid"] = lblofinishedid.Text;
                dr["Qty"] = txtqty.Text;
                dr["rate"] = txtrate.Text == "" ? "0" : txtrate.Text;
                dr["unitid"] = lblunitid.Text;
                dr["losspercentage"] = txtlossperc.Text == "" ? "0" : txtlossperc.Text;
                dr["remark"] = txtitemremark.Text;
                dr["flagsize"] = lblflagsize.Text;
                dr["orderid"] = lblorderid.Text;
                dr["orderdetailid"] = lblorderdetailid.Text;
                dr["orderqty"] = lblorderqty.Text;

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
                param[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[7] = new SqlParameter("@userid", Session["varuserid"]);
                param[8] = new SqlParameter("@ReqDate", TxtReqDate.Text);
                param[9] = new SqlParameter("@orderwiseflag", (TDppno.Visible == true ? 0 : 1));
                param[10] = new SqlParameter("@Gremark", txtremarks.Text);
                param[11] = new SqlParameter("@Indentdetailid", 0);            
                param[12] = new SqlParameter("@Msgflag", SqlDbType.VarChar, 100);
                param[12].Direction = ParameterDirection.Output;
                param[13] = new SqlParameter("@dtrecords", dtrecords);              
                //**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveIndentNew", param);
                Tran.Commit();
                ViewState["Indentid"] = param[0].Value.ToString();
                TxtIndentNo.Text = param[5].Value.ToString();
                lblmsg.Text = param[12].Value.ToString();
                Clearcontrols();
                Fillgrid();
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast on check box to save data.');", true);
        }
    }
    protected void Clearcontrols()
    {
        DDOrderNo.SelectedIndex = -1;
        DGBuyerDesc.DataSource = null;
        DGBuyerDesc.DataBind();
        DGoutdescription.DataSource = null;
        DGoutdescription.DataBind();
    }
    protected void Fillgrid()
    {
        string str;
        DataSet ds;
        str = @"select IM.indentNo,IM.Indentid,ID.indentdetailid,vf.CATEGORY_NAME+' '+vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName
                +' '+vf.ShadeColorName+' '+vf.ShapeName+' '+case When ID.flagsize=0 Then Vf.SizeFt When ID.flagsize=1 Then vf.SizeMtr When Id.flagsize=2 Then Vf.SizeInch Else
                vf.SizeFt End+' '+case When vf.SizeId>0 then ST.type Else '' End As OutDescription,
                ID.Quantity,ID.Rate,OM.CustomerOrderNo,
                vf1.CATEGORY_NAME+' '+vf1.ITEM_NAME+' '+vf1.QualityName+' '+vf1.designName+' '+vf1.ColorName
                +' '+vf1.ShadeColorName+' '+vf1.ShapeName+' '+case When od.flagsize=0 Then vf1.SizeFt When od.flagsize=1 Then vf1.SizeMtr When od.flagsize=2 Then vf1.SizeInch Else
                vf1.SizeFt End+' '+case When vf1.SizeId>0 then Sz1.type Else '' End As OrderDescription,IndentQty,Orderqty
                from Indentmaster IM inner join IndentDetail ID on IM.IndentID=ID.IndentId
                inner join V_FinishedItemDetail vf on ID.OFinishedId=vf.ITEM_FINISHED_ID
                inner join SizeType ST on ID.flagsize=ST.Val
                left join OrderMaster OM on ID.ORDERID=OM.OrderId
                inner join OrderDetail OD on ID.OrderDetailID=OD.OrderDetailId
                inner join V_FinishedItemDetail vf1 on OD.Item_Finished_Id=Vf1.ITEM_FINISHED_ID
                inner join SizeType Sz1 on OD.flagsize=Sz1.Val Where IM.Indentid=" + ViewState["Indentid"];
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGIndentDetail.DataSource = ds.Tables[0];
        DGIndentDetail.DataBind();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        //string str = @"select  * from [V_IndentDetailReport] Where Indentid=" + ViewState["Indentid"];
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        SqlParameter [] param=new SqlParameter[1];
        param[0] = new SqlParameter("@indentid", ViewState["Indentid"]);        
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "pro_IndentdetailReport", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptgenerateindentnew.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptgenerateindentnew.xsd";
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
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDIndentNo.Visible = true;
            DDIndentNO.Items.Clear();
        }
        else
        {
            TDIndentNo.Visible = false;
            DDIndentNO.Items.Clear();
        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Indentid"] = "0";
        string str = "select Indentid,IndentNo from Indentmaster where CompanyId=" + DDCompanyName.SelectedValue + " and PartyId=" + DDPartyName.SelectedValue + " and ProcessID=" + DDProcessName.SelectedValue + " order by IndentID desc";
        UtilityModule.ConditionalComboFill(ref DDIndentNO, str, true, "--Plz Select--");

    }
    protected void DDIndentNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Indentid,IndentNo,Replace(convert(nvarchar(11),Date,106),' ','-') as IndentDate,
                     Replace(convert(nvarchar(11),ReqDate,106),' ','-') as ReqDate,Gremark from Indentmaster where Indentid=" + DDIndentNO.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["Indentid"] = ds.Tables[0].Rows[0]["Indentid"].ToString();
            TxtIndentNo.Text = ds.Tables[0].Rows[0]["IndentNo"].ToString();
            TxtDate.Text = ds.Tables[0].Rows[0]["IndentDate"].ToString();
            TxtReqDate.Text = ds.Tables[0].Rows[0]["ReqDate"].ToString();
            txtremarks.Text = ds.Tables[0].Rows[0]["Gremark"].ToString();
            Fillgrid();
        }
        else
        {
            ViewState["Indentid"] = "0";
            TxtIndentNo.Text = "";
            TxtDate.Text = "";
            TxtReqDate.Text = "";
            txtremarks.Text = "";
        }
    }
    protected void DGIndentDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblindentdetailid = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblindentdetailid"));
            Label lblindentid = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblindentid"));
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@IndentDetailid", lblindentdetailid.Text);
            param[1] = new SqlParameter("@Indentid", lblindentid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //Execute Proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DelIndentDetail", param);
            //
            lblmsg.Text = param[2].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void DGIndentDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGIndentDetail.EditIndex = e.NewEditIndex;
        Fillgrid();
    }
    protected void DGIndentDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGIndentDetail.EditIndex = -1;
        Fillgrid();
    }
    protected void DGIndentDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblindentdetailid = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblindentdetailid"));
            Label lblindentid = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblindentid"));
            Label lblHQty = ((Label)DGIndentDetail.Rows[e.RowIndex].FindControl("lblHQty"));
            TextBox Qty = ((TextBox)DGIndentDetail.Rows[e.RowIndex].FindControl("Qty"));
            TextBox txtindentqty = ((TextBox)DGIndentDetail.Rows[e.RowIndex].FindControl("txtindentqty"));
            TextBox txtorderqty = ((TextBox)DGIndentDetail.Rows[e.RowIndex].FindControl("txtorderqty"));
            TextBox Rate = ((TextBox)DGIndentDetail.Rows[e.RowIndex].FindControl("Rate"));
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@IndentDetailid", lblindentdetailid.Text);
            param[1] = new SqlParameter("@Indentid", lblindentid.Text);
            param[2] = new SqlParameter("@HQty", lblHQty.Text);
            param[3] = new SqlParameter("@Qty", Qty.Text == "" ? "0" : Qty.Text);
            param[4] = new SqlParameter("@rate", Rate.Text == "" ? "0" : Rate.Text);
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@IndentQty",txtindentqty.Text);
            param[8] = new SqlParameter("@Orderqty", txtorderqty.Text);
            //Execute Proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateIndentNew", param);
            //
            lblmsg.Text = param[6].Value.ToString();
            Tran.Commit();
            DGIndentDetail.EditIndex = -1;
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
    }
   
}