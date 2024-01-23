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
public partial class Masters_Order_FrmAssignOtherOrder : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            DDCompanyName.Focus();
            UtilityModule.ConditionalComboFill(ref DDCompanyName, @"Select CI.CompanyId, CI.CompanyName 
                From CompanyInfo CI(Nolock)
                JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId=" + Session["varuserId"] + @"
                Where CI.MasterCompanyid=" + Session["varMasterCompanyIDForERP"] + " order by CI.CompanyName", true, "--Select--");

            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            CompanyNameSelectedIndexChanged();
            if (DDNewCustomerCode.Items.Count > 0)
            {
                DDNewCustomerCode.SelectedIndex = 1;
                EditCheckedChanged();
            }
            DDNewOrderNo.Focus();
        }
    }
    protected void DDCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
    }
    private void CompanyNameSelectedIndexChanged()
    {
        string Str = "";
        if (DDCompanyName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                Str = @"SELECT distinct C.CustomerId,(companyName+'     '+C.CustomerCode) CustomerCode  
                        FROM OrderMaster OM(Nolock) 
                        JOIN OrderAttachAnotherOrder a(Nolock) ON a.NewOrderID = OM.OrderId 
                        JOIN CustomerInfo C(Nolock) ON C.CustomerId = OM.CustomerId 
                        Where OM.Companyid=" + DDCompanyName.SelectedValue;
            }
            else
            {
                Str = @"SELECT distinct C.CustomerId,(companyName+'     '+C.CustomerCode) CustomerCode  
                        FROM OrderMaster OM(Nolock) 
                        JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderId 
                        JOIN CustomerInfo C(Nolock) ON C.CustomerId = OM.CustomerId 
                        Where OM.Companyid=" + DDCompanyName.SelectedValue;
            }
            Str = Str + "  order by  companyName+'     '+C.CustomerCode ";
        }
        UtilityModule.ConditionalComboFill(ref DDNewCustomerCode, Str, true, "Select CustomerCode");
    }
    protected void DDNewCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        EditCheckedChanged();
    }
    private void EditCheckedChanged()
    {
        string Str = "";
        if (DDCompanyName.SelectedIndex > 0 && DDCompanyName.SelectedIndex > 0)
        {
            if (ChkForEdit.Checked == true)
            {
                Str = @"SELECT Distinct OM.OrderId, Case When " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 Then OM.LocalOrder + ' | ' + OM.CustomerOrderNo Else OM.customerorderno End OrderNo 
                    FROM OrderMaster OM(Nolock)
                    JOIN OrderAttachAnotherOrder a(Nolock) ON a.NewOrderID = OM.OrderId 
                    Where OM.Companyid=" + DDCompanyName.SelectedValue + " And OM.Customerid = " + DDNewCustomerCode.SelectedValue;
            }
            else
            {
                Str = @"SELECT Distinct OM.OrderId, Case When " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 Then OM.LocalOrder + ' | ' + OM.CustomerOrderNo Else OM.customerorderno End OrderNo 
                    FROM OrderMaster OM(Nolock)
                    JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderId 
                    Where OM.CompanyID = " + DDCompanyName.SelectedValue + " And OM.CustomerID = " + DDNewCustomerCode.SelectedValue;
            }
            Str = Str + "  Order by OrderNo";
            UtilityModule.ConditionalComboFill(ref DDNewOrderNo, Str, true, "--Select--");
        }
    }
    protected void DDNewOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OrderNoSelectedChanged();
    }
    private void OrderNoSelectedChanged()
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select Distinct a.Item_Finished_Id, VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName 
            FROM OrderAttachAnotherOrder a(Nolock)
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.Item_Finished_Id 
            Where a.Companyid = " + DDCompanyName.SelectedValue + " And a.NewOrderID = " + DDNewOrderNo.SelectedValue + @" 
            Order By VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName";
        }
        else
        {
            Str = @"Select Distinct OD.Item_Finished_Id, VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName 
            FROM OrderMaster OM(Nolock) 
            JOIN OrderDetail OD(Nolock) ON OM.OrderId = OD.OrderId 
            JOIN JobAssigns JA(Nolock) ON JA.OrderId = OM.OrderId And JA.Item_Finished_ID = OD.Item_Finished_ID 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OD.Item_Finished_Id 
            Where OM.Companyid = " + DDCompanyName.SelectedValue + " And OM.OrderID = " + DDNewOrderNo.SelectedValue + @" 
            Order By VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName";
        }
        UtilityModule.ConditionalComboFill(ref DDDescription, Str, true, "--Select--");
    }
    protected void DDDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select Distinct CI.CustomerId,(CI.CompanyName + '     ' + CI.CustomerCode) CustomerCode  
            FROM OrderAttachAnotherOrder a(Nolock)
            JOIN OrderMaster OM(Nolock) ON OM.OrderID = a.OldOrderID 
            JOIN CustomerInfo CI(Nolock) ON CI.CustomerId = OM.CustomerId 
            Where a.Companyid = " + DDCompanyName.SelectedValue + " And a.NewOrderID = " + DDNewOrderNo.SelectedValue + @" 
               And a.Item_Finished_ID = " + DDDescription.SelectedValue;
        }
        else
        {
            Str = @"Select Distinct CI.CustomerId,(CI.CompanyName + '     ' + CI.CustomerCode) CustomerCode  
            From OrderMaster OM(Nolock) 
            JOIN CustomerInfo CI(Nolock) ON CI.CustomerID = OM.CustomerID 
            JOIN OrderDetail OD(Nolock) ON OM.OrderId = OD.OrderId And OD.Item_Finished_Id = " + DDDescription.SelectedValue + @" 
            Where OM.OrderID <> " + DDNewOrderNo.SelectedValue;
        }
        Str = Str + "  order by CI.CompanyName + '     ' + CI.CustomerCode";

        UtilityModule.ConditionalComboFill(ref DDOldCustomerCode, Str, true, "--Select--");
    }
    protected void DDOldCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select Distinct OM.OrderId, Case When " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 Then OM.LocalOrder + ' | ' + OM.CustomerOrderNo Else OM.customerorderno End OrderNo 
            FROM OrderAttachAnotherOrder a(Nolock)
            JOIN OrderMaster OM(Nolock) ON OM.OrderID = a.OldOrderID And OM.CustomerID = " + DDNewCustomerCode.SelectedValue + @" 
            Where a.Companyid = " + DDCompanyName.SelectedValue + " And a.NewOrderID = " + DDNewOrderNo.SelectedValue + @" 
                And a.Item_Finished_ID = " + DDDescription.SelectedValue;
        }
        else
        {
            Str = @"Select Distinct OM.OrderId,case when " + variable.VarORDERNODROPDOWNWITHLOCALORDER + @" = 1 Then OM.LocalOrder + ' | ' + OM.CustomerOrderNo Else OM.customerorderno End OrderNo 
            From OrderMaster OM(Nolock) 
            JOIN CarpetNumber CN(Nolock) ON CN.OrderId = OM.OrderId And CN.Pack = 0 And CN.Item_Finished_Id = " + DDDescription.SelectedValue + @" 
            Where OM.OrderID <> " + DDNewOrderNo.SelectedValue + " And OM.CustomerID = " + DDOldCustomerCode.SelectedValue;
        }

        Str = Str + "  Order by OrderNo";
        UtilityModule.ConditionalComboFill(ref DDOldOrderNo, Str, true, "--Select--");
    }

    protected void DDOldOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        OldOrderNoSelectedChanged();
    }
    private void OldOrderNoSelectedChanged()
    {
        string Str = "";
        if (ChkForEdit.Checked == true)
        {
            Str = @"Select CN.StockNo, CN.TStockNo 
            From CarpetNumber CN(Nolock) 
            JOIN OrderAttachAnotherOrder a(Nolock) ON a.StockNo = CN.StockNo And a.NewOrderID = " + DDNewOrderNo.SelectedValue + @"
            Where CN.Pack = 0 And CN.OrderId = " + DDOldOrderNo.SelectedValue + " And CN.Item_Finished_Id = " + DDDescription.SelectedValue + @"
            Order By TStockNo";
        }
        else
        {
            Str = @"Select CN.StockNo, CN.TStockNo 
            From CarpetNumber CN(Nolock) 
            LEFT JOIN OrderAttachAnotherOrder a(Nolock) ON a.StockNo = CN.StockNo 
            Where CN.Pack = 0 And a.StockNo is null And CN.OrderId = " + DDOldOrderNo.SelectedValue + " And CN.Item_Finished_Id = " + DDDescription.SelectedValue + @"
            Order By TStockNo";
        }
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, Str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        string Str = "";
        if (lblErrorMessage.Text == "")
        {
            for (int i = 0; i < DG.Rows.Count; i++)
            {
                CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));
                if (Chkboxitem.Checked == true)
                {
                    Label lblStockNo = ((Label)DG.Rows[i].FindControl("lblStockNo"));
                    if (Str == "")
                    {
                        Str = lblStockNo.Text + "~";
                    }
                    else
                    {
                        Str = Str + lblStockNo.Text + "~";
                    }
                }
            }
            if (Str == "")
            {
                lblErrorMessage.Text = "Please select atleast one StockNo";
                return;
            }
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("Pro_SaveAssignOtherOrder", con, Tran);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
                cmd.Parameters.AddWithValue("@NewOrderID", DDNewOrderNo.SelectedValue);
                cmd.Parameters.AddWithValue("@OldOrderID", DDOldOrderNo.SelectedValue);
                cmd.Parameters.AddWithValue("@Item_Finished_ID", DDDescription.SelectedValue);
                cmd.Parameters.AddWithValue("@UserID", Session["varuserid"]);
                cmd.Parameters.AddWithValue("@MastercompanyID", Session["varMasterCompanyIDForERP"]);
                cmd.Parameters.AddWithValue("@StockNos", Str);
                cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
                {
                    lblErrorMessage.Text = cmd.Parameters["@Msg"].Value.ToString();
                    Tran.Rollback();
                }
                else
                {
                    lblErrorMessage.Text = "Data Saved Successfully.";
                    Tran.Commit();
                    DDOldOrderNo.SelectedIndex = 0;
                    OldOrderNoSelectedChanged();
                }
            }
            catch (Exception ex)
            {
                UtilityModule.MessageAlert(ex.Message, "Master/Order/FrmAssignOtherOrder.aspx/Save");
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
    protected void ChkForEdit_CheckedChanged(object sender, EventArgs e)
    {
        CompanyNameSelectedIndexChanged();
        EditCheckedChanged();
    }

    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}