using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ZDevi_FrmPropertyMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack != true)
        {
            String Str = @"Select EmployeeID, EmployeeName + ' (' + EmployeeAddress + ')' EmployeeName 
            From EmployeeMaster EM(Nolock) Where EmployeeTypeID = 1";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref ddPropertyOwner, ds, 0, false, "");
            Fill_Grid();
            TxtRegistrationDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtLoanDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["PropertyID"] = "0";
        }
    }

    protected void ddOwnerPartnerVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }

    protected void ddPropertyOwner_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Grid();
    }

    private void Fill_Grid()
    {
        lblmsg.Text = "";
        String Str = @"Select a.PropertyID, a.PropertyName, a.PropertySize Size, a.PropertyRate Rate, a.PropertyCashAmount CashAmount, 
            a.PropertyChequeAmount ChqAmount, Round(PropertySize * PropertyRate, 0) + StampDuty + GovtFee + RegitryExpence + DealerComm + OtherCharge TotalAmount 
            From PropertyMaster a(nolock) 
            Where PropertyOwnerID = " + ddPropertyOwner.SelectedValue + " Order By a.PropertyID ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DGPropertDetail.DataSource = ds.Tables[0];
        DGPropertDetail.DataBind();
        if (ds.Tables[0].Rows.Count == 0)
        {
            //ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('No Record present for property owner');", true);
            lblmsg.Text = "No Record present for " + ddPropertyOwner.SelectedItem.Text;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[21];
            _arrPara[0] = new SqlParameter("@PropertyID", SqlDbType.Int);
            _arrPara[0].Direction = ParameterDirection.InputOutput;
            _arrPara[1] = new SqlParameter("@Name", SqlDbType.NVarChar, 200);
            _arrPara[2] = new SqlParameter("@Size", SqlDbType.Float);
            _arrPara[3] = new SqlParameter("@Rate", SqlDbType.Int);
            _arrPara[4] = new SqlParameter("@CashAmount", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@ChequeAmount", SqlDbType.Int);
            _arrPara[6] = new SqlParameter("@RegistrationDate", SqlDbType.DateTime);
            _arrPara[7] = new SqlParameter("@StampDuty", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@GovtFee", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@RegitryExpence", SqlDbType.Int);
            _arrPara[10] = new SqlParameter("@DealerComm", SqlDbType.Int);
            _arrPara[11] = new SqlParameter("@OtherCharge", SqlDbType.Int);
            _arrPara[12] = new SqlParameter("@LoanAmount", SqlDbType.Int);
            _arrPara[13] = new SqlParameter("@LoanDate", SqlDbType.DateTime);
            _arrPara[14] = new SqlParameter("@PropertyAddress", SqlDbType.NVarChar, 500);
            _arrPara[15] = new SqlParameter("@Remarks", SqlDbType.NVarChar, 500);
            _arrPara[16] = new SqlParameter("@PropertyOwnerID", SqlDbType.Int);
            _arrPara[17] = new SqlParameter("@Status", SqlDbType.Int);
            _arrPara[18] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[19] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[20] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = ViewState["PropertyID"];
            _arrPara[1].Value = txtPropertyName.Text.ToUpper();
            _arrPara[2].Value = TxtSize.Text;
            _arrPara[3].Value = TxtRate.Text;
            _arrPara[4].Value = TxtCashAmount.Text;
            _arrPara[5].Value = txtChequeAmount.Text;
            _arrPara[6].Value = TxtRegistrationDate.Text;
            _arrPara[7].Value = TxtStampDuty.Text;
            _arrPara[8].Value = TxtGovtFee.Text;
            _arrPara[9].Value = TxtRegistryExpence.Text;
            _arrPara[10].Value = TxtDealerComm.Text;
            _arrPara[11].Value = TxtOtherCharge.Text;
            _arrPara[12].Value = TxtLoanAmount.Text == "" ? "0" : TxtLoanAmount.Text;
            _arrPara[13].Value = txtLoanDate.Text;
            _arrPara[14].Value = TxtAddress.Text.ToUpper();
            _arrPara[15].Value = TxtRemark.Text.ToUpper();
            _arrPara[16].Value = ddPropertyOwner.SelectedValue;
            _arrPara[17].Value = 0;
            _arrPara[18].Value = Session["varuserid"].ToString();
            _arrPara[19].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[20].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_Save_PropertyMaster", _arrPara);

            if (_arrPara[20].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Saved');", true);
                tran.Commit();
                btnsave.Text = "Save";
                txtPropertyName.Text = "";
                TxtSize.Text = "";
                TxtRate.Text = "";
                TxtCashAmount.Text = "";
                txtChequeAmount.Text = "";
                TxtRegistrationDate.Text = "";
                TxtStampDuty.Text = "";
                TxtGovtFee.Text = "";
                TxtRegistryExpence.Text = "";
                TxtDealerComm.Text = "";
                TxtOtherCharge.Text = "";
                TxtLoanAmount.Text = "";
                txtLoanDate.Text = "";
                TxtAddress.Text = "";
                TxtRemark.Text = "";
                Fill_Grid();
                txtPropertyName.Focus();
                ViewState["PropertyID"] = "0";
            }
            else
            {
                lblmsg.Text = _arrPara[20].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("FrmPropertyMaster|" + ex.Message);
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

    protected void DGPropertDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPropertDetail, "Select$" + e.Row.RowIndex);
        }
    }

    protected void DGPropertDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        string PropertyID = DGPropertDetail.SelectedDataKey.Value.ToString();
        ViewState["PropertyID"] = PropertyID;

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text,
        @"Select PropertyID, PropertyName, PropertySize, PropertyRate, PropertyCashAmount, PropertyChequeAmount, 
        REPLACE(CONVERT(NVARCHAR(11), PropertyRegistrationDate, 106), ' ', '-') PropertyRegistrationDate, StampDuty, GovtFee, 
        RegitryExpence, DealerComm, OtherCharge, LoanAmount, REPLACE(CONVERT(NVARCHAR(11), LoanDate, 106), ' ', '-') LoanDate, 
        PropertyAddress, Remarks, PropertyOwnerID 
        From PropertyMaster(nolock) Where PropertyID = " + PropertyID);
        if (ds.Tables[0].Rows.Count == 1)
        {
            txtPropertyName.Text = ds.Tables[0].Rows[0]["PropertyName"].ToString();
            TxtSize.Text = ds.Tables[0].Rows[0]["PropertySize"].ToString();
            TxtRate.Text = ds.Tables[0].Rows[0]["PropertyRate"].ToString();
            TxtCashAmount.Text = ds.Tables[0].Rows[0]["PropertyCashAmount"].ToString();
            txtChequeAmount.Text = ds.Tables[0].Rows[0]["PropertyChequeAmount"].ToString();
            TxtRegistrationDate.Text = ds.Tables[0].Rows[0]["PropertyRegistrationDate"].ToString();
            TxtStampDuty.Text = ds.Tables[0].Rows[0]["StampDuty"].ToString();
            TxtGovtFee.Text = ds.Tables[0].Rows[0]["GovtFee"].ToString();
            TxtRegistryExpence.Text = ds.Tables[0].Rows[0]["RegitryExpence"].ToString();
            TxtDealerComm.Text = ds.Tables[0].Rows[0]["DealerComm"].ToString();
            TxtOtherCharge.Text = ds.Tables[0].Rows[0]["OtherCharge"].ToString();
            TxtLoanAmount.Text = ds.Tables[0].Rows[0]["LoanAmount"].ToString();
            txtLoanDate.Text = ds.Tables[0].Rows[0]["LoanDate"].ToString();
            TxtAddress.Text = ds.Tables[0].Rows[0]["PropertyAddress"].ToString();
            TxtRemark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
        }
        btnsave.Text = "Update";
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {

    }
}