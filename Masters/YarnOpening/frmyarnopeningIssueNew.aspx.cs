using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_YarnOpening_frmyarnopeningIssue : System.Web.UI.Page
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
                            CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyName";

            if (variable.VarYARNOPENINGISSUEEMPWISE == "1")
            {
                lblyarnopendept.Text = "Employee Name";
                TDdept.Visible = true;

                str = str + @"     select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";
            }
            else
            {
                str = str + @"     select EI.EmpId,EI.EmpName from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where D.DepartmentName='Yarn Opening' and EI.EmpName in('Yarn Opening','YARN OPENING-2','YARN OPENING-3')
                           and isnull(Ei.Blacklist,0)=0 order by EmpName  ";
            }

            str = str + @"  select customerid,CustomerCode+'  '+companyname as customer from customerinfo WHere mastercompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by customer
            select D.Departmentid,D.DepartmentName From Department D Where D.DepartmentName in('Yarn Opening','WEFT DEPARTMENT')";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDvendor, ds, 1, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDcustcode, ds, 2, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDdept, ds, 3, true, "--Select Department--");

            ViewState["id"] = "0";
            txtissuedate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txttargetdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            if (variable.Carpetcompany == "1")
            {
                TRempcodescan.Visible = true;
            }

            if (Session["varMasterCompanyIDForERP"].ToString() == "30")
            {
                TDItemDescription.Visible = false;
            }
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Savenew();
    }
    protected void Savenew()
    {
        lblmessage.Text = "";
        //*********sql Table TYpe
        DataTable dtrecord = new DataTable();
        dtrecord.Columns.Add("Item_finished_id", typeof(int));
        dtrecord.Columns.Add("Unitid", typeof(int));
        dtrecord.Columns.Add("Godownid", typeof(int));
        dtrecord.Columns.Add("LotNo", typeof(string));
        dtrecord.Columns.Add("TagNo", typeof(string));
        dtrecord.Columns.Add("Issueqty", typeof(Decimal));
        dtrecord.Columns.Add("Rectype", typeof(string));
        dtrecord.Columns.Add("Noofcone", typeof(int));
        dtrecord.Columns.Add("conetype", typeof(string));
        dtrecord.Columns.Add("flagsize", typeof(int));
        dtrecord.Columns.Add("orderid", typeof(int));
        dtrecord.Columns.Add("orderdetailid", typeof(int));
        dtrecord.Columns.Add("Rate", typeof(Decimal));
        dtrecord.Columns.Add("ReqdQty", typeof(Decimal));
        dtrecord.Columns.Add("IssueMachineNo", typeof(string));
        dtrecord.Columns.Add("PlyType", typeof(string));
        dtrecord.Columns.Add("TransportType", typeof(string));

        for (int i = 0; i < GVItemDetails.Rows.Count; i++)
        {
            Label lblitemfinishedid = (Label)GVItemDetails.Rows[i].FindControl("lblitemfinishedid");
            DropDownList ddunitgrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddunitgrid");
            DropDownList ddgodowngrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddgodowngrid");
            DropDownList ddlotnogrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddlotnogrid");
            DropDownList ddtagnogrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddtagnogrid");
            TextBox txtissueqtygrid = (TextBox)GVItemDetails.Rows[i].FindControl("txtissueqtygrid");
            DropDownList ddrectypegrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddrectypegrid");
            TextBox txtnoofconegrid = (TextBox)GVItemDetails.Rows[i].FindControl("txtnoofconegrid");
            DropDownList ddconetypegrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddconetypegrid");
            Label lblorderid = (Label)GVItemDetails.Rows[i].FindControl("lblorderid");
            Label lblorderdetailid = (Label)GVItemDetails.Rows[i].FindControl("lblorderdetailid");
            CheckBox Chkboxitem = (CheckBox)GVItemDetails.Rows[i].FindControl("Chkboxitem");
            TextBox txtYarnRate = (TextBox)GVItemDetails.Rows[i].FindControl("txtYarnRate");
            Label lblreqdqty = (Label)GVItemDetails.Rows[i].FindControl("lblreqdqty");
            TextBox txtIssueMachineNo = (TextBox)GVItemDetails.Rows[i].FindControl("txtIssueMachineNo");
            DropDownList DDPlyType = (DropDownList)GVItemDetails.Rows[i].FindControl("DDPlyType");
            DropDownList DDTransportType = (DropDownList)GVItemDetails.Rows[i].FindControl("DDTransportType");

            //************values
            if (Chkboxitem.Checked == true)
            {
                if (Session["varMasterCompanyIDForERP"].ToString() == "21")
                {
                    if (txtIssueMachineNo.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please fill Machine No Mandatory field data.')", true);
                        return;
                    }
                }

                if (ddunitgrid.SelectedIndex != -1 && ddgodowngrid.SelectedIndex > 0 && ddlotnogrid.SelectedIndex > 0 && ddtagnogrid.SelectedIndex > 0 && Convert.ToDecimal(txtissueqtygrid.Text == "" ? "0" : txtissueqtygrid.Text) > 0)
                {
                    DataRow dr = dtrecord.NewRow();
                    dr["Item_finished_id"] = lblitemfinishedid.Text;
                    dr["Unitid"] = ddunitgrid.SelectedValue;
                    dr["Godownid"] = ddgodowngrid.SelectedValue;
                    dr["LotNo"] = ddlotnogrid.SelectedValue;
                    dr["TagNo"] = ddtagnogrid.SelectedValue;
                    dr["Issueqty"] = txtissueqtygrid.Text;
                    dr["Rectype"] = ddrectypegrid.SelectedItem.Text;
                    dr["Noofcone"] = txtnoofconegrid.Text == "" ? "0" : txtnoofconegrid.Text;
                    dr["conetype"] = ddconetypegrid.SelectedItem.Text;
                    dr["flagsize"] = 0;
                    dr["orderid"] = lblorderid.Text;
                    dr["orderdetailid"] = lblorderdetailid.Text;
                    dr["Rate"] = txtYarnRate.Text == "" ? "0" : txtYarnRate.Text;
                    dr["Reqdqty"] = lblreqdqty.Text == "" ? "0" : lblreqdqty.Text;
                    dr["IssueMachineNo"] = txtIssueMachineNo.Text;
                    dr["PlyType"] = DDPlyType.SelectedItem.Text;
                    dr["TransportType"] = DDTransportType.SelectedItem.Text;

                    dtrecord.Rows.Add(dr);
                }

            }
        }
        //************
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
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@ID", SqlDbType.Int);
                param[0].Direction = ParameterDirection.InputOutput;
                param[0].Value = ViewState["id"] == null ? 0 : ViewState["id"];
                param[1] = new SqlParameter("Detailid", SqlDbType.BigInt);
                param[1].Value = 0;
                param[2] = new SqlParameter("@CompanyId", DDcompany.SelectedValue);
                param[3] = new SqlParameter("@vendorid", DDvendor.SelectedValue);
                param[4] = new SqlParameter("IssueNo", SqlDbType.VarChar, 100);
                param[4].Direction = ParameterDirection.InputOutput;
                param[4].Value = txtissueno.Text;
                param[5] = new SqlParameter("@IssueDate", txtissuedate.Text);
                param[6] = new SqlParameter("@TargetDate", txttargetdate.Text);
                param[7] = new SqlParameter("@Userid", Session["varuserid"]);
                param[8] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[8].Direction = ParameterDirection.Output;
                param[9] = new SqlParameter("@dtrecord", dtrecord);
                param[10] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
                param[11] = new SqlParameter("@Departmentid", DDdept.SelectedIndex > 0 ? DDdept.SelectedValue : "0");
                param[12] = new SqlParameter("@DepartmentName", DDdept.SelectedIndex > 0 ? DDdept.SelectedItem.Text : "");
                param[13] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
                //********************
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "[Pro_SaveYarnOpeningIssuenew]", param);
                Tran.Commit();
                ViewState["id"] = param[0].Value.ToString();
                txtissueno.Text = param[4].Value.ToString();
                if (param[8].Value.ToString() != "")
                {
                    lblmessage.Text = param[8].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "Data Saved Sucessfully...";
                    FillGrid();
                    Refreshcontrol();
                }
                //********************
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
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "alt2", "alert('Please fill all checked Mandatory field data.')", true);
        }

    }

    protected void FillGrid()
    {
        string str = @"select dbo.F_getItemDescription(Yt.Item_Finished_id,YT.flagsize) as ItemDescription,GM.GodownName,YT.Lotno
                      ,Yt.Tagno,U.UnitName,
                    YT.issueQty, YT.Rectype, YT.Noofcone, YT.Conetype,YM.ID,YT.Detailid,YM.issueNo,OM.customerorderno,Ym.Departmentid,ISNULL(YM.EWayBillNo,'') as EWayBillNo
                    from YarnOpeningIssueMaster YM inner join YarnOpeningIssueTran YT on YM.ID=YT.MasterId 
                    inner join GodownMaster GM on YT.GodownId=GM.GoDownId
                    inner join Unit U on Yt.Unitid=U.UnitId
                    Left join ordermaster om on YT.orderid=OM.orderid                     
                    Where YM.Id=" + ViewState["id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
        if (Chkedit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueno.Text = ds.Tables[0].Rows[0]["issueNo"].ToString();
                TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            }
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"select  * from [V_yarnOpeningIssue] Where id=" + ViewState["id"];
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\rptyarnopeningissuenew.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\rptyarnopeningissue.xsd";
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
    protected void DDvendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id"] = "0";
        txtissueno.Text = "";
        FillIssueNo();
    }
    protected void DG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DG.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void DG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DG.EditIndex = -1;
        FillGrid();
    }
    protected void DDissuedNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id"] = DDissuedNo.SelectedValue;
        FillGrid();
    }
    protected void Chkedit_CheckedChanged(object sender, EventArgs e)
    {
        GVItemDetails.DataSource = null;
        GVItemDetails.DataBind();
        if (Chkedit.Checked == true)
        {
            TDIssuedNo.Visible = true;
            TDcustcode.Visible = false;
            TDorderNo.Visible = false;
            TDItemDescription.Visible = false;
        }
        else
        {
            TDIssuedNo.Visible = false;
            TDcustcode.Visible = true;
            TDorderNo.Visible = true;
            TDItemDescription.Visible = true;
        }
        FillIssueNo();
    }
    protected void FillIssueNo()
    {
        string str;

        str = "select ID,IssueNo+'/'+REPLACE(CONVERT(nvarchar(11),IssueDate,106),' ','-') as IssueNo from YarnOpeningIssueMaster Where companyid=" + DDcompany.SelectedValue + " and vendorId=" + DDvendor.SelectedValue + "";

        if (chkforComp.Checked == true)
        {
            str = str + " and Status='Complete'";
        }
        else
        {
            str = str + " and Status='Pending'";
        }
        str = str + "  order by id desc";

        UtilityModule.ConditionalComboFill(ref DDissuedNo, str, true, "--Plz Select--");
    }
    protected void chkforComp_CheckedChanged(object sender, EventArgs e)
    {
        FillIssueNo();
    }
    protected void DG_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string masterid = ((Label)DG.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            TextBox txtqty = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtqty"));
            TextBox txtrectype = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtrectype"));
            TextBox txtnoofcone = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtnoofcone"));
            TextBox txtconetype = ((TextBox)DG.Rows[e.RowIndex].FindControl("txtconetype"));
            Label lbldeptid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldeptid"));

            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@IssueQty", (txtqty.Text == "" ? "0" : txtqty.Text));
            param[3] = new SqlParameter("@Rectype", txtrectype.Text);
            param[4] = new SqlParameter("@Noofcone", txtnoofcone.Text);
            param[5] = new SqlParameter("@Conetype", txtconetype.Text);
            param[6] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[6].Direction = ParameterDirection.Output;
            param[7] = new SqlParameter("@Departmentid", lbldeptid.Text);
            param[8] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
            //*************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEYARNOPENINGISSUENEW", param);
            Tran.Commit();
            lblmessage.Text = param[6].Value.ToString();
            DG.EditIndex = -1;
            FillGrid();
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
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
            string masterid = ((Label)DG.Rows[e.RowIndex].FindControl("lblid")).Text;
            string Detailid = ((Label)DG.Rows[e.RowIndex].FindControl("lbldetailid")).Text;
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Id", masterid);
            param[1] = new SqlParameter("@Detailid", Detailid);
            param[2] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[2].Direction = ParameterDirection.Output;
            //************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteYarnOpeningIssue", param);
            Tran.Commit();
            lblmessage.Text = param[2].Value.ToString();
            FillGrid();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
    }
    protected void DDcustcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //        UtilityModule.ConditionalComboFill(ref DDorderNo, @"select orderid,LocalOrder+' '+CustomerOrderNo as orderno from ordermaster where CompanyId=1 and customerid=1 
        //                                            and status=0 order by orderno", true, "--Plz Select--");

        //ViewState["id"] = "0";

        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@companyid", DDcompany.SelectedValue);
            param[1] = new SqlParameter("@Customerid", DDcustcode.SelectedValue);
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "FILL_ORDERNOYARNOPENING", param);
            UtilityModule.ConditionalComboFillWithDS(ref DDorderNo, ds, 0, true, "--Plz Select--");
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
        }
    }
    protected void DDorderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VarCompanyNo"].ToString() == "30")
        {
            BindGVItemDetailsGrid();
        }
        else
        {
            lblmessage.Text = "";
            try
            {
                //SqlParameter[] param = new SqlParameter[1];
                //param[0] = new SqlParameter("@orderid", DDorderNo.SelectedValue);
                ////************
                //DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetYarnOpeningDataOrderWise", param);
                GVItemDetails.DataSource = null;
                GVItemDetails.DataBind();
                UtilityModule.ConditionalComboFill(ref DDitemdescription, @"select OD.OrderDetailId,vf.ITEM_NAME+' '+vf.QualityName+' '+vf.designName+' '+vf.ColorName+' '+vf.ShapeName+case 
                                                                        When OD.flagsize=1 Then Vf.sizemtr When Od.flagsize=2 Then Vf.SizeInch Else vf.SizeFt End +' ('+cast(QtyRequired as varchar)+' Pcs)' as ItemDescription
                                                                        From orderdetail OD inner Join V_finisheditemDetail vf on OD.item_finished_id=Vf.ITEM_FINISHED_ID WHere OD.orderid=" + DDorderNo.SelectedValue + " order by ItemDescription", true, "--Plz Select--");
            }
            catch (Exception ex)
            {
                lblmessage.Text = ex.Message;

            }
        }       

    }
    protected void GVItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblitemid = (Label)e.Row.FindControl("lblitemid");
            DropDownList ddunitgrid = (DropDownList)e.Row.FindControl("ddunitgrid");
            DropDownList ddgodowngrid = (DropDownList)e.Row.FindControl("ddgodowngrid");
            DropDownList ddrecconetype = (DropDownList)e.Row.FindControl("ddrectypegrid");
            DropDownList ddconetypegrid = (DropDownList)e.Row.FindControl("ddconetypegrid");
            TextBox txtYarnRate = (TextBox)e.Row.FindControl("txtYarnRate");

            string str = @"select Distinct U.UnitId,U.UnitName from ITEM_MASTER IM inner Join Unit U on IM.UnitTypeID=U.UnitTypeID Where Im.Item_id=" + lblitemid.Text + @" order by U.UnitName
                     Select distinct GM.GodownId,GM.GodownName From GodownMaster GM JOIN Godown_Authentication GA ON GM.GodownId=GA.GodownId  Where GA.UserId=" + Session["varUserId"] + " and GA.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order by GodownName
                    Select ConeType, ConeType From ConeMaster Order By SrNo ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref ddunitgrid, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref ddgodowngrid, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref ddconetypegrid, ds, 2, false, "");
            
            switch (Session["varcompanyNo"].ToString())
            {
                case "21":
                    if (ddrecconetype.Items.FindByText("Hank") != null)
                    {
                        ddrecconetype.SelectedValue = "Hank";
                        ddconetypegrid.SelectedValue = "";
                    }
                    break;
                default:
                    break;
            }

            if (variable.VarGetYarnOpeningRateFromMaster == "1")
            {
                txtYarnRate.Enabled = false;
            }
            else
            {
                txtYarnRate.Enabled = true;
            }
        }
    }
    protected void DDgodowngrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        int index = row.RowIndex;
        Label Ifinishedid = ((Label)row.FindControl("lblitemfinishedid"));
        DropDownList ddLotno = ((DropDownList)row.FindControl("ddlotnogrid"));
        string str = "select Distinct S.Lotno,S.Lotno as Lotno1 from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + "";
        if (MySession.Stockapply == "True")
        {
            str = str + " and S.Qtyinhand>0";
        }
        str = str + " order by Lotno1";
        UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");
        if (ddLotno.Items.Count > 0)
        {
            ddLotno.SelectedIndex = 1;
            DDLotnogrid_SelectedIndexChanged(ddLotno, e);
        }
        //**********Select auto Godown

        for (int i = 0; i < GVItemDetails.Rows.Count; i++)
        {
            DropDownList ddgodowngrid = (DropDownList)GVItemDetails.Rows[i].FindControl("ddgodowngrid");
            if (ddgodowngrid.SelectedIndex <= 0)
            {
                ddgodowngrid.SelectedValue = ddlgodown.SelectedValue;
                DropDownList ddLotnogrid = ((DropDownList)GVItemDetails.Rows[i].FindControl("ddlotnogrid"));
                Label Ifinishedidgrid = ((Label)GVItemDetails.Rows[i].FindControl("lblitemfinishedid"));
                str = "select Distinct S.Lotno,S.Lotno as Lotno1 from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddgodowngrid.SelectedValue + " and S.item_finished_id=" + Ifinishedidgrid.Text + "";
                if (MySession.Stockapply == "True")
                {
                    str = str + " and S.Qtyinhand>0";
                }
                str = str + " order by Lotno1";
                UtilityModule.ConditionalComboFill(ref ddLotnogrid, str, true, "--Plz Select--");
                if (ddLotnogrid.Items.Count > 0)
                {
                    ddLotnogrid.SelectedIndex = 1;
                    DDLotnogrid_SelectedIndexChanged(ddLotnogrid, e);
                }

            }
        }
        //***************

    }
    protected void DDLotnogrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLotno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlLotno.Parent.Parent;
        int index = row.RowIndex;
        //Label Ifinishedid = ((Label)DG.Rows[index].FindControl("lblifinishedid"));
        Label Ifinishedid = (Label)row.FindControl("lblitemfinishedid");
        //DropDownList DDTagNo = ((DropDownList)DG.Rows[index].FindControl("DDTagNo"));
        //DropDownList ddlgodown = ((DropDownList)DG.Rows[index].FindControl("DDgodown"));
        DropDownList DDTagNo = ((DropDownList)row.FindControl("ddtagnogrid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("ddgodowngrid"));
        string str = "select Distinct S.TagNo,S.Tagno as Tagno1 from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "'";
        if (MySession.Stockapply == "True")
        {
            str = str + "  and S.Qtyinhand>0";
        }
        str = str + " order by Tagno1";
        UtilityModule.ConditionalComboFill(ref DDTagNo, str, true, "--Plz Select--");
        if (DDTagNo.Items.Count > 0)
        {
            DDTagNo.SelectedIndex = 1;
            DDTagnogrid_SelectedIndexChanged(DDTagNo, e);
        }
    }
    protected void DDTagnogrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddTagno = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddTagno.Parent.Parent;
        int index = row.RowIndex;
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblitemfinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("ddgodowngrid"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("ddlotnogrid"));
        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text);
        lblstockqty.Text = StockQty.ToString();
    }
    protected void Refreshcontrol()
    {
        DDitemdescription.SelectedIndex = -1;
        GVItemDetails.DataSource = null;
        GVItemDetails.DataBind();
    }
    protected void BindGVItemDetailsGrid()
    {
        lblmessage.Text = "";
        try
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@orderid", DDorderNo.SelectedValue);

            if (Session["VarCompanyNo"].ToString() == "30")
            {
                param[1] = new SqlParameter("@orderdetailid", 0);
            }
            else
            {
                param[1] = new SqlParameter("@orderdetailid", DDitemdescription.SelectedValue);
            }

            param[2] = new SqlParameter("@EmpId", DDvendor.SelectedValue);
            param[3] = new SqlParameter("@AssignDate", txtissuedate.Text);
            param[4] = new SqlParameter("@Departmentid", DDdept.SelectedValue);
            param[5] = new SqlParameter("@Departmentname", DDdept.SelectedItem.Text);
            //************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetYarnOpeningDataOrderWise", param);
            GVItemDetails.DataSource = ds.Tables[0];
            GVItemDetails.DataBind();

        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;

        }
    }

    protected void DDitemdescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGVItemDetailsGrid();
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
    protected void DDdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id"] = "0";
        txtissueno.Text = "";

        string str = @" select EI.EmpId,EI.EmpName + CASE WHEN EI.EMPCODE<>'' THEN ' ['+EI.EMPCODE+']' ELSE '' END AS EMPNAME from empinfo EI inner join Department D 
                           on EI.departmentId=D.DepartmentId Where isnull(Ei.Blacklist,0)=0 and D.departmentid=" + DDdept.SelectedValue + " order by EmpName  ";
        UtilityModule.ConditionalComboFill(ref DDvendor, str, false, "");
        if (DDvendor.Items.Count > 0)
        {
            DDvendor_SelectedIndexChanged(sender, new EventArgs());
        }
        DDitemdescription_SelectedIndexChanged(sender, new EventArgs());

    }

}