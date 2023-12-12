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
    private const string SCRIPT_DOFOCUS =
  @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    static string btnclickflag = "";
    static decimal WashingByWeight = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            HookOnFocus(this.Page as Control);

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Masters_Process_frm_receive_process_next),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(nolock)
                            JOIN Company_Authentication CA(nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @"  
                            Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                            Select unitid,unitname from unit where unitid in (1,2,6)
                            Select U.UnitsId,U.UnitName from Units U inner join Units_authentication UA on U.unitsId=UA.UnitsId and UA.Userid=" + Session["varuserid"] + @" order by U.unitsId";
            str = str + " select PNM.PROCESS_NAME_ID,PNM.PROCESS_NAME From PROCESS_NAME_MASTER PNM inner join UserRightsProcess URP on PNM.PROCESS_NAME_ID=URP.ProcessId and URP.Userid=" + Session["varuserid"] + @"
                          WHere PNM.ProcessType=1 and PNM.PROCESS_NAME_ID<>1 order by PROCESS_NAME";
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

            UtilityModule.ConditionalComboFillWithDS(ref DDUnit, ds, 1, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddUnits, ds, 2, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddprocess, ds, 3, true, "--SELECT--");
            UtilityModule.ConditionalComboFillWithDS(ref ddcattype, ds, 4, true, "--SELECT--");
            if (ddcattype.Items.Count > 0)
            {
                ddcattype.SelectedIndex = 1;
                ddcattype_SelectedIndexChanged(ddcattype, new EventArgs());

            }
            if (ddUnits.Items.Count > 0)
            {
                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    if (Session["CurrentWorkingCompanyID"].ToString() == "2")
                    {
                        ddUnits.SelectedIndex = 2;
                    }
                    else
                    {
                        ddUnits.SelectedIndex = 1;
                    }
                }
                else
                {
                    ddUnits.SelectedIndex = 1;
                }                

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
                    break;
                case 14:
                    if (Session["Usertype"].ToString() == "1")
                    {
                        btnsavegrid.Visible = true;
                    }
                    else
                    {
                        btnsavegrid.Visible = false;
                    }
                    break;
                
                case 44:
                     TDemployee.Visible = false;
                    TablePartyChallanNo.Visible = true;
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    Tr4.Visible = true;
                    break;
                case 16:
                case 28:
                    TDemployee.Visible = false;
                    TablePartyChallanNo.Visible = true;
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    TdDyingLotNo.Visible = true;
                    break;
                case 30:
                    lblStockCarpetNo.Text = "Enter Carpet No";
                    break;
                case 38:
                    TDemployee.Visible = true;
                    TablePartyChallanNo.Visible = true;
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "200";
                    DGStockDetail.PageSize = 200;
                    break;
                case 43:
                    DDUnit.SelectedValue = "1";
                    TDTotalPcsNew.Visible = true;
                    Tr4.Visible = true;
                    Label4.Text = "Group No";
                    break;
                default:
                    DDUnit.SelectedValue = "1";
                    break;
            }
            hnorderid.Value = "0";
            if (variable.VarQctype == "1")
            {
                btnQcPreview.Visible = true;
                btnqcreport.Visible = true;
            }
        }
    }
    private void Fill_Temp_OrderNo()
    {
        SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "DELETE TEMP_PROCESS_RECEIVE_MASTER_NEW");
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select * from PROCESS_NAME_MASTER(nolock) Where Process_Name_Id<>1 And MasterCompanyId=" + Session["varCompanyId"] + " Order By Process_Name_Id");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Insert into TEMP_PROCESS_RECEIVE_MASTER_NEW SELECT CompanyId,EmpId,Process_Rec_Id," + Ds.Tables[0].Rows[i]["Process_Name_Id"] + " ProcessId FROM PROCESS_RECEIVE_MASTER_" + Ds.Tables[0].Rows[i]["Process_Name_Id"] + "(Nolock)");
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
        UtilityModule.ConditionalComboFill(ref ddemp, "Select Distinct EI.Empid,EI.EmpName From Empinfo EI(nolock),process_issue_master_" + ddprocess.SelectedValue + " PM(nolock) Where EI.EmpId=PM.EmpId And EI.MasterCompanyId=" + Session["varCompanyId"] + "", true, "--SELECT--");

        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"SELECT DISTINCT OCALTYPE FROM PROCESSCONSUMPTIONMASTER PM(nolock),PROCESSCONSUMPTIONDETAIL PD(nolock) WHERE PM.PCMID=PD.PCMID And PROCESSID=" + ddprocess.SelectedValue + " And PM.MasterCompanyId=" + Session["varCompanyId"] + "");
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
        if (Session["varcompanyId"].ToString() == "44")
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName From ITEM_MASTER IM inner join CategorySeparate CS on IM.CATEGORY_ID=cs.Categoryid and cs.id=0  inner join Quality Q on IM.ITEM_ID=q.Item_Id and Cs.Categoryid=" + ddcattype.SelectedValue + " and Im.mastercompanyid=" + Session["varcompanyid"] + "  where upper(q.QualityName) not in('BACK','FRONT','PIPING','LINING','TOP','BOTTOM','SIDE','PATCH') order by Qualityname", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref ddquality, @"select Distinct Q.QualityId,q.QualityName+' ['+Im.Item_Name+']' as QualityName From ITEM_MASTER IM inner join CategorySeparate CS on 
                                                         IM.CATEGORY_ID=cs.Categoryid and cs.id=0  inner join Quality Q on IM.ITEM_ID=q.Item_Id and Cs.Categoryid=" + ddcattype.SelectedValue + " and Im.mastercompanyid=" + Session["varcompanyid"] + " order by Qualityname", true, "--Plz Select--");
           
        
        }
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
    }
    protected void ddcolour_SelectedIndexChanged(object sender, EventArgs e)
    {
        string StrSize = "vf.SizeMtr + ' ' + vf.shapename";
        string str = "";

        if (Session["varcompanyId"].ToString() == "38")
        {
            StrSize = "vf.Sizeft + ' ' + vf.shapename";
        }

        str = "select Distinct vf.sizeid," + StrSize + @" as size From V_FinishedItemDetail vf where vf.QualityId=" + ddquality.SelectedValue + " and vf.designid=" + ddldesig.SelectedValue + " and vf.colorid=" + ddcolour.SelectedValue + " and vf.sizeid<>0 order by Size";

        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Plz Select--");
    }
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void ddlshade_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    private void GetStock()
    {
        DataSet ds1 = null;
        try
        {
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
            ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, strsql);
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
    }
    private void save_carpet_wise(int FlagType, string EmpId)
    {
        string Str = "";

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            if (lstWeaverName.Items.Count == 0)
            {
                LblErrorMessage.Text = "Plz Enter ID No...";
                return;
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrpara = new SqlParameter[37];
            DataSet ds3 = new DataSet(); 
            Str = @"Select CalType,UnitId,PM.IssueOrderId,PD.issue_Detail_id,length,width,area,rate,Round(Amount/Qty, 2) Amount,PD.Item_Finished_id,PD.orderid,PM.remarks,Cn.Companyid,isnull(PD.Bonus,0) as Bonus,isnull(PD.BonusAmt,0) as BonusAmt 
                    From CarpetNumber CN,Process_Stock_Detail PSD,Process_Issue_Master_" + Hn_ProcessId.Value + " PM,Process_Issue_Detail_" + Hn_ProcessId.Value + @" PD 
                       Where CN.StockNo=PSD.StockNo And PSD.IssueDetailId=PD.Issue_Detail_Id And PM.IssueOrderId=PD.IssueOrderId And PSD.ReceiveDetailId=0 And CN.TStockNo='" + TxtStockNo.Text + @"' And 
                       PSD.ToProcessId=" + Hn_ProcessId.Value + "  And PM.CompanyId=" + ddCompName.SelectedValue;
            if (EmpId != "")
            {
                Str = Str + " And PM.IssueOrderId in(select IssueOrderId From Employee_ProcessOrderNo Where ProcessId=" + Hn_ProcessId.Value + " And EmpId in(" + EmpId + "))";
            }
            ds3 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
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
                    if (DDUnit.Items.FindByValue(ds3.Tables[0].Rows[0]["UnitId"].ToString()) != null)
                    {
                        DDUnit.SelectedValue = ds3.Tables[0].Rows[0]["UnitId"].ToString();
                    }
                   
                    int ISSUEID = Convert.ToInt32(ds3.Tables[0].Rows[0]["issueorderid"]);
                    int issuedetailid = Convert.ToInt32(ds3.Tables[0].Rows[0]["issue_detail_id"]);
                    string length = ds3.Tables[0].Rows[0]["length"].ToString();
                    string width = ds3.Tables[0].Rows[0]["width"].ToString();
                    double area = Convert.ToDouble(ds3.Tables[0].Rows[0]["area"]);
                    double rate = Convert.ToDouble(ds3.Tables[0].Rows[0]["rate"]);
                    double amount = Convert.ToDouble(ds3.Tables[0].Rows[0]["amount"]);
                    double Bonus = Convert.ToDouble(ds3.Tables[0].Rows[0]["Bonus"]);
                    double BonusAmt = Convert.ToDouble(ds3.Tables[0].Rows[0]["BonusAmt"]);

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
                    _arrpara[24] = new SqlParameter("@StockValue", SqlDbType.VarChar, 8000);
                    _arrpara[25] = new SqlParameter("@ProcessId", SqlDbType.Int);
                    _arrpara[26] = new SqlParameter("@Mastercompanyid", SqlDbType.Int);
                    _arrpara[27] = new SqlParameter("@QualityType", SqlDbType.Int);
                    _arrpara[28] = new SqlParameter("@StockNoRemarks", SqlDbType.VarChar, 200);
                    _arrpara[29] = new SqlParameter("@ActualWidth", SqlDbType.VarChar, 200);
                    _arrpara[30] = new SqlParameter("@ActualLength", SqlDbType.VarChar, 200);
                    _arrpara[31] = new SqlParameter("@MSG", SqlDbType.VarChar, 250);
                    _arrpara[32] = new SqlParameter("@PartyChallanNo", SqlDbType.VarChar, 50);
                    _arrpara[33] = new SqlParameter("@Bonus", SqlDbType.Float);
                    _arrpara[34] = new SqlParameter("@BonusAmt", SqlDbType.Float);
                    _arrpara[35] = new SqlParameter("@QAPersonname", SqlDbType.VarChar,50);
                    _arrpara[36] = new SqlParameter("@DyingLotNo", SqlDbType.VarChar, 100);

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

                    if(Session["VarCompanyNo"].ToString()=="43")
                    {
                        _arrpara[7].Value = TxtRemarks.Text;
                    }
                    else
                    {
                        _arrpara[7].Value = remarks;
                    }                    
                    _arrpara[8].Direction = ParameterDirection.InputOutput;
                    _arrpara[8].Value = 0;
                    _arrpara[9].Value = hn_finished.Value;
                    _arrpara[10].Value = length;
                    _arrpara[11].Value = width;
                    _arrpara[12].Value = area;
                    _arrpara[13].Value = rate;
                    _arrpara[14].Value = amount;
                    _arrpara[15].Value = ds3.Tables[0].Rows.Count;

                    if (Session["VarCompanyNo"].ToString() == "16" || Session["VarcompanyNo"].ToString() == "28")
                    {
                        if (ddprocess.SelectedItem.Text.ToUpper() == "WASHING BY WEIGHT")
                        {
                            _arrpara[16].Value = WashingByWeight;
                        }
                        else
                        {
                            _arrpara[16].Value = txtweight.Text == "" ? "0" : txtweight.Text;
                        }
                    }
                    else
                    {
                        _arrpara[16].Value = txtweight.Text == "" ? "0" : txtweight.Text;
                    }

                    _arrpara[17].Value = 0;
                    _arrpara[18].Value = 0;
                    _arrpara[19].Value = ISSUEID;
                    _arrpara[20].Value = issuedetailid;
                    _arrpara[21].Value = hnorderid.Value;
                    _arrpara[22].Value = 0;
                    _arrpara[23].Value = DDcaltype.SelectedValue;
                    _arrpara[24].Value = hnstockno.Value;
                    _arrpara[25].Value = Hn_ProcessId.Value;// ddprocess.SelectedValue;
                    _arrpara[26].Value = Session["varcompanyid"].ToString();
                    _arrpara[27].Value = ddStockQualityType.SelectedValue;
                    _arrpara[28].Value = txtstocknoremarks.Text.Trim();
                    _arrpara[29].Value = TxtWidth.Text;
                    _arrpara[30].Value = TxtLength.Text;
                    _arrpara[31].Direction = ParameterDirection.InputOutput;
                    _arrpara[32].Value = txtPartyChallanNo.Text;

                    if (Session["VarCompanyNo"].ToString() == "42")
                    {
                        _arrpara[33].Value =Bonus;
                        _arrpara[34].Value = BonusAmt;
                    }
                    else
                    {
                        _arrpara[33].Value = 0;
                        _arrpara[34].Value = 0;
                    }
                    
                    if (DDQaname.Items.Count > 0)
                    {
                        _arrpara[35].Value = (DDQaname.SelectedIndex > 0 ? DDQaname.SelectedItem.Text : "");
                    }
                    else
                    {
                        _arrpara[35].Value = "";
                    }
                    _arrpara[36].Value = TdDyingLotNo.Visible == false ? "" : TxtDyingLotNo.Text;

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_NextProcessReceiveForOther]", _arrpara);
                    ViewState["recid"] = _arrpara[0].Value.ToString();
                    TxtChallanNo.Text = _arrpara[5].Value.ToString();
                }
            }
            else
            {
                Str = SqlHelper.ExecuteScalar(Tran, CommandType.Text, @"Select (select * from [dbo].[Get_Folio_EMployee](PIM.IssueOrderID,CurrentProstatus,PID.Issue_Detail_ID)) +'  /  '+replace(convert(varchar(11),AssignDate,106), ' ','-') EmpInFormation 
                    From Process_Stock_detail PSD,CarpetNumber CN,Process_Issue_Master_" + Hn_ProcessId.Value + " PIM,Process_Issue_Detail_" + Hn_ProcessId.Value + @" PID
                    Where PSD.StockNo=CN.StockNo And ToProcessID=" + Hn_ProcessId.Value + @" And PSD.IssueDetailId=PID.Issue_Detail_Id And PIM.IssueOrderId=PID.IssueOrderId  And CN.TStockNo='" + TxtStockNo.Text + "' And PIM.CompanyId=" + ddCompName.SelectedValue + "").ToString();

                LblErrorMessage.Text = Str;
                Tran.Commit();
                return;
            }
            TxtStockNo.Text = "";

            Tran.Commit();
            if (_arrpara[31].Value.ToString() != "")
            {
                LblErrorMessage.Text = _arrpara[31].Value.ToString();
            }
            else
            {
                LblErrorMessage.Text = "Data saved successfully...";
                FillData_Dgdetail();
                TxtLength.Text = "";
                TxtWidth.Text = "";
                if (FlagType == 2)
                {
                    TxtStockNo.Focus();
                }
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
            string sqlstr = "";
            sqlstr = @"Select process_rec_Detail_Id,ICM.Category_Name Category,IM.Item_Name Item,IPM.QDCS  Description,Width,Length,
                   Width + 'x' + Length Size,Qty,Rate,Area,Amount,(Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](process_rec_Detail_Id," + ddprocess.SelectedValue + @",Issue_Detail_Id)) StockNo
                   ,isnull(PD.Bonus,0) as Bonus,Isnull(PD.BonusAmt,0) as BonusAmt
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
    protected void SaveScanCarpetNumber()
    {
        if (ddprocess.SelectedIndex <= 0)
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Select Process Name.";
            return;
        }
        savedetail();
        txtweight.Text = "";
        //Fill_StockNoNew();
    }

    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "27" && Session["usertype"].ToString() != "1")
        {
            btnclickflag = "";
            btnclickflag = "BtnSaveScanCarpetNumber";
            Popup(true);
            txtpwd.Focus();
        }
        else
        {
            SaveScanCarpetNumber();
        }
    }
    protected void savedetail()
    {
        LblErrorMessage.Text = "";
        LblErrorMessage.Visible = true;
        string StrEmpid = "";
        if (lstWeaverName.Items.Count == 0)
        {
            //LblErrorMessage.Text = "Plz Enter ID No...";
            //return;
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
            SqlParameter[] array = new SqlParameter[8];
            array[0] = new SqlParameter("@TStockNo", SqlDbType.VarChar, 100);
            array[1] = new SqlParameter("@StockNo", SqlDbType.Int);
            array[2] = new SqlParameter("@EmpId", SqlDbType.VarChar, 200);
            array[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            array[4] = new SqlParameter("@ProcessId", SqlDbType.Int);
            array[5] = new SqlParameter("@CompanyId", SqlDbType.Int);
            array[6] = new SqlParameter("@RecProcessid", SqlDbType.Int);
            array[7] = new SqlParameter("@ReceiveDate", SqlDbType.DateTime);

            array[0].Value = TxtStockNo.Text;
            array[1].Direction = ParameterDirection.Output;
            array[2].Value = StrEmpid;
            array[3].Direction = ParameterDirection.Output;
            array[4].Direction = ParameterDirection.Output;
            array[5].Direction = ParameterDirection.Output;
            array[6].Value = ddprocess.SelectedValue;
            array[7].Value = TxtreceiveDate.Text;

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "PRO_CHECKEMPLOYEE_STOCKNOOTHER", array);

            LblErrorMessage.Text = array[3].Value.ToString();
            if (LblErrorMessage.Text == "")
            {
                if (Convert.ToInt16(array[5].Value) == Convert.ToInt16(Session["CurrentWorkingCompanyID"]))
                {
                    hnstockno.Value = array[1].Value.ToString();
                    Hn_ProcessId.Value = array[4].Value.ToString();
                    ddCompName.SelectedValue = array[5].Value.ToString();
                    hnrate1.Value = "";
                    save_carpet_wise(2, StrEmpid);
                    TxtStockNo.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('This stock no not belongs to in this company');", true);
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
        try
        {
            string str = @"select Ci.CompanyId,BM.BranchName CompanyName,BM.BranchAddress CompAddr1,'' CompAddr2, '' CompAddr3,BM.PhoneNo CompTel,'' CompFax,CI.GSTNo 
                                ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' as Address3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PID.issueorderid
                                ,PIM.Receivedate,PIS.ReqByDate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") as Job,
                                Vf.CATEGORY_NAME,Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
                                PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
                                (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + ddprocess.SelectedValue + @",PID.issue_detail_id)) TStockNo,PID.Item_Finished_Id
                                ,case when " + Session["varcompanyId"].ToString() + @"=27 then DBO.F_GetFolioNoByOrderIdItemFinishedId(PID.ITEM_FINISHED_ID,PID.issueorderid," + ddprocess.SelectedValue + @") else '''' end as FolioNo
                                ,ISNULL(PID.stockNoRemarks,'') as StockNoRemarks,ISNULL(PID.ActualWidth,'') as ActualWidth,ISNULL(PID.ActualLength,'') as ActualLength,
                                isnull(PID.Weight,0) as Weight,isnull(PIM.PartyChallanNo,'') as PartyChallanNo," + Session["varcompanyId"].ToString() + @" as MasterCompanyId
                                ,isnull(NU.UserName,'') as UserName,(Select Distinct OM.CustomerOrderNo+',' from PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PID1(NoLock) JOIN  OrderMaster OM(NoLock) ON PID1.OrderID=OM.OrderID 
                            Where PID.IssueOrderID=PID1.IssueOrderId and PID.ITEM_FINISHED_ID=PID1.Item_Finished_Id For XML PATH('')) as CustomerOrderNo,
                        (Select Distinct CustIn.CustomerCode+',' from PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PID2(NoLock) JOIN  OrderMaster OM2(NoLock) ON OM2.OrderID = PID2.OrderID 
                            JOIN CustomerInfo CustIn(NoLock) ON OM2.CustomerId=CustIn.CustomerId  Where PID2.IssueOrderId=PID.IssueOrderID For XML PATH('')) as CustomerCode,pim.Remarks,
                                Case When PIM.CALTYPE=0 Then 'Area Wise' Else 'Pcs Wise' End as CalType
                                From PROCESS_Receive_MASTER_" + ddprocess.SelectedValue + @" PIM(NoLock) 
                                Join PROCESS_Receive_DETAIL_" + ddprocess.SelectedValue + @" PID(NoLock) on PIM.Process_Rec_Id=PID.Process_Rec_Id
                        Join PROCESS_ISSUE_DETAIL_" + ddprocess.SelectedValue + @" PIS(NoLock) on PIS.IssueOrderId=PID.IssueOrderId AND PIS.Issue_Detail_Id=PID.Issue_Detail_Id
                                Join BranchMaster BM(Nolock) ON BM.ID = PIM.BranchID 
                                join CompanyInfo CI(NoLock) on PIM.Companyid=CI.CompanyId
                                cross apply(select * From dbo.F_GetJobReceiveEmployeeDetail(" + ddprocess.SelectedValue + @",PIM.Process_rec_id)) EI
                                inner join V_FinishedItemDetail vf(NoLock) on PID.Item_finished_id=vf.ITEM_FINISHED_ID
                                JOIN NewUserDetail NU(NoLock) ON PIM.UserId=NU.UserId
                                 Where PIM.Process_rec_id=" + ViewState["recid"] + " order by Process_rec_detail_id";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkForSummary.Checked == true)
                {
                    switch (Session["varCompanyId"].ToString())
                    {
                        case "27":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReportAntique.rpt";
                            break;
                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReport_barcode.rpt";
                            break;
                        case "44":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReportagni.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2SummaryReport.rpt";
                            break;
                    }
                }
                else if (ChkForActualSize.Checked == true)
                {
                    switch (Session["VarCompanyId"].ToString())
                    {

                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport_barcode.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport.rpt";
                            break;
                    }

                    // Session["rptFileName"] = "~\\Reports\\RptNextReceiveNewActualSizeReport.rpt";
                }
                else
                {
                    switch (Session["VarCompanyId"].ToString())
                    {
                        case "27":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2Antique.rpt";
                            break;
                        case "16":
                        case "28":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2_barcode.rpt";
                            break;
                        case "44":
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2_agni.rpt";
                            break;
                        default:
                            Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2.rpt";
                            break;
                    }
                }
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_nextForOther.aspx");
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddquality_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref ddldesig, "select Distinct vf.designId,vf.designName From V_FinishedItemDetail vf where vf.QualityId=" + ddquality.SelectedValue + @" and vf.designid<>0 order by vf.designName", true, "--Plz select--");
    }
    protected void DGDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGDetail.Columns.Count; i++)
            {
                if (DGDetail.Columns[i].HeaderText == "Bonus")
                {
                    if (Convert.ToInt32(Session["varcompanyId"]) == 42)
                    {
                        DGDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
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
            DataSet Ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, "Select PSD.stockno,PD.IssueOrderId from Process_Stock_Detail PSD,CarpetNumber CN,PROCESS_RECEIVE_DETAIL_" + lblprocessId.Text + @" PD 
            Where PSD.StockNo=CN.StockNo And PSD.ReceiveDetailId=PD.Process_Rec_Detail_Id And PSD.IssueDetailId=PD.Issue_Detail_Id And ReceiveDetailId=" + VarProcess_Receive_Detail_Id + " And ToProcessId=" + lblprocessId.Text + " And CurrentProStatus=" + lblprocessId.Text + " And IssRecStatus=0 And CN.CompanyId=" + ddCompName.SelectedValue + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
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
                //********************* fill_DetailGride();
                FillData_Dgdetail();

                Fill_StockNoNew();

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
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChange();
    }
    private void ChallanNoSelectedChange()
    {
        ViewState["recid"] = DDChallanNo.SelectedValue;
        string Str = "Select Process_Rec_Id,EmpId,Replace(convert(varchar(11),ReceiveDate,106), ' ','-') ReceiveDate,UnitId,Remarks From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + "(Nolock) Where Process_Rec_Id=" + DDChallanNo.SelectedValue + " And CompanyId=" + ddCompName.SelectedValue;
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

        if (Session["VarCompanyNo"].ToString() == "43")
        {
            txtTotalPcsNew.Text = varTotalPcs.ToString();
        }
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
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameterJobWiseOther", param);
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
                                    if (grow.Cells[k].Controls.Count > 1)
                                    {
                                        TextBox txt = grow.Cells[k].Controls[1] as TextBox;
                                        if (txt != null)
                                        {
                                            txt.Text = ds.Tables[2].Rows[j]["Reason"].ToString();
                                        }
                                    }
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
        #region
        //        string SName = "";
        //        string QCValue = "";
        //        string qry = "";
        //        DataSet ds = new DataSet();

        //        qry = @"Select ICM.Category_Name,PD.Qty*PD.Area Area,PD.Qty,PD.Weight,CI.CompanyName,CI.CompAddr1,CI.CompAddr2,IM.Item_Name,IPM.QDCS + Space(5) + Width+'x'+Length Description,
        //                PM.ReceiveDate,PD.Length,PD.Width,PD.Rate,PD.Amount,PD.IssueOrderid,EI.EmpName,EI.Address,U.UnitName,(Select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") ShortName,
        //                PM.UnitId,PM.ChallanNo,'' StockNo,PM.Process_Rec_Id,PD.Process_Rec_Detail_Id
        //                From PROCESS_RECEIVE_MASTER_" + ddprocess.SelectedValue + " PM,PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + @" PD,ViewFindFinishedidItemidQDCSS IPM,Item_Master IM,ITEM_CATEGORY_MASTER ICM,CompanyInfo CI,
        //                EmpInfo EI,Unit U Where PM.Process_Rec_Id=PD.Process_Rec_Id And PD.Item_Finished_id=IPM.Finishedid And IM.Item_Id=IPM.Item_Id And 
        //                PM.Companyid=CI.CompanyId And PM.EmpId=EI.EmpId And PM.UnitId=U.UnitId And IM.Category_Id=ICM.Category_Id And Isnull(PD.QualityType,0)<>3 And PM.Process_Rec_Id=" + ViewState["recid"];
        //        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, qry);
        //        DataTable mytable = new DataTable();
        //        mytable.Columns.Add("PrtID", typeof(int));
        //        mytable.Columns.Add("SName", typeof(string));
        //        mytable.Columns.Add("QCValue", typeof(string));

        //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //        {
        //            string str = @"Select SName, Case when QCValue='1' then 'YES' else 'NO' END QCValue from QCdetail QCD inner join QCMaster QCM ON 
        //                         QCD.QCMasterID=QCM.ID Inner Join QCParameter QCP ON QCM.ParaID=QCP.ParaID
        //                         Where RefName= 'PROCESS_RECEIVE_DETAIL_" + ddprocess.SelectedValue + "' And ProcessId=" + ddprocess.SelectedValue + " And QCD.RecieveID=" + ViewState["recid"] + " And QCD.RecieveDetailID=" + ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"];
        //            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //            SqlDataAdapter sda = new SqlDataAdapter(str, con);
        //            DataTable dt = new DataTable();
        //            sda.Fill(dt);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                if (SName == "" && QCValue == "")
        //                {
        //                    SName = dt.Rows[i]["SName"].ToString();
        //                    QCValue = dt.Rows[i]["QCValue"].ToString();
        //                }
        //                else
        //                {
        //                    SName = SName + ' ' + dt.Rows[i]["SName"].ToString();
        //                    QCValue = QCValue + ' ' + dt.Rows[i]["QCValue"].ToString();
        //                }
        //            }
        //            mytable.Rows.Add(ds.Tables[0].Rows[j]["Process_Rec_Detail_Id"], SName, QCValue);
        //            SName = "";
        //            QCValue = "";
        //        }
        //        ds.Tables.Add(mytable);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            Session["rptFileName"] = "Reports/NextProcessReceiveQC.rpt";
        //            Session["dsFileName"] = "~\\ReportSchema\\NextProcessReceiveQC.xsd";
        //            Session["GetDataset"] = ds;
        //            StringBuilder stb = new StringBuilder();
        //            stb.Append("<script>");
        //            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
        //        }
        #endregion
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

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "[Pro_FillGridDetail_NextReceiveForOther]", _array);
            //Table 0 For DGDetail And table 1 For dgstock
            if (ds.Tables[0].Rows.Count > 0)
            {
                DGDetail.DataSource = ds.Tables[0];
                DGDetail.DataBind();

                TxtTotalPcs.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                TxtArea.Text = ds.Tables[0].Compute("Sum(Area)", "").ToString();
                TxtAmount.Text = ds.Tables[0].Compute("Sum(Amount)", "").ToString();

                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    txtTotalPcsNew.Text = ds.Tables[0].Compute("Sum(Qty)", "").ToString();
                }
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
       // SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        try
        {
            if (txtWeaverIdNo.Text != "")
            {

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where EMpid='" + txtgetvalue.Text + "'");
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
            //con.Dispose();
            //con.Close();
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
            if (DGStockDetail.Rows.Count > 0)
            {
                TDButtonsavegrid.Visible = true;
            }
            else
            {
                TDButtonsavegrid.Visible = false;
            }
        }
    }
    protected void Fill_StockNoNew()
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
            //SqlParameter[] param = new SqlParameter[9];

            //param[0] = new SqlParameter("@Unitsid", ddUnits.SelectedIndex > 0 ? ddUnits.SelectedValue : "0");
            //param[1] = new SqlParameter("@Jobid", ddprocess.SelectedIndex > 0 ? ddprocess.SelectedValue : "0");
            //param[2] = new SqlParameter("@QualityId", ddquality.SelectedIndex > 0 ? ddquality.SelectedValue : "0");
            //param[3] = new SqlParameter("@DesignId", ddldesig.SelectedIndex > 0 ? ddldesig.SelectedValue : "0");
            //param[4] = new SqlParameter("@colorid", ddcolour.SelectedIndex > 0 ? ddcolour.SelectedValue : "0");
            //param[5] = new SqlParameter("@sizeid", ddsize.SelectedIndex > 0 ? ddsize.SelectedValue : "0");
            //param[6] = new SqlParameter("@EmployeeId", Strempid);
            //param[7] = new SqlParameter("@CompanyId", Session["CurrentWorkingCompanyID"]);
            
            //if (TBDDIssueNo.Visible == true && DDIssueNo.SelectedIndex > 0)
            //{
            //    param[8] = new SqlParameter("@IssueOrderID", DDIssueNo.SelectedValue);
            //}
            //else
            //{
            //    param[8] = new SqlParameter("@IssueOrderID", 0);
            //}
            
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Pro_GetPendingJobStockNoNew", param);

            SqlCommand cmd = new SqlCommand("Pro_GetPendingJobStockNoNew", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@Unitsid", ddUnits.SelectedIndex > 0 ? ddUnits.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@Jobid", ddprocess.SelectedIndex > 0 ? ddprocess.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@QualityId", ddquality.SelectedIndex > 0 ? ddquality.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@DesignId", ddldesig.SelectedIndex > 0 ? ddldesig.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@colorid", ddcolour.SelectedIndex > 0 ? ddcolour.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@sizeid", ddsize.SelectedIndex > 0 ? ddsize.SelectedValue : "0");
            cmd.Parameters.AddWithValue("@EmployeeId", Strempid);
            cmd.Parameters.AddWithValue("@CompanyId", Session["CurrentWorkingCompanyID"]);

            if (TBDDIssueNo.Visible == true && DDIssueNo.SelectedIndex > 0)
            {
                cmd.Parameters.AddWithValue("@IssueOrderID", DDIssueNo.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@IssueOrderID", 0);
            }

            DataSet ds = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            ad.Fill(ds);
            //*************

            con.Close();
            con.Dispose();

            DGStockDetail.DataSource = ds;
            DGStockDetail.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttotalpcsgrid.Text = Convert.ToString(ds.Tables[0].Compute("Count(TstockNo)", ""));
                TDButtonsavegrid.Visible = true;
            }
            else
            {
                txttotalpcsgrid.Text = "";
                TDButtonsavegrid.Visible = false;
            }
        }
    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        ShowDetailClick();
    }
    private void ShowDetailClick()
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
        txtWeaverIdNo_AutoCompleteExtender.ContextKey = ddprocess.SelectedValue;

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
                TRWeight.Visible = false;
                txtweight.Text = "";
                break;
        }
        if (Session["VarcompanyNo"].ToString() == "16" || Session["VarcompanyNo"].ToString() == "28" || Session["VarcompanyNo"].ToString() == "44")
        {
            //string sqlstr = "";
            //sqlstr = @"Select * From Finishing_Process_Customer_L_W_Wt Where MasterCompanyID = " + Session["VarcompanyNo"] + " And ProcessID = " + ddprocess.SelectedValue;
            //DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sqlstr);
            //if (DS.Tables[0].Rows.Count > 0)
            //{
            TRWeight.Visible = true;
            txtweight.Text = "";
            //}
            //else
            //{
            //    TRWeight.Visible = false;
            //    txtweight.Text = "";
            //}

            //switch (ddprocess.SelectedItem.Text.ToUpper())
            //{
            //    case "WASHING BY WEIGHT":
            TDemployee.Visible = true;
            TBDDIssueNo.Visible = true;
            //    break;
            //default:
            //    TDemployee.Visible = false;
            //    TDButtonsavegrid.Visible = false;
            //    TBDDIssueNo.Visible = false;
            //    break;
            //}
            if (ddprocess.SelectedItem.Text.ToUpper() == "TUFTING")
            {
                BtnGetIssueNo.Visible = false;
                btnShowDetail.Visible = false;
            }
            else
            {
                BtnGetIssueNo.Visible = true;
                btnShowDetail.Visible = true;
            }
        }
        FillQAName();
    }
    private void FillQAName()
    {
        UtilityModule.ConditionalComboFill(ref ddldesig, @"Select EI.EmpId, EI.EmpName 
            From Empinfo EI(Nolock) 
            JOIN Department D(Nolock) ON EI.Departmentid = D.DepartmentId And EI.Blacklist = 0 And D.DepartmentName = 'QC Department' 
            JOIN EmpProcess EP(Nolock) ON EP.EmpId = EI.EmpId And EP.ProcessId = " + ddprocess.SelectedValue + @" 
            Order by EI.EmpName ", true, "--Plz select--");
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
            dtrecord.Columns.Add("Reason", typeof(string));
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
                    if (gvr.Cells[j].Controls.Count > 1)
                    {

                        TextBox txt = gvr.Cells[j].Controls[1] as TextBox;
                        if (txt != null)
                        {
                            dr["Reason"] = txt.Text.Trim();
                        }

                    }
                    dtrecord.Rows.Add(dr);
                }
            }
            //*********
            if (dtrecord.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@processid", ddprocess.SelectedValue);
                param[3] = new SqlParameter("@UNITNAME", ddUnits.SelectedItem.Text);
                param[4] = new SqlParameter("@UserID", Session["varuserid"]);
                //*****
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_SAVEQCJOBWISEOTHER", param);
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
    protected void SaveGridCarpetNumber()
    {
        if (TRWeight.Visible == true && (Session["VarCompanyNo"].ToString() == "16" || Session["VarCompanyNo"].ToString() == "28" || Session["VarCompanyNo"].ToString() == "44"))
        {
            int count = 0;
            foreach (GridViewRow row in this.DGStockDetail.Rows)
            {
                CheckBox chkId = (row.FindControl("Chkboxitem") as CheckBox);
                if (chkId.Checked)
                {
                    count++;
                }
                //this.lblCountedCheckBox.Text = count.ToString();
            }

            if (count > 0)
            {
                WashingByWeight = Math.Round(Convert.ToDecimal(txtweight.Text == "" ? "0" : txtweight.Text) / Convert.ToDecimal(count), 3);
            }
            else
            {
                WashingByWeight = 0;
            }
        }

        //Grid Loop
        for (int i = 0; i < DGStockDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                TxtStockNo.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                savedetail();
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
    }

    protected void btnsavegrid_Click(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "27" && Session["usertype"].ToString() != "1")
        {
            btnclickflag = "";
            btnclickflag = "BtnSaveGridCarpetNumber";
            Popup(true);
            txtpwd.Focus();
        }
        else
        {
            SaveGridCarpetNumber();
        }
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
    protected void btnqcreport_Click(object sender, EventArgs e)
    {

        try
        {
            #region
            //            string str = @"select Ci.CompanyId,Ci.CompanyName,Ci.CompAddr1,CI.CompAddr2,CI.CompAddr3,CI.CompTel,CI.CompFax,CI.GSTNo
            //                                ,EI.Empname,Ei.Empaddress as address,'' as Address2,'' asAddress3,'' as Mobile,Ei.EMPGSTIN as Empgstin,PID.issueorderid
            //                                ,PIM.Receivedate,(select PROCESS_NAME From PROCESS_NAME_MASTER Where PROCESS_NAME_ID=" + ddprocess.SelectedValue + @") as Job,
            //                                Vf.QualityName,Vf.designName,Vf.ColorName,Case When Vf.shapeid=1 Then '' Else Left(vf.shapename,1) End  as Shapename,
            //                                PID.Width+' x ' +PID.Length as Size,PID.qty,PID.Qty*PID.area as Area,PIM.UnitId,PID.Rate,PID.Amount,PID.Process_Rec_Detail_Id,PIM.challanNo,
            //                                (Select * from [dbo].[Get_StockNoNext_Receive_Detail_Wise](PID.process_rec_detail_id," + ddprocess.SelectedValue + @",PID.issue_detail_id)) TStockNo,
            //                                dbo.F_GETQCValueFinishing('" + ddprocess.SelectedItem.Text + "'," + ddprocess.SelectedValue + @",PIM.Process_rec_id,PID.Process_Rec_Detail_Id) as QCVALUE,
            //                                dbo.F_GETQCValueFinishingparaname('" + ddprocess.SelectedItem.Text + "'," + ddprocess.SelectedValue + @",PIM.Process_rec_id) as QCPARAMETER 
            //                                From PROCESS_Receive_MASTER_" + ddprocess.SelectedValue + " PIM inner Join PROCESS_Receive_DETAIL_" + ddprocess.SelectedValue + @" PID on PIM.Process_Rec_Id=PID.Process_Rec_Id
            //                                inner join CompanyInfo CI on PIM.Companyid=CI.CompanyId
            //                                cross apply(select * From dbo.F_GetJobReceiveEmployeeDetail(" + ddprocess.SelectedValue + @",PIM.Process_rec_id)) EI
            //                                inner join V_FinishedItemDetail vf on PID.Item_finished_id=vf.ITEM_FINISHED_ID
            //                                 Where PIM.Process_rec_id=" + ViewState["recid"] + " order by Process_rec_detail_id";

            //            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            #endregion
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Processid", ddprocess.SelectedValue);
            param[1] = new SqlParameter("@Refname", ddprocess.SelectedItem.Text);
            param[2] = new SqlParameter("@processrecid", ViewState["recid"]);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQCREPORTFINISHING", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["rptFileName"] = "~\\Reports\\RptNextReceiveNew2Qc.rpt";
                Session["GetDataset"] = ds;
                Session["dsFileName"] = "~\\ReportSchema\\RptNextReceiveNew2Qc.xsd";
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn3", "alert('No Record Found!');", true); }

        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Process/frm_receive_process_next.aspx");

        }

    }
    protected void GDQC_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 4; i < e.Row.Cells.Count; i++)
            {
                TextBox txt = new TextBox();
                txt.ID = "txt" + i;

                txt.Attributes.Add("runat", "server");
                e.Row.Cells[i].Controls.Add(txt);

            }
        }
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        FillEmployee();
    }
    protected void FillEmployee()
    {
        ViewState["recid"] = 0;
        SqlConnection con = new SqlConnection(ErpGlobalNew.DBCONNECTIONSTRINGNEW);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            if (txtWeaverIdNoscan.Text != "")
            {

                DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select Empid,Empname from empinfo Where empcode='" + txtWeaverIdNoscan.Text + "' and isnull(Empcode,'')<>''");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (lstWeaverName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) == null)
                    {

                        lstWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Empname"].ToString(), ds.Tables[0].Rows[0]["Empid"].ToString()));
                    }

                    txtWeaverIdNoscan.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Employee", "alert('No Weaver found at this ID No..');", true);
                }

                ds.Dispose();

            }
            txtWeaverIdNoscan.Focus();
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
    protected void chkscan_CheckedChanged(object sender, EventArgs e)
    {
        txtWeaverIdNo.Visible = true;
        txtWeaverIdNoscan.Visible = false;
        if (chkscan.Checked == true)
        {
            txtWeaverIdNoscan.Visible = true;
            txtWeaverIdNo.Visible = false;
        }
    }
    protected void TxtreceiveDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["recid"] = "0";
        DGDetail.DataSource = null;
        DGDetail.DataBind();
        TxtChallanNo.Text = "";
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (variable.VarFinishingOrderReceivePassword == txtpwd.Text)
        {
            if (btnclickflag == "BtnSaveGridCarpetNumber")
            {
                SaveGridCarpetNumber();
            }
            if (btnclickflag == "BtnSaveScanCarpetNumber")
            {
                SaveScanCarpetNumber();
            }
            Popup(false);
        }
        else
        {
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = "Please Enter Correct Password..";
        }
    }
    void Popup(bool isDisplay)
    {
        StringBuilder builder = new StringBuilder();
        if (isDisplay)
        {
            builder.Append("<script>");
            builder.Append("ShowPopup();</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", builder.ToString());
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowPopup", builder.ToString(), false);
        }
        else
        {
            builder.Append("<script>");
            builder.Append("HidePopup();</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HidePopup", builder.ToString(), false);
        }
    }
    protected void BtnGetIssueNo_Click(object sender, EventArgs e)
    {
        string Strempid = string.Empty, Wheres = "";
        Strempid = EmployeeId();

        if (ddcattype.SelectedIndex > 0)
        {
            Wheres = " And VF.CATEGORY_ID = " + ddcattype.SelectedValue;
        }
        if (tdquality.Visible == true && ddquality.SelectedIndex > 0)
        {
            if (Wheres == "")
            {
                Wheres = " And VF.QualityId = " + ddquality.SelectedValue;
            }
            else
            {
                Wheres = Wheres + " And VF.QualityId = " + ddquality.SelectedValue;
            }
        }
        if (tddesign.Visible == true && ddldesig.SelectedIndex > 0)
        {
            if (Wheres == "")
            {
                Wheres = " And VF.DesignID = " + ddldesig.SelectedValue;
            }
            else
            {
                Wheres = Wheres + " And VF.DesignID = " + ddldesig.SelectedValue;
            }
        }
        if (tdcolor.Visible == true && ddcolour.SelectedIndex > 0)
        {
            if (Wheres == "")
            {
                Wheres = " And VF.ColorID = " + ddcolour.SelectedValue;
            }
            else
            {
                Wheres = Wheres + " And VF.ColorID = " + ddcolour.SelectedValue;
            }
        }
        if (tdsize.Visible == true && ddsize.SelectedIndex > 0)
        {
            if (Wheres == "")
            {
                Wheres = " And VF.SizeID = " + ddsize.SelectedValue;
            }
            else
            {
                Wheres = Wheres + " And VF.SizeID = " + ddsize.SelectedValue;
            }
        }
        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@ProcessID", SqlDbType.Int);
        param[1] = new SqlParameter("@Units", SqlDbType.Int);
        param[2] = new SqlParameter("@EmpIDS", SqlDbType.VarChar, 500);
        param[3] = new SqlParameter("@WHERE", SqlDbType.VarChar, 500);

        param[0].Value = ddprocess.SelectedValue;
        param[1].Value = ddUnits.SelectedValue;
        param[2].Value = Strempid;
        param[3].Value = Wheres;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETJOBWISEISSUECHALLANNO", param);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "--SELECT--");
    }
    protected void TxtReceiveQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        if (TxtReceiveQty.Text != "")
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtReceiveQty.Text);
            ShowDetailClick();
        }
    }
}