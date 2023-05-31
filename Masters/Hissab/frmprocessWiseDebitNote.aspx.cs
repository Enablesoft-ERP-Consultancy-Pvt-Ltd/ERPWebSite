using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
public partial class Masters_Hissab_frmprocessWiseDebitNote : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            UtilityModule.ConditionalComboFill(ref DDCompanyName, "select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CI.masterCompanyId=" + Session["varcompanyId"] + " And CA.UserId=" + Session["varuserid"] + " Order by CompanyName", false, "");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            // SqlhelperEnum.FillDropDown(AllEnums.MasterTables.PROCESS_NAME_MASTER, DDProcessName, pWhere: "MasterCompanyId=" + Session["varcompanyId"] + "", pID: "Process_Name_Id", pName: "Process_Name", pFillBlank: true, Selecttext: "--Plz Select Process--");
            UtilityModule.ConditionalComboFill(ref DDProcessName, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 order by Process_Name", true, "--Plz Select Process--");
            txtDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            RDForProduction.Checked = true;
            RDForProduction_CheckedChanged(sender, new EventArgs());
            txtfromdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttodate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["VarCompanyNo"].ToString() == "42")
            {
                TDGSTPercentage.Visible = true;
            }
            else
            {
                TDGSTPercentage.Visible = false;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDParty, "select EI.EmpId,EI.EmpName+'/'+EI.Address from Empinfo EI,Empprocess EP Where EI.EmpId=EP.EmpId And EP.Processid=" + DDProcessName.SelectedValue + " And EI.masterCompanyId=" + Session["varcompanyId"] + " Order by EI.Empname", true, "--Plz Select Party name--");

        if (RDForJoborder.Checked == true && DDProcessName.SelectedValue == "5")
        {
            ChkForSample.Visible = true;
        }
        else
        {
            ChkForSample.Visible = false;
        }
    }
    protected void DDParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";

        if (RDForProduction.Checked == true)
        {
            str = "select IssueOrderId,issueOrderId As FolioNo From Process_issue_Master_" + DDProcessName.SelectedValue + " Where Empid=" + DDParty.SelectedValue + " And CompanyId=" + DDCompanyName.SelectedValue + " and Status<>'canceled' Order by IssueOrderId";

        }
        if (RDForJoborder.Checked == true)
        {
            //str = "select  PrmId,ChallanNo From PP_ProcessRecMaster Where prmid not in(select ProcessRec_prmId From RawMaterialPreprationHissabDetail) And  CompanyId=" + DDCompanyName.SelectedValue + " And EmpId=" + DDParty.SelectedValue + " And ProcessId=" + DDProcessName.SelectedValue + " Order by Challanno";

            if (ChkForSample.Checked == true)
            {
                str = @"select DISTINCT SDRM.ID,SDRM.CHALLANNO + '/' + SDM.INDENTNO AS CHALLANNO from SampleDyeingReceivemaster SDRM INNER JOIN SampleDyeingReceiveDetail SDRD ON SDRM.ID=SDRD.Masterid
                        INNER JOIN SampleDyeingmaster SDM ON SDRD.IssueId=SDM.Id
                        WHERE SDRM.ID NOT IN(SELECT PROCESSREC_PRMID FROM RAWMATERIALPREPRATIONHISSABDETAIL) And  SDRM.CompanyId=" + DDCompanyName.SelectedValue + " And SDRM.EmpId=" + DDParty.SelectedValue + " And SDRM.ProcessId=" + DDProcessName.SelectedValue + " Order by Challanno";
            }
            else
            {
                str = @"SELECT DISTINCT PRM.PRMID,PRM.CHALLANNO + '/' + IM.INDENTNO AS CHALLANNO FROM PP_PROCESSRECMASTER PRM INNER JOIN PP_PROCESSRECTRAN PRT ON PRM.PRMID=PRT.PRMID
                    INNER JOIN INDENTMASTER IM ON PRT.INDENTID=IM.INDENTID
                    WHERE PRM.PRMID NOT IN(SELECT PROCESSREC_PRMID FROM RAWMATERIALPREPRATIONHISSABDETAIL) And  prm.CompanyId=" + DDCompanyName.SelectedValue + " And prm.EmpId=" + DDParty.SelectedValue + " And prm.ProcessId=" + DDProcessName.SelectedValue + " Order by Challanno";
            }
           
        }
        if (RDForPurchase.Checked == true)
        {
            str = "select PurchaseReceiveId,BillNo from PurchaseReceivemaster Where Challan_Status=0 And CompanyId=" + DDCompanyName.SelectedValue + " And PartyId=" + DDParty.SelectedValue + " And MasterCompanyId=" + Session["varcompanyid"] + " Order by PurchaseReceiveId";
        }
        UtilityModule.ConditionalComboFill(ref DDBillNo, str, true, "--Plz Select Order No.--");
    }
    protected void RDForProduction_CheckedChanged(object sender, EventArgs e)
    {
        if (RDForProduction.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDProcessName, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID=1  order by Process_Name", true, "--Plz Select Process--");

            if (DDParty.Items.Count > 0)
            {
                DDParty.SelectedIndex = 0;
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 0;
            }
        }
    }
    protected void RDForJoborder_CheckedChanged(object sender, EventArgs e)
    {
        if (RDForJoborder.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDProcessName, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 and PROCESS_NAME not in ('PURCHASE') order by Process_Name", true, "--Plz Select Process--");
            if (DDParty.Items.Count > 0)
            {
                DDParty.SelectedIndex = 0;
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 0;
            }
        }
    }
    protected void RDForPurchase_CheckedChanged(object sender, EventArgs e)
    {
        if (RDForPurchase.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDProcessName, "select PROCESS_NAME_ID,Process_name from PROCESS_NAME_MASTER Where PROCESS_NAME_ID<>1 and PROCESS_NAME in ('PURCHASE') order by Process_Name", true, "--Plz Select Process--");
            if (DDParty.Items.Count > 0)
            {
                DDParty.SelectedIndex = 0;
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 0;
            }

        }
    }
    
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _array = new SqlParameter[14];
            _array[0] = new SqlParameter("@ID", SqlDbType.Int);
            _array[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
            _array[2] = new SqlParameter("@ProcessId", SqlDbType.Int);
            _array[3] = new SqlParameter("@EmpId", SqlDbType.Int);
            _array[4] = new SqlParameter("@OrderNo", SqlDbType.VarChar, 100);
            _array[5] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
            _array[6] = new SqlParameter("@Amount", SqlDbType.Float);
            _array[7] = new SqlParameter("@Remarks", SqlDbType.VarChar, 500);
            _array[8] = new SqlParameter("@UserId", SqlDbType.Int);
            _array[9] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _array[10] = new SqlParameter("@Msg", SqlDbType.VarChar, 50);
            _array[11] = new SqlParameter("@Type", SqlDbType.Int);
            _array[12] = new SqlParameter("@BillId", SqlDbType.Int);
            _array[13] = new SqlParameter("@GstPercentage", SqlDbType.Int);

            _array[0].Direction = ParameterDirection.InputOutput;
            _array[0].Value = 0;
            _array[1].Value = DDCompanyName.SelectedValue;
            _array[2].Value = DDProcessName.SelectedValue;
            _array[3].Value = DDParty.SelectedValue;
            _array[4].Value = DDBillNo.SelectedItem.Text;
            _array[5].Value = txtDate.Text;
            _array[6].Value = txtDebitAmt.Text;
            _array[7].Value = txtremarks.Text;
            _array[8].Value = Session["varuserId"];
            _array[9].Value = Session["varcompanyId"];
            _array[10].Direction = ParameterDirection.Output;
            int Type = 0;
            if (RDForProduction.Checked == true)
            {
                Type = 0;
            }
            if (RDForJoborder.Checked == true)
            {
                Type = 1;
            }
            if (RDForPurchase.Checked == true)
            {
                Type = 2;
            }
            _array[11].Value = Type;
            _array[12].Value = DDBillNo.SelectedValue;
            _array[13].Value = TDGSTPercentage.Visible==true ? txtGst.Text=="" ? "0" :txtGst.Text :"0";

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDebitNote", _array);
            Tran.Commit();
            lblMessage.Text = _array[10].Value.ToString();
            lblGateNo.Text = "DEBIT No.- " + "" + _array[0].Value.ToString();
            hnDebitNo.Value = _array[0].Value.ToString();
            Fillgrid();
            refreshControl();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void refreshControl()
    {
        txtDebitAmt.Text = "";
        txtremarks.Text = "";
        txtGst.Text = "";
    }
    protected void shoWReport()
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        try
        {
            string str = "";
            if (Session["VarCompanyNo"].ToString() == "42")
            {
                str = @"select CI.CompanyName,CI.CompAddr1,CI.CompTel,EI.EmpName,EI.Address,PNM.Process_Name,Date,
                        PM.Remarks,Amount,PM.OrderNo,ID As DebitNo,isnull(CI.GSTNo,'') as GSTIN,Isnull(EI.GSTNo,'') as EMPGSTIN,isnull(PM.GSTPercentage,0) as GSTPercentage,
                        PM.MasterCompanyId,Case When PM.ProcessId=9 Then (Select isnull(PRM.BillNo,'') From PurchaseReceiveMaster PRM(NoLock) Where PRM.PurchaseReceiveId=PM.BillId and PM.ProcessId=9 ) Else '' End As PurchaseBillNo 
                        From ProcessDebitNote PM(NoLock) JOIN CompanyInfo CI(NoLock) ON PM.Companyid=CI.CompanyId
                        JOIN Empinfo EI(NoLock) ON Pm.EmpId=EI.Empid
                        JOIN Process_Name_Master PNM(NoLock) ON PM.ProcessId=PNM.Process_Name_Id 
                        Where PM.id=" + hnDebitNo.Value;
            }
            else
            {
                str = @"select CI.CompanyName,CI.CompAddr1,CI.CompTel,EI.EmpName,EI.Address,PNM.Process_Name,Date,
                        PM.Remarks,Amount,OrderNo,ID As DebitNo,isnull(CI.GSTNo,'') as GSTIN,Isnull(EI.GSTNo,'') as EMPGSTIN,isnull(PM.GSTPercentage,0) as GSTPercentage 
                        from ProcessDebitNote PM,CompanyInfo CI,Empinfo EI,
                        Process_Name_Master PNM  Where PM.Companyid=CI.CompanyId And Pm.EmpId=EI.Empid And PM.ProcessId=PNM.Process_Name_Id And PM.id=" + hnDebitNo.Value;
            }
            

            DataSet DS = SqlHelper.ExecuteDataset(con, CommandType.Text, str);

            if (DS.Tables[0].Rows.Count > 0)
            {
                Session["dsFilename"] = "~\\ReportSchema\\RptProcessDebitNote.xsd";
                if (Session["VarCompanyNo"].ToString() == "42")
                {
                    Session["rptFilename"] = "Reports/RptProcessDebitNoteWithGST.rpt";
                }
                else
                {
                    Session["rptFilename"] = "Reports/RptProcessDebitNote.rpt";
                }

                
                Session["GetDataset"] = DS;
                StringBuilder stb = new StringBuilder();
                stb.Append("<script>");
                stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn1", "alert('No Record Found!');", true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void BtnPriview_Click(object sender, EventArgs e)
    {
        shoWReport();
    }
    protected void lnkpreview_Click(object sender, EventArgs e)
    {
        GridViewRow gvr = ((LinkButton)sender).NamingContainer as GridViewRow;
        Label lblid = (Label)gvr.FindControl("lblid");
        hnDebitNo.Value = lblid.Text;
        shoWReport();
    }
    protected void DDBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillgrid();
    }
    protected void Fillgrid()
    {
        string str = "";
        str = "select OrderNo,Replace(convert(nvarchar(11),Date,106),' ','-') as Date,Remarks,Amount,ID,BillId,Type from ProcessDebitnote where 1=1";

        if (RDForPurchase.Checked == true)
        {
            str = str + " and Type=2 and BillId=" + DDBillNo.SelectedValue;  //PURCHASE
        }
        else if (RDForJoborder.Checked == true)
        {
            str = str + " and Type=1 and BillId=" + DDBillNo.SelectedValue; //JOB ORDER
        }
        else if (RDForProduction.Checked == true)
        {
            str = str + " and Type=0 and BillId=" + DDBillNo.SelectedValue; //For Production
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        GVDetail.DataSource = ds.Tables[0];
        GVDetail.DataBind();
    }
    //protected void GVDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
    //    if (con.State == ConnectionState.Closed)
    //    {
    //        con.Open();
    //    }
    //    SqlTransaction Tran = con.BeginTransaction();
    //    try
    //    {
    //        Label lblId = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblid"));
    //        Label lblBillId = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lblbillid"));
    //        Label lbltype = ((Label)GVDetail.Rows[e.RowIndex].FindControl("lbltype"));


    //        SqlParameter[] param = new SqlParameter[6];
    //        param[0] = new SqlParameter("@Id", lblId.Text);
    //        param[1] = new SqlParameter("@billid", lblBillId.Text);
    //        param[2] = new SqlParameter("@Type", lbltype.Text);
    //        param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
    //        param[3].Direction = ParameterDirection.Output;
    //        param[4] = new SqlParameter("@userid", Session["varuserid"].ToString());
    //        param[5] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);

    //        SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDebitNote", param);
    //        lblMessage.Text = param[3].Value.ToString();
    //        Tran.Commit();
    //        Fillgrid();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.Text = ex.Message;
    //        Tran.Rollback();
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con.Dispose();
    //    }
    //}
    protected void btndebitdetail_Click(object sender, EventArgs e)
    {
        string str = @"select CI.CompanyName,CI.CompAddr1,CI.CompTel,EI.EmpName,EI.Address,PNM.Process_Name,Date,
                    PM.Remarks,Amount,OrderNo,ID As DebitNo,isnull(CI.GSTNo,'') as GSTIN,Isnull(EI.GSTNo,'') as EMPGSTIN from ProcessDebitNote PM,CompanyInfo CI,Empinfo EI,
                    Process_Name_Master PNM  Where PM.Companyid=CI.CompanyId And Pm.EmpId=EI.Empid And PM.ProcessId=PNM.Process_Name_Id and PM.companyId=" + DDCompanyName.SelectedValue;
        if (DDProcessName.SelectedIndex > 0)
        {
            str = str + " and PM.processid=" + DDProcessName.SelectedValue;
        }
        if (DDParty.SelectedIndex > 0)
        {
            str = str + " and PM.Empid=" + DDParty.SelectedValue;
        }
        str = str + " and PM.date>='" + txtfromdate.Text + "' and PM.date<='" + txttodate.Text + "'";
        DataSet DS = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (DS.Tables[0].Rows.Count > 0)
        {
            Session["dsFilename"] = "~\\ReportSchema\\Rptdebitnotedetail.xsd";
            Session["rptFilename"] = "Reports/Rptdebitnotedetail.rpt";
            Session["GetDataset"] = DS;
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn2", "alert('No Record Found!');", true);
        }


    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (GVDetail.Rows.Count > 0)
        {

            if (variable.VarPAYMENTDEL_PWD == txtpwd.Text)
            {
                Rowdelete(rowindex, sender);
                Popup(false);
            }
            else
            {
                lblMessage.Text = "Please Enter Correct Password..";
            }

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
    protected void Rowdelete(int rowindex, object sender = null)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblId = ((Label)GVDetail.Rows[rowindex].FindControl("lblid"));
            Label lblBillId = ((Label)GVDetail.Rows[rowindex].FindControl("lblbillid"));
            Label lbltype = ((Label)GVDetail.Rows[rowindex].FindControl("lbltype"));


            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@Id", lblId.Text);
            param[1] = new SqlParameter("@billid", lblBillId.Text);
            param[2] = new SqlParameter("@Type", lbltype.Text);
            param[3] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@userid", Session["varuserid"].ToString());
            param[5] = new SqlParameter("@MastercompanyId", Session["varcompanyId"]);

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDebitNote", param);
            lblMessage.Text = param[3].Value.ToString();
            Tran.Commit();
            Fillgrid();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void lnkdel_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        LinkButton lnkdel = sender as LinkButton;
        GridViewRow gvr = lnkdel.NamingContainer as GridViewRow;
        rowindex = gvr.RowIndex;
    }
    
    protected void ChkForSample_CheckedChanged(object sender, EventArgs e)
    {        
        DDParty_SelectedIndexChanged(sender, new EventArgs());      
    }
}