using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Process_FrmBazaarWeightUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            string str = @"select Distinct CI.CompanyId,CI.CompanyName from Companyinfo CI,Company_Authentication CA Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + "  And CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName
                            Select UnitID, UnitName From Unit(Nolock) Where UnitID in (1, 2) ";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
            UtilityModule.ConditionalComboFillWithDS(ref DDCompany, ds, 0, false, "");

            if (DDCompany.Items.Count > 0)
            {
                DDCompany.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompany.Enabled = false;
            }

            txtBazaarDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtBazaarDate_TextChanged(sender, new EventArgs());
        }
    }
    protected void txtBazaarDate_TextChanged(object sender, EventArgs e)
    {
        FillRecDetails();
    }
    protected void FillRecDetails()
    {
        DG.DataSource = null;
        DG.DataBind();

        SqlParameter[] array = new SqlParameter[2];
        array[0] = new SqlParameter("@ReceiveDate", txtBazaarDate.Text);
        array[1] = new SqlParameter("@MasterCompanyId", Session["VarCompanyNo"]);

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_PRODUCTIONRECEIVEDETAIL_FORWEIGHTUPDATE", array);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DG.DataSource = ds.Tables[0];
            DG.DataBind();
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                TextBox txtTotalWeight = ((TextBox)DG.Rows[i].FindControl("txtTotalWeight"));
                TextBox txtChkPcs = ((TextBox)DG.Rows[i].FindControl("txtChkPcs"));
                TextBox txtChkWeight = ((TextBox)DG.Rows[i].FindControl("txtChkWeight"));
                TextBox txtDryWeight = ((TextBox)DG.Rows[i].FindControl("txtDryWeight"));
                Label lblPROCESS_REC_ID = ((Label)DG.Rows[i].FindControl("lblPROCESS_REC_ID"));
                Label lblQualityId = ((Label)DG.Rows[i].FindControl("lblQualityId"));

                if (txtDryWeight.Text != "0")
                {
                    txtTotalWeight.Enabled = false;
                    txtChkPcs.Enabled = false;
                    txtChkWeight.Enabled = false;
                }
                else
                {
                    txtTotalWeight.Enabled = true;
                    txtChkPcs.Enabled = true;
                    txtChkWeight.Enabled = true;
                }
            }
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DG, "Select$" + e.Row.RowIndex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmessage.Text = "";
        string status = "";
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtTotalWeight = ((TextBox)DG.Rows[i].FindControl("txtTotalWeight"));
            TextBox txtChkPcs = ((TextBox)DG.Rows[i].FindControl("txtChkPcs"));
            TextBox txtChkWeight = ((TextBox)DG.Rows[i].FindControl("txtChkWeight"));
            TextBox txtDryWeight = ((TextBox)DG.Rows[i].FindControl("txtDryWeight"));
            Label lblPROCESS_REC_ID = ((Label)DG.Rows[i].FindControl("lblPROCESS_REC_ID"));
            Label lblQualityId = ((Label)DG.Rows[i].FindControl("lblQualityId"));

            int NoofChkPcs = txtChkPcs.Text == "" ? 0 : Convert.ToInt32(txtChkPcs.Text);
            decimal NoofChkPcsWeight = txtChkWeight.Text == "" ? 0 : Convert.ToDecimal(txtChkWeight.Text);
            decimal NoofChkPcsDryWeight = txtDryWeight.Text == "" ? 0 : Convert.ToDecimal(txtDryWeight.Text);

            if (Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                status = "1";

                if (DetailData == "")
                {
                    DetailData = lblPROCESS_REC_ID.Text + "|" + txtTotalWeight.Text + "|" + NoofChkPcs + "|" + NoofChkPcsWeight + "|" + NoofChkPcsDryWeight + "|" + lblQualityId.Text + "~";
                }
                else
                {
                    DetailData = DetailData + lblPROCESS_REC_ID.Text + "|" + txtTotalWeight.Text + "|" + NoofChkPcs + "|" + NoofChkPcsWeight + "|" + NoofChkPcsDryWeight + "|" + lblQualityId.Text + "~";
                }

            }
            //if ((txtTotalWeight.Text == "" || txtChkPcs.Text=="" || txtChkWeight.Text=="") && Chkboxitem.Checked == true)   // Change when Updated Completed
            if ((txtTotalWeight.Text == "") && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Total Weight/Check Pcs/Check Weight can not be blank');", true);
                txtTotalWeight.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && ((Convert.ToDecimal(txtTotalWeight.Text) <= 0)))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Total Weight/Check Pcs/Check Weight always greater then zero');", true);
                txtTotalWeight.Focus();
                return;
            }
        }

        if (status == "" || DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
            return;
        }

        if (lblmessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
                param[1] = new SqlParameter("@ReceiveDate", txtBazaarDate.Text);
                param[2] = new SqlParameter("@DetailData", DetailData);
                param[3] = new SqlParameter("@userid", Session["varuserid"]);
                param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
                param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                param[5].Direction = ParameterDirection.Output;

                ///**********
                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveBazaarReceiveCheckWeight", param);
                //*******************                   
                Tran.Commit();
                if (param[5].Value.ToString() != "")
                {
                    lblmessage.Text = param[5].Value.ToString();
                }
                else
                {
                    lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                    FillRecDetails();
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
    }
    protected void chkEdit_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void btnFinalChallan_Click(object sender, EventArgs e)
    {
        string status = "";
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
            TextBox txtTotalWeight = ((TextBox)DG.Rows[i].FindControl("txtTotalWeight"));
            TextBox txtChkPcs = ((TextBox)DG.Rows[i].FindControl("txtChkPcs"));
            TextBox txtChkWeight = ((TextBox)DG.Rows[i].FindControl("txtChkWeight"));
            TextBox txtDryWeight = ((TextBox)DG.Rows[i].FindControl("txtDryWeight"));
            Label lblPROCESS_REC_ID = ((Label)DG.Rows[i].FindControl("lblPROCESS_REC_ID"));
            Label lblQualityId = ((Label)DG.Rows[i].FindControl("lblQualityId"));

            if (Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                status = "1";

                if (DetailData == "")
                {
                    DetailData = lblPROCESS_REC_ID.Text + "|" + txtTotalWeight.Text + "|" + txtChkPcs.Text + "|" + txtChkWeight.Text + "|" + txtDryWeight.Text + "|" + lblQualityId.Text + "~";
                }
                else
                {
                    DetailData = DetailData + lblPROCESS_REC_ID.Text + "|" + txtTotalWeight.Text + "|" + txtChkPcs.Text + "|" + txtChkWeight.Text + "|" + txtDryWeight.Text + "|" + lblQualityId.Text + "~";
                }

            }
            if ((txtTotalWeight.Text == "" || txtChkPcs.Text == "" || txtChkWeight.Text == "" || txtDryWeight.Text == "") && Chkboxitem.Checked == true)   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Total Weight/Check Pcs/Check Weight/Dry Weight can not be blank');", true);
                txtTotalWeight.Focus();
                return;
            }
            if (Chkboxitem.Checked == true && ((Convert.ToDecimal(txtTotalWeight.Text) <= 0) || (Convert.ToDecimal(txtChkPcs.Text) <= 0) || (Convert.ToDecimal(txtChkWeight.Text) <= 0) || (Convert.ToDecimal(txtDryWeight.Text) <= 0)))   // Change when Updated Completed
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Total Weight/Check Pcs/Check Weight/Dry Weight always greater then zero');", true);
                txtTotalWeight.Focus();
                return;
            }
        }

        if (status == "" || DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check boxes');", true);
            return;
        }

        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[9];
            param[0] = new SqlParameter("@Companyid", DDCompany.SelectedValue);
            param[1] = new SqlParameter("@ReceiveDate", txtBazaarDate.Text);
            param[2] = new SqlParameter("@DetailData", DetailData);
            param[3] = new SqlParameter("@userid", Session["varuserid"]);
            param[4] = new SqlParameter("@Mastercompanyid", Session["varcompanyid"]);
            param[5] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;

            ///**********
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_UPDATEBAZAARCONSUMPTION_WithDryWeightCalculation", param);
            //*******************                   
            Tran.Commit();
            if (param[5].Value.ToString() != "")
            {
                lblmessage.Text = param[5].Value.ToString();
            }
            else
            {
                lblmessage.Text = "DATA SAVED SUCCESSFULLY.";
                FillRecDetails();
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
}