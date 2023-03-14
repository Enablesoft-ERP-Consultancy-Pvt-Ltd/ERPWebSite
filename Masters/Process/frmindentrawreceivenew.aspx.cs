using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Process_frmindentrawreceivenew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By Companyname
                          select DISTINCT PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varCompanyId"] + @" order by PROCESS_NAME_ID
                          select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME
                          select customerid,CustomerCode+'/'+CompanyName from customerinfo order by CustomerName";            
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "");
            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 3, true, "--Plz Select--");
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //Chk edit visiblity
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            //
        }
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        ViewState["prmid"] = "0";
        DGRecDetail.DataSource = null;
        DGRecDetail.DataBind();
        DGSavedetail.DataSource = null;
        DGSavedetail.DataBind();

        if (chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"select Distinct PRM.PRMid,PRM.ChallanNo+'/'+replace(convert(nvarchar(11),Date,106),' ','-') as ChallanNo from PP_ProcessRecMaster  PRM inner join PP_ProcessRecTran PRT 
                                           on PRM.PRMid=PRT.PRMid Where PRM.CompanyId=" + ddCompName.SelectedValue + " and PRM.ProcessId=" + ddProcessName.SelectedValue + " and PRM.Empid=" + ddempname.SelectedValue + " order by PRM.Prmid desc", true, "--Plz Select--");

            //            str = @"select IndentID,IndentNo from Indentmaster where PartyId= " + ddempname.SelectedValue + @" and 
            //                    ProcessID=" + ddProcessName.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + " order by indentid desc";
        }
        else
        {
            str = @"select IndentID,IndentNo from Indentmaster where PartyId= " + ddempname.SelectedValue + @" and 
                    ProcessID=" + ddProcessName.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + " and status='Pending' order by indentid desc";
            UtilityModule.ConditionalComboFill(ref ddindentno, str, true, "--Plz Select--");
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        DGRecDetail.DataSource = null;
        DGRecDetail.DataBind();
        DGSavedetail.DataSource = null;
        DGSavedetail.DataBind();
        string str = @"select Distinct EI.EmpId,EI.EmpName from IndentMaster  IM 
                       inner Join EmpInfo EI on IM.Partyid=EI.Empid where IM.ProcessID=" + ddProcessName.SelectedValue + " and IM.companyId=" + ddCompName.SelectedValue + " order by EmpName";
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Plz Select--");
    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {        
        if (chkedit.Checked == true)
        {
//            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"select Distinct PRM.PRMid,PRM.ChallanNo+'/'+replace(convert(nvarchar(11),Date,106),' ','-') as ChallanNo from PP_ProcessRecMaster  PRM inner join PP_ProcessRecTran PRT 
//                                           on PRM.PRMid=PRT.PRMid Where PRT.IndentId=" + ddindentno.SelectedValue + "", true, "--Plz Select--");
            FillGridsave();
        }
        else
        {            
            Fillgrid();
        }
    }
    protected void Fillgrid()
    {
        string str = @"select OM.CustomerOrderNo,Vf.category_Name+' '+vf.item_name+' '+vf.QualityName+' '+vf.designName+' '+
                        vf.ColorName+' '+Vf.ShadeColorName+' '+vf.Shapename+
                        case When OD.flagsize=0 Then Vf.SizeFt When OD.flagsize=1 Then Vf.SizeMtr 
                        When OD.flagsize=2 Then Vf.SizeInch Else Vf.SizeFt End+' '+case when vf.sizeid>0 
                        then Sz.Type else '' End as orderdescription,
                        OM.CustomerOrderNo,vf1.category_Name+' '+vf1.item_name+' '+vf1.QualityName+' '+vf1.designName+' '+
                        vf1.ColorName+' '+vf1.ShadeColorName+' '+vf1.Shapename+
                        case When ID.flagsize=0 Then vf1.SizeFt When ID.flagsize=1 Then vf1.SizeMtr 
                        When ID.flagsize=2 Then vf1.SizeInch Else vf1.SizeFt End+' '+case when vf1.sizeid>0 
                        then Sz1.Type else '' End as ItemDescription,U.UnitName,ID.Quantity as IndentQty,isnull(VRD.Recqty,0) as PreRec
                        ,ID.ofinishedid,ID.orderdetailid,ID.indentdetailid,ID.LossPercent,ID.Rate as UnitRate,ID.unitid,ID.flagsize,Im.indentid,Im.IndentNo,ID.Lotno
                        from IndentMaster Im 
                        inner Join IndentDetail ID on IM.IndentID=ID.IndentId
                        inner join orderdetail od on ID.orderdetailid=OD.OrderDetailId
                        inner join OrderMaster om on oD.OrderId=om.OrderId
                        inner join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                        inner join SizeType SZ on OD.flagsize=SZ.Val
                        inner join V_FinishedItemDetail vf1 on ID.ofinishedid=vf1.ITEM_FINISHED_ID
                        inner join SizeType Sz1 on ID.flagsize=Sz1.val
                        inner join unit U on ID.UNITID=U.UnitId
                        left  Join V_IndentRecDetailNew VRD on ID.indentdetailid=VRD.Indentdetailid and ID.orderdetailid=VRD.orderdetailid 
                        and ID.OFinishedid=VRD.Finishedid Where IM.companyid=" + ddCompName.SelectedValue + " and IM.processid=" + ddProcessName.SelectedValue + " and IM.partyid=" + ddempname.SelectedValue;
        if (ddindentno.SelectedIndex > 0)
        {
            str = str + " and Im.Indentid=" + ddindentno.SelectedValue;

        }
        if (ddCatagory.SelectedIndex > 0)
        {
            str = str + " and vf.CATEGORY_ID=" + ddCatagory.SelectedValue;
        }
        if (dditemname.SelectedIndex > 0)
        {
            str = str + " and vf.Item_Id=" + dditemname.SelectedValue;
        }
        if (dquality.SelectedIndex > 0)
        {
            str = str + " and vf.qualityid=" + dquality.SelectedValue;
        }
        if (dddesign.SelectedIndex > 0)
        {
            str = str + " and vf.designid=" + dddesign.SelectedValue;
        }
        if (ddcolor.SelectedIndex > 0)
        {
            str = str + " and vf.Colorid=" + ddcolor.SelectedValue;
        }
        if (ddshape.SelectedIndex > 0)
        {
            str = str + " and vf.shapeid=" + ddshape.SelectedValue;
        }
        if (ddsize.SelectedIndex > 0)
        {
            str = str + " and vf.Sizeid=" + ddsize.SelectedValue;
        }
        if (DDCustomerCode.SelectedIndex>0)
        {
            str = str + " and OM.customerid=" +DDCustomerCode.SelectedValue;
        }
        if (DDOrderNo.SelectedIndex>0)
        {
            str = str + " and ID.orderid=" + DDOrderNo.SelectedValue;
        }
        str = str + " order by IM.Indentid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGRecDetail.DataSource = ds.Tables[0];
        DGRecDetail.DataBind();
    }
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DDgodown = ((DropDownList)e.Row.FindControl("DDgodown"));
            string str = "select godownid,GodownName from godownmaster order by GodownName";
            UtilityModule.ConditionalComboFill(ref DDgodown, str, true, "--Plz Select--");
            if (DDgodown.Items.Count>0)
            {
                DDgodown.SelectedIndex = 1;
            }
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //************Data table for Sql Table Types
            DataTable dtrecords = new DataTable();
            dtrecords.Columns.Add("Finishedid", typeof(int));
            dtrecords.Columns.Add("RecQty", typeof(float));
            dtrecords.Columns.Add("godownid", typeof(int));
            dtrecords.Columns.Add("Unitid", typeof(int));
            dtrecords.Columns.Add("LotNo", typeof(string));
            dtrecords.Columns.Add("LossQty", typeof(float));
            dtrecords.Columns.Add("RecIssItemflag", typeof(int));
            dtrecords.Columns.Add("Remark", typeof(string));
            dtrecords.Columns.Add("flagsize", typeof(int));
            dtrecords.Columns.Add("Rate", typeof(float));
            dtrecords.Columns.Add("orderdetailid", typeof(int));
            dtrecords.Columns.Add("Indentdetailid", typeof(int));
            //***************
            for (int i = 0; i < DGRecDetail.Rows.Count; i++)
            {
                CheckBox chkitem = ((CheckBox)DGRecDetail.Rows[i].FindControl("chkitem"));
                if (chkitem.Checked == true)
                {
                    TextBox txtrecqty = ((TextBox)DGRecDetail.Rows[i].FindControl("txtrecqty"));
                    DropDownList DDgodown = ((DropDownList)DGRecDetail.Rows[i].FindControl("DDgodown"));
                    if ((txtrecqty.Text != "") && Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0)
                    {
                        if ((DDgodown.SelectedIndex == -1 || DDgodown.SelectedIndex == 0))
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('Please select godown and Lotno....');", true);
                            Tran.Commit();
                            return;
                        }
                        DataRow dr = dtrecords.NewRow();
                        int finishedid = Convert.ToInt32(((Label)DGRecDetail.Rows[i].FindControl("lblofinishedid")).Text);
                        int unitid = Convert.ToInt32(((Label)DGRecDetail.Rows[i].FindControl("lblunitid")).Text);
                        TextBox txtlotno = ((TextBox)DGRecDetail.Rows[i].FindControl("txtlotno"));
                        TextBox txtremark = ((TextBox)DGRecDetail.Rows[i].FindControl("txtremark"));
                        int flagsize = Convert.ToInt32(((Label)DGRecDetail.Rows[i].FindControl("lblsizeflag")).Text);
                        TextBox txtlossQty = ((TextBox)DGRecDetail.Rows[i].FindControl("txtlossQty"));
                        TextBox txtrate = ((TextBox)DGRecDetail.Rows[i].FindControl("txtrate"));
                        int orderdetailid = Convert.ToInt32(((Label)DGRecDetail.Rows[i].FindControl("lblorderdetailid")).Text);
                        int indentdetailid = Convert.ToInt32(((Label)DGRecDetail.Rows[i].FindControl("lblIndentDetailid")).Text);
                        //******assign Data in Datarow
                        dr["Finishedid"] = finishedid;
                        dr["RecQty"] = txtrecqty.Text;
                        dr["godownid"] = DDgodown.SelectedValue;
                        dr["Unitid"] = unitid;
                        dr["LotNo"] = txtlotno.Text == "" ? "Without Lot No" : txtlotno.Text;
                        dr["LossQty"] = txtlossQty.Text == "" ? "0" : txtlossQty.Text;
                        dr["RecIssItemflag"] = 0;
                        dr["Remark"] = txtremark.Text;
                        dr["flagsize"] = flagsize;
                        dr["Rate"] = txtrate.Text == "" ? "0" : txtrate.Text;
                        dr["orderdetailid"] = orderdetailid;
                        dr["Indentdetailid"] = indentdetailid;
                        //
                        dtrecords.Rows.Add(dr);

                    }
                }
            }
            //********************
            if (dtrecords.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@prmId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = ViewState["prmid"];
                param[1] = new SqlParameter("@companyId", ddCompName.SelectedValue);
                param[2] = new SqlParameter("@Empid", ddempname.SelectedValue);
                param[3] = new SqlParameter("@processid", ddProcessName.SelectedValue);
                param[4] = new SqlParameter("@RecDate", txtrecdate.Text);
                param[5] = new SqlParameter("@GateinNo", SqlDbType.VarChar, 50);
                param[5].Direction = ParameterDirection.InputOutput;
                param[5].Value = txtgateinno.Text;
                param[6] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                param[6].Direction = ParameterDirection.InputOutput;
                param[6].Value = txtchalanno.Text;
                param[7] = new SqlParameter("@masterremark", txtmsterremark.Text);
                param[8] = new SqlParameter("@varuserid", Session["varuserid"]);
                param[9] = new SqlParameter("@varcompanyid", Session["varcompanyId"]);
                param[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[10].Direction = ParameterDirection.Output;
                param[11] = new SqlParameter("@dtrecord", dtrecords);
                param[12] = new SqlParameter("@prtid", 0);
                param[13] = new SqlParameter("@indentid", 0); //Internally getting value
                //**********execute Proc
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PP_PRM_RECEIVE_NEW", param);
                lblmsg.Text = param[10].Value.ToString();
                ViewState["prmid"] = param[0].Value.ToString();
                ViewState["reportprmid"] = ViewState["prmid"];
                txtchalanno.Text = param[6].Value.ToString();
                txtgateinno.Text = param[5].Value.ToString();
                Tran.Commit();
                RefreshControls();
                FillGridsave();
                //*****************
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt1", "alert('No record select for save.');", true);
                Tran.Commit();
            }
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void RefreshControls()
    {
        DGRecDetail.DataSource = null;
        DGRecDetail.DataBind();
       
    }
    protected void txtrecqty_TextChanged(object sender, EventArgs e)
    {
        TextBox txtrecqty = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtrecqty.Parent.Parent;
        int i = row.RowIndex;
        Label lbllossperc = ((Label)DGRecDetail.Rows[i].FindControl("lbllossperc"));
        TextBox txtlossQty = ((TextBox)DGRecDetail.Rows[i].FindControl("txtlossQty"));
        Label lblpendingqty = ((Label)DGRecDetail.Rows[i].FindControl("lblpendingqty"));
        Double lossperc = Convert.ToDouble((lbllossperc.Text == "" ? "0" : lbllossperc.Text));
        Double lossqty = 0, pendingqty = 0;
        pendingqty = Convert.ToDouble(lblpendingqty.Text == "" ? "0" : lblpendingqty.Text);
        //*****Check PendingQty
        if (Convert.ToDouble(txtrecqty.Text) > pendingqty)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Rec Qty can not be greater Than Pending Qty.');", true);
            txtrecqty.Text = "0";
        }
        //*****
        if (lossperc > 0)
        {
            lossqty = Math.Round(Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) * lossperc / 100, 3);
        }
        txtlossQty.Text = lossqty.ToString();
    }
    protected void FillGridsave()
    {
        string str = @"select Vf.category_Name+' '+vf.item_name+' '+vf.QualityName+' '+vf.designName+' '+
                    vf.ColorName+' '+Vf.ShadeColorName+' '+vf.Shapename+
                    case When PRT.flagsize=0 Then Vf.SizeFt When PRT.flagsize=1 Then Vf.SizeMtr 
                    When PRT.flagsize=2 Then Vf.SizeInch Else Vf.SizeFt End+' '+case when vf.sizeid>0 then Sz.Type else '' End as ItemDescription,
                    OM.customerorderNo,Vf1.category_Name+' '+vf1.item_name+' '+vf1.QualityName+' '+vf1.designName+' '+
                    vf1.ColorName+' '+Vf1.ShadeColorName+' '+vf1.Shapename+
                    case When OD.flagsize=0 Then Vf1.SizeFt When OD.flagsize=1 Then Vf1.SizeMtr 
                    When OD.flagsize=2 Then Vf1.SizeInch Else Vf1.SizeFt End+' '+case when vf1.sizeid>0 
                    then Sz1.Type else '' End as Orderdescription,PRT.RecQuantity,PRT.LossQty,PRT.Remark,U.UnitName,PRt.PRTid,IM.IndentNo
                    from PP_ProcessRecMaster PRM inner Join PP_ProcessRecTran PRT on  PRM.PrmId=PRT.prmid
                    inner join Indentmaster Im on PRT.indentid=IM.indentid
                    inner join V_FinishedItemDetail vf on PRT.finishedid=vf.item_finished_id
                    inner join sizetype sz on PRT.flagsize=sz.Val
                    inner join unit u on PRT.unitid=U.UnitId
                    Left join OrderDetail OD on PRT.Orderdetailid=OD.OrderDetailId
                    left join OrderMaster OM on OD.OrderId=OM.OrderId
                    left join V_FinishedItemDetail vf1 on OD.Item_Finished_Id=vf1.item_finished_id
                    left join sizetype Sz1 on OD.flagsize=sz1.val where PRM.Prmid=" + ViewState["reportprmid"];
        if (ddindentno.SelectedIndex>0)
        {
            str = str + " and IM.indentid=" + ddindentno.SelectedValue;
        }
        str = str + " order by Im.indentid";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGSavedetail.DataSource = ds.Tables[0];
        DGSavedetail.DataBind();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        TDChallanNo.Visible = false;
        DGRecDetail.DataSource = null;
        DGRecDetail.DataBind();
        ViewState["prmid"] = "0";
        ddindentno.Items.Clear();
        DDChallanNo.Items.Clear();
        if (chkedit.Checked == true)
        {
            TDChallanNo.Visible = true;
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select REPLACE(CONVERT(nvarchar(11),date,106),' ','-') as Recdate,ChallanNo,Gateinno,RRRemark,PRMid from PP_ProcessRecmaster Where Prmid=" + DDChallanNo.SelectedValue;
        str = str + @" select Distinct IM.IndentID,IM.IndentNo From PP_ProcessRecMaster PRM inner join PP_ProcessRecTran PRT on PRM.PRMid=PRT.PRMid
                   inner join IndentMaster IM on PRT.IndentId=IM.IndentID and PRM.PRMid=" + DDChallanNo.SelectedValue + " order by IM.Indentid";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtrecdate.Text = ds.Tables[0].Rows[0]["RecDate"].ToString();
            txtchalanno.Text = ds.Tables[0].Rows[0]["challanno"].ToString();
            txtgateinno.Text = ds.Tables[0].Rows[0]["gateinno"].ToString();
            txtmsterremark.Text = ds.Tables[0].Rows[0]["RRRemark"].ToString();
            ViewState["prmid"] = ds.Tables[0].Rows[0]["prmid"].ToString();
            ViewState["reportprmid"] = ds.Tables[0].Rows[0]["prmid"].ToString();
        }
        else
        {
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtchalanno.Text = "";
            txtgateinno.Text = "";
            txtmsterremark.Text = "";
            ViewState["prmid"] = "0";
            ViewState["reportprmid"] = "0";
        }
        UtilityModule.ConditionalComboFillWithDS(ref ddindentno, ds, 1, true, "--Plz Select--");
        FillGridsave();
    }
    protected void DGSavedetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGSavedetail.EditIndex = e.NewEditIndex;
        FillGridsave();
    }
    protected void DGSavedetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGSavedetail.EditIndex = -1;
        FillGridsave();
    }
    protected void DGSavedetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprtid = ((Label)DGSavedetail.Rows[e.RowIndex].FindControl("lblprtid"));
            TextBox txteditRecqty = ((TextBox)DGSavedetail.Rows[e.RowIndex].FindControl("txteditRecqty"));
            TextBox txteditremark = ((TextBox)DGSavedetail.Rows[e.RowIndex].FindControl("txteditremark"));
            TextBox txteditLossqty = ((TextBox)DGSavedetail.Rows[e.RowIndex].FindControl("txteditLossqty"));
            //*******
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@prtid", lblprtid.Text);
            param[1] = new SqlParameter("@RecQty", txteditRecqty.Text == "" ? "0" : txteditRecqty.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Remark", txteditremark.Text);
            param[4] = new SqlParameter("@userid", Session["varusserid"]);
            param[5] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[6] = new SqlParameter("@LossQty", txteditLossqty.Text == "" ? "0" : txteditLossqty.Text);
            //********execute proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateIndentReceive", param);
            lblmsg.Text = param[2].Value.ToString();
            //********         
            Tran.Commit();
            DGSavedetail.EditIndex = -1;
            FillGridsave();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGSavedetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprtid = ((Label)DGSavedetail.Rows[e.RowIndex].FindControl("lblprtid"));
            //*******
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@prtid", lblprtid.Text);
            param[1] = new SqlParameter("@userid", Session["varuserid"]);
            param[2] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //********execute proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteIndentReceive", param);
            lblmsg.Text = param[3].Value.ToString();
            //********         
            Tran.Commit();
            FillGridsave();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from V_IndentReceiveDetailReport Where prmid=" + ViewState["reportprmid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptgenerateindentReceivenew.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptgenerateindentReceivenew.xsd";
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

    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        break;
                }
            }
        }
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapename";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            //if (ddshape.Items.Count > 0)
            //{
            //    ddshape.SelectedIndex = 1;
            //    ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            //}

        }
        //Shade

        //Unit
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void Fillsize()
    {

        string str = null, size = null;
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;

        }

        str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + " order by Vf." + size;

        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void btngetdata_Click(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, "select OM.orderid,OM.LocalOrder+' # '+OM.CustomerOrderNo as orderNo from ordermaster OM where OM.CustomerId=" + DDCustomerCode.SelectedValue + " and OM.status=0  order by orderNo", true, "--Plz Select--");
    }
}