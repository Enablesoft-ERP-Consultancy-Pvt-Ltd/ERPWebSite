using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Packing_FrmInvoiceOtherDetailDestini : System.Web.UI.Page
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
            UtilityModule.ConditionalComboFill(ref DDGoods, "Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GoodsName", true, "--Select--");
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        con.Open();
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            LblErrorMessage.Text = "";
            Validation_Check();
            if (LblErrorMessage.Text == "")
            {
                SqlParameter[] _arrPara = new SqlParameter[21];
                _arrPara[0] = new SqlParameter("@InvoiceID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Contents", SqlDbType.NVarChar, 500);
                _arrPara[2] = new SqlParameter("@OtherRef", SqlDbType.NVarChar, 100);
                _arrPara[3] = new SqlParameter("@DrawBackTerms", SqlDbType.NVarChar, 100);
                _arrPara[4] = new SqlParameter("@SingleCountryPerson", SqlDbType.NVarChar, 100);
                _arrPara[5] = new SqlParameter("@SpclInstr", SqlDbType.NVarChar, 500);
                _arrPara[6] = new SqlParameter("@InvoiceCovering", SqlDbType.NVarChar, 300);
                _arrPara[7] = new SqlParameter("@Knots", SqlDbType.NVarChar, 50);
                _arrPara[8] = new SqlParameter("@orderNo", SqlDbType.NVarChar, 50);
                _arrPara[9] = new SqlParameter("@TorderNo", SqlDbType.NVarChar, 100);
                _arrPara[10] = new SqlParameter("@ACDPerson", SqlDbType.NVarChar, 50);
                _arrPara[11] = new SqlParameter("@SDFNo", SqlDbType.NVarChar, 50);
                _arrPara[12] = new SqlParameter("@GRNo", SqlDbType.NVarChar, 50);
                _arrPara[13] = new SqlParameter("@BLAgencyInfo", SqlDbType.NVarChar, 500);
                _arrPara[14] = new SqlParameter("@BLAgent", SqlDbType.NVarChar, 500);
                _arrPara[15] = new SqlParameter("@ExchRate", SqlDbType.Float);
                _arrPara[16] = new SqlParameter("@TRRRNOAndDate", SqlDbType.NVarChar, 50);
                _arrPara[17] = new SqlParameter("@DocType", SqlDbType.NVarChar, 50);
                _arrPara[18] = new SqlParameter("@DocRef", SqlDbType.NVarChar, 100);
                _arrPara[19] = new SqlParameter("@ProformaRef", SqlDbType.NVarChar, 300);
                _arrPara[20] = new SqlParameter("@DescriptionGoods", SqlDbType.NVarChar, 300);

                _arrPara[0].Value = TxtInvoiceId.Text;
                _arrPara[1].Value = TxtContents.Text == "" ? "" : TxtContents.Text;
                _arrPara[2].Value = TxtOtherRef.Text == "" ? "" : TxtOtherRef.Text;
                _arrPara[3].Value = TxtDBackSwhiteBill.Text == "" ? "" : TxtDBackSwhiteBill.Text;
                _arrPara[4].Value = TxtPersonName.Text == "" ? "" : TxtPersonName.Text;
                _arrPara[5].Value = TxtSplInstr.Text == "" ? "" : TxtSplInstr.Text;
                _arrPara[6].Value = txtInvoiceCvrng.Text == "" ? "" : txtInvoiceCvrng.Text;
                _arrPara[7].Value = TxtKnots.Text == "" ? "" : TxtKnots.Text;
                _arrPara[8].Value = TxtOrderNo.Text == "" ? "" : TxtOrderNo.Text;
                _arrPara[9].Value = TxtTOrderNo.Text == "" ? "" : TxtTOrderNo.Text;
                _arrPara[10].Value = TxtACDPerson.Text == "" ? "" : TxtACDPerson.Text;
                _arrPara[11].Value = TxtSDFNo.Text == "" ? "" : TxtSDFNo.Text;
                _arrPara[12].Value = TxtGRNo.Text == "" ? "" : TxtGRNo.Text;
                _arrPara[13].Value = TxtBLAgencyInfo.Text == "" ? "" : TxtBLAgencyInfo.Text;
                _arrPara[14].Value = TxtVesselAgent.Text == "" ? "" : TxtVesselAgent.Text;
                _arrPara[15].Value = TxtExchRate.Text == "" ? "0" : TxtExchRate.Text;
                _arrPara[16].Value = TxtTRNo.Text == "" ? "" : TxtTRNo.Text;
                _arrPara[17].Value = DDDocType.SelectedValue;
                _arrPara[18].Value = TxtDocRef.Text == "" ? "" : TxtDocRef.Text;
                _arrPara[19].Value = txtPrfmaRef.Text == "" ? "" : txtPrfmaRef.Text;
                _arrPara[20].Value = DDGoods.SelectedValue;

                string Str = "Update Invoice Set Contents='" + _arrPara[1].Value + "',OtherRef='" + _arrPara[2].Value + "',DrawBackTerms='" + _arrPara[3].Value + @"',
                SingleCountryPerson='" + _arrPara[4].Value + "',SpclInstr='" + _arrPara[5].Value + "',InvoiceCovering='" + _arrPara[6].Value + "',Knots='" + _arrPara[7].Value + @"',
                OrderNo='" + _arrPara[8].Value + "',TorderNo='" + _arrPara[9].Value + "',ACDPerson='" + _arrPara[10].Value + "',SDFNo='" + _arrPara[11].Value + @"',
                GRNo='" + _arrPara[12].Value + "',BLAgencyInfo='" + _arrPara[13].Value + "',BLAgent='" + _arrPara[14].Value + "',ExchRate=" + _arrPara[15].Value + @",
                TRRRNOAndDate='" + _arrPara[16].Value + "',DocType='" + _arrPara[17].Value + "',DocRef='" + _arrPara[18].Value + "',ProformaRef='" + _arrPara[19].Value + @"',
                Goodsid=" + _arrPara[20].Value + " Where InvoiceId=" + _arrPara[0].Value;

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
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceOtherDetailDestini.aspx");
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
        if (UtilityModule.VALIDDROPDOWNLIST(DDGoods) == false)
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
    protected void BtnRefreshDescriptionOfGood_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDGoods, "Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GoodsName", true, "--Select--");
    }
}