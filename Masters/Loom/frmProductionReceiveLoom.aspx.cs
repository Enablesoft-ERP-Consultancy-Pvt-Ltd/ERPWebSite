using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Loom_frmProductionReceiveLoom : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName                           
                           select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");
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
            //**********************
            if (variable.VarMaintainStockSeries == "1")
            {
                TBStockseries.Visible = true;
                if (TBStockseries.Visible == true)
                {
                    if (variable.VarSTOCKNOPREFIX != "")
                    {
                        txtprefix.Text = variable.VarSTOCKNOPREFIX;
                    }
                    txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix(txtprefix.Text));
                }
            }

        }

    }
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        AutoCompleteExtenderloomno.ContextKey = "0#0#" + DDProdunit.SelectedValue;
        //        if (chkedit.Checked)
        //        {
        //            str = @"select Distinct PL.UID,PL.LoomNo from PROCESS_ISSUE_MASTER_1 PM inner join ProductionLoomMaster PL
        //                    on PM.LoomId=PL.UID and PM.Status<>'Canceled'
        //                    And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue;
        //        }
        //        else
        //        {
        //            str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(Pl.LoomNo)=1 Then cast(Pl.LoomNo as Int) ELse 999999 End as LoomNo1 
        //                    from PROCESS_ISSUE_MASTER_1 PM inner join ProductionLoomMaster PL
        //                    on PM.LoomId=PL.UID  WHere PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + " order by LoomNo1,Pl.LoomNo";

        //        }
        //        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Plz Select--");
        if (txteditempcode.Text != "" || txtfoliono.Text != "")
        {
            TDloomno.Visible = true;
            TDLoomNotextbox.Visible = false;
            str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(Pl.LoomNo)=1 Then cast(Pl.LoomNo as Int) ELse 999999 End as LoomNo1 
                    From PROCESS_ISSUE_MASTER_1 PM ";
            if (chkedit.Checked == false)
            {
                str = str + @" JOIN PROCESS_ISSUE_DETAIL_1 PD ON PD.IssueOrderID = PM.IssueOrderID And PD.PQty > 0 ";
            }

            str = str + @" inner join ProductionLoomMaster PL on PM.LoomId=PL.UID 
                    Inner join EMployee_Processorderno emp on PM.issueorderid=emp.issueorderid and emp.processid=1  
                    WHere PM.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue;
            if (txteditempcode.Text != "")
            {
                str = str + " and Emp.empid=" + txteditempid.Text;
            }
            if (txtfoliono.Text != "")
            {
                ////str = str + " and PM.issueorderid=" + txtfoliono.Text;

                str = str + " and PM.ChallanNo='" + txtfoliono.Text + "'";
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
            TDloomno.Visible = false;
            TDLoomNotextbox.Visible = true;
        }
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        txtloomid.Text = DDLoomNo.SelectedValue;
        if (chkedit.Checked == true)
        {
            if (variable.VarLoomNoGenerated == "1")
            {
                str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
                from PROCESS_ISSUE_MASTER_1 PM 
                inner join EMployee_Processorderno EMP on PM.issueorderid=EMp.issueorderid and EMP.processid=1
                left join LoomstockNo ls on pm.issueorderid=ls.issueorderid And ls.ProcessID = 1 
                where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + "  and ls.issueorderid is null and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + "";
            }
            else
            {
                str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
                from PROCESS_ISSUE_MASTER_1 PM 
                inner join EMployee_Processorderno EMP on PM.issueorderid=EMp.issueorderid and EMP.processid=1
                where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + "";
            }
            if (txteditempcode.Text != "")
            {
                str = str + " and EMP.EMPID=" + txteditempid.Text + "";
            }
            if (txtfoliono.Text != "")
            {
                ////str = str + " and PM.issueorderid=" + txtfoliono.Text + "";

                str = str + " and PM.ChallanNo='" + txtfoliono.Text + "'";
            }
        }
        else
        {
            if (variable.VarLoomNoGenerated == "1")
            {
                str = @"select Distinct PM.IssueOrderId,PM.ChallanNo 
                    from PROCESS_ISSUE_MASTER_1 PM  
                    INNER JOIN  Employee_ProcessorderNo EMP on PM.issueorderid=EMP.issueorderid and EMP.Processid=1
                    Left Join LoomStockNo LS on Pm.issueorderid=Ls.issueorderid And LS.ProcessID = 1 
                    where  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Status='Pending' and PM.Status<>'canceled' and ls.issueorderid is null and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
            }
            else
            {
                str = @"select Distinct PM.IssueOrderId,PM.ChallanNo from PROCESS_ISSUE_MASTER_1 PM  INNER JOIN  Employee_ProcessorderNo EMP on PM.issueorderid=EMP.issueorderid and EMP.Processid=1
                    where  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Status='Pending' and PM.Status<>'canceled' and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
            }
            if (txteditempcode.Text != "")
            {
                str = str + " and EMP.EMPID=" + txteditempid.Text + "";
            }
            if (txtfoliono.Text != "")
            {
                //// str = str + " and PM.issueorderid=" + txtfoliono.Text + "";

                str = str + " and PM.ChallanNo='" + txtfoliono.Text + "'";
            }
        }
        if (Session["varcompanyId"].ToString() == "28")
        {
            str = str + " and PM.IssueOrderId in (Select IssueOrderID From Process_Issue_Detail_1 Where OrderID < 730)";
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
        if (chkedit.Checked == true)
        {
            str = @"select Distinct PRM.Process_Rec_Id,PRM.ChallanNo+' /'+REPLACE(CONVERT(nvarchar(11),receivedate,106),' ','-') As ChallanNo from Process_receive_master_1 PRM inner join 
                PROCESS_RECEIVE_DETAIL_1 PRD on PRM.Process_Rec_Id=PRD.Process_Rec_Id
                where PRD.IssueOrderId=" + DDFolioNo.SelectedValue + " order by Process_Rec_Id";
            UtilityModule.ConditionalComboFill(ref DDreceiveNo, str, true, "--Plz Select--");
        }
        fillGrid();
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
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listWeaverName.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Empname"].ToString(), ds.Tables[0].Rows[i]["Empid"].ToString()));
            }
        }
        //
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
        dtrecords.Columns.Add("StockStatus", typeof(int));
        dtrecords.Columns.Add("StockNoremarks", typeof(string));
        dtrecords.Columns.Add("Penalityamt", typeof(float));
        dtrecords.Columns.Add("Penalityremark", typeof(string));
        //
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtrecqty = ((TextBox)DG.Rows[i].FindControl("txtrecqty"));
            if (Chkboxitem.Checked == true && Convert.ToInt16(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0)
            {
                Label lblitemfinishedid = (Label)DG.Rows[i].FindControl("lblitemfinishedid");
                TextBox lbllength = (TextBox)DG.Rows[i].FindControl("txtlength");
                TextBox lblwidth = (TextBox)DG.Rows[i].FindControl("txtwidth");
                Label lblarea = (Label)DG.Rows[i].FindControl("lblarea");
                Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
                Label lblissueorderid = (Label)DG.Rows[i].FindControl("lblissueorderid");
                Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
                Label lblorderid = (Label)DG.Rows[i].FindControl("lblorderid");
                Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
                Label lblcaltype = (Label)DG.Rows[i].FindControl("lblcaltype");
                Label lblorderedqty = (Label)DG.Rows[i].FindControl("lblorderedqty");
                TextBox txtweight = (TextBox)DG.Rows[i].FindControl("txtweight");
                DropDownList ddStockQualityType = (DropDownList)DG.Rows[i].FindControl("ddStockQualityType");
                TextBox txtremark = (TextBox)DG.Rows[i].FindControl("txtremark");
                TextBox txtpenalityamt = (TextBox)DG.Rows[i].FindControl("txtpenalityamt");
                TextBox txtpenalityremark = (TextBox)DG.Rows[i].FindControl("txtpenalityremark");

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
                dr["stockstatus"] = ddStockQualityType.SelectedValue;
                dr["StockNoremarks"] = txtremark.Text.Trim();
                dr["Penalityamt"] = txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text;
                dr["Penalityremark"] = txtpenalityremark.Text.Trim();
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
                param[12] = new SqlParameter("@Prefix", TBStockseries.Visible == false ? "" : txtprefix.Text);
                param[13] = new SqlParameter("@Postfix", TBStockseries.Visible == false ? "0" : (txtpostfix.Text == "" ? "0" : txtpostfix.Text));
                //**************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_ProductionLoomReceive", param);
                txtrecdate.Enabled = false;
                if (param[8].Value.ToString() != "")
                {
                    Tran.Rollback();
                    lblmessage.Text = param[8].Value.ToString();
                }
                else
                {
                    Tran.Commit();
                    txtreceiveno.Text = param[5].Value.ToString();
                    hnprocessrecid.Value = param[0].Value.ToString();
                    lblmessage.Text = "Data saved successfully...";
                    FillRecDetails();
                    Refreshcontrol();
                }
                //********Stock Series
                if (TBStockseries.Visible == true)
                {
                    txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((txtprefix.Text).ToUpper()));
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
    protected void Refreshcontrol()
    {
        DDLoomNo.SelectedIndex = -1;
        DDFolioNo.SelectedIndex = -1;
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void FillRecDetails()
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_FORPRODUCTIONRECEIVEREPORT_NEW", array);
        DGRecDetail.DataSource = ds.Tables[0];
        DGRecDetail.DataBind();
        //********EmpDetail
        listWeaverName.Items.Clear();
        if (ds.Tables[1].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                listWeaverName.Items.Add(new ListItem(ds.Tables[1].Rows[i]["Empname"].ToString(), ds.Tables[1].Rows[i]["Empid"].ToString()));
            }
        }
        //
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] array = new SqlParameter[1];
        array[0] = new SqlParameter("@Process_Rec_Id", hnprocessrecid.Value);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForProductionReceiveReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptProductionreceivedetailold.rpt";
            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptProductionreceivedetail.xsd";

            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else { ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); }
    }
    protected void DGRecDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGRecDetail.EditIndex = e.NewEditIndex;
        FillRecDetails();
    }
    protected void DGRecDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGRecDetail.EditIndex = -1;
        FillRecDetails();
    }
    protected void DDreceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprocessrecid.Value = DDreceiveNo.SelectedValue;
        FillRecDetails();
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == true)
        {
            TDreceiveNo.Visible = true;
            btnsave.Visible = false;
        }
        else
        {
            TDreceiveNo.Visible = false;
            btnsave.Visible = true;
        }
        //*****************
        txtloomno.Text = "";
        DDProdunit.SelectedIndex = -1;
        DDLoomNo.SelectedIndex = -1;
        DDFolioNo.SelectedIndex = -1;
        DDreceiveNo.Items.Clear();
        hnprocessrecid.Value = "0";
        txtrecdate.Enabled = true;
    }
    protected void DGRecDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

            for (int i = 0; i < DGRecDetail.Columns.Count; i++)
            {

                if (Session["varuserid"].ToString() == "1")
                {
                    if (DGRecDetail.Columns[i].HeaderText == "Rate")
                    {
                        DGRecDetail.Columns[i].Visible = true;
                    }

                }
                else
                {
                    if (DGRecDetail.Columns[i].HeaderText == "Rate")
                    {
                        DGRecDetail.Columns[i].Visible = false;
                    }

                }
            }

        }
    }
    protected void DGRecDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            TextBox txtweight = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtweight");
            TextBox txtpenalityamt = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtpenalityamt");
            Label lblrecqty = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblrecqty");
            Double weight = Convert.ToDouble(txtweight.Text) / Convert.ToDouble(lblrecqty.Text);
            Double Penality = Convert.ToDouble(txtpenalityamt.Text == "" ? "0" : txtpenalityamt.Text) / Convert.ToDouble(lblrecqty.Text);
            TextBox txtRate = (TextBox)DGRecDetail.Rows[e.RowIndex].FindControl("txtRate");
            Label lblReccaltype = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblReccaltype");
            Label lblRecArea = (Label)DGRecDetail.Rows[e.RowIndex].FindControl("lblRecArea");

            if (Convert.ToDouble(txtRate.Text) > 0)
            {
                Double Amount = 0;
                if (lblReccaltype.Text == "0")
                {
                    Amount = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text) * Convert.ToDouble(lblRecArea.Text);
                }
                else
                {
                    Amount = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
                }
                string str = "update Process_Receive_Detail_1 set Weight=Round(" + weight + ",3),Penality=ROUND(" + Penality + ",2),Rate=ROUND(" + txtRate.Text + ",2),Amount=ROUND(" + Amount + ",2) Where Process_Rec_Detail_Id in(" + lblprocessrecdetailid.Text + ")";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
                lblmessage.Text = "Rate or Weight or Penality updated successfully.";
                Tran.Commit();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please fill rate.');", true);
            }

            DGRecDetail.EditIndex = -1;
            FillRecDetails();
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
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Process_Rec_id", lblprocessrecid.Text);
            param[1] = new SqlParameter("@ReceiveDetailID", lblprocessrecdetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //***
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteProductionReceiveDetailLoomWise", param);
            lblmessage.Text = param[2].Value.ToString();
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
                chk.Enabled = true;
                chk.Checked = true;
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
                    dr["Reason"] = "";
                    dtrecord.Rows.Add(dr);
                }
            }
            //*********
            if (dtrecord.Rows.Count > 0)
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@dtrecord", dtrecord);
                param[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[1].Direction = ParameterDirection.Output;
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
        if (txtloomid.Text != "")
        {
            FillFolioNo();
        }
    }
    protected void FillFolioNo()
    {
        string str = "";
        if (chkedit.Checked == true)
        {
            switch (variable.VarLoomNoGenerated)
            {
                case "1":
                    str = @"select Distinct PM.IssueOrderId,PM.IssueOrderId 
                    from PROCESS_ISSUE_MASTER_1 PM 
                    Left join LoomstockNo Ls on Pm.issueorderid=Ls.Issueorderid And ls.ProcessID = 1 
                    where PM.Status<>'Canceled' and Ls.issueorderid is null and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + " order by PM.IssueOrderId";
                    break;
                default:
                    str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Status<>'Canceled' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text + " order by PM.IssueOrderId";
                    break;
            }
        }
        else
        {
            switch (variable.VarLoomNoGenerated)
            {
                case "1":
                    str = @"select PM.IssueOrderId,PM.IssueOrderId 
                    from PROCESS_ISSUE_MASTER_1 PM 
                    Left join LoomstockNo Ls on Pm.issueorderid=Ls.Issueorderid And ls.processid = 1 
                    where PM.Status<>'Canceled' and Ls.issueorderid is null and PM.status='Pending' and  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
                    break;
                default:
                    str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Status<>'Canceled' and PM.status='Pending' and  PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
                    break;
            }
        }
        if (Session["varcompanyId"].ToString() == "28")
        {
            str = str + " and PM.IssueOrderId in (Select IssueOrderID From Process_Issue_Detail_1 Where OrderID < 730)";
        }
        UtilityModule.ConditionalComboFill(ref DDFolioNo, str, true, "--Plz Select--");
    }
    protected void txtpostfix_TextChanged(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        string TStockNo = txtprefix.Text + txtpostfix.Text;
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select TStockNo from CarpetNumber Where TStockNo='" + TStockNo + "' And CompanyId=" + DDcompany.SelectedValue + "");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            lblmessage.Text = "Stock No AllReady Exits....";
            txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((txtprefix.Text).ToUpper()));
        }
    }
    protected void txtprefix_TextChanged(object sender, EventArgs e)
    {
        txtpostfix.Text = Convert.ToString(UtilityModule.CalculatePostFix((txtprefix.Text).ToUpper()));
    }
    protected void btnsearchedit_Click(object sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select EmpID From EmpInfo EI(Nolock) Where EmpCode = '" + txteditempcode.Text + "'");

        if (ds.Tables[0].Rows.Count > 0)
        {
            txteditempid.Text = ds.Tables[0].Rows[0]["EmpID"].ToString();
        }
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
            TDloomno.Visible = false;
            TDLoomNotextbox.Visible = true;
            txtloomid.Text = "";
        }
    }
    private void Check_Length_Width_Format(int rowindex = 0)
    {
        int FootLength = 0;
        int FootWidth = 0;
        int FootLengthInch = 0;
        int FootWidthInch = 0;
        string Str = "";

        TextBox Txtlength = (TextBox)DG.Rows[rowindex].FindControl("txtlength");
        TextBox TxtWidth = (TextBox)DG.Rows[rowindex].FindControl("txtwidth");
        Label lblunitid = (Label)DG.Rows[rowindex].FindControl("lblunitid");
        Label lblarea = (Label)DG.Rows[rowindex].FindControl("lblarea");
        Label lblcaltype = (Label)DG.Rows[rowindex].FindControl("lblcaltype");
        Label lblshapeid = (Label)DG.Rows[rowindex].FindControl("lblshapeid");

        if (Txtlength.Text != "")
        {
            if (Convert.ToInt32(lblunitid.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(Txtlength.Text));
                Txtlength.Text = Str;
                FootLength = Convert.ToInt32(Str.Split('.')[0]);
                FootLengthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootLengthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    Txtlength.Text = "";
                    Txtlength.Focus();
                }
            }
        }
        if (TxtWidth.Text != "")
        {
            if (Convert.ToInt32(lblunitid.Text) == 2)
            {
                Str = string.Format("{0:#0.00}", Convert.ToDouble(TxtWidth.Text));
                TxtWidth.Text = Str;
                FootWidth = Convert.ToInt32(Str.Split('.')[0]);
                FootWidthInch = Convert.ToInt32(Str.Split('.')[1]);
                if (FootWidthInch > 11)
                {
                    lblmessage.Text = "Inch value must be less than 12";
                    TxtWidth.Text = "";
                    TxtWidth.Focus();
                }
            }
        }
        if (Txtlength.Text != "" && TxtWidth.Text != "")
        {
            int Shape = Convert.ToInt16(lblshapeid.Text);

            if (Convert.ToInt32(lblunitid.Text) == 1)
            {
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Mtr(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblcaltype.Text), Shape));
            }
            if (Convert.ToInt32(lblunitid.Text) == 2 || Convert.ToInt16(lblunitid.Text) == 6)
            {
                lblarea.Text = Convert.ToString(UtilityModule.Calculate_Area_Ft(Convert.ToDouble(Txtlength.Text), Convert.ToDouble(TxtWidth.Text), Convert.ToInt32(lblcaltype.Text), Shape, UnitId: Convert.ToInt16(lblunitid.Text)));
            }
        }
    }
    protected void Txtwidthlength_TextChanged(object sender, EventArgs e)
    {
        TextBox txtwidthlength = (TextBox)sender;
        GridViewRow gvr = (GridViewRow)txtwidthlength.NamingContainer;
        Check_Length_Width_Format(gvr.RowIndex);
    }
    protected void chkitem_CheckedChanged(object sender, EventArgs e)
    {
        if (variable.VarMaintainStockSeries == "1")
        {
            CheckBox lnk = sender as CheckBox;
            if (lnk != null)
            {
                GridViewRow grv = lnk.NamingContainer as GridViewRow;
                Label lblitemcode = (Label)grv.FindControl("lblitemcode");
                switch (Session["varcompanyId"].ToString())
                {
                    case "28":
                        break;
                    default:
                        txtprefix.Text = lblitemcode.Text;
                        break;
                }
                txtprefix_TextChanged(sender, new EventArgs());

            }

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
    protected void txtfoliono_TextChanged(object sender, EventArgs e)
    {
        FIllProdUnit(sender);
    }
    protected void FIllProdUnit(object sender = null)
    {
        string str = @"select Distinct U.UnitsId,U.UnitName,PIm.CompanyId,Pim.Loomid From Process_issue_master_1 PIM inner Join  Units U on PIM.Units=U.UnitsId
                        inner join Employee_ProcessOrderNo EMP on PIM.Issueorderid=EMP.IssueOrderId and EMP.ProcessId=1
                        Where PIM.Companyid=" + DDcompany.SelectedValue;

        if (txteditempid.Text != "")
        {
            str = str + " and EMP.EMPID=" + txteditempid.Text;
        }
        if (txtfoliono.Text != "")
        {
            ////str = str + " and EMP.Issueorderid=" + txtfoliono.Text;

            str = str + " and PIM.ChallanNo='" + txtfoliono.Text + "'";
        }

        str = str + " order by Unitname";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 0, true, "--Plz Select--");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDProdunit.Items.FindByValue(ds.Tables[0].Rows[0]["unitsid"].ToString()) != null)
            {
                DDProdunit.SelectedValue = ds.Tables[0].Rows[0]["unitsid"].ToString();
            }
        }
        //UtilityModule.ConditionalComboFill(ref DDProdunit, str, true, "--Plz Select--");
        if (DDProdunit.Items.Count > 0)
        {
            DDProdunit.SelectedIndex = 1;
            DDProdunit_SelectedIndexChanged(sender, new EventArgs());
        }

    }
}