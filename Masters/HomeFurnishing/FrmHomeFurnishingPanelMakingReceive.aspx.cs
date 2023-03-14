using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_HomeFurnishing_FrmHomeFurnishingPanelMakingReceive : System.Web.UI.Page
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
                typeof(Masters_HomeFurnishing_FrmHomeFurnishingPanelMakingReceive),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);

            string str = @"select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock) 
                        JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CompanyName 
                        Select Distinct a.ProcessID, PNM.PROCESS_NAME 
                        From HomeFurnishingMakingOrderMaster a(nolock)
                        JOIN PROCESS_NAME_MASTER PNM(nolock) ON PNM.PROCESS_NAME_ID = a.PROCESSID 
                        Where a.MASTERCOMPANYID = " + Session["varCompanyId"] + @" 
                        Order By PNM.PROCESS_NAME ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            ProcessNameSelectedChanged();
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
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
            str = @"Select Distinct PRM.ProcessRecId, PRM.ChallanNo + ' /' + REPLACE(CONVERT(nvarchar(11), PRM.ReceiveDate, 106), ' ', '-') ChallanNo 
                    From HomeFurnishingMakingReceiveMaster PRM(Nolock)  
                    JOIN HomeFurnishingMakingReceiveDetail PRD(Nolock) ON PRD.ProcessRecId = PRM.ProcessRecId 
                    JOIN HomeFurnishingMakingOrderMaster PM(Nolock) ON PM.IssueOrderID = PRD.IssueOrderID ";
            if (txtfolionoedit.Text != "")
            {
                str = str + " And PM.ChallanNo = '" + txtfolionoedit.Text + "'";
            }

            str = str + @" JOIN Employee_HomeFurnishingMakingReceiveMaster EMP(Nolock) ON EMP.ProcessRecID = PRM.ProcessRecID and EMP.processid = PRM.ProcessID ";

            if (txteditempid.Text != "")
            {
                str = str + " And EMP.EMPID = " + txteditempid.Text;
            }
            str = str + " Where 1 = 1";
            if (DDProcessName.SelectedIndex > 0)
            {
                str = str + " And PRM.ProcessID = " + DDProcessName.SelectedValue;
            }
            str = str + @" Order By PRM.ProcessRecId ";

            UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
        }
        else
        {
            FillFolioNo();
        }
    }
    protected void FillFolioNo()
    {
        string str = @"select Distinct PM.IssueOrderId, PM.ChallanNo 
                from HomeFurnishingMakingOrderMaster PM(Nolock) 
                join Employee_HomeFurnishingMakingOrderMaster EMP(Nolock) on PM.issueorderid=EMp.issueorderid and EMP.processid= PM.ProcessID 
                Where PM.Status <> 'Canceled' And PM.Companyid = " + DDcompany.SelectedValue + " And PM.ProcessID = " + DDProcessName.SelectedValue;

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
    }
    protected void fillGrid()
    {
        SqlParameter[] param = new SqlParameter[6];
        param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
        param[1] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
        param[4] = new SqlParameter("@issueorderid", DDFolioNo.SelectedValue);

        //*************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_FillProductionDataHomeFurnishingMaking", param);
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
    protected void FillRecDetails()
    {
        SqlParameter[] array = new SqlParameter[2];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);
        array[1] = new SqlParameter("@FlagGridReport", 0);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HOMEFURNISHING_MAKING_RECEIVE_FILLGRID_REPORT", array);
        DGRecDetail.DataSource = ds.Tables[0];
        DGRecDetail.DataBind();
        if (chkedit.Checked == true)
        {
            txtreceiveno.Text = ds.Tables[1].Rows[0]["ChallanNo"].ToString();
            txtcheckedby.Text = ds.Tables[1].Rows[0]["Remarks"].ToString();
            txtrecdate.Text = ds.Tables[1].Rows[0]["ReceiveDate"].ToString();
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[2];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);
        array[1] = new SqlParameter("@FlagGridReport", 1);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_HOMEFURNISHING_MAKING_RECEIVE_FILLGRID_REPORT", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varcompanyid"].ToString())
            {
                case "16":
                case "28":
                    Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailchampo.rpt";
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
            param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETE_HOMEFURNISHING_MAKING_RECEIVEMASTER", param);
            lblmessage.Text = param[5].Value.ToString();
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
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGRecDetail, "select$" + e.Row.RowIndex);
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
    protected void BtnSave_Click(object sender, EventArgs e)
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
        string DetailTable = "";

        for (int i = 0; i < DG.Rows.Count; i++)
        {            
            CheckBox Chkboxitem = (CheckBox)DG.Rows[i].FindControl("Chkboxitem");
            if (Chkboxitem.Checked == true)
            {
                Label lblorderid = (Label)DG.Rows[i].FindControl("lblorderid");
                Label lblItem_Finished_ID = (Label)DG.Rows[i].FindControl("lblItem_Finished_ID");
                Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
                Label lblissueorderid = (Label)DG.Rows[i].FindControl("lblissueorderid");
                Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
                Label lbllength = (Label)DG.Rows[i].FindControl("lbllength");
                Label lblWidth = (Label)DG.Rows[i].FindControl("lblwidth");
                TextBox TxtRecQty = (TextBox)DG.Rows[i].FindControl("txtrecqty");
                Label Lblarea = (Label)DG.Rows[i].FindControl("lblarea");
                TextBox txtstockweight = (TextBox)DG.Rows[i].FindControl("txtweight");
                TextBox txtpenalityamt = (TextBox)DG.Rows[i].FindControl("txtPenality");
                TextBox txtpenalityremarks = (TextBox)DG.Rows[i].FindControl("TxtPenalityRemark");
                
                if (DetailTable == "")
                {
                    DetailTable = lblorderid.Text + "|" + lblItem_Finished_ID.Text + "|" + lbllength.Text + "|" + lblWidth.Text + "|" + TxtRecQty.Text + "|" + Lblarea.Text + "|" + lblrate.Text + "|" + txtstockweight.Text + "|" +
                        lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "~";
                }
                else
                {
                    DetailTable = DetailTable + lblorderid.Text + "|" + lblItem_Finished_ID.Text + "|" + lbllength.Text + "|" + lblWidth.Text + "|" + TxtRecQty.Text + "|" + Lblarea.Text + "|" + lblrate.Text + "|" + txtstockweight.Text + "|" +
                        lblissueorderid.Text + "|" + lblissuedetailid.Text + "|" + txtpenalityamt.Text + "|" + txtpenalityremarks.Text + "~";
                }
            }
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("PRO_SAVEHOMEFURNISHING_MAKING_RECEIVEMASTER", con, Tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000;

            cmd.Parameters.Add("@ProcessRecID", SqlDbType.Int);
            cmd.Parameters["@ProcessRecID"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@ProcessRecID"].Value = hnprocessrecid.Value;

            cmd.Parameters.AddWithValue("@CompanyID", DDcompany.SelectedValue);
            cmd.Parameters.AddWithValue("@ProcessID", DDProcessName.SelectedValue);
            cmd.Parameters.AddWithValue("@ReceiveDate", txtrecdate.Text);
            cmd.Parameters.AddWithValue("@Status", "");
            cmd.Parameters.Add("@ChallanNo", SqlDbType.VarChar, 50);
            cmd.Parameters["@ChallanNo"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@Remarks", txtcheckedby.Text);
            cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
            cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyId"]);
            cmd.Parameters.AddWithValue("@DetailTable", DetailTable);
            cmd.Parameters.AddWithValue("@EmpIDS", StrEmpid);
            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 500);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@msg"].Value.ToString() + "');", true);
                lblmessage.Text = cmd.Parameters["@msg"].Value.ToString();
                Tran.Rollback();
            }
            else
            {
                Tran.Commit();
                txtreceiveno.Text = cmd.Parameters["@ChallanNo"].Value.ToString();
                hnprocessrecid.Value = cmd.Parameters["@ProcessRecID"].Value.ToString();
                FillRecDetails();
                fillGrid();
                lblmessage.Text = "Data saved successfully...";
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
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
        if (txteditempcode.Text != "")
        {
            ProcessNameSelectedChanged();
        }
    }
    protected void txtrecdate_TextChanged(object sender, EventArgs e)
    {
        hnprocessrecid.Value = "0";
    }
    protected void TxtReceiveQty_TextChanged(object sender, EventArgs e)
    {
        if (DDFolioNo.SelectedIndex > 0)
        {

        }
    }
}

