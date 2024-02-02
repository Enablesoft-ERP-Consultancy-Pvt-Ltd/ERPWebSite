using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_RawMaterial_frmindentrowissuenew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,Companyname from Companyinfo CI,Company_Authentication CA Where CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By Companyname 
                          select DISTINCT PROCESS_NAME_ID,process_name from PROCESS_NAME_MASTER pm inner join IndentMaster im on pm.PROCESS_NAME_ID=im.processid And pm.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PROCESS_NAME_ID";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref ddProcessName, ds, 1, true, "--Plz Select--");
            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            //Edit Checkbox
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            //
        }
    }
    protected void ddProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct EI.EmpId,EI.EmpName from IndentMaster  IM 
                       inner Join EmpInfo EI on IM.Partyid=EI.Empid where IM.ProcessID=" + ddProcessName.SelectedValue + " and IM.companyId=" + ddCompName.SelectedValue + " order by EmpName";
        UtilityModule.ConditionalComboFill(ref ddempname, str, true, "--Plz Select--");
    }
    protected void ddempname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str;
        if (chkedit.Checked == true)
        {
            str = @"select IndentID,IndentNo from Indentmaster where PartyId= " + ddempname.SelectedValue + @" and 
                    ProcessID=" + ddProcessName.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + " order by indentid desc";
        }
        else
        {
            str = @"select IndentID,IndentNo from Indentmaster where PartyId= " + ddempname.SelectedValue + @" and 
                    ProcessID=" + ddProcessName.SelectedValue + " and CompanyId=" + ddCompName.SelectedValue + " and status='Pending' order by indentid desc";
        }

        UtilityModule.ConditionalComboFill(ref ddindentno, str, true, "--Plz Select--");

    }
    protected void ddindentno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDChallanNo, @"select Distinct PRM.PRMid,PRM.ChallanNo+'/'+Replace(convert(nvarchar(11),PRM.Date,106),' ','-') from PP_ProcessRawmaster PRM inner Join PP_ProcessRawTran PRT 
                                                                 on PRM.PRMid=PRT.PRMid Where PRT.IndentId=" + ddindentno.SelectedValue + " order by PRM.PRMID desc", true, "--Plz Select--");

        }
        else
        {
            Fillgrid();
            txtchalanno.Text = "";
        }

    }
    protected void Fillgrid()
    {
        string str = @" select Vf.category_Name+' '+vf.item_name+' '+vf.QualityName+' '+vf.designName+' '+
                        vf.ColorName+' '+Vf.ShadeColorName+' '+vf.Shapename+
                        case When I.ISizeflag=0 Then Vf.SizeFt When I.ISizeflag=1 Then Vf.SizeMtr 
                        When I.ISizeflag=2 Then Vf.SizeInch Else Vf.SizeFt End+' '+case when vf.sizeid>0 then Sz.Type else '' End as ItemDescription,
                        Vf1.category_Name+' '+vf1.item_name+' '+vf1.QualityName+' '+vf1.designName+' '+
                        vf1.ColorName+' '+Vf1.ShadeColorName+' '+vf1.Shapename+
                        case When OD.flagsize=0 Then Vf1.SizeFt When OD.flagsize=1 Then Vf1.SizeMtr 
                        When OD.flagsize=2 Then Vf1.SizeInch Else Vf1.SizeFt End+' '+case when vf1.sizeid>0 
                        then Sz1.Type else '' End as Orderdescription,OM.CustomerOrderNo,
                        I.Orderdetailid,I.Ofinishedid,Ifinishedid,I.IUnitid,I.Isizeflag,Sum(I.IQty) as IQty,isnull(Sum(VI.issqty),0)as PreIssue,u.UnitName
                        from IndentQtytobeIssued I Inner Join V_FinishedItemDetail VF on
                        I.IFinishedid=Vf.Item_finished_id
                        inner join SizeType SZ on I.ISizeflag=SZ.Val
                        inner join orderdetail OD on I.Orderdetailid=OD.OrderDetailId
                        inner join OrderMaster OM on Om.OrderId=OD.OrderId
                        inner join V_FinishedItemDetail vf1 on od.item_finished_id=vf1.ITEM_FINISHED_ID
                        inner join SizeType SZ1 on Od.flagsize=SZ1.Val 
                        inner join Unit U on I.IunitId=u.UnitId
                        left Join V_PreIssueIndentQty VI on I.indentid=VI.indentid and I.orderdetailid=VI.orderdetailid
                        and I.Iunitid=VI.unitid and I.Isizeflag=Vi.flagsize and I.Ifinishedid=VI.finishedid
                        and I.ofinishedid=VI.ofinishedid where I.Indentid=" + ddindentno.SelectedValue + @"
                        group by 
                        Vf.category_Name,vf.item_name,vf.QualityName,vf.designName,
                        vf.ColorName,Vf.ShadeColorName,vf.Shapename,I.ISizeflag,Vf.SizeFt,Vf.SizeMtr,Vf.SizeInch,
                        vf.sizeid,Sz.Type,
                        Vf1.category_Name,Vf1.item_name,Vf1.QualityName,Vf1.designName,
                        Vf1.ColorName,Vf1.ShadeColorName,Vf1.Shapename,I.ISizeflag,Vf1.SizeFt,Vf1.SizeMtr,Vf1.SizeInch,
                        Vf1.sizeid,Sz1.Type,OD.flagsize,I.OFinishedid,I.ifinishedid,I.Orderdetailid,
                        OM.CustomerOrderNo,I.IUnitid,I.Isizeflag,u.UnitName";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGIssueDetail.DataSource = ds.Tables[0];
        DGIssueDetail.DataBind();
    }
    protected void DGIssueDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddgodown = ((DropDownList)e.Row.FindControl("DDgodown"));
            Label lblIfinishedid = ((Label)e.Row.FindControl("lblifinishedid"));
            string str = "select Distinct Gm.godownid,Gm.godownname from Stock S inner join GodownMaster Gm on S.godownid=Gm.godownid and S.companyid=" + ddCompName.SelectedValue + " and S.item_finished_id=" + lblIfinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref ddgodown, str, true, "--Plz Select--");
            if (ddgodown.Items.Count > 0)
            {
                ddgodown.SelectedIndex = 1;
                DDgodown_SelectedIndexChanged(ddgodown, new EventArgs());
            }
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        int index = row.RowIndex;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddLotno = ((DropDownList)row.FindControl("DDlotno"));
        string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + ddCompName.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, new EventArgs());
        }
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlotno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        Double StockQty = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid);

        //Get Issue Qty in Grid
        Double IssueQty = 0;
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            DropDownList ddgridgodown = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDgodown"));
            DropDownList ddgridLotno = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDlotno"));
            int gridIfinishedid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblifinishedid")).Text);
            TextBox txtissueqty = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtissQty"));

            if ((ddgodown.SelectedValue == ddgridgodown.SelectedValue) && (ddlotno.Text == ddgridLotno.Text) && (Ifinishedid == gridIfinishedid))
            {
                IssueQty = IssueQty + Convert.ToDouble((txtissueqty.Text == "" ? "0" : txtissueqty.Text));
            }
        }
        //
        lblstockqty.Text = (StockQty - IssueQty).ToString();

    }
    protected void txtiss_TextChanged(object sender, EventArgs e)
    {
        TextBox txtissqty = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtissqty.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)DGIssueDetail.Rows[index].FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)DGIssueDetail.Rows[index].FindControl("lblstockqty"));
        Label lblpendingQty = ((Label)DGIssueDetail.Rows[index].FindControl("lblpendingqty"));
        DropDownList ddgodown = ((DropDownList)DGIssueDetail.Rows[index].FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)DGIssueDetail.Rows[index].FindControl("DDlotno"));
        if (ddgodown.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please select Godown Name.');", true);
            txtissqty.Text = "";
            return;
        }
        Double StockQty = UtilityModule.getstockQty(ddCompName.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid);

        //Get Issue Qty in Grid
        Double IssueQty = 0;
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            DropDownList ddgridgodown = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDgodown"));
            DropDownList ddgridLotno = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDlotno"));
            int gridIfinishedid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblifinishedid")).Text);
            TextBox txtissueqty = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtissQty"));

            if ((ddgodown.SelectedValue == ddgridgodown.SelectedValue) && (ddlotno.Text == ddgridLotno.Text) && (Ifinishedid == gridIfinishedid))
            {
                IssueQty = IssueQty + Convert.ToDouble((txtissueqty.Text == "" ? "0" : txtissueqty.Text));
            }
        }
        if (Convert.ToDouble(lblpendingQty.Text == "" ? "0" : lblpendingQty.Text) < Convert.ToDouble(txtissqty.Text == "" ? "0" : txtissqty.Text))
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt", "alert('Issue Qty can not be greater than pending Qty.');", true);
            txtissqty.Text = "0";
            return;
        }
        lblstockqty.Text = (StockQty - IssueQty).ToString();
        //*******check stock
        if (MySession.Stockapply == "True")
        {
            if ((Convert.ToDouble(lblstockqty.Text == "" ? "0" : lblstockqty.Text)) < 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Issue Qty can not be greater than Stock Qty.');", true);
                txtissqty.Text = "0";
                return;
            }
        }
        //*******

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
            //***************
            DataTable dtrecords = new DataTable();
            dtrecords.Columns.Add("Ifinishedid", typeof(int));
            dtrecords.Columns.Add("Godownid", typeof(int));
            dtrecords.Columns.Add("Lotno", typeof(string));
            dtrecords.Columns.Add("Unitid", typeof(int));
            dtrecords.Columns.Add("Sizeflag", typeof(int));
            dtrecords.Columns.Add("IssQty", typeof(float));
            dtrecords.Columns.Add("Remark", typeof(string));
            dtrecords.Columns.Add("OrderDetailid", typeof(int));
            dtrecords.Columns.Add("Ofinishedid", typeof(int));
            for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
            {
                CheckBox chkitem = ((CheckBox)DGIssueDetail.Rows[i].FindControl("chkitem"));
                TextBox txtIssueQty = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtissQty"));
                DropDownList DDgodown = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDgodown"));
                DropDownList DDLotno = ((DropDownList)DGIssueDetail.Rows[i].FindControl("DDlotno"));
                if ((chkitem.Checked == true) && (Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text)) > 0)
                {
                    if ((DDgodown.SelectedIndex == -1 || DDgodown.SelectedIndex == 0) && (DDLotno.SelectedIndex == -1 || DDLotno.SelectedIndex == 0))
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('Please select godown and Lotno....');", true);
                        Tran.Commit();
                        return;
                    }
                    DataRow dr = dtrecords.NewRow();
                    int Ifinishedid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblifinishedid")).Text);
                    int unitid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblunitid")).Text);
                    int Sizflag = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblsizeflag")).Text);
                    string Remark = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtremark")).Text;
                    int orderdetailid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblorderdetailid")).Text);
                    int ofinishedid = Convert.ToInt32(((Label)DGIssueDetail.Rows[i].FindControl("lblofinishedid")).Text);
                    //***** assign Data into Datarow
                    dr["Ifinishedid"] = Ifinishedid;
                    dr["Godownid"] = DDgodown.SelectedValue;
                    dr["Lotno"] = DDLotno.Text;
                    dr["Unitid"] = unitid;
                    dr["Sizeflag"] = Sizflag;
                    dr["IssQty"] = txtIssueQty.Text;
                    dr["Remark"] = Remark;
                    dr["OrderDetailid"] = orderdetailid;
                    dr["Ofinishedid"] = ofinishedid;
                    dtrecords.Rows.Add(dr);
                }
            }
            //********************
            if (dtrecords.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@prmId", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = ViewState["prmid"];
                param[1] = new SqlParameter("@companyId", ddCompName.SelectedValue);
                param[2] = new SqlParameter("@Empid", ddempname.SelectedValue);
                param[3] = new SqlParameter("@processid", ddProcessName.SelectedValue);
                param[4] = new SqlParameter("@issueDate", txtdate.Text);
                param[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                param[5].Value = txtchalanno.Text;
                param[6] = new SqlParameter("@masterremark", txtmsterremark.Text);
                param[7] = new SqlParameter("@varuserid", Session["varuserid"]);
                param[8] = new SqlParameter("@varcompanyid", Session["varMasterCompanyIDForERP"]);
                param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[9].Direction = ParameterDirection.Output;
                param[10] = new SqlParameter("@dtrecord", dtrecords);
                param[11] = new SqlParameter("@prtid", 0);
                param[12] = new SqlParameter("@indentid", ddindentno.SelectedValue);
                //**********execute Proc
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_PP_PRM_SaveNew", param);
                lblmsg.Text = param[9].Value.ToString();
                ViewState["prmid"] = param[0].Value.ToString();
                ViewState["reportprmid"] = ViewState["prmid"];
                txtchalanno.Text = param[5].Value.ToString();
                Tran.Commit();
                FillGridsave();
                refreshcontrols();
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
            con.Dispose();
            con.Close();
        }
    }
    protected void refreshcontrols()
    {
        ddindentno.SelectedIndex = -1;
        txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["prmid"] = "0";
        DGIssueDetail.DataSource = null;
        DGIssueDetail.DataBind();
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_IndentDetail_ForReport] Where prmid=" + ViewState["reportprmid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptIndentrowissuenew.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptIndentrowissuenew.xsd";
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
                    then Sz1.Type else '' End as Orderdescription,PRT.IssueQuantity,PRT.Remark,U.UnitName,PRt.PRTid
                    from PP_ProcessRawMaster PRM inner Join PP_ProcessRawTran PRT on  PRM.PrmId=PRT.prmid
                    inner join V_FinishedItemDetail vf on PRT.finishedid=vf.item_finished_id
                    inner join sizetype sz on PRT.flagsize=sz.Val
                    inner join unit u on PRT.unitid=U.UnitId
                    Left join OrderDetail OD on PRT.Orderdetailid=OD.OrderDetailId
                    left join OrderMaster OM on OD.OrderId=OM.OrderId
                    left join V_FinishedItemDetail vf1 on OD.Item_Finished_Id=vf1.item_finished_id
                    left join sizetype Sz1 on OD.flagsize=sz1.val where PRM.Prmid=" + ViewState["reportprmid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGSavedetail.DataSource = ds.Tables[0];
        DGSavedetail.DataBind();
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select prmid,replace(convert(nvarchar(11),Date,106),' ','-') as IssueDate,ChallanNo,RiRemark from PP_ProcessRawmaster Where Prmid=" + DDChallanNo.SelectedValue + "";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtdate.Text = ds.Tables[0].Rows[0]["Issuedate"].ToString();
            txtchalanno.Text = ds.Tables[0].Rows[0]["challanNo"].ToString();
            txtmsterremark.Text = ds.Tables[0].Rows[0]["riremark"].ToString();
            ViewState["reportprmid"] = ds.Tables[0].Rows[0]["prmid"].ToString();
        }
        else
        {
            txtdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtchalanno.Text = "";
            txtmsterremark.Text = "";
            ViewState["reportprmid"] = "0";
        }
        FillGridsave();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            TDChallanNo.Visible = true;
        }
        else
        {
            TDChallanNo.Visible = false;
            DDChallanNo.SelectedIndex = -1;
        }
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
            TextBox txteditissueqty = ((TextBox)DGSavedetail.Rows[e.RowIndex].FindControl("txteditissqty"));
            TextBox txteditremark = ((TextBox)DGSavedetail.Rows[e.RowIndex].FindControl("txteditremark"));
            //*******
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@prtid", lblprtid.Text);
            param[1] = new SqlParameter("@IssueQty", txteditissueqty.Text == "" ? "0" : txteditissueqty.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Remark", txteditremark.Text);
            param[4] = new SqlParameter("@userid", Session["varusserid"]);
            param[5] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //********execute proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateIndentIssue", param);
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
            param[2] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            //********execute proc
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteIndentIssue", param);
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
}