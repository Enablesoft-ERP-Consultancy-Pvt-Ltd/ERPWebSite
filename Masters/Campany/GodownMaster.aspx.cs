using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Masters_Campany_GodownMaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            txtid.Text = "0";
            
            UtilityModule.ConditionalComboFill(ref DDBranchName, @"Select ID, BranchName 
                            From BRANCHMASTER BM(nolock) 
                            Where BM.CompanyID = " + Session["CurrentWorkingCompanyID"] + " And BM.MasterCompanyID = " + Session["varCompanyId"], false, "");

            if (DDBranchName.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "opn1", "alert('Branch not define for this user!');", true);
                return;
            }

            UtilityModule.ConditonalChkBoxListFill(ref ChkBoxListProcessEmployeName, @"Select EI.EmpId, EI.EmpName 
                From EmpInfo EI(Nolock) 
                Join EmpProcess EP(Nolock) ON EI.EmpId = EP.EmpId And EP.ProcessID = 5 
                Where EI.MasterCompanyId = " + Session["varCompanyId"] + @" Order By EI.EmpName");
            if (Convert.ToInt32(Session["varCompanyId"]) == 42)
            {
                TDProcessEmployeeName.Visible = true;
            }
            Fill_Grid();
        }
    }
    private void Fill_Grid()
    {
        DGGodownMaster.DataSource = Fill_Grid_Data();
        DGGodownMaster.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string strsql = "Select GodownId SrNo, GodownName, CompanyGodown From GodownMaster(nolock) where MasterCompanyId=" + Session["varCompanyid"];
            ds = SqlHelper.ExecuteDataset(strsql);
        }
        catch (Exception ex)
        {
            Logs.WriteErrorLog("Masters_Campany_frmGodownMaster|Fill_Grid_Data|" + ex.Message);
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/GodownMaster.aspx");
        }
        return ds;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtGodawnName.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                string EmpID = "";
                for (int i = 0; i < ChkBoxListProcessEmployeName.Items.Count; i++)
                {
                    if (ChkBoxListProcessEmployeName.Items[i].Selected)
                    {
                        if (EmpID == "")
                        {
                            EmpID = ChkBoxListProcessEmployeName.Items[i].Value + '|';
                        }
                        else
                        {
                            EmpID = EmpID + ChkBoxListProcessEmployeName.Items[i].Value + '|';
                        }
                    }
                }

                SqlParameter[] _arrPara = new SqlParameter[8];
                _arrPara[0] = new SqlParameter("@GodawnId", Convert.ToInt32(txtid.Text));
                _arrPara[1] = new SqlParameter("@GodownName", txtGodawnName.Text.ToUpper());
                _arrPara[2] = new SqlParameter("@varuserid", Session["varuserid"]);
                _arrPara[3] = new SqlParameter("@varCompanyId", Session["varCompanyId"]);
                _arrPara[4] = new SqlParameter("@CompanyGodownFlag", ChkForCompanyGodown.Checked == true ? 1 : 0);
                _arrPara[5] = new SqlParameter("@EmpIDs", EmpID);
                _arrPara[6] = new SqlParameter("@Msg", SqlDbType.NVarChar, 250);
                _arrPara[6].Direction = ParameterDirection.Output;
                _arrPara[7] = new SqlParameter("@BranchID", DDBranchName.SelectedValue);

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "Pro_Godown", _arrPara);

                Tran.Commit();
                LblErr.Text = _arrPara[6].Value.ToString();
                txtid.Text = "0";
                txtGodawnName.Text = "";
                ChkForCompanyGodown.Checked = false;

                for (int j = 0; j < ChkBoxListProcessEmployeName.Items.Count; j++)
                {
                    ChkBoxListProcessEmployeName.Items[j].Selected = false;
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LblErr.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/GodownMaster.aspx");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                if (con != null)
                {
                    con.Dispose();
                }
            }

            Fill_Grid();
            btnsave.Text = "Save";
        }
        else
        {
            LblErr.Text = "Please enter Godown Name....";
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtid.Text = "0";
        txtGodawnName.Text = "";
    }
    protected void DGGodownMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblErr.Text = "";
        ChkForCompanyGodown.Checked = false;

        for (int j = 0; j < ChkBoxListProcessEmployeName.Items.Count; j++)
        {
            ChkBoxListProcessEmployeName.Items[j].Selected = false;
        }

        txtid.Text = ((Label)DGGodownMaster.Rows[DGGodownMaster.SelectedIndex].FindControl("lblSrNo")).Text;
        txtGodawnName.Text = ((Label)DGGodownMaster.Rows[DGGodownMaster.SelectedIndex].FindControl("lblGodownName")).Text;
        int CompanyGodownID = Convert.ToInt32(((Label)DGGodownMaster.Rows[DGGodownMaster.SelectedIndex].FindControl("lblCompanyGodown")).Text);

        if (CompanyGodownID == 1)
        {
            ChkForCompanyGodown.Checked = true;
        }
        string strsql = "Select EmpID From GodownWiseEmp(nolock) Where GodownID = " + txtid.Text + @"
                Select BranchID From GodownMaster(nolock) Where GodownID = " + txtid.Text;
        DataSet ds = SqlHelper.ExecuteDataset(strsql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ChkBoxListProcessEmployeName.Items.Count; j++)
                {
                    if (Convert.ToInt32(ChkBoxListProcessEmployeName.Items[j].Value) == Convert.ToInt32(ds.Tables[0].Rows[i]["EmpID"]))
                    {
                        ChkBoxListProcessEmployeName.Items[j].Selected = true;
                    }
                }
            }
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (DDBranchName.Items.FindByValue(ds.Tables[1].Rows[0]["BranchID"].ToString()) != null)
            {
                DDBranchName.SelectedValue = ds.Tables[1].Rows[0]["BranchID"].ToString();
            }
        }

        btnsave.Text = "Update";
    }
    protected void DGGodownMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGGodownMaster.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void DGGodownMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGGodownMaster, "select$" + e.Row.RowIndex);
        }
    }
    protected void DGGodownMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
        // e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        // if (e.Row.RowType == DataControlRowType.DataRow &&
        // e.Row.RowState == DataControlRowState.Alternate)
        // e.Row.CssClass = "alternate";
    }
}