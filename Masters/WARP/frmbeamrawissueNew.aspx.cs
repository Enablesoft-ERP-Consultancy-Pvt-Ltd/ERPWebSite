using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_WARP_frmbeamrawissue : System.Web.UI.Page
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
            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 2, true, "--Plz Select--");
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (Session["canedit"].ToString() == "1")
            {
                TDEdit.Visible = true;
                TDcomplete.Visible = true;
            }
            ViewState["reportid"] = "0";
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
        string status = "Pending";
        if (chkcomplete.Checked == true)
        {
            status = "Complete";
        }
        string str = "select  ID,IssueNo+'/'+REPLACE(CONVERT(nvarchar(11),Issuedate,106),' ','-') as IssueNo from WarpOrderMaster Where CompanyId=" + DDcompany.SelectedValue + " and Empid=" + DDemployee.SelectedValue + " and Status='" + status + "' and Processid=" + DDProcess.SelectedValue + " order by Id desc";
        UtilityModule.ConditionalComboFill(ref DDIssueno, str, true, "--Plz Select--");
    }
    protected void FillGrid()
    {
        //        string str = @"select  WI.IFinishedid,WI.ISizeflag,WI.Iunitid,dbo.F_getItemDescription(WI.IFinishedid,WI.ISizeflag) as ItemDescription,U.UnitName,
        //                       Sum(Round(case When WI.Sizeflag=1 Then WD.Area*(WI.IQty*1.196) Else Wd.Area*Wi.IQty End,3)) as IssueQty,dbo.F_GetWarpRawissue(WM.ID,WI.ifinishedid,WI.ISizeflag,WI.Iunitid) As issuedQty,WM.ID
        //                       from WarpOrderDetail WD inner join WarpRawIssue_ReceiveDetail WI on WD.Detailid=WI.IssueDetailid
        //                       inner join Unit u on WI.Iunitid=U.UnitId
        //                       inner join WarpOrderMaster WM on WD.Id=WM.ID where WM.id=" + DDIssueno.SelectedValue + @"
        //                       group by WI.IFinishedid,WI.ISizeflag,WI.Iunitid,U.UnitName,WM.ID";
        try
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@issueid", DDIssueno.SelectedValue);
            param[1] = new SqlParameter("@Item_finished_id", DDArtilceroll.SelectedValue);
            param[2] = new SqlParameter("@Noofpcs", SqlDbType.Int);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Noofbeam", SqlDbType.Int);
            param[3].Direction = ParameterDirection.Output;
            //***********
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWARPINGRAWMATERIALDETAIL", param);
            txtnoofpcs.Text = param[2].Value.ToString();
            txtnoofbeam.Text = param[3].Value.ToString();
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void DDIssueno_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Chkedit.Checked == true)
        {
            FillGatepassNo();
        }
        //FillGrid();
        FillArticleroll();
        if (DDArtilceroll.Items.Count > 0)
        {
            DDArtilceroll.SelectedIndex = 1;
            DDArtilceroll_SelectedIndexChanged(sender, new EventArgs());
        }
    }
    protected void FillArticleroll()
    {
        string str = @"select Distinct WD.Item_Finished_id,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+' '+case When
                        Wd.flagsize=1 Then vf.SizeMtr when Wd.flagsize=2 Then vf.SizeInch Else Vf.SizeFt End as ItemDescription
                        From WarpOrderDetail WD inner join V_FinishedItemDetail vf
                        on WD.Item_Finished_id=vf.ITEM_FINISHED_ID left join SizeType SZ on WD.flagsize=Sz.val
                        Where Id=" + DDIssueno.SelectedValue;
        UtilityModule.ConditionalComboFill(ref DDArtilceroll, str, true, "--Plz Select--");

    }
    decimal qtytobeissued = 0;
    decimal alreadyissued = 0;
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string str = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            Label lblqtytobeissued = (Label)e.Row.FindControl("lblqtytobeissued");
            Label lblalreadyeissued = (Label)e.Row.FindControl("lblalreadyeissued");
            qtytobeissued += Convert.ToDecimal(lblqtytobeissued.Text == "" ? "0" : lblqtytobeissued.Text);
            alreadyissued += Convert.ToDecimal(lblalreadyeissued.Text == "" ? "0" : lblalreadyeissued.Text);

            str = "Select GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varCompanyId"]+" Order by GodownName";
            UtilityModule.ConditionalComboFill(ref DDGodown, str, true, "--Plz Select--");
            if (Session["varcompanyid"].ToString() == "14")
            {
                //Warping Godown

                if (DDGodown.Items.FindByValue("5") != null)
                {
                    DDGodown.SelectedValue = "5";
                    DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
                }
            }
            else
            {
                if (DDGodown.Items.Count > 0)
                {
                    DDGodown.SelectedIndex = 1;
                    DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
                }
            }

            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText== "Bin_No")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "Bin_No")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblfqtytobeissued = (Label)e.Row.FindControl("lblfqtytobeissued");
            Label lblfalreadyeissued = (Label)e.Row.FindControl("lblfalreadyeissued");

            lblfqtytobeissued.Text = qtytobeissued.ToString();
            lblfalreadyeissued.Text = alreadyissued.ToString();
        }
    }
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        // DropDownList ddLotno = ((DropDownList)DG.Rows[index].FindControl("DDLotNo"));
        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotno_SelectedIndexChanged(ddLotno, e);
        }
    }
    protected void DDLotno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        // Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagno_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagno_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        //int Ifinishedid = Convert.ToInt32(((Label)DG.Rows[index].FindControl("lblifinishedid")).Text);
        //Label lblstockqty = ((Label)DG.Rows[index].FindControl("lblstockqty"));
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        //DropDownList ddgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        //DropDownList ddlotno = ((DropDownList)DG.Rows[index].FindControl("DDLotNo"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
        if (variable.VarBINNOWISE == "1")
        {
            string str = "select Distinct S.BinNo,S.BinNo from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid + " and S.Lotno='" + ddlotno.Text + "' and S.TagNo='" + ddTagno.Text + "' and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
                DDBinNo_SelectedIndexChanged(DDBinNo, e);
            }
           
        }
        else
        {
            Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text);
            lblstockqty.Text = StockQty.ToString();
        }       
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        int index = row.RowIndex;
        //int Ifinishedid = Convert.ToInt32(((Label)DG.Rows[index].FindControl("lblifinishedid")).Text);
        //Label lblstockqty = ((Label)DG.Rows[index].FindControl("lblstockqty"));
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        //DropDownList ddgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        //DropDownList ddlotno = ((DropDownList)DG.Rows[index].FindControl("DDLotNo"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, DDTagNo.Text, DDBinNo.Text);
        lblstockqty.Text = StockQty.ToString();
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
        dtrecords.Columns.Add("ifinishedid", typeof(int));
        dtrecords.Columns.Add("IUnitid", typeof(int));
        dtrecords.Columns.Add("Isizeflag", typeof(int));
        dtrecords.Columns.Add("Godownid", typeof(int));
        dtrecords.Columns.Add("Lotno", typeof(string));
        dtrecords.Columns.Add("TagNo", typeof(string));
        dtrecords.Columns.Add("issueqty", typeof(float));
        dtrecords.Columns.Add("Noofcone", typeof(int));
        dtrecords.Columns.Add("Issuemasterid", typeof(int));
        dtrecords.Columns.Add("BinNo", typeof(string));
        //*******************
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            TextBox txtnoofcone = ((TextBox)DG.Rows[i].FindControl("txtnoofcone"));
            
            Boolean Isvalid = true;
            switch (Session["varcompanyNo"].ToString())
            {
                case "21":
                    if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
                    {
                        Isvalid = true;
                    }
                    else
                    {
                        Isvalid = false;
                    }
                    break;
                default:
                    if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0 && Convert.ToInt16(txtnoofcone.Text == "" ? "0" : txtnoofcone.Text) > 0)
                    {
                        Isvalid = true;
                    }
                    else
                    {
                        Isvalid = false;
                    }
                    break;
            }
            if (Isvalid)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));
                Label lblflagsize = ((Label)DG.Rows[i].FindControl("lblisizeflag"));
                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                string BinNo = "";
                if (variable.VarBINNOWISE == "1")
                {
                    DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));
                    BinNo = DDBinNo.Text;
                }

                Label lblissuemasterid = ((Label)DG.Rows[i].FindControl("lblid"));
                //*********************
                DataRow dr = dtrecords.NewRow();
                dr["ifinishedid"] = lblitemfinishedid.Text;
                dr["IUnitid"] = lblunitid.Text;
                dr["Isizeflag"] = lblflagsize.Text;
                dr["Godownid"] = DDGodown.SelectedValue;
                dr["Lotno"] = Lotno;
                dr["TagNo"] = TagNo;
                dr["IssueQty"] = txtissueqty.Text == "" ? "0" : txtissueqty.Text;
                dr["Noofcone"] = txtnoofcone.Text == "" ? "0" : txtnoofcone.Text;
                dr["Issuemasterid"] = lblissuemasterid.Text;
                dr["BinNo"] = BinNo;
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
                param[0].Value = (ViewState["reportid"] == null ? "0" : ViewState["reportid"]);
                param[1] = new SqlParameter("@dtrecords", dtrecords);
                param[2] = new SqlParameter("@companyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@DeptId", DDDept.SelectedValue);
                param[4] = new SqlParameter("@Empid", DDemployee.SelectedValue);
                param[5] = new SqlParameter("@IssueNo", SqlDbType.VarChar, 50);
                param[5].Value = txtogpno.Text;
                param[5].Direction = ParameterDirection.InputOutput;
                param[6] = new SqlParameter("@issuedate", txtissuedate.Text);
                param[7] = new SqlParameter("@Issuemasterid", DDIssueno.SelectedValue);
                param[8] = new SqlParameter("@userid", Session["varuserid"]);
                param[9] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[9].Direction = ParameterDirection.Output;
                param[10] = new SqlParameter("@Processid", DDProcess.SelectedValue);
                param[11] = new SqlParameter("@Item_finished_id", DDArtilceroll.SelectedValue);

                //*******************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWarpRawIssue", param);
                //*******************
                ViewState["reportid"] = param[0].Value.ToString();
                lblmessage.Text = param[9].Value.ToString();
                txtogpno.Text = param[5].Value.ToString();
                Tran.Commit();
                if (param[9].Value.ToString() != "")
                {
                    lblmessage.Text = param[9].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                    FillGrid();
                    FillRawIssuedetail();
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box to save data and issue Qty and No of Cone can not be zero.');", true);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = "select * from  V_getWarpRawissueDetail where Id=" + ViewState["reportid"];
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\rptwarprawissueDetail.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptwarprawissueDetail.xsd";
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
    protected void FillRawIssuedetail()
    {
        string str = @"select dbo.F_getItemDescription(WID.Ifinishedid,WID.Sizeflag) as ItemDescription,
                        U.UnitName,WID.LotNo,WID.TagNo,WID.IssueQty,WID.Noofcone,WID.Id,WID.Detailid,WIM.OGPNo,REPLACE(CONVERT(nvarchar(11),WIM.issuedate,106),' ','-') as IssueDate 
                        from Warprawissuemaster WIM inner join WarpRawIssueDetail WID on WIM.ID=WID.id	  
                        inner join unit u on WID.Unitid=U.UnitId Where WIM.id=" + ViewState["reportid"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (Chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                txtogpno.Text = ds.Tables[0].Rows[0]["OGPNo"].ToString();
                txtissuedate.Text = ds.Tables[0].Rows[0]["Issuedate"].ToString();
            }
            else
            {

                txtogpno.Text = "";
                txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        DGRawIssueDetail.DataSource = ds.Tables[0];
        DGRawIssueDetail.DataBind();
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        DDProcess.SelectedIndex = -1;
        DDProcess_SelectedIndexChanged(DDProcess, e);
        if (Chkedit.Checked == true)
        {
            TDGatepassNo.Visible = true;
            DDIssueno.SelectedIndex = -1;
        }
        else
        {
            ViewState["reportid"] = "0";
            TDGatepassNo.Visible = false;
            DDIssueno.SelectedIndex = -1;
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtogpno.Text = "";
            DG.DataSource = null;
            DG.DataBind();
            DGRawIssueDetail.DataSource = null;
            DGRawIssueDetail.DataBind();
        }
    }
    protected void FillGatepassNo()
    {
        string str = @"select Distinct WIM.ID,WIM.OGPNo from Warprawissuemaster WIM inner join WarpRawIssueDetail WID on WIM.ID=WID.id	
                        Where  WIM.Companyid=" + DDcompany.SelectedValue + " and WIM.DeptId=" + DDDept.SelectedValue + " and WIM.Empid=" + DDemployee.SelectedValue + " and WID.Issuemasterid=" + DDIssueno.SelectedValue + " order by WIM.ID";
        UtilityModule.ConditionalComboFill(ref DDGatepassNo, str, true, "--Plz Select--");
    }
    protected void DDGatepassNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = DDGatepassNo.SelectedValue;
        //***********
        FillRawIssuedetail();
        //***********
    }

    protected void DGRawIssueDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (chkcomplete.Checked == true)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnDelete = ((LinkButton)e.Row.FindControl("lnkDelete"));
                btnDelete.Visible = false;

                LinkButton lb = (LinkButton)e.Row.Cells[8].Controls[0];

                lb.Visible = false;

                //for (int i = 0; i < DG.Columns.Count; i++)
                //{
                //    if (variable.VarBINNOWISE == "1")
                //    {
                //        if (DG.Columns[i].HeaderText == "Bin_No")
                //        {
                //            DG.Columns[i].Visible = true;
                //        }
                //    }
                //    else
                //    {
                //        if (DG.Columns[i].HeaderText == "Bin_No")
                //        {
                //            DG.Columns[i].Visible = false;
                //        }
                //    }
                //}
            }
        }
       
    }
    protected void DGRawIssueDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DGRawIssueDetail.EditIndex = e.NewEditIndex;
        FillRawIssuedetail();
    }
    protected void DGRawIssueDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DGRawIssueDetail.EditIndex = -1;
        FillRawIssuedetail();
    }
    protected void DGRawIssueDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
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
            Label lblid = (Label)DGRawIssueDetail.Rows[e.RowIndex].FindControl("lblid");
            Label lblDetailid = (Label)DGRawIssueDetail.Rows[e.RowIndex].FindControl("lblDetailid");
            TextBox txtissuedqty = (TextBox)DGRawIssueDetail.Rows[e.RowIndex].FindControl("txtissuedqty");
            TextBox txtnoofcone = (TextBox)DGRawIssueDetail.Rows[e.RowIndex].FindControl("txtnoofcone");
            //*************
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@id", lblid.Text);
            param[1] = new SqlParameter("@Detailid", lblDetailid.Text);
            param[2] = new SqlParameter("@issueQty", txtissuedqty.Text == "" ? "0" : txtissuedqty.Text);
            param[3] = new SqlParameter("@Noofcone", txtnoofcone.Text == "" ? "0" : txtnoofcone.Text);
            param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@userid", Session["varuserid"]);
            //********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_UpdateWarpingrawIssue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillGrid();
            DGRawIssueDetail.EditIndex = -1;
            FillRawIssuedetail();
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
    protected void DGRawIssueDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            Label lblid = (Label)DGRawIssueDetail.Rows[e.RowIndex].FindControl("lblid");
            Label lblDetailid = (Label)DGRawIssueDetail.Rows[e.RowIndex].FindControl("lblDetailid");

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@id", lblid.Text);
            param[1] = new SqlParameter("@Detailid", lblDetailid.Text);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteWarpingrawIssue", param);
            lblmessage.Text = param[2].Value.ToString();
            Tran.Commit();
            FillGrid();
            FillRawIssuedetail();
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
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["reportid"] = "0";

        if (Chkedit.Checked == true)
        {
            UtilityModule.ConditionalComboFill(ref DDemployee, "select Distinct EI.EmpId,EI.EmpName+' ['+EI.Empcode+']' as Empname from Empinfo EI inner join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentId=" + DDDept.SelectedValue + " inner join Warprawissuemaster WIM on WIM.empid=Ei.empid and WIM.Processid=" + DDProcess.SelectedValue + " order by EmpName", true, "--Plz Select--");
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
    protected void DDArtilceroll_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void chkcomplete_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcomplete.Checked == true)
        {
            if (Session["varCompanyId"].ToString() == "21")
            {
                btnsave.Visible = true;
            }
            else
            {
                btnsave.Visible = false;
            }

        }
        else
        {
            DDIssueno.Items.Clear();
            DG.DataSource = null;
            DG.DataBind();
            DGRawIssueDetail.DataSource = null;
            DGRawIssueDetail.DataBind();
            btnsave.Visible = true;
        }

    }
}