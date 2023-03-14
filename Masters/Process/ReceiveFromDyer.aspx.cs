using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_ReceiveFromDyer : System.Web.UI.Page
{
    public static int rowindex = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyid"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Processtype=0 and mastercompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME 
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
                if (DDProcessName.Items.FindByValue("5") != null) //DYEING
                {
                    DDProcessName.SelectedValue = "5";
                    DDProcessName_SelectedIndexChanged(sender, e);
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDgodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDgodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                }
            }
            txtrecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (Session["canedit"].ToString() == "1")
            {
                TRedit.Visible = true;
            }
            //********
            switch (Session["varcompanyNo"].ToString())
            {
                //case "16":
                //    lblindentno.Text = "Challan No.";
                //    break;
                case "20":
                    lblindentno.Text = "Challan No.";
                    break;
                default:
                    break;
            }
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
        }
    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by EI.empname", true, "--Plz Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtchallanNo.Text = "";
        FillIndentNo();
    }
    protected void FillIndentNo()
    {
        string str = @"select ID,indentNo from DyerIssueMaster Where companyid=" + DDCompanyName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + " and empid=" + DDPartyName.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + " order by ID desc";
        UtilityModule.ConditionalComboFill(ref DDindentNo, str, true, "--Plz Select--");
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Boolean saveflag = true;
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
        //dtrecord.Columns.Add("BinNo", typeof(string));
        //************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblrfinishedid = (Label)DG.Rows[i].FindControl("lblrfinishedid");
            // DropDownList DDgodown = (DropDownList)DG.Rows[i].FindControl("DDgodown");
            Label lblrlotno = (Label)DG.Rows[i].FindControl("lblrlotno");
            TextBox txtrtagno = (TextBox)DG.Rows[i].FindControl("txtrtagno");
            TextBox lblrate = (TextBox)DG.Rows[i].FindControl("txtrate");
            TextBox txtrecqty = (TextBox)DG.Rows[i].FindControl("txtrecqty");
            TextBox txtlossqty = (TextBox)DG.Rows[i].FindControl("txtlossqty");
            Label lblissuedetailid = (Label)DG.Rows[i].FindControl("lblissuedetailid");
            Label lblunitid = (Label)DG.Rows[i].FindControl("lblunitid");
            Label lblrflagsize = (Label)DG.Rows[i].FindControl("lblrflagsize");
            TextBox txtundyedqty = (TextBox)DG.Rows[i].FindControl("txtundyedqty");
            Label lblifinishedid = (Label)DG.Rows[i].FindControl("lblifinishedid");
            //DropDownList DDBinNo = (DropDownList)DG.Rows[i].FindControl("DDBinNo");

            saveflag = false;
            if (variable.VarBINNOWISE == "1")
            {
                if (DDgodown.SelectedIndex > 0 && DDBinNo.SelectedIndex > 0 && (Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0 || Convert.ToDouble(txtundyedqty.Text == "" ? "0" : txtundyedqty.Text) > 0))
                {
                    saveflag = true;
                }
            }
            else
            {
                if (DDgodown.SelectedIndex > 0 && (Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0 || Convert.ToDouble(txtundyedqty.Text == "" ? "0" : txtundyedqty.Text) > 0))
                {
                    saveflag = true;
                }
            }

            //check rec qty and loss qty
            if (DDgodown.SelectedIndex > 0 && (Convert.ToDouble(txtrecqty.Text == "" ? "0" : txtrecqty.Text) > 0 || Convert.ToDouble(txtlossqty.Text == "" ? "0" : txtlossqty.Text) > 0 || Convert.ToDouble(txtundyedqty.Text == "" ? "0" : txtundyedqty.Text) > 0))
            {
                DataRow dr = dtrecord.NewRow();
                dr["Rfinishedid"] = lblrfinishedid.Text;
                dr["godownid"] = DDgodown.SelectedValue;
                dr["Lotno"] = lblrlotno.Text;
                dr["TagNo"] = txtrtagno.Text == "" ? "Without Tag No" : txtrtagno.Text;
                dr["Rate"] = lblrate.Text == "" ? "0" : lblrate.Text;
                dr["Recqty"] = txtrecqty.Text == "" ? "0" : txtrecqty.Text;
                dr["Lossqty"] = txtlossqty.Text == "" ? "0" : txtlossqty.Text;
                dr["issuedetailid"] = lblissuedetailid.Text;
                dr["unitid"] = lblunitid.Text;
                dr["Rflagsize"] = lblrflagsize.Text;
                dr["undyedqty"] = txtundyedqty.Text == "" ? "0" : txtundyedqty.Text;
                dr["Ifinishedid"] = lblifinishedid.Text;
                //dr["BinNo"] = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";
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
                arr[4] = new SqlParameter("@Indentid", DDindentNo.SelectedValue);
                arr[5] = new SqlParameter("@ChallanNo", SqlDbType.VarChar, 50);
                arr[5].Direction = ParameterDirection.InputOutput;
                arr[5].Value = txtchallanNo.Text;
                arr[6] = new SqlParameter("@RecDate", txtrecdate.Text);
                arr[7] = new SqlParameter("@userid", Session["varuserid"]);
                arr[8] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
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
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveDyerReceive", arr);
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
                    DDindentNo.SelectedIndex = -1;
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
    protected void FillGridReceive()
    {
        string str = @"select DIM.id as Issueid,DID.Detailid as IssueDetailid,dbo.F_getItemDescription(DID.Ifinishedid,DID.iflagsize) as Iitemdescription,
                        dbo.F_getItemDescription(DID.Rfinishedid,DID.Rflagsize) as Ritemdescription,DID.RecLotNo,DID.RecTagno,DID.issueqty as Issuedqty,
                        dbo.F_getDyerreceiveqty(DID.Detailid) as Receivedqty,DID.Rate,DID.Rfinishedid,DID.unitid,DID.Rflagsize,DID.ifinishedid
                        From DyerIssueMaster DIM inner join DyerIssueDetail DID  on DIM.ID=DID.masterid 
                        Where DIM.ID=" + DDindentNo.SelectedValue;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DDindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkedit.Checked == false)
        {
            FillGridReceive();
        }
        else
        {
            string str = @"select Distinct DRM.ID,DRM.ChallanNo From DyerReceiveMaster DRM inner join DyerReceiveDetail DRD on DRM.ID=DRD.Masterid
                            inner join DyerIssueMaster DIM on DRD.issueid=DIM.ID Where DRM.companyid=" + DDCompanyName.SelectedValue + @" and 
                            DRM.Processid=" + DDProcessName.SelectedValue + " and DRM.empid=" + DDPartyName.SelectedValue + " and DIM.ID=" + DDindentNo.SelectedValue + @" 
                            order by DRM.ID desc";
            UtilityModule.ConditionalComboFill(ref DDpartychallan, str, true, "--Plz Select--");
        }

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //for (int i = 0; i < DG.Columns.Count; i++)
            //{

            //    if (DG.Columns[i].HeaderText.ToUpper() == "BIN NO.")
            //    {
            //        if (variable.VarBINNOWISE == "1")
            //        {
            //            DG.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DG.Columns[i].Visible = false;
            //        }

            //    }
            //}
            //if (variable.VarBINNOWISE == "1")
            //{
            //    DropDownList DDBinNo = ((DropDownList)e.Row.FindControl("DDBinNo"));
            //    if (DDBinNo != null)
            //    {
            //        UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");
            //    }
            //}

        }
    }
    protected void fillgrid()
    {
        string str = @"select DRM.ID,DRD.Detailid,dbo.F_getItemDescription(DRD.Rfinishedid,DRD.Rflagsize) as RItemdescription,
                        gm.GodownName,DRD.LotNo,DRD.TagNo,DRD.Recqty,DRD.Lossqty,DRD.Rate,DRM.GateinNo,DRM.challanNo,DRM.receiveDate,DRD.Undyedqty,
                        isnull(DRM.Checkedby,'') as Checkedby,isnull(DRM.Approvedby,'') as Approvedby
                        From DyerReceiveMaster DRM inner join DyerReceiveDetail DRD on DRM.ID=DRD.Masterid
                        inner join GodownMaster gm on DRD.godownid=gm.GoDownID Where DRM.id=" + hnid.Value;

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
    protected void btnpreview_Click(object sender, EventArgs e)
    {
        string str = "select * from V_DyerReceive where id=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Session["rptFileName"] = "~\\Reports\\rptsampledyeingreceive.rpt";
            Session["rptFileName"] = "~\\Reports\\RptDyerReceive.rpt";
            Session["Getdataset"] = ds;
            //Session["dsFileName"] = "~\\ReportSchema\\rptsampledyeingreceive.xsd";
            Session["dsFileName"] = "~\\ReportSchema\\RptDyerReceive.xsd";
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        DDpartychallan.SelectedIndex = -1;
        DDindentNo.Items.Clear();
        DDpartychallan.Items.Clear();
        TDPartychallan.Visible = false;
        DGrecdetail.DataSource = null;
        DGrecdetail.DataBind();
        DG.DataSource = null;
        DG.DataBind();
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
    protected void DDpartychallan_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDpartychallan.SelectedValue;
        fillgrid();
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
    protected void DGrecdetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Popup(true);
        txtpwd.Focus();
        rowindex = e.RowIndex;
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
            arr[4] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteDyerReceive", arr);
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
    protected void Updatedetails(int rowindex)
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
            Label lblid = (Label)DGrecdetail.Rows[rowindex].FindControl("lblid");
            Label lbldetailid = (Label)DGrecdetail.Rows[rowindex].FindControl("lbldetailid");
            TextBox txtrecqty = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtrecqty");
            TextBox txtlossqty = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtlossqty");
            TextBox txteditrate = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txteditrate");
            TextBox txtundyedqtyedit = (TextBox)DGrecdetail.Rows[rowindex].FindControl("txtundyedqtyedit");

            if (txtrecqty.Text == "" || txtrecqty.Text == "0")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn", "alert('Please Enter Qty');", true);
                return;
            }
            //*************
            SqlParameter[] arr = new SqlParameter[10];
            arr[0] = new SqlParameter("@ID", lblid.Text);
            arr[1] = new SqlParameter("@Detailid", lbldetailid.Text);
            arr[2] = new SqlParameter("@recqty", txtrecqty.Text == "" ? "0" : txtrecqty.Text);
            arr[3] = new SqlParameter("@Lossqty", txtlossqty.Text == "" ? "0" : txtlossqty.Text);
            arr[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[4].Direction = ParameterDirection.Output;
            arr[5] = new SqlParameter("@userid", Session["varuserid"]);
            arr[6] = new SqlParameter("@Rate", txteditrate.Text == "" ? "0" : txteditrate.Text);
            arr[7] = new SqlParameter("@undyedqty", txtundyedqtyedit.Text == "" ? "0" : txtundyedqtyedit.Text);
            arr[8] = new SqlParameter("@checkedby", txtcheckedby.Text);
            arr[9] = new SqlParameter("@Approvedby", txtapprovedby.Text);
            //*******
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updateDyerReceive", arr);
            lblmsg.Text = arr[4].Value.ToString();
            Tran.Commit();
            DGrecdetail.EditIndex = -1;
            fillgrid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void txtpwd_TextChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        if (variable.VarDyeingeditpwd == txtpwd.Text)
        {
            Updatedetails(rowindex);
            Popup(false);
        }
        else
        {
            lblmsg.Text = "Please Enter Correct Password..";
        }
        DGrecdetail.EditIndex = -1;
        fillgrid();
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
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TDBinNo.Visible == true)
        {

            UtilityModule.ConditionalComboFill(ref DDBinNo, "select BINNO,BINNO From BinMaster Where Godownid=" + DDgodown.SelectedValue + " order by BINID", true, "--Plz Select--");

        }
    }
}