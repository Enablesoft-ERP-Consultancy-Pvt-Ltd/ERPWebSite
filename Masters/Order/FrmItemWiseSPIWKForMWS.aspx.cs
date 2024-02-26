using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;
public partial class Masters_Order_FrmItemWiseSPIWKForMWS : CustomPage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDLInCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDLInCompanyName, @"select CI.CompanyId,CompanyName From CompanyInfo CI(Nolock),Company_Authentication CA(Nolock) 
            Where CI.CompanyId=CA.CompanyId And CA.UserId=" + Session["varuserId"] + " And CA.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CompanyName", true, "--Select--");

            if (DDLInCompanyName.Items.Count > 0)
            {
                DDLInCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDLInCompanyName.Enabled = false;
            }

            CompanyNameSelectedIndexChanged();
            
            TxtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }
    protected void DDLInCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        if (DDLInCompanyName.SelectedIndex > 0)
        {
            Fill_Grid();
        }
    }
    private void Fill_Grid()
    {
        DGItemDetail.DataSource = GetDetail();
        DGItemDetail.DataBind();
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] para = new SqlParameter[8];
            para[0] = new SqlParameter("@CompanyID", SqlDbType.Int);
            para[1] = new SqlParameter("@UserID", SqlDbType.Int);
            para[2] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            para[3] = new SqlParameter("@Msg", SqlDbType.VarChar, 250);

            para[0].Value = DDLInCompanyName.SelectedValue;
            para[1].Value = Session["varuserid"];
            para[2].Value = Session["varMasterCompanyIDForERP"];
            para[3].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "PRO_GET_ITEMDETAILSPIWKForMWS", para);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/GetDetail/FrmItemWiseSPIWKForMWS.aspx");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
        return ds;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Savedetail();
    }
    protected void Savedetail()
    {
        string Str = "";
        if (lblErrorMessage.Text == "")
        {
            for (int i = 0; i < DGItemDetail.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DGItemDetail.Rows[i].FindControl("Chkboxitem"));
                if (Chkboxitem.Checked == true)
                {
                    string strItem_Finished_ID = DGItemDetail.DataKeys[i].Value.ToString();
                    TextBox txtStatus = (TextBox)DGItemDetail.Rows[i].FindControl("txtStatus");
                    TextBox txtSPIWKQty = (TextBox)DGItemDetail.Rows[i].FindControl("txtSPIWKQty");
                    if (Str == "")
                    {
                        Str = strItem_Finished_ID + "|" + txtStatus.Text + "|" + txtSPIWKQty.Text + "~";
                    }
                    else
                    {
                        Str = Str + strItem_Finished_ID + "|" + txtStatus.Text + "|" + txtSPIWKQty.Text + "~";
                    }
                }
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Pro_Save_ItemWiseSPIWKForMWS", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                if (Str != "")
                {
                    cmd.Parameters.AddWithValue("@CompanyID", DDLInCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
                    cmd.Parameters.AddWithValue("@DetailData", Str);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@MastercompanyID", Session["varMasterCompanyIDForERP"]);
                    
                    cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@msg"].Value.ToString() != "")
                    {
                        Tran.Rollback();
                    }
                    else
                    {    
                        Tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/Savedetail/FrmItemWiseSPIWKForMWS.aspx");
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = ex.Message;
                Logs.WriteErrorLog(ex.Message);
                Tran.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    private void MessageSave()
    {
        StringBuilder stb = new StringBuilder();
        stb.Append("<script>");
        stb.Append("alert('Record(s) has been saved successfully!');</script>");
        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
    }
    protected void DGItemDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGItemDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGItemDetail_RowDataBound111(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < DGItemDetail.Columns.Count; i++)
            {
                if (DGItemDetail.Columns[i].HeaderText.ToUpper() == "INTERNAL PROD QTY REQ." || DGItemDetail.Columns[i].HeaderText.ToUpper() == "PRE INTERNAL PROD QTY.")
                {
                    if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
                    {
                        DGItemDetail.Columns[i].Visible = true;
                    }
                    else
                    {
                        DGItemDetail.Columns[i].Visible = false;
                    }
                }
                if (Convert.ToInt32(Session["varcompanyno"]) != 16)
                {
                    if (DGItemDetail.Columns[i].HeaderText.ToUpper() == "PROD WEAVING RATE" || DGItemDetail.Columns[i].HeaderText.ToUpper() == "INT WEAVING RATE")
                    {
                        DGItemDetail.Columns[i].Visible = false;
                    }
                }
            }
        }
    }
}