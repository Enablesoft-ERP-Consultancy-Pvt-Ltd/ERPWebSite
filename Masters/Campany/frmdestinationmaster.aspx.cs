using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_frmdestinationmaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtefftdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            SqlParameter[] param = new SqlParameter[31];
            param[0] = new SqlParameter("@effDate", txtefftdate.Text);
            param[1] = new SqlParameter("@Destcode", txtdestcode.Text);
            param[2] = new SqlParameter("@Buyername", txtbuyername.Text);
            param[3] = new SqlParameter("@Buyer_address", txtbuyeraddress.Text.Trim());
            param[4] = new SqlParameter("@consignee", txtconsignee.Text);
            param[5] = new SqlParameter("@Consignee_DT", txtconsigneeDt.Text);
            param[6] = new SqlParameter("@consignee_address", txtconsigneeaddress.Text.Trim());
            param[7] = new SqlParameter("@Notifyparty", txtnotifyparty.Text);
            param[8] = new SqlParameter("@Notifyparty_DT", txtnotifydt.Text);
            param[9] = new SqlParameter("@Notifyparty_address", txtnotifyaddress.Text.Trim());
            param[10] = new SqlParameter("@Notifyparty2", txtnotifyparty2.Text);
            param[11] = new SqlParameter("@Notifyparty2_DT", txtnotifyparty2dt.Text);
            param[12] = new SqlParameter("@Notifyparty2_address", txtnotifyparty2address.Text.Trim());
            param[13] = new SqlParameter("@Receiver", txtreceiver.Text);
            param[14] = new SqlParameter("@Receiver_address", txtreceiveraddress.Text);
            param[15] = new SqlParameter("@Payingagent", txtpayingagent.Text);
            param[16] = new SqlParameter("@Payingagent_address", txtpayingagentaddress.Text);
            param[17] = new SqlParameter("@Buyer_Otherthanconsignee", txtbuyerotherthan.Text);
            param[18] = new SqlParameter("@Otherthanconsignee_address", txtbuyerotherthanconsgadd.Text.Trim());
            param[19] = new SqlParameter("@country", txtcountry.Text);
            param[20] = new SqlParameter("@Portofdisc", txtportofdisch.Text);
            param[21] = new SqlParameter("@userid", Session["varuserid"]);
            param[22] = new SqlParameter("@Mastercompanyid", Session["varMasterCompanyIDForERP"]);
            param[23] = new SqlParameter("@msg", SqlDbType.VarChar, 100);
            param[23].Direction = ParameterDirection.Output;
            param[24] = new SqlParameter("@Rec_Gstin", (txtreceivegstin.Text).Trim());
            param[25] = new SqlParameter("@Rec_State", (txtreceiverstate.Text).Trim());
            param[26] = new SqlParameter("@Rec_Statecode", (txtrecstatecode.Text).Trim());
            param[27] = new SqlParameter("@Rec_PanNo", (txtrecpanno.Text).Trim());
            param[28] = new SqlParameter("@Rec_CinNo", (txtreccinNo.Text).Trim());
            param[29] = new SqlParameter("@Invoice_receiver", (txtinvoice_receiver.Text).Trim());
            param[30] = new SqlParameter("@Invoice_receiverAddress", (txtinvoicereceive_address.Text).Trim());
            //*******************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_savedestinationmaster", param);
            lblmsg.Text = param[23].Value.ToString();
            Tran.Commit();
            refreshcontrol();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            lblmsg.Text = ex.Message;
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void refreshcontrol()
    {
        txtefftdate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtdestcode.Text = "";
        txtbuyername.Text = "";
        txtbuyeraddress.Text = "";
        txtconsignee.Text = "";
        txtconsigneeDt.Text = "";
        txtconsigneeaddress.Text = "";
        txtnotifyparty.Text = "";
        txtnotifydt.Text = "";
        txtnotifyaddress.Text = "";
        txtnotifyparty2.Text = "";
        txtnotifyparty2dt.Text = "";
        txtnotifyparty2address.Text = "";
        txtreceiver.Text = "";
        txtreceiveraddress.Text = "";
        txtpayingagent.Text = "";
        txtpayingagentaddress.Text = "";
        txtbuyerotherthan.Text = "";
        txtbuyerotherthanconsgadd.Text = "";
        txtcountry.Text = "";
        txtportofdisch.Text = "";

        txtreceivegstin.Text = "";
        txtreceiverstate.Text = "";
        txtrecstatecode.Text = "";
        txtrecpanno.Text = "";
        txtreccinNo.Text = "";
        txtinvoice_receiver.Text = "";
        txtinvoicereceive_address.Text = "";
    }
    protected void txtdestcode_TextChanged(object sender, EventArgs e)
    {
        string str = "select * From Destinationmaster where destcode='" + txtdestcode.Text + "'";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtefftdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["effectdate"]).ToString("dd-MMM-yyyy");
            txtbuyername.Text = ds.Tables[0].Rows[0]["Buyername"].ToString();
            txtbuyeraddress.Text = ds.Tables[0].Rows[0]["Buyer_address"].ToString();
            txtconsignee.Text = ds.Tables[0].Rows[0]["consignee"].ToString();
            txtconsigneeDt.Text = ds.Tables[0].Rows[0]["consignee_dt"].ToString();
            txtconsigneeaddress.Text = ds.Tables[0].Rows[0]["consignee_address"].ToString();
            txtnotifyparty.Text = ds.Tables[0].Rows[0]["Notifyparty"].ToString();
            txtnotifydt.Text = ds.Tables[0].Rows[0]["Notifyparty_Dt"].ToString();
            txtnotifyaddress.Text = ds.Tables[0].Rows[0]["Notifyparty_address"].ToString();
            txtnotifyparty2.Text = ds.Tables[0].Rows[0]["Notifyparty2"].ToString();
            txtnotifyparty2dt.Text = ds.Tables[0].Rows[0]["Notifyparty2_Dt"].ToString();
            txtnotifyparty2address.Text = ds.Tables[0].Rows[0]["Notifyparty2_address"].ToString();
            txtreceiver.Text = ds.Tables[0].Rows[0]["Receiver"].ToString();
            txtreceiveraddress.Text = ds.Tables[0].Rows[0]["Receiver_address"].ToString();
            txtpayingagent.Text = ds.Tables[0].Rows[0]["payingagent"].ToString();
            txtpayingagentaddress.Text = ds.Tables[0].Rows[0]["payingagent_address"].ToString();
            txtbuyerotherthan.Text = ds.Tables[0].Rows[0]["Buyer_otherthanconsignee"].ToString();
            txtbuyerotherthanconsgadd.Text = ds.Tables[0].Rows[0]["otherthanconsignee_address"].ToString();
            txtcountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
            txtportofdisch.Text = ds.Tables[0].Rows[0]["portofdisc"].ToString();
            txtreceivegstin.Text = ds.Tables[0].Rows[0]["rec_gstin"].ToString();
            txtrecstatecode.Text = ds.Tables[0].Rows[0]["rec_statecode"].ToString();
            txtreceiverstate.Text = ds.Tables[0].Rows[0]["rec_State"].ToString();
            txtrecpanno.Text = ds.Tables[0].Rows[0]["rec_PanNo"].ToString();
            txtreccinNo.Text = ds.Tables[0].Rows[0]["rec_CinNo"].ToString();
            txtinvoice_receiver.Text = ds.Tables[0].Rows[0]["Invoice_receiver"].ToString();
            txtinvoicereceive_address.Text = ds.Tables[0].Rows[0]["invoice_receiveradd"].ToString();
        }

    }
}