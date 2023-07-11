using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports;
using System.Text;
using System.IO;

public partial class Masters_Process_frm_receive_process_nextForAnisa : System.Web.UI.Page
{
    private const string SCRIPT_DOFOCUS =
  @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            //replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request["__LASTFOCUS"]
            //and registers the script to start after Update panel was rendered
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Process_frm_receive_process_nextForAnisa),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            string str = "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI where  CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order by Companyname
                          select unitid,unitname from unit where unitid in (1,2)
                          select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId";
            if (Session["varcompanyId"].ToString() == "8")
            {
                str = str + " select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 order by Process_Name ";
            }
            else
            {
                str = str + @" Select PNM.PROCESS_NAME_ID, PNM.Process_name 
                    From PROCESS_NAME_MASTER PNM(Nolock) 
                    JOIN UserRightsProcess URP(Nolock) ON URP.ProcessId = PNM.PROCESS_NAME_ID And URP.Userid = " + Session["varuserid"] + @" 
                    Where PNM.PROCESS_NAME_ID <> 1 and PNM.Processtype = 1 order by PNM.Process_Name ";
            }
            str = str + " select ICm.CATEGORY_ID,ICM.CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM inner join CategorySeparate cs on ICM.CATEGORY_ID=Cs.Categoryid and cs.id=0 order by CATEGORY_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(str);

            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "--SELECT--");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            ddprocess.Focus();
            ViewState["recid"] = 0;
            // UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 2, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 3, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcattype, ds, 4, true, "--SELECT--");
            if (ddcattype.Items.Count > 0)
            {
                ddcattype.SelectedIndex = 1;
                ddcattype_SelectedIndexChanged(ddcattype, new EventArgs());

            }
            TxtreceiveDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            int VarCompanyNo = Convert.ToInt32(Session["varCompanyId"].ToString());
            switch (VarCompanyNo)
            {
                case 5:
                    DDUnit.SelectedValue = "1";
                    break;
                case 4:
                    DDUnit.SelectedValue = "2";
                    break;
                case 8:
                    // Tdrecdate.Visible = false;
                    TDQDCS.Visible = false;
                    TDButtonsavegrid.Visible = false;
                    TRWeight.Visible = false;
                    TDcheckedby.Visible = false;
                    break;
                case 14:
                    TDBulkReceiveQty.Visible = true;
                    TxtBulkReceiveQty.Enabled = true;
                    TxtBulkReceiveQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    break;
                //case 14:
                //    if (Session["Usertype"].ToString() == "1")
                //    {
                //        btnsavegrid.Visible = true;
                //    }
                //    else
                //    {
                //        btnsavegrid.Visible = false;
                //    }
                //    break;
                case 22:
                    DDUnit.SelectedValue = "1";
                    TDBulkReceiveQty.Visible = false;
                    TRStockNoRemark.Visible = true;
                    TRWeight.Visible = true;
                    TDFinishingDateStamp.Visible = true;                  
                    TDCottonMoisture.Visible = true;
                    TDWoolMoisture.Visible = true;
                    break;
                default:
                    DDUnit.SelectedValue = "1";
                    TDBulkReceiveQty.Visible = false;
                    TRStockNoRemark.Visible = false;
                    break;
            }
            //Fill_Temp_OrderNo();
            //qulitychk.Visible = false;
            hnorderid.Value = "0";
            if (variable.VarQctype == "1")
            {
                btnQcPreview.Visible = true;
            }
        }
    }
    private void Fill_Temp_OrderNo()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_RECEIVE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where Process_Name_Id<>1 And MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name_Id");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_RECEIVE_MASTER_NEW SELECT CompanyId,EmpId,Process_Rec_Id," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId FROM PROCESS_RECEIVE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + "");
            }
        }
    }
    protected void ddprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChanges();
    }
    private void ProcessSelectedChanges()
    {
        ViewState["recid"] = 0;
        UtilityModule.ConditionalComboFill(ref ddemp, "Select Distinct EI.Empid,EI.EmpName From Empinfo EI,process_issue_master_" + ddprocess.SelectedValue + " PM Where EI.EmpId=PM.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT DISTINCT OCALTYPE FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD WHERE PM.PCMID=PD.PCMID And PROCESSID=" + ddprocess.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["OCALTYPE"].ToString();
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "PLS DEFINE RATE OR CONSUMPTION OF " + ddprocess.SelectedItem.Text + " PROCESS";
            ddprocess.SelectedIndex = 0;
        }
    }
    protected void ddemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        EmpSelectedChanges();
    }
    private void EmpSelectedChanges()
    {
        ViewState["recid"] = 0;
        if (ddemp.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                string Str = "Select Process_Rec_Id,ChallanNo From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where CompanyId=" + ddCompName.SelectedValue + " And Empid=" + ddemp.SelectedValue + "";
                UtilityModule.ConditionalComboFill(ref DDChallanNo, Str, true, "--SELECT--");
            }
            string str = @" select distinct issueorderid,ChallanNo from process_issue_master_" + ddprocess.SelectedValue + " where empid=" + ddemp.SelectedValue + " ";
            str = str + @" Select distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddorderno, ds, 0, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcattype, ds, 1, true, "--Select Category--");
        }
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddorderno.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddcattype, "select distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varCompanyId"] + " Order by CATEGORY_ID", true, "--Select Category--");
        }
        fillgrid();
    }
    protected void ddcattype_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlcategorycange();

        //UtilityModule.ConditionalComboFill(ref dditem, "select Item_id, Item_Name from Item_Master where Category_Id=" + ddcattype.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "---Select Item----");
        UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName From ITEM_MASTER IM inner join CategorySeparate CS on 
                                                         IM.CATEGORY_ID=cs.Categoryid and cs.id=0  inner join Quality Q on IM.ITEM_ID=q.Item_Id and Cs.Categoryid=" + ddcattype.SelectedValue + " and Im.mastercompanyid=" + Session["varcompanyid"] + " order by Qualityname", true, "--Plz Select--");
    }
    private void ddlcategorycange()
    {
        tdquality.Visible = false;
        tddesign.Visible = false;
        tdcolor.Visible = false;
        tdshape.Visible = false;
        tdsize.Visible = false;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "SELECT PARAMETER_ID FROM ITEM_CATEGORY_PARAMETERS where CATEGORY_ID=" + ddcattype.SelectedValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["PARAMETER_ID"].ToString())
                {
                    case "1":
                        tdquality.Visible = true;
                        break;
                    case "2":
                        tddesign.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddldesig, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Designname", true, "--Select Design--");
                        break;
                    case "3":
                        tdcolor.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolour, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varCompanyId"] + " Order by Colorname", true, "--Select Color--");
                        break;
                    case "4":
                        tdshape.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,Shapename from Shape Where MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Shape--");
                        tdsize.Visible = true;
                        break;
                }
            }
        }
    }
    protected void dditem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dditem.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddquality, "Select QualityId,QualityName from Quality Where Item_Id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Quality--");
        }
        fillgrid();
    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsize();
    }
    private void fillsize()
    {
        if (DDUnit.SelectedValue == "2")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,SizeFt from Size where ShapeId=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Size--");
        }
        else if (DDUnit.SelectedValue == "1")
        {
            UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,SizeMtr from Size where ShapeId=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varCompanyId"] + "", true, "--Select Size--");
        }
        fillgrid();
    }
    protected void ChkFt_CheckedChanged(object sender, EventArgs e)
    {
        if (ddshape.SelectedIndex > 0)
        {
            fillsize();
        }
        fillgrid();
    }
    private void fillgrid()
    {
        LblErrorMessage.Text = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {


            if (ddCompName.SelectedIndex > 0 && ddprocess.SelectedIndex > 0)
            {
                string strsql = @"Select vd.CATEGORY_NAME,vd.Item_Name,vd.qualityname+'  '+ vd.designname+'  '+vd.colorname+'  '+ShapeName+'  '+ Case When pim.UnitId=1 Then 
                           vd.sizemtr Else vd.sizeft End Description,CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,Case When pim.UnitId=1 Then vd.areamtr Else vd.areaft End Area 
                           From v_finisheditemdetail vd,CarpetNumber CN,Process_Stock_Detail psd,process_issue_detail_" + ddprocess.SelectedValue + @" pid,process_issue_master_" + ddprocess.SelectedValue + @" pim
                           Where cn.item_finished_id=vd.item_finished_id and psd.stockno=cn.stockno and psd.toprocessid=" + ddprocess.SelectedValue + @" and 
                           pid.issue_detail_id=psd.issuedetailid and pim.issueorderid=pid.issueorderid and cn.currentprostatus=" + ddprocess.SelectedValue + @" and 
                           issrecstatus=1 and pim.CompanyId=" + ddCompName.SelectedValue + " And Vd.MasterCompanyId=" + Session["varCompanyId"];
                if (ddemp.SelectedIndex > 0)
                {
                    strsql = strsql + " And pim.empid=" + ddemp.SelectedValue;
                }
                if (ddorderno.SelectedIndex > 0)
                {
                    strsql = strsql + " And pim.issueorderid=" + ddorderno.SelectedValue;
                }
                if (ddcattype.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.Category_Id=" + ddcattype.SelectedValue;
                }
                if (dditem.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.Item_Id=" + dditem.SelectedValue;
                }
                if (ddquality.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.QualityId=" + ddquality.SelectedValue;
                }
                if (ddldesig.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.DesignId=" + ddldesig.SelectedValue;
                }
                if (ddcolour.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.ColorId=" + ddcolour.SelectedValue;
                }
                if (ddshape.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.ShapeId=" + ddshape.SelectedValue;
                }
                if (ddsize.SelectedIndex > 0)
                {
                    strsql = strsql + " And vd.SizeId=" + ddsize.SelectedValue;
                }
                strsql = strsql + " group by cn.item_finished_id,vd.CATEGORY_NAME,vd.Item_Name ,vd.qualityname,vd.designname,vd.colorname,ShapeName,cn.orderid,sizemtr,sizeft,areaft,areamtr,pim.UnitId";
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                //mygdv.DataSource = ds;
                //mygdv.DataBind();
                hnorderid.Value = "0";
                hn_finished.Value = "0";
                GetStock();
            }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    protected void ddldesig_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddcolour, "select Distinct vf.ColorId,vf.ColorName From V_FinishedItemDetail vf where vf.QualityId=" + ddquality.SelectedValue + " and vf.designid=" + ddldesig.SelectedValue + " and vf.colorid<>0 order by vf.colorname", true, "--Plz Select--");
        //fillgrid();
    }
    protected void ddcolour_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddsize, "select Distinct vf.sizeid,vf.SizeMtr+' '+ vf.shapename as size From V_FinishedItemDetail vf where vf.QualityId=" + ddquality.SelectedValue + " and vf.designid=" + ddldesig.SelectedValue + " and vf.colorid=" + ddcolour.SelectedValue + " and vf.sizeid<>0 order by Size", true, "--Plz Select--");
        //fillgrid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    //protected void mygdv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    Label t_code = (Label)mygdv.Rows[e.RowIndex].FindControl("lblfinishedid");
    //    Label orderid = (Label)mygdv.Rows[e.RowIndex].FindControl("lblorderid");
    //    hn_finished.Value = t_code.Text;
    //    hnorderid.Value = orderid.Text;
    //    GetStock();
    //}
    private void GetStock()
    {
        DataSet ds1 = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            con.Open();
            string strsql = @"Select Distinct CN.StockNo,CN.TStockNo,[dbo].[GET_Process_Rate](CN.OrderId,CN.Item_Finished_Id," + ddprocess.SelectedValue + @") Rate 
                              From CarpetNumber CN,Process_Stock_Detail PSD Where CN.StockNo=PSD.StockNo And 
                              CN.CompanyId=" + ddCompName.SelectedValue + " And CN.Item_Finished_Id=" + hn_finished.Value + @" and CN.IssRecStatus=1 And 
                              CN.OrderId=" + hnorderid.Value + " And CurrentProStatus=" + ddprocess.SelectedValue + @" And IssueDetailId in (Select Issue_Detail_Id From 
                              process_issue_master_" + ddprocess.SelectedValue + " PM,process_issue_detail_" + ddprocess.SelectedValue + @" PD 
                              Where PM.IssueOrderId=PD.IssueOrderId And CN.CompanyId=" + ddCompName.SelectedValue;
            if (ddemp.SelectedIndex > 0)
            {
                strsql = strsql + " And PM.EmpID=" + ddemp.SelectedValue;
            }
            if (ddorderno.SelectedIndex > 0)
            {
                strsql = strsql + " And PM.IssueOrderId=" + ddorderno.SelectedValue;
            }
            strsql = strsql + ")";
            ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //mygdstock.DataSource = ds1;
                //mygdstock.DataBind();
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    //protected void mygdv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    hn_finished.Value = ((Label)mygdv.Rows[e.RowIndex].FindControl("lblfinishedid")).Text;
    //    hnorderid.Value = ((Label)mygdv.Rows[e.RowIndex].FindControl("lblorderid")).Text;
    //    Hn_Qty.Value = ((Label)mygdv.Rows[e.RowIndex].FindControl("lblqty")).Text;
    //    save_carpet_wise(0, "");
    //}
    //protected void mygdstock_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    Label stockno = (Label)mygdstock.Rows[e.RowIndex].FindControl("lblstockno");
    //    TextBox rate1 = (TextBox)mygdstock.Rows[e.RowIndex].FindControl("txtRate");
    //    hnstockno.Value = stockno.Text;
    //    hnrate1.Value = rate1.Text;
    //    save_carpet_wise(1, "");
    //}

    private void CHECKVALIDCONTROL()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(ddprocess) == false)
        {
            goto a;
        }
        if (Session["VarCompanyId"].ToString() == "22" && ddprocess.SelectedItem.Text == "TABLE CHECKING")
        {
            if (TDFinishingDateStamp.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtFinishingReceiveDateStamp) == false)
                {
                    goto a;
                }
            }
            if (TRWeight.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtweight) == false)
                {
                    goto a;
                }
            }
            if (TDCottonMoisture.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtReceiveCottonMoisture) == false)
                {
                    goto a;
                }
            }
            if (TDWoolMoisture.Visible == true)
            {
                if (UtilityModule.VALIDTEXTBOX(txtReceiveWoolMoisture) == false)
                {
                    goto a;
                }
            }
        }
        else if (Session["VarCompanyId"].ToString() == "22" && (ddprocess.SelectedItem.Text == "FINISHING" || ddprocess.SelectedItem.Text == "RE-WORK FINISHING"))
        {
            if (UtilityModule.VALIDTEXTBOX(txtcheckedby) == false)
            {
                goto a;
            }
        }

        if (UtilityModule.VALIDTEXTBOX(TxtreceiveDate) == false)
        {
            goto a;
        }      

        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }

    private void save_carpet_wise(int FlagType, string EmpId, string StrTStockNos)
    {
        if (Session["VarCompanyId"].ToString() == "22" && (ddprocess.SelectedItem.Text == "TABLE CHECKING" || ddprocess.SelectedItem.Text == "FINISHING" || ddprocess.SelectedItem.Text == "RE-WORK FINISHING"))
        {
            CHECKVALIDCONTROL();       
        }

        if (LblErrorMessage.Text == "")
        {
            string Str = "";
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            SqlTransaction Tran = con.BeginTransaction();
            try
            {

                if (Hn_ProcessId.Value != ddprocess.SelectedValue)
                {
                    LblErrorMessage.Text = "Stock Issued in differnet process!";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Stock Issued in differnet process!');", true);
                    TxtStockNo.Text = "";
                    TxtStockNo.Focus();
                    return;
                
                }
                SqlParameter[] _arrpara = new SqlParameter[37];
                DataSet ds3 = new DataSet();
                Str = "Select CalType,UnitId,PM.IssueOrderId,PD.issue_Detail_id,length,width,area,rate,Amount,PD.Item_Finished_id,PD.orderid,PM.remarks,Cn.Companyid From CarpetNumber CN,Process_Stock_Detail PSD,Process_Issue_Master_" + Hn_ProcessId.Value + " PM,Process_Issue_Detail_" + Hn_ProcessId.Value + @" PD 
                       Where CN.StockNo=PSD.StockNo And PSD.IssueDetailId=PD.Issue_Detail_Id And PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0 And CN.TStockNo='" + TxtStockNo.Text + @"' And 
                       PSD.ToProcessId=" + Hn_ProcessId.Value + " And PM.IssueOrderId in(select IssueOrderId From Employee_ProcessOrderNo Where ProcessId=" + Hn_ProcessId.Value + " And EmpId in(" + EmpId + ")) And PM.CompanyId=" + ddCompName.SelectedValue;
                ds3 = SqlHelper.ExecuteDataset(Tran, CommandType.Text, Str);
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ViewState["recid"]) != 0)
                    {
                        if (Convert.ToInt32(ds3.Tables[0].Rows[0]["CalType"]) != Convert.ToInt32(DDcaltype.SelectedValue))
                        {
                            LblErrorMessage.Text = "Pls enter same caltype stock no";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Pls enter same caltype stock no');", true);
                            TxtStockNo.Text = "";
                            TxtStockNo.Focus();
                        }
                        if (Convert.ToInt32(ds3.Tables[0].Rows[0]["UnitID"]) != Convert.ToInt32(DDUnit.SelectedValue))
                        {
                            LblErrorMessage.Text = "Pls enter same unit stock no";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Pls enter same unit stock no');", true);
                            TxtStockNo.Text = "";
                            TxtStockNo.Focus();
                        }
                    }
                    if (LblErrorMessage.Text == "")
                    {
                        DDcaltype.SelectedValue = ds3.Tables[0].Rows[0]["CalType"].ToString();
                        DDUnit.SelectedValue = ds3.Tables[0].Rows[0]["UnitId"].ToString();
                        int ISSUEID = Convert.ToInt32(ds3.Tables[0].Rows[0]["issueorderid"]);
                        int issuedetailid = Convert.ToInt32(ds3.Tables[0].Rows[0]["issue_detail_id"]);
                        string length = ds3.Tables[0].Rows[0]["length"].ToString();
                        string width = ds3.Tables[0].Rows[0]["width"].ToString();
                        double area = Convert.ToDouble(ds3.Tables[0].Rows[0]["area"]);
                        double rate = Convert.ToDouble(ds3.Tables[0].Rows[0]["rate"]);
                        double amount = Convert.ToDouble(ds3.Tables[0].Rows[0]["amount"]);
                        hn_finished.Value = ds3.Tables[0].Rows[0]["Item_Finished_Id"].ToString();
                        hnorderid.Value = ds3.Tables[0].Rows[0]["OrderId"].ToString();
                        string remarks = ds3.Tables[0].Rows[0]["Remarks"].ToString();
                        int companyid = Convert.ToInt16(ds3.Tables[0].Rows[0]["companyid"]);

                        _arrpara[0] = new SqlParameter("@process_rec_id", SqlDbType.Int);
                        _arrpara[1] = new SqlParameter("@Empid", SqlDbType.Int);
                        _arrpara[2] = new SqlParameter("@receiveDate", SqlDbType.DateTime);
                        _arrpara[3] = new SqlParameter("@unitid", SqlDbType.Int);
                        _arrpara[4] = new SqlParameter("@Userid", SqlDbType.Int);
                        _arrpara[5] = new SqlParameter("@challanno", SqlDbType.VarChar, 250);
                        _arrpara[6] = new SqlParameter("@companyid", SqlDbType.Int);
                        _arrpara[7] = new SqlParameter("@remarks", SqlDbType.VarChar, 500);
                        _arrpara[8] = new SqlParameter("@process_rec_detail_id", SqlDbType.Int);
                        _arrpara[9] = new SqlParameter("@item_finished_id", SqlDbType.Int);
                        _arrpara[10] = new SqlParameter("@length", SqlDbType.VarChar, 250);
                        _arrpara[11] = new SqlParameter("@width", SqlDbType.VarChar, 250);
                        _arrpara[12] = new SqlParameter("@area", SqlDbType.Float);
                        _arrpara[13] = new SqlParameter("@rate", SqlDbType.Float);
                        _arrpara[14] = new SqlParameter("@amount", SqlDbType.Float);
                        _arrpara[15] = new SqlParameter("@Qty", SqlDbType.Float);
                        _arrpara[16] = new SqlParameter("@weight", SqlDbType.Float);
                        _arrpara[17] = new SqlParameter("@comm", SqlDbType.Float);
                        _arrpara[18] = new SqlParameter("@commamt", SqlDbType.Float);
                        _arrpara[19] = new SqlParameter("@issueorderid", SqlDbType.Int);
                        _arrpara[20] = new SqlParameter("@issue_detail_id", SqlDbType.Int);
                        _arrpara[21] = new SqlParameter("@orderid", SqlDbType.Int);
                        _arrpara[22] = new SqlParameter("@penality", SqlDbType.Int);
                        _arrpara[23] = new SqlParameter("@CalType", SqlDbType.Int);
                        _arrpara[24] = new SqlParameter("@ProcessId", SqlDbType.Int);
                        _arrpara[25] = new SqlParameter("@StockValue", SqlDbType.VarChar, 8000);
                        _arrpara[26] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                        _arrpara[27] = new SqlParameter("@Checkedby", SqlDbType.VarChar, 100);
                        _arrpara[28] = new SqlParameter("@QualityType", SqlDbType.Int);
                        _arrpara[29] = new SqlParameter("@ActualWidth", SqlDbType.VarChar, 30);
                        _arrpara[30] = new SqlParameter("@ActualLength", SqlDbType.VarChar, 30);
                        _arrpara[32] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                        _arrpara[33] = new SqlParameter("@StockNoRemarks", SqlDbType.VarChar, 200);
                        _arrpara[34] = new SqlParameter("@ReceiveCottonMoisture", SqlDbType.Float);
                        _arrpara[35] = new SqlParameter("@ReceiveWoolMoisture", SqlDbType.Float);
                        _arrpara[36] = new SqlParameter("@ReceiveDateStamp", SqlDbType.VarChar, 50);

                        if (ViewState["recid"] == null)
                        {
                            ViewState["recid"] = 0;
                        }
                        _arrpara[0].Direction = ParameterDirection.InputOutput;
                        _arrpara[0].Value = ViewState["recid"].ToString();
                        _arrpara[1].Value = 0;// ddemp.SelectedValue;
                        _arrpara[2].Value = TxtreceiveDate.Text;
                        _arrpara[3].Value = DDUnit.SelectedValue;
                        _arrpara[4].Value = Session["varuserid"].ToString();
                        _arrpara[5].Direction = ParameterDirection.InputOutput;
                        _arrpara[5].Value = TxtChallanNo.Text;
                        _arrpara[6].Value = companyid;
                        _arrpara[7].Value = remarks;
                        //_arrpara[8].Value = SqlHelper.ExecuteScalar(Tran, CommandType.Text, "Select Isnull(Max(process_rec_detail_Id),0)+1 from process_receive_detail_" + ddprocess.SelectedValue);
                        _arrpara[8].Direction = ParameterDirection.InputOutput;
                        _arrpara[8].Value = 0;
                        _arrpara[9].Value = hn_finished.Value;
                        _arrpara[10].Value = length;
                        _arrpara[11].Value = width;
                        _arrpara[12].Value = area;
                        _arrpara[13].Value = rate;
                        _arrpara[14].Value = amount;
                        _arrpara[15].Value = ds3.Tables[0].Rows.Count;
                        _arrpara[16].Value = txtweight.Text == "" ? "0" : txtweight.Text;
                        _arrpara[17].Value = 0;
                        _arrpara[18].Value = 0;
                        _arrpara[19].Value = ISSUEID;
                        _arrpara[20].Value = issuedetailid;
                        _arrpara[21].Value = hnorderid.Value;
                        _arrpara[22].Value = 0;
                        _arrpara[23].Value = DDcaltype.SelectedValue;
                        _arrpara[24].Value = Hn_ProcessId.Value;// ddprocess.SelectedValue;
                        _arrpara[25].Value = hnstockno.Value;
                        _arrpara[26].Value = Session["varcompanyid"].ToString();
                        _arrpara[27].Value = TDcheckedby.Visible == true ? txtcheckedby.Text : "";
                        _arrpara[28].Value = ddStockQualityType.SelectedValue;
                        _arrpara[29].Value = txtactualwidth.Text.Trim();
                        _arrpara[30].Value = txtactuallength.Text.Trim();
                        _arrpara[32].Direction = ParameterDirection.Output;
                        _arrpara[33].Value = TRStockNoRemark.Visible == true ? txtstocknoremarks.Text.Trim() : "";
                        _arrpara[34].Value = TDCottonMoisture.Visible == false ? "0" : txtReceiveCottonMoisture.Text == "" ? "0" : txtReceiveCottonMoisture.Text;
                        _arrpara[35].Value = TDWoolMoisture.Visible == false ? "0" : txtReceiveWoolMoisture.Text == "" ? "0" : txtReceiveWoolMoisture.Text;
                        _arrpara[36].Value = TDFinishingDateStamp.Visible == false ? "" : txtFinishingReceiveDateStamp.Text;

                        if (Session["varcompanyId"].ToString() == "14")
                        {
                            _arrpara[31] = new SqlParameter("@TStockNos", StrTStockNos);
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessReceiveForAnisaBulkStockNos", _arrpara);
                        }
                        else
                        {
                            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_NextProcessReceiveForAnisa]", _arrpara);
                        }

                        LblErrorMessage.Text = _arrpara[32].Value.ToString();
                        if (LblErrorMessage.Text != "")
                        {
                            return;
                        }
                        if (LblErrorMessage.Text == "")
                        {
                            ViewState["recid"] = _arrpara[0].Value.ToString();
                            TxtChallanNo.Text = _arrpara[5].Value.ToString();

                            //// TxtChallanNo.Text = ViewState["recid"].ToString();
                            txtactualwidth.Text = "";
                            txtcheckedby.Text = "";
                            txtactuallength.Text = "";
                            txtReceiveCottonMoisture.Text = "";
                            txtReceiveWoolMoisture.Text = "";
                            txtFinishingReceiveDateStamp.Text = "";
                            ddStockQualityType.SelectedValue = "1";
                            if (Session["VarCompanyNo"].ToString() == "22")
                            {
                                txtweight.Text = "";
                            }
                            //UtilityModule.PROCESS_RECEIVE_CONSUMPTION(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(ddprocess.SelectedValue), Convert.ToDouble(_arrpara[12].Value), Convert.ToDouble(_arrpara[16].Value), Convert.ToInt32(_arrpara[3].Value), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[19].Value), Tran, 0, Convert.ToInt32(_arrpara[15].Value), Convert.ToInt32(_arrpara[23].Value));
                            //}
                        }
                    }
                }
                else
                {
                    Str = SqlHelper.ExecuteScalar(Tran, CommandType.Text, @"Select (select * from [dbo].[Get_Folio_EMployee](PIM.IssueOrderID,CurrentProstatus,PID.Issue_Detail_ID)) +'  /  '+replace(convert(varchar(11),AssignDate,106), ' ','-') EmpInFormation 
                    From Process_Stock_detail PSD,CarpetNumber CN,Process_Issue_Master_" + Hn_ProcessId.Value + " PIM,Process_Issue_Detail_" + Hn_ProcessId.Value + @" PID
                    Where PSD.StockNo=CN.StockNo And ToProcessID=" + Hn_ProcessId.Value + @" And PSD.IssueDetailId=PID.Issue_Detail_Id And PIM.IssueOrderId=PID.IssueOrderId  And CN.TStockNo='" + TxtStockNo.Text + "' And PIM.CompanyId=" + ddCompName.SelectedValue + "").ToString();
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Issue To " + Str + "');", true);
                    LblErrorMessage.Text = Str;
                    Tran.Commit();
                    return;
                }
                TxtStockNo.Text = "";

                Tran.Commit();
                LblErrorMessage.Text = "Data saved successfully...";
                FillData_Dgdetail();
                //fill_DetailGride();
                //Fill_Grid_Total();
                //GetStock();
                if (FlagType == 2)
                {
                    TxtStockNo.Focus();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
                Tran.Rollback();
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        
    }
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = 0;
    }
    private void fill_DetailGride()
    {
        if (ddprocess.SelectedIndex > 0)
        {
            string sqlstr = "";
            sqlstr = @"Select process_rec_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS  Description,Width,Length,
                   Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) StockNo
                   From process_receive_master_" + ddprocess.SelectedValue + @" PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,
                   ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM 
                   Where PM.process_rec_id=PD.process_rec_id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
                   PM.process_rec_id=" + ViewState["recid"] + " And PM.CompanyId=" + ddCompName.SelectedValue + " Order By process_rec_Detail_Id desc";
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = DS;
                DGDetail.DataBind();
            }
            else
            {
                DGDetail.DataSource = null;
                DGDetail.DataBind();
            }
        }
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        savedetail("");
        if (Session["VarCompanyNo"].ToString() != "22")
        {

            txtweight.Text = "";
        }
        if (Session["varcompanyId"].ToString() == "8")
        {
            Fill_StockNo();
        }
        else
        {
            Fill_StockNoNew();

        }
    }
    protected void savedetail(string StrTStockNos)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = true;
        string StrEmpid = "";

        if (lstWeaverName.Items.Count == 0)
        {
            LblErrorMessage.Text = "Plz Enter ID No...";
            return;
        }
        else
        {
            //Find EmployeeId
            for (int i = 0; i < lstWeaverName.Items.Count; i++)
            {
                if (StrEmpid == "")
                {
                    StrEmpid = lstWeaverName.Items[i].Value;
                }
                else
                {
                    StrEmpid = StrEmpid + "," + lstWeaverName.Items[i].Value;
                }

            }
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            SqlParameter[] array = new SqlParameter[6];
            array[0] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 100);
            array[1] = new SqlParameter("@StockNo", SqlDbType.Int);
            array[2] = new SqlParameter("@EmpId", SqlDbType.VarChar, 200);
            array[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[4] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);

            array[0].Value = TxtStockNo.Text;
            array[1].Direction = ParameterDirection.Output;
            array[2].Value = StrEmpid;
            array[3].Direction = ParameterDirection.Output;
            array[4].Direction = ParameterDirection.Output;
            array[5].Direction = ParameterDirection.Output;


            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_CheckEmployee_StockNo", array);

            LblErrorMessage.Text = array[3].Value.ToString();
            if (LblErrorMessage.Text == "")
            {
                if (Convert.ToInt16(array[5].Value) == Convert.ToInt16(Session["CurrentWorkingCompanyID"]))
                {
                    hnstockno.Value = array[1].Value.ToString();
                    Hn_ProcessId.Value = array[4].Value.ToString();
                    //ddCompName.SelectedValue = array[5].Value.ToString();
                    hnrate1.Value = "";
                    save_carpet_wise(2, StrEmpid, StrTStockNos);
                    TxtStockNo.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('This stock no not belongs to this company');", true);
                }
            }
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string str = @"Delete TEMP_PROCESS_RECEIVE_MASTER   Delete TEMP_PROCESS_RECEIVE_DETAIL  " + " ";
            str = str + @" Insert into TEMP_PROCESS_RECEIVE_MASTER Select Process_Rec_Id,EmpId,ReceiveDate,UnitId,UserId,ChallanNo,Companyid,Remarks,CalType," + ddprocess.SelectedValue + "  from PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
            str = str + @" Insert into TEMP_PROCESS_RECEIVE_DETAIL Select Process_Rec_Detail_Id, Process_Rec_Id, Item_Finished_Id,Length=case when isnull(actuallength,'')<>'' then Actuallength else Length End,Width=casew when isnull(actualwidth,'')<>'' then actualwidth else width end, Area, Rate, Amount, Qty, Weight, Comm, CommAmt, IssueOrderId, Issue_Detail_Id, OrderId, Penality, PRemarks, QualityType, GatePassNo, FlagFixOrWeight, TDSPercentage, Warp_10cm, Weft_10cm, straightness, Design, OBA, Date_Stamp, StockNoRemarks, WyPly, Cyply from PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);


            Session["ReportPath"] = "Reports/NextProcessReceive.rpt";
            Session["CommanFormula"] = "{V_NextProcessReceive.Process_Rec_Id}=" + ViewState["recid"] + "";
            LblErrorMessage.Visible = false;
            LblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
        //Report();
    }
    private void Report()
    {
        string qry = @"  SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,
        V_NextProcessReceive.QualityName,V_NextProcessReceive.designName,V_NextProcessReceive.ColorName,V_NextProcessReceive.ShapeName,V_NextProcessReceive.Qty,V_NextProcessReceive.Rate,V_NextProcessReceive.Areamtr as area,
        V_NextProcessReceive.Amount,V_NextProcessReceive.ITEM_NAME,V_NextProcessReceive.ReceiveDate,V_NextProcessReceive.UnitId,V_NextProcessReceive.ChallanNo,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessReceive.Length,
        V_NextProcessReceive.Width,V_NextProcessReceive.StockNo
        FROM  PROCESS_NAME_MASTER INNER JOIN V_Companyinfo INNER JOIN V_NextProcessReceive ON V_Companyinfo.CompanyId=V_NextProcessReceive.Companyid ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessReceive.PROCESSID
        INNER JOIN V_EmployeeInfo ON V_NextProcessReceive.Empid=V_EmployeeInfo.EmpId
        Where V_NextProcessReceive.Process_Rec_Id=" + ViewState["recid"] + " And Process_name_Master.MasterCompanyId=" + Session["varCompanyId"] + " And V_CompanyInfo.CompanyId=" + ddCompName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        Session["rptFileName"] = "~\\Reports\\NextProcessReceiveNEW.rpt";
        // Session["rptFileName"] = Session["ReportPath"];
        Session["GetDataset"] = ds;
        Session["dsFileName"] = "~\\ReportSchema\\NextProcessReceiveNEW.xsd";
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddldesig, "select Distinct vf.designId,vf.designName From V_FinishedItemDetail vf where vf.QualityId=" + ddquality.SelectedValue + @" and vf.designid<>0 order by vf.designName", true, "--Plz select--");
        //fillgrid();
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Receive_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
            Label lblprocessId = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblprocessId"));
            Label lblIssueOrderId = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueOrderId"));
            Label lblIssueDetailId = ((Label)DGDetail.Rows[e.RowIndex].FindControl("lblIssueDetailId"));
            Hn_ProcessId.Value = lblprocessId.Text;
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select PSD.*,PD.IssueOrderId from Process_Stock_Detail PSD,CarpetNumber CN,PROCESS_RECEIVE_DETAIL_" + lblprocessId.Text + @" PD 
            Where PSD.StockNo=CN.StockNo And PSD.ReceiveDetailId=PD.Process_Rec_Detail_Id And PSD.IssueDetailId=PD.Issue_Detail_Id And ReceiveDetailId=" + VarProcess_Receive_Detail_Id + " And ToProcessId=" + lblprocessId.Text + " And CurrentProStatus=" + lblprocessId.Text + " And IssRecStatus=0 And CN.CompanyId=" + ddCompName.SelectedValue + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                //                if (DGDetail.Rows.Count == 1)
                //                {
                //                    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, "DELETE process_receive_master_" + lblprocessId.Text + @" 
                //                     Where Process_Rec_Id in (Select Process_Rec_Id from process_receive_detail_" + lblprocessId.Text + " Where Process_Rec_Detail_Id=" + VarProcess_Receive_Detail_Id + ")");
                //                }
                //                string str = @"Update CarpetNumber Set IssRecStatus=1 From Process_Stock_Detail PSD Where CarpetNumber.StockNo=PSD.StockNo And ReceiveDetailId=" + VarProcess_Receive_Detail_Id + " And ToProcessId=" + lblprocessId.Text + " And PSD.IssueDetailId=" + Ds.Tables[0].Rows[0]["IssueDetailId"];
                //                str = str + @" Update Process_Stock_Detail Set ReceiveDate=Null,ReceiveDetailId=0 Where ReceiveDetailId=" + VarProcess_Receive_Detail_Id + " And ToProcessId=" + lblprocessId.Text + " And IssueDetailId=" + Ds.Tables[0].Rows[0]["IssueDetailId"];
                //                str = str + @" Update PROCESS_ISSUE_DETAIL_" + lblprocessId.Text + " Set PQty=PQty+PRD.Qty From PROCESS_RECEIVE_DETAIL_" + lblprocessId.Text + " PRD Where PRD.Issue_Detail_Id=PROCESS_ISSUE_DETAIL_" + lblprocessId.Text + ".Issue_Detail_Id And PRD.Process_Rec_Detail_Id=" + VarProcess_Receive_Detail_Id + "";
                //                str = str + @" Delete process_receive_detail_" + lblprocessId.Text + " Where Process_Rec_Detail_Id=" + VarProcess_Receive_Detail_Id;
                //                str = str + @" Delete PROCESS_RECEIVE_CONSUMPTION Where Process_Rec_Detail_Id=" + VarProcess_Receive_Detail_Id + " And Processid=" + lblprocessId.Text + " And IssueOrderId=" + Ds.Tables[0].Rows[0]["IssueOrderId"];
                //                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);

                //                Tran.Commit();


                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
                param[1] = new SqlParameter("@ReceiveDetailID", SqlDbType.Int);
                param[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
                param[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
                param[4] = new SqlParameter("@RowCount", SqlDbType.Int);


                param[0].Value = Hn_ProcessId.Value;
                param[1].Value = VarProcess_Receive_Detail_Id;
                param[2].Value = lblIssueDetailId.Text;
                param[3].Value = lblIssueOrderId.Text;
                param[4].Value = DGDetail.Rows.Count;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_DeleteNextProductionReceiveDetailForAnisa]", param);

                Tran.Commit();
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Deleted Successfully....";
                // fill_DetailGride();
                FillData_Dgdetail();
                if (Session["varcompanyId"].ToString() == "8")
                {
                    Fill_StockNo();
                }
                else
                {
                    Fill_StockNoNew();
                }
            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "You Have issued....";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
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
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = 0;
        Fill_Temp_OrderNo();
        if (ChkForEdit.Checked == true)
        {
            TDChallanNo.Visible = true;
            TDDDChallanNo.Visible = true;
            TxtEditChallanNo.Text = "";
            TxtEditChallanNo.Focus();
        }
        else
        {
            TDChallanNo.Visible = false;
            TDDDChallanNo.Visible = false;
        }
        if (ddCompName.SelectedIndex > 0 && ddprocess.SelectedIndex > 0)
        {
            fill_DetailGride();
        }
    }
    protected void TxtEditChallanNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtEditChallanNo.Text != "")
        {
            DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
            @"Select * From TEMP_PROCESS_RECEIVE_MASTER_NEW Where CompanyId = " + ddCompName.SelectedValue + " And Process_Rec_Id=" + TxtEditChallanNo.Text + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ddprocess.SelectedValue = Ds.Tables[0].Rows[0]["ProcessId"].ToString();
                ProcessSelectedChanges();
                ddemp.SelectedValue = Ds.Tables[0].Rows[0]["EmpId"].ToString();
                EmpSelectedChanges();
                DDChallanNo.SelectedValue = Ds.Tables[0].Rows[0]["Process_Rec_Id"].ToString();
                ChallanNoSelectedChange();
            }
            else
            {
                if (DDChallanNo.Items.Count > 0)
                {
                    DDChallanNo.SelectedIndex = 0;
                    ChallanNoSelectedChange();
                }
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Pls Enter Correct Challan No";
                TxtEditChallanNo.Text = "";
                TxtEditChallanNo.Focus();
            }
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        ViewState["recid"] = DDChallanNo.SelectedValue;
        string Str = "Select Process_Rec_Id,EmpId,Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,UnitId,Remarks From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + DDChallanNo.SelectedValue + " And CompanyId=" + ddCompName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtreceiveDate.Text = Ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
            TxtChallanNo.Text = Ds.Tables[0].Rows[0]["Process_Rec_Id"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
        }
        fill_DetailGride();
        Fill_Grid_Total();
    }
    private void Fill_Grid_Total()
    {
        int varTotalPcs = 0;
        double varTotalArea = 0;
        double varTotalAmount = 0;
        for (int i = 0; i < DGDetail.Rows.Count; i++)
        {
            varTotalPcs = varTotalPcs + Convert.ToInt32(DGDetail.Rows[i].Cells[6].Text);
            varTotalArea = varTotalArea + Convert.ToDouble(DGDetail.Rows[i].Cells[8].Text);
            varTotalAmount = varTotalAmount + Convert.ToDouble(DGDetail.Rows[i].Cells[9].Text);
        }
        TxtTotalPcs.Text = varTotalPcs.ToString();
        TxtArea.Text = varTotalArea.ToString();
        TxtAmount.Text = varTotalAmount.ToString();
    }
    protected void btnQcPreview_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        int Gridrows = DGDetail.Rows.Count;
        if (Gridrows > 0 && ddprocess.SelectedIndex > 0)
        {
            DataTable dttable = new DataTable();
            dttable.Columns.Add("TstockNo", typeof(string));
            dttable.Columns.Add("Process_Rec_Detail_id", typeof(int));
            dttable.Columns.Add("Process_Rec_id", typeof(int));
            for (int i = 0; i < Gridrows; i++)
            {
                DataRow dr = dttable.NewRow();
                Label lblstockno = (Label)DGDetail.Rows[i].FindControl("lblstockno");
                int Process_rec_detail_id = Convert.ToInt32(DGDetail.DataKeys[i].Value);
                dr["TstockNo"] = lblstockno.Text;
                dr["Process_Rec_Detail_id"] = Process_rec_detail_id;
                dr["Process_rec_id"] = ViewState["recid"];
                dttable.Rows.Add(dr);
            }
            //*********************
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Processid", ddprocess.SelectedValue);
            param[1] = new SqlParameter("@dttable", dttable);
            //*******
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameterJobWise", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Modalpopupextqc.Show();
                DataTable dt = new DataTable();
                dt.Columns.Add("SRNO", typeof(int));
                dt.Columns.Add("STOCKNO", typeof(string));
                dt.Columns.Add("Processrecid", typeof(int));
                dt.Columns.Add("Processrecdetailid", typeof(int));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt.Columns.Add(dr["Paraname"].ToString(), typeof(bool));

                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["SrNo"] = i + 1;
                    dr["StockNo"] = ds.Tables[1].Rows[i]["TstockNo"].ToString();
                    dr["Processrecid"] = ds.Tables[1].Rows[i]["Process_Rec_id"].ToString();
                    dr["Processrecdetailid"] = ds.Tables[1].Rows[i]["Process_Rec_Detail_Id"].ToString();
                    //**********                   
                    dt.Rows.Add(dr);
                }
                dt.Columns["Processrecid"].ColumnMapping = MappingType.Hidden;
                GDQC.DataSource = dt;
                GDQC.DataBind();
                //check checkboxes
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < GDQC.Rows.Count; i++)
                    {
                        int Processrecdetailid = Convert.ToInt32(GDQC.Rows[i].Cells[3].Text);
                        GridViewRow grow = GDQC.Rows[i];
                        for (int k = 4; k < grow.Cells.Count; k++)
                        {
                            string celltext = GDQC.HeaderRow.Cells[k].Text;
                            for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            {
                                int subprocessrecdetailid = Convert.ToInt32(ds.Tables[2].Rows[j]["RecieveDetailID"]);
                                string paramname = ds.Tables[2].Rows[j]["ParaName"].ToString();
                                if ((Processrecdetailid == subprocessrecdetailid) && (celltext == paramname))
                                {
                                    CheckBox ch = grow.Cells[k].Controls[0] as CheckBox;
                                    ch.Checked = Convert.ToBoolean(ds.Tables[2].Rows[j]["QCVALUE"]);
                                }
                            }
                        }
                    }
                }
                //
            }
            else
            {

            }
            //*************
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "qc1", "alert('Please Insert Data first to Save QC Detail')", true);
        }
    }
    private void Report1()
    {
        string SName = "";
        string QCValue = "";
        string qry = @" SELECT V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,
        V_NextProcessReceive.QualityName,V_NextProcessReceive.designName,V_NextProcessReceive.ColorName,V_NextProcessReceive.ShapeName,V_NextProcessReceive.Qty,V_NextProcessReceive.Rate,V_NextProcessReceive.Area,
        V_NextProcessReceive.Amount,V_NextProcessReceive.ITEM_NAME,V_NextProcessReceive.ReceiveDate,V_NextProcessReceive.UnitId,V_NextProcessReceive.ChallanNo,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessReceive.Length,
        V_NextProcessReceive.Width,V_NextProcessReceive.StockNo,'' SName,'' QcValue
        FROM  PROCESS_NAME_MASTER INNER JOIN V_Companyinfo INNER JOIN V_NextProcessReceive ON V_Companyinfo.CompanyId=V_NextProcessReceive.Companyid ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessReceive.PROCESSID
        INNER JOIN V_EmployeeInfo ON V_NextProcessReceive.Empid=V_EmployeeInfo.EmpId
        where V_NextProcessReceive.Process_Rec_Id=" + ViewState["recid"] + " And Process_Name_Master.MasterCompanyId=" + Session["varCompanyId"] + " And V_Companyinfo.CompanyId=" + ddCompName.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
where RefName= 'process_receive_detail_" + ddprocess.SelectedValue + "' and QCD.RecieveDetailID=" + Convert.ToInt32(ViewState["VarProcess_Receive_Detail_Id"]) + "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        SqlDataAdapter sda = new SqlDataAdapter(str, con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (SName == "" && QCValue == "")
            {
                SName = dt.Rows[i]["SName"].ToString();
                QCValue = dt.Rows[i]["QCValue"].ToString();
            }
            else
            {
                SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
                QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
            }
        }
        DataTable mytable = new DataTable();
        mytable.Columns.Add("SName", typeof(string));
        mytable.Columns.Add("QCValue", typeof(string));
        mytable.Rows.Add(SName, QCValue);
        ds.Tables.Add(mytable);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\NextProcessReceiveQCNEW.rpt";
            // Session["rptFileName"] = Session["ReportPath"];
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\NextProcessReceiveQCNEW.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void mygdv_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void mygdstock_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void grdqualitychk_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varCompanyId"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }

    protected void FillData_Dgdetail()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlParameter[] _array = new SqlParameter[9];
            _array[0] = new SqlParameter("@Process_Rec_id", SqlDbType.Int);
            _array[1] = new SqlParameter("@EmpId", SqlDbType.Int);
            _array[2] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
            _array[3] = new SqlParameter("@OrderId", SqlDbType.Int);
            _array[4] = new SqlParameter("@ItemFinishedid", SqlDbType.Int);
            _array[5] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[6] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[7] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);

            _array[0].Value = ViewState["recid"];
            _array[1].Value = 0;// ddemp.SelectedValue;
            _array[2].Value = 0;// ddorderno.SelectedValue;
            _array[3].Value = 0;// hnorderid.Value;
            _array[4].Value = 0;// hn_finished.Value;
            _array[5].Value = Hn_ProcessId.Value;// ddprocess.SelectedValue;
            _array[6].Value = ddCompName.SelectedValue;
            _array[7].Value = Session["varcompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_FillGridDetail_NextReceiveForAnisa", _array);
            //Table 0 For DGDetail And table 1 For dgstock
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = ds.Tables[0];
                DGDetail.DataBind();

                TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
                TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();
            }
            else
            {
                DGDetail.DataSource = null;
                DGDetail.DataBind();
            }
            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    mygdstock.DataSource = ds.Tables[1];
            //    mygdstock.DataBind();
            //}
            //else
            //{
            //    mygdstock.DataSource = null;
            //    mygdstock.DataBind();
            //}

        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void btnShowdata_Click(object sender, EventArgs e)
    {
        FillData_Dgdetail();
    }
    protected void txtWeaverIdNo_TextChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = 0;
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNo.Text != "")
            {
                //DataRow[] rows = AllEnums.MasterTables.Empinfo.ToTable().Select("EmpCode='" + txtWeaverIdNo.Text + "'");
                //foreach (DataRow dr in rows)
                //{

                //    lstWeaverName.Items.Add(new ListItem("" + dr["Empname"] + "", dr["Empid"].ToString()));
                //    txtWeaverIdNo.Text = "";

                //}
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where Empcode='" + txtWeaverIdNo.Text + "'  and BlackList=0");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                    {

                        lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    }

                    txtWeaverIdNo.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                }

                ds.Dispose();

            }
            txtWeaverIdNo.Focus();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void btnDeleteName_Click(object sender, EventArgs e)
    {
        lstWeaverName.Items.Remove(lstWeaverName.SelectedItem);
    }
    protected string EmployeeId()
    {
        string StrEmpId = string.Empty;
        for (int i = 0; i < lstWeaverName.Items.Count; i++)
        {
            if (StrEmpId == "")
            {
                StrEmpId = lstWeaverName.Items[i].Value.ToString();
            }
            else
            {
                StrEmpId = StrEmpId + "," + lstWeaverName.Items[i].Value.ToString();
            }
        }

        return StrEmpId;
    }
    protected void Fill_StockNo()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        string Strempid = string.Empty;
        //Get Employee id
        Strempid = EmployeeId();
        //Check if employeeid is blank
        if (Strempid != "")
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@EmployeeId", SqlDbType.VarChar, 50);
            param[1] = new SqlParameter("@CompanyID", SqlDbType.Int);

            param[0].Value = Strempid;
            param[1].Value = Session["CurrentWorkingCompanyID"];

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_GetPendingJobStockNo", param);

            DGStockDetail.DataSource = ds;
            DGStockDetail.DataBind();

        }

    }
    protected void Fill_StockNoNew()
    {
        ////SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        ////if (con.State == ConnectionState.Closed)
        ////{
        ////    con.Open();
        ////}

        //if (Session["varcompanyId"].ToString() == "14")
        //{
        //    DGStockDetail.PageSize = 200;
        //}

        string Strempid = string.Empty;
        ////Get Employee id
        Strempid = EmployeeId();
        ////Check if employeeid is blank
        if (Strempid != "")
        {
            ////SqlParameter[] param = new SqlParameter[7];

            ////param[0] = new SqlParameter("@Unitsid", ddUnits.SelectedIndex > 0 ? ddUnits.SelectedValue : "0");
            ////param[1] = new SqlParameter("@Jobid", ddprocess.SelectedIndex > 0 ? ddprocess.SelectedValue : "0");
            ////param[2] = new SqlParameter("@QualityId", ddquality.SelectedIndex > 0 ? ddquality.SelectedValue : "0");
            ////param[3] = new SqlParameter("@DesignId", ddldesig.SelectedIndex > 0 ? ddldesig.SelectedValue : "0");
            ////param[4] = new SqlParameter("@colorid", ddcolour.SelectedIndex > 0 ? ddcolour.SelectedValue : "0");
            ////param[5] = new SqlParameter("@sizeid", ddsize.SelectedIndex > 0 ? ddsize.SelectedValue : "0");
            ////param[6] = new SqlParameter("@EmployeeId", Strempid);

            ////DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_GetPendingJobStockNoNew", param);

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("Pro_GetPendingJobStockNoNew", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.AddWithValue("@Unitsid", ddUnits.SelectedIndex > 0 ? ddUnits.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Jobid", ddprocess.SelectedIndex > 0 ? ddprocess.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@QualityId", ddquality.SelectedIndex > 0 ? ddquality.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@DesignId", ddldesig.SelectedIndex > 0 ? ddldesig.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@colorid", ddcolour.SelectedIndex > 0 ? ddcolour.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@sizeid", ddsize.SelectedIndex > 0 ? ddsize.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@EmployeeId", Strempid);
            cmd.Parameters.AddWithValue("@CompanyId", Session["CurrentWorkingCompanyID"]);

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);

            DGStockDetail.DataSource = ds;
            DGStockDetail.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalpcsgrid.Text = Convert.ToString(ds.Tables[0].Compute("Count(TstockNo)", ""));
            }
            else
            {
                txttotalpcsgrid.Text = "";
            }
        }
    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "8")
        {
            Fill_StockNo();
        }
        else
        {
            Fill_StockNoNew();

        }

        if (DGStockDetail.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alert", "alert('No records Found..');", true);
        }
    }

    protected void ddprocess_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ViewState["recid"] = "0";
        DGDetail.DataSource = null;
        DGDetail.DataBind();

        switch (ddprocess.SelectedItem.Text.ToUpper())
        {
            case "BINDING":
                switch (Session["VarcompanyNo"].ToString())
                {
                    case "8":
                        break;
                    default:
                        TRWeight.Visible = true;
                        txtweight.Text = "";
                        break;
                }
                break;
            default:
                switch (Session["VarcompanyNo"].ToString())
                {
                    case "22":
                        TRWeight.Visible = true;
                        txtweight.Text = "";
                        break;
                    default:
                        TRWeight.Visible = false;
                        txtweight.Text = "";
                        break;
                }
                break;
              
        }
    }
    protected void ddUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = "0";
        DGDetail.DataSource = null;
        DGDetail.DataBind();
    }
    protected void GDQC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            // bind checkbox control with gridview :
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                CheckBox chk = e.Row.Cells[i].Controls[0] as CheckBox;
                chk.Enabled = true;
                chk.Checked = true;
                //checked box

                //
            }

        }
    }
    protected void btnqcsavenew_Click(object sender, EventArgs e)
    {
        lblqcmsg.Text = "";
        try
        {
            DataTable dtrecord = new DataTable();
            dtrecord.Columns.Add("Processrecid", typeof(int));
            dtrecord.Columns.Add("Processrecdetailid", typeof(int));
            dtrecord.Columns.Add("Parameter", typeof(string));
            dtrecord.Columns.Add("paramvalue", typeof(int));
            //**********
            for (int i = 0; i < GDQC.Rows.Count; i++)
            {
                GridViewRow gvr = GDQC.Rows[i];
                for (int j = 4; j < gvr.Cells.Count; j++)
                {
                    DataRow dr = dtrecord.NewRow();
                    dr["Processrecid"] = GDQC.Rows[i].Cells[2].Text; //Processrecid
                    dr["Processrecdetailid"] = GDQC.Rows[i].Cells[3].Text;//Processrecdetailid                    
                    dr["Parameter"] = GDQC.HeaderRow.Cells[j].Text;
                    CheckBox chk = gvr.Cells[j].Controls[0] as CheckBox;
                    dr["paramvalue"] = chk.Checked == true ? 1 : 0;
                    dtrecord.Rows.Add(dr);
                }
            }
            //*********
            if (dtrecord.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@processid", ddprocess.SelectedValue);
                //*****
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_saveQcJobwise", param);
                lblqcmsg.Text = param[1].Value.ToString();
                Modalpopupextqc.Show();
            }
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
    private void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
            (CurrentControl as WebControl).Attributes.Add(
                "onfocus",
                "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");

        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }
    protected void DGStockDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGStockDetail.PageIndex = e.NewPageIndex;
        if (Session["varcompanyid"].ToString() == "8")
        {
            Fill_StockNo();
        }
        else
        {
            Fill_StockNoNew();
        }

    }
    protected void btnsavegrid_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "14")
        {
            string StrTStockNos = "";
            //Grid Loop
            for (int i = 0; i < DGStockDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                if (Chkboxitem.Checked == true)
                {
                    StrTStockNos = StrTStockNos + ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text + "~";

                    if (TxtStockNo.Text == "")
                    {
                        TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                    }
                }
            }
            savedetail(StrTStockNos);
        }
        else
        {
            for (int i = 0; i < DGStockDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                if (Chkboxitem.Checked == true)
                {
                    TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                    savedetail("");
                }
            }
        }
        txtweight.Text = "";
        if (Session["varcompanyid"].ToString() == "8")
        {
            Fill_StockNo();
        }
        else
        {
            Fill_StockNoNew();
        }

        //
    }

    protected void DGStockDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox ChkAllItem = (CheckBox)e.Row.FindControl("ChkAllItem");
            if (Session["varcompanyId"].ToString() == "8")
            {
                ChkAllItem.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox Chkboxitem = (CheckBox)e.Row.FindControl("Chkboxitem");
            if (Session["varcompanyId"].ToString() == "8")
            {
                Chkboxitem.Visible = false;
            }
        }
    }
    protected void TxtreceiveDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = "0";
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        TxtChallanNo.Text = "";
    }
    protected void TxtBulkReceiveQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        
        if (Session["varcompanyId"].ToString() == "14")
        {
            if (Convert.ToInt32(TxtBulkReceiveQty.Text) > 200)
            {
                TxtBulkReceiveQty.Text = "200";
            }
        }


        if (TxtBulkReceiveQty.Text != "")
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtBulkReceiveQty.Text);
            Fill_StockNoNew();
        }
    }
    protected void txtReceiveCottonMoisture_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "22" && ddprocess.SelectedItem.Text == "TABLE CHECKING")
        {
            if (Convert.ToInt32(txtReceiveCottonMoisture.Text.Trim()) < 0 || Convert.ToInt32(txtReceiveCottonMoisture.Text.Trim()) > 10)
            {
                txtReceiveCottonMoisture.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Enter Value Between 0 To 10...');", true);
                return;
            }           
        }
    }
    protected void txtReceiveWoolMoisture_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "22" && ddprocess.SelectedItem.Text == "TABLE CHECKING")
        {
            if (Convert.ToInt32(txtReceiveWoolMoisture.Text.Trim()) < 0 || Convert.ToInt32(txtReceiveWoolMoisture.Text.Trim()) > 10)
            {
                txtReceiveWoolMoisture.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Enter Value Between 0 To 10...');", true);
                return;
            }
        }
    }
    protected void txtFinishingReceiveDateStamp_TextChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyId"].ToString() == "22" && ddprocess.SelectedItem.Text == "TABLE CHECKING")
        {
            int DateStampLength = 0;
            DateStampLength = txtFinishingReceiveDateStamp.Text.Length;

            if (DateStampLength <4 || DateStampLength > 4)
            {
                txtFinishingReceiveDateStamp.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Plz Enter Numeric 4 Digit Value...');", true);
                return;
            }
        }
    }
}