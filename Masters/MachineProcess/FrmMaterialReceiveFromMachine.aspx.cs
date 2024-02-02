using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_MachineProcess_FrmMaterialReceiveFromMachine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName 
                           select UnitsId,UnitName from Units order by UnitName
                            select PROCESS_NAME_ID,PROCESS_NAME from PROCESS_NAME_MASTER Where ProcessType=1 and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" and Process_Name='WEAVING' order by PROCESS_NAME_ID
                            select MachineNoId,MachineNoName From MachineNoMaster(Nolock) order by MachineNoName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDMachineNo, ds, 3, true, "--Plz Select--");

            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
            }


//            string str2 = "";
//            if (Session["VarCompanyNo"].ToString() == "21")
//            {
//                str2 = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  
//                Where  GM.GodownName in('SPARE TANA','YARN OPENING TANA','TANA HOUSE') and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName ";
//            }
//            else
//            {
//                str2 = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName ";
//            }
//            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);

//            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds2, 0, true, "--Plz Select--");
//            if (DDGodown.Items.Count > 0)
//            {
//                DDGodown.SelectedIndex = 1;
//            }

            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
            }
            ////********
            //if (Session["usertype"].ToString() == "1")
            //{
            //    TDcomplete.Visible = true;
            //}
        }
    }

    protected void DDMachineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        MachineNoSelectedChanged();
    }
    private void MachineNoSelectedChanged()
    {
        //ViewState["MaterialIssueID"] = 0;
        hnMaterialReceiveId.Value = "0";
        txtReceiveNo.Text = "";
        //txtchalanno.Text = "";
        
         string str = @"Select MaterialIssueId,IssueNo from MaterialIssueOnMachineMaster where CompanyId=" + DDcompany.SelectedValue + " and ProcessId=" + DDProcessName.SelectedValue + " and ProductionUnitId=" + DDProdunit.SelectedValue + " and MachineNoId=" + DDMachineNo.SelectedValue + "";

         str = str + " order by MaterialIssueId";
         UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "-Select Issue No-");
        
    }   
    //protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    // FillGrid();
    //}
    protected void btnsave_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGIssueDetail.Rows[i].FindControl("Chkboxitem"));
            TextBox txtReceiveQty = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtReceiveQty"));
            Label lblBalToRecQty = ((Label)DGIssueDetail.Rows[i].FindControl("lblBalToRecQty"));

            //if (Chkboxitem.Checked == false)   // Change when Updated Completed
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
            //    return;
            //}

            if (txtReceiveQty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be blank');", true);
                txtReceiveQty.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtReceiveQty.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty always greater then zero');", true);
                txtReceiveQty.Focus();
                return;
            }
            if (Convert.ToDecimal(txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text) > Convert.ToDecimal(lblBalToRecQty.Text) && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be greater than balance qty');", true);
                txtReceiveQty.Text = "";
                txtReceiveQty.Focus();
                return;
            }
        }
        string Strdetail = "";
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGIssueDetail.Rows[i].FindControl("Chkboxitem"));
            TextBox txtReceiveQty = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtReceiveQty"));
            Label lblBalToRecQty = ((Label)DGIssueDetail.Rows[i].FindControl("lblBalToRecQty"));
            Label lblConsumeQty = ((Label)DGIssueDetail.Rows[i].FindControl("lblConsumeQty"));
            Label lblIssueQty = ((Label)DGIssueDetail.Rows[i].FindControl("lblIssueQty"));
            Label lblAlreadyReceivedQty = ((Label)DGIssueDetail.Rows[i].FindControl("lblAlreadyReceivedQty"));
            Label lblMaterialIssueId = ((Label)DGIssueDetail.Rows[i].FindControl("lblMaterialIssueId"));
            Label lblMaterialIssueDetailId = ((Label)DGIssueDetail.Rows[i].FindControl("lblMaterialIssueDetailId"));
            Label lblMachineNoId = ((Label)DGIssueDetail.Rows[i].FindControl("lblMachineNoId"));
            Label lblprocessid = ((Label)DGIssueDetail.Rows[i].FindControl("lblprocessid"));
            Label lblUnitId = ((Label)DGIssueDetail.Rows[i].FindControl("lblUnitId"));
            Label lblFinishedId = ((Label)DGIssueDetail.Rows[i].FindControl("lblFinishedId"));
            Label lblIssueNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblIssueNo"));
            Label lblGodownId = ((Label)DGIssueDetail.Rows[i].FindControl("lblGodownId"));
            Label lblLotNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblLotNo"));
            Label lblTagNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblTagNo"));

            if (Chkboxitem.Checked == true && (txtReceiveQty.Text != "") && DDProdunit.SelectedIndex > 0 && DDIssueNo.SelectedIndex > 0 && DDProcessName.SelectedIndex > 0 && DDMachineNo.SelectedIndex > 0)
            {
                Strdetail = Strdetail + lblIssueQty.Text + '|' + lblMaterialIssueId.Text + '|' + lblMaterialIssueDetailId.Text + '|' + lblMachineNoId.Text + '|' + lblprocessid.Text + '|' + lblUnitId.Text + '|' + lblFinishedId.Text + '|' + lblIssueNo.Text + '|' + txtReceiveQty.Text + '|' + lblGodownId.Text + '|' + lblLotNo.Text + '|' + lblTagNo.Text + '~';
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
            SqlParameter[] param = new SqlParameter[12];
            param[0] = new SqlParameter("@MaterialReceiveId", SqlDbType.Int);
            if (chkEdit.Checked == true)
            {
                param[0].Value = DDReceiveNo.SelectedValue;
            }
            else
            {
                param[0].Value = hnMaterialReceiveId.Value;
            }

            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@ProductionUnitId", DDProdunit.SelectedValue);
            param[4] = new SqlParameter("@MachineNoId", DDMachineNo.SelectedValue);
            param[5] = new SqlParameter("@IssueNoId", DDIssueNo.SelectedValue);
            param[6] = new SqlParameter("@ReceiveNo", txtReceiveNo.Text);
            param[7] = new SqlParameter("@ReceiveDate", txtReceiveDate.Text);
            param[8] = new SqlParameter("@UserID", Session["varuserid"]);
            param[9] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[10] = new SqlParameter("@StringDetail", Strdetail);
            param[11] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;


            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveMaterialReceiveFromMachine", param);
            //*******************

            lblmessage.Text = param[11].Value.ToString();
            txtReceiveNo.Text = param[0].Value.ToString();
            hnMaterialReceiveId.Value = param[0].Value.ToString();
            Tran.Commit();
            //refreshcontrol();
            Fillissuedetail();
            FillMaterialReceiveDetail();



            //if (param[11].Value.ToString() != "")
            //{
            //    lblmessage.Text = param[11].Value.ToString();
            //}
            //else
            //{
            //    lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
            //    //Fillgrid();
            //   // FillissueGrid();
            //}
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
    protected void refreshcontrol()
    {
        DDMachineNo.SelectedIndex =- 1;
        DDIssueNo.SelectedIndex = -1;
     
        //DG.DataSource = null;
        // DG.DataBind();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[2];
        param[0] = new SqlParameter("@processid", DDProcessName.SelectedValue);
        param[1] = new SqlParameter("@MaterialReceiveId", hnMaterialReceiveId.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_MATERIALRECEIVEFROMMACHINEREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptMaterialReceiveFromMachine.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptMaterialReceiveFromMachine.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        DDMachineNo.SelectedIndex = -1;
        DDIssueNo.SelectedIndex = -1;
        TDissue.Visible = false;
        TDReceiveNo.Visible = false;
        // DG.DataSource = null;
        // DG.DataBind();
        DGIssueDetail.DataSource = null;
        DGIssueDetail.DataBind();
        txtReceiveNo.Text = "";
        txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        DGReceiveDetail.DataSource = null;
        DGReceiveDetail.DataBind();

        if (chkEdit.Checked == true)
        {
            TDissue.Visible = true;
            TDReceiveNo.Visible = true;
            DDReceiveNo.SelectedIndex = -1;
        }
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
       hnMaterialReceiveId.Value = DDReceiveNo.SelectedValue;
        FillMaterialReceiveDetail();
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hnprmid.Value = DDissueno.SelectedValue;
        // Fillissuedetail();

        if (chkEdit.Checked == true)
        {
            string str = @"Select distinct MRM.MaterialReceiveId,MRM.ReceiveNo from MaterialReceiveFromMachineMaster MRM JOIN MaterialReceiveFromMachineDetail MRD ON MRM.MaterialReceiveId=MRD.MaterialReceiveId
                            Where MRD.MaterialIssueId=" + DDIssueNo.SelectedValue + "";           
            UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");
        }
        else
        {
            Fillissuedetail();
        }
    }
    protected void Fillissuedetail()
    {
        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@MaterialIssueId",DDIssueNo.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserid"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIssueMaterialOnMachineDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIssueDetail.DataSource = ds.Tables[0];
            DGIssueDetail.DataBind();

            //if (Session["VarCompanyNo"].ToString() != "21")
            //{
            //    DDGodown.Enabled = false;
            //    DDGodown.SelectedValue = ds.Tables[0].Rows[0]["GodownId"].ToString();
            //}

        }
        else
        {
            DGIssueDetail.DataSource = null;
            DGIssueDetail.DataBind();
        }       

    }
    protected void FillMaterialReceiveDetail()
    {

        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@MaterialReceiveId", hnMaterialReceiveId.Value);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserid"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetMaterialReceiveFromMachineDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGReceiveDetail.DataSource = ds.Tables[0];
            DGReceiveDetail.DataBind();
        }
        else
        {
            DGReceiveDetail.DataSource = null;
            DGReceiveDetail.DataBind();
        }

        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
                txtReceiveDate.Text = ds.Tables[0].Rows[0]["Receivedate"].ToString();
            }
        }
        //else
        //{
        //    txtReceiveNo.Text = "";
        //    txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //}


    }
    protected void DGReceiveDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblMaterialReceiveId = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblMaterialReceiveId");
            Label lblMaterialReceiveDetailId = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblMaterialReceiveDetailId");
            Label lblMaterialIssueId = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblMaterialIssueId");
            Label lblFinishedId = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblFinishedId");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@MaterialReceiveId", lblMaterialReceiveId.Text);
            param[1] = new SqlParameter("@MaterialReceiveDetailId", lblMaterialReceiveDetailId.Text);
            param[2] = new SqlParameter("@MaterialIssueId", lblMaterialIssueId.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@UserId", Session["VarUserid"]);
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteMaterialReceiveFromMachine", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            Fillissuedetail();
            FillMaterialReceiveDetail();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    if (txtloomid.Text != "")
    //    {
    //        FillFolioNo();
    //    }
    //}
}