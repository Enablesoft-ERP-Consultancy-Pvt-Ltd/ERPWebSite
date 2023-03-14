using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using CrystalDecisions.CrystalReports;
using System.Configuration;
using System.Text;

public partial class Masters_Order_FrmProductionTNAPLAN : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select CI.CompanyId,CompanyName From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varCompanyId"] + " order by CompanyName", true, "-Select Company Name");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
                fillcustomer();
            }

        }
    }
    private void fillcustomer()
    {
        if (DDCompanyName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref DDCustomerCode, "Select customerid,customercode from customerinfo where MasterCompanyid=" + Session["varcompanyno"] + " order by customercode", true, "-Select Customer-");
        }
    }
    private void fillorder()
    {
        if (DDCustomerCode.SelectedIndex > 0 && DDCompanyName.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddorderno, "Select orderid,localorder+'/'+customerorderno from ordermaster where customerid=" + DDCustomerCode.SelectedValue + " and companyid=" + DDCompanyName.SelectedValue + "", true, "-Select Order No-");
        }
    }
    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillorder();
    }
    private void fillGrid()
    {
        if (ddorderno.SelectedIndex > 0)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select orderdetailid as Sr_No,CATEGORY_NAME CATEGORY,ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShapeName ITEMNAME ,QtyRequired Qty,TotalArea Area
            from orderdetail od inner join v_finisheditemdetail vd On od.ITEM_FINISHED_ID=vd.ITEM_FINISHED_ID where orderid=" + ddorderno.SelectedValue + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGOrderDetail.DataSource = ds;
                DGOrderDetail.DataBind();
                trdrig.Style.Add("Display", "");
            }
            else
            {
                trdrig.Style.Add("Display", "none");
            }
        }
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }
    protected void Chkbox_checked(object sender, EventArgs e)
    {
        string order = "0";
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                order = DGOrderDetail.DataKeys[i].Value.ToString();
                i = DGOrderDetail.Rows.Count + 1;
                fillprocess(Convert.ToInt32(order));
            }
        }
        DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select replace(convert(varchar(11),max(Date),106),' ','-') from InspectionDateDetail where OrderID=" + ddorderno.SelectedValue + @" and OrderDetailID=" + order + @" and InspectionID=1
                     select replace(convert(varchar(11),max(Date),106),' ','-')  from InspectionDateDetail where orderid=" + ddorderno.SelectedValue + " and OrderDetailID=" + order + @" and InspectionID=2
                     select replace(convert(varchar(11),max(Date),106),' ','-') from InspectionDateDetail where orderid=" + ddorderno.SelectedValue + " and OrderDetailID=" + order + " and InspectionID=3");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            txtinline.Text = ds2.Tables[0].Rows[0][0].ToString();
        }
        if (ds2.Tables[1].Rows.Count > 0)
        {
            txtmidline.Text = ds2.Tables[1].Rows[0][0].ToString();
        }
        if (ds2.Tables[2].Rows.Count > 0)
        {
            txtfinal.Text = ds2.Tables[2].Rows[0][0].ToString();
        }
        if (ChkEditOrder.Checked == true)
        {
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select tnaid,replace(convert(varchar(11),yarncolourapp,106),' ','-') yarncolourapp ,
            replace(convert(varchar(11),PIECEAPP,106),' ','-') PIECEAPP,replace(convert(varchar(11),GOODRECEIVEAPP,106),' ','-') GOODRECEIVEAPP,
            replace(convert(varchar(11),TESTING,106),' ','-') TESTING,replace(convert(varchar(11),PREPRODUCTIONAPP,106),' ','-') PREPRODUCTIONAPP,
            replace(convert(varchar(11),PACKINGAPP,106),' ','-') PACKINGAPP,replace(convert(varchar(11),LABELARTAPP,106),' ','-') LABELARTAPP,
            replace(convert(varchar(11),LABELINHOUSEAPP,106),' ','-') LABELINHOUSEAPP,YARNCOLOURAPPROVED,PIECEAPPROVED,GOODRECEIVEAPPROVED,PREPRODUCTIONAPPROVED,
            PACKINGAPPROVED,LABELARTAPPROVED,LABELINHOUSEAPPROVED,TESTED,Inlineinspection,Midlineinspection,Finalinspection from TnaPlanning where orderdetailid=" + order + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtgoods.Text = ds.Tables[0].Rows[0]["GOODRECEIVEAPP"].ToString();
                txtlabel.Text = ds.Tables[0].Rows[0]["LABELARTAPP"].ToString();
                txtlabinhouse.Text = ds.Tables[0].Rows[0]["LABELINHOUSEAPP"].ToString();
                txtpacking.Text = ds.Tables[0].Rows[0]["PACKINGAPP"].ToString();
                txtpeice.Text = ds.Tables[0].Rows[0]["PIECEAPP"].ToString();
                txtpreprod.Text = ds.Tables[0].Rows[0]["PREPRODUCTIONAPP"].ToString();
                txttesting.Text = ds.Tables[0].Rows[0]["TESTING"].ToString();
                Txtyarndate.Text = ds.Tables[0].Rows[0]["yarncolourapp"].ToString();
                TXTYARAPPROVED.Text = ds.Tables[0].Rows[0]["YARNCOLOURAPPROVED"].ToString();
                TXTPIECEAPPROVED.Text = ds.Tables[0].Rows[0]["PIECEAPPROVED"].ToString();
                TXTGOODRECEIVED.Text = ds.Tables[0].Rows[0]["GOODRECEIVEAPPROVED"].ToString();
                TXTPREPRODAPPROVED.Text = ds.Tables[0].Rows[0]["PREPRODUCTIONAPPROVED"].ToString();
                TXTPACKINGAPPROVED.Text = ds.Tables[0].Rows[0]["PACKINGAPPROVED"].ToString();
                TXTLABELAPPROVED.Text = ds.Tables[0].Rows[0]["LABELARTAPPROVED"].ToString();
                TXTLABELINHOUSEAPPROVED.Text = ds.Tables[0].Rows[0]["LABELINHOUSEAPPROVED"].ToString();
                TXTTESTED.Text = ds.Tables[0].Rows[0]["TESTED"].ToString();
                txtinline.Text = ds.Tables[0].Rows[0]["Inlineinspection"].ToString();
                txtmidline.Text = ds.Tables[0].Rows[0]["Midlineinspection"].ToString();
                txtfinal.Text = ds.Tables[0].Rows[0]["Finalinspection"].ToString();
                DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select PROCESSID,replace(convert(varchar(11),PROCESSDATE,106),' ','-') as PROCESSDATE,APPROVEDDATE from TnaProcessDetail where TNAID=" + ds.Tables[0].Rows[0][0].ToString() + "");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    chkprocessdate.Checked = true;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < grdprocessdate.Rows.Count; j++)
                        {
                            if (ds1.Tables[0].Rows[i]["PROCESSID"].ToString() == ((Label)grdprocessdate.Rows[j].FindControl("lblprocessid")).Text)
                            {
                                ((CheckBox)grdprocessdate.Rows[j].FindControl("Chkbox")).Checked = true;
                                ((TextBox)grdprocessdate.Rows[j].FindControl("Txtdate")).Text = ds1.Tables[0].Rows[i]["PROCESSDATE"].ToString();
                                ((TextBox)grdprocessdate.Rows[j].FindControl("TxtApproveddate")).Text = ds1.Tables[0].Rows[i]["APPROVEDDATE"].ToString();
                                j = grdprocessdate.Rows.Count + 1;
                            }
                        }
                    }
                }
            }
        }
    }
    private void fillprocess(int order)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Distinct PROCESS_NAME_ID Sr_No,PROCESS_NAME process from ORDER_CONSUMPTION_DETAIL oc inner join 
        process_name_master p ON  p.PROCESS_NAME_ID=oc.processid where orderdetailid=" + order + "");
        grdprocessdate.DataSource = ds;
        grdprocessdate.DataBind();
        if (grdprocessdate.Rows.Count > 0)
        {
            tdprocess.Visible = true;
        }
        else
        {
            tdprocess.Visible = false;
        }
    }
    protected void DGOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
        e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
        e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderDetail, "Select$" + e.Row.RowIndex);
    }
    protected void BtnSave1_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            ViewState["TNAID"] = 0;
            ViewState["TNAPROCESSID"] = 0;
            string str = "";
            SqlParameter[] _arrPara = new SqlParameter[23];
            _arrPara[0] = new SqlParameter("@TNAID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@ORDERID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@ORDERDETAILID", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@YARNCOLOURAPP", SqlDbType.SmallDateTime);
            _arrPara[4] = new SqlParameter("@PIECEAPP", SqlDbType.SmallDateTime);
            _arrPara[5] = new SqlParameter("@GOODRECEIVEAPP", SqlDbType.SmallDateTime);
            _arrPara[6] = new SqlParameter("@PREPRODUCTIONAPP", SqlDbType.SmallDateTime);
            _arrPara[7] = new SqlParameter("@PACKINGAPP", SqlDbType.SmallDateTime);
            _arrPara[8] = new SqlParameter("@LABELARTAPP", SqlDbType.SmallDateTime);
            _arrPara[9] = new SqlParameter("@LABELINHOUSEAPP", SqlDbType.SmallDateTime);
            _arrPara[10] = new SqlParameter("@TESTING", SqlDbType.SmallDateTime);
            _arrPara[11] = new SqlParameter("@TNADATE", SqlDbType.SmallDateTime);
            _arrPara[12] = new SqlParameter("@YARNCOLOURAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[13] = new SqlParameter("@PIECEAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[14] = new SqlParameter("@GOODRECEIVEAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[15] = new SqlParameter("@PREPRODUCTIONAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[16] = new SqlParameter("@PACKINGAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[17] = new SqlParameter("@LABELARTAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[18] = new SqlParameter("@LABELINHOUSEAPPROVED", SqlDbType.NVarChar, 50);
            _arrPara[19] = new SqlParameter("@TESTED", SqlDbType.NVarChar, 50);
            _arrPara[20] = new SqlParameter("@Inlineinspection", SqlDbType.NVarChar, 50);
            _arrPara[21] = new SqlParameter("@midlineinspection", SqlDbType.NVarChar, 50);
            _arrPara[22] = new SqlParameter("@finalinspection", SqlDbType.NVarChar, 50);
            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[0].Value = ViewState["TNAID"];
            _arrPara[1].Value = ddorderno.SelectedValue;
            for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
            {
                if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    _arrPara[2].Value = DGOrderDetail.DataKeys[i].Value;
                    i = DGOrderDetail.Rows.Count + 1;
                }
            }
            _arrPara[3].Value = Txtyarndate.Text != "" ? Txtyarndate.Text : "''";
            _arrPara[4].Value = txtpeice.Text != "" ? txtpeice.Text : "''";
            _arrPara[5].Value = txtgoods.Text != "" ? txtgoods.Text : "''";
            _arrPara[6].Value = txtpreprod.Text != "" ? txtpreprod.Text : "''";
            _arrPara[7].Value = txtpacking.Text != "" ? txtpacking.Text : "''";
            _arrPara[8].Value = txtlabel.Text != "" ? txtlabel.Text : "''";
            _arrPara[9].Value = txtlabinhouse.Text != "" ? txtlabinhouse.Text : "''";
            _arrPara[10].Value = txttesting.Text != "" ? txttesting.Text : "''";
            _arrPara[11].Value = DateTime.Now.Date;
            _arrPara[12].Value = TXTYARAPPROVED.Text;
            _arrPara[13].Value = TXTPIECEAPPROVED.Text;
            _arrPara[14].Value = TXTGOODRECEIVED.Text;
            _arrPara[15].Value = TXTPREPRODAPPROVED.Text;
            _arrPara[16].Value = TXTPACKINGAPPROVED.Text;
            _arrPara[17].Value = TXTLABELAPPROVED.Text;
            _arrPara[18].Value = TXTLABELINHOUSEAPPROVED.Text;
            _arrPara[19].Value = TXTTESTED.Text;
            _arrPara[20].Value = txtinline.Text;
            _arrPara[21].Value = txtmidline.Text;
            _arrPara[22].Value = txtfinal.Text;
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_TNAPLANING", _arrPara);
            ViewState["TNAID"] = _arrPara[0].Value;
            if (chkprocessdate.Checked == true)
            {
                SqlParameter[] _arrPara1 = new SqlParameter[5];
                _arrPara1[0] = new SqlParameter("@TNAPROCESSID", SqlDbType.Int);
                _arrPara1[1] = new SqlParameter("@TNAID", SqlDbType.Int);
                _arrPara1[2] = new SqlParameter("@PROCESSID", SqlDbType.Int);
                _arrPara1[3] = new SqlParameter("@PROCESSDATE", SqlDbType.SmallDateTime);
                _arrPara1[4] = new SqlParameter("@APPROVEDDATE", SqlDbType.NVarChar, 50);

                _arrPara1[0].Direction = ParameterDirection.InputOutput;
                _arrPara1[0].Value = ViewState["TNAPROCESSID"];
                _arrPara1[1].Value = ViewState["TNAID"].ToString();
                for (int i = 0; i < grdprocessdate.Rows.Count; i++)
                {
                    if (((CheckBox)grdprocessdate.Rows[i].FindControl("Chkbox")).Checked == true)
                    {
                        _arrPara1[2].Value = ((Label)grdprocessdate.Rows[i].FindControl("lblprocessid")).Text;
                        _arrPara1[3].Value = ((TextBox)grdprocessdate.Rows[i].FindControl("Txtdate")).Text != "" ? ((TextBox)grdprocessdate.Rows[i].FindControl("Txtdate")).Text : "''";
                        _arrPara1[4].Value = ((TextBox)grdprocessdate.Rows[i].FindControl("TxtApproveddate")).Text;
                        if (((TextBox)grdprocessdate.Rows[i].FindControl("Txtdate")).Text != "")
                        {
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_TNAprocessid", _arrPara1);
                        }
                        ViewState["TNAPROCESSID"] = _arrPara1[0].Value;
                    }
                }
            }
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Saved Successfully!');", true);
            refresh();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void refresh()
    {
        Txtyarndate.Text = "";
        txtpeice.Text = "";
        txtgoods.Text = "";
        txtlabel.Text = "";
        txtlabinhouse.Text = "";
        txtpacking.Text = "";
        txtpreprod.Text = "";
        txttesting.Text = "";
        TXTGOODRECEIVED.Text = "";
        TXTLABELAPPROVED.Text = "";
        TXTLABELINHOUSEAPPROVED.Text = "";
        TXTPACKINGAPPROVED.Text = "";
        TXTPIECEAPPROVED.Text = "";
        TXTPREPRODAPPROVED.Text = "";
        TXTTESTED.Text = "";
        TXTYARAPPROVED.Text = "";
        txtinline.Text = "";
        txtmidline.Text = "";
        txtfinal.Text = "";
        for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
        {
            if (((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked == true)
            {
                ((CheckBox)DGOrderDetail.Rows[i].FindControl("Chkbox")).Checked = false;
                i = DGOrderDetail.Rows.Count + 1;
            }
        }
        if (chkprocessdate.Checked == true)
        {
            for (int i = 0; i < grdprocessdate.Rows.Count; i++)
            {
                if (((CheckBox)grdprocessdate.Rows[i].FindControl("Chkbox")).Checked == true)
                {
                    ((CheckBox)grdprocessdate.Rows[i].FindControl("Chkbox")).Checked = false;
                    ((TextBox)grdprocessdate.Rows[i].FindControl("Txtdate")).Text = "";
                    ((TextBox)grdprocessdate.Rows[i].FindControl("TxtApproveddate")).Text = "";
                }
            }
        }
        chkprocessdate.Checked = false;
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (DDCustomerCode.Items.Count > 0)
        {
            DDCustomerCode.SelectedIndex = 0;
        }
        if (ddorderno.Items.Count > 0)
        {
            ddorderno.SelectedIndex = 0;
        }
    }
    protected void btnreport_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = "", str = "", str2 = "", str3 = "", str4 = "", str5 = "";
            double sumrecqty = 0;
            DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select Distinct processid ,pm.PROCESS_NAME,pm.ProcessType
        from TnaProcessDetail td inner join TnaPlanning tp ON td.tnaid=tp.tnaid inner join process_name_master pm On pm.PROCESS_NAME_ID=td.processid   where tp.orderid=" + ddorderno.SelectedValue + "");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    str1 = str1 + "," + "[dbo].[F_TNAPROCESSDATE](tp.tnaid," + ds1.Tables[0].Rows[i][0].ToString() + ") as '" + ds1.Tables[0].Rows[i]["PROCESS_NAME"].ToString() + "'";
                }
            }
            str = @"select replace(convert(varchar(11),planningdate,106),' ','-') as PlanDate,CATEGORY_NAME,
                     ITEM_NAME+' '+QualityName+' '+designName+' '+ColorName+' '+ShadeColorName+' '+ShapeName as DESCRIPTION,SizeMtr as 'SIZE',sum(QtyRequired) as QTY ,
                     replace(convert(varchar(11),YARNCOLOURAPP,106),' ','-') as 'YARN COLOR APPROVAL',replace(convert(varchar(11),PIECEAPP,106),' ','-') as 'PIECE APPROVAL',replace(convert(varchar(11),GOODRECEIVEAPP,106),' ','-') 'GOOD RECEIVAL' " + str1 + @",
                     replace(convert(varchar(11),PREPRODUCTIONAPP,106),' ','-') as 'PRE PRODUCTION APPROVAL',replace(convert(varchar(11),PACKINGAPP,106),' ','-') 'PACKAGING APPROVAL',replace(convert(varchar(11),LABELARTAPP,106),' ','-') 'LABEL ARTWORK APPROVAL',
                     replace(convert(varchar(11),LABELINHOUSEAPP,106),' ','-') 'LABELLING INHOUSE',replace(convert(varchar(11),TESTING,106),' ','-') 'TESTING',
                     (select replace(convert(varchar(11),max(Date),106),' ','-') from InspectionDateDetail where OrderID=om.orderid and OrderDetailID=od.OrderDetailID and InspectionID=1) as INLINE,
                     (select replace(convert(varchar(11),max(Date),106),' ','-')  from InspectionDateDetail where orderid=om.orderid and OrderDetailID=od.OrderDetailID and InspectionID=2) as MIDLINE,
                     (select replace(convert(varchar(11),max(Date),106),' ','-') from InspectionDateDetail where orderid=om.orderid and OrderDetailID=od.OrderDetailID and InspectionID=3) as FINAL
            From ordermaster Om inner join orderdetail od On om.orderid=od.orderid inner join 
            OrderProductionPalanning op On op.orderid=od.orderid and op.orderdetailid=od.orderdetailid inner join 
            V_FinishedItemDetail vd On vd.ITEM_FINISHED_ID=od.ITEM_FINISHED_ID inner join TnaPlanning tp On tp.ORDERDETAILID=od.orderdetailid and tp.orderid=om.orderid
            where om.orderid=" + ddorderno.SelectedValue + @"
            group by planningdate,CATEGORY_NAME,ITEM_NAME,QualityName,designName,ColorName,ShadeColorName,ShapeName,SizeMtr,YARNCOLOURAPP,PIECEAPP,GOODRECEIVEAPP,
            PREPRODUCTIONAPP,PACKINGAPP,LABELARTAPP,LABELINHOUSEAPP,TESTING,om.orderid,od.orderdetailid,tp.tnaid";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            DgExcel.DataSource = ds;
            DgExcel.DataBind();
            DataSet ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"select isnull(sum(QTY),0) as RecQty,replace(convert(varchar(11),ReceiveDate,106),' ','-') as recdate from PurchaseReceiveDetail prd inner join 
            PurchaseIndentIssue pii On prd.PIndentIssueId=pii.PIndentIssueId inner join 
            PurchaseIndentMaster pim On pim.pindentid=pii.indentid inner join PurchaseReceiveMaster prm On prm.PurchaseReceiveId=prd.PurchaseReceiveId
            where pim.orderid=" + ddorderno.SelectedValue + @"
            group by ReceiveDate,prd.PurchaseReceiveId,pim.orderid");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                {
                    str3 = str3 + ",'" + ds3.Tables[0].Rows[i][0].ToString() + "  " + ds3.Tables[0].Rows[i][1].ToString() + "' as 'RECD QTY & DATE" + i + "'";
                }
                sumrecqty = Convert.ToDouble(ds3.Tables[0].Compute("sum(RecQty)", ""));
            }
            else
            {
                sumrecqty = 0;
            }
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (ds1.Tables[0].Rows[i]["ProcessType"].ToString() == "0")
                    {
                        DataSet ds5 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select isnull(sum(issqty)-sum(recqty),0) from V_RowMaterialIssueReceive where orderid=" + ddorderno.SelectedValue + "");
                        str4 = str4 + "," + ds5.Tables[0].Rows[0][0].ToString() + "as '" + ds1.Tables[0].Rows[i]["PROCESS_NAME"].ToString() + "'";
                    }
                    else if (ds1.Tables[0].Rows[i]["ProcessType"].ToString() == "1")
                    {
                        DataSet ds5 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(pqty,0) from PROCESS_ISSUE_DETAIL_" + ds1.Tables[0].Rows[0]["processid"].ToString() + " where orderid=" + ddorderno.SelectedValue + "");
                        str4 = str4 + "," + ds5.Tables[0].Rows[0][0].ToString() + " as '" + ds1.Tables[0].Rows[i]["PROCESS_NAME"].ToString() + "'";
                    }
                }
            }
            DataSet ds6 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Distinct item_name from LabelDetail ld inner join Item_Master im On im.ITEM_ID=ld.labelid where orderid=" + ddorderno.SelectedValue + "");
            if (ds6.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds6.Tables[0].Rows.Count; i++)
                {
                    str5 = str5 + ", 0 as '" + ds6.Tables[0].Rows[i][0].ToString() + "'";
                }
            }

            str2 = @"select replace(convert(varchar(11) ,getdate(),106),' ','-')as DATE,YARNCOLOURAPPROVED,PIECEAPPROVED,
            GOODRECEIVEAPPROVED,PREPRODUCTIONAPPROVED,PACKINGAPPROVED,LABELARTAPPROVED,LABELINHOUSEAPPROVED,TESTED ,[dbo].[F_PurchaseorderItem](orderid) as FABRIC,
            [dbo].[F_PurchaseorderQty](Orderid) as ORDEREDQTY " + str3 + @" ,([dbo].[F_PurchaseorderQty](Orderid)-" + sumrecqty + @") as BALQTY" + str4 + @",(select isnull(sum(SalesQty),0)  from V_TotalPackedQty where orderid=" + ddorderno.SelectedValue + @") as PACKQTY" + str5 + @",INLINEINSPECTION,MIDLINEINSPECTION,FINALINSPECTION
            From TnaPlanning  where orderid=" + ddorderno.SelectedValue + "";
            DataSet ds4 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
            DgExcel1.DataSource = ds4;
            DgExcel1.DataBind();
            if (DgExcel.Rows.Count > 0)
            {
                DataSet DS2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select DISTINCT ci.customercode,replace(convert(varchar(11),Custorderdate,106),' ','-') as Custorderdate, localorder+'/'+customerorderno as orderno,REPLACE(CONVERT(VARCHAR(11),OM.DispatchDate,106),' ','-') DispatchDate,REPLACE(CONVERT(VARCHAR(11),GETDATE(),106),' ','-') AS REVIEWDATE
                From ordermaster om Inner join orderdetail od On om.orderid=od.orderid inner join customerinfo ci On ci.customerid=om.customerid where om.orderid=" + ddorderno.SelectedValue + "");
                DgExcel.Style.Add("font-size", "1em");
                Response.Clear();
                string attachment = "attachment; filename=PRODUCTION TNA PLAN.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                DgExcel.RenderControl(htmlWrite);
                Response.Write(@"<table><tr style=background-color:black;><td colspan=10 align=left style=color:White;font-size:small;font-weight:bold>PRODUCTION TNA PLAN</td></tr><tr style=background-color:black;><tdcolspan=10 align=left style=color:White;></td></tr>
                <tr style=background-color:#3F4040;><td style=color:White;>BUYER CODE</td><td align=left style=color:White;>" + DS2.Tables[0].Rows[0]["customercode"].ToString() + @"</td><td style=color:White;>DATE</td><td align=left style=color:White;>" + DS2.Tables[0].Rows[0]["Custorderdate"].ToString() + @"</td><td style=color:White;>ORDER ISSUD TO PRODUCTION</td><td align=left style=color:White;></td></tr>
                <tr style=background-color:#3F4040;><td style=color:White;>ORDER#</td><td align=left style=color:White;>" + DS2.Tables[0].Rows[0]["orderno"].ToString() + "</td><td style=color:White;>SHIP DATE</td><td align=left style=color:White;>" + DS2.Tables[0].Rows[0]["DispatchDate"].ToString() + "</td><td style=color:White;>ORDER REVIEW</td><td align=left style=color:White;>" + DS2.Tables[0].Rows[0]["REVIEWDATE"].ToString() + "</td><tr><tr style=background-color:#3F4040;;><td colspan=10 align=left style=color:White;></td></tr></table>" + stringWrite.ToString() + "<table><tr></tr><tr></tr></table>");

                System.IO.StringWriter stringWrite1 = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite1 = new HtmlTextWriter(stringWrite1);

                DgExcel1.Style.Add("font-size", "1em");
                DgExcel1.RenderControl(htmlWrite1);
                Response.Write(stringWrite1.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('" + ex.Message + "');", true);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
    }
    //protected void DgExcel_RowCreated(object sender, GridViewRowEventArgs e)
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
}