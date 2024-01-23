using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_FrmInvoiceTermsBankDetailDestini : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            TxtInvoiceId.Text = Request.QueryString["ID"];
            string Str = @"Select PaymentId, PaymentName from Payment Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" order by PaymentName
            Select TermId,TermName from Term Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By TermName";
            DataSet ds = SqlHelper.ExecuteDataset(Str);
            UtilityModule.ConditionalComboFillWithDS(ref DDDelivery, ds, 0, true, "--Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDTerms, ds, 1, true, "--Select--");
            if (DDDelivery.Items.Count > 0)
            {
                DDDelivery.SelectedIndex = 1;
            }
        }
    }
    protected void RDDeclaration1_CheckedChanged(object sender, EventArgs e)
    {
        if (RDDeclaration1.Checked == true)
        {
            RDDeclaration2.Checked = false;
            TxtDeclaration.Text = "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Declaration1 from companyinfo CI,Invoice I Where CI.Companyid=I.ConsignorId And I.Invoiceid=" + TxtInvoiceId.Text + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtDeclaration.Text = ds.Tables[0].Rows[0]["Declaration1"].ToString();
            }
        }
    }
    protected void RDDeclaration2_CheckedChanged(object sender, EventArgs e)
    {
        if (RDDeclaration2.Checked == true)
        {
            TxtDeclaration.Text = "";
            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "Select Declaration2 from companyinfo CI,Invoice I Where CI.Companyid=I.ConsignorId And I.Invoiceid=" + TxtInvoiceId.Text + " And CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                TxtDeclaration.Text = ds.Tables[0].Rows[0]["Declaration2"].ToString();
            }
            RDDeclaration1.Checked = false;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Validation_Check();
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrPara = new SqlParameter[19];
                _arrPara[0] = new SqlParameter("@InvoiceID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@DelTerms", SqlDbType.Int);
                _arrPara[2] = new SqlParameter("@CreditId", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@terms", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@frmbldate", SqlDbType.Int);
                _arrPara[5] = new SqlParameter("@frmbldateafter", SqlDbType.Int);
                _arrPara[6] = new SqlParameter("@FreightTerms", SqlDbType.NVarChar, 100);
                _arrPara[7] = new SqlParameter("@TBuyerOConsignee", SqlDbType.NVarChar, 500);
                _arrPara[8] = new SqlParameter("@Declaration", SqlDbType.NVarChar, 500);
                _arrPara[9] = new SqlParameter("@GriNo", SqlDbType.Int);
                _arrPara[10] = new SqlParameter("@GSPNo", SqlDbType.NVarChar, 50);
                _arrPara[11] = new SqlParameter("@BlNo", SqlDbType.NVarChar, 50);
                _arrPara[12] = new SqlParameter("@BlDt", SqlDbType.NVarChar, 50);
                _arrPara[13] = new SqlParameter("@ExControlNo", SqlDbType.NVarChar, 50);
                _arrPara[14] = new SqlParameter("@ExControlDate", SqlDbType.NVarChar, 50);
                _arrPara[15] = new SqlParameter("@drawbackamount", SqlDbType.Float);
                _arrPara[16] = new SqlParameter("@LcNo", SqlDbType.NVarChar, 50);
                _arrPara[17] = new SqlParameter("@Lcdate", SqlDbType.NVarChar, 50);
                _arrPara[18] = new SqlParameter("@InsPolicyNo", SqlDbType.NVarChar, 50);

                _arrPara[0].Value = TxtInvoiceId.Text;
                _arrPara[1].Value = DDDelivery.SelectedValue;
                _arrPara[2].Value = DDTerms.SelectedIndex == 0 ? "0" : DDTerms.SelectedValue;
                _arrPara[3].Value = TxtDays.Text == "" ? "0" : TxtDays.Text;
                _arrPara[4].Value = ChkBilling.Checked == true ? 1 : 0;
                _arrPara[5].Value = ChkShipmentDays.Checked == true ? 1 : 0;
                _arrPara[6].Value = TxtFreightTerms.Text == "" ? "" : TxtFreightTerms.Text;
                _arrPara[7].Value = TxtNotify.Text != "" ? "NOTIFY: " + TxtNotify.Text : " ";
                _arrPara[8].Value = TxtDeclaration.Text == "" ? "" : TxtDeclaration.Text;
                _arrPara[9].Value = TxtGriNo.Text == "" ? "0" : TxtGriNo.Text;
                _arrPara[10].Value = TxtGspNo.Text == "" ? "" : TxtGspNo.Text;
                _arrPara[11].Value = TxtAWBNo.Text == "" ? "" : TxtAWBNo.Text;
                _arrPara[12].Value = TxtBlAwbDate.Text == "" ? "" : TxtBlAwbDate.Text;
                _arrPara[13].Value = TxtExControlNo.Text == "" ? "" : TxtExControlNo.Text;
                _arrPara[14].Value = TxtExControlDate.Text == "" ? "" : TxtExControlDate.Text;
                _arrPara[15].Value = TxtDrawBackAmount.Text == "" ? "0" : TxtDrawBackAmount.Text;
                _arrPara[16].Value = TxtLCNo.Text == "" ? "" : TxtLCNo.Text;
                _arrPara[17].Value = TxtLCDate.Text == "" ? "" : TxtLCDate.Text;
                _arrPara[18].Value = txtInsPolicy.Text == "" ? "" : txtInsPolicy.Text;

                string Str = "Update Invoice Set DelTerms=" + _arrPara[1].Value + ",CreditId=" + _arrPara[2].Value + ",terms=" + _arrPara[3].Value + ",frmbldate=" + _arrPara[4].Value + @",
                frmbldateafter=" + _arrPara[5].Value + ",FreightTerms='" + _arrPara[6].Value + "',TBuyerOConsignee='" + _arrPara[7].Value + "',Declaration='" + _arrPara[8].Value + @"',
                GriNo=" + _arrPara[9].Value + ",GSPNo='" + _arrPara[10].Value + "',BlNo='" + _arrPara[11].Value + "',BlDt='" + _arrPara[12].Value + @"',
                ExControlNo='" + _arrPara[13].Value + "',ExControlDate='" + _arrPara[14].Value + "',drawbackamount=" + _arrPara[15].Value + ",LcNo='" + _arrPara[16].Value + @"',
                Lcdate='" + _arrPara[17].Value + "',InsPolicyNo='" + _arrPara[18].Value + "' Where InvoiceId=" + _arrPara[0].Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                Tran.Commit();
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Invoice'," + _arrPara[0].Value + ",getdate(),'Update')");
                LblErrorMessage.Visible = true;
                LblErrorMessage.Text = "Data Saved Successfully";
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceTermsBankDetailDestini.aspx");
            Tran.Rollback();
            LblErrorMessage.Visible = true;
            LblErrorMessage.Text = ex.Message;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private void Validation_Check()
    {
        LblErrorMessage.Text = "";
        if (UtilityModule.VALIDDROPDOWNLIST(DDDelivery) == false)
        {
            goto a;
        }
        else
        {
            goto B;
        }
    a:
        UtilityModule.SHOWMSG(LblErrorMessage);
    B: ;
    }
}