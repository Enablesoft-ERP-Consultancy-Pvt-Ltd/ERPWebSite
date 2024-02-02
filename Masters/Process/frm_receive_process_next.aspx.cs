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

public partial class Masters_Process_frm_receive_process_next : System.Web.UI.Page
{
    protected static string Focus = "";
    static int tempcaltype = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Focus != "")
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            sm.SetFocus(Focus);
        }

        if (!IsPostBack)
        {
            logo();
            string str = "select Distinct CI.CompanyId,CI.Companyname From CompanyInfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Companyname";
            if (variable.VarNextProcessUserAuthentication == "1")
            {
                str = str + " select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From UserRightsProcess UR inner Join PROCESS_NAME_MASTER PNM on UR.ProcessId=PNM.PROCESS_NAME_ID Where UR.Userid=" + Session["varuserid"] + " and PNM.Mastercompanyid=" + Session["varMasterCompanyIDForERP"] + " order by PNM.PROCESS_NAME";
            }
            else
            {
                str = str + " select process_name_id,process_name from process_name_master Where  MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by process_name ";
            }
            str = str + " select unitid,unitname from unit where unitid in (1,2,6) ";
            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "--SELECT--");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }

            ddprocess.Focus();
            ViewState["recid"] = 0;
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 2, true, "--SELECT--");
            lablechange();
            TxtreceiveDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            int VarCompanyNo = Convert.ToInt32(Session["varMasterCompanyIDForERP"].ToString());

            Fill_Temp_OrderNo();
            qulitychk.Visible = false;
            hnorderid.Value = "0";
            //************************************
            switch (VarCompanyNo)
            {
                case 5:
                    DDUnit.SelectedValue = "1";
                    break;
                case 6:
                    btnQcPreview.Visible = false;
                    break;
                case 9:
                    divstock.Visible = false;
                    TDsrno.Visible = true;
                    break;
                case 4:
                    DDUnit.SelectedValue = "2";
                    break;
                default:
                    DDUnit.SelectedValue = "1";
                    break;
            }
        }

    }
    public void lablechange()
    {
        String[] ParameterList = new String[8];
        ParameterList = UtilityModule.ParameteLabel(Convert.ToInt32(Session["varMasterCompanyIDForERP"]));
        lblqualityname.Text = ParameterList[0];
        lbldesignname.Text = ParameterList[1];
        lblcolorname.Text = ParameterList[2];
        lblshapename.Text = ParameterList[3];
        lblsizename.Text = ParameterList[4];
        lblcategoryname.Text = ParameterList[5];
        lblitemname.Text = ParameterList[6];

    }
    private void Fill_Temp_OrderNo()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_RECEIVE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER Where Process_Name_Id<>1 And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By Process_Name_Id");
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
        UtilityModule.ConditionalComboFill(ref ddemp, "Select Distinct EI.Empid,EI.EmpName From Empinfo EI,process_issue_master_" + ddprocess.SelectedValue + " PM Where EI.EmpId=PM.EmpId And EI.Blacklist=0 and EI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " order by EI.EmpName", true, "--SELECT--");

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT DISTINCT OCALTYPE FROM PROCESSCONSUMPTIONMASTER PM,PROCESSCONSUMPTIONDETAIL PD WHERE PM.PCMID=PD.PCMID And PROCESSID=" + ddprocess.SelectedValue + " And PM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["OCALTYPE"].ToString();
        }
        else
        {
            switch (Convert.ToInt16(Session["varMasterCompanyIDForERP"]))
            {
                case 9:
                    break;
                case 10:  //RAAS INDIA
                    break;
                default:
                    LblErrorMessage.Visible = true;
                    LblErrorMessage.Text = "PLS DEFINE RATE OR CONSUMPTION OF " + ddprocess.SelectedItem.Text + " PROCESS";
                    ddprocess.SelectedIndex = 0;
                    break;
            }
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
            string str = @" select distinct issueorderid,ChallanNo from process_issue_master_" + ddprocess.SelectedValue + " where empid=" + ddemp.SelectedValue + " And CompanyId= " + ddCompName.SelectedValue + "";
            str = str + @" Select distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_ID";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddorderno, ds, 0, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcattype, ds, 1, true, "--Select Category--");
            if (TDsrno.Visible == true)
            {
                UtilityModule.ConditionalComboFill(ref DDsrno, @"select Distinct OM.OrderId,OM.LocalOrder From PROCESS_ISSUE_MASTER_" + ddprocess.SelectedValue + @" PIM 
                                                            inner Join PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId 
                                                            inner join OrderMaster Om on PID.Orderid=OM.OrderId  Where PIM.Companyid=" + ddCompName.SelectedValue + " and PIM.EMpid=" + ddemp.SelectedValue + " and PID.Pqty>0 order by OM.OrderId", true, "--Select Srno--");
            }
        }
    }
    protected void ddorderno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddorderno.SelectedIndex > 0)
        {
            UtilityModule.ConditionalComboFill(ref ddcattype, "select distinct CATEGORY_ID,CATEGORY_NAME from ITEM_CATEGORY_MASTER IM,CategorySeparate CS Where IM.Category_Id=CS.CategoryId And CS.Id=0 And IM.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by CATEGORY_ID", true, "--Select Category--");
        }
        fillgrid();
    }
    protected void ddcattype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcategorycange();
        UtilityModule.ConditionalComboFill(ref dditem, "select Item_id, Item_Name from Item_Master where Category_Id=" + ddcattype.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "---Select Item----");
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
                        UtilityModule.ConditionalComboFill(ref ddldesig, "select DesignId,Designname from Design Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Designname", true, "--Select Design--");
                        break;
                    case "3":
                        tdcolor.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddcolour, "select  ColorId,ColorName from Color Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order by Colorname", true, "--Select Color--");
                        break;
                    case "4":
                        tdshape.Visible = true;
                        UtilityModule.ConditionalComboFill(ref ddshape, "select Shapeid,Shapename from Shape Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Shape--");
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
            UtilityModule.ConditionalComboFill(ref ddquality, "Select QualityId,QualityName from Quality Where Item_Id=" + dditem.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Quality--");
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
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,Export_Format as SizeFt from QualitySizeNew where ShapeId=" + ddshape.SelectedValue + "", true, "--Select Size--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,SizeFt from Size where ShapeId=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Size--");
            }
        }
        else if (DDUnit.SelectedValue == "1")
        {
            if (variable.VarNewQualitySize == "1")
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,MtrSize as SizeMtr from QualitySizeNew where ShapeId=" + ddshape.SelectedValue + "", true, "--Select Size--");
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref ddsize, "select Sizeid,SizeMtr from Size where ShapeId=" + ddshape.SelectedValue + " And MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "", true, "--Select Size--");
            }
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
        // LblErrorMessage.Text = "";
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            if (ddCompName.SelectedIndex > 0 && ddprocess.SelectedIndex > 0)
            {
                string strsql = "";
                string view = "";
                //*************************
                if (variable.VarNewQualitySize == "1")
                {
                    view = "v_finisheditemdetailNew";
                }
                else
                {
                    view = "v_finisheditemdetail";
                }
                //*******************

                if (Session["varCompanyNo"].ToString() == "9")
                {
                    strsql = @"Select vd.CATEGORY_NAME,vd.Item_Name,vd.qualityname+'  '+ vd.designname+'  '+vd.colorname+'  '+ShapeName+'  '+ Case When pim.UnitId=1 Then 
                           vd.sizemtr Else case when pim.UnitId=6 then vd.SizeInch else vd.sizeft End End Description,CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,
                            pid.Area as Area,OM.localorder,PIM.unitID,PIM.CALTYPE
                           From " + view + @" vd,CarpetNumber CN,Process_Stock_Detail psd,process_issue_detail_" + ddprocess.SelectedValue + @" pid,process_issue_master_" + ddprocess.SelectedValue + @" pim,ordermaster om
                           Where cn.item_finished_id=vd.item_finished_id and psd.stockno=cn.stockno and psd.toprocessid=" + ddprocess.SelectedValue + @" and 
                           pid.issue_detail_id=psd.issuedetailid and pim.issueorderid=pid.issueorderid and pid.orderid=om.orderid and cn.currentprostatus=" + ddprocess.SelectedValue + @" and 
                           issrecstatus=1 and pim.CompanyId=" + ddCompName.SelectedValue + " And Vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                else
                {
                    strsql = @"Select vd.CATEGORY_NAME,vd.Item_Name,vd.qualityname+'  '+ vd.designname+'  '+vd.colorname+'  '+ShapeName+'  '+ Case When pim.UnitId=1 Then 
                           vd.sizemtr Else case when pim.UnitId=6 then vd.SizeInch else vd.sizeft End End Description,CN.Item_Finished_Id,Count(CN.Item_Finished_Id) as Qty,CN.OrderId,
                            Case When pim.UnitId=1 Then vd.areamtr Else vd.areaft End Area,OM.localorder,PIM.unitID,PIM.CALTYPE
                           From " + view + @" vd,CarpetNumber CN,Process_Stock_Detail psd,process_issue_detail_" + ddprocess.SelectedValue + @" pid,process_issue_master_" + ddprocess.SelectedValue + @" pim,ordermaster om
                           Where cn.item_finished_id=vd.item_finished_id and psd.stockno=cn.stockno and psd.toprocessid=" + ddprocess.SelectedValue + @" and 
                           pid.issue_detail_id=psd.issuedetailid and pim.issueorderid=pid.issueorderid and pid.orderid=om.orderid and cn.currentprostatus=" + ddprocess.SelectedValue + @" and 
                           issrecstatus=1 and pim.CompanyId=" + ddCompName.SelectedValue + " And Vd.MasterCompanyId=" + Session["varMasterCompanyIDForERP"];
                }
                
                    
               
               

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
                if (DDsrno.SelectedIndex > 0)
                {
                    strsql = strsql + " And OM.Localorder='" + DDsrno.SelectedItem.Text + "'";
                }

                if (Session["varCompanyNo"].ToString() == "9")
                {
                    strsql = strsql + " group by cn.item_finished_id,vd.CATEGORY_NAME,vd.Item_Name ,vd.qualityname,vd.designname,vd.colorname,ShapeName,cn.orderid,sizemtr,sizeft,pim.UnitId,Om.localorder,vd.SizeInch,PIM.CALTYPE,pid.Area";
                }
                else
                {
                    strsql = strsql + " group by cn.item_finished_id,vd.CATEGORY_NAME,vd.Item_Name ,vd.qualityname,vd.designname,vd.colorname,ShapeName,cn.orderid,sizemtr,sizeft,areaft,areamtr,pim.UnitId,Om.localorder,vd.SizeInch,PIM.CALTYPE";
                }

                
                ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                    //DDcaltype.SelectedValue = ds.Tables[0].Rows[0]["CALTYPE"].ToString();

                    
                    if (Convert.ToInt32(ViewState["recid"]) == 0)
                    {
                        tempcaltype = Convert.ToInt32(ds.Tables[0].Rows[0]["CALTYPE"]);
                    }

                    if (Convert.ToInt32(ViewState["recid"]) != 0)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["CalType"]) != tempcaltype)
                        {
                            LblErrorMessage.Text = "Pls select same caltype P OrderNo";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Pls select same caltype P OrderNo');", true);
                        }
                    }
                    else
                    {
                        DDUnit.SelectedValue = ds.Tables[0].Rows[0]["UnitId"].ToString();
                        DDcaltype.SelectedValue = ds.Tables[0].Rows[0]["CALTYPE"].ToString();
                    }                   


                }
                mygdv.DataSource = ds;
                mygdv.DataBind();
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
        fillgrid();
    }
    protected void ddcolour_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void mygdv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label t_code = (Label)mygdv.Rows[e.RowIndex].FindControl("lblfinishedid");
        Label orderid = (Label)mygdv.Rows[e.RowIndex].FindControl("lblorderid");
        hn_finished.Value = t_code.Text;
        hnorderid.Value = orderid.Text;
        GetStock();
    }
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

            mygdstock.DataSource = ds1;
            mygdstock.DataBind();

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
    protected void mygdv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        hn_finished.Value = ((Label)mygdv.Rows[e.RowIndex].FindControl("lblfinishedid")).Text;
        hnorderid.Value = ((Label)mygdv.Rows[e.RowIndex].FindControl("lblorderid")).Text;
        Hn_Qty.Value = ((TextBox)mygdv.Rows[e.RowIndex].FindControl("lblqty")).Text;
        Hn_TQty.Value = ((TextBox)mygdv.Rows[e.RowIndex].FindControl("txtTQty")).Text;

        string str = @"select cn.StockNo,cn.TStockNo  From PROCESS_ISSUE_MASTER_" + ddprocess.SelectedValue + " PIM inner Join PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PID on PIM.IssueOrderId=PID.IssueOrderId
                    inner join Process_Stock_Detail psd on PID.Issue_Detail_Id=psd.IssueDetailId and psd.ToProcessId=" + ddprocess.SelectedValue + @" and psd.receivedetailid=0
                    inner join CarpetNumber cn on psd.StockNo=cn.StockNo and cn.IssRecStatus=1 and Cn.CurrentProStatus=" + ddprocess.SelectedValue + " Where PIM.companyid=" + ddCompName.SelectedValue + " and PID.item_finished_id=" + hn_finished.Value + " and pid.orderid=" + hnorderid.Value + " and PIM.empid=" + ddemp.SelectedValue;
        if (ddorderno.SelectedIndex > 0)
        {
            str = str + " and pid.IssueOrderId=" + ddorderno.SelectedValue;
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            int Qty = Convert.ToInt32(Hn_Qty.Value == "" ? "0" : Hn_Qty.Value);
            int stockqty = Convert.ToInt32(ds.Tables[0].Compute("count(tstockno)", ""));
            if (Qty > stockqty)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "op1", "alert('Receive quantity can not greater than Issue Qty.!');", true);
                fillgrid();
                return;
            }
            for (int i = 0; i < Qty; i++)
            {
                TxtStockNo.Text = Convert.ToString(ds.Tables[0].Rows[i]["Tstockno"]);
                save_detail();
            }
            fillgrid();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "op", "alert('No stock avilable for this Combination!');", true);
            fillgrid();
        }

    }
    protected void mygdstock_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label stockno = (Label)mygdstock.Rows[e.RowIndex].FindControl("lblStockNo");
        TextBox rate1 = (TextBox)mygdstock.Rows[e.RowIndex].FindControl("txtRate");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CompanyId,IssRecStatus,CurrentProStatus,Pack,stockno from CarpetNumber Where StockNo=" + stockno.Text + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "";
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CompanyId"]) != Convert.ToInt32(ddCompName.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Company...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["issRecStatus"]) != 1)
            {
                LblErrorMessage.Text = "This Stock No Already Received...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CurrentProStatus"]) != Convert.ToInt32(ddprocess.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Process...";
                TxtStockNo.Focus();
                return;
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Pack"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Packed....";
                TxtStockNo.Focus();
                return;
            }
        }
        else
        {
            LblErrorMessage.Text = "Pls Check Stock No....";
            TxtStockNo.Focus();
            return;
        }
        hnstockno.Value = stockno.Text;
        hnrate1.Value = rate1.Text;
        save_carpet_wise(1);
    }
    private void save_carpet_wise(int FlagType)
    {
        string Str = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[26];
            DataSet ds3 = new DataSet();
            if (FlagType == 1)
            {
                Str = "Select * From CarpetNumber CN,Process_Stock_Detail PSD,Process_Issue_Master_" + ddprocess.SelectedValue + " PM,Process_Issue_Detail_" + ddprocess.SelectedValue + @" PD 
                      Where CN.StockNo=PSD.StockNo And PSD.IssueDetailId=PD.Issue_Detail_Id And PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0 And CN.StockNo=" + hnstockno.Value + @" And 
                      PSD.ToProcessId=" + ddprocess.SelectedValue + " And PM.Empid=" + ddemp.SelectedValue + " And PM.CompanyId=" + ddCompName.SelectedValue;
            }
            else if (FlagType == 2)
            {
                Str = "Select * From CarpetNumber CN,Process_Stock_Detail PSD,Process_Issue_Master_" + ddprocess.SelectedValue + " PM,Process_Issue_Detail_" + ddprocess.SelectedValue + @" PD 
                       Where CN.StockNo=PSD.StockNo And PSD.IssueDetailId=PD.Issue_Detail_Id And PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0 And CN.TStockNo='" + TxtStockNo.Text + @"' And 
                       PSD.ToProcessId=" + ddprocess.SelectedValue + " And PM.Empid=" + ddemp.SelectedValue + " And PM.CompanyId=" + ddCompName.SelectedValue;
            }
            else if (FlagType == 0)
            {
                //hnstockno.Value = "";
                if (mygdstock.Rows.Count > 0)
                    for (int i = 0; i < mygdstock.Rows.Count; i++)
                    {
                        if (hnstockno.Value == "")
                            hnstockno.Value = ((Label)mygdstock.Rows[i].FindControl("lblstockno")).Text;
                        else
                            hnstockno.Value = hnstockno.Value + ',' + ((Label)mygdstock.Rows[i].FindControl("lblstockno")).Text;
                    }
                else
                {
                    hnstockno.Value = "";
                    string strsql = @"Select Distinct Top(" + Hn_Qty.Value + @") CN.StockNo
                              From CarpetNumber CN,Process_Stock_Detail PSD Where CN.StockNo=PSD.StockNo And 
                              CN.CompanyId=" + ddCompName.SelectedValue + " And CN.Item_Finished_Id=" + hn_finished.Value + @" and CN.IssRecStatus=1 And 
                              CN.OrderId=" + hnorderid.Value + " And CurrentProStatus=" + ddprocess.SelectedValue + @" And IssueDetailId in (Select Issue_Detail_Id From 
                              process_issue_master_" + ddprocess.SelectedValue + " PM,process_issue_detail_" + ddprocess.SelectedValue + @" PD 
                              Where PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0";
                    if (ddemp.SelectedIndex > 0)
                    {
                        strsql = strsql + " And PM.EmpID=" + ddemp.SelectedValue;
                    }
                    if (ddorderno.SelectedIndex > 0)
                    {
                        strsql = strsql + " And PM.IssueOrderId=" + ddorderno.SelectedValue;
                    }
                    strsql = strsql + ")";
                    DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            if (hnstockno.Value == "")
                            {
                                hnstockno.Value = ds1.Tables[0].Rows[i][0].ToString();
                            }
                            else
                            {
                                hnstockno.Value = hnstockno.Value + ',' + ds1.Tables[0].Rows[i][0].ToString();
                            }
                        }
                    }
                }
                Str = "Select Top (" + Hn_Qty.Value + ") Caltype,Unitid,Pm.issueOrderId,Issue_Detail_Id,length,width,area,rate,Case When Caltype=0 Then (Qty-isnull(CancelQty,0))*rate*area Else (Qty-isnull(CancelQty,0))*rate End amount,PD.Item_Finished_Id,PD.OrderId From CarpetNumber CN,Process_Stock_Detail PSD,Process_Issue_Master_" + ddprocess.SelectedValue + " PM,Process_Issue_Detail_" + ddprocess.SelectedValue + @" PD 
                       Where CN.StockNo=PSD.StockNo And PSD.IssueDetailId=PD.Issue_Detail_Id And PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0 And CN.StockNo in(" + hnstockno.Value + ") And PSD.ToProcessId=" + ddprocess.SelectedValue + " And PM.CompanyId=" + ddCompName.SelectedValue;
            }
            ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
            if (ds3.Tables[0].Rows.Count > 0)
            {
                if (Hn_Qty.Value != "")
                {
                    if ((Convert.ToInt32(Hn_Qty.Value) > Convert.ToInt32(Hn_TQty.Value) || Convert.ToInt32(Hn_Qty.Value) <= 0) && FlagType == 0)
                    {
                        LblErrorMessage.Text = "Receive Qty must be greater than 0 and less or equals to " + Hn_TQty.Value + "............";
                        Tran.Commit();
                        return;
                    }
                }
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
                    _arrpara[24] = new SqlParameter("@StockValue", SqlDbType.VarChar, 8000);
                    _arrpara[25] = new SqlParameter("@ProcessId", SqlDbType.Int);
                    if (ViewState["recid"] == null)
                    {
                        ViewState["recid"] = 0;
                    }
                    _arrpara[0].Direction = ParameterDirection.InputOutput;
                    _arrpara[0].Value = ViewState["recid"].ToString();
                    _arrpara[1].Value = ddemp.SelectedValue;
                    _arrpara[2].Value = TxtreceiveDate.Text;
                    _arrpara[3].Value = DDUnit.SelectedValue;
                    _arrpara[4].Value = Session["varuserid"].ToString();
                    _arrpara[5].Direction = ParameterDirection.InputOutput;
                    _arrpara[5].Value = TxtChallanNo.Text;
                    _arrpara[6].Value = ddCompName.SelectedValue;
                    _arrpara[7].Value = "";
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
                    _arrpara[16].Value = 0;
                    _arrpara[17].Value = 0;
                    _arrpara[18].Value = 0;
                    _arrpara[19].Value = ISSUEID;
                    _arrpara[20].Value = issuedetailid;
                    _arrpara[21].Value = hnorderid.Value;
                    _arrpara[22].Value = 0;
                    _arrpara[23].Value = DDcaltype.SelectedValue;
                    _arrpara[24].Value = hnstockno.Value;
                    _arrpara[25].Value = ddprocess.SelectedValue;
                    #region on 19-Feb-2014
                    //if (Convert.ToUInt32(ViewState["recid"]) == 0)
                    //{
                    //    int a = Convert.ToInt32(SqlHelper.ExecuteScalar(Tran, CommandType.Text, "select Isnull(Max(process_rec_id ),0)+1 from MasterSetting"));
                    //    ViewState["recid"] = a;
                    //    _arrpara[0].Value = ViewState["recid"].ToString();
                    //    TxtChallanNo.Text = ViewState["recid"].ToString();
                    //    _arrpara[5].Value = ViewState["recid"].ToString();
                    //    string str = @"Update MasterSetting Set process_rec_id =" + _arrpara[0].Value + " ";
                    //    str = str + @" insert into process_receive_master_" + ddprocess.SelectedValue + " (process_rec_id,empid,receivedate,unitid,userid,challanno,companyid,remarks,CalType) values(" + _arrpara[0].Value + "," + _arrpara[1].Value + ",'" + _arrpara[2].Value + "'," + _arrpara[3].Value + "," + _arrpara[4].Value + ",'" + _arrpara[5].Value + "'," + _arrpara[6].Value + ",'" + _arrpara[7].Value + "'," + _arrpara[23].Value + ")";
                    //    SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                    //    //string strproc = @"insert into process_receive_master_" + ddprocess.SelectedValue + " (process_rec_id,empid,receivedate,unitid,userid,challanno,companyid,remarks,CalType) values(" + _arrpara[0].Value + "," + _arrpara[1].Value + ",'" + _arrpara[2].Value + "'," + _arrpara[3].Value + "," + _arrpara[4].Value + ",'" + _arrpara[5].Value + "'," + _arrpara[6].Value + ",'" + _arrpara[7].Value + "'," + _arrpara[23].Value + ")";
                    //    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strproc); commented By: Rajeev
                    //}
                    //string strpro_detail = @"insert into process_receive_detail_" + ddprocess.SelectedValue + " (process_rec_detail_id,process_rec_id,item_finished_id,length,width,area,rate,amount,qty,weight,comm,commamt,issueorderid,issue_detail_id,orderid,penality,QualityType) values(" + _arrpara[8].Value + "," + _arrpara[0].Value + "," + _arrpara[9].Value + "," + _arrpara[10].Value + "," + _arrpara[11].Value + "," + _arrpara[12].Value + "," + _arrpara[13].Value + "," + _arrpara[14].Value + "," + _arrpara[15].Value + "," + _arrpara[16].Value + "," + _arrpara[17].Value + "," + _arrpara[18].Value + "," + _arrpara[19].Value + "," + _arrpara[20].Value + "," + _arrpara[21].Value + "," + _arrpara[22].Value + ",1)";
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strpro_detail);


                    //UtilityModule.PROCESS_RECEIVE_CONSUMPTION(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(ddprocess.SelectedValue), Convert.ToDouble(_arrpara[12].Value), Convert.ToDouble(_arrpara[16].Value), Convert.ToInt32(_arrpara[3].Value), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[19].Value), Tran, 0, Convert.ToInt32(_arrpara[15].Value), Convert.ToInt32(_arrpara[23].Value));

                    //for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    //{

                    //string str1 = @"Update PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + " Set PQty=PQty-" + _arrpara[15].Value + " Where IssueOrderid=" + _arrpara[19].Value + " And Issue_Detail_Id=" + _arrpara[20].Value + "";
                    //str1 = str1 + @" Update Process_Stock_Detail set receivedate='" + _arrpara[2].Value + "',receivedetailid=" + _arrpara[8].Value + " Where IssueDetailId=" + _arrpara[20].Value + " And Stockno in(" + hnstockno.Value + ") and toprocessid=" + ddprocess.SelectedValue + "";
                    //str1 = str1 + @" Update carpetnumber set issrecstatus=0 where stockno in(" + hnstockno.Value + ")";
                    //SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str1);
                    #endregion
                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_NextProcessReceive", _arrpara);
                    ViewState["recid"] = _arrpara[0].Value.ToString();
                    ////TxtChallanNo.Text = ViewState["recid"].ToString();

                    TxtChallanNo.Text = _arrpara[5].Value.ToString();

                    //UtilityModule.PROCESS_RECEIVE_CONSUMPTION(Convert.ToInt32(_arrpara[8].Value), Convert.ToInt32(_arrpara[0].Value), Convert.ToInt32(_arrpara[9].Value), Convert.ToInt32(ddprocess.SelectedValue), Convert.ToDouble(_arrpara[12].Value), Convert.ToDouble(_arrpara[16].Value), Convert.ToInt32(_arrpara[3].Value), Convert.ToInt32(_arrpara[20].Value), Convert.ToInt32(_arrpara[19].Value), Tran, 0, Convert.ToInt32(_arrpara[15].Value), Convert.ToInt32(_arrpara[23].Value));

                    //}
                }
            }
            else
            {
                Str = SqlHelper.ExecuteScalar(Tran, CommandType.Text, @"Select Replace(Str(PIM.IssueOrderId),' ','')+'  /  '+EI.EmpName+'  /  '+replace(convert(varchar(11),AssignDate,106), ' ','-') EmpInFormation 
                    From Process_Stock_detail PSD,CarpetNumber CN,Process_Issue_Master_" + ddprocess.SelectedValue + " PIM,Process_Issue_Detail_" + ddprocess.SelectedValue + @" PID,Empinfo EI
                    Where PSD.StockNo=CN.StockNo And ToProcessID=" + ddprocess.SelectedValue + @" And PSD.IssueDetailId=PID.Issue_Detail_Id And PIM.IssueOrderId=PID.IssueOrderId And EI.EmpId=PIM.EmpId And CN.TStockNo='" + TxtStockNo.Text + "' And PIM.CompanyId=" + ddCompName.SelectedValue + "").ToString();
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Issue To " + Str + "');", true);
            }
            TxtStockNo.Text = "";
            Tran.Commit();
            LblErrorMessage.Text = "Data saved successfully...";
            //fill_DetailGride();
            //Fill_Grid_Total();
            //GetStock();
            if (FlagType == 2)
            {
                TxtStockNo.Focus();
            }
            else if (FlagType == 0)
            {
                fillgrid();
            }
            if (Session["varMasterCompanyIDForERP"].ToString() == "16")
            {
                fill_DetailGride();
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
    protected void ddCompName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = 0;
    }
    private void fill_DetailGride()
    {
        if (ddprocess.SelectedIndex > 0)
        {
            string sqlstr, view = "";
            if (variable.VarNewQualitySize == "1")
            {
                view = "v_finisheditemdetailnew";
            }
            else
            {
                view = "v_finisheditemdetail";
            }

            #region
            //            if (variable.VarNewQualitySize == "1")
            //            {
            //                sqlstr = @"Select process_rec_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
            //                   Length + 'x' + Width Size,Qty,Rate,Area,Amount,case When " + Session["varMasterCompanyIDForERP"] + "=9 Then '' else  (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) End as StockNo,OM.Localorder
            //                   From process_receive_master_" + ddprocess.SelectedValue + @" PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,
            //                   ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM ,ORDERMASTER OM
            //                   Where PM.process_rec_id=PD.process_rec_id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
            //                   PD.orderid=OM.orderid and PM.process_rec_id=" + ViewState["recid"] + " And PM.CompanyId=" + ddCompName.SelectedValue + " Order By process_rec_Detail_Id desc";
            //            }
            //            else
            //            {
            //                sqlstr = @"Select process_rec_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS + Space(5) + Case When PM.Unitid=1 Then SizeMtr Else SizeFt End Description,Width,Length,
            //                   Length + 'x' + Width Size,Qty,Rate,Area,Amount,case When " + Session["varMasterCompanyIDForERP"] + "=9 Then '' else  (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) End as StockNo,OM.localorder
            //                   From process_receive_master_" + ddprocess.SelectedValue + @" PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,
            //                   ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM ,ordermaster om
            //                   Where PM.process_rec_id=PD.process_rec_id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And IM.Category_Id=ICM.Category_Id And 
            //                   Pd.orderid=om.orderid and PM.process_rec_id=" + ViewState["recid"] + " And PM.CompanyId=" + ddCompName.SelectedValue + " Order By process_rec_Detail_Id desc";

            //            }
            #endregion

            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {
                sqlstr = @" Select pd.process_rec_Detail_Id,vf.Category_Name Category,vf.Item_Name Item,vf.QualityName+'  '+vf.designName+'  '+vf.ColorName+'  '+vf.ShapeName + Space(5) + Case When PM.Unitid=1 Then vf.SizeMtr Else case when PM.UnitId=6 then vf.SizeInch Else vf.SizeFt End End Description,Width,Length,
                        Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](pd.process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) as StockNo,OM.localorder,case when cn.Currentprostatus<>" + ddprocess.SelectedValue + @" then 'Y' Else 'N' end as isissued
                        From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " PM inner join PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD on PM.Process_Rec_Id=PD.Process_Rec_Id
                        inner join " + view + @" vf on PD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                        inner join OrderMaster om on pd.OrderId=om.OrderId
	                    inner join process_stock_detail psd on pd.process_rec_detail_id=psd.ReceiveDetailId and psd.ToProcessId=" + ddprocess.SelectedValue + @"
						inner join CarpetNumber cn on psd.StockNo=cn.StockNo
                        Where PM.Process_rec_id=" + ViewState["recid"] + " and PM.Companyid=" + ddCompName.SelectedValue + " order by PD.Process_Rec_Detail_Id";
            }
            else
            {
                sqlstr = @" Select pd.process_rec_Detail_Id,vf.Category_Name Category,vf.Item_Name Item,vf.QualityName+'  '+vf.designName+'  '+vf.ColorName+'  '+vf.ShapeName + Space(5) + Case When PM.Unitid=1 Then vf.SizeMtr Else vf.SizeFt End Description,Width,Length,
                        Length + 'x' + Width Size,Qty,Rate,Area,Amount,(Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](pd.process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) as StockNo,OM.localorder,case when cn.Currentprostatus<>" + ddprocess.SelectedValue + @" then 'Y' Else 'N' end as isissued
                        From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " PM inner join PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD on PM.Process_Rec_Id=PD.Process_Rec_Id
                        inner join " + view + @" vf on PD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                        inner join OrderMaster om on pd.OrderId=om.OrderId
	                    inner join process_stock_detail psd on pd.process_rec_detail_id=psd.ReceiveDetailId and psd.ToProcessId=" + ddprocess.SelectedValue + @"
						inner join CarpetNumber cn on psd.StockNo=cn.StockNo
                        Where PM.Process_rec_id=" + ViewState["recid"] + " and PM.Companyid=" + ddCompName.SelectedValue + " order by PD.Process_Rec_Detail_Id";
            }

            
            DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
            if (DS.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = DS;
                DGDetail.DataBind();
                TxtTotalPcs.Text = Convert.ToString(DS.Tables[0].Compute("sum(qty)", ""));
                TxtArea.Text = Convert.ToString(DS.Tables[0].Compute("sum(area)", ""));
                TxtAmount.Text = Convert.ToString(DS.Tables[0].Compute("sum(amount)", ""));
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
        save_detail();
        Focus = "TxtStockNo";
    }
    protected void save_detail()
    {
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select CompanyId,IssRecStatus,CurrentProStatus,Pack,stockno from CarpetNumber Where TStockNo='" + TxtStockNo.Text + "'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "";
            if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CompanyId"]) != Convert.ToInt32(ddCompName.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Company...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["issRecStatus"]) != 1)
            {
                LblErrorMessage.Text = "This Stock No Already Received...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["CurrentProStatus"]) != Convert.ToInt32(ddprocess.SelectedValue))
            {
                LblErrorMessage.Text = "This Stock No Does Not Belong To That Process...";
                TxtStockNo.Focus();
            }
            else if (Convert.ToInt32(Ds.Tables[0].Rows[0]["Pack"]) != 0)
            {
                LblErrorMessage.Text = "This Stock No Already Packed....";
                TxtStockNo.Focus();
            }
        }
        else
        {
            LblErrorMessage.Text = "Pls Check Stock No....";
            TxtStockNo.Focus();
        }
        if (LblErrorMessage.Text == "")
        {
            hnstockno.Value = Ds.Tables[0].Rows[0]["stockno"].ToString();
            hnrate1.Value = "";
            save_carpet_wise(2);
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        try
        {
            string str = "";
            if (Session["varMasterCompanyIDForERP"].ToString() == "9")
            {

                str = @"Delete TEMP_PROCESS_RECEIVE_MASTER   Delete TEMP_PROCESS_RECEIVE_DETAIL  " + " ";
                str = str + @" Insert into TEMP_PROCESS_RECEIVE_MASTER Select Process_Rec_Id,EmpId,ReceiveDate,UnitId,UserId,ChallanNo,Companyid,Remarks,CalType," + ddprocess.SelectedValue + "  from PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
                str = str + @" Insert into TEMP_PROCESS_RECEIVE_DETAIL Select Process_Rec_Detail_Id, Process_Rec_Id, Item_Finished_Id, Length, Width, Area, Rate, Amount, Qty, Weight, Comm, CommAmt, IssueOrderId, Issue_Detail_Id, OrderId, Penality, PRemarks, QualityType, GatePassNo, FlagFixOrWeight, TDSPercentage, Warp_10cm, Weft_10cm, straightness, Design, OBA, Date_Stamp, StockNoRemarks, WyPly, Cyply from PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);
                Report();
            }
            else
            {
                str = @"select Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
                                ,Ei.EmpId,EI.Empname,EI.address,EI.Address2,EI.Address3,EI.Mobile,Ei.GSTNo as Empgstin,PID.issueorderid
                                ,PIM.Receivedate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") as Job,
                                Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                                PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
                                (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + ddprocess.SelectedValue + @",PID.issue_detail_id)) TStockNo
                                From PROCESS_Receive_MASTER_" + ddprocess.SelectedValue + " PIM inner Join PROCESS_Receive_DETAIL_" + ddprocess.SelectedValue + @" PID on PIM.Process_Rec_Id=PID.Process_Rec_Id
                                inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
                                inner Join EmpInfo EI on PIM.Empid=EI.empid
                                inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                                 Where PIM.Process_rec_id=" + ViewState["recid"] + " order by Process_rec_detail_id";
                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                    Session["GetDataset"] = ds;
                    Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2.xsd";
                    StringBuilder stb = new StringBuilder();
                    stb.Append("<script>");
                    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }
                //string str = @"Delete TEMP_PROCESS_RECEIVE_MASTER   Delete TEMP_PROCESS_RECEIVE_DETAIL  " + " ";
                //str = str + @" Insert into TEMP_PROCESS_RECEIVE_MASTER Select Process_Rec_Id,EmpId,ReceiveDate,UnitId,UserId,ChallanNo,Companyid,Remarks,CalType," + ddprocess.SelectedValue + "  from PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
                //str = str + @" Insert into TEMP_PROCESS_RECEIVE_DETAIL Select Process_Rec_Detail_Id, Process_Rec_Id, Item_Finished_Id, Length, Width, Area, Rate, Amount, Qty, Weight, Comm, CommAmt, IssueOrderId, Issue_Detail_Id, OrderId, Penality, PRemarks, QualityType, GatePassNo, FlagFixOrWeight, TDSPercentage, Warp_10cm, Weft_10cm, straightness, Design, OBA, Date_Stamp, StockNoRemarks, WyPly, Cyply from PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + ViewState["recid"] + "";
                //SqlHelper.ExecuteNonQuery(con, CommandType.Text, str);


                //Session["ReportPath"] = "Reports/NextProcessReceive.rpt";
                //Session["CommanFormula"] = "{V_NextProcessReceive.Process_Rec_Id}=" + ViewState["recid"] + "";
                //LblErrorMessage.Visible = false;
                //LblErrorMessage.Text = "";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "Preview();", true);
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

        //Report();
    }
    private void Report()
    {
        string qry = @" SELECT  V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,
        V_NextProcessReceive.QualityName,V_NextProcessReceive.designName,V_NextProcessReceive.ColorName,V_NextProcessReceive.ShapeName,
		Sum(V_NextProcessReceive.Qty) as QTY,V_NextProcessReceive.Rate,Sum(V_NextProcessReceive.Area) as area,
        Sum(V_NextProcessReceive.Amount) as Amount,V_NextProcessReceive.ITEM_NAME,V_NextProcessReceive.ReceiveDate,V_NextProcessReceive.UnitId,V_NextProcessReceive.ChallanNo,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessReceive.Length,
        V_NextProcessReceive.Width,UnitName,LocalOrder,V_NextProcessReceive.MastercompanyId,V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN
        FROM  PROCESS_NAME_MASTER INNER JOIN V_Companyinfo INNER JOIN V_NextProcessReceive ON V_Companyinfo.CompanyId=V_NextProcessReceive.Companyid ON PROCESS_NAME_MASTER.PROCESS_NAME_ID=V_NextProcessReceive.PROCESSID
        INNER JOIN V_EmployeeInfo ON V_NextProcessReceive.Empid=V_EmployeeInfo.EmpId
        Where V_NextProcessReceive.Process_Rec_Id=" + ViewState["recid"] + " And Process_name_Master.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And V_CompanyInfo.CompanyId=" + ddCompName.SelectedValue + @" group by V_Companyinfo.CompanyName,V_Companyinfo.CompanyAddress,V_Companyinfo.CompanyPhoneNo,V_Companyinfo.CompanyFaxNo,V_Companyinfo.TinNo,V_EmployeeInfo.EmpName,V_EmployeeInfo.EmpAddress,V_EmployeeInfo.EmpPhoneNo,
        V_NextProcessReceive.QualityName,V_NextProcessReceive.designName,V_NextProcessReceive.ColorName,V_NextProcessReceive.ShapeName,V_NextProcessReceive.Rate,V_NextProcessReceive.ITEM_NAME,V_NextProcessReceive.ReceiveDate,V_NextProcessReceive.UnitId,V_NextProcessReceive.ChallanNo,PROCESS_NAME_MASTER.PROCESS_NAME,V_NextProcessReceive.Length
		,V_NextProcessReceive.Width,UnitName,LocalOrder,V_NextProcessReceive.MastercompanyId,V_Companyinfo.GSTIN,V_EmployeeInfo.EMPGSTIN";
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
        fillgrid();
    }
    protected void DGDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int VarProcess_Receive_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select PSD.*,PD.IssueOrderId from Process_Stock_Detail PSD,CarpetNumber CN,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD 
            Where PSD.StockNo=CN.StockNo And PSD.ReceiveDetailId=PD.Process_Rec_Detail_Id And PSD.IssueDetailId=PD.Issue_Detail_Id And ReceiveDetailId=" + VarProcess_Receive_Detail_Id + " And ToProcessId=" + ddprocess.SelectedValue + " And CurrentProStatus=" + ddprocess.SelectedValue + " And IssRecStatus=0 And CN.CompanyId=" + ddCompName.SelectedValue + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                LblErrorMessage.Text = "";
                SqlParameter[] _array = new SqlParameter[5];
                _array[0] = new SqlParameter("@ProcessId", SqlDbType.Int);
                _array[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
                _array[2] = new SqlParameter("@IssueDetailId", SqlDbType.Int);
                _array[3] = new SqlParameter("@IssueOrderId", SqlDbType.Int);
                _array[4] = new SqlParameter("@RowCount", SqlDbType.Int);

                _array[0].Value = ddprocess.SelectedValue;
                _array[1].Value = VarProcess_Receive_Detail_Id;
                _array[2].Value = Ds.Tables[0].Rows[0]["IssueDetailId"];
                _array[3].Value = Ds.Tables[0].Rows[0]["IssueOrderId"];
                _array[4].Value = DGDetail.Rows.Count;

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextProductionReceiveDetail", _array);

                Tran.Commit();
                fill_DetailGride();

            }
            else
            {
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "You Have issueed....";
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
            @"Select * From TEMP_PROCESS_RECEIVE_MASTER_NEW Where CompanyId = " + ddCompName.SelectedValue + " And Process_Rec_Id=" + TxtEditChallanNo.Text);
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
        string Str = "Select Process_Rec_Id,EmpId,Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,UnitId,Remarks,caltype From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " Where Process_Rec_Id=" + DDChallanNo.SelectedValue + " And CompanyId=" + ddCompName.SelectedValue;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        if (Ds.Tables[0].Rows.Count > 0)
        {
            TxtreceiveDate.Text = Ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            TxtRemarks.Text = Ds.Tables[0].Rows[0]["Remarks"].ToString();
            TxtChallanNo.Text = Ds.Tables[0].Rows[0]["Process_Rec_Id"].ToString();
            DDUnit.SelectedValue = Ds.Tables[0].Rows[0]["UnitId"].ToString();
            DDcaltype.SelectedValue = Ds.Tables[0].Rows[0]["caltype"].ToString();
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
            Label lblqty = (Label)DGDetail.Rows[i].FindControl("lblqty");
            Label lblarea = (Label)DGDetail.Rows[i].FindControl("lblarea");
            Label lblamount = (Label)DGDetail.Rows[i].FindControl("lblamount");
            varTotalPcs = varTotalPcs + Convert.ToInt32(lblqty.Text);
            varTotalArea = varTotalArea + Convert.ToDouble(lblarea.Text);
            varTotalAmount = varTotalAmount + Convert.ToDouble(lblamount.Text);
        }
        TxtTotalPcs.Text = varTotalPcs.ToString();
        TxtArea.Text = Math.Round(varTotalArea, 4).ToString();
        TxtAmount.Text = Math.Round(varTotalAmount, 2).ToString();
    }
    protected void DGDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int VarProcess_Receive_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value);
        //ViewState["VarProcess_Receive_Detail_Id"] = VarProcess_Receive_Detail_Id;
        //int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
        //if (VARQCTYPE == 1)
        //{
        //    qulitychk.Visible = true;
        //    fillgrdquality();
        //    fillchkbox(Convert.ToInt32(ViewState["VarProcess_Receive_Detail_Id"]), "process_receive_detail_" + ddprocess.SelectedValue + "");
        //}
        //else
        //{
        //    qulitychk.Visible = false;
        //}
        LblErrorMessage.Visible = true;
        LblErrorMessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            TextBox txteditrate = (TextBox)DGDetail.Rows[e.RowIndex].FindControl("txteditrate");
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@processid", ddprocess.SelectedValue);
            param[1] = new SqlParameter("@Process_Rec_detail_id", Convert.ToInt32(DGDetail.DataKeys[e.RowIndex].Value));
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@rate", txteditrate.Text == "" ? "0" : txteditrate.Text);
            //******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_updateNextreceive", param);
            LblErrorMessage.Text = param[2].Value.ToString();
            Tran.Commit();
            DGDetail.EditIndex = -1;
            fill_DetailGride();
        }
        catch (Exception ex)
        {
            LblErrorMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void fillgrdquality()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        DataSet mydataset;
        if (variable.VarNewQualitySize == "1")
        {
            mydataset = SqlHelper.ExecuteDataset(con, CommandType.Text, @"SELECT CATEGORY_ID,ITEM_ID,Process_Rec_ID FROM V_FinishedItemDetailNew
        inner join process_receive_detail_" + ddprocess.SelectedValue + " on V_FinishedItemDetailNew.ITEM_FINISHED_ID=process_receive_detail_" + ddprocess.SelectedValue + ".ITEM_FINISHED_ID WHERE Process_Rec_Detail_Id =" + ViewState["VarProcess_Receive_Detail_Id"] + " And V_FinishedItemDetailNew.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        }
        else
        {
            mydataset = SqlHelper.ExecuteDataset(con, CommandType.Text, @"SELECT CATEGORY_ID,ITEM_ID,Process_Rec_ID FROM V_FinishedItemDetail
        inner join process_receive_detail_" + ddprocess.SelectedValue + " on V_FinishedItemDetail.ITEM_FINISHED_ID=process_receive_detail_" + ddprocess.SelectedValue + ".ITEM_FINISHED_ID WHERE Process_Rec_Detail_Id =" + ViewState["VarProcess_Receive_Detail_Id"] + " And V_FinishedItemDetail.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
        }

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "Select QCMaster.ID,SrNo,ParaName from QCParameter inner join QCMaster on QCParameter.ParaID=QCMaster.ParaID where CategoryID=" + mydataset.Tables[0].Rows[0]["CATEGORY_ID"] + " and ItemID=" + mydataset.Tables[0].Rows[0]["ITEM_ID"] + " and ProcessID=" + ddprocess.SelectedValue + " order by SrNo");
        grdqualitychk.DataSource = ds;
        grdqualitychk.DataBind();
    }
    protected void btnqcsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            QCSAVE(Tran, Convert.ToInt32(ViewState["recid"]), Convert.ToInt32(ViewState["VarProcess_Receive_Detail_Id"]), "process_receive_detail_" + ddprocess.SelectedValue + "");
            Tran.Commit();
            qulitychk.Visible = false;
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    public void fillchkbox(int RecieveDetailID, string tablename)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        string qry = @"Select QcmasterID from QCDETAIL where RecieveDetailID=" + RecieveDetailID + " and Qcvalue='1' and RefName='" + tablename + "'";
        SqlDataAdapter sda = new SqlDataAdapter(qry, con);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            for (int j = 0; j < grdqualitychk.Rows.Count; j++)
            {
                //if (((Label)grdqualitychk.Rows[j].FindControl("Label1")).Text == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                if (Convert.ToString(grdqualitychk.DataKeys[j].Value) == ds.Tables[0].Rows[i]["QcmasterID"].ToString())
                {
                    CheckBox chk = (CheckBox)grdqualitychk.Rows[j].FindControl("CheckBox1");
                    chk.Checked = true;
                }
            }
        }
    }
    private void QCSAVE(SqlTransaction Tran, int ReceiveId, int ReceiveDetailId, string tablename)
    {
        string checkpara = "";
        string noncheck = "";

        for (int i = 0; i < grdqualitychk.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)grdqualitychk.Rows[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                if (checkpara == "")
                {
                    checkpara = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    checkpara = checkpara + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
            }
            else
                if (noncheck == "")
                {
                    noncheck = grdqualitychk.DataKeys[i].Value.ToString();
                }
                else
                {
                    noncheck = noncheck + "," + grdqualitychk.DataKeys[i].Value.ToString();
                }
        }
        SqlParameter[] _arrpara1 = new SqlParameter[5];
        _arrpara1[0] = new SqlParameter("@ReceiveId", SqlDbType.Int);
        _arrpara1[1] = new SqlParameter("@ReceiveDetailId", SqlDbType.Int);
        _arrpara1[2] = new SqlParameter("@checkpara", SqlDbType.NVarChar, 50);
        _arrpara1[3] = new SqlParameter("@noncheck", SqlDbType.NVarChar, 50);
        _arrpara1[4] = new SqlParameter("@TableName", SqlDbType.NVarChar, 50);
        _arrpara1[0].Value = ReceiveId;
        _arrpara1[1].Value = ReceiveDetailId;
        _arrpara1[2].Value = checkpara;
        _arrpara1[3].Value = noncheck;
        _arrpara1[4].Value = tablename;
        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PurchaseReceivequalitychk", _arrpara1);
    }
    protected void btnQcPreview_Click(object sender, EventArgs e)
    {
        string SName = "";
        string QCValue = "";
        string qry = "";
        DataSet ds = new DataSet();

        if (variable.VarNewQualitySize == "1")
        {
            qry = @"Select ICM.Category_Name,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Width+'x'+Length Description,
                PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,(Select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") ShortName,
                PM.UnitId,PM.ChallanNo,'' StockNo,PM.Process_Rec_Id,PD.Process_Rec_Detail_Id
                From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSSNew IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,
                EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And 
                PM.Companyid=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And IM.Category_Id=ICM.Category_Id And Isnull(PD.QualityType,0)<>3 And PM.Process_Rec_Id=" + ViewState["recid"];
        }
        else
        {
            qry = @"Select ICM.Category_Name,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Width+'x'+Length Description,
                PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,(Select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") ShortName,
                PM.UnitId,PM.ChallanNo,'' StockNo,PM.Process_Rec_Id,PD.Process_Rec_Detail_Id
                From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,
                EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And 
                PM.Companyid=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And IM.Category_Id=ICM.Category_Id And Isnull(PD.QualityType,0)<>3 And PM.Process_Rec_Id=" + ViewState["recid"];
        }
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        DataTable mytable = new DataTable();
        mytable.Columns.Add("PrtID", typeof(int));
        mytable.Columns.Add("SName", typeof(string));
        mytable.Columns.Add("QCValue", typeof(string));

        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        {
            string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
                         Where RefName= 'PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + "' And ProcessId=" + ddprocess.SelectedValue + " And QCD.RecieveID=" + ViewState["recid"] + " And QCD.RecieveDetailID=" + ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"];
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
            mytable.Rows.Add(ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"], SName, QCValue);
            SName = "";
            QCValue = "";
        }
        ds.Tables.Add(mytable);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/NextProcessReceiveQC.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\NextProcessReceiveQC.xsd";
            Session["GetDataset"] = ds;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
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
        where V_NextProcessReceive.Process_Rec_Id=" + ViewState["recid"] + " And Process_Name_Master.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " And V_Companyinfo.CompanyId=" + ddCompName.SelectedValue;
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
    //protected void mygdv_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void mygdstock_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void DGDetail_RowCreated(object sender, GridViewRowEventArgs e)
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
    //protected void grdqualitychk_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        UtilityModule.LogOut(Convert.ToInt32(Session["varuserid"]));
        Session["varuserid"] = null;
        Session["varMasterCompanyIDForERP"] = null;
        string message = "you are successfully loggedout..";
        Response.Redirect("~/Login.aspx?Message=" + message + "");
    }
    private void logo()
    {
        if (File.Exists(Server.MapPath("~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif")))
        {
            imgLogo.ImageUrl.DefaultIfEmpty();
            imgLogo.ImageUrl = "~/Images/Logo/" + Session["varMasterCompanyIDForERP"] + "_company.gif?" + DateTime.Now.ToString("ddyyhhmmss");
        }
        LblCompanyName.Text = Session["varCompanyName"].ToString();
        LblUserName.Text = Session["varusername"].ToString();
    }
    protected void DGDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.Equals("Penality"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = DGDetail.Rows[index];

            int id = Convert.ToInt32(DGDetail.DataKeys[index].Value.ToString());
            TDPenality.Visible = true;
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Process_Rec_Detail_Id Rec_Detail_Id,Isnull(Penality,0) PenalityAmt,Isnull(PRemarks,'') PenalityRemark from PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + " Where Process_Rec_Detail_Id=" + id);
            DGPenality.DataSource = ds;
            DGPenality.DataBind();
            ((TextBox)DGPenality.Rows[0].FindControl("TxtPenalityAmt")).Focus();
        }
        else if (e.CommandName.Equals("update"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int VarProcess_Receive_Detail_Id = Convert.ToInt32(DGDetail.DataKeys[index].Value);
            ViewState["VarProcess_Receive_Detail_Id"] = VarProcess_Receive_Detail_Id;
            int VARQCTYPE = Convert.ToInt32(SqlHelper.ExecuteScalar(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select VARQCTYPE From MasterSetting"));
            if (VARQCTYPE == 1)
            {
                qulitychk.Visible = true;
                fillgrdquality();
                fillchkbox(Convert.ToInt32(ViewState["VarProcess_Receive_Detail_Id"]), "process_receive_detail_" + ddprocess.SelectedValue + "");
            }
            else
            {
                qulitychk.Visible = false;
            }
        }
        else
        {
            TDPenality.Visible = false;
        }
    }
    protected void BtnPenalitySave_Click(object sender, EventArgs e)
    {
        int VarProcess_Receive_Detail_Id = Convert.ToInt32(DGPenality.DataKeys[0].Value);
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Update PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + " Set Penality=" + ((TextBox)DGPenality.Rows[0].FindControl("TxtPenalityAmt")).Text + ",PRemarks='" + ((TextBox)DGPenality.Rows[0].FindControl("TxtPenalityRemark")).Text + "' Where Process_Rec_Detail_Id=" + VarProcess_Receive_Detail_Id);
        TDPenality.Visible = false;
        ((TextBox)DGPenality.Rows[0].FindControl("TxtPenalityAmt")).Text = "";
        ((TextBox)DGPenality.Rows[0].FindControl("TxtPenalityRemark")).Text = "";
    }
    protected void DGPenality_RowCreated(object sender, GridViewRowEventArgs e)
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
            _array[1].Value = ddemp.SelectedValue;
            _array[2].Value = ddorderno.SelectedValue;
            _array[3].Value = hnorderid.Value;
            _array[4].Value = hn_finished.Value;
            _array[5].Value = ddprocess.SelectedValue;
            _array[6].Value = ddCompName.SelectedValue;
            _array[7].Value = Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_FillGridDetail_NextReceive", _array);
            //Table 0 For DGDetail And table 1 For dgstock
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = ds.Tables[0];
                DGDetail.DataBind();

                TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                TxtArea.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("Sum(Area)", "")), 4).ToString();
                TxtAmount.Text = Math.Round(Convert.ToDecimal(ds.Tables[0].Compute("Sum(AMount)", "")), 4).ToString();
            }
            else
            {
                DGDetail.DataSource = null;
                DGDetail.DataBind();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                mygdstock.DataSource = ds.Tables[1];
                mygdstock.DataBind();
            }
            else
            {
                mygdstock.DataSource = null;
                mygdstock.DataBind();
            }

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
        //FillData_Dgdetail();
        fill_DetailGride();
    }
    protected void mygdv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (Session["varcompanyNO"].ToString())
        {
            case "9":
                mygdv.Columns[6].Visible = true;
                break;
        }
    }
    protected void DGDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGDetail.EditIndex = e.NewEditIndex;
        fill_DetailGride();
        Focus = "DGDetail";
        TextBox txt = (TextBox)DGDetail.Rows[e.NewEditIndex].FindControl("txteditrate");
        txt.Focus();
        txt.Attributes.Add("onFocus", "this.select()");

        //DGDetail.Rows[e.NewEditIndex].FindControl("txteditrate").Focus();

    }
    protected void DGDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGDetail.EditIndex = -1;
        fill_DetailGride();
    }
    protected void DDsrno_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = false;
        //*sql Table Types
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Process_rec_detail_id", typeof(int));
        //***************
        int rowcount = DGDetail.Rows.Count;
        for (int i = 0; i < rowcount; i++)
        {
            CheckBox chkboxitem = (CheckBox)DGDetail.Rows[i].FindControl("chkboxitem");
            if (chkboxitem.Checked == true)
            {
                DataRow dr = dtrecords.NewRow();
                dr["Process_rec_detail_id"] = Convert.ToInt32(DGDetail.DataKeys[i].Value);
                dtrecords.Rows.Add(dr);
            }

        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Processid", ddprocess.SelectedValue);
                param[1] = new SqlParameter("@Msg", SqlDbType.VarChar, 500);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@dtrecords", dtrecords);
                param[3] = new SqlParameter("@process_rec_id", ViewState["recid"]);
                //****
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteNextReceiveBulk", param);
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = param[1].Value.ToString();
                Tran.Commit();
                fill_DetailGride();
            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = ex.Message;
                LblErrorMessage.Visible = true;
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "del", "alert('Please check atleast one check box to delete data!!!')", true);
        }
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblisissued = (Label)e.Row.FindControl("lblisissued");
            CheckBox chkboxitem = (CheckBox)e.Row.FindControl("chkboxitem");
            if (lblisissued.Text == "Y")
            {
                e.Row.BackColor = System.Drawing.Color.Green;
                chkboxitem.Enabled = false;

            }

            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "9")
                {
                    if (DGDetail.Columns[i].HeaderText == "QualityChk")
                    {
                        DGDetail.Columns[i].Visible = false;
                    }
                }
                else
                {
                    if (DGDetail.Columns[i].HeaderText == "QualityChk")
                    {
                        DGDetail.Columns[i].Visible = true;
                    }
                }
            }
        }
    }
}