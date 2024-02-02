using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_ReportForms_FrmPartnerPropertyTransactionDetail : System.Web.UI.Page
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
                Order By EmployeeName 

                Select PM.PropertyID, PM.PropertyName + ' (' + EM.EmployeeName + ')' PropertyName
                From PropertyMaster PM(Nolock) 
                JOIN EmployeeMaster EM(Nolock) ON EM.EmployeeID = PM.PropertyOwnerID 
                Order By PM.PropertyName 
                Select ObjectID, TranslatedString From Translation(Nolock) Where ObjectName = 'TransactionType' Order By TranslatedString ";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

            UtilityModule.ConditionalComboFillWithDS(ref DDFromPartnerName, ds, 0, false, "");
            UtilityModule.ConditionalComboFillWithDS(ref DDPropertyName, ds, 1, true, " Select Property Name ");
            UtilityModule.ConditionalComboFillWithDS(ref DDPaymentMode, ds, 2, false, "");
            FillToPartnerName();
        }
    }

    protected void DDFromPartnerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillToPartnerName();
        if (DDPaymentMode.SelectedIndex > 0)
        {
            FillFromBankAccountNo();
        }
    }
    private void FillFromBankAccountNo()
    {
        String Str = @"Select BAM.ID, BAM.BankName + ' (' + BAM.NickName + ')' Name 
            From BankAccountMaster BAM(Nolock) 
            Where BAM.EmployeeID = " + DDFromPartnerName.SelectedValue + " Order By Name";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDFromBankAccountNo, ds, 0, false, "");
    }
    protected void DDPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        trBankDetail.Visible = false;
        tdToBankAccountNo.Visible = false;
        if (DDPaymentMode.SelectedIndex > 0)
        {
            trBankDetail.Visible = true;
            tdToBankAccountNo.Visible = true;
        }
        FillFromBankAccountNo();
    }
    private void FillToPartnerName()
    {
        String Str = @"Select EM.EmployeeID, EM.EmployeeName + ' (' + T.TranslatedString + ')' EmployeeName 
                From EmployeeMaster EM(Nolock) 
                JOIN Translation T(Nolock) ON T.ObjectID = EM.EmployeeTypeID And T.PageName = 'PartnerEmployeeVendorMaster'
                Where EM.EmployeeID <> " + DDFromPartnerName.SelectedValue + @"
                Order By EmployeeName ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDToPartnerName, ds, 0, true, "  select  ");

    }
    protected void DDToPartnerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDPaymentMode.SelectedIndex > 0)
        {
            FillToBankAccountNo();
        }
    }

    private void FillToBankAccountNo()
    {
        String Str = @"Select BAM.ID, BAM.BankName + ' (' + BAM.NickName + ')' Name 
            From BankAccountMaster BAM(Nolock) 
            Where BAM.EmployeeID = " + DDToPartnerName.SelectedValue + " Order By Name";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);

        UtilityModule.ConditionalComboFillWithDS(ref DDToBankAccountNo, ds, 0, false, "");
    }
    protected void DGPartnerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGPartnerDetail, "Select$" + e.Row.RowIndex);
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] _arrPara = new SqlParameter[15];
            _arrPara[0] = new SqlParameter("@PartnerEmployeeID", SqlDbType.Int);
            _arrPara[1] = new SqlParameter("@PropertyID", SqlDbType.Int);
            _arrPara[2] = new SqlParameter("@PaymentMode", SqlDbType.Int);
            _arrPara[3] = new SqlParameter("@TransactionDate", SqlDbType.DateTime);
            _arrPara[4] = new SqlParameter("@FromBankAccountNoID", SqlDbType.Int);
            _arrPara[5] = new SqlParameter("@ChequeTransactionNo", SqlDbType.NVarChar, 250);
            _arrPara[6] = new SqlParameter("@ChequeTransactionDate", SqlDbType.DateTime);
            _arrPara[7] = new SqlParameter("@ToPartnerEmployeeID", SqlDbType.Int);
            _arrPara[8] = new SqlParameter("@ToBankAccountNoID", SqlDbType.Int);
            _arrPara[9] = new SqlParameter("@Amount", SqlDbType.Float);
            _arrPara[10] = new SqlParameter("@Remark", SqlDbType.NVarChar, 500);
            _arrPara[11] = new SqlParameter("@Status", SqlDbType.Int);
            _arrPara[12] = new SqlParameter("@UserID", SqlDbType.Int);
            _arrPara[13] = new SqlParameter("@MasterCompanyId", SqlDbType.Int);
            _arrPara[14] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);

            _arrPara[0].Value = DDFromPartnerName.SelectedValue;
            _arrPara[1].Value = DDPropertyName.SelectedValue;
            _arrPara[2].Value = DDPaymentMode.SelectedValue;
            _arrPara[3].Value = TxtTransactionDate.Text;
            _arrPara[4].Value = trBankDetail.Visible == true ? DDFromBankAccountNo.SelectedValue : "0";
            _arrPara[5].Value = trBankDetail.Visible == true ? txtChequeTransactionNo.Text : "";
            _arrPara[6].Value = trBankDetail.Visible == true ? txtChequeTransactionDate.Text : "01-Jan-2000";
            _arrPara[7].Value = DDToPartnerName.SelectedValue;
            _arrPara[8].Value = tdToBankAccountNo.Visible == true ? DDToBankAccountNo.SelectedValue : "0";
            _arrPara[9].Value = TxtAmount.Text;
            _arrPara[10].Value = txtRemark.Text.ToUpper();
            _arrPara[11].Value = 0;
            _arrPara[12].Value = Session["varuserid"].ToString();
            _arrPara[13].Value = Session["varMasterCompanyIDForERP"].ToString();
            _arrPara[14].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "Pro_Save_PropertyPartnerDetail", _arrPara);

            if (_arrPara[14].Value.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Data Successfully Saved');", true);
                tran.Commit();
            }
            else
            {
                lblErrorMsg.Text = _arrPara[14].Value.ToString();
                tran.Rollback();
            }
        }
        catch (Exception ex)
        {

            Logs.WriteErrorLog("FrmPropertyPartnerDetail|" + ex.Message);
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