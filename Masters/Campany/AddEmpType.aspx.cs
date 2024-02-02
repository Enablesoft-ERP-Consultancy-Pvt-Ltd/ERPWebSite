using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class Masters_Campany_AddEmpType : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["varMasterCompanyIDForERP"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            TxtId.Text = "0";
            fill_Gride();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (TxtEmpType.Text != "")
        {
            SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
            try
            {
                SqlParameter[] _arrPara = new SqlParameter[5];
                _arrPara[0] = new SqlParameter("@EmpCatId", SqlDbType.Int);
                _arrPara[1] = new SqlParameter("@EmpType", SqlDbType.NVarChar);
                _arrPara[2] = new SqlParameter("@Id", SqlDbType.Int);
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                _arrPara[0].Value = TxtId.Text;
                _arrPara[1].Value = TxtEmpType.Text;
                _arrPara[2].Direction = ParameterDirection.Output;
                _arrPara[3] = new SqlParameter("@varuserid", SqlDbType.Int);
                _arrPara[4] = new SqlParameter("@varCompanyId", SqlDbType.Int);
                con.Open();
                SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Pro_EmpType", _arrPara);
                if (Convert.ToInt32(_arrPara[2].Value) == 0)
                {
                    lblErr.Text = "Data Saved.......";
                   
                }
                else
                {
                    lblErr.Text = "Type Already Exists.....";
                    TxtEmpType.Text = "";
                    TxtEmpType.Focus();
                }
                fill_Gride();
                TxtEmpType.Text = "";
            }
            catch (Exception ex)
            {
                lblErr.Text = "Wrong Entery.........";
                UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddEmpType.aspx");
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        else
        {
            lblErr.Text = "EmpType Can not null.....";
            
        }
    }
    protected void DGEmpType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
           // e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.DGEmpType, "select$" + e.Row.RowIndex);
        
    }
    protected void DGEmpType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DGEmpType.PageIndex = e.NewPageIndex;
        fill_Gride();
    }
    private void fill_Gride()
    {
        DGEmpType.DataSource = Fill_Grid_Data();
        DGEmpType.DataBind();
    }
    private DataSet Fill_Grid_Data()
    {
        DataSet ds = null;
        SqlConnection con = new SqlConnection(ErpGlobal.DBCONNECTIONSTRING);
        try
        {
            string strsql = "select EmpCatId SrNo,EmpType from EmpCategory";
            con.Open();
            ds = SqlHelper.ExecuteDataset(con, CommandType.Text, strsql);
        }
        catch (Exception  ex)
        {
            UtilityModule.MessageAlert(ex.Message, "Master/Campany/AddEmpType.aspx");
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        return ds;
    }
    protected void DGEmpType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";
    }
}