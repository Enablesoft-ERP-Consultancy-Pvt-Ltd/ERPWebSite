using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClosedXML.Excel;
using System.IO;

public partial class Masters_Rapier_FrmRapierOrderRawIssue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock) 
                        JOIN Company_Authentication CA (Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varMasterCompanyIDForERP"] + @" Order By CI.CompanyName 
                        Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                        From PROCESS_NAME_MASTER PNM(Nolock)
                        JOIN RapierOrderMaster ROM(Nolock) ON ROM.ProcessID = PNM.PROCESS_NAME_ID 
                        Where PNM.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + " Order By PNM.PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDcompany, ds, 0, false, "");

            if (DDcompany.Items.Count > 0)
            {
                DDcompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDcompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDprocess, ds, 1, true, "--Plz Select--");
            TxtIssueDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDprocess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChanged();
    }
    protected void ProcessSelectedChanged()
    {
        string str = @"Select Distinct EI.EmpID, EI.EmpName 
                    From Empinfo EI(Nolock)
                    JOIN RapierOrderMaster ROM(Nolock) ON ROM.EmpID = EI.EmpID 
                    Where EI.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + " Order By EI.EmpName";
        UtilityModule.ConditionalComboFill(ref DDVendorName, str, true, "--Plz Select--");
    }
    protected void DDVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        VendorNameSelectedChanged();
    }
    protected void VendorNameSelectedChanged()
    {
        string str = @"Select ROM.ID, ROM.ID 
                    From RapierOrderMaster ROM(Nolock) 
                    Where ROM.MasterCompanyid = " + Session["varMasterCompanyIDForERP"] + " And ROM.CompanyID = " + DDcompany.SelectedValue + @" And 
                        ROM.ProcessID = " + DDprocess.SelectedValue + " And ROM.EmpID = " + DDVendorName.SelectedValue + " Order By ROM.ID Desc";

        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Plz Select--");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChanged();
    }
    protected void ChallanNoSelectedChanged()
    {
        DDIssueNo.Items.Clear();
        if (DDprocess.SelectedIndex > 0 && DDVendorName.SelectedIndex > 0)
        {
            string str = @"Select ID, Cast(IssueNo as Nvarchar) + '  /  ' + REPLACE(CONVERT(NVARCHAR(11), IssueDate, 106), ' ', '-') IssueDate 
                    From RapierRawIssueMaster (Nolock) 
                    Where MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + " And TranType = 0 And CompanyID = " + DDcompany.SelectedValue + @" And 
                        ProcessID = " + DDprocess.SelectedValue + " And EmpID = " + DDVendorName.SelectedValue + @" 
                        And ROM.RapierOrderMasterID = " + DDChallanNo.SelectedValue + " Order By ID Desc";
            UtilityModule.ConditionalComboFill(ref DDIssueNo, str, true, "--Plz Select--");
        }
        Fillgrid();
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDIssueNo.SelectedValue;
        FillissueGrid();
    }
    protected void Fillgrid()
    {
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@ID", DDChallanNo.SelectedValue);
        array[1] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
        array[2] = new SqlParameter("@ProcessID", DDprocess.Text);
        array[3] = new SqlParameter("@EmpID", DDVendorName.Text);
        array[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        array[4].Direction = ParameterDirection.Output;
        array[5] = new SqlParameter("@UserID", Session["varuserid"]);
        array[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyNo"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRapierOrderConsmpIssueDetail", array);

        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DG.Columns.Count; i++)
            {
                if (variable.VarBINNOWISE == "1")
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = true;
                    }
                }
                else
                {
                    if (DG.Columns[i].HeaderText == "BinNo")
                    {
                        DG.Columns[i].Visible = false;
                    }
                }
            }
            DropDownList DDGodown = ((DropDownList)e.Row.FindControl("DDGodown"));
            string str = @"select GoDownID,GodownName from GodownMaster order by GodownName
                           select godownid From Modulewisegodown Where ModuleName='" + Page.Title + "'";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDGodown, ds, 0, true, "--Plz Select--");

            if (hngodownid.Value == "0")
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (DDGodown.Items.FindByValue(ds.Tables[1].Rows[0]["godownid"].ToString()) != null)
                    {
                        DDGodown.SelectedValue = ds.Tables[1].Rows[0]["godownid"].ToString();
                    }
                }
                else
                {
                    if (DDGodown.Items.Count > 0)
                    {
                        DDGodown.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                if (DDGodown.Items.FindByValue(hngodownid.Value) != null)
                {
                    DDGodown.SelectedValue = hngodownid.Value;
                }
            }
            DDgodown_SelectedIndexChanged(DDGodown, new EventArgs());
            ds.Dispose();
        }
    }    
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            string str = "select Distinct S.BinNo,S.BinNo from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref DDBinNo, str, true, "--Plz Select--");
            if (DDBinNo.Items.Count > 0)
            {
                DDBinNo.SelectedIndex = 1;
                DDBinNo_SelectedIndexChanged(DDBinNo, e);
            }
        }
        else
        {
            int index = row.RowIndex;
            DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            UtilityModule.ConditionalComboFill(ref ddLotno, str, true, "--Plz Select--");

            if (ddLotno.Items.Count > 0)
            {
                ddLotno.SelectedIndex = 1;
                DDLotno_SelectedIndexChanged(ddLotno, e);
            }
        }
    }
    protected void DDBinNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList DDBinNo = (DropDownList)sender;
        GridViewRow row = (GridViewRow)DDBinNo.Parent.Parent;
        Label Ifinishedid = ((Label)row.FindControl("lblifinishedid"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDGodown"));

        DropDownList ddLotno = ((DropDownList)row.FindControl("DDLotNo"));
        
            string str = "select Distinct S.Lotno,S.Lotno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Qtyinhand>0";
            if (variable.VarBINNOWISE == "1")
            {
                str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
            }
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

        Label Ifinishedid = (Label)row.FindControl("lblifinishedid");
        DropDownList DDTagNo = ((DropDownList)row.FindControl("DDTagNo"));
        DropDownList ddlgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        string str = "select Distinct S.TagNo,S.Tagno from Stock S Where S.companyid=" + DDcompany.SelectedValue + " and S.godownId=" + ddlgodown.SelectedValue + " and S.item_finished_id=" + Ifinishedid.Text + " and S.Lotno='" + ddlLotno.Text + "' and S.Qtyinhand>0";
        if (variable.VarBINNOWISE == "1")
        {
            str = str + " and BinNo='" + DDBinNo.SelectedItem.Text + "'";
        }
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
        int Ifinishedid = Convert.ToInt32(((Label)row.FindControl("lblifinishedid")).Text);
        Label lblstockqty = ((Label)row.FindControl("lblstockqty"));
        DropDownList ddgodown = ((DropDownList)row.FindControl("DDgodown"));
        DropDownList ddlotno = ((DropDownList)row.FindControl("DDLotNo"));
        DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));

        Double StockQty = UtilityModule.getstockQty(DDcompany.SelectedValue, ddgodown.SelectedValue, ddlotno.Text, Ifinishedid, ddTagno.Text, BinNo: (variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : ""));
        lblstockqty.Text = StockQty.ToString();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));

            if (txtissueqty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty can not be blank');", true);
                txtissueqty.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtissueqty.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Issue qty always greater then zero');", true);
                txtissueqty.Focus();
                return;
            }
        }
        string Strdetail = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtissueqty = ((TextBox)DG.Rows[i].FindControl("txtissueqty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDLotNo = ((DropDownList)DG.Rows[i].FindControl("DDLotNo"));
            DropDownList DDTagNo = ((DropDownList)DG.Rows[i].FindControl("DDTagNo"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));

            if (Chkboxitem.Checked == true && (txtissueqty.Text != "") && DDGodown.SelectedIndex > 0 && DDLotNo.SelectedIndex > 0 && DDTagNo.SelectedIndex > 0)
            {
                Label lblitemfinishedid = ((Label)DG.Rows[i].FindControl("lblifinishedid"));
                Label lblunitid = ((Label)DG.Rows[i].FindControl("lbliunitid"));

                string Lotno = DDLotNo.Text;
                string TagNo = DDTagNo.Text;
                string BinNo = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";

                Label lblconsmpqty = ((Label)DG.Rows[i].FindControl("lblconsmpqty"));

                Strdetail = Strdetail + lblitemfinishedid.Text + '|' + lblunitid.Text + '|' + DDGodown.SelectedValue + '|' + Lotno + '|' + TagNo + '|' + BinNo + '|' + txtissueqty.Text + '|' + lblconsmpqty.Text + '~';
            }
        }

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
            if (chkEdit.Checked == true)
            {
                param[0].Value = DDIssueNo.SelectedValue;
            }
            else
            {
                param[0].Value = 0;
            }

            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@CompanyID", DDcompany.SelectedValue);
            param[2] = new SqlParameter("@ProcessID", DDprocess.SelectedValue);
            param[3] = new SqlParameter("@EmpID", DDVendorName.SelectedValue);
            param[4] = new SqlParameter("@RapierOrderMasterID", DDChallanNo.SelectedValue);
            param[5] = new SqlParameter("@TranType", SqlDbType.TinyInt);
            param[5].Value = 0;
            param[6] = new SqlParameter("@IssueNo", SqlDbType.Int);
            param[6].Direction = ParameterDirection.InputOutput;
            param[7] = new SqlParameter("@IssueDate", TxtIssueDate.Text);
            param[8] = new SqlParameter("@EWayBillNo", TxtEWayBillNo.Text);
            param[9] = new SqlParameter("@UserID", Session["varuserid"]);
            param[10] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[11] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[11].Direction = ParameterDirection.Output;
            param[12] = new SqlParameter("@Detail", Strdetail);

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveRapierRawIssue", param);
            //*******************

            TxtIssueNo.Text = param[0].Value.ToString();
            hnissueid.Value = param[0].Value.ToString();
            Tran.Commit();
            if (param[11].Value.ToString() != "")
            {
                lblmessage.Text = param[11].Value.ToString();
            }
            else
            {
                lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                Fillgrid();
                FillissueGrid();
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
    protected void FillissueGrid()
    {
        string str = @"Select a.ID, b.DetailID, Replace(VF.CATEGORY_NAME + '  ' + VF.ITEM_NAME + '  ' + VF.QualityName + '  ' + VF.DesignName + '  ' + VF.ColorName + '  ' + VF.ShapeName + '  ' + 
	                Case When b.UnitID = 1 Then VF.SizeMtr Else Case When b.UnitID = 6 Then VF.SizeInch Else VF.SizeFt End End  + '  ' + VF.ShadeColorName, '   ', '') [ItemDescription], 
	                LotNo, TagNo, b.Qty IssueQuantity, a.ProcessID, a.IssueNo, REPLACE(CONVERT(NVARCHAR(11), IssueDate, 106), ' ', '-') IssueDate, a.EWayBillNo  
                    From RapierRawIssueMaster a(Nolock)
                    JOIN RapierRawIssueDetail b(Nolock) ON b.ID = a.ID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                    Where a.ID = " + hnissueid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtIssueNo.Text = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                TxtIssueDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                TxtEWayBillNo.Text = ds.Tables[0].Rows[0]["EWayBillNo"].ToString();
            }
            else
            {
                TxtIssueNo.Text = "";
                TxtIssueDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETRAPIERORDERRAWISSUEREPORT", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptRapierOrderRawIssue.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRapierOrderRawIssue.xsd";
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
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        TDIssueNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        TxtIssueNo.Text = "";
        TxtIssueDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        if (chkEdit.Checked == true)
        {
            TDIssueNo.Visible = true;
            DDIssueNo.Items.Clear();
            if (DDChallanNo.Items.Count > 0)
            {
                DDChallanNo.SelectedIndex = -1;
            }
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label lblID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprmid");
            Label lblDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblprtid");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ID", lblID.Text);
            param[1] = new SqlParameter("@DetailID", lblDetailID.Text);
            param[2] = new SqlParameter("@RapierOrderMasterID", DDChallanNo.SelectedValue);
            param[3] = new SqlParameter("@ProcessID", DDprocess.SelectedValue);
            param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRapierRawIssue", param);
            lblmessage.Text = param[4].Value.ToString();
            Tran.Commit();
            FillissueGrid();
            //***************
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
}