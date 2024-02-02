using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmPartnerEmployeeVendorMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            String Str = @"Select ObjectID, TranslatedString From Translation Where PageName = 'PartnerEmployeeVendorMaster' Order By ObjectID ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddType, ds, 0, false, "");
            Fill_Grid();
        }
    }

    protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        lblmsg.Text = "";
        String Str = @"Select EmployeeID, EmployeeName, EmployeeAddress Address, PhoneNo, EmailID, Case When Active = 0 Then 'Active' Else 'InActive' End ActiveStatus 
        From EmployeeMaster(Nolock) Where EmployeeTypeID = " + ddType.SelectedValue + @"
        And Active = 0 ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGPPDetail.DataSource = ds.Tables[0];
        DGPPDetail.DataBind();
        if (ds.Tables[0].Rows.Count == 0)
        {
            lblmsg.Text = "No Record present for " + ddType.SelectedItem.Text;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[10];
            _arrPara[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[1] = new SqlParameter("@TypeID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@EmployeeName", SqlDbType.NVarChar, 300);
            _arrPara[3] = new SqlParameter("@Address", SqlDbType.NVarChar, 500);
            _arrPara[4] = new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 100);
            _arrPara[5] = new SqlParameter("@EMailID", SqlDbType.NVarChar, 300);
            _arrPara[6] = new SqlParameter("@Status", SqlDbType.Int);
            _arrPara[7] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = 0;
            _arrPara[1].Value = ddType.SelectedValue;
            _arrPara[2].Value = txtOwnerPartnerVendor.Text.ToUpper();
            _arrPara[3].Value = TxtAddress.Text.ToUpper();
            _arrPara[4].Value = TxtPhoneNo.Text.ToUpper();
            _arrPara[5].Value = TxtMailID.Text.ToUpper();
            _arrPara[6].Value = 0;
            _arrPara[7].Value = Session["varuserid"].ToString();
            _arrPara[8].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[9].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_Save_EmployeeMaster", _arrPara);

            if (_arrPara[9].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Saved');", true);
                tran.Commit();
                txtOwnerPartnerVendor.Text = "";
                TxtAddress.Text = "";
                TxtPhoneNo.Text = "";
                TxtMailID.Text = "";
                Fill_Grid();
                txtOwnerPartnerVendor.Focus();
            }
            else
            {
                lblmsg.Text = _arrPara[9].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("FrmPartnerEmployeeVendorMaster|" + ex.Message);
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