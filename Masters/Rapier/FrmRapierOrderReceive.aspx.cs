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

public partial class Masters_Rapier_FrmRapierOrderReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                        From Companyinfo CI(Nolock) 
                        JOIN Company_Authentication CA (Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                        Where CI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By CI.CompanyName 
                        Select Distinct PNM.PROCESS_NAME_ID, PNM.PROCESS_NAME 
                        From PROCESS_NAME_MASTER PNM(Nolock)
                        JOIN RapierOrderMaster ROM(Nolock) ON ROM.ProcessID = PNM.PROCESS_NAME_ID 
                        Where PNM.MasterCompanyid = " + Session["varcompanyid"] + " Order By PNM.PROCESS_NAME";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");
            
            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            UtilityModule.ConditionalComboFillWithDS(ref DDProcess, ds, 1, true, "--Plz Select--");
            TxtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ProcessSelectedChanged();
    }
    protected void ProcessSelectedChanged()
    {
        string str = @"Select Distinct EI.EmpID, EI.EmpName 
                    From Empinfo EI(Nolock)
                    JOIN RapierOrderMaster ROM(Nolock) ON ROM.EmpID = EI.EmpID 
                    Where EI.MasterCompanyid = " + Session["varcompanyid"] + " Order By EI.EmpName";
        UtilityModule.ConditionalComboFill(ref DDVendorName, str, true, "--Plz Select--");
    }
    protected void DDVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        VendorNameSelectedChanged();
    }
    protected void VendorNameSelectedChanged()
    {
        string str = @"Select ROM.ID, ROM.ChallanNo 
                    From RapierOrderMaster ROM(Nolock) 
                    Where ROM.MasterCompanyid = " + Session["varcompanyid"] + " And ROM.CompanyID = " + DDCompany.SelectedValue + @" And 
                        ROM.ProcessID = " + DDProcess.SelectedValue + " And ROM.EmpID = " + DDVendorName.SelectedValue;

        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Plz Select--");
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChallanNoSelectedChanged();
    }
    protected void ChallanNoSelectedChanged()
    {
        DDReceiveNo.Items.Clear();
        if (DDProcess.SelectedIndex > 0 && DDVendorName.SelectedIndex > 0 && DDChallanNo.SelectedIndex > 0)
        {
            string str = @"Select Distinct a.ID, Cast(a.ChallanNo as Nvarchar) + '  /  ' + REPLACE(CONVERT(NVARCHAR(11), a.ReceiveDate, 106), ' ', '-') IssueDate 
                    From RapierOrderReceiveMaster a(Nolock) 
					JOIN RapierOrderReceiveDetail b(Nolock) ON b.ID = a.ID 
                    Where a.MasterCompanyID = " + Session["varcompanyid"] + " And a.CompanyID = " + DDCompany.SelectedValue + @" And 
                        a.ProcessID = " + DDProcess.SelectedValue + " And a.EmpID = " + DDVendorName.SelectedValue + " And b.RapierOrderID = " + DDChallanNo.SelectedValue;

            UtilityModule.ConditionalComboFill(ref DDReceiveNo, str, true, "--Plz Select--");
        }
        Fillgrid();
    }
    protected void DDReceiveNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnissueid.Value = DDReceiveNo.SelectedValue;
        FillissueGrid();
    }
    protected void Fillgrid()
    {
        SqlParameter[] array = new SqlParameter[7];
        array[0] = new SqlParameter("@ID", DDChallanNo.SelectedValue);
        array[1] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
        array[2] = new SqlParameter("@ProcessID", DDProcess.Text);
        array[3] = new SqlParameter("@EmpID", DDVendorName.Text);
        array[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
        array[4].Direction = ParameterDirection.Output;
        array[5] = new SqlParameter("@UserID", Session["varuserid"]);
        array[6] = new SqlParameter("@Mastercompanyid", Session["varcompanyNo"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRapierOrderDetail", array);

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
            string str = @"select GoDownID,GodownName from GodownMaster(Nolock) order by GodownName
                           select godownid From Modulewisegodown(Nolock) Where ModuleName='" + Page.Title + "'";

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
            ds.Dispose();
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtRecQty = ((TextBox)DG.Rows[i].FindControl("txtRecQty"));

            if (txtRecQty.Text == "" && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty can not be blank');", true);
                txtRecQty.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && (Convert.ToDecimal(txtRecQty.Text) <= 0))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Receive qty always greater then zero');", true);
                txtRecQty.Focus();
                return;
            }
        }
        string Strdetail = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtRecQty = ((TextBox)DG.Rows[i].FindControl("txtRecQty"));
            DropDownList DDGodown = ((DropDownList)DG.Rows[i].FindControl("DDGodown"));
            DropDownList DDBinNo = ((DropDownList)DG.Rows[i].FindControl("DDBinNo"));
            TextBox TxtLotNo = ((TextBox)DG.Rows[i].FindControl("txtLotNo"));
            TextBox TxtTagNo = ((TextBox)DG.Rows[i].FindControl("txtTagNo"));

            if (Chkboxitem.Checked == true && (txtRecQty.Text != "") && DDGodown.SelectedIndex > 0)
            {
                Label lblID = ((Label)DG.Rows[i].FindControl("lblID"));
                Label lblDetailID = ((Label)DG.Rows[i].FindControl("lblDetailID"));
                Label lblItem_Finished_ID = ((Label)DG.Rows[i].FindControl("lblItem_Finished_ID"));
                Label lblUnitID = ((Label)DG.Rows[i].FindControl("lblUnitID"));
                string BinNo = variable.VarBINNOWISE == "1" ? DDBinNo.SelectedItem.Text : "";

                string LotNo = TxtLotNo.Text == "" ? "Without Lot No" : TxtLotNo.Text;
                string TagNo = TxtTagNo.Text == "" ? "Without Tag No" : TxtTagNo.Text;
                
                Strdetail = Strdetail + lblID.Text + '|' + lblDetailID.Text + '|' + lblUnitID.Text + '|' + DDGodown.SelectedValue + '|' + BinNo + '|' + LotNo + '|' + TagNo + '|' + lblItem_Finished_ID.Text + '|' + txtRecQty.Text + '~';
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
                param[0].Value = DDReceiveNo.SelectedValue;
            }
            else
            {
                param[0].Value = 0;
            }

            param[0].Direction = ParameterDirection.InputOutput;
            param[1] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
            param[2] = new SqlParameter("@ProcessID", DDProcess.SelectedValue);
            param[3] = new SqlParameter("@EmpID", DDVendorName.SelectedValue);
            param[4] = new SqlParameter("@RapierOrderMasterID", DDChallanNo.SelectedValue);
            param[5] = new SqlParameter("@ReceiveNo", SqlDbType.Int);
            param[5].Direction = ParameterDirection.InputOutput;
            param[6] = new SqlParameter("@ReceiveDate", TxtReceiveDate.Text);
            param[7] = new SqlParameter("@UserID", Session["varuserid"]);
            param[8] = new SqlParameter("@MasterCompanyID", Session["varcompanyid"]);
            param[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[9].Direction = ParameterDirection.Output;
            param[10] = new SqlParameter("@Detail", Strdetail);

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveRapierOrderReceive", param);
            //*******************

            TxtReceiveNo.Text = param[0].Value.ToString();
            hnissueid.Value = param[0].Value.ToString();
            Tran.Commit();
            if (param[9].Value.ToString() != "")
            {
                lblmessage.Text = param[9].Value.ToString();
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
                    GM.GodownName, b.LotNo, b.TagNo, b.Qty RecQty, a.ProcessID, a.ChallanNo ReceiveNo, REPLACE(CONVERT(NVARCHAR(11), ReceiveDate, 106), ' ', '-') ReceiveDate 
                    From RapierOrderReceiveMaster a(Nolock)
                    JOIN RapierOrderReceiveDetail b(Nolock) ON b.ID = a.ID 
                    JOIN GodownMaster GM(Nolock) ON GM.GoDownID = b.GodownID 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = b.Item_Finished_ID 
                    Where a.ID = " + hnissueid.Value;
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
        if (chkEdit.Checked == true)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtReceiveNo.Text = ds.Tables[0].Rows[0]["ReceiveNo"].ToString();
                TxtReceiveDate.Text = ds.Tables[0].Rows[0]["ReceiveDate"].ToString();
            }
            else
            {
                TxtReceiveNo.Text = "";
                TxtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", hnissueid.Value);
        //************
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_GetRapierOrderReceiveReport", param);
        if (ds.Tables[0].Rows.Count > 0)
        {

            Session["rptFileName"] = "~\\Reports\\RptRapierOrderReceive.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptRapierOrderReceive.xsd";
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
        TDReceiveNo.Visible = false;
        DG.DataSource = null;
        DG.DataBind();
        gvdetail.DataSource = null;
        gvdetail.DataBind();
        TxtReceiveNo.Text = "";
        TxtReceiveDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

        if (chkEdit.Checked == true)
        {
            TDReceiveNo.Visible = true;
            DDReceiveNo.Items.Clear();
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
            Label lblID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblID");
            Label lblDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("lblDetailID");

            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ID", lblID.Text);
            param[1] = new SqlParameter("@DetailID", lblDetailID.Text);
            param[2] = new SqlParameter("@RapierOrderMasterID", DDChallanNo.SelectedValue);
            param[3] = new SqlParameter("@ProcessID", DDProcess.SelectedValue);
            param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRapierOrderReceive", param);
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
    protected void DDgodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlgodown = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlgodown.Parent.Parent;
        Label lblItem_Finished_ID = ((Label)row.FindControl("lblItem_Finished_ID"));

        if (variable.VarBINNOWISE == "1")
        {
            DropDownList DDBinNo = ((DropDownList)row.FindControl("DDBinNo"));
            if (variable.VarCHECKBINCONDITION == "1")
            {

                UtilityModule.FillBinNO(DDBinNo, Convert.ToInt32(ddlgodown.SelectedValue), Convert.ToInt32(lblItem_Finished_ID.Text), New_Edit: 0);
            }
            else
            {
                UtilityModule.ConditionalComboFill(ref DDBinNo, "select BinNo, BinNO From BinMaster(Nolock) Where GODOWNID = " + ddlgodown.SelectedValue + " Order By BINID", true, "--Plz Select--");
            }
        }
    }
}