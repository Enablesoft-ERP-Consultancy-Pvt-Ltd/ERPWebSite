using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_HandSpinning_frmHandspinningreceive : System.Web.UI.Page
{
    static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Process_Name='HAND SPINNING' order by PROCESS_NAME 
                           select GoDownID,GodownName From GodownMaster
                           select Godownid From ModuleWiseGodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                DDProcessName_SelectedIndexChanged(sender, new EventArgs());

            }
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDgodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDgodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                    DDgodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TRedit.Visible = true;
            }
            //********

            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            //**********
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName+case when Isnull(Ei.Empcode,'')='' Then '' Else '['+EI.Empcode+']'  End as Empname From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by empname", true, "--Plz Select--");
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDgodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            }

        }
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = @"select ID,IssueNo from HANDSPINNINGISSUEMASTER Where companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " and empid=" + DDPartyName.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + " order by ID desc";
        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        DDpartychallan.SelectedIndex = -1;
        DDissueno.Items.Clear();
        DDpartychallan.Items.Clear();
        TDPartychallan.Visible = false;
        //DGrecdetail.DataSource = null;
        //DGrecdetail.DataBind();
        //DG.DataSource = null;
        //DG.DataBind();
        hnid.Value = "0";
        txtgateinno.Text = "";
        txtchallanNo.Text = "";
        txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        chkcomplete.Visible = false;
        if (chkedit.Checked == true)
        {
            TDPartychallan.Visible = true;
            chkcomplete.Visible = true;
        }
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == false)
        {
            FillGridReceive();
        }
        else
        {
            string str = @"select Distinct SRM.ID,SRM.ChallanNo From HANDSPINNINGRECEIVEMASTER SRM inner join HANDSPINNINGRECEIVEDETAIL SRD on SRM.ID=SRD.Masterid
                                     inner join HANDSPINNINGISSUEMASTER SM on SRD.issueid=SM.ID Where SRM.companyid=" + DDCompanyName.SelectedValue + " and SRM.Processid=" + DDProcessName.SelectedValue + " and SRM.empid=" + DDPartyName.SelectedValue + " and SM.ID=" + DDissueno.SelectedValue + " order by srm.ID desc";
            UtilityModule.ConditionalComboFill(ref DDpartychallan, str, true, "--Plz Select--");
        }

    }

    private void FillGridReceive()
    {
        string str = @"select dbo.F_getItemDescription(MD.Rfinishedid,MD.Rflagsize) as RItemdescription,
                        MD.unitid,dbo.F_GETHANDSPINNINGRECLOTNO(MM.id,MD.Rfinishedid,MD.unitid) as Lotno,
                        dbo.F_GETHANDSPINNINGRECTAGNO(MM.id,MD.Rfinishedid,MD.unitid) as TAGNO,
                        SUM(MD.issueqty-isnull(vr.retqty,0)) as Issuedqty,
                        DBO.F_GETHANDSPINNINGRECDQTY(MM.ID,MD.RFINISHEDID,MD.Unitid) as Receivedqty,MD.Rate,MM.ID as Issueid,0 as IssueDetailid,MD.Rfinishedid,MD.Rflagsize
                        From HANDSPINNINGISSUEMASTER MM 
                        inner join HANDSPINNINGISSUEDETAIL MD on MM.ID=MD.masterid
                        LEFT JOIN V_GETHANDSPINNINGRETURNQTY VR ON MD.DETAILID=VR.ISSUEDETAILID
                        WHere MM.ID=" + DDissueno.SelectedValue + @" 
                        group by MM.ID,MD.Rfinishedid,MD.Rflagsize,MD.unitid,MD.Rate";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //**********Data table
        DataTable dtrecord = new DataTable();
        dtrecord.Columns.Add("Rfinishedid", typeof(int));
        dtrecord.Columns.Add("godownid", typeof(int));
        dtrecord.Columns.Add("lotno", typeof(string));
        dtrecord.Columns.Add("Tagno", typeof(string));
        dtrecord.Columns.Add("Rate", typeof(float));
        dtrecord.Columns.Add("Recqty", typeof(float));
        dtrecord.Columns.Add("Lossqty", typeof(float));
        dtrecord.Columns.Add("issuedetailid", typeof(int));
        dtrecord.Columns.Add("unitid", typeof(int));
        dtrecord.Columns.Add("Rflagsize", typeof(int));
        dtrecord.Columns.Add("Undyedqty", typeof(float));
        dtrecord.Columns.Add("Ifinishedid", typeof(int));
        dtrecord.Columns.Add("BELLWT", typeof(float));
        
        //************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblrfinishedid = (Label)DG.Rows[i].FindControl("lblrfinishedid");
            // DropDownList DDgodown = (DropDownList)DG.Rows[i].FindControl("DDgodown");
            Label lblrlotno = (Label)DG.Rows[i].FindControl("lblrlotno");
            Label lblrtagno = (Label)DG.Rows[i].FindControl("lblrtagno");
            Label lblrate = (Label)DG.Rows[i].FindControl("lblrate");
            TextBox txtrecqty = (TextBox)DG.Rows[i].FindControl("txtrecqty");
            TextBox txtlossqty = (TextBox)DG.Rows[i].FindControl("txtlossqty");
            Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
            Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
            Label lblrflagsize = (Label)DG.Rows[i].FindControl("lblrflagsize");
            TextBox TxtBellWt = (TextBox)DG.Rows[i].FindControl("TxtBellWt");

            // Label lblifinishedid = (Label)DG.Rows[i].FindControl("lblifinishedid");

            //check rec qty and loss qty
            if (DDgodown.SelectedIndex > 0 && (Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0))
            {
                DataRow dr = dtrecord.NewRow();
                dr["Rfinishedid"] = lblrfinishedid.Text;
                dr["godownid"] = DDgodown.SelectedValue;
                dr["Lotno"] = lblrlotno.Text;
                dr["TagNo"] = lblrtagno.Text;
                dr["Rate"] = lblrate.Text == "" ? "0" : lblrate.Text;
                dr["Recqty"] = txtrecqty.Text == "" ? "0" : txtrecqty.Text;
                dr["Lossqty"] = txtlossqty.Text == "" ? "0" : txtlossqty.Text;
                dr["issuedetailid"] = lblissuedetailid.Text;
                dr["unitid"] = lblunitid.Text;
                dr["Rflagsize"] = lblrflagsize.Text;
                dr["undyedqty"] = 0;
                dr["Ifinishedid"] = 0;
                dr["BELLWT"] = TxtBellWt.Text == "" ? "0" : TxtBellWt.Text; ;
                dtrecord.Rows.Add(dr);
            }
            //************
        }
        if (dtrecord.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] arr = new SqlParameter[15];
                arr[0] = new SqlParameter("@ID", SqlDbType.Int);
                arr[0].Direction = ParameterDirection.InputOutput;
                arr[0].Value = hnid.Value;
                arr[1] = new SqlParameter("@companyid", DDCompanyName.SelectedValue);
                arr[2] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
                arr[3] = new SqlParameter("@empid", DDPartyName.SelectedValue);
                arr[4] = new SqlParameter("@Indentid", DDissueno.SelectedValue);
                arr[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = txtchallanNo.Text;
                arr[6] = new SqlParameter("@RecDate", txtrecdate.Text);
                arr[7] = new SqlParameter("@userid", Session["varuserid"]);
                arr[8] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                arr[9] = new SqlParameter("@dtrecord", dtrecord);
                arr[10] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                arr[10].Direction = ParameterDirection.Output;
                arr[11] = new SqlParameter("@GateinNo", SqlDbType.VarChar, 100);
                arr[11].Direction = ParameterDirection.InputOutput;
                arr[11].Value = txtgateinno.Text;
                arr[12] = new SqlParameter("@CheckedBy", txtcheckedby.Text);
                arr[13] = new SqlParameter("@Approvedby", txtapprovedby.Text);
                arr[14] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_HANDSPINNINGRECEIVE]", arr);

                hnid.Value = arr[0].Value.ToString();
                txtchallanNo.Text = arr[5].Value.ToString();
                txtgateinno.Text = arr[11].Value.ToString();

                if (arr[10].Value.ToString() != "")
                {
                    lblmsg.Text = arr[10].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    Tran.Commit();
                    DDissueno.SelectedIndex = -1;
                    DG.DataSource = null;
                    DG.DataBind();
                    lblmsg.Text = "Data saved successfully.";
                    fillgrid();
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
                Tran.Rollback();
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select Godown or Enter (Recqty or Loss qty) to save data.');", true);
        }
    }
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from V_HANDSPINNINGRECEIVE where id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rpthandspinningreceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rpthandspinningreceive.xsd";
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
    protected void fillgrid()
    {
        string str = @"select SRM.ID,SRD.Detailid,dbo.F_getItemDescription(srd.Rfinishedid,SRD.Rflagsize) as RItemdescription,
                        gm.GodownName,SRD.LotNo,SRD.TagNo,SRD.Recqty,SRD.Lossqty,SRD.Rate,isnull(SRM.GateinNo,'') as GateinNo,SRM.challanNo,SRM.receiveDate,srd.Undyedqty,
                        isnull(Srm.Checkedby,'') as Checkedby,isnull(Srm.Approvedby,'') as Approvedby
                        From HANDSPINNINGRECEIVEMASTER SRM inner join HANDSPINNINGRECEIVEDETAIL SRD on SRM.ID=SRD.Masterid
                        inner join GodownMaster gm on SRD.godownid=gm.GoDownID Where SRM.id=" + hnid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGrecdetail.DataSource = ds.Tables[0];
        DGrecdetail.DataBind();
        if (chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                txtgateinno.Text = ds.Tables[0].Rows[0]["gateinno"].ToString();
                txtchallanNo.Text = ds.Tables[0].Rows[0]["challanNo"].ToString();
                txtrecdate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
                txtcheckedby.Text = ds.Tables[0].Rows[0]["checkedby"].ToString();
                txtapprovedby.Text = ds.Tables[0].Rows[0]["approvedby"].ToString();

            }
            else
            {
                txtgateinno.Text = "";
                txtchallanNo.Text = "";
                txtrecdate.Text = "";
                txtcheckedby.Text = "";
                txtapprovedby.Text = "";
            }
        }
    }
    protected void DGrecdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblDetailid = (Label)DGrecdetail.Rows[e.RowIndex].FindControl("lblDetailid");
            Label lblid = (Label)DGrecdetail.Rows[e.RowIndex].FindControl("lblid");
            SqlParameter[] arr = new SqlParameter[5];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[PRO_DELETEHANDSPINNINGRECEIVE]", arr);
            lblmsg.Text = arr[1].Value.ToString();
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }

    }
    protected void DGrecdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void DGrecdetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGrecdetail.EditIndex = e.NewEditIndex;
        fillgrid();
    }
    protected void DGrecdetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGrecdetail.EditIndex = -1;
        fillgrid();
    }
    protected void DDpartychallan_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDpartychallan.SelectedValue;
        fillgrid();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = "select empid From Empinfo Where empcode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDPartyName.Items.FindByValue(ds.Tables[0].Rows[0]["empid"].ToString()) != null)
            {
                DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDPartyName_SelectedIndexChanged(sender, new EventArgs());
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
    protected void lnkeditrate_Click(object sender, EventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        LinkButton lnkeditrate = sender as LinkButton;
        GridViewRow gvr = lnkeditrate.NamingContainer as GridViewRow;
        rowindex = gvr.RowIndex;
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarMOTTELINGEDITPWD == txtpwd.Text)
        {
            UpdateRate(rowindex);
            Popup(false);
        }
        else
        {
            lblmsg.Text = "Please Enter Correct Password..";
        }
    }
    protected void UpdateRate(int rowindex)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblDetailid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblDetailid");
            Label lblid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblid");
            TextBox lblrate = (TextBox)DGrecdetail.Rows[rowindex].FindControl("lblrate");
            SqlParameter[] arr = new SqlParameter[6];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            arr[3] = new SqlParameter("@userid", Session["varuserid"]);
            arr[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            arr[5] = new SqlParameter("@Rate", lblrate.Text == "" ? "0" : lblrate.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEHANDSPINNINGRECRATE", arr);
            lblmsg.Text = arr[1].Value.ToString();
            Tran.Commit();
            fillgrid();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
}