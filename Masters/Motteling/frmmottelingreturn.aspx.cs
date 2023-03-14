using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Motteling_frmmottelingreturn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                           select PROCESS_NAME_ID,Process_name From process_name_master  where Processtype=0 and Process_name in('Motteling','YARN OPENING+MOTTELING', 'HANK MAKING') and mastercompanyid=" + Session["varcompanyid"] + @" order by PROCESS_NAME
                           Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"] + @" Order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"'
                            Select ID, BranchName 
                                From BRANCHMASTER BM(nolock) 
                                JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                                Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcessName, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 2, true, "--Plz Select--");
            //auto select godown
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDGodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDGodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                }
            }
            if (DDProcessName.Items.Count > 0)
            {
                DDProcessName.SelectedIndex = 1;
                DDProcessName_SelectedIndexChanged(sender, e);
            }
            txtretdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            ds.Dispose();
            ////**********Edit
            if (Session["canedit"].ToString() == "1")
            {
                TREdit.Visible = true;
                TDComplete.Visible = true;
            }
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            //**********
        }

    }
    protected void DDProcessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDPartyName, "Select EI.EmpId,EI.EmpName+case when isnull(Ei.Empcode,'')='' Then '' Else '['+EI.Empcode+']' End as Empname From EmpInfo EI inner join EmpProcess EP on EI.EmpId=EP.EmpId and EP.ProcessId=" + DDProcessName.SelectedValue + " order by empname", true, "--Plz Select--");
    }
    protected void DDPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @"select ID,IssueNo + ' # ' +REPLACE(CONVERT(nvarchar(11),IssueDate,106),' ','-') as IssueNo 
        From MOTTELINGISSUEMASTER 
        Where Companyid=" + DDCompanyName.SelectedValue + " And BranchID = " + DDBranchName.SelectedValue + " and processid=" + DDProcessName.SelectedValue + @" And empid=" + DDPartyName.SelectedValue;
        if (chkcomplete.Checked == true)
        {
            str = str + " and status='Complete'";
        }
        else
        {
            str = str + " and status='Pending'";
        }

        str = str + "  order by id desc";

        UtilityModule.ConditionalComboFill(ref DDissueno, str, true, "--Plz Select--");
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
    }
    protected void DDissueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //**********
        if (chkedit.Checked == true)
        {
            string str = @"select Distinct MRM.ID,MRM.ReturnNo +' # '+Replace(convert(nvarchar(11),MRM.ReturnDate,106),' ','-') as RetNo 
                    From Mottelingreturnmaster MRM 
                    join MOttelingreturnDetail MD on MRM.Id=MD.Masterid
                    Where MD.Issuemasterid=" + DDissueno.SelectedValue + "  order by MRM.Id desc";
            UtilityModule.ConditionalComboFill(ref DDreturnno, str, true, "--Plz Select--");
        }
        FillIssueDetail();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Item_Finished_id", typeof(int));
        dtrecords.Columns.Add("Unitid", typeof(int));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("RetQty", typeof(float));
        dtrecords.Columns.Add("Issuemasterid", typeof(int));
        dtrecords.Columns.Add("IssuemasterDetailid", typeof(int));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtretqty = ((TextBox)DG.Rows[i].FindControl("txtretqty"));
            // DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            if (Chkboxitem.Checked == true && txtretqty.Text != "" && DDGodown.SelectedIndex != -1)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblitemfinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lblunitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblflagsize"));
                Label lblLotno = ((Label)DG.Rows[i].FindControl("lblLotno"));
                Label lblTagno = ((Label)DG.Rows[i].FindControl("lblTagno"));
                Label lblissuemasterid = ((Label)DG.Rows[i].FindControl("lblissuemasterid"));
                Label lblissuemasterdetailid = ((Label)DG.Rows[i].FindControl("lblissuemasterdetailid"));
                //**********Data Row
                DataRow dr = dtrecords.NewRow();

                dr["Item_Finished_id"] = lblitemfinishedid.Text;
                dr["Unitid"] = lblunitid.Text;
                dr["flagsize"] = lblflagsize.Text;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = lblLotno.Text;
                dr["TagNo"] = lblTagno.Text;
                dr["RetQty"] = txtretqty.Text == "" ? "0" : txtretqty.Text;
                dr["Issuemasterid"] = lblissuemasterid.Text;
                dr["IssuemasterDetailid"] = lblissuemasterdetailid.Text;
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = 0;
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@companyId", DDCompanyName.SelectedValue);
                param[3] = new SqlParameter("@vendorid", DDPartyName.SelectedValue);
                param[4] = new SqlParameter("@returnNo", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@RetDate", txtretdate.Text);
                param[6] = new SqlParameter("@userid", Session["varuserid"]);
                param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[7].Direction = ParameterDirection.Output;
                param[8] = new SqlParameter("@mastercompanyid", Session["varcompanyid"]);
                param[9] = new SqlParameter("@issuemasterid", DDissueno.SelectedValue);
                param[10] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
                param[11] = new SqlParameter("@Processid", DDProcessName.SelectedValue);
                param[12] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_MottelingReturn", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                txtretNo.Text = param[4].Value.ToString();
                if (param[7].Value.ToString() != "")
                {
                    lblmessage.Text = param[7].Value.ToString();
                    Tran.Commit();
                }
                else
                {
                    lblmessage.Text = "Data Saved Successfully.";
                    Tran.Commit();
                    FillIssueDetail();
                    FillReturnDetails();
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data.');", true);
        }
    }

    private void FillReturnDetails()
    {
        string str = @"select Ym.id,Yt.detailid,YM.ReturnNo,dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,Gm.GodownName,YT.Lotno,YT.Tagno,yt.Retqty as Returnqty,Replace(convert(nvarchar(11),Ym.ReturnDate,106),' ','-') as ReturnDate
                        from Mottelingreturnmaster YM inner join MottelingReturnDetail YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        inner join GodownMaster GM on YT.GodownId=Gm.GoDownID Where YM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtretNo.Text = ds.Tables[0].Rows[0]["ReturnNo"].ToString();
            txtretdate.Text = ds.Tables[0].Rows[0]["returndate"].ToString();
        }
        DGReturnedDetail.DataSource = ds.Tables[0];
        DGReturnedDetail.DataBind();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_MOTTELINGRETURN] Where id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptmottelingreturn.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptmottelingreturn.xsd";
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
    protected void FillIssueDetail()
    {
        string str = @"select dbo.F_getItemDescription(YT.iFinishedid,YT.iflagsize) as ItemDescription,
                        U.UnitName,YT.Lotno,YT.Tagno,Round(yt.IssueQty,3) as IssueQty, isnull(YRQ.retqty,0) as Returnedqty,YT.ifinishedid,YT.Unitid,YT.iflagsize as flagsize,YM.ID,yt.Detailid
                        from MottelingIssueMaster YM inner join MottelingIssueDetail YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        left join V_getMottelingReturnQty YRQ on YT.Detailid=YRQ.Issuedetailid and YT.MasterId=YRQ.issuemasterid 
                        Where YM.Id=" + DDissueno.SelectedValue;
        str = str + " select isnull(dbo.[F_GETMOTTELINGRECDQTY_ISSUENO](" + DDissueno.SelectedValue + "),0) as Recdqty";
        // Where YM.Id=" + DDissueno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        lbltotalrec.Text = "Total Received Qty : 0";
        if (ds.Tables[1].Rows.Count > 0)
        {
            lbltotalrec.Text = "Total Received Qty : " + ds.Tables[1].Rows[0]["Recdqty"];
        }

    }
    protected void DGReturnedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lblDetailid = (Label)DGReturnedDetail.Rows[e.RowIndex].FindControl("lblDetailid");
            Label lblid = (Label)DGReturnedDetail.Rows[e.RowIndex].FindControl("lblid");
            SqlParameter[] arr = new SqlParameter[3];
            arr[0] = new SqlParameter("@Detailid", lblDetailid.Text);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;
            arr[2] = new SqlParameter("@ID", lblid.Text);
            //***********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEMOTTELINGRETURN", arr);
            lblmessage.Text = arr[1].Value.ToString();
            Tran.Commit();
            FillReturnDetails();
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
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        chkcomplete.Checked = false;
        TDreturnNo.Visible = false;
        ViewState["reportid"] = "0";
        txtretNo.Text = "";
        txtretdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        DDPartyName.SelectedIndex = -1;
        if (chkedit.Checked == true)
        {
            DDissueno.Items.Clear();
            DDreturnno.Items.Clear();
            TDreturnNo.Visible = true;
        }
        DG.DataSource = null;
        DG.DataBind();
    }
    protected void DDreturnno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDreturnno.SelectedValue;
        FillReturnDetails();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Empid From EmpInfo EI  Where EmpCode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDPartyName.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDPartyName.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDPartyName_SelectedIndexChanged(sender, new EventArgs());
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
            txtWeaverIdNoscan.Focus();
        }
    }
}