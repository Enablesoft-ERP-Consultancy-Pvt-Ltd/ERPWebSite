using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_WARP_frmwarpIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varcompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select CI.CompanyId,CompanyName 
                            From CompanyInfo CI 
                            JOIN Company_Authentication CA on CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + @" And 
                            CA.MasterCompanyid=" + Session["varCompanyId"] + @" order by CompanyName 
                           select D.Departmentid,D.Departmentname from Department D Where D.DepartmentName='WARPING' order by Departmentname
                           select Distinct ICM.CATEGORY_ID,ICM.CATEGORY_NAME from ITEM_CATEGORY_MASTER ICM inner Join CategorySeparate CS on ICM.CATEGORY_ID=CS.Categoryid and cs.id=0 order by ICM.CATEGORY_NAME
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
            UtilityModule.ConditionalComboFillWithDS(ref ddCatagory, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 3, true, "--Plz Select--");

            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
                TDcomplete.Visible = true;
            }
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "select Item_Id,ITEM_NAME from Item_master Where CATEGORY_ID=" + ddCatagory.SelectedValue + " order by ITEM_NAME";
        UtilityModule.ConditionalComboFill(ref dditemname, str, true, "--Plz Select--");
        FillCombo();
    }
    protected void FillCombo()
    {
        TDQuality.Visible = false;
        TDDesign.Visible = false;
        TDColor.Visible = false;
        TDShape.Visible = false;
        TDSize.Visible = false;
        TDShade.Visible = false;

        string str = @"select PARAMETER_ID from ITEM_CATEGORY_PARAMETERS where category_id=" + ddCatagory.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Parameter_Id"].ToString())
                {
                    case "1":
                        TDQuality.Visible = true;
                        break;
                    case "2":
                        TDDesign.Visible = true;
                        break;
                    case "3":
                        TDColor.Visible = true;
                        break;
                    case "4":
                        TDShape.Visible = true;
                        break;
                    case "5":
                        TDSize.Visible = true;
                        UtilityModule.ConditionalComboFill(ref DDsizetype, "select val,Type from SizeType Order by val", false, "");
                        break;
                    case "6":
                        TDShade.Visible = true;
                        break;
                }
            }
        }
    }
    protected void FillQDCS()
    {
        string str = null;
        //Quality
        if (TDQuality.Visible == true)
        {

            str = "select Distinct QualityId,QualityName from Quality Where Item_Id=" + dditemname.SelectedValue + " order by QualityName";
            UtilityModule.ConditionalComboFill(ref dquality, str, true, "--Select--");

        }
        //Design
        if (TDDesign.Visible == true)
        {
            str = "select Distinct D.designId,D.designName from V_FinishedItemDetail vf inner Join Design D on vf.DesignId=D.designid  Where Item_Id=" + dditemname.SelectedValue + " order by D.designname";
            UtilityModule.ConditionalComboFill(ref dddesign, str, true, "--Select--");
        }
        //Color
        if (TDColor.Visible == true)
        {
            str = "select Distinct C.colorid,C.colorname from V_FinishedItemDetail vf inner Join Color C on Vf.colorid=C.colorid  Where Item_Id=" + dditemname.SelectedValue + " order by C.Colorname";
            UtilityModule.ConditionalComboFill(ref ddcolor, str, true, "--Select--");

        }
        //Shape
        if (TDShape.Visible == true)
        {

            str = "select Distinct Sh.shapeid,Sh.shapename from Shape Sh  order by shapename";
            UtilityModule.ConditionalComboFill(ref ddshape, str, true, "--Select--");
            if (ddshape.Items.Count > 0)
            {
                ddshape.SelectedIndex = 1;
                ddshape_SelectedIndexChanged(ddshape, new EventArgs());
            }

        }
        //Shade
        if (TDShade.Visible == true)
        {
            str = "select shadecolorid,shadecolorname from shadecolor   order by shadecolorname";
            UtilityModule.ConditionalComboFill(ref ddlshade, str, true, "--Select--");
        }
        //Unit
    }
    protected void dditemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillQDCS();
    }
    protected void Fillsize()
    {

        string str = null, size = null;
        switch (DDsizetype.SelectedValue.ToString())
        {
            case "0":
                size = "SizeFt";
                break;
            case "1":
                size = "Sizemtr";
                break;
            case "2":
                size = "Sizeinch";
                break;
            default:
                size = "SizeFt";
                break;

        }

        str = "select Distinct Vf.sizeid,Vf." + size + " from V_FinishedItemDetail vf inner join  Size S on vf.Sizeid=S.sizeid Where ITEM_ID=" + dditemname.SelectedValue + " and vf.shapeid=" + ddshape.SelectedValue + " order by Vf." + size;

        UtilityModule.ConditionalComboFill(ref ddsize, str, true, "--Select--");

    }
    protected void ddshape_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void DDsizetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillsize();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            int varfinishedid = UtilityModule.getItemFinishedId(dditemname, dquality, dddesign, ddcolor, ddshape, ddsize, TxtProdCode, Tran, ddlshade, "", Convert.ToInt32(Session["varCompanyId"]));
            SqlParameter[] param = new SqlParameter[15];
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
            param[8] = new SqlParameter("@Item_finished_id", varfinishedid);
            param[9] = new SqlParameter("@flagsize", DDsizetype.SelectedValue);
            param[10] = new SqlParameter("@pcs", txtpcs.Text);
            param[11] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;
            Double Area = 0;
            Area = Convert.ToDouble(txtarea.Text) * Convert.ToDouble(txtpcs.Text) * Convert.ToDouble(txtbeamreq.Text);
            param[12] = new SqlParameter("@Area", Area);
            param[13] = new SqlParameter("@Noofbeamreq", txtbeamreq.Text);
            param[14] = new SqlParameter("@Processid", DDProcess.SelectedValue);
            //**********************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWarpOrder", param);
            hnid.Value = param[0].Value.ToString();
            txtissueno.Text = param[4].Value.ToString();
            Tran.Commit();
            lblmessage.Text = param[11].Value.ToString();
            FillMaingrid();
            FillRawdetail();
            FillReceivedetail();
            Refreshcontrol();
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
        string str = @"select  dbo.F_getItemDescription(WI.IFinishedid,WI.ISizeflag) as ItemDescription,U.UnitName,
                    Sum(Round(case When WI.Sizeflag=1 Then WD.Area*(WI.IQty*1.196) Else Wd.Area*Wi.IQty End,3)) as IQTY
                    from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
                    inner join Unit u on WI.Iunitid=U.UnitId
                    inner join WarpOrderMaster WM on WD.Id=WM.ID where WM.id=" + hnid.Value + @"
                    group by WI.IFinishedid,WI.ISizeflag,U.UnitName";
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
    protected void ddsize_SelectedIndexChanged(object sender, EventArgs e)
    {
        string area, str;
        switch (DDsizetype.SelectedValue)
        {
            case "0":
                area = "ProdAreaFt";
                break;
            case "1":
                area = "ProdAreaMtr";
                break;
            case "2":
                area = "ProdAreaFt";
                break;
            default:
                area = "ProdAreaFt";
                break;

        }
        str = "select Sizeid," + area + " as Area from Size where Sizeid=" + ddsize.SelectedValue;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtarea.Text = ds.Tables[0].Rows[0]["area"].ToString();
        }
        else
        {
            txtarea.Text = "0";
        }
    }
    protected void Refreshcontrol()
    {
        ddsize.SelectedIndex = -1;
        txtarea.Text = "";
        txtpcs.Text = "";
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
            FillReceivedetail();
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
        FillReceivedetail();
        //*********************
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnid.Value = "0";
        if (chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDEmp, "select Distinct EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " inner join warpordermaster WO on Wo.empid=ei.empid and wo.processid=" + DDProcess.SelectedValue + " order by EmpName", true, "--Plz Select--");
        }
        else
        {
            UtilityModule.ConditionalComboFill(ref DDEmp, "select EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " order by EI.EmpName", true, "--Plz Select--");
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
}