using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Hissab_FrmWeaverRawMaterialHissabChampa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select CompanyId,CompanyName From Companyinfo CI where MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + @" order by CompanyId
                           select EI.EmpId,EI.EmpName + case When isnull(Ei.empcode,'')='' then '' else ' ['+EI.empcode+']' end as Empname  From EmpInfo  EI inner Join Department D on EI.Departmentid=D.DepartmentId and D.DepartmentName='PRODUCTION' and Ei.MastercompanyId=" + Session["varMasterCompanyIDForERP"] + @" INNER JOIN EmpProcess EP ON EP.Empid=EI.EmpId and EP.ProcessId=1 order by EmpName
                          select CATEGORY_ID,CATEGORY_NAME From ITEM_CATEGORY_MASTER ICM ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDWeaverName, ds, 1, true, "---Plz Select---");
            UtilityModule.ConditionalComboFillWithDS(ref DDCategory, ds, 2, true, "---Plz Select---");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedIndex = 1;
            }
            if (DDCategory.Items.Count > 0)
            {
                DDCategory.SelectedIndex = 1;
                BindCategory();
            }
            txtfromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillCustomercode();
        //DG.DataSource = null;
        //DG.DataBind();
    }
    protected void DDWeaverName_SelectedIndexChanged(object sender, EventArgs e)
    {
        // FillFolioNo();
    }
    protected void BindCategory()
    {
        UtilityModule.ConditionalComboFill(ref DDItemName, "select IM.ITEM_ID,IM.ITEM_NAME From Item_Master IM Where IM.category_id=" + DDCategory.SelectedValue + " order by ITEM_NAME", true, "--Plz Select--");

    }
    protected void DDCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategory();
    }
    protected void BindChallanNo()
    {
        string str = @"Select WRHissabId,Challanno from WeaverRawHiisab Where CompanyId=" + DDCompany.SelectedValue + " and EmpId="+DDWeaverName.SelectedValue+@" 
                        and QualityTypeId="+DDItemName.SelectedValue+" ";
        UtilityModule.ConditionalComboFill(ref DDChallanNo, str, true, "--Plz Select--"); 
       
    }
    protected void DDItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkEdit.Checked == true)
        {
            BindChallanNo();
        }
        Fillgrid();
        CalTotalWool();
        // FillFolioNo();
    }   
    protected void Fillgrid()
    {
        if (chkEdit.Checked == false)
        {
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@EmpId", DDWeaverName.SelectedValue);
            param[2] = new SqlParameter("@MasterQualityId", DDItemName.SelectedValue);
            param[3] = new SqlParameter("@TranDate", txtfromDate.Text);
            param[4] = new SqlParameter("@MasterQualityName", DDItemName.SelectedItem.Text);
            param[5] = new SqlParameter("@MasterCompanyId", Session["varMasterCompanyIDForERP"]);
            param[6] = new SqlParameter("@UserId", Session["VarUserid"]);
            //************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERRAWHISSABDETAIL_CHAMPA", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DG.DataSource = ds.Tables[0];
                DG.DataBind();
            }
            else
            {
                DG.DataSource = null;
                DG.DataBind();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                txtCommAmt.Text = ds.Tables[1].Rows[0]["CommAmt"].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                txtCommAmt.Text =Convert.ToString(Convert.ToDecimal(txtCommAmt.Text)- Convert.ToDecimal(ds.Tables[1].Rows[0]["CommAmt"]));
            }
        }
        else if (chkEdit.Checked == true)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@CompanyId", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@EmpId", DDWeaverName.SelectedValue);
            param[2] = new SqlParameter("@MasterQualityId", DDItemName.SelectedValue);
            param[3] = new SqlParameter("@ChallanNo", DDChallanNo.SelectedValue);
           
            //************
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GETWEAVERRAWHISSABDETAILBYCHALLANNO_CHAMPA", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTotalAmount.Text = ds.Tables[0].Rows[0]["TotalAmt"].ToString();
                txtCommAmt.Text = ds.Tables[0].Rows[0]["TotalComm"].ToString();
                txtMapStencilExtra.Text = ds.Tables[0].Rows[0]["MSExtra"].ToString();
                txtAnyOther.Text = ds.Tables[0].Rows[0]["OtherAny"].ToString();
                txtNetAmount.Text = ds.Tables[0].Rows[0]["NetAmt"].ToString();
                txtfromDate.Text= ds.Tables[0].Rows[0]["TranDate"].ToString(); 
            }
            
            if (ds.Tables[1].Rows.Count > 0)
            {
                DG.DataSource = ds.Tables[1];
                DG.DataBind();
            }
            else
            {
                DG.DataSource = null;
                DG.DataBind();
            }
        }

    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBalanceWeaverStock = (Label)e.Row.FindControl("lblBalanceWeaverStock");

            TextBox txtAdjustQty = (TextBox)e.Row.FindControl("txtAdjustQty");
            TextBox txtBalanceQty = (TextBox)e.Row.FindControl("txtBalanceQty");

            txtBalanceQty.Text = Convert.ToString(Convert.ToDecimal(lblBalanceWeaverStock.Text) - Convert.ToDecimal(txtAdjustQty.Text));                    
        }
    }
    protected void test(object sender)
    {
        string HKStrWL = "";
        double BZW = 0, BZL = 0;
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;

        txt.Focus();

        Label lblBalanceWeaverStock = (Label)gvRow.FindControl("lblBalanceWeaverStock");
        TextBox txtRate2 = (TextBox)gvRow.FindControl("txtRate2");
        TextBox txtRate = (TextBox)gvRow.FindControl("txtRate");

        TextBox txtBalanceQty = (TextBox)gvRow.FindControl("txtBalanceQty");
        TextBox txtAdjustQty = (TextBox)gvRow.FindControl("txtAdjustQty");
        TextBox txtAmount = (TextBox)gvRow.FindControl("txtAmount");

        txtAdjustQty.Text = Convert.ToString(Convert.ToDecimal(lblBalanceWeaverStock.Text) - Convert.ToDecimal(txtBalanceQty.Text));

        if (Convert.ToDecimal(txtAdjustQty.Text) <= 0)
        {
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtAdjustQty.Text) * Convert.ToDecimal(txtRate2.Text));
        }
        else
        {
            txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtAdjustQty.Text) * Convert.ToDecimal(txtRate.Text));
        }
    }
    decimal TotalAmt = 0;
    protected void CalTotalAmount()
    {
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            TextBox txtAmount = ((TextBox)DG.Rows[i].FindControl("txtAmount"));
            TotalAmt = TotalAmt + Convert.ToDecimal(txtAmount.Text == "" ? "0" : txtAmount.Text);
        }
        txtTotalAmount.Text = string.Format("{0:#0.00}", TotalAmt).ToString();
        txtNetAmount.Text = string.Format("{0:#0.00}", Convert.ToDecimal(txtCommAmt.Text == "" ? "0" : txtCommAmt.Text) + Convert.ToDecimal(txtMapStencilExtra.Text == "" ? "0" : txtMapStencilExtra.Text) + Convert.ToDecimal(txtAnyOther.Text == "" ? "0" : txtAnyOther.Text) + Convert.ToDecimal(txtTotalAmount.Text == "" ? "0" : txtTotalAmount.Text));

    }
    decimal TotalWool = 0;
    decimal TotalCotton= 0;
    decimal TotalTar = 0;
    decimal TotalTharri = 0;
    decimal TotalMisc = 0;
    protected void CalTotalWool()
    {
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            Label lblItemName = ((Label)DG.Rows[i].FindControl("lblItemName"));           
            if (lblItemName.Text == "WOOL")
            {
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TotalWool = TotalWool + Convert.ToDecimal(lblBalanceWeaverStock.Text == "" ? "0" : lblBalanceWeaverStock.Text);
            }
            if (lblItemName.Text == "COTTON")
            {
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TotalCotton = TotalCotton + Convert.ToDecimal(lblBalanceWeaverStock.Text == "" ? "0" : lblBalanceWeaverStock.Text);
            }
            if (lblItemName.Text == "TAR")
            {
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TotalTar = TotalTar + Convert.ToDecimal(lblBalanceWeaverStock.Text == "" ? "0" : lblBalanceWeaverStock.Text);
            }
            if (lblItemName.Text == "THARRI")
            {
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TotalTharri = TotalTharri + Convert.ToDecimal(lblBalanceWeaverStock.Text == "" ? "0" : lblBalanceWeaverStock.Text);
            }
            if (lblItemName.Text == "MISC")
            {
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TotalMisc = TotalMisc + Convert.ToDecimal(lblBalanceWeaverStock.Text == "" ? "0" : lblBalanceWeaverStock.Text);
            }

        }

        lblTotalWoolYarn.Text = string.Format("{0:#0.000}", TotalWool).ToString();
        lblTotalCotton.Text = string.Format("{0:#0.000}", TotalCotton).ToString();
        lblTotalTar.Text = string.Format("{0:#0.000}", TotalTar).ToString();
        lblTotalTharri.Text = string.Format("{0:#0.000}", TotalTharri).ToString();
        lblTotalMisc.Text = string.Format("{0:#0.000}", TotalMisc).ToString();
        
    }
    protected void txtBalanceQty_TextChanged(object sender, EventArgs e)
    {
        test(sender);
        CalTotalAmount();

        //TextBox txt = (TextBox)sender;
        //GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        //txt.Focus();

        //Label lblBalanceWeaverStock = (Label)gvRow.FindControl("lblBalanceWeaverStock");
        //TextBox txtRate2 = (TextBox)gvRow.FindControl("txtRate2");
        //TextBox txtRate = (TextBox)gvRow.FindControl("txtRate");

        //TextBox txtBalanceQty = (TextBox)gvRow.FindControl("txtBalanceQty");
        //TextBox txtAdjustQty = (TextBox)gvRow.FindControl("txtAdjustQty");
        //TextBox txtAmount = (TextBox)gvRow.FindControl("txtAmount");

        //txtAdjustQty.Text =Convert.ToString(Convert.ToDecimal(lblBalanceWeaverStock.Text) -Convert.ToDecimal(txtBalanceQty.Text));

        //if (Convert.ToDecimal(txtAdjustQty.Text) <= 0)
        //{
        //    txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtAdjustQty.Text) * Convert.ToDecimal(txtRate2.Text));
        //}
        //else
        //{
        //    txtAmount.Text = Convert.ToString(Convert.ToDecimal(txtAdjustQty.Text) * Convert.ToDecimal(txtRate.Text));
        //}

    }
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        test(sender);
        CalTotalAmount();
    }
    protected void txtRate2_TextChanged(object sender, EventArgs e)
    {
        test(sender);
        CalTotalAmount();
    }
    protected void txtCommAmt_TextChanged(object sender, EventArgs e)
    {
        CalTotalAmount();
    }
    protected void txtMapStencilExtra_TextChanged(object sender, EventArgs e)
    {
        CalTotalAmount();
    }
    protected void txtAnyOther_TextChanged(object sender, EventArgs e)
    {
        CalTotalAmount();
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {
        btnsave.Text = "Save";
        TDChallanNo.Visible = false;
        DDWeaverName.SelectedIndex = -1;
        DDItemName.SelectedIndex = -1;

        DG.DataSource = null;
        DG.DataBind();
        txtChallanNo.Text = "";
        lblTotalWoolYarn.Text = "0";
        txtTotalAmount.Text = "0";
        txtCommAmt.Text = "0";
        txtMapStencilExtra.Text = "0";
        txtAnyOther.Text = "0";
        txtNetAmount.Text = "0";

        DDChallanNo.Items.Clear();

        if (chkEdit.Checked == true)
        {           
            TDChallanNo.Visible = true;
            DDChallanNo.SelectedIndex = -1;
            btnsave.Text = "Update";                 
        }
    }
    protected void DDChallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        hnChallanNo.Value = DDChallanNo.SelectedValue;
        txtChallanNo.Text = DDChallanNo.SelectedItem.Text;
        Fillgrid();
        CalTotalWool();
       // FillBeamReceiveDetail();
    }
    private void CHECKVALIDCONTROL()
    {
        lblmessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDCompany) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDWeaverName) == false)
        {
            goto a;
        }
        if (UtilityModule.VALIDDROPDOWNLIST(DDItemName) == false)
        {
            goto a;
        }
        
        //if (UtilityModule.VALIDDROPDOWNLIST(DDCustomerCode) == false)
        //{
        //    goto a;
        //}       
        else
        {
            goto B;
        }
    a:
        lblmessage.Visible = true;
        UtilityModule.SHOWMSG(lblmessage);
    B: ;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        CHECKVALIDCONTROL();
        if (lblmessage.Text == "")
        {
            string Strdetail = "";
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                Label lblItemId = ((Label)DG.Rows[i].FindControl("lblItemId"));
                Label lblQualityId = ((Label)DG.Rows[i].FindControl("lblQualityId"));
                Label lblBalanceWeaverStock = ((Label)DG.Rows[i].FindControl("lblBalanceWeaverStock"));
                TextBox txtAdjustQty = ((TextBox)DG.Rows[i].FindControl("txtAdjustQty"));
                TextBox txtRate = ((TextBox)DG.Rows[i].FindControl("txtRate"));
                TextBox txtRate2 = ((TextBox)DG.Rows[i].FindControl("txtRate2"));
                TextBox txtAmount = ((TextBox)DG.Rows[i].FindControl("txtAmount"));

                Strdetail = Strdetail + lblItemId.Text + '|' + lblQualityId.Text + '|' + lblBalanceWeaverStock.Text + '|' + txtAdjustQty.Text + '|' + txtRate.Text + '|' + txtRate2.Text + '|' + txtAmount.Text + '~';

                //if (txtHDCItemNo.Text.Trim() != "" || txtProductCode.Text.Trim() != "" || txtGrossWt.Text != "0" || txtNetWt.Text != "0" || txtPcs.Text != "0" || txtCBM.Text != "0")
                //{
                //    Strdetail = Strdetail + lblItemFinishedId.Text + '|' + txtHDCItemNo.Text + '|' + txtProductCode.Text + '|' + txtGrossWt.Text + '|' + txtNetWt.Text + '|' + txtPcs.Text + '|' + txtCBM.Text + '~';
                //}
            }

            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@WRHissabId", SqlDbType.Int);              
                if (chkEdit.Checked == true)
                {
                    param[0].Value = DDChallanNo.SelectedValue;
                }
                else
                {
                    param[0].Value = 0;
                }

                param[0].Direction = ParameterDirection.InputOutput;
                param[1] = new SqlParameter("@CompanyID", DDCompany.SelectedValue);
                param[2] = new SqlParameter("@EmpId", DDWeaverName.SelectedValue);
                param[3] = new SqlParameter("@QualityTypeId", DDItemName.SelectedValue);
                param[4] = new SqlParameter("@ChallanNo", DDChallanNo.SelectedIndex<0 ?"0":DDChallanNo.SelectedValue);
                param[4].Direction = ParameterDirection.InputOutput;
                param[5] = new SqlParameter("@TotalAmt", txtTotalAmount.Text == "" ? "0" : txtTotalAmount.Text);
                param[6] = new SqlParameter("@TotalComm", txtCommAmt.Text == "" ? "0" : txtCommAmt.Text);
                param[7] = new SqlParameter("@MSExtra", txtMapStencilExtra.Text == "" ? "0" : txtMapStencilExtra.Text);
                param[8] = new SqlParameter("@OtherAny", txtAnyOther.Text == "" ? "0" : txtAnyOther.Text);
                param[9] = new SqlParameter("@NetAmt", txtNetAmount.Text == "" ? "0" : txtNetAmount.Text);
                param[10] = new SqlParameter("@TranDate", txtfromDate.Text);
                param[11] = new SqlParameter("@StringDetail", Strdetail);
                param[12] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
                param[13] = new SqlParameter("@UserID", Session["varuserid"]);
                param[14] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                param[14].Direction = ParameterDirection.Output;
                param[15] = new SqlParameter("@SaveFlag", chkEdit.Checked==true ?"1": "0");

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveWeaverRawHissab", param);
                //*******************               

                lblmessage.Text = param[14].Value.ToString();
                if (chkEdit.Checked == true)
                {
                    txtChallanNo.Text = DDChallanNo.SelectedItem.Text;
                }
                else
                {
                    txtChallanNo.Text = param[4].Value.ToString();
                }
                hnChallanNo.Value = param[0].Value.ToString();
                Tran.Commit();
                Fillgrid();
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
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        if (DDChallanNo.SelectedIndex > 0 || hnChallanNo.Value!="0")
        {
            Report3();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Challan no not showing!');", true);
        }
    }
    private void Report3()
    {
        DataSet ds = new DataSet();
        // string qry = "";
        // string str = "";
        SqlParameter[] array = new SqlParameter[3];
        array[0] = new SqlParameter("@ChallanNo", SqlDbType.Int);       
        array[1] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
        array[2] = new SqlParameter("@UserID", SqlDbType.Int);       

        //array[0].Value = Convert.ToInt32(DDChallanNo.SelectedValue == "" ? "0" : DDChallanNo.SelectedValue);
        array[0].Value = hnChallanNo.Value;
        array[1].Value = Session["varMasterCompanyIDForERP"];
        array[2].Value = Session["VarUserId"];

        ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_ForWeaverRawMaterialHissabReport", array);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptWeaverRawHissab.rpt";

            Session["GetDataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptWeaverRawHissab.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);

        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record Found!');", true); 
        }
    }

}