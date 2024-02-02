using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmInvoiceOtherDetail : System.Web.UI.Page
{
    string Msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (IsPostBack == false)
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            con.Open();
            TxtInvoiceId.Text = Request.QueryString["ID"];
            string Qry = "Select Contents,Declaration,ACDPerson,TBuyerOConsignee,Goodsid from invoice Where InvoiceId=" + TxtInvoiceId.Text + @"
                          Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GoodsName";
            DataSet Ds = SqlHelper.ExecuteDataset(con, CommandType.Text, Qry);
            UtilityModule.ConditionalComboFillWithDS(ref DDGoods, Ds, 1, true, "--Select--");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TxtContents.Text = Ds.Tables[0].Rows[0]["Contents"].ToString();
                TxtACDPerson.Text = Ds.Tables[0].Rows[0]["ACDPerson"].ToString();
                TxtDeclaration.Text = Ds.Tables[0].Rows[0]["Declaration"].ToString();
                TxtNotify.Text = Ds.Tables[0].Rows[0]["TBuyerOConsignee"].ToString();
                DDGoods.SelectedValue = Ds.Tables[0].Rows[0]["Goodsid"].ToString();
            }
        }
    }
    protected void refreshcolor_Click(object sender, EventArgs e)
    {
        UtilityModule.ConditionalComboFill(ref DDGoods, "Select GoodsId,GoodsName From GoodsDesc Where MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + " Order By GoodsName", true, "--Select--");
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
                SqlParameter[] _arrPara = new SqlParameter[6];
                _arrPara[0] = new SqlParameter("@InvoiceID", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@Contents", SqlDbType.NVarChar, 500);
                _arrPara[2] = new SqlParameter("@Declaration", SqlDbType.NVarChar, 500);
                _arrPara[3] = new SqlParameter("@ACDPerson", SqlDbType.NVarChar, 50);
                _arrPara[4] = new SqlParameter("@TBuyerOConsignee", SqlDbType.NVarChar, 500);
                _arrPara[5] = new SqlParameter("@DescriptionGoods", SqlDbType.NVarChar, 300);

                _arrPara[0].Value = TxtInvoiceId.Text;
                _arrPara[1].Value = TxtContents.Text == "" ? "" : TxtContents.Text;
                _arrPara[2].Value = TxtDeclaration.Text == "" ? "" : TxtDeclaration.Text;
                _arrPara[3].Value = TxtACDPerson.Text == "" ? "" : TxtACDPerson.Text;
                _arrPara[4].Value = TxtNotify.Text != "" ? "NOTIFY: " + TxtNotify.Text : " ";
                _arrPara[5].Value = DDGoods.SelectedValue;

                string Str = "Update Invoice Set Contents='" + _arrPara[1].Value + "',Declaration='" + _arrPara[2].Value + "',ACDPerson='" + _arrPara[3].Value + @"',
                 TBuyerOConsignee='" + _arrPara[4].Value + "', Goodsid=" + _arrPara[5].Value + " Where InvoiceId=" + _arrPara[0].Value;
                SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, Str);
                Tran.Commit();
                Msg = "Record(s) has been saved successfuly!";
                MessageSave(Msg);
                LblErrorMessage.Visible = true;
                // LblErrorMessage.Text = "Data Saved Successfully....";
                DataSet dt = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "select isnull(max(id),0)+1  from UpdateStatus");
                SqlHelper.ExecuteNonQuery(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, "insert into UpdateStatus(id,companyid,userid,tablename,tableid,date,status)values(" + dt.Tables[0].Rows[0][0].ToString() + "," + Session["varMasterCompanyIDForERP"].ToString() + "," + Session["varuserid"].ToString() + ",'Invoice'," + _arrPara[0].Value + ",getdate(),'Update')");
            }
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Packing/FrmInvoiceOtherDetail.aspx");
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
    private void MessageSave(string msg)
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('");
        stb.Append(msg);
        stb.Append("');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
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

}