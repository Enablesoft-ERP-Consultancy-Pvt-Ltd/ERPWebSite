using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Loom_FrmBeamReceiveonloom : System.Web.UI.Page
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
                           select UnitsId,UnitName from Units order by UnitName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProdunit, ds, 1, true, "--Plz Select--");

            string str2 = "";
            if (Session["VarCompanyNo"].ToString() == "21")
            {
                str2 = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  
                Where  GM.GodownName in('SPARE TANA','YARN OPENING TANA','TANA HOUSE','HOUSE NO-12','KE RUNAKTA TANA') and GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName ";
            }
            else
            {
                str2 = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName ";
            }
            DataSet ds2 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str2);

            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds2, 0, true, "--Plz Select--");
            if (DDGodown.Items.Count > 0)
            {
                DDGodown.SelectedIndex = 1;
            }
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
    protected void DDProdunit_SelectedIndexChanged(object sender, EventArgs e)
    {
        AutoCompleteExtenderloomno.ContextKey = "0#0#" + DDProdunit.SelectedValue;
        string str = @"select Distinct PL.UID,PL.LoomNo,case when ISNUMERIC(PL.LoomNo)=1 then CONVERT(int,replace(PL.loomno, '.', '')) Else '9999999' End Loom1 from PROCESS_ISSUE_MASTER_1 PM inner join ProductionLoomMaster PL
                    on PM.LoomId=PL.UID and PM.Status='Pending'
                    And PL.companyid=" + DDcompany.SelectedValue + " and  PL.UnitId=" + DDProdunit.SelectedValue + " order by Loom1,Pl.Loomno";
        UtilityModule.ConditionalComboFill(ref DDLoomNo, str, true, "--Plz Selec--");
    }
    protected void DDLoomNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select PM.IssueOrderId,isnull(PM.ChallanNo,PM.IssueOrderId) from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Status='Pending' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Selec--");
    }
    protected void FillFolioNo()
    {
        //        string str = @"select PM.IssueOrderId,PM.IssueOrderId from PROCESS_ISSUE_MASTER_1 PM 
        //                    where PM.Status='Pending' and PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + DDLoomNo.SelectedValue;

        string str = @"select PM.IssueOrderId,isnull(PM.ChallanNo,PM.IssueOrderId) from PROCESS_ISSUE_MASTER_1 PM 
                    where PM.Companyid=" + DDcompany.SelectedValue + " and PM.Units=" + DDProdunit.SelectedValue + " and PM.LoomId=" + txtloomid.Text;
        if (chkcomplete.Checked == true)
        {
            str = str + " and Pm.status='Complete'";
        }
        else
        {
            str = str + " and Pm.status='Pending'";
        }

        UtilityModule.ConditionalComboFill(ref DDFoliono, str, true, "--Plz Select--");
    }
    protected void FillGrid()
    {
        //        string str = "";
        //        if (Session["varMasterCompanyIDForERP"].ToString() == "14")
        //        {
        //            str = @"select Distinct V.BeamDescription, V.unitid, V.UnitName, V.LotNo, V.TagNo, V.GodownId, V.BeamNo, V.Grossweight,
        //                        V.TareWeight, V.NetWeight, V.ofinishedid, V.Srno, oSizeflag, V.pcs 
        //						from V_BeamStock V 
        //						inner join PROCESS_ISSUE_DETAIL_1 PD on V.Item_Finished_id=PD.Item_Finished_Id ";

        //            if (DDFoliono.SelectedIndex <= 0)
        //            {
        //                str = str + " and PD.issueorderid=0";
        //            }
        //            else
        //            {
        //                str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
        //            }
        //            //            str = str + @" LEFT JOIN (Select Distinct PRM.PRORderID, PRT.BeamNo From ProcessRawMaster PRM JOIN ProcessRawTran PRT ON PRT.PRMID = PRM.PRMID 
        //            //								Where PRM.PRORderID = " + DDFoliono.SelectedValue + @" And PRM.BeamType = 1) a ON a.PRORderID = PD.issueorderid And a.BeamNo = V.BeamNo
        //            //                            Where 1=1";
        //            str = str + @" LEFT JOIN (Select Distinct IsNull(PRT.BeamNo, 0) BeamNo 
        //                                        From ProcessRawMaster PRM(Nolock) 
        //                                        JOIN ProcessRawTran PRT(Nolock) ON PRT.PRMID = PRM.PRMID 
        //								        Where PRM.BeamType = 1) a ON a.BeamNo = V.BeamNo
        //                            Where 1 = 1";
        //            if (DDGodown.SelectedIndex <= 0)
        //            {
        //                str = str + " and V.godownid = 0";
        //            }
        //            else
        //            {
        //                str = str + " and V.godownid = " + DDGodown.SelectedValue;
        //            }
        //            str = str + " And a.BeamNo Is Null";
        //            str = str + "  order by V.Srno";
        //        }
        //        else
        //        {
        //            str = @"select Distinct BeamDescription,unitid,UnitName,LotNo,TagNo,GodownId,BeamNo,Grossweight,
        //                        TareWeight,NetWeight,ofinishedid,V.Srno,oSizeflag,V.pcs from V_BeamStock V inner join PROCESS_ISSUE_DETAIL_1 PD
        //                        on V.Item_Finished_id=PD.Item_Finished_Id ";

        //            if (DDFoliono.SelectedIndex <= 0)
        //            {
        //                str = str + " and PD.issueorderid=0";
        //            }
        //            else
        //            {
        //                str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
        //            }
        //            if (DDGodown.SelectedIndex <= 0)
        //            {
        //                str = str + " and V.godownid=0";
        //            }
        //            else
        //            {
        //                str = str + " and V.godownid=" + DDGodown.SelectedValue;
        //            }
        //            str = str + "  order by V.Srno";
        //        }



        //        //        string str = @"select Distinct BeamDescription,unitid,UnitName,LotNo,TagNo,GodownId,BeamNo,Grossweight,
        //        //                        TareWeight,NetWeight,ofinishedid,V.Srno,oSizeflag,V.pcs from V_BeamStock V inner join PROCESS_ISSUE_DETAIL_1 PD
        //        //                        on V.Item_Finished_id=PD.Item_Finished_Id ";
        //        //        if (DDFoliono.SelectedIndex <= 0)
        //        //        {
        //        //            str = str + " and PD.issueorderid=0";
        //        //        }
        //        //        else
        //        //        {
        //        //            str = str + " and PD.issueorderid=" + DDFoliono.SelectedValue;
        //        //        }        

        //        //            if (DDGodown.SelectedIndex <= 0)
        //        //            {
        //        //                str = str + " and V.godownid=0";
        //        //            }
        //        //            else
        //        //            {
        //        //                str = str + " and V.godownid=" + DDGodown.SelectedValue;
        //        //            }      

        //        //        str = str + "  order by V.Srno";

        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        DG.DataSource = ds.Tables[0];
        //        DG.DataBind();
    }
    protected void DDFoliono_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select Distinct PRM.PrmId,PRM.ChalanNo from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
                     and PRM.BeamType=1 And PRM.TypeFlag = 0 and PRM.Prorderid=" + DDFoliono.SelectedValue + " and Processid=1 and PRM.TranType=0";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");

        //        if (chkEdit.Checked == true)
        //        {
        //            string str = @"select Distinct PRM.PrmId,PRM.ChalanNo from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
        //                     and PRM.BeamType=1 and PRM.Prorderid=" + DDFoliono.SelectedValue + " and Processid=1";
        //            UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
        //        }
        //        else
        //        {
        //            FillGrid();
        //        }
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        // FillGrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGIssueDetail.Rows[i].FindControl("Chkboxitem"));
            TextBox txtReceivePcs = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtReceivePcs"));
            Label lblBalToRecBeamPcs = ((Label)DGIssueDetail.Rows[i].FindControl("lblBalToRecBeamPcs"));

            //if (Chkboxitem.Checked == false)   // Change when Updated Completed
            //{
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please Select Checkbox');", true);               
            //    return;
            //}

            if (txtReceivePcs.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be blank');", true);
                txtReceivePcs.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtReceivePcs.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty always greater then zero');", true);
                txtReceivePcs.Focus();
                return;
            }
            if (Convert.ToInt32(txtReceivePcs.Text == "" ? "0" : txtReceivePcs.Text) > Convert.ToInt32(lblBalToRecBeamPcs.Text) && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be greater than balance qty');", true);
                txtReceivePcs.Text = "";
                txtReceivePcs.Focus();
                return;
            }
        }
        string Strdetail = "";
        for (int i = 0; i < DGIssueDetail.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DGIssueDetail.Rows[i].FindControl("Chkboxitem"));
            TextBox txtReceivePcs = ((TextBox)DGIssueDetail.Rows[i].FindControl("txtReceivePcs"));
            Label lblBeamNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblbeamno"));
            Label lblpcs = ((Label)DGIssueDetail.Rows[i].FindControl("lblpcs"));
            Label lblPOrderPcs = ((Label)DGIssueDetail.Rows[i].FindControl("lblPOrderPcs"));
            Label lblBazaarPcs = ((Label)DGIssueDetail.Rows[i].FindControl("lblBazaarPcs"));
            Label lblBalToRec = ((Label)DGIssueDetail.Rows[i].FindControl("lblBalToRec"));
            Label lblprmid = ((Label)DGIssueDetail.Rows[i].FindControl("lblprmid"));
            Label lblprtid = ((Label)DGIssueDetail.Rows[i].FindControl("lblprtid"));
            Label lblprorderid = ((Label)DGIssueDetail.Rows[i].FindControl("lblprorderid"));
            Label lblBeamFinishedid = ((Label)DGIssueDetail.Rows[i].FindControl("lblBeamFinishedid"));
            Label lblBeamLotNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblBeamLotNo"));
            Label lblBeamTagNo = ((Label)DGIssueDetail.Rows[i].FindControl("lblBeamTagNo"));
            Label lblIssueQuantity = ((Label)DGIssueDetail.Rows[i].FindControl("lblIssueQuantity"));
            Label lblGodownId = ((Label)DGIssueDetail.Rows[i].FindControl("lblGodownId"));
            Label lblUnitId = ((Label)DGIssueDetail.Rows[i].FindControl("lblUnitId"));
            Label lblFlagSize = ((Label)DGIssueDetail.Rows[i].FindControl("lblFlagSize"));
            Label lblTotalIssuedpcs = ((Label)DGIssueDetail.Rows[i].FindControl("lblTotalIssuedpcs"));

            if (Chkboxitem.Checked == true && (txtReceivePcs.Text != "") && DDProdunit.SelectedIndex > 0 && DDFoliono.SelectedIndex > 0 && DDissueno.SelectedIndex > 0)
            {
                Strdetail = Strdetail + lblBeamNo.Text + '|' + lblpcs.Text + '|' + lblPOrderPcs.Text + '|' + lblBazaarPcs.Text + '|' + lblBalToRec.Text + '|' + lblprmid.Text + '|' + lblprtid.Text + '|' + lblprorderid.Text + '|' + lblBeamFinishedid.Text + '|' + txtReceivePcs.Text + '|' + lblBeamLotNo.Text + '|' + lblBeamTagNo.Text + '|' + lblIssueQuantity.Text + '|' + lblGodownId.Text + '|' + lblUnitId.Text + '|' + lblFlagSize.Text + '|' + lblTotalIssuedpcs.Text + '~';
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
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@PRMID", SqlDbType.Int);
            if (chkEdit.Checked == true)
            {
                param[0].Value = DDReceiveNo.SelectedValue;
            }
            else
            {
                param[0].Value = 0;
            }

            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@ProductionUnitId", DDProdunit.SelectedValue);
            param[3] = new SqlParameter("@ProductionLoomNo", txtloomno.Text);
            param[4] = new SqlParameter("@FolioNo", DDFoliono.SelectedValue);
            param[5] = new SqlParameter("@IssuePrmid", DDissueno.SelectedValue);
            param[6] = new SqlParameter("@GodownId", DDGodown.SelectedValue);
            param[7] = new SqlParameter("@ProcessID", 1);
            param[8] = new SqlParameter("@ReceiveDate", txtReceiveDate.Text);
            param[9] = new SqlParameter("@UserID", Session["varuserid"]);
            param[10] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[11] = new SqlParameter("@StringDetail", Strdetail);
            param[12] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[12].Direction = ParameterDirection.Output;


            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBeamReceiveOnLoom", param);
            //*******************

            lblmessage.Text = param[12].Value.ToString();
            txtReceiveNo.Text = param[0].Value.ToString();
            hnprmid.Value = param[0].Value.ToString();
            Tran.Commit();
            //refreshcontrol();
            Fillissuedetail();
            FillBeamReceiveDetail();



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

        #region
        //SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        //if (con.State == ConnectionState.Closed)
        //{
        //    con.Open();
        //}
        ////**************Sql Table
        //DataTable dtrecords = new DataTable();
        //dtrecords.Columns.Add("Finishedid", typeof(int));
        //dtrecords.Columns.Add("grosswt", typeof(float));
        //dtrecords.Columns.Add("Netwt", typeof(float));
        //dtrecords.Columns.Add("GodownId", typeof(int));
        //dtrecords.Columns.Add("LotNo", typeof(string));
        //dtrecords.Columns.Add("TagNo", typeof(string));
        //dtrecords.Columns.Add("UnitId", typeof(int));
        //dtrecords.Columns.Add("BeamNo", typeof(string));
        //dtrecords.Columns.Add("flagsize", typeof(int));
        ////**************
        //for (int i = 0; i < DG.Rows.Count; i++)
        //{
        //    CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
        //    if (Chkboxitem.Checked == true)
        //    {
        //        Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
        //        Label lblgrossweight = ((Label)DG.Rows[i].FindControl("lblgrossweight"));
        //        Label lblnetweight = ((Label)DG.Rows[i].FindControl("lblnetweight"));
        //        Label lbllotno = ((Label)DG.Rows[i].FindControl("lbllotno"));
        //        Label lbltagno = ((Label)DG.Rows[i].FindControl("lbltagno"));
        //        Label lblunitid = ((Label)DG.Rows[i].FindControl("lblunitid"));
        //        Label lblbeamno = ((Label)DG.Rows[i].FindControl("lblbeamno"));
        //        Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblflagsize"));

        //        //********Data Row
        //        DataRow dr = dtrecords.NewRow();
        //        dr["Finishedid"] = lblitemfinishedid.Text;
        //        dr["grosswt"] = lblgrossweight.Text;
        //        dr["netwt"] = lblnetweight.Text;
        //        dr["godownid"] = DDGodown.SelectedValue;
        //        dr["Lotno"] = lbllotno.Text;
        //        dr["TagNo"] = lbltagno.Text;
        //        dr["Unitid"] = lblunitid.Text;
        //        dr["BeamNo"] = lblbeamno.Text;
        //        dr["flagsize"] = lblflagsize.Text;
        //        dtrecords.Rows.Add(dr);
        //    }
        //}
        //if (dtrecords.Rows.Count > 0)
        //{
        //    SqlTransaction Tran = con.BeginTransaction();
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[9];
        //        param[0] = new SqlParameter("@PrmId", SqlDbType.Int);
        //        param[0].Value = 0;
        //        param[0].Direction = ParameterDirection.InputOutput;
        //        param[1] = new SqlParameter("@companyid", DDcompany.SelectedValue);
        //        param[2] = new SqlParameter("@Processid", 1);//Weaving
        //        param[3] = new SqlParameter("@Prorderid", DDFoliono.SelectedValue);
        //        param[4] = new SqlParameter("@issueDate", txtissuedate.Text);
        //        param[5] = new SqlParameter("@userid", Session["varuserid"]);
        //        param[6] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
        //        param[7] = new SqlParameter("@dtrecords", dtrecords);
        //        param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
        //        param[8].Direction = ParameterDirection.Output;
        //        ///**********
        //        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBeamIssueonLoom", param);
        //        Tran.Commit();
        //        lblmessage.Text = param[8].Value.ToString();
        //        txtissueno.Text = param[0].Value.ToString();
        //        hnprmid.Value = param[0].Value.ToString();
        //        refreshcontrol();
        //        Fillissuedetail();
        //    }
        //    catch (Exception ex)
        //    {
        //        Tran.Rollback();
        //        lblmessage.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        //}
        #endregion

    }
    protected void refreshcontrol()
    {
        DDLoomNo.SelectedIndex = -1;
        DDFoliono.SelectedIndex = -1;
        DDissueno.SelectedIndex = -1;
        //DG.DataSource = null;
        // DG.DataBind();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        //string str = "select * from  V_BeamIssueonLoom where prmid=" + hnprmid.Value;
        ////************
        //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //if (ds.Tables[0].Rows.Count > 0)
        //{

        //    Session["rptFileName"] = "~\\Reports\\Rptbeamissueonloom.rpt";
        //    Session["Getdataset"] = ds;
        //    Session["dsFileName"] = "~\\ReportSchema\\Rptbeamissueonloom.xsd";
        //    StringBuilder stb = new StringBuilder();
        //    stb.Append("<script>");
        //    stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
        //    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        //}
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        txtloomno.Text = "";
        DDLoomNo.SelectedIndex = -1;
        DDFoliono.SelectedIndex = -1;
        TDissue.Visible = false;
        TDReceiveNo.Visible = false;
        // DG.DataSource = null;
        // DG.DataBind();
        DGIssueDetail.DataSource = null;
        DGIssueDetail.DataBind();
        txtReceiveNo.Text = "";
        txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        TDLoomno.Visible = false;
        TDLoomNotextbox.Visible = true;

        if (chkEdit.Checked == true)
        {
            TDissue.Visible = true;
            TDReceiveNo.Visible = true;
            DDReceiveNo.SelectedIndex = -1;
            TDLoomno.Visible = true;
            TDLoomNotextbox.Visible = false;
        }
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnprmid.Value = DDReceiveNo.SelectedValue;
        FillBeamReceiveDetail();
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hnprmid.Value = DDissueno.SelectedValue;
        // Fillissuedetail();

        if (chkEdit.Checked == true)
        {
            string str = @"select Distinct PRM.PrmId,PRM.ChalanNo from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
                     and PRM.BeamType=1 And PRM.TypeFlag = 0 and PRM.Prorderid=" + DDFoliono.SelectedValue + " and Processid=1 and PRM.TranType=1";
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
        param[0] = new SqlParameter("@Prmid", DDissueno.SelectedValue);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserid"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetIssueBeamOnLoomDetail", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DGIssueDetail.DataSource = ds.Tables[0];
            DGIssueDetail.DataBind();

            if (Session["VarCompanyNo"].ToString() != "21")
            {
                DDGodown.Enabled = false;
                DDGodown.SelectedValue = ds.Tables[0].Rows[0]["GodownId"].ToString();
            }

        }
        else
        {
            DGIssueDetail.DataSource = null;
            DGIssueDetail.DataBind();
        }



        //        string str = @"select Distinct PRT.BeamNo,dbo.F_getItemDescription(PRT.Finishedid,prt.flagsize) as BeamDescription,
        //                    WD.pcs,prt.prmid,PRT.PRTid,PRM.Prorderid,PRM.chalanno,Replace(CONVERT(nvarchar(11),PRM.date,106),' ','-') as IssueDate,isnull(PRT.Godownid,0) as GodownId 
        //                    from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
        //                    inner join warploommaster WLM on WLM.LoomNo=PRT.BeamNo inner join WarpLoomDetail WLD on WLM.ID=WLD.ID
        //                    and WLD.ofinishedid=PRT.Finishedid inner join WarpOrderDetail WD on WLD.Issuemasterid=Wd.Id and Wld.IssueDetailid=WD.Detailid
        //                    Where PRM.Prmid=" + hnprmid.Value;
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        DGIssueDetail.DataSource = ds.Tables[0];
        //        DGIssueDetail.DataBind();

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            DDGodown.Enabled = false;
        //            DDGodown.SelectedValue = ds.Tables[0].Rows[0]["GodownId"].ToString();
        //        }

        //        //if (chkEdit.Checked == true)
        //        //{
        //        //    if (ds.Tables[0].Rows.Count > 0)
        //        //    {
        //        //        txtissueno.Text = ds.Tables[0].Rows[0]["chalanNo"].ToString();
        //        //        txtissuedate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    txtissueno.Text = "";
        //        //    txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //        //}

    }
    protected void FillBeamReceiveDetail()
    {

        SqlParameter[] param = new SqlParameter[3];
        param[0] = new SqlParameter("@Prmid", hnprmid.Value);
        param[1] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
        param[2] = new SqlParameter("@UserId", Session["VarUserid"]);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetReceiveBeamOnLoomDetail", param);
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
                txtReceiveNo.Text = ds.Tables[0].Rows[0]["chalanNo"].ToString();
                txtReceiveDate.Text = ds.Tables[0].Rows[0]["Receivedate"].ToString();
            }
        }
        else
        {
            txtReceiveNo.Text = "";
            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }


        //        string str = @"select Distinct PRT.BeamNo,dbo.F_getItemDescription(PRT.Finishedid,prt.flagsize) as BeamDescription,
        //                    WD.pcs,prt.prmid,PRT.PRTid,PRM.Prorderid,PRM.chalanno,Replace(CONVERT(nvarchar(11),PRM.date,106),' ','-') as IssueDate from ProcessRawMaster PRM inner join ProcessRawTran PRT on PRM.PRMid=PRT.PRMid
        //                    inner join warploommaster WLM on WLM.LoomNo=PRT.BeamNo inner join WarpLoomDetail WLD on WLM.ID=WLD.ID
        //                    and WLD.ofinishedid=PRT.Finishedid inner join WarpOrderDetail WD on WLD.Issuemasterid=Wd.Id and Wld.IssueDetailid=WD.Detailid
        //                    Where PRM.Prmid=" + hnprmid.Value;
        //        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //        DGIssueDetail.DataSource = ds.Tables[0];
        //        DGIssueDetail.DataBind();
        //        if (chkEdit.Checked == true)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                txtReceiveNo.Text = ds.Tables[0].Rows[0]["chalanNo"].ToString();
        //                txtReceiveDate.Text = ds.Tables[0].Rows[0]["issuedate"].ToString();
        //            }
        //        }
        //        else
        //        {
        //            txtReceiveNo.Text= "";
        //            txtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        //        }

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
            Label lblprmid = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblprtid = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblprtid");
            Label lblprorderod = (Label)DGReceiveDetail.Rows[e.RowIndex].FindControl("lblprorderid");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@prmid", lblprmid.Text);
            param[1] = new SqlParameter("@prtid", lblprtid.Text);
            param[2] = new SqlParameter("@prorderid", lblprorderod.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@UserId", Session["VarUserid"]);
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteBeamReceiveOnLoom", param);
            lblmessage.Text = param[3].Value.ToString();
            Tran.Commit();
            Fillissuedetail();
            FillBeamReceiveDetail();
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtloomid.Text != "")
        {
            FillFolioNo();
        }
    }
}