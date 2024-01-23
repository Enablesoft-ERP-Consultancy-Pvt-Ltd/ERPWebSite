using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;

public partial class Masters_Carpet_FrmAttachCompanyCustomer : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            UtilityModule.ConditonalChkBoxListFill(ref ChkBoxListCustomerCode, @"Select CI.CustomerID, CI.CustomerCode + '/' + CI.CustomerName CustomerName 
                From CustomerInfo CI(Nolock) 
                Where CI.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @" Order By CustomerName ");

            string str = @"Select CI.CompanyID, CI.CompanyName 
                From CompanyInfo CI(Nolock)
                Join Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + @" 
                Where CI.MasterCompanyID = " + Session["varMasterCompanyIDForERP"] + @"
                Order By CI.CompanyName";

            UtilityModule.ConditionalComboFill(ref DDCompanyName, str, true, "---Select---");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedIndex = 1;
                CompanySelectedIndexChanged();
            }
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanySelectedIndexChanged();
    }
    private void CompanySelectedIndexChanged()
    {
        for (int j = 0; j < ChkBoxListCustomerCode.Items.Count; j++)
        {
            ChkBoxListCustomerCode.Items[j].Selected = false;
        }
        DataSet Ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, @"Select CompanyID, CustomerID 
            From CompanyWiseCustomerDetail(Nolock) 
            Where CompanyID = " + DDCompanyName.SelectedValue + " Order By CustomerID");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ChkBoxListCustomerCode.Items.Count; j++)
                {
                    if (Convert.ToInt32(ChkBoxListCustomerCode.Items[j].Value) == Convert.ToInt32(Ds.Tables[0].Rows[i]["CustomerID"]))
                    {
                        ChkBoxListCustomerCode.Items[j].Selected = true;
                    }
                }
            }
        }
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
            string VarCustomerID = "";
            for (int i = 0; i < ChkBoxListCustomerCode.Items.Count; i++)
            {
                if (ChkBoxListCustomerCode.Items[i].Selected)
                {
                    if (VarCustomerID == "")
                    {
                        VarCustomerID = ChkBoxListCustomerCode.Items[i].Value + '|';
                    }
                    else
                    {
                        VarCustomerID = VarCustomerID + ChkBoxListCustomerCode.Items[i].Value + '|';
                    }
                }
            }
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@CompanyID", DDCompanyName.SelectedValue);
            param[1] = new SqlParameter("@CustomerIDs", VarCustomerID);
            param[2] = new SqlParameter("@UserID", Session["varuserid"]);
            param[3] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[4] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_SaveCompanyCustomerAttach", param);
            Tran.Commit();
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('" + param[4].Value.ToString() + "');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('" + ex.Message + "');", true);
            Tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ChkForAllSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkForAllSelect.Checked == true)
        {
            for (int j = 0; j < ChkBoxListCustomerCode.Items.Count; j++)
            {
                ChkBoxListCustomerCode.Items[j].Selected = true;
            }
        }
        else
        {
            for (int j = 0; j < ChkBoxListCustomerCode.Items.Count; j++)
            {
                ChkBoxListCustomerCode.Items[j].Selected = false;
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
}