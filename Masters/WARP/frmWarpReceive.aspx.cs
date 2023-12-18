using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_WARP_frmWarpReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CompanyId,CompanyName from companyinfo Where mastercompanyId=" + Session["varcompanyId"] + @" order by Companyname
                           select D.Departmentid,D.Departmentname from Department D Where D.DepartmentName='WARPING' order by Departmentname                           
                            Select GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"]+@" Order by GodownName
                           select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where Process_Name in('WARPING WOOL','WARPING COTTON') and MasterCompanyid=" + Session["varcompanyid"];
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDDept, ds, 1, true, "--Plz Select--");
            if (DDDept.Items.Count > 0)
            {
                DDDept.SelectedIndex = 1;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDgodown, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 3, true, "--Plz Select--");

            txtReceivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["reportid"] = "0";
            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
                TDcomplete.Visible = true;
            }

        }
    }
    //protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref DDemployee, "select EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " order by EI.EmpName", true, "--Plz Select--");
    //}
    protected void DDemployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        DG.DataSource = null;
        DG.DataBind();
        FIllIssueno();
    }
    protected void FIllIssueno()
    {
        string str;
        if (Chkedit.Checked == true)
        {
            string status = "Pending";
            if (chkcomplete.Checked == true)
            {
                status = "Complete";
            }
            str = "select  ID,IssueNo+'/'+REPLACE(CONVERT(nvarchar(11),Issuedate,106),' ','-') as IssueNo from WarpOrderMaster Where CompanyId=" + DDcompany.SelectedValue + " and Empid=" + DDemployee.SelectedValue + " and Status='" + status + "' and Processid=" + DDProcess.SelectedValue + "  order by Id desc";
        }
        else
        {
            str = "select  ID,IssueNo+'/'+REPLACE(CONVERT(nvarchar(11),Issuedate,106),' ','-') as IssueNo from WarpOrderMaster Where CompanyId=" + DDcompany.SelectedValue + " and Empid=" + DDemployee.SelectedValue + " and Status='Pending' and Processid=" + DDProcess.SelectedValue + " order by Id desc";
        }
        UtilityModule.ConditionalComboFill(ref DDIssueno, str, true, "--Plz Select--");

    }
    protected void DDIssueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = "0";
        if (Chkedit.Checked == true)
        {
            FillBeamNo();
        }
        FillGrid();
    }
    protected void FillGrid()
    {
        string str = @"select Distinct dbo.F_getItemDescription(WRD.ofinishedid,WRD.osizeflag) as ItemDescription,U.unitname,
                            NoofBeamReq,dbo.F_getWarpLoomRecqty(WD.Id,WD.Detailid,WRD.Ofinishedid) As ReceivedQty,WD.iD,WD.Detailid,Ofinishedid,OSizeflag,
                            OUnitid,WarpLotnoTagno.Lotno,WarpLotnoTagno.TagNo,WD.Pcs from WarpRawissue_Receivedetail WRD 
                            inner join Warporderdetail WD on WRD.Issuedetailid=WD.Detailid and WD.id=" + DDIssueno.SelectedValue + @"
                            inner join unit U on WRD.ounitid=U.unitid
                            cross apply(select * from Dbo.[F_GetWarpRawLotno_TagNo](WD.Id)) As WarpLotnoTagNo";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DG.DataSource = ds.Tables[0];
        DG.DataBind();


        txtgrossweight.Text = "";
        txttareweight.Text = "";
        txtnetweight.Text = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            TRWeight.Visible = true;
        }
        else
        {
            TRWeight.Visible = false;
        }
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
        dtrecords.Columns.Add("Ofinishedid", typeof(int));
        dtrecords.Columns.Add("OUnitid", typeof(int));
        dtrecords.Columns.Add("Osizeflag", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("ReceiveQty", typeof(float));
        dtrecords.Columns.Add("Issuemasterid", typeof(int));
        dtrecords.Columns.Add("IssueDetailid", typeof(int));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblofinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lblounitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblosizeflag"));
                string LotNo = ((Label)DG.Rows[i].FindControl("lblLotno")).Text;
                string TagNo = ((Label)DG.Rows[i].FindControl("lblTagno")).Text;
                Label lblissuemasterid = ((Label)DG.Rows[i].FindControl("lblid"));
                Label lblissueDetailid = ((Label)DG.Rows[i].FindControl("lbldetailid"));
                //*********************
                DataRow dr = dtrecords.NewRow();
                dr["Ofinishedid"] = lblitemfinishedid.Text;
                dr["OUnitid"] = lblunitid.Text;
                dr["Osizeflag"] = lblflagsize.Text;
                dr["Godownid"] = DDgodown.SelectedValue;
                dr["Lotno"] = LotNo;
                dr["TagNo"] = TagNo;
                dr["ReceiveQty"] = 1;
                dr["Issuemasterid"] = lblissuemasterid.Text;
                dr["IssueDetailid"] = lblissueDetailid.Text;
                dtrecords.Rows.Add(dr);
            }
        }
        if (dtrecords.Rows.Count > 0)
        {
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = ViewState["reportid"];
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@DeptId", DDDept.SelectedValue);
                param[4] = new SqlParameter("@Empid", DDemployee.SelectedValue);
                param[5] = new SqlParameter("@LoomNo", SqlDbType.VarChar, 50);
                param[5].Value = txtloomno.Text;
                param[5].Direction = ParameterDirection.InputOutput;
                param[6] = new SqlParameter("@Receivedate", txtReceivedate.Text);
                param[7] = new SqlParameter("@Issuemasterid", DDIssueno.SelectedValue);
                param[8] = new SqlParameter("@userid", Session["varuserid"]);
                param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[9].Direction = ParameterDirection.Output;
                param[10] = new SqlParameter("@Grossweight", txtgrossweight.Text == "" ? "0" : txtgrossweight.Text);
                param[11] = new SqlParameter("@TareWeight", txttareweight.Text == "" ? "0" : txttareweight.Text);
                param[12] = new SqlParameter("@NetWeight", txtnetweight.Text == "" ? "0" : txtnetweight.Text);
                param[13] = new SqlParameter("@Processid", DDProcess.SelectedValue);
                param[14] = new SqlParameter("@Pcs", txtpcs.Text == "" ? "0" : txtpcs.Text);
                param[15] = new SqlParameter("@McNo", txtm_cno.Text);
                param[16] = new SqlParameter("@Remark", txtRemark.Text);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_saveWarpReceive", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                //lblmessage.Text = param[9].Value.ToString();
                txtloomno.Text = param[5].Value.ToString();
                if (param[9].Value.ToString() == "")
                {
                    Tran.Commit();
                    lblmessage.Text = "DATA SAVED SUCCESSFULLY...";
                    txtpcs.Text = "";
                    txtm_cno.Text = "";
                    FillBeamDetail();
                    DDemployee_SelectedIndexChanged(sender, e);
                }
                else
                {
                    Tran.Commit();
                    lblmessage.Text = param[9].Value.ToString();
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
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = "select * from  V_BeamReceive where Id=" + ViewState["reportid"];
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rptBeamreceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptBeamreceive.xsd";
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
        DDProcess.SelectedIndex = -1;
        DDProcess_SelectedIndexChanged(DDProcess, e);
        if (Chkedit.Checked == true)
        {
            TDBeamNo.Visible = true;
            DDIssueno.SelectedIndex = -1;
        }
        else
        {
            ViewState["reportid"] = "0";
            TDBeamNo.Visible = false;
            DDIssueno.SelectedIndex = -1;
            txtReceivedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtloomno.Text = "";
            DG.DataSource = null;
            DG.DataBind();
            DGBeam.DataSource = null;
            DGBeam.DataBind();
        }
    }
    protected void FillBeamNo()
    {
        string str = @"select Distinct WLM.ID,WLM.LoomNo 
                    From WarpLoommaster WLM(Nolock) 
                    join WarpLoomDetail WLD(Nolock) on WLM.ID=WLD.ID 
                    join LoomStock LS(Nolock) on WLM.LoomNo=LS.LoomNo 
                    Where WLM.CompanyId=" + DDcompany.SelectedValue + " and WLM.DeptId=" + DDDept.SelectedValue + " and WLM.EmpId=" + DDemployee.SelectedValue + " and WLD.Issuemasterid=" + DDIssueno.SelectedValue;

        if (Session["VarCompanyNo"].ToString() == "21" || Session["VarCompanyNo"].ToString() == "45")
        {
            str = str + " ";
        }
        else
        {
            str = str + " And LS.Qtyinhand>0 ";
        }
        str = str + " order by WLM.id";

        UtilityModule.ConditionalComboFill(ref DDBeamNo, str, true, "--Plz Select--");
    }
    protected void FillBeamDetail()
    {
        string str = @"select Distinct WLM.ID,WLM.Grossweight,WLM.TareWeight,WLM.NetWeight,WLM.LoomNo,Beam.Beamdescription,wld.pcs
                    from Warploommaster WLM inner join WarpLoomDetail WLD on WLM.ID=WLD.ID
                    cross apply (select * from dbo.[F_GetBeamDescriptionCommaSeparated](WLM.LoomNo)) Beam
                    Where WLM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGBeam.DataSource = ds.Tables[0];
        DGBeam.DataBind();
    }
    protected void DDBeamNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDBeamNo.SelectedValue;
        FillBeamDetail();
    }
    protected void DGBeam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGBeam.EditIndex = e.NewEditIndex;
        FillBeamDetail();
    }
    protected void DGBeam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGBeam.EditIndex = -1;
        FillBeamDetail();
    }
    protected void DGBeam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = (Label)DGBeam.Rows[e.RowIndex].FindControl("lblid");
            Label lblbeamno = (Label)DGBeam.Rows[e.RowIndex].FindControl("lblbeamno");
            TextBox txtgrossweight = (TextBox)DGBeam.Rows[e.RowIndex].FindControl("txtgrossweight");
            TextBox txttareweight = (TextBox)DGBeam.Rows[e.RowIndex].FindControl("txttareweight");
            TextBox txtnetweight = (TextBox)DGBeam.Rows[e.RowIndex].FindControl("txtnetweight");
            //***********            
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", lblid.Text);
            param[1] = new SqlParameter("@beamNo", lblbeamno.Text);
            param[2] = new SqlParameter("@grosswt", txtgrossweight.Text == "" ? "0" : txtgrossweight.Text);
            param[3] = new SqlParameter("@Tarewt", txttareweight.Text == "" ? "0" : txttareweight.Text);
            param[4] = new SqlParameter("@Netwt", 0);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            param[6] = new SqlParameter("@userid", Session["varuserid"]);
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_updateWarpReceive", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            DGBeam.EditIndex = -1;
            FillBeamDetail();
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

    protected void DGBeam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = (Label)DGBeam.Rows[e.RowIndex].FindControl("lblid");
            Label lblbeamno = (Label)DGBeam.Rows[e.RowIndex].FindControl("lblbeamno");
            //***********            
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", lblid.Text);
            param[1] = new SqlParameter("@beamNo", lblbeamno.Text);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //**************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteWarpReceive", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillBeamDetail();
            FillGrid();
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
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDemployee, "select Distinct EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " inner join WarpLoommaster WLM on ei.empid=WLM.empid and WLM.Processid=" + DDProcess.SelectedValue + " order by EmpName", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDemployee, "select EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " order by EI.EmpName", true, "--Plz Select--");
        }
        if (DDemployee.Items.Count > 0)
        {
            DDemployee_SelectedIndexChanged(DDemployee, e);
        }
    }
    protected void Chkboxitem_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkboxitem = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkboxitem.Parent.Parent;
        Label lblofinishedid = (Label)row.FindControl("lblofinishedid");
        txtmaterialissued.Text = "";
        txtweightrecd.Text = "";
        if (chkboxitem != null)
        {
            if (chkboxitem.Checked == true)
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@issueid", DDIssueno.SelectedValue);
                param[1] = new SqlParameter("@ofinishedid", lblofinishedid.Text);
                param[2] = new SqlParameter("@issuedqty", SqlDbType.Decimal);
                param[2].Precision = 18;
                param[2].Scale = 3;
                param[2].Direction = ParameterDirection.Output;
                param[3] = new SqlParameter("@RecdQty", SqlDbType.Decimal);
                param[3].Precision = 18;
                param[3].Scale = 3;
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_BEAMISSUED_RECDQTY", param);
                txtmaterialissued.Text = param[2].Value.ToString();
                txtweightrecd.Text = param[3].Value.ToString();
            }
        }


    }
    //protected void DG_RowDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    GridViewRow row = e.Row;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        row.Cells[5].Text = row.Cells[5].Text.Replace("\n", "<br />");


    //        Label lblLotno = e.Row.FindControl("lblLotno") as Label;
    //        Label lblTagno = e.Row.FindControl("lblTagno") as Label;
    //        if (null != lblLotno)
    //        {
    //            lblLotno.Text = lblLotno.Text.Replace(System.Environment.NewLine, "<br />");
    //        }
    //        //if (null != lblTagno)
    //        //{
    //        //    //lblTagno.Text = lblTagno.Text.Replace(System.Environment.NewLine, "<br />");
    //        //    lblTagno.Text = lblTagno.Text.Replace("<br>","\r\n");
               
    //        //}

    //        //row.Cells[4].Text = row.Cells[4].Text.Replace("\n", "<br />");  
    //    }
    //}

}