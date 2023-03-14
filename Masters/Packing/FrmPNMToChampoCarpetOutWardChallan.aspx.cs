using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Masters_Packing_FrmPNMToChampoCarpetOutWardChallan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            string str = @"Select Distinct CI.CompanyId, CI.CompanyName 
                            From Companyinfo CI(Nolock)
                            JOIN Company_Authentication CA(Nolock) ON CA.CompanyId = CI.CompanyId And CA.UserId=" + Session["varuserId"] + @" 
                            Where CI.MasterCompanyId=" + Session["varCompanyId"] + @" Order By CompanyName";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDCompanyName, ds, 0, false, "");
            if (DDCompanyName.Items.Count > 0)
            {
                DDCompanyName.SelectedValue = Session["CurrentWorkingCompanyID"].ToString();
                DDCompanyName.Enabled = false;
            }

            TxtIssueDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["IssueNo"] = "0";
        }
    }
    protected void TxtStockNo_TextChanged(object sender, EventArgs e)
    {
        if (TxtStockNo.Text != "")
        {
            if (TxtRemark.Text != "")
            {
                SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlTransaction Tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("PRO_SAVE_PNMToChampoCarpetOutWardChallan", con, Tran);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30000;

                    cmd.Parameters.AddWithValue("@Companyid", DDCompanyName.SelectedValue);
                    cmd.Parameters.AddWithValue("@TSTOCKNO", TxtStockNo.Text);

                    cmd.Parameters.Add("@IssueNo", SqlDbType.Int);
                    cmd.Parameters["@IssueNo"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters["@IssueNo"].Value = ViewState["IssueNo"];
                    cmd.Parameters.AddWithValue("@IssueDate", TxtIssueDate.Text);

                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 500);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Userid", Session["varuserid"]);
                    cmd.Parameters.AddWithValue("@MasterCompanyID", Session["varCompanyId"]);
                    cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);

                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@Msg"].Value.ToString() != "") //IF DATA NOT SAVED
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + cmd.Parameters["@Msg"].Value.ToString() + "');", true);
                        TxtStockNo.Text = "";
                        TxtStockNo.Focus();
                        Tran.Rollback();
                    }
                    else
                    {
                        Tran.Commit();
                        TxtIssueNo.Text = cmd.Parameters["@IssueNo"].Value.ToString();
                        ViewState["IssueNo"] = TxtIssueNo.Text;
                        TxtStockNo.Text = "";
                        //Fill_Grid();
                        TxtNoofPc.Text = (TxtNoofPc.Text == "" ? 1 : Convert.ToInt32(TxtNoofPc.Text) + 1).ToString();
                    }
                    TxtStockNo.Focus();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + ex.Message + "');", true);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('Please Enter remark')", true);
                TxtStockNo.Focus();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "al", "alert('Please Enter Stock No.')", true);
            TxtStockNo.Focus();
        }
    }

    protected void DGOrderdetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGOrderdetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void DGOrderdetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int VarStockNo = Convert.ToInt32(DGOrderdetail.DataKeys[e.RowIndex].Value);
        
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        SqlTransaction Tran = con.BeginTransaction();
        try
        {
            string str = @" Delete StockNoOutWardChallan Where IssueNo = " + ViewState["IssueNo"] + " And StockNo = " + VarStockNo + @" 
                        UpDate CarpetNumber Set Pack = 0 Where StockNo = " + VarStockNo;

            SqlHelper.ExecuteNonQuery(Tran, CommandType.Text, str);
            
            Tran.Commit();
            Fill_Grid();
        }
        catch (Exception ex)
        {
            Tran.Rollback();
            ScriptManager.RegisterStartupScript(Page, GetType(), "altsave", "alert('" + ex.Message + "');", true);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void BtnShowDataInGrid_Click(object sender, EventArgs e)
    {
        Fill_Grid();
    }
    private void Fill_Grid()
    {
        string str = @"Select CI.CompanyName, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt ItemDescription, 
                    a.IssueNo, a.IssueDate, 1 Qty, CN.TStockNo, a.StockNo 
                    From StockNoOutWardChallan a(Nolock)
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
                    JOIN CarpetNumber CN(Nolock) ON CN.StockNo = a.StockNo 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
                    Where a.IssueNo = " + ViewState["IssueNo"] + @" Order By a.ID desc";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);
        DGOrderdetail.DataSource = ds.Tables[0];
        DGOrderdetail.DataBind();
        TxtNoofPc.Text = DGOrderdetail.Rows.Count.ToString();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string str = @"Select CI.CompanyName, VF.ITEM_NAME + ' ' + VF.QualityName + ' ' + VF.DesignName + ' ' + VF.ColorName + ' ' + VF.ShapeName + ' ' + VF.SizeFt ItemDescription, 
                    a.IssueNo, REPLACE(CONVERT(NVARCHAR(11), a.IssueDate, 106), ' ', '-') IssueDate, Count(CN.TStockNo) Qty, a.Remark, 
					(Select TStockNo + ', ' 
                        From StockNoOutWardChallan b(Nolock) 
                        Where b.IssueNo = a.IssueNo And b.ITEM_FINISHED_ID = a.ITEM_FINISHED_ID And b.UserID = a.UserID 
                        Order BY TStockNo For XML Path('')) StockNo, NUD.UserName 
                    From StockNoOutWardChallan a(Nolock)
                    JOIN CompanyInfo CI(Nolock) ON CI.CompanyId = a.CompanyID 
                    JOIN CarpetNumber CN(Nolock) ON CN.StockNo = a.StockNo 
                    JOIN V_FinishedItemDetail VF(Nolock) ON VF.ITEM_FINISHED_ID = CN.Item_Finished_Id 
                    JOIN NewUserDetail NUD(Nolock) ON NUD.UserId = a.UserID 
                    Where a.IssueNo = " + ViewState["IssueNo"] + @" 
                    Group By CI.CompanyName, VF.ITEM_NAME, VF.QualityName, VF.DesignName, VF.ColorName, VF.ShapeName, VF.SizeFt, a.IssueNo, a.IssueDate, 
                    a.ITEM_FINISHED_ID, a.Remark, NUD.UserName, a.UserID";

        DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["rptFileName"] = "~\\Reports\\RptStockNoOutWardChallan.rpt";
            Session["Getdataset"] = ds;
            Session["dsFileName"] = "~\\ReportSchema\\RptStockNoOutWardChallan.xsd";
            StringBuilder stb = new StringBuilder();
            stb.Append("<script>");
            stb.Append("window.open('../../ViewReport.aspx', 'nwwin', 'toolbar=0, titlebar=1,  top=0px, left=0px, scrollbars=1, resizable = yes');</script>");
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "opn", stb.ToString(), false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "opn", "alert('No records found!!!');", true);
        }
    }
    protected void ChkEditOrder_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkEditOrder.Checked == true)
        {
            TDDDIssueNo.Visible = true;
            string str = @"Select Distinct a.IssueNo, a.IssueNo 
                    From StockNoOutWardChallan a(Nolock)
                    Where MasterCompanyID = " + Session["varCompanyId"] + " And CompanyID = " + DDCompanyName .SelectedValue + @" 
                    Order By a.IssueNo";

            DataSet ds = SqlHelper.ExecuteDataset(ErpGlobal.DBCONNECTIONSTRING, CommandType.Text, str);

            UtilityModule.ConditionalComboFillWithDS(ref DDIssueNo, ds, 0, true, "Select Issue No");
        }
        else
        {
            TDDDIssueNo.Visible = false;
        }
    }
    protected void DDIssueNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["IssueNo"] = DDIssueNo.SelectedValue;
        Fill_Grid();
        TxtIssueNo.Text = ViewState["IssueNo"].ToString();
    }
}