using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Loom_frmProductionReceiveLoomStockWise : System.Web.UI.Page
{
    static string TempProcessRecId = "";
    static int VarConfirmButtonStatus = 1;
    static int VarProcess_Rec_Detail_Id = 0;
    static int VarProcess_Rec_Id = 0;
    static string btnclickflag = "";
    static int ItemFinishedId = 0;
    static string varLength = "";
    static string varWidth = "";
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
                typeof(Masters_Loom_frmProductionReceiveLoomStockWise),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                           
                    select UnitsId,UnitName from Units order by UnitName
                    select Ei.EmpId,EI.EmpName From Empinfo Ei inner join Department D on Ei.Departmentid=D.DepartmentId And EI.Blacklist = 0 And 
                    D.DepartmentName='QC Department' order by Ei.EmpName 
                    Select * From NewUserDetail Where canedit = 1 And UserType = 1 And UserId = " + Session["varuserId"]+@"
                    Select ID, BranchName From BRANCHMASTER BM(nolock) JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDQaname, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");           
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDchkedit.Visible = true;
            }

            if (Session["VarCompanyNo"].ToString() == "21")
            {
                TDstockno.Visible = true;
            }
            else
            {
                if (Session["usertype"].ToString() == "1")
                {
                    TDstockno.Visible = true;
                }
            }
            
            TxtUserType.Text = "0";
            if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
            {
                TxtUserType.Text = "1";
            }
            switch (Session["varcompanyid"].ToString())
            {
                case "14":
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "1000";
                    DGStockDetail.PageSize = 1000;
                    TDstockno.Visible = true;
                    TDbatch.Visible = true;
                    
                    break;
                case "16":
                case "28":
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtReceiveQty.Enabled = true;
                        TxtReceiveQty.Text = "500";
                        DGStockDetail.PageSize = 500;
                    }
                    Tractualwidthlength.Visible = true;
                    TDchkedit.Visible = false;
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        TDchkedit.Visible = true;
                    }
                    TDstockno.Visible = true;
                    LblPcsType.Visible = true;
                    TDPartyChallanNo.Visible = true;
                    
                    break;
                case "27":
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtReceiveQty.Enabled = true;
                        TxtReceiveQty.Text = "500";
                        DGStockDetail.PageSize = 500;
                        TDstockno.Visible = true;
                    }
                    break;
                case "21":
                    TDprodshift.Visible = true;
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "500";
                    DGStockDetail.PageSize = 500;
                    break;
                case "22":
                    DDCarpetGrade.Visible = true;
                    Label34.Visible = true;
                     Label10.Text = "Amt.";
                    Label11.Text = "Remarks";
                    break;
                case "42":
                    TDStockStatus.Visible = true;
                    Tractualwidthlength.Visible = true;
                    break;
                case "43":
                    TDStockStatus.Visible = true;
                    Tractualwidthlength.Visible = true;
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtReceiveQty.Enabled = true;
                        TxtReceiveQty.Text = "200";
                        DGStockDetail.PageSize = 200;
                    }
                    ChkForSummaryReport.Visible = true;
                    TRShowTotalReceivePcs.Visible = true;
                    break;
                case "44":
                    TxtReceiveQty.Enabled = true;
                    break;
                case "45":
                    TxtReceiveQty.Enabled = true;
                    TxtReceiveQty.Text = "500";
                    DGStockDetail.PageSize = 500;
                    TDstockno.Visible = true;
                    TDStockStatus.Visible = true;
                    Tractualwidthlength.Visible = true;
                    break;
                case "46":
                    TDStockStatus.Visible = true;
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtReceiveQty.Enabled = true;
                        TxtReceiveQty.Text = "200";
                        DGStockDetail.PageSize = 200;
                    }
                    break;
                case "247":
                    TDStockStatus.Visible = true;
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtReceiveQty.Enabled = true;
                        TxtReceiveQty.Text = "200";
                        DGStockDetail.PageSize = 200;
                    }
                    break;
                default:
                    break;
            }
            //
            if (variable.VarSHOWMATERIALISSUEDONFOLIO_BAZARFORM == "1")
            {
                TdRawmaterialissued.Visible = true;
            }

            HnWPenalityId.Value = "0";
        }
    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        hnprocessrecid.Value = "0";
        hn100_ISSUEORDERID.Value = "0";
        hn100_PROCESS_REC_ID.Value = "0";
        AutoCompleteExtenderloomno.ContextKey = "0#" + DDcompany.SelectedValue + "#" + DDProdunit.SelectedValue;
        FillLoomNo(sender);
    }
    protected void FillLoomNo(Object sender = null)
    {
        string str;
        if (txteditempcode.Text != "" || txtfolionoedit.Text != "")
        {
            TDLoomno.Visible = true;
            TDLoomNotextbox.Visible = false;
            str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(Pl.LoomNo)=1 Then CONVERT(int,replace(loomno, '.', '')) ELse 999999 End as LoomNo1 
                    from PROCESS_ISSUE_MASTER_1 PM inner join ProductionLoomMaster PL
                    on PM.LoomId=PL.UID Inner join EMployee_Processorderno emp on PM.issueorderid=emp.issueorderid and emp.processid=1  WHere PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue;
            if (txteditempid.Text != "")
            {
                str = str + " and Emp.empid=" + txteditempid.Text + "";
            }
            if (txtfolionoedit.Text != "")
            {
                ////str = str + " and PM.issueorderid='" + txtfolionoedit.Text + "'";

                str = str + " and PM.ChallanNo='" + txtfolionoedit.Text + "'";
            }
            str = str + " order by LoomNo1,Pl.LoomNo";

            UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Plz Select--");
            if (DDLoomNo.Items.Count > 0)
            {
                DDLoomNo.SelectedIndex = 1;
                DDLoomNo_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        else
        {
            TDLoomno.Visible = false;
            TDLoomNotextbox.Visible = true;
        }
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        string str = "";
        //        if (chkedit.Checked == true)
        //        {
        //            //            str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //            //                    where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue + " order by PM.IssueOrderId";

        //            str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //                    where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + " order by PM.IssueOrderId";

        //        }
        //        else
        //        {
        //            //            str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //            //                    where PM.Status='Pending' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;
        //            //            str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //            //                    where  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;
        //            str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //                    where  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;

        //        }
        //     UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");

        string str = "";
        txtloomid.Text = DDLoomNo.SelectedValue;

        str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
            From PROCESS_ISSUE_MASTER_1 PM 
            inner join EMployee_Processorderno EMP on PM.issueorderid=EMp.issueorderid and EMP.processid=1
            inner join LoomstockNo ls on PM.issueorderid=Ls.issueorderid and ls.processid=1
            where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + "";
        if (txteditempcode.Text != "")
        {
            str = str + " and EMP.EMPID=" + txteditempid.Text + "";
        }
        if (txtfolionoedit.Text != "")
        {
            ////str = str + " and PM.Issueorderid=" + txtfolionoedit.Text + "";

            str = str + " and PM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        if (chkedit.Checked == false)
        {
            str = str + " and PM.Status='Pending'";
        }
        str = str + "  order by PM.IssueOrderId";

        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
        if (DDFolioNo.Items.Count > 0)
        {
            DDFolioNo.SelectedIndex = 1;
            DDFolioNo_SelectedIndexChanged(sender, new EventArgs());
        }

    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        ItemFinishedId = 0;
        if (Session["VarCompanyNo"].ToString() != "43")
        {
            hnprocessrecid.Value = "0";
            hn100_ISSUEORDERID.Value = "0";
            hn100_PROCESS_REC_ID.Value = "0";
        }
        if (chkedit.Checked == true)
        {
            str = @"select Distinct PRM.Process_Rec_Id,PRM.ChallanNo+' /'+REPLACE(CONVERT(nvarchar(11),receivedate,106),' ','-') As ChallanNo from Process_receive_master_1 PRM inner join 
                PROCESS_RECEIVE_DETAIL_1 PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                where PRD.IssueOrderId=" + DDFolioNo.SelectedValue + " order by Process_Rec_Id";
            UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
            if (DDreceiveNo.Items.Count > 0)
            {
                
                if (txtEditReceiveNoForCI.Text.Trim() != "")
                {
                    DDreceiveNo.SelectedValue = TempProcessRecId;
                    DDreceiveNo_SelectedIndexChanged(sender, new EventArgs());
                }
                else
                {
                    DDreceiveNo.SelectedIndex = 1;
                    DDreceiveNo_SelectedIndexChanged(sender, new EventArgs());
                }
            }
        }
        fillGrid();
        if (TdRawmaterialissued.Visible == true)
        {
            fillRawMaterialIssued();
        }
        if (TDstockno.Visible == true)
        {
            Fillstockno();
        }

    }
    protected void Fillstockno()
    {
        if (ItemFinishedId == 0)
        {
            if (DG.Rows.Count > 0)
            {
                //for (int i = 0; i < DG.Rows.Count; i++)
                //{
                //    Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                //}
                Label lblitemfinishedid = ((Label)DG.Rows[0].FindControl("lblitemfinishedid"));
                ItemFinishedId = Convert.ToInt32(lblitemfinishedid.Text);
            }
        }

        string str = @"SELECT LS.TSTOCKNO 
        FROM LOOMSTOCKNO LS(Nolock) 
        INNER JOIN PROCESS_ISSUE_MASTER_1 PIM(Nolock) ON LS.ISSUEORDERID=PIM.ISSUEORDERID AND LS.BAZARSTATUS=0 AND PIM.STATUS<>'CANCELED' 
        WHere LS.ProcessID = 1 And Pim.CompanyId=" + DDcompany.SelectedValue + " and PIM.Units=" + DDProdunit.SelectedValue + @" and Pim.loomid=" + txtloomid.Text + " and Pim.issueorderid=" + DDFolioNo.SelectedValue + " and LS.Item_finished_id=" + ItemFinishedId + " order by Ls.stockNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGStockDetail.DataSource = ds.Tables[0];
        DGStockDetail.DataBind();
        txttotalpcsgrid.Text = "0";
        Trsave.Visible = false;
        if (ds.Tables[0].Rows.Count > 0)
        {
            Trsave.Visible = true;
            txttotalpcsgrid.Text = ds.Tables[0].Compute("count(Tstockno)", "").ToString();
        }


    }
    protected void fillRawMaterialIssued()
    {
        string str = @" SELECT REPLACE(CONVERT(nvarchar(11),PM.date,106),' ','-') as Issuedate,VF1.ITEM_NAME+SPACE(2)+VF1.QUALITYNAME as ItemName,Vf1.ShadeColorName,
	                    ROUND(SUM(CASE WHEN PM.TRANTYPE=0 THEN PT.ISSUEQUANTITY ELSE -PT.ISSUEQUANTITY END),3) AS ISSUEDQTY
	                    FROM  PROCESSRAWMASTER PM INNER JOIN PROCESSRAWTRAN PT ON PM.PRMID=PT.PRMID
	                    INNER JOIN V_FINISHEDITEMDETAIL VF1 ON PT.FINISHEDID=VF1.ITEM_FINISHED_ID	                    
	                    WHERE PM.PRORDERID=" + DDFolioNo.SelectedValue + @" AND PM.PROCESSID=1 AND PM.BEAMTYPE=0 And PM.TypeFlag = 0
	                    GROUP  BY VF1.ITEM_NAME,VF1.QUALITYNAME,Vf1.ShadeColorName,PM.date";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            GVRawMaterialIssued.DataSource = ds.Tables[0];
            GVRawMaterialIssued.DataBind();
        }
        else
        {
            GVRawMaterialIssued.DataSource = null;
            GVRawMaterialIssued.DataBind();
        }
    }
    protected void fillGrid()
    {
        SqlParameter[] param = new SqlParameter[4];
        param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@Produnitid", DDProdunit.SelectedValue);
        //param[2] = new SqlParameter("@LoomId", DDLoomNo.SelectedValue);
        param[2] = new SqlParameter("@LoomId", txtloomid.Text);
        param[3] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillProductionDataLoom", param);
        DG.DataSource = ds.Tables[1]; //
        DG.DataBind();
        //********EmpDetail
        listWeaverName.Items.Clear();
        string empids = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                empids = empids + "," + ds.Tables[0].Rows[i]["Empid"].ToString();

                listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Empname"].ToString(), ds.Tables[0].Rows[i]["Empid"].ToString()));
            }
        }
       
        if (hnlastempids.Value == "")
        {
            hnlastempids.Value = empids;
        }
        DGEmployee.DataSource = ds.Tables[0];
        DGEmployee.DataBind();

        //********Check If last Empids is not equal to current empids
        if (hnlastempids.Value != empids)
        {
            hnRejectedGatePassNo.Value = "0";
            hnlastempids.Value = empids;
        }


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //Check LoomNo
        if (txtloomid.Text == "" || txtloomid.Text == "0" || txtloomid.Text == null)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "loom", "alert('Please Enter proper Loom No.');", true);
            return;
        }
        //
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //Sql Table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Item_Finished_Id", typeof(int));
        dtrecords.Columns.Add("Length", typeof(string));
        dtrecords.Columns.Add("Width", typeof(string));
        dtrecords.Columns.Add("Area", typeof(float));
        dtrecords.Columns.Add("Rate", typeof(float));
        dtrecords.Columns.Add("IssueOrderId", typeof(int));
        dtrecords.Columns.Add("Issue_Detail_Id", typeof(int));
        dtrecords.Columns.Add("Orderid", typeof(int));
        dtrecords.Columns.Add("UnitId", typeof(int));
        dtrecords.Columns.Add("CalType", typeof(int));
        dtrecords.Columns.Add("Recqty", typeof(int));
        dtrecords.Columns.Add("Bazarweight", typeof(float));
        dtrecords.Columns.Add("Orderedqty", typeof(int));
        //
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtrecqty = ((TextBox)DG.Rows[i].FindControl("txtrecqty"));
            if (Chkboxitem.Checked == true && Convert.ToInt16(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0)
            {
                Label lblitemfinishedid = (Label)DG.Rows[i].FindControl("lblitemfinishedid");
                Label lbllength = (Label)DG.Rows[i].FindControl("lbllength");
                Label lblwidth = (Label)DG.Rows[i].FindControl("lblwidth");
                Label lblarea = (Label)DG.Rows[i].FindControl("lblarea");
                Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
                Label lblissueorderid = (Label)DG.Rows[i].FindControl("lblissueorderid");
                Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
                Label lblorderid = (Label)DG.Rows[i].FindControl("lblorderid");
                Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
                Label lblcaltype = (Label)DG.Rows[i].FindControl("lblcaltype");
                Label lblorderedqty = (Label)DG.Rows[i].FindControl("lblorderedqty");
                TextBox txtweight = (TextBox)DG.Rows[i].FindControl("txtweight");

                //******Fill Data
                DataRow dr = dtrecords.NewRow();
                dr["Item_Finished_Id"] = lblitemfinishedid.Text;
                dr["Length"] = lbllength.Text;
                dr["Width"] = lblwidth.Text;
                dr["Area"] = lblarea.Text;
                dr["Rate"] = lblrate.Text;
                dr["IssueOrderId"] = lblissueorderid.Text;
                dr["Issue_Detail_Id"] = lblissuedetailid.Text;
                dr["Orderid"] = lblorderid.Text;
                dr["UnitId"] = lblunitid.Text;
                dr["CalType"] = lblcaltype.Text;
                dr["Recqty"] = txtrecqty.Text;
                dr["Bazarweight"] = txtweight.Text == "" ? "0" : txtweight.Text;
                dr["Orderedqty"] = lblorderedqty.Text;
                //
                dtrecords.Rows.Add(dr);
            }
        }
        //***************Employee
        DataTable dtemployee = new DataTable();
        dtemployee.Columns.Add("Empid", typeof(int));
        dtemployee.Columns.Add("Processid", typeof(int));
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            DataRow dr1 = dtemployee.NewRow();
            dr1["empid"] = listWeaverName.Items[i].Value;
            dr1["Processid"] = 1;
            dtemployee.Rows.Add(dr1);
        }
        //*******
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@Process_Rec_id", SqlDbType.Int);
                param[0].Value = 0;
                param[0].Direction = ParameterDirection.Output;
                param[1] = new SqlParameter("@Companyid", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@Productionunit", DDProdunit.SelectedValue);
                //param[3] = new SqlParameter("@Loomid", DDLoomNo.SelectedValue);
                param[3] = new SqlParameter("@Loomid", txtloomid.Text);
                param[4] = new SqlParameter("@Issueorderid", DDFolioNo.SelectedValue);
                param[5] = new SqlParameter("@ReceiveNo", SqlDbType.VarChar, 50);
                param[5].Direction = ParameterDirection.Output;
                param[6] = new SqlParameter("ReceiveDate", txtrecdate.Text);
                param[7] = new SqlParameter("@dtrecords", dtrecords);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@Userid", Session["varuserid"]);
                param[10] = new SqlParameter("@dtemployee", dtemployee);
                param[11] = new SqlParameter("@Checkedby", txtcheckedby.Text);
                param[12] = new SqlParameter("@BranchId", DDBranchName.SelectedValue);
                param[13] = new SqlParameter("@PartyChallanNo", txtPartyChallanNo.Text);
                //**************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProductionLoomReceive", param);

                if (param[8].Value.ToString() != "")
                {
                    Tran.Rollback();
                    lblmessage.Text = param[8].Value.ToString();
                }
                else
                {
                    Tran.Commit();
                    txtreceiveno.Text = param[5].Value.ToString();
                 //   txtBatchChallanNo.Text = param[16].Value.ToString();
                    hnprocessrecid.Value = param[0].Value.ToString();
                    lblmessage.Text = "Data saved successfully...";
                    FillRecDetails();
                    Refreshcontrol();
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }

    }
    protected void Refreshcontrol(object sender = null)
    {
        // txtstockno.Text = "";
        txtwidth.Text = "";
        txtlength.Text = "";
        txtarea.Text = "";
        txtstockweight.Text = "";
        txtpenalityamt.Text = "";
        txtpenalityremarks.Text = "";
        //ddStockQualityType.SelectedValue = "1";
        if (sender != null)
        {
            ddStockQualityType_SelectedIndexChanged(sender, new EventArgs());
        }
        txtcommrate.Text = "";
        txtactuallength.Text = "";
        txtactualwidth.Text = "";
    }
    protected void FillRecDetails()
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionReceiveReport_Loom", array);
        DGRecDetail.DataSource = ds.Tables[0];
        DGRecDetail.DataBind();
        txttotalpcs.Text = "";
        txtTotalPcsNew.Text = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            tdtotalwt.Visible = true;
            txttotalpcs.Text = ds.Tables[0].Compute("Sum(recqty)", "").ToString();
            txtTotalPcsNew.Text = ds.Tables[0].Compute("Sum(recqty)", "").ToString(); ////For Carpet International Show With Carpet Scan TextBox

            if (TDPartyChallanNo.Visible == true)
            {
                txtPartyChallanNo.Text = ds.Tables[0].Rows[0]["PartyChallanNo"].ToString();
            }

            if (Session["varcompanyid"].ToString() == "28" && Convert.ToInt32(Session["usertype"]) > 1)
            {
                DGRecDetail.Columns[14].Visible = false;
                DGRecDetail.Columns[15].Visible = false;
            }
        }
        else
        {
            tdtotalwt.Visible = false;
        }
        //*********CONSUMED DETAIL
        DGconsumedDetails.DataSource = null;
        DGconsumedDetails.DataBind();
        if (ds.Tables[2].Rows.Count > 0)
        {
            DGconsumedDetails.DataSource = ds.Tables[2];
            DGconsumedDetails.DataBind();
        }
        //********EmpDetail

        listWeaverName.Items.Clear();
        if (ds.Tables[1].Rows.Count > 0)
        {
            //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            //{
            //    listWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[i]["Empname"].ToString(), ds.Tables[1].Rows[i]["Empid"].ToString()));
            //}
            DGEmployee.DataSource = ds.Tables[1];
            DGEmployee.DataBind();
        }
        if (chkedit.Checked)
        {
            if (ds.Tables[3].Rows.Count > 0)
            {
                txtBatchChallanNo.Text = ds.Tables[3].Rows[0]["BatchChallanNo"].ToString();
            }
        }
        //
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FORPRODUCTIONRECEIVEREPORT_LOOMREPORT", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyid"].ToString())
            {
                case "16":
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailchampo.rpt";
                    break;
                case "43":
                    if (ChkForSummaryReport.Checked == true)
                    {
                        Session["rptFileName"] = "~\\Reports\\rptProductionreceiveSummaryCarpetInternational.rpt";
                    }
                    else
                    {
                        Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailCarpetInternational.rpt";
                    }                    
                    break;
                case "14":
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetaileastern.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetail.rpt";
                    break;
            }
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptProductionreceivedetail.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }

    protected void GetRejectedGatePassNo()
    {
        hnRejectedGatePassNo.Value = "0";
        string str = @"select Distinct Id,GatePassNo from ProductionReceiveRejectedStock PRRS Where PRRS.Process_Rec_Id='" + DDreceiveNo.SelectedValue + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            hnRejectedGatePassNo.Value = ds.Tables[0].Rows[0]["GatePassNo"].ToString();
        }
        else
        {
            hnRejectedGatePassNo.Value = "0";
        }
    }
    protected void DDreceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprocessrecid.Value = DDreceiveNo.SelectedValue;
        GetRejectedGatePassNo();
        FillRecDetails();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {                   
            TDreceiveNo.Visible = true;
            btnsave.Visible = false;
            TDFolioNotext.Visible = true;          

            switch (Session["varcompanyid"].ToString())
            {               
                case "16":
                case "28":
                    if (Session["usertype"].ToString() == "1")
                    {
                        BtnUpdateRate.Visible = true;
                    }
                    TDEditReceiveNoForCI.Visible = false;
                    break;
                case "43":
                    BtnUpdateRate.Visible = false;
                    BtnUpdateConsumption.Visible = true;
                    TDEditReceiveNoForCI.Visible = true;
                    break;
                default:
                    BtnUpdateRate.Visible = false;
                    BtnUpdateConsumption.Visible = true;
                    TDEditReceiveNoForCI.Visible = false;
                    break;
            }
            

            if (Session["usertype"].ToString() != "1" && variable.VarOnlyPreviewButtonShowOnAllEditForm == "1")
            {
                DGRecDetail.Columns[13].Visible = false;
                DGRecDetail.Columns[14].Visible = false;
                DGRecDetail.Columns[15].Visible = false;
                DGRecDetail.Columns[16].Visible = false;
            }

        }
        else
        {           
            TDEditReceiveNoForCI.Visible = false;
            TDreceiveNo.Visible = false;
            btnsave.Visible = true;
            BtnUpdateRate.Visible = false;
            BtnUpdateConsumption.Visible = false;

        }
        //*****************
        txtloomno.Text = "";
        DDProdunit.SelectedIndex = -1;
        DDLoomNo.SelectedIndex = -1;
        DDFolioNo.SelectedIndex = -1;
        DDreceiveNo.Items.Clear();
        hnprocessrecid.Value = "0";
        hn100_ISSUEORDERID.Value = "0";
        hn100_PROCESS_REC_ID.Value = "0";
    }
    protected void DGRecDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGRecDetail.EditIndex = e.NewEditIndex;
        FillRecDetails();
        DGRecDetail.Rows[e.NewEditIndex].FindControl("txtweight").Focus();
    }
    protected void DGRecDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGRecDetail.EditIndex = -1;
        FillRecDetails();
    }
    protected void DGRecDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblprocessrecid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecid");
            Label lblprocessrecdetailid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecdetailid");
            TextBox txtweight = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtweight");
            Label lblrecqty = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblrecqty");
            //Double weight = Convert.ToDouble(txtweight.Text) / Convert.ToDouble(lblrecqty.Text);
            Double weight = Convert.ToDouble(txtweight.Text);
            TextBox txtrategrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtrategrid");
            TextBox txtcommrategrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtcommrategrid");
            Label lblHrate = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblHrate");
            Label lblHcommrate = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblHcommrate");

            TextBox txtpenalitygrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtpenalitygrid");
            TextBox txtpenalityremarkgrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtpenalityremarkgrid");
            TextBox txtqanamegrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtqanamegrid");

            TextBox txtawidthgrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtawidthgrid");
            TextBox txtalengthgrid = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtalengthgrid");


            SqlCommand cmd = new SqlCommand("PRO_UPDATEBAZARDETAIL_LOOMWISE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@processrecid", lblprocessrecid.Text);
            cmd.Parameters.AddWithValue("@Processrecdetailid", lblprocessrecdetailid.Text);
            cmd.Parameters.AddWithValue("@Weight", weight);
            cmd.Parameters.AddWithValue("@Rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);
            cmd.Parameters.AddWithValue("@Commrate", txtcommrategrid.Text == "" ? "0" : txtcommrategrid.Text);
            cmd.Parameters.AddWithValue("@Hrate", lblHrate.Text);
            cmd.Parameters.AddWithValue("@HCommrate", lblHcommrate.Text);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@mastercompanyid", Session["varcompanyNo"]);
            cmd.Parameters.AddWithValue("@Penality", txtpenalitygrid.Text == "" ? "0" : txtpenalitygrid.Text);
            cmd.Parameters.AddWithValue("@PRemarks", txtpenalityremarkgrid.Text.Trim());
            cmd.Parameters.AddWithValue("@QANAME", txtqanamegrid.Text.Trim());
            cmd.Parameters.AddWithValue("@Actualwidth", txtawidthgrid.Text.Trim());
            cmd.Parameters.AddWithValue("@ActualLength", txtalengthgrid.Text.Trim());

            //SqlParameter[] param = new SqlParameter[10];
            //param[0] = new SqlParameter("@processrecid", lblprocessrecid.Text);
            //param[1] = new SqlParameter("@Processrecdetailid", lblprocessrecdetailid.Text);
            //param[2] = new SqlParameter("@Weight", weight);
            //param[3] = new SqlParameter("@Rate", txtrategrid.Text == "" ? "0" : txtrategrid.Text);
            //param[4] = new SqlParameter("@Commrate", txtcommrategrid.Text == "" ? "0" : txtcommrategrid.Text);
            //param[5] = new SqlParameter("@Hrate", lblHrate.Text);
            //param[6] = new SqlParameter("@HCommrate", lblHcommrate.Text);
            //param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            //param[7].Direction = ParameterDirection.Output;
            //param[8] = new SqlParameter("@userid", Session["varuserid"]);
            //param[9] = new SqlParameter("@mastercompanyid", Session["varcompanyNo"]);

            // SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEBAZARDETAIL_LOOMWISE", param);
            cmd.ExecuteNonQuery();
            //lblmessage.Text = param[7].Value.ToString();
            lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
            Tran.Commit();
            DGRecDetail.EditIndex = -1;
            FillRecDetails();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DeleteRow(int VarProcess_Rec_Detail_Id, int VarProcess_Rec_Id)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //Label lblprocessrecid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecid");
            //Label lblprocessrecdetailid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecdetailid");
            ////**********

            SqlCommand cmd = new SqlCommand("PRO_DELETEPRODUCTIONRECEIVEDETAILLOOMSTOCKNOWISE", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@Process_Rec_id", VarProcess_Rec_Id);
            cmd.Parameters.AddWithValue("@ReceiveDetailID", VarProcess_Rec_Detail_Id);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@Mastercompanyid", Session["varcompanyNo"]);
            cmd.Parameters.Add("@hnprocessrecid", SqlDbType.Int);
            cmd.Parameters["@hnprocessrecid"].Direction = ParameterDirection.InputOutput;

            cmd.ExecuteNonQuery();
            lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
            Tran.Commit();
            if (cmd.Parameters["@Process_Rec_id"].Value.ToString() == "0")
            {
                hnprocessrecid.Value = "0";
                hn100_PROCESS_REC_ID.Value = "0";
                hn100_ISSUEORDERID.Value = "0";
            }

            #region
            //SqlParameter[] param = new SqlParameter[6];
            //param[0] = new SqlParameter("@Process_Rec_id", lblprocessrecid.Text);
            //param[1] = new SqlParameter("@ReceiveDetailID", lblprocessrecdetailid.Text);
            //param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            //param[2].Direction = ParameterDirection.Output;
            //param[3] = new SqlParameter("@userid", Session["varuserid"]);
            //param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            //param[5] = new SqlParameter("@hnprocessrecid", SqlDbType.Int);
            //param[5].Direction = ParameterDirection.InputOutput;
            ////***
            //SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEPRODUCTIONRECEIVEDETAILLOOMSTOCKNOWISE", param);
            //lblmessage.Text = param[2].Value.ToString();
            //Tran.Commit();
            //if (param[0].Value.ToString() == "0")
            //{
            //    hnprocessrecid.Value = "0";
            //    hn100_PROCESS_REC_ID.Value = "0";
            //    hn100_ISSUEORDERID.Value = "0";
            //}
            #endregion

            FillRecDetails();
            if (Session["VarCompanyNo"].ToString() == "43")
            {
                TableIssueDetail.Visible = false;                
            }
            else
            {
                TableIssueDetail.Visible = true;
                fillGrid();
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGRecDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblprocessrecid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecid");
        Label lblprocessrecdetailid = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblprocessrecdetailid");

        VarProcess_Rec_Detail_Id =Convert.ToInt32(lblprocessrecdetailid.Text);
        VarProcess_Rec_Id = Convert.ToInt32(lblprocessrecid.Text);
        if (Session["VarCompanyNo"].ToString() == "21")
        {           
            btnclickflag = "";

            btnclickflag = "BtnDeleteRow";
            txtpwd.Focus();
            Popup(true);
        }
        else
        {
            DeleteRow(VarProcess_Rec_Detail_Id, VarProcess_Rec_Id);
        }  
        
    }
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtrategrid = (TextBox)e.Row.FindControl("txtrategrid");
            TextBox txtcommrategrid = (TextBox)e.Row.FindControl("txtcommrategrid");
            Label lblDefectStatus = (Label)e.Row.FindControl("lblDefectStatus");
            LinkButton lnkRemoveDefect = (LinkButton)e.Row.FindControl("lnkRemoveQccheck");

            if (Convert.ToInt32(lblDefectStatus.Text) > 0)
            {
                lnkRemoveDefect.Visible = true;
            }
            else
            {
                lnkRemoveDefect.Visible = false;
            }

            switch (Session["varcompanyId"].ToString())
            {
                case "22":
                    if (txtrategrid != null)
                    {
                        txtrategrid.Enabled = false;
                    }
                    //if (txtcommrategrid != null)
                    //{
                    //    txtcommrategrid.Enabled = false;
                    //}

                    break;
                default:
                    break;
            }
            //*****************COLUMN VISIBLE 
            for (int i = 0; i < DGRecDetail.Columns.Count; i++)
            {

                if (Session["varcompanyId"].ToString() != "16")
                {
                    if (DGRecDetail.Columns[i].HeaderText.ToUpper() == "ACTUAL WIDTH" || DGRecDetail.Columns[i].HeaderText.ToUpper() == "ACTUAL LENGTH")
                    {
                        DGRecDetail.Columns[i].Visible = false;
                    }
                }
                if (Session["varcompanyId"].ToString() == "22")
                {
                    if (DGRecDetail.Columns[i].HeaderText.ToUpper() == "GRADE")
                    {
                        DGRecDetail.Columns[i].Visible = true;
                    }
                }
                else
                    if (DGRecDetail.Columns[i].HeaderText.ToUpper() == "GRADE")
                    {
                        DGRecDetail.Columns[i].Visible = false;
                    }

                if (Session["varcompanyId"].ToString() == "43" || Session["varcompanyId"].ToString() == "46")
                {
                    if (DGRecDetail.Columns[i].HeaderText.ToUpper() == "ADD PENALITY")
                    {
                        DGRecDetail.Columns[i].Visible = true;
                    }
                }

            }
            //*****************
        }
       
    }

    protected void lnkqcparameter_Click(object sender, EventArgs e)
    {
        Modalpopupext.Show();
        ViewState["qcdetail"] = null;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label Processrecid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecid");
            Label processrecdetailid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecdetailid");

            //**********get QC parameter
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Processrecid", Processrecid.Text);
            param[1] = new SqlParameter("@Processrecdetailid", processrecdetailid.Text);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameter", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
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
                TextBox txtreason = e.Row.Cells[i].Controls[1] as TextBox;
                chk.Enabled = true;
                chk.Checked = true;
                if (txtreason != null)
                {
                    switch (Session["varcompanyid"].ToString())
                    {
                        case "14":
                            txtreason.Visible = false;
                            break;
                        default:
                            break;
                    }
                }
                //checked box

                //
            }

        }


    }
    protected void btnqcsave_Click(object sender, EventArgs e)
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
                        //CPH_Form_GDQC_txt4_0
                        //CPH_Form_GDQC_txt4_0
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
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@UserID", Session["varuserId"]);

                //*****
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_saveQc", param);
                lblqcmsg.Text = param[1].Value.ToString();
                Modalpopupext.Show();
            }
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hnprocessrecid.Value = "0";
        hn100_ISSUEORDERID.Value = "0";
        hn100_PROCESS_REC_ID.Value = "0";
        if (txtloomid.Text != "")
        {
            FillFolioNo(0);
        }
    }
    protected void FillFolioNo(int FolioNo)
    {
        string str = "";
        if (chkedit.Checked == true)
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
            from PROCESS_ISSUE_MASTER_1 PM(Nolock) 
            inner join LoomStockNo LS(Nolock) on Pm.issueorderid=Ls.issueorderid And LS.ProcessID = 1 
           
            where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + @" 
            and PM.LoomId=" + txtloomid.Text + @"   order by PM.IssueOrderId";
        }
        else
        {
            str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
            From PROCESS_ISSUE_MASTER_1 PM(Nolock)  
            --inner join LoomStockNo LS(Nolock) on Pm.issueorderid=Ls.issueorderid AND LS.ProcessID = 1 
            INNER JOIN Process_issue_detail_1 PID ON PM.IssueOrderId=PID.IssueOrderId
            where PM.Status<>'Canceled' and PM.status='Pending' and  PM.Companyid=" + DDcompany.SelectedValue + @" 
            and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text+" and PID.PQty>0";
            if (FolioNo > 0)
            {
                str = str + " And PM.IssueOrderID = " + FolioNo;
            }
            str = str + " Order By PM.IssueOrderId";
        }
        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void txtstockno_TextChanged(object sender, EventArgs e)
    {
        LblPcsType.Text = "";
        if (variable.VarLOOMBAZARBYPERCENTAGE == "1")
        {
            hnprocessrecid.Value = "0";
            hn100_ISSUEORDERID.Value = "0";
            hn100_PROCESS_REC_ID.Value = "0";
        }
        FillStockDetail();
        switch (Session["varcompanyNo"].ToString())
        {
            case "14":
            case "21":
                btnconfirm_Click(sender, new EventArgs());
                break;
            case "16":
                if (Session["varcompanyId"].ToString() == "16")
                {
//                    string Str = @"Select StockNo, TStockNo 
//                        From LoomStockNo CN(Nolock) 
//                        JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id And VF.ITEM_NAME <> 'CUSHION'
//                        Where CN.TStockNo = '" + txtstockno.Text + "'";

//                    DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
//                    if (ds.Tables[0].Rows.Count > 0)
//                    {
//                        txtactualwidth.Focus();
//                    }
//                    else
//                    {
//                        btnconfirm_Click(sender, new EventArgs());
//                    }
                    txtactualwidth.Focus();
                }
                break;
            case "28":
                txtactualwidth.Focus();
                break;
            case "22":
                txtwidth.Focus();
                break;
            case "43":
                if (VarConfirmButtonStatus == 0)
                {
                    btnconfirm.Visible = false;
                }
                else
                {
                    btnconfirm_Click(sender, new EventArgs());
                }
                break;
            default:
                btnconfirm.Focus();
                break;
        }

        //if (Convert.ToInt32(Session["varcompanyNo"]) == 28)
        //{
        //    if (Convert.ToInt32(Session["varSubCompanyId"]) == 283)
        //    {
        //        btnconfirm_Click(sender, new EventArgs());
        //    }
        //    else
        //    {
        //        txtactualwidth.Focus();
        //        return;
        //    }
        //}
    }
    protected void Savedetail(Object sender = null)
    {
        lblmessage.Text = "";

        //switch (Session["varcompanyid"].ToString())
        //{
        //    case "16":
        //        if (Tdreturnremark.Visible == false)
        //        {

        //            if (Convert.ToDecimal(txtstockweight.Text == "" ? "0" : txtstockweight.Text) <= 0)
        //            {
        //                lblmessage.Text = "Please enter stock Weight.";
        //                txtstockweight.Focus();
        //                return;
        //            }
        //        }
        //        break;
        //    default:
        //        break;
        //}

        if (Session["varcompanyId"].ToString() == "22")
        {
            if (DDCarpetGrade.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please select carpet grade');", true);
                return;
            }
        }

        //****************Return Remark
        if (Tdreturnremark.Visible == true)
        {
            if (txtretremark.Text == "")
            {
                lblmessage.Text = "Please enter Return Remark.";
                txtretremark.Focus();
                return;
            }
        }
        //******************
        if (txtstockno.Text != "")
        {
            //***************Employee
            DataTable dtemployee = new DataTable();
            dtemployee.Columns.Add("Empid", typeof(int));
            dtemployee.Columns.Add("Processid", typeof(int));
            dtemployee.Columns.Add("Workpercentage", typeof(float));
            //for (int i = 0; i < listWeaverName.Items.Count; i++)
            //{
            //    DataRow dr1 = dtemployee.NewRow();
            //    dr1["empid"] = listWeaverName.Items[i].Value;
            //    dr1["Processid"] = 1;
            //    dtemployee.Rows.Add(dr1);
            //}
            for (int i = 0; i < DGEmployee.Rows.Count; i++)
            {
                Label lblgridempid = (Label)DGEmployee.Rows[i].FindControl("lblgridempid");
                TextBox txtworkpercentage = (TextBox)DGEmployee.Rows[i].FindControl("txtworkpercentage");

                DataRow dr1 = dtemployee.NewRow();
                dr1["empid"] = lblgridempid.Text;
                dr1["Processid"] = 1;
                dr1["Workpercentage"] = txtworkpercentage == null ? "0" : (txtworkpercentage.Text == "" ? "0" : txtworkpercentage.Text);
                dtemployee.Rows.Add(dr1);
            }
            //**************
            if (dtemployee.Rows.Count > 0)
            {

                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("PRO_PRODUCTIONLOOMRECEIVESTOCKNOWISE", con, Tran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;

                    cmd.Parameters.Add("@Process_Rec_id", SqlDbType.Int);
                    cmd.Parameters["@Process_Rec_id"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@Process_Rec_id"].Value = hnprocessrecid.Value;

                    cmd.Parameters.AddWithValue("@Companyid", DDcompany.SelectedValue);
                    cmd.Parameters.AddWithValue("@Productionunit", DDProdunit.SelectedValue);
                    cmd.Parameters.AddWithValue("@Loomid", txtloomid.Text);
                    cmd.Parameters.AddWithValue("@Issueorderid", DDFolioNo.SelectedValue);
                    cmd.Parameters.Add("@ReceiveNo", SqlDbType.VarChar, 50);
                    cmd.Parameters["@ReceiveNo"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@ReceiveDate", txtrecdate.Text);
                    cmd.Parameters.Add("@msg", SqlDbType.VarChar, 500);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@dtemployee", dtemployee);
                    cmd.Parameters.AddWithValue("@Checkedby", txtcheckedby.Text);
                    cmd.Parameters.AddWithValue("@TstockNo", txtstockno.Text);
                    cmd.Parameters.AddWithValue("@WEIGHT", txtstockweight.Text == "" ? "0" : txtstockweight.Text);
                    cmd.Parameters.AddWithValue("@Penalityamt", txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text);
                    cmd.Parameters.AddWithValue("@Penalityremark", txtpenalityremarks.Text.Trim());
                    cmd.Parameters.AddWithValue("@LENGTH", txtlength.Text.Trim());
                    cmd.Parameters.AddWithValue("@Width", txtwidth.Text.Trim());
                    cmd.Parameters.AddWithValue("@Area", txtarea.Text == "" ? "0" : txtarea.Text);
                    cmd.Parameters.AddWithValue("@Commrate", txtcommrate.Text == "" ? "0" : txtcommrate.Text);
                    cmd.Parameters.AddWithValue("@QUALITYTYPE", ddStockQualityType.SelectedValue);
                    cmd.Parameters.AddWithValue("@QAPersonname", (DDQaname.SelectedIndex > 0 ? DDQaname.SelectedItem.Text : ""));
                    cmd.Parameters.AddWithValue("@actualwidth", txtactualwidth.Text);
                    cmd.Parameters.AddWithValue("@actuallength", txtactuallength.Text);

                    cmd.Parameters.Add("@MaxRejectedGatePassNo", SqlDbType.Int);
                    cmd.Parameters["@MaxRejectedGatePassNo"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@MaxRejectedGatePassNo"].Value = hnRejectedGatePassNo.Value;

                    cmd.Parameters.AddWithValue("@Returnremark", txtretremark.Text);
                    cmd.Parameters.AddWithValue("@Prodshift", TDprodshift.Visible == true ? DDprodshift.SelectedItem.Text : "");

                    cmd.Parameters.Add("@100_ISSUEORDERID", SqlDbType.Int);
                    cmd.Parameters["@100_ISSUEORDERID"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@100_ISSUEORDERID"].Value = hn100_ISSUEORDERID.Value;

                    cmd.Parameters.Add("@100_PROCESS_REC_ID", SqlDbType.Int);
                    cmd.Parameters["@100_PROCESS_REC_ID"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@100_PROCESS_REC_ID"].Value = hn100_PROCESS_REC_ID.Value;

                    cmd.Parameters.AddWithValue("@username", Session["UserName"]);
                    cmd.Parameters.AddWithValue("@CarpetGrade", (DDCarpetGrade.SelectedIndex > 0 ? DDCarpetGrade.SelectedValue: "0"));
                    cmd.Parameters.AddWithValue("@BranchId", DDBranchName.SelectedValue);
                    cmd.Parameters.AddWithValue("@PartyChallanNo", txtPartyChallanNo.Text);
                    cmd.Parameters.AddWithValue("@StockStatus", TDStockStatus.Visible == true ? DDStockStatus.SelectedValue : "0");
                    cmd.Parameters.AddWithValue("@STOCKNOREMARKS", TxtRemarks.Text);

                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "');", true);
                        lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                        Tran.Rollback();
                    }
                    else
                    {
                        Tran.Commit();
                        txtreceiveno.Text = cmd.Parameters["@ReceiveNo"].Value.ToString();
                        hnprocessrecid.Value = cmd.Parameters["@Process_Rec_id"].Value.ToString();
                        hn100_ISSUEORDERID.Value = cmd.Parameters["@100_ISSUEORDERID"].Value.ToString();
                        hn100_PROCESS_REC_ID.Value = cmd.Parameters["@100_PROCESS_REC_ID"].Value.ToString();
                        hnlastfoliono.Value = DDFolioNo.SelectedValue;
                        hnRejectedGatePassNo.Value = cmd.Parameters["@MaxRejectedGatePassNo"].Value.ToString();

                        lblmessage.Text = "Data saved successfully...";
                        txtstockno.Text = "";
                        ddStockQualityType.SelectedValue = "1";
                        Refreshcontrol(sender);
                    }
                    txtstockno.Focus();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    lblmessage.Text = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "a", "alert('Please fill employee detail')", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('Please Enter Stock No.')", true);
            txtstockno.Focus();
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
    protected void btnsubmitweight_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_UPDATEWEIGHT_ONRECNO", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;
            cmd.Parameters.AddWithValue("@Processrecid", hnprocessrecid.Value);
            cmd.Parameters.AddWithValue("@TotalWt", txttotalstockwt.Text == "" ? "0" : txttotalstockwt.Text);
            cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@mastercompanyId", Session["varcompanyid"]);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();

            lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
            txttotalstockwt.Text = "";
            Tran.Commit();
            FillRecDetails();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void DGStockDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGStockDetail.PageIndex = e.NewPageIndex;
        Fillstockno();
    }
    protected void btnsavefrmgrid_Click(object sender, EventArgs e)
    {
        //Grid Loop
        if (Session["varcompanyid"].ToString() == "14" || Session["varcompanyid"].ToString() == "16" || Session["varcompanyid"].ToString() == "21" || Convert.ToInt16(Request.QueryString["UserType"]) == 1)   //////For More than 1 Stock No To Save ONE Times.
        {
            SaveDetailForBulk();
            txtstockweight.Text = "";
        }
        else
        {
            for (int i = 0; i < DGStockDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
                if (Chkboxitem.Checked == true)
                {
                    if (variable.VarLOOMBAZARBYPERCENTAGE == "1")
                    {
                        hnprocessrecid.Value = "0";
                        hn100_ISSUEORDERID.Value = "0";
                        hn100_PROCESS_REC_ID.Value = "0";
                    }
                    txtstockno.Text = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text;
                    FillStockDetail();
                    Savedetail(sender);
                }
            }
        }
        fillGrid();
        //FillRecDetails();
        //Fillstockno();
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
    }

    protected void SaveDetailForBulk()
    {
        string TStockNo = "";
        for (int i = 0; i < DGStockDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = (CheckBox)DGStockDetail.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                if (TStockNo == "")
                {
                    TStockNo = ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text + "~";
                }
                else
                {
                    TStockNo = TStockNo + ((Label)DGStockDetail.Rows[i].FindControl("lbltstockno")).Text + "~";
                }
            }
        }
        if (TStockNo != "")
        {

            ////SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            ////if (con.State == ConnectionState.Closed)
            ////{
            ////    con.Open();
            ////}
            ////SqlTransaction Tran = con.BeginTransaction();
            ////try
            ////{
            ////    SqlParameter[] param = new SqlParameter[29];
            ////    param[0] = new SqlParameter("@PROCESS_REC_ID", SqlDbType.Int);
            ////    param[0].Value = hnprocessrecid.Value;
            ////    param[0].Direction = ParameterDirection.InputOutput;
            ////    param[1] = new SqlParameter("@COMPANYID", DDcompany.SelectedValue);
            ////    param[2] = new SqlParameter("@PRODUCTIONUNIT", DDProdunit.SelectedValue);
            ////    param[3] = new SqlParameter("@LOOMID", txtloomid.Text);
            ////    param[4] = new SqlParameter("@ISSUEORDERID", DDFolioNo.SelectedValue);
            ////    param[5] = new SqlParameter("@RECEIVENO", SqlDbType.VarChar, 50);
            ////    param[5].Direction = ParameterDirection.Output;
            ////    param[6] = new SqlParameter("@RECEIVEDATE", txtrecdate.Text);
            ////    param[7] = new SqlParameter("@TSTOCKNO", TStockNo);
            ////    param[8] = new SqlParameter("@Checkedby", txtcheckedby.Text);
            ////    param[9] = new SqlParameter("@WEIGHT", txtstockweight.Text == "" ? "0" : txtstockweight.Text);
            ////    param[10] = new SqlParameter("@Penalityamt", txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text);
            ////    param[11] = new SqlParameter("@Penalityremark", txtpenalityremarks.Text.Trim());
            ////    param[12] = new SqlParameter("@QUALITYTYPE", ddStockQualityType.SelectedValue);
            ////    param[13] = new SqlParameter("@msg", SqlDbType.VarChar, 500);
            ////    param[13].Direction = ParameterDirection.Output;
            ////    param[14] = new SqlParameter("@Userid", Session["varuserid"]);
            ////    param[15] = new SqlParameter("@MasterCompanyID", Session["varCompanyId"]);

            ////    //
            ////    //**************
            ////    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_PRODUCTIONLOOMRECEIVE_BULK_STOCKNO_WISE", param);
            ////    if (param[13].Value.ToString() != "")
            ////    {
            ////        Tran.Rollback();
            ////        ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + param[13].Value.ToString() + "');", true);
            ////        lblmessage.Text = param[13].Value.ToString();
            ////    }
            ////    else
            ////    {
            ////        Tran.Commit();
            ////        txtreceiveno.Text = param[5].Value.ToString();

            ////        hnprocessrecid.Value = param[0].Value.ToString();

            ////        hnlastfoliono.Value = DDFolioNo.SelectedValue;
            ////        lblmessage.Text = "Data saved successfully...";
            ////        txtstockno.Text = "";
            ////        ddStockQualityType.SelectedValue = "1";
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    Tran.Rollback();
            ////    lblmessage.Text = ex.Message;
            ////}
            ////finally
            ////{
            ////    con.Close();
            ////    con.Dispose();
            ////}


            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                string sp = string.Empty;
                if (Session["varcompanyid"].ToString() == "44")
                {
                    sp = "PRO_PRODUCTIONLOOMRECEIVE_BULK_STOCKNO_WISE_AGNI";
                }
                else
                {
                    sp = "PRO_PRODUCTIONLOOMRECEIVE_BULK_STOCKNO_WISE";
                
                }


                SqlCommand cmd = new SqlCommand(sp, con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.Add("@PROCESS_REC_ID", SqlDbType.Int);
                cmd.Parameters["@PROCESS_REC_ID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@PROCESS_REC_ID"].Value = hnprocessrecid.Value;

                cmd.Parameters.AddWithValue("@COMPANYID", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@PRODUCTIONUNIT", DDProdunit.SelectedValue);
                cmd.Parameters.AddWithValue("@LOOMID", txtloomid.Text);
                cmd.Parameters.AddWithValue("@ISSUEORDERID", DDFolioNo.SelectedValue);
                cmd.Parameters.Add("@RECEIVENO", SqlDbType.VarChar, 50);
                cmd.Parameters["@RECEIVENO"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@RECEIVEDATE", txtrecdate.Text);
                cmd.Parameters.AddWithValue("@TSTOCKNO", TStockNo);
                cmd.Parameters.AddWithValue("@Checkedby", txtcheckedby.Text);
                cmd.Parameters.AddWithValue("@WEIGHT", txtstockweight.Text == "" ? "0" : txtstockweight.Text);
                cmd.Parameters.AddWithValue("@Penalityamt", txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text);
                cmd.Parameters.AddWithValue("@Penalityremark", txtpenalityremarks.Text.Trim());
                cmd.Parameters.AddWithValue("@QUALITYTYPE", ddStockQualityType.SelectedValue);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 500);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyId"]);
                cmd.Parameters.AddWithValue("@BranchId", DDBranchName.SelectedValue);
                cmd.Parameters.AddWithValue("@PartyChallanNo", txtPartyChallanNo.Text);
                cmd.Parameters.Add("@Batch_challanno", SqlDbType.Int);
                cmd.Parameters["@Batch_challanno"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@msg"].Value.ToString() != "") //IF DATA NOT SAVED
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "');", true);
                    lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    Tran.Commit();
                    txtreceiveno.Text = cmd.Parameters["@ReceiveNo"].Value.ToString();
                    hnprocessrecid.Value = cmd.Parameters["@Process_Rec_id"].Value.ToString();
                    txtBatchChallanNo.Text = cmd.Parameters["@Batch_challanno"].Value.ToString();
                    hnlastfoliono.Value = DDFolioNo.SelectedValue;

                    lblmessage.Text = "Data saved successfully...";
                    txtstockno.Text = "";
                    ddStockQualityType.SelectedValue = "1";
                }
                //txtstockno.Focus();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                lblmessage.Text = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Please select atleats one StockNo!');", true);
        }
    }
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
        if (txteditempcode.Text != "")
        {
            string str = @"select Distinct U.UnitsId,U.UnitName From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        Where PIM.Companyid=" + DDcompany.SelectedValue;
            if (txteditempid.Text != "")
            {
                str = str + " and EMP.EMPID=" + txteditempid.Text;
            }
            str = str + " order by Unitname";

            UtilityModule.ConditionalComboFill(ref DDProdunit, str, true, "--Plz Select--");
            if (DDProdunit.Items.Count > 0)
            {
                DDProdunit.SelectedIndex = 1;
                DDProdunit_SelectedIndexChanged(sender, new EventArgs());
            }
        }
        else
        {
            TDLoomno.Visible = false;
            TDLoomNotextbox.Visible = true;
            txtloomid.Text = "";
        }
    }
    protected void FillStockDetail()
    {
        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@TstockNo", txtstockno.Text);
            param[1] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value == "" ? "0" : hnprocessrecid.Value);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETLOOMSTOCKDETAIL", param);
            Refreshcontrol();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //btnconfirm.Visible = true;
                VarConfirmButtonStatus = 1;
                if (ds.Tables[0].Rows[0]["PcsType"].ToString() == "999")
                {
                    VarConfirmButtonStatus = 0;
                    btnconfirm.Visible = false;
                    //lblmessage.Text = "Stock No. Quality does match with last scan carpet quality. Please scan same quality carpet!.";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + ds.Tables[0].Rows[0]["Msg"] + "');", true);
                    return;                   
                }               

                if (DDcompany.SelectedValue.ToString() != ds.Tables[0].Rows[0]["CompanyId"].ToString())
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('This Stock no does not belong to this company');", true);
                    return;
                }

                LblPcsType.Text = ds.Tables[0].Rows[0]["PcsType"].ToString();
                TRItemdetail.Visible = true;
                txtwidth.Text = ds.Tables[0].Rows[0]["width"].ToString();
                txtlength.Text = ds.Tables[0].Rows[0]["Length"].ToString();
                txtarea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                txtcommrate.Text = ds.Tables[0].Rows[0]["comm"].ToString();
                hnunitid.Value = ds.Tables[0].Rows[0]["unitid"].ToString();
                hncaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                hnshapeid.Value = ds.Tables[0].Rows[0]["shapeid"].ToString();
                if (DDcompany.SelectedValue != ds.Tables[0].Rows[0]["CompanyId"].ToString())
                {
                    hnRejectedGatePassNo.Value = "0";
                }
                DDcompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDProdunit.SelectedValue = ds.Tables[0].Rows[0]["Units"].ToString();
                txtloomid.Text = ds.Tables[0].Rows[0]["Loomid"].ToString();
                txtloomno.Text = ds.Tables[0].Rows[0]["LoomNo"].ToString();
                txtitem.Text = ds.Tables[0].Rows[0]["Item_name"].ToString();
                txtQuality.Text = ds.Tables[0].Rows[0]["QualityName"].ToString();
                txtDesign.Text = ds.Tables[0].Rows[0]["designname"].ToString();
                txtcolor.Text = ds.Tables[0].Rows[0]["colorname"].ToString();
                txtshape.Text = ds.Tables[0].Rows[0]["shapename"].ToString();
                txtsize.Text = ds.Tables[0].Rows[0]["size"].ToString();
                TxtConsumption.Text = ds.Tables[0].Rows[0]["Consumption"].ToString();
                imgProduct.ImageUrl = ds.Tables[0].Rows[0]["PHOTO"].ToString();
                FillFolioNo(Convert.ToInt32(ds.Tables[0].Rows[0]["issueorderid"]));

                varWidth = ds.Tables[0].Rows[0]["width"].ToString();
                varLength = ds.Tables[0].Rows[0]["Length"].ToString();

                if (Session["VarCompanyNo"].ToString() == "43")
                {
                    txtactualwidth.Text = ds.Tables[0].Rows[0]["ExportSizeWidth"].ToString();
                    txtactuallength.Text = ds.Tables[0].Rows[0]["ExportSizeLength"].ToString();
                }
                else
                {
                    txtactualwidth.Text = "";
                    txtactuallength.Text = "";
                }                

                if (DDFolioNo.Items.FindByValue(ds.Tables[0].Rows[0]["issueorderid"].ToString()) != null)
                {
                    DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
                    if (TdRawmaterialissued.Visible == true)
                    {
                        fillRawMaterialIssued();
                    }
                }
                else
                {
                    DDFolioNo.SelectedIndex = -1;
                    lblmessage.Text = "Please select Folio No.";
                    return;
                }
               
                if (Convert.ToInt32(hnlastfoliono.Value) != Convert.ToInt32(DDFolioNo.SelectedValue))
                {                   
                   if (Session["VarCompanyNo"].ToString()!= "43")
                   {
                        hnprocessrecid.Value = "0";                   
                        hn100_ISSUEORDERID.Value = "0";
                        hn100_PROCESS_REC_ID.Value = "0";
                   }                        
                    
                }
                DGEmployee.DataSource = ds.Tables[1];
                DGEmployee.DataBind();
            }
            else
            {
                lblmessage.Text = "Stock No. does not exists or Pending.";
                TRItemdetail.Visible = false;

                if (Session["varCompanyNo"].ToString() == "43")
                {
                    txtstockno.Text = "";
                    txtstockno.Focus();
                }
                return;
            }

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;

        }
    }

    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        if (Session["varcompanyId"].ToString() == "16" || Session["varcompanyId"].ToString() == "42" || Session["varcompanyId"].ToString() == "45")
        {
            if (txtactualwidth.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please fill actual width');", true);
                txtactualwidth.Focus();
                return;
            }
            if (txtactuallength.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please fill actual length');", true);
                txtactuallength.Focus();
                return;
            }
        }
        
        if (Session["varcompanyId"].ToString() == "22")
        {
            if (DDCarpetGrade.SelectedIndex==0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('Please select carpet grade');", true);                
                return;
            }           
        }
        Savedetail(sender);
        if (Session["varCompanyNo"].ToString() == "43")
        {
            TableIssueDetail.Visible = false;
            DG.DataSource = "";
            DG.DataBind();
            FillRecDetails();
        }
        //fillGrid();
        //FillRecDetails();

    }
    private void Check_Length_Width_Format()
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";


        if (txtlength.Text != "")
        {
            if (Convert.ToInt32(hnunitid.Value) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtlength.Text));
                txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    txtlength.Text = "";
                    txtlength.Focus();
                }
            }
        }
        if (txtwidth.Text != "")
        {
            if (Convert.ToInt32(hnunitid.Value) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(txtwidth.Text));
                txtwidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    txtwidth.Text = "";
                    txtwidth.Focus();
                }
            }
        }
        if (txtlength.Text != "" && txtwidth.Text != "")
        {
            int Shape = Convert.ToInt16(hnshapeid.Value);

            if (Convert.ToInt32(hnunitid.Value) == 1)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(hncaltype.Value), Shape));
            }
            if (Convert.ToInt32(hnunitid.Value) == 2 || Convert.ToInt16(hnunitid.Value) == 6)
            {
                txtarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(txtlength.Text), Convert.ToDouble(txtwidth.Text), Convert.ToInt32(hncaltype.Value), Shape, UnitId: Convert.ToInt16(hnunitid.Value)));
            }
        }
    }
    protected void Check_Length_Width_Size()
    {
        lblmessage.Text = "";
        if (txtwidth.Text != "")
        {
            if ((Convert.ToDouble(txtwidth.Text) > Convert.ToDouble(varWidth) + 20) || (Convert.ToDouble(txtwidth.Text) < Convert.ToDouble(varWidth) - 20))
            {
                lblmessage.Text = "Width value can't be exceed 20cm extra/Less from showing width value";
                txtwidth.Text = "";
                txtwidth.Focus();
            }
        }

        if (txtlength.Text != "")
        {
            if ((Convert.ToDouble(txtlength.Text) > Convert.ToDouble(varLength) + 20) || (Convert.ToDouble(txtlength.Text) < Convert.ToDouble(varLength) - 20))
            {
                lblmessage.Text = "Length value can't be exceed 20cm extra/Less from showing length value";
                txtlength.Text = "";
                txtlength.Focus();
            }
        }
    }

    protected void txtwidth_TextChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "22")
        {
            Check_Length_Width_Size();
        }

        Check_Length_Width_Format();
    }
    protected void txtlength_TextChanged(object sender, EventArgs e)
    {
        if (Session["varCompanyNo"].ToString() == "22")
        {
            Check_Length_Width_Size();
        }

        Check_Length_Width_Format();
    }
    protected void txtrecdate_TextChanged(object sender, EventArgs e)
    {
        hnprocessrecid.Value = "0";
        hnlastfoliono.Value = "0";
        hnRejectedGatePassNo.Value = "0";
        hn100_ISSUEORDERID.Value = "0";
        hn100_PROCESS_REC_ID.Value = "0";
    }
    protected void DGEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGEmployee.Columns.Count; i++)
            {
                if (DGEmployee.Columns[i].HeaderText.ToUpper() == "WORK(%)")
                {
                    if (variable.VarLOOMBAZARBYPERCENTAGE == "0")
                    {
                        DGEmployee.Columns[i].Visible = false;
                    }
                }
            }
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
                txt.ToolTip = "give reason for not ok";
                txt.Attributes.Add("runat", "server");
                e.Row.Cells[i].Controls.Add(txt);

            }
        }
    }
    protected void btnQcreport_Click(object sender, EventArgs e)
    {
        reportQcheck();
    }
    private void reportQcheck()
    {

        DataSet ds = new DataSet();
        int Issueorderid = 0;

        SqlParameter[] param = new SqlParameter[5];
        param[0] = new SqlParameter("@processid", SqlDbType.Int);
        param[0].Value = 1;
        param[1] = new SqlParameter("@Processrecid", hnprocessrecid.Value);
        param[2] = new SqlParameter("@issueorderid", Issueorderid);
        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETQCREPORTBAZAR_NEWBAZAR", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "Reports/WCarpetRecvQCNew.rpt";
            Session["dsFileName"] = "~\\ReportSchema\\WCarpetRecvQCNew.xsd";
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
    protected void btncheckallpcsqc_Click(object sender, EventArgs e)
    {
        Modalpopupext.Show();
        ViewState["qcdetail"] = null;
        string Processrecdetailidgrid = "";
        for (int i = 0; i < DGRecDetail.Rows.Count; i++)
        {
            Label lblprocessrecdetailid = (Label)DGRecDetail.Rows[i].FindControl("lblprocessrecdetailid");
            Processrecdetailidgrid = Processrecdetailidgrid + "," + lblprocessrecdetailid.Text;
        }
        Processrecdetailidgrid = Processrecdetailidgrid.TrimStart(',');
        //**********get QC parameter
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@Processrecid", hnprocessrecid.Value);
        param[1] = new SqlParameter("@Processrecdetailid", Processrecdetailidgrid);
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_getqcparameter", param);

        if (ds.Tables[0].Rows.Count > 0)
        {
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
    protected void btnReturnGatePass_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@GatePassNo", hnRejectedGatePassNo.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FORPRODUCTIONRECEIVE_RETURNLOOMGATEPASSREPORT", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptProductionreceivereturnloomgatepassdetail.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptProductionreceivereturnloomgatepassdetail.xsd";

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
    protected void ddStockQualityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Tdreturnremark.Visible = false;
        txtretremark.Text = "";
        switch (ddStockQualityType.SelectedValue)
        {
            case "4":
                Tdreturnremark.Visible = true;
                break;
            case "5":
                Tdreturnremark.Visible = true;
                break;
            default:
                break;
        }
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        FIllProdUnit(sender);
    }
    protected void FIllProdUnit(object sender = null)
    {

        string str = @"select Distinct U.UnitsId,U.UnitName From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        Where PIM.Companyid=" + DDcompany.SelectedValue;
        if (txteditempid.Text != "")
        {
            str = str + " and EMP.EMPID=" + txteditempid.Text;
        }
        if (txtfolionoedit.Text != "")
        {
            ////str = str + " and EMP.Issueorderid=" + txtfolionoedit.Text;

            str = str + " and PIM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        str = str + " order by Unitname";
        UtilityModule.ConditionalComboFill(ref DDProdunit, str, true, "--Plz Select--");
        if (DDProdunit.Items.Count > 0)
        {
            DDProdunit.SelectedIndex = 1;
            DDProdunit_SelectedIndexChanged(sender, new EventArgs());
        }

    }
    protected void TxtReceiveQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        if (Session["VarCompanyNo"].ToString() == "43")
        {
            if (Convert.ToInt32(TxtReceiveQty.Text) > 200)
            {
                TxtReceiveQty.Text = "200";
            }
        }

        if (TxtReceiveQty.Text != "" && DDFolioNo.SelectedIndex > 0)
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtReceiveQty.Text);
            Fillstockno();
        }
    }
    protected void lnkRemoveQccheck_Click(object sender, EventArgs e)
    {
        lblRemoveqcmsg.Text = "";
        ModalPopuptext2.Show();
        ViewState["qcdetail"] = null;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label Processrecid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecid");
            Label processrecdetailid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecdetailid");

            lblRemoveQCProcessRecId.Text = Processrecid.Text;
            lblRemoveQCProcessRecDetailId.Text = processrecdetailid.Text;
            txtRemoveQCDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        }


        ////Modalpopupext.Show();
        ////ViewState["qcdetail"] = null;
        //LinkButton lnk = sender as LinkButton;
        //if (lnk != null)
        //{
        //    GridViewRow grv = lnk.NamingContainer as GridViewRow;
        //    Label Processrecid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecid");
        //    Label processrecdetailid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecdetailid");

        //    if (Convert.ToInt32(Processrecid.Text) > 0)
        //    {
        //        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        SqlTransaction Tran = con.BeginTransaction();
        //        try
        //        {
        //            //**********Remove QC Defects
        //            SqlParameter[] param = new SqlParameter[6];
        //            param[0] = new SqlParameter("@Process_Rec_Id", Processrecid.Text);
        //            param[1] = new SqlParameter("@Process_Rec_Detail_Id", processrecdetailid.Text);
        //            param[2] = new SqlParameter("@ProcessId", 1);
        //            param[3] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
        //            param[3].Direction = ParameterDirection.Output;
        //            param[4] = new SqlParameter("@UserId", Session["varuserid"]);
        //            param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);

        //            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_REMOVEQCDEFECTS", param);
        //            lblmessage.Text = param[3].Value.ToString();
        //            Tran.Commit();
        //            FillRecDetails();
        //        }
        //        catch (Exception ex)
        //        {
        //            lblmessage.Text = ex.Message;
        //            Tran.Rollback();
        //        }
        //        finally
        //        {
        //            con.Close();
        //            con.Dispose();
        //        }
        //    }


        //}
    }
    protected void BtnRemoveQCSave_Click(object sender, EventArgs e)
    {
        lblRemoveqcmsg.Text = "";
        try
        {

            if (Convert.ToInt32(lblRemoveQCProcessRecId.Text) > 0)
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    ////**********Remove QC Defects
                    SqlParameter[] param = new SqlParameter[7];
                    param[0] = new SqlParameter("@Process_Rec_Id", lblRemoveQCProcessRecId.Text);
                    param[1] = new SqlParameter("@Process_Rec_Detail_Id", lblRemoveQCProcessRecDetailId.Text);
                    param[2] = new SqlParameter("@ProcessId", 1);
                    param[3] = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
                    param[3].Direction = ParameterDirection.Output;
                    param[4] = new SqlParameter("@UserId", Session["varuserid"]);
                    param[5] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                    param[6] = new SqlParameter("@QCRemoveDate", txtRemoveQCDate.Text);

                    SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_REMOVEQCDEFECTS", param);
                    lblRemoveqcmsg.Text = param[3].Value.ToString();
                    Tran.Commit();
                    ModalPopuptext2.Show();
                }
                catch (Exception ex)
                {
                    lblmessage.Text = ex.Message;
                    Tran.Rollback();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            lblqcmsg.Text = ex.Message;
        }
    }
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        ItemFinishedId = Convert.ToInt32(DG.SelectedValue);
        Fillstockno();
    }

    protected void BtnUpdateRate_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
           
            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);  
            param[1] = new SqlParameter("@Userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
           
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEBAZAARRATEFOLIONOWISE", param);
            //*************
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            fillGrid();
            if (TdRawmaterialissued.Visible == true)
            {
                fillRawMaterialIssued();
            }
            if (TDstockno.Visible == true)
            {
                Fillstockno();
            }

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void ShowData_Click(object sender, EventArgs e)
    {
        fillGrid();
        FillRecDetails();
    }
    protected void BtnUpdateConsumption_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {

            SqlParameter[] param = new SqlParameter[10];
            param[0] = new SqlParameter("@Process_Rec_ID", DDreceiveNo.SelectedValue);
            param[1] = new SqlParameter("@Userid", Session["varuserid"]);
            param[2] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;

            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEBAZAARCONSUMPTION", param);
            //*************
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();            

        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void lnkAddPenality_Click(object sender, EventArgs e)
    {
        lblPenalityUpdateMsg.Text = "";
        ModalPopuptext3.Show();
        ViewState["PenalityDetail"] = null;
        LinkButton lnk = sender as LinkButton;
        if (lnk != null)
        {
            GridViewRow grv = lnk.NamingContainer as GridViewRow;
            Label Processrecid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecid");
            Label processrecdetailid = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblprocessrecdetailid");
            Label lblQualityId = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblQualityId");
            Label lblCalType = (Label)DGRecDetail.Rows[grv.RowIndex].FindControl("lblCalType");

            lblPenalityProcessRecId.Text = Processrecid.Text;
            lblPenalityProcessRecDetailId.Text = processrecdetailid.Text;

            string sql = "";
            sql = @"select PenalityId,PenalityName,rate,PenalityType,QualityId from PenalityMaster where (Qualityid=" + lblQualityId.Text + " or Qualityid=0) and PenalityWF='W' And PenalityId<>-1 ";

            if (lblCalType.Text == "1")
            {
                sql = sql + "And PenalityType='A'";
            }
            else
            {
                sql = sql + "And PenalityType='C'";
            }
            sql = sql + "Order By PenalityName Desc";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GVPenalty.DataSource = ds;
                GVPenalty.DataBind();
                GVPenalty.Visible = true;


                string str2 = "";
                str2 = @"select PenalityId,1 as flag from WeaverCarpetReceivePenality where Process_Rec_Id=" + Processrecid.Text + " and Process_Rec_Detail_Id=" + processrecdetailid.Text + "";
                DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
                 if (ds2.Tables[0].Rows.Count > 0)
                 {
                     for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                     {
                         int PId = Convert.ToInt32(ds2.Tables[0].Rows[j]["PenalityId"]);
                         foreach (GridViewRow row in GVPenalty.Rows)
                         {                             
                             CheckBox Chkboxitem = row.FindControl("Chkboxitem") as CheckBox;
                             Label lblPenalityId = row.FindControl("lblPenalityId") as Label;
                             if (PId == Convert.ToInt32(lblPenalityId.Text))
                             {
                                 Chkboxitem.Checked = true;
                             }                             
                         }
                     }                     
                 }

            }
            else
            {
                GVPenalty.Visible = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }

        }
        
    }
    protected void GVPenalty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GVPenalty, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnSavePenality_Click(object sender, EventArgs e)
    {
        string PenalityDetailData = "";       

        for (int i = 0; i < GVPenalty.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)GVPenalty.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label lblPenalityId = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityId"));
                Label lblPenalityType = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityType"));
                Label lblPenalityName = ((Label)GVPenalty.Rows[i].FindControl("lblPenalityName"));
                Label lblRate = ((Label)GVPenalty.Rows[i].FindControl("lblRate"));

                if (PenalityDetailData == "")
                {
                    PenalityDetailData = lblPenalityId.Text + "|" + lblPenalityType.Text + "|" + lblPenalityName.Text + "|" + lblRate.Text + "~";
                }
                else
                {
                    PenalityDetailData = PenalityDetailData + lblPenalityId.Text + "|" + lblPenalityType.Text + "|" + lblPenalityName.Text + "|" + lblRate.Text + "~";
                }
            }
        }
        if (PenalityDetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }


        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[8];
            arr[0] = new SqlParameter("@WPenalityId", SqlDbType.Int);
            arr[1] = new SqlParameter("@Process_Rec_Id", SqlDbType.Int);
            arr[2] = new SqlParameter("@Process_Rec_Detail_Id", SqlDbType.Int);
            arr[3] = new SqlParameter("@PenalityDetailData", SqlDbType.NVarChar);   
            arr[4] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[6] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);
            arr[7] = new SqlParameter("@ProcessId", SqlDbType.Int);

            arr[0].Direction = ParameterDirection.InputOutput;
            arr[0].Value = HnWPenalityId.Value;
            arr[1].Value = lblPenalityProcessRecId.Text;
            arr[2].Value = lblPenalityProcessRecDetailId.Text;
            arr[3].Value = PenalityDetailData;   
            arr[4].Value = Session["varuserid"];          
            arr[5].Value = Session["varCompanyId"];
            arr[6].Direction = ParameterDirection.Output;
            arr[7].Value = 1;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SavePenalityOnBazaar]", arr);

            if (arr[6].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[6].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                HnWPenalityId.Value = arr[0].Value.ToString();                
                tran.Commit();
            }
            fillGrid();
            FillRecDetails();
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (MySession.ProductionEditPwd == txtpwd.Text)
        {
            if (btnclickflag == "BtnDeleteRow")
            {
                DeleteRow(VarProcess_Rec_Detail_Id,VarProcess_Rec_Id);
            }
            Popup(false);
        }
        else
        {
            lblmessage.Visible = true;
            lblmessage.Text = "Please Enter Correct Password..";
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
    protected void DDStockStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtEditReceiveNoForCI_TextChanged(object sender, EventArgs e)
    {
        string str="",str2="";       

        str = @"Select Distinct PRD.ISSUEORDERID,isnull(PRM.Process_Rec_Id,0) as Process_Rec_Id From PROCESS_RECEIVE_MASTER_1 PRM(NoLock) JOIN PROCESS_RECEIVE_DETAIL_1 PRD(NoLock) ON PRM.PROCESS_REC_ID=PRD.PROCESS_REC_ID
                       Where PRM.CHALLANNO='" +txtEditReceiveNoForCI.Text+"'";
         DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
         if (ds.Tables[0].Rows.Count > 0)
         {
             int issueorderid = 0;
             issueorderid =Convert.ToInt32(ds.Tables[0].Rows[0]["ISSUEORDERID"].ToString());
             TempProcessRecId = ds.Tables[0].Rows[0]["Process_Rec_Id"].ToString();

             str2 = @" Select distinct isnull(PIM.ChallanNo,PIM.ISSUEORDERID) as FolioChallanNo from PROCESS_ISSUE_MASTER_1 PIM(NoLock) JOIN PROCESS_ISSUE_DETAIL_1 PID(NoLock) ON PIM.ISSUEORDERID=PID.ISSUEORDERID
                    Where PIM.ISSUEORDERID="+issueorderid+"";
             DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);
             if (ds2.Tables[0].Rows.Count > 0)
             {
                 txtfolionoedit.Text = ds2.Tables[0].Rows[0]["FolioChallanNo"].ToString();
                 txtfolionoedit_TextChanged(sender, new EventArgs());
             }
         }
         else
         {           
             ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true);
         }        
        
    }
}