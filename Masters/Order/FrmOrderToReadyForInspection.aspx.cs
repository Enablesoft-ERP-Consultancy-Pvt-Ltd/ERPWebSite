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
public partial class Masters_Campany_FrmOrderToReadyForInspection : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDLInCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDLInCompanyName, @"Select CI.CompanyId, CI.CompanyName 
                    From CompanyInfo CI(Nolock) 
                    JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId = " + Session["varuserId"] + " And CA.MasterCompanyid = " + Session["varCompanyId"] + @" 
                    Order By CI.CompanyName", true, "--Select--");

            if (DDLInCompanyName.Items.Count > 0)
            {
                DDLInCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDLInCompanyName.Enabled = false;
                CompanyNameSelectedIndexChanged();
            }

            if (DDLCustomerCode.Items.Count > 0)
            {
                DDLCustomerCode.SelectedIndex = 1;
                CustomerCodeSelectedIndexChanged();
            }
            DDLCustomerCode.Focus();
        }
    }
    protected void DDLInCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        string Str = "";
        Str = @"SELECT Distinct C.CustomerId, CompanyName + '     ' + C.CustomerCode CustomerCode  
                FROM OrderMaster OM(Nolock) 
                JOIN Customerinfo C(Nolock)  ON OM.CustomerId=C.CustomerId And C.MasterCompanyId=" + Session["varCompanyId"] + @" 
                Where OM.Status = 0 And OM.Companyid = " + DDLInCompanyName.SelectedValue + " Order By CompanyName + '     ' + C.CustomerCode";

        UtilityModule.ConditionalComboFill(ref DDLCustomerCode, Str, true, "Select CustomerCode");
    }
    protected void DDLCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChanged();
    }
    private void CustomerCodeSelectedIndexChanged()
    {
        DGOrderDetail.DataSource = null;
        DGOrderDetail.DataBind();
        string Str = "";
        Str = @"SELECT Distinct OM.OrderID, OM.CustomerOrderNo 
                FROM OrderMaster OM(Nolock) 
                Where OM.Status = 0 And OM.Companyid = " + DDLInCompanyName.SelectedValue + " And OM.CustomerID =  " + DDLCustomerCode.SelectedValue + @" 
                Order By OM.CustomerOrderNo";

        UtilityModule.ConditionalComboFill(ref DDLOrderNo, Str, true, "Select Order No");
    }
    protected void DDLOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedIndexChanged();
    }
    private void OrderNoSelectedIndexChanged()
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        if (DDLOrderNo.Items.Count > 0)
        {
            DGOrderDetail.DataSource = GetDetail();
            DGOrderDetail.DataBind();
        }
    }
    private DataSet GetDetail()
    {
        DataSet ds = null;
        try
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter("@OrderId", SqlDbType.Int);
            para[1] = new SqlParameter("@EditFlag", SqlDbType.Int);

            para[0].Value = DDLOrderNo.SelectedValue;
            para[1].Value = 0;
            ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.StoredProcedure, "Pro_Get_Order_Detail", para);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderToReadyForInspection.aspx/GetDetail");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
        }
        return ds;
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Str = "";
        lblErrorMessage.Text = "";
        if (lblErrorMessage.Text == "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("PRO_SAVE_ORDERINSPECTIONDETAIL", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                for (int i = 0; i < DGOrderDetail.Rows.Count; i++)
                {
                    string strFinishedID = DGOrderDetail.DataKeys[i].Value.ToString();
                    TextBox TxtInspectionQty = (TextBox)DGOrderDetail.Rows[i].FindControl("Txt_Inspection_Qty");
                    if (TxtInspectionQty.Text != "")
                    {
                        Str = Str + strFinishedID + "|" + TxtInspectionQty.Text + "~";
                    }
                }
                cmd.Parameters.AddWithValue("@OrderId", DDLOrderNo.SelectedValue);
                cmd.Parameters.AddWithValue("@Str", Str);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@MastercompanyId", Session["varcompanyId"]);
                cmd.Parameters.AddWithValue("@userid", Session["varuserid"]);

                cmd.ExecuteNonQuery();

                Tran.Commit();
                DDLOrderNo.SelectedIndex = 0;
                OrderNoSelectedIndexChanged();
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmOrderToReadyForInspection.aspx/BtnSave");
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
    protected void DGOrderDetail_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //for (int i = 0; i < DGOrderDetail.Columns.Count; i++)
            //{

            //    if (DGOrderDetail.Columns[i].HeaderText.ToUpper() == "INTERNAL PROD QTY REQ." || DGOrderDetail.Columns[i].HeaderText.ToUpper() == "PRE INTERNAL PROD QTY.")
            //    {
            //        if (variable.VarTAGGINGWITHINTERNALPRODUCTION == "1")
            //        {
            //            DGOrderDetail.Columns[i].Visible = true;
            //        }
            //        else
            //        {
            //            DGOrderDetail.Columns[i].Visible = false;
            //        }
            //    }

            //}
        }
    }
}