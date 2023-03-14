using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class FrmConeTypeMaster : CustomPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varCompanyId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["ConeTypeID"] = 0;
            Fill_Grid();
        }
        Label2.Visible = false;
    }
    private void Fill_Grid()
    {
        DGConeType.DataSource = Fill_Grid_Data();
        DGConeType.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        try
        {
            string str = "Select ID, ConeType, ConeWeight, SrNo From ConeMaster(Nolock) Order By SrNo";
            ds = SqlHelper.ExecuteDataset(str);
        }
        catch (Exception ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/FrmConeTypeMaster.aspx");
        }
        return ds;
    }
    protected void DGConeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ConeTypeID"] = DGConeType.SelectedRow.Cells[0].Text;
        TxtConeType.Text = DGConeType.SelectedRow.Cells[1].Text.Replace("&nbsp;", "");
        TxtConeWeight.Text = DGConeType.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        TxtSeqenceNo.Text = DGConeType.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        btnsave.Text = "Update";
    }
    protected void DGConeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGConeType, "select$" + e.Row.RowIndex);

        }
    }
    protected void DGConeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGConeType.PageIndex = e.NewPageIndex;
        Fill_Grid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (TxtConeType.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlTransaction Tran = con.BeginTransaction();
            try
            {
                SqlParameter[] _arrpara = new SqlParameter[6];
                _arrpara[0] = new SqlParameter("@ConeTypeID", SqlDbType.Int);
                _arrpara[1] = new SqlParameter("@ConeType", SqlDbType.NVarChar, 50);
                _arrpara[2] = new SqlParameter("@ConeWeight", SqlDbType.NVarChar, 50);
                _arrpara[3] = new SqlParameter("@SrNo", SqlDbType.Int);
                _arrpara[4] = new SqlParameter("@UserID", SqlDbType.Int);
                _arrpara[5] = new SqlParameter("@MasterCompanyID", SqlDbType.Int);

                
                _arrpara[0].Value = ViewState["ConeTypeID"];
                _arrpara[1].Value = TxtConeType.Text.ToUpper();
                _arrpara[2].Value = TxtConeWeight.Text;
                _arrpara[3].Value = TxtSeqenceNo.Text;
                _arrpara[4].Value = Session["varuserid"].ToString();
                _arrpara[5].Value = Session["varCompanyId"].ToString();

                SqlHelper.ExecuteNonQuery(Tran, CommandType.StoredProcedure, "PRO_SAVECONETYPE", _arrpara);
                Tran.Commit();
                Label2.Visible = true;
                Label2.Text = "Save Details............";
                Fill_Grid();
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                Label2.Visible = true;
                Label2.Text = ex.Message;
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/FrmConeTypeMaster.aspx");
            }
            finally
            {
                con.Close();
            }
            TxtConeType.Text = "";
            TxtConeWeight.Text = "";
            TxtSeqenceNo.Text = "";
            btnsave.Text = "Save";
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        TxtConeType.Text = "";
        TxtConeWeight.Text = "";
        TxtSeqenceNo.Text = "";
    }
  
    protected void DGConeType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        //if (e.Row.RowType == DataControlRowType.Header)
            //e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        //if (e.Row.RowType == DataControlRowType.DataRow &&
                 // e.Row.RowState == DataControlRowState.Alternate)
           // e.Row.CssClass = "alternate";
    }
}
