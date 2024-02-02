using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ZDevi_FrmBankAccountMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            String Str = @"Select EmployeeID, EmployeeName + ' (' + T.TranslatedString + ')' EmployeeName 
                From EmployeeMaster EM(Nolock) 
                JOIN Translation T(Nolock) ON T.ObjectID = EM.EmployeeTypeID And T.PageName = 'PartnerEmployeeVendorMaster'
                Order By EmployeeName ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddOwnerPartnerVendorName, ds, 0, false, "");
            Fill_Grid();
        }
    }

    protected void ddOwnerPartnerVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        lblmsg.Text = "";
        String Str = @"Select ID, BankName, BankAddress Address, AccountNo, IfscCode, NickName 
            From BankAccountMaster(Nolock) 
            Where EmployeeID = " + ddOwnerPartnerVendorName.SelectedValue + " Order By BankName ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        DGPPDetail.DataSource = ds.Tables[0];
        DGPPDetail.DataBind();
        if (ds.Tables[0].Rows.Count == 0)
        {
            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record present for owner');", true);
            lblmsg.Text = "No Record present for " + ddOwnerPartnerVendorName.SelectedItem.Text;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[12];
            _arrPara[0] = new SqlParameter("@ID", SqlDbType.Int);
            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[1] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@BankName", SqlDbType.NVarChar, 200);
            _arrPara[3] = new SqlParameter("@BankAddress", SqlDbType.NVarChar, 200);
            _arrPara[4] = new SqlParameter("@AccountNo", SqlDbType.NVarChar, 200);
            _arrPara[5] = new SqlParameter("@IfscCode", SqlDbType.NVarChar, 200);
            _arrPara[6] = new SqlParameter("@MicrCode", SqlDbType.NVarChar, 200);
            _arrPara[7] = new SqlParameter("@NickName", SqlDbType.NVarChar, 200);
            _arrPara[8] = new SqlParameter("@Status", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[11] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddOwnerPartnerVendorName.SelectedValue;
            _arrPara[2].Value = txtBankName.Text.ToUpper();
            _arrPara[3].Value = TxtAddress.Text.ToUpper();
            _arrPara[4].Value = TxtAccountNo.Text.ToUpper();
            _arrPara[5].Value = TxtIFSCCode.Text.ToUpper();
            _arrPara[6].Value = txtMicrCode.Text.ToUpper();
            _arrPara[7].Value = TxtNickName.Text.ToUpper();
            _arrPara[8].Value = 0;
            _arrPara[9].Value = Session["varuserid"].ToString();
            _arrPara[10].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[11].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_Save_BankAccountMaster", _arrPara);

            if (_arrPara[11].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Saved');", true);
                tran.Commit();
                txtBankName.Text = "";
                TxtAddress.Text = "";
                TxtAccountNo.Text = "";
                TxtIFSCCode.Text = "";
                txtMicrCode.Text = "";
                TxtNickName.Text = "";
                Fill_Grid();
                txtBankName.Focus();
            }
            else
            {
                lblmsg.Text = _arrPara[11].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("FrmBankAccountMaster|" + ex.Message);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
}