using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_WARP_frmwarpIssueNew : System.Web.UI.Page
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
                            Select D.Departmentid,D.Departmentname from Department D Where D.DepartmentName='WARPING' order by Departmentname
                            Select PROCESS_NAME_ID,PROCESS_NAME from Process_Name_Master Where Process_Name in('WARPING WOOL','WARPING COTTON') and MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @"
                            Select customerid,CustomerCode+'  '+companyname as customer from customerinfo WHere mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by customer
                            Select UnitsID, UnitName From Units(Nolock) Where MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By UnitName ";
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
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 3, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProductionUnit, ds, 4, true, "--Plz Select--");
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
                TDcomplete.Visible = true;
            }
        }
    }


    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        //****Sql Table Types
        DataTable dtrecords = new DataTable();
        dtrecords.Columns.Add("Item_finished_id", typeof(int));
        dtrecords.Columns.Add("flagsize", typeof(int));
        dtrecords.Columns.Add("Pcs", typeof(int));
        dtrecords.Columns.Add("Area", typeof(decimal));
        dtrecords.Columns.Add("Noofbeamreq", typeof(int));
        dtrecords.Columns.Add("Rate", typeof(decimal));

        for (int i = 0; i < GvBeamDesc.Rows.Count; i++)
        {
            DataRow dr = dtrecords.NewRow();
            CheckBox Chkboxitem = (CheckBox)GvBeamDesc.Rows[i].FindControl("Chkboxitem");
            TextBox txtissueqty = (TextBox)GvBeamDesc.Rows[i].FindControl("txtissueqty");
            TextBox txtrate = (TextBox)GvBeamDesc.Rows[i].FindControl("txtrate");

            if (Chkboxitem.Checked == true && Convert.ToInt32((txtissueqty.Text == "" ? "0" : txtissueqty.Text)) > 0)
            {
                Label lblitemfinishedid = (Label)GvBeamDesc.Rows[i].FindControl("lblitemfinishedid");
                Label lblosizeflag = (Label)GvBeamDesc.Rows[i].FindControl("lblosizeflag");
                Label lblarea = (Label)GvBeamDesc.Rows[i].FindControl("lblarea");
                TextBox txtnoofbeamreq = (TextBox)GvBeamDesc.Rows[i].FindControl("txtnoofbeamreq");

                dr["Item_Finished_id"] = lblitemfinishedid.Text;
                dr["flagsize"] = lblosizeflag.Text;
                dr["Pcs"] = txtissueqty.Text;
                dr["Area"] = lblarea.Text;
                dr["Noofbeamreq"] = txtnoofbeamreq.Text == "" ? "0" : txtnoofbeamreq.Text;
                dr["Rate"] = txtrate.Text == "" ? "0" : txtrate.Text;

                dtrecords.Rows.Add(dr);
            }
        }
        //*********
        if (dtrecords.Rows.Count > 0)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = hnid.Value == "" ? "0" : hnid.Value;
                param[1] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
                param[2] = new SqlParameter("@DeptId", DDDept.SelectedValue);
                param[3] = new SqlParameter("@Empid", DDEmp.SelectedValue);
                param[4] = new SqlParameter("@IssueNo", SqlDbType.VarChar, 50);
                param[4].Value = txtissueno.Text;
                param[4].Direction = ParameterDirection.InputOutput;
                param[5] = new SqlParameter("@IssueDate", txtissuedate.Text);
                param[6] = new SqlParameter("@TargetDate", txttargetdate.Text);
                param[7] = new SqlParameter("@Userid", Session["varuserid"]);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@Processid", DDProcess.SelectedValue);
                param[10] = new SqlParameter("@dtrecords", dtrecords);
                param[11] = new SqlParameter("@orderdetailid", DDitemdescription.SelectedValue);
                param[12] = new SqlParameter("@ProductionUnit", DDProductionUnit.SelectedValue);
                //**********************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWarpOrder", param);
                hnid.Value = param[0].Value.ToString();
                txtissueno.Text = param[4].Value.ToString();
                lblmessage.Text = param[8].Value.ToString();
                Tran.Commit();
                if (param[8].Value.ToString() != "")
                {
                    lblmessage.Text = param[8].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "Data saved successfully..";
                    FillMaingrid();
                    FillRawdetail();
                    FillBeamDescription();
                    //FillReceivedetail();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please fill all checked Mandatory field data.')", true);
        }

    }
    //protected void DDDept_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    UtilityModule.ConditionalComboFill(ref DDEmp, "select EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " order by EI.EmpName", true, "--Plz Select--");
    //}
    protected void DDEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        txtissueno.Text = "";
        txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        if (chkedit.Checked == true)
        {
            FillIssueNo();
        }
        //***********************
        DG.DataSource = null;
        DG.DataBind();
        DGRawdetail.DataSource = null;
        DGRawdetail.DataBind();
        DGreceiveDetail.DataSource = null;
        DGreceiveDetail.DataBind();
        //************************
    }
    protected void FillMaingrid()
    {
        string str = @"select dbo.F_getItemDescription(WD.item_finished_id,WD.flagsize) as ItemDescription,WD.Pcs,WD.area,WD.ID,WD.DetailId,Noofbeamreq,IssueNo,REPLACE(CONVERT(nvarchar(11),IssueDate,106),' ','-') as IssueDate,REPLACE(CONVERT(nvarchar(11),TargetDate,106),' ','-') as TargetDate
                      from Warpordermaster WM inner join Warporderdetail WD on WM.id=WD.Id Where WM.ID=" + hnid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        //***********
        if (chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["Issueno"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                txttargetdate.Text = ds.Tables[0].Rows[0]["Targetdate"].ToString();
            }
            else
            {
                txtissueno.Text = "";
                txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        //***********
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void FillRawdetail()
    {
        string str = "";
        switch (Session["varcompanyNo"].ToString())
        {
            case "14":
                str = @"select  dbo.F_getItemDescription(WI.IFinishedid,WI.ISizeflag) as ItemDescription,U.UnitName,
                    Sum(Round(case When WI.Sizeflag=1 Then WD.Area*(WI.IQty*1.196) Else Wd.Area*Wi.IQty End,3)) as IQTY
                    from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
                    inner join Unit u on WI.Iunitid=U.UnitId
                    inner join WarpOrderMaster WM on WD.Id=WM.ID where WM.id=" + hnid.Value + @"
                    group by WI.IFinishedid,WI.ISizeflag,U.UnitName";
                break;
            default:
                if (variable.Varkatiwithbomcaltype == "1")
                {
                    str = @"select  dbo.F_getItemDescription(WI.IFinishedid,WI.ISizeflag) as ItemDescription,U.UnitName,
                            Sum( Round(case When OCd.ICALTYPE=1 Then (WD.pcs*WD.NoofBeamreq)*OCD.IQTY Else case When WI.Sizeflag=1 Then WD.Area*(WI.IQty*1.196) Else Wd.Area*Wi.IQty End End,3)  ) as IQTY
                            from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
                            inner join Unit u on WI.Iunitid=U.UnitId
                            inner join WarpOrderMaster WM on WD.Id=WM.ID 
                            inner join Order_consumption_detail ocd on Wd.orderdetailid=OCd.ORDERDETAILID and Ocd.processid=Wm.Processid
                            and WI.IFinishedid=ocd.IFINISHEDID and WI.Ofinishedid=ocd.OFINISHEDID
                            where  WM.id=" + hnid.Value + " group by WI.IFinishedid,WI.ISizeflag,U.UnitName";
                }
                else
                {
                    str = @"select  dbo.F_getItemDescription(WI.IFinishedid,WI.ISizeflag) as ItemDescription,U.UnitName,
                    Sum(Round(case When WI.Sizeflag=1 Then WD.Area*(WI.IQty*1.196) Else Wd.Area*Wi.IQty End,3)) as IQTY
                    from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
                    inner join Unit u on WI.Iunitid=U.UnitId
                    inner join WarpOrderMaster WM on WD.Id=WM.ID where WM.id=" + hnid.Value + @"
                    group by WI.IFinishedid,WI.ISizeflag,U.UnitName";
                }

                break;
        }

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        DGRawdetail.DataSource = ds.Tables[0];
        DGRawdetail.DataBind();


    }
    protected void FillReceivedetail()
    {
        string str = @"select Distinct  dbo.F_getItemDescription(WI.OFinishedid,WI.OSizeflag) as ItemDescription,U.UnitName                    
                    from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
                    inner join Unit u on WI.Ounitid=U.UnitId
                    inner join WarpOrderMaster WM on WD.Id=WM.ID where WM.id=" + hnid.Value;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGreceiveDetail.DataSource = ds.Tables[0];
        DGreceiveDetail.DataBind();


    }

    protected void Refreshcontrol()
    {

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", hnid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_reportWarpingOrder", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rtpwarporder.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rtpwarporder.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
        //
    }
    protected void DG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblid = (Label)DG.Rows[e.RowIndex].FindControl("lblid");
            Label lblDetailid = (Label)DG.Rows[e.RowIndex].FindControl("lbldetailid");
            //******
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Id", lblid.Text);
            param[1] = new SqlParameter("@DetailId", lblDetailid.Text);
            param[2] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //****
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_deleteWarpOrder", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillMaingrid();
            FillRawdetail();
            //FillReceivedetail();
            FillBeamDescription();
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
    protected void FillIssueNo()
    {
        string status = "Pending";
        if (chkcomplete.Checked == true)
        {
            status = "Complete";
        }
        string str = @"select Id,IssueNo from WarpOrderMaster WHere CompanyId=" + DDcompany.SelectedValue + " and Empid=" + DDEmp.SelectedValue + " and DeptId=" + DDDept.SelectedValue + " and Status='" + status + "' and Processid=" + DDProcess.SelectedValue + " order by ID";
        UtilityModule.ConditionalComboFill(ref DDissueNo, str, true, "--Plz Select--");
    }
    protected void chkedit_CheckedChanged(object sender, EventArgs e)
    {
        DDProcess.SelectedIndex = -1;
        DDProcess_SelectedIndexChanged(DDProcess, e);
        if (chkedit.Checked == true)
        {
            DDissueNo.SelectedIndex = -1;
            TDissueNo.Visible = true;
            btnupdateconsmp.Visible = true;
        }
        else
        {
            txtissueno.Text = "";
            hnid.Value = "0";
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DG.DataSource = null;
            DG.DataBind();
            DGRawdetail.DataSource = null;
            DGRawdetail.DataBind();
            DGreceiveDetail.DataSource = null;
            DGreceiveDetail.DataBind();
            TDissueNo.Visible = false;
            btnupdateconsmp.Visible = false;
        }
    }
    protected void DDissueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = DDissueNo.SelectedValue;
        //*********************
        FillMaingrid();
        FillRawdetail();
        // FillReceivedetail();
        //*********************
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        if (chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDEmp, "select Distinct EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and EI.Blacklist=0 and D.DepartmentId=" + DDDept.SelectedValue + " inner join warpordermaster WO on Wo.empid=ei.empid and wo.processid=" + DDProcess.SelectedValue + "  order by EmpName", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmp, "select EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " and EI.Blacklist=0 order by EI.EmpName", true, "--Plz Select--");
        }
        if (DDEmp.Items.Count > 0)
        {
            DDEmp_SelectedIndexChanged(sender, e);
        }
    }
    protected void btnupdateconsmp_Click(object sender, EventArgs e)
    {
        if (DDissueNo.SelectedIndex > 0)
        {
            SqlParameter[] arr = new SqlParameter[2];
            arr[0] = new SqlParameter("@id", DDissueNo.SelectedValue);
            arr[1] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            arr[1].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_updateWarpissueconsumption", arr);
            lblmessage.Text = arr[1].Value.ToString();
            DDissueNo_SelectedIndexChanged(DDissueNo, e);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "consmp", "alert('Please select Issue No!!!');", true);
        }
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        FillMaingrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        FillMaingrid();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lblid = (Label)DG.Rows[e.RowIndex].FindControl("lblid");
            Label lbldetailid = (Label)DG.Rows[e.RowIndex].FindControl("lbldetailid");
            TextBox txtnoofbeam = (TextBox)DG.Rows[e.RowIndex].FindControl("txtnoofbeam");
            //**********
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@id", lblid.Text);
            param[1] = new SqlParameter("@detailid", lbldetailid.Text);
            param[2] = new SqlParameter("@userid", Session["varuserid"]);
            param[3] = new SqlParameter("@NoofbeamReq", txtnoofbeam.Text == "" ? "0" : txtnoofbeam.Text);
            param[4] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "pro_Updatewarpissue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            DG.EditIndex = -1;
            FillMaingrid();
            FillRawdetail();
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
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDorderNo, @"select OM.orderid,OM.CustomerOrderNo+' '+Om.LocalOrder orderNo 
        From ordermaster OM(Nolock) 
        Where companyid=" + DDcompany.SelectedValue + " and CustomerId=" + DDcustcode.SelectedValue + @" and Om.status=0 
        order by OM.orderid", true, "--Plz Select--");
    }
    protected void FillOrderDescription()
    {
        string str = @"select OD.OrderDetailId,Vf.ITEM_NAME+' '+VF.QualityName+' '+Vf.designName+' '+Vf.ColorName+' '+vf.ShapeName+' '+case when OD.flagsize=1 Then Vf.SizeMtr When OD.flagsize=2 Then
                    vf.sizeinch ELse vf.sizeft End+' '+case when Vf.SizeId>0 Then Sz.Type Else '' ENd as ItemDescription
                    From OrderMaster Om inner join OrderDetail OD on Om.OrderId=Od.OrderId
                    inner Join V_FinishedItemDetail vf on OD.Item_Finished_Id=vf.ITEM_FINISHED_ID
                    Left join SizeType Sz on Od.flagsize=Sz.val Where OM.Companyid=" + DDcompany.SelectedValue + " and Om.customerid=" + DDcustcode.SelectedValue + @" 
                    and OM.orderid=" + DDorderNo.SelectedValue + " order by OD.orderdetailid";
        UtilityModule.ConditionalComboFill(ref DDitemdescription, str, true, "--Plz Select--");

    }
    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillOrderDescription();
    }
    protected void DDitemdescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBeamDescription();
    }
    protected void FillBeamDescription()
    {
        //lblmessage.Text = "";

        try
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@orderdetailid", DDitemdescription.SelectedValue);
            param[1] = new SqlParameter("@Processid", DDProcess.SelectedValue);
            param[2] = new SqlParameter("@Totalpcs", SqlDbType.Int);
            param[3] = new SqlParameter("@Totalarea", SqlDbType.Float);
            param[2].Direction = ParameterDirection.Output;
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@Effectivedate", txtissuedate.Text);
            param[5] = new SqlParameter("@EmpId", DDEmp.SelectedValue);
            param[6] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            //****************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETARTICLEBEAMDESCRIPTION", param);
            GvBeamDesc.DataSource = ds.Tables[0];
            GvBeamDesc.DataBind();
            txttotalpcs.Text = param[2].Value.ToString();
            txttotalarea.Text = param[3].Value.ToString();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    decimal totalrawissue = 0;
    protected void DGRawdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblissueqty = (Label)e.Row.FindControl("lblissueqty");
            totalrawissue += Convert.ToDecimal(lblissueqty.Text);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblfooterissueqty = (Label)e.Row.FindControl("lblfooterissueqty");
            lblfooterissueqty.Text = totalrawissue.ToString();
        }
    }
}