using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_YarnOpening_frmyarnopeningreturn : System.Web.UI.Page
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
                           select EI.EmpId,EI.EmpName+case when isnull(EI.empcode,'')<>'' Then ' ['+EI.empcode+']' Else '' End Empname from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT') and isnull(Blacklist,0)=0";
            if (Session["varMasterCompanyIDForERP"].ToString() == "14")
            {
                str = str + " and EI.EmpName in('Yarn Opening','YARN OPENING-2','YARN OPENING-3')";
            }
            str = str + "  order by EmpName";
            str = str + " Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName";
            str = str + " select Godownid From ModuleWiseGodown where ModuleName='" + Page.Title + "' ";
            str = str + @" Select ID, BranchName 
                    From BRANCHMASTER BM(nolock) 
                    JOIN BranchUser BU(nolock) ON BU.BranchID = BM.ID And BU.UserID = " + Session["varuserId"] + @" 
                    Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varMasterCompanyIDForERP"];

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDBranchName, ds, 4, false, "");
            DDBranchName.Enabled = false;
            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDvendor, ds, 1, false, "");
            if (DDvendor.Items.Count > 0)
            {
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
            }
            //*******GODOWN
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
            //**************
            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }
            txtretdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            if (variable.VarBINNOWISE == "1")
            {
                TDBinNo.Visible = true;
            }
            //**********
        }
    }
    protected void FillIssueno()
    {

        string str = @"select  Distinct YM.Id,YM.Issueno+'/'+REPLACE(CONVERT(nvarchar(11),YM.Issuedate,106),' ','-') 
        from YarnOpeningIssueMaster YM 
        inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId 
        Where ym.companyid=" + DDcompany.SelectedValue + " And ym.BranchID = " + DDBranchName.SelectedValue + " and ym.vendorId=" + DDvendor.SelectedValue + @" ";
        if (txtLotno.Text != "")
        {
            str = str + " and yt.Lotno='" + txtLotno.Text + "'";
        }
        if (ChkForComplete.Checked == true)
        {
            str = str + " and ym.Status='COMPLETE' OR ym.Status='Complete'";
        }
        else
        {
            str = str + " and ym.Status='Pending'";
        }
        str = str + " order by ym.id desc";
        //        str = str + @" select EI.EmpId,EI.EmpName+' '+case WHen EI.EMpcode<>'' Then '['+EI.empcode+']' Else '' End as EmpName from empinfo EI inner join Department D 
        //                       on EI.departmentId=D.DepartmentId Where D.DepartmentName='Yarn Opening' order by EmpName";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        UtilityModule.ConditionalComboFillWithDS(ref DDIssueno, ds, 0, true, "--Select--");
    }
    protected void ChkForComplete_CheckedChanged(object sender, EventArgs e)
    {
        FillIssueno();
    }

    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueno();
    }
    protected void DDIssueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillIssueDetail();
    }
    protected void FillIssueDetail()
    {
        string str = @"select dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,YT.Lotno,YT.Tagno,Round(yt.IssueQty-isnull(YRQ.retqty,0),3) as IssueQty,Round(isnull(YR.ReceivedQty,0),3) As ReceivedQty,YT.Item_Finished_id,YT.Unitid,YT.flagsize,YM.ID,yt.Detailid
                        from YarnOpeningIssueMaster YM inner join YarnOpeningIssueTran YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        left join V_getYarnOpeningReceivedQty YR on Yt.Detailid=YR.issuemasterDetailid and YT.MasterId=YR.issuemasterid
                        left join V_getYarnOpeningReturnQty YRQ on YT.Detailid=YRQ.Issuedetailid and YT.MasterId=YRQ.issuemasterid Where YM.Id=" + DDIssueno.SelectedValue;
        DataSet ds1 = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        ds1.Tables[0].DefaultView.RowFilter = "(Issueqty-ReceivedQty)>0";
        DataView dv = ds1.Tables[0].DefaultView;
        DataSet ds = new DataSet();
        ds.Tables.Add(dv.ToTable());
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (ChKForEdit.Checked == true)
        {
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select distinct ID, ReturnNo 
            From YARNOPENINGRETURNMASTER a(Nolock)
            JOIN YARNOPENINGRETURNDETAIL b(Nolock) on b.MasterID = a.ID And b.IssueMasterID = " + DDIssueno.SelectedValue + @" 
            Where mastercompanyid = " + Session["varMasterCompanyIDForERP"] + " And BranchID = " + DDBranchName.SelectedValue + @"
            Order By ID Desc ");
        
            UtilityModule.ConditionalComboFillWithDS(ref DDChallanNo, ds, 0, true, "--Select--");            
        }
    }
    protected void txtLotno_TextChanged(object sender, EventArgs e)
    {
        FillIssueno();
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
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = 0;
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@vendorid", DDvendor.SelectedValue);
                param[4] = new SqlParameter("@returnNo", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.Output;
                param[5] = new SqlParameter("@RetDate", txtretdate.Text);
                param[6] = new SqlParameter("@userid", Session["varuserid"]);
                param[7] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[7].Direction = ParameterDirection.Output;
                param[8] = new SqlParameter("@mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[9] = new SqlParameter("@issuemasterid", DDIssueno.SelectedValue);
                param[10] = new SqlParameter("@BinNo", TDBinNo.Visible == true ? DDBinNo.SelectedItem.Text : "");
                param[11] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_yarnopeningReturn", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                lblmessage.Text = param[7].Value.ToString();
                txtretNo.Text = param[4].Value.ToString();
                Tran.Commit();
                FillIssueDetail();
                FillReturnDetails();
                //Refresh Data
                DG.DataSource = null;
                DG.DataBind();
                DDvendor_SelectedIndexChanged(sender, new EventArgs());
                //
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
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_yarnopeningReturn] Where id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptyarnopeningReturn.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningReturn.xsd";
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
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string str1 = "";
            str1 = "Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName";

            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            UtilityModule.ConditionalComboFill(ref DDGodown, str1, false, "");
        }
    }
    protected void FillReturnDetails()
    {
        string str = @"select YM.ReturnNo,dbo.F_getItemDescription(YT.Item_Finished_id,YT.flagsize) as ItemDescription,
                        U.UnitName,Gm.GodownName,YT.Lotno,YT.Tagno,yt.Retqty as Returnqty,YM.ID,yt.Detailid
                        from YarnOpeningReturnMaster YM inner join YarnOpeningReturnDetail YT on 
                        YM.ID=YT.MasterId
                        inner join Unit U on YT.Unitid=U.UnitId
                        inner join GodownMaster GM on YT.GodownId=Gm.GoDownID Where YM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGReturnedDetail.DataSource = ds.Tables[0];
        DGReturnedDetail.DataBind();
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
                txtWeaverIdNoscan.Text = "";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "altemp", "alert('Employee Code does not exists in this Department.')", true);
            txtWeaverIdNoscan.Focus();
        }
    }
    protected void DDGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDBinNo, "SELECT DISTINCT BINNO,BINNO AS BINNO1 FROM STOCK S WHERE GODOWNID=" + DDGodown.SelectedValue + "  ORDER BY BINNO1", true, "--Plz Select--");
    }
    protected void ChKForEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (ChKForEdit.Checked == true)
        {
            tdChallanNo.Visible = true;
            ChkForComplete.Visible = true;
        }
        else
        {
            tdChallanNo.Visible = false;
            ChkForComplete.Visible = false;
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDChallanNo.SelectedValue;
        FillReturnDetails();
    }
    protected void DGReturnedDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DGReturnedDetail.Rows[e.RowIndex].FindControl("lblissuemasterid")).Text;
            string Detailid = ((Label)DGReturnedDetail.Rows[e.RowIndex].FindControl("lblissuemasterdetailid")).Text;
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_DELETEYARNOPENINGRETURN", param);
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillReturnDetails();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }
}