using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_HomeFurnishing_FrmFirstProcessReceive : System.Web.UI.Page
{
    static string varLength = "";
    static string varWidth = "";
    static int varIssueDetailID = 0;
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
        if (Session["varMasterCompanyIDForERP"] == null)
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
                typeof(Masters_HomeFurnishing_FrmFirstProcessReceive),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            string str = @"select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock) 
                        JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName                          
                        Select Distinct PNM.Process_Name_ID, PNM.PROCESS_NAME                      
                        From PROCESS_NAME_MASTER PNM(nolock) 
                        Where PNM.MASTERCOMPANYID = " + Session["varMasterCompanyIDForERP"] + @" and Process_Name in ('BATHMAT DYEING', 'STITCHING') 
                        Order By PNM.PROCESS_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            //UtilityModule.ConditionalComboFillWithDS(ref DDQaname, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            ProcessNameSelectedChanged();
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {

                case "16":
                case "28":
                    if (Convert.ToInt16(Request.QueryString["UserType"]) == 1)
                    {
                        TxtRecQty.Enabled = true;
                        TxtRecQty.Text = "500";
                        DGStockDetail.PageSize = 500;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedChanged();
    }
    protected void ProcessNameSelectedChanged()
    {
        string str = "";
        if (chkedit.Checked == true)
        {
            str = @"select Distinct PRM.Process_Rec_Id,PRM.ChallanNo+' /'+REPLACE(CONVERT(nvarchar(11),receivedate,106),' ','-') As ChallanNo 
                from Process_receive_master_" + DDProcessName.SelectedValue + @" PRM(NoLock)  
                inner join PROCESS_RECEIVE_DETAIL_" + DDProcessName.SelectedValue + @" PRD(NoLock)  on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                JOIN Process_Issue_Master_" + DDProcessName.SelectedValue + @" PM(NoLock)  ON PM.IssueOrderID=PRD.IssueOrderID";
            if (txtfolionoedit.Text != "")
            {
                str = str + " And PM.ChallanNo = '" + txtfolionoedit.Text + "'";
            }

            str = str + @" JOIN Employee_ProcessReceiveNo EMP(Nolock) ON EMP.Process_Rec_id = PRM.Process_Rec_id and EMP.processid = " + DDProcessName.SelectedValue;

            //str = str + @" where PRD.IssueOrderId=" + DDFolioNo.SelectedValue + "";

            if (txteditempid.Text != "")
            {
                str = str + " And EMP.EMPID = " + txteditempid.Text;
            }
            str = str + @" Order By PRM.Process_Rec_Id ";

            UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");          
        }
        else
        {
            FillFolioNo();
        }
    }
    protected void FillFolioNo()
    {
        string str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
                from PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PM(NoLock) 
                inner join EMployee_Processorderno EMP(NoLock)  on PM.issueorderid=EMp.issueorderid and EMP.processid=" + DDProcessName.SelectedValue + @"
                inner join LoomstockNo ls(NoLock) on PM.issueorderid=Ls.issueorderid and ls.ProcessId=" + DDProcessName.SelectedValue + @"
                where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " ";
        if (txteditempcode.Text != "")
        {
            str = str + " and EMP.EMPID=" + txteditempid.Text + "";
        }
        if (txtfolionoedit.Text != "")
        {
            str = str + " and PM.ChallanNo='" + txtfolionoedit.Text + "'";
        }
        if (chkedit.Checked == false)
        {
            str = str + " and PM.Status='Pending'";
        }

        str = str + "  order by PM.IssueOrderId";

        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void txtfolionoedit_TextChanged(object sender, EventArgs e)
    {
        ProcessNameSelectedChanged();
    }
    protected void DDFolioNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FolioNoSelectedChanged();
    }
    protected void FolioNoSelectedChanged()
    {
        fillGrid();
        if (TDstockno.Visible == true)
        {
            Fillstockno(varIssueDetailID);
        }
    }
    protected void Fillstockno(int IssueDetailID)
    {
        string str = @"SELECT LS.TSTOCKNO 
                FROM LOOMSTOCKNO LS(NoLock)  
                INNER JOIN PROCESS_ISSUE_MASTER_" + DDProcessName.SelectedValue + @" PIM(NoLock)  ON LS.ISSUEORDERID=PIM.ISSUEORDERID 
                and LS.ProcessId = " + DDProcessName.SelectedValue + @" And LS.BAZARSTATUS = 0 AND LS.IssueDetailId = " + IssueDetailID;
        if (txtstockno.Text != "")
        {
            str = str + " And Ls.TStockNo = '" + txtstockno.Text + "'";
        }
        str = str + " Where PIM.CompanyId = " + DDcompany.SelectedValue + " And PIM.ISSUEORDERID = " + DDFolioNo.SelectedValue + @" And PIM.STATUS <> 'CANCELED' 
                Order By Ls.stockNo";
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
    protected void fillGrid()
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
        param[4] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);
        param[5] = new SqlParameter("@TStockNo", txtstockno.Text);

        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FILLFIRSTPROCESSORDERDATALOOM", param);
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
    }
    protected void Refreshcontrol(object sender = null)
    {
        txtwidth.Text = "";
        txtlength.Text = "";
        txtarea.Text = "";
        //txtstockweight.Text = "";
        txtpenalityamt.Text = "";
        txtpenalityremarks.Text = "";
        if (sender != null)
        {
            ddStockQualityType_SelectedIndexChanged(sender, new EventArgs());
        }
        //txtcommrate.Text = "";
    }
    protected void FillRecDetails()
    {
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);
        array[1] = new SqlParameter("@FlagGridReport", 0);
        array[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FIRSTPROCESSRECEIVE_FILLGRID_REPORT", array);
        DGRecDetail.DataSource = ds.Tables[0];
        DGRecDetail.DataBind();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[2];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);
        array[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FORPRODUCTIONRECEIVEREPORT_LOOMREPORT", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "16":
                case "28":
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailchampo.rpt";
                    break;
                case "44":
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailagni.rpt";
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
    protected void DDreceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprocessrecid.Value = DDreceiveNo.SelectedValue;
        FillRecDetails();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        DDreceiveNo.Items.Clear();
        hnprocessrecid.Value = "0";
        if (chkedit.Checked == true)
        {
            TDreceiveNo.Visible = true;
            TDFolioNotext.Visible = true;
            ProcessNameSelectedChanged();
        }
        else
        {
            TDreceiveNo.Visible = false;
        }
    }
    protected void DGRecDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
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
            //**********
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@ProcessRecID", lblprocessrecid.Text);
            param[1] = new SqlParameter("@ProcessRecDetailID", lblprocessrecdetailid.Text);
            param[2] = new SqlParameter("@ProcessID ", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["varuserid"]);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_FIRSTPROCESSRECEIVEMASTER", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            FillRecDetails();
            fillGrid();
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
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRecDetail, "select$" + e.Row.RowIndex);
        }
    }
    protected void txtstockno_TextChanged(object sender, EventArgs e)
    {
        FillStockDetail();
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
        Fillstockno(varIssueDetailID);
    }
    protected void btnsavefrmgrid_Click(object sender, EventArgs e)
    {
        //Get Empid 
        string StrEmpid = "";
        for (int i = 0; i < listWeaverName.Items.Count; i++)
        {
            if (StrEmpid == "")
            {
                StrEmpid = listWeaverName.Items[i].Value;
            }
            else
            {
                StrEmpid = StrEmpid + "," + listWeaverName.Items[i].Value;
            }
        }
        //Check Employee Entry
        if (StrEmpid == "")
        {
            lblmessage.Text = "Plz Enter Weaver ID No...";
            return;
        }
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
        if (TStockNo == "")
        {
            lblmessage.Text = "Plz select atleast one stock no...";
            return;
        }

        string DetailTable = "", QAName = "", ReturnRemak = "";

       //// QAName = DDQaname.SelectedIndex > 0 ? DDQaname.SelectedItem.Text : "";

        
        if (ddStockQualityType.SelectedValue == "4")
        {
            if (Tdreturnremark.Visible == true)
            {
                if (txtretremark.Text == "")
                {
                    lblmessage.Text = "Please enter Return Remark.";
                    txtretremark.Focus();
                    return;
                }
            }
            else
            {
                ReturnRemak = txtretremark.Text;
            }
        }

        //for (int i = 0; i < DG.Rows.Count; i++)
        //{
        //    Label lblissuedetailID = (Label)DG.Rows[i].FindControl("lblissuedetailid");
        //    if (Convert.ToInt32(lblissuedetailID.Text) == varIssueDetailID)
        //    {
        //        Label lblorderid = (Label)DG.Rows[i].FindControl("lblorderid");
        //        Label lblItemFinishedId = (Label)DG.Rows[i].FindControl("lblItemFinishedId");               
        //        Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
        //        Label lblissueorderid = (Label)DG.Rows[i].FindControl("lblissueorderid");
        //        Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");

        //        if (DetailTable == "")
        //        {
        //            DetailTable = lblorderid.Text + "|" + lblItemFinishedId.Text + "|" + txtlength.Text + "|" + txtwidth.Text + "|" +
        //                 TxtRecQty.Text + "|" + txtarea.Text + "|" + lblrate.Text + "|" + 
        //                lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "|" + ddStockQualityType.SelectedValue + "|" +
        //                QAName + "|" + ReturnRemak + "~";
        //        }
        //        else
        //        {
        //            DetailTable = DetailTable + lblorderid.Text + "|" + lblItemFinishedId.Text + "|" + txtlength.Text + "|" + txtwidth.Text + "|" +
        //                TxtRecQty.Text + "|" + txtarea.Text + "|" + lblrate.Text + "|" + 
        //                lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "|" + ddStockQualityType.SelectedValue + "|" +
        //                 QAName + "|" + ReturnRemak + "~";
        //        }
        //    }
        //}

        if (TStockNo != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("PRO_FIRSTPROCESSRECEIVE_BULK_STOCKNO_WISE", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                cmd.Parameters.Add("@PROCESS_REC_ID", SqlDbType.Int);
                cmd.Parameters["@PROCESS_REC_ID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@PROCESS_REC_ID"].Value = hnprocessrecid.Value;

                cmd.Parameters.AddWithValue("@COMPANYID", DDcompany.SelectedValue);
                cmd.Parameters.AddWithValue("@ProcessId", DDProcessName.SelectedValue);
                //cmd.Parameters.AddWithValue("@PRODUCTIONUNIT", DDProdunit.SelectedValue);
                //cmd.Parameters.AddWithValue("@LOOMID", txtloomid.Text);
                cmd.Parameters.AddWithValue("@ISSUEORDERID", DDFolioNo.SelectedValue);
                cmd.Parameters.Add("@RECEIVENO", SqlDbType.VarChar, 50);
                cmd.Parameters["@RECEIVENO"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@RECEIVEDATE", txtrecdate.Text);
                cmd.Parameters.AddWithValue("@TSTOCKNO", TStockNo);
                //cmd.Parameters.AddWithValue("@Checkedby", txtcheckedby.Text);
                //cmd.Parameters.AddWithValue("@WEIGHT", txtstockweight.Text == "" ? "0" : txtstockweight.Text);
                cmd.Parameters.AddWithValue("@Penalityamt", txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text); 
                cmd.Parameters.AddWithValue("@Penalityremark", txtpenalityremarks.Text.Trim());
                cmd.Parameters.AddWithValue("@QUALITYTYPE", ddStockQualityType.SelectedValue);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 500);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);

                cmd.Parameters.Add("@MaxRejectedGatePassNo", SqlDbType.Int);
                cmd.Parameters["@MaxRejectedGatePassNo"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@MaxRejectedGatePassNo"].Value = hnRejectedGatePassNo.Value;

                cmd.Parameters.AddWithValue("@Returnremark", txtretremark.Text);
                cmd.Parameters.AddWithValue("@LENGTH", txtlength.Text.Trim());
                cmd.Parameters.AddWithValue("@Width", txtwidth.Text.Trim());
                cmd.Parameters.AddWithValue("@Area", txtarea.Text == "" ? "0" : txtarea.Text);
                ///cmd.Parameters.AddWithValue("@DetailTable", DetailTable);

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
                    hnlastfoliono.Value = DDFolioNo.SelectedValue;
                    hnRejectedGatePassNo.Value = cmd.Parameters["@MaxRejectedGatePassNo"].Value.ToString();
                    FillRecDetails();
                    fillGrid();
                    lblmessage.Text = "Data saved successfully...";
                    txtstockno.Text = "";
                    DGStockDetail.DataSource = null;
                    DGStockDetail.DataBind();
                    ddStockQualityType.SelectedValue = "1";
                    Refreshcontrol(sender);
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

        ////for (int i = 0; i < DG.Rows.Count; i++)
        ////{
        ////    Label lblissuedetailID = (Label)DG.Rows[i].FindControl("lblissuedetailid");
        ////    if (Convert.ToInt32(lblissuedetailID.Text) == varIssueDetailID)
        ////    {
        ////        Label lblorderid = (Label)DG.Rows[i].FindControl("lblorderid");
        ////        Label lblOrder_FinishedID = (Label)DG.Rows[i].FindControl("lblOrder_FinishedID");
        ////        Label lblOrderDetailDetail_FinishedID = (Label)DG.Rows[i].FindControl("lblOrderDetailDetail_FinishedID");
        ////        Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
        ////        Label lblissueorderid = (Label)DG.Rows[i].FindControl("lblissueorderid");
        ////        Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");

        ////        if (DetailTable == "")
        ////        {
        ////            DetailTable = lblorderid.Text + "|" + lblOrder_FinishedID.Text + "|" + lblOrderDetailDetail_FinishedID.Text + "|" + txtlength.Text + "|" + txtwidth.Text + "|" +
        ////                txtlength.Text + "|" + txtwidth.Text + "|" + TxtRecQty.Text + "|" + txtarea.Text + "|" + lblrate.Text + "|" + txtstockweight.Text + "|" +
        ////                lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "|" + ddStockQualityType.SelectedValue + "|" +
        ////                txtcheckedby.Text + "|" + QAName + "|" + ReturnRemak + "~";
        ////        }
        ////        else
        ////        {
        ////            DetailTable = DetailTable + lblorderid.Text + "|" + lblOrder_FinishedID.Text + "|" + lblOrderDetailDetail_FinishedID.Text + "|" + txtlength.Text + "|" + txtwidth.Text + "|" +
        ////                txtlength.Text + "|" + txtwidth.Text + "|" + TxtRecQty.Text + "|" + txtarea.Text + "|" + lblrate.Text + "|" + txtstockweight.Text + "|" +
        ////                lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "|" + ddStockQualityType.SelectedValue + "|" +
        ////                txtcheckedby.Text + "|" + QAName + "|" + ReturnRemak + "~";
        ////        }
        ////    }
        ////}

        ////SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        ////if (con.State == ConnectionState.Closed)
        ////{
        ////    con.Open();
        ////}
        ////SqlTransaction Tran = con.BeginTransaction();
        ////try
        ////{
        ////    SqlCommand cmd = new SqlCommand("PRO_SAVEHOMEFURNISHINGRECEIVEMASTER", con, Tran);
        ////    cmd.CommandType = CommandType.StoredProcedure;
        ////    cmd.CommandTimeout = 300;

        ////    cmd.Parameters.Add("@ProcessRecID", SqlDbType.Int);
        ////    cmd.Parameters["@ProcessRecID"].Direction = ParameterDirection.InputOutput;
        ////    cmd.Parameters["@ProcessRecID"].Value = hnprocessrecid.Value;

        ////    cmd.Parameters.AddWithValue("@CompanyID", DDcompany.SelectedValue);
        ////    cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
        ////    cmd.Parameters.AddWithValue("@ReceiveDate", txtrecdate.Text);
        ////    cmd.Parameters.AddWithValue("@Status", "");
        ////    cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar, 50);
        ////    cmd.Parameters["@ChallanNo"].Direction = ParameterDirection.Output;
        ////    cmd.Parameters.AddWithValue("@Remarks", "");
        ////    cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
        ////    cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
        ////    cmd.Parameters.AddWithValue("@DetailTable", DetailTable);
        ////    cmd.Parameters.AddWithValue("@EmpIDS", StrEmpid);
        ////    cmd.Parameters.AddWithValue("@TStockNo", TStockNo);
        ////    cmd.Parameters.Add("@GatePassNo", SqlDbType.Int);
        ////    cmd.Parameters["@GatePassNo"].Direction = ParameterDirection.Output;
        ////    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 500);
        ////    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

        ////    cmd.ExecuteNonQuery();
        ////    if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
        ////    {
        ////        ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "');", true);
        ////        lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
        ////        Tran.Rollback();
        ////    }
        ////    else
        ////    {
        ////        Tran.Commit();
        ////        txtreceiveno.Text = cmd.Parameters["@ChallanNo"].Value.ToString();
        ////        hnprocessrecid.Value = cmd.Parameters["@ProcessRecID"].Value.ToString();
        ////        FillRecDetails();
        ////        lblmessage.Text = "Data saved successfully...";
        ////        txtstockno.Text = "";
        ////        DGStockDetail.DataSource = null;
        ////        DGStockDetail.DataBind();
        ////        ddStockQualityType.SelectedValue = "1";
        ////        Refreshcontrol(sender);
        ////    }
        ////    txtstockno.Focus();
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
    }
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
        if (txteditempcode.Text != "")
        {
            ProcessNameSelectedChanged();
        }
    }
    protected void FillStockDetail()
    {
        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@TstockNo", txtstockno.Text);
            param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETFIRSTPROCESSSTOCKDETAIL", param);
            Refreshcontrol();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtwidth.Text = ds.Tables[0].Rows[0]["width"].ToString();
                txtlength.Text = ds.Tables[0].Rows[0]["Length"].ToString();
                txtarea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                //txtcommrate.Text = ds.Tables[0].Rows[0]["comm"].ToString();
                hnunitid.Value = ds.Tables[0].Rows[0]["unitid"].ToString();
                hncaltype.Value = ds.Tables[0].Rows[0]["caltype"].ToString();
                DDcompany.SelectedValue = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                DDProcessName.SelectedValue = ds.Tables[0].Rows[0]["PROCESSID"].ToString();
                FillFolioNo();
                TxtRecQty.Text = "1";
                varWidth = ds.Tables[0].Rows[0]["width"].ToString();
                varLength = ds.Tables[0].Rows[0]["Length"].ToString();

                if (DDFolioNo.Items.FindByValue(ds.Tables[0].Rows[0]["issueorderid"].ToString()) != null)
                {
                    DDFolioNo.SelectedValue = ds.Tables[0].Rows[0]["issueorderid"].ToString();
                    FolioNoSelectedChanged();
                    varIssueDetailID = Convert.ToInt32(ds.Tables[0].Rows[0]["Issue_Detail_Id"]);
                    Fillstockno(varIssueDetailID);
                }
                else
                {
                    DDFolioNo.SelectedIndex = -1;
                    lblmessage.Text = "Please select Folio No.";
                    return;
                }
            }
            else
            {
                lblmessage.Text = "Stock No. does not exists or Pending.";
                if (DDFolioNo.Items.Count > 0)
                {
                    DDFolioNo.SelectedIndex = 0;
                }
                DG.DataSource = null;
                DG.DataBind();
                DGStockDetail.DataSource = null;
                DGStockDetail.DataBind();

                return;
            }
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
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
            int Shape = 0;

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
    protected void TxtReceiveQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        if (DDFolioNo.SelectedIndex > 0)
        {
            //DGStockDetail.PageSize = Convert.ToInt32(TxtReceiveQty.Text);
            Fillstockno(varIssueDetailID);
        }
    }
    protected void DG_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rowindex = DG.SelectedRow.RowIndex;
        //Convert.ToInt32(DG.SelectedValue);
        Label lblissuedetailid = (Label)DG.Rows[rowindex].FindControl("lblissuedetailid");
        varIssueDetailID = Convert.ToInt32(lblissuedetailid.Text);
        Label lblwidth = (Label)DG.Rows[rowindex].FindControl("lblwidth");
        Label lbllength = (Label)DG.Rows[rowindex].FindControl("lbllength");
        Label lblarea = (Label)DG.Rows[rowindex].FindControl("lblarea");
        Label lblPendingQty = (Label)DG.Rows[rowindex].FindControl("lblPendingQty");

        txtwidth.Text = lblwidth.Text;
        txtlength.Text = lbllength.Text;
        txtarea.Text = lblarea.Text;
        TxtRecQty.Text = lblPendingQty.Text;
        Fillstockno(varIssueDetailID);
    }
    protected void TxtRecQty_TextChanged(object sender, EventArgs e)
    {
        DGStockDetail.DataSource = null;
        DGStockDetail.DataBind();
        if (TxtRecQty.Text != "" && DDFolioNo.SelectedIndex > 0)
        {
            DGStockDetail.PageSize = Convert.ToInt32(TxtRecQty.Text);
            Fillstockno(varIssueDetailID);
        }
    }
}

