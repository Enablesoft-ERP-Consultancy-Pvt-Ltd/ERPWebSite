using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Masters_Purchase_FrmDefineItemUserWise : System.Web.UI.Page
{
    string str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            str = @"Select Distinct CI.CompanyId, CI.Companyname 
            From Companyinfo CI(nolock)
            JOIN Company_Authentication CA(nolock) ON CA.CompanyId=CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
            Where CI.MasterCompanyId=" + Session["varMasterCompanyIDForERP"] + @" Order By CI.Companyname 
            Select Distinct CI.CustomerId, CI.CustomerCode 
            From OrderMaster OM(nolock)
            JOIN CustomerInfo CI(nolock) ON CI.CustomerId = OM.CustomerId 
            Where OM.Status = 0 And OM.CompanyID = " + Session["CurrentWorkingCompanyID"] + @" 
            Order By CI.CustomerCode 
            Select UserID, UserName From NewUserDetail Where IsVisible <> 0 Order By UserName ";

            DataSet ds = SqlHelper.ExecuteDataset(str);
            UtilityModule.ConditionalComboFillWithDS(ref ddCompName, ds, 0, true, "Select Comp Name");

            if (ddCompName.Items.Count > 0)
            {
                ddCompName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                ddCompName.Enabled = false;
            }
            UtilityModule.ConditionalComboFillWithDS(ref DDCustomerCode, ds, 1, true, "--Plz Select--");
            UtilityModule.ConditionalComboFillWithDS(ref DDUserName, ds, 2, true, "--Plz Select--");
            DDCustomerCode.Focus();
        }
    }

    protected void DDCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustomerCodeSelectedIndexChanged();
    }
    private void CustomerCodeSelectedIndexChanged()
    {
        UtilityModule.ConditionalComboFill(ref DDOrderNo, @"Select OM.OrderID, OM.CustomerOrderNo 
                From OrderMaster OM(nolock)
                Where OM.Status = 0 And OM.CompanyId = " + ddCompName.SelectedValue + " And OM.CustomerId = " + DDCustomerCode.SelectedValue + @" 
                Order By OM.OrderId Desc ", true, "--Select Order No--");
    }
    protected void DDOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDGGrid();
    }

    private void FillDGGrid()
    {
        string str = @"Select Distinct OM.OrderID, OCD.IFINISHEDID, 
            Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '') ItemDescription
            FROM ORDERMASTER OM(Nolock) 
            JOIN ORDERDETAIL OD(Nolock) ON OD.OrderID = OM.OrderID 
            JOIN ORDER_CONSUMPTION_DETAIL OCD(Nolock) ON OCD.OrderID = OM.OrderID And OCD.OrderDetailID = OD.OrderDetailID And OCD.PROCESSID in (5, 9, 89, 102, 127) 
            JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = OCD.IFINISHEDID 
            Where OM.OrderID = " + DDOrderNo.SelectedValue + @" 
            Order By Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '')";
        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DG.DataSource = ds.Tables[0];
        DG.DataBind();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string DetailData = "";
        for (int i = 0; i < DG.Rows.Count; i++)
        {
            CheckBox Chkboxitem = ((CheckBox)DG.Rows[i].FindControl("Chkboxitem"));

            if (Chkboxitem.Checked == true)
            {
                Label LblOrderID = ((Label)DG.Rows[i].FindControl("LblOrderID"));
                Label LblIFinishedID = ((Label)DG.Rows[i].FindControl("LblIFinishedID"));
                if (DetailData == "")
                {
                    DetailData = LblIFinishedID.Text + "~";
                }
                else
                {
                    DetailData = DetailData + LblIFinishedID.Text + "~";
                }
            }
        }
        if (DetailData == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save1", "alert('Please select atleast one check box');", true);
            return;
        }
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction tran = con.BeginTransaction();
        try
        {
            SqlParameter[] arr = new SqlParameter[6];
            arr[0] = new SqlParameter("@OrderID", SqlDbType.Int);
            arr[1] = new SqlParameter("@UserID", SqlDbType.Int);
            arr[2] = new SqlParameter("@IFinishedIDData", SqlDbType.NVarChar);
            arr[3] = new SqlParameter("@LoginUserID", SqlDbType.Int);
            arr[4] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);
            arr[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 200);

            arr[0].Value = DDOrderNo.SelectedValue;
            arr[1].Value = DDUserName.SelectedValue;
            arr[2].Value = DetailData;
            arr[3].Value = Session["varuserid"];
            arr[4].Value = Session["varMasterCompanyIDForERP"];
            arr[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "[Pro_SaveDefineItemUserWise]", arr);

            if (arr[5].Value.ToString() != "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + arr[8].Value.ToString() + "');", true);
                tran.Rollback();
            }
            else
            {
                tran.Commit();
            }
            fill_grid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "save", "alert('" + ex.Message + "');", true);
            tran.Rollback();
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    private void fill_grid()
    {
        string str = @"Select Distinct a.OrderID, a.UserID, a.ITEM_FINISHED_ID IFINISHEDID, 
                Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '') ItemDescription
                FROM DefinePurchaseItemUserWise a(Nolock) 
                JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = a.ITEM_FINISHED_ID 
                Where a.OrderID = " + DDOrderNo.SelectedValue + " And a.UserID = " + DDUserName.SelectedValue + @" 
                Order By Replace(VF.ITEM_NAME + ', ' + VF.QualityName + ', ' + VF.DesignName + ', ' + VF.ColorName + ', ' + VF.ShapeName + ', ' + VF.SizeFt + ', ' + VF.ShadeColorName, ', , ', '') ";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        gvdetail.DataSource = ds.Tables[0];
        gvdetail.DataBind();
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gvdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            Label LblRollIssueToNextID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextID");
            Label LblRollIssueToNextDetailID = (Label)gvdetail.Rows[e.RowIndex].FindControl("LblRollIssueToNextDetailID");

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RollIssueToNextID", LblRollIssueToNextID.Text);
            param[1] = new SqlParameter("@RollIssueToNextDetailID", LblRollIssueToNextDetailID.Text);
            //param[2] = new SqlParameter("@ProcessID", DDProcessName.SelectedValue);
            param[3] = new SqlParameter("@UserID", Session["VarUserId"]);
            param[4] = new SqlParameter("@MasterCompanyID", Session["varMasterCompanyIDForERP"]);
            param[5] = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
            param[5].Direction = ParameterDirection.Output;
            //****************
            SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_DeleteRollIssueToNextProcess", param);
            lblmessage.Text = param[5].Value.ToString();
            Tran.Commit();
            fill_grid();
            //***************
        }
        catch (Exception ex)
        {
            lblmessage.Text = ex.Message;
            Tran.Rollback();
        }
        finally
        {
            con.Dispose();
            con.Close();
        }
    }
    protected void DG_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void DDUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int j = 0; j < DG.Rows.Count; j++)
        {
            ((CheckBox)DG.Rows[j].FindControl("Chkboxitem")).Checked = false;
        }

        fill_grid();
        
        DataSet ds = SqlHelper.ExecuteDataset(@"Select Item_Finished_ID 
            From DefinePurchaseItemUserWise(Nolock)
            Where OrderID = " + DDOrderNo.SelectedValue + " And UserID = " + DDUserName.SelectedValue + " And MasterCompanyID = " + Session["varMasterCompanyIDForERP"]);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < DG.Rows.Count; j++)
                {
                    Label LblIFinishedID = ((Label)DG.Rows[j].FindControl("LblIFinishedID"));
                    if (Convert.ToInt32(LblIFinishedID.Text) == Convert.ToInt32(ds.Tables[0].Rows[i]["Item_Finished_ID"]))
                    {
                        ((CheckBox)DG.Rows[j].FindControl("Chkboxitem")).Checked = true;
                        j = DG.Rows.Count;
                    }
                }
            }
        }
    }
}
