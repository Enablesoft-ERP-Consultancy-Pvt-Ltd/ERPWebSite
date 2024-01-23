using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_YarnOpening_frmyarnopeningReceiveNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName 
                           select EI.EmpId,EI.EmpName+case when isnull(EI.empcode,'')<>'' Then ' ['+EI.empcode+']' Else '' End Empname  from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') and isnull(Blacklist,0)=0";
            if (Session["varcompanyNo"].ToString() != "16")
            {
                if (variable.VarYARNOPENINGISSUEEMPWISE == "0")
                {

                    str = str + "  and EI.EmpName in('Yarn Opening','YARN OPENING-2','YARN OPENING-3')";
                }

            }
            str = str + " order by EmpName";
            str = str + " Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName";
            str = str + " select Godownid From ModuleWiseGodown where ModuleName='" + Page.Title + "' ";
            str = str + " select D.Departmentid,D.DepartmentName From Department D Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT')";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDvendor, ds, 1, false, "");
            if (DDvendor.Items.Count > 0)
            {
                DDvendor_SelectedIndexChanged(sender, e);
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 2, true, "--Plz Select--");
            if (ds.Tables[3].Rows.Count > 0)
            {
                if (DDGodown.Items.FindByValue(ds.Tables[3].Rows[0]["godownid"].ToString()) != null)
                {
                    DDGodown.SelectedValue = ds.Tables[3].Rows[0]["godownid"].ToString();
                    DDGodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            else
            {
                if (DDGodown.Items.Count > 0)
                {
                    DDGodown.SelectedIndex = 1;
                    DDGodown_SelectedIndexChanged(sender, new EventArgs());
                }
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDdept, ds, 4, true, "--Select Department--");

            txtRecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
            {
                lblyarnopendept.Text = "Employee Name";
                TDdept.Visible = true;
            }
            
            //**************
            if (Session["canedit"].ToString() == "1")
            {
                Chkedit.Visible = true;
                chkcompleteissueno.Visible = true;
            }
            //if (variable.VarBINNOWISE == "1")
            //{
            //    TDBinNo.Visible = true;
            //}
        }
    }
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueno();
    }
    protected void FillIssueno()
    {

        string str = @"select  Distinct YM.Id,YM.Issueno+'/'+REPLACE(CONVERT(nvarchar(11),YM.Issuedate,106),' ','-') from YarnOpeningIssueMaster YM  inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId
                       Left join YarnOpeningReceiveTran YRT on YM.ID=YRT.issuemasterid Where ym.vendorId=" + DDvendor.SelectedValue + @" and ym.companyid=" + DDcompany.SelectedValue + " and ym.Status='Pending'";
        if (DDdept.SelectedIndex>0)
        {
            str = str + " and Ym.departmentid=" + DDdept.SelectedValue;
        }
        if (txtLotno.Text != "")
        {
            str = str + " and yt.Lotno='" + txtLotno.Text + "'";
        }
        str = str + "  and Yrt.issuemasterid is null order by ym.id desc";

        str = str + @" select EI.EmpId,EI.EmpName+' '+case WHen EI.EMpcode<>'' Then '['+EI.empcode+']' Else '' End as EmpName from empinfo EI inner join Department D 
                       on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') and isnull(Blacklist,0)=0 order by EmpName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //UtilityModule.ConditionalComboFillWithDS(ref DDIssueno, ds, 0, true, "--Select--");
        UtilityModule.ConditonalChkBoxListFillWithDs(ref chkissueno, ds);
        UtilityModule.ConditionalComboFillWithDS(ref DDemployee, ds, 1, true, "--Select--");
        if (DDemployee.Items.Count > 0)
        {
            if (DDemployee.Items.FindByValue(DDvendor.SelectedValue) != null)
            {
                DDemployee.SelectedValue = DDvendor.SelectedValue;
            }
        }
    }
    protected void FillIssueDetail()
    {
        string str = @"select dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,YT.Lotno,YT.Tagno,Round(yt.IssueQty-isnull(VRT.Retqty,0),3) as IssueQty,Round(isnull(YR.ReceivedQty,0),3) As ReceivedQty,YT.Item_Finished_id,YT.Unitid,YT.flagsize,YM.ID,yt.Detailid,
                        ISNULL(Rectype,'') as RecType,Isnull(YT.ConeType,'') as Conetype,
                        Isnull(YT.Rate,0) as Rate
                        from YarnOpeningIssueMaster YM inner join YarnOpeningIssueTran YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        left join V_getYarnOpeningReceivedQty YR on Yt.Detailid=YR.issuemasterDetailid and YT.MasterId=YR.issuemasterid
                        left join V_getYarnOpeningReturnQty VRT on YT.MasterId=VRT.issuemasterid and YT.Detailid=VRT.Issuedetailid
                        Where YM.Id=" + DDIssueno.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DDIssueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueDetail();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        //********sql table Type
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("IssueId", typeof(int));

        //*******************
        for (int j = 0; j < chkissueno.Items.Count; j++)
        {
            if (chkissueno.Items[j].Selected)
            {
                DataRow dr = dtrecords.NewRow();
                dr["issueid"] = chkissueno.Items[j].Value;
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
                param[1] = new SqlParameter("@DT", dtrecords);
                param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@vendorid", DDvendor.SelectedValue);
                param[4] = new SqlParameter("@receiveNo", txtreceiveNo.Text);
                param[5] = new SqlParameter("@RecDate", txtRecdate.Text);
                param[7] = new SqlParameter("@userid", Session["varuserid"]);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@Empid", DDemployee.SelectedValue);
                param[10] = new SqlParameter("@Wastage", txtwastageweight.Text == "" ? "0" : txtwastageweight.Text);
                param[11] = new SqlParameter("@Godownid", DDGodown.SelectedValue);
                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveYarnReceiveNew", param);
                //*******************
                if (param[8].Value.ToString() != "")
                {
                    lblmessage.Text = param[8].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    ViewState["reportid"] = param[0].Value.ToString();
                    lblmessage.Text = "Data Saved successfully.";
                    Tran.Commit();
                    //FillIssueDetail();
                    chkissueno.Items.Clear();
                    FillReceiveDetails();
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

            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one Issue No. to save data.');", true);
        }

    }
    protected void Refreshcontrol()
    {
        //DDIssueno.SelectedIndex = -1;
        DDemployee.SelectedIndex = -1;
        txtreceiveNo.Text = "";
        txtRecdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        // DG.DataSource = null;
        //DG.DataBind();
    }
    protected void FillReceiveDetails()
    {
        string str = @"select YM.ReceiveNo,dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,Gm.GodownName,YT.Lotno,YT.Tagno,yt.ReceiveQty,Yt.LossQty,YM.ID,yt.Detailid,YT.issuemasterid,YT.issuemasterDetailid,yt.rectype,yt.noofcone,yt.Conetype
                        from YarnOpeningreceiveMaster YM inner join YarnOpeningReceiveTran YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        inner join GodownMaster GM on YT.GodownId=Gm.GoDownID Where YM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGReceivedDetail.DataSource = ds.Tables[0];
        DGReceivedDetail.DataBind();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            Label lblrectype = (Label)e.Row.FindControl("lblrectype");
            Label lblconetype = (Label)e.Row.FindControl("lblconetype");
            DropDownList DDRecType = (DropDownList)e.Row.FindControl("DDRecType");
            DropDownList DDconetype = (DropDownList)e.Row.FindControl("DDconetype");
            DropDownList DDBinNo = ((DropDownList)e.Row.FindControl("DDBinNo"));

            if (DDRecType.Items.FindByText(lblrectype.Text) != null)
            {
                DDRecType.SelectedValue = lblrectype.Text;
            }
            if (DDconetype.Items.FindByText(lblconetype.Text) != null)
            {
                DDconetype.SelectedValue = lblconetype.Text;
            }
            string str = @"Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                            Select godownid From Modulewisegodown Where ModuleName='" + Page.Title + @"'
                            Select ConeType, ConeType From ConeMaster Order By SrNo ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDconetype, ds, 2, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, false, "");
            if (ds.Tables[1].Rows.Count > 0)
            {
                if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                {
                    DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    if (variable.VarBINNOWISE == "1")
                    {
                        if (variable.VarCHECKBINCONDITION == "1")
                        {
                            UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
                        }
                        else
                        {
                            UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
                        }
                    }
                }
            }
            //**************COLUMN BIN NO VISIBLE TRUE OR FALSE
            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (DG.Columns[i].HeaderText.ToUpper() == "BIN NO")
                {
                    if (variable.VarBINNOWISE == "1")
                    {
                        DG.Columns[i].Visible = true;
                    }
                    else
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_yarnOpeningReceive] Where id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (Session["varMasterCompanyIDForERP"].ToString())
            {
                case "14":
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningReceive.rpt";
                    break;
                default:
                    Session["rptFileName"] = "~\\Reports\\rptyarnopeningReceivewithrate.rpt";
                    break;
            }
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningReceive.xsd";
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
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            TDIssueNo.Visible = false;
            TDReceiveNo.Visible = true;
            DG.DataSource = null;
            DG.DataBind();
        }
        else
        {
            TDIssueNo.Visible = true;
            TDReceiveNo.Visible = false;
        }
    }
    protected void DDemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillReceiveNo();
    }
    protected void fillReceiveNo()
    {
        string str;
        str = @"select Distinct YRM.Id,ReceiveNo+'/'+REPLACE(CONVERT(nvarchar(11),Receivedate,106),' ','-') as Receivedate  from YarnOpeningReceiveMaster YRM 
        inner Join YarnOpeningReceiveTran YRT on YRM.ID=YRT.MasterId inner join YarnOpeningIssueMaster YIM on YRT.issuemasterid=YIM.ID  
        Where YRM.CompanyId=" + DDcompany.SelectedValue + " and YRM.Empid=" + DDemployee.SelectedValue + " and Yrm.vendorid=" + DDvendor.SelectedValue + " ";
        if (chkcompleteissueno.Checked == true)
        {
            str = str + " And YIM.status='Complete' ";
        }
        else
        {
            str = str + "  And YIM.status='Pending' ";
        }
        str = str + " order by YRM.ID desc";
        UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDReceiveNo.SelectedValue;
        FillReceiveDetails();
    }
    protected void DGReceivedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DGReceivedDetail.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DGReceivedDetail.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteYarnOpeningReceive", param);
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillReceiveDetails();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcomplete.Checked == true)
        {
            if (DDvendor.SelectedIndex <= 0)
            {
                lblmessage.Text = "Please select Yarn Opening Dept.";
                return;
            }
            DGGridForComp.DataSource = null;
            DGGridForComp.DataBind();
            ModalPopupExtender1.Show();

            string str = @"select  Distinct YM.Id,YM.Issueno+'/'+REPLACE(CONVERT(nvarchar(11),YM.Issuedate,106),' ','-') from YarnOpeningIssueMaster YM  inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId
                       inner join YarnOpeningReceiveTran YRT on YM.ID=YRT.issuemasterid
                       Where ym.vendorId=" + DDvendor.SelectedValue + @" and ym.companyid=" + DDcompany.SelectedValue + " and ym.Status='Pending'";
            if (txtLotno.Text != "")
            {
                str = str + " and yt.Lotno='" + txtLotno.Text + "'";
            }
            str = str + "  order by ym.id desc";
            UtilityModule.ConditionalComboFill(ref DDcompissueno, str, true, "--Select--");

        }
        //else
        //{
        //    FillIssueno();
        //}
    }
    protected void btncomp_Click(object sender, EventArgs e)
    {
        string strinsert = "";
        string Id, Detailid, GainQty, LossQty, str;
        Id = "";

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);

        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            //********Check Pending 
            str = @"select Distinct Yt.MasterId From YarnOpeningIssueTran YT Left join  V_getYarnOpeningReturnQty VRT  on YT.detailid=VRT.Issuedetailid
             Left join YarnOpeningReceiveTran YRT on YT.Detailid=YRT.issuemasterDetailid
             Where  YT.MasterId=" + DDcompissueno.SelectedValue + " and YRT.issuemasterDetailid is null and yt.issueqty-isnull(vrt.retqty,0)>0 ";
            DataSet ds = SqlHelper.ExecuteDataset(Tran, CommandType.Text, str);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "altr", "alert('All shade must be final to Complete status.')", true);
                ModalPopupExtender1.Show();
                Tran.Commit();
                return;
            }
            //
            for (int i = 0; i < DGGridForComp.Rows.Count; i++)
            {
                Id = ((Label)DGGridForComp.Rows[i].FindControl("lblId")).Text;
                Detailid = ((Label)DGGridForComp.Rows[i].FindControl("lblDetailId")).Text;
                GainQty = ((Label)DGGridForComp.Rows[i].FindControl("lblgainQty")).Text;
                LossQty = ((Label)DGGridForComp.Rows[i].FindControl("lblLossqty")).Text;
                if (i == 0)
                {
                    strinsert = strinsert + " Delete from YarnOpeningCompleteStatus Where IssueId=" + Id;
                }
                strinsert = strinsert + " Insert into YarnOpeningCompleteStatus(IssueId,DetailId,GainQty,LossQty,Userid)values(" + Id + "," + Detailid + "," + GainQty + "," + LossQty + "," + Session["varuserid"] + ")";
            }
            if (strinsert != "")
            {
                strinsert = strinsert + " update yarnOpeningIssuemaster set Status='Complete' where Id=" + Id + "";
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, strinsert);
            }
            Tran.Commit();
            lblmessage.Text = "Issue No. Compeleted Successfully.";
            FillIssueno();
            chkcomplete.Checked = false;
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
    protected void txtLotno_TextChanged(object sender, EventArgs e)
    {
        FillIssueno();
    }
    protected void txtWeaverIdNoscan_TextChanged(object sender, EventArgs e)
    {
        string str = @"select Empid From EmpInfo EI inner join Department Dp on EI.Departmentid=DP.departmentid 
                       and Dp.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') Where EmpCode='" + txtWeaverIdNoscan.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (DDvendor.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
            {
                DDvendor.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
                DDIssueno.Focus();
                if (Session["varcompanyNo"].ToString() == "16")
                {
                    if (DDemployee.Items.FindByValue(ds.Tables[0].Rows[0]["Empid"].ToString()) != null)
                    {
                        DDemployee.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                    }

                }
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
            txtWeaverIdNoscan.Focus();
        }
    }
    protected void chkcompleteissueno_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcompleteissueno.Checked == true)
        {
            if (TDReceiveNo.Visible == true)
            {
                fillReceiveNo();
            }
            DG.DataSource = null;
            DG.DataBind();
        }

    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (variable.VarCHECKBINCONDITION == "1")
        {
            UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
        }
    }
    protected void DDGodownDG_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDGodown = (DropDownList)sender;
        GridViewRow gvr = (GridViewRow)DDGodown.Parent.Parent;

        DropDownList DDBinNo = (DropDownList)gvr.FindControl("DDBinNo");

        if (variable.VarBINNOWISE == "1")
        {
            if (variable.VarCHECKBINCONDITION == "1")
            {
                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(DDGodown.SelectedValue), Item_finished_id: 0, New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
            }
        }
    }
    protected void DDcompissueno_SelectedIndexChanged(object sender, EventArgs e)
    {

        string str = @"select dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,yt.IssueQty-isnull(vrt.retqty,0) as Issueqty,
                            isnull(YR.ReceivedQty,0)+isnull(vl.lossqty,0) As ReceivedQty,
                            YT.Item_Finished_id,YM.ID,yt.Detailid,Round(case When ((Yt.issueqty-isnull(vrt.retqty,0))-(isnull(YR.ReceivedQty,0)+isnull(vl.lossqty,0)))>0 
                            Then ((Yt.issueqty-isnull(VRT.Retqty,0))-(isnull(YR.ReceivedQty,0)+isnull(vl.lossqty,0))) ELse 0 End,3) as LossQty,
                            0 as GainQty
                            from YarnOpeningIssueMaster YM inner join YarnOpeningIssueTran YT on 
                            YM.ID=YT.MasterId
                            inner join Unit U on YT.Unitid=U.UnitId
                            left join V_getYarnOpeningReceivedQty YR on Yt.Detailid=YR.issuemasterDetailid 
                            and YT.MasterId=YR.issuemasterid 
                            left join V_getYarnOpeningReturnQty VRT on YT.detailid=VRT.Issuedetailid
                            LEFT JOIN V_GETYARNOPENINGLOSSQTY Vl ON yT.DETAILID=vl.detailid
                            Where YM.Status='Pending' and YM.ID=" + DDcompissueno.SelectedValue + " and yt.IssueQty-isnull(vrt.retqty,0)>0";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGGridForComp.DataSource = ds.Tables[0];
        DGGridForComp.DataBind();

        ModalPopupExtender1.Show();
    }
    protected void DDdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = @" select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where isnull(Ei.Blacklist,0)=0 and D.departmentid=" + DDdept.SelectedValue + " order by EmpName  ";
        UtilityModule.ConditionalComboFill(ref DDvendor, str, false, "");
        if (DDvendor.Items.Count>0)
        {
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
        }

    }
}